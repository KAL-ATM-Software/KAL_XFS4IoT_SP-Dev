/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Runtime.Serialization;

namespace XFS4IoT.Commands
{
    [DataContract]
    public abstract class Command<T> : Message<T> where T : MessagePayloadBase
    {
        /// <summary>
        /// Default constructor is required for JsonSerializer to create an instance of the class
        /// </summary>
        public Command() : base()
        { }

        /// <summary>
        /// Initialise any command objects
        /// </summary>
        /// <param name="RequestId">request id</param>
        /// <param name="Payload">payload contents</param>
        /// <param name="Timeout">command timeout</param>
        public Command(int RequestId, T Payload, int Timeout) :
            base(RequestId, MessageHeader.TypeEnum.Command, Payload, Timeout)
        { }
    }
}