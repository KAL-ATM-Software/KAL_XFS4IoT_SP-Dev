/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ItemsPresentedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashAcceptor.Events
{

    [DataContract]
    [Event(Name = "CashAcceptor.ItemsPresentedEvent")]
    public sealed class ItemsPresentedEvent : UnsolicitedEvent<ItemsPresentedEvent.PayloadData>
    {

        public ItemsPresentedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(PositionEnum? Position = null, AdditionalBunchesEnum? AdditionalBunches = null, int? BunchesRemaining = null)
                : base()
            {
                this.Position = Position;
                this.AdditionalBunches = AdditionalBunches;
                this.BunchesRemaining = BunchesRemaining;
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
            /// "inLeft": Items presented at the left input position.
            /// 
            /// "inRight": Items presented at the right input position.
            /// 
            /// "inCenter": Items presented at the center input position.
            /// 
            /// "inTop": Items presented at the top input position.
            /// 
            /// "inBottom": Items presented at the bottom input position.
            /// 
            /// "inFront": Items presented at the front input position.
            /// 
            /// "inRear": Items presented at the rear input position.
            /// 
            /// "outLeft": Items presented at the left output position.
            /// 
            /// "outRight": Items presented at the right output position.
            /// 
            /// "outCenter": Items presented at the center output position.
            /// 
            /// "outTop": Items presented at the top output position.
            /// 
            /// "outBottom": Items presented at the bottom output position.
            /// 
            /// "outFront": Items presented at the front output position.
            /// 
            /// "outRear": Items presented at the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

            public enum AdditionalBunchesEnum
            {
                None,
                OneMore,
                Unknown
            }

            /// <summary>
            /// Specifies whether or not additional bunches of items are remaining to be presented as a result of the current operation. Following values are possible:
            /// 
            /// "none": No additional bunches remain.
            /// 
            /// "oneMore": At least one additional bunch remains.
            /// 
            /// "unknown": It is unknown whether additional bunches remain.
            /// </summary>
            [DataMember(Name = "additionalBunches")]
            public AdditionalBunchesEnum? AdditionalBunches { get; init; }

            /// <summary>
            /// If *additionalBunches* is "oneMore", specifies the number of additional bunches of items remaining to be presented as a result of the current operation. 
            /// If the number of additional bunches is at least one, but the precise number is unknown, *bunchesRemaining* will be 255 (TODO: Check if there is a better way to represent this state). 
            /// For any other value of *additionalBunches*, *bunchesRemaining* will be zero.
            /// </summary>
            [DataMember(Name = "bunchesRemaining")]
            public int? BunchesRemaining { get; init; }

        }

    }
}
