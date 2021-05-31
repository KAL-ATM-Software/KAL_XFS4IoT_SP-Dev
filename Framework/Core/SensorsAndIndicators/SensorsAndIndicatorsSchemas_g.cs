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
        /// \"notAvailable\": The light indicator is not available.
        /// \"off\": The light can be turned off.
        /// \"slow\": The light can blink slowly.
        /// \"medium\": The light can blink medium frequency.
        /// \"quick\": The light can blink quickly.
        /// \"continuous\":The light can be continuous (steady).
        /// </summary>
        [DataMember(Name = "flashRate")]
        public FlashRateEnum? FlashRate { get; private set; }

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
        /// \"defaultColor\": The light indicator is not available.
        /// \"red\": The light can be red.
        /// \"green\": The light can be green.
        /// \"yellow\": The light can be yellow.
        /// \"blue\": The light can be blue.
        /// \"cyan\":The light can be cyan.
        /// \"magenta\": The light can be magenta.
        /// \"white\":The light can be white.
        /// </summary>
        [DataMember(Name = "colour")]
        public ColourEnum? Colour { get; private set; }

        public enum DirectionEnum
        {
            Entry,
            Exit
        }

        /// <summary>
        /// Indicates the guidelight direction. The following values are possible:
        /// \"entry\": The light can  indicate entry.
        /// \"exit\": The light can indicate exit.
        /// </summary>
        [DataMember(Name = "direction")]
        public DirectionEnum? Direction { get; private set; }

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
        /// \"default\": The default position.
        /// \"left\": The left position.
        /// \"right\": The right position.
        /// \"center\": The center position.
        ///  \"top\": The top position.
        /// \"bottom\": The bottom position.
        /// \"front\": The front position.
        /// \"rear\": The rear position.
        /// </summary>
        [DataMember(Name = "position")]
        public PositionEnum? Position { get; private set; }

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
            public SensorTypeClass(bool? Sensors = null, bool? Doors = null, bool? Indicators = null, bool? Auxiliary = null, bool? Guidelights = null, OperatorSwitchEnum? OperatorSwitch = null, TamperSensorEnum? TamperSensor = null, IntTamperSensorEnum? IntTamperSensor = null, SeismicSensorEnum? SeismicSensor = null, HeatSensorEnum? HeatSensor = null, ProximitySensorEnum? ProximitySensor = null, AmbientLightSensorEnum? AmbientLightSensor = null, EnhancedAudioSensorClass EnhancedAudioSensor = null, BootSwitchSensorEnum? BootSwitchSensor = null, DisplaySensorEnum? DisplaySensor = null, OperatorCallButtonSensorEnum? OperatorCallButtonSensor = null, HandsetSensorClass HandsetSensor = null, List<bool> GeneralInputPort = null, HeadsetMicrophoneSensorClass HeadsetMicrophoneSensor = null, FasciaMicrophoneSensorEnum? FasciaMicrophoneSensor = null, CabinetDoorClass CabinetDoor = null, SafeDoorClass SafeDoor = null, VandalShieldClass VandalShield = null, FrontCabinetClass FrontCabinet = null, RearCabinetClass RearCabinet = null, LeftCabinetClass LeftCabinet = null, RightCabinetClass RightCabinet = null, OpenCloseIndicatorEnum? OpenCloseIndicator = null, FasciaLightEnum? FasciaLight = null, AudioEnum? Audio = null, HeatingEnum? Heating = null, ConsumerDisplayBacklightEnum? ConsumerDisplayBacklight = null, SignageDisplayEnum? SignageDisplay = null, List<bool> TransactionIndicator = null, List<bool> GeneralOutputPort = null, VolumeClass Volume = null, UPSClass UPS = null, RemoteStatusMonitorEnum? RemoteStatusMonitor = null, AudibleAlarmEnum? AudibleAlarm = null, EnhancedAudioControlClass EnhancedAudioControl = null, EnhancedMicrophoneControlStateClass EnhancedMicrophoneControlState = null, MicrophoneVolumeClass MicrophoneVolume = null, AutoStartupModeClass AutoStartupMode = null, GuideLightsClass GuideLights = null)
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
            public bool? Sensors { get; private set; }

            /// <summary>
            /// The device supports door sensors.
            /// </summary>
            [DataMember(Name = "doors")]
            public bool? Doors { get; private set; }

            /// <summary>
            /// The device supports indicators.
            /// </summary>
            [DataMember(Name = "indicators")]
            public bool? Indicators { get; private set; }

            /// <summary>
            /// The device supports auxiliary indicators.
            /// </summary>
            [DataMember(Name = "auxiliary")]
            public bool? Auxiliary { get; private set; }

            /// <summary>
            /// The device supports guidance lights.
            /// </summary>
            [DataMember(Name = "guidelights")]
            public bool? Guidelights { get; private set; }

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
            public OperatorSwitchEnum? OperatorSwitch { get; private set; }

            public enum TamperSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the Tamper sensor.
            /// </summary>
            [DataMember(Name = "tamperSensor")]
            public TamperSensorEnum? TamperSensor { get; private set; }

            public enum IntTamperSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the internal Tamper sensor.
            /// </summary>
            [DataMember(Name = "intTamperSensor")]
            public IntTamperSensorEnum? IntTamperSensor { get; private set; }

            public enum SeismicSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the Seismic sensor.
            /// </summary>
            [DataMember(Name = "seismicSensor")]
            public SeismicSensorEnum? SeismicSensor { get; private set; }

            public enum HeatSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the heat sensor.
            /// </summary>
            [DataMember(Name = "heatSensor")]
            public HeatSensorEnum? HeatSensor { get; private set; }

            public enum ProximitySensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the proximity sensor.
            /// </summary>
            [DataMember(Name = "proximitySensor")]
            public ProximitySensorEnum? ProximitySensor { get; private set; }

            public enum AmbientLightSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the ambient light sensor.
            /// </summary>
            [DataMember(Name = "ambientLightSensor")]
            public AmbientLightSensorEnum? AmbientLightSensor { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "manual")]
                public bool? Manual { get; private set; }


                [DataMember(Name = "auto")]
                public bool? Auto { get; private set; }


                [DataMember(Name = "semiAuto")]
                public bool? SemiAuto { get; private set; }


                [DataMember(Name = "bidirectional")]
                public bool? Bidirectional { get; private set; }

            }

            /// <summary>
            /// Specifies whether the Audio Jack is present, and if so, which modes it supports.
            /// </summary>
            [DataMember(Name = "enhancedAudioSensor")]
            public EnhancedAudioSensorClass EnhancedAudioSensor { get; private set; }

            public enum BootSwitchSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the boot switch sensor.
            /// </summary>
            [DataMember(Name = "bootSwitchSensor")]
            public BootSwitchSensorEnum? BootSwitchSensor { get; private set; }

            public enum DisplaySensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies the Consumer Display.
            /// </summary>
            [DataMember(Name = "displaySensor")]
            public DisplaySensorEnum? DisplaySensor { get; private set; }

            public enum OperatorCallButtonSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Operator Call Button is available. The Operator Call Button does not actually call the operator but just sends a signal to the application.
            /// </summary>
            [DataMember(Name = "operatorCallButtonSensor")]
            public OperatorCallButtonSensorEnum? OperatorCallButtonSensor { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "manual")]
                public bool? Manual { get; private set; }


                [DataMember(Name = "auto")]
                public bool? Auto { get; private set; }


                [DataMember(Name = "semiAuto")]
                public bool? SemiAuto { get; private set; }


                [DataMember(Name = "microphone")]
                public bool? Microphone { get; private set; }

            }

            /// <summary>
            /// Specifies whether the Handset is present, and if so, which modes it supports.
            /// </summary>
            [DataMember(Name = "handsetSensor")]
            public HandsetSensorClass HandsetSensor { get; private set; }

            /// <summary>
            /// Specifies whether the vendor dependent General-Purpose Input Ports are available. This value is an array and each index represents one General-Purpose Input Port.
            /// </summary>
            [DataMember(Name = "generalInputPort")]
            public List<bool> GeneralInputPort { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "manual")]
                public bool? Manual { get; private set; }


                [DataMember(Name = "auto")]
                public bool? Auto { get; private set; }


                [DataMember(Name = "semiAuto")]
                public bool? SemiAuto { get; private set; }

            }

            /// <summary>
            /// Specifies whether the Microphone Jack is present, and if so, which modes it supports.
            /// </summary>
            [DataMember(Name = "headsetMicrophoneSensor")]
            public HeadsetMicrophoneSensorClass HeadsetMicrophoneSensor { get; private set; }

            public enum FasciaMicrophoneSensorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether a Fascia Microphone (for public audio input) is present.
            /// </summary>
            [DataMember(Name = "fasciaMicrophoneSensor")]
            public FasciaMicrophoneSensorEnum? FasciaMicrophoneSensor { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; private set; }


                [DataMember(Name = "open")]
                public bool? Open { get; private set; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; private set; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; private set; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; private set; }

            }

            /// <summary>
            /// Specifies whether at least one Cabinet Doors is available, and if so, which states they can take.
            /// </summary>
            [DataMember(Name = "cabinetDoor")]
            public CabinetDoorClass CabinetDoor { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; private set; }


                [DataMember(Name = "open")]
                public bool? Open { get; private set; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; private set; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; private set; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; private set; }

            }

            /// <summary>
            /// Specifies whether the safe Door is available, and if so, which states it can take.
            /// </summary>
            [DataMember(Name = "safeDoor")]
            public SafeDoorClass SafeDoor { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; private set; }


                [DataMember(Name = "open")]
                public bool? Open { get; private set; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; private set; }


                [DataMember(Name = "service")]
                public bool? Service { get; private set; }


                [DataMember(Name = "keyboard")]
                public bool? Keyboard { get; private set; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; private set; }

            }

            /// <summary>
            /// Specifies whether the Vandal Shield is available, and if so, which states it can take.
            /// </summary>
            [DataMember(Name = "vandalShield")]
            public VandalShieldClass VandalShield { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; private set; }


                [DataMember(Name = "open")]
                public bool? Open { get; private set; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; private set; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; private set; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; private set; }

            }

            /// <summary>
            /// Specifies whether at least one Front Cabinet Door is available, and if so, which states they can take.
            /// </summary>
            [DataMember(Name = "frontCabinet")]
            public FrontCabinetClass FrontCabinet { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; private set; }


                [DataMember(Name = "open")]
                public bool? Open { get; private set; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; private set; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; private set; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; private set; }

            }

            /// <summary>
            /// Specifies whether at least one rear Cabinet Door is available, and if so, which states they can take.
            /// </summary>
            [DataMember(Name = "rearCabinet")]
            public RearCabinetClass RearCabinet { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; private set; }


                [DataMember(Name = "open")]
                public bool? Open { get; private set; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; private set; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; private set; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; private set; }

            }

            /// <summary>
            /// Specifies whether at least one left Cabinet Door is available, and if so, which states they can take.
            /// </summary>
            [DataMember(Name = "leftCabinet")]
            public LeftCabinetClass LeftCabinet { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "closed")]
                public bool? Closed { get; private set; }


                [DataMember(Name = "open")]
                public bool? Open { get; private set; }


                [DataMember(Name = "locked")]
                public bool? Locked { get; private set; }


                [DataMember(Name = "bolted")]
                public bool? Bolted { get; private set; }


                [DataMember(Name = "tampered")]
                public bool? Tampered { get; private set; }

            }

            /// <summary>
            /// Specifies whether at least one right Cabinet Door is available, and if so, which states they can take.
            /// </summary>
            [DataMember(Name = "rightCabinet")]
            public RightCabinetClass RightCabinet { get; private set; }

            public enum OpenCloseIndicatorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Open/Closed Indicator is available.
            /// </summary>
            [DataMember(Name = "openCloseIndicator")]
            public OpenCloseIndicatorEnum? OpenCloseIndicator { get; private set; }

            public enum FasciaLightEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the fascia light is available.
            /// </summary>
            [DataMember(Name = "fasciaLight")]
            public FasciaLightEnum? FasciaLight { get; private set; }

            public enum AudioEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Audio Indicator device is available.
            /// </summary>
            [DataMember(Name = "audio")]
            public AudioEnum? Audio { get; private set; }

            public enum HeatingEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the internal Heating device is available.
            /// </summary>
            [DataMember(Name = "heating")]
            public HeatingEnum? Heating { get; private set; }

            public enum ConsumerDisplayBacklightEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Consumer Display Backlight is available.
            /// </summary>
            [DataMember(Name = "consumerDisplayBacklight")]
            public ConsumerDisplayBacklightEnum? ConsumerDisplayBacklight { get; private set; }

            public enum SignageDisplayEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Signage Display is available.
            /// </summary>
            [DataMember(Name = "signageDisplay")]
            public SignageDisplayEnum? SignageDisplay { get; private set; }

            /// <summary>
            /// Specifies whether the Transaction Indicators are available as an array. Each index of this array represents one Transaction Indicator .
            /// </summary>
            [DataMember(Name = "transactionIndicator")]
            public List<bool> TransactionIndicator { get; private set; }

            /// <summary>
            /// Specifies whether the vendor dependent General-Purpose Output Ports are available. This value is an array and each index represents one General-Purpose Output Port.
            /// </summary>
            [DataMember(Name = "generalOutputPort")]
            public List<bool> GeneralOutputPort { get; private set; }

            [DataContract]
            public sealed class VolumeClass
            {
                public VolumeClass(bool? Available = null, int? VolumeLevel = null)
                {
                    this.Available = Available;
                    this.VolumeLevel = VolumeLevel;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; private set; }


                [DataMember(Name = "volumeLevel")]
                public int? VolumeLevel { get; private set; }

            }

            /// <summary>
            /// Specifies whether the Volume Control is available, and if so, the increment/decrement value recommended by the vendor.
            /// </summary>
            [DataMember(Name = "volume")]
            public VolumeClass Volume { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "low")]
                public bool? Low { get; private set; }


                [DataMember(Name = "engaged")]
                public bool? Engaged { get; private set; }


                [DataMember(Name = "powering")]
                public bool? Powering { get; private set; }


                [DataMember(Name = "recovered")]
                public bool? Recovered { get; private set; }

            }

            /// <summary>
            /// Specifies whether the UPS device is available, and if so, which states it can take.
            /// </summary>
            [DataMember(Name = "UPS")]
            public UPSClass UPS { get; private set; }

            public enum RemoteStatusMonitorEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Remote Status Monitor device is available.
            /// </summary>
            [DataMember(Name = "remoteStatusMonitor")]
            public RemoteStatusMonitorEnum? RemoteStatusMonitor { get; private set; }

            public enum AudibleAlarmEnum
            {
                NotAvailable,
                Available
            }

            /// <summary>
            /// Specifies whether the Audible Alarm device is available.
            /// </summary>
            [DataMember(Name = "audibleAlarm")]
            public AudibleAlarmEnum? AudibleAlarm { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "headsetDetection")]
                public bool? HeadsetDetection { get; private set; }


                [DataMember(Name = "modeControllable")]
                public bool? ModeControllable { get; private set; }

            }

            /// <summary>
            /// Specifies whether the Enhanced Audio Controller is available, and if so, which modes it supports.
            /// </summary>
            [DataMember(Name = "enhancedAudioControl")]
            public EnhancedAudioControlClass EnhancedAudioControl { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "headsetDetection")]
                public bool? HeadsetDetection { get; private set; }


                [DataMember(Name = "modeControllable")]
                public bool? ModeControllable { get; private set; }

            }

            /// <summary>
            /// Specifies whether the Enhanced Microphone Controller is available, and if so, which modes it supports.
            /// </summary>
            [DataMember(Name = "enhancedMicrophoneControlState")]
            public EnhancedMicrophoneControlStateClass EnhancedMicrophoneControlState { get; private set; }

            [DataContract]
            public sealed class MicrophoneVolumeClass
            {
                public MicrophoneVolumeClass(bool? Available = null, int? VolumeLevel = null)
                {
                    this.Available = Available;
                    this.VolumeLevel = VolumeLevel;
                }


                [DataMember(Name = "available")]
                public bool? Available { get; private set; }


                [DataMember(Name = "volumeLevel")]
                public int? VolumeLevel { get; private set; }

            }

            /// <summary>
            /// Specifies whether the Microphone Volume Control is available, and if so, the increment/decrement value recommended by the vendor.
            /// </summary>
            [DataMember(Name = "microphoneVolume")]
            public MicrophoneVolumeClass MicrophoneVolume { get; private set; }

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
                public bool? Available { get; private set; }


                [DataMember(Name = "specific")]
                public bool? Specific { get; private set; }


                [DataMember(Name = "daily")]
                public bool? Daily { get; private set; }


                [DataMember(Name = "weekly")]
                public bool? Weekly { get; private set; }

            }

            /// <summary>
            /// Specifies which mode of the auto start-up control is supported.
            /// </summary>
            [DataMember(Name = "autoStartupMode")]
            public AutoStartupModeClass AutoStartupMode { get; private set; }

            [DataContract]
            public sealed class GuideLightsClass
            {
                public GuideLightsClass(GuideLightCapabilitiesClass CardReader = null, GuideLightCapabilitiesClass PinPad = null, GuideLightCapabilitiesClass NotesDispenser = null, GuideLightCapabilitiesClass CoinDispenser = null, GuideLightCapabilitiesClass ReceiptPrinter = null, GuideLightCapabilitiesClass PassbookPrinter = null, GuideLightCapabilitiesClass EnvelopeDepository = null, GuideLightCapabilitiesClass ChequeUnit = null, GuideLightCapabilitiesClass BillAcceptor = null, GuideLightCapabilitiesClass EnvelopeDispenser = null, GuideLightCapabilitiesClass DocumentPrinter = null, GuideLightCapabilitiesClass CoinAcceptor = null, GuideLightCapabilitiesClass Scanner = null, GuideLightCapabilitiesClass Contactless = null, GuideLightCapabilitiesClass CardUnit2 = null, GuideLightCapabilitiesClass NotesDispenser2 = null, GuideLightCapabilitiesClass BillAcceptor2 = null, GuideLightCapabilitiesClass VendorDependent = null)
                {
                    this.CardReader = CardReader;
                    this.PinPad = PinPad;
                    this.NotesDispenser = NotesDispenser;
                    this.CoinDispenser = CoinDispenser;
                    this.ReceiptPrinter = ReceiptPrinter;
                    this.PassbookPrinter = PassbookPrinter;
                    this.EnvelopeDepository = EnvelopeDepository;
                    this.ChequeUnit = ChequeUnit;
                    this.BillAcceptor = BillAcceptor;
                    this.EnvelopeDispenser = EnvelopeDispenser;
                    this.DocumentPrinter = DocumentPrinter;
                    this.CoinAcceptor = CoinAcceptor;
                    this.Scanner = Scanner;
                    this.Contactless = Contactless;
                    this.CardUnit2 = CardUnit2;
                    this.NotesDispenser2 = NotesDispenser2;
                    this.BillAcceptor2 = BillAcceptor2;
                    this.VendorDependent = VendorDependent;
                }

                /// <summary>
                /// Card Unit Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "cardReader")]
                public GuideLightCapabilitiesClass CardReader { get; private set; }

                /// <summary>
                /// Pin Pad Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "pinPad")]
                public GuideLightCapabilitiesClass PinPad { get; private set; }

                /// <summary>
                /// Notes Dispenser Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "notesDispenser")]
                public GuideLightCapabilitiesClass NotesDispenser { get; private set; }

                /// <summary>
                /// Coin Dispenser Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "coinDispenser")]
                public GuideLightCapabilitiesClass CoinDispenser { get; private set; }

                /// <summary>
                /// Receipt Printer Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "receiptPrinter")]
                public GuideLightCapabilitiesClass ReceiptPrinter { get; private set; }

                /// <summary>
                /// Passbook Printer Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "passbookPrinter")]
                public GuideLightCapabilitiesClass PassbookPrinter { get; private set; }

                /// <summary>
                /// Envelope Depository Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "EnvelopeDepository")]
                public GuideLightCapabilitiesClass EnvelopeDepository { get; private set; }

                /// <summary>
                /// Cheque Unit Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "chequeUnit")]
                public GuideLightCapabilitiesClass ChequeUnit { get; private set; }

                /// <summary>
                /// Bill Acceptor Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "billAcceptor")]
                public GuideLightCapabilitiesClass BillAcceptor { get; private set; }

                /// <summary>
                /// Envelope Dispenser Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "envelopeDispenser")]
                public GuideLightCapabilitiesClass EnvelopeDispenser { get; private set; }

                /// <summary>
                /// Document Printer Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "documentPrinter")]
                public GuideLightCapabilitiesClass DocumentPrinter { get; private set; }

                /// <summary>
                /// Coin Acceptor Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "coinAcceptor")]
                public GuideLightCapabilitiesClass CoinAcceptor { get; private set; }

                /// <summary>
                /// scanner Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "scanner")]
                public GuideLightCapabilitiesClass Scanner { get; private set; }

                /// <summary>
                /// Contactless Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "contactless")]
                public GuideLightCapabilitiesClass Contactless { get; private set; }

                /// <summary>
                /// Card Unit 2 Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "cardUnit2")]
                public GuideLightCapabilitiesClass CardUnit2 { get; private set; }

                /// <summary>
                /// Notes Dispenser 2 Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "notesDispenser2")]
                public GuideLightCapabilitiesClass NotesDispenser2 { get; private set; }

                /// <summary>
                /// Bill Acceptor 2 Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "billAcceptor2")]
                public GuideLightCapabilitiesClass BillAcceptor2 { get; private set; }

                /// <summary>
                /// Vendor Dependent Guidelight.
                /// 
                /// </summary>
                [DataMember(Name = "vendorDependent")]
                public GuideLightCapabilitiesClass VendorDependent { get; private set; }

            }

            /// <summary>
            /// Available guidelights.
            /// </summary>
            [DataMember(Name = "guideLights")]
            public GuideLightsClass GuideLights { get; private set; }

        }

        /// <summary>
        /// Specifies the type of sensors and indicators supported by this device.
        /// </summary>
        [DataMember(Name = "sensorType")]
        public SensorTypeClass SensorType { get; private set; }

    }


}
