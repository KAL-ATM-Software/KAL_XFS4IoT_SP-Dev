/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * PartialDispenseEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashDispenser.Events
{

    [DataContract]
    [Event(Name = "CashDispenser.PartialDispenseEvent")]
    public sealed class PartialDispenseEvent : Event<PartialDispenseEvent.PayloadData>
    {

        public PartialDispenseEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(int? DispNum = null)
                : base()
            {
                this.DispNum = DispNum;
            }

            /// <summary>
            /// The number of sub-dispense operations into which the dispense operation has been divided.
            /// </summary>
            [DataMember(Name = "dispNum")]
            [DataTypes(Minimum = 1)]
            public int? DispNum { get; init; }

        }

    }
}
