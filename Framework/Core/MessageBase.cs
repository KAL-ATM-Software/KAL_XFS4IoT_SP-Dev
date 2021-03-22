/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Runtime.Serialization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace XFS4IoT
{
    /// <summary>
    /// A base class of the message object
    /// </summary>
    [DataContract]
    public abstract class MessageBase : ICloneable
    {
        /// <summary>
        /// JsonSerializerOptions for the serializer.
        /// </summary>
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true
        };

        /// <summary>
        /// Headers of the message for command
        /// </summary>
        [DataMember(IsRequired = true, Name = "headers")]
        public MessageHeader Headers { get; private set; }

        /// <summary>
        /// Constructor of the base message object
        /// </summary>
        /// <param name="RequestId">Request ID</param>
        /// <param name="Type">Type of the message, Command, Response, Events</param>
        public MessageBase(string RequestId, MessageHeader.TypeEnum Type)
        {
            Contracts.IsNotNullOrWhitespace(RequestId, $"Invalid received in the {nameof(MessageBase)} constructor. { RequestId }");

            string attribNameValue = LookupMessageName(GetType());
            Contracts.IsNotNullOrWhitespace(attribNameValue, $"Invalid command Name attribute is set for the command or response structure in the {nameof(MessageBase)} constructor. { Type }");
            this.Headers = new MessageHeader(attribNameValue, RequestId, Type);
        }

        /// <summary>
        /// Internal constructor of the base message object
        /// For use by JsonSerializer.
        /// </summary>
        /// <param name="Headers">header contents</param>
        internal MessageBase(MessageHeader Headers)
        {
            Headers.IsNotNull($"Invalid header received in the {nameof(MessageBase)} constructor.");
            Contracts.IsNotNullOrWhitespace(Headers.RequestId, $"Invalid received in the {nameof(MessageBase)} constructor. { Headers.RequestId }");
            Contracts.IsNotNullOrWhitespace(Headers.Name, $"Invalid command Name attribute is set for the command or response structure in the {nameof(MessageBase)} constructor. { Headers.Type }");
            this.Headers = Headers;
        }

        /// <summary>
        /// Serialise this object in JSON format
        /// </summary>
        /// <returns></returns>
        public string Serialise() => JsonSerializer.Serialize(this, GetType(), JsonOptions);

        /// <summary>
        /// Deep copy of the message object
        /// </summary>
        /// <returns>Copied message object</returns>
        public object Clone() => MemberwiseClone();

        /// <summary>
        /// LookupMessageName
        /// Look up message name for commands and response with the custom attributes
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string LookupMessageName(Type type)
        {
            var attribNames = from attrib in type.CustomAttributes
                              from n in attrib.NamedArguments
                              where n.MemberName is "Name" && n.TypedValue.ArgumentType == typeof(string)
                              select n.TypedValue.Value;

            foreach (string s in attribNames)
                return s;
        
            return null;
        }
    }
}