﻿/***********************************************************************************************\
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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XFS4IoT
{
    public class MessageDecoder : IMessageDecoder
    {
        /// <summary>
        /// JsonSerializerOptions for the serializer.
        /// </summary>
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new Base64Converter() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public MessageDecoder(AutoPopulateType AutoPopulate, AssemblyName AssemblyName = null)
        {
            if (AutoPopulate == AutoPopulateType.Command)
            {
                searchType = typeof(CommandAttribute);
            }
            else if (AutoPopulate == AutoPopulateType.Response)
            {
                searchType = typeof(CompletionAttribute);
            }
            else if (AutoPopulate != AutoPopulateType.None)
            {
                Contracts.Fail($"Unknown Autopopulate value in {nameof(MessageDecoder)}. {AutoPopulate}");
            }

            if (searchType != null)
            {
                Add(from assem in AppDomain.CurrentDomain.GetAssemblies()
                    where assem.IsDynamic == false &&
                        (AssemblyName == null || assem.GetName().FullName == AssemblyName.FullName)
                    from type in assem.ExportedTypes
                    from attrib in type.CustomAttributes
                    where attrib.AttributeType == searchType || attrib.AttributeType == typeof(EventAttribute)
                    select type);
            }
        }

        public enum AutoPopulateType { Command, Response, None };

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
            string messageName = null;
            MessageHeader.TypeEnum? messageType = null;

            // Sniff header first and know message type
            if (searchType == typeof(CommandAttribute) ||
                searchType == typeof(CompletionAttribute))
            {
                if (!TryUnserialise(JSON, typeof(MessageDeserializerHelper<MessagePayloadBase>), out object objMessage))
                    return false;
                if (objMessage is null || objMessage is not MessageDeserializerHelper<MessagePayloadBase>)
                    Contracts.Fail($"Failed to unserialize JSON message in the {nameof(TryUnserialise)} nethod. SearchType:{searchType} Contents: {JSON}");
                MessageDeserializerHelper<MessagePayloadBase> baseMessage = objMessage as MessageDeserializerHelper<MessagePayloadBase>;
                baseMessage.IsNotNull($"Failed on type casing class to MessageDeserializerHelper.");
                messageName = baseMessage.Header.Name;
                messageType = baseMessage.Header.Type;
            }
            else
            {
                Contracts.Fail($"MessageDecoder requires to register for command or response to populate message classes.");
            }

            messageType.IsNotNull($"Failed to unserialize JOSN message type. {JSON}");
            messageName.IsNotNullOrWhitespace($"Failed to unserialize JOSN message. {JSON}");

            Type thisMessageType;

            if (messageType == MessageHeader.TypeEnum.Acknowledge)
                thisMessageType = typeof(Acknowledge);
            else if (MessageTypes.ContainsKey(messageName))
                thisMessageType = MessageTypes[messageName];
            else 
                return false;

            // Now we know the type of message and can deserialize it
            thisMessageType.BaseType.IsNotNull($"MessageType does not have base type. {nameof(thisMessageType)}");
            Contracts.Assert(thisMessageType.BaseType.IsGenericType, $"MessageType base type is not generic. {nameof(thisMessageType)}");

            Type payloadType = thisMessageType.BaseType.GetGenericArguments()[0]; //Get payload type
            Contracts.Assert(payloadType.IsSubclassOf(typeof(MessagePayloadBase)) || payloadType == typeof(MessagePayloadBase), $"Payload type was not {nameof(MessagePayloadBase)}. {nameof(payloadType)}");

            //Deserialise as MessageDeserializerHelper<payloadType> to avoid requiring MessageHeader in the "thisMessageType" constructor.
            if (TryUnserialise(JSON, typeof(MessageDeserializerHelper<>).MakeGenericType(new Type[] { payloadType }), out result))
            {
                result = ((IDeserializerHelper)result).CreateMessage(thisMessageType); //Create the message object using its constructor.
                return result != null && result.GetType() == thisMessageType;
            }
            return false;
        }
        
        /// <summary>
        /// Add a collection of message types to this decode. 
        /// </summary>
        /// <param name="types">A collection of message type classes</param>
        public void Add(IEnumerable<Type> types)
        {
            foreach (Type t in types)
                Add(t);
        }

        // Implement collection symantics so that we can work as a collection 
        // of message types. 
        /// <summary>
        /// Add a message type to this decoder
        /// </summary>
        /// <param name="type">Type of a message class</param>
        public void Add(Type type)
        {
            if (type is not null)
            {
                string attribNameValue = MessageBase.LookupMessageName(type);
                Contracts.IsNotNullOrWhitespace(attribNameValue, $"Invalid command Name attribute is set for the command or response structure. {type}");

                if (!MessageTypes.ContainsKey(attribNameValue))
                    MessageTypes.Add(attribNameValue, type);
            }
        }

        public IEnumerator GetEnumerator() => MessageTypes.Keys.GetEnumerator();
        private readonly Dictionary<string, Type> MessageTypes = new Dictionary<string, Type>();
        private Type searchType = default;

        //Interface to get access to the CreateMessage method from the MessageDeserializerHelper generic type.
        private interface IDeserializerHelper { public object CreateMessage(Type messageType); }

        //Class to use for deserializing types. 
        private class MessageDeserializerHelper<T> : Message<T>, IDeserializerHelper where T : MessagePayloadBase
        {
            public MessageDeserializerHelper(MessageHeader Header, T Payload)
                : base(Header, Payload)
            { }

            //Create the expected message type using its constructor.
            public object CreateMessage(Type messageType)
            {
                //Check for Acknowledge and copy command name (RequestId, CommandName, Payload)
                if (messageType == typeof(Acknowledge) && Header.RequestId.HasValue)
                {
                    return new Acknowledge((int)Header.RequestId, Header.Name, Header.Version, Header.Status);
                }

                //Get constructor (RequestId, payloadType, completion, errorDescription) (Completion)
                ConstructorInfo ci = messageType.GetConstructor([typeof(int), typeof(T), typeof(MessageHeader.CompletionCodeEnum), typeof(string)]);
                if (ci != null && Header.RequestId.HasValue)
                    return ci.Invoke([Header.RequestId.Value, Payload, Header.CompletionCode ?? MessageHeader.CompletionCodeEnum.Success, Header.ErrorDescription]);

                //Get constructor (RequestId, completion, errorDescription) (Completion without payload)
                ci = messageType.GetConstructor([typeof(int), typeof(MessageHeader.CompletionCodeEnum), typeof(string)]);
                if (ci != null && Header.RequestId.HasValue)
                    return ci.Invoke([Header.RequestId.Value, Header.CompletionCode ?? MessageHeader.CompletionCodeEnum.Success, Header.ErrorDescription]);

                //Get constructor (RequestId, payloadType, timeout) (Command with Payload)
                ci = messageType.GetConstructor([typeof(int), typeof(T), typeof(int)]);
                if (ci != null && Header.RequestId.HasValue)
                    return ci.Invoke([Header.RequestId.Value, Payload, Header.Timeout ?? 0]);

                //Get constructor (RequestId, timeout) (Command without Payload)
                ci = messageType.GetConstructor([typeof(int), typeof(int)]);
                if (ci != null && Header.RequestId.HasValue)
                    return ci.Invoke([Header.RequestId.Value, Header.Timeout ?? 0]);

                //Try to get constructor (RequestId, Payload) for events
                ci = messageType.GetConstructor([typeof(int), typeof(T)]);
                if (ci != null && Header.RequestId.HasValue)
                    return ci.Invoke([Header.RequestId.Value, Payload]);

                //Try to get constructor (RequestId) for events
                ci = messageType.GetConstructor([typeof(int)]);
                if (ci != null && Header.RequestId.HasValue)
                    return ci.Invoke([ Header.RequestId.Value ]);

                //Try to get constructor (payloadType) for events
                ci = messageType.GetConstructor([typeof(T)]);
                if (ci != null)
                    return ci.Invoke([Payload]);

                //Try to get constructor () for unsolic events (no request ID.) 
                ci = messageType.GetConstructor([]);
                if (ci != null)
                    return ci.Invoke([]);

                return Contracts.Fail<object>($"Unable to find constructor for type {nameof(messageType)} with parameters (RequestId, Payload, Completion) or (RequestId).");
            }
        }

    }
}