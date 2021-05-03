/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * TonerThresholdEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [Event(Name = "Printer.TonerThresholdEvent")]
    public sealed class TonerThresholdEvent : UnsolicitedEvent<TonerThresholdEvent.PayloadData>
    {

        public TonerThresholdEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public enum StateEnum
            {
                Full,
                Low,
                Out,
            }


            public PayloadData(StateEnum? State = null)
                : base()
            {
                this.State = State;
            }

            /// <summary>
            /// Specifies the current state of the toner (or ink) as one of the following:
            /// 
            /// * ```full``` - The toner (or ink) in the printer is in a good state.
            /// * ```low``` - The toner (or ink) in the printer is low.
            /// * ```out``` - The toner (or ink) in the printer is out.
            /// </summary>
            [DataMember(Name = "state")] 
            public StateEnum? State { get; private set; }
        }

    }
}
