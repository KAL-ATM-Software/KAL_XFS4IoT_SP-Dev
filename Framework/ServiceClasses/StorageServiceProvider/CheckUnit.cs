﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Commands;

namespace XFS4IoTFramework.Storage
{
    [Serializable()]
    public sealed record CheckUnitStorage : UnitStorageBase
    {
        public CheckUnitStorage(CheckUnitStorageConfiguration StorageConfiguration) :
            base(StorageConfiguration.PositionName,
                 StorageConfiguration.Capacity,
                 StatusEnum.NotConfigured,
                 StorageConfiguration.SerialNumber)
        {
            Unit = new CheckUnit(StorageConfiguration.Capabilities,
                                 StorageConfiguration.Configuration);
        }

        /// <summary>
        /// Check Unit information
        /// </summary>
        public CheckUnit Unit { get; init; }
    }

    /// <summary>
    /// Capabilities of the check unit
    /// </summary>
    [Serializable()]
    public sealed record CheckCapabilitiesClass
    {
        [Flags]
        public enum TypesEnum
        {
            MediaIn = 1 << 0,
            Retract = 1 << 1,
        }

        public enum SensorEnum
        {
            None = 0,
            Empty = 1 << 0,
            High = 1 << 1,
            Full = 1 << 2,
        }

        public CheckCapabilitiesClass(TypesEnum Types,
                                      SensorEnum Sensors)
        {
            this.Types = Types;
            this.Sensors = Sensors;
        }
        public CheckCapabilitiesClass(CheckCapabilitiesClass Capabilities)
        {
            Types = Capabilities.Types;
            Sensors = Capabilities.Sensors;
        }

        /// <summary>
        /// The types of operation the unit is capable of or configured to perform.
        /// This is a combination of one or more operations.
        /// </summary>
        public TypesEnum Types { get; init; }

        /// <summary>
        ///  The types of sensor the unit has.
        /// </summary>
        public SensorEnum Sensors { get; init; } 
    }

    /// <summary>
    /// Configuration of the check unit
    /// </summary>
    [Serializable()]
    public sealed record CheckConfigurationClass
    {
        public CheckConfigurationClass(CheckCapabilitiesClass.TypesEnum Types,
                                       string Id,
                                       int HighThreshold,
                                       int RetractHighThreshold)
        {
            this.Types = Types;
            this.Id = Id;
            this.HighThreshold = HighThreshold;
            this.RetractHighThreshold = RetractHighThreshold;
            this.HighThreshold = HighThreshold;
        }
        public CheckConfigurationClass(CheckConfigurationClass Configuration)
        {
            Types = Configuration.Types;
            Id = Configuration.Id;
            HighThreshold = Configuration.HighThreshold;
            RetractHighThreshold = Configuration.RetractHighThreshold;
            HighThreshold = Configuration.HighThreshold;
        }
        /// <summary>
        /// The types of operation the unit is capable of or configured to perform. 
        /// This is a combination of one or  more operations.
        /// </summary>
        public CheckCapabilitiesClass.TypesEnum Types { get; set; }

        /// <summary>
        /// An application defined Storage Unit Identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// If specified is greater than zero, ReplenishmentStatus is set to High if the total number of items
        /// in the storage unit is greater than this number.
        /// </summary>
        public int HighThreshold { get; set; }

        /// <summary>
        /// If specified greater than zero and the storage unit is configured as Retract,
        /// ReplenishmentStatus is set to High if the total number of retract operations 
        /// in the storage unit is greater than this number.
        /// </summary>
        public int RetractHighThreshold { get; set; }
    }

    /// <summary>
    /// Status of the cash unit
    /// </summary>
    [Serializable()]
    public sealed record CheckStatusClass : StorageChangedBaseRecord
    {
        public enum ReplenishmentStatusEnum
        {
            Healthy,
            Full,
            High,
            Empty,
        }

        public CheckStatusClass()
        {
            Index = 0;
            replenishmentStatus = ReplenishmentStatusEnum.Healthy;
            InitialCounts = new()
            {
                ParentPropertyName = nameof(InitialCounts)
            };
            CheckInCounts = new()
            {
                ParentPropertyName = nameof(CheckInCounts)
            };
        }
        public CheckStatusClass(int Index)
        {
            this.Index = Index;
            replenishmentStatus = ReplenishmentStatusEnum.Healthy;
            InitialCounts = new()
            {
                ParentPropertyName = nameof(InitialCounts)
            };
            CheckInCounts = new()
            {
                ParentPropertyName = nameof(CheckInCounts)
            };
        }
        public CheckStatusClass(CheckStatusClass Status) :
            base(Status)
        {
            Index = Status.Index;
            replenishmentStatus = Status.ReplenishmentStatus;
            InitialCounts = Status.InitialCounts;
            CheckInCounts = Status.CheckInCounts;
        }

