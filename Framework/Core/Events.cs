/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Runtime.Serialization;

namespace XFS4IoT.Events
{
    [DataContract]
    public abstract class Event<T> : Message<T> where T : MessagePayloadBase
    {
        public Event() : base()
        { }

        /// <summary>
        /// Event message class to be derived from.
        /// </summary>
        /// <param name="RequestId">Request id of the command this even relates to</param>
        /// <param name="Payload">Data for the event</param>
        public Event(int RequestId, T Payload = null) :
            base(RequestId, MessageHeader.TypeEnum.Event, Payload, Timeout: null)
        {
        }
    }

    [DataContract]
    public abstract class UnsolicitedEvent<T> : Message<T> where T : MessagePayloadBase
    {
        /// <summary>
        /// Unsolicited event message class to be derived from.
        /// </summary>
        /// <param name="Payload">Data for the event</param>
        public UnsolicitedEvent(T Payload = null) :
            base(RequestId: null, MessageHeader.TypeEnum.Event, Payload, Timeout: null)
        {
        }
    }
}