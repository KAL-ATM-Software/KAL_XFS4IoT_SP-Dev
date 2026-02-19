/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * Register_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Auxiliaries.Commands
{
    //Original name = Register
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Auxiliaries.Register")]
    public sealed class RegisterCommand : Command<RegisterCommand.PayloadData>
    {
        public RegisterCommand()
            : base()
        { }

        public RegisterCommand(int RequestId, RegisterCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(OperatorSwitchEnum? OperatorSwitch = null, TamperSensorEnum? TamperSensor = null, InternalTamperSensorEnum? InternalTamperSensor = null, SeismicSensorEnum? SeismicSensor = null, HeatSensorEnum? HeatSensor = null, ProximitySensorEnum? ProximitySensor = null, AmbientLightSensorEnum? AmbientLightSensor = null, EnhancedAudioSensorEnum? EnhancedAudioSensor = null, BootSwitchSensorEnum? BootSwitchSensor = null, ConsumerDisplaySensorEnum? ConsumerDisplaySensor = null, OperatorCallButtonSensorEnum? OperatorCallButtonSensor = null, HandsetSensorEnum? HandsetSensor = null, HeadsetMicrophoneSensorEnum? HeadsetMicrophoneSensor = null, FasciaMicrophoneSensorEnum? FasciaMicrophoneSensor = null, CabinetDoorEnum? CabinetDoor = null, SafeDoorEnum? SafeDoor = null, VandalShieldEnum? VandalShield = null, CabinetFrontEnum? CabinetFront = null, CabinetRearEnum? CabinetRear = null, CabinetRightEnum? CabinetRight = null, CabinetLeftEnum? CabinetLeft = null, OpenCloseIndicatorEnum? OpenCloseIndicator = null, AudioIndicatorEnum? AudioIndicator = null, HeatingIndicatorEnum? HeatingIndicator = null, ConsumerDisplayBacklightEnum? ConsumerDisplayBacklight = null, SignageDisplayEnum? SignageDisplay = null, VolumeEnum? Volume = null, UpsEnum? Ups = null, AudibleAlarmEnum? AudibleAlarm = null, EnhancedAudioControlEnum? EnhancedAudioControl = null, EnhancedMicrophoneControlEnum? EnhancedMicrophoneControl = null, MicrophoneVolumeEnum? MicrophoneVolume = null)
                : base()
            {
                this.OperatorSwitch = OperatorSwitch;
                this.TamperSensor = TamperSensor;
                this.InternalTamperSensor = InternalTamperSensor;
                this.SeismicSensor = SeismicSensor;
                this.HeatSensor = HeatSensor;
                this.ProximitySensor = ProximitySensor;
                this.AmbientLightSensor = AmbientLightSensor;
                this.EnhancedAudioSensor = EnhancedAudioSensor;
                this.BootSwitchSensor = BootSwitchSensor;
                this.ConsumerDisplaySensor = ConsumerDisplaySensor;
                this.OperatorCallButtonSensor = OperatorCallButtonSensor;
                this.HandsetSensor = HandsetSensor;
                this.HeadsetMicrophoneSensor = HeadsetMicrophoneSensor;
                this.FasciaMicrophoneSensor = FasciaMicrophoneSensor;
                this.CabinetDoor = CabinetDoor;
                this.SafeDoor = SafeDoor;
                this.VandalShield = VandalShield;
                this.CabinetFront = CabinetFront;
                this.CabinetRear = CabinetRear;
                this.CabinetRight = CabinetRight;
                this.CabinetLeft = CabinetLeft;
                this.OpenCloseIndicator = OpenCloseIndicator;
                this.AudioIndicator = AudioIndicator;
                this.HeatingIndicator = HeatingIndicator;
                this.ConsumerDisplayBacklight = ConsumerDisplayBacklight;
                this.SignageDisplay = SignageDisplay;
                this.Volume = Volume;
                this.Ups = Ups;
                this.AudibleAlarm = AudibleAlarm;
                this.EnhancedAudioControl = EnhancedAudioControl;
                this.EnhancedMicrophoneControl = EnhancedMicrophoneControl;
                this.MicrophoneVolume = MicrophoneVolume;
            }

            public enum OperatorSwitchEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Operator Switch should report whenever the switch changes the operating mode:
            /// 
            /// * ```register``` - Report when this sensor is triggered.
            /// * ```deregister``` - Do not report when this sensor is triggered.
            /// 
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "operatorSwitch")]
            public OperatorSwitchEnum? OperatorSwitch { get; init; }

            public enum TamperSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Tamper Sensor should report whenever someone tampers with the terminal. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "tamperSensor")]
            public TamperSensorEnum? TamperSensor { get; init; }

            public enum InternalTamperSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Internal Tamper Sensor should report whenever someone tampers with the internal
            /// alarm. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "internalTamperSensor")]
            public InternalTamperSensorEnum? InternalTamperSensor { get; init; }

            public enum SeismicSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Seismic Sensor should report whenever any seismic activity is detected. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "seismicSensor")]
            public SeismicSensorEnum? SeismicSensor { get; init; }

            public enum HeatSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Heat Sensor should report whenever any excessive heat is detected. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "heatSensor")]
            public HeatSensorEnum? HeatSensor { get; init; }

            public enum ProximitySensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Proximity Sensor should report whenever any movement is detected close to the
            /// terminal. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "proximitySensor")]
            public ProximitySensorEnum? ProximitySensor { get; init; }

            public enum AmbientLightSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Ambient Light Sensor should report whenever it detects changes in the ambient light. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "ambientLightSensor")]
            public AmbientLightSensorEnum? AmbientLightSensor { get; init; }

            public enum EnhancedAudioSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Audio Jack should report whenever it detects changes in the audio jack. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "enhancedAudioSensor")]
            public EnhancedAudioSensorEnum? EnhancedAudioSensor { get; init; }

            public enum BootSwitchSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Boot Switch should report whenever the delayed effect boot switch is used. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "bootSwitchSensor")]
            public BootSwitchSensorEnum? BootSwitchSensor { get; init; }

            public enum ConsumerDisplaySensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Consumer Display Sensor should report whenever it detects changes to the consumer
            /// display. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "consumerDisplaySensor")]
            public ConsumerDisplaySensorEnum? ConsumerDisplaySensor { get; init; }

            public enum OperatorCallButtonSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Operator Call Button should report whenever the Operator Call Button is pressed or
            /// released. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "operatorCallButtonSensor")]
            public OperatorCallButtonSensorEnum? OperatorCallButtonSensor { get; init; }

            public enum HandsetSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Handset Sensor should report whenever it detects changes of its status. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "handsetSensor")]
            public HandsetSensorEnum? HandsetSensor { get; init; }

            public enum HeadsetMicrophoneSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Microphone Jack should report whenever it detects changes in the microphone jack. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "headsetMicrophoneSensor")]
            public HeadsetMicrophoneSensorEnum? HeadsetMicrophoneSensor { get; init; }

            public enum FasciaMicrophoneSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Fascia Microphone should report whenever it detects changes in the microphone state. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "fasciaMicrophoneSensor")]
            public FasciaMicrophoneSensorEnum? FasciaMicrophoneSensor { get; init; }

            public enum CabinetDoorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Cabinet Doors should report whenever the doors are opened, closed, bolted or locked. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "cabinetDoor")]
            public CabinetDoorEnum? CabinetDoor { get; init; }

            public enum SafeDoorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Safe Doors should report whenever the doors are opened, closed, bolted or locked. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "safeDoor")]
            public SafeDoorEnum? SafeDoor { get; init; }

            public enum VandalShieldEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Vandal Shield should report whenever the shield changed position. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "vandalShield")]
            public VandalShieldEnum? VandalShield { get; init; }

            public enum CabinetFrontEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the front Cabinet Doors should report whenever the front doors are opened, closed, bolted
            /// or locked. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "cabinetFront")]
            public CabinetFrontEnum? CabinetFront { get; init; }

            public enum CabinetRearEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the rear Cabinet Doors should report whenever the front doors are opened, closed, bolted
            /// or locked. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "cabinetRear")]
            public CabinetRearEnum? CabinetRear { get; init; }

            public enum CabinetRightEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the right Cabinet Doors should report whenever the front doors are opened, closed, bolted
            /// or locked. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "cabinetRight")]
            public CabinetRightEnum? CabinetRight { get; init; }

            public enum CabinetLeftEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the left Cabinet Doors should report whenever the front doors are opened, closed, bolted
            /// or locked. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "cabinetLeft")]
            public CabinetLeftEnum? CabinetLeft { get; init; }

            public enum OpenCloseIndicatorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Open/Closed Indicator should report whenever it is turned on (set to open) or turned
            /// off (set to closed). See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "openCloseIndicator")]
            public OpenCloseIndicatorEnum? OpenCloseIndicator { get; init; }

            public enum AudioIndicatorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Audio Indicator should report whenever it is turned on or turned off. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "audioIndicator")]
            public AudioIndicatorEnum? AudioIndicator { get; init; }

            public enum HeatingIndicatorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Heating device should report whenever it is turned on or turned off. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "heatingIndicator")]
            public HeatingIndicatorEnum? HeatingIndicator { get; init; }

            public enum ConsumerDisplayBacklightEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Consumer Display Backlight should report whenever it is turned on or turned off. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "consumerDisplayBacklight")]
            public ConsumerDisplayBacklightEnum? ConsumerDisplayBacklight { get; init; }

            public enum SignageDisplayEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Signage Display should report whenever it is turned on or turned off. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "signageDisplay")]
            public SignageDisplayEnum? SignageDisplay { get; init; }

            public enum VolumeEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Volume Control device should report whenever it is changed. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "volume")]
            public VolumeEnum? Volume { get; init; }

            public enum UpsEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the UPS device should report whenever it is changed. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "ups")]
            public UpsEnum? Ups { get; init; }

            public enum AudibleAlarmEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Audible Alarm device should report whenever it is changed. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "audibleAlarm")]
            public AudibleAlarmEnum? AudibleAlarm { get; init; }

            public enum EnhancedAudioControlEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Enhanced Audio Controller should report whenever it changes status (assuming the
            /// device is capable of generating events). See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "enhancedAudioControl")]
            public EnhancedAudioControlEnum? EnhancedAudioControl { get; init; }

            public enum EnhancedMicrophoneControlEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Enhanced Microphone Controller should report whenever it changes status (assuming the
            /// device is capable of generating events). See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "enhancedMicrophoneControl")]
            public EnhancedMicrophoneControlEnum? EnhancedMicrophoneControl { get; init; }

            public enum MicrophoneVolumeEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Microphone Volume Control device should report whenever it is changed. See
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "microphoneVolume")]
            public MicrophoneVolumeEnum? MicrophoneVolume { get; init; }

            public enum AdditionalPropertiesEnum
            {
                Register,
                Deregister
            }

            [System.Text.Json.Serialization.JsonExtensionData]
            public Dictionary<string, System.Text.Json.JsonElement> ExtensionData { get; set; } = new();

            [System.Text.Json.Serialization.JsonIgnore]
            public Dictionary<string, AdditionalPropertiesEnum> ExtendedProperties
            {
                get => MessageBase.ParseExtendedProperties<AdditionalPropertiesEnum>(ExtensionData);
                set => ExtensionData = MessageBase.CreateExtensionData<AdditionalPropertiesEnum>(value);
            }

        }
    }
}
