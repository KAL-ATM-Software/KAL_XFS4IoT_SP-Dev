/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT IntelligentBanknoteNeutralization interface.
 * IntelligentBanknoteNeutralizationSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.IntelligentBanknoteNeutralization
{

    [DataContract]
    public sealed class StateClass
    {
        public StateClass(ModeEnum? Mode = null, SubmodeEnum? Submode = null)
        {
            this.Mode = Mode;
            this.Submode = Submode;
        }

        public enum ModeEnum
        {
            Fault,
            Armed,
            Disarmed,
            NeutralizationTriggered
        }

        /// <summary>
        /// The state of the banknote neutralization.
        /// 
        /// * ```fault``` - A system fault occurred or due to a hardware error or other condition, the status cannot be determined.
        /// * ```armed``` - The protection is now armed. 
        /// * ```disarmed``` - The protection is now disarmed.
        /// * ```neutralizationTriggered``` - The neutralization trigger occurred.
        /// 
        /// This may be null in [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// </summary>
        [DataMember(Name = "mode")]
        public ModeEnum? Mode { get; init; }

        public enum SubmodeEnum
        {
            AllSafeSensorsIgnored,
            ArmPending
        }

        /// <summary>
        /// Additional information related to the current mode of the banknote neutralization. If this property is null, it is not applicable in the current context or it has not changed.
        /// 
        /// * ```allSafeSensorsIgnored``` - Intentionally the protection is now partially armed in response to SetProtection "ignoreAllSafeSensors". All the safe sensors are ignored meanwhile the banknote neutralization in the Storage Unit remains armed.
        /// * ```armPending``` - The protection activation is intentionally delayed by configuration.
        /// 
        /// This may be null in [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// </summary>
        [DataMember(Name = "submode")]
        public SubmodeEnum? Submode { get; init; }

    }


    public enum SafeDoorStateEnum
    {
        Fault,
        DoorOpened,
        DoorClosed,
        Disabled
    }


    public enum SafeBoltStateEnum
    {
        Fault,
        BoltUnlocked,
        BoltLocked,
        Disabled
    }


    public enum TiltStateEnum
    {
        Fault,
        NotTilted,
        Tilted,
        Disabled
    }


    public enum LightStateEnum
    {
        Fault,
        NotConfigured,
        Detected,
        NotDetected,
        Disabled
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
        Disabled
    }


    public enum TemperatureStateEnum
    {
        Fault,
        Ok,
        TooCold,
        TooHot,
        Disabled
    }


    public enum SeismicStateEnum
    {
        Fault,
        NotConfigured,
        NotDetected,
        WarningLevel,
        CriticalLevel,
        Disabled
    }


    [DataContract]
    public sealed class WarningsClass
    {
        public WarningsClass(bool? ProtectionArmingFault = null, bool? ProtectionDisarmingFault = null, bool? ExternalMainPowerOutage = null, bool? StorageUnitLowPowerSupply = null, bool? ArmedAutonomous = null, bool? ArmedAlarm = null, bool? GasWarningLevel = null, bool? SeismicActivityWarningLevel = null)
        {
            this.ProtectionArmingFault = ProtectionArmingFault;
            this.ProtectionDisarmingFault = ProtectionDisarmingFault;
            this.ExternalMainPowerOutage = ExternalMainPowerOutage;
            this.StorageUnitLowPowerSupply = StorageUnitLowPowerSupply;
            this.ArmedAutonomous = ArmedAutonomous;
            this.ArmedAlarm = ArmedAlarm;
            this.GasWarningLevel = GasWarningLevel;
            this.SeismicActivityWarningLevel = SeismicActivityWarningLevel;
        }

        /// <summary>
        /// At least one banknote neutralization protection of a Storage Unit stayed disarmed after attempting to arm it.
        /// </summary>
        [DataMember(Name = "protectionArmingFault")]
        public bool? ProtectionArmingFault { get; init; }

        /// <summary>
        /// At least one banknote neutralization protection of a Storage Unit stayed armed after attempting to disarm it.
        /// </summary>
        [DataMember(Name = "protectionDisarmingFault")]
        public bool? ProtectionDisarmingFault { get; init; }

        /// <summary>
        /// A main power outage of the banknote neutralization occurred.
        /// </summary>
        [DataMember(Name = "externalMainPowerOutage")]
        public bool? ExternalMainPowerOutage { get; init; }

        /// <summary>
        /// At least the power supply of one banknote neutralization of a Storage Unit is low.
        /// </summary>
        [DataMember(Name = "storageUnitLowPowerSupply")]
        public bool? StorageUnitLowPowerSupply { get; init; }

        /// <summary>
        /// The protection is armed but at least one banknote neutralization of a Storage Unit runs in an autonomous mode.
        /// </summary>
        [DataMember(Name = "armedAutonomous")]
        public bool? ArmedAutonomous { get; init; }

        /// <summary>
        /// The protection is armed but the banknote neutralization is in alarm mode.
        /// </summary>
        [DataMember(Name = "armedAlarm")]
        public bool? ArmedAlarm { get; init; }

        /// <summary>
        /// A warning level of gas is detected.
        /// </summary>
        [DataMember(Name = "gasWarningLevel")]
        public bool? GasWarningLevel { get; init; }

        /// <summary>
        /// A warning level of the seismic activity is detected.
        /// </summary>
        [DataMember(Name = "seismicActivityWarningLevel")]
        public bool? SeismicActivityWarningLevel { get; init; }

    }


    [DataContract]
    public sealed class ErrorsClass
    {
        public ErrorsClass(bool? ProtectionEnablingFailure = null, bool? ProtectionDisarmingFailure = null, bool? StorageUnitPowerSupplyFailure = null, bool? BackupBatteryFailure = null, bool? GasCriticalLevel = null, bool? Light = null, bool? Tilted = null, bool? SeismicActivityCriticalLevel = null)
        {
            this.ProtectionEnablingFailure = ProtectionEnablingFailure;
            this.ProtectionDisarmingFailure = ProtectionDisarmingFailure;
            this.StorageUnitPowerSupplyFailure = StorageUnitPowerSupplyFailure;
            this.BackupBatteryFailure = BackupBatteryFailure;
            this.GasCriticalLevel = GasCriticalLevel;
            this.Light = Light;
            this.Tilted = Tilted;
            this.SeismicActivityCriticalLevel = SeismicActivityCriticalLevel;
        }

        /// <summary>
        /// A critical error occurred while arming it.
        /// </summary>
        [DataMember(Name = "protectionEnablingFailure")]
        public bool? ProtectionEnablingFailure { get; init; }

        /// <summary>
        /// A critical error occurred while disarming it.
        /// </summary>
        [DataMember(Name = "protectionDisarmingFailure")]
        public bool? ProtectionDisarmingFailure { get; init; }

        /// <summary>
        /// There is a failure of at least one banknote neutralization power supply of a Storage Unit.
        /// </summary>
        [DataMember(Name = "storageUnitPowerSupplyFailure")]
        public bool? StorageUnitPowerSupplyFailure { get; init; }

        /// <summary>
        /// There is a failure of the backup battery.
        /// </summary>
        [DataMember(Name = "backupBatteryFailure")]
        public bool? BackupBatteryFailure { get; init; }

        /// <summary>
        /// A critical level of gas is detected.
        /// </summary>
        [DataMember(Name = "gasCriticalLevel")]
        public bool? GasCriticalLevel { get; init; }

        /// <summary>
        /// Light is detected.
        /// </summary>
        [DataMember(Name = "light")]
        public bool? Light { get; init; }

        /// <summary>
        /// At least one banknote neutralization unit has been tilted from its normal operating position.
        /// </summary>
        [DataMember(Name = "tilted")]
        public bool? Tilted { get; init; }

        /// <summary>
        /// A critical level of the seismic activity is detected.
        /// </summary>
        [DataMember(Name = "seismicActivityCriticalLevel")]
        public bool? SeismicActivityCriticalLevel { get; init; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(StateClass State = null, SafeDoorStateEnum? SafeDoor = null, SafeBoltStateEnum? SafeBolt = null, TiltStateEnum? Tilt = null, LightStateEnum? Light = null, GasStateEnum? Gas = null, TemperatureStateEnum? Temperature = null, SeismicStateEnum? Seismic = null, Dictionary<string, CustomInputsClass> CustomInputs = null, PowerManagement.StatusClass PowerSupply = null, WarningsClass Warnings = null, ErrorsClass Errors = null)
        {
            this.State = State;
            this.SafeDoor = SafeDoor;
            this.SafeBolt = SafeBolt;
            this.Tilt = Tilt;
            this.Light = Light;
            this.Gas = Gas;
            this.Temperature = Temperature;
            this.Seismic = Seismic;
            this.CustomInputs = CustomInputs;
            this.PowerSupply = PowerSupply;
            this.Warnings = Warnings;
            this.Errors = Errors;
        }

        [DataMember(Name = "state")]
        public StateClass State { get; init; }

        [DataMember(Name = "safeDoor")]
        public SafeDoorStateEnum? SafeDoor { get; init; }

        [DataMember(Name = "safeBolt")]
        public SafeBoltStateEnum? SafeBolt { get; init; }

        [DataMember(Name = "tilt")]
        public TiltStateEnum? Tilt { get; init; }

        [DataMember(Name = "light")]
        public LightStateEnum? Light { get; init; }

        [DataMember(Name = "gas")]
        public GasStateEnum? Gas { get; init; }

        [DataMember(Name = "temperature")]
        public TemperatureStateEnum? Temperature { get; init; }

        [DataMember(Name = "seismic")]
        public SeismicStateEnum? Seismic { get; init; }

        [DataContract]
        public sealed class CustomInputsClass
        {
            public CustomInputsClass(InputStateEnum? InputState = null)
            {
                this.InputState = InputState;
            }

            public enum InputStateEnum
            {
                Fault,
                Ok,
                Triggered,
                Disabled
            }

            /// <summary>
            /// Specifies the status of a custom input as one of the following values:
            /// 
            /// * ```fault``` - A fault has occurred on the custom input, the status cannot be determined.
            /// * ```ok``` - The custom input is in a non-triggered state.
            /// * ```triggered``` - The custom input has been triggered.
            /// * ```disabled``` - The custom input is disabled by configuration.
            /// </summary>
            [DataMember(Name = "inputState")]
            public InputStateEnum? InputState { get; init; }

        }

        /// <summary>
        /// A list of state for one or more custom input accordingly to those defined in CustomInputs in [Capabilities](#common.capabilities.completion.properties.banknoteneutralization.custominputs).
        /// If there is no available configured Custom inputs, null will be reported in [Common.Status](#common.status) 
        /// This may be null in [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// </summary>
        [DataMember(Name = "customInputs")]
        public Dictionary<string, CustomInputsClass> CustomInputs { get; init; }

        [DataMember(Name = "powerSupply")]
        public PowerManagement.StatusClass PowerSupply { get; init; }

        [DataMember(Name = "warnings")]
        public WarningsClass Warnings { get; init; }

        [DataMember(Name = "errors")]
        public ErrorsClass Errors { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(ModeEnum? Mode = null, bool? GasSensor = null, bool? LightSensor = null, bool? SeismicSensor = null, bool? SafeIntrusionDetection = null, bool? ExternalDryContactStatusBox = null, bool? RealTimeClock = null, bool? PhysicalStorageUnitsAccessControl = null, Dictionary<string, CustomInputsClass> CustomInputs = null)
        {
            this.Mode = Mode;
            this.GasSensor = GasSensor;
            this.LightSensor = LightSensor;
            this.SeismicSensor = SeismicSensor;
            this.SafeIntrusionDetection = SafeIntrusionDetection;
            this.ExternalDryContactStatusBox = ExternalDryContactStatusBox;
            this.RealTimeClock = RealTimeClock;
            this.PhysicalStorageUnitsAccessControl = PhysicalStorageUnitsAccessControl;
            this.CustomInputs = CustomInputs;
        }

        public enum ModeEnum
        {
            Autonomous,
            ClientControlled,
            VendorSpecific
        }

        /// <summary>
        /// Indicates the operating mode of the banknote neutralization.
        /// 
        /// * ```autonomous``` - The banknote neutralization autonomously activates the surveillance as soon as the safe door is closed and locked and to deactivate it when it detects a legal entry.
        /// * ```clientControlled``` - The Client Application is in charge of arming and disarming the system.
        /// * ```vendorSpecific``` - Neither autonomous nor programmable. The mode is vendor specific.
        /// </summary>
        [DataMember(Name = "mode")]
        public ModeEnum? Mode { get; init; }

        /// <summary>
        /// Indicates the presence and management of a gas sensor in the banknote neutralization.
        /// </summary>
        [DataMember(Name = "gasSensor")]
        public bool? GasSensor { get; init; }

        /// <summary>
        /// Indicates the presence and management of a light sensor in the banknote neutralization.
        /// </summary>
        [DataMember(Name = "lightSensor")]
        public bool? LightSensor { get; init; }

        /// <summary>
        /// Indicates the presence and management of a seismic sensor in the banknote neutralization.
        /// </summary>
        [DataMember(Name = "seismicSensor")]
        public bool? SeismicSensor { get; init; }

        /// <summary>
        /// Indicates the presence and management of a safe intrusion detection in the banknote neutralization.
        /// </summary>
        [DataMember(Name = "safeIntrusionDetection")]
        public bool? SafeIntrusionDetection { get; init; }

        /// <summary>
        /// Indicates the presence and management of an external dry Contact Box in the banknote neutralization.
        /// </summary>
        [DataMember(Name = "externalDryContactStatusBox")]
        public bool? ExternalDryContactStatusBox { get; init; }

        /// <summary>
        /// Indicates the presence and management of a Real Time Clock in the banknote neutralization.
        /// </summary>
        [DataMember(Name = "realTimeClock")]
        public bool? RealTimeClock { get; init; }

        /// <summary>
        /// Indicates the presence of a physical access to the Storage Units and controlled by the banknote neutralization.
        /// </summary>
        [DataMember(Name = "physicalStorageUnitsAccessControl")]
        public bool? PhysicalStorageUnitsAccessControl { get; init; }

        [DataContract]
        public sealed class CustomInputsClass
        {
            public CustomInputsClass(bool? ActiveInput = null)
            {
                this.ActiveInput = ActiveInput;
            }

            /// <summary>
            /// This input is configured and active.
            /// </summary>
            [DataMember(Name = "activeInput")]
            public bool? ActiveInput { get; init; }

        }

        /// <summary>
        /// Indicates the presence of a set of custom inputs managed by the banknote neutralization. Each of the custom inputs are dedicated to one specific feature.
        /// </summary>
        [DataMember(Name = "customInputs")]
        public Dictionary<string, CustomInputsClass> CustomInputs { get; init; }

    }


    [DataContract]
    public sealed class StorageUnitStatusClass
    {
        public StorageUnitStatusClass(string Identifier = null, ProtectionEnum? Protection = null, WarningEnum? Warning = null, PowerManagement.PowerInfoClass PowerSupply = null, TiltStateEnum? Tilt = null, TemperatureStateEnum? Temperature = null, LidEnum? Lid = null, NeutralizationTriggerEnum? NeutralizationTrigger = null, string StorageUnitIdentifier = null)
        {
            this.Identifier = Identifier;
            this.Protection = Protection;
            this.Warning = Warning;
            this.PowerSupply = PowerSupply;
            this.Tilt = Tilt;
            this.Temperature = Temperature;
            this.Lid = Lid;
            this.NeutralizationTrigger = NeutralizationTrigger;
            this.StorageUnitIdentifier = StorageUnitIdentifier;
        }

        /// <summary>
        /// Indicates the identifier assigned to the banknote neutralization of a Storage Unit by the vendor of the banknote neutralization. There is no default value because banknote neutralization unit must be defined.
        /// <example>123456781</example>
        /// </summary>
        [DataMember(Name = "identifier")]
        [DataTypes(Pattern = @"^[0-9A-Za-z]*$")]
        public string Identifier { get; init; }

        public enum ProtectionEnum
        {
            Fault,
            Armed,
            Disarmed,
            NeutralizationTriggered
        }

        /// <summary>
        /// Specifies the state of the banknote neutralization of a Storage Unit as one of the following values:
        /// 
        /// * ```fault``` - A system fault occurred.
        /// * ```armed``` - The protection is armed.
        /// * ```disarmed``` - The protection is disarmed.
        /// * ```neutralizationTriggered``` - The neutralization trigger occurred.
        /// </summary>
        [DataMember(Name = "protection")]
        public ProtectionEnum? Protection { get; init; }

        public enum WarningEnum
        {
            CassetteRunsAutonomously,
            Alarm
        }

        /// <summary>
        /// Gives additional information that requires attention:
        /// 
        /// * ```cassetteRunsAutonomously``` - The protection is armed but the banknote neutralization of a Storage Unit runs in an autonomous mode.
        /// * ```alarm``` - The protection is armed but in alarm mode.
        /// </summary>
        [DataMember(Name = "warning")]
        public WarningEnum? Warning { get; init; }

        [DataMember(Name = "powerSupply")]
        public PowerManagement.PowerInfoClass PowerSupply { get; init; }

        [DataMember(Name = "tilt")]
        public TiltStateEnum? Tilt { get; init; }

        [DataMember(Name = "temperature")]
        public TemperatureStateEnum? Temperature { get; init; }

        public enum LidEnum
        {
            Fault,
            Opened,
            Closed,
            Disabled
        }

        /// <summary>
        /// Specifies the Storage Unit lid state as one of the following values:
        /// 
        /// * ```fault``` - A fault is detected in the sensor or due to a hardware error or other condition, the status cannot be determined.
        /// * ```opened``` - The lid is opened.
        /// * ```closed``` - Too lid is closed.
        /// * ```disabled``` - The banknote neutralization disabled this sensor for malfunction or by configuration.
        /// </summary>
        [DataMember(Name = "lid")]
        public LidEnum? Lid { get; init; }

        public enum NeutralizationTriggerEnum
        {
            Initializing,
            Ready,
            Disabled,
            Fault
        }

        /// <summary>
        /// Specifies the state of the neutralization trigger as one of the following values:
        /// 
        /// * ```initializing``` - The neutralization trigger is being initialized. It may take a few seconds before the ready state.
        /// * ```ready``` - The neutralization is ready to trigger on demand.
        /// * ```disabled``` - The neutralization trigger is inhibited and disabled.
        /// * ```fault``` - A fault is detected in the neutralization trigger or due to a hardware error or other condition, the status cannot be determined. .
        /// </summary>
        [DataMember(Name = "neutralizationTrigger")]
        public NeutralizationTriggerEnum? NeutralizationTrigger { get; init; }

        /// <summary>
        /// If the Storage Unit Identifier can be written at installation or production time, this property returns the
        /// Storage Unit Identifier to which this BanknoteNeutralization is linked. Otherwise, this property is null.
        /// <example>123</example>
        /// </summary>
        [DataMember(Name = "storageUnitIdentifier")]
        public string StorageUnitIdentifier { get; init; }

    }


}
