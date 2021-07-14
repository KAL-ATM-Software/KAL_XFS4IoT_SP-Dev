/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT SensorsAndIndicators interface.
 * SensorsAndIndicatorsSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.SensorsAndIndicators
{

    [DataContract]
    public sealed class GuideLightCapabilitiesClass
    {
        public GuideLightCapabilitiesClass(FlashRateEnum? FlashRate = null, ColourEnum? Colour = null, DirectionEnum? Direction = null, PositionEnum? Position = null)
        {
            this.FlashRate = FlashRate;
            this.Colour = Colour;
            this.Direction = Direction;
            this.Position = Position;
        }

        public enum FlashRateEnum
        {
            NotAvailable,
            Off,
            Slow,
            Medium,
            Quick,
            Continuous
        }

        /// <summary>
        /// Indicates the guidelight flash rate. The following values are possible:
        /// "notAvailable": The light indicator is not available.
        /// "off": The light can be turned off.
        /// "slow": The light can blink slowly.
        /// "medium": The light can blink medium frequency.
        /// "quick": The light can blink quickly.
        /// "continuous":The light can be continuous (steady).
        /// </summary>
        [DataMember(Name = "flashRate")]
        public FlashRateEnum? FlashRate { get; init; }

        public enum ColourEnum
        {
            Red,
            Green,
            Yellow,
            Blue,
            Cyan,
            Magenta,
            White
        }

        /// <summary>
        /// Indicates the guidelight colour. The following values are possible:
        /// "defaultColor": The light indicator is not available.
        /// "red": The light can be red.
        /// "green": The light can be green.
        /// "yellow": The light can be yellow.
        /// "blue": The light can be blue.
        /// "cyan":The light can be cyan.
        /// "magenta": The light can be magenta.
        /// "white":The light can be white.
        /// </summary>
        [DataMember(Name = "colour")]
        public ColourEnum? Colour { get; init; }

        public enum DirectionEnum
        {
            Entry,
            Exit
        }

        /// <summary>
        /// Indicates the guidelight direction. The following values are possible:
        /// "entry": The light can  indicate entry.
        /// "exit": The light can indicate exit.
        /// </summary>
        [DataMember(Name = "direction")]
        public DirectionEnum? Direction { get; init; }

        public enum PositionEnum
        {
            Default,
            Left,
            Right,
            Center,
            Top,
            Bottom,
            Front,
            Rear
        }

        /// <summary>
        /// Indicates the guidelight position. The following values are possible:
        /// "default": The default position.
        /// "left": The left position.
        /// "right": The right position.
        /// "center": The center position.
        ///  "top": The top position.
        /// "bottom": The bottom position.
        /// "front": The front position.
        /// "rear": The rear position.
        /// </summary>
        [DataMember(Name = "position")]
        public PositionEnum? Position { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(SensorTypeClass SensorType = null)
        {
            this.SensorType = SensorType;
        }

        [DataContract]
        public sealed class SensorTypeClass
        {
            public SensorTypeClass(bool? Sensors = null, bool? Doors = null, bool? Indicators = null, bool? Auxiliary = null, bool? Guidelights = null, OperatorSwitchEnum? OperatorSwitch = null, TamperSensorEnum? TamperSensor = null, IntTamperSensorEnum? IntTamperSensor = null, SeismicSensorEnum? SeismicSensor = null, HeatSensorEnum? HeatSensor = null, ProximitySensorEnum? ProximitySensor = null, AmbientLightSensorEnum? AmbientLightSensor = null, EnhancedAudioSensorClass EnhancedAudioSensor = null, BootSwitchSensorEnum? BootSwitchSensor = null, DisplaySensorEnum? DisplaySensor = null, OperatorCallButtonSensorEnum? OperatorCallButtonSensor = null, HandsetSensorClass HandsetSensor = null, List<bool> GeneralInputPort = null, HeadsetMicrophoneSensorClass HeadsetMicrophoneSensor = null, FasciaMicrophoneSensorEnum? FasciaMicrophoneSensor = null, CabinetDoorClass CabinetDoor = null, SafeDoorClass SafeDoor = null, VandalShieldClass VandalShield = null, FrontCabinetClass FrontCabinet = null, RearCabinetClass RearCabinet = null, LeftCabinetClass LeftCabinet = null, RightCabinetClass RightCabinet = null, OpenCloseIndicatorEnum? OpenCloseIndicator = null, FasciaLightEnum? FasciaLight = null, AudioEnum? Audio = null, HeatingEnum? Heating = null, ConsumerDisplayBacklightEnum? ConsumerDisplayBacklight = null, SignageDisplayEnum? SignageDisplay = null, List<bool> TransactionIndicator = null, List<bool> GeneralOutputPort = null, VolumeClass Volume = null, UPSClass UPS = null, RemoteStatusMonitorEnum? RemoteStatusMonitor = null, AudibleAlarmEnum? AudibleAlarm = null, EnhancedAudioControlClass EnhancedAudioControl = null, EnhancedMicrophoneControlStateClass EnhancedMicrophoneControlState = null, MicrophoneVolumeClass MicrophoneVolume = null, AutoStartupModeClass AutoStartupMode = null, Dictionary<string, GuideLightCapabilitiesClass> GuideLights = null)
            {
                this.Sensors = Sensors;
                this.Doors = Doors;
                this.Indicators = Indicators;
                this.Auxiliary = Auxiliary;
                this.Guidelights = Guidelights;
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
                this.GeneralInputPort = GeneralInputPort;
                this.HeadsetMicrophoneSensor = HeadsetMicrophoneSensor;
                this.FasciaMicrophoneSensor = FasciaMicrophoneSensor;
                this.CabinetDoor = CabinetDoor;
                this.SafeDoor = SafeDoor;
                this.VandalShield = VandalShield;
                this.FrontCabinet = FrontCabinet;
                this.RearCabinet = RearCabinet;
                this.LeftCabinet = LeftCabinet;
                this.RightCabinet = RightCabinet;
                this.OpenCloseIndicator = OpenCloseIndicator;
                this.FasciaLight = FasciaLight;
                this.Audio = Audio;
                this.Heating = Heating;
                this.ConsumerDisplayBacklight = ConsumerDisplayBacklight;
                this.SignageDisplay = SignageDisplay;
                this.TransactionIndicator = TransactionIndicator;
                this.GeneralOutputPort = GeneralOutputPort;
                this.Volume = Volume;
                this.UPS = UPS;
                this.RemoteStatusMonitor = RemoteStatusMonitor;
                this.AudibleAlarm = AudibleAlarm;
                this.EnhancedAudioControl = EnhancedAudioControl;
                this.EnhancedMicrophoneControlState = EnhancedMicrophoneControlState;
                this.MicrophoneVolume = MicrophoneVolume;
                this.AutoStartupMode = AutoStartupMode;
                this.GuideLights = GuideLights;
            }

            /// <summary>
            /// The device supports input sensors.
            /// </summary>
            [DataMember(Name = "sensors")]
            public bool? Sensors { get; init; }

            /// <summary>
            /// The device supports door sensors.
            /// </summary>
            [DataMember(Name = "doors")]
            public bool? Doors { get; init; }

            /// <summary>
            /// The device supports indicators.
            /// </summary>
            [DataMember(Name = "indicators")]
            public bool? Indicators { get; init; }

            /// <summary>
            /// The device supports auxiliary indicators.
            /// </summary>
            [DataMember(Name = "auxiliary")]
            public bool? Auxiliary { get; init; }

            /// <summary>
            /// The device supports guidance lights.
            /// </summary>
            [DataMember(Name = "guidelights")]
            public bool? Guidelights { get; init; }

            public enum OperatorSwitchEnum
            {
                NotAvailable,
                Run,
                Maintenance,
                Supervisor
            }

            /// <summary>
            /// Specifies the Operator switch.
            /// </summary>
            [DataMember(Name = "operatorSwitch")]
            public OperatorSwitchEnum? OperatorSwitch { get; init; }

            public enum TamperSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the Tamper sensor.
            /// </summary>
            [DataMember(Name = "tamperSensor")]
            public TamperSensorEnum? TamperSensor { get; init; }

            public enum IntTamperSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the internal Tamper sensor.
            /// </summary>
            [DataMember(Name = "intTamperSensor")]
            public IntTamperSensorEnum? IntTamperSensor { get; init; }

            public enum SeismicSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the Seismic sensor.
            /// </summary>
            [DataMember(Name = "seismicSensor")]
            public SeismicSensorEnum? SeismicSensor { get; init; }

            public enum HeatSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the heat sensor.
            /// </summary>
            [DataMember(Name = "heatSensor")]
            public HeatSensorEnum? HeatSensor { get; init; }

            public enum ProximitySensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the proximity sensor.
            /// </summary>
            [DataMember(Name = "proximitySensor")]
            public ProximitySensorEnum? ProximitySensor { get; init; }

            public enum AmbientLightSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the ambient light sensor.
            /// </summary>
            [DataMember(Name = "ambientLightSensor")]
            public AmbientLightSensorEnum? AmbientLightSensor { get; init; }

            [DataContract]
            public sealed class EnhancedAudioSensorClass
            {
                public EnhancedAudioSensorClass(bool? Available = null, bool? Manual = null, bool? Auto = null, bool? SemiAuto = null, bool? Bidirectional = null)
                {
                    this.Available = Available;
                    this.Manual = Manual;
                    this.Auto = Auto;
                    this.SemiAuto = SemiAuto;
                    this.Bidirectional = Bidirectional;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "manual")]
                public bool? Manual { get; init; }


                [DataMember(Name = "auto")]
                public bool? Auto { get; init; }


                [DataMember(Name = "semiAuto")]
                public bool? SemiAuto { get; init; }


                [DataMember(Name = "bidirectional")]
                public bool? Bidirectional { get; init; }

            }

            /// <summary>
            /// Specifies whether the Audio Jack is present, and if so, which modes it supports.
            /// </summary>
            [DataMember(Name = "enhancedAudioSensor")]
            public EnhancedAudioSensorClass EnhancedAudioSensor { get; init; }

            public enum BootSwitchSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the boot switch sensor.
            /// </summary>
            [DataMember(Name = "bootSwitchSensor")]
            public BootSwitchSensorEnum? BootSwitchSensor { get; init; }

            public enum DisplaySensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the Consumer Display.
            /// </summary>
            [DataMember(Name = "displaySensor")]
            public DisplaySensorEnum? DisplaySensor { get; init; }

            public enum OperatorCallButtonSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Operator Call Button is available. The Operator Call Button does not actually call the operator but just sends a signal to the application.
            /// </summary>
            [DataMember(Name = "operatorCallButtonSensor")]
            public OperatorCallButtonSensorEnum? OperatorCallButtonSensor { get; init; }

            [DataContract]
            public sealed class HandsetSensorClass
            {
                public HandsetSensorClass(bool? Available = null, bool? Manual = null, bool? Auto = null, bool? SemiAuto = null, bool? Microphone = null)
                {
                    this.Available = Available;
                    this.Manual = Manual;
                    this.Auto = Auto;
                    this.SemiAuto = SemiAuto;
                    this.Microphone = Microphone;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "manual")]
                public bool? Manual { get; init; }


                [DataMember(Name = "auto")]
                public bool? Auto { get; init; }


                [DataMember(Name = "semiAuto")]
                public bool? SemiAuto { get; init; }


                [DataMember(Name = "microphone")]
                public bool? Microphone { get; init; }

            }

            /// <summary>
            /// Specifies whether the Handset is present, and if so, which modes it supports.
            /// </summary>
            [DataMember(Name = "handsetSensor")]
            public HandsetSensorClass HandsetSensor { get; init; }

            /// <summary>
            /// Specifies whether the vendor dependent General-Purpose Input Ports are available. This value is an array and each index represents one General-Purpose Input Port.
            /// </summary>
            [DataMember(Name = "generalInputPort")]
            public List<bool> GeneralInputPort { get; init; }

            [DataContract]
            public sealed class HeadsetMicrophoneSensorClass
            {
                public HeadsetMicrophoneSensorClass(bool? Available = null, bool? Manual = null, bool? Auto = null, bool? SemiAuto = null)
                {
                    this.Available = Available;
                    this.Manual = Manual;
                    this.Auto = Auto;
                    this.SemiAuto = SemiAuto;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "manual")]
                public bool? Manual { get; init; }


                [DataMember(Name = "auto")]
                public bool? Auto { get; init; }


                [DataMember(Name = "semiAuto")]
                public bool? SemiAuto { get; init; }

            }

            /// <summary>
            /// Specifies whether the Microphone Jack is present, and if so, which modes it supports.
            /// </summary>
            [DataMember(Name = "headsetMicrophoneSensor")]
            public HeadsetMicrophoneSensorClass HeadsetMicrophoneSensor { get; init; }

            public enum FasciaMicrophoneSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether a Fascia Microphone (for public audio input) is present.
            /// </summary>
            [DataMember(Name = "fasciaMicrophoneSensor")]
            public FasciaMicrophoneSensorEnum? FasciaMicrophoneSensor { get; init; }

            [DataContract]
            public sealed class CabinetDoorClass
            {
                public CabinetDoorClass(bool? Available = null, bool? Closed = null, bool? Open = null, bool? Locked = null, bool? Bolted = null, bool? Tampered = null)
                {
                    this.Available = Available;
                    this.Closed = Closed;
                    this.Open = Open;
                    this.Locked = Locked;
                    this.Bolted = Bolted;
                    this.Tampered = Tampered;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; init; }


                [DataMember(Name = "open")]
                public bool? Open { get; init; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; init; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; init; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; init; }

            }

            /// <summary>
            /// Specifies whether at least one Cabinet Doors is available, and if so, which states they can take.
            /// </summary>
            [DataMember(Name = "cabinetDoor")]
            public CabinetDoorClass CabinetDoor { get; init; }

            [DataContract]
            public sealed class SafeDoorClass
            {
                public SafeDoorClass(bool? Available = null, bool? Closed = null, bool? Open = null, bool? Locked = null, bool? Bolted = null, bool? Tampered = null)
                {
                    this.Available = Available;
                    this.Closed = Closed;
                    this.Open = Open;
                    this.Locked = Locked;
                    this.Bolted = Bolted;
                    this.Tampered = Tampered;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; init; }


                [DataMember(Name = "open")]
                public bool? Open { get; init; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; init; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; init; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; init; }

            }

            /// <summary>
            /// Specifies whether the safe Door is available, and if so, which states it can take.
            /// </summary>
            [DataMember(Name = "safeDoor")]
            public SafeDoorClass SafeDoor { get; init; }

            [DataContract]
            public sealed class VandalShieldClass
            {
                public VandalShieldClass(bool? Available = null, bool? Closed = null, bool? Open = null, bool? Locked = null, bool? Service = null, bool? Keyboard = null, bool? Tampered = null)
                {
                    this.Available = Available;
                    this.Closed = Closed;
                    this.Open = Open;
                    this.Locked = Locked;
                    this.Service = Service;
                    this.Keyboard = Keyboard;
                    this.Tampered = Tampered;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; init; }


                [DataMember(Name = "open")]
                public bool? Open { get; init; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; init; }


                [DataMember(Name = "service")]
                public bool? Service { get; init; }


                [DataMember(Name = "keyboard")]
                public bool? Keyboard { get; init; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; init; }

            }

            /// <summary>
            /// Specifies whether the Vandal Shield is available, and if so, which states it can take.
            /// </summary>
            [DataMember(Name = "vandalShield")]
            public VandalShieldClass VandalShield { get; init; }

            [DataContract]
            public sealed class FrontCabinetClass
            {
                public FrontCabinetClass(bool? Available = null, bool? Closed = null, bool? Open = null, bool? Locked = null, bool? Bolted = null, bool? Tampered = null)
                {
                    this.Available = Available;
                    this.Closed = Closed;
                    this.Open = Open;
                    this.Locked = Locked;
                    this.Bolted = Bolted;
                    this.Tampered = Tampered;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; init; }


                [DataMember(Name = "open")]
                public bool? Open { get; init; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; init; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; init; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; init; }

            }

            /// <summary>
            /// Specifies whether at least one Front Cabinet Door is available, and if so, which states they can take.
            /// </summary>
            [DataMember(Name = "frontCabinet")]
            public FrontCabinetClass FrontCabinet { get; init; }

            [DataContract]
            public sealed class RearCabinetClass
            {
                public RearCabinetClass(bool? Available = null, bool? Closed = null, bool? Open = null, bool? Locked = null, bool? Bolted = null, bool? Tampered = null)
                {
                    this.Available = Available;
                    this.Closed = Closed;
                    this.Open = Open;
                    this.Locked = Locked;
                    this.Bolted = Bolted;
                    this.Tampered = Tampered;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; init; }


                [DataMember(Name = "open")]
                public bool? Open { get; init; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; init; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; init; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; init; }

            }

            /// <summary>
            /// Specifies whether at least one rear Cabinet Door is available, and if so, which states they can take.
            /// </summary>
            [DataMember(Name = "rearCabinet")]
            public RearCabinetClass RearCabinet { get; init; }

            [DataContract]
            public sealed class LeftCabinetClass
            {
                public LeftCabinetClass(bool? Available = null, bool? Closed = null, bool? Open = null, bool? Locked = null, bool? Bolted = null, bool? Tampered = null)
                {
                    this.Available = Available;
                    this.Closed = Closed;
                    this.Open = Open;
                    this.Locked = Locked;
                    this.Bolted = Bolted;
                    this.Tampered = Tampered;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; init; }


                [DataMember(Name = "open")]
                public bool? Open { get; init; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; init; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; init; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; init; }

            }

            /// <summary>
            /// Specifies whether at least one left Cabinet Door is available, and if so, which states they can take.
            /// </summary>
            [DataMember(Name = "leftCabinet")]
            public LeftCabinetClass LeftCabinet { get; init; }

            [DataContract]
            public sealed class RightCabinetClass
            {
                public RightCabinetClass(bool? Available = null, bool? Closed = null, bool? Open = null, bool? Locked = null, bool? Bolted = null, bool? Tampered = null)
                {
                    this.Available = Available;
                    this.Closed = Closed;
                    this.Open = Open;
                    this.Locked = Locked;
                    this.Bolted = Bolted;
                    this.Tampered = Tampered;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; init; }


                [DataMember(Name = "open")]
                public bool? Open { get; init; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; init; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; init; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; init; }

            }

            /// <summary>
            /// Specifies whether at least one right Cabinet Door is available, and if so, which states they can take.
            /// </summary>
            [DataMember(Name = "rightCabinet")]
            public RightCabinetClass RightCabinet { get; init; }

            public enum OpenCloseIndicatorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Open/Closed Indicator is available.
            /// </summary>
            [DataMember(Name = "openCloseIndicator")]
            public OpenCloseIndicatorEnum? OpenCloseIndicator { get; init; }

            public enum FasciaLightEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the fascia light is available.
            /// </summary>
            [DataMember(Name = "fasciaLight")]
            public FasciaLightEnum? FasciaLight { get; init; }

            public enum AudioEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Audio Indicator device is available.
            /// </summary>
            [DataMember(Name = "audio")]
            public AudioEnum? Audio { get; init; }

            public enum HeatingEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the internal Heating device is available.
            /// </summary>
            [DataMember(Name = "heating")]
            public HeatingEnum? Heating { get; init; }

            public enum ConsumerDisplayBacklightEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Consumer Display Backlight is available.
            /// </summary>
            [DataMember(Name = "consumerDisplayBacklight")]
            public ConsumerDisplayBacklightEnum? ConsumerDisplayBacklight { get; init; }

            public enum SignageDisplayEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Signage Display is available.
            /// </summary>
            [DataMember(Name = "signageDisplay")]
            public SignageDisplayEnum? SignageDisplay { get; init; }

            /// <summary>
            /// Specifies whether the Transaction Indicators are available as an array. Each index of this array represents one Transaction Indicator .
            /// </summary>
            [DataMember(Name = "transactionIndicator")]
            public List<bool> TransactionIndicator { get; init; }

            /// <summary>
            /// Specifies whether the vendor dependent General-Purpose Output Ports are available. This value is an array and each index represents one General-Purpose Output Port.
            /// </summary>
            [DataMember(Name = "generalOutputPort")]
            public List<bool> GeneralOutputPort { get; init; }

            [DataContract]
            public sealed class VolumeClass
            {
                public VolumeClass(bool? Available = null, int? VolumeLevel = null)
                {
                    this.Available = Available;
                    this.VolumeLevel = VolumeLevel;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "volumeLevel")]
                [DataTypes(Minimum = 1, Maximum = 1000)]
                public int? VolumeLevel { get; init; }

            }

            /// <summary>
            /// Specifies whether the Volume Control is available, and if so, the increment/decrement value recommended by the vendor.
            /// </summary>
            [DataMember(Name = "volume")]
            public VolumeClass Volume { get; init; }

            [DataContract]
            public sealed class UPSClass
            {
                public UPSClass(bool? Available = null, bool? Low = null, bool? Engaged = null, bool? Powering = null, bool? Recovered = null)
                {
                    this.Available = Available;
                    this.Low = Low;
                    this.Engaged = Engaged;
                    this.Powering = Powering;
                    this.Recovered = Recovered;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "low")]
                public bool? Low { get; init; }


                [DataMember(Name = "engaged")]
                public bool? Engaged { get; init; }


                [DataMember(Name = "powering")]
                public bool? Powering { get; init; }


                [DataMember(Name = "recovered")]
                public bool? Recovered { get; init; }

            }

            /// <summary>
            /// Specifies whether the UPS device is available, and if so, which states it can take.
            /// </summary>
            [DataMember(Name = "UPS")]
            public UPSClass UPS { get; init; }

            public enum RemoteStatusMonitorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Remote Status Monitor device is available.
            /// </summary>
            [DataMember(Name = "remoteStatusMonitor")]
            public RemoteStatusMonitorEnum? RemoteStatusMonitor { get; init; }

            public enum AudibleAlarmEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Audible Alarm device is available.
            /// </summary>
            [DataMember(Name = "audibleAlarm")]
            public AudibleAlarmEnum? AudibleAlarm { get; init; }

            [DataContract]
            public sealed class EnhancedAudioControlClass
            {
                public EnhancedAudioControlClass(bool? Available = null, bool? HeadsetDetection = null, bool? ModeControllable = null)
                {
                    this.Available = Available;
                    this.HeadsetDetection = HeadsetDetection;
                    this.ModeControllable = ModeControllable;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "headsetDetection")]
                public bool? HeadsetDetection { get; init; }


                [DataMember(Name = "modeControllable")]
                public bool? ModeControllable { get; init; }

            }

            /// <summary>
            /// Specifies whether the Enhanced Audio Controller is available, and if so, which modes it supports.
            /// </summary>
            [DataMember(Name = "enhancedAudioControl")]
            public EnhancedAudioControlClass EnhancedAudioControl { get; init; }

            [DataContract]
            public sealed class EnhancedMicrophoneControlStateClass
            {
                public EnhancedMicrophoneControlStateClass(bool? Available = null, bool? HeadsetDetection = null, bool? ModeControllable = null)
                {
                    this.Available = Available;
                    this.HeadsetDetection = HeadsetDetection;
                    this.ModeControllable = ModeControllable;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "headsetDetection")]
                public bool? HeadsetDetection { get; init; }


                [DataMember(Name = "modeControllable")]
                public bool? ModeControllable { get; init; }

            }

            /// <summary>
            /// Specifies whether the Enhanced Microphone Controller is available, and if so, which modes it supports.
            /// </summary>
            [DataMember(Name = "enhancedMicrophoneControlState")]
            public EnhancedMicrophoneControlStateClass EnhancedMicrophoneControlState { get; init; }

            [DataContract]
            public sealed class MicrophoneVolumeClass
            {
                public MicrophoneVolumeClass(bool? Available = null, int? VolumeLevel = null)
                {
                    this.Available = Available;
                    this.VolumeLevel = VolumeLevel;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "volumeLevel")]
                [DataTypes(Minimum = 1, Maximum = 1000)]
                public int? VolumeLevel { get; init; }

            }

            /// <summary>
            /// Specifies whether the Microphone Volume Control is available, and if so, the increment/decrement value recommended by the vendor.
            /// </summary>
            [DataMember(Name = "microphoneVolume")]
            public MicrophoneVolumeClass MicrophoneVolume { get; init; }

            [DataContract]
            public sealed class AutoStartupModeClass
            {
                public AutoStartupModeClass(bool? Available = null, bool? Specific = null, bool? Daily = null, bool? Weekly = null)
                {
                    this.Available = Available;
                    this.Specific = Specific;
                    this.Daily = Daily;
                    this.Weekly = Weekly;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; init; }


                [DataMember(Name = "specific")]
                public bool? Specific { get; init; }


                [DataMember(Name = "daily")]
                public bool? Daily { get; init; }


                [DataMember(Name = "weekly")]
                public bool? Weekly { get; init; }

            }

            /// <summary>
            /// Specifies which mode of the auto start-up control is supported.
            /// </summary>
            [DataMember(Name = "autoStartupMode")]
            public AutoStartupModeClass AutoStartupMode { get; init; }

            /// <summary>
            /// Available guidelights.
            /// </summary>
            [DataMember(Name = "guideLights")]
            public Dictionary<string, GuideLightCapabilitiesClass> GuideLights { get; init; }

        }

        /// <summary>
        /// Specifies the type of sensors and indicators supported by this device.
        /// </summary>
        [DataMember(Name = "sensorType")]
        public SensorTypeClass SensorType { get; init; }

    }


}