        /// <summary>
        /// Assigned by the Service. Will be a unique number which can be used to determine
        /// usBinNumber* in XFS 3.x migration.This can change as storage units are added and removed
        /// from the storage collection.
        /// if this property is not used, value zero is set as XFS 3.x is 1 based.
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// The check related items which are in the storage unit at the last replenishment.
        /// </summary>
        public StorageCheckCountClass InitialCounts { get; init; }
        /// <summary>
        /// The check items added to the unit since the last replenishment.
        /// </summary>
        public StorageCheckCountClass CheckInCounts { get; init; }

        /// <summary>
        /// The state of the media in the unit if it can be determined.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public ReplenishmentStatusEnum ReplenishmentStatus
        {
            get { return replenishmentStatus; }
            set
            {
                if (replenishmentStatus != value)
                {
                    replenishmentStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ReplenishmentStatusEnum replenishmentStatus;
    }

    /// <summary>
    /// Representing storage counts for check items
    /// </summary>
    [Serializable()]
    public sealed record StorageCheckCountClass : StorageChangedBaseRecord
    {
        public StorageCheckCountClass()
        {
            mediaInCount = 0;
            count = 0;
            retractOperations = 0;
        }
        public StorageCheckCountClass(int MediaInCount,
                                      int Count,
                                      int RetractOperations)
        {
            mediaInCount = MediaInCount;
            count = Count;
            retractOperations = RetractOperations;
        }
        public StorageCheckCountClass(StorageCheckCountClass StorageCheckCount) :
            base(StorageCheckCount)
        {
            StorageCheckCount.IsNotNull("Copy constructure used with null reference. " + nameof(StorageCheckCountClass));
            mediaInCount = StorageCheckCount.MediaInCount;
            count = StorageCheckCount.Count;
            retractOperations = StorageCheckCount.RetractOperations;
        }

        /// <summary>
        /// Count of items added to the storage unit due to Check operations. 
        /// If the number of items is not counted this is not reported and RetractOperations
        /// is incremented as items are added to the unit.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.CountChanged)]
        public int MediaInCount
        {
            get { return mediaInCount; }
            set
            {
                if (mediaInCount != value)
                {
                    mediaInCount = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int mediaInCount;

        /// <summary>
        /// Total number of items added to the storage unit due to any operations. 
        /// If the number of items is not counted this is not reported and RetractOperations is 
        /// incremented as items are added to the unit.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.CountChanged)]
        public int Count
        {
            get { return count; }
            set
            {
                if (count != value)
                {
                    count = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int count;

        /// <summary>
        /// Total number of operations which resulted in items being retracted to the storage unit.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public int RetractOperations
        {
            get { return mediaInCount; }
            set
            {
                if (retractOperations != value)
                {
                    retractOperations = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int retractOperations;
    }

    /// <summary>
    /// Check Unit strcuture the device class supports
    /// </summary>
    [Serializable()]
    public sealed record CheckUnit
    {
        public CheckUnit(CheckCapabilitiesClass Capabilities,
                         CheckConfigurationClass Configuration)
        {
            this.Capabilities = Capabilities;
            this.Configuration = Configuration;
            Status = new();
        }

        public CheckCapabilitiesClass Capabilities { get; init; }

        public CheckConfigurationClass Configuration { get; init; }

        public CheckStatusClass Status { get; init; }
    }

    /// <summary>
    /// Structure receiving from the device
    /// </summary>
    public sealed class CheckUnitStorageConfiguration(
        string PositionName,
        int Capacity,
        string SerialNumber,
        CheckCapabilitiesClass Capabilities,
        CheckConfigurationClass Configuration)
    {

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
        /// The hardware capabilities of the check unit.
        /// </summary>
        public CheckCapabilitiesClass Capabilities { get; init; } = Capabilities;

        /// <summary>
        /// Current configuration for check unit is set by the device.
        /// </summary>
        public CheckConfigurationClass Configuration { get; init; } = Configuration;
    }

    /// <summary>
    /// Counts of the check unit updated by the device class
    /// </summary>
    public sealed record CheckUnitCountClass
    {
        public CheckUnitCountClass(
            int Count,
            bool MediaRetracted = false)
        {
            this.Count = Count;
            this.MediaRetracted = MediaRetracted;
        }

        /// <summary>
        /// Total number of items added in the single operation
        /// </summary>
        public int Count { get; init; }

        /// <summary>
        /// True if media is retracted in the retract bin, stacker, rebuncher.
        /// </summary>
        public bool MediaRetracted { get; init; }
    }
}
