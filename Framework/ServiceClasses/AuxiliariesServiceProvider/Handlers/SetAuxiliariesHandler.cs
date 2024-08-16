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
        private async Task<CommandResult<SetAuxiliariesCompletion.PayloadData>> HandleSetAuxiliaries(ISetAuxiliariesEvents events, SetAuxiliariesCommand setAuxiliaries, CancellationToken cancel)
        {
            if (setAuxiliaries.Payload.CabinetDoors.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.Cabinet))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "CabinetDoor is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.SafeDoor.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.Safe))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "SafeDoor is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.VandalShield.HasValue)
            {
                if (Device.AuxiliariesCapabilities.VandalShield == AuxiliariesCapabilitiesClass.VandalShieldCapabilities.NotAvailable)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "VandalShield is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.FrontCabinetDoors.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.FrontCabinet))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "FrontCabinetDoors is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.RearCabinetDoors.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.RearCabinet))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "RearCabinetDoors is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.LeftCabinetDoors.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.LeftCabinet))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "LeftCabinetDoors is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.RightCabinetDoors.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.SupportedDoorSensors.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.RightCabinet))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "RightCabinetDoors is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.OpenClose.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.OpenCloseIndicator))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "OpenClose is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.Audio is not null)
            {
                if (!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.Audio))
                {
                    return new
                        (MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "Audio is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.Heating.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.Heating))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "Heating is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.ConsumerDisplayBackLight.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.ConsumerDisplayBacklight))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "DisplayBackLight is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.SignageDisplay.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.SignageDisplay))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "SignageDisplay is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.Volume.HasValue)
            {
                if (Device.AuxiliariesCapabilities.Volume == -1)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "Volume is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.Ups.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.Ups.HasFlag(AuxiliariesCapabilitiesClass.UpsEnum.Engaged))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "Engage/Disengage Ups is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.AudibleAlarm.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.AudibleAlarm))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "AudibleAlarm is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.EnhancedAudioControl.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.EnhancedAudioControl.HasFlag(AuxiliariesCapabilitiesClass.EnhancedAudioControlEnum.ModeControllable))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "EnhancedAudioControl is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.EnhancedMicrophoneControl.HasValue)
            {
                if (!Device.AuxiliariesCapabilities.EnhancedMicrophoneControlState.HasFlag(AuxiliariesCapabilitiesClass.EnhancedAudioControlEnum.ModeControllable))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedData,
                        "EnhancedMicrophoneControl is not supported by this device.");
                }
            }
            if (setAuxiliaries.Payload.MicrophoneVolume.HasValue) 
            {
                //if(!Device.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilities.AuxiliariesSupportedEnum.MicrophoneVolume))
                //    return new SetAuxiliariesCompletion.PayloadData(MessageHeader.CompletionCodeEnum.UnsupportedData, "MicrophoneVolume is not supported by this device.", SetAuxiliariesCompletion.PayloadData.ErrorCodeEnum.InvalidAuxiliary);
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
                AudioRate = setAuxiliaries.Payload.Audio?.Rate switch
                {
                    SetAuxiliariesCommand.PayloadData.AudioClass.RateEnum.On => AuxiliariesStatusClass.AudioRateEnum.On,
                    SetAuxiliariesCommand.PayloadData.AudioClass.RateEnum.Off => AuxiliariesStatusClass.AudioRateEnum.Off,
                    _ => null
                },
                AudioSignal = setAuxiliaries.Payload.Audio?.Signal switch
                {
                    SetAuxiliariesCommand.PayloadData.AudioClass.SignalEnum.Keypress => AuxiliariesStatusClass.AudioSignalEnum.Keypress,
                    SetAuxiliariesCommand.PayloadData.AudioClass.SignalEnum.Exclamation => AuxiliariesStatusClass.AudioSignalEnum.Exclamation,
                    SetAuxiliariesCommand.PayloadData.AudioClass.SignalEnum.Warning => AuxiliariesStatusClass.AudioSignalEnum.Warning,
                    SetAuxiliariesCommand.PayloadData.AudioClass.SignalEnum.Error => AuxiliariesStatusClass.AudioSignalEnum.Error,
                    SetAuxiliariesCommand.PayloadData.AudioClass.SignalEnum.Critical => AuxiliariesStatusClass.AudioSignalEnum.Critical,
                    _ => null
                },
                Heating = setAuxiliaries.Payload.Heating switch
                {
                    SetAuxiliariesCommand.PayloadData.HeatingEnum.On => SetAuxiliariesRequest.SetAuxiliaryOnOff.On,
                    SetAuxiliariesCommand.PayloadData.HeatingEnum.Off => SetAuxiliariesRequest.SetAuxiliaryOnOff.Off,
                    _ => null
                },
                DisplayBackLight = setAuxiliaries.Payload.ConsumerDisplayBackLight switch
                {
                    SetAuxiliariesCommand.PayloadData.ConsumerDisplayBackLightEnum.On => SetAuxiliariesRequest.SetAuxiliaryOnOff.On,
                    SetAuxiliariesCommand.PayloadData.ConsumerDisplayBackLightEnum.Off => SetAuxiliariesRequest.SetAuxiliaryOnOff.Off,
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
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PublicAudioManual => AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioManual,
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PublicAudioAuto => AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PublicAudioSemiAuto => AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioSemiAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PrivateAudioManual => AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioManual,
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PrivateAudioAuto => AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedAudioControlEnum.PrivateAudioSemiAuto => AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioSemiAuto,
                    _ => null
                },
                EnhancedMicrophoneControl = setAuxiliaries.Payload.EnhancedMicrophoneControl switch
                {
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PublicAudioManual => AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioManual,
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PublicAudioAuto => AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PublicAudioSemiAuto => AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioSemiAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PrivateAudioManual => AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioManual,
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PrivateAudioAuto => AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioAuto,
                    SetAuxiliariesCommand.PayloadData.EnhancedMicrophoneControlEnum.PrivateAudioSemiAuto => AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioSemiAuto,
                    _ => null
                },
                MicrophoneVolume = setAuxiliaries.Payload.MicrophoneVolume
            };


            Logger.Log(Constants.DeviceClass, "AuxiliariesDev.SetAuxiliaries()");

            var result = await Device.SetAuxiliaries(request, cancel);

            Logger.Log(Constants.DeviceClass, $"AuxiliariesDev.SetAuxiliaries() -> {result.CompletionCode}");

            return new(
                result.CompletionCode, 
                result.ErrorDescription);
        }

    }
}
