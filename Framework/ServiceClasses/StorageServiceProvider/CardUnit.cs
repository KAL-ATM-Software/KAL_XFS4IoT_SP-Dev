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
    public sealed record CardUnitStorage
    {
        public enum StatusEnum
        {
            Good,
            Inoperative,
            Missing,
            NotConfigured,
            Manipulated,
        }

        public CardUnitStorage(CardUnitStorageConfiguration StorageConfiguration)
        {
            this.PositionName = StorageConfiguration.PositionName;
            this.Capacity = StorageConfiguration.Capacity;
            this.Status = StatusEnum.NotConfigured;
            this.SerialNumber = StorageConfiguration.SerialNumber;

            this.Unit = new CardUnit(StorageConfiguration.Capabilities,
                                     StorageConfiguration.Configuration);
        }

        /// <summary>
        /// Fixed physical name for the position.
        /// </summary>
        public string PositionName { get; init; }

        /// <summary>
        /// Fixed physical name for the position.
        /// </summary>
        public int Capacity { get; init; }

        /// <summary>
        /// Status of this storage
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// The storage unit's serial number if it can be read electronically.
        /// </summary>
        public string SerialNumber { get; init; }

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
            Retain = 0x0001,
            Dispense = 0x0002,
            Pard = 0x0004,
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
    public sealed record CardStatusClass
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
            this.InitialCount = InitialCount;
            this.Count = Count;
            this.RetainCount = RetainCount;
            this.ReplenishmentStatus = ReplenishmentStatus;
        }

        /// <summary>
        /// The initial number of cards in the storage unit.
        /// </summary>
        public int InitialCount { get; set; }

        /// <summary>
        /// The number of cards in the storage unit.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The number of cards from this storage unit which are in a retain storage unit.
        /// </summary>
        public int RetainCount { get; set; }

        /// <summary>
        /// The state of the cards in the storage unit if it can be determined. 
        /// </summary>
        public ReplenishmentStatusEnum ReplenishmentStatus { get; set; }
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
    public sealed class CardUnitStorageConfiguration
    {
        public CardUnitStorageConfiguration(string PositionName,
                                            int Capacity,
                                            string SerialNumber,
                                            CardCapabilitiesClass Capabilities,
                                            CardConfigurationClass Configuration)
        {
            this.PositionName = PositionName;
            this.Capacity = Capacity;
            this.SerialNumber = SerialNumber;
            this.Capabilities = Capabilities;
            this.Configuration = Configuration;
        }

        /// <summary>
        /// Fixed physical name for the position.
        /// </summary>
        public string PositionName { get; init; }

        /// <summary>
        /// Fixed physical name for the position.
        /// </summary>
        public int Capacity { get; init; }

        /// <summary>
        /// The storage unit's serial number if it can be read electronically.
        /// </summary>
        public string SerialNumber { get; init; }

        public CardCapabilitiesClass Capabilities { get; init; }

        public CardConfigurationClass Configuration { get; init; }
    }

    /// <summary>
    /// Structure to update card unit from the device
    /// </summary>
    public sealed class CardUnitCount
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
            this.RetainCount = 0;
        }

        public int InitialCount { get; init; }
        public int Count { get; init; }
        public int RetainCount { get; init; }
    }
}