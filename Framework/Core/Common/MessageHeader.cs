/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Runtime.Serialization;

namespace XFS4IoT
{
    /// <summary>
    /// A message object representing header
    /// </summary>
    [DataContract]
    public sealed class MessageHeader
    {
        /// <summary>
        /// The original message name, for example "CardReader.Status"
        /// </summary>
        [DataMember(IsRequired = true, Name = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// Unique request identifier supplied by the client used to correlate the command with responses, events and
        /// completions.For Unsolicited Events the field will be empty.
        /// example: "b34800d0-9dd2-4d50-89ea-92d1b13df54b"
        /// </summary>
        [DataMember(IsRequired = true, Name = "requestId")]
        public string RequestId { get; private set; }

        /// <summary>
        /// Possible type of message 
        /// </summary>
        [DataContract]
        public enum TypeEnum
        {
            Command,
            Acknowledgement,
            Event,
            Completion,
            Unsolicited,
        }

        /// <summary>
        /// The original message name, for example "CardReader.Status"
        /// </summary>
        [DataMember(IsRequired = true, Name = "type")]
        public TypeEnum Type { get; private set; }

        /// <summary>
        /// MessageHeader class representing XFS4 message header
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="RequestId"></param>
        /// <param name="Type"></param>
        public MessageHeader(string Name, string RequestId, TypeEnum Type)
        {
            Contracts.IsNotNullOrWhitespace(Name, $"Null or an empty value for {nameof(Name)} in the headers.");
            Contracts.IsNotNullOrWhitespace(RequestId, $"Null or an empty value for {nameof(RequestId)} in the headers.");

            this.Name = Name;
            this.RequestId = RequestId;
            this.Type = Type;
        }
    }
}
