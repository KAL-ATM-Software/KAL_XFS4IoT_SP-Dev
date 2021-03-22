/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

namespace XFS4IoTServer
{
    internal class ClientConnection : IConnection
    {
        public ClientConnection(WebSocket socket, 
                                IMessageDecoder CommandDecoder, 
                                ICommandDispatcher CommandDispatcher,
                                ILogger Logger
                                )
        {
            this.socket = socket;
            this.CommandDecoder = CommandDecoder;
            this.CommandDispatcher = CommandDispatcher;
            this.Logger = Logger;
        }

        public async Task RunAsync()
        {
            try
            {
                while (true)
                {
                    // client is no longer connected
                    if (socket.State != WebSocketState.Open)
                        break;

                    var receivedBuffer = new ArraySegment<byte>(new byte[MAX_BUFFER]);

                    /// Wait for data from the client
                    var res = await socket.ReceiveAsync(receivedBuffer, CancellationToken.None);

                    if (res.MessageType == WebSocketMessageType.Text ||
                        res.MessageType == WebSocketMessageType.Binary)
                    {
                        string message = Encoding.UTF8.GetString(receivedBuffer.Take(res.Count).ToArray());

                        await HandleIncommingMessage(message);
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
                Logger.Warning(Constants.Component, $"Unexpected exception: {ex.Message}");
                throw; 
            }
            finally
            {
                // Remove client from the list 
                socket.Dispose();
            }
        }

        private readonly IMessageDecoder CommandDecoder;

        private async Task HandleIncommingMessage(string messageString)
        {
            bool rc = CommandDecoder.TryUnserialise(messageString, out object command);
            Contracts.IsTrue(rc, $"Invalid JSON or unknown command received in the {nameof(HandleIncommingMessage)} method.");
            Contracts.Assert(command is not null, $"Failed on unserialzing received JSON in ${nameof(HandleIncommingMessage)} method. JSON:{messageString}");

            MessageBase commandBase = command as MessageBase;
            try
            {
                Logger.LogSensitive(Constants.Component, $"Received:{ProcessDataTypes(command)}");
            }
            catch (InvalidDataException ex)
            {
                await CommandDispatcher.DispatchError(this, command, ex);
                return;
            }
            catch (Exception ex)
            {
                Contracts.Fail($"Exception caught while in serialising JSON on receiving incomming message. {ex.Message}");
            }

            CancellationTokenSource cts = new CancellationTokenSource();

            try
            {
                await CommandDispatcher.Dispatch(this, command, cts.Token);
            }
            catch (NotImplementedException ex) // Add more exception can be thrown by the device specific class
            {
                await CommandDispatcher.DispatchError(this, command, ex);
            }
            catch (Exception ex)
            {
                Contracts.Fail($"Exception caught while in processing command. {ex.Message}");
            }

            cts.Dispose();
        }

        public async Task SendMessageAsync(object messsage)
        {
            // Allow to send command/response/event/acknowledgement type of mesage derived from MessageBase class
            MessageBase messageBase = messsage as MessageBase;
            messageBase.IsNotNull($"Unexpected type of object received to serialize message in the ${nameof(SendMessageAsync)} method.");

            try
            {
                string JSON = messageBase.Serialise();
                Logger.LogSensitive(Constants.Component, $"Sending: {ProcessDataTypes(messsage)}");

                await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JSON)), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (ObjectDisposedException)
            {
                Logger.Warning(Constants.Component, "The ClientWebSocket has been closed");
            }
            catch (InvalidOperationException)
			{
                Logger.Warning(Constants.Component, "The ClientWebSocket is not connected");
            }
            catch (InvalidDataException ex)
            {
                Contracts.Fail($"Invalid data is set by the device class.{messageBase.Headers.Name}. {ex.Message}");
            }
            catch (Exception ex)
            {
                Contracts.Fail($"Exception caught while in serialising JSON on sending message. {ex.Message}");
            }
        }

        /// <summary>
        /// Process properties in the given message object to check the custom attribute 'DataTypes' are set. The 'DataTypes' customer attributes are converted from the YAML.
        /// 
        /// The keyword Pattern, MaxLength, MinLength are applies to the type string object.
        /// The pattern keyword lets you define a regular expression template for the string value. Only the values that match this template will be accepted.
        /// String length can be restricted using MinLength and MaxLength.
        /// The keyword Maximum, Minimum are applied to the type int and specify the range of possible values.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string ProcessDataTypes(object message)
        {
            Contracts.IsTrue(message.GetType().IsSubclassOf(typeof(MessageBase)), $"Unexpected type of message in the {nameof(ProcessDataTypes)}. {message.GetType()}");
            return DataTypesValidation(message.GetType(), message, ((MessageBase)message).Serialise());
        }

        /// <summary>
        /// Data validation to ensure data follow rules specified in the custom attrbute "DataTypes" of the message properties.
        /// </summary>
        /// <param name="type">Data type of the message object</param>
        /// <param name="message">Message objec to access variables</param>
        /// <param name="JSON">Process received JSON message to handle customer sensitive data</param>
        /// <returns></returns>
        private string DataTypesValidation(Type type, object message, string JSON)
        {
            string processedJson = JSON;
            if (message is null)
                return processedJson;

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                var value = message;

                if (!type.IsPrimitive && type != typeof(string))
                    value = propertyInfo.GetValue(message);
            
                if (value is null)
                    continue;

                if (propertyInfo.PropertyType.IsArray)
                {
                    foreach (var c in (Array)value)
                        processedJson = DataTypesValidation(c.GetType(), c, processedJson);
                    continue;
                }
                else if (propertyInfo.PropertyType.IsGenericType ||
                         value.GetType() == typeof(ArrayList))
                {
                    if (propertyInfo.PropertyType.GetGenericArguments().Length > 1)
                        continue; // Dictionary is not supported yet

                    if (value is IEnumerable enumerable)
                    {
                        foreach (var c in enumerable)
                            processedJson = DataTypesValidation(c.GetType(), c, processedJson);
                    }
                    continue;
                }
                else if (propertyInfo.PropertyType.IsClass &&
                        Type.GetTypeCode(propertyInfo.PropertyType) == TypeCode.Object)
                {
                    processedJson = DataTypesValidation(propertyInfo.PropertyType, value, processedJson);
                    continue;
                }

                foreach (Attribute attrib in propertyInfo.GetCustomAttributes())
                {
                    if (attrib.GetType() != typeof(DataTypesAttribute))
                        continue;

                    DataTypesAttribute dataTypeAttrib = (DataTypesAttribute)attrib;
                    if (value.GetType() == typeof(string))
                    {
                        if (!string.IsNullOrEmpty(dataTypeAttrib.Pattern))
                        {
                            if (!Regex.IsMatch((string)value, dataTypeAttrib.Pattern))
                                throw new InvalidDataException($"{nameof(type)} doesn't match with the specified regilar expression. {dataTypeAttrib.Pattern}. Property:{propertyInfo.Name}");
                        }
                        if (dataTypeAttrib.MaxLength is not null)
                        {
                            if (((string)value).Length > dataTypeAttrib.MaxLength)
                                throw new InvalidDataException($"{nameof(type)} has longer length than the Maximum={dataTypeAttrib.MaxLength}. Property:{propertyInfo.Name}");
                        }
                        if (dataTypeAttrib.MinLength is not null)
                        {
                            if (((string)value).Length < dataTypeAttrib.MinLength)
                                throw new InvalidDataException($"{nameof(type)} has shorter length than the MinLength={dataTypeAttrib.MinLength}. Property:{propertyInfo.Name}");
                        }
                    }
                    else if (value.GetType() == typeof(int))
                    {
                        if (dataTypeAttrib.Minimum is not null)
                        {
                            if ((int)value < dataTypeAttrib.Minimum)
                                throw new InvalidDataException($"{nameof(type)} is smaller than the Minimum={dataTypeAttrib.Minimum}. Property:{propertyInfo.Name}");
                        }
                        if (dataTypeAttrib.Maximum is not null)
                        {
                            if ((int)value > dataTypeAttrib.Maximum)
                                throw new InvalidDataException($"{nameof(type)} is greater than the Maximum={dataTypeAttrib.Maximum}. Property:{propertyInfo.Name}");
                        }
                    }

                    if (!dataTypeAttrib.Sensitive)
                        continue;

                    string camelCaseName = propertyInfo.Name;
                    if (camelCaseName.Length > 1)
                        camelCaseName = Char.ToLowerInvariant(camelCaseName[0]) + camelCaseName.Substring(1);
                    else
                        camelCaseName = camelCaseName.ToLower();

                    // Replacing all sensitive data matching with key name
                    string pattern = string.Empty;
                    string replace = string.Empty;
                    switch (Type.GetTypeCode(value.GetType()))
                    {
                        case TypeCode.String:
                            pattern = $"\"{camelCaseName}\":\"(.+?)\"";
                            replace = $"\"{camelCaseName}\"<:\"$1\">";
                            break;
                        case TypeCode.Int64:
                        case TypeCode.Int32:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                        case TypeCode.Char:
                        case TypeCode.Byte:
                        case TypeCode.Decimal:
                            pattern = $"\"{camelCaseName}\":([0-9]+)";
                            replace = $"\"{camelCaseName}\"<:$1>";
                            break;
                        default:
                            Logger.Log(Constants.Component, $"Logging sensitive data is not supporting data type {Type.GetTypeCode(value.GetType())}");
                            break;
                    }

                    if (!string.IsNullOrEmpty(pattern) &&
                        !string.IsNullOrEmpty(replace))
                    {
                        processedJson = Regex.Replace(processedJson, pattern, replace, RegexOptions.Multiline);
                    }
                }
            }

            return processedJson;
        }

        private readonly WebSocket socket;
        private readonly ILogger Logger;
        private const int MAX_BUFFER = 4096;
        private readonly ICommandDispatcher CommandDispatcher;
    }
}
