/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(SafeDoorEnum? SafeDoor = null, DispenserEnum? Dispenser = null, AcceptorEnum? Acceptor = null)
        {
            this.SafeDoor = SafeDoor;
            this.Dispenser = Dispenser;
            this.Acceptor = Acceptor;
        }

        public enum SafeDoorEnum
        {
            DoorNotSupported,
            DoorOpen,
            DoorClosed,
            DoorUnknown
        }

        /// <summary>
        /// Supplies the state of the safe door. Following values are possible:
        /// 
        /// * ```doorNotSupported``` - Physical device has no safe door or safe door state reporting is not supported.
        /// * ```doorOpen``` - Safe door is open.
        /// * ```doorClosed``` - Safe door is closed.
        /// * ```doorUnknown``` - Due to a hardware error or other condition, the state of the safe door cannot be determined.
        /// </summary>
        [DataMember(Name = "safeDoor")]
        public SafeDoorEnum? SafeDoor { get; init; }

        public enum DispenserEnum
        {
            Ok,
            CashUnitState,
            CashUnitStop,
            CashUnitUnknown
        }

        /// <summary>
        /// Supplies the state of the storage units for dispensing cash. Following values are possible:
        /// 
        /// * ```ok``` - All storage units present are in a good state.
        /// * ```cashUnitState``` - One or more of the storage units is in a low, empty, inoperative or manipulated condition. 
        /// Items can still be dispensed from at least one of the storage units.
        /// * ```cashUnitStop``` - Due to a storage unit failure dispensing is impossible. No items can be dispensed because 
        /// all of the storage units are in an empty, inoperative or manipulated condition. This state may also occur 
        /// when a reject/retract storage unit is full or no reject/retract storage unit is present, or when an application 
        /// lock is set on every storage unit which can be locked.
        /// * ```cashUnitUnknown``` - Due to a hardware error or other condition, the state of the storage units cannot be determined.
        /// </summary>
        [DataMember(Name = "dispenser")]
        public DispenserEnum? Dispenser { get; init; }

        public enum AcceptorEnum
        {
            Ok,
            CashUnitState,
            CashUnitStop,
            CashUnitUnknown
        }

        /// <summary>
        /// Supplies the state of the storage units for accepting cash. Following values are possible:
        /// 
        /// * ```ok``` - All storage units present are in a good state.
        /// * ```cashUnitState``` - One or more of the storage units is in a high, full, inoperative or manipulated condition. Items can still be accepted into at least one of the storage units.
        /// * ```cashUnitStop``` - Due to a storage unit failure accepting is impossible. No items can be accepted because all of the storage units are in a full, inoperative or manipulated condition.
        /// This state may also occur when a retract storage unit is full or no retract cash storage unit is present, 
        /// or when an application lock is set on every storage unit, or when Level 2/3 notes are to be automatically retained within storage units, 
        /// but all of the designated storage units for storing them are full or inoperative.
        /// * ```cashUnitUnknown``` - Due to a hardware error or other condition, the state of the storage units cannot be determined.
        /// </summary>
        [DataMember(Name = "acceptor")]
        public AcceptorEnum? Acceptor { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(bool? SafeDoor = null, bool? CashBox = null, ExchangeTypeClass ExchangeType = null, ItemInfoTypesClass ItemInfoTypes = null, bool? ClassificationList = null, bool? PhysicalNoteList = null)
        {
            this.SafeDoor = SafeDoor;
            this.CashBox = CashBox;
            this.ExchangeType = ExchangeType;
            this.ItemInfoTypes = ItemInfoTypes;
            this.ClassificationList = ClassificationList;
            this.PhysicalNoteList = PhysicalNoteList;
        }

        /// <summary>
        /// Specifies whether or not the  CashManagement.OpenSafeDoor command is supported.
        /// </summary>
        [DataMember(Name = "safeDoor")]
        public bool? SafeDoor { get; init; }

        /// <summary>
        /// This field is only applicable to teller type devices. 
        /// It specifies whether or not tellers have been assigned a cash box.
        /// </summary>
        [DataMember(Name = "cashBox")]
        public bool? CashBox { get; init; }

        [DataContract]
        public sealed class ExchangeTypeClass
        {
            public ExchangeTypeClass(bool? ByHand = null, bool? ToCassettes = null, bool? ClearRecycler = null, bool? DepositInto = null)
            {
                this.ByHand = ByHand;
                this.ToCassettes = ToCassettes;
                this.ClearRecycler = ClearRecycler;
                this.DepositInto = DepositInto;
            }

            /// <summary>
            /// The device supports manual replenishment either by filling the cash storage unit by hand or by replacing the cash storage unit.
            /// </summary>
            [DataMember(Name = "byHand")]
            public bool? ByHand { get; init; }

            /// <summary>
            /// The device supports moving items from the replenishment cash storage unit to another cash storage unit.
            /// </summary>
            [DataMember(Name = "toCassettes")]
            public bool? ToCassettes { get; init; }

            /// <summary>
            /// The device supports the emptying of recycle cash storage units.
            /// </summary>
            [DataMember(Name = "clearRecycler")]
            public bool? ClearRecycler { get; init; }

            /// <summary>
            /// The device supports moving items from the deposit entrance to the bill cash storage units.
            /// </summary>
            [DataMember(Name = "depositInto")]
            public bool? DepositInto { get; init; }

        }

        /// <summary>
        /// Specifies the type of cash storage unit exchange operations supported by the device.
        /// </summary>
        [DataMember(Name = "exchangeType")]
        public ExchangeTypeClass ExchangeType { get; init; }

        [DataContract]
        public sealed class ItemInfoTypesClass
        {
            public ItemInfoTypesClass(bool? SerialNumber = null, bool? Signature = null, bool? ImageFile = null)
            {
                this.SerialNumber = SerialNumber;
                this.Signature = Signature;
                this.ImageFile = ImageFile;
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
            /// Image file of the item.
            /// </summary>
            [DataMember(Name = "imageFile")]
            public bool? ImageFile { get; init; }

        }

        /// <summary>
        /// Specifies the types of information that can be retrieved through the CashManagement.GetItemInfo command.
        /// </summary>
        [DataMember(Name = "itemInfoTypes")]
        public ItemInfoTypesClass ItemInfoTypes { get; init; }

        /// <summary>
        /// Specifies whether the device has the capability to maintain a classification list of serial numbers as well as
        /// supporting the associated operations. This can either be TRUE if the device has the capability or FALSE if it does not.
        /// </summary>
        [DataMember(Name = "classificationList")]
        public bool? ClassificationList { get; init; }

        /// <summary>
        /// Specifies whether the Service supports note number lists on physical cash units.
        /// This can either be TRUE if the Service has the capability or FALSE if it does not.
        /// </summary>
        [DataMember(Name = "physicalNoteList")]
        public bool? PhysicalNoteList { get; init; }

    }


    [DataContract]
    public sealed class StorageCashTypesClass
    {
        public StorageCashTypesClass(bool? CashIn = null, bool? CashOut = null, bool? Replenishment = null, bool? CashInRetract = null, bool? CashOutRetract = null, bool? Reject = null, bool? Coupon = null, bool? Document = null)
        {
            this.CashIn = CashIn;
            this.CashOut = CashOut;
            this.Replenishment = Replenishment;
            this.CashInRetract = CashInRetract;
            this.CashOutRetract = CashOutRetract;
            this.Reject = Reject;
            this.Coupon = Coupon;
            this.Document = Document;
        }

        /// <summary>
        /// The unit can accept cash items. If _cashOut_ is also true then the unit can recycle.
        /// </summary>
        [DataMember(Name = "cashIn")]
        public bool? CashIn { get; init; }

        /// <summary>
        /// The unit can dispense cash items. If _cashIn_ is also true then the unit can recycle.
        /// </summary>
        [DataMember(Name = "cashOut")]
        public bool? CashOut { get; init; }

        /// <summary>
        /// Replenishment container. A storage unit can be refilled from or emptied to a replenishment container.
        /// </summary>
        [DataMember(Name = "replenishment")]
        public bool? Replenishment { get; init; }

        /// <summary>
        /// Retract unit. Items can be retracted into this unit during Cash In operations.
        /// </summary>
        [DataMember(Name = "cashInRetract")]
        public bool? CashInRetract { get; init; }

        /// <summary>
        /// Retract unit. Items can be retracted into this unit during Cash Out operations.
        /// </summary>
        [DataMember(Name = "cashOutRetract")]
        public bool? CashOutRetract { get; init; }

        /// <summary>
        /// Reject unit. Items can be rejected into this unit.
        /// </summary>
        [DataMember(Name = "reject")]
        public bool? Reject { get; init; }

        /// <summary>
        /// Storage unit containing coupons or advertising material.
        /// </summary>
        [DataMember(Name = "coupon")]
        public bool? Coupon { get; init; }

        /// <summary>
        /// Storage unit containing documents.
        /// </summary>
        [DataMember(Name = "document")]
        public bool? Document { get; init; }

    }


    [DataContract]
    public sealed class StorageCashItemTypesClass
    {
        public StorageCashItemTypesClass(bool? Fit = null, bool? Unfit = null, bool? Unrecognized = null, bool? Counterfeit = null, bool? Suspect = null, bool? Inked = null)
        {
            this.Fit = Fit;
            this.Unfit = Unfit;
            this.Unrecognized = Unrecognized;
            this.Counterfeit = Counterfeit;
            this.Suspect = Suspect;
            this.Inked = Inked;
        }

        /// <summary>
        /// The storage unit can store cash items which are fit for recycling (classification level 4).
        /// </summary>
        [DataMember(Name = "fit")]
        public bool? Fit { get; init; }

        /// <summary>
        /// The storage unit can store cash items which are unfit for recycling (classification level 4).
        /// </summary>
        [DataMember(Name = "unfit")]
        public bool? Unfit { get; init; }

        /// <summary>
        /// The storage unit can store unrecognized cash items (classification level 1).
        /// </summary>
        [DataMember(Name = "unrecognized")]
        public bool? Unrecognized { get; init; }

        /// <summary>
        /// The storage unit can store counterfeit cash items (classification level 2).
        /// </summary>
        [DataMember(Name = "counterfeit")]
        public bool? Counterfeit { get; init; }

        /// <summary>
        /// The storage unit can store suspect cash items (classification level 3).
        /// </summary>
        [DataMember(Name = "suspect")]
        public bool? Suspect { get; init; }

        /// <summary>
        /// The storage unit can store cash items which have been identified as ink stained.
        /// </summary>
        [DataMember(Name = "inked")]
        public bool? Inked { get; init; }

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
        /// Assigned by the XFS4IoT service. A unique number identifying a single cash item. 
        /// Each unique combination of the other properties will have a different noteID. 
        /// Can be used for migration of _usNoteID_ from XFS 3.x.
        /// </summary>
        [DataMember(Name = "noteID")]
        [DataTypes(Minimum = 1)]
        public int? NoteID { get; init; }

        /// <summary>
        /// ISO 4217 currency.
        /// </summary>
        [DataMember(Name = "currency")]
        public string Currency { get; init; }

        /// <summary>
        /// Absolute value of all contents, 0 if mixed. May only be modified in an exchange state if applicable. May be 
        /// a floating point value to allow for coins and notes which have a value which is not a whole multiple 
        /// of the currency unit.
        /// </summary>
        [DataMember(Name = "value")]
        public double? Value { get; init; }

        /// <summary>
        /// The release of the cash item. The higher this number is, the newer the release.
        /// Zero means that there is only one release of that cash item or the device is not capable of
        /// distinguishing different release of the item, for example in a simple cash dispenser. This value has not 
        /// been standardized and therefore a release number of the same cash item will not necessarily have the same 
        /// value in different systems.
        /// </summary>
        [DataMember(Name = "release")]
        [DataTypes(Minimum = 0)]
        public int? Release { get; init; }

    }


    [DataContract]
    public sealed class StorageCashCapabilitiesClass
    {
        public StorageCashCapabilitiesClass(StorageCashTypesClass Types = null, StorageCashItemTypesClass Items = null, bool? HardwareSensors = null, int? RetractAreas = null, bool? RetractThresholds = null, Dictionary<string, CashItemClass> CashItems = null)
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
        /// Indicates whether the storage unit has sensors which report the status.
        /// </summary>
        [DataMember(Name = "hardwareSensors")]
        public bool? HardwareSensors { get; init; }

        /// <summary>
        /// If items can be retracted into this storage unit, this is the number of areas within the storage unit which 
        /// allow physical separation of different bunches. If there is no physical separation of retracted bunches 
        /// within this storage unit, this value is 1.
        /// </summary>
        [DataMember(Name = "retractAreas")]
        [DataTypes(Minimum = 1)]
        public int? RetractAreas { get; init; }

        /// <summary>
        /// If true, indicates that retract capacity is based on counts.
        /// If false, indicates that retract capacity is based on the number of commands which resulted in items 
        /// being retracted into the storage unit.
        /// </summary>
        [DataMember(Name = "retractThresholds")]
        public bool? RetractThresholds { get; init; }

        /// <summary>
        /// Lists the cash items which the storage unit is physically capable of handling. For example a coin storage
        /// unit may be restricted to one coin denomination due to the hardware.
        /// </summary>
        [DataMember(Name = "cashItems")]
        public Dictionary<string, CashItemClass> CashItems { get; init; }

    }


    [DataContract]
    public sealed class StorageCashConfigurationClass
    {
        public StorageCashConfigurationClass(StorageCashTypesClass Types = null, StorageCashItemTypesClass Items = null, string Currency = null, double? Value = null, int? HighThreshold = null, int? LowThreshold = null, bool? AppLockIn = null, bool? AppLockOut = null, Dictionary<string, CashItemClass> CashItems = null, string Name = null, int? MaxRetracts = null)
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
        /// ISO 4217 currency. May only be modified in an exchange state if applicable.
        /// </summary>
        [DataMember(Name = "currency")]
        public string Currency { get; init; }

        /// <summary>
        /// Absolute value of all contents, 0 if mixed. May only be modified in an exchange state if applicable. May be 
        /// a floating point value to allow for coins and notes which have a value which is not a whole multiple 
        /// of the currency unit.
        /// </summary>
        [DataMember(Name = "value")]
        public double? Value { get; init; }

        /// <summary>
        /// If specified, 
        /// [replenishmentStatus](#storage.getstorage.completion.properties.storage.unit1.cash.status.replenishmentstatus) 
        /// is set to _high_ if 
        /// [count](#storage.getstorage.completion.properties.storage.unit1.cash.status.count) is greater than this number.
        /// 
        /// If not specified, high is based on hardware sensors if supported - see
        /// [hardwareSensors](#storage.getstorage.completion.properties.storage.unit1.cash.capabilities.hardwaresensors).
        /// </summary>
        [DataMember(Name = "highThreshold")]
        [DataTypes(Minimum = 1)]
        public int? HighThreshold { get; init; }

        /// <summary>
        /// If specified, 
        /// [replenishmentStatus](#storage.getstorage.completion.properties.storage.unit1.cash.status.replenishmentstatus) 
        /// is set to _low_ if 
        /// [count](#storage.getstorage.completion.properties.storage.unit1.cash.status.count) is less than this number.
        /// 
        /// If not specified, low is based on hardware sensors if supported - see
        /// [hardwareSensors](#storage.getstorage.completion.properties.storage.unit1.cash.capabilities.hardwaresensors).
        /// </summary>
        [DataMember(Name = "lowThreshold")]
        [DataTypes(Minimum = 1)]
        public int? LowThreshold { get; init; }

        /// <summary>
        /// If true, items cannot be accepted into the storage unit in Cash In operations.
        /// </summary>
        [DataMember(Name = "appLockIn")]
        public bool? AppLockIn { get; init; }

        /// <summary>
        /// If true, items cannot be dispensed from the storage unit in Cash Out operations.
        /// </summary>
        [DataMember(Name = "appLockOut")]
        public bool? AppLockOut { get; init; }

        /// <summary>
        /// An object containing multiple cash items, listing what a storage unit is capable of or configured to handle.
        /// </summary>
        [DataMember(Name = "cashItems")]
        public Dictionary<string, CashItemClass> CashItems { get; init; }

        /// <summary>
        /// Application configured name of the unit.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; init; }

        /// <summary>
        /// If specified, this is the number of retract operations allowed into the unit.
        /// 
        /// If not specified, the maximum number is not limited by counts.
        /// </summary>
        [DataMember(Name = "maxRetracts")]
        [DataTypes(Minimum = 1)]
        public int? MaxRetracts { get; init; }

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
        /// Count of genuine cash items which are fit for recycling (classification level 4 - see 
        /// [Note Classification](#noteclassification)).
        /// </summary>
        [DataMember(Name = "fit")]
        public int? Fit { get; init; }

        /// <summary>
        /// Count of genuine cash items which are unfit for recycling (classification level 4 - see 
        /// [Note Classification](#noteclassification)).
        /// </summary>
        [DataMember(Name = "unfit")]
        public int? Unfit { get; init; }

        /// <summary>
        /// Count of suspect cash items (classification level 3 - see 
        /// [Note Classification](#noteclassification)).
        /// </summary>
        [DataMember(Name = "suspect")]
        public int? Suspect { get; init; }

        /// <summary>
        /// Count of counterfeit cash items (classification level 2 - see 
        /// [Note Classification](#noteclassification)).
        /// </summary>
        [DataMember(Name = "counterfeit")]
        public int? Counterfeit { get; init; }

        /// <summary>
        /// Count of cash items which have been identified as ink stained.
        /// </summary>
        [DataMember(Name = "inked")]
        public int? Inked { get; init; }

    }


    [DataContract]
    public sealed class StorageCashCountsClass
    {
        public StorageCashCountsClass(int? Unrecognized = null, Dictionary<string, StorageCashCountClass> Cash = null)
        {
            this.Unrecognized = Unrecognized;
            this.Cash = Cash;
        }

        /// <summary>
        /// Count of unrecognized items handled by the cash interface
        /// </summary>
        [DataMember(Name = "unrecognized")]
        public int? Unrecognized { get; init; }

        /// <summary>
        /// Counts of cash items broken down by cash item type and classification
        /// </summary>
        [DataMember(Name = "cash")]
        public Dictionary<string, StorageCashCountClass> Cash { get; init; }

    }


    [DataContract]
    public sealed class StorageCashStatusClass
    {
        public StorageCashStatusClass(int? Index = null, StorageCashCountsClass Initial = null, OutClass Out = null, InClass In = null, int? Count = null, AccuracyEnum? Accuracy = null, ReplenishmentStatusEnum? ReplenishmentStatus = null)
        {
            this.Index = Index;
            this.Initial = Initial;
            this.Out = Out;
            this.In = In;
            this.Count = Count;
            this.Accuracy = Accuracy;
            this.ReplenishmentStatus = ReplenishmentStatus;
        }

        /// <summary>
        /// Assigned by the XFS4IoT service. Will be a unique number which can be used to determine 
        /// _usNumber_ in XFS 3.x migration. This can change as cash storage units are added and removed 
        /// from the storage collection.
        /// </summary>
        [DataMember(Name = "index")]
        [DataTypes(Minimum = 1)]
        public int? Index { get; init; }

        /// <summary>
        /// The cash related items which were in the storage unit at the last replenishment.
        /// </summary>
        [DataMember(Name = "initial")]
        public StorageCashCountsClass Initial { get; init; }

        [DataContract]
        public sealed class OutClass
        {
            public OutClass(StorageCashCountsClass Presented = null, StorageCashCountsClass Rejected = null, StorageCashCountsClass Distributed = null, StorageCashCountsClass Unknown = null, StorageCashCountsClass Stacked = null, StorageCashCountsClass Diverted = null, StorageCashCountsClass Transport = null)
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
            /// The items dispensed from this storage unit which are or were customer accessible.
            /// </summary>
            [DataMember(Name = "presented")]
            public StorageCashCountsClass Presented { get; init; }

            /// <summary>
            /// The items dispensed from this storage unit which were invalid and were diverted to a reject storage 
            /// unit and were not customer accessible during the operation.
            /// </summary>
            [DataMember(Name = "rejected")]
            public StorageCashCountsClass Rejected { get; init; }

            /// <summary>
            /// The items dispensed from this storage unit which were moved to a storage unit other than a reject storage unit 
            /// and were not customer accessible during the operation.
            /// </summary>
            [DataMember(Name = "distributed")]
            public StorageCashCountsClass Distributed { get; init; }

            /// <summary>
            /// The items dispensed from this storage unit which moved to an unknown position.
            /// </summary>
            [DataMember(Name = "unknown")]
            public StorageCashCountsClass Unknown { get; init; }

            /// <summary>
            /// The items dispensed from this storage unit which are not customer accessible and are currently stacked 
            /// awaiting presentation to the customer. This item list can increase and decrease as items are moved 
            /// around in the device.
            /// </summary>
            [DataMember(Name = "stacked")]
            public StorageCashCountsClass Stacked { get; init; }

            /// <summary>
            /// The items dispensed from this storage unit which are not customer accessible and were diverted to a 
            /// temporary location due to being invalid and have not yet been deposited in a storage unit. This item 
            /// list can increase and decrease as items are moved around in the device.
            /// </summary>
            [DataMember(Name = "diverted")]
            public StorageCashCountsClass Diverted { get; init; }

            /// <summary>
            /// The items dispensed from this storage unit which are not customer accessible and which have jammed in
            /// the transport. This item list can increase and decrease as items are moved around in the device.
            /// </summary>
            [DataMember(Name = "transport")]
            public StorageCashCountsClass Transport { get; init; }

        }

        /// <summary>
        /// The items moved from this storage unit by cash commands to another destination since the last 
        /// replenishment of this unit. Reset to empty if 
        /// [initial](#storage.getstorage.completion.properties.storage.unit1.cash.status.initial) is set for this unit
        /// by [Storage.GetStorage](#storage.getstorage).
        /// </summary>
        [DataMember(Name = "out")]
        public OutClass Out { get; init; }

        [DataContract]
        public sealed class InClass
        {
            public InClass(int? RetractOperations = null, StorageCashCountsClass Deposited = null, StorageCashCountsClass Retracted = null, StorageCashCountsClass Rejected = null, StorageCashCountsClass Distributed = null)
            {
                this.RetractOperations = RetractOperations;
                this.Deposited = Deposited;
                this.Retracted = Retracted;
                this.Rejected = Rejected;
                this.Distributed = Distributed;
            }

            /// <summary>
            /// Number of cash retract operations which resulted in items entering this storage unit. This can be 
            /// used where devices do not have the capability to count or validate items after presentation.
            /// </summary>
            [DataMember(Name = "retractOperations")]
            public int? RetractOperations { get; init; }

            /// <summary>
            /// The items deposited in the storage unit during a Cash In transaction.
            /// </summary>
            [DataMember(Name = "deposited")]
            public StorageCashCountsClass Deposited { get; init; }

            /// <summary>
            /// The items deposited in the storage unit after being accessible to a customer. This may be inaccurate 
            /// or not counted if items are not counted or re-validated after presentation, the number of retract 
            /// operations is also reported separately in _retractOperations_.
            /// </summary>
            [DataMember(Name = "retracted")]
            public StorageCashCountsClass Retracted { get; init; }

            /// <summary>
            /// The items deposited in this storage unit originating from another storage unit but rejected due to being 
            /// invalid. This count may be inaccurate due to the nature of rejected items.
            /// </summary>
            [DataMember(Name = "rejected")]
            public StorageCashCountsClass Rejected { get; init; }

            /// <summary>
            /// The items deposited in this storage unit originating from another storage unit but not rejected.
            /// </summary>
            [DataMember(Name = "distributed")]
            public StorageCashCountsClass Distributed { get; init; }

        }

        /// <summary>
        /// List of items inserted in this storage unit by cash commands from another source since the last 
        /// replenishment of this unit. Reset to empty if 
        /// [initial](#storage.getstorage.completion.properties.storage.unit1.cash.status.initial) is set for this unit
        /// by [Storage.GetStorage](#storage.getstorage).
        /// </summary>
        [DataMember(Name = "in")]
        public InClass In { get; init; }

        /// <summary>
        /// Total count of the items in the unit, derived from the 
        /// [initial](#storage.getstorage.completion.properties.storage.unit1.cash.status.initial),
        /// [out](#storage.getstorage.completion.properties.storage.unit1.cash.status.out) and
        /// [in](#storage.getstorage.completion.properties.storage.unit1.cash.status.in) counts, but
        /// supplied for ease of use.
        /// 
        /// For units which dispense items, this count is only decremented when the items are either known to be 
        /// in customer access or successfully rejected, therefore the intermediate _out_ fields are 
        /// not included in this calculation:
        /// - [stacked](#storage.getstorage.completion.properties.storage.unit1.cash.status.out.stacked)
        /// - [transport](#storage.getstorage.completion.properties.storage.unit1.cash.status.out.transport)
        /// - [unknown](#storage.getstorage.completion.properties.storage.unit1.cash.status.out.unknown)
        /// 
        /// If counts being incorrectly set at replenishment time 
        /// means that this would result in a negative number, this reports 0.
        /// </summary>
        [DataMember(Name = "count")]
        [DataTypes(Minimum = 0)]
        public int? Count { get; init; }

        public enum AccuracyEnum
        {
            NotSupported,
            Accurate,
            AccurateSet,
            Inaccurate,
            Unknown
        }

        /// <summary>
        /// Describes the accuracy of _count_. Following values are possible:
        /// 
        /// * ```notSupported``` - The hardware is not capable of determining the accuracy of _count_.
        /// * ```accurate``` - The _count_ is expected to be accurate. The notes were previously counted 
        /// and there have since been no events that might have introduced inaccuracy. 
        /// * ```accurateSet``` - The _count_ is expected to be accurate. The counts were previously set and there have 
        /// since been no events that might have introduced inaccuracy.
        /// * ```inaccurate``` - The _count_ is likely to be inaccurate. A jam, picking fault, or some other event may 
        /// have resulted in a counting inaccuracy.
        /// * ```unknown``` - The accuracy of _count_ cannot be determined. This may be due to cash unit insertion or 
        /// some other hardware event.
        /// </summary>
        [DataMember(Name = "accuracy")]
        public AccuracyEnum? Accuracy { get; init; }

        public enum ReplenishmentStatusEnum
        {
            Ok,
            Full,
            High,
            Low,
            Empty
        }

        /// <summary>
        /// The state of the media in the unit if it can be determined. Note that overall 
        /// [status](#storage.getstorage.completion.properties.storage.unit1.status) of the storage unit must
        /// be taken into account when deciding whether the storage unit is usable and whether replenishment status
        /// is applicable. In particular, if the overall status is _missing_ this will not be reported.
        /// The following values are possible:
        /// 
        /// * ```ok``` - The storage unit media is in a good state.
        /// * ```full``` - The storage unit is full.
        /// * ```high``` - The storage unit is almost full (either sensor based or exceeded the 
        /// [highThreshold](#storage.getstorage.completion.properties.storage.unit1.cash.configuration.highthreshold).
        /// * ```low``` - The storage unit is almost empty (either sensor based or below the 
        /// [lowThreshold](#storage.getstorage.completion.properties.storage.unit1.cash.configuration.lowthreshold)). 
        /// * ```empty``` - The storage unit is empty, or insufficient items in the storage unit are preventing further 
        /// dispense operations.
        /// </summary>
        [DataMember(Name = "replenishmentStatus")]
        public ReplenishmentStatusEnum? ReplenishmentStatus { get; init; }

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
