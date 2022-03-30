/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Auxiliaries.Commands;
using XFS4IoT.Auxiliaries.Completions;
using System.Collections.Generic;

using static XFS4IoT.Auxiliaries.Commands.RegisterCommand.PayloadData;
using System.Linq;

namespace XFS4IoTFramework.Auxiliaries
{
    public partial class RegisterHandler
    {

        private async Task<RegisterCompletion.PayloadData> HandleRegister(IRegisterEvents events, RegisterCommand register, CancellationToken cancel)
        {
            if (register.Payload is null)
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "No Payload provided.");

            RegisterCommand.PayloadData payload = register.Payload;

            EventTypesEnum RegisterEvents = EventTypesEnum.None;
            EventTypesEnum DeregisterEvents = EventTypesEnum.None;

            if (payload.OperatorSwitch is not null && Device.AuxiliariesCapabilities.OperatorSwitch == AuxiliariesCapabilities.OperatorSwitchEnum.NotAvailable)
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "OperatorSwitch is not supported by this device.");
            else if (payload.OperatorSwitch is OperatorSwitchEnum.Register)
                RegisterEvents |= EventTypesEnum.OperatorSwitch;
            else if (payload.OperatorSwitch is OperatorSwitchEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.OperatorSwitch;

            if (payload.TamperSensor is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.TamperSensor))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "TamperSensor is not supported by this device.");
            else if (payload.TamperSensor is TamperSensorEnum.Register)
                RegisterEvents |= EventTypesEnum.TamperSensor;
            else if (payload.TamperSensor is TamperSensorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.TamperSensor;

            if (payload.InternalTamperSensor is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.InternalTamperSensor))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "InternalTamperSensor is not supported by this device.");
            else if (payload.InternalTamperSensor is InternalTamperSensorEnum.Register)
                RegisterEvents |= EventTypesEnum.InternalTamperSensor;
            else if (payload.InternalTamperSensor is InternalTamperSensorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.InternalTamperSensor;

            if (payload.SeismicSensor is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.SeismicSensor))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "SeismicSensor is not supported by this device.");
            else if (payload.SeismicSensor is SeismicSensorEnum.Register)
                RegisterEvents |= EventTypesEnum.SeismicSensor;
            else if (payload.SeismicSensor is SeismicSensorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.SeismicSensor;

            if (payload.HeatSensor is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.HeatSensor))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "HeatSensor is not supported by this device.");
            else if (payload.HeatSensor is HeatSensorEnum.Register)
                RegisterEvents |= EventTypesEnum.HeatSensor;
            else if (payload.HeatSensor is HeatSensorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.HeatSensor;

            if (payload.ProximitySensor is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.ProximitySensor))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "ProximitySensor is not supported by this device.");
            else if (payload.ProximitySensor is ProximitySensorEnum.Register)
                RegisterEvents |= EventTypesEnum.ProximitySensor;
            else if (payload.ProximitySensor is ProximitySensorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.ProximitySensor;

            if (payload.AmbientLightSensor is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.AmbientLightSensor))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "AmbientLightSensor is not supported by this device.");
            else if (payload.AmbientLightSensor is AmbientLightSensorEnum.Register)
                RegisterEvents |= EventTypesEnum.AmbientLightSensor;
            else if (payload.AmbientLightSensor is AmbientLightSensorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.AmbientLightSensor;

            if(payload.EnhancedAudio is not null && Device.AuxiliariesCapabilities.EnhancedAudioSensor == AuxiliariesCapabilities.EnhancedAudioCapabilitiesEnum.NotAvailable)
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "EnhancedAudio is not supported by this device.");
            else if (payload.EnhancedAudio is EnhancedAudioEnum.Register)
                RegisterEvents |= EventTypesEnum.EnhancedAudio;
            else if (payload.EnhancedAudio is EnhancedAudioEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.EnhancedAudio;

            if (payload.BootSwitch is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.BootSwitchSensor))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "BootSwitch is not supported by this device.");
            else if (payload.BootSwitch is BootSwitchEnum.Register)
                RegisterEvents |= EventTypesEnum.BootSwitch;
            else if (payload.BootSwitch is BootSwitchEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.BootSwitch;

            if (payload.ConsumerDisplay is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.ConsumerDisplaySensor))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "ConsumerDisplay is not supported by this device.");
            else if (payload.ConsumerDisplay is ConsumerDisplayEnum.Register)
                RegisterEvents |= EventTypesEnum.ConsumerDisplay;
            else if (payload.ConsumerDisplay is ConsumerDisplayEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.ConsumerDisplay;

            if (payload.OperatorCallButton is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.OperatorCallButtonSensor))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "OperatorCallButton is not supported by this device.");
            else if (payload.OperatorCallButton is OperatorCallButtonEnum.Register)
                RegisterEvents |= EventTypesEnum.OperatorCallButton;
            else if (payload.OperatorCallButton is OperatorCallButtonEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.OperatorCallButton;

            if (payload.HandsetSensor is not null && Device.AuxiliariesCapabilities.HandsetSensor == AuxiliariesCapabilities.HandsetSensorCapabilities.NotAvailable)
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "HandsetSensor is not supported by this device.");
            else if (payload.HandsetSensor is HandsetSensorEnum.Register)
                RegisterEvents |= EventTypesEnum.HandsetSensor;
            else if (payload.HandsetSensor is HandsetSensorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.HandsetSensor;

            if (payload.HeadsetMicrophone is not null && Device.AuxiliariesCapabilities.HeadsetMicrophoneSensor == AuxiliariesCapabilities.HeadsetMicrophoneSensorCapabilities.NotAvailable)
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "HeadsetMicrophone is not supported by this device.");
            else if (payload.HeadsetMicrophone is HeadsetMicrophoneEnum.Register)
                RegisterEvents |= EventTypesEnum.HeadsetMicrophone;
            else if (payload.HeadsetMicrophone is HeadsetMicrophoneEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.HeadsetMicrophone;

            if (payload.CabinetDoor is not null && !Device.AuxiliariesCapabilities.SupportedDoorSensors.Where(c => c.Key != AuxiliariesCapabilities.DoorType.Safe).Any())
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "No CabinetDoors are not supported by this device.");
            else if (payload.CabinetDoor is not null)
            {
                EventTypesEnum doors = EventTypesEnum.None;
                if (Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.FrontCabinet))
                    doors |= EventTypesEnum.CabinetFront;
                if (Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.RearCabinet))
                    doors |= EventTypesEnum.CabinetRear;
                if (Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.LeftCabinet))
                    doors |= EventTypesEnum.CabinetLeft;
                if (Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.RightCabinet))
                    doors |= EventTypesEnum.CabinetRight;

                if (payload.CabinetDoor is CabinetDoorEnum.Register && doors != EventTypesEnum.None)
                    RegisterEvents |= doors;
                else if (payload.CabinetDoor is CabinetDoorEnum.Deregister && doors != EventTypesEnum.None)
                    DeregisterEvents |= doors;
            }

            if (payload.SafeDoor is not null && !Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.Safe))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "SafeDoor is not supported by this device.");
            else if (payload.SafeDoor is SafeDoorEnum.Register)
                RegisterEvents |= EventTypesEnum.SafeDoor;
            else if (payload.SafeDoor is SafeDoorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.SafeDoor;

            if (payload.VandalShield is not null && Device.AuxiliariesCapabilities.VandalShield == AuxiliariesCapabilities.VandalShieldCapabilities.NotAvailable)
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "VandalShield is not supported by this device.");
            else if (payload.VandalShield is VandalShieldEnum.Register)
                RegisterEvents |= EventTypesEnum.VandalShield;
            else if (payload.VandalShield is VandalShieldEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.VandalShield;

            if (payload.CabinetFront is not null && !Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.FrontCabinet))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "CabinetFront is not supported by this device.");
            else if (payload.CabinetFront is CabinetFrontEnum.Register)
                RegisterEvents |= EventTypesEnum.CabinetFront;
            else if (payload.CabinetFront is CabinetFrontEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.CabinetFront;

            if (payload.CabinetFront is not null && !Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.FrontCabinet))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "CabinetFront is not supported by this device.");
            else if (payload.CabinetFront is CabinetFrontEnum.Register)
                RegisterEvents |= EventTypesEnum.CabinetFront;
            else if (payload.CabinetFront is CabinetFrontEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.CabinetFront;

            if (payload.CabinetRear is not null && !Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.RearCabinet))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "CabinetRear is not supported by this device.");
            else if (payload.CabinetRear is CabinetRearEnum.Register)
                RegisterEvents |= EventTypesEnum.CabinetRear;
            else if (payload.CabinetRear is CabinetRearEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.CabinetRear;

            if (payload.CabinetRight is not null && !Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.RightCabinet))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "CabinetRight is not supported by this device.");
            else if (payload.CabinetRight is CabinetRightEnum.Register)
                RegisterEvents |= EventTypesEnum.CabinetRight;
            else if (payload.CabinetRight is CabinetRightEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.CabinetRight;

            if (payload.CabinetLeft is not null && !Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.LeftCabinet))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "CabinetLeft is not supported by this device.");
            else if (payload.CabinetLeft is CabinetLeftEnum.Register)
                RegisterEvents |= EventTypesEnum.CabinetLeft;
            else if (payload.CabinetLeft is CabinetLeftEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.CabinetLeft;

            if (payload.OpenCloseIndicator is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.OpenCloseIndicator))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "OpenCloseIndicator is not supported by this device.");
            else if (payload.OpenCloseIndicator is OpenCloseIndicatorEnum.Register)
                RegisterEvents |= EventTypesEnum.OpenCloseIndicator;
            else if (payload.OpenCloseIndicator is OpenCloseIndicatorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.OpenCloseIndicator;

            if (payload.OpenCloseIndicator is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.OpenCloseIndicator))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "OpenCloseIndicator is not supported by this device.");
            else if (payload.OpenCloseIndicator is OpenCloseIndicatorEnum.Register)
                RegisterEvents |= EventTypesEnum.OpenCloseIndicator;
            else if (payload.OpenCloseIndicator is OpenCloseIndicatorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.OpenCloseIndicator;

            if (payload.FasciaLight is FasciaLightEnum.Register)
                RegisterEvents |= EventTypesEnum.FasciaLight;
            else if (payload.FasciaLight is FasciaLightEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.FasciaLight;

            if (payload.AudioIndicator is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.Audio))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "AudioIndicator is not supported by this device.");
            else if (payload.AudioIndicator is AudioIndicatorEnum.Register)
                RegisterEvents |= EventTypesEnum.AudioIndicator;
            else if (payload.AudioIndicator is AudioIndicatorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.AudioIndicator;

            if (payload.HeatingIndicator is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.Heating))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "HeatingIndicator is not supported by this device.");
            else if (payload.HeatingIndicator is HeatingIndicatorEnum.Register)
                RegisterEvents |= EventTypesEnum.HeatingIndicator;
            else if (payload.HeatingIndicator is HeatingIndicatorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.HeatingIndicator;

            if (payload.ConsumerDisplayBacklight is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.ConsumerDisplayBacklight))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "ConsumerDisplayBacklight is not supported by this device.");
            else if (payload.ConsumerDisplayBacklight is ConsumerDisplayBacklightEnum.Register)
                RegisterEvents |= EventTypesEnum.ConsumerDisplayBacklight;
            else if (payload.ConsumerDisplayBacklight is ConsumerDisplayBacklightEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.ConsumerDisplayBacklight;

            if (payload.SignageDisplay is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.SignageDisplay))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "SignageDisplay is not supported by this device.");
            else if (payload.SignageDisplay is SignageDisplayEnum.Register)
                RegisterEvents |= EventTypesEnum.SignageDisplay;
            else if (payload.SignageDisplay is SignageDisplayEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.SignageDisplay;

            if (payload.VolumeControl is VolumeControlEnum.Register)
                RegisterEvents |= EventTypesEnum.VolumeControl;
            else if (payload.VolumeControl is VolumeControlEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.VolumeControl;

            if (payload.Ups is not null && Device.AuxiliariesCapabilities.Ups == AuxiliariesCapabilities.UpsEnum.NotAvailable)
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "Ups is not supported by this device.");
            else if (payload.Ups is UpsEnum.Register)
                RegisterEvents |= EventTypesEnum.Ups;
            else if (payload.Ups is UpsEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.Ups;

            if (payload.RemoteStatusMonitor is RemoteStatusMonitorEnum.Register)
                RegisterEvents |= EventTypesEnum.RemoteStatusMonitor;
            else if (payload.RemoteStatusMonitor is RemoteStatusMonitorEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.RemoteStatusMonitor;

            if (payload.AudibleAlarm is not null && !Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.AudibleAlarm))
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "AudibleAlarm is not supported by this device.");
            else if (payload.AudibleAlarm is AudibleAlarmEnum.Register)
                RegisterEvents |= EventTypesEnum.AudibleAlarm;
            else if (payload.AudibleAlarm is AudibleAlarmEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.AudibleAlarm;

            if (payload.EnhancedAudioControl is not null && Device.AuxiliariesCapabilities.EnhancedAudioControl == AuxiliariesCapabilities.EnhancedAudioControlEnum.NotAvailable)
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "EnhancedAudioControl is not supported by this device.");
            else if (payload.EnhancedAudioControl is EnhancedAudioControlEnum.Register)
                RegisterEvents |= EventTypesEnum.EnhancedAudioControl;
            else if (payload.EnhancedAudioControl is EnhancedAudioControlEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.EnhancedAudioControl;

            if (payload.EnhancedMicrophoneControl is not null && Device.AuxiliariesCapabilities.EnhancedMicrophoneControlState == AuxiliariesCapabilities.EnhancedAudioControlEnum.NotAvailable)
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "EnhancedMicrophoneControl is not supported by this device.");
            else if (payload.EnhancedMicrophoneControl is EnhancedMicrophoneControlEnum.Register)
                RegisterEvents |= EventTypesEnum.EnhancedMicrophoneControl;
            else if (payload.EnhancedMicrophoneControl is EnhancedMicrophoneControlEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.EnhancedMicrophoneControl;

            if (payload.MicrophoneVolume is MicrophoneVolumeEnum.Register)
                RegisterEvents |= EventTypesEnum.MicrophoneVolume;
            else if (payload.MicrophoneVolume is MicrophoneVolumeEnum.Deregister)
                DeregisterEvents |= EventTypesEnum.MicrophoneVolume;


            if (RegisterEvents != EventTypesEnum.None)
                await Auxiliaries.RegisterStatusChangedEvents(Connection, RegisterEvents, cancel);
            if(DeregisterEvents != EventTypesEnum.None)
                await Auxiliaries.DeregisterStatusChangedEvents(Connection, DeregisterEvents, cancel);

            if (RegisterEvents != EventTypesEnum.None || DeregisterEvents != EventTypesEnum.None)
                return new(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success, null);
            else
                return new RegisterCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "No Auxiliaries specified in Payload.");
        }

    }
}
