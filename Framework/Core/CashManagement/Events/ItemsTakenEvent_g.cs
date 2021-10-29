/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * ItemsTakenEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashManagement.Events
{

    [DataContract]
    [Event(Name = "CashManagement.ItemsTakenEvent")]
    public sealed class ItemsTakenEvent : Event<ItemsTakenEvent.PayloadData>
    {

        public ItemsTakenEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(PositionInfoClass Position = null)
                : base()
            {
                this.Position = Position;
            }

            [DataMember(Name = "position")]
            public PositionInfoClass Position { get; init; }

        }

    }
}
