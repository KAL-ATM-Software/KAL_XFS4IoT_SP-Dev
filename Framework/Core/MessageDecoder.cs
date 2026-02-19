/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using XFS4IoT.CardReader.Completions;

namespace XFS4IoT
{
    public class GenericMessageClass
    {
        /// <summary>
        /// Represents the Header part of a generic message
        /// </summary>
        public class HeaderClass
        {
            /// <summary>
            /// Name of the commands, events or completions
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Unique request ID of the message
            /// </summary>
            public int? RequestId { get; set; } = default;

            /// <summary>
            /// Type of the message
            /// </summary>
            public MessageHeader.TypeEnum? Type { get; set; } = default;

            /// <summary>
            /// Version of the message, e.g. "1.0"
            /// </summary>
            public string Version { get; set; }

            /// <summary>
            /// Timeout on handling the message in milliseconds
            /// </summary>
            public int? Timeout { get; set; } = default;

            /// <summary>
            /// 
            /// </summary>
            public object Status { get; set; } = default;

            /// <summary>
            /// Completion code of the message
            /// </summary>
            public object CompletionCode { get; set; } = default;

            /// <summary>
            /// Description of error if any
            /// </summary>
            public string ErrorDescription { get; set; } = default;
        }

        /// <summary>
        /// Header of the generic message
        /// </summary>
        public HeaderClass Header { get; set; } = null;

        /// <summary>
        /// Payload of the generic message
        /// </summary>
        public object Payload { get; set; } = null;
    }

    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, UseStringEnumConverter = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonSerializable(typeof(GenericMessageClass))]
    [JsonSerializable(typeof(MessageHeader.TypeEnum))]
    [JsonSerializable(typeof(GenericMessageClass.HeaderClass))]
    public partial class GenericMessageContext : JsonSerializerContext
    {
    }

    public class MessageDecoder : IMessageDecoder
    {
        /// <summary>
        /// JsonSerializerOptions for the serializer.
        /// </summary>
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new Base64Converter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public MessageDecoder(Assembly[] assemblies = null)
        {
            if (assemblies is null)
            {
                return;
            }

            foreach (var assem in assemblies.Where(assem => assem.IsDynamic == false))
            {
                foreach (var type in assem.ExportedTypes)
                {
                    foreach (var attrib in type.CustomAttributes)
                    {
                        if (attrib.AttributeType == typeof(CommandAttribute) ||
                            attrib.AttributeType == typeof(CompletionAttribute) ||
                            attrib.AttributeType == typeof(EventAttribute))
                        {
                            string name = attrib.NamedArguments.First(n => n.MemberName == "Name").TypedValue.Value.ToString();
                            MessageHeader.TypeEnum messageType = MessageHeader.TypeEnum.Event;
                            if (attrib.AttributeType == typeof(CommandAttribute))
                            {
                                messageType = MessageHeader.TypeEnum.Command;
                            }
                            else if (attrib.AttributeType == typeof(CompletionAttribute))
                            {
                                messageType = MessageHeader.TypeEnum.Completion;
                            }
                            MessageCollection.Add(messageType, name, type);
                        }
                    }
                }
            }
        }

        private static bool TryUnserialise(string JSON, Type type, JsonSerializerContext context, out object result)
        {
            result = default;
            try
            {
                result = JsonSerializer.Deserialize(JSON, type, context);
                return (result != null) && (result.GetType().Name == type.Name);
            }
            catch (JsonException) //Can occur when json is not in correct format
            {
                return false;
            }
            catch (InvalidOperationException) //Can occur when JsonSerializer is unable to construct object from json data.
            {
                return false;
            }
        }
        private static bool TryUnserialise(string JSON, Type type, out object result)
        {
            result = default;
            try
            {
                result = JsonSerializer.Deserialize(JSON, type, JsonOptions);
                return (result != null) && (result.GetType().Name == type.Name);
            }
            catch (JsonException) //Can occur when json is not in correct format
            {
                return false;
            }
            catch (InvalidOperationException) //Can occur when JsonSerializer is unable to construct object from json data.
            {
                return false;
            }
        }

        /// <summary>
        /// Unserialise to an object based on the Name of header 
        /// registered with this MessageDecoder collection. 
        /// </summary>
        /// <remarks>
        /// JSON must contain an Name field in the Header. 
        /// JSON must match type registered for that Name. 
        /// </remarks>
        /// <param name="JSON">JSON for object</param>
        /// <param name="result">Resulting Message object</param>
        /// <returns>true if de-serialisation worked</returns>
        public bool TryUnserialise(string JSON, out object result)
        {
            result = default;
            // Sniff header first and know message type
            if (!TryUnserialise(JSON, typeof(GenericMessageClass), GenericMessageContext.Default, out result))
            {
                return false;
            }

            string messageName = ((GenericMessageClass)result).Header.Name;
            MessageHeader.TypeEnum? messageType = ((GenericMessageClass)result).Header.Type;

            messageType.IsNotNull($"Failed to unserialize JOSN message type. {JSON}");
            messageName.IsNotNullOrWhitespace($"Failed to unserialize JOSN message. {JSON}");

            if (messageType == MessageHeader.TypeEnum.Acknowledge)
            {
                return true;
            }

            Type thisMessageType = null;
            List<MessageHeader.TypeEnum> searchType = [MessageHeader.TypeEnum.Command, MessageHeader.TypeEnum.Event, MessageHeader.TypeEnum.Completion];
            foreach (var type in searchType)
            {
                if (messageType != type)
                {
                    continue;
                }
                if (MessageCollection.TryGetMessageType(type, messageName, out Type thisType))
                {
                    thisMessageType = thisType;
                    break;
                }
            }
            if (thisMessageType is null)
            {
                throw new InternalErrorException($"Message type {messageName} not registered with MessageCollection. JSON: {JSON}");
            }

            result = default;
            if (!TryUnserialise(JSON, thisMessageType, out result))
            {
                return false;
            }

            // Now we know the type of message and can deserialize it
            thisMessageType.BaseType.IsNotNull($"MessageType does not have base type. {nameof(thisMessageType)}");
            Contracts.Assert(thisMessageType.BaseType.IsGenericType, $"MessageType base type is not generic. {nameof(thisMessageType)}");

            Type payloadType = thisMessageType.BaseType.GetGenericArguments()[0]; //Get payload type
            Contracts.Assert(payloadType.IsSubclassOf(typeof(MessagePayloadBase)) || payloadType == typeof(MessagePayloadBase), $"Payload type was not {nameof(MessagePayloadBase)}. {nameof(payloadType)}");

            return true;
        }
    }
}