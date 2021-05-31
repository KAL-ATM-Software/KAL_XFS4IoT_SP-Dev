/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * RetainBinThresholdEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CardReader.Events
{

    [DataContract]
    [Event(Name = "CardReader.RetainBinThresholdEvent")]
    public sealed class RetainBinThresholdEvent : UnsolicitedEvent<RetainBinThresholdEvent.PayloadData>
    {

        public RetainBinThresholdEvent(PayloadData Payload)
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
                Ok,
                Full,
                High
            }

            /// <summary>
            /// Specifies the state of the ID card unit retain bin as one of the following:
            /// 
            /// * ```ok``` - The retain bin of the ID card unit was emptied.
            /// * ```full``` - The retain bin of the ID card unit is full.
            /// * ```high``` - The retain bin of the ID card unit is nearly full.
            /// </summary>
            [DataMember(Name = "state")]
            public StateEnum? State { get; private set; }

        }

    }
}
