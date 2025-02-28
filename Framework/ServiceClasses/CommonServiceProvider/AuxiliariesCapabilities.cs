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

namespace XFS4IoTFramework.Common
{
    public sealed class AuxiliariesCapabilitiesClass(
        AuxiliariesCapabilitiesClass.OperatorSwitchEnum OperatorSwitch = AuxiliariesCapabilitiesClass.OperatorSwitchEnum.NotAvailable,
        AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum AuxiliariesSupported = AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.None,
        AuxiliariesCapabilitiesClass.EnhancedAudioCapabilitiesEnum EnhancedAudioSensor = AuxiliariesCapabilitiesClass.EnhancedAudioCapabilitiesEnum.NotAvailable,
        AuxiliariesCapabilitiesClass.HandsetSensorCapabilities HandsetSensor = AuxiliariesCapabilitiesClass.HandsetSensorCapabilities.NotAvailable,
        AuxiliariesCapabilitiesClass.HeadsetMicrophoneSensorCapabilities HeadsetMicrophoneSensor = AuxiliariesCapabilitiesClass.HeadsetMicrophoneSensorCapabilities.NotAvailable,
        AuxiliariesCapabilitiesClass.VandalShieldCapabilities VandalShield = AuxiliariesCapabilitiesClass.VandalShieldCapabilities.NotAvailable,
        Dictionary<AuxiliariesCapabilitiesClass.DoorType, AuxiliariesCapabilitiesClass.DoorCapabilities> SupportedDoorSensors = null,
        int Volume = 1,
        AuxiliariesCapabilitiesClass.UpsEnum Ups = AuxiliariesCapabilitiesClass.UpsEnum.NotAvailable,
        AuxiliariesCapabilitiesClass.EnhancedAudioControlEnum EnhancedAudioControl = AuxiliariesCapabilitiesClass.EnhancedAudioControlEnum.NotAvailable,
        AuxiliariesCapabilitiesClass.EnhancedAudioControlEnum EnhancedMicrophoneControlState = AuxiliariesCapabilitiesClass.EnhancedAudioControlEnum.NotAvailable,
        int MicrophoneVolume = 1,
        AuxiliariesCapabilitiesClass.AutoStartupModes AutoStartupMode = AuxiliariesCapabilitiesClass.AutoStartupModes.NotAvailable)
    {
        [Flags]
        public enum OperatorSwitchEnum
        {
            /// <summary>
            /// The OperatorSwitch is not available.
            /// </summary>
            NotAvailable = 0,
            /// <summary>
            /// The switch can be set in Run mode.
            /// </summary>
            Run = 1 << 0,
            /// <summary>
            /// The switch can be set in Maintenance mode.
            /// </summary>
            Maintenance = 1 << 1,
            /// <summary>
            /// The switch can be set in Supervisor mode.
            /// </summary> 
            Supervisor = 1 << 2,
        }

        [Flags]
        public enum AuxiliariesSupportedEnum
        {
            None = 0, 
            TamperSensor = 1 << 0,
            InternalTamperSensor = 1 << 1,
            SeismicSensor = 1 << 2,
            HeatSensor = 1 << 3,
            ProximitySensor = 1 << 4,
            AmbientLightSensor = 1 << 5,
            BootSwitchSensor = 1 << 6,
            ConsumerDisplaySensor = 1 << 7,
            OperatorCallButtonSensor = 1 << 8,
            FasciaMicrophoneSensor = 1 << 9,
            OpenCloseIndicator = 1 << 10,
            Audio = 1 << 11,
            Heating = 1 << 12,
            ConsumerDisplayBacklight = 1 << 13,
            SignageDisplay = 1 << 14,
            AudibleAlarm = 1 << 15,
        }

        /// <summary>
        /// Specifies which states the Operator Switch can be set to, if available.
        /// </summary>
        public OperatorSwitchEnum OperatorSwitch { get; init; } = OperatorSwitch;

        /// <summary>
        /// Specifies Auxiliaries which are supported as a combination of Enum flags.
        /// </summary>
        public AuxiliariesSupportedEnum AuxiliariesSupported { get; init; } = AuxiliariesSupported;

        [Flags]
        public enum EnhancedAudioCapabilitiesEnum
        {
            /// <summary>
            /// EnhancedAudio is not available.
            /// </summary>
            NotAvailable = 0,
            /// <summary>
            /// The Audio Jack is available and supports manual mode.
            /// </summary>
            Manual = 1 << 0,
            /// <summary>
            /// The Audio Jack is available and supports auto mode.
            /// </summary>
            Auto = 1 << 1,
            /// <summary>
            /// The Audio Jack is available and supports semi-auto mode.
            /// </summary>
            SemiAuto = 1 << 2,
            /// <summary>
            /// The Audio Jack is available and can support headphones that have an integrated microphone via a
            /// single jack.
            /// </summary>
            Bidirectional = 1 << 3,
        }

        /// <summary>
        /// Specifies which mode the Audio Jack supports, if present.
        /// </summary>
        public EnhancedAudioCapabilitiesEnum EnhancedAudioSensor { get; init; } = EnhancedAudioSensor;

        [Flags]
        public enum HandsetSensorCapabilities
        {
            /// <summary>
            /// HandsetSensor is not available.
            /// </summary>
            NotAvailable = 0,
            /// <summary>
            /// The Handset is available and it supports manual mode.
            /// </summary>
            Manual = 1 << 0,
            /// <summary>
            /// The Handset is available and it supports auto mode.
            /// </summary>
            Auto = 1 << 1,
            /// <summary>
            /// The Handset is available and it supports semi-auto mode.
            /// </summary>
            SemiAuto = 1 << 2,
            /// <summary>
            /// The Handset is available and contains an embedded microphone for audio input.
            /// </summary>
            Microphone = 1 << 3,
        }

        /// <summary>
        /// Specifies which mode the Handset supports, if present.
        /// </summary>
        public HandsetSensorCapabilities HandsetSensor { get; init; } = HandsetSensor;

        [Flags]
        public enum HeadsetMicrophoneSensorCapabilities
        {
            /// <summary>
            /// HeadsetMicrophoneSensor is not available.
            /// </summary>
            NotAvailable = 0,
            /// <summary>
            /// The Microphone Jack is available and it supports manual mode.
            /// </summary>
            Manual = 1 << 0,
            /// <summary>
            /// The Microphone Jack is available and it supports auto mode.
            /// </summary>
            Auto = 1 << 1,
            /// <summary>
            /// The Microphone Jack is available and it supports semi-auto mode.
            /// </summary>
            SemiAuto = 1 << 2,
        }

        /// <summary>
        /// Specifies whether the Microphone Jack is present, and if so, which modes it supports. If the 
        /// enhancedAudio capability indicates the presence of a bi-directional Audio Jack then both sensors 
        /// reference the same physical jack.
        /// </summary>
        public HeadsetMicrophoneSensorCapabilities HeadsetMicrophoneSensor { get; init; } = HeadsetMicrophoneSensor;

        [Flags]
        public enum VandalShieldCapabilities
        {
            /// <summary>
            /// VandalShield is not available.
            /// </summary>
            NotAvailable = 0,
            /// <summary>
            /// The Vandal Shield can be closed.
            /// </summary>
            Closed = 1 << 0,
            /// <summary>
            /// The Vandal Shield can be open.
            /// </summary>
            Open = 1 << 1,
            /// <summary>
            /// The Vandal Shield can be locked.
            /// </summary>
            Locked = 1 << 2,
            /// <summary>
            /// The Vandal Shield can be in the service position.
            /// </summary>
            Service = 1 << 3,
            /// <summary>
            /// The Vandal Shield can be in a position that permits access to the keyboard.
            /// </summary>
            Keyboard = 1 << 4,
            /// <summary>
            /// The Vandal Shield can detect potential tampering.
            /// </summary>
            Tampered = 1 << 5,
        }

        /// <summary>
        /// Specifies the states the Vandal Shield can support, if available.
        /// </summary>
        public VandalShieldCapabilities VandalShield { get; init; } = VandalShield;

        public enum DoorType
        {
            Safe,
            Cabinet,
            FrontCabinet,
            RearCabinet,
            LeftCabinet,
            RightCabinet
        }

        [Flags]
        public enum DoorCapabilities
        {
            /// <summary>
            /// The door can report the closed state.
            /// </summary>
            Closed = 0,
            /// <summary>
            /// The door can report the open state.
            /// </summary>
            Open = 1 << 0,
            /// <summary>
            /// The door can report the locked state.
            /// </summary>
            Locked = 1 << 1,
            /// <summary>
            /// The door can report the bolted state.
            /// </summary>
            Bolted = 1 << 2,
            /// <summary>
            /// The door can report the tampered state.
            /// </summary>
            Tampered = 1 << 3,
        }

        /// <summary>
        /// Specifies a list of doors supported by the device, and their capabilities as a combination of Enum flags.
        /// </summary>
        public Dictionary<DoorType, DoorCapabilities> SupportedDoorSensors { get; init; } = SupportedDoorSensors;

        /// <summary>
        /// Specifies the Volume Control increment/decrement value recommended by the vendor.
        /// </summary>
        public int Volume { get; init; } = Volume;

        [Flags]
        public enum UpsEnum
        {
            /// <summary>
            /// Ups is not available.
            /// </summary>
            NotAvailable = 0,
            /// <summary>
            /// The UPS can indicate that its charge level is low.
            /// </summary>
            Low = 1 << 0,
            /// <summary>
            /// The UPS can be engaged and disengaged by the application.
            /// </summary>
            Engaged = 1 << 1,
            /// <summary>
            /// The UPS can indicate that it is powering the system while the main power supply is off.
            /// </summary>
            Powering = 1 << 2,
            /// <summary>
            /// The UPS can indicate that it was engaged when the main power went off.
            /// </summary>
            Recovered = 1 << 3,
        }

        /// <summary>
        /// Specifies what states the UPS device supports as a combination of the following values:
        /// </summary>
        public UpsEnum Ups { get; init; } = Ups;

        [Flags]
        public enum EnhancedAudioControlEnum
        {
            /// <summary>
            /// EnhancedAudioControl is not available.
            /// </summary>
            NotAvailable = 0,
            /// <summary>
            /// The Enhanced Audio/Microphone Controller is supports Privacy Device activation/deactivation. The device is able 
            /// to report events to indicate Privacy Device activation/deactivation.
            /// </summary>
            HeadsetDetection = 1 << 0,
            /// <summary>
            /// The Enhanced Audio/Microphone Controller supports application control of the Privacy Device mode via the 
            /// setAuxiliaries command.
            /// </summary>
            ModeControllable = 1 << 1,
        }

        /// <summary>
        /// Specifies the modes the Enhanced Audio Controller can support. The Enhanced Audio Controller controls how 
        /// private and public audio are broadcast when the headset is inserted into/removed from the audio jack and 
        /// when the handset is off-hook/on-hook. In the following Privacy Device is used to refer to either the 
        /// headset or handset. The modes it supports are specified as a combination of the following values:
        /// </summary>
        public EnhancedAudioControlEnum EnhancedAudioControl { get; init; } = EnhancedAudioControl;

        /// <summary>
        /// Specifies the modes the Enhanced Microphone Controller can support. The Enhanced Microphone Controller 
        /// controls how private and public audio input are transmitted when the headset is inserted into/removed 
        /// from the audio jack and when the handset is off-hook/on-hook. In the following Privacy Device is used 
        /// to refer to either the headset or handset. The modes it supports are specified as a combination of the 
        /// following values:
        /// </summary>
        public EnhancedAudioControlEnum EnhancedMicrophoneControlState { get; init; } = EnhancedMicrophoneControlState;

        /// <summary>
        /// Specifies the Microphone Volume Control increment/decrement value recommended by the vendor.
        /// </summary>
        public int MicrophoneVolume { get; init; } = MicrophoneVolume;

        public enum AutoStartupModes
        {
            /// <summary>
            /// AutoStartup is not available
            /// </summary>
            NotAvailable,
            /// <summary>
            /// The device supports one-time auto start-up on a specific date at a specific time.
            /// </summary>
            Specific,
            /// <summary>
            /// The device supports auto start-up every day at a specific time.
            /// </summary>
            Daily,
            /// <summary>
            /// The device supports auto start-up at a specified time on a specific day of every week.
            /// </summary>
            Weekly
        }

        /// <summary>
        /// AutoStartup Modes supported by the device
        /// </summary>
        public AutoStartupModes AutoStartupMode { get; init; } = AutoStartupMode;

    }
}
