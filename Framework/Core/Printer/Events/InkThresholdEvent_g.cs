/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * InkThresholdEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [Event(Name = "Printer.InkThresholdEvent")]
    public sealed class InkThresholdEvent : UnsolicitedEvent<InkThresholdEvent.PayloadData>
    {

        public InkThresholdEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(StateEnum? State = null)
                : base()
            {
                this.State = State;
            }

            public enum StateEnum
            {
                Full,
                Low,
                Out
            }

            /// <summary>
            /// Specifies the current state of the stamping ink as one of the following:
            /// 
            /// * ```full``` - The stamping ink in the printer is in a good state.
            /// * ```low``` - The stamping ink in the printer is low.
            /// * ```out``` - The stamping ink in the printer is out.
            /// </summary>
            [DataMember(Name = "state")]
            public StateEnum? State { get; init; }

        }

    }
}
