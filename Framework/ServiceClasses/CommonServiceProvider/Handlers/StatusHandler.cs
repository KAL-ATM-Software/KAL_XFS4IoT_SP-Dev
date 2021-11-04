/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;

namespace XFS4IoTFramework.Common
{
    [CommandHandlerAsync]
    public partial class StatusHandler
    {
        private Task<StatusCompletion.PayloadData> HandleStatus(IStatusEvents events, StatusCommand status, CancellationToken cancel)
        {
            
            if (Common.CommonStatus is null)
            {
                return Task.FromResult(new StatusCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError, $"No common status is reported by the device class."));
            }

            XFS4IoT.Common.StatusPropertiesClass common = new (
                Device: Common.CommonStatus.Device switch
                {
                    CommonStatusClass.DeviceEnum.DeviceBusy => XFS4IoT.Common.StatusPropertiesClass.DeviceEnum.DeviceBusy,
                    CommonStatusClass.DeviceEnum.FraudAttempt=> XFS4IoT.Common.StatusPropertiesClass.DeviceEnum.FraudAttempt,
                    CommonStatusClass.DeviceEnum.HardwareError=> XFS4IoT.Common.StatusPropertiesClass.DeviceEnum.HardwareError,
                    CommonStatusClass.DeviceEnum.NoDevice=> XFS4IoT.Common.StatusPropertiesClass.DeviceEnum.NoDevice,
                    CommonStatusClass.DeviceEnum.Offline=> XFS4IoT.Common.StatusPropertiesClass.DeviceEnum.Offline,
                    CommonStatusClass.DeviceEnum.Online=> XFS4IoT.Common.StatusPropertiesClass.DeviceEnum.Online,
                    CommonStatusClass.DeviceEnum.PotentialFraud=> XFS4IoT.Common.StatusPropertiesClass.DeviceEnum.PotentialFraud,
                    CommonStatusClass.DeviceEnum.PowerOff=> XFS4IoT.Common.StatusPropertiesClass.DeviceEnum.PowerOff,
                    _ => XFS4IoT.Common.StatusPropertiesClass.DeviceEnum.UserError,
                },
                DevicePosition: Common.CommonStatus.DevicePosition switch
                {
                    CommonStatusClass.PositionStatusEnum.InPosition => XFS4IoT.Common.PositionStatusEnum.InPosition,
                    CommonStatusClass.PositionStatusEnum.NotInPosition=> XFS4IoT.Common.PositionStatusEnum.NotInPosition,
                    _ => XFS4IoT.Common.PositionStatusEnum.Unknown
                },
                PowerSaveRecoveryTime: Common.CommonStatus.PowerSaveRecoveryTime,
                AntiFraudModule: Common.CommonStatus.AntiFraudModule switch
                {
                    CommonStatusClass.AntiFraudModuleEnum.DeviceDetected => XFS4IoT.Common.StatusPropertiesClass.AntiFraudModuleEnum.DeviceDetected,
                    CommonStatusClass.AntiFraudModuleEnum.Inoperable=> XFS4IoT.Common.StatusPropertiesClass.AntiFraudModuleEnum.Inoperable,
                    CommonStatusClass.AntiFraudModuleEnum.Ok=> XFS4IoT.Common.StatusPropertiesClass.AntiFraudModuleEnum.Ok,
                    CommonStatusClass.AntiFraudModuleEnum.Unknown=> XFS4IoT.Common.StatusPropertiesClass.AntiFraudModuleEnum.DeviceDetected,
                    _ => XFS4IoT.Common.StatusPropertiesClass.AntiFraudModuleEnum.NotSupported

                },
                Exchange: Common.CommonStatus.Exchange switch
                {
                    CommonStatusClass.ExchangeEnum.Active => XFS4IoT.Common.ExchangeEnum.Active,
                    CommonStatusClass.ExchangeEnum.Inactive => XFS4IoT.Common.ExchangeEnum.Inactive,
                    _ => XFS4IoT.Common.ExchangeEnum.NotSupported
                }
                );

