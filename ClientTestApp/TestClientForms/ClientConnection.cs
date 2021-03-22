/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Reflection;
using XFS4IoT;

namespace XFS4IoTClient
{
    public class ClientConnection
    {
        /// <summary>
        /// Create a client connection with message decoding. 
        /// </summary>
        /// <param name="EndPoint">URI of the endpoint to connect to</param>
        /// <param name="Responses">decoder to use with incomming messages</param>
        public ClientConnection(Uri EndPoint)
        {
            this.EndPoint = EndPoint ?? throw new ArgumentException("Endpoint required");
        }

        /// <summary>
        /// Open the network connection to the endpoint
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            if (Socket.State != WebSocketState.Open)
            {
                await Socket.ConnectAsync(EndPoint, CancellationToken.None);
            }
        }

        /// <summary>
        /// Disconnect from an endpoint explicitly
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectAsync()
        {
            try
            {
                if (Socket.State == WebSocketState.Open)
                {
                    await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the application.", CancellationToken.None);
                }
            }
            catch (WebSocketException ex) when (ex.InnerException is SocketException)
            { }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                // Create new cancel source
                Socket.Dispose();
                Socket = new ClientWebSocket();
            }
        }

        public bool IsConnected { get { return Socket.State == WebSocketState.Open; } }


        /// <summary>
        /// Send a command to the endpoint. 
        /// </summary>
        /// <param name="command">The command object to send</param>
        public async Task SendCommandAsync(object command)
        {
            if (Socket.State != WebSocketState.Open)
                throw new Exception("Attempted to send a command to a WebSocket that wasn't open");

            if (command is not MessageBase messageBase)
            {
                throw new Exception($"Unexpected message type{command.GetType()}");
            }

            var cmdAttrib = from CustomAttributeData attib in command.GetType().CustomAttributes
                          where attib.AttributeType == typeof(XFS4IoTServer.CommandHandlerAttribute)
                          select attib;

            if (cmdAttrib is null)
                throw new Exception($"Unexpected command type specifid. {command.GetType()}");

            ArraySegment<byte> JSON = new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageBase.Serialise()));

            await Socket.SendAsync( JSON, WebSocketMessageType.Text, true, CancellationToken.None );
        }

        /// <summary>
        /// Wait for and receive a message
        /// </summary>
        /// <returns>Decoded message. </returns>
        public async Task<object> ReceiveMessageAsync(CancellationToken CancelTaken = default)
        {
            try
            {
                var buffer = new ArraySegment<byte>(new byte[MessageBufferSize]);

                // Get the next message
                var rc = await Socket.ReceiveAsync(buffer, CancelTaken == default ? CancellationToken.None : CancelTaken);

                if (rc.MessageType == WebSocketMessageType.Text)
                {
                    // trim the incomming message and extract a string
                    var messageString = Encoding.UTF8.GetString(buffer.Take(rc.Count).ToArray());

                    // see if the decoder can decode the message
                    if (!ResponseDecoder.TryUnserialise(messageString, out object message))
                    {
                        throw new Exception($"Invalid JSON or unknown response received: {messageString}");
                    }
                    if (message == null)
                        throw new Exception("Internal error: Unexpected null");
                    return message;
                }
            }
            catch (WebSocketException ex) when (ex.InnerException is SocketException)
            {
                // closed connection and create new object
                Socket.Dispose();
                Socket = new ClientWebSocket();
            }
            catch (Exception ex) when (ex.InnerException is OperationCanceledException || ex is OperationCanceledException)
            {
                // Cancelled by the application but we want to keep connection up until explicitly socket is closed by the overlaying application.
                // throw an exception to the caller
                await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the cancelled task.", CancellationToken.None);
                Socket.Dispose();
                Socket = new ClientWebSocket();
                await ConnectAsync();
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " StackTrace:" + ex.StackTrace);
            }

            return null;
        }

        /// <summary>
        /// maximum buffer size for recieving incomming messages. 
        /// </summary>
        private const int MessageBufferSize = 3072;

        /// <summary>
        /// Endpoint of this device to connect
        /// </summary>
        private readonly Uri EndPoint;

        /// <summary>
        /// Message decoder for responses
        /// </summary>
        private static readonly MessageDecoder ResponseDecoder = new MessageDecoder(MessageDecoder.AutoPopulateType.Response);

        /// <summary>
        /// WebSocket client object
        /// </summary>
        private ClientWebSocket Socket = new ClientWebSocket();
    }
}