/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
    [Command(Name = "Auxiliaries.Register")]
    public sealed class RegisterCommand : Command<RegisterCommand.PayloadData>
    {
        public RegisterCommand(int RequestId, RegisterCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, OperatorSwitchEnum? OperatorSwitch = null, TamperSensorEnum? TamperSensor = null, InternalTamperSensorEnum? InternalTamperSensor = null, SeismicSensorEnum? SeismicSensor = null, HeatSensorEnum? HeatSensor = null, ProximitySensorEnum? ProximitySensor = null, AmbientLightSensorEnum? AmbientLightSensor = null, EnhancedAudioEnum? EnhancedAudio = null, BootSwitchEnum? BootSwitch = null, ConsumerDisplayEnum? ConsumerDisplay = null, OperatorCallButtonEnum? OperatorCallButton = null, HandsetSensorEnum? HandsetSensor = null, HeadsetMicrophoneEnum? HeadsetMicrophone = null, CabinetDoorEnum? CabinetDoor = null, SafeDoorEnum? SafeDoor = null, VandalShieldEnum? VandalShield = null, CabinetFrontEnum? CabinetFront = null, CabinetRearEnum? CabinetRear = null, CabinetRightEnum? CabinetRight = null, CabinetLeftEnum? CabinetLeft = null, OpenCloseIndicatorEnum? OpenCloseIndicator = null, FasciaLightEnum? FasciaLight = null, AudioIndicatorEnum? AudioIndicator = null, HeatingIndicatorEnum? HeatingIndicator = null, ConsumerDisplayBacklightEnum? ConsumerDisplayBacklight = null, SignageDisplayEnum? SignageDisplay = null, VolumeControlEnum? VolumeControl = null, UpsEnum? Ups = null, RemoteStatusMonitorEnum? RemoteStatusMonitor = null, AudibleAlarmEnum? AudibleAlarm = null, EnhancedAudioControlEnum? EnhancedAudioControl = null, EnhancedMicrophoneControlEnum? EnhancedMicrophoneControl = null, MicrophoneVolumeEnum? MicrophoneVolume = null)
                : base(Timeout)
            {
                this.OperatorSwitch = OperatorSwitch;
                this.TamperSensor = TamperSensor;
                this.InternalTamperSensor = InternalTamperSensor;
                this.SeismicSensor = SeismicSensor;
                this.HeatSensor = HeatSensor;
                this.ProximitySensor = ProximitySensor;
                this.AmbientLightSensor = AmbientLightSensor;
                this.EnhancedAudio = EnhancedAudio;
                this.BootSwitch = BootSwitch;
                this.ConsumerDisplay = ConsumerDisplay;
                this.OperatorCallButton = OperatorCallButton;
                this.HandsetSensor = HandsetSensor;
                this.HeadsetMicrophone = HeadsetMicrophone;
                this.CabinetDoor = CabinetDoor;
                this.SafeDoor = SafeDoor;
                this.VandalShield = VandalShield;
                this.CabinetFront = CabinetFront;
                this.CabinetRear = CabinetRear;
                this.CabinetRight = CabinetRight;
                this.CabinetLeft = CabinetLeft;
                this.OpenCloseIndicator = OpenCloseIndicator;
                this.FasciaLight = FasciaLight;
                this.AudioIndicator = AudioIndicator;
                this.HeatingIndicator = HeatingIndicator;
                this.ConsumerDisplayBacklight = ConsumerDisplayBacklight;
                this.SignageDisplay = SignageDisplay;
                this.VolumeControl = VolumeControl;
                this.Ups = Ups;
                this.RemoteStatusMonitor = RemoteStatusMonitor;
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
            /// </summary>
            [DataMember(Name = "ambientLightSensor")]
            public AmbientLightSensorEnum? AmbientLightSensor { get; init; }

            public enum EnhancedAudioEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Audio Jack should report whenever it detects changes in the audio jack. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// </summary>
            [DataMember(Name = "enhancedAudio")]
            public EnhancedAudioEnum? EnhancedAudio { get; init; }

            public enum BootSwitchEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Boot Switch should report whenever the delayed effect boot switch is used. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// </summary>
            [DataMember(Name = "bootSwitch")]
            public BootSwitchEnum? BootSwitch { get; init; }

            public enum ConsumerDisplayEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Consumer Display Sensor should report whenever it detects changes to the consumer
            /// display. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// </summary>
            [DataMember(Name = "consumerDisplay")]
            public ConsumerDisplayEnum? ConsumerDisplay { get; init; }

            public enum OperatorCallButtonEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Operator Call Button should report whenever the Operator Call Button is pressed or
            /// released. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// </summary>
            [DataMember(Name = "operatorCallButton")]
            public OperatorCallButtonEnum? OperatorCallButton { get; init; }

            public enum HandsetSensorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Handset Sensor should report whenever it detects changes of its status. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// </summary>
            [DataMember(Name = "handsetSensor")]
            public HandsetSensorEnum? HandsetSensor { get; init; }

            public enum HeadsetMicrophoneEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Microphone Jack should report whenever it detects changes in the microphone jack. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// </summary>
            [DataMember(Name = "headsetMicrophone")]
            public HeadsetMicrophoneEnum? HeadsetMicrophone { get; init; }

            public enum CabinetDoorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Cabinet Doors should report whenever the doors are opened, closed, bolted or locked. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
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
            /// </summary>
            [DataMember(Name = "openCloseIndicator")]
            public OpenCloseIndicatorEnum? OpenCloseIndicator { get; init; }

            public enum FasciaLightEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Fascia Light should report whenever it is turned on or turned off. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// </summary>
            [DataMember(Name = "fasciaLight")]
            public FasciaLightEnum? FasciaLight { get; init; }

            public enum AudioIndicatorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Audio Indicator should report whenever it is turned on or turned off. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
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
            /// </summary>
            [DataMember(Name = "signageDisplay")]
            public SignageDisplayEnum? SignageDisplay { get; init; }

            public enum VolumeControlEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Volume Control device should report whenever it is changed. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// </summary>
            [DataMember(Name = "volumeControl")]
            public VolumeControlEnum? VolumeControl { get; init; }

            public enum UpsEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the UPS device should report whenever it is changed. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// </summary>
            [DataMember(Name = "ups")]
            public UpsEnum? Ups { get; init; }

            public enum RemoteStatusMonitorEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Remote Status Monitor device should report whenever it is changed. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
            /// </summary>
            [DataMember(Name = "remoteStatusMonitor")]
            public RemoteStatusMonitorEnum? RemoteStatusMonitor { get; init; }

            public enum AudibleAlarmEnum
            {
                Register,
                Deregister
            }

            /// <summary>
            /// Specifies whether the Audible Alarm device should report whenever it is changed. See 
            /// [operatorSwitch](#auxiliaries.register.command.properties.operatorswitch) for the possible values.
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
