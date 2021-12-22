/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * SetAuxiliaries_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Auxiliaries.Commands
{
    //Original name = SetAuxiliaries
    [DataContract]
    [Command(Name = "Auxiliaries.SetAuxiliaries")]
    public sealed class SetAuxiliariesCommand : Command<SetAuxiliariesCommand.PayloadData>
    {
        public SetAuxiliariesCommand(int RequestId, SetAuxiliariesCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, SafeDoorEnum? SafeDoor = null, VandalShieldEnum? VandalShield = null, FrontCabinetDoorsEnum? FrontCabinetDoors = null, RearCabinetDoorsEnum? RearCabinetDoors = null, LeftCabinetDoorsEnum? LeftCabinetDoors = null, RightCabinetDoorsEnum? RightCabinetDoors = null, OpenCloseEnum? OpenClose = null, FasciaLightEnum? FasciaLight = null, AudioClass Audio = null, HeatingEnum? Heating = null, DisplayBackLightEnum? DisplayBackLight = null, SignageDisplayEnum? SignageDisplay = null, int? Volume = null, UpsEnum? Ups = null, AudibleAlarmEnum? AudibleAlarm = null, EnhancedAudioControlEnum? EnhancedAudioControl = null, EnhancedMicrophoneControlEnum? EnhancedMicrophoneControl = null, int? MicrophoneVolume = null)
                : base(Timeout)
            {
                this.SafeDoor = SafeDoor;
                this.VandalShield = VandalShield;
                this.FrontCabinetDoors = FrontCabinetDoors;
                this.RearCabinetDoors = RearCabinetDoors;
                this.LeftCabinetDoors = LeftCabinetDoors;
                this.RightCabinetDoors = RightCabinetDoors;
                this.OpenClose = OpenClose;
                this.FasciaLight = FasciaLight;
                this.Audio = Audio;
                this.Heating = Heating;
                this.DisplayBackLight = DisplayBackLight;
                this.SignageDisplay = SignageDisplay;
                this.Volume = Volume;
                this.Ups = Ups;
                this.AudibleAlarm = AudibleAlarm;
                this.EnhancedAudioControl = EnhancedAudioControl;
                this.EnhancedMicrophoneControl = EnhancedMicrophoneControl;
                this.MicrophoneVolume = MicrophoneVolume;
            }

            public enum SafeDoorEnum
            {
                Bolt,
                Unbolt
            }

            /// <summary>
            /// Specifies whether the safe doors should be bolted or unbolted as one of the following values:
            /// 
            /// * ```bolt``` - All Safe Doors are bolted.
            /// * ```unbolt``` - All Safe Doors are unbolted.
            /// </summary>
            [DataMember(Name = "safeDoor")]
            public SafeDoorEnum? SafeDoor { get; init; }

            public enum VandalShieldEnum
            {
                Closed,
                Open,
                Service,
                Keyboard
            }

            /// <summary>
            /// Specifies whether the Vandal Shield should change position as one of the following values:
            /// 
            /// * ```closed``` - Close the Vandal Shield.
            /// * ```open``` - Open the Vandal Shield.
            /// * ```service``` - Position the Vandal Shield in the service position.
            /// * ```keyboard``` - Position the Vandal Shield to permit access to the keyboard.
            /// </summary>
            [DataMember(Name = "vandalShield")]
            public VandalShieldEnum? VandalShield { get; init; }

            public enum FrontCabinetDoorsEnum
            {
                Bolt,
                Unbolt
            }

            /// <summary>
            /// Specifies whether all the front Cabinet Doors should be bolted or unbolted as one of the following values:
            /// 
            /// * ```bolt``` - All front Cabinet Doors are bolted.
            /// * ```unbolt``` - All front Cabinet Doors are unbolted.
            /// </summary>
            [DataMember(Name = "frontCabinetDoors")]
            public FrontCabinetDoorsEnum? FrontCabinetDoors { get; init; }

            public enum RearCabinetDoorsEnum
            {
                Bolt,
                Unbolt
            }

            /// <summary>
            /// Specifies whether all the rear Cabinet Doors should be bolted or unbolted as one of the following values:
            /// 
            /// * ```bolt``` - All rear Cabinet Doors are bolted.
            /// * ```unbolt``` - All rear Cabinet Doors are unbolted.
            /// </summary>
            [DataMember(Name = "rearCabinetDoors")]
            public RearCabinetDoorsEnum? RearCabinetDoors { get; init; }

            public enum LeftCabinetDoorsEnum
            {
                Bolt,
                Unbolt
            }

            /// <summary>
            /// Specifies whether all the left Cabinet Doors should be bolted or unbolted as one of the following values:
            /// 
            /// * ```bolt``` - All left Cabinet Doors are bolted.
            /// * ```unbolt``` - All left Cabinet Doors are unbolted.
            /// </summary>
            [DataMember(Name = "leftCabinetDoors")]
            public LeftCabinetDoorsEnum? LeftCabinetDoors { get; init; }

            public enum RightCabinetDoorsEnum
            {
                Bolt,
                Unbolt
            }

            /// <summary>
            /// Specifies whether all the right Cabinet Doors should be bolted or unbolted as one of the following values:
            /// 
            /// * ```bolt``` - All right Cabinet Doors are bolted.
            /// * ```unbolt``` - All right Cabinet Doors are unbolted.
            /// </summary>
            [DataMember(Name = "rightCabinetDoors")]
            public RightCabinetDoorsEnum? RightCabinetDoors { get; init; }

            public enum OpenCloseEnum
            {
                Closed,
                Open
            }

            /// <summary>
            /// Specifies whether the Open/Closed Indicator should show Open or Close to a consumer as one of the 
            /// following values:
            /// 
            /// * ```closed``` - The Open/Closed Indicator is changed to show that the terminal is closed for a consumer.
            /// * ```open``` - The Open/Closed Indicator is changed to show that the terminal is open to be used by a consumer.
            /// </summary>
            [DataMember(Name = "openClose")]
            public OpenCloseEnum? OpenClose { get; init; }

            public enum FasciaLightEnum
            {
                Off,
                On
            }

            /// <summary>
            /// Specifies whether the Fascia Lights should be turned on or off as one of the following values:
            /// 
            /// * ```off``` - Turn off the Fascia Light.
            /// * ```on``` - Turn on the Fascia Light.
            /// </summary>
            [DataMember(Name = "fasciaLight")]
            public FasciaLightEnum? FasciaLight { get; init; }

            [DataContract]
            public sealed class AudioClass
            {
                public AudioClass(RateEnum? Rate = null, SignalEnum? Signal = null)
                {
                    this.Rate = Rate;
                    this.Signal = Signal;
                }

                public enum RateEnum
                {
                    On,
                    Off
                }

                /// <summary>
                /// Specifies the rate of the Audio Indicator as one of the following values:
                /// 
                /// * ```on``` - Turn on the Audio Indicator.
                /// * ```off``` - Turn off the Audio Indicator.
                /// * ```continuous``` - Turn the Audio Indicator to continuous.
                /// </summary>
                [DataMember(Name = "rate")]
                public RateEnum? Rate { get; init; }

                public enum SignalEnum
                {
                    Keypress,
                    Exclamation,
                    Warning,
                    Error,
                    Critical
                }

                /// <summary>
                /// Specifies the Audio sound as one of the following values:
                ///         
                /// * ```keypress``` - Sound a key click signal.
                /// * ```exclamation``` - Sound an exclamation signal.  
                /// * ```warning``` - Sound a warning signal.
                /// * ```error``` - Sound an error signal. 
                /// * ```critical``` - Sound a critical error signal.
                /// </summary>
                [DataMember(Name = "signal")]
                public SignalEnum? Signal { get; init; }

            }

            /// <summary>
            /// Specifies whether the Audio Indicator should be turned on or off, if available.
            /// </summary>
            [DataMember(Name = "audio")]
            public AudioClass Audio { get; init; }

            public enum HeatingEnum
            {
                On,
                Off
            }

            /// <summary>
            /// Specifies whether the Internal Heating device should be turned on or off as one of the 
            /// following values:
            /// 
            /// * ```off``` - The Internal Heating device is turned off.
            /// * ```on``` - The Internal Heating device is turned on.
            /// </summary>
            [DataMember(Name = "heating")]
            public HeatingEnum? Heating { get; init; }

            public enum DisplayBackLightEnum
            {
                On,
                Off
            }

            /// <summary>
            /// Specifies whether the Consumer Display Backlight should be turned on or off as one of the 
            /// following values:
            /// 
            /// * ```off``` - The Consumer Display Backlight is turned off.
            /// * ```on``` - The Consumer Display Backlight is turned on.
            /// </summary>
            [DataMember(Name = "displayBackLight")]
            public DisplayBackLightEnum? DisplayBackLight { get; init; }

            public enum SignageDisplayEnum
            {
                On,
                Off
            }

            /// <summary>
            /// Specifies whether the Signage Display should be turned on or off as one of the following values:
            /// 
            /// * ```off``` - The Signage Display is turned off.
            /// * ```on``` - The Signage Display is turned on.
            /// </summary>
            [DataMember(Name = "signageDisplay")]
            public SignageDisplayEnum? SignageDisplay { get; init; }

            /// <summary>
            /// Specifies whether the value of the Volume Control should be changed. If so, the value of Volume Control is
            /// defined in an interval from 1 to 1000 where 1 is the lowest volume level and 1000 is the highest volume
            /// level.
            /// </summary>
            [DataMember(Name = "volume")]
            [DataTypes(Minimum = 1, Maximum = 1000)]
            public int? Volume { get; init; }

            public enum UpsEnum
            {
                Engage,
                Disengage
            }

            /// <summary>
            /// Specifies whether the UPS device should be engaged or disengaged. The UPS device should not be engaged when
            /// the charge level is low. Specified as one of the following values:
            /// 
            /// * ```engage``` - Engage the UPS.
            /// * ```disengage``` - Disengage the UPS.
            /// </summary>
            [DataMember(Name = "ups")]
            public UpsEnum? Ups { get; init; }

            public enum AudibleAlarmEnum
            {
                Off,
                On
            }

            /// <summary>
            /// Specifies whether the state of the Audible Alarm device should be changed as one of the following values:
            /// 
            /// * ```off``` - Turn off the Audible Alarm device.
            /// * ```on``` - Turn on the Audible Alarm device.
            /// </summary>
            [DataMember(Name = "audibleAlarm")]
            public AudibleAlarmEnum? AudibleAlarm { get; init; }

            public enum EnhancedAudioControlEnum
            {
                PublicAudioManual,
                PublicAudioAuto,
                PublicAudioSemiAuto,
                PrivateAudioManual,
                PrivateAudioAuto,
                PrivateAudioSemiAuto
            }

            /// <summary>
            /// Specifies whether the state of the Enhanced Audio Controller should be changed as 
            /// one of the following values:
            /// 
            /// * ```publicAudioManual``` - Set the Enhanced Audio Controller to manual mode, public 
            /// state (i.e. audio will be played through speakers only). 
            /// * ```publicAudioAuto``` - Set the Enhanced Audio Controller to auto mode, public state 
            /// (i.e. audio will be played through speakers). When a Privacy Device is activated (headset 
            /// connected/handset off-hook), the device will go to the private state. 
            /// * ```publicAudioSemiAuto``` - Set the Enhanced Audio Controller to semi-auto mode, public 
            /// state (i.e. audio will be played through speakers). When a Privacy Device is activated, 
            /// the device will go to the private state.
            /// * ```privateAudioManual``` - Set the Enhanced Audio Controller to manual mode, private 
            /// state (i.e. audio will be played only through a connected Privacy Device). In private 
            /// mode, no audio is transmitted through the speakers. 
            /// * ```privateAudioAuto``` - Set the Enhanced Audio Controller to auto mode, private state 
            /// (i.e. audio will be played only through an activated Privacy Device). In private mode, 
            /// no audio is transmitted through the speakers. When a Privacy Device is deactivated 
            /// (headset disconnected/handset on-hook), the device will go to the public state. 
            /// * ```privateAudioSemiAuto``` - Set the Enhanced Audio Controller to semi-auto mode, 
            /// private state (i.e. audio will be played only through an activated Privacy Device). In 
            /// private mode, no audio is transmitted through the speakers. When a Privacy Device is 
            /// deactivated, the device will remain in the private state.
            /// </summary>
            [DataMember(Name = "enhancedAudioControl")]
            public EnhancedAudioControlEnum? EnhancedAudioControl { get; init; }

            public enum EnhancedMicrophoneControlEnum
            {
                PublicAudioManual,
                PublicAudioAuto,
                PublicAudioSemiAuto,
                PrivateAudioManual,
                PrivateAudioAuto,
                PrivateAudioSemiAuto
            }

            /// <summary>
            /// Specifies whether the state of the Enhanced Microphone Controller should be changed as 
            /// one of the following values:
            /// 
            /// * ```publicAudioManual``` - Set the Enhanced Microphone Controller to manual mode, public 
            /// state (i.e. only the microphone in the fascia is active).
            /// * ```publicAudioAuto``` - Set the Enhanced Microphone Controller to auto mode, public 
            /// state (i.e. only the microphone in the fascia is active). When a Privacy Device with a 
            /// microphone is activated (headset connected/handset off-hook), the device will go to the private state.
            /// * ```publicAudioSemiAuto``` - Set the Enhanced Microphone Controller to semi-auto mode, public state 
            /// (i.e. only the microphone in the fascia is active). When a Privacy Device with a microphone is 
            /// activated, the device will go to the private state.
            /// * ```privateAudioManual``` - Set the Enhanced Microphone Controller to manual mode, private state 
            /// (i.e. audio input will be only via a microphone in the Privacy Device). In private mode, no audio 
            /// input is transmitted through the fascia microphone.
            /// * ```privateAudioAuto``` - Set the Enhanced Microphone Controller to auto mode, private state 
            /// (i.e. audio input will be only via a microphone in the Privacy Device). In private mode, no audio 
            /// input is transmitted through the fascia microphone. When a Privacy Device with a microphone is 
            /// deactivated (headset disconnected/handset on-hook), the device will go to the public state. 
            /// * ```privateAudioSemiAuto``` - Set the Enhanced Microphone Controller to semi-auto mode, private 
            /// state (i.e. audio input will be only via a microphone in the Privacy Device). In private mode, no 
            /// audio input is transmitted through the fascia microphone. When a Privacy Device with a microphone is 
            /// deactivated, the device will remain in the private state.
            /// </summary>
            [DataMember(Name = "enhancedMicrophoneControl")]
            public EnhancedMicrophoneControlEnum? EnhancedMicrophoneControl { get; init; }

            /// <summary>
            /// Specifies whether the value of the Microphone Volume Control should be changed. If so, the value of
            /// Microphone Volume Control is defined in an interval from 1 to 1000 where 1 is the lowest volume level and
            /// 1000 is the highest volume level.
            /// </summary>
            [DataMember(Name = "microphoneVolume")]
            [DataTypes(Minimum = 1, Maximum = 1000)]
            public int? MicrophoneVolume { get; init; }

        }
    }
}
