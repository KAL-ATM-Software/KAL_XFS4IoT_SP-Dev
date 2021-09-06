/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * DelayedDispenseEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashDispenser.Events
{

    [DataContract]
    [Event(Name = "CashDispenser.DelayedDispenseEvent")]
    public sealed class DelayedDispenseEvent : Event<DelayedDispenseEvent.PayloadData>
    {

        public DelayedDispenseEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(int? Delay = null)
                : base()
            {
                this.Delay = Delay;
            }

            /// <summary>
            /// The time in milliseconds by which the dispense operation will be delayed.
            /// </summary>
            [DataMember(Name = "delay")]
            public int? Delay { get; init; }

        }

    }
}
