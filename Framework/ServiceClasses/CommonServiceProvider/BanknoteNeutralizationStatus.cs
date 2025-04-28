/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace XFS4IoTFramework.Common
{
    public sealed class IBNSStatusClass(
        IBNSStatusClass.StateClass State,
        IBNSStatusClass.WarningStateClass WarningState,
        IBNSStatusClass.ErrorStateClass ErrorState,
        IBNSStatusClass.SafeDoorStateEnum SafeDoorState = IBNSStatusClass.SafeDoorStateEnum.NotSupported,
        IBNSStatusClass.SafeBoltStateEnum SafeBoltState = IBNSStatusClass.SafeBoltStateEnum.NotSupported,
        IBNSStatusClass.LightStateEnum LightState = IBNSStatusClass.LightStateEnum.NotSupported,
        IBNSStatusClass.TiltStateEnum TiltState = IBNSStatusClass.TiltStateEnum.NotSupported,
        IBNSStatusClass.GasStateEnum GasState = IBNSStatusClass.GasStateEnum.NotSupported,
        IBNSStatusClass.TemperatureStateEnum TemperatureState = IBNSStatusClass.TemperatureStateEnum.NotSupported,
        IBNSStatusClass.SeismicStateEnum SeismicState = IBNSStatusClass.SeismicStateEnum.NotSupported,
        Dictionary<IBNSCapabilitiesClass.CustomInputEnum, IBNSStatusClass.CustomInputStatusClass> CustomInputStatus = null,
        Dictionary<string, IBNSStatusClass.CustomInputStatusClass> VendorSpecificCustomInputStatus = null,
        PowerManagementStatusClass.PowerInfoClass PowerInfo = null) : StatusBase
    {
        public sealed class StateClass(StateClass.ModeEnum Mode,
            StateClass.SubModeEnum SubMode = StateClass.SubModeEnum.NotSupported) : StatusBase
        {
            public enum ModeEnum
            {
                Fault,
                Armed,
                Disarmed,
                NeutralizationTriggered,
            }

            public enum SubModeEnum
            {
                AllSafeSensorsIgnored,
                ArmPending,
                NotSupported,
            }

            /// <summary>
            ///  The state of the banknote neutralization.
            ///  * ```fault``` - A system fault occurred or due to a hardware error or other condition, the status cannot be determined.
            ///  * ```armed``` - The protection is now armed.
            ///  * ```disarmed``` - The protection is now disarmed.
            ///  * ```neutralizationTriggered``` - The neutralization trigger occurred.
            /// </summary>
            public ModeEnum Mode
            {
                get { return mode; }
                set
                {
                    if (mode != value)
                    {
                        mode = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private ModeEnum mode = Mode;

            /// <summary>
            /// Additional information related to the current mode of the banknote neutralization. If this property is null, it is not applicable in the current context or it has not changed.
            /// * ```allSafeSensorsIgnored``` - Intentionally the protection is now partially armed in response to SetProtection "ignoreAllSafeSensors". All the safe sensors are ignored meanwhile the banknote neutralization in the Storage Unit remains armed.
            /// * ```armPending``` - The protection activation is intentionally delayed by configuration.
            /// </summary>
            public SubModeEnum SubMode
            {
                get { return subMode; }
                set
                {
                    if (subMode != value)
                    {
                        subMode = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private SubModeEnum subMode = SubMode;
        }

        public enum SafeDoorStateEnum
        {
            Fault,
            DoorOpened,
            DoorClosed,
            Disabled,
            NotSupported,
        }

        public enum SafeBoltStateEnum
        {
            Fault,
            BoltUnlocked,
            BoltLocked,
            Disabled,
            NotSupported,
        }

        public enum TiltStateEnum
        {
            Fault,
            NotTilted,
            Tilted,
            Disabled,
            NotSupported,
        }

        public enum LightStateEnum
        {
            Fault,
            NotConfigured,
            Detected,
            NotDetected,
            Disabled,
            NotSupported,
        }

        public enum GasStateEnum
        {
            Initializing,
            Fault,
            NotConfigured,
            NotDetected,
            PartialWarningLevel,
            WarningLevel,
            PartialCriticalLevel,
            CriticalLevel,
            Disabled,
            NotSupported,
        }

        public enum TemperatureStateEnum
        {
            Fault,
            Healthy,
            TooCold,
            TooHot,
            Disabled,
            NotSupported,
        }

        public enum SeismicStateEnum
        {
            Fault,
            NotConfigured,
            NotDetected,
            WarningLevel,
            CriticalLevel,
            Disabled,
            NotSupported,
        }

        public sealed class WarningStateClass(bool ProtectionArmingFault = false,
            bool ProtectionDisarmingFault = false,
            bool ExternalMainPowerOutage = false,
            bool StorageUnitLowPowerSupply = false,
            bool ArmedAutonomous = false,
            bool ArmedAlarm = false,
            bool GasWarningLevel = false,
            bool SeismicActivityWarningLevel = false) : StatusBase
        {

            /// <summary>
            /// At least one banknote neutralization protection of a Storage Unit stayed disarmed after attempting to arm it.
            /// </summary>
            public bool ProtectionArmingFault
            {
                get { return protectionArmingFault; }
                set
                {
                    if (protectionArmingFault != value)
                    {
                        protectionArmingFault = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool protectionArmingFault = ProtectionArmingFault;

            /// <summary>
            /// At least one banknote neutralization protection of a Storage Unit stayed armed after attempting to disarm it.
            /// </summary>
            public bool ProtectionDisarmingFault
            {
                get { return protectionDisarmingFault; }
                set
                {
                    if (protectionDisarmingFault != value)
                    {
                        protectionDisarmingFault = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool protectionDisarmingFault = ProtectionDisarmingFault;

            /// <summary>
            /// A main power outage of the banknote neutralization occurred.
            /// </summary>
            public bool ExternalMainPowerOutage
            {
                get { return externalMainPowerOutage; }
                set
                {
                    if (externalMainPowerOutage != value)
                    {
                        externalMainPowerOutage = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool externalMainPowerOutage = ExternalMainPowerOutage;

            /// <summary>
            /// At least the power supply of one banknote neutralization of a Storage Unit is low.
            /// </summary>
            public bool StorageUnitLowPowerSupply
            {
                get { return storageUnitLowPowerSupply; }
                set
                {
                    if (storageUnitLowPowerSupply != value)
                    {
                        storageUnitLowPowerSupply = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool storageUnitLowPowerSupply = StorageUnitLowPowerSupply;

            /// <summary>
            /// The protection is armed but at least one banknote neutralization of a Storage Unit runs in an autonomous mode.
            /// </summary>
            public bool ArmedAutonomous
            {
                get { return armedAutonomous; }
                set
                {
                    if (armedAutonomous != value)
                    {
                        armedAutonomous = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool armedAutonomous = ArmedAutonomous;

            /// <summary>
            /// The protection is armed but the banknote neutralization is in alarm mode.
            /// </summary>
            public bool ArmedAlarm
            {
                get { return armedAlarm; }
                set
                {
                    if (armedAlarm != value)
                    {
                        armedAlarm = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool armedAlarm = ArmedAlarm;

            /// <summary>
            /// A warning level of gas is detected.
            /// </summary>
            public bool GasWarningLevel
            {
                get { return gasWarningLevel; }
                set
                {
                    if (gasWarningLevel != value)
                    {
                        gasWarningLevel = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool gasWarningLevel = GasWarningLevel;

            /// <summary>
            /// A warning level of the seismic activity is detected.
            /// </summary>
            public bool SeismicActivityWarningLevel
            {
                get { return seismicActivityWarningLevel; }
                set
                {
                    if (seismicActivityWarningLevel != value)
                    {
                        seismicActivityWarningLevel = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool seismicActivityWarningLevel = SeismicActivityWarningLevel;
        }

        public sealed class ErrorStateClass(bool ProtectionEnablingFailure = false,
            bool ProtectionDisarmingFailure = false,
            bool StorageUnitPowerSupplyFailure = false,
            bool BackupBatteryFailure = false,
            bool GasCriticalLevel = false,
            bool Light = false,
            bool Tilted = false,
            bool SeismicActivityCriticalLevel = false) : StatusBase
        {
            /// <summary>
            /// A critical error occurred while arming it.
            /// </summary>
            public bool ProtectionEnablingFailure
            {
                get { return protectionEnablingFailure; }
                set
                {
                    if (protectionEnablingFailure != value)
                    {
                        protectionEnablingFailure = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool protectionEnablingFailure = ProtectionEnablingFailure;

            /// <summary>
            /// A critical error occurred while disarming it.
            /// </summary>
            public bool ProtectionDisarmingFailure
            {
                get { return protectionDisarmingFailure; }
                set
                {
                    if (protectionDisarmingFailure != value)
                    {
                        protectionDisarmingFailure = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool protectionDisarmingFailure = ProtectionDisarmingFailure;

            /// <summary>
            /// There is a failure of at least one banknote neutralization power supply of a Storage Unit.
            /// </summary>
            public bool StorageUnitPowerSupplyFailure
            {
                get { return storageUnitPowerSupplyFailure; }
                set
                {
                    if (storageUnitPowerSupplyFailure != value)
                    {
                        storageUnitPowerSupplyFailure = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool storageUnitPowerSupplyFailure = StorageUnitPowerSupplyFailure;

            /// <summary>
            /// There is a failure of the backup battery.
            /// </summary>
            public bool BackupBatteryFailure
            {
                get { return backupBatteryFailure; }
                set
                {
                    if (backupBatteryFailure != value)
                    {
                        backupBatteryFailure = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool backupBatteryFailure = BackupBatteryFailure;

            /// <summary>
            /// A critical level of gas is detected.
            /// </summary>
            public bool GasCriticalLevel
            {
                get { return gasCriticalLevel; }
                set
                {
                    if (gasCriticalLevel != value)
                    {
                        gasCriticalLevel = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool gasCriticalLevel = GasCriticalLevel;

            /// <summary>
            /// Light is detected.
            /// </summary>
            public bool Light
            {
                get { return light; }
                set
                {
                    if (light != value)
                    {
                        light = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool light = Light;

            /// <summary>
            /// At least one banknote neutralization unit has been tilted from its normal operating position.
            /// </summary>
            public bool Tilted
            {
                get { return tilted; }
                set
                {
                    if (tilted != value)
                    {
                        tilted = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool tilted = Tilted;

            /// <summary>
            /// A critical level of the seismic activity is detected.
            /// </summary>
            public bool SeismicActivityCriticalLevel
            {
                get { return seismicActivityCriticalLevel; }
                set
                {
                    if (seismicActivityCriticalLevel != value)
                    {
                        seismicActivityCriticalLevel = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private bool seismicActivityCriticalLevel = SeismicActivityCriticalLevel;
        }

        public sealed class CustomInputStatusClass(CustomInputStatusClass.InputStateEnum InputState) : StatusBase
        {
            public enum InputStateEnum
            {
                Fault,
                Healthy,
                Triggered,
                Disabled,
            }

            /// <summary>
            /// This property is set by the framework to generate status changed event
            /// </summary>
            public IBNSCapabilitiesClass.CustomInputEnum? CustomInputPosition { get; set; } = null;
            public string VendorSpecificCustomInputPosition { get; set; } = null;

            /// <summary>
            ///  Specifies the status of a custom input as one of the following values:
            ///  * ```Fault``` - A fault has occurred on the custom input, the status cannot be determined.
            ///  * ```Ok``` - The custom input is in a non-triggered state.
            ///  * ```Triggered``` - The custom input has been triggered.
            ///  * ```Disabled``` - The custom input is disabled by configuration.
            /// </summary>
            public InputStateEnum InputState
            {
                get { return inputState; }
                set
                {
                    if (inputState != value)
                    {
                        inputState = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private InputStateEnum inputState = InputState;
        }

        /// <summary>
        /// The state of the banknote neutralization.
        /// The Client Application can monitor and verify it before deciding to continue operations. 
        /// This state can be specified more precisely by using the optional property SubMode.
        /// </summary>
        public StateClass State { get; init; } = State;

        /// <summary>
        /// The state of the safe door viewed by the sensors dedicated to the banknote neutralization.
        /// * ```Fault``` - A fault has occurred on this sensor.
        /// * ```DoorOpened``` - The door is opened.
        /// * ```DoorClosed``` - The door is closed.
        /// * ```Disabled``` - This sensor is disabled by configuration.
        /// * ```NotSupported``` - This is not supported.
        /// </summary>
        public SafeDoorStateEnum SafeDoorState
        {
            get { return safeDoorState; }
            set
            {
                if (safeDoorState != value)
                {
                    safeDoorState = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The state of the safe bolt viewed by the sensors dedicated to the banknote neutralization.
        /// * ```Fault``` - A fault has occurred on this sensor.
        /// * ```BoltUnlocked``` - The bolt is unlocked.
        /// * ```BoltLocked``` - The bolt is locked.
        /// * ```Disabled``` - This sensor is disabled by configuration.
        /// * ```NotSupported``` - This is not supported.
        /// </summary>
        public SafeBoltStateEnum SafeBoltState
        {
            get { return safeBoltState; }
            set
            {
                if (safeBoltState != value)
                {
                    safeBoltState = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Specifies the tilt state as one of the following values:
        /// * ```Fault``` - A fault has occurred on this sensor.
        /// * ```NotTilted``` - It is in normal operating position.
        /// * ```Tilted``` - It has been tilted from its normal operating position.
        /// * ```Disabled``` - This sensor is disabled by configuration.
        /// * ```NotSupported``` - This is not supported.
        /// </summary>
        public TiltStateEnum TiltState
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

        /// <summary>
        /// The state of the safe bolt viewed by the sensors dedicated to the banknote neutralization.
        /// Specifies the light sensing as one of the following values:
        /// 
        /// * ```Fault``` - A fault has occurred on this sensor.
        /// * ```NotConfigured``` - The light module is found but it is not enabled by configuration.
        /// * ```Detected``` - Light is detected.
        /// * ```NotDetected``` - No light is detected.
        /// * ```Disabled``` - This sensor is disabled by configuration.
        /// * ```NotSupported``` - This is not supported.
        /// </summary>
        public LightStateEnum LightState
        {
            get { return lightState; }
            set
            {
                if (lightState != value)
                {
                    lightState = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Specifies the gas sensing as one of the following values:
        /// 
        /// * ```Initializing``` - The status is not available yet.It is important to mention that the warm-up and calibration of the gas sensor can take a few minutes.
        /// * ```Fault``` - A fault has occurred on this sensor.
        /// * ```NotConfigured``` - The gas module is found but it is not enabled by configuration.
        /// * ```NotDetected``` - No gas detected.
        /// * ```PartialWarningLevel``` - A warning level of gas is partially detected.
        /// * ```WarningLevel``` - A warning level of gas is undoubtedly detected.
        /// * ```PrtialCriticalLevel``` - A critical level of gas is partially detected.
        /// * ```CriticalLevel``` - A critical level of gas is undoubtedly detected.
        /// * ```Disabled``` - This sensor is disabled by configuration.
        /// * ```NotSupported``` - This is not supported.
        /// </summary>
        public GasStateEnum GasState
        {
            get { return gasState; }
            set
            {
                if (gasState != value)
                {
                    gasState = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Specifies the temperature sensing as one of the following values:
        /// * ```Fault``` - A fault has occurred on this sensor.
        /// * ```Ok``` - The temperature is in the operating range.
        /// * ```TooCold``` - Too cold temperature.
        /// * ```TooHot``` - Too hot temperature.
        /// * ```Disabled``` - This sensor is disabled by configuration.
        /// * ```NotSupported``` - This is not supported.
        /// </summary>
        public TemperatureStateEnum TemperatureState
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

        /// <summary>
        /// Specifies the seismic sensing in the banknote neutralization as one of the following values:
        /// * ```Fault``` - A fault has occurred on this sensor.
        /// * ```NotConfigured``` - The seismic sensor is found but it is not enabled by configuration.
        /// * ```NotDetected``` - No seismic activity detected.
        /// * ```WarningLevel``` - A warning level of seismic activity has been detected.
        /// * ```CriticalLevel``` - A critical level of seismic activity has been detected.
        /// * ```Disabled``` - This sensor is disabled by configuration.
        /// * ```NotSupported``` - This is not supported.
        /// </summary>
        public SeismicStateEnum SeismicState
        {
            get { return seismicState; }
            set
            {
                if (seismicState != value)
                {
                    seismicState = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// A list of state for one or more custom input accordingly.
        /// </summary>
        public Dictionary<IBNSCapabilitiesClass.CustomInputEnum, CustomInputStatusClass> CustomInputStatus { get; init; } = CustomInputStatus;
        
        /// <summary>
        /// A list of vendor specific state for one or more custom input accordingly
        /// </summary>
        public Dictionary<string, CustomInputStatusClass> VendorSpecificCustomInputStatus { get; init; } = VendorSpecificCustomInputStatus;

        /// <summary>
        /// This indicates warnings that require special attention but banknote neutralization is still operational. 
        /// Each status is reflected by a boolean value which is true if the warning status is detected otherwise it is false.
        /// </summary>
        public WarningStateClass WarningState { get; init; } = WarningState;

        /// <summary>
        /// This indicates errors reflect reasons of failure requiring immediate attention. 
        /// Each status is reflected by a boolean value which is true if the error status is detected otherwise it is false.
        /// If one of them is true, banknote neutralization no longer works under normal conditions and may no longer be fully operational.
        /// </summary>
        public ErrorStateClass ErrorState { get; init; } = ErrorState;

        /// <summary>
        /// Information that can be generically applied to module providing power ranging from a simple non-rechargeable 
        /// battery to a more complex device such as a UPS.
        /// </summary>
        public PowerManagementStatusClass.PowerInfoClass PowerInfo { get; init; } = PowerInfo;

        private SafeDoorStateEnum safeDoorState = SafeDoorState;
        private SafeBoltStateEnum safeBoltState = SafeBoltState;
        private TiltStateEnum tiltState = TiltState;
        private LightStateEnum lightState = LightState;
        private GasStateEnum gasState = GasState;
        private TemperatureStateEnum temperatureState = TemperatureState;
        private SeismicStateEnum seismicState = SeismicState;
    }
}
