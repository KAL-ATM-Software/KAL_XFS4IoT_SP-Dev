/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ItemsTakenEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashAcceptor.Events
{

    [DataContract]
    [Event(Name = "CashAcceptor.ItemsTakenEvent")]
    public sealed class ItemsTakenEvent : UnsolicitedEvent<ItemsTakenEvent.PayloadData>
    {

        public ItemsTakenEvent(PayloadData Payload)
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
            /// \"inLeft\": Items taken from the left input position.
            /// 
            /// \"inRight\": Items taken from the right input position.
            /// 
            /// \"inCenter\": Items taken from the center input position.
            /// 
            /// \"inTop\": Items taken from the top input position.
            /// 
            /// \"inBottom\": Items taken from the bottom input position.
            /// 
            /// \"inFront\": Items taken from the front input position.
            /// 
            /// \"inRear\": Items taken from the rear input position.
            /// 
            /// \"outLeft\": Items taken from the left output position.
            /// 
            /// \"outRight\": Items taken from the right output position.
            /// 
            /// \"outCenter\": Items taken from the center output position.
            /// 
            /// \"outTop\": Items taken from the top output position.
            /// 
            /// \"outBottom\": Items taken from the bottom output position.
            /// 
            /// \"outFront\": Items taken from the front output position.
            /// 
            /// \"outRear\": Items taken from the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; private set; }

        }

    }
}
