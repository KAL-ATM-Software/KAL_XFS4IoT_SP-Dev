/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Storage
{
    /// <summary>
    /// IBNSUnitStorage class representing XFS4IoT Storage class strcuture
    /// </summary>
    [Serializable()]
    public sealed record IBNSUnitStorage : UnitStorageBase
    {
        public IBNSUnitStorage(
            IBNSUnitStorageConfiguration StorageConfiguration,
            IBNSStatusClass Status) :
            base(StorageConfiguration.PositionName,
                 StorageConfiguration.Capacity,
                 StatusEnum.NotConfigured,
                 StorageConfiguration.SerialNumber)
        {
            Unit = new IBNSUnit(StorageConfiguration.Capabilities,
                                StorageConfiguration.Configuration,
                                ref Status);
        }

        /// <summary>
        /// IBNS Unit information
        /// </summary>
        public IBNSUnit Unit { get; init; }
    }

    /// <summary>
    /// Capabilities of the IBNS unit
    /// </summary>
    [Serializable()]
    public sealed record IBNSCapabilitiesClass
    {
        public IBNSCapabilitiesClass()
        { }
    }

    /// <summary>
    /// Configuration of the IBNS Unit
    /// </summary>
    [Serializable()]
    public sealed record IBNSConfigurationClass
    {
        public IBNSConfigurationClass()
        { }
    }

    /// <summary>
    /// Status of the IBNS unit
    /// </summary>
    [Serializable()]
    public sealed class IBNSStatusClass : StorageChangedBaseClass
    {
        public enum ProtectionEnum
        {
            Fault,                   //A system fault occurred.
            Armed,                   //The protection is armed.
            Disarmed,                //The protection is disarmed.
            NeutralizationTriggered, //The neutralization trigger occurred.
        }

        public enum WarningEnum
        {
            CassetteRunsAutonomously, //The protection is armed but the banknote neutralization of a Storage Unit runs in an autonomous mode.
            Alarm,                    //The protection is armed but in alarm mode.
            NotSupported,             //The protection is not supported.
        }

        public enum LidStatusEnum
        {
            Fault,    //A fault is detected in the sensor or due to a hardware error or other condition, the status cannot be determined.
            Opened,   //The lid is opened.
            Closed,   //Too lid is closed.
            Disabled, //The banknote neutralization disabled this sensor for malfunction or by configuration.
            NotSupported, //The sensor is not supported.
        }

        public enum NeutralizationTriggerEnum
        {
            Initializing, //The neutralization trigger is being initialized. It may take a few seconds before the ready state.
            Ready,        //The neutralization is ready to trigger on demand.
            Disabled,     //The neutralization trigger is inhibited and disabled.
            Fault,        //A fault is detected in the neutralization trigger or due to a hardware error or other condition, the status cannot be determined.
            NotSupported, //The neutralization trigger is not supported.
        }

        public IBNSStatusClass(
            string Identifier,
            string StorageUnitIdentifier,
            ProtectionEnum Protection,
            WarningEnum Warning = WarningEnum.NotSupported,
            PowerInfoClass PowerInfo = null,
            XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum TiltState = XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum.NotSupported,
            XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum TemperatureState = XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum.NotSupported,
            LidStatusEnum LidStatus = LidStatusEnum.NotSupported,
            NeutralizationTriggerEnum NeutralizationTrigger = NeutralizationTriggerEnum.NotSupported)
        {
            this.Identifier = Identifier;
            this.StorageUnitIdentifier = StorageUnitIdentifier;
            protection = Protection;
            this.Warning = Warning;
            this.PowerInfo = PowerInfo;
            this.TiltState = TiltState;
            this.TemperatureState = TemperatureState;
            this.LidStatus = LidStatus;
            this.NeutralizationTrigger = NeutralizationTrigger;
        }

        /// <summary>
        /// Indicates the identifier assigned to the banknote neutralization of a Storage Unit by the vendor of the banknote neutralization.
        /// </summary>
        public string Identifier { get; init; }

        /// <summary>
        /// If the Storage Unit Identifier can be written at installation or production time, 
        /// this property returns the Storage Unit Identifier to which this BanknoteNeutralization is linked.
        /// Usually it's a cash unit identifier.
        /// </summary>
        public string StorageUnitIdentifier { get; init; }

        /// <summary>
        /// Indicates the identifier assigned to the banknote neutralization of a Storage Unit by the vendor of the banknote neutralization.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public ProtectionEnum Protection
        {
            get { return protection; }
            set
            {
                if (protection != value)
                {
                    protection = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ProtectionEnum protection;

        /// <summary>
        /// Gives additional information that requires attention.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public WarningEnum Warning
        {
            get { return warning; }
            set
            {
                if (warning != value)
                {
                    warning = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private WarningEnum warning;

        /// <summary>
        /// Information that can be generically applied to module providing power ranging from a simple non-rechargeable 
        /// battery to a more complex device such as a UPS.
        /// </summary>
        public PowerInfoClass PowerInfo { get; init; }

        /// <summary>
        /// The tilt state.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum TiltState
        {
            get { return tiltState; }
            set
            {
                if (tiltState != value)
                {
                    tiltState = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private XFS4IoTFramework.Common.IBNSStatusClass.TiltStateEnum tiltState;

        /// <summary>
        /// The temperature sensing state.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum TemperatureState
        {
            get { return temperatureState; }
            set
            {
                if (temperatureState != value)
                {
                    temperatureState = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private XFS4IoTFramework.Common.IBNSStatusClass.TemperatureStateEnum temperatureState;

        /// <summary>
        /// The state of the lid. 
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public LidStatusEnum LidStatus
        {
            get { return lidStatus; }
            set
            {
                if (lidStatus != value)
                {
                    lidStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private LidStatusEnum lidStatus;

        /// <summary>
        /// The state of the neutralization trigger.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public NeutralizationTriggerEnum NeutralizationTrigger
        {
            get { return neutralizationTrigger; }
            set
            {
                if (neutralizationTrigger != value)
                {
                    neutralizationTrigger = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private NeutralizationTriggerEnum neutralizationTrigger;
    }

    /// <summary>
    /// IBNS Unit strcuture the device class supports
    /// </summary>
    [Serializable()]
    public sealed record IBNSUnit
    {
        public IBNSUnit(IBNSCapabilitiesClass Capabilities,
                        IBNSConfigurationClass Configuration,
                        ref IBNSStatusClass Status)
        {
            this.Capabilities = Capabilities;
            this.Configuration = Configuration;
            this.Status = Status;
        }

        public IBNSCapabilitiesClass Capabilities { get; init; }

        public IBNSConfigurationClass Configuration { get; init; }

        public IBNSStatusClass Status { get; init; }
    }

    public sealed class PowerInfoClass(
            PowerInfoClass.PoweringStatusEnum PowerInStatus,
            PowerInfoClass.PoweringStatusEnum PowerOutStatus,
            PowerInfoClass.BatteryStatusEnum BatteryStatus = PowerInfoClass.BatteryStatusEnum.NotSupported,
            PowerInfoClass.BatteryChargingStatusEnum BatteryChargingStatus = PowerInfoClass.BatteryChargingStatusEnum.NotSupported
            ) : StorageChangedBaseClass
    {
        public enum PoweringStatusEnum
        {
            Powering,
            NotPower,
        }

        public enum BatteryStatusEnum
        {
            Full,
            Low,
            Operational,
            Critical,
            Failure,
            NotSupported,
        }

        public enum BatteryChargingStatusEnum
        {
            Charging,
            Discharging,
            NotCharging,
            NotSupported,
        }

        /// <summary>
        /// Specify the input power or mains power status. Specified as one of the following:
        /// * ```Powering``` - The input power source is live and supplying power to the power supply module.
        /// * ```NoPower``` - The input power source is not supplying power to the power supply module.
        /// * ```NotSupported``` - The input power source is not supported.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public PoweringStatusEnum PowerInStatus
        {
            get { return powerInStatus; }
            set
            {
                if (powerInStatus != value)
                {
                    powerInStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private PoweringStatusEnum powerInStatus = PowerInStatus;

        /// <summary>
        /// Specify the output power status. Specified as one of the following:
        /// * ```Powering``` - The input power source is live and supplying power to the power supply module.
        /// * ```NoPower``` - The input power source is not supplying power to the power supply module.
        /// * ```NotSupported``` - The input power source is not supported.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public PoweringStatusEnum PowerOutStatus
        {
            get { return powerOutStatus; }
            set
            {
                if (powerOutStatus != value)
                {
                    powerOutStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private PoweringStatusEnum powerOutStatus = PowerOutStatus;

        /// <summary>
        /// The charge level of the battery. Specified as one of the following:
        /// * ```Full``` - The battery charge level is full, either the battery is new or fully charged for a rechargeable battery.
        /// * ```Low``` - Although the battery level is still operational, this is an advance notice which should trigger a maintenance schedule without delay.
        /// * ```Operational``` - The charge level is nominally between the levels "full" and "low".
        /// * ```Critical``` - The battery level is no longer operational, this is an alert which should trigger maintenance without delay.Consider that the device may also not be powered properly.
        /// * ```Failure``` - A battery fault detected. The device powered by the battery is no longer powered properly. Immediate maintenance should be performed.This may be a failure from the battery charging module for a rechargeable battery.
        /// * ```NotSupported``` - The device does not support battery status.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public BatteryStatusEnum BatteryStatus
        {
            get { return batteryStatus; }
            set
            {
                if (batteryStatus != value)
                {
                    batteryStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private BatteryStatusEnum batteryStatus = BatteryStatus;

        /// <summary>
        /// The charging status of the battery. This will be null if the battery is not rechargeable. Specified as one of the following:
        /// * ```Charging``` - The battery is charging power.When the battery is fully charged, the state changes to "notCharging" and the property BatteryStatus reports "full".   
        /// * ```Discharging``` - The battery is discharging power.
        /// * ```NotCharging``` - The battery is not charging power.
        /// * ```NotSupported``` - The device does not support battery charging status.
        /// </summary>
        [Event(Type = EventAttribute.EventTypeEnum.StorageChanged)]
        public BatteryChargingStatusEnum BatteryChargingStatus
        {
            get { return batteryChargingStatus; }
            set
            {
                if (batteryChargingStatus != value)
                {
                    batteryChargingStatus = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private BatteryChargingStatusEnum batteryChargingStatus = BatteryChargingStatus;
    }

    /// <summary>
    /// Structure receiving from the device
    /// </summary>
    public sealed class IBNSUnitStorageConfiguration(string PositionName,
        int Capacity,
        string SerialNumber,
        IBNSCapabilitiesClass Capabilities = null,
        IBNSConfigurationClass Configuration = null)
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

        public IBNSCapabilitiesClass Capabilities { get; init; } = Capabilities;

        public IBNSConfigurationClass Configuration { get; init; } = Configuration;
    }

    /// <summary>
    /// This class represents the storage information of IBNS from the device class
    /// </summary>
    public sealed class IBNSStorageInfo(IBNSUnitStorageConfiguration StorageConfiguration, IBNSStatusClass StorageStatus)
    {
        public IBNSUnitStorageConfiguration StorageConfiguration { get; init; } = StorageConfiguration;

        public IBNSStatusClass StorageStatus { get; init; } = StorageStatus;
    }
}