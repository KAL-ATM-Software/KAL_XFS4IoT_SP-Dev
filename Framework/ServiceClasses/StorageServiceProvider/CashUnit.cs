/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTFramework.Storage
{
    [Serializable()]
    public sealed record CashUnitStorage : UnitStorageBase
    {
        public CashUnitStorage(CashUnitStorageConfiguration StorageConfiguration) :
            base(StorageConfiguration.PositionName,
                 StorageConfiguration.Capacity,
                 StatusEnum.NotConfigured,
                 StorageConfiguration.SerialNumber)
        {
            Id = StorageConfiguration.Id;
            Unit = new CashUnit(StorageConfiguration.Capabilities,
                                StorageConfiguration.Configuration,
                                StorageConfiguration.CashUnitAdditionalInfo);
        }

        /// <summary>
        /// An identifier which can be used for cUnitID in CDM/CIM XFS 3.x migration. Not required if not applicable.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Cash Unit information
        /// </summary>
        public CashUnit Unit { get; init; }
    }

    /// <summary>
    /// Capabilities of the cash unit
    /// </summary>
    [Serializable()]
    public sealed record CashCapabilitiesClass
    {
        [Flags]
        public enum TypesEnum
        {
            CashIn = 1 << 0,
            CashOut = 1 << 1,
            Replenishment = 1 << 2,
            CashInRetract = 1 << 3,
            CashOutRetract = 1 << 4,
            Reject = 1 << 5,
        }

        [Flags]
        public enum ItemsEnum
        {
            Fit = 1 << 0,
            Unfit = 1 << 1,
            Unrecognized = 1 << 2,
            Counterfeit = 1 << 3,
            Suspect = 1 << 4,
            Inked = 1 << 5,
            Coupon = 1 << 6,
            Document = 1 << 7,
        }

        public CashCapabilitiesClass(TypesEnum Types,
                                     ItemsEnum Items,
                                     bool HardwareSensors,
                                     int RetractAreas,
                                     List<string> BanknoteItems)
        {
            this.Types = Types;
            this.Items = Items;
            this.HardwareSensors = HardwareSensors;
            this.RetractAreas = RetractAreas;
            this.BanknoteItems = BanknoteItems;
        }

        /// <summary>
        /// The types of operation the unit is capable to perform. This is a combination of one or 
        /// more operations
        /// </summary>
        public TypesEnum Types { get; init; }

        /// <summary>
        /// The types of cash media the unit is capable of storing to store.This is a combination of
        /// one or more item types.May only be modified in an exchange state if applicable.
        /// </summary>
        public ItemsEnum Items { get; init; }

        /// <summary>
        /// The storage unit has hardware sensors that can detect threshold states.
        /// </summary>
        public bool HardwareSensors { get; init; }

        /// <summary>
        ///  If items can be retracted into this storage unit, this is the number of areas within the storage unit which 
        ///  allow physical separation of different bunches.If there is no physical separation of retracted bunches
        ///  within this storage unit, this value is 1.
        /// </summary>
        public int RetractAreas { get; init; }

        /// <summary>
        /// If true, indicates that retract capacity is based on counts.
        /// If false, indicates that retract capacity is based on the number of commands which resulted in items
        /// being retracted into the storage unit.
        /// </summary>
        public bool RetractThresholds { get; init; }

        /// <summary>
        /// Lists the cash items which the storage unit is physically capable of handling.
        /// </summary>
        public List<string> BanknoteItems { get; init; }
    }

    /// <summary>
    /// Configuration of the cash unit
    /// </summary>
    [Serializable()]
    public sealed record CashConfigurationClass
    {
        public CashConfigurationClass(CashCapabilitiesClass.TypesEnum Types,
                                      CashCapabilitiesClass.ItemsEnum Items,
                                      string Currency,
                                      double Value,
                                      int HighThreshold,
                                      int LowThreshold,
                                      bool AppLockIn,
                                      bool AppLockOut,
                                      List<string> BanknoteItems)
        {
            this.Types = Types;
            this.Items = Items;
            this.Currency = Currency;
            this.Value = Value;
            this.HighThreshold = HighThreshold;
            this.LowThreshold = LowThreshold;
            this.AppLockIn = AppLockIn;
            this.AppLockOut = AppLockOut;
            this.BanknoteItems = BanknoteItems;
        }

        /// <summary>
        /// The types of operation the unit is capable of configured to perform. This is a combination of one or 
        /// more operations
        /// </summary>
        public CashCapabilitiesClass.TypesEnum Types { get; set; }

        /// <summary>
        /// The types of cash media the unit is configured to store. This is a combination of
        /// one or more item types.May only be modified in an exchange state if applicable.
        /// </summary>
        public CashCapabilitiesClass.ItemsEnum Items { get; set; }

        /// <summary>
        /// ISO 4217 currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Absolute value of all contents, 0 if mixed. May only be modified in an exchange state if applicable. May be 
        /// a floating point value to allow for coins and notes which have a value which is not a whole multiple
        /// of the currency unit.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// If specified, ReplenishmentStatus is set to High if the count is greater than this number.
        /// </summary>
        public int HighThreshold { get; set; }

        /// <summary>
        /// IIf specified, ReplenishmentStatus is set to Low if the count is lower than this number.
        /// </summary>
        public int LowThreshold { get; set; }

        /// <summary>
        /// If true, items cannot be accepted into the storage unit in Cash In operations.
        /// </summary>
        public bool AppLockIn { get; set; }

        /// <summary>
        /// If true, items cannot be dispensed from the storage unit in Cash Out operations.
        /// </summary>
        public bool AppLockOut { get; set; }

        /// <summary>
        /// Lists the cash items which are configured to this unit.
        /// </summary>
        public List<string> BanknoteItems { get; set; }
    }

    /// <summary>
    /// Status of the cash unit
    /// </summary>
    [Serializable()]
    public sealed record CashStatusClass
    {
        public CashStatusClass(CashUnitAdditionalInfoClass AdditionalInfo)
        {
            Index = AdditionalInfo.Index;
            InitialCounts = new();
            StorageCashOutCount = new();
            StorageCashInCount = new();
            Count = 0;
            if (AdditionalInfo.AccuracySupported)
                Accuracy = AccuracyEnum.Unknown;
            else
                Accuracy = AccuracyEnum.NotSupported;
            ReplenishmentStatus = ReplenishmentStatusEnum.Empty;
        }

        public enum AccuracyEnum
        {
            NotSupported,
            Accurate, // The count is expected to be accurate. The notes were previously counted and there have since been no events that might have introduced inaccuracy. 
            AccurateSet, // The count is expected to be accurate. The counts were previously set and there have since been no events that might have introduced inaccuracy.
            Inaccurate, // The count is likely to be inaccurate. A jam, picking fault, or some other event may have resulted in a counting inaccuracy.
            Unknown, // The accuracy of count cannot be determined. This may be due to storage unit insertion or some other hardware event.
        }

        public enum ReplenishmentStatusEnum
        {
            Healthy,
            Full,
            Low,
            High,
            Empty,
        }

        /// <summary>
        /// Assigned by the device class. Will be a unique number which can be used to determine 
        /// usNumber in XFS 3.x migration.This can change as cash storage units are added and removed
        /// from the storage collection.
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// The cash related items which are in the storage unit at the last replenishment.
        /// </summary>
        public StorageCashCountClass InitialCounts
        {
            get { return initialCounts; }
            set { initialCounts = value with { }; }
        }
        private StorageCashCountClass initialCounts = new();

        /// <summary>
        /// The items moved from this storage unit by cash commands to another destination since the last 
        /// replenishment of this unit.
        /// </summary>
        public StorageCashOutCountClass StorageCashOutCount
        {
            get { return storageCashOutCount; }
            set { storageCashOutCount = value with { }; }
        }
        private StorageCashOutCountClass storageCashOutCount = new();

        /// <summary>
        /// List of items inserted in this storage unit by cash commands from another source since the last 
        /// replenishment of this unit.
        /// </summary>
        public StorageCashInCountClass StorageCashInCount
        {
            get { return storageCashInCount; }
            set { storageCashInCount = value with { }; }
        }
        private StorageCashInCountClass storageCashInCount = new();

        /// <summary>
        /// Total count of the items in the unit
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Describes the accuracy of count
        /// </summary>
        public AccuracyEnum Accuracy { get; set; }

        /// <summary>
        /// The state of the media in the unit if it can be determined.
        /// </summary>
        public ReplenishmentStatusEnum ReplenishmentStatus { get; set; }
    }

    /// <summary>
    /// Representing storage counts including L1 notes
    /// </summary>
    public sealed record StorageCashCountClass
    {
        public StorageCashCountClass()
        {
            Unrecognized = 0;
        }

        public StorageCashCountClass(int Unrecognized,
                                     Dictionary<string, CashItemCountClass> ItemCounts)
        {
            this.Unrecognized = Unrecognized;
            if (ItemCounts is not null)
            {
                foreach (var count in ItemCounts)
                {
                    (itemCounts ??= []).Add(count.Key, count.Value with { });
                }
            }
        }

        public StorageCashCountClass(StorageCashCountClass StorageCashCount)
        {
            StorageCashCount.IsNotNull("Copy constructure used with null reference. " + nameof(StorageCashCountClass));
            Unrecognized = StorageCashCount.Unrecognized;
            if (StorageCashCount.ItemCounts is not null)
            {
                foreach (var count in StorageCashCount.ItemCounts)
                {
                    (itemCounts ??= []).Add(count.Key, count.Value with { });
                }
            }
        }

        /// <summary>
        /// Count of unrecognized items handled by the cash interface
        /// </summary>
        public int Unrecognized { get; set; }

        /// <summary>
        /// Counts of cash items broken down by cash item type and classification
        /// </summary>
        public Dictionary<string, CashItemCountClass> ItemCounts
        {
            get { return itemCounts ??= []; }
            set
            {
                (itemCounts ??= []).Clear();
                foreach (var count in value)
                {
                    itemCounts.Add(count.Key, count.Value with { });
                }
            }
        }

        private Dictionary<string, CashItemCountClass> itemCounts = [];

        /// <summary>
        /// Copty structure to the message class generated automatically.
        /// </summary>
        /// <returns></returns>
        public XFS4IoT.CashManagement.StorageCashCountsClass CopyTo()
        {
            Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> counts = [];
            foreach (var count in itemCounts ??= [])
            {
                counts.Add(count.Key, new XFS4IoT.CashManagement.StorageCashCountClass(count.Value.Fit,
                                                                                       count.Value.Unfit,
                                                                                       count.Value.Suspect,
                                                                                       count.Value.Counterfeit,
                                                                                       count.Value.Inked));
            }

            XFS4IoT.CashManagement.StorageCashCountsClass countClass = new(Unrecognized)
            {
                ExtendedProperties = counts
            };
            return countClass;
        }

        /// <summary>
        /// Return total counts of items
        /// </summary>
        public int Total
        {
            get
            {
                int total = Unrecognized;
                foreach (var item in itemCounts ??= [])
                {
                    total += item.Value.Fit;
                    total += item.Value.Unfit;
                    total += item.Value.Suspect;
                    total += item.Value.Counterfeit;
                    total += item.Value.Inked;
                }

                return total;
            }
        }
    }

    /// <summary>
    /// Details of recognised banknote count
    /// </summary>
    [Serializable()]
    public sealed record CashItemCountClass
    {
        public CashItemCountClass()
        {
            Fit = 0;
            Unfit = 0;
            Suspect = 0;
            Counterfeit = 0;
            Inked = 0;
        }

        public CashItemCountClass(int Fit,
                                  int Unfit,
                                  int Suspect,
                                  int Counterfeit,
                                  int Inked)
        {
            this.Fit = Fit;
            this.Unfit = Unfit;
            this.Suspect = Suspect;
            this.Counterfeit = Counterfeit;
            this.Inked = Inked;
        }

        public CashItemCountClass(CashItemCountClass CashItemCount)
        {
            CashItemCount.IsNotNull("Copy constructure used with null reference. " + nameof(CashItemCountClass));
            Fit = CashItemCount.Fit;
            Unfit = CashItemCount.Unfit;
            Suspect = CashItemCount.Suspect;
            Counterfeit = CashItemCount.Counterfeit;
            Inked = CashItemCount.Inked;
        }

        /// <summary>
        /// Count of genuine cash items which are fit for recycling.
        /// </summary>
        public int Fit { get; set; }

        /// <summary>
        /// Count of genuine cash items which are unfit for recycling.
        /// </summary>
        public int Unfit { get; set; }

        /// <summary>
        /// Count of suspected counterfeit cash items.
        /// </summary>
        public int Suspect { get; set; }

        /// <summary>
        /// Count of counterfeit cash items.
        /// </summary>
        public int Counterfeit { get; set; }

        /// <summary>
        /// Count of cash items which have been identified as ink stained.
        /// </summary>
        public int Inked { get; set; }
    }

    /// <summary>
    /// Representing counts moved from the cash unit
    /// </summary>
    public sealed record StorageCashOutCountClass
    {
        public StorageCashOutCountClass()
        {
            Presented = new();
            Rejected = new();
            Distributed = new();
            Unknown = new();
            Stacked = new();
            Diverted = new();
            Transport = new();
        }

        public StorageCashOutCountClass(StorageCashOutCountClass StorageCashOutCount)
        {
            StorageCashOutCount.IsNotNull("Copy constructure used with null reference. " + nameof(StorageCashOutCountClass));
            Presented = new(StorageCashOutCount.Presented);
            Rejected = new(StorageCashOutCount.Rejected);
            Distributed = new(StorageCashOutCount.Distributed);
            Unknown = new(StorageCashOutCount.Unknown);
            Stacked = new(StorageCashOutCount.Stacked);
            Diverted = new(StorageCashOutCount.Diverted);
            Transport = new(StorageCashOutCount.Transport);
        }

        /// <summary>
        /// The items dispensed from this storage unit which are or were customer accessible.
        /// </summary>
        public StorageCashCountClass Presented
        {
            get { return presented ??= new(); }
            set { presented = new(value); }
        }
        private StorageCashCountClass presented = new();

        /// <summary>
        /// The items dispensed from this storage unit which were invalid and were diverted to a reject storage
        /// unit and were not customer accessible during the operation.
        /// </summary>
        public StorageCashCountClass Rejected
        {
            get { return rejected ??= new(); }
            set { rejected = new(value); }
        }
        private StorageCashCountClass rejected = new();

        /// <summary>
        /// The items dispensed from this storage unit which were moved to a storage unit other than a reject storage unit
        /// and were not customer accessible during the operation.
        /// </summary>
        public StorageCashCountClass Distributed
        {
            get { return distributed ??= new(); }
            set { distributed = new(value); }
        }
        private StorageCashCountClass distributed = new();

        /// <summary>
        /// The items dispensed from this storage unit which moved to an unknown position.
        /// </summary>
        public StorageCashCountClass Unknown
        {
            get { return unknown ??= new(); }
            set { unknown = new(value); }
        }
        private StorageCashCountClass unknown = new();

        /// <summary>
        /// The items dispensed from this storage unit which are not customer accessible and are currently stacked
        /// awaiting presentation to the customer.This item list can increase and decrease as items are moved around in the device.
        /// </summary>
        public StorageCashCountClass Stacked
        {
            get { return stacked ??= new(); }
            set { stacked = new(value); }
        }
        private StorageCashCountClass stacked = new();

        /// <summary>
        /// The items dispensed from this storage unit which are not customer accessible and were diverted to a
        /// temporary location due to being invalid and have not yet been deposited in a storage unit.This item
        /// list can increase and decrease as items are moved around in the device.
        /// </summary>
        public StorageCashCountClass Diverted
        {
            get { return diverted ??= new(); }
            set { diverted = new(value); }
        }
        private StorageCashCountClass diverted = new();

        /// <summary>
        /// The items dispensed from this storage unit which are not customer accessible and which have jammed in
        /// the transport. This item list can increase and decrease as items are moved around in the device.
        /// </summary>
        public StorageCashCountClass Transport
        {
            get { return transport ??= new(); }
            set { transport = new(value); }
        }
        private StorageCashCountClass transport = new();
    }

    /// <summary>
    /// Representing count stored in the cash unit
    /// </summary>
    public sealed record StorageCashInCountClass
    {
        public StorageCashInCountClass()
        {
            RetractOperations = 0;
            Deposited = new();
            Retracted = new();
            Rejected = new();
            Distributed = new();
            Transport = new();
        }

        public StorageCashInCountClass(StorageCashInCountClass StorageCashInCount)
        {
            StorageCashInCount.IsNotNull("Copy constructure used with null reference. " + nameof(StorageCashInCountClass));
            RetractOperations = StorageCashInCount.RetractOperations;
            Deposited = new(StorageCashInCount.Deposited);
            Retracted = new(StorageCashInCount.Retracted);
            Rejected = new(StorageCashInCount.Rejected);
            Distributed = new(StorageCashInCount.Distributed);
            Transport = new(StorageCashInCount.Transport);
        }

        /// <summary>
        /// Number of cash retract operations which resulted in items entering this storage unit. This can be 
        /// used where devices do not have the capability to count or validate items after presentation.
        /// </summary>
        public int RetractOperations { get; set; }

        /// <summary>
        /// The items deposited in the storage unit during a Cash In transaction.
        /// </summary>
        public StorageCashCountClass Deposited 
        {
            get { return deposited ??= new(); }
            set { deposited = new(value); }
        }
        private StorageCashCountClass deposited = new();

        /// <summary>
        /// The items deposited in the storage unit after being accessible to a customer. This may be inaccurate 
        /// or not counted if items are not counted or re-validated after presentation, the number of retract
        /// operations is also reported separately in RetractOperations.
        /// </summary>
        public StorageCashCountClass Retracted
        {
            get { return retracted ??= new(); }
            set {  retracted = new(value); }
        }
        private StorageCashCountClass retracted = new();

        /// <summary>
        /// The items deposited in this storage unit originating from another storage unit but rejected due to being 
        /// invalid.This count may be inaccurate due to the nature of rejected items.
        /// </summary>
        public StorageCashCountClass Rejected
        {
            get { return rejected ??= new(); }
            set { rejected = new(value); }
        }
        private StorageCashCountClass rejected = new();

        /// <summary>
        /// The items deposited in this storage unit originating from another storage unit but not rejected.
        /// </summary>
        public StorageCashCountClass Distributed
        {
            get { return distributed ??= new(); }
            set { distributed = new(value); }
        }
        private StorageCashCountClass distributed = new();

        /// <summary>
        /// The items which were intended to be deposited in this storage unit but are not yet deposited. Typical use
        /// case for this property is tracking items after a jam during
        /// CashAcceptor.CashInEnd
        /// </summary>
        public StorageCashCountClass Transport
        {
            get { return transport ??= new(); }
            set { transport = new(value); }
        }
        private StorageCashCountClass transport = new();
    }

    /// <summary>
    /// Cash Unit strcuture the device class supports
    /// </summary>
    [Serializable()]
    public sealed record CashUnit
    {
        public CashUnit(CashCapabilitiesClass Capabilities,
                        CashConfigurationClass Configuration,
                        CashUnitAdditionalInfoClass AdditionalInfo)
        {
            this.Capabilities = Capabilities;
            this.Configuration = Configuration;
            Status = new(AdditionalInfo);
        }

        public CashCapabilitiesClass Capabilities { get; init; }

        public CashConfigurationClass Configuration { get; init; }

        public CashStatusClass Status { get; init; }
    }

    /// <summary>
    /// Structure receiving from the device
    /// </summary>
    public sealed class CashUnitStorageConfiguration(
        string Id,
        string PositionName,
        int Capacity,
        string SerialNumber,
        CashCapabilitiesClass Capabilities,
        CashConfigurationClass Configuration,
        CashUnitAdditionalInfoClass CashUnitAdditionalInfo)
    {

        /// <summary>
        /// An identifier which can be used for cUnitID in CDM/CIM XFS 3.x migration. Not required if not applicable.
        /// </summary>
        public string Id { get; init; } = Id;

        /// <summary>
        /// Fixed physical name for the position.
        /// </summary>
        public string PositionName { get; init; } = PositionName;

        /// <summary>
        /// Fixed physical name for the position.
        /// </summary>
        public int Capacity { get; init; } = Capacity;

        /// <summary>
        /// The storage unit's serial number if it can be read electronically.
        /// </summary>
        public string SerialNumber { get; init; } = SerialNumber;

        /// <summary>
        /// The hardware capabilities of the cash unit
        /// </summary>
        public CashCapabilitiesClass Capabilities { get; init; } = Capabilities;

        /// <summary>
        /// Current configuration set by the device
        /// </summary>
        public CashConfigurationClass Configuration { get; init; } = Configuration;

        /// <summary>
        /// Addtional cash unit information
        /// </summary>
        public CashUnitAdditionalInfoClass CashUnitAdditionalInfo { get; init; } = CashUnitAdditionalInfo;
    }

    /// <summary>
    /// Additional cash unit information device supports
    /// </summary>
    public sealed class CashUnitAdditionalInfoClass(
        int Index,
        bool AccuracySupported)
    {

        /// <summary>
        /// Assigned by the device class. Will be a unique number which can be used to determine 
        /// usNumber in XFS 3.x migration.This can change as cash storage units are added and removed
        /// from the storage collection.
        /// </summary>
        public int Index { get; init; } = Index;

        /// <summary>
        /// Accuracy of count supported or not
        /// </summary>
        public bool AccuracySupported { get; set; } = AccuracySupported;
    }

    /// <summary>
    /// Counts of the cash unit updated by the device class
    /// </summary>
    public sealed record CashUnitCountClass
    {
        public CashUnitCountClass(StorageCashOutCountClass StorageCashOutCount,
                                  StorageCashInCountClass StorageCashInCount,
                                  int Count)
        {
            if (StorageCashOutCount is not null)
                this.StorageCashOutCount = StorageCashOutCount with { };
            if (StorageCashInCount is not null)
                this.StorageCashInCount = StorageCashInCount with { };
            this.Count = Count;
        }

        /// <summary>
        /// The items moved from this storage unit by cash commands to another destination since the last 
        /// replenishment of this unit.
        /// </summary>
        public StorageCashOutCountClass StorageCashOutCount { get; set; } = null;

        /// <summary>
        /// List of items inserted in this storage unit by cash commands from another source since the last 
        /// replenishment of this unit.
        /// </summary>
        public StorageCashInCountClass StorageCashInCount { get; set; } = null;

        /// <summary>
        /// Total count of the items in the unit
        /// </summary>
        public int Count { get; set; }
    }
}