            XFS4IoT.CardReader.StatusClass cardReader = null;
            if (Common.CardReaderStatus is not null)
            {
                cardReader = new (
                    Media: Common.CardReaderStatus.Media switch
                    {
                        CardReaderStatusClass.MediaEnum.Entering => XFS4IoT.CardReader.StatusClass.MediaEnum.Entering,
                        CardReaderStatusClass.MediaEnum.Jammed => XFS4IoT.CardReader.StatusClass.MediaEnum.Jammed,
                        CardReaderStatusClass.MediaEnum.Latched => XFS4IoT.CardReader.StatusClass.MediaEnum.Latched,
                        CardReaderStatusClass.MediaEnum.NotPresent => XFS4IoT.CardReader.StatusClass.MediaEnum.NotPresent,
                        CardReaderStatusClass.MediaEnum.Present => XFS4IoT.CardReader.StatusClass.MediaEnum.Present,
                        CardReaderStatusClass.MediaEnum.Unknown => XFS4IoT.CardReader.StatusClass.MediaEnum.Unknown,
                        _ => XFS4IoT.CardReader.StatusClass.MediaEnum.NotSupported,
                    },
                    Security: Common.CardReaderStatus.Security switch
                    {
                        CardReaderStatusClass.SecurityEnum.Open => XFS4IoT.CardReader.StatusClass.SecurityEnum.Open,
                        CardReaderStatusClass.SecurityEnum.Ready => XFS4IoT.CardReader.StatusClass.SecurityEnum.Ready,
                        _ => XFS4IoT.CardReader.StatusClass.SecurityEnum.NotSupported,
                    },
                    ChipPower: Common.CardReaderStatus.ChipPower switch
                    {
                        CardReaderStatusClass.ChipPowerEnum.Busy => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.Busy,
                        CardReaderStatusClass.ChipPowerEnum.HardwareError => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.HardwareError,
                        CardReaderStatusClass.ChipPowerEnum.NoCard => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.NoCard,
                        CardReaderStatusClass.ChipPowerEnum.NoDevice => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.NoDevice,
                        CardReaderStatusClass.ChipPowerEnum.Online => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.Online,
                        CardReaderStatusClass.ChipPowerEnum.PoweredOff => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.PoweredOff,
                        CardReaderStatusClass.ChipPowerEnum.Unknown => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.Unknown,
                        _ => XFS4IoT.CardReader.StatusClass.ChipPowerEnum.NotSupported,
                    },
                    ChipModule: Common.CardReaderStatus.ChipModule switch
                    {
                        CardReaderStatusClass.ChipModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.ChipModuleEnum.Inoperable,
                        CardReaderStatusClass.ChipModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.ChipModuleEnum.Ok,
                        CardReaderStatusClass.ChipModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.ChipModuleEnum.Unknown,
                        _ => XFS4IoT.CardReader.StatusClass.ChipModuleEnum.NotSupported,
                    },
                    MagWriteModule: Common.CardReaderStatus.MagWriteModule switch
                    {
                        CardReaderStatusClass.MagWriteModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.MagWriteModuleEnum.Inoperable,
                        CardReaderStatusClass.MagWriteModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.MagWriteModuleEnum.Ok,
                        CardReaderStatusClass.MagWriteModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.MagWriteModuleEnum.Unknown,
                        _ => XFS4IoT.CardReader.StatusClass.MagWriteModuleEnum.NotSupported,
                    },
                    FrontImageModule: Common.CardReaderStatus.FrontImageModule switch
                    {
                        CardReaderStatusClass.FrontImageModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.FrontImageModuleEnum.Inoperable,
                        CardReaderStatusClass.FrontImageModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.FrontImageModuleEnum.Ok,
                        CardReaderStatusClass.FrontImageModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.FrontImageModuleEnum.Unknown,
                        _ => XFS4IoT.CardReader.StatusClass.FrontImageModuleEnum.NotSupported,
                    },
                    BackImageModule: Common.CardReaderStatus.BackImageModule switch
                    {
                        CardReaderStatusClass.BackImageModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.BackImageModuleEnum.Inoperable,
                        CardReaderStatusClass.BackImageModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.BackImageModuleEnum.Ok,
                        CardReaderStatusClass.BackImageModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.BackImageModuleEnum.Unknown,
                        _ => XFS4IoT.CardReader.StatusClass.BackImageModuleEnum.NotSupported,
                    }
                    );
            }

