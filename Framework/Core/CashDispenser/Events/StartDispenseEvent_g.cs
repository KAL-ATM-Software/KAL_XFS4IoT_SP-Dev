/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * StartDispenseEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashDispenser.Events
{

    [DataContract]
    [Event(Name = "CashDispenser.StartDispenseEvent")]
    public sealed class StartDispenseEvent : Event<StartDispenseEvent.PayloadData>
    {

        public StartDispenseEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(int? ReqID = null)
                : base()
            {
                this.ReqID = ReqID;
            }

            /// <summary>
            /// The requestId of the original dispense command.
            /// </summary>
            [DataMember(Name = "reqID")]
            public int? ReqID { get; init; }

        }

    }
}
