/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * ItemsTakenEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashDispenser.Events
{

    [DataContract]
    [Event(Name = "CashDispenser.ItemsTakenEvent")]
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
                Default,
                Left,
                Right,
                Center,
                Top,
                Bottom,
                Front,
                Rear
            }

            /// <summary>
            /// The output position from which the items have been removed. Following values are possible:
            /// 
            /// * ```default``` - The default configuration.
            /// * ```left``` - The left output position.
            /// * ```right``` - The right output position.
            /// * ```center``` - The center output position.
            /// * ```top``` - The top output position.
            /// * ```bottom``` - The bottom output position.
            /// * ```front``` - The front output position.
            /// * ```rear``` - The rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

        }

    }
}
