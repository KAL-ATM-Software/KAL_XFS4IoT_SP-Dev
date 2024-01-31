/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetItemInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = GetItemInfo
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashManagement.GetItemInfo")]
    public sealed class GetItemInfoCommand : Command<GetItemInfoCommand.PayloadData>
    {
        public GetItemInfoCommand(int RequestId, GetItemInfoCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ItemsClass Items = null, ItemInfoTypeClass ItemInfoType = null)
                : base()
            {
                this.Items = Items;
                this.ItemInfoType = ItemInfoType;
            }

            [DataContract]
            public sealed class ItemsClass
            {
                public ItemsClass(NoteLevelEnum? Level = null, int? Index = null)
                {
                    this.Level = Level;
                    this.Index = Index;
                }

                [DataMember(Name = "level")]
                public NoteLevelEnum? Level { get; init; }

                /// <summary>
                /// Specifies the zero based index for the item information required. If null, all items of the
                /// specified *level* will be returned.
                /// <example>1</example>
                /// </summary>
                [DataMember(Name = "index")]
                [DataTypes(Minimum = 0)]
                public int? Index { get; init; }

            }

            /// <summary>
            /// Specifies which item or items to return information for. If null, all information on all items is
            /// returned.
            /// </summary>
            [DataMember(Name = "items")]
            public ItemsClass Items { get; init; }

            [DataContract]
            public sealed class ItemInfoTypeClass
            {
                public ItemInfoTypeClass(bool? SerialNumber = null, bool? Signature = null, bool? Image = null)
                {
                    this.SerialNumber = SerialNumber;
                    this.Signature = Signature;
                    this.Image = Image;
                }

                /// <summary>
                /// Request the serial number of the item.
                /// </summary>
                [DataMember(Name = "serialNumber")]
                public bool? SerialNumber { get; init; }

                /// <summary>
                /// Request the signature of the item.
                /// </summary>
                [DataMember(Name = "signature")]
                public bool? Signature { get; init; }

                /// <summary>
                /// Request the image of the item.
                /// </summary>
                [DataMember(Name = "image")]
                public bool? Image { get; init; }

            }

            /// <summary>
            /// Specifies the type of information required. If null, all available information will be returned.
            /// </summary>
            [DataMember(Name = "itemInfoType")]
            public ItemInfoTypeClass ItemInfoType { get; init; }

        }
    }
}