            XFS4IoT.CashDispenser.StatusClass cashDispenser = null;
            if (Common.CashDispenserStatus is not null)
            {
                List<XFS4IoT.CashDispenser.OutPosClass> positions = null;
                if (Common.CashDispenserStatus.Positions is not null &&
                    Common.CashDispenserStatus.Positions.Count > 0)
                {
                    positions = new();
                    foreach (var position in Common.CashDispenserStatus.Positions)
                    {
                        positions.Add(new XFS4IoT.CashDispenser.OutPosClass(
                            Position: position.Key switch
                            {
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom => XFS4IoT.CashManagement.OutputPositionEnum.OutBottom,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Center => XFS4IoT.CashManagement.OutputPositionEnum.OutCenter,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Front => XFS4IoT.CashManagement.OutputPositionEnum.OutFront,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Left => XFS4IoT.CashManagement.OutputPositionEnum.OutLeft,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Rear => XFS4IoT.CashManagement.OutputPositionEnum.OutRear,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Right => XFS4IoT.CashManagement.OutputPositionEnum.OutRight,
                                CashDispenserCapabilitiesClass.OutputPositionEnum.Top => XFS4IoT.CashManagement.OutputPositionEnum.OutTop,
                                _ => XFS4IoT.CashManagement.OutputPositionEnum.OutDefault
                            },
                            Shutter: position.Value.Shutter switch
                            {
                                CashDispenserStatusClass.PositionStatusClass.ShutterEnum.Closed => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Closed,
                                CashDispenserStatusClass.PositionStatusClass.ShutterEnum.Jammed => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Jammed,
                                CashDispenserStatusClass.PositionStatusClass.ShutterEnum.Open => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Open,
                                CashDispenserStatusClass.PositionStatusClass.ShutterEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Unknown,
                                _ => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.NotSupported,
                            },
                            PositionStatus: position.Value.PositionStatus switch
                            {
                                CashDispenserStatusClass.PositionStatusClass.PositionStatusEnum.Empty => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.Empty,
                                CashDispenserStatusClass.PositionStatusClass.PositionStatusEnum.NotEmpty => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.NotEmpty,
                                CashDispenserStatusClass.PositionStatusClass.PositionStatusEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.Unknown,
                                _ => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.NotSupported,
                            },
                            Transport: position.Value.Transport switch
                            {
                                CashDispenserStatusClass.PositionStatusClass.TransportEnum.Inoperative => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Inoperative,
                                CashDispenserStatusClass.PositionStatusClass.TransportEnum.Ok => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Ok,
                                CashDispenserStatusClass.PositionStatusClass.TransportEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Unknown,
                                _ => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.NotSupported,
                            },
                            TransportStatus: position.Value.TransportStatus switch
                            {
                                CashDispenserStatusClass.PositionStatusClass.TransportStatusEnum.Empty => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.Empty,
                                CashDispenserStatusClass.PositionStatusClass.TransportStatusEnum.NotEmpty => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotEmpty,
                                CashDispenserStatusClass.PositionStatusClass.TransportStatusEnum.NotEmptyCustomer => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotEmptyCustomer,
                                CashDispenserStatusClass.PositionStatusClass.TransportStatusEnum.NotEmptyUnknown => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotEmptyUnknown,
                                _ => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotSupported,
                            },
                            JammedShutterPosition: position.Value.JammedShutterPosition switch
                            {
                                CashDispenserStatusClass.PositionStatusClass.JammedShutterPositionEnum.Closed => XFS4IoT.CashDispenser.OutPosClass.JammedShutterPositionEnum.Closed,
                                CashDispenserStatusClass.PositionStatusClass.JammedShutterPositionEnum.NotJammed => XFS4IoT.CashDispenser.OutPosClass.JammedShutterPositionEnum.NotJammed,
                                CashDispenserStatusClass.PositionStatusClass.JammedShutterPositionEnum.Open => XFS4IoT.CashDispenser.OutPosClass.JammedShutterPositionEnum.Open,
                                CashDispenserStatusClass.PositionStatusClass.JammedShutterPositionEnum.PartiallyOpen => XFS4IoT.CashDispenser.OutPosClass.JammedShutterPositionEnum.PartiallyOpen,
                                CashDispenserStatusClass.PositionStatusClass.JammedShutterPositionEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.JammedShutterPositionEnum.Unknown,
                                _ => XFS4IoT.CashDispenser.OutPosClass.JammedShutterPositionEnum.NotSupported,
                            } 
                            ));
                    }
                }

                cashDispenser = new(
                    IntermediateStacker: Common.CashDispenserStatus.IntermediateStacker switch
                    {
                        CashDispenserStatusClass.IntermediateStackerEnum.Empty => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.Empty,
                        CashDispenserStatusClass.IntermediateStackerEnum.NotEmpty => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.NotEmpty,
                        CashDispenserStatusClass.IntermediateStackerEnum.NotEmptyCustomer => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.NotEmptyCustomer,
                        CashDispenserStatusClass.IntermediateStackerEnum.NotEmptyUnknown => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.NotEmptyUnknown,
                        CashDispenserStatusClass.IntermediateStackerEnum.Unknown => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.Unknown,
                        _ => XFS4IoT.CashDispenser.StatusClass.IntermediateStackerEnum.NotSupported,
                    },
                    Positions: positions
                );

            }

