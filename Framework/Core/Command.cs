/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
        /// Initialise any command objects
        /// </summary>
        /// <param name="RequestId">request id</param>
        /// <param name="Payload">payload contents</param>
        public Command(int RequestId, T Payload) :
            base(RequestId, MessageHeader.TypeEnum.Command, Payload)
        { }
    }
}