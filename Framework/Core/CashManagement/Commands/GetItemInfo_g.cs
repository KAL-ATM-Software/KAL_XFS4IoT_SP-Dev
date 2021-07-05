/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "CashManagement.GetItemInfo")]
    public sealed class GetItemInfoCommand : Command<GetItemInfoCommand.PayloadData>
    {
        public GetItemInfoCommand(int RequestId, GetItemInfoCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, LevelEnum? Level = null, int? Index = null, ItemInfoTypeClass ItemInfoType = null)
                : base(Timeout)
            {
                this.Level = Level;
                this.Index = Index;
                this.ItemInfoType = ItemInfoType;
            }

            public enum LevelEnum
            {
                Level1,
                Level2,
                Level3,
                Level4,
                LevelAll
            }

            /// <summary>
            /// Defines the requested note level. Following values are possible:
            /// 
            /// * ```level1``` - Information for level 1 notes. Only an image file can be retrieved for level 1 notes.
            /// * ```level2``` - Information for level 2 notes. On systems that do not classify notes as level 2 this value 
            /// cannot be used and invalidData will be returned.
            /// * ```level3``` - Information for level 3 notes. On systems that do not  classify notes as level 3 this value 
            /// cannot be used and invalidData will be returned.
            /// * ```level4``` - Information for level 4 notes.
            /// * ```levelAll``` - Information for all levels and all items is to be returned with the *itemsList* output 
            /// parameter.
            /// </summary>
            [DataMember(Name = "level")]
            public LevelEnum? Level { get; private set; }

            /// <summary>
            /// Specifies the index for the item information required. If no index is provided, all items of the specified 
            /// [level](#cashmanagement.getiteminfo.command.properties.level) will be returned. If *level* is set to 
            /// ```levelAll```, this property will be ignored.
            /// </summary>
            [DataMember(Name = "index")]
            public int? Index { get; private set; }

            [DataContract]
            public sealed class ItemInfoTypeClass
            {
                public ItemInfoTypeClass(bool? SerialNumber = null, bool? Signature = null, bool? ImageFile = null)
                {
                    this.SerialNumber = SerialNumber;
                    this.Signature = Signature;
                    this.ImageFile = ImageFile;
                }

                /// <summary>
                /// Serial number of the item.
                /// </summary>
                [DataMember(Name = "serialNumber")]
                public bool? SerialNumber { get; private set; }

                /// <summary>
                /// Signature of the item.
                /// </summary>
                [DataMember(Name = "signature")]
                public bool? Signature { get; private set; }

                /// <summary>
                /// Image file of the item.
                /// </summary>
                [DataMember(Name = "imageFile")]
                public bool? ImageFile { get; private set; }

            }

            /// <summary>
            /// Specifies the type of information required. If nothing is specified, 
            /// all available information will be returned.
            /// </summary>
            [DataMember(Name = "itemInfoType")]
            public ItemInfoTypeClass ItemInfoType { get; private set; }

        }
    }
}