            XFS4IoT.CashManagement.StatusClass cashManagement = null;
            if (Common.CashManagementStatus is not null)
            {
                cashManagement = new(
                    SafeDoor: Common.CashManagementStatus.SafeDoor switch
                    {
                        CashManagementStatusClass.SafeDoorEnum.Closed => XFS4IoT.CashManagement.StatusClass.SafeDoorEnum.DoorClosed,
                        CashManagementStatusClass.SafeDoorEnum.Open => XFS4IoT.CashManagement.StatusClass.SafeDoorEnum.DoorOpen,
                        CashManagementStatusClass.SafeDoorEnum.Unknown => XFS4IoT.CashManagement.StatusClass.SafeDoorEnum.DoorUnknown,
                        _ => XFS4IoT.CashManagement.StatusClass.SafeDoorEnum.DoorNotSupported,
                    },
                    Dispenser: Common.CashManagementStatus.Dispenser == CashManagementStatusClass.DispenserEnum.NotSupported ? null :
                        Common.CashManagementStatus.Dispenser switch
                        {
                            CashManagementStatusClass.DispenserEnum.Attention => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Attention,
                            CashManagementStatusClass.DispenserEnum.Ok => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Ok,
                            CashManagementStatusClass.DispenserEnum.Stop => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Stop,
                            _ => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Unknown,
                        },
                    Acceptor: Common.CashManagementStatus.Acceptor == CashManagementStatusClass.AcceptorEnum.NotSupported ? null :
                        Common.CashManagementStatus.Acceptor switch
                        {
                            CashManagementStatusClass.AcceptorEnum.Attention => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Attention,
                            CashManagementStatusClass.AcceptorEnum.Ok => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Ok,
                            CashManagementStatusClass.AcceptorEnum.Stop => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Stop,
                            _ => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Unknown,
                        }
                    );
            }

