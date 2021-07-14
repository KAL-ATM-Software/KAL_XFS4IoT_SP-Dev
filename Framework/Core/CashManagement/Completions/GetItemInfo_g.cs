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
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [Completion(Name = "CashManagement.GetItemInfo")]
    public sealed class GetItemInfoCompletion : Completion<GetItemInfoCompletion.PayloadData>
    {
        public GetItemInfoCompletion(int RequestId, GetItemInfoCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<ItemsListClass> ItemsList = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ItemsList = ItemsList;
            }

            [DataContract]
            public sealed class ItemsListClass
            {
                public ItemsListClass(string CurrencyID = null, double? Value = null, int? Release = null, int? NoteID = null, LevelEnum? Level = null, string SerialNumber = null, OrientationClass Orientation = null, string P6Signature = null, string ImageFile = null, OnClassificationListEnum? OnClassificationList = null, ItemLocationEnum? ItemLocation = null, string Cashunit = null, ItemDeviceLocationEnum? ItemDeviceLocation = null)
                {
                    this.CurrencyID = CurrencyID;
                    this.Value = Value;
                    this.Release = Release;
                    this.NoteID = NoteID;
                    this.Level = Level;
                    this.SerialNumber = SerialNumber;
                    this.Orientation = Orientation;
                    this.P6Signature = P6Signature;
                    this.ImageFile = ImageFile;
                    this.OnClassificationList = OnClassificationList;
                    this.ItemLocation = ItemLocation;
                    this.Cashunit = Cashunit;
                    this.ItemDeviceLocation = ItemDeviceLocation;
                }

                /// <summary>
                /// Currency ID in ISO 4217 format [Ref. 2]. This value will be omitted for level 1 items.
                /// </summary>
                [DataMember(Name = "currencyID")]
                public string CurrencyID { get; init; }

                /// <summary>
                /// The value of a single item expressed as floating point value. This value will be zero for level 1 items.
                /// </summary>
                [DataMember(Name = "value")]
                public double? Value { get; init; }

                /// <summary>
                /// The release of the banknote type. The higher this number is, the newer the release. 
                /// Zero means that there is only one release of that banknote type. This value has not been 
                /// standardized and therefore a release number of the same banknote will not necessarily have 
                /// the same value in different systems. This value will be zero for level 1 items.
                /// </summary>
                [DataMember(Name = "release")]
                public int? Release { get; init; }

                /// <summary>
                /// Identification of note type. This value will be zero for level 1 items.
                /// </summary>
                [DataMember(Name = "noteID")]
                public int? NoteID { get; init; }

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
                /// * ```level1``` - A level 1 banknote.
                /// * ```level2``` - A level 2 banknote.
                /// * ```level3``` - A level 3 banknote.
                /// * ```level4Fit``` - A fit level 4 banknote.
                /// * ```level4Unfit``` - An unfit level 4 banknote.
                /// </summary>
                [DataMember(Name = "level")]
                public LevelEnum? Level { get; init; }

                /// <summary>
                /// This field contains the serial number of the item as a string. A '?' character (0x003F) is used 
                /// to represent any serial number character that cannot be recognized. If no serial number is available or 
                /// has not been requested then *serialNumber* is omitted.
                /// </summary>
                [DataMember(Name = "serialNumber")]
                public string SerialNumber { get; init; }

                [DataContract]
                public sealed class OrientationClass
                {
                    public OrientationClass(bool? FrontTop = null, bool? FrontBottom = null, bool? BackTop = null, bool? BackBottom = null, bool? Unknown = null, bool? NotSupported = null)
                    {
                        this.FrontTop = FrontTop;
                        this.FrontBottom = FrontBottom;
                        this.BackTop = BackTop;
                        this.BackBottom = BackBottom;
                        this.Unknown = Unknown;
                        this.NotSupported = NotSupported;
                    }

                    /// <summary>
                    /// If note is inserted wide side as the leading edge, the note was inserted with the front image 
                    /// facing up and the top edge of the note was inserted first. If the note is inserted short side 
                    /// as the leading edge, the note was inserted with the front image face up and the left edge was inserted first.
                    /// </summary>
                    [DataMember(Name = "frontTop")]
                    public bool? FrontTop { get; init; }

                    /// <summary>
                    /// If note is inserted wide side as the leading edge, the note was inserted with the front image 
                    /// facing up and the bottom edge of the note was inserted first. If the note is inserted short side 
                    /// as the leading edge, the note was inserted with the front image face up and the right edge was inserted first.
                    /// </summary>
                    [DataMember(Name = "frontBottom")]
                    public bool? FrontBottom { get; init; }

                    /// <summary>
                    /// If note is inserted wide side as the leading edge, the note was inserted with the back image facing up and 
                    /// the top edge of the note was inserted first. If the note is inserted short side as the leading edge, the note 
                    /// was inserted with the back image face up and the left edge was inserted first.
                    /// </summary>
                    [DataMember(Name = "backTop")]
                    public bool? BackTop { get; init; }

                    /// <summary>
                    /// If note is inserted wide side as the leading edge, the note was inserted with the back image facing up and the 
                    /// bottom edge of the note was inserted first. If the note is inserted short side as the leading edge, the note was 
                    /// inserted with the back image face up and the right edge was inserted first.
                    /// </summary>
                    [DataMember(Name = "backBottom")]
                    public bool? BackBottom { get; init; }

                    /// <summary>
                    /// The orientation for the inserted note can not be determined.
                    /// </summary>
                    [DataMember(Name = "unknown")]
                    public bool? Unknown { get; init; }

                    /// <summary>
                    /// The hardware is not capable to determine the orientation.
                    /// </summary>
                    [DataMember(Name = "notSupported")]
                    public bool? NotSupported { get; init; }

                }

                /// <summary>
                /// Orientation of the entered banknote.
                /// </summary>
                [DataMember(Name = "orientation")]
                public OrientationClass Orientation { get; init; }

                /// <summary>
                /// Base64 encoded binary file containing only the vendor specific P6 signature data. 
                /// If no P6 signature is available then this field is omitted.
                /// </summary>
                [DataMember(Name = "p6Signature")]
                public string P6Signature { get; init; }

                /// <summary>
                /// Base64 encoded binary image data. If the Service does not support this function or the image file has 
                /// not been requested then imageFile is omitted.
                /// </summary>
                [DataMember(Name = "imageFile")]
                public string ImageFile { get; init; }

                public enum OnClassificationListEnum
                {
                    OnClassificationList,
                    NotOnClassificationList,
                    ClassificationListUnknown
                }

                /// <summary>
                /// Specifies if the serial number reported in the *serialNumber* field is on the classification list. 
                /// If the classification list reporting capability is not supported this field will be omitted.
                /// Following values are possible:
                /// 
                /// * ```onClassificationList``` - The serial number of the items is on the classification list.
                /// * ```notOnClassificationList``` - The serial number of the items is not on the classification list.
                /// * ```classificationListUnknown``` - It is unknown if the serial number of the item is on the classification list.
                /// </summary>
                [DataMember(Name = "onClassificationList")]
                public OnClassificationListEnum? OnClassificationList { get; init; }

                public enum ItemLocationEnum
                {
                    Device,
                    CashUnit,
                    Customer,
                    Unknown
                }

                /// <summary>
                /// Specifies the location of the item. Following values are possible:
                /// 
                /// * ```device``` - The item is inside the device in some position other than a cash unit.
                /// * ```cashUnit``` - The item is in a cash unit. The cash unit is defined by 
                /// [cashunit](#cashmanagement.getiteminfo.completion.properties.itemslist.cashunit).
                /// * ```customer``` - The item has been dispensed to the customer.
                /// * ```unknown``` - The item location is unknown.
                /// </summary>
                [DataMember(Name = "itemLocation")]
                public ItemLocationEnum? ItemLocation { get; init; }

                /// <summary>
                /// If [itemLocation](#cashmanagement.getiteminfo.completion.properties.itemslist.itemlocation) is 
                /// ```cashUnit``` this parameter specifies the object name of the cash unit 
                /// which received the item as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
                /// command. 
                /// If *itemLocation* is not ```cashUnit``` *cashunit* will be omitted.
                /// </summary>
                [DataMember(Name = "cashunit")]
                public string Cashunit { get; init; }

                public enum ItemDeviceLocationEnum
                {
                    Stacker,
                    Output,
                    Transport,
                    Unknown
                }

                /// <summary>
                /// If [itemLocation](#cashmanagement.getiteminfo.completion.properties.itemslist.itemlocation) is 
                /// ```device``` this parameter specifies where the item is in the device. 
                /// If *itemLocation* is not ```device``` then *itemDeviceLocation* will be omitted.
                /// Following values are possible:
                /// 
                /// * ```stacker``` - The item is in the intermediate stacker.
                /// * ```output``` - The item is at the output position. The items have not been in customer access.
                /// * ```transport``` - The item is at another location in the device.
                /// * ```unknown``` - The item is in the device but its location is unknown.
                /// </summary>
                [DataMember(Name = "itemDeviceLocation")]
                public ItemDeviceLocationEnum? ItemDeviceLocation { get; init; }

            }

            /// <summary>
            /// Array of "item info" objects.
            /// </summary>
            [DataMember(Name = "itemsList")]
            public List<ItemsListClass> ItemsList { get; init; }

        }
    }
}
