/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Events;
using static XFS4IoTFramework.Storage.DepositStatusClass;

namespace XFS4IoTFramework.Storage
{
    /// <summary>
    /// DepositUnitStorage class representing XFS4IoT Storage class strcuture.
    /// This class is used to represent the deposit unit storage for deposit class.
    /// </summary>
    [Serializable()]
    public sealed record DepositUnitStorage : UnitStorageBase
    {
        public DepositUnitStorage(
            DepositUnitStorageConfiguration StorageConfiguration) :
            base(StorageConfiguration.PositionName,
                 StorageConfiguration.Capacity,
                 StatusEnum.NotConfigured,
                 StorageConfiguration.SerialNumber)
        {
            Unit = new DepositUnit(
                StorageConfiguration.Capabilities,
                StorageConfiguration.Configuration);
        }

        /// <summary>
        /// Deposit Unit information
        /// </summary>
        public DepositUnit Unit { get; init; }
    }

    /// <summary>
    /// Capabilities of the deposit unit
    /// </summary>
    [Serializable()]
    public sealed record DepositCapabilitiesClass
    {
        public enum EnvelopeSupplyEnum
        {
            NotSupported,
            Motorized, //Envelope supply can dispense envelopes.
            Manual,    //Envelope supply is manual and must be unlocked to allow envelopes to be taken.
        }

        public DepositCapabilitiesClass(EnvelopeSupplyEnum EnvelpeSupply)
        {
            this.EnvelpeSupply = EnvelpeSupply;
        }

        /// <summary>
        /// Defines what type of envelope supply unit the device has.
        /// </summary>
        public EnvelopeSupplyEnum EnvelpeSupply { get; init; }
    }

    /// <summary>
    /// Configuration of the deposit device
    /// </summary>
    [Serializable()]
    public sealed record DepositConfigurationClass
    {
        public DepositConfigurationClass()
        { }
    }

    /// <summary>
    /// Status of the deposit unit
    /// </summary>
    [Serializable()]
    public sealed record DepositStatusClass : StorageChangedBaseRecord
    {
        public enum DepositoryContainerStatusEnum
        {
            Healthy,     //The deposit container is in a good state.
            High,        //The deposit container is almost full (threshold).
            Full,        //The deposit container is full.
            Inoperative, //The deposit container is inoperative.
            Missing,     //The deposit container is missing.
            Unknown,     //Due to a hardware error or other condition, the state of the deposit container cannot be determined.
            NotSupported,
        }

        public enum EnvelopSupplyStatusEnum
        {
            Healthy, //The envelope supply unit is in a good state (and locked).
            Low,     //The envelope supply unit is present but low.
            Empty,   //The envelope supply unit is present but empty.No envelopes can be dispensed.
            Inoperative, //The envelope supply unit is in an inoperable state.No envelopes can be dispensed.
            Missing,     //The envelope supply unit is missing.
            Unlocked,    //The envelope supply unit is unlocked.
            Unknown,     //Due to a hardware error or other condition, the state of the envelope supply cannot be determined.
            NotSupported,
        }

        public DepositStatusClass(
            int NumberOfDeposits,
            DepositoryContainerStatusEnum DepositoryContainerStatus,
            EnvelopSupplyStatusEnum EnvelopSupplyStatus)
        {
            this.NumberOfDeposits = NumberOfDeposits;
            depositoryContainerStatus = DepositoryContainerStatus;
            envelopSupplyStatus = EnvelopSupplyStatus;
        }

        /// <summary>
        /// The number of envelopes or bags in the deposit container.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.CountChanged)]
        public int NumberOfDeposits
        {
            get { return numberOfDeposits; }
            set
            {
                if (numberOfDeposits != value)
                {
                    numberOfDeposits = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private int numberOfDeposits;

        /// <summary>
        /// The state of the deposit container that contains the deposited envelopes or bags.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public DepositoryContainerStatusEnum DepositoryContainerStatus
        {
            get { return depositoryContainerStatus; }
            set
            {
                if (depositoryContainerStatus != value)
                {
                    depositoryContainerStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private DepositoryContainerStatusEnum depositoryContainerStatus;

        /// <summary>
        /// The state of the envelope supply unit.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public EnvelopSupplyStatusEnum EnvelopSupplyStatus
        {
            get { return envelopSupplyStatus; }
            set
            {
                if (envelopSupplyStatus != value)
                {
                    envelopSupplyStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private EnvelopSupplyStatusEnum envelopSupplyStatus;
    }

    /// <summary>
    /// The Printer Unit strcuture supports for the device class
    /// </summary>
    [Serializable()]
    public sealed record DepositUnit
    {
        public DepositUnit(
            DepositCapabilitiesClass Capabilities,
            DepositConfigurationClass Configuration)
        {
            this.Capabilities = Capabilities;
            this.Configuration = Configuration;
            this.Status = new DepositStatusClass(
                0, 
                DepositStatusClass.DepositoryContainerStatusEnum.Healthy, 
                DepositStatusClass.EnvelopSupplyStatusEnum.Healthy);
        }

        public DepositCapabilitiesClass Capabilities { get; init; }

        public DepositConfigurationClass Configuration { get; init; }

        public DepositStatusClass Status { get; init; }
    }

    /// <summary>
    /// Structure receiving from the device
    /// </summary>
    public sealed class DepositUnitStorageConfiguration(
        string PositionName,
        int Capacity,
        string SerialNumber,
        DepositCapabilitiesClass Capabilities,
        DepositConfigurationClass Configuration = null)
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

        public DepositCapabilitiesClass Capabilities { get; init; } = Capabilities;

        public DepositConfigurationClass Configuration { get; init; } = Configuration;
    }

    /// <summary>
    /// Structure to update deposit unit information from the device
    /// </summary>
    public sealed record DepositUnitInfo
    {
        public DepositUnitInfo(
            int NumberOfDeposits,
            DepositUnitStorage.StatusEnum StorageStatus,
            DepositoryContainerStatusEnum DepositoryContainerStatus = DepositoryContainerStatusEnum.NotSupported,
            EnvelopSupplyStatusEnum EnvelopSupplyStatus = EnvelopSupplyStatusEnum.NotSupported)
        {
            this.StorageStatus = StorageStatus;
            this.NumberOfDeposits = NumberOfDeposits;
            this.DepositoryContainerStatus = DepositoryContainerStatus;
            this.EnvelopSupplyStatus = EnvelopSupplyStatus;
        }

        /// <summary>
        /// Number of media in the storage.
        /// </summary>
        public int NumberOfDeposits { get; init; }

        /// <summary>
        /// Set status of storage
        /// </summary>
        public DepositUnitStorage.StatusEnum StorageStatus { get; init; }

        /// <summary>
        /// The state of the deposit container that contains the deposited envelopes or bags.
        /// null if the device doesn't support
        /// </summary>
        public DepositoryContainerStatusEnum DepositoryContainerStatus { get; init; }

        /// <summary>
        /// The state of the envelope supply unit.
        /// null if the device doesn't support
        /// </summary>
        public EnvelopSupplyStatusEnum EnvelopSupplyStatus { get; init; }
    }
}