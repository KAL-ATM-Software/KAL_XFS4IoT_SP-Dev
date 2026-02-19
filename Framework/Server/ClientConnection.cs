/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;

using XFS4IoT;
using System.IO;

namespace XFS4IoTServer
{
    internal class ClientConnection : IConnection
    {
        public ClientConnection(WebSocket socket, 
                                IMessageDecoder CommandDecoder, 
                                ICommandDispatcher CommandDispatcher,
                                IServiceProvider ServiceProvider,
                                ILogger Logger,
                                IJsonSchemaValidator JsonSchemaValidator)
        {
            this.socket = socket;
            this.CommandDecoder = CommandDecoder;
            this.CommandDispatcher = CommandDispatcher;
            this.ServiceProvider = ServiceProvider;
            this.Logger = Logger;
            this.JsonSchemaValidator = JsonSchemaValidator;
        }

        public async Task RunAsync(CancellationToken token)
        {
            try
            {
                var receivedBuffer = new Memory<byte>(new byte[MAX_BUFFER]);

                while (!token.IsCancellationRequested)
                {
                    // client is no longer connected
                    if (socket.State != WebSocketState.Open)
                        break;

                    // a single message could be delivered in multiple chunks
                    ValueWebSocketReceiveResult res;
                    int ReceivedBufferReceived = 0;
                    do
                    {
                        var BufferSlice = receivedBuffer.Slice(ReceivedBufferReceived, receivedBuffer.Length - ReceivedBufferReceived);
                        // Wait for data from the client
                        res = await socket.ReceiveAsync(BufferSlice, token);
                        ReceivedBufferReceived += res.Count;
                    } while (!res.EndOfMessage && ReceivedBufferReceived < receivedBuffer.Length);
                    res.EndOfMessage.IsTrue($"Failed to receive message within MAX_BUFFER. {MAX_BUFFER}");

                    if (res.MessageType is WebSocketMessageType.Text or WebSocketMessageType.Binary)
                    {
                        string message = Encoding.UTF8.GetString(receivedBuffer[0..ReceivedBufferReceived].Span);

                        await HandleIncommingMessage(message, token);
                    }
                    else if (res.MessageType == WebSocketMessageType.Close)
                    {
                        // client closed session
                        break;
                    }
                }
            }
            catch (WebSocketException ex) when (ex.InnerException is HttpListenerException)
            {
                Logger.Warning(Constants.Component, "Client Closed connection");
            }
            catch (WebSocketException ex) when (ex.InnerException is TaskCanceledException)
            {
                Logger.Warning(Constants.Component, "Connection aborted");
                Debugger.Break();
                throw;
            }
            catch(Exception ex)
            {
                Logger.Warning(Constants.Component, $"Unexpected exception: {ex}");
                throw; 
            }
            finally
            {
                // Remove client from the list 
                socket.Dispose();

                try
                {
                    // Cancel any active or queued commands from this connection as there is no way for the application to cancel or receive the completion message.
                    await CommandDispatcher.CancelCommandsAsync(this, null, CancellationToken.None);
                }
                catch(Exception ex)
                {
                    Logger.Warning(Constants.Component, $"Caught exception cancelling commands on client disconnect. {ex}");
                }
            }
        }

        private readonly IMessageDecoder CommandDecoder;

        private async Task HandleIncommingMessage(string messageString, CancellationToken token)
        {
            bool rc = CommandDecoder.TryUnserialise(messageString, out object command);
            Contracts.IsTrue(rc, $"Invalid JSON or unknown command received in the {nameof(HandleIncommingMessage)} method.");
            Contracts.Assert(command is not null, $"Failed on unserialzing received JSON in ${nameof(HandleIncommingMessage)} method. JSON:{messageString}");

            MessageBase commandBase = command as MessageBase;

            // Logging customer sensitve information if there are any
            try
            {
                Logger.LogSensitive(Constants.Component, $"Received:{commandBase}");
            }
            catch (XFS4IoT.InvalidDataException ex)
            {
                await CommandDispatcher.DispatchError(ServiceProvider, this, commandBase, ex);
                return;
            }
            catch (Exception ex)
            {
                Contracts.Fail($"Exception caught while in serialising JSON on receiving incomming message. {ex}");
            }

            // Validate message first if JSON schema validator is being injected
            try
            {
                if (JsonSchemaValidator?.SchemaLoaded is true)
                {
                    string failedReason = string.Empty;
                    if (!JsonSchemaValidator.Validate(messageString, out failedReason))
                    {
                        throw new InvalidCommandException($"Command message validation failed. {failedReason}");
                    }
                }
            }
            catch (Exception ex)
            {
                await CommandDispatcher.DispatchError(ServiceProvider, this, commandBase, ex);
                return;
            }

            // Process command
            try
            {
                await CommandDispatcher.Dispatch(ServiceProvider, this, commandBase, token);
            }
            catch (NotImplementedException ex) // Add more exception can be thrown by the device specific class
            {
                await CommandDispatcher.DispatchError(ServiceProvider, this, commandBase, ex);
            }
            catch (Exception ex)
            {
                Contracts.Fail($"Exception caught while in processing command. {ex}");
            }
        }

        public async Task SendMessageAsync(object messsage)
        {
            // Allow to send command/response/event/acknowledgement type of mesage derived from MessageBase class
            MessageBase messageBase = messsage as MessageBase;
            messageBase.IsNotNull($"Unexpected type of object received to serialize message in the ${nameof(SendMessageAsync)} method.");

            try
            {
                string JSON = messageBase.Serialise();
                Logger.LogSensitive(Constants.Component, $"Sending: {messageBase}");

                // Ensure only a single message can be sent at a time.
                {
                    using var sync = await DisposableLock.Create(SendSyncObject);
                    await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JSON)), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch( Exception e ) when ((e.InnerException is WebSocketException we) && ((uint)we.HResult == 0x80004005))
            {
                Logger.Warning(Constants.Component, "The ClientWebSocket has been aborted");
            }
            catch (ObjectDisposedException)
            {
                Logger.Warning(Constants.Component, "The ClientWebSocket has been closed");
            }
            catch (InvalidOperationException)
			{
                Logger.Warning(Constants.Component, "The ClientWebSocket is not connected");
            }
            catch (XFS4IoT.InvalidDataException ex)
            {
                Contracts.Fail($"Invalid data is set by the device class.{messageBase.Header.Name}. {ex}");
            }
            catch (Exception ex)
            {
                Contracts.Fail($"Exception caught while in serialising JSON on sending message. {ex}");
            }
        }
        
        private readonly WebSocket socket;
        private readonly ILogger Logger;
        private const int MAX_BUFFER = 2 * 1024 * 1024; // 2MB
        private readonly ICommandDispatcher CommandDispatcher;
        private readonly IServiceProvider ServiceProvider;
        private readonly IJsonSchemaValidator JsonSchemaValidator;
        private readonly SemaphoreSlim SendSyncObject = new(1, 1);
    }
}
