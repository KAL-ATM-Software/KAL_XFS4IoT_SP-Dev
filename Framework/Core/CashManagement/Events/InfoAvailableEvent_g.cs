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
                public ItemInfoSummaryClass(LevelEnum? Level = null, int? NumOfItems = null)
                {
                    this.Level = Level;
                    this.NumOfItems = NumOfItems;
                }

                public enum LevelEnum
                {
                    Level1,
                    Level2,
                    Level3,
                    Level4Fit,
                    Level4Unfit
                }

                /// <summary>
                /// Defines the note level. Following values are possible:
                /// 
                ///  * ```level1 ``` - Information for level 1 notes.
                ///  * ```level2 ``` - Information for level 2 notes.
                ///  * ```level3 ``` - Information for level 3 notes.
                ///  * ```level4 ``` - Information for level 4 notes.
                /// </summary>
                [DataMember(Name = "level")]
                public LevelEnum? Level { get; private set; }

                /// <summary>
                /// Number of items classified as *level* which have information available.
                /// </summary>
                [DataMember(Name = "numOfItems")]
                public int? NumOfItems { get; private set; }

            }

            /// <summary>
            /// Array of itemInfoSummary objects, one object for every level.
            /// </summary>
            [DataMember(Name = "itemInfoSummary")]
            public List<ItemInfoSummaryClass> ItemInfoSummary { get; private set; }

        }

    }
}
