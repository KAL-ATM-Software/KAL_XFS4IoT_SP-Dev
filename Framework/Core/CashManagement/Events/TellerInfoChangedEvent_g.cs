/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * TellerInfoChangedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashManagement.Events
{

    [DataContract]
    [Event(Name = "CashManagement.TellerInfoChangedEvent")]
    public sealed class TellerInfoChangedEvent : UnsolicitedEvent<TellerInfoChangedEvent.PayloadData>
    {

        public TellerInfoChangedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(int? TellerID = null)
                : base()
            {
                this.TellerID = TellerID;
            }

            /// <summary>
            /// Integer holding the ID of the teller whose counts have changed.
            /// </summary>
            [DataMember(Name = "tellerID")]
            public int? TellerID { get; init; }

        }

    }
}
