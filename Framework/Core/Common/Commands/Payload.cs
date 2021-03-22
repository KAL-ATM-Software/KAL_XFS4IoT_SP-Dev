/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Runtime.Serialization;

namespace XFS4IoT.Commands
{
    /// <summary>
    /// A message object representing payload for commands
    /// </summary>
    [DataContract]
    public class MessagePayload : MessagePayloadBase
    {
        /// <summary>
        /// The original message name, for example "CardReader.Status"
        /// </summary>
        [DataMember(IsRequired = true, Name = "timeout")]
        public int Timeout { get; private set; }

        /// <summary>
        /// Constructor for the common command payload
        /// </summary>
        /// <param name="Timeout"></param>
        public MessagePayload(int Timeout)
        {
            this.Timeout = Timeout;
        }
    }
}