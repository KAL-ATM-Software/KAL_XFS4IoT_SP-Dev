/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Event(Name = "CashManagement.ItemsTakenEvent")]
    public sealed class ItemsTakenEvent : UnsolicitedEvent<ItemsTakenEvent.PayloadData>
    {

        public ItemsTakenEvent()
            : base()
        { }

        public ItemsTakenEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(PositionEnum? Position = null, string AdditionalBunches = null)
                : base()
            {
                this.Position = Position;
                this.AdditionalBunches = AdditionalBunches;
            }

            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

            /// <summary>
            /// Specifies how many more bunches will be required to present the request. Following values are possible:
            /// 
            ///   * ```[number]``` - The number of additional bunches to be presented.
            ///   * ```unknown``` - More than one additional bunch is required but the precise number is unknown.
            /// <example>1</example>
            /// </summary>
            [DataMember(Name = "additionalBunches")]
            [DataTypes(Pattern = @"^unknown$|^[0-9]*$")]
            public string AdditionalBunches { get; init; }

        }

    }
}