            XFS4IoT.KeyManagement.StatusClass keyManagement = null;
            if (Common.KeyManagementStatus is not null)
            {
                keyManagement = new(
                    EncryptionState: Common.KeyManagementStatus.EncryptionState switch
                    {
                        KeyManagementStatusClass.EncryptionStateEnum.Busy => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.Busy,
                        KeyManagementStatusClass.EncryptionStateEnum.Initialized => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.Initialized,
                        KeyManagementStatusClass.EncryptionStateEnum.NotInitialized => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.NotInitialized,
                        KeyManagementStatusClass.EncryptionStateEnum.NotReady => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.NotReady,
                        KeyManagementStatusClass.EncryptionStateEnum.Ready => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.Ready,
                        _ => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.Undefined,
                    },
                    CertificateState: Common.KeyManagementStatus.CertificateState == KeyManagementStatusClass.CertificateStateEnum.NotSupported ? null :
                        Common.KeyManagementStatus.CertificateState switch
                        {
                            KeyManagementStatusClass.CertificateStateEnum.NotReady => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.NotReady,
                            KeyManagementStatusClass.CertificateStateEnum.Primary => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.Primary,
                            KeyManagementStatusClass.CertificateStateEnum.Secondary => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.Secondary,
                            _ => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.Unknown,
                        }
                    );
            }

            XFS4IoT.Keyboard.StatusClass keyboard = null;
            if (Common.KeyboardStatus is not null)
            {
                keyboard = new XFS4IoT.Keyboard.StatusClass(
                    AutoBeepMode: Common.KeyboardStatus.AutoBeepMode == KeyboardStatusClass.AutoBeepModeEnum.NotSupported ? null :
                        Common.KeyboardStatus.AutoBeepMode switch
                        {
                            KeyboardStatusClass.AutoBeepModeEnum.Active => XFS4IoT.Keyboard.StatusClass.AutoBeepModeEnum.Active,
                            _ => XFS4IoT.Keyboard.StatusClass.AutoBeepModeEnum.InActive
                        });
            }

            XFS4IoT.TextTerminal.StatusClass textTerminal = null;
            if (Common.TextTerminalStatus is not null)
            {
                textTerminal = new XFS4IoT.TextTerminal.StatusClass(
                    Keyboard: Common.TextTerminalStatus.Keyboard switch
                    {
                        TextTerminalStatusClass.KeyboardEnum.Off => XFS4IoT.TextTerminal.StatusClass.KeyboardEnum.Off,
                        TextTerminalStatusClass.KeyboardEnum.On => XFS4IoT.TextTerminal.StatusClass.KeyboardEnum.On,
                        _ => XFS4IoT.TextTerminal.StatusClass.KeyboardEnum.NotAvailable,
                    },
                    KeyLock: Common.TextTerminalStatus.KeyLock switch
                    {
                        TextTerminalStatusClass.KeyLockEnum.Off => XFS4IoT.TextTerminal.StatusClass.KeyLockEnum.Off,
                        TextTerminalStatusClass.KeyLockEnum.On => XFS4IoT.TextTerminal.StatusClass.KeyLockEnum.On,
                        _ => XFS4IoT.TextTerminal.StatusClass.KeyLockEnum.NotAvailable
                    },
                    DisplaySizeX: Common.TextTerminalStatus.DisplaySizeX,
                    DisplaySizeY: Common.TextTerminalStatus.DisplaySizeY
                    );
            }

