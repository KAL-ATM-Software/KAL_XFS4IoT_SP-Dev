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

    [DataContract]
    public sealed class OperatorSwitchStateClass
    {
        public OperatorSwitchStateClass(OperatorSwitchStateEnum? OperatorSwitchState = null)
        {
            this.OperatorSwitchState = OperatorSwitchState;
        }

        public enum OperatorSwitchStateEnum
        {
            NotAvailable,
            Run,
            Maintenance,
            Supervisor
        }

        /// <summary>
        /// Specifies the state of the Operator switch.
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```run``` - The switch is in run mode.
        /// * ```maintenance``` - The switch is in maintenance mode.
        /// * ```supervisor``` - TThe switch is in supervisor mode.
        /// </summary>
        [DataMember(Name = "operatorSwitchState")]
        public OperatorSwitchStateEnum? OperatorSwitchState { get; init; }

    }


    [DataContract]
    public sealed class TamperSensorStateClass
    {
        public TamperSensorStateClass(TamperSensorStateEnum? TamperSensorState = null)
        {
            this.TamperSensorState = TamperSensorState;
        }

        public enum TamperSensorStateEnum
        {
            NotAvailable,
            On,
            Off
        }

        /// <summary>
        /// Specifies the state of the Tamper sensor.
        /// 
        /// * ```notAvailable``` - The tamper sensor is not available.
        /// * ```off``` - There is no indication of a tampering attempt.
        /// * ```on``` - There has been a tampering attempt.
        /// </summary>
        [DataMember(Name = "tamperSensorState")]
        public TamperSensorStateEnum? TamperSensorState { get; init; }

    }


    [DataContract]
    public sealed class IntTamperSensorStateClass
    {
        public IntTamperSensorStateClass(IntTamperSensorStateEnum? IntTamperSensorState = null)
        {
            this.IntTamperSensorState = IntTamperSensorState;
        }

        public enum IntTamperSensorStateEnum
        {
            NotAvailable,
            On,
            Off
        }

        /// <summary>
        /// Specifies the state of the Internal Tamper Sensor for the internal alarm. This sensor indicates whether the internal alarm has been tampered with 
        /// (such as a burglar attempt). Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The internal tamper sensor is not available.
        /// * ```off``` - There is no indication of a tampering attempt.
        /// * ```on``` - There has been a tampering attempt.
        /// </summary>
        [DataMember(Name = "intTamperSensorState")]
        public IntTamperSensorStateEnum? IntTamperSensorState { get; init; }

    }


    [DataContract]
    public sealed class SeismicSensorStateClass
    {
        public SeismicSensorStateClass(SeismicSensorStateEnum? SeismicSensorState = null)
        {
            this.SeismicSensorState = SeismicSensorState;
        }

        public enum SeismicSensorStateEnum
        {
            NotAvailable,
            On,
            Off
        }

        /// <summary>
        /// Specifies the state of the Seismic Sensor. This sensor indicates whether the terminal has been shaken 
        /// (e.g. burglar attempt or seismic activity). Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status sensor is not available.
        /// * ```off``` - TThe seismic activity has not been high enough to trigger the sensor.
        /// * ```on``` - The seismic or other activity has triggered the sensor.
        /// </summary>
        [DataMember(Name = "seismicSensorState")]
        public SeismicSensorStateEnum? SeismicSensorState { get; init; }

    }


    [DataContract]
    public sealed class HeatSensorStateClass
    {
        public HeatSensorStateClass(HeatSensorStateEnum? HeatSensorState = null)
        {
            this.HeatSensorState = HeatSensorState;
        }

        public enum HeatSensorStateEnum
        {
            NotAvailable,
            On,
            Off
        }

        /// <summary>
        /// Specifies the state of the Heat Sensor. This sensor is triggered by excessive heat (fire) near 
        /// the terminal. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```off``` - The heat has not been high enough to trigger the sensor.
        /// * ```on``` - The heat has been high enough to trigger the sensor.
        /// </summary>
        [DataMember(Name = "heatSensorState")]
        public HeatSensorStateEnum? HeatSensorState { get; init; }

    }


    [DataContract]
    public sealed class ProximitySensorStateClass
    {
        public ProximitySensorStateClass(ProximitySensorStateEnum? ProximitySensorState = null)
        {
            this.ProximitySensorState = ProximitySensorState;
        }

        public enum ProximitySensorStateEnum
        {
            NotAvailable,
            Present,
            NotPresent
        }

        /// <summary>
        /// Specifies the state of the Proximity Sensor. This sensor is triggered by movements around the
        /// terminal. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```present``` - The sensor is showing that there is someone present at the terminal.
        /// * ```notPresent``` - The sensor can not sense any people around the terminal.
        /// </summary>
        [DataMember(Name = "proximitySensorState")]
        public ProximitySensorStateEnum? ProximitySensorState { get; init; }

    }


    [DataContract]
    public sealed class AmbientLightSensorStateClass
    {
        public AmbientLightSensorStateClass(AmbientLightSensorStateEnum? AmbientLightSensorState = null)
        {
            this.AmbientLightSensorState = AmbientLightSensorState;
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

        /// <summary>
        /// Specifies the state of the Ambient Light Sensor. This sensor indicates the level of ambient 
        /// light around the terminal. Interpretation of this value is vendor-specific and therefore it 
        /// is not guaranteed to report a consistent actual ambient light level across different vendor 
        /// hardware. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```veryDark``` - The level of light is very dark.
        /// * ```dark``` - The level of light is dark.
        /// * ```mediumLight``` - The level of light is medium light.
        /// * ```light``` - The level of light is light.
        /// * ```veryLight``` - The level of light is very light.
        /// </summary>
        [DataMember(Name = "ambientLightSensorState")]
        public AmbientLightSensorStateEnum? AmbientLightSensorState { get; init; }

    }


    [DataContract]
    public sealed class EnhancedAudioSensorStateClass
    {
        public EnhancedAudioSensorStateClass(EnhancedAudioSensorStateEnum? EnhancedAudioSensorState = null)
        {
            this.EnhancedAudioSensorState = EnhancedAudioSensorState;
        }

        public enum EnhancedAudioSensorStateEnum
        {
            NotAvailable,
            Present,
            NotPresent
        }

        /// <summary>
        /// Specifies the presence or absence of a consumer’s headphone connected to the Audio Jack. Specified 
        /// as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```present``` - There is a headset connected. 
        /// * ```notPresent``` - There is no headset connected.
        /// </summary>
        [DataMember(Name = "enhancedAudioSensorState")]
        public EnhancedAudioSensorStateEnum? EnhancedAudioSensorState { get; init; }

    }


    [DataContract]
    public sealed class BootSwitchSensorStateClass
    {
        public BootSwitchSensorStateClass(BootSwitchSensorStateEnum? BootSwitchSensorState = null)
        {
            this.BootSwitchSensorState = BootSwitchSensorState;
        }

        public enum BootSwitchSensorStateEnum
        {
            NotAvailable,
            Off,
            On
        }

        /// <summary>
        /// Specifies the state of the Boot Switch Sensor. This sensor is triggered whenever the terminal is about to be rebooted or 
        /// shutdown due to a delayed effect switch. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```off``` - The sensor has not been triggered.
        /// * ```on``` - The terminal is about to be rebooted or shutdown.
        /// </summary>
        [DataMember(Name = "bootSwitchSensorState")]
        public BootSwitchSensorStateEnum? BootSwitchSensorState { get; init; }

    }


    [DataContract]
    public sealed class DisplaySensorStateClass
    {
        public DisplaySensorStateClass(DisplaySensorStateEnum? DisplaySensorState = null)
        {
            this.DisplaySensorState = DisplaySensorState;
        }

        public enum DisplaySensorStateEnum
        {
            NotAvailable,
            Off,
            On,
            DisplayError
        }

        /// <summary>
        /// Specifies the state of the Consumer Display. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```off``` - The Consumer Display is switched off.    
        /// * ```on``` - The Consumer Display is in a good state and is turned on.   
        /// * ```displayError``` - The Consumer Display is in an error state.
        /// </summary>
        [DataMember(Name = "displaySensorState")]
        public DisplaySensorStateEnum? DisplaySensorState { get; init; }

    }


    [DataContract]
    public sealed class OperatorCallButtonSensorStateClass
    {
        public OperatorCallButtonSensorStateClass(OperatorCallButtonSensorStateEnum? OperatorCallButtonSensorState = null)
        {
            this.OperatorCallButtonSensorState = OperatorCallButtonSensorState;
        }

        public enum OperatorCallButtonSensorStateEnum
        {
            NotAvailable,
            Off,
            On
        }

        /// <summary>
        /// Specifies the state of the Operator Call Button as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```off``` - The Operator Call Button is released (not pressed).
        /// * ```on``` - The Operator Call Button is being pressed.
        /// </summary>
        [DataMember(Name = "operatorCallButtonSensorState")]
        public OperatorCallButtonSensorStateEnum? OperatorCallButtonSensorState { get; init; }

    }


    [DataContract]
    public sealed class HandsetSensorStateClass
    {
        public HandsetSensorStateClass(HandsetSensorStateEnum? HandsetSensorState = null)
        {
            this.HandsetSensorState = HandsetSensorState;
        }

        public enum HandsetSensorStateEnum
        {
            NotAvailable,
            OnTheHook,
            OffTheHook
        }

        /// <summary>
        /// Specifies the state of the Handset, which is a device similar to a telephone 
        /// receiver. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```onTheHook``` - The Handset is on the hook.
        /// * ```offTheHook``` - The Handset is off the hook.
        /// </summary>
        [DataMember(Name = "handsetSensorState")]
        public HandsetSensorStateEnum? HandsetSensorState { get; init; }

    }


    [DataContract]
    public sealed class HeadsetMicrophoneSensorStateClass
    {
        public HeadsetMicrophoneSensorStateClass(HeadsetMicrophoneSensorStateEnum? HeadsetMicrophoneSensorState = null)
        {
            this.HeadsetMicrophoneSensorState = HeadsetMicrophoneSensorState;
        }

        public enum HeadsetMicrophoneSensorStateEnum
        {
            NotAvailable,
            Present,
            NotPresent
        }

        /// <summary>
        /// Specifies the presence or absence of a consumer’s headset microphone connected to 
        /// the Microphone Jack. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```present``` - There is a headset microphone connected.
        /// * ```notPresent``` - There is no headset microphone connected.
        /// </summary>
        [DataMember(Name = "headsetMicrophoneSensorState")]
        public HeadsetMicrophoneSensorStateEnum? HeadsetMicrophoneSensorState { get; init; }

    }


    [DataContract]
    public sealed class FasciaMicrophoneSensorStateClass
    {
        public FasciaMicrophoneSensorStateClass(FasciaMicrophoneSensorStateEnum? FasciaMicrophoneSensorState = null)
        {
            this.FasciaMicrophoneSensorState = FasciaMicrophoneSensorState;
        }

        public enum FasciaMicrophoneSensorStateEnum
        {
            NotAvailable,
            Off,
            On
        }

        /// <summary>
        /// Specifies the state of the fascia microphone as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```off``` - The Fascia Microphone is turned off.
        /// * ```on``` - The Fascia Microphone is turned on.
        /// </summary>
        [DataMember(Name = "fasciaMicrophoneSensorState")]
        public FasciaMicrophoneSensorStateEnum? FasciaMicrophoneSensorState { get; init; }

    }


    [DataContract]
    public sealed class SafeDoorStateClass
    {
        public SafeDoorStateClass(SafeDoorStateEnum? SafeDoorState = null)
        {
            this.SafeDoorState = SafeDoorState;
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

        /// <summary>
        /// Specifies the state of the Safe Doors. Safe Doors are doors that open up for secure 
        /// hardware, such as the note dispenser, the security device, etc. Specified as one of the 
        /// following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```closed``` - The Safe Doors are closed.
        /// * ```open``` - At least one of the Safe Doors is open.
        /// * ```locked``` - All Safe Doors are closed and locked.
        /// * ```bolted``` - All Safe Doors are closed, locked and bolted.
        /// * ```tampered``` - At least one of the Safe Doors has potentially been tampered with.
        /// </summary>
        [DataMember(Name = "safeDoorState")]
        public SafeDoorStateEnum? SafeDoorState { get; init; }

    }


    [DataContract]
    public sealed class VandalShieldStateClass
    {
        public VandalShieldStateClass(VandalShieldStateEnum? VandalShieldState = null)
        {
            this.VandalShieldState = VandalShieldState;
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

        /// <summary>
        /// Specifies the state of the Vandal Shield. The Vandal Shield is a door that opens up for 
        /// consumer access to the terminal. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```closed``` - The Vandal Shield is closed.
        /// * ```open``` - The Vandal Shield is open.
        /// * ```locked``` - The Vandal Shield is closed and locked.
        /// * ```service``` - The Vandal Shield is in service position.
        /// * ```keyboard``` - The Vandal Shield position permits access to the keyboard.
        /// * ```ajar``` - The Vandal Shield is ajar.
        /// * ```jammed``` - The Vandal Shield is jammed.
        /// * ```tampered``` - The Vandal Shield has potentially been tampered with.
        /// </summary>
        [DataMember(Name = "vandalShieldState")]
        public VandalShieldStateEnum? VandalShieldState { get; init; }

    }


    [DataContract]
    public sealed class CabinetFrontDoorStateClass
    {
        public CabinetFrontDoorStateClass(CabinetFrontDoorStateEnum? CabinetFrontDoorState = null)
        {
            this.CabinetFrontDoorState = CabinetFrontDoorState;
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

        /// <summary>
        /// Specifies the overall state of the Front Cabinet Doors. The front is defined as the side 
        /// facing the customer/consumer. Cabinet Doors are doors that open up for consumables, and 
        /// hardware that does not have to be in a secure place. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```closed``` - All front Cabinet Doors are closed.
        /// * ```open``` - At least one of the front Cabinet Doors is open.
        /// * ```locked``` - All front Cabinet Doors are closed and locked.
        /// * ```bolted``` - All front Cabinet Doors are closed, locked and bolted.
        /// * ```tampered``` - At least one of the front Cabinet Doors has potentially been tampered with.
        /// </summary>
        [DataMember(Name = "cabinetFrontDoorState")]
        public CabinetFrontDoorStateEnum? CabinetFrontDoorState { get; init; }

    }


    [DataContract]
    public sealed class CabinetRearDoorStateClass
    {
        public CabinetRearDoorStateClass(CabinetRearDoorStateEnum? CabinetRearDoorState = null)
        {
            this.CabinetRearDoorState = CabinetRearDoorState;
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

        /// <summary>
        /// Specifies the overall state of the Rear Cabinet Doors. The front is defined as the side 
        /// opposite the side facing the customer/consumer. Cabinet Doors are doors that open up for 
        /// consumables, and hardware that does not have to be in a secure place. Specified as one 
        /// of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```closed``` - All rear Cabinet Doors are closed.
        /// * ```open``` - At least one of the rear Cabinet Doors is open.
        /// * ```locked``` - All rear Cabinet Doors are closed and locked.
        /// * ```bolted``` - All rear Cabinet Doors are closed, locked and bolted.
        /// * ```tampered``` - At least one of the rear Cabinet Doors has potentially been tampered with.
        /// </summary>
        [DataMember(Name = "cabinetRearDoorState")]
        public CabinetRearDoorStateEnum? CabinetRearDoorState { get; init; }

    }


    [DataContract]
    public sealed class CabinetLeftDoorStateClass
    {
        public CabinetLeftDoorStateClass(CabinetLeftDoorStateEnum? CabinetLeftDoorState = null)
        {
            this.CabinetLeftDoorState = CabinetLeftDoorState;
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

        /// <summary>
        /// Specifies the overall state of the Left Cabinet Doors. The left is defined as the side to 
        /// the left as seen by the customer/consumer. Cabinet Doors are doors that open up for consumables, 
        /// and hardware that does not have to be in a secure place. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```closed``` - All left Cabinet Doors are closed.
        /// * ```open``` - At least one of the left Cabinet Doors is open.
        /// * ```locked``` - All left Cabinet Doors are closed and locked.
        /// * ```bolted``` - All left Cabinet Doors are closed, locked and bolted.
        /// * ```tampered``` - At least one of the left Cabinet Doors has potentially been tampered with.
        /// </summary>
        [DataMember(Name = "cabinetLeftDoorState")]
        public CabinetLeftDoorStateEnum? CabinetLeftDoorState { get; init; }

    }


    [DataContract]
    public sealed class CabinetRightDoorStateClass
    {
        public CabinetRightDoorStateClass(CabinetRightDoorStateEnum? CabinetRightDoorState = null)
        {
            this.CabinetRightDoorState = CabinetRightDoorState;
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

        /// <summary>
        /// Specifies the overall state of the Right Cabinet Doors. The right is defined as the side to 
        /// the right as seen by the customer/consumer. Cabinet Doors are doors that open up for consumables, 
        /// and hardware that does not have to be in a secure place. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```closed``` - All right Cabinet Doors are closed.
        /// * ```open``` - At least one of the right Cabinet Doors is open.
        /// * ```locked``` - All right Cabinet Doors are closed and locked.
        /// * ```bolted``` - All right Cabinet Doors are closed, locked and bolted.
        /// * ```tampered``` - At least one of the right Cabinet Doors has potentially been tampered with.
        /// </summary>
        [DataMember(Name = "cabinetRightDoorState")]
        public CabinetRightDoorStateEnum? CabinetRightDoorState { get; init; }

    }


    [DataContract]
    public sealed class OpenClosedIndicatorStateClass
    {
        public OpenClosedIndicatorStateClass(OpenClosedIndicatorStateEnum? OpenClosedIndicatorState = null)
        {
            this.OpenClosedIndicatorState = OpenClosedIndicatorState;
        }

        public enum OpenClosedIndicatorStateEnum
        {
            NotAvailable,
            Closed,
            Open
        }

        /// <summary>
        /// Specifies the state of the Open/Closed Indicator as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```closed``` - The terminal is closed for a consumer.
        /// * ```open``` - The terminal is open to be used by a consumer.
        /// </summary>
        [DataMember(Name = "openClosedIndicatorState")]
        public OpenClosedIndicatorStateEnum? OpenClosedIndicatorState { get; init; }

    }


    [DataContract]
    public sealed class AudioStateClass
    {
        public AudioStateClass(AudioStateClassClass AudioState = null)
        {
            this.AudioState = AudioState;
        }

        [DataContract]
        public sealed class AudioStateClassClass
        {
            public AudioStateClassClass(bool? NotAvailable = null, bool? Off = null, bool? Keypress = null, bool? Exclamation = null, bool? Warning = null, bool? Error = null, bool? Critical = null, bool? Continuous = null)
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

        /// <summary>
        /// Specifies the state of the Audio Indicator as either one or a combination of the following 
        /// values. Interpretation of this value is vendor-specific and therefore it is not possible to 
        /// guarantee a consistent actual sound pattern across different vendor hardware:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```off``` - The Audio Indicator is turned off.
        /// * ```keypress``` - The Audio Indicator sounds a key click signal.
        /// * ```exclamation``` - The Audio Indicator sounds an exclamation signal.
        /// * ```warning``` - The Audio Indicator sounds a warning signal.
        /// * ```error``` - The Audio Indicator sounds an error signal.
        /// * ```critical``` - The Audio Indicator sounds a critical signal.
        /// * ```continuous``` - The Audio Indicator is turned on continuously.
        /// </summary>
        [DataMember(Name = "audioState")]
        public AudioStateClassClass AudioState { get; init; }

    }


    [DataContract]
    public sealed class HeatingStateClass
    {
        public HeatingStateClass(HeatingEnum? Heating = null)
        {
            this.Heating = Heating;
        }

        public enum HeatingEnum
        {
            NotAvailable,
            Off,
            On
        }

        /// <summary>
        /// Specifies the state of the internal heating as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```off``` - The internal heating is turned off.
        /// * ```on``` - The internal heating is turned on.
        /// </summary>
        [DataMember(Name = "heating")]
        public HeatingEnum? Heating { get; init; }

    }


    [DataContract]
    public sealed class ConsumerDisplayBacklightStateClass
    {
        public ConsumerDisplayBacklightStateClass(ConsumerDisplayBacklightEnum? ConsumerDisplayBacklight = null)
        {
            this.ConsumerDisplayBacklight = ConsumerDisplayBacklight;
        }

        public enum ConsumerDisplayBacklightEnum
        {
            NotAvailable,
            Off,
            On
        }

        /// <summary>
        /// Specifies the Consumer Display Backlight as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```off``` - The Consumer Display Backlight is turned off.
        /// * ```on``` - Consumer Display Backlight is turned on.
        /// </summary>
        [DataMember(Name = "consumerDisplayBacklight")]
        public ConsumerDisplayBacklightEnum? ConsumerDisplayBacklight { get; init; }

    }


    [DataContract]
    public sealed class SignageDisplayStateClass
    {
        public SignageDisplayStateClass(SignageDisplayEnum? SignageDisplay = null)
        {
            this.SignageDisplay = SignageDisplay;
        }

        public enum SignageDisplayEnum
        {
            NotAvailable,
            Off,
            On
        }

        /// <summary>
        /// Specifies the state of the Signage Display. The Signage Display is a lighted banner or marquee 
        /// that can be used to display information or an advertisement. Any dynamic data displayed must 
        /// be loaded by a means external to the Service Provider. Specified as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```off``` - The Signage Display is turned off.
        /// * ```on``` - The Signage Display is turned on.
        /// </summary>
        [DataMember(Name = "signageDisplay")]
        public SignageDisplayEnum? SignageDisplay { get; init; }

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
        public UPSStateClass(UPSStateClassClass UPSState = null)
        {
            this.UPSState = UPSState;
        }

        [DataContract]
        public sealed class UPSStateClassClass
        {
            public UPSStateClassClass(bool? NotAvailable = null, bool? Low = null, bool? Engaged = null, bool? Powering = null, bool? Recovered = null)
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

        /// <summary>
        /// Specifies the state of the Uninterruptible Power Supply device as a combination of the 
        /// following values:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```low``` - The charge level of the UPS is\tlow.
        /// * ```engaged``` - TThe UPS is engaged.
        /// * ```powering``` - The UPS is powering the system.
        /// * ```recovered``` - The UPS was engaged when the main power went off.
        /// </summary>
        [DataMember(Name = "UPSState")]
        public UPSStateClassClass UPSState { get; init; }

    }


    [DataContract]
    public sealed class AudibleAlarmStateClass
    {
        public AudibleAlarmStateClass(AudibleAlarmEnum? AudibleAlarm = null)
        {
            this.AudibleAlarm = AudibleAlarm;
        }

        public enum AudibleAlarmEnum
        {
            NotAvailable,
            On,
            Off
        }

        /// <summary>
        /// Species the state of the Audible Alarm device as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```off``` - The Alarm is turned off.
        /// * ```on``` - The Alarm is turned on.
        /// </summary>
        [DataMember(Name = "audibleAlarm")]
        public AudibleAlarmEnum? AudibleAlarm { get; init; }

    }


    [DataContract]
    public sealed class EnhancedAudioControlStateClass
    {
        public EnhancedAudioControlStateClass(EnhancedAudioControlStateEnum? EnhancedAudioControlState = null)
        {
            this.EnhancedAudioControlState = EnhancedAudioControlState;
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

        /// <summary>
        /// Specifies the state of the Enhanced Audio Controller. The Enhanced Audio Controller controls 
        /// how private and public audio are broadcast when the headset is inserted into/removed from the
        /// audio jack and when the handset is off-hook/on-hook. In the following, Privacy Device is used 
        /// to refer to either the headset or handset. The Enhanced Audio Controller state is specified 
        /// as one of the following:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```publicAudioManual``` - The Enhanced Audio Controller is in manual mode and is in the 
        /// public state (i.e. audio will be played through speakers). Activating a Privacy Device 
        /// (headset connected/handset off-hook) will have no impact, i.e. Output will remain through 
        /// the speakers &amp; no audio will be directed to the Privacy Device.
        /// * ```publicAudioAuto``` - The Enhanced Audio Controller is in auto mode and is in the public 
        /// state (i.e. audio will be played through speakers). When a Privacy Device is activated, the 
        /// device will go to the private state.      
        /// * ```publicAudioSemiAuto``` - The Enhanced Audio Controller is in semi-auto mode and is in 
        /// the public state (i.e. audio will be played through speakers). When a Privacy Device is 
        /// activated, the device will go to the private state.
        /// * ```privateAudioManual``` - he Enhanced Audio Controller is in manual mode and is in the 
        /// private state (i.e. audio will be played only through a connected Privacy Device). In private 
        /// mode, no audio is transmitted through the speakers.
        /// * ```privateAudioAuto``` - The Enhanced Audio Controller is in auto mode and is in the private 
        /// state (i.e. audio will be played only through a connected Privacy Device). In private mode, no 
        /// audio is transmitted through the speakers. When a Privacy Device is deactivated (headset 
        /// disconnected/handset on-hook), the device will go to the public state. Where there is more 
        /// than one Privacy Device, the device will go to the public state only when all Privacy Devices 
        /// have been deactivated.
        /// * ```privateAudioSemiAuto``` - The Enhanced Audio Controller is in semi-auto mode and is in 
        /// the private state (i.e. audio will be played only through a connected Privacy Device). In 
        /// private mode, no audio is transmitted through the speakers. When a Privacy Device is deactivated, 
        /// the device will remain in the private state.
        /// </summary>
        [DataMember(Name = "enhancedAudioControlState")]
        public EnhancedAudioControlStateEnum? EnhancedAudioControlState { get; init; }

    }


    [DataContract]
    public sealed class EnhancedMicrophoneControlStateClass
    {
        public EnhancedMicrophoneControlStateClass(EnhancedMicrophoneControlStateEnum? EnhancedMicrophoneControlState = null)
        {
            this.EnhancedMicrophoneControlState = EnhancedMicrophoneControlState;
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

        /// <summary>
        /// Specifies the state of the Enhanced Microphone Controller. The Enhanced Microphone Controller controls 
        /// how private and public audio input are transmitted when the headset is inserted into/removed from the 
        /// audio jack and when the handset is off-hook/on-hook. In the following, Privacy Device is used to refer 
        /// to either the headset or handset. The Enhanced Microphone Controller state is specified as one of the 
        /// followings:
        /// 
        /// * ```notAvailable``` - The status is not available.
        /// * ```publicAudioManual``` - The Enhanced Microphone Controller is in manual mode and is in the public 
        /// state (i.e. the microphone in the fascia is active). Activating a Privacy Device (headset connected/handset 
        /// off-hook) will have no impact, i.e. input will remain through the fascia microphone and any microphone 
        /// associated with the Privacy Device will not be active
        /// * ```publicAudioAuto``` - The Enhanced Microphone Controller is in auto mode and is in the public state 
        /// (i.e. the microphone in the fascia is active). When a Privacy Device with a microphone is activated, the 
        /// device will go to the private state. 
        /// * ```publicAudioSemiAuto``` - The Enhanced Microphone Controller is in semi-auto mode and is in the public 
        /// state (i.e. the microphone in the fascia is active). When a Privacy Device with a microphone is activated, 
        /// the device will go to the private state.
        /// * ```privateAudioManual``` - The Enhanced Microphone Controller is in manual mode and is in the private 
        /// state (i.e. audio input will be via a microphone in the Privacy Device). In private mode, no audio input 
        /// is transmitted through the fascia microphone. 
        /// * ```privateAudioAuto``` - The Enhanced Microphone Controller is in auto mode and is in the private 
        /// state (i.e. audio input will be via a microphone in the Privacy Device). In private mode, no audio input 
        /// is transmitted through the fascia microphone. When a Privacy Device with a microphone is deactivated 
        /// (headset disconnected/handset on-hook), the device will go to the public state. Where there is more than one 
        /// Privacy Device with a microphone, the device will go to the public state only when all such Privacy Devices 
        /// have been deactivated.  
        /// * ```privateAudioSemiAuto``` - The Enhanced Microphone Controller is in semi-auto mode and is in the 
        /// private state (i.e. audio input will be via a microphone in the Privacy Device). In private mode, no 
        /// audio is transmitted through the fascia microphone. When a Privacy Device with a microphone is deactivated, 
        /// the device will remain in the private state.
        /// </summary>
        [DataMember(Name = "enhancedMicrophoneControlState")]
        public EnhancedMicrophoneControlStateEnum? EnhancedMicrophoneControlState { get; init; }

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
        public StatusClass(OperatorSwitchStateClass OperatorSwitch = null, TamperSensorStateClass Tamper = null, IntTamperSensorStateClass IntTamperSensorState = null, SeismicSensorStateClass SeismicSensor = null, HeatSensorStateClass HeatSensor = null, ProximitySensorStateClass ProximitySensor = null, AmbientLightSensorStateClass AmbientLightSensor = null, EnhancedAudioSensorStateClass EnhancedAudioSensor = null, BootSwitchSensorStateClass BootSwitchSensor = null, DisplaySensorStateClass DisplaySensor = null, OperatorCallButtonSensorStateClass OperatorCallButtonSensor = null, HandsetSensorStateClass HandsetSensor = null, HeadsetMicrophoneSensorStateClass HeadsetMicrophoneSensor = null, FasciaMicrophoneSensorStateClass FasciaMicrophoneSensor = null, SafeDoorStateClass SafeDoor = null, VandalShieldStateClass VandalShield = null, CabinetFrontDoorStateClass CabinetFrontDoor = null, CabinetRearDoorStateClass CabinetRearDoor = null, CabinetLeftDoorStateClass CabinetLeftDoor = null, CabinetRightDoorStateClass CabinetRightDoor = null, OpenClosedIndicatorStateClass OpenClosedIndicator = null, AudioStateClass Audio = null, HeatingStateClass Heating = null, ConsumerDisplayBacklightStateClass ConsumerDisplayBacklight = null, SignageDisplayStateClass SignageDisplay = null, VolumeStateClass Volume = null, UPSStateClass UPS = null, AudibleAlarmStateClass AudibleAlarm = null, EnhancedAudioControlStateClass EnhancedAudioControl = null, EnhancedMicrophoneControlStateClass EnhancedMicrophoneControl = null, MicrophoneVolumeStateClass MicrophoneVolume = null)
        {
            this.OperatorSwitch = OperatorSwitch;
            this.Tamper = Tamper;
            this.IntTamperSensorState = IntTamperSensorState;
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
        public OperatorSwitchStateClass OperatorSwitch { get; init; }

        [DataMember(Name = "tamper")]
        public TamperSensorStateClass Tamper { get; init; }

        [DataMember(Name = "intTamperSensorState")]
        public IntTamperSensorStateClass IntTamperSensorState { get; init; }

        [DataMember(Name = "seismicSensor")]
        public SeismicSensorStateClass SeismicSensor { get; init; }

        [DataMember(Name = "heatSensor")]
        public HeatSensorStateClass HeatSensor { get; init; }

        [DataMember(Name = "proximitySensor")]
        public ProximitySensorStateClass ProximitySensor { get; init; }

        [DataMember(Name = "ambientLightSensor")]
        public AmbientLightSensorStateClass AmbientLightSensor { get; init; }

        [DataMember(Name = "enhancedAudioSensor")]
        public EnhancedAudioSensorStateClass EnhancedAudioSensor { get; init; }

        [DataMember(Name = "bootSwitchSensor")]
        public BootSwitchSensorStateClass BootSwitchSensor { get; init; }

        [DataMember(Name = "displaySensor")]
        public DisplaySensorStateClass DisplaySensor { get; init; }

        [DataMember(Name = "operatorCallButtonSensor")]
        public OperatorCallButtonSensorStateClass OperatorCallButtonSensor { get; init; }

        [DataMember(Name = "handsetSensor")]
        public HandsetSensorStateClass HandsetSensor { get; init; }

        [DataMember(Name = "headsetMicrophoneSensor")]
        public HeadsetMicrophoneSensorStateClass HeadsetMicrophoneSensor { get; init; }

        [DataMember(Name = "FasciaMicrophoneSensor")]
        public FasciaMicrophoneSensorStateClass FasciaMicrophoneSensor { get; init; }

        [DataMember(Name = "safeDoor")]
        public SafeDoorStateClass SafeDoor { get; init; }

        [DataMember(Name = "vandalShield")]
        public VandalShieldStateClass VandalShield { get; init; }

        [DataMember(Name = "cabinetFrontDoor")]
        public CabinetFrontDoorStateClass CabinetFrontDoor { get; init; }

        [DataMember(Name = "cabinetRearDoor")]
        public CabinetRearDoorStateClass CabinetRearDoor { get; init; }

        [DataMember(Name = "cabinetLeftDoor")]
        public CabinetLeftDoorStateClass CabinetLeftDoor { get; init; }

        [DataMember(Name = "cabinetRightDoor")]
        public CabinetRightDoorStateClass CabinetRightDoor { get; init; }

        [DataMember(Name = "openClosedIndicator")]
        public OpenClosedIndicatorStateClass OpenClosedIndicator { get; init; }

        [DataMember(Name = "audio")]
        public AudioStateClass Audio { get; init; }

        [DataMember(Name = "heating")]
        public HeatingStateClass Heating { get; init; }

        [DataMember(Name = "consumerDisplayBacklight")]
        public ConsumerDisplayBacklightStateClass ConsumerDisplayBacklight { get; init; }

        [DataMember(Name = "signageDisplay")]
        public SignageDisplayStateClass SignageDisplay { get; init; }

        [DataMember(Name = "volume")]
        public VolumeStateClass Volume { get; init; }

        [DataMember(Name = "UPS")]
        public UPSStateClass UPS { get; init; }

        [DataMember(Name = "audibleAlarm")]
        public AudibleAlarmStateClass AudibleAlarm { get; init; }

        [DataMember(Name = "enhancedAudioControl")]
        public EnhancedAudioControlStateClass EnhancedAudioControl { get; init; }

        [DataMember(Name = "enhancedMicrophoneControl")]
        public EnhancedMicrophoneControlStateClass EnhancedMicrophoneControl { get; init; }

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
