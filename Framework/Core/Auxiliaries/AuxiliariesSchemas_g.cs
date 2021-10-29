/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * AuxiliariesSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Auxiliaries
{

    public enum OperatorSwitchStateEnum
    {
        NotAvailable,
        Run,
        Maintenance,
        Supervisor
    }


    public enum TamperSensorStateEnum
    {
        NotAvailable,
        On,
        Off
    }


    public enum IntTamperSensorStateEnum
    {
        NotAvailable,
        On,
        Off
    }


    public enum SeismicSensorStateEnum
    {
        NotAvailable,
        On,
        Off
    }


    public enum HeatSensorStateEnum
    {
        NotAvailable,
        On,
        Off
    }


    public enum ProximitySensorStateEnum
    {
        NotAvailable,
        Present,
        NotPresent
    }


    public enum AmbientLightSensorStateEnum
    {
        NotAvailable,
        VeryDark,
        Dark,
        MediumLight,
        Light,
        VeryLight
    }


    public enum EnhancedAudioSensorStateEnum
    {
        NotAvailable,
        Present,
        NotPresent
    }


    public enum BootSwitchSensorStateEnum
    {
        NotAvailable,
        Off,
        On
    }


    public enum DisplaySensorStateEnum
    {
        NotAvailable,
        Off,
        On,
        DisplayError
    }


    public enum OperatorCallButtonSensorStateEnum
    {
        NotAvailable,
        Off,
        On
    }


    public enum HandsetSensorStateEnum
    {
        NotAvailable,
        OnTheHook,
        OffTheHook
    }


    public enum HeadsetMicrophoneSensorStateEnum
    {
        NotAvailable,
        Present,
        NotPresent
    }


    public enum FasciaMicrophoneSensorStateEnum
    {
        NotAvailable,
        Off,
        On
    }


    public enum SafeDoorStateEnum
    {
        NotAvailable,
        Closed,
        Open,
        Locked,
        Bolted,
        Tampered
    }


    public enum VandalShieldStateEnum
    {
        NotAvailable,
        Closed,
        Open,
        Locked,
        Service,
        Keyboard,
        Ajar,
        Jammed,
        Tampered
    }


    public enum CabinetFrontDoorStateEnum
    {
        NotAvailable,
        Closed,
        Open,
        Locked,
        Bolted,
        Tampered
    }


    public enum CabinetRearDoorStateEnum
    {
        NotAvailable,
        Closed,
        Open,
        Locked,
        Bolted,
        Tampered
    }


    public enum CabinetLeftDoorStateEnum
    {
        NotAvailable,
        Closed,
        Open,
        Locked,
        Bolted,
        Tampered
    }


    public enum CabinetRightDoorStateEnum
    {
        NotAvailable,
        Closed,
        Open,
        Locked,
        Bolted,
        Tampered
    }


    public enum OpenClosedIndicatorStateEnum
    {
        NotAvailable,
        Closed,
        Open
    }


    [DataContract]
    public sealed class AudioStateClass
    {
        public AudioStateClass(bool? NotAvailable = null, bool? Off = null, bool? Keypress = null, bool? Exclamation = null, bool? Warning = null, bool? Error = null, bool? Critical = null, bool? Continuous = null)
        {
            this.NotAvailable = NotAvailable;
            this.Off = Off;
            this.Keypress = Keypress;
            this.Exclamation = Exclamation;
            this.Warning = Warning;
            this.Error = Error;
            this.Critical = Critical;
            this.Continuous = Continuous;
        }

        [DataMember(Name = "notAvailable")]
        public bool? NotAvailable { get; init; }

        [DataMember(Name = "off")]
        public bool? Off { get; init; }

        [DataMember(Name = "keypress")]
        public bool? Keypress { get; init; }

        [DataMember(Name = "exclamation")]
        public bool? Exclamation { get; init; }

        [DataMember(Name = "warning")]
        public bool? Warning { get; init; }

        [DataMember(Name = "error")]
        public bool? Error { get; init; }

        [DataMember(Name = "critical")]
        public bool? Critical { get; init; }

        [DataMember(Name = "continuous")]
        public bool? Continuous { get; init; }

    }


    public enum HeatingStateEnum
    {
        NotAvailable,
        Off,
        On
    }


    public enum ConsumerDisplayBacklightStateEnum
    {
        NotAvailable,
        Off,
        On
    }


    public enum SignageDisplayStateEnum
    {
        NotAvailable,
        Off,
        On
    }


    [DataContract]
    public sealed class VolumeStateClass
    {
        public VolumeStateClass(bool? Available = null, int? VolumeLevel = null)
        {
            this.Available = Available;
            this.VolumeLevel = VolumeLevel;
        }

        /// <summary>
        /// Specifies if the volume control is available. Possible values:
        /// 
        /// * ```false``` - The volume control is not available.
        /// * ```true``` - The volume control is available.
        /// </summary>
        [DataMember(Name = "available")]
        public bool? Available { get; init; }

        /// <summary>
        /// Specifies the value of the Volume Control, if available. The value of Volume Control is 
        /// defined in an interval from 1 to 1000 where 1 is the lowest volume level and 1000 is the 
        /// highest volume level. The interval is defined in logarithmic steps, e.g. a volume control 
        /// on a radio. Note: The Volume Control property is vendor-specific and therefore it is not possible to 
        /// guarantee a consistent actual volume level across different vendor hardware.
        /// </summary>
        [DataMember(Name = "volumeLevel")]
        [DataTypes(Minimum = 1, Maximum = 1000)]
        public int? VolumeLevel { get; init; }

    }


    [DataContract]
    public sealed class UPSStateClass
    {
        public UPSStateClass(bool? NotAvailable = null, bool? Low = null, bool? Engaged = null, bool? Powering = null, bool? Recovered = null)
        {
            this.NotAvailable = NotAvailable;
            this.Low = Low;
            this.Engaged = Engaged;
            this.Powering = Powering;
            this.Recovered = Recovered;
        }

        [DataMember(Name = "notAvailable")]
        public bool? NotAvailable { get; init; }

        [DataMember(Name = "low")]
        public bool? Low { get; init; }

        [DataMember(Name = "engaged")]
        public bool? Engaged { get; init; }

        [DataMember(Name = "powering")]
        public bool? Powering { get; init; }

        [DataMember(Name = "recovered")]
        public bool? Recovered { get; init; }

    }


    public enum AudibleAlarmStateEnum
    {
        NotAvailable,
        On,
        Off
    }


    public enum EnhancedAudioControlStateEnum
    {
        NotAvailable,
        PublicAudioManual,
        PublicAudioAuto,
        PublicAudioSemiAuto,
        PrivateAudioManual,
        PrivateAudioAuto,
        PrivateAudioSemiAuto
    }


    public enum EnhancedMicrophoneControlStateEnum
    {
        NotAvailable,
        PublicAudioManual,
        PublicAudioAuto,
        PublicAudioSemiAuto,
        PrivateAudioManual,
        PrivateAudioAuto,
        PrivateAudioSemiAuto
    }


    [DataContract]
    public sealed class MicrophoneVolumeStateClass
    {
        public MicrophoneVolumeStateClass(bool? Available = null, int? VolumeLevel = null)
        {
            this.Available = Available;
            this.VolumeLevel = VolumeLevel;
        }

        /// <summary>
        /// Specifies if the Microphone Volume Control is available. Possible values:
        /// 
        /// * ```false``` - The volume control is not available.
        /// * ```true``` - The volume control is available.
        /// </summary>
        [DataMember(Name = "available")]
        public bool? Available { get; init; }

        /// <summary>
        /// Specifies the value of the Microphone Volume Control, if available. The value of Volume Control is 
        /// defined in an interval from 1 to 1000 where 1 is the lowest volume level and 1000 is the 
        /// highest volume level. The interval is defined in logarithmic steps, e.g. a volume control 
        /// on a radio. Note: The Microphone Volume Control property is vendor-specific and therefore it is 
        /// not possible to guarantee a consistent actual volume level across different vendor hardware.
        /// </summary>
        [DataMember(Name = "volumeLevel")]
        [DataTypes(Minimum = 1, Maximum = 1000)]
        public int? VolumeLevel { get; init; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(OperatorSwitchStateEnum? OperatorSwitch = null, TamperSensorStateEnum? TamperSensor = null, IntTamperSensorStateEnum? IntTamperSensor = null, SeismicSensorStateEnum? SeismicSensor = null, HeatSensorStateEnum? HeatSensor = null, ProximitySensorStateEnum? ProximitySensor = null, AmbientLightSensorStateEnum? AmbientLightSensor = null, EnhancedAudioSensorStateEnum? EnhancedAudioSensor = null, BootSwitchSensorStateEnum? BootSwitchSensor = null, DisplaySensorStateEnum? DisplaySensor = null, OperatorCallButtonSensorStateEnum? OperatorCallButtonSensor = null, HandsetSensorStateEnum? HandsetSensor = null, HeadsetMicrophoneSensorStateEnum? HeadsetMicrophoneSensor = null, FasciaMicrophoneSensorStateEnum? FasciaMicrophoneSensor = null, SafeDoorStateEnum? SafeDoor = null, VandalShieldStateEnum? VandalShield = null, CabinetFrontDoorStateEnum? CabinetFrontDoor = null, CabinetRearDoorStateEnum? CabinetRearDoor = null, CabinetLeftDoorStateEnum? CabinetLeftDoor = null, CabinetRightDoorStateEnum? CabinetRightDoor = null, OpenClosedIndicatorStateEnum? OpenClosedIndicator = null, AudioStateClass Audio = null, HeatingStateEnum? Heating = null, ConsumerDisplayBacklightStateEnum? ConsumerDisplayBacklight = null, SignageDisplayStateEnum? SignageDisplay = null, VolumeStateClass Volume = null, UPSStateClass UPS = null, AudibleAlarmStateEnum? AudibleAlarm = null, EnhancedAudioControlStateEnum? EnhancedAudioControl = null, EnhancedMicrophoneControlStateEnum? EnhancedMicrophoneControl = null, MicrophoneVolumeStateClass MicrophoneVolume = null)
        {
            this.OperatorSwitch = OperatorSwitch;
            this.TamperSensor = TamperSensor;
            this.IntTamperSensor = IntTamperSensor;
            this.SeismicSensor = SeismicSensor;
            this.HeatSensor = HeatSensor;
            this.ProximitySensor = ProximitySensor;
            this.AmbientLightSensor = AmbientLightSensor;
            this.EnhancedAudioSensor = EnhancedAudioSensor;
            this.BootSwitchSensor = BootSwitchSensor;
            this.DisplaySensor = DisplaySensor;
            this.OperatorCallButtonSensor = OperatorCallButtonSensor;
            this.HandsetSensor = HandsetSensor;
            this.HeadsetMicrophoneSensor = HeadsetMicrophoneSensor;
            this.FasciaMicrophoneSensor = FasciaMicrophoneSensor;
            this.SafeDoor = SafeDoor;
            this.VandalShield = VandalShield;
            this.CabinetFrontDoor = CabinetFrontDoor;
            this.CabinetRearDoor = CabinetRearDoor;
            this.CabinetLeftDoor = CabinetLeftDoor;
            this.CabinetRightDoor = CabinetRightDoor;
            this.OpenClosedIndicator = OpenClosedIndicator;
            this.Audio = Audio;
            this.Heating = Heating;
            this.ConsumerDisplayBacklight = ConsumerDisplayBacklight;
            this.SignageDisplay = SignageDisplay;
            this.Volume = Volume;
            this.UPS = UPS;
            this.AudibleAlarm = AudibleAlarm;
            this.EnhancedAudioControl = EnhancedAudioControl;
            this.EnhancedMicrophoneControl = EnhancedMicrophoneControl;
            this.MicrophoneVolume = MicrophoneVolume;
        }

        [DataMember(Name = "operatorSwitch")]
        public OperatorSwitchStateEnum? OperatorSwitch { get; init; }

        [DataMember(Name = "tamperSensor")]
        public TamperSensorStateEnum? TamperSensor { get; init; }

        [DataMember(Name = "intTamperSensor")]
        public IntTamperSensorStateEnum? IntTamperSensor { get; init; }

        [DataMember(Name = "seismicSensor")]
        public SeismicSensorStateEnum? SeismicSensor { get; init; }

        [DataMember(Name = "heatSensor")]
        public HeatSensorStateEnum? HeatSensor { get; init; }

        [DataMember(Name = "proximitySensor")]
        public ProximitySensorStateEnum? ProximitySensor { get; init; }

        [DataMember(Name = "ambientLightSensor")]
        public AmbientLightSensorStateEnum? AmbientLightSensor { get; init; }

        [DataMember(Name = "enhancedAudioSensor")]
        public EnhancedAudioSensorStateEnum? EnhancedAudioSensor { get; init; }

        [DataMember(Name = "bootSwitchSensor")]
        public BootSwitchSensorStateEnum? BootSwitchSensor { get; init; }

        [DataMember(Name = "displaySensor")]
        public DisplaySensorStateEnum? DisplaySensor { get; init; }

        [DataMember(Name = "operatorCallButtonSensor")]
        public OperatorCallButtonSensorStateEnum? OperatorCallButtonSensor { get; init; }

        [DataMember(Name = "handsetSensor")]
        public HandsetSensorStateEnum? HandsetSensor { get; init; }

        [DataMember(Name = "headsetMicrophoneSensor")]
        public HeadsetMicrophoneSensorStateEnum? HeadsetMicrophoneSensor { get; init; }

        [DataMember(Name = "FasciaMicrophoneSensor")]
        public FasciaMicrophoneSensorStateEnum? FasciaMicrophoneSensor { get; init; }

        [DataMember(Name = "safeDoor")]
        public SafeDoorStateEnum? SafeDoor { get; init; }

        [DataMember(Name = "vandalShield")]
        public VandalShieldStateEnum? VandalShield { get; init; }

        [DataMember(Name = "cabinetFrontDoor")]
        public CabinetFrontDoorStateEnum? CabinetFrontDoor { get; init; }

        [DataMember(Name = "cabinetRearDoor")]
        public CabinetRearDoorStateEnum? CabinetRearDoor { get; init; }

        [DataMember(Name = "cabinetLeftDoor")]
        public CabinetLeftDoorStateEnum? CabinetLeftDoor { get; init; }

        [DataMember(Name = "cabinetRightDoor")]
        public CabinetRightDoorStateEnum? CabinetRightDoor { get; init; }

        [DataMember(Name = "openClosedIndicator")]
        public OpenClosedIndicatorStateEnum? OpenClosedIndicator { get; init; }

        [DataMember(Name = "audio")]
        public AudioStateClass Audio { get; init; }

        [DataMember(Name = "heating")]
        public HeatingStateEnum? Heating { get; init; }

        [DataMember(Name = "consumerDisplayBacklight")]
        public ConsumerDisplayBacklightStateEnum? ConsumerDisplayBacklight { get; init; }

        [DataMember(Name = "signageDisplay")]
        public SignageDisplayStateEnum? SignageDisplay { get; init; }

        [DataMember(Name = "volume")]
        public VolumeStateClass Volume { get; init; }

        [DataMember(Name = "UPS")]
        public UPSStateClass UPS { get; init; }

        [DataMember(Name = "audibleAlarm")]
        public AudibleAlarmStateEnum? AudibleAlarm { get; init; }

        [DataMember(Name = "enhancedAudioControl")]
        public EnhancedAudioControlStateEnum? EnhancedAudioControl { get; init; }

        [DataMember(Name = "enhancedMicrophoneControl")]
        public EnhancedMicrophoneControlStateEnum? EnhancedMicrophoneControl { get; init; }

        [DataMember(Name = "microphoneVolume")]
        public MicrophoneVolumeStateClass MicrophoneVolume { get; init; }

    }


    [DataContract]
    public sealed class DoorCapsClass
    {
        public DoorCapsClass(bool? Closed = null, bool? Open = null, bool? Locked = null, bool? Bolted = null, bool? Tampered = null)
        {
            this.Closed = Closed;
            this.Open = Open;
            this.Locked = Locked;
            this.Bolted = Bolted;
            this.Tampered = Tampered;
        }

        /// <summary>
        /// Specifies that the door can report the closed state.
        /// </summary>
        [DataMember(Name = "closed")]
        public bool? Closed { get; init; }

        /// <summary>
        /// Specifies that the door can report the open state.
        /// </summary>
        [DataMember(Name = "open")]
        public bool? Open { get; init; }

        /// <summary>
        /// Specifies that the door can report the locked state.
        /// </summary>
        [DataMember(Name = "locked")]
        public bool? Locked { get; init; }

        /// <summary>
        /// Specifies that the door can report the bolted state.
        /// </summary>
        [DataMember(Name = "bolted")]
        public bool? Bolted { get; init; }

        /// <summary>
        /// Specifies that the door can report the tampered state.
        /// </summary>
        [DataMember(Name = "tampered")]
        public bool? Tampered { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(OperatorSwitchClass OperatorSwitch = null, bool? TamperSensor = null, bool? IntTamperSensor = null, bool? SeismicSensor = null, bool? HeatSensor = null, bool? ProximitySensor = null, bool? AmbientLightSensor = null, EnhancedAudioSensorClass EnhancedAudioSensor = null, bool? BootSwitchSensor = null, bool? DisplaySensor = null, bool? OperatorCallButtonSensor = null, HandsetSensorClass HandsetSensor = null, HeadsetMicrophoneSensorClass HeadsetMicrophoneSensor = null, bool? FasciaMicrophoneSensor = null, DoorCapsClass SafeDoor = null, VandalShieldClass VandalShield = null, DoorCapsClass FrontCabinet = null, DoorCapsClass RearCabinet = null, DoorCapsClass LeftCabinet = null, DoorCapsClass RightCabinet = null, bool? OpenCloseIndicator = null, bool? Audio = null, bool? Heating = null, bool? ConsumerDisplayBacklight = null, bool? SignageDisplay = null, int? Volume = null, UpsClass Ups = null, bool? AudibleAlarm = null, EnhancedAudioControlClass EnhancedAudioControl = null, EnhancedMicrophoneControlStateClass EnhancedMicrophoneControlState = null, int? MicrophoneVolume = null, AutoStartupModeClass AutoStartupMode = null)
        {
            this.OperatorSwitch = OperatorSwitch;
            this.TamperSensor = TamperSensor;
            this.IntTamperSensor = IntTamperSensor;
            this.SeismicSensor = SeismicSensor;
            this.HeatSensor = HeatSensor;
            this.ProximitySensor = ProximitySensor;
            this.AmbientLightSensor = AmbientLightSensor;
            this.EnhancedAudioSensor = EnhancedAudioSensor;
            this.BootSwitchSensor = BootSwitchSensor;
            this.DisplaySensor = DisplaySensor;
            this.OperatorCallButtonSensor = OperatorCallButtonSensor;
            this.HandsetSensor = HandsetSensor;
            this.HeadsetMicrophoneSensor = HeadsetMicrophoneSensor;
            this.FasciaMicrophoneSensor = FasciaMicrophoneSensor;
            this.SafeDoor = SafeDoor;
            this.VandalShield = VandalShield;
            this.FrontCabinet = FrontCabinet;
            this.RearCabinet = RearCabinet;
            this.LeftCabinet = LeftCabinet;
            this.RightCabinet = RightCabinet;
            this.OpenCloseIndicator = OpenCloseIndicator;
            this.Audio = Audio;
            this.Heating = Heating;
            this.ConsumerDisplayBacklight = ConsumerDisplayBacklight;
            this.SignageDisplay = SignageDisplay;
            this.Volume = Volume;
            this.Ups = Ups;
            this.AudibleAlarm = AudibleAlarm;
            this.EnhancedAudioControl = EnhancedAudioControl;
            this.EnhancedMicrophoneControlState = EnhancedMicrophoneControlState;
            this.MicrophoneVolume = MicrophoneVolume;
            this.AutoStartupMode = AutoStartupMode;
        }

        [DataContract]
        public sealed class OperatorSwitchClass
        {
            public OperatorSwitchClass(bool? Run = null, bool? Maintenance = null, bool? Supervisor = null)
            {
                this.Run = Run;
                this.Maintenance = Maintenance;
                this.Supervisor = Supervisor;
            }

            /// <summary>
            /// The switch can be set in Run mode.
            /// </summary>
            [DataMember(Name = "run")]
            public bool? Run { get; init; }

            /// <summary>
            /// The switch can be set in Maintenance mode.
            /// </summary>
            [DataMember(Name = "maintenance")]
            public bool? Maintenance { get; init; }

            /// <summary>
            /// The switch can be set in Supervisor mode.
            /// </summary>
            [DataMember(Name = "supervisor")]
            public bool? Supervisor { get; init; }

        }

        /// <summary>
        /// Specifies which states the Operator Switch can be set to, if available.
        /// </summary>
        [DataMember(Name = "operatorSwitch")]
        public OperatorSwitchClass OperatorSwitch { get; init; }

        /// <summary>
        /// Specifies whether the Tamper Sensor for the terminal is available.
        /// </summary>
        [DataMember(Name = "tamperSensor")]
        public bool? TamperSensor { get; init; }

        /// <summary>
        /// Specifies whether the Internal Tamper Sensor for the terminal is available.
        /// </summary>
        [DataMember(Name = "intTamperSensor")]
        public bool? IntTamperSensor { get; init; }

        /// <summary>
        /// Specifies whether the Seismic Sensor for the terminal is available.
        /// </summary>
        [DataMember(Name = "seismicSensor")]
        public bool? SeismicSensor { get; init; }

        /// <summary>
        /// Specifies whether the Heat Sensor for the terminal is available.
        /// </summary>
        [DataMember(Name = "heatSensor")]
        public bool? HeatSensor { get; init; }

        /// <summary>
        /// Specifies whether the Proximity Sensor for the terminal is available.
        /// </summary>
        [DataMember(Name = "proximitySensor")]
        public bool? ProximitySensor { get; init; }

        /// <summary>
        /// Specifies whether the Ambient Light Sensor for the terminal is available.
        /// </summary>
        [DataMember(Name = "ambientLightSensor")]
        public bool? AmbientLightSensor { get; init; }

        [DataContract]
        public sealed class EnhancedAudioSensorClass
        {
            public EnhancedAudioSensorClass(bool? Manual = null, bool? Auto = null, bool? SemiAuto = null, bool? Bidirectional = null)
            {
                this.Manual = Manual;
                this.Auto = Auto;
                this.SemiAuto = SemiAuto;
                this.Bidirectional = Bidirectional;
            }

            /// <summary>
            /// The Audio Jack is available and supports manual mode.
            /// </summary>
            [DataMember(Name = "manual")]
            public bool? Manual { get; init; }

            /// <summary>
            /// The Audio Jack is available and supports auto mode.
            /// </summary>
            [DataMember(Name = "auto")]
            public bool? Auto { get; init; }

            /// <summary>
            /// The Audio Jack is available and supports semi-auto mode.
            /// </summary>
            [DataMember(Name = "semiAuto")]
            public bool? SemiAuto { get; init; }

            /// <summary>
            /// The Audio Jack is available and can support headphones that have an integrated microphone via a
            /// single jack.
            /// </summary>
            [DataMember(Name = "bidirectional")]
            public bool? Bidirectional { get; init; }

        }

        /// <summary>
        /// Specifies which mode the Audio Jack supports, if present.
        /// </summary>
        [DataMember(Name = "enhancedAudioSensor")]
        public EnhancedAudioSensorClass EnhancedAudioSensor { get; init; }

        /// <summary>
        /// Specifies whether the Boot Switch Sensor for the terminal is available.
        /// </summary>
        [DataMember(Name = "bootSwitchSensor")]
        public bool? BootSwitchSensor { get; init; }

        /// <summary>
        /// Specifies whether the Consumer Display Sensor is available.
        /// </summary>
        [DataMember(Name = "displaySensor")]
        public bool? DisplaySensor { get; init; }

        /// <summary>
        /// Specifies whether the Operator Call Button is available. The Operator Call Button does not actually
        /// call the operator but just sends a signal to the application.
        /// </summary>
        [DataMember(Name = "operatorCallButtonSensor")]
        public bool? OperatorCallButtonSensor { get; init; }

        [DataContract]
        public sealed class HandsetSensorClass
        {
            public HandsetSensorClass(bool? Manual = null, bool? Auto = null, bool? SemiAuto = null, bool? Microphone = null)
            {
                this.Manual = Manual;
                this.Auto = Auto;
                this.SemiAuto = SemiAuto;
                this.Microphone = Microphone;
            }

            /// <summary>
            /// The Handset is available and it supports manual mode.
            /// </summary>
            [DataMember(Name = "manual")]
            public bool? Manual { get; init; }

            /// <summary>
            /// The Handset is available and it supports auto mode.
            /// </summary>
            [DataMember(Name = "auto")]
            public bool? Auto { get; init; }

            /// <summary>
            /// The Handset is available and it supports semi-auto mode.
            /// </summary>
            [DataMember(Name = "semiAuto")]
            public bool? SemiAuto { get; init; }

            /// <summary>
            /// The Handset is available and contains an embedded\tmicrophone for audio input.
            /// </summary>
            [DataMember(Name = "microphone")]
            public bool? Microphone { get; init; }

        }

        /// <summary>
        /// Specifies which mode the Handset supports, if present.
        /// </summary>
        [DataMember(Name = "handsetSensor")]
        public HandsetSensorClass HandsetSensor { get; init; }

        [DataContract]
        public sealed class HeadsetMicrophoneSensorClass
        {
            public HeadsetMicrophoneSensorClass(bool? Manual = null, bool? Auto = null, bool? SemiAuto = null)
            {
                this.Manual = Manual;
                this.Auto = Auto;
                this.SemiAuto = SemiAuto;
            }

            /// <summary>
            /// The Microphone Jack is available and it supports manual mode.
            /// </summary>
            [DataMember(Name = "manual")]
            public bool? Manual { get; init; }

            /// <summary>
            /// The Microphone Jack is available and it supports auto mode.
            /// </summary>
            [DataMember(Name = "auto")]
            public bool? Auto { get; init; }

            /// <summary>
            /// The Microphone Jack is available and it supports semi-auto mode.
            /// </summary>
            [DataMember(Name = "semiAuto")]
            public bool? SemiAuto { get; init; }

        }

        /// <summary>
        /// Specifies whether the Microphone Jack is present, and if so, which modes it supports. If the 
        /// enhancedAudio capability indicates the presence of a bi-directional Audio Jack then both sensors 
        /// reference the same physical jack.
        /// </summary>
        [DataMember(Name = "headsetMicrophoneSensor")]
        public HeadsetMicrophoneSensorClass HeadsetMicrophoneSensor { get; init; }

        /// <summary>
        /// Specifies whether a Fascia Microphone (for public audio input) is present.
        /// </summary>
        [DataMember(Name = "fasciaMicrophoneSensor")]
        public bool? FasciaMicrophoneSensor { get; init; }

        /// <summary>
        /// Specifies whether the Safe Door is available, and if so, which states it supports.
        /// </summary>
        [DataMember(Name = "safeDoor")]
        public DoorCapsClass SafeDoor { get; init; }

        [DataContract]
        public sealed class VandalShieldClass
        {
            public VandalShieldClass(bool? Closed = null, bool? Open = null, bool? Locked = null, bool? Service = null, bool? Keyboard = null, bool? Tampered = null)
            {
                this.Closed = Closed;
                this.Open = Open;
                this.Locked = Locked;
                this.Service = Service;
                this.Keyboard = Keyboard;
                this.Tampered = Tampered;
            }

            /// <summary>
            /// The Vandal Shield can be closed.
            /// </summary>
            [DataMember(Name = "closed")]
            public bool? Closed { get; init; }

            /// <summary>
            /// The Vandal Shield can be open.
            /// </summary>
            [DataMember(Name = "open")]
            public bool? Open { get; init; }

            /// <summary>
            /// The Vandal Shield can be locked.
            /// </summary>
            [DataMember(Name = "locked")]
            public bool? Locked { get; init; }

            /// <summary>
            /// The Vandal Shield can be in the service position.
            /// </summary>
            [DataMember(Name = "service")]
            public bool? Service { get; init; }

            /// <summary>
            /// The Vandal Shield can be in a position that permits access to the keyboard.
            /// </summary>
            [DataMember(Name = "keyboard")]
            public bool? Keyboard { get; init; }

            /// <summary>
            /// The Vandal Shield can detect potential tampering.
            /// </summary>
            [DataMember(Name = "tampered")]
            public bool? Tampered { get; init; }

        }

        /// <summary>
        /// Specifies the states the Vandal Shield can support, if available.
        /// </summary>
        [DataMember(Name = "vandalShield")]
        public VandalShieldClass VandalShield { get; init; }

        /// <summary>
        /// Specifies whether at least one Front Cabinet Door is available, and if so, which states they 
        /// support.
        /// </summary>
        [DataMember(Name = "frontCabinet")]
        public DoorCapsClass FrontCabinet { get; init; }

        /// <summary>
        /// Specifies whether at least one Rear Cabinet Door is available, and if so, which states they 
        /// support.
        /// </summary>
        [DataMember(Name = "rearCabinet")]
        public DoorCapsClass RearCabinet { get; init; }

        /// <summary>
        /// Specifies whether at least one left Cabinet Door is available, and if so, which states they 
        /// support.
        /// </summary>
        [DataMember(Name = "leftCabinet")]
        public DoorCapsClass LeftCabinet { get; init; }

        /// <summary>
        /// Specifies whether at least one right Cabinet Door is available, and if so, which states they 
        /// support.
        /// </summary>
        [DataMember(Name = "rightCabinet")]
        public DoorCapsClass RightCabinet { get; init; }

        /// <summary>
        /// Specifies whether the Open/Closed Indicator is available.
        /// </summary>
        [DataMember(Name = "openCloseIndicator")]
        public bool? OpenCloseIndicator { get; init; }

        /// <summary>
        /// Specifies whether the Audio Indicator device is available.
        /// </summary>
        [DataMember(Name = "audio")]
        public bool? Audio { get; init; }

        /// <summary>
        /// Specifies whether the Internal Heating device is available.
        /// </summary>
        [DataMember(Name = "heating")]
        public bool? Heating { get; init; }

        /// <summary>
        /// Specifies whether the Consumer Display Backlight is available.
        /// </summary>
        [DataMember(Name = "consumerDisplayBacklight")]
        public bool? ConsumerDisplayBacklight { get; init; }

        /// <summary>
        /// Specifies whether the Signage Display is available.
        /// </summary>
        [DataMember(Name = "signageDisplay")]
        public bool? SignageDisplay { get; init; }

        /// <summary>
        /// Specifies the Volume Control increment/decrement value recommended by the vendor.
        /// </summary>
        [DataMember(Name = "volume")]
        [DataTypes(Minimum = 1, Maximum = 1000)]
        public int? Volume { get; init; }

        [DataContract]
        public sealed class UpsClass
        {
            public UpsClass(bool? Low = null, bool? Engaged = null, bool? Powering = null, bool? Recovered = null)
            {
                this.Low = Low;
                this.Engaged = Engaged;
                this.Powering = Powering;
                this.Recovered = Recovered;
            }

            /// <summary>
            /// The UPS can indicate that its charge level is low.
            /// </summary>
            [DataMember(Name = "low")]
            public bool? Low { get; init; }

            /// <summary>
            /// The UPS can be engaged and disengaged by the application.
            /// </summary>
            [DataMember(Name = "engaged")]
            public bool? Engaged { get; init; }

            /// <summary>
            /// The UPS can indicate that it is powering the system while the main power supply is off.
            /// </summary>
            [DataMember(Name = "powering")]
            public bool? Powering { get; init; }

            /// <summary>
            /// The UPS can indicate that it was engaged when the main power went off.
            /// </summary>
            [DataMember(Name = "recovered")]
            public bool? Recovered { get; init; }

        }

        /// <summary>
        /// Specifies what states the UPS device supports as a combination of the following values:
        /// </summary>
        [DataMember(Name = "ups")]
        public UpsClass Ups { get; init; }

        /// <summary>
        /// Specifies whether the Audible Alarm is available.
        /// </summary>
        [DataMember(Name = "audibleAlarm")]
        public bool? AudibleAlarm { get; init; }

        [DataContract]
        public sealed class EnhancedAudioControlClass
        {
            public EnhancedAudioControlClass(bool? HeadsetDetection = null, bool? ModeControllable = null)
            {
                this.HeadsetDetection = HeadsetDetection;
                this.ModeControllable = ModeControllable;
            }

            /// <summary>
            /// The Enhanced Audio Controller is supports Privacy Device activation/deactivation. The device is able 
            /// to report events to indicate Privacy Device activation/deactivation.
            /// </summary>
            [DataMember(Name = "headsetDetection")]
            public bool? HeadsetDetection { get; init; }

            /// <summary>
            /// The Enhanced Audio Controller supports application control of the Privacy Device mode via the 
            /// setAuxiliaries command.
            /// </summary>
            [DataMember(Name = "modeControllable")]
            public bool? ModeControllable { get; init; }

        }

        /// <summary>
        /// Specifies the modes the Enhanced Audio Controller can support. The Enhanced Audio Controller controls how 
        /// private and public audio are broadcast when the headset is inserted into/removed from the audio jack and 
        /// when the handset is off-hook/on-hook. In the following Privacy Device is used to refer to either the 
        /// headset or handset. The modes it supports are specified as a combination of the following values:
        /// </summary>
        [DataMember(Name = "enhancedAudioControl")]
        public EnhancedAudioControlClass EnhancedAudioControl { get; init; }

        [DataContract]
        public sealed class EnhancedMicrophoneControlStateClass
        {
            public EnhancedMicrophoneControlStateClass(bool? HeadsetDetection = null, bool? ModeControllable = null)
            {
                this.HeadsetDetection = HeadsetDetection;
                this.ModeControllable = ModeControllable;
            }

            /// <summary>
            /// The Enhanced Microphone Controller supports Privacy Device activation/deactivation. The device is 
            /// able to report events to indicate Privacy Device activation/deactivation.
            /// </summary>
            [DataMember(Name = "headsetDetection")]
            public bool? HeadsetDetection { get; init; }

            /// <summary>
            /// The Enhanced Microphone Controller supports application control of the Privacy Device mode via 
            /// the setAuxiliaries command.
            /// </summary>
            [DataMember(Name = "modeControllable")]
            public bool? ModeControllable { get; init; }

        }

        /// <summary>
        /// Specifies the modes the Enhanced Microphone Controller can support. The Enhanced Microphone Controller 
        /// controls how private and public audio input are transmitted when the headset is inserted into/removed 
        /// from the audio jack and when the handset is off-hook/on-hook. In the following Privacy Device is used 
        /// to refer to either the headset or handset. The modes it supports are specified as a combination of the 
        /// following values:
        /// </summary>
        [DataMember(Name = "enhancedMicrophoneControlState")]
        public EnhancedMicrophoneControlStateClass EnhancedMicrophoneControlState { get; init; }

        /// <summary>
        /// Specifies the Microphone Volume Control increment/decrement value recommended by the vendor.
        /// </summary>
        [DataMember(Name = "microphoneVolume")]
        [DataTypes(Minimum = 1, Maximum = 1000)]
        public int? MicrophoneVolume { get; init; }

        [DataContract]
        public sealed class AutoStartupModeClass
        {
            public AutoStartupModeClass(bool? Specific = null, bool? Daily = null, bool? Weekly = null)
            {
                this.Specific = Specific;
                this.Daily = Daily;
                this.Weekly = Weekly;
            }

            /// <summary>
            /// The device supports one-time auto start-up on a specific date at a specific time.
            /// </summary>
            [DataMember(Name = "specific")]
            public bool? Specific { get; init; }

            /// <summary>
            /// TThe device supports auto start-up every day at a specific time.
            /// </summary>
            [DataMember(Name = "daily")]
            public bool? Daily { get; init; }

            /// <summary>
            /// The device supports auto start-up at a specified time on a specific day of every week.
            /// </summary>
            [DataMember(Name = "weekly")]
            public bool? Weekly { get; init; }

        }

        /// <summary>
        /// Specifies which mode of the auto start-up control is supported. Specified as a combination of 
        /// the following values:
        /// </summary>
        [DataMember(Name = "autoStartupMode")]
        public AutoStartupModeClass AutoStartupMode { get; init; }

    }


    [DataContract]
    public sealed class SystemTimeClass
    {
        public SystemTimeClass(int? Year = null, int? Month = null, DayOfWeekEnum? DayOfWeek = null, int? Day = null, int? Hour = null, int? Minute = null)
        {
            this.Year = Year;
            this.Month = Month;
            this.DayOfWeek = DayOfWeek;
            this.Day = Day;
            this.Hour = Hour;
            this.Minute = Minute;
        }

        /// <summary>
        /// Specifies the year. The value should be ignored if it is not relevant to the mode value.
        /// </summary>
        [DataMember(Name = "year")]
        [DataTypes(Minimum = 1601, Maximum = 30827)]
        public int? Year { get; init; }

        /// <summary>
        /// Specifies the month. The value should be ignored if it is not relevant to the mode value.
        /// </summary>
        [DataMember(Name = "month")]
        [DataTypes(Minimum = 1, Maximum = 12)]
        public int? Month { get; init; }

        public enum DayOfWeekEnum
        {
            Saturday,
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday
        }

        /// <summary>
        /// Specifies the day of the week. The following values are possible:
        /// 
        /// * ```Saturday``` - the day of the week is Saturday.
        /// * ```Sunday``` - the day of the week is Sunday.
        /// * ```Monday``` - the day of the week is Monday.
        /// * ```Tuesday``` - the day of the week is Tuesday.
        /// * ```Wednesday``` - the day of the week is Wednesday.
        /// * ```Thursday``` - the day of the week is Thursday.
        /// * ```Friday``` - the day of the week is Friday.
        /// </summary>
        [DataMember(Name = "dayOfWeek")]
        public DayOfWeekEnum? DayOfWeek { get; init; }

        /// <summary>
        /// Specifies the day. The value should be ignored if it is not relevant to the mode value.
        /// </summary>
        [DataMember(Name = "day")]
        [DataTypes(Minimum = 1, Maximum = 31)]
        public int? Day { get; init; }

        /// <summary>
        /// Specifies the hour. The value should be ignored if it is not relevant to the mode value.
        /// </summary>
        [DataMember(Name = "hour")]
        [DataTypes(Minimum = 0, Maximum = 23)]
        public int? Hour { get; init; }

        /// <summary>
        /// Specifies the minute. The value should be ignored if it is not relevant to the mode value.
        /// </summary>
        [DataMember(Name = "minute")]
        [DataTypes(Minimum = 0, Maximum = 59)]
        public int? Minute { get; init; }

    }


}
