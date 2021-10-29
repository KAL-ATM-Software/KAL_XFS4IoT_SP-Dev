/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * InfoAvailableEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CashManagement.Events
{

    [DataContract]
    [Event(Name = "CashManagement.InfoAvailableEvent")]
    public sealed class InfoAvailableEvent : Event<InfoAvailableEvent.PayloadData>
    {

        public InfoAvailableEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(List<ItemInfoSummaryClass> ItemInfoSummary = null)
                : base()
            {
                this.ItemInfoSummary = ItemInfoSummary;
            }

            [DataContract]
            public sealed class ItemInfoSummaryClass
            {
                public ItemInfoSummaryClass(NoteLevelEnum? Level = null, int? NumOfItems = null)
                {
                    this.Level = Level;
                    this.NumOfItems = NumOfItems;
                }

                [DataMember(Name = "level")]
                public NoteLevelEnum? Level { get; init; }

                /// <summary>
                /// Number of items classified as _level_ which have information available.
                /// <example>2</example>
                /// </summary>
                [DataMember(Name = "numOfItems")]
                [DataTypes(Minimum = 1)]
                public int? NumOfItems { get; init; }

            }

            /// <summary>
            /// Array of itemInfoSummary objects, one object for every level.
            /// </summary>
            [DataMember(Name = "itemInfoSummary")]
            public List<ItemInfoSummaryClass> ItemInfoSummary { get; init; }

        }

    }
}
