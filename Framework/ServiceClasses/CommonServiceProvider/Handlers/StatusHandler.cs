/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
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
                    _ => XFS4IoT.Common.StatusPropertiesClass.AntiFraudModuleEnum.Unknown,
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
                        CardReaderStatusClass.SecurityEnum.NotReady => XFS4IoT.CardReader.StatusClass.SecurityEnum.NotReady,
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
                                CashManagementCapabilitiesClass.OutputPositionEnum.Bottom => XFS4IoT.CashManagement.OutputPositionEnum.OutBottom,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Center => XFS4IoT.CashManagement.OutputPositionEnum.OutCenter,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Front => XFS4IoT.CashManagement.OutputPositionEnum.OutFront,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Left => XFS4IoT.CashManagement.OutputPositionEnum.OutLeft,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Rear => XFS4IoT.CashManagement.OutputPositionEnum.OutRear,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Right => XFS4IoT.CashManagement.OutputPositionEnum.OutRight,
                                CashManagementCapabilitiesClass.OutputPositionEnum.Top => XFS4IoT.CashManagement.OutputPositionEnum.OutTop,
                                _ => XFS4IoT.CashManagement.OutputPositionEnum.OutDefault
                            },
                            Shutter: position.Value.Shutter switch
                            {
                                CashManagementStatusClass.ShutterEnum.Closed => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Closed,
                                CashManagementStatusClass.ShutterEnum.JammedOpen => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.JammedOpen,
                                CashManagementStatusClass.ShutterEnum.JammedPartiallyOpen => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.JammedPartiallyOpen,
                                CashManagementStatusClass.ShutterEnum.JammedClosed => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.JammedClosed,
                                CashManagementStatusClass.ShutterEnum.JammedUnknown => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.JammedUnknown,
                                CashManagementStatusClass.ShutterEnum.Open => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Open,
                                CashManagementStatusClass.ShutterEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Unknown,
                                _ => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.NotSupported,
                            },
                            PositionStatus: position.Value.PositionStatus switch
                            {
                                CashManagementStatusClass.PositionStatusEnum.Empty => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.Empty,
                                CashManagementStatusClass.PositionStatusEnum.NotEmpty => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.NotEmpty,
                                CashManagementStatusClass.PositionStatusEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.Unknown,
                                _ => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.NotSupported,
                            },
                            Transport: position.Value.Transport switch
                            {
                                CashManagementStatusClass.TransportEnum.Inoperative => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Inoperative,
                                CashManagementStatusClass.TransportEnum.Ok => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Ok,
                                CashManagementStatusClass.TransportEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Unknown,
                                _ => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.NotSupported,
                            },
                            TransportStatus: position.Value.TransportStatus switch
                            {
                                CashManagementStatusClass.TransportStatusEnum.Empty => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.Empty,
                                CashManagementStatusClass.TransportStatusEnum.NotEmpty => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotEmpty,
                                CashManagementStatusClass.TransportStatusEnum.NotEmptyCustomer => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotEmptyCustomer,
                                CashManagementStatusClass.TransportStatusEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.Unknown,
                                _ => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotSupported,
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
                    new XFS4IoT.Keyboard.StatusClass.AutoBeepModeClass(
                        Common.KeyboardStatus.AutoBeepMode.HasFlag(KeyboardStatusClass.AutoBeepModeEnum.Active),
                        Common.KeyboardStatus.AutoBeepMode.HasFlag(KeyboardStatusClass.AutoBeepModeEnum.InActive)
                        ));
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

            Dictionary<LightsCapabilitiesClass.DeviceEnum, XFS4IoT.Lights.LightStateClass> stdLights = null;
            Dictionary<string, XFS4IoT.Lights.LightStateClass> customLights = null;
            if (Common.LightsStatus?.Status?.Count > 0)
            {
                stdLights = new();
                foreach (var light in Common.LightsStatus.Status)
                {
                    stdLights.Add(light.Key, new (
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
                        Color: light.Value.Colour == LightsStatusClass.LightOperation.ColourEnum.Default ? null :
                        light.Value.Colour switch
                        {
                            LightsStatusClass.LightOperation.ColourEnum.Blue => XFS4IoT.Lights.LightStateClass.ColorEnum.Blue,
                            LightsStatusClass.LightOperation.ColourEnum.Cyan => XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan,
                            LightsStatusClass.LightOperation.ColourEnum.Green => XFS4IoT.Lights.LightStateClass.ColorEnum.Green,
                            LightsStatusClass.LightOperation.ColourEnum.Magenta => XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta,
                            LightsStatusClass.LightOperation.ColourEnum.Red => XFS4IoT.Lights.LightStateClass.ColorEnum.Red,
                            LightsStatusClass.LightOperation.ColourEnum.White => XFS4IoT.Lights.LightStateClass.ColorEnum.White,
                            _ => XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow,
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
            if (Common.LightsStatus?.CustomStatus?.Count > 0)
            {
                customLights = new();
                foreach (var light in Common.LightsStatus.CustomStatus)
                {
                    customLights.Add(light.Key, new(
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
                        Color: light.Value.Colour == LightsStatusClass.LightOperation.ColourEnum.Default ? null :
                        light.Value.Colour switch
                        {
                            LightsStatusClass.LightOperation.ColourEnum.Blue => XFS4IoT.Lights.LightStateClass.ColorEnum.Blue,
                            LightsStatusClass.LightOperation.ColourEnum.Cyan => XFS4IoT.Lights.LightStateClass.ColorEnum.Cyan,
                            LightsStatusClass.LightOperation.ColourEnum.Green => XFS4IoT.Lights.LightStateClass.ColorEnum.Green,
                            LightsStatusClass.LightOperation.ColourEnum.Magenta => XFS4IoT.Lights.LightStateClass.ColorEnum.Magenta,
                            LightsStatusClass.LightOperation.ColourEnum.Red => XFS4IoT.Lights.LightStateClass.ColorEnum.Red,
                            LightsStatusClass.LightOperation.ColourEnum.White => XFS4IoT.Lights.LightStateClass.ColorEnum.White,
                            _ => XFS4IoT.Lights.LightStateClass.ColorEnum.Yellow,
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
            XFS4IoT.Lights.StatusClass lights = null;
            if (stdLights?.Count > 0)
            {
                lights = new XFS4IoT.Lights.StatusClass
                    (
                    CardReader: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CardReader) ? stdLights[LightsCapabilitiesClass.DeviceEnum.CardReader] : null,
                    PinPad: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.PinPad) ? stdLights[LightsCapabilitiesClass.DeviceEnum.PinPad] : null,
                    NotesDispenser: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.NotesDispenser) ? stdLights[LightsCapabilitiesClass.DeviceEnum.NotesDispenser] : null,
                    CoinDispenser: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CoinDispenser) ? stdLights[LightsCapabilitiesClass.DeviceEnum.CoinDispenser] : null,
                    ReceiptPrinter: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter) ? stdLights[LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter] : null,
                    PassbookPrinter: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.PassbookPrinter) ? stdLights[LightsCapabilitiesClass.DeviceEnum.PassbookPrinter] : null,
                    EnvelopeDepository: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository) ? stdLights[LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository] : null,
                    BillAcceptor: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.BillAcceptor) ? stdLights[LightsCapabilitiesClass.DeviceEnum.BillAcceptor] : null,
                    EnvelopeDispenser: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser) ? stdLights[LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser] : null,
                    DocumentPrinter: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.DocumentPrinter) ? stdLights[LightsCapabilitiesClass.DeviceEnum.DocumentPrinter] : null,
                    CoinAcceptor: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CoinAcceptor) ? stdLights[LightsCapabilitiesClass.DeviceEnum.CoinAcceptor] : null,
                    Scanner: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.Scanner) ? stdLights[LightsCapabilitiesClass.DeviceEnum.Scanner] : null,
                    Contactless: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.Contactless) ? stdLights[LightsCapabilitiesClass.DeviceEnum.Contactless] : null, 
                    CardReader2: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CardReader2) ? stdLights[LightsCapabilitiesClass.DeviceEnum.CardReader2] : null,
                    NotesDispenser2: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.NotesDispenser2) ? stdLights[LightsCapabilitiesClass.DeviceEnum.NotesDispenser2] : null,
                    BillAcceptor2: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.BillAcceptor2) ? stdLights[LightsCapabilitiesClass.DeviceEnum.BillAcceptor2] : null,
                    StatusGood: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator) ? stdLights[LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator] : null,
                    StatusWarning: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator) ? stdLights[LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator] : null,
                    StatusBad: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator) ? stdLights[LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator] : null,
                    StatusSupervisor: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusSupervisorIndicator) ? stdLights[LightsCapabilitiesClass.DeviceEnum.StatusSupervisorIndicator] : null,
                    StatusInService: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusInServiceIndicator) ? stdLights[LightsCapabilitiesClass.DeviceEnum.StatusInServiceIndicator] : null,
                    FasciaLight: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.FasciaLight) ? stdLights[LightsCapabilitiesClass.DeviceEnum.FasciaLight] : null
                    );
            }
            if (customLights?.Count > 0)
            {
                if (lights is null)
                {
                    lights = new();
                }
                lights.ExtendedProperties = customLights;
            }

            XFS4IoT.Printer.StatusClass printer = null;
            if (Common.PrinterStatus is not null)
            {
                XFS4IoT.Printer.StatusClass.PaperClass supplyStatus = null;
                XFS4IoT.Printer.StatusClass.PaperTypeClass typeStatus = null;

                if (Common.PrinterStatus.Paper is not null)
                {
                    XFS4IoT.Printer.PaperSupplyEnum? upperSupplyStatus = null;
                    XFS4IoT.Printer.PaperSupplyEnum? lowerSupplyStatus = null;
                    XFS4IoT.Printer.PaperSupplyEnum? externalSupplyStatus = null;
                    XFS4IoT.Printer.PaperSupplyEnum? auxSupplyStatus = null;
                    XFS4IoT.Printer.PaperSupplyEnum? aux2SupplyStatus = null;
                    XFS4IoT.Printer.PaperSupplyEnum? parkSupplyStatus = null;

                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.Upper))
                    {
                        upperSupplyStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.Upper].PaperSupply switch
                        {
                            PrinterStatusClass.PaperSupplyEnum.Full => XFS4IoT.Printer.PaperSupplyEnum.Full,
                            PrinterStatusClass.PaperSupplyEnum.Jammed => XFS4IoT.Printer.PaperSupplyEnum.Jammed,
                            PrinterStatusClass.PaperSupplyEnum.Low => XFS4IoT.Printer.PaperSupplyEnum.Low,
                            PrinterStatusClass.PaperSupplyEnum.Out => XFS4IoT.Printer.PaperSupplyEnum.Out,
                            PrinterStatusClass.PaperSupplyEnum.Unknown => XFS4IoT.Printer.PaperSupplyEnum.Unknown,
                            _ => XFS4IoT.Printer.PaperSupplyEnum.NotSupported,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.Lower))
                    {
                        lowerSupplyStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.Lower].PaperSupply switch
                        {
                            PrinterStatusClass.PaperSupplyEnum.Full => XFS4IoT.Printer.PaperSupplyEnum.Full,
                            PrinterStatusClass.PaperSupplyEnum.Jammed => XFS4IoT.Printer.PaperSupplyEnum.Jammed,
                            PrinterStatusClass.PaperSupplyEnum.Low => XFS4IoT.Printer.PaperSupplyEnum.Low,
                            PrinterStatusClass.PaperSupplyEnum.Out => XFS4IoT.Printer.PaperSupplyEnum.Out,
                            PrinterStatusClass.PaperSupplyEnum.Unknown => XFS4IoT.Printer.PaperSupplyEnum.Unknown,
                            _ => XFS4IoT.Printer.PaperSupplyEnum.NotSupported,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.Lower))
                    {
                        externalSupplyStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.Lower].PaperSupply switch
                        {
                            PrinterStatusClass.PaperSupplyEnum.Full => XFS4IoT.Printer.PaperSupplyEnum.Full,
                            PrinterStatusClass.PaperSupplyEnum.Jammed => XFS4IoT.Printer.PaperSupplyEnum.Jammed,
                            PrinterStatusClass.PaperSupplyEnum.Low => XFS4IoT.Printer.PaperSupplyEnum.Low,
                            PrinterStatusClass.PaperSupplyEnum.Out => XFS4IoT.Printer.PaperSupplyEnum.Out,
                            PrinterStatusClass.PaperSupplyEnum.Unknown => XFS4IoT.Printer.PaperSupplyEnum.Unknown,
                            _ => XFS4IoT.Printer.PaperSupplyEnum.NotSupported,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.AUX))
                    {
                        auxSupplyStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.AUX].PaperSupply switch
                        {
                            PrinterStatusClass.PaperSupplyEnum.Full => XFS4IoT.Printer.PaperSupplyEnum.Full,
                            PrinterStatusClass.PaperSupplyEnum.Jammed => XFS4IoT.Printer.PaperSupplyEnum.Jammed,
                            PrinterStatusClass.PaperSupplyEnum.Low => XFS4IoT.Printer.PaperSupplyEnum.Low,
                            PrinterStatusClass.PaperSupplyEnum.Out => XFS4IoT.Printer.PaperSupplyEnum.Out,
                            PrinterStatusClass.PaperSupplyEnum.Unknown => XFS4IoT.Printer.PaperSupplyEnum.Unknown,
                            _ => XFS4IoT.Printer.PaperSupplyEnum.NotSupported,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.AUX2))
                    {
                        aux2SupplyStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.AUX2].PaperSupply switch
                        {
                            PrinterStatusClass.PaperSupplyEnum.Full => XFS4IoT.Printer.PaperSupplyEnum.Full,
                            PrinterStatusClass.PaperSupplyEnum.Jammed => XFS4IoT.Printer.PaperSupplyEnum.Jammed,
                            PrinterStatusClass.PaperSupplyEnum.Low => XFS4IoT.Printer.PaperSupplyEnum.Low,
                            PrinterStatusClass.PaperSupplyEnum.Out => XFS4IoT.Printer.PaperSupplyEnum.Out,
                            PrinterStatusClass.PaperSupplyEnum.Unknown => XFS4IoT.Printer.PaperSupplyEnum.Unknown,
                            _ => XFS4IoT.Printer.PaperSupplyEnum.NotSupported,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.Park))
                    {
                        parkSupplyStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.Park].PaperSupply switch
                        {
                            PrinterStatusClass.PaperSupplyEnum.Full => XFS4IoT.Printer.PaperSupplyEnum.Full,
                            PrinterStatusClass.PaperSupplyEnum.Jammed => XFS4IoT.Printer.PaperSupplyEnum.Jammed,
                            PrinterStatusClass.PaperSupplyEnum.Low => XFS4IoT.Printer.PaperSupplyEnum.Low,
                            PrinterStatusClass.PaperSupplyEnum.Out => XFS4IoT.Printer.PaperSupplyEnum.Out,
                            PrinterStatusClass.PaperSupplyEnum.Unknown => XFS4IoT.Printer.PaperSupplyEnum.Unknown,
                            _ => XFS4IoT.Printer.PaperSupplyEnum.NotSupported,
                        };
                    }

                    supplyStatus = new(upperSupplyStatus,
                                       lowerSupplyStatus,
                                       externalSupplyStatus,
                                       auxSupplyStatus,
                                       aux2SupplyStatus,
                                       parkSupplyStatus);

                    if (Common.PrinterStatus.CustomPaper is not null)
                    {
                        Dictionary<string, XFS4IoT.Printer.PaperSupplyEnum > customPaperSupplies = new();
                        foreach (var supply in Common.PrinterStatus.CustomPaper)
                        {
                            customPaperSupplies.Add(supply.Key, supply.Value.PaperSupply switch
                            {
                                PrinterStatusClass.PaperSupplyEnum.Full => XFS4IoT.Printer.PaperSupplyEnum.Full,
                                PrinterStatusClass.PaperSupplyEnum.Jammed => XFS4IoT.Printer.PaperSupplyEnum.Jammed,
                                PrinterStatusClass.PaperSupplyEnum.Low => XFS4IoT.Printer.PaperSupplyEnum.Low,
                                PrinterStatusClass.PaperSupplyEnum.Out => XFS4IoT.Printer.PaperSupplyEnum.Out,
                                PrinterStatusClass.PaperSupplyEnum.Unknown => XFS4IoT.Printer.PaperSupplyEnum.Unknown,
                                _ => XFS4IoT.Printer.PaperSupplyEnum.NotSupported,
                            });
                        }
                        supplyStatus.ExtendedProperties = customPaperSupplies;
                    }

                    XFS4IoT.Printer.PaperTypeEnum? upperTypeStatus = null;
                    XFS4IoT.Printer.PaperTypeEnum? lowerTypeStatus = null;
                    XFS4IoT.Printer.PaperTypeEnum? externalTypeStatus = null;
                    XFS4IoT.Printer.PaperTypeEnum? auxTypeStatus = null;
                    XFS4IoT.Printer.PaperTypeEnum? aux2TypeStatus = null;
                    XFS4IoT.Printer.PaperTypeEnum? parkTypeStatus = null;

                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.Upper))
                    {
                        upperTypeStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.Upper].PaperType switch
                        {
                            PrinterStatusClass.PaperTypeEnum.Dual => XFS4IoT.Printer.PaperTypeEnum.Dual,
                            PrinterStatusClass.PaperTypeEnum.Single => XFS4IoT.Printer.PaperTypeEnum.Single,
                            _ => XFS4IoT.Printer.PaperTypeEnum.Unknown,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.Lower))
                    {
                        lowerTypeStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.Lower].PaperType switch
                        {
                            PrinterStatusClass.PaperTypeEnum.Dual => XFS4IoT.Printer.PaperTypeEnum.Dual,
                            PrinterStatusClass.PaperTypeEnum.Single => XFS4IoT.Printer.PaperTypeEnum.Single,
                            _ => XFS4IoT.Printer.PaperTypeEnum.Unknown,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.Lower))
                    {
                        externalTypeStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.Lower].PaperType switch
                        {
                            PrinterStatusClass.PaperTypeEnum.Dual => XFS4IoT.Printer.PaperTypeEnum.Dual,
                            PrinterStatusClass.PaperTypeEnum.Single => XFS4IoT.Printer.PaperTypeEnum.Single,
                            _ => XFS4IoT.Printer.PaperTypeEnum.Unknown,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.AUX))
                    {
                        auxTypeStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.AUX].PaperType switch
                        {
                            PrinterStatusClass.PaperTypeEnum.Dual => XFS4IoT.Printer.PaperTypeEnum.Dual,
                            PrinterStatusClass.PaperTypeEnum.Single => XFS4IoT.Printer.PaperTypeEnum.Single,
                            _ => XFS4IoT.Printer.PaperTypeEnum.Unknown,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.AUX2))
                    {
                        aux2TypeStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.AUX2].PaperType switch
                        {
                            PrinterStatusClass.PaperTypeEnum.Dual => XFS4IoT.Printer.PaperTypeEnum.Dual,
                            PrinterStatusClass.PaperTypeEnum.Single => XFS4IoT.Printer.PaperTypeEnum.Single,
                            _ => XFS4IoT.Printer.PaperTypeEnum.Unknown,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.Park))
                    {
                        parkTypeStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.Park].PaperType switch
                        {
                            PrinterStatusClass.PaperTypeEnum.Dual => XFS4IoT.Printer.PaperTypeEnum.Dual,
                            PrinterStatusClass.PaperTypeEnum.Single => XFS4IoT.Printer.PaperTypeEnum.Single,
                            _ => XFS4IoT.Printer.PaperTypeEnum.Unknown,
                        };
                    }

                    typeStatus = new(upperTypeStatus,
                                     lowerTypeStatus,
                                     externalTypeStatus,
                                     auxTypeStatus,
                                     aux2TypeStatus,
                                     parkTypeStatus);

                    if (Common.PrinterStatus.CustomPaper is not null)
                    {
                        Dictionary<string, XFS4IoT.Printer.PaperTypeEnum> customPaperTypes = new();
                        foreach (var supply in Common.PrinterStatus.CustomPaper)
                        {
                            customPaperTypes.Add(supply.Key, supply.Value.PaperType switch
                            {
                                PrinterStatusClass.PaperTypeEnum.Dual => XFS4IoT.Printer.PaperTypeEnum.Dual,
                                PrinterStatusClass.PaperTypeEnum.Single => XFS4IoT.Printer.PaperTypeEnum.Single,
                                _ => XFS4IoT.Printer.PaperTypeEnum.Unknown,
                            });
                        }
                        typeStatus.ExtendedProperties = customPaperTypes;
                    }
                }

                List<XFS4IoT.Printer.StatusClass.RetractBinsClass> retractBins = null;
                if (Common.PrinterStatus.RetractBins?.Count > 0)
                {
                    retractBins = new();
                    foreach (var bin in Common.PrinterStatus.RetractBins)
                    {
                        retractBins.Add(new(bin.State switch
                        {
                            PrinterStatusClass.RetractBinsClass.StateEnum.Full => XFS4IoT.Printer.StatusClass.RetractBinsClass.StateEnum.Full,
                            PrinterStatusClass.RetractBinsClass.StateEnum.High => XFS4IoT.Printer.StatusClass.RetractBinsClass.StateEnum.High,
                            PrinterStatusClass.RetractBinsClass.StateEnum.Missing => XFS4IoT.Printer.StatusClass.RetractBinsClass.StateEnum.Missing,
                            PrinterStatusClass.RetractBinsClass.StateEnum.Ok => XFS4IoT.Printer.StatusClass.RetractBinsClass.StateEnum.Ok,
                            _ => XFS4IoT.Printer.StatusClass.RetractBinsClass.StateEnum.Unknown,
                        },
                        bin.Count));
                    }
                }

                printer = new XFS4IoT.Printer.StatusClass(
                Common.PrinterStatus.Media switch
                {
                    PrinterStatusClass.MediaEnum.Entering => XFS4IoT.Printer.StatusClass.MediaEnum.Entering,
                    PrinterStatusClass.MediaEnum.Jammed => XFS4IoT.Printer.StatusClass.MediaEnum.Jammed,
                    PrinterStatusClass.MediaEnum.NotPresent => XFS4IoT.Printer.StatusClass.MediaEnum.NotPresent,
                    PrinterStatusClass.MediaEnum.Present => XFS4IoT.Printer.StatusClass.MediaEnum.Present,
                    PrinterStatusClass.MediaEnum.Retracted => XFS4IoT.Printer.StatusClass.MediaEnum.Retracted,
                    PrinterStatusClass.MediaEnum.Unknown => XFS4IoT.Printer.StatusClass.MediaEnum.Unknown,
                    _ => XFS4IoT.Printer.StatusClass.MediaEnum.NotSupported,
                },
                supplyStatus,
                Common.PrinterStatus.Toner switch
                {
                    PrinterStatusClass.TonerEnum.Full => XFS4IoT.Printer.StatusClass.TonerEnum.Full,
                    PrinterStatusClass.TonerEnum.Low => XFS4IoT.Printer.StatusClass.TonerEnum.Low,
                    PrinterStatusClass.TonerEnum.Out => XFS4IoT.Printer.StatusClass.TonerEnum.Out,
                    PrinterStatusClass.TonerEnum.Unknown => XFS4IoT.Printer.StatusClass.TonerEnum.Unknown,
                    _ => XFS4IoT.Printer.StatusClass.TonerEnum.NotSupported,
                },
                Common.PrinterStatus.Ink switch
                {
                    PrinterStatusClass.InkEnum.Full => XFS4IoT.Printer.StatusClass.InkEnum.Full,
                    PrinterStatusClass.InkEnum.Low => XFS4IoT.Printer.StatusClass.InkEnum.Low,
                    PrinterStatusClass.InkEnum.Out => XFS4IoT.Printer.StatusClass.InkEnum.Out,
                    PrinterStatusClass.InkEnum.Unknown => XFS4IoT.Printer.StatusClass.InkEnum.Unknown,
                    _ => XFS4IoT.Printer.StatusClass.InkEnum.NotSupported,
                },
                Common.PrinterStatus.Lamp switch
                {
                    PrinterStatusClass.LampEnum.Fading => XFS4IoT.Printer.StatusClass.LampEnum.Fading,
                    PrinterStatusClass.LampEnum.Inop => XFS4IoT.Printer.StatusClass.LampEnum.Inop,
                    PrinterStatusClass.LampEnum.Ok => XFS4IoT.Printer.StatusClass.LampEnum.Ok,
                    PrinterStatusClass.LampEnum.Unknown => XFS4IoT.Printer.StatusClass.LampEnum.Unknown,
                    _ => XFS4IoT.Printer.StatusClass.LampEnum.NotSupported,
                },
                retractBins,
                Common.PrinterStatus.MediaOnStacker,
                typeStatus,
                Common.PrinterStatus.BlackMarkMode switch
                {
                    PrinterStatusClass.BlackMarkModeEnum.Off => XFS4IoT.Printer.StatusClass.BlackMarkModeEnum.Off,
                    PrinterStatusClass.BlackMarkModeEnum.On => XFS4IoT.Printer.StatusClass.BlackMarkModeEnum.On,
                    PrinterStatusClass.BlackMarkModeEnum.Unknown => XFS4IoT.Printer.StatusClass.BlackMarkModeEnum.Unknown,
                    _ => XFS4IoT.Printer.StatusClass.BlackMarkModeEnum.NotSupported
                });
            }
			
			XFS4IoT.Auxiliaries.StatusClass auxiliaries = null;
            if (Common.AuxiliariesStatus is not null)
            {
                auxiliaries = new(
                    Common.AuxiliariesStatus.OperatorSwitch switch
                    {
                        AuxiliariesStatus.OperatorSwitchEnum.Run => XFS4IoT.Auxiliaries.OperatorSwitchStateEnum.Run,
                        AuxiliariesStatus.OperatorSwitchEnum.Maintenance => XFS4IoT.Auxiliaries.OperatorSwitchStateEnum.Maintenance,
                        AuxiliariesStatus.OperatorSwitchEnum.Supervisor => XFS4IoT.Auxiliaries.OperatorSwitchStateEnum.Supervisor,
                        _ => XFS4IoT.Auxiliaries.OperatorSwitchStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.TamperSensor switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.TamperSensorStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.TamperSensorStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.TamperSensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.InternalTamperSensor switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.InternalTamperSensorStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.InternalTamperSensorStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.InternalTamperSensorStateEnum.NotAvailable,
                    },
                    Common.AuxiliariesStatus.SeismicSensor switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.SeismicSensorStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.SeismicSensorStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.SeismicSensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.HeatSensor switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.HeatSensorStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.HeatSensorStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.HeatSensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.ProximitySensor switch
                    {
                        AuxiliariesStatus.PresenceSensorEnum.Present => XFS4IoT.Auxiliaries.ProximitySensorStateEnum.Present,
                        AuxiliariesStatus.PresenceSensorEnum.NotPresent => XFS4IoT.Auxiliaries.ProximitySensorStateEnum.NotPresent,
                        _ => XFS4IoT.Auxiliaries.ProximitySensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.AmbientLightSensor switch
                    {
                        AuxiliariesStatus.AmbientLightSensorEnum.VeryDark => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.VeryDark,
                        AuxiliariesStatus.AmbientLightSensorEnum.Dark => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.Dark,
                        AuxiliariesStatus.AmbientLightSensorEnum.MediumLight => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.MediumLight,
                        AuxiliariesStatus.AmbientLightSensorEnum.Light => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.Light,
                        AuxiliariesStatus.AmbientLightSensorEnum.VeryLight => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.VeryLight,
                        _ => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.EnhancedAudioSensor switch
                    {
                        AuxiliariesStatus.PresenceSensorEnum.Present => XFS4IoT.Auxiliaries.EnhancedAudioSensorStateEnum.Present,
                        AuxiliariesStatus.PresenceSensorEnum.NotPresent => XFS4IoT.Auxiliaries.EnhancedAudioSensorStateEnum.NotPresent,
                        _ => XFS4IoT.Auxiliaries.EnhancedAudioSensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.BootSwitchSensor switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.BootSwitchSensorStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.BootSwitchSensorStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.BootSwitchSensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.DisplaySensor switch
                    {
                        AuxiliariesStatus.DisplaySensorEnum.Off => XFS4IoT.Auxiliaries.DisplaySensorStateEnum.Off,
                        AuxiliariesStatus.DisplaySensorEnum.On => XFS4IoT.Auxiliaries.DisplaySensorStateEnum.On,
                        AuxiliariesStatus.DisplaySensorEnum.DisplayError => XFS4IoT.Auxiliaries.DisplaySensorStateEnum.DisplayError,
                        _ => XFS4IoT.Auxiliaries.DisplaySensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.OperatorCallButtonSensor switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.OperatorCallButtonSensorStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.OperatorCallButtonSensorStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.OperatorCallButtonSensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.HandsetSensor switch
                    {
                        AuxiliariesStatus.HandsetSensorStatusEnum.OnTheHook => XFS4IoT.Auxiliaries.HandsetSensorStateEnum.OnTheHook,
                        AuxiliariesStatus.HandsetSensorStatusEnum.OffTheHook => XFS4IoT.Auxiliaries.HandsetSensorStateEnum.OffTheHook,
                        _ => XFS4IoT.Auxiliaries.HandsetSensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.HeadsetMicrophoneSensor switch
                    {
                        AuxiliariesStatus.PresenceSensorEnum.Present => XFS4IoT.Auxiliaries.HeadsetMicrophoneSensorStateEnum.Present,
                        AuxiliariesStatus.PresenceSensorEnum.NotPresent => XFS4IoT.Auxiliaries.HeadsetMicrophoneSensorStateEnum.NotPresent,
                        _ => XFS4IoT.Auxiliaries.HeadsetMicrophoneSensorStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.FasciaMicrophoneSensor switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.FasciaMicrophoneSensorStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.FasciaMicrophoneSensorStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.FasciaMicrophoneSensorStateEnum.NotAvailable
                    },
                    (Common.AuxiliariesStatus.Doors?.ContainsKey(AuxiliariesCapabilities.DoorType.Safe) is true ?
                        Common.AuxiliariesStatus.Doors[AuxiliariesCapabilities.DoorType.Safe] switch
                        {
                            AuxiliariesStatus.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Closed,
                            AuxiliariesStatus.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Open,
                            AuxiliariesStatus.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Locked,
                            AuxiliariesStatus.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Bolted,
                            AuxiliariesStatus.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Tampered,
                            _ => XFS4IoT.Auxiliaries.SafeDoorStateEnum.NotAvailable
                        }
                        : XFS4IoT.Auxiliaries.SafeDoorStateEnum.NotAvailable),
                    (Common.AuxiliariesStatus.VandalShield != AuxiliariesStatus.VandalShieldStatusEnum.NotAvailable ?
                        Common.AuxiliariesStatus.VandalShield switch
                        {
                            AuxiliariesStatus.VandalShieldStatusEnum.Closed => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Closed,
                            AuxiliariesStatus.VandalShieldStatusEnum.Open => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Open,
                            AuxiliariesStatus.VandalShieldStatusEnum.Locked => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Locked,
                            AuxiliariesStatus.VandalShieldStatusEnum.Service => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Service,
                            AuxiliariesStatus.VandalShieldStatusEnum.Keyboard => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Keyboard,
                            AuxiliariesStatus.VandalShieldStatusEnum.PartiallyOpen => XFS4IoT.Auxiliaries.VandalShieldStateEnum.PartiallyOpen,
                            AuxiliariesStatus.VandalShieldStatusEnum.Jammed => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Jammed,
                            AuxiliariesStatus.VandalShieldStatusEnum.Tampered => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Tampered,
                            _ => XFS4IoT.Auxiliaries.VandalShieldStateEnum.NotAvailable
                        }
                        : XFS4IoT.Auxiliaries.VandalShieldStateEnum.NotAvailable),
                    (Common.AuxiliariesStatus.Doors?.ContainsKey(AuxiliariesCapabilities.DoorType.FrontCabinet) is true ?
                        Common.AuxiliariesStatus.Doors[AuxiliariesCapabilities.DoorType.FrontCabinet] switch
                        {
                            AuxiliariesStatus.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Closed,
                            AuxiliariesStatus.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Open,
                            AuxiliariesStatus.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Locked,
                            AuxiliariesStatus.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Bolted,
                            AuxiliariesStatus.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Tampered,
                            _ => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.NotAvailable
                        }
                        : XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.NotAvailable),
                    (Common.AuxiliariesStatus.Doors?.ContainsKey(AuxiliariesCapabilities.DoorType.RearCabinet) is true ?
                        Common.AuxiliariesStatus.Doors[AuxiliariesCapabilities.DoorType.RearCabinet] switch
                        {
                            AuxiliariesStatus.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Closed,
                            AuxiliariesStatus.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Open,
                            AuxiliariesStatus.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Locked,
                            AuxiliariesStatus.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Bolted,
                            AuxiliariesStatus.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Tampered,
                            _ => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.NotAvailable
                        }
                        : XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.NotAvailable),
                    (Common.AuxiliariesStatus.Doors?.ContainsKey(AuxiliariesCapabilities.DoorType.LeftCabinet) is true ?
                        Common.AuxiliariesStatus.Doors[AuxiliariesCapabilities.DoorType.LeftCabinet] switch
                        {
                            AuxiliariesStatus.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Closed,
                            AuxiliariesStatus.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Open,
                            AuxiliariesStatus.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Locked,
                            AuxiliariesStatus.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Bolted,
                            AuxiliariesStatus.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Tampered,
                            _ => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.NotAvailable
                        }
                        : XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.NotAvailable),
                    (Common.AuxiliariesStatus.Doors?.ContainsKey(AuxiliariesCapabilities.DoorType.RightCabinet) is true ?
                        Common.AuxiliariesStatus.Doors[AuxiliariesCapabilities.DoorType.RightCabinet] switch
                        {
                            AuxiliariesStatus.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Closed,
                            AuxiliariesStatus.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Open,
                            AuxiliariesStatus.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Locked,
                            AuxiliariesStatus.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Bolted,
                            AuxiliariesStatus.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Tampered,
                            _ => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.NotAvailable
                        }
                        : XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.NotAvailable),
                    Common.AuxiliariesStatus.OpenClosedIndicator switch
                    {
                        AuxiliariesStatus.OpenClosedIndicatorEnum.Closed => XFS4IoT.Auxiliaries.OpenClosedIndicatorStateEnum.Closed,
                        AuxiliariesStatus.OpenClosedIndicatorEnum.Open => XFS4IoT.Auxiliaries.OpenClosedIndicatorStateEnum.Open,
                        _ => XFS4IoT.Auxiliaries.OpenClosedIndicatorStateEnum.NotAvailable
                    }, 
                    new XFS4IoT.Auxiliaries.AudioStateClass(
                        Common.AuxiliariesStatus.AudioRate switch
                        {
                            AuxiliariesStatus.AudioRateEnum.On => XFS4IoT.Auxiliaries.AudioStateClass.RateEnum.On,
                            _ => XFS4IoT.Auxiliaries.AudioStateClass.RateEnum.Off
                        },
                        Common.AuxiliariesStatus.AudioSignal switch
                        {
                            AuxiliariesStatus.AudioSignalEnum.Exclamation => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Exclamation,
                            AuxiliariesStatus.AudioSignalEnum.Warning => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Warning,
                            AuxiliariesStatus.AudioSignalEnum.Error => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Error,
                            AuxiliariesStatus.AudioSignalEnum.Critical => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Critical,
                            _ => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Keypress,
                        }),
                    Common.AuxiliariesStatus.Heating switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.HeatingStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.HeatingStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.HeatingStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.ConsumerDisplayBacklight switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.ConsumerDisplayBacklightStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.ConsumerDisplayBacklightStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.ConsumerDisplayBacklightStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.SignageDisplay switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.SignageDisplayStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.SignageDisplayStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.SignageDisplayStateEnum.NotAvailable
                    },
                    new XFS4IoT.Auxiliaries.VolumeStateClass(Common.AuxiliariesStatus.Volume),
                    new XFS4IoT.Auxiliaries.UPSStateClass(Common.AuxiliariesStatus.UPS.HasFlag(AuxiliariesStatus.UpsStatusEnum.Low),
                                                              Common.AuxiliariesStatus.UPS.HasFlag(AuxiliariesStatus.UpsStatusEnum.Engaged),
                                                              Common.AuxiliariesStatus.UPS.HasFlag(AuxiliariesStatus.UpsStatusEnum.Powering),
                                                              Common.AuxiliariesStatus.UPS.HasFlag(AuxiliariesStatus.UpsStatusEnum.Recovered)
                    ),
                    Common.AuxiliariesStatus.AudibleAlarm switch
                    {
                        AuxiliariesStatus.SensorEnum.On => XFS4IoT.Auxiliaries.AudibleAlarmStateEnum.On,
                        AuxiliariesStatus.SensorEnum.Off => XFS4IoT.Auxiliaries.AudibleAlarmStateEnum.Off,
                        _ => XFS4IoT.Auxiliaries.AudibleAlarmStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.EnhancedAudioControl switch
                    {
                        AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioManual => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PublicAudioManual,
                        AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PublicAudioAuto,
                        AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PublicAudioSemiAuto,
                        AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioManual => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PrivateAudioManual,
                        AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PrivateAudioAuto,
                        AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PrivateAudioSemiAuto,
                        _ => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.NotAvailable
                    },
                    Common.AuxiliariesStatus.EnhancedMicrophoneControl switch
                    {
                        AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioManual => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PublicAudioManual,
                        AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PublicAudioAuto,
                        AuxiliariesStatus.EnhancedAudioControlEnum.PublicAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PublicAudioSemiAuto,
                        AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioManual => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PrivateAudioManual,
                        AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PrivateAudioAuto,
                        AuxiliariesStatus.EnhancedAudioControlEnum.PrivateAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PrivateAudioSemiAuto,
                        _ => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.NotAvailable
                    },
                    new XFS4IoT.Auxiliaries.MicrophoneVolumeStateClass(Common.AuxiliariesStatus.MicrophoneVolume?.Available ?? false, Common.AuxiliariesStatus.MicrophoneVolume?.VolumeLevel ?? 1)
                );
            }

            XFS4IoT.VendorApplication.StatusClass vendorApplication = null;
            if (Common.VendorApplicationStatus is not null)
            {
                vendorApplication = new XFS4IoT.VendorApplication.StatusClass(Common.VendorApplicationStatus.AccessLevel switch
                                                                              {
                                                                                  VendorApplicationStatusClass.AccessLevelEnum.Basic => XFS4IoT.VendorApplication.StatusClass.AccessLevelEnum.Basic,
                                                                                  VendorApplicationStatusClass.AccessLevelEnum.Intermediate => XFS4IoT.VendorApplication.StatusClass.AccessLevelEnum.Intermediate,
                                                                                  VendorApplicationStatusClass.AccessLevelEnum.Full => XFS4IoT.VendorApplication.StatusClass.AccessLevelEnum.Full,
                                                                                  _ => XFS4IoT.VendorApplication.StatusClass.AccessLevelEnum.NotActive,
                                                                              });
            }

            XFS4IoT.VendorMode.StatusClass vendorMode = null;
            if (Common.VendorModeStatus is not null)
            {
                vendorMode = new XFS4IoT.VendorMode.StatusClass(Common.VendorModeStatus.DeviceStatus switch
                                                                {
                                                                    VendorModeStatusClass.DeviceStatusEnum.Offline => XFS4IoT.VendorMode.StatusClass.DeviceEnum.Offline,
                                                                    VendorModeStatusClass.DeviceStatusEnum.Online => XFS4IoT.VendorMode.StatusClass.DeviceEnum.Online,
                                                                    _ => null,
                                                                },
                                                                Common.VendorModeStatus.ServiceStatus switch
                                                                {
                                                                    VendorModeStatusClass.ServiceStatusEnum.Active => XFS4IoT.VendorMode.StatusClass.ServiceEnum.Active,
                                                                    VendorModeStatusClass.ServiceStatusEnum.EnterPending => XFS4IoT.VendorMode.StatusClass.ServiceEnum.EnterPending,
                                                                    VendorModeStatusClass.ServiceStatusEnum.ExitPending => XFS4IoT.VendorMode.StatusClass.ServiceEnum.ExitPending,
                                                                    _ => XFS4IoT.VendorMode.StatusClass.ServiceEnum.Inactive,
                                                                });
            }

            XFS4IoT.BarcodeReader.StatusClass barcodeReader = null;
            if (Common.BarcodeReaderStatus is not null)
            {
                barcodeReader = new XFS4IoT.BarcodeReader.StatusClass(Common.BarcodeReaderStatus.ScannerStatus switch
                {
                    BarcodeReaderStatusClass.ScannerStatusEnum.Inoperative => XFS4IoT.BarcodeReader.StatusClass.ScannerEnum.Inoperative,
                    BarcodeReaderStatusClass.ScannerStatusEnum.Off => XFS4IoT.BarcodeReader.StatusClass.ScannerEnum.Off,
                    BarcodeReaderStatusClass.ScannerStatusEnum.On => XFS4IoT.BarcodeReader.StatusClass.ScannerEnum.On,
                    _ => XFS4IoT.BarcodeReader.StatusClass.ScannerEnum.Unknown,
                });
            }

            XFS4IoT.Biometric.StatusClass biometric = null;
            if(Common.BiometricStatus is not null)
            {
                biometric = new XFS4IoT.Biometric.StatusClass(Common.BiometricStatus.Subject switch
                {
                    BiometricStatusClass.SubjectStatusEnum.NotSupported => XFS4IoT.Biometric.StatusClass.SubjectEnum.NotSupported,
                    BiometricStatusClass.SubjectStatusEnum.NotPresent => XFS4IoT.Biometric.StatusClass.SubjectEnum.NotPresent,
                    BiometricStatusClass.SubjectStatusEnum.Present => XFS4IoT.Biometric.StatusClass.SubjectEnum.Present,
                    _ => XFS4IoT.Biometric.StatusClass.SubjectEnum.Unknown,
                },
                Common.BiometricStatus.Capture,
                Common.BiometricStatus.DataPersistence switch
                {
                    BiometricCapabilitiesClass.PersistenceModesEnum.Persist => XFS4IoT.Biometric.StatusClass.DataPersistenceEnum.Persist,
                    BiometricCapabilitiesClass.PersistenceModesEnum.Clear => XFS4IoT.Biometric.StatusClass.DataPersistenceEnum.Clear,
                    _ => throw Contracts.Fail<NotImplementedException>($"Unexpected value for Common.BiometricStatus.DataPersistence. {Common.BiometricStatus.DataPersistence}")
                },
                Common.BiometricStatus.RemainingStorage);
            }

            XFS4IoT.CashAcceptor.StatusClass cashAcceptor = null;
            if (Common.CashAcceptorStatus is not null)
            {
                List<XFS4IoT.CashAcceptor.PositionClass> positions = null;
                if (Common.CashAcceptorStatus.Positions is not null &&
                    Common.CashAcceptorStatus.Positions.Count > 0)
                {
                    positions = new();
                    foreach (var position in Common.CashAcceptorStatus.Positions)
                    {
                        positions.Add(new XFS4IoT.CashAcceptor.PositionClass(
                            Position: position.Key switch
                            {
                                CashManagementCapabilitiesClass.PositionEnum.InBottom => XFS4IoT.CashManagement.PositionEnum.InBottom,
                                CashManagementCapabilitiesClass.PositionEnum.InCenter => XFS4IoT.CashManagement.PositionEnum.InCenter,
                                CashManagementCapabilitiesClass.PositionEnum.InDefault => XFS4IoT.CashManagement.PositionEnum.InDefault,
                                CashManagementCapabilitiesClass.PositionEnum.InFront => XFS4IoT.CashManagement.PositionEnum.InFront,
                                CashManagementCapabilitiesClass.PositionEnum.InLeft => XFS4IoT.CashManagement.PositionEnum.InLeft,
                                CashManagementCapabilitiesClass.PositionEnum.InRear => XFS4IoT.CashManagement.PositionEnum.InRear,
                                CashManagementCapabilitiesClass.PositionEnum.InRight => XFS4IoT.CashManagement.PositionEnum.InRight,
                                CashManagementCapabilitiesClass.PositionEnum.InTop => XFS4IoT.CashManagement.PositionEnum.InTop,
                                CashManagementCapabilitiesClass.PositionEnum.OutBottom => XFS4IoT.CashManagement.PositionEnum.OutBottom,
                                CashManagementCapabilitiesClass.PositionEnum.OutCenter => XFS4IoT.CashManagement.PositionEnum.OutCenter,
                                CashManagementCapabilitiesClass.PositionEnum.OutDefault => XFS4IoT.CashManagement.PositionEnum.OutDefault,
                                CashManagementCapabilitiesClass.PositionEnum.OutFront => XFS4IoT.CashManagement.PositionEnum.OutFront,
                                CashManagementCapabilitiesClass.PositionEnum.OutLeft => XFS4IoT.CashManagement.PositionEnum.OutLeft,
                                CashManagementCapabilitiesClass.PositionEnum.OutRear => XFS4IoT.CashManagement.PositionEnum.OutRear,
                                CashManagementCapabilitiesClass.PositionEnum.OutRight => XFS4IoT.CashManagement.PositionEnum.OutRight,
                                CashManagementCapabilitiesClass.PositionEnum.OutTop => XFS4IoT.CashManagement.PositionEnum.OutTop,
                                _ => null
                            },
                            Shutter: position.Value.Shutter switch
                            {
                                CashManagementStatusClass.ShutterEnum.Closed => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.Closed,
                                CashManagementStatusClass.ShutterEnum.JammedOpen => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.JammedOpen,
                                CashManagementStatusClass.ShutterEnum.JammedPartiallyOpen => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.JammedPartiallyOpen,
                                CashManagementStatusClass.ShutterEnum.JammedClosed => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.JammedClosed,
                                CashManagementStatusClass.ShutterEnum.JammedUnknown => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.JammedUnknown,
                                CashManagementStatusClass.ShutterEnum.Open => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.Open,
                                CashManagementStatusClass.ShutterEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.Unknown,
                                _ => XFS4IoT.CashAcceptor.PositionClass.ShutterEnum.NotSupported,
                            },
                            PositionStatus: position.Value.PositionStatus switch
                            {
                                CashManagementStatusClass.PositionStatusEnum.Empty => XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum.Empty,
                                CashManagementStatusClass.PositionStatusEnum.NotEmpty => XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum.NotEmpty,
                                CashManagementStatusClass.PositionStatusEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum.Unknown,
                                _ => XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum.NotSupported,
                            },
                            Transport: position.Value.Transport switch
                            {
                                CashManagementStatusClass.TransportEnum.Inoperative => XFS4IoT.CashAcceptor.PositionClass.TransportEnum.Inoperative,
                                CashManagementStatusClass.TransportEnum.Ok => XFS4IoT.CashAcceptor.PositionClass.TransportEnum.Ok,
                                CashManagementStatusClass.TransportEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.TransportEnum.Unknown,
                                _ => XFS4IoT.CashAcceptor.PositionClass.TransportEnum.NotSupported,
                            },
                            TransportStatus: position.Value.TransportStatus switch
                            {
                                CashManagementStatusClass.TransportStatusEnum.Empty => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.Empty,
                                CashManagementStatusClass.TransportStatusEnum.NotEmpty => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.NotEmpty,
                                CashManagementStatusClass.TransportStatusEnum.NotEmptyCustomer => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.NotEmptyCustomer,
                                CashManagementStatusClass.TransportStatusEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.Unknown,
                                _ => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.NotSupported,
                            }
                            ));
                    }
                }

                cashAcceptor = new(
                    IntermediateStacker: Common.CashAcceptorStatus.IntermediateStacker switch
                    {
                        CashAcceptorStatusClass.IntermediateStackerEnum.Empty => XFS4IoT.CashAcceptor.StatusClass.IntermediateStackerEnum.Empty,
                        CashAcceptorStatusClass.IntermediateStackerEnum.NotEmpty => XFS4IoT.CashAcceptor.StatusClass.IntermediateStackerEnum.NotEmpty,
                        CashAcceptorStatusClass.IntermediateStackerEnum.Full => XFS4IoT.CashAcceptor.StatusClass.IntermediateStackerEnum.Full,
                        CashAcceptorStatusClass.IntermediateStackerEnum.Unknown => XFS4IoT.CashAcceptor.StatusClass.IntermediateStackerEnum.Unknown,
                        _ => XFS4IoT.CashAcceptor.StatusClass.IntermediateStackerEnum.NotSupported,
                    },
                    StackerItems: Common.CashAcceptorStatus.StackerItems switch
                    {
                        CashAcceptorStatusClass.StackerItemsEnum.AccessUnknown => XFS4IoT.CashAcceptor.StatusClass.StackerItemsEnum.AccessUnknown,
                        CashAcceptorStatusClass.StackerItemsEnum.CustomerAccess => XFS4IoT.CashAcceptor.StatusClass.StackerItemsEnum.CustomerAccess,
                        CashAcceptorStatusClass.StackerItemsEnum.NoCustomerAccess => XFS4IoT.CashAcceptor.StatusClass.StackerItemsEnum.NoCustomerAccess,
                        CashAcceptorStatusClass.StackerItemsEnum.NoItems => XFS4IoT.CashAcceptor.StatusClass.StackerItemsEnum.NoItems,
                        _ => null,
                    },
                    BanknoteReader: Common.CashAcceptorStatus.BanknoteReader switch
                    {
                        CashAcceptorStatusClass.BanknoteReaderEnum.Inoperable => XFS4IoT.CashAcceptor.StatusClass.BanknoteReaderEnum.Inoperable,
                        CashAcceptorStatusClass.BanknoteReaderEnum.Ok => XFS4IoT.CashAcceptor.StatusClass.BanknoteReaderEnum.Ok,
                        CashAcceptorStatusClass.BanknoteReaderEnum.Unknown => XFS4IoT.CashAcceptor.StatusClass.BanknoteReaderEnum.Unknown,
                        _ => XFS4IoT.CashAcceptor.StatusClass.BanknoteReaderEnum.NotSupported,
                    },
                    DropBox: Common.CashAcceptorStatus.DropBox,
                    Positions: positions);
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
                    Printer: printer,
                    Lights: lights,
					Auxiliaries: auxiliaries,
                    VendorApplication: vendorApplication,
                    VendorMode: vendorMode,
                    BarcodeReader: barcodeReader,
                    Biometric: biometric,
                    CashAcceptor: cashAcceptor)
                );
        }
    }
}
