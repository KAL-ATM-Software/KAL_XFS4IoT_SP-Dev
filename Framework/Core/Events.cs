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
        /// Event class to be derived from device class
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="Payload"></param>
        public Event(string RequestId, T Payload = null) :
            base(RequestId, MessageHeader.TypeEnum.Event, Payload)
        {
            if (Payload is null)
            {
                Contracts.Assert(typeof(T) == typeof(MessagePayloadBase), $"Invalid Payload in {nameof(Event<T>)} constructor. Payload can not be null.");
                this.Payload = (T)new MessagePayloadBase();
            }
        }
    }

    [DataContract]
    public abstract class Acknowledgement<T> : Message<T> where T : MessagePayloadBase
    {
        /// <summary>
        /// Acknowledgement event class derived from the device class
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="Payload"></param>
        public Acknowledgement(string RequestId, T Payload = null) :
            base(RequestId, MessageHeader.TypeEnum.Acknowledgement, Payload)
        {
            if (Payload is null)
            {
                Contracts.Assert(typeof(T) == typeof(MessagePayloadBase), $"Invalid Payload in {nameof(Acknowledgement<T>)} constructor. Payload can not be null.");
                this.Payload = (T)new MessagePayloadBase();
            }
        }
    }
}