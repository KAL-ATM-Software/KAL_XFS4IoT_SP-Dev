/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ItemsInsertedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashAcceptor.Events
{

    [DataContract]
    [Event(Name = "CashAcceptor.ItemsInsertedEvent")]
    public sealed class ItemsInsertedEvent : UnsolicitedEvent<ItemsInsertedEvent.PayloadData>
    {

        public ItemsInsertedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(PositionEnum? Position = null)
                : base()
            {
                this.Position = Position;
            }

            public enum PositionEnum
            {
                InLeft,
                InRight,
                InCenter,
                InTop,
                InBottom,
                InFront,
                InRear,
                OutLeft,
                OutRight,
                OutCenter,
                OutTop,
                OutBottom,
                OutFront,
                OutRear
            }

            /// <summary>
            /// Specifies the position where the items have been inserted. Following values are possible:
            /// 
            /// \"inLeft\": Items detected in the left input position.
            /// 
            /// \"inRight\": Items detected in the right input position.
            /// 
            /// \"inCenter\": Items detected in the center input position.
            /// 
            /// \"inTop\": Items detected in the top input position.
            /// 
            /// \"inBottom\": Items detected in the bottom input position.
            /// 
            /// \"inFront\": Items detected in the front input position.
            /// 
            /// \"inRear\": Items detected in the rear input position.
            /// 
            /// \"outLeft\": Items detected in the left output position.
            /// 
            /// \"outRight\": Items detected in the right output position.
            /// 
            /// \"outCenter\": Items detected in the center output position.
            /// 
            /// \"outTop\": Items detected in the top output position.
            /// 
            /// \"outBottom\": Items detected in the bottom output position.
            /// 
            /// \"outFront\": Items detected in the front output position.
            /// 
            /// \"outRear\": Items detected in the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; private set; }

        }

    }
}
