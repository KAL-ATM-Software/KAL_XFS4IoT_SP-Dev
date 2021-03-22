/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
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
        /// JSON must contain an Name field in the Headers. 
        /// JSON must match type registered for that Name. 
        /// </remarks>
        /// <param name="JSON">JSON for object</param>
        /// <param name="result">Resulting Message object</param>
        /// <returns>true if de-serialisation worked</returns>
        public bool TryUnserialise(string JSON, out object result)
        {
            result = default;
            string messageName = null;

            // Sniff headers first and know message type
            if (searchType == typeof(CommandAttribute) ||
                searchType == typeof(CompletionAttribute))
            {
                if (!TryUnserialise(JSON, typeof(MessageDeserializerHelper<MessagePayloadBase>), out object objMessage))
                    return false;
                if (objMessage is null || objMessage is not MessageDeserializerHelper<MessagePayloadBase>)
                    Contracts.Fail($"Failed to unserialize JSON message in the {nameof(TryUnserialise)} nethod. SearchType:{searchType} Contents: {JSON}");
                MessageDeserializerHelper<MessagePayloadBase> baseMessage = objMessage as MessageDeserializerHelper<MessagePayloadBase>;
                messageName = baseMessage.Headers.Name;
            }
            else
            {
                Contracts.Fail($"MessageDecoder requires to register for command or response to populate message classes.");
            }

            Contracts.IsNotNullOrWhitespace(messageName, $"Failed to unserialize JOSN message. {JSON}");

            if (!MessageTypes.ContainsKey(messageName))
                return false;

            // Now we know the type of message and deserialize it
            Type thisMessageType = MessageTypes[messageName];
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
            string attribNameValue = MessageBase.LookupMessageName(type);
            Contracts.IsNotNullOrWhitespace(attribNameValue, $"Invalid command Name attribute is set for the command or response structure. {type}");

            if (!MessageTypes.ContainsKey(attribNameValue))
                MessageTypes.Add(attribNameValue, type);
        }

        public IEnumerator GetEnumerator() => MessageTypes.Keys.GetEnumerator();
        private readonly Dictionary<string, Type> MessageTypes = new Dictionary<string, Type>();
        private Type searchType = default;

        //Interface to get access to the CreateMessage method from the MessageDeserializerHelper generic type.
        private interface IDeserializerHelper { public object CreateMessage(Type messageType); }

        //Class to use for deserializing types. 
        private class MessageDeserializerHelper<T> : Message<T>, IDeserializerHelper where T : MessagePayloadBase
        {
            public MessageDeserializerHelper(MessageHeader Headers, T Payload)
                : base(Headers, Payload)
            { }

            //Create the expected message type using its constructor.
            public object CreateMessage(Type messageType)
            {
                ConstructorInfo ci = messageType.GetConstructor(new Type[] { typeof(string), typeof(T) }); //Get constructor (RequestId, payloadType)
                if (ci != null)
                {
                    return ci.Invoke(new object[] { Headers.RequestId, Payload });
                }
                else //Try to get constructor (RequestId) for events
                {
                    ci = messageType.GetConstructor(new Type[] { typeof(string) });
                    ci.IsNotNull($"Unable to find constructor for type {nameof(messageType)} with parameters (RequestId, Payload) or (RequestId).");
                    return ci.Invoke(new object[] { Headers.RequestId });
                }
            }
        }

    }
}