/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
                                CashManagementStatusClass.ShutterEnum.Closed => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Closed,
                                CashManagementStatusClass.ShutterEnum.Jammed => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Jammed,
                                CashManagementStatusClass.ShutterEnum.Open => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Open,
                                CashManagementStatusClass.ShutterEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.ShutterEnum.Unknown,
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
                    ChequeUnit: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.ChequeUnit) ? stdLights[LightsCapabilitiesClass.DeviceEnum.ChequeUnit] : null,
                    BillAcceptor: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.BillAcceptor) ? stdLights[LightsCapabilitiesClass.DeviceEnum.BillAcceptor] : null,
                    EnvelopeDispenser: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser) ? stdLights[LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser] : null,
                    DocumentPrinter: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.DocumentPrinter) ? stdLights[LightsCapabilitiesClass.DeviceEnum.DocumentPrinter] : null,
                    CoinAcceptor: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CoinAcceptor) ? stdLights[LightsCapabilitiesClass.DeviceEnum.CoinAcceptor] : null,
                    Scanner: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.Scanner) ? stdLights[LightsCapabilitiesClass.DeviceEnum.Scanner] : null,
                    CardUnit2: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.CardUnit2) ? stdLights[LightsCapabilitiesClass.DeviceEnum.CardUnit2] : null,
                    NotesDispenser2: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.NotesDispenser2) ? stdLights[LightsCapabilitiesClass.DeviceEnum.NotesDispenser2] : null,
                    BillAcceptor2: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.BillAcceptor2) ? stdLights[LightsCapabilitiesClass.DeviceEnum.BillAcceptor2] : null,
                    StatusGood: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator) ? stdLights[LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator] : null,
                    StatusWarning: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator) ? stdLights[LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator] : null,
                    StatusBad: stdLights.ContainsKey(LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator) ? stdLights[LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator] : null,
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
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.Upper))
                    {
                        lowerSupplyStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.Upper].PaperSupply switch
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
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.Upper))
                    {
                        lowerTypeStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.Upper].PaperType switch
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
                    Lights: lights)
                );
        }
    }
}
