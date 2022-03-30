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

namespace XFS4IoTFramework.Auxiliaries
{
    public partial class SetAuxiliariesHandler
    {

        private async Task<SetAuxiliariesCompletion.PayloadData> HandleSetAuxiliaries(ISetAuxiliariesEvents events, SetAuxiliariesCommand setAuxiliaries, CancellationToken cancel)
        {
            if (setAuxiliaries.Payload.SafeDoor.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.Safe))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "SafeDoor is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.VandalShield.HasValue)
            {
                if (Device.AuxiliariesCapabilities.VandalShield == AuxiliariesCapabilities.VandalShieldCapabilities.NotAvailable)
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "VandalShield is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.FrontCabinetDoors.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.FrontCabinet))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "FrontCabinetDoors is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.RearCabinetDoors.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.RearCabinet))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "RearCabinetDoors is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.LeftCabinetDoors.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.LeftCabinet))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "LeftCabinetDoors is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.RightCabinetDoors.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilities.DoorType.RightCabinet))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "RightCabinetDoors is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.OpenClose.HasValue) 
            {
                if(!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.OpenCloseIndicator))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "OpenClose is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.FasciaLight.HasValue) 
            {
                //if(!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.FasciaLight))
                //    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "FasciaLight is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.Audio is not null)
            {
                if (!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.Audio))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "Audio is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.Heating.HasValue) 
            {
                if(!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.Heating))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "Heating is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.DisplayBackLight.HasValue) 
            {
                if(!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.ConsumerDisplayBacklight))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "DisplayBackLight is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.SignageDisplay.HasValue) 
            {
                if(!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.SignageDisplay))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "SignageDisplay is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.Volume.HasValue) 
            {
                //if(!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.Volume))
                //    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "Volume is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.Ups.HasValue) 
            {
                if(!Device.AuxiliariesCapabilities.Ups.HasFlag(AuxiliariesCapabilities.UpsEnum.Engaged))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "Engage/Disengage Ups is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.AudibleAlarm.HasValue) 
            {
                if(!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.AudibleAlarm))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "AudibleAlarm is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.EnhancedAudioControl.HasValue) 
            {
                if(!Device.AuxiliariesCapabilities.EnhancedAudioControl.HasFlag(AuxiliariesCapabilities.EnhancedAudioControlEnum.ModeControllable))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "EnhancedAudioControl is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.EnhancedMicrophoneControl.HasValue) 
            {
                if(!Device.AuxiliariesCapabilities.EnhancedMicrophoneControlState.HasFlag(AuxiliariesCapabilities.EnhancedAudioControlEnum.ModeControllable))
                    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "EnhancedMicrophoneControl is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }
            if (setAuxiliaries.Payload.MicrophoneVolume.HasValue) 
            {
                //if(!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.MicrophoneVolume))
                //    return new SetAuxiliariesCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.UnsupportedData, "MicrophoneVolume is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
            }

            
            SetAuxiliariesRequest request = new()
            {
                SafeDoor = setAuxiliaries.Payload.SafeDoor switch
                {
                    SetAuxiliariesCommand.PayloadData.SafeDoorEnum.Bolt => SetAuxiliariesRequest.SetDoorEnum.Bolt,
                    SetAuxiliariesCommand.PayloadData.SafeDoorEnum.Unbolt => SetAuxiliariesRequest.SetDoorEnum.Unbolt,
                    _ => null
                },
                VandalShield = setAuxiliaries.Payload.VandalShield switch
                {
                    SetAuxiliariesCommand.PayloadData.VandalShieldEnum.Closed => SetAuxiliariesRequest.SetVandalShieldEnum.Closed,
                    SetAuxiliariesCommand.PayloadData.VandalShieldEnum.Open => SetAuxiliariesRequest.SetVandalShieldEnum.Open,
                    SetAuxiliariesCommand.PayloadData.VandalShieldEnum.Service => SetAuxiliariesRequest.SetVandalShieldEnum.Service,
                    SetAuxiliariesCommand.PayloadData.VandalShieldEnum.Keyboard => SetAuxiliariesRequest.SetVandalShieldEnum.Keyboard,
                    _ => null
                },
                FrontCabinetDoor = setAuxiliaries.Payload.FrontCabinetDoors switch
                {
                    SetAuxiliariesCommand.PayloadData.FrontCabinetDoorsEnum.Bolt => SetAuxiliariesRequest.SetDoorEnum.Bolt,
                    SetAuxiliariesCommand.PayloadData.FrontCabinetDoorsEnum.Unbolt => SetAuxiliariesRequest.SetDoorEnum.Unbolt,
                    _ => null
                },
                RearCabinetDoor = setAuxiliaries.Payload.RearCabinetDoors switch
                {
                    SetAuxiliariesCommand.PayloadData.RearCabinetDoorsEnum.Bolt => SetAuxiliariesRequest.SetDoorEnum.Bolt,
                    SetAuxiliariesCommand.PayloadData.RearCabinetDoorsEnum.Unbolt => SetAuxiliariesRequest.SetDoorEnum.Unbolt,
                    _ => null
                },
                LeftCabinetDoor = setAuxiliaries.Payload.LeftCabinetDoors switch
                {
                    SetAuxiliariesCommand.PayloadData.LeftCabinetDoorsEnum.Bolt => SetAuxiliariesRequest.SetDoorEnum.Bolt,
                    SetAuxiliariesCommand.PayloadData.LeftCabinetDoorsEnum.Unbolt => SetAuxiliariesRequest.SetDoorEnum.Unbolt,
                    _ => null
                },
                RightCabinetDoor = setAuxiliaries.Payload.RightCabinetDoors switch
                {
                    SetAuxiliariesCommand.PayloadData.RightCabinetDoorsEnum.Bolt => SetAuxiliariesRequest.SetDoorEnum.Bolt,
                    SetAuxiliariesCommand.PayloadData.RightCabinetDoorsEnum.Unbolt => SetAuxiliariesRequest.SetDoorEnum.Unbolt,
                    _ => null
                },
                OpenClosedIndicator = setAuxiliaries.Payload.OpenClose switch
                {
                    SetAuxiliariesCommand.PayloadData.OpenCloseEnum.Open => SetAuxiliariesRequest.SetOpenClosedIndicatorEnum.Open,
                    SetAuxiliariesCommand.PayloadData.OpenCloseEnum.Closed => SetAuxiliariesRequest.SetOpenClosedIndicatorEnum.Closed,
                    _ => null
                },
                FasciaLight = setAuxiliaries.Payload.FasciaLight switch
                {
                    SetAuxiliariesCommand.PayloadData.FasciaLightEnum.On => SetAuxiliariesRequest.SetAuxiliaryOnOff.On,
                    SetAuxiliariesCommand.PayloadData.FasciaLightEnum.Off => SetAuxiliariesRequest.SetAuxiliaryOnOff.Off,
                    _ => null
                },
                AudioRate = setAuxiliaries.Payload.Audio?.Rate switch
                {
                    SetAuxiliariesCommand.PayloadData.AudioClass.RateEnum.On => AuxiliariesStatus.AudioRateEnum.On,
                    SetAuxiliariesCommand.PayloadData.AudioClass.RateEnum.Off => AuxiliariesStatus.AudioRateEnum.Off,
                    _ => null
                },
                AudioSignal = setAuxiliaries.Payload.Audio?.Signal switch
                {
                    SetAuxiliariesCommand.PayloadData.AudioClass.SignalEnum.Keypress => AuxiliariesStatus.AudioSignalEnum.Keypress,
                    SetAuxiliariesCommand.PayloadData.AudioClass.SignalEnum.Exclamation => AuxiliariesStatus.AudioSignalEnum.Exclamation,
                    SetAuxiliariesCommand.PayloadData.AudioClass.SignalEnum.Warning => AuxiliariesStatus.AudioSignalEnum.Warning,
                    SetAuxiliariesCommand.PayloadData.AudioClass.SignalEnum.Error => AuxiliariesStatus.AudioSignalEnum.Error,
                    SetAuxiliariesCommand.PayloadData.AudioClass.SignalEnum.Critical => AuxiliariesStatus.AudioSignalEnum.Critical,
                    _ => null
                },
                Heating = setAuxiliaries.Payload.Heating switch
                {
                    SetAuxiliariesCommand.PayloadData.HeatingEnum.On => SetAuxiliariesRequest.SetAuxiliaryOnOff.On,
                    SetAuxiliariesCommand.PayloadData.HeatingEnum.Off => SetAuxiliariesRequest.SetAuxiliaryOnOff.Off,
                    _ => null
                },
                DisplayBackLight = setAuxiliaries.Payload.DisplayBackLight switch
                {
                    SetAuxiliariesCommand.PayloadData.DisplayBackLightEnum.On => SetAuxiliariesRequest.SetAuxiliaryOnOff.On,
                    SetAuxiliariesCommand.PayloadData.DisplayBackLightEnum.Off => SetAuxiliariesRequest.SetAuxiliaryOnOff.Off,
                    _ => null
                },
                SignageDisplay = setAuxiliaries.Payload.SignageDisplay switch
                {
                    SetAuxiliariesCommand.PayloadData.SignageDisplayEnum.On => SetAuxiliariesRequest.SetAuxiliaryOnOff.On,
                    SetAuxiliariesCommand.PayloadData.SignageDisplayEnum.Off => SetAuxiliariesRequest.SetAuxiliaryOnOff.Off,
                    _ => null
                },
                Volume = setAuxiliaries.Payload.Volume,
                Ups = setAuxiliaries.Payload.Ups switch
                {
                    SetAuxiliariesCommand.PayloadData.UpsEnum.Engage => SetAuxiliariesRequest.SetUpsEnum.Engage,
                    SetAuxiliariesCommand.PayloadData.UpsEnum.Disengage => SetAuxiliariesRequest.SetUpsEnum.Disengage,
                    _ => null
                },
                AudibleAlarm = setAuxiliaries.Payload.AudibleAlarm switch
                {
                    SetAuxiliariesCommand.PayloadData.AudibleAlarmEnum.Off => SetAuxiliariesRequest.SetAuxiliaryOnOff.Off,
                    SetAuxiliariesCommand.PayloadData.AudibleAlarmEnum.On => SetAuxiliariesRequest.SetAuxiliaryOnOff.On,
                    _ => null
                },
                EnhancedAudioControl = setAuxiliaries.Payload.EnhancedAudioControl switch
                {
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PublicAudioManual => AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioManual,
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PublicAudioAuto => AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PublicAudioSemiAuto => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioSemiAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PrivateAudioManual => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioManual,
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PrivateAudioAuto => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PrivateAudioSemiAuto => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioSemiAuto,
                    _ => null
                },
                EnhancedMicrophoneControl = setAuxiliaries.Payload.EnhancedMicrophoneControl switch
                {
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PublicAudioManual => AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioManual,
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PublicAudioAuto => AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PublicAudioSemiAuto => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioSemiAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PrivateAudioManual => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioManual,
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PrivateAudioAuto => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PrivateAudioSemiAuto => AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioSemiAuto,
                    _ => null
                },
                MicrophoneVolume = setAuxiliaries.Payload.MicrophoneVolume
            };


            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.SetAuxiliaries()");

            var result = await Device.SetAuxiliaries(request, cancel);

            Logger.Log(Constants.DeviceClass, $"AuxiliariesDev.SetAuxiliaries() -> {result.CompletionCode}");

            return new SetAuxiliariesCompletion.PayloadData(result.CompletionCode,
                                                            result.ErrorDescription);
        }

    }
}
