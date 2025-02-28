/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Events;

namespace XFS4IoTFramework.Storage
{
    /// <summary>
    /// PrinterUnitStorage class representing XFS4IoT Storage class strcuture.
    /// This class is used to represent the printer unit storage for printer class.
    /// </summary>
    [Serializable()]
    public sealed record PrinterUnitStorage : UnitStorageBase
    {
        public PrinterUnitStorage(
            int Index,
            PrinterUnitStorageConfiguration StorageConfiguration) :
            base(StorageConfiguration.PositionName,
                 StorageConfiguration.Capacity,
                 StatusEnum.NotConfigured,
                 StorageConfiguration.SerialNumber)
        {
            Unit = new PrinterUnit(
                Index,
                StorageConfiguration.Capabilities,
                StorageConfiguration.Configuration);
        }

        /// <summary>
        /// Printer Unit information
        /// </summary>
        public PrinterUnit Unit { get; init; }
    }

    /// <summary>
    /// Capabilities of the printer unit
    /// </summary>
    [Serializable()]
    public sealed record PrinterCapabilitiesClass
    {
        public PrinterCapabilitiesClass(int MaxRetracts)
        {
            this.MaxRetracts = MaxRetracts;
        }

        /// <summary>
        /// Indicates the storage unit capabilities.
        /// </summary>
        public int MaxRetracts { get; init; }
    }

    /// <summary>
    /// Configuration of the printer retract Unit or passbook unit
    /// </summary>
    [Serializable()]
    public sealed record PrinterConfigurationClass
    {
        public PrinterConfigurationClass()
        { }
    }

    /// <summary>
    /// Status of the printer retract unit or passbook unit
    /// </summary>
    [Serializable()]
    public sealed record PrinterStatusClass : StorageChangedBaseRecord
    {
        public enum ReplenishmentStatusEnum
        {
            Healthy,
            Full,
            High,
            Unknown,
        }

        public PrinterStatusClass(
            int Index,
            int InitialCount,
            int InCount,
            ReplenishmentStatusEnum ReplenishmentStatus)
        {
            this.Index = Index;
            initialCount = InitialCount;
            inCount = InCount;
            replenishmentStatus = ReplenishmentStatus;
        }

        /// <summary>
        /// This property will be a unique number which can be used to determine
        /// the index of the retract bin in XFS 3.x migration.
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// The printer related count as set at the last replenishment.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.CountChanged)]
        public int InitialCount
        {
            get { return initialCount; }
            set
            {
                if (initialCount != value)
                {
                    initialCount = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int initialCount;

        /// <summary>
        /// The printer related items added to the unit since the last replenishment.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.CountChanged)]
        public int InCount
        {
            get { return inCount; }
            set
            {
                if (inCount != value)
                {
                    inCount = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int inCount;

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
    /// The Printer Unit strcuture supports for the device class
    /// </summary>
    [Serializable()]
    public sealed record PrinterUnit
    {
        public PrinterUnit(
            int index,
            PrinterCapabilitiesClass Capabilities,
            PrinterConfigurationClass Configuration)
        {
            this.Capabilities = Capabilities;
            this.Configuration = Configuration;
            this.Status = new PrinterStatusClass(index, 0, 0, PrinterStatusClass.ReplenishmentStatusEnum.Unknown);
        }

        public PrinterCapabilitiesClass Capabilities { get; init; }

        public PrinterConfigurationClass Configuration { get; init; }

        public PrinterStatusClass Status { get; init; }
    }

    /// <summary>
    /// Structure receiving from the device
    /// </summary>
    public sealed class PrinterUnitStorageConfiguration(
        string PositionName,
        int Capacity,
        string SerialNumber,
        PrinterCapabilitiesClass Capabilities,
        PrinterConfigurationClass Configuration = null)
    {
        /// <summary>
        /// Fixed physical name for the position.
        /// </summary>
        public string PositionName { get; init; } = PositionName;

        /// <summary>
        /// Capacity of the position.
        /// </summary>
        public int Capacity { get; init; } = Capacity;

        /// <summary>
        /// The storage unit's serial number if it can be read electronically.
        /// </summary>
        public string SerialNumber { get; init; } = SerialNumber;

        public PrinterCapabilitiesClass Capabilities { get; init; } = Capabilities;

        public PrinterConfigurationClass Configuration { get; init; } = Configuration;
    }

    /// <summary>
    /// Structure to update printer unit from the device
    /// </summary>
    public sealed record PrinterUnitCount
    {
        public PrinterUnitCount(
            int InitialCount,
            int InCount)
        {
            this.InitialCount = InitialCount;
            this.InCount = InCount;
        }

        public int InitialCount { get; init; }
        public int InCount { get; init; }
    }
}