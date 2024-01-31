/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CashManagementSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.CashManagement
{

    public enum PositionEnum
    {
        InDefault,
        InLeft,
        InRight,
        InCenter,
        InTop,
        InBottom,
        InFront,
        InRear,
        OutDefault,
        OutLeft,
        OutRight,
        OutCenter,
        OutTop,
        OutBottom,
        OutFront,
        OutRear
    }


    public enum OutputPositionEnum
    {
        OutDefault,
        OutLeft,
        OutRight,
        OutCenter,
        OutTop,
        OutBottom,
        OutFront,
        OutRear
    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(DispenserEnum? Dispenser = null, AcceptorEnum? Acceptor = null)
        {
            this.Dispenser = Dispenser;
            this.Acceptor = Acceptor;
        }

        public enum DispenserEnum
        {
            Ok,
            Attention,
            Stop,
            Unknown
        }

        /// <summary>
        /// Supplies the state of the storage units for dispensing cash. This may be null in
        /// [Common.Status](#common.status) if the device is not capable of dispensing cash, otherwise the
        /// following values are possible:
        /// 
        /// * ```ok``` - All storage units present are in a good state.
        /// * ```attention``` - One or more of the storage units is in a low, empty, inoperative or manipulated condition.
        /// Items can still be dispensed from at least one of the storage units.
        /// * ```stop``` - Due to a storage unit failure dispensing is impossible. No items can be dispensed because
        /// all of the storage units are empty, missing, inoperative or in a manipulated condition. This state may also occur
        /// when a reject/retract storage unit is full or no reject/retract storage unit is present, or when an application
        /// lock is set on every storage unit which can be locked.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the storage units cannot be determined.
        /// </summary>
        [DataMember(Name = "dispenser")]
        public DispenserEnum? Dispenser { get; init; }

        public enum AcceptorEnum
        {
            Ok,
            Attention,
            Stop,
            Unknown
        }

        /// <summary>
        /// Supplies the state of the storage units for accepting cash. This may be null in
        /// [Common.Status](#common.status) if the device is not capable of accepting cash, otherwise the
        /// following values are possible:
        /// 
        /// * ```ok``` - All storage units present are in a good state.
        /// * ```attention``` - One or more of the storage units is in a high, full, inoperative or manipulated condition.
        /// Items can still be accepted into at least one of the storage units.
        /// * ```stop``` - Due to a storage unit failure accepting is impossible. No items can be accepted because
        /// all of the storage units are in a full, inoperative or manipulated condition. This state may also occur when
        /// a retract storage unit is full or no retract storage unit is present, or when an application lock is
        /// set on every storage unit, or when items are to be automatically retained within
        /// storage units (see [retainAction](#common.capabilities.completion.description.cashacceptor.retainaction)),
        /// but all of the designated storage units for storing them are full or inoperative.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the storage units cannot be
        /// determined.
        /// </summary>
        [DataMember(Name = "acceptor")]
        public AcceptorEnum? Acceptor { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(bool? CashBox = null, ExchangeTypeClass ExchangeType = null, ItemInfoTypesClass ItemInfoTypes = null, bool? ClassificationList = null, ClassificationsClass Classifications = null)
        {
            this.CashBox = CashBox;
            this.ExchangeType = ExchangeType;
            this.ItemInfoTypes = ItemInfoTypes;
            this.ClassificationList = ClassificationList;
            this.Classifications = Classifications;
        }

        /// <summary>
        /// This property is only applicable to teller type devices.
        /// It specifies whether or not tellers have been assigned a cash box.
        /// </summary>
        [DataMember(Name = "cashBox")]
        public bool? CashBox { get; init; }

        [DataContract]
        public sealed class ExchangeTypeClass
        {
            public ExchangeTypeClass(bool? ByHand = null)
            {
                this.ByHand = ByHand;
            }

            /// <summary>
            /// The device supports manual replenishment either by filling the storage unit by hand or by replacing the storage unit.
            /// </summary>
            [DataMember(Name = "byHand")]
            public bool? ByHand { get; init; }

        }

        /// <summary>
        /// Specifies the type of storage unit exchange operations supported by the device.
        /// </summary>
        [DataMember(Name = "exchangeType")]
        public ExchangeTypeClass ExchangeType { get; init; }

        [DataContract]
        public sealed class ItemInfoTypesClass
        {
            public ItemInfoTypesClass(bool? SerialNumber = null, bool? Signature = null, bool? Image = null)
            {
                this.SerialNumber = SerialNumber;
                this.Signature = Signature;
                this.Image = Image;
            }

            /// <summary>
            /// Serial Number of the item.
            /// </summary>
            [DataMember(Name = "serialNumber")]
            public bool? SerialNumber { get; init; }

            /// <summary>
            /// Signature of the item.
            /// </summary>
            [DataMember(Name = "signature")]
            public bool? Signature { get; init; }

            /// <summary>
            /// Image of the item.
            /// </summary>
            [DataMember(Name = "image")]
            public bool? Image { get; init; }

        }

        /// <summary>
        /// Specifies the types of information that can be retrieved through the
        /// [CashManagement.GetItemInfo](#cashmanagement.getiteminfo) command. This property is null if not supported.
        /// </summary>
        [DataMember(Name = "itemInfoTypes")]
        public ItemInfoTypesClass ItemInfoTypes { get; init; }

        /// <summary>
        /// Specifies whether the Service has the capability to maintain a classification list of serial numbers as well
        /// as supporting the associated operations.
        /// </summary>
        [DataMember(Name = "classificationList")]
        public bool? ClassificationList { get; init; }

        [DataContract]
        public sealed class ClassificationsClass
        {
            public ClassificationsClass(bool? Unrecognized = null, bool? Counterfeit = null, bool? Suspect = null, bool? Inked = null, bool? Fit = null, bool? Unfit = null)
            {
                this.Unrecognized = Unrecognized;
                this.Counterfeit = Counterfeit;
                this.Suspect = Suspect;
                this.Inked = Inked;
                this.Fit = Fit;
                this.Unfit = Unfit;
            }

            /// <summary>
            /// Items can be classified as unrecognized.
            /// </summary>
            [DataMember(Name = "unrecognized")]
            public bool? Unrecognized { get; init; }

            /// <summary>
            /// Items can be recognized as counterfeit.
            /// </summary>
            [DataMember(Name = "counterfeit")]
            public bool? Counterfeit { get; init; }

            /// <summary>
            /// Items can be suspected as counterfeit.
            /// </summary>
            [DataMember(Name = "suspect")]
            public bool? Suspect { get; init; }

            /// <summary>
            /// Ink-stained items are recognized.
            /// </summary>
            [DataMember(Name = "inked")]
            public bool? Inked { get; init; }

            /// <summary>
            /// Genuine items fit for recycling are recognized.
            /// </summary>
            [DataMember(Name = "fit")]
            public bool? Fit { get; init; }

            /// <summary>
            /// Genuine items not fit for recycling are recognized.
            /// </summary>
            [DataMember(Name = "unfit")]
            public bool? Unfit { get; init; }

        }

        /// <summary>
        /// Specifies the classifications supported - see [Note Classification](#cashmanagement.generalinformation.noteclassification).
        /// </summary>
        [DataMember(Name = "classifications")]
        public ClassificationsClass Classifications { get; init; }

    }


    [DataContract]
    public sealed class CashItemClass
    {
        public CashItemClass(int? NoteID = null, string Currency = null, double? Value = null, int? Release = null)
        {
            this.NoteID = NoteID;
            this.Currency = Currency;
            this.Value = Value;
            this.Release = Release;
        }

        /// <summary>
        /// Assigned by the Service. A unique number identifying a single cash item.
        /// Each unique combination of the other properties will have a different noteID.
        /// Can be used for migration of *usNoteID* from XFS 3.x.
        /// <example>25</example>
        /// </summary>
        [DataMember(Name = "noteID")]
        [DataTypes(Minimum = 1)]
        public int? NoteID { get; init; }

        /// <summary>
        /// ISO 4217 currency identifier [[Ref. cashmanagement-1](#ref-cashmanagement-1)].
        /// <example>USD</example>
        /// </summary>
        [DataMember(Name = "currency")]
        [DataTypes(Pattern = @"^[A-Z]{3}$")]
        public string Currency { get; init; }

        /// <summary>
        /// Absolute value of a cash item or items. May be a floating point value to allow for coins and notes which have
        /// a value which is not a whole multiple of the currency unit.
        /// 
        /// If applied to a storage unit, this applies to all contents, may be 0 if mixed and may only be modified in
        /// an exchange state if applicable.
        /// <example>20</example>
        /// </summary>
        [DataMember(Name = "value")]
        public double? Value { get; init; }

        /// <summary>
        /// The release of the cash item. The higher this number is, the newer the release.
        /// 
        /// If 0 or not reported, there is only one release of that cash item or the device is not
        /// capable of distinguishing different release of the item, for example in a simple cash dispenser.
        /// 
        /// An example of how this can be used is being able to sort different releases of the same denomination
        /// note to different storage units to take older notes out of circulation.
        /// 
        /// This value is device, banknote reader and currency description configuration data dependent, therefore a release number of the
        /// same cash item will not necessarily have the same value in different systems and any such usage
        /// would be specific to a specific device's configuration.
        /// <example>1</example>
        /// </summary>
        [DataMember(Name = "release")]
        [DataTypes(Minimum = 0)]
        public int? Release { get; init; }

    }


    [DataContract]
    public sealed class BankNoteClass
    {
        public BankNoteClass(CashItemClass CashItem = null, bool? Enabled = null)
        {
            this.CashItem = CashItem;
            this.Enabled = Enabled;
        }

        [DataMember(Name = "cashItem")]
        public CashItemClass CashItem { get; init; }

        /// <summary>
        /// If true the banknote reader will accept this note type during a cash-in operations.
        /// If false the banknote reader will refuse this note type unless it must be retained by note classification
        /// rules.
        /// </summary>
        [DataMember(Name = "enabled")]
        public bool? Enabled { get; init; }

    }


    public enum InputPositionEnum
    {
        InDefault,
        InLeft,
        InRight,
        InCenter,
        InTop,
        InBottom,
        InFront,
        InRear
    }


    [DataContract]
    public sealed class TellerTotalsClass
    {
        public TellerTotalsClass(double? ItemsReceived = null, double? ItemsDispensed = null, double? CoinsReceived = null, double? CoinsDispensed = null, double? CashBoxReceived = null, double? CashBoxDispensed = null)
        {
            this.ItemsReceived = ItemsReceived;
            this.ItemsDispensed = ItemsDispensed;
            this.CoinsReceived = CoinsReceived;
            this.CoinsDispensed = CoinsDispensed;
            this.CashBoxReceived = CashBoxReceived;
            this.CashBoxDispensed = CashBoxDispensed;
        }

        /// <summary>
        /// The total absolute value of items (other than coins) of the specified currency accepted.
        /// The amount is expressed as a floating point value.
        /// <example>1405.00</example>
        /// </summary>
        [DataMember(Name = "itemsReceived")]
        public double? ItemsReceived { get; init; }

        /// <summary>
        /// The total absolute value of items (other than coins) of the specified currency dispensed.
        /// The amount is expressed as a floating point value.
        /// <example>1405.00</example>
        /// </summary>
        [DataMember(Name = "itemsDispensed")]
        public double? ItemsDispensed { get; init; }

        /// <summary>
        /// The total absolute value of coin currency accepted.
        /// The amount is expressed as a floating point value.
        /// <example>0.05</example>
        /// </summary>
        [DataMember(Name = "coinsReceived")]
        public double? CoinsReceived { get; init; }

        /// <summary>
        /// The total absolute value of coin currency dispensed.
        /// The amount is expressed as a floating point value.
        /// <example>0.05</example>
        /// </summary>
        [DataMember(Name = "coinsDispensed")]
        public double? CoinsDispensed { get; init; }

        /// <summary>
        /// The total absolute value of cash box currency accepted.
        /// The amount is expressed as a floating point value.
        /// <example>1407.15</example>
        /// </summary>
        [DataMember(Name = "cashBoxReceived")]
        public double? CashBoxReceived { get; init; }

        /// <summary>
        /// The total absolute value of cash box currency dispensed.
        /// The amount is expressed as a floating point value.
        /// <example>1407.15</example>
        /// </summary>
        [DataMember(Name = "cashBoxDispensed")]
        public double? CashBoxDispensed { get; init; }

    }


    [DataContract]
    public sealed class TellerDetailsClass
    {
        public TellerDetailsClass(int? TellerID = null, InputPositionEnum? InputPosition = null, OutputPositionEnum? OutputPosition = null, Dictionary<string, TellerTotalsClass> TellerTotals = null)
        {
            this.TellerID = TellerID;
            this.InputPosition = InputPosition;
            this.OutputPosition = OutputPosition;
            this.TellerTotals = TellerTotals;
        }

        /// <summary>
        /// Identification of the teller.
        /// <example>104</example>
        /// </summary>
        [DataMember(Name = "tellerID")]
        [DataTypes(Minimum = 0)]
        public int? TellerID { get; init; }

        [DataMember(Name = "inputPosition")]
        public InputPositionEnum? InputPosition { get; init; }

        [DataMember(Name = "outputPosition")]
        public OutputPositionEnum? OutputPosition { get; init; }

        /// <summary>
        /// List of teller total objects. There is one object per currency.
        /// </summary>
        [DataMember(Name = "tellerTotals")]
        public Dictionary<string, TellerTotalsClass> TellerTotals { get; init; }

    }


    public enum NoteLevelEnum
    {
        Unrecognized,
        Counterfeit,
        Suspect,
        Fit,
        Unfit,
        Inked
    }


    public enum OrientationEnum
    {
        FrontTop,
        FrontBottom,
        BackTop,
        BackBottom,
        Unknown
    }


    public enum OnClassificationListEnum
    {
        OnClassificationList,
        NotOnClassificationList,
        ClassificationListUnknown
    }


    [DataContract]
    public sealed class ItemInfoClass
    {
        public ItemInfoClass(string NoteType = null, OrientationEnum? Orientation = null, List<byte> Signature = null, NoteLevelEnum? Level = null, string SerialNumber = null, List<byte> Image = null, OnClassificationListEnum? OnClassificationList = null, string ItemLocation = null)
        {
            this.NoteType = NoteType;
            this.Orientation = Orientation;
            this.Signature = Signature;
            this.Level = Level;
            this.SerialNumber = SerialNumber;
            this.Image = Image;
            this.OnClassificationList = OnClassificationList;
            this.ItemLocation = ItemLocation;
        }

        /// <summary>
        /// A cash item as reported by [CashManagement.GetBankNoteTypes](#cashmanagement.getbanknotetypes). This is null
        /// if the item was not identified as a cash item.
        /// <example>type20USD1</example>
        /// </summary>
        [DataMember(Name = "noteType")]
        [DataTypes(Pattern = @"^type[0-9A-Z]+$")]
        public string NoteType { get; init; }

        [DataMember(Name = "orientation")]
        public OrientationEnum? Orientation { get; init; }

        /// <summary>
        /// Base64 encoded vendor specific signature data. If no signature is available or has not been requested then this is null.
        /// <example>MAA5ADgANwA2ADUANAAz ...</example>
        /// </summary>
        [DataMember(Name = "signature")]
        [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
        public List<byte> Signature { get; init; }

        [DataMember(Name = "level")]
        public NoteLevelEnum? Level { get; init; }

        /// <summary>
        /// This property contains the serial number of the item as a string. A '?' character is used
        /// to represent any serial number character that cannot be recognized. If no serial number is available or
        /// has not been requested then this is null.
        /// <example>AB12345YG</example>
        /// </summary>
        [DataMember(Name = "serialNumber")]
        public string SerialNumber { get; init; }

        /// <summary>
        /// Base64 encoded binary image data. If the Service does not support this function or the image has not
        /// been requested then this is null.
        /// <example>MAA5ADgANwA2ADUANAAz ...</example>
        /// </summary>
        [DataMember(Name = "image")]
        [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
        public List<byte> Image { get; init; }

        [DataMember(Name = "onClassificationList")]
        public OnClassificationListEnum? OnClassificationList { get; init; }

        /// <summary>
        /// Specifies the location of the item. Following values are possible:
        /// 
        /// * ```customer``` - The item has been presented to the customer.
        /// * ```unknown``` - The item location is unknown, for example, it may have been removed manually.
        /// * ```stacker``` - The item is in the intermediate stacker.
        /// * ```output``` - The item is at the output position. The items have not been in customer access.
        /// * ```transport``` - The item is in an intermediate location in the device.
        /// * ```deviceUnknown``` - The item is in the device but its location is unknown.
        /// * ```&lt;storage unit identifier&gt;``` - The item is in a storage unit with matching
        /// [identifier](#storage.getstorage.completion.properties.storage.unit1).
        /// <example>unit1</example>
        /// </summary>
        [DataMember(Name = "itemLocation")]
        [DataTypes(Pattern = @"^customer$|^unknown$|^stacker$|^output$|^transport$|^deviceUnknown$|^unit[0-9A-Za-z]+$")]
        public string ItemLocation { get; init; }

    }


    [DataContract]
    public sealed class ClassificationElementClass
    {
        public ClassificationElementClass(string SerialNumber = null, string Currency = null, double? Value = null, NoteLevelEnum? Level = null)
        {
            this.SerialNumber = SerialNumber;
            this.Currency = Currency;
            this.Value = Value;
            this.Level = Level;
        }

        /// <summary>
        /// This string defines the serial number or a mask of serial numbers of one element with the
        /// defined currency and value. For a definition of the mask see [Note Classification](#cashmanagement.generalinformation.noteclassification).
        /// <example>AB1234D</example>
        /// </summary>
        [DataMember(Name = "serialNumber")]
        public string SerialNumber { get; init; }

        /// <summary>
        /// ISO 4217 currency identifier [[Ref. cashmanagement-1](#ref-cashmanagement-1)].
        /// <example>USD</example>
        /// </summary>
        [DataMember(Name = "currency")]
        [DataTypes(Pattern = @"^[A-Z]{3}$")]
        public string Currency { get; init; }

        /// <summary>
        /// Absolute value of a cash item or items. May be a floating point value to allow for coins and notes which have
        /// a value which is not a whole multiple of the currency unit.
        /// 
        /// If applied to a storage unit, this applies to all contents, may be 0 if mixed and may only be modified in
        /// an exchange state if applicable.
        /// <example>20</example>
        /// </summary>
        [DataMember(Name = "value")]
        public double? Value { get; init; }

        [DataMember(Name = "level")]
        public NoteLevelEnum? Level { get; init; }

    }


    public enum RetractAreaEnum
    {
        Retract,
        Transport,
        Stacker,
        Reject,
        ItemCassette,
        CashIn
    }


    [DataContract]
    public sealed class RetractClass
    {
        public RetractClass(OutputPositionEnum? OutputPosition = null, RetractAreaEnum? RetractArea = null, int? Index = null)
        {
            this.OutputPosition = OutputPosition;
            this.RetractArea = RetractArea;
            this.Index = Index;
        }

        [DataMember(Name = "outputPosition")]
        public OutputPositionEnum? OutputPosition { get; init; }

        [DataMember(Name = "retractArea")]
        public RetractAreaEnum? RetractArea { get; init; }

        /// <summary>
        /// If *retractArea* is set to *retract* this property defines the position inside the retract storage units into
        /// which the cash is to be retracted. *index* starts with a value of 1 for the first retract position
        /// and increments by one for each subsequent position. If there are several retract storage units
        /// (of type *retractCassette* in [Storage.GetStorage](#storage.getstorage)), *index* would be incremented from the
        /// first position of the first retract storage unit to the last position of the last retract storage unit.
        /// The maximum value of *index* is the sum of *maximum* of each retract storage unit. If *retractArea* is not
        /// set to *retract* the value of this property is ignored and may be null.
        /// </summary>
        [DataMember(Name = "index")]
        [DataTypes(Minimum = 1)]
        public int? Index { get; init; }

    }


    [DataContract]
    public sealed class StorageCashCountClass
    {
        public StorageCashCountClass(int? Fit = null, int? Unfit = null, int? Suspect = null, int? Counterfeit = null, int? Inked = null)
        {
            this.Fit = Fit;
            this.Unfit = Unfit;
            this.Suspect = Suspect;
            this.Counterfeit = Counterfeit;
            this.Inked = Inked;
        }

        /// <summary>
        /// Count of genuine cash items which are fit for recycling. May be null in command data and events if
        /// not changed or not to be changed.
        /// <example>15</example>
        /// </summary>
        [DataMember(Name = "fit")]
        [DataTypes(Minimum = 0)]
        public int? Fit { get; init; }

        /// <summary>
        /// Count of genuine cash items which are unfit for recycling. May be null in command data and events if
        /// not changed or not to be changed.
        /// </summary>
        [DataMember(Name = "unfit")]
        [DataTypes(Minimum = 0)]
        public int? Unfit { get; init; }

        /// <summary>
        /// Count of suspected counterfeit cash items. May be null in command data and events if
        /// not changed or not to be changed.
        /// </summary>
        [DataMember(Name = "suspect")]
        [DataTypes(Minimum = 0)]
        public int? Suspect { get; init; }

        /// <summary>
        /// Count of counterfeit cash items. May be null in command data and events if
        /// not changed or not to be changed.
        /// </summary>
        [DataMember(Name = "counterfeit")]
        [DataTypes(Minimum = 0)]
        public int? Counterfeit { get; init; }

        /// <summary>
        /// Count of cash items which have been identified as ink stained. May be null in command data and events if
        /// not changed or not to be changed.
        /// </summary>
        [DataMember(Name = "inked")]
        [DataTypes(Minimum = 0)]
        public int? Inked { get; init; }

    }


    [DataContract]
    public sealed class StorageCashCountsClass
    {
        public StorageCashCountsClass(int? Unrecognized = null)
        {
            this.Unrecognized = Unrecognized;
        }

        /// <summary>
        /// Count of unrecognized items handled by the cash interface. May be null in command data and events if
        /// not changed or not to be changed.
        /// <example>5</example>
        /// </summary>
        [DataMember(Name = "unrecognized")]
        [DataTypes(Minimum = 0)]
        public int? Unrecognized { get; init; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public Dictionary<string, System.Text.Json.JsonElement> ExtensionData { get; set; } = new();

        [System.Text.Json.Serialization.JsonIgnore]
        public Dictionary<string, StorageCashCountClass> ExtendedProperties
        {
            get => MessageBase.ParseExtendedProperties<StorageCashCountClass>(ExtensionData);
            set => ExtensionData = MessageBase.CreateExtensionData<StorageCashCountClass>(value);
        }

    }


    [DataContract]
    public sealed class StorageCashInClass
    {
        public StorageCashInClass(int? RetractOperations = null, StorageCashCountsClass Deposited = null, StorageCashCountsClass Retracted = null, StorageCashCountsClass Rejected = null, StorageCashCountsClass Distributed = null, StorageCashCountsClass Transport = null)
        {
            this.RetractOperations = RetractOperations;
            this.Deposited = Deposited;
            this.Retracted = Retracted;
            this.Rejected = Rejected;
            this.Distributed = Distributed;
            this.Transport = Transport;
        }

        /// <summary>
        /// Number of cash retract operations which resulted in items entering this storage unit. This can be
        /// used where devices do not have the capability to count or validate items after presentation. May be null
        /// in command data and events if not changing.
        /// <example>15</example>
        /// </summary>
        [DataMember(Name = "retractOperations")]
        [DataTypes(Minimum = 0)]
        public int? RetractOperations { get; init; }

        /// <summary>
        /// The items deposited in the storage unit during a Cash In transaction. Can be null, if all values are 0.
        /// </summary>
        [DataMember(Name = "deposited")]
        public StorageCashCountsClass Deposited { get; init; }

        /// <summary>
        /// The items retracted into the storage unit after being accessible to a customer. This may be inaccurate
        /// or not counted if items are not counted or re-validated after presentation, the number of retract
        /// operations is also reported separately in *retractOperations*. Can be null, if all values are 0.
        /// </summary>
        [DataMember(Name = "retracted")]
        public StorageCashCountsClass Retracted { get; init; }

        /// <summary>
        /// The items deposited in this storage unit originating from another storage unit but rejected due to being
        /// invalid. This count may be inaccurate due to the nature of rejected items. Can be null, if all values are 0.
        /// </summary>
        [DataMember(Name = "rejected")]
        public StorageCashCountsClass Rejected { get; init; }

        /// <summary>
        /// The items deposited in this storage unit originating from another storage unit but not rejected. Can be null, if all values are 0.
        /// </summary>
        [DataMember(Name = "distributed")]
        public StorageCashCountsClass Distributed { get; init; }

        /// <summary>
        /// The items which were intended to be deposited in this storage unit but are not yet deposited. Typical use
        /// case for this property is tracking items after a jam during
        /// [CashAcceptor.CashInEnd](#cashacceptor.cashinend). This is not reset if
        /// [initial](#storage.getstorage.completion.properties.storage.unit1.cash.status.initial) is set for this unit
        /// by [Storage.GetStorage](#storage.getstorage). Can be null, if all values are 0.
        /// </summary>
        [DataMember(Name = "transport")]
        public StorageCashCountsClass Transport { get; init; }

    }


    [DataContract]
    public sealed class StorageCashTypesClass
    {
        public StorageCashTypesClass(bool? CashIn = null, bool? CashOut = null, bool? Replenishment = null, bool? CashInRetract = null, bool? CashOutRetract = null, bool? Reject = null)
        {
            this.CashIn = CashIn;
            this.CashOut = CashOut;
            this.Replenishment = Replenishment;
            this.CashInRetract = CashInRetract;
            this.CashOutRetract = CashOutRetract;
            this.Reject = Reject;
        }

        /// <summary>
        /// The unit can accept cash items. If *cashOut* is also true then the unit can recycle. May be null in command
        /// data or events if not changed or being changed.
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "cashIn")]
        public bool? CashIn { get; init; }

        /// <summary>
        /// The unit can dispense cash items. If *cashIn* is also true then the unit can recycle. May be null in command
        /// data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "cashOut")]
        public bool? CashOut { get; init; }

        /// <summary>
        /// Replenishment container. A storage unit can be refilled from or emptied to a replenishment container.
        /// May be null in command data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "replenishment")]
        public bool? Replenishment { get; init; }

        /// <summary>
        /// Retract unit. Items can be retracted into this unit during Cash In operations. May be null in command data
        /// or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "cashInRetract")]
        public bool? CashInRetract { get; init; }

        /// <summary>
        /// Retract unit. Items can be retracted into this unit during Cash Out operations. May be null in command data
        /// or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "cashOutRetract")]
        public bool? CashOutRetract { get; init; }

        /// <summary>
        /// Reject unit. Items can be rejected into this unit. May be null in command data or events if not changed or
        /// being changed.
        /// </summary>
        [DataMember(Name = "reject")]
        public bool? Reject { get; init; }

    }


    [DataContract]
    public sealed class StorageCashItemTypesClass
    {
        public StorageCashItemTypesClass(bool? Fit = null, bool? Unfit = null, bool? Unrecognized = null, bool? Counterfeit = null, bool? Suspect = null, bool? Inked = null, bool? Coupon = null, bool? Document = null)
        {
            this.Fit = Fit;
            this.Unfit = Unfit;
            this.Unrecognized = Unrecognized;
            this.Counterfeit = Counterfeit;
            this.Suspect = Suspect;
            this.Inked = Inked;
            this.Coupon = Coupon;
            this.Document = Document;
        }

        /// <summary>
        /// The storage unit can store cash items which are fit for recycling. May be null in command data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "fit")]
        public bool? Fit { get; init; }

        /// <summary>
        /// The storage unit can store cash items which are unfit for recycling. May be null in command data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "unfit")]
        public bool? Unfit { get; init; }

        /// <summary>
        /// The storage unit can store unrecognized cash items. May be null in command data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "unrecognized")]
        public bool? Unrecognized { get; init; }

        /// <summary>
        /// The storage unit can store counterfeit cash items. May be null in command data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "counterfeit")]
        public bool? Counterfeit { get; init; }

        /// <summary>
        /// The storage unit can store suspect counterfeit cash items. May be null in command data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "suspect")]
        public bool? Suspect { get; init; }

        /// <summary>
        /// The storage unit can store cash items which have been identified as ink stained. May be null in command data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "inked")]
        public bool? Inked { get; init; }

        /// <summary>
        /// Storage unit containing coupons or advertising material. May be null in command data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "coupon")]
        public bool? Coupon { get; init; }

        /// <summary>
        /// Storage unit containing documents. May be null in command data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "document")]
        public bool? Document { get; init; }

    }


    [DataContract]
    public sealed class StorageCashCapabilitiesClass
    {
        public StorageCashCapabilitiesClass(StorageCashTypesClass Types = null, StorageCashItemTypesClass Items = null, bool? HardwareSensors = null, int? RetractAreas = null, bool? RetractThresholds = null, List<string> CashItems = null)
        {
            this.Types = Types;
            this.Items = Items;
            this.HardwareSensors = HardwareSensors;
            this.RetractAreas = RetractAreas;
            this.RetractThresholds = RetractThresholds;
            this.CashItems = CashItems;
        }

        [DataMember(Name = "types")]
        public StorageCashTypesClass Types { get; init; }

        [DataMember(Name = "items")]
        public StorageCashItemTypesClass Items { get; init; }

        /// <summary>
        /// Indicates whether the storage unit has sensors which report the status. If true, then hardware sensors will
        /// override count-based replenishment status for *empty* and *full*. Other replenishment states can be
        /// overridden by counts. May be null in command data or events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "hardwareSensors")]
        public bool? HardwareSensors { get; init; }

        /// <summary>
        /// If items can be retracted into this storage unit, this is the number of areas within the storage unit which
        /// allow physical separation of different bunches. If there is no physical separation of retracted bunches
        /// within this storage unit, this value is 1. May be null if items can not be retracted into this storage unit
        /// or in events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "retractAreas")]
        [DataTypes(Minimum = 1)]
        public int? RetractAreas { get; init; }

        /// <summary>
        /// If true, indicates that retract capacity is based on counts.
        /// If false, indicates that retract capacity is based on the number of commands which resulted in items
        /// being retracted into the storage unit.
        /// May be null if items can not be retracted into this storage unit or in events if not changed or being changed.
        /// </summary>
        [DataMember(Name = "retractThresholds")]
        public bool? RetractThresholds { get; init; }

        /// <summary>
        /// An array containing multiple cash items, listing what a storage unit is capable of or configured to handle.
        /// Each member is the object name of a cash item reported by
        /// [CashManagement.GetBankNoteTypes](#cashmanagement.getbanknotetypes).
        /// May be null in command data or events if not being modified.
        /// <example>["type20USD1", "type50USD1"]</example>
        /// </summary>
        [DataMember(Name = "cashItems")]
        public List<string> CashItems { get; init; }

    }


    [DataContract]
    public sealed class StorageCashConfigurationClass
    {
        public StorageCashConfigurationClass(StorageCashTypesClass Types = null, StorageCashItemTypesClass Items = null, string Currency = null, double? Value = null, int? HighThreshold = null, int? LowThreshold = null, bool? AppLockIn = null, bool? AppLockOut = null, List<string> CashItems = null, string Name = null, int? MaxRetracts = null)
        {
            this.Types = Types;
            this.Items = Items;
            this.Currency = Currency;
            this.Value = Value;
            this.HighThreshold = HighThreshold;
            this.LowThreshold = LowThreshold;
            this.AppLockIn = AppLockIn;
            this.AppLockOut = AppLockOut;
            this.CashItems = CashItems;
            this.Name = Name;
            this.MaxRetracts = MaxRetracts;
        }

        [DataMember(Name = "types")]
        public StorageCashTypesClass Types { get; init; }

        [DataMember(Name = "items")]
        public StorageCashItemTypesClass Items { get; init; }

        /// <summary>
        /// ISO 4217 currency identifier [[Ref. cashmanagement-1](#ref-cashmanagement-1)]. May only be modified in an exchange state if applicable. May be null if the unit is
        /// configured to store mixed currencies or non-cash items.
        /// <example>USD</example>
        /// </summary>
        [DataMember(Name = "currency")]
        [DataTypes(Pattern = @"^[A-Z]{3}$")]
        public string Currency { get; init; }

        /// <summary>
        /// Absolute value of a cash item or items. May be a floating point value to allow for coins and notes which have
        /// a value which is not a whole multiple of the currency unit.
        /// 
        /// If applied to a storage unit, this applies to all contents, may be 0 if mixed and may only be modified in
        /// an exchange state if applicable.
        /// 
        /// May be null in command data or events if not being modified.
        /// <example>20</example>
        /// </summary>
        [DataMember(Name = "value")]
        public double? Value { get; init; }

        /// <summary>
        /// If specified,
        /// [replenishmentStatus](#storage.getstorage.completion.properties.storage.unit1.cash.status.replenishmentstatus)
        /// is set to *high* if the total number of items in the storage unit is greater than this number. The total number
        /// is not reported directly, but derived from *initial* + *in* - *out*.
        /// 
        /// If null, high is based on hardware sensors if supported - see
        /// [hardwareSensors](#storage.getstorage.completion.properties.storage.unit1.cash.capabilities.hardwaresensors).
        /// May be null in command data or events if not being modified.
        /// <example>500</example>
        /// </summary>
        [DataMember(Name = "highThreshold")]
        [DataTypes(Minimum = 1)]
        public int? HighThreshold { get; init; }

        /// <summary>
        /// If specified,
        /// [replenishmentStatus](#storage.getstorage.completion.properties.storage.unit1.cash.status.replenishmentstatus)
        /// is set to *low* if total number of items in the storage unit is less than this number. The total number
        /// is not reported directly, but derived from *initial* + *in* - *out*.
        /// 
        /// If null, low is based on hardware sensors if supported - see
        /// [hardwareSensors](#storage.getstorage.completion.properties.storage.unit1.cash.capabilities.hardwaresensors).
        /// May be null in command data or events if not being modified.
        /// <example>10</example>
        /// </summary>
        [DataMember(Name = "lowThreshold")]
        [DataTypes(Minimum = 1)]
        public int? LowThreshold { get; init; }

        /// <summary>
        /// If true, items cannot be accepted into the storage unit in Cash In operations.
        /// May be null in command data or events if not being modified.
        /// </summary>
        [DataMember(Name = "appLockIn")]
        public bool? AppLockIn { get; init; }

        /// <summary>
        /// If true, items cannot be dispensed from the storage unit in Cash Out operations.
        /// May be null in command data or events if not being modified.
        /// </summary>
        [DataMember(Name = "appLockOut")]
        public bool? AppLockOut { get; init; }

        /// <summary>
        /// An array containing multiple cash items, listing what a storage unit is capable of or configured to handle.
        /// Each member is the object name of a cash item reported by
        /// [CashManagement.GetBankNoteTypes](#cashmanagement.getbanknotetypes).
        /// May be null in command data or events if not being modified.
        /// <example>["type20USD1", "type50USD1"]</example>
        /// </summary>
        [DataMember(Name = "cashItems")]
        public List<string> CashItems { get; init; }

        /// <summary>
        /// Application configured name of the unit.
        /// May be null in command data or events if not being modified.
        /// <example>$10</example>
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; init; }

        /// <summary>
        /// If specified, this is the number of retract operations allowed into the unit. Only applies to retract units. If
        /// [retractOperations](#storage.getstorage.completion.properties.storage.unit1.cash.status.in.retractoperations)
        /// equals this number, then no further retracts are allowed into this storage unit.
        /// 
        /// If null in output, the maximum number is not limited by counts. May be null in command data or events if not being modified.
        /// <example>5</example>
        /// </summary>
        [DataMember(Name = "maxRetracts")]
        [DataTypes(Minimum = 1)]
        public int? MaxRetracts { get; init; }

    }


    [DataContract]
    public sealed class StorageCashOutClass
    {
        public StorageCashOutClass(StorageCashCountsClass Presented = null, StorageCashCountsClass Rejected = null, StorageCashCountsClass Distributed = null, StorageCashCountsClass Unknown = null, StorageCashCountsClass Stacked = null, StorageCashCountsClass Diverted = null, StorageCashCountsClass Transport = null)
        {
            this.Presented = Presented;
            this.Rejected = Rejected;
            this.Distributed = Distributed;
            this.Unknown = Unknown;
            this.Stacked = Stacked;
            this.Diverted = Diverted;
            this.Transport = Transport;
        }

        /// <summary>
        /// The items dispensed from this storage unit which are or were customer accessible. Will be null if no items were presented.
        /// </summary>
        [DataMember(Name = "presented")]
        public StorageCashCountsClass Presented { get; init; }

        /// <summary>
        /// The items dispensed from this storage unit which were invalid and were diverted to a reject storage
        /// unit and were not customer accessible during the operation. Will be null if no items were rejected.
        /// </summary>
        [DataMember(Name = "rejected")]
        public StorageCashCountsClass Rejected { get; init; }

        /// <summary>
        /// The items dispensed from this storage unit which were moved to a storage unit other than a reject storage unit
        /// and were not customer accessible during the operation. Will be null if no items were distributed.
        /// </summary>
        [DataMember(Name = "distributed")]
        public StorageCashCountsClass Distributed { get; init; }

        /// <summary>
        /// The items dispensed from this storage unit which moved to an unknown position. Will be null if no items were unknown.
        /// </summary>
        [DataMember(Name = "unknown")]
        public StorageCashCountsClass Unknown { get; init; }

        /// <summary>
        /// The items dispensed from this storage unit which are not customer accessible and are currently stacked
        /// awaiting presentation to the customer. This item list can increase and decrease as items are moved
        /// around in the device. This is not reset if
        /// [initial](#storage.getstorage.completion.properties.storage.unit1.cash.status.initial) is set for this unit
        /// by [Storage.GetStorage](#storage.getstorage). Will be null if no items were stacked.
        /// </summary>
        [DataMember(Name = "stacked")]
        public StorageCashCountsClass Stacked { get; init; }

        /// <summary>
        /// The items dispensed from this storage unit which are not customer accessible and were diverted to a
        /// temporary location due to being invalid and have not yet been deposited in a storage unit. This item
        /// list can increase and decrease as items are moved around in the device. This is not reset if
        /// [initial](#storage.getstorage.completion.properties.storage.unit1.cash.status.initial) is set for this unit
        /// by [Storage.GetStorage](#storage.getstorage). Will be null if no items were diverted.
        /// </summary>
        [DataMember(Name = "diverted")]
        public StorageCashCountsClass Diverted { get; init; }

        /// <summary>
        /// The items dispensed from this storage unit which are not customer accessible and which have jammed in
        /// the transport. This item list can increase and decrease as items are moved around in the device. This is not
        /// reset if [initial](#storage.getstorage.completion.properties.storage.unit1.cash.status.initial) is set for
        /// this unit by [Storage.GetStorage](#storage.getstorage). Will be null if no items apply.
        /// </summary>
        [DataMember(Name = "transport")]
        public StorageCashCountsClass Transport { get; init; }

    }


    public enum ReplenishmentStatusEnum
    {
        Ok,
        Full,
        High,
        Low,
        Empty
    }


    [DataContract]
    public sealed class StorageCashStatusClass
    {
        public StorageCashStatusClass(int? Index = null, StorageCashCountsClass Initial = null, StorageCashOutClass Out = null, StorageCashInClass In = null, AccuracyEnum? Accuracy = null, ReplenishmentStatusEnum? ReplenishmentStatus = null, OperationStatusEnum? OperationStatus = null)
        {
            this.Index = Index;
            this.Initial = Initial;
            this.Out = Out;
            this.In = In;
            this.Accuracy = Accuracy;
            this.ReplenishmentStatus = ReplenishmentStatus;
            this.OperationStatus = OperationStatus;
        }

        /// <summary>
        /// Assigned by the Service. Will be a unique number which can be used to determine
        /// *usNumber* in XFS 3.x migration. This can change as storage units are added and removed
        /// from the storage collection.
        /// <example>4</example>
        /// </summary>
        [DataMember(Name = "index")]
        [DataTypes(Minimum = 1)]
        public int? Index { get; init; }

        /// <summary>
        /// The cash related items which were in the storage unit at the last replenishment.
        /// </summary>
        [DataMember(Name = "initial")]
        public StorageCashCountsClass Initial { get; init; }

        /// <summary>
        /// The items moved from this storage unit by cash commands to another destination since the last
        /// replenishment of this unit. This includes intermediate positions such as a stacker, where an item has been
        /// moved before moving to the final destination such as another storage unit or presentation to a customer.
        /// 
        /// Counts for non-intermediate positions are reset if
        /// [initial](#storage.getstorage.completion.properties.storage.unit1.cash.status.initial) is set for this
        /// unit by [Storage.GetStorage](#storage.getstorage). See descriptions for the counts which will not be reset
        /// by this command.
        /// 
        /// Intermediate position counts are reset when the intermediate position is empty:
        /// * If it is known where the items moved to then the appropriate count or counts are modified.
        /// * If it is not known where the items moved, for example because they have been removed manually after jam
        /// clearance, then *unknown* is modified.
        /// 
        /// May be null if items have not or can not be moved from the storage unit by cash commands.
        /// </summary>
        [DataMember(Name = "out")]
        public StorageCashOutClass Out { get; init; }

        /// <summary>
        /// List of items inserted in this storage unit by cash commands from another source since the last
        /// replenishment of this unit. This also reports items in the *transport*, where an item has jammed before being
        /// deposited in the storage unit.
        /// 
        /// Counts other than *transport* are reset if
        /// [initial](#storage.getstorage.completion.properties.storage.unit1.cash.status.initial) is set for this
        /// unit by [Storage.GetStorage](#storage.getstorage). See descriptions for the counts which will not be reset
        /// by this command.
        /// 
        /// The *transport* count is reset when it is empty:
        /// * If it is known where the items moved to then the appropriate count or counts are modified.
        /// * If it is not known where the items moved, for example because they have been removed manually after jam
        /// clearance, then *unknown* is modified.
        /// 
        /// May be null if items have not or can not be moved into the storage unit by cash commands.
        /// </summary>
        [DataMember(Name = "in")]
        public StorageCashInClass In { get; init; }

        public enum AccuracyEnum
        {
            Accurate,
            AccurateSet,
            Inaccurate,
            Unknown
        }

        /// <summary>
        /// Describes the accuracy of the counts reported by *out* and *in*. If null in 
        /// [Storage.GetStorage](#storage.getstorage), the hardware is not capable of determining the accuracy, 
        /// otherwise the following values are possible:
        /// 
        /// * ```accurate``` - The *count* is expected to be accurate. The notes were previously counted 
        /// and there have since been no events that might have introduced inaccuracy. 
        /// * ```accurateSet``` - The *count* is expected to be accurate. The counts were previously set and there have 
        /// since been no events that might have introduced inaccuracy.
        /// * ```inaccurate``` - The *count* is likely to be inaccurate. A jam, picking fault, or some other event may
        /// have resulted in a counting inaccuracy.
        /// * ```unknown``` - The accuracy of *count* cannot be determined. This may be due to storage unit insertion or
        /// some other hardware event.
        /// </summary>
        [DataMember(Name = "accuracy")]
        public AccuracyEnum? Accuracy { get; init; }

        [DataMember(Name = "replenishmentStatus")]
        public ReplenishmentStatusEnum? ReplenishmentStatus { get; init; }

        public enum OperationStatusEnum
        {
            DispenseInoperative,
            DepositInoperative
        }

        /// <summary>
        /// On some devices it may be possible to allow items to be dispensed in a recycling storage unit while deposit
        /// is inoperable or vice-versa. This property allows the Service to report that one operation is possible while
        /// the other is not, without taking the storage unit out of Service completely with
        /// [status](#storage.getstorage.completion.properties.storage.unit1.status) or
        /// [replenishmentStatus](#storage.getstorage.completion.properties.storage.unit1.cash.status.replenishmentstatus).
        /// 
        /// Following values are possible:
        /// 
        /// * ```dispenseInoperative``` - Dispense operations are possible and deposit operations are not possible on
        /// this recycling storage unit.
        /// * ```depositInoperative``` - Deposit operations are possible and dispense operations are not possible on
        /// this recycling storage unit.
        /// 
        /// If null in [Storage.GetStorage](#storage.getstorage), *status* and *replenishmentStatus* apply to both cash
        /// out and cash in operations.
        /// <example>dispenseInoperative</example>
        /// </summary>
        [DataMember(Name = "operationStatus")]
        public OperationStatusEnum? OperationStatus { get; init; }

    }


    [DataContract]
    public sealed class StorageCashClass
    {
        public StorageCashClass(StorageCashCapabilitiesClass Capabilities = null, StorageCashConfigurationClass Configuration = null, StorageCashStatusClass Status = null)
        {
            this.Capabilities = Capabilities;
            this.Configuration = Configuration;
            this.Status = Status;
        }

        [DataMember(Name = "capabilities")]
        public StorageCashCapabilitiesClass Capabilities { get; init; }

        [DataMember(Name = "configuration")]
        public StorageCashConfigurationClass Configuration { get; init; }

        [DataMember(Name = "status")]
        public StorageCashStatusClass Status { get; init; }

    }


    public enum ItemTargetEnumEnum
    {
        SingleUnit,
        Retract,
        Transport,
        Stacker,
        Reject,
        ItemCassette,
        CashIn,
        OutDefault,
        OutLeft,
        OutRight,
        OutCenter,
        OutTop,
        OutBottom,
        OutFront,
        OutRear
    }


    [DataContract]
    public sealed class ItemTargetClass
    {
        public ItemTargetClass(ItemTargetEnumEnum? Target = null, string Unit = null, int? Index = null)
        {
            this.Target = Target;
            this.Unit = Unit;
            this.Index = Index;
        }

        /// <summary>
        /// This property specifies the target where items are to be moved to. Following values are possible:
        /// 
        /// * ```singleUnit``` - Move the items to a single storage unit defined by *unit*.
        /// * ```retract``` - Move the items to a retract storage unit.
        /// * ```transport``` - Move the items to the transport.
        /// * ```stacker``` - Move the items to the intermediate stacker area.
        /// * ```reject``` - Move the items to a reject storage unit.
        /// * ```itemCassette``` - Move the items to the storage units which would be used during a Cash In transaction including recycling storage units.
        /// * ```cashIn``` - Move the items to the storage units which would be used during a Cash In transaction but not including recycling storage units.
        /// * ```outDefault``` - Default output position.
        /// * ```outLeft``` - Left output position.
        /// * ```outRight``` - Right output position.
        /// * ```outCenter``` - Center output position.
        /// * ```outTop``` - Top output position.
        /// * ```outBottom``` - Bottom output position.
        /// * ```outFront``` - Front output position.
        /// * ```outRear``` - Rear output position.
        /// </summary>
        [DataMember(Name = "target")]
        public ItemTargetEnumEnum? Target { get; init; }

        /// <summary>
        /// If *target* is set to *singleUnit*, this property specifies the object name (as stated by the
        /// [Storage.GetStorage](#storage.getstorage) command) of the single unit to
        /// be used for the storage of any items found.
        /// <example>unit4</example>
        /// </summary>
        [DataMember(Name = "unit")]
        [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
        public string Unit { get; init; }

        /// <summary>
        /// If *target* is set to *retract* this property defines the position inside the retract storage units into
        /// which the cash is to be retracted. *index* starts with a value of 1 for the first retract position
        /// and increments by one for each subsequent position. If there are several retract storage units
        /// (of type *retractCassette* in [Storage.GetStorage](#storage.getstorage)), *index* would be incremented from the
        /// first position of the first retract storage unit to the last position of the last retract storage unit.
        /// The maximum value of *index* is the sum of *maximum* of each retract storage unit. If *retractArea* is not
        /// set to *retract* the value of this property is ignored.
        /// </summary>
        [DataMember(Name = "index")]
        [DataTypes(Minimum = 1)]
        public int? Index { get; init; }

    }


    public enum ShutterEnum
    {
        Closed,
        Open,
        Jammed,
        Unknown
    }


    [DataContract]
    public sealed class StorageInOutClass
    {
        public StorageInOutClass(Dictionary<string, StorageCashInClass> In = null, Dictionary<string, StorageCashOutClass> Out = null)
        {
            this.In = In;
            this.Out = Out;
        }

        /// <summary>
        /// Object containing the storage units which have had items inserted during the associated operation or
        /// transaction. Only storage units whose contents have been modified are included.
        /// </summary>
        [DataMember(Name = "in")]
        public Dictionary<string, StorageCashInClass> In { get; init; }

        /// <summary>
        /// Object containing the storage units which have had items removed during the associated operation or
        /// transaction. Only storage units whose contents have been modified are included.
        /// </summary>
        [DataMember(Name = "out")]
        public Dictionary<string, StorageCashOutClass> Out { get; init; }

    }


    [DataContract]
    public sealed class PositionInfoNullableClass
    {
        public PositionInfoNullableClass(PositionEnum? Position = null, string AdditionalBunches = null)
        {
            this.Position = Position;
            this.AdditionalBunches = AdditionalBunches;
        }

        [DataMember(Name = "position")]
        public PositionEnum? Position { get; init; }

        /// <summary>
        /// Specifies how many more bunches will be required to present the request. Following values are possible:
        /// 
        ///   * ```&lt;number&gt;``` - The number of additional bunches to be presented.
        ///   * ```unknown``` - More than one additional bunch is required but the precise number is unknown.
        /// <example>1</example>
        /// </summary>
        [DataMember(Name = "additionalBunches")]
        [DataTypes(Pattern = @"^unknown$|^[0-9]*$")]
        public string AdditionalBunches { get; init; }

    }


    [DataContract]
    public sealed class SignatureClass
    {
        public SignatureClass(string NoteType = null, OrientationEnum? Orientation = null, List<byte> Signature = null)
        {
            this.NoteType = NoteType;
            this.Orientation = Orientation;
            this.Signature = Signature;
        }

        /// <summary>
        /// A cash item as reported by [CashManagement.GetBankNoteTypes](#cashmanagement.getbanknotetypes). This is null
        /// if the item was not identified as a cash item.
        /// <example>type20USD1</example>
        /// </summary>
        [DataMember(Name = "noteType")]
        [DataTypes(Pattern = @"^type[0-9A-Z]+$")]
        public string NoteType { get; init; }

        [DataMember(Name = "orientation")]
        public OrientationEnum? Orientation { get; init; }

        /// <summary>
        /// Base64 encoded vendor specific signature data. If no signature is available or has not been requested then this is null.
        /// <example>MAA5ADgANwA2ADUANAAz ...</example>
        /// </summary>
        [DataMember(Name = "signature")]
        [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
        public List<byte> Signature { get; init; }

    }


    [DataContract]
    public sealed class StorageSetCashStatusClass
    {
        public StorageSetCashStatusClass(StorageCashCountsClass Initial = null)
        {
            this.Initial = Initial;
        }

        /// <summary>
        /// The cash related items which are in the storage unit at the last replenishment. If specified,
        /// [out](#storage.getstorage.completion.properties.storage.unit1.cash.status.out) and
        /// [in](#storage.getstorage.completion.properties.storage.unit1.cash.status.in)
        /// are reset to empty.
        /// </summary>
        [DataMember(Name = "initial")]
        public StorageCashCountsClass Initial { get; init; }

    }


    [DataContract]
    public sealed class StorageSetCashClass
    {
        public StorageSetCashClass(StorageCashConfigurationClass Configuration = null, StorageSetCashStatusClass Status = null)
        {
            this.Configuration = Configuration;
            this.Status = Status;
        }

        [DataMember(Name = "configuration")]
        public StorageCashConfigurationClass Configuration { get; init; }

        [DataMember(Name = "status")]
        public StorageSetCashStatusClass Status { get; init; }

    }


}
