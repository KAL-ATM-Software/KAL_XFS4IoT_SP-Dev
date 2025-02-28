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

namespace XFS4IoTFramework.Storage
{
    /// <summary>
    /// CardUnitStorage class representing XFS4IoT Storage class strcuture
    /// </summary>
    [Serializable()]
    public sealed record CardUnitStorage : UnitStorageBase
    {
        public CardUnitStorage(CardUnitStorageConfiguration StorageConfiguration) :
            base(StorageConfiguration.PositionName,
                 StorageConfiguration.Capacity,
                 StatusEnum.NotConfigured,
                 StorageConfiguration.SerialNumber)
        {
            Unit = new CardUnit(StorageConfiguration.Capabilities,
                                StorageConfiguration.Configuration);
        }

        /// <summary>
        /// Card Unit information
        /// </summary>
        public CardUnit Unit { get; init; }
    }

    /// <summary>
    /// Capabilities of the card unit
    /// </summary>
    [Serializable()]
    public sealed record CardCapabilitiesClass
    {
        [Flags]
        public enum TypeEnum
        {
            Retain = 1 << 0,
            Dispense = 1 << 1,
            Park = 1 << 2,
        }

        public CardCapabilitiesClass(TypeEnum Type, 
                                     bool HardwareSensors)
        {
            this.Type = Type;
            this.HardwareSensors = HardwareSensors;
        }

        /// <summary>
        /// The type of card storage
        /// </summary>
        public TypeEnum Type { get; init; }

        /// <summary>
        /// The storage unit has hardware sensors that can detect threshold states.
        /// </summary>
        public bool HardwareSensors { get; init; }
    }

    /// <summary>
    /// Configuration of the card Unit
    /// </summary>
    [Serializable()]
    public sealed record CardConfigurationClass
    {
        public CardConfigurationClass(int Threshold, 
                                      string CardId = null)
        {
            this.CardId = CardId;
            this.Threshold = Threshold;
        }

        /// <summary>
        /// The identifier that may be used to identify the type of cards in the storage unit.
        /// </summary>
        public string CardId { get; set; }

        /// <summary>
        /// If the threshold value is non zero, hardware sensors in the storage unit do not trigger Storage.StorageThresholdEvent events.
        /// </summary>
        public int Threshold { get; set; }
    }

    /// <summary>
    /// Status of the card unit
    /// </summary>
    [Serializable()]
    public sealed record CardStatusClass : StorageChangedBaseRecord
    {
        public enum ReplenishmentStatusEnum
        {
            Healthy,
            Full,
            Low,
            High,
            Empty,
        }

        public CardStatusClass(int InitialCount, 
                               int Count, 
                               int RetainCount, 
                               ReplenishmentStatusEnum ReplenishmentStatus)
        {
            initialCount = InitialCount;
            count = Count;
            retainCount = RetainCount;
            replenishmentStatus = ReplenishmentStatus;
        }

        /// <summary>
        /// The initial number of cards in the storage unit.
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
        /// The number of cards in the storage unit.
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
        /// The number of cards from this storage unit which are in a retain storage unit.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.CountChanged)]
        public int RetainCount
        {
            get { return retainCount; }
            set
            {
                if (retainCount != value)
                {
                    retainCount = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int retainCount;

        /// <summary>
        /// The state of the cards in the storage unit if it can be determined. 
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
    /// Card Unit strcuture the device class supports
    /// </summary>
    [Serializable()]
    public sealed record CardUnit
    {
        public CardUnit(CardCapabilitiesClass Capabilities,
                        CardConfigurationClass Configuration)
        {
            this.Capabilities = Capabilities;
            this.Configuration = Configuration;
            this.Status = new CardStatusClass(0, 0, 0, CardStatusClass.ReplenishmentStatusEnum.Empty);
        }

        public CardCapabilitiesClass Capabilities { get; init; }

        public CardConfigurationClass Configuration { get; init; }

        public CardStatusClass Status { get; init; }
    }

    /// <summary>
    /// Structure receiving from the device
    /// </summary>
    public sealed class CardUnitStorageConfiguration(string PositionName,
                                        int Capacity,
                                        string SerialNumber,
                                        CardCapabilitiesClass Capabilities,
                                        CardConfigurationClass Configuration)
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

        public CardCapabilitiesClass Capabilities { get; init; } = Capabilities;

        public CardConfigurationClass Configuration { get; init; } = Configuration;
    }

    /// <summary>
    /// Structure to update card unit from the device
    /// </summary>
    public sealed record CardUnitCount
    {
        public CardUnitCount(int InitialCount,
                             int Count,
                             int RetainCount)
        {
            this.InitialCount = InitialCount;
            this.Count = Count;
            this.RetainCount = RetainCount;
        }

        public CardUnitCount(int InitialCount,
                             int Count)
        {
            this.InitialCount = InitialCount;
            this.Count = Count;
            // Only used for dispensing unit
            RetainCount = 0;
        }

        public int InitialCount { get; init; }
        public int Count { get; init; }
        public int RetainCount { get; init; }
    }
}