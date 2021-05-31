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
                await CommandDispatcher.DispatchError(this, commandBase, ex);
                return;
            }
            catch (Exception ex)
            {
                Contracts.Fail($"Exception caught while in serialising JSON on receiving incomming message. {ex.Message}");
            }


            try
            {
                await CommandDispatcher.Dispatch(this, commandBase);
            }
            catch (NotImplementedException ex) // Add more exception can be thrown by the device specific class
            {
                await CommandDispatcher.DispatchError(this, commandBase, ex);
            }
            catch (Exception ex)
            {
                Contracts.Fail($"Exception caught while in processing command. {ex.Message}");
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
        /// </summary>
        /// <remarks>
        /// The keyword Pattern, MaxLength, MinLength are applies to the type string object.
        /// The pattern keyword lets you define a regular expression template for the string value. Only the values that match this template will be accepted.
        /// String length can be restricted using MinLength and MaxLength.
        /// The keyword Maximum, Minimum are applied to the type int and specify the range of possible values.
        /// </remarks>
        /// <param name="message">Object to be processed</param>
        /// <returns>Process YAML value</returns>
        private string ProcessDataTypes(object message)
        {
            Contracts.IsTrue(message.GetType().IsSubclassOf(typeof(MessageBase)), $"Unexpected type of message in the {nameof(ProcessDataTypes)}. {message.GetType()}");
            return DataTypesValidation(message.GetType(), message, ((MessageBase)message).Serialise());
        }

        /// <summary>
        /// Data validation to ensure data follow rules specified in the custom attrbute "DataTypes" of the message properties.
        /// </summary>
        /// <param name="type">Data type of the message object</param>
        /// <param name="message">Message object to access variables</param>
        /// <param name="JSON">Process received JSON message to handle customer sensitive data</param>
        /// <returns>Processed YAML</returns>
        private string DataTypesValidation(Type type, object message, string JSON)
        {
            if (message is null)
                return JSON;

            string processedJson = JSON;
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                // Process each value in this object
                object value = !type.IsPrimitive && type != typeof(string)
                                ? propertyInfo.GetValue(message)
                                : message;

                if (value is null)
                    continue;

                if (propertyInfo.PropertyType.IsArray)
                {
                    foreach (var c in (Array)value)
                        processedJson = DataTypesValidation(c.GetType(), c, processedJson);
                    continue;
                }
                else if (propertyInfo.PropertyType.IsGenericType || value is ArrayList)
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
                else if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType is Object)
                {
                    processedJson = DataTypesValidation(propertyInfo.PropertyType, value, processedJson);
                    continue;
                }

                // Process each DataTypeAttribute attached to this value, which tells us how 
                // to handle this value. 
                foreach (DataTypesAttribute dataTypeAttrib in from a in propertyInfo.GetCustomAttributes()
                                                              where a is DataTypesAttribute
                                                              select a as DataTypesAttribute )
                {
                    // Check value matches the rules set by the DataTypeAttribute
                    switch (value)
                    {
                        case string stringValue:
                            if (!string.IsNullOrEmpty(dataTypeAttrib.Pattern) && !Regex.IsMatch(stringValue, dataTypeAttrib.Pattern))
                                throw new InvalidDataException($"{nameof(type)} doesn't match with the specified regilar expression. {dataTypeAttrib.Pattern}. Property:{propertyInfo.Name}");

                            if (dataTypeAttrib.MaxLength is not null && stringValue.Length > dataTypeAttrib.MaxLength)
                                throw new InvalidDataException($"{nameof(type)} has longer length than the Maximum={dataTypeAttrib.MaxLength}. Property:{propertyInfo.Name}");

                            if (dataTypeAttrib.MinLength is not null && stringValue.Length < dataTypeAttrib.MinLength)
                                throw new InvalidDataException($"{nameof(type)} has shorter length than the MinLength={dataTypeAttrib.MinLength}. Property:{propertyInfo.Name}");
                            break;
                        case int intValue:
                            if (dataTypeAttrib.Minimum is not null && intValue < dataTypeAttrib.Minimum)
                                throw new InvalidDataException($"{nameof(type)} is smaller than the Minimum={dataTypeAttrib.Minimum}. Property:{propertyInfo.Name}");

                            if (dataTypeAttrib.Maximum is not null && intValue > dataTypeAttrib.Maximum)
                                throw new InvalidDataException($"{nameof(type)} is greater than the Maximum={dataTypeAttrib.Maximum}. Property:{propertyInfo.Name}");
                            break;
                    }

                    // If the this is tagged as sensitive, mask out the sensitive parts. 
                    if (!dataTypeAttrib.Sensitive)
                        continue;

                    string camelCaseName = propertyInfo.Name.Length > 1 
                                            ? $"{char.ToLowerInvariant(propertyInfo.Name[0])}{propertyInfo.Name[1..]}" 
                                            : propertyInfo.Name.ToLower();

                    // Replacing all sensitive data matching with key name
                    (string pattern, string replace) = value switch
                    {
                        string      => ($"\"{camelCaseName}\":\"(.+?)\"", $"\"{camelCaseName}\"<:\"$1\">"),
                        Int16  or Int32  or Int64  or
                        UInt16 or UInt32 or UInt64 or
                        Char or Byte or Decimal     
                                    => ($"\"{camelCaseName}\":([0-9]+)", $"\"{camelCaseName}\"<:$1>"),

                        _ =>(null, null)
                    };

                    if (pattern == null || replace == null )
                        Logger.Log(Constants.Component, $"Logging sensitive data is not supporting data type {Type.GetTypeCode(value.GetType())}");
                    else                    
                        processedJson = Regex.Replace(processedJson, pattern, replace, RegexOptions.Multiline);
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