            Dictionary<string, XFS4IoT.Lights.LightStateClass> lights = null;
            if (Common.LightsStatus is not null &&
                Common.LightsStatus.Status is not null &&
                Common.LightsStatus.Status.Count > 0)
            {
                lights = new();
                foreach (var light in Common.LightsStatus.Status)
                {
                    lights.Add(light.Key, new (
                        Position: light.Value.Position == LightsStatusClass.LightOperation.PositionEnum.Default ? null :
                        light.Value.Position switch
                        {
                            LightsStatusClass.LightOperation.PositionEnum.Bottom => XFS4IoT.Lights.LightStateClass.PositionEnum.Bottom,
                            LightsStatusClass.LightOperation.PositionEnum.Center => XFS4IoT.Lights.LightStateClass.PositionEnum.Center,
                            LightsStatusClass.LightOperation.PositionEnum.Front => XFS4IoT.Lights.LightStateClass.PositionEnum.Front,
                            LightsStatusClass.LightOperation.PositionEnum.Left => XFS4IoT.Lights.LightStateClass.PositionEnum.Left,
                            LightsStatusClass.LightOperation.PositionEnum.Rear => XFS4IoT.Lights.LightStateClass.PositionEnum.Rear,
                            LightsStatusClass.LightOperation.PositionEnum.Right => XFS4IoT.Lights.LightStateClass.PositionEnum.Right,
                            _ => XFS4IoT.Lights.LightStateClass.PositionEnum.Top,
                        },
                        FlashRate: light.Value.FlashRate switch
                        {
                            LightsStatusClass.LightOperation.FlashRateEnum.Continuous => XFS4IoT.Lights.LightStateClass.FlashRateEnum.Continuous,
                            LightsStatusClass.LightOperation.FlashRateEnum.Medium => XFS4IoT.Lights.LightStateClass.FlashRateEnum.Medium,
                            LightsStatusClass.LightOperation.FlashRateEnum.Off => XFS4IoT.Lights.LightStateClass.FlashRateEnum.Off,
                            LightsStatusClass.LightOperation.FlashRateEnum.Quick => XFS4IoT.Lights.LightStateClass.FlashRateEnum.Quick,
                            _ => XFS4IoT.Lights.LightStateClass.FlashRateEnum.Slow,
                        },
                        Colour: light.Value.Colour == LightsStatusClass.LightOperation.ColourEnum.Default ? null :
                        light.Value.Colour switch
                        {
                            LightsStatusClass.LightOperation.ColourEnum.Blue => XFS4IoT.Lights.LightStateClass.ColourEnum.Blue,
                            LightsStatusClass.LightOperation.ColourEnum.Cyan => XFS4IoT.Lights.LightStateClass.ColourEnum.Cyan,
                            LightsStatusClass.LightOperation.ColourEnum.Green => XFS4IoT.Lights.LightStateClass.ColourEnum.Green,
                            LightsStatusClass.LightOperation.ColourEnum.Magenta => XFS4IoT.Lights.LightStateClass.ColourEnum.Magenta,
                            LightsStatusClass.LightOperation.ColourEnum.Red => XFS4IoT.Lights.LightStateClass.ColourEnum.Red,
                            LightsStatusClass.LightOperation.ColourEnum.White => XFS4IoT.Lights.LightStateClass.ColourEnum.White,
                            _ => XFS4IoT.Lights.LightStateClass.ColourEnum.Yellow,
                        },
                        Direction: light.Value.Direction == LightsStatusClass.LightOperation.DirectionEnum.None ? null :
                        light.Value.Direction switch
                        {
                            LightsStatusClass.LightOperation.DirectionEnum.Entry => XFS4IoT.Lights.LightStateClass.DirectionEnum.Entry,
                            _ => XFS4IoT.Lights.LightStateClass.DirectionEnum.Exit,
                        }
                        ));
                }
            }

            return Task.FromResult(
                new StatusCompletion.PayloadData(
                    MessagePayload.CompletionCodeEnum.Success,
                    string.Empty,
                    Common: common,
                    CardReader: cardReader,
                    CashDispenser: cashDispenser,
                    CashManagement: cashManagement,
                    KeyManagement: keyManagement,
                    Keyboard: keyboard,
                    TextTerminal: textTerminal,
                    Lights: lights)
                );
        }
    }
}
