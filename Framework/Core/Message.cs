/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Runtime.Serialization;


namespace XFS4IoT
{
    /// <summary>
    /// A payload object
    /// </summary>
    [DataContract]
    public class MessagePayloadBase
    {
        /// <summary>
        /// MessageHeader class representing XFS4 message header
        /// The XFS4IoT payload can't be a null value and need an empty structure to be set for an empty payload in brackets
        /// </summary>
        public MessagePayloadBase()
        { }
    }

    /// <summary>
    /// A message class for the response message strcuture
    /// </summary>
    [DataContract]
    public class Message<T> : MessageBase where T : MessagePayloadBase
    {
        /// <summary>
        /// Payload of the message for response and this set by the derived class
        /// </summary>
        [DataMember(IsRequired = true, Name = "payload")]
        public T Payload { get; protected set; }

        /// <summary>
        /// Internal constructor of the message object for response
        /// The Payload property must be set by the derived class and passing paramters for the headers to the base class
        /// For use by JsonSerializer.
        /// </summary>
        /// <param name="Headers">header contents</param>
        /// <param name="Payload">payload contents</param>
        internal Message(MessageHeader headers, T Payload) :
            base(headers)
        {
            this.Payload = Payload.Ignore();
        }

        /// <summary>
        /// Constructor of the message object for response
        /// The Payload property must be set by the derived class and passing paramters for the headers to the base class
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="RequestId"></param>
        /// <param name="Type"></param>
        public Message(int? RequestId, MessageHeader.TypeEnum Type, T Payload) :
            base(RequestId, Type)
        {
            this.Payload = Payload.Ignore();
        }
    }
}