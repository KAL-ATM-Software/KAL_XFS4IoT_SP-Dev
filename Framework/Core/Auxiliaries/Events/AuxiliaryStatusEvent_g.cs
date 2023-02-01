/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * AuxiliaryStatusEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Auxiliaries.Events
{

    [DataContract]
    [Event(Name = "Auxiliaries.AuxiliaryStatusEvent")]
    public sealed class AuxiliaryStatusEvent : UnsolicitedEvent<AuxiliaryStatusEvent.PayloadData>
    {

        public AuxiliaryStatusEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(OperatorSwitchStateEnum? OperatorSwitch = null, TamperSensorStateEnum? TamperSensor = null, InternalTamperSensorStateEnum? InternalTamperSensor = null, SeismicSensorStateEnum? SeismicSensorState = null, HeatSensorStateEnum? HeatSensorState = null, ProximitySensorStateEnum? ProximitySensorState = null, AmbientLightSensorStateEnum? AmbientLightSensorState = null, EnhancedAudioSensorStateEnum? EnhancedAudioSensorState = null, BootSwitchSensorStateEnum? BootSwitchSensorState = null, DisplaySensorStateEnum? DisplaySensorState = null, OperatorCallButtonSensorStateEnum? OperatorCallButtonSensorState = null, HandsetSensorStateEnum? HandsetSensorState = null, HeadsetMicrophoneSensorStateEnum? HeadsetMicrophoneSensorState = null, FasciaMicrophoneSensorStateEnum? FasciaMicrophoneSensorState = null, SafeDoorStateEnum? SafeDoorState = null, VandalShieldStateEnum? VandalShieldState = null, CabinetFrontDoorStateEnum? CabinetFrontDoorState = null, CabinetRearDoorStateEnum? CabinetRearDoorState = null, CabinetLeftDoorStateEnum? CabinetLeftDoorState = null, CabinetRightDoorStateEnum? CabinetRightDoorState = null, OpenClosedIndicatorStateEnum? OpenClosedIndicatorState = null, AudioStateClass AudioState = null, HeatingStateEnum? HeatingState = null, ConsumerDisplayBacklightStateEnum? ConsumerDisplayBacklightState = null, SignageDisplayStateEnum? SignageDisplayState = null, VolumeStateClass VolumeState = null, UPSStateClass UpsState = null, AudibleAlarmStateEnum? AudibleAlarmState = null, EnhancedAudioControlStateEnum? EnhancedAudioControlState = null, EnhancedMicrophoneControlStateEnum? EnhancedMicrophoneControlState = null, MicrophoneVolumeStateClass MicrophoneVolumeState = null)
                : base()
            {
                this.OperatorSwitch = OperatorSwitch;
                this.TamperSensor = TamperSensor;
                this.InternalTamperSensor = InternalTamperSensor;
                this.SeismicSensorState = SeismicSensorState;
                this.HeatSensorState = HeatSensorState;
                this.ProximitySensorState = ProximitySensorState;
                this.AmbientLightSensorState = AmbientLightSensorState;
                this.EnhancedAudioSensorState = EnhancedAudioSensorState;
                this.BootSwitchSensorState = BootSwitchSensorState;
                this.DisplaySensorState = DisplaySensorState;
                this.OperatorCallButtonSensorState = OperatorCallButtonSensorState;
                this.HandsetSensorState = HandsetSensorState;
                this.HeadsetMicrophoneSensorState = HeadsetMicrophoneSensorState;
                this.FasciaMicrophoneSensorState = FasciaMicrophoneSensorState;
                this.SafeDoorState = SafeDoorState;
                this.VandalShieldState = VandalShieldState;
                this.CabinetFrontDoorState = CabinetFrontDoorState;
                this.CabinetRearDoorState = CabinetRearDoorState;
                this.CabinetLeftDoorState = CabinetLeftDoorState;
                this.CabinetRightDoorState = CabinetRightDoorState;
                this.OpenClosedIndicatorState = OpenClosedIndicatorState;
                this.AudioState = AudioState;
                this.HeatingState = HeatingState;
                this.ConsumerDisplayBacklightState = ConsumerDisplayBacklightState;
                this.SignageDisplayState = SignageDisplayState;
                this.VolumeState = VolumeState;
                this.UpsState = UpsState;
                this.AudibleAlarmState = AudibleAlarmState;
                this.EnhancedAudioControlState = EnhancedAudioControlState;
                this.EnhancedMicrophoneControlState = EnhancedMicrophoneControlState;
                this.MicrophoneVolumeState = MicrophoneVolumeState;
            }

            [DataMember(Name = "operatorSwitch")]
            public OperatorSwitchStateEnum? OperatorSwitch { get; init; }

            [DataMember(Name = "tamperSensor")]
            public TamperSensorStateEnum? TamperSensor { get; init; }

            [DataMember(Name = "internalTamperSensor")]
            public InternalTamperSensorStateEnum? InternalTamperSensor { get; init; }

            [DataMember(Name = "seismicSensorState")]
            public SeismicSensorStateEnum? SeismicSensorState { get; init; }

            [DataMember(Name = "heatSensorState")]
            public HeatSensorStateEnum? HeatSensorState { get; init; }

            [DataMember(Name = "proximitySensorState")]
            public ProximitySensorStateEnum? ProximitySensorState { get; init; }

            [DataMember(Name = "ambientLightSensorState")]
            public AmbientLightSensorStateEnum? AmbientLightSensorState { get; init; }

            [DataMember(Name = "enhancedAudioSensorState")]
            public EnhancedAudioSensorStateEnum? EnhancedAudioSensorState { get; init; }

            [DataMember(Name = "bootSwitchSensorState")]
            public BootSwitchSensorStateEnum? BootSwitchSensorState { get; init; }

            [DataMember(Name = "displaySensorState")]
            public DisplaySensorStateEnum? DisplaySensorState { get; init; }

            [DataMember(Name = "operatorCallButtonSensorState")]
            public OperatorCallButtonSensorStateEnum? OperatorCallButtonSensorState { get; init; }

            [DataMember(Name = "handsetSensorState")]
            public HandsetSensorStateEnum? HandsetSensorState { get; init; }

            [DataMember(Name = "headsetMicrophoneSensorState")]
            public HeadsetMicrophoneSensorStateEnum? HeadsetMicrophoneSensorState { get; init; }

            [DataMember(Name = "fasciaMicrophoneSensorState")]
            public FasciaMicrophoneSensorStateEnum? FasciaMicrophoneSensorState { get; init; }

            [DataMember(Name = "safeDoorState")]
            public SafeDoorStateEnum? SafeDoorState { get; init; }

            [DataMember(Name = "vandalShieldState")]
            public VandalShieldStateEnum? VandalShieldState { get; init; }

            [DataMember(Name = "cabinetFrontDoorState")]
            public CabinetFrontDoorStateEnum? CabinetFrontDoorState { get; init; }

            [DataMember(Name = "cabinetRearDoorState")]
            public CabinetRearDoorStateEnum? CabinetRearDoorState { get; init; }

            [DataMember(Name = "cabinetLeftDoorState")]
            public CabinetLeftDoorStateEnum? CabinetLeftDoorState { get; init; }

            [DataMember(Name = "cabinetRightDoorState")]
            public CabinetRightDoorStateEnum? CabinetRightDoorState { get; init; }

            [DataMember(Name = "openClosedIndicatorState")]
            public OpenClosedIndicatorStateEnum? OpenClosedIndicatorState { get; init; }

            [DataMember(Name = "audioState")]
            public AudioStateClass AudioState { get; init; }

            [DataMember(Name = "heatingState")]
            public HeatingStateEnum? HeatingState { get; init; }

            [DataMember(Name = "consumerDisplayBacklightState")]
            public ConsumerDisplayBacklightStateEnum? ConsumerDisplayBacklightState { get; init; }

            [DataMember(Name = "signageDisplayState")]
            public SignageDisplayStateEnum? SignageDisplayState { get; init; }

            [DataMember(Name = "volumeState")]
            public VolumeStateClass VolumeState { get; init; }

            [DataMember(Name = "upsState")]
            public UPSStateClass UpsState { get; init; }

            [DataMember(Name = "audibleAlarmState")]
            public AudibleAlarmStateEnum? AudibleAlarmState { get; init; }

            [DataMember(Name = "enhancedAudioControlState")]
            public EnhancedAudioControlStateEnum? EnhancedAudioControlState { get; init; }

            [DataMember(Name = "enhancedMicrophoneControlState")]
            public EnhancedMicrophoneControlStateEnum? EnhancedMicrophoneControlState { get; init; }

            [DataMember(Name = "microphoneVolumeState")]
            public MicrophoneVolumeStateClass MicrophoneVolumeState { get; init; }

        }

    }
}
