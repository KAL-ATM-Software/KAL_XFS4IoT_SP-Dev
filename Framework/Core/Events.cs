/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Runtime.Serialization;

namespace XFS4IoT.Events
{
    [DataContract]
    public abstract class Event<T> : Message<T> where T : MessagePayloadBase
    {
        /// <summary>
        /// Event message class to be derived from.
        /// </summary>
        /// <param name="RequestId">Request id of the command this even relates to</param>
        /// <param name="Payload">Data for the event</param>
        public Event(string RequestId, T Payload = null) :
            base(RequestId, MessageHeader.TypeEnum.Event, Payload)
        {
            if (Payload is null)
            {
                this.Payload = new MessagePayloadBase().IsA<T>($"Invalid Payload in {nameof(Event<T>)} constructor. Payload can not be null.");
            }
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
            base(null, MessageHeader.TypeEnum.Event, Payload)
        {
            if (Payload is null)
            {
                this.Payload = new MessagePayloadBase().IsA<T>($"Invalid Payload in {nameof(UnsolicitedEvent<T>)} constructor. Payload can not be null.");
            }
        }
    }

    [DataContract]
    public abstract class Acknowledgement<T> : Message<T> where T : MessagePayloadBase
    {
        /// <summary>
        /// Acknowledgement event message class to be derived from.
        /// </summary>
        /// <param name="RequestId">Request id of the command this even relates to. May be null in some error cases.</param>
        /// <param name="Payload">Data for the event</param>
        public Acknowledgement(string RequestId, T Payload = null) :
            base(RequestId, MessageHeader.TypeEnum.Acknowledgement, Payload)
        {
            if (Payload is null)
            {
                this.Payload = new MessagePayloadBase().IsA<T>($"Invalid Payload in {nameof(Acknowledgement<T>)} constructor. Payload can not be null.");
            }
        }
    }
}