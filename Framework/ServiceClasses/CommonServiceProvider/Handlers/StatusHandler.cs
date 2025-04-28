/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using XFS4IoT.Lights;
using static XFS4IoTFramework.Common.LightsCapabilitiesClass;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace XFS4IoTFramework.Common
{
    [CommandHandlerAsync]
    public partial class StatusHandler
    {
        private Task<CommandResult<StatusCompletion.PayloadData>> HandleStatus(IStatusEvents events, StatusCommand status, CancellationToken cancel)
        {
            if (Common.CommonStatus is null)
            {
                return Task.FromResult(
                    new CommandResult<StatusCompletion.PayloadData>(
                        MessageHeader.CompletionCodeEnum.InternalError, 
                        $"No common status is reported by the device class.")
                    );
            }

            XFS4IoT.Common.StatusPropertiesClass common = new(
                Device: Common.CommonStatus.Device switch
                {
                    CommonStatusClass.DeviceEnum.DeviceBusy => XFS4IoT.Common.DeviceEnum.DeviceBusy,
                    CommonStatusClass.DeviceEnum.FraudAttempt=> XFS4IoT.Common.DeviceEnum.FraudAttempt,
                    CommonStatusClass.DeviceEnum.HardwareError=> XFS4IoT.Common.DeviceEnum.HardwareError,
                    CommonStatusClass.DeviceEnum.NoDevice=> XFS4IoT.Common.DeviceEnum.NoDevice,
                    CommonStatusClass.DeviceEnum.Offline=> XFS4IoT.Common.DeviceEnum.Offline,
                    CommonStatusClass.DeviceEnum.Online=> XFS4IoT.Common.DeviceEnum.Online,
                    CommonStatusClass.DeviceEnum.PotentialFraud=> XFS4IoT.Common.DeviceEnum.PotentialFraud,
                    CommonStatusClass.DeviceEnum.PowerOff=> XFS4IoT.Common.DeviceEnum.PowerOff,
                    CommonStatusClass.DeviceEnum.UserError => XFS4IoT.Common.DeviceEnum.UserError,
                    _ => throw new InternalErrorException($"Unexpected device status value specified. {Common.CommonStatus.Device}"),
                },
                DevicePosition: Common.CommonStatus.DevicePosition switch
                {
                    CommonStatusClass.PositionStatusEnum.InPosition => XFS4IoT.Common.PositionStatusEnum.InPosition,
                    CommonStatusClass.PositionStatusEnum.NotInPosition=> XFS4IoT.Common.PositionStatusEnum.NotInPosition,
                    CommonStatusClass.PositionStatusEnum.Unknown => XFS4IoT.Common.PositionStatusEnum.Unknown,
                    _ => null
                },
                PowerSaveRecoveryTime: Common.CommonStatus.PowerSaveRecoveryTime,
                AntiFraudModule: Common.CommonStatus.AntiFraudModule switch
                {
                    CommonStatusClass.AntiFraudModuleEnum.DeviceDetected => XFS4IoT.Common.AntiFraudModuleEnum.DeviceDetected,
                    CommonStatusClass.AntiFraudModuleEnum.Inoperable=> XFS4IoT.Common.AntiFraudModuleEnum.Inoperable,
                    CommonStatusClass.AntiFraudModuleEnum.Ok=> XFS4IoT.Common.AntiFraudModuleEnum.Ok,
                    CommonStatusClass.AntiFraudModuleEnum.Unknown => XFS4IoT.Common.AntiFraudModuleEnum.Unknown,
                    _ => null,
                },
                Exchange: Common.CommonStatus.Exchange switch
                {
                    CommonStatusClass.ExchangeEnum.Active => XFS4IoT.Common.ExchangeEnum.Active,
                    CommonStatusClass.ExchangeEnum.Inactive => XFS4IoT.Common.ExchangeEnum.Inactive,
                    _ => null
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
                        _ => null,
                    },
                    Security: Common.CardReaderStatus.Security switch
                    {
                        CardReaderStatusClass.SecurityEnum.Open => XFS4IoT.CardReader.StatusClass.SecurityEnum.Open,
                        CardReaderStatusClass.SecurityEnum.NotReady => XFS4IoT.CardReader.StatusClass.SecurityEnum.NotReady,
                        _ => null,
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
                        _ => null,
                    },
                    ChipModule: Common.CardReaderStatus.ChipModule switch
                    {
                        CardReaderStatusClass.ChipModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.ChipModuleEnum.Inoperable,
                        CardReaderStatusClass.ChipModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.ChipModuleEnum.Ok,
                        CardReaderStatusClass.ChipModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.ChipModuleEnum.Unknown,
                        _ => null,
                    },
                    MagWriteModule: Common.CardReaderStatus.MagWriteModule switch
                    {
                        CardReaderStatusClass.MagWriteModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.MagWriteModuleEnum.Inoperable,
                        CardReaderStatusClass.MagWriteModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.MagWriteModuleEnum.Ok,
                        CardReaderStatusClass.MagWriteModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.MagWriteModuleEnum.Unknown,
                        _ => null,
                    },
                    FrontImageModule: Common.CardReaderStatus.FrontImageModule switch
                    {
                        CardReaderStatusClass.FrontImageModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.FrontImageModuleEnum.Inoperable,
                        CardReaderStatusClass.FrontImageModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.FrontImageModuleEnum.Ok,
                        CardReaderStatusClass.FrontImageModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.FrontImageModuleEnum.Unknown,
                        _ => null,
                    },
                    BackImageModule: Common.CardReaderStatus.BackImageModule switch
                    {
                        CardReaderStatusClass.BackImageModuleEnum.Inoperable => XFS4IoT.CardReader.StatusClass.BackImageModuleEnum.Inoperable,
                        CardReaderStatusClass.BackImageModuleEnum.Ok => XFS4IoT.CardReader.StatusClass.BackImageModuleEnum.Ok,
                        CardReaderStatusClass.BackImageModuleEnum.Unknown => XFS4IoT.CardReader.StatusClass.BackImageModuleEnum.Unknown,
                        _ => null,
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
                    foreach (var position in Common.CashDispenserStatus.Positions)
                    {
                        (positions ??= []).Add(new(
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
                                _ => null,
                            },
                            PositionStatus: position.Value.PositionStatus switch
                            {
                                CashManagementStatusClass.PositionStatusEnum.Empty => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.Empty,
                                CashManagementStatusClass.PositionStatusEnum.NotEmpty => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.NotEmpty,
                                CashManagementStatusClass.PositionStatusEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.PositionStatusEnum.Unknown,
                                _ => null,
                            },
                            Transport: position.Value.Transport switch
                            {
                                CashManagementStatusClass.TransportEnum.Inoperative => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Inoperative,
                                CashManagementStatusClass.TransportEnum.Ok => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Ok,
                                CashManagementStatusClass.TransportEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.TransportEnum.Unknown,
                                _ => null,
                            },
                            TransportStatus: position.Value.TransportStatus switch
                            {
                                CashManagementStatusClass.TransportStatusEnum.Empty => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.Empty,
                                CashManagementStatusClass.TransportStatusEnum.NotEmpty => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotEmpty,
                                CashManagementStatusClass.TransportStatusEnum.NotEmptyCustomer => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.NotEmptyCustomer,
                                CashManagementStatusClass.TransportStatusEnum.Unknown => XFS4IoT.CashDispenser.OutPosClass.TransportStatusEnum.Unknown,
                                _ => null,
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
                        _ => null,
                    },
                    Positions: positions
                );

            }

            XFS4IoT.CashManagement.StatusClass cashManagement = null;
            if (Common.CashManagementStatus is not null)
            {
                cashManagement = new(
                    Dispenser: Common.CashManagementStatus.Dispenser == CashManagementStatusClass.DispenserEnum.NotSupported ? 
                    null : Common.CashManagementStatus.Dispenser switch
                    {
                        CashManagementStatusClass.DispenserEnum.Attention => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Attention,
                        CashManagementStatusClass.DispenserEnum.Ok => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Ok,
                        CashManagementStatusClass.DispenserEnum.Stop => XFS4IoT.CashManagement.StatusClass.DispenserEnum.Stop,
                        _ => null,
                    },
                    Acceptor: Common.CashManagementStatus.Acceptor == CashManagementStatusClass.AcceptorEnum.NotSupported ? 
                    null : Common.CashManagementStatus.Acceptor switch
                    {
                        CashManagementStatusClass.AcceptorEnum.Attention => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Attention,
                        CashManagementStatusClass.AcceptorEnum.Ok => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Ok,
                        CashManagementStatusClass.AcceptorEnum.Stop => XFS4IoT.CashManagement.StatusClass.AcceptorEnum.Stop,
                        _ => null,
                    });
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
                        KeyManagementStatusClass.EncryptionStateEnum.Undefined => XFS4IoT.KeyManagement.StatusClass.EncryptionStateEnum.Undefined,
                        _ => null,
                    },
                    CertificateState: Common.KeyManagementStatus.CertificateState == KeyManagementStatusClass.CertificateStateEnum.NotSupported ? null :
                        Common.KeyManagementStatus.CertificateState switch
                        {
                            KeyManagementStatusClass.CertificateStateEnum.NotReady => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.NotReady,
                            KeyManagementStatusClass.CertificateStateEnum.Primary => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.Primary,
                            KeyManagementStatusClass.CertificateStateEnum.Secondary => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.Secondary,
                            KeyManagementStatusClass.CertificateStateEnum.Unknown => XFS4IoT.KeyManagement.StatusClass.CertificateStateEnum.Unknown,
                            _ => null,
                        }
                    );
            }

            XFS4IoT.Keyboard.StatusClass keyboard = null;
            if (Common.KeyboardStatus is not null)
            {
                keyboard = new XFS4IoT.Keyboard.StatusClass(
                    AutoBeepMode: new(
                        ActiveAvailable: Common.KeyboardStatus.AutoBeepMode.HasFlag(KeyboardStatusClass.AutoBeepModeEnum.Active),
                        InactiveAvailable: Common.KeyboardStatus.AutoBeepMode.HasFlag(KeyboardStatusClass.AutoBeepModeEnum.InActive)
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
                        _ => null,
                    },
                    KeyLock: Common.TextTerminalStatus.KeyLock switch
                    {
                        TextTerminalStatusClass.KeyLockEnum.Off => XFS4IoT.TextTerminal.StatusClass.KeyLockEnum.Off,
                        TextTerminalStatusClass.KeyLockEnum.On => XFS4IoT.TextTerminal.StatusClass.KeyLockEnum.On,
                        _ => null
                    },
                    DisplaySizeX: Common.TextTerminalStatus.DisplaySizeX,
                    DisplaySizeY: Common.TextTerminalStatus.DisplaySizeY
                    );
            }

            XFS4IoT.Lights.StatusClass lights = null;
            if (Common.LightsStatus?.Status is not null ||
                Common.LightsStatus?.CustomStatus is not null)
            {
                Dictionary<string, PositionStatusClass> cardReaderLight = null;
                Dictionary<string, PositionStatusClass> pinPadLight = null;
                Dictionary<string, PositionStatusClass> notesDispenserLight = null;
                Dictionary<string, PositionStatusClass> coinDispenserLight = null;
                Dictionary<string, PositionStatusClass> receiptPrinterLight = null;
                Dictionary<string, PositionStatusClass> passbookPrinterLight = null;
                Dictionary<string, PositionStatusClass> envelopeDepositoryLight = null;
                Dictionary<string, PositionStatusClass> checkUnitLight = null;
                Dictionary<string, PositionStatusClass> billAcceptorLight = null;
                Dictionary<string, PositionStatusClass> envelopeDispenserLight = null;
                Dictionary<string, PositionStatusClass> documentPrinterLight = null;
                Dictionary<string, PositionStatusClass> coinAcceptorLight = null;
                Dictionary<string, PositionStatusClass> scannerLight = null;
                Dictionary<string, PositionStatusClass> contactlessLight = null;
                Dictionary<string, PositionStatusClass> cardReader2Light = null;
                Dictionary<string, PositionStatusClass> notesDispenser2Light = null;
                Dictionary<string, PositionStatusClass> billAcceptor2Light = null;
                Dictionary<string, PositionStatusClass> statusGoodLight = null;
                Dictionary<string, PositionStatusClass> statusWarningLight = null;
                Dictionary<string, PositionStatusClass> statusBadLight = null;
                Dictionary<string, PositionStatusClass> statusSupervisorLight = null;
                Dictionary<string, PositionStatusClass> statusInServiceLight = null;
                Dictionary<string, PositionStatusClass> fasciaLight = null;

                if (Common.LightsStatus?.Status is not null &&
                    Common.LightsStatus?.Status.Count > 0)
                {
                    foreach (var light in Common.LightsStatus.Status)
                    {
                        PositionStatusClass lightStatus = new(
                            FlashRate: light.Value.FlashRate switch
                            {
                                LightsStatusClass.LightOperation.FlashRateEnum.Continuous => PositionStatusClass.FlashRateEnum.Continuous,
                                LightsStatusClass.LightOperation.FlashRateEnum.Medium => PositionStatusClass.FlashRateEnum.Medium,
                                LightsStatusClass.LightOperation.FlashRateEnum.Off => PositionStatusClass.FlashRateEnum.Off,
                                LightsStatusClass.LightOperation.FlashRateEnum.Quick => PositionStatusClass.FlashRateEnum.Quick,
                                LightsStatusClass.LightOperation.FlashRateEnum.Slow => PositionStatusClass.FlashRateEnum.Slow,
                                _ => null
                            },
                            Color: light.Value.Colour switch
                            {
                                LightsStatusClass.LightOperation.ColourEnum.Blue => PositionStatusClass.ColorEnum.Blue,
                                LightsStatusClass.LightOperation.ColourEnum.Cyan => PositionStatusClass.ColorEnum.Cyan,
                                LightsStatusClass.LightOperation.ColourEnum.Green => PositionStatusClass.ColorEnum.Green,
                                LightsStatusClass.LightOperation.ColourEnum.Magenta => PositionStatusClass.ColorEnum.Magenta,
                                LightsStatusClass.LightOperation.ColourEnum.Red => PositionStatusClass.ColorEnum.Red,
                                LightsStatusClass.LightOperation.ColourEnum.White => PositionStatusClass.ColorEnum.White,
                                LightsStatusClass.LightOperation.ColourEnum.Yellow => PositionStatusClass.ColorEnum.Yellow,
                                _ => null
                            },
                            Direction: light.Value.Direction switch
                            {
                                LightsStatusClass.LightOperation.DirectionEnum.Entry => PositionStatusClass.DirectionEnum.Entry,
                                LightsStatusClass.LightOperation.DirectionEnum.Exit => PositionStatusClass.DirectionEnum.Exit,
                                _ => null
                            });

                        if (light.Value.Position == LightsStatusClass.LightOperation.PositionEnum.Custom)
                        {
                            throw new InternalErrorException($"Custom lights are not supported by the framework.");
                        }

                        string lightPositionName = light.Value.Position.ToString().ToCamelCase();

                        Dictionary<string, PositionStatusClass> thisLight = new()
                        {
                            { lightPositionName, lightStatus }
                        };

                        switch (light.Key)
                        {
                            case LightsCapabilitiesClass.DeviceEnum.CardReader:
                                cardReaderLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.PinPad:
                                pinPadLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.NotesDispenser:
                                notesDispenserLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.CoinDispenser:
                                coinDispenserLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.ReceiptPrinter:
                                receiptPrinterLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.PassbookPrinter:
                                passbookPrinterLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.EnvelopeDepository:
                                envelopeDepositoryLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.CheckUnit:
                                checkUnitLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.BillAcceptor:
                                billAcceptorLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.EnvelopeDispenser:
                                envelopeDispenserLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.DocumentPrinter:
                                documentPrinterLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.CoinAcceptor:
                                coinAcceptorLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.Scanner:
                                scannerLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.Contactless:
                                contactlessLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.CardReader2:
                                cardReader2Light = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.NotesDispenser2:
                                notesDispenser2Light = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.BillAcceptor2:
                                billAcceptor2Light = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.StatusGoodIndicator:
                                statusGoodLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.StatusWarningIndicator:
                                statusWarningLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.StatusBadIndicator:
                                statusBadLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.StatusSupervisorIndicator:
                                statusSupervisorLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.StatusInServiceIndicator:
                                statusInServiceLight = thisLight;
                                break;
                            case LightsCapabilitiesClass.DeviceEnum.FasciaLight:
                                fasciaLight = thisLight;
                                break;
                            default:
                                throw new InternalErrorException($"Unsupported light device is specified by the device class. {light.Key}");
                        }
                    }
                }
                if (Common.LightsStatus?.CustomStatus is not null &&
                    Common.LightsStatus.CustomStatus.Count > 0)
                {
                    foreach (var custom in Common.LightsStatus.CustomStatus)
                    {
                        PositionStatusClass lightStatus = new(
                            FlashRate: custom.Value.FlashRate switch
                            {
                                LightsStatusClass.LightOperation.FlashRateEnum.Continuous => PositionStatusClass.FlashRateEnum.Continuous,
                                LightsStatusClass.LightOperation.FlashRateEnum.Medium => PositionStatusClass.FlashRateEnum.Medium,
                                LightsStatusClass.LightOperation.FlashRateEnum.Off => PositionStatusClass.FlashRateEnum.Off,
                                LightsStatusClass.LightOperation.FlashRateEnum.Quick => PositionStatusClass.FlashRateEnum.Quick,
                                LightsStatusClass.LightOperation.FlashRateEnum.Slow => PositionStatusClass.FlashRateEnum.Slow,
                                _ => null
                            },
                            Color: custom.Value.Colour switch
                            {
                                LightsStatusClass.LightOperation.ColourEnum.Blue => PositionStatusClass.ColorEnum.Blue,
                                LightsStatusClass.LightOperation.ColourEnum.Cyan => PositionStatusClass.ColorEnum.Cyan,
                                LightsStatusClass.LightOperation.ColourEnum.Green => PositionStatusClass.ColorEnum.Green,
                                LightsStatusClass.LightOperation.ColourEnum.Magenta => PositionStatusClass.ColorEnum.Magenta,
                                LightsStatusClass.LightOperation.ColourEnum.Red => PositionStatusClass.ColorEnum.Red,
                                LightsStatusClass.LightOperation.ColourEnum.White => PositionStatusClass.ColorEnum.White,
                                LightsStatusClass.LightOperation.ColourEnum.Yellow => PositionStatusClass.ColorEnum.Yellow,
                                _ => null
                            },
                            Direction: custom.Value.Direction switch
                            {
                                LightsStatusClass.LightOperation.DirectionEnum.Entry => PositionStatusClass.DirectionEnum.Entry,
                                LightsStatusClass.LightOperation.DirectionEnum.Exit => PositionStatusClass.DirectionEnum.Exit,
                                _ => null
                            });

                        string lightPositionName = custom.Value.CustomPosition;
                        if (!string.IsNullOrEmpty(lightPositionName))
                        {
                            Logger.Warning(Constants.Framework, "Custom light position name is not specified for status. Skip this light.");
                            continue;
                        }

                        // converter is not generating extended properties for the custom lights
                        // ADD CUSTOM LIGHTS HERE
                    }
                }

                lights = new XFS4IoT.Lights.StatusClass(
                    CardReader: cardReaderLight,
                    PinPad: pinPadLight,
                    NotesDispenser: notesDispenserLight,
                    CoinDispenser: coinDispenserLight,
                    ReceiptPrinter: receiptPrinterLight,
                    PassbookPrinter: passbookPrinterLight,
                    EnvelopeDepository: envelopeDepositoryLight,
                    CheckUnit: checkUnitLight,
                    BillAcceptor: billAcceptorLight,
                    EnvelopeDispenser: envelopeDispenserLight,
                    DocumentPrinter: documentPrinterLight,
                    CoinAcceptor: coinAcceptorLight,
                    Scanner: scannerLight,
                    Contactless: contactlessLight,
                    CardReader2: cardReader2Light,
                    NotesDispenser2: notesDispenser2Light,
                    BillAcceptor2: billAcceptor2Light,
                    StatusGood: statusGoodLight,
                    StatusWarning: statusWarningLight,
                    StatusBad: statusBadLight,
                    StatusSupervisor: statusSupervisorLight,
                    StatusInService: statusInServiceLight,
                    FasciaLight: fasciaLight);
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
                            _ => null,
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
                            _ => null,
                        };
                    }
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.External))
                    {
                        externalSupplyStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.External].PaperSupply switch
                        {
                            PrinterStatusClass.PaperSupplyEnum.Full => XFS4IoT.Printer.PaperSupplyEnum.Full,
                            PrinterStatusClass.PaperSupplyEnum.Jammed => XFS4IoT.Printer.PaperSupplyEnum.Jammed,
                            PrinterStatusClass.PaperSupplyEnum.Low => XFS4IoT.Printer.PaperSupplyEnum.Low,
                            PrinterStatusClass.PaperSupplyEnum.Out => XFS4IoT.Printer.PaperSupplyEnum.Out,
                            PrinterStatusClass.PaperSupplyEnum.Unknown => XFS4IoT.Printer.PaperSupplyEnum.Unknown,
                            _ => null,
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
                            _ => null,
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
                            _ => null,
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
                            _ => null,
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
                        Dictionary<string, XFS4IoT.Printer.PaperSupplyEnum> customPaperSupplies = null;
                        foreach (var supply in Common.PrinterStatus.CustomPaper)
                        {
                            (customPaperSupplies ??= []).Add(supply.Key, supply.Value.PaperSupply switch
                            {
                                PrinterStatusClass.PaperSupplyEnum.Full => XFS4IoT.Printer.PaperSupplyEnum.Full,
                                PrinterStatusClass.PaperSupplyEnum.Jammed => XFS4IoT.Printer.PaperSupplyEnum.Jammed,
                                PrinterStatusClass.PaperSupplyEnum.Low => XFS4IoT.Printer.PaperSupplyEnum.Low,
                                PrinterStatusClass.PaperSupplyEnum.Out => XFS4IoT.Printer.PaperSupplyEnum.Out,
                                _ => XFS4IoT.Printer.PaperSupplyEnum.Unknown,
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
                    if (Common.PrinterStatus.Paper.ContainsKey(PrinterStatusClass.PaperSourceEnum.External))
                    {
                        externalTypeStatus = Common.PrinterStatus.Paper[PrinterStatusClass.PaperSourceEnum.External].PaperType switch
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
                        Dictionary<string, XFS4IoT.Printer.PaperTypeEnum> customPaperTypes = null;
                        foreach (var supply in Common.PrinterStatus.CustomPaper)
                        {
                            (customPaperTypes ??= []).Add(supply.Key, supply.Value.PaperType switch
                            {
                                PrinterStatusClass.PaperTypeEnum.Dual => XFS4IoT.Printer.PaperTypeEnum.Dual,
                                PrinterStatusClass.PaperTypeEnum.Single => XFS4IoT.Printer.PaperTypeEnum.Single,
                                _ => XFS4IoT.Printer.PaperTypeEnum.Unknown,
                            });
                        }
                        typeStatus.ExtendedProperties = customPaperTypes;
                    }
                }

                printer = new XFS4IoT.Printer.StatusClass(
                    Media: Common.PrinterStatus.Media switch
                    {
                        PrinterStatusClass.MediaEnum.Entering => XFS4IoT.Printer.StatusClass.MediaEnum.Entering,
                        PrinterStatusClass.MediaEnum.Jammed => XFS4IoT.Printer.StatusClass.MediaEnum.Jammed,
                        PrinterStatusClass.MediaEnum.NotPresent => XFS4IoT.Printer.StatusClass.MediaEnum.NotPresent,
                        PrinterStatusClass.MediaEnum.Present => XFS4IoT.Printer.StatusClass.MediaEnum.Present,
                        PrinterStatusClass.MediaEnum.Retracted => XFS4IoT.Printer.StatusClass.MediaEnum.Retracted,
                        PrinterStatusClass.MediaEnum.Unknown => XFS4IoT.Printer.StatusClass.MediaEnum.Unknown,
                        _ => null,
                    },
                    Paper: supplyStatus,
                    Toner: Common.PrinterStatus.Toner switch
                    {
                        PrinterStatusClass.TonerEnum.Full => XFS4IoT.Printer.StatusClass.TonerEnum.Full,
                        PrinterStatusClass.TonerEnum.Low => XFS4IoT.Printer.StatusClass.TonerEnum.Low,
                        PrinterStatusClass.TonerEnum.Out => XFS4IoT.Printer.StatusClass.TonerEnum.Out,
                        PrinterStatusClass.TonerEnum.Unknown => XFS4IoT.Printer.StatusClass.TonerEnum.Unknown,
                        _ => null,
                    },
                    Ink: Common.PrinterStatus.Ink switch
                    {
                        PrinterStatusClass.InkEnum.Full => XFS4IoT.Printer.StatusClass.InkEnum.Full,
                        PrinterStatusClass.InkEnum.Low => XFS4IoT.Printer.StatusClass.InkEnum.Low,
                        PrinterStatusClass.InkEnum.Out => XFS4IoT.Printer.StatusClass.InkEnum.Out,
                        PrinterStatusClass.InkEnum.Unknown => XFS4IoT.Printer.StatusClass.InkEnum.Unknown,
                        _ => null,
                    },
                    Lamp: Common.PrinterStatus.Lamp switch
                    {
                        PrinterStatusClass.LampEnum.Fading => XFS4IoT.Printer.StatusClass.LampEnum.Fading,
                        PrinterStatusClass.LampEnum.Inop => XFS4IoT.Printer.StatusClass.LampEnum.Inop,
                        PrinterStatusClass.LampEnum.Ok => XFS4IoT.Printer.StatusClass.LampEnum.Ok,
                        PrinterStatusClass.LampEnum.Unknown => XFS4IoT.Printer.StatusClass.LampEnum.Unknown,
                        _ => null,
                    },
                    MediaOnStacker: Common.PrinterStatus.MediaOnStacker,
                    PaperType: typeStatus,
                    BlackMarkMode: Common.PrinterStatus.BlackMarkMode switch
                    {
                        PrinterStatusClass.BlackMarkModeEnum.Off => XFS4IoT.Printer.StatusClass.BlackMarkModeEnum.Off,
                        PrinterStatusClass.BlackMarkModeEnum.On => XFS4IoT.Printer.StatusClass.BlackMarkModeEnum.On,
                        PrinterStatusClass.BlackMarkModeEnum.Unknown => XFS4IoT.Printer.StatusClass.BlackMarkModeEnum.Unknown,
                        _ => null,
                    });
            }
			
			XFS4IoT.Auxiliaries.StatusClass auxiliaries = null;
            if (Common.AuxiliariesStatus is not null)
            {
                auxiliaries = new(
                    OperatorSwitch: Common.AuxiliariesStatus.OperatorSwitch switch
                    {
                        AuxiliariesStatusClass.OperatorSwitchEnum.Run => XFS4IoT.Auxiliaries.OperatorSwitchStateEnum.Run,
                        AuxiliariesStatusClass.OperatorSwitchEnum.Maintenance => XFS4IoT.Auxiliaries.OperatorSwitchStateEnum.Maintenance,
                        AuxiliariesStatusClass.OperatorSwitchEnum.Supervisor => XFS4IoT.Auxiliaries.OperatorSwitchStateEnum.Supervisor,
                        _ => null,
                    },
                    TamperSensor: Common.AuxiliariesStatus.TamperSensor switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.TamperSensorStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.TamperSensorStateEnum.Off,
                        _ => null,
                    },
                    InternalTamperSensor: Common.AuxiliariesStatus.InternalTamperSensor switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.InternalTamperSensorStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.InternalTamperSensorStateEnum.Off,
                        _ => null,
                    },
                    SeismicSensor: Common.AuxiliariesStatus.SeismicSensor switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.SeismicSensorStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.SeismicSensorStateEnum.Off,
                        _ => null,
                    },
                    HeatSensor: Common.AuxiliariesStatus.HeatSensor switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.HeatSensorStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.HeatSensorStateEnum.Off,
                        _ => null,
                    },
                    ProximitySensor: Common.AuxiliariesStatus.ProximitySensor switch
                    {
                        AuxiliariesStatusClass.PresenceSensorEnum.Present => XFS4IoT.Auxiliaries.ProximitySensorStateEnum.Present,
                        AuxiliariesStatusClass.PresenceSensorEnum.NotPresent => XFS4IoT.Auxiliaries.ProximitySensorStateEnum.NotPresent,
                        _ => null,
                    },
                    AmbientLightSensor: Common.AuxiliariesStatus.AmbientLightSensor switch
                    {
                        AuxiliariesStatusClass.AmbientLightSensorEnum.VeryDark => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.VeryDark,
                        AuxiliariesStatusClass.AmbientLightSensorEnum.Dark => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.Dark,
                        AuxiliariesStatusClass.AmbientLightSensorEnum.MediumLight => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.MediumLight,
                        AuxiliariesStatusClass.AmbientLightSensorEnum.Light => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.Light,
                        AuxiliariesStatusClass.AmbientLightSensorEnum.VeryLight => XFS4IoT.Auxiliaries.AmbientLightSensorStateEnum.VeryLight,
                        _ => null,
                    },
                    EnhancedAudioSensor: Common.AuxiliariesStatus.EnhancedAudioSensor switch
                    {
                        AuxiliariesStatusClass.PresenceSensorEnum.Present => XFS4IoT.Auxiliaries.EnhancedAudioSensorStateEnum.Present,
                        AuxiliariesStatusClass.PresenceSensorEnum.NotPresent => XFS4IoT.Auxiliaries.EnhancedAudioSensorStateEnum.NotPresent,
                        _ => null,
                    },
                    BootSwitchSensor: Common.AuxiliariesStatus.BootSwitchSensor switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.BootSwitchSensorStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.BootSwitchSensorStateEnum.Off,
                        _ => null,
                    },
                    ConsumerDisplaySensor: Common.AuxiliariesStatus.DisplaySensor switch
                    {
                        AuxiliariesStatusClass.DisplaySensorEnum.Off => XFS4IoT.Auxiliaries.ConsumerDisplaySensorStateEnum.Off,
                        AuxiliariesStatusClass.DisplaySensorEnum.On => XFS4IoT.Auxiliaries.ConsumerDisplaySensorStateEnum.On,
                        AuxiliariesStatusClass.DisplaySensorEnum.DisplayError => XFS4IoT.Auxiliaries.ConsumerDisplaySensorStateEnum.DisplayError,
                        _ => null,
                    },
                    OperatorCallButtonSensor: Common.AuxiliariesStatus.OperatorCallButtonSensor switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.OperatorCallButtonSensorStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.OperatorCallButtonSensorStateEnum.Off,
                        _ => null,
                    },
                    HandsetSensor: Common.AuxiliariesStatus.HandsetSensor switch
                    {
                        AuxiliariesStatusClass.HandsetSensorStatusEnum.OnTheHook => XFS4IoT.Auxiliaries.HandsetSensorStateEnum.OnTheHook,
                        AuxiliariesStatusClass.HandsetSensorStatusEnum.OffTheHook => XFS4IoT.Auxiliaries.HandsetSensorStateEnum.OffTheHook,
                        _ => null,
                    },
                    HeadsetMicrophoneSensor: Common.AuxiliariesStatus.HeadsetMicrophoneSensor switch
                    {
                        AuxiliariesStatusClass.PresenceSensorEnum.Present => XFS4IoT.Auxiliaries.HeadsetMicrophoneSensorStateEnum.Present,
                        AuxiliariesStatusClass.PresenceSensorEnum.NotPresent => XFS4IoT.Auxiliaries.HeadsetMicrophoneSensorStateEnum.NotPresent,
                        _ => null,
                    },
                    FasciaMicrophoneSensor: Common.AuxiliariesStatus.FasciaMicrophoneSensor switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.FasciaMicrophoneSensorStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.FasciaMicrophoneSensorStateEnum.Off,
                        _ => null,
                    },
                    SafeDoor: (Common.AuxiliariesStatus.Doors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.Safe) is true &&
                               Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.Safe].DoorStatus != AuxiliariesStatusClass.DoorStatusEnum.NotAvailable) ?
                        Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.Safe].DoorStatus switch
                        {
                            AuxiliariesStatusClass.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Closed,
                            AuxiliariesStatusClass.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Open,
                            AuxiliariesStatusClass.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Locked,
                            AuxiliariesStatusClass.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Bolted,
                            AuxiliariesStatusClass.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.SafeDoorStateEnum.Tampered,
                            _ => throw new InternalErrorException($"Unexpected safe door status received. {Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.Safe]}"),
                        }
                        : null,
                    VandalShield: Common.AuxiliariesStatus.VandalShield != AuxiliariesStatusClass.VandalShieldStatusEnum.NotAvailable ?
                        Common.AuxiliariesStatus.VandalShield switch
                        {
                            AuxiliariesStatusClass.VandalShieldStatusEnum.Closed => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Closed,
                            AuxiliariesStatusClass.VandalShieldStatusEnum.Open => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Open,
                            AuxiliariesStatusClass.VandalShieldStatusEnum.Locked => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Locked,
                            AuxiliariesStatusClass.VandalShieldStatusEnum.Service => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Service,
                            AuxiliariesStatusClass.VandalShieldStatusEnum.Keyboard => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Keyboard,
                            AuxiliariesStatusClass.VandalShieldStatusEnum.PartiallyOpen => XFS4IoT.Auxiliaries.VandalShieldStateEnum.PartiallyOpen,
                            AuxiliariesStatusClass.VandalShieldStatusEnum.Jammed => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Jammed,
                            AuxiliariesStatusClass.VandalShieldStatusEnum.Tampered => XFS4IoT.Auxiliaries.VandalShieldStateEnum.Tampered,
                            _ => throw new InternalErrorException($"Unexpected vandal shield status received. {Common.AuxiliariesStatus.VandalShield}"),
                        }
                        : null,
                    CabinetFrontDoor: (Common.AuxiliariesStatus.Doors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.FrontCabinet) is true &&
                                       Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.FrontCabinet].DoorStatus != AuxiliariesStatusClass.DoorStatusEnum.NotAvailable) ?
                        Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.FrontCabinet].DoorStatus switch
                        {
                            AuxiliariesStatusClass.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Closed,
                            AuxiliariesStatusClass.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Open,
                            AuxiliariesStatusClass.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Locked,
                            AuxiliariesStatusClass.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Bolted,
                            AuxiliariesStatusClass.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetFrontDoorStateEnum.Tampered,
                            _ => throw new InternalErrorException($"Unexpected front cabinet door status received. {Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.FrontCabinet]}"),
                        }
                        : null,
                    CabinetRearDoor: (Common.AuxiliariesStatus.Doors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.RearCabinet) is true &&
                                      Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.RearCabinet].DoorStatus != AuxiliariesStatusClass.DoorStatusEnum.NotAvailable) ?
                        Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.RearCabinet].DoorStatus switch
                        {
                            AuxiliariesStatusClass.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Closed,
                            AuxiliariesStatusClass.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Open,
                            AuxiliariesStatusClass.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Locked,
                            AuxiliariesStatusClass.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Bolted,
                            AuxiliariesStatusClass.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetRearDoorStateEnum.Tampered,
                            _ => throw new InternalErrorException($"Unexpected rear cabinet door status received. {Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.RearCabinet]}"),
                        }
                        : null,
                    CabinetLeftDoor: (Common.AuxiliariesStatus.Doors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.LeftCabinet) is true &&
                                      Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.LeftCabinet].DoorStatus != AuxiliariesStatusClass.DoorStatusEnum.NotAvailable) ?
                        Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.LeftCabinet].DoorStatus switch
                        {
                            AuxiliariesStatusClass.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Closed,
                            AuxiliariesStatusClass.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Open,
                            AuxiliariesStatusClass.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Locked,
                            AuxiliariesStatusClass.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Bolted,
                            AuxiliariesStatusClass.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetLeftDoorStateEnum.Tampered,
                            _ => throw new InternalErrorException($"Unexpected left cabinet door status received. {Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.LeftCabinet]}"),
                        }
                        : null,
                    CabinetRightDoor: (Common.AuxiliariesStatus.Doors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.RightCabinet) is true &&
                                       Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.RightCabinet].DoorStatus != AuxiliariesStatusClass.DoorStatusEnum.NotAvailable) ?
                        Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.RightCabinet].DoorStatus switch
                        {
                            AuxiliariesStatusClass.DoorStatusEnum.Closed => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Closed,
                            AuxiliariesStatusClass.DoorStatusEnum.Open => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Open,
                            AuxiliariesStatusClass.DoorStatusEnum.Locked => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Locked,
                            AuxiliariesStatusClass.DoorStatusEnum.Bolted => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Bolted,
                            AuxiliariesStatusClass.DoorStatusEnum.Tampered => XFS4IoT.Auxiliaries.CabinetRightDoorStateEnum.Tampered,
                            _ => throw new InternalErrorException($"Unexpected right cabinet door status received. {Common.AuxiliariesStatus.Doors[AuxiliariesCapabilitiesClass.DoorType.RightCabinet]}"),
                        }
                        : null,
                    OpenClosedIndicator: Common.AuxiliariesStatus.OpenClosedIndicator switch
                    {
                        AuxiliariesStatusClass.OpenClosedIndicatorEnum.Closed => XFS4IoT.Auxiliaries.OpenClosedIndicatorStateEnum.Closed,
                        AuxiliariesStatusClass.OpenClosedIndicatorEnum.Open => XFS4IoT.Auxiliaries.OpenClosedIndicatorStateEnum.Open,
                        _ => null,
                    }, 
                    Audio: (Common.AuxiliariesStatus.AudioRate == AuxiliariesStatusClass.AudioRateEnum.NotAvailable && 
                            Common.AuxiliariesStatus.AudioSignal == AuxiliariesStatusClass.AudioSignalEnum.NotAvailable) ?
                        null :
                        new(
                        Rate: Common.AuxiliariesStatus.AudioRate switch
                        {
                            AuxiliariesStatusClass.AudioRateEnum.On => XFS4IoT.Auxiliaries.AudioStateClass.RateEnum.On,
                            AuxiliariesStatusClass.AudioRateEnum.Off => XFS4IoT.Auxiliaries.AudioStateClass.RateEnum.Off,
                            _ => null
                        },
                        Signal: Common.AuxiliariesStatus.AudioSignal switch
                        {
                            AuxiliariesStatusClass.AudioSignalEnum.Exclamation => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Exclamation,
                            AuxiliariesStatusClass.AudioSignalEnum.Warning => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Warning,
                            AuxiliariesStatusClass.AudioSignalEnum.Error => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Error,
                            AuxiliariesStatusClass.AudioSignalEnum.Critical => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Critical,
                            AuxiliariesStatusClass.AudioSignalEnum.Keypress => XFS4IoT.Auxiliaries.AudioStateClass.SignalEnum.Keypress,
                            _ => null,
                        }),
                    Heating: Common.AuxiliariesStatus.Heating switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.HeatingStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.HeatingStateEnum.Off,
                        _ => null,
                    },
                    ConsumerDisplayBacklight: Common.AuxiliariesStatus.ConsumerDisplayBacklight switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.ConsumerDisplayBacklightStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.ConsumerDisplayBacklightStateEnum.Off,
                        _ => null,
                    },
                    SignageDisplay: Common.AuxiliariesStatus.SignageDisplay switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.SignageDisplayStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.SignageDisplayStateEnum.Off,
                        _ => null,
                    },
                    Volume: Common.AuxiliariesStatus.Volume > 0 ?
                            Common.AuxiliariesStatus.Volume :
                            null,
                    UPS: Common.AuxiliariesStatus.UPS == AuxiliariesStatusClass.UpsStatusEnum.NotAvailable ?
                         null :
                         new XFS4IoT.Auxiliaries.UPSStateClass(
                             !Common.AuxiliariesStatus.UPS.HasFlag(AuxiliariesStatusClass.UpsStatusEnum.Good) && Common.AuxiliariesStatus.UPS.HasFlag(AuxiliariesStatusClass.UpsStatusEnum.Low),
                             Common.AuxiliariesStatus.UPS.HasFlag(AuxiliariesStatusClass.UpsStatusEnum.Engaged),
                             Common.AuxiliariesStatus.UPS.HasFlag(AuxiliariesStatusClass.UpsStatusEnum.Powering),
                             Common.AuxiliariesStatus.UPS.HasFlag(AuxiliariesStatusClass.UpsStatusEnum.Recovered)
                    ),
                    AudibleAlarm: Common.AuxiliariesStatus.AudibleAlarm switch
                    {
                        AuxiliariesStatusClass.SensorEnum.On => XFS4IoT.Auxiliaries.AudibleAlarmStateEnum.On,
                        AuxiliariesStatusClass.SensorEnum.Off => XFS4IoT.Auxiliaries.AudibleAlarmStateEnum.Off,
                        _ => null,
                    },
                    EnhancedAudioControl: Common.AuxiliariesStatus.EnhancedAudioControl switch
                    {
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioManual => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PublicAudioManual,
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PublicAudioAuto,
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PublicAudioSemiAuto,
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioManual => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PrivateAudioManual,
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PrivateAudioAuto,
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedAudioControlStateEnum.PrivateAudioSemiAuto,
                        _ => null,
                    },
                    EnhancedMicrophoneControl: Common.AuxiliariesStatus.EnhancedMicrophoneControl switch
                    {
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioManual => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PublicAudioManual,
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PublicAudioAuto,
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PublicAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PublicAudioSemiAuto,
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioManual => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PrivateAudioManual,
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PrivateAudioAuto,
                        AuxiliariesStatusClass.EnhancedAudioControlEnum.PrivateAudioSemiAuto => XFS4IoT.Auxiliaries.EnhancedMicrophoneControlStateEnum.PrivateAudioSemiAuto,
                        _ => null,
                    },
                    MicrophoneVolume: Common.AuxiliariesStatus.MicrophoneVolume > 0 ?
                    Common.AuxiliariesStatus.MicrophoneVolume :
                    null
                );
            }

            XFS4IoT.VendorApplication.StatusClass vendorApplication = null;
            if (Common.VendorApplicationStatus is not null)
            {
                vendorApplication = new XFS4IoT.VendorApplication.StatusClass(
                    Common.VendorApplicationStatus.AccessLevel switch
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
                vendorMode = new XFS4IoT.VendorMode.StatusClass(
                    Common.VendorModeStatus.DeviceStatus switch
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
                biometric = Common.BiometricStatus.Subject == BiometricStatusClass.SubjectStatusEnum.NotSupported ?
                null : new(
                    Subject: Common.BiometricStatus.Subject switch
                    {
                        BiometricStatusClass.SubjectStatusEnum.NotPresent => XFS4IoT.Biometric.StatusClass.SubjectEnum.NotPresent,
                        BiometricStatusClass.SubjectStatusEnum.Present => XFS4IoT.Biometric.StatusClass.SubjectEnum.Present,
                        BiometricStatusClass.SubjectStatusEnum.Unknown => XFS4IoT.Biometric.StatusClass.SubjectEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected subject status received. {Common.BiometricStatus.Subject}"),
                    },
                    Capture: Common.BiometricStatus.Capture,
                    DataPersistence: Common.BiometricStatus.DataPersistence == BiometricCapabilitiesClass.PersistenceModesEnum.None ?
                    null : Common.BiometricStatus.DataPersistence switch
                    {
                        BiometricCapabilitiesClass.PersistenceModesEnum.Persist => XFS4IoT.Biometric.StatusClass.DataPersistenceEnum.Persist,
                        BiometricCapabilitiesClass.PersistenceModesEnum.Clear => XFS4IoT.Biometric.StatusClass.DataPersistenceEnum.Clear,
                        _ => throw Contracts.Fail<NotImplementedException>($"Unexpected value for Common.BiometricStatus.DataPersistence. {Common.BiometricStatus.DataPersistence}")
                    },
                    RemainingStorage: Common.BiometricStatus.RemainingStorage);
            }

            XFS4IoT.CashAcceptor.StatusClass cashAcceptor = null;
            if (Common.CashAcceptorStatus is not null)
            {
                List<XFS4IoT.CashAcceptor.PositionClass> positions = null;
                if (Common.CashAcceptorStatus.Positions is not null &&
                    Common.CashAcceptorStatus.Positions.Count > 0)
                {
                    foreach (var position in Common.CashAcceptorStatus.Positions)
                    {
                        (positions ??= []).Add(new XFS4IoT.CashAcceptor.PositionClass(
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
                                _ => throw new InternalErrorException($"Unexpected position specified for the status. {position.Key}"),
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
                                _ => null,
                            },
                            PositionStatus: position.Value.PositionStatus switch
                            {
                                CashManagementStatusClass.PositionStatusEnum.Empty => XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum.Empty,
                                CashManagementStatusClass.PositionStatusEnum.NotEmpty => XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum.NotEmpty,
                                CashManagementStatusClass.PositionStatusEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.PositionStatusEnum.Unknown,
                                _ => null,
                            },
                            Transport: position.Value.Transport switch
                            {
                                CashManagementStatusClass.TransportEnum.Inoperative => XFS4IoT.CashAcceptor.PositionClass.TransportEnum.Inoperative,
                                CashManagementStatusClass.TransportEnum.Ok => XFS4IoT.CashAcceptor.PositionClass.TransportEnum.Ok,
                                CashManagementStatusClass.TransportEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.TransportEnum.Unknown,
                                _ => null,
                            },
                            TransportStatus: position.Value.TransportStatus switch
                            {
                                CashManagementStatusClass.TransportStatusEnum.Empty => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.Empty,
                                CashManagementStatusClass.TransportStatusEnum.NotEmpty => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.NotEmpty,
                                CashManagementStatusClass.TransportStatusEnum.NotEmptyCustomer => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.NotEmptyCustomer,
                                CashManagementStatusClass.TransportStatusEnum.Unknown => XFS4IoT.CashAcceptor.PositionClass.TransportStatusEnum.Unknown,
                                _ => null,
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
                        _ => null,
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
                        _ => null,
                    },
                    DropBox: Common.CashAcceptorStatus.DropBox,
                    Positions: positions);
            }

            XFS4IoT.Camera.StatusClass camera = null;
            if (Common.CameraStatus is not null)
            {
                XFS4IoT.Camera.StatusClass.MediaClass mediaStatusLocation = null;
                XFS4IoT.Camera.StatusClass.CamerasClass camStatusLocation = null;
                XFS4IoT.Camera.StatusClass.PicturesClass numPicsLocation = null;

                XFS4IoT.Camera.MediaStateEnum MediaStateMapping(XFS4IoTFramework.Common.CameraStatusClass.CameraLocationStatusClass camera)
                    => camera.MediaState switch
                    {
                        CameraStatusClass.CameraLocationStatusClass.MediaStateEnum.Ok => XFS4IoT.Camera.MediaStateEnum.Ok,
                        CameraStatusClass.CameraLocationStatusClass.MediaStateEnum.High => XFS4IoT.Camera.MediaStateEnum.High,
                        CameraStatusClass.CameraLocationStatusClass.MediaStateEnum.Full => XFS4IoT.Camera.MediaStateEnum.Full,
                        _ => XFS4IoT.Camera.MediaStateEnum.Unknown,
                    };
                XFS4IoT.Camera.CamerasStateEnum CameraStateMapping(XFS4IoTFramework.Common.CameraStatusClass.CameraLocationStatusClass camera)
                    => camera.CamerasState switch
                    {
                        CameraStatusClass.CameraLocationStatusClass.CamerasStateEnum.Ok => XFS4IoT.Camera.CamerasStateEnum.Ok,
                        CameraStatusClass.CameraLocationStatusClass.CamerasStateEnum.Inoperable => XFS4IoT.Camera.CamerasStateEnum.Inop,
                        _ => XFS4IoT.Camera.CamerasStateEnum.Unknown,
                    };

                if (Common.CameraStatus.CameraLocationStatus is not null && Common.CameraStatus.CameraLocationStatus.Count > 0)
                {
                    mediaStatusLocation = new XFS4IoT.Camera.StatusClass.MediaClass(
                        Room: Common.CameraStatus.CameraLocationStatus.ContainsKey(CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room)
                            ? MediaStateMapping(Common.CameraStatus.CameraLocationStatus[CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room])
                            : null,
                        Person:
                            Common.CameraStatus.CameraLocationStatus.ContainsKey(CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person)
                            ? MediaStateMapping(Common.CameraStatus.CameraLocationStatus[CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person])
                            : null,
                        ExitSlot:
                            Common.CameraStatus.CameraLocationStatus.ContainsKey(CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot)
                            ? MediaStateMapping(Common.CameraStatus.CameraLocationStatus[CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot])
                            : null
                        );

                    camStatusLocation = new XFS4IoT.Camera.StatusClass.CamerasClass(
                        Room:
                            Common.CameraStatus.CameraLocationStatus.ContainsKey(CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room)
                            ? CameraStateMapping(Common.CameraStatus.CameraLocationStatus[CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room])
                            : null,
                        Person:
                            Common.CameraStatus.CameraLocationStatus.ContainsKey(CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person)
                            ? CameraStateMapping(Common.CameraStatus.CameraLocationStatus[CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person])
                            : null,
                        ExitSlot:
                            Common.CameraStatus.CameraLocationStatus.ContainsKey(CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot)
                            ? CameraStateMapping(Common.CameraStatus.CameraLocationStatus[CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot])
                            : null
                        );

                    numPicsLocation = new XFS4IoT.Camera.StatusClass.PicturesClass(
                        Room:
                            Common.CameraStatus.CameraLocationStatus.ContainsKey(CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room)
                            ? Common.CameraStatus.CameraLocationStatus[CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Room].NumberOfPictures
                            : null,
                        Person:
                            Common.CameraStatus.CameraLocationStatus.ContainsKey(CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person)
                            ? Common.CameraStatus.CameraLocationStatus[CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.Person].NumberOfPictures
                            : null,
                        ExitSlot:
                            Common.CameraStatus.CameraLocationStatus.ContainsKey(CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot)
                            ? Common.CameraStatus.CameraLocationStatus[CameraStatusClass.CameraLocationStatusClass.CameraLocationEnum.ExitSlot].NumberOfPictures
                            : null
                        );
                }

                if (Common.CameraStatus.CustomCameraLocationStatus is not null)
                {
                    foreach (var cam in Common.CameraStatus.CustomCameraLocationStatus)
                    {
                        XFS4IoT.Camera.MediaStateEnum mediaStatus = MediaStateMapping(cam.Value);

                        mediaStatusLocation ??= new();
                        mediaStatusLocation.ExtendedProperties.Add(cam.Key, mediaStatus);

                        XFS4IoT.Camera.CamerasStateEnum camStatus = CameraStateMapping(cam.Value);

                        camStatusLocation ??= new();
                        camStatusLocation.ExtendedProperties.Add(cam.Key, camStatus);

                        int? numPictures = cam.Value.NumberOfPictures;
                        if (numPictures is not null)
                        {
                            numPicsLocation ??= new();
                            numPicsLocation.ExtendedProperties.Add(cam.Key, (int)numPictures);
                        }
                    }
                }

                camera = new(
                    Media: mediaStatusLocation,
                    Cameras: camStatusLocation,
                    Pictures: numPicsLocation);
            }

            XFS4IoT.Check.StatusClass checkScanner = null;
            if (Common.CheckScannerStatus is not null)
            {
                XFS4IoT.Check.PositionStatusClass input = null;
                XFS4IoT.Check.PositionStatusClass output = null;
                XFS4IoT.Check.PositionStatusClass refused = null;
                if (Common.CheckScannerStatus.Positions is not null)
                {
                    foreach (var positionStat in Common.CheckScannerStatus.Positions)
                    {
                        XFS4IoT.Check.PositionStatusClass stat = new(
                            Shutter: positionStat.Value.Shutter == CheckScannerStatusClass.ShutterEnum.NotSupported ?
                            null : positionStat.Value.Shutter switch
                            {
                                CheckScannerStatusClass.ShutterEnum.Open => XFS4IoT.Check.ShutterStateEnum.Open,
                                CheckScannerStatusClass.ShutterEnum.Closed => XFS4IoT.Check.ShutterStateEnum.Closed,
                                CheckScannerStatusClass.ShutterEnum.Jammed => XFS4IoT.Check.ShutterStateEnum.Jammed,
                                CheckScannerStatusClass.ShutterEnum.Unknown => XFS4IoT.Check.ShutterStateEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected shutter status specified. {positionStat.Value.Shutter}")
                            },
                            PositionStatus: positionStat.Value.PositionStatus == CheckScannerStatusClass.PositionStatusEnum.NotSupported ?
                            null : positionStat.Value.PositionStatus switch
                            {
                                CheckScannerStatusClass.PositionStatusEnum.Empty => XFS4IoT.Check.PositionStatusClass.PositionStatusEnum.Empty,
                                CheckScannerStatusClass.PositionStatusEnum.NotEmpty => XFS4IoT.Check.PositionStatusClass.PositionStatusEnum.NotEmpty,
                                CheckScannerStatusClass.PositionStatusEnum.Unknown => XFS4IoT.Check.PositionStatusClass.PositionStatusEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected position status specified. {positionStat.Value.PositionStatus}")
                            },
                            Transport: positionStat.Value.Transport == CheckScannerStatusClass.TransportEnum.NotSupported ?
                            null : positionStat.Value.Transport switch
                            {
                                CheckScannerStatusClass.TransportEnum.Ok => XFS4IoT.Check.PositionStatusClass.TransportEnum.Ok,
                                CheckScannerStatusClass.TransportEnum.Inoperative => XFS4IoT.Check.PositionStatusClass.TransportEnum.Inoperative,
                                CheckScannerStatusClass.TransportEnum.Unknown => XFS4IoT.Check.PositionStatusClass.TransportEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected transport status specified. {positionStat.Value.Transport}")
                            },
                            TransportMediaStatus: positionStat.Value.TransportMediaStatus == CheckScannerStatusClass.TransportMediaStatusEnum.NotSupported ?
                            null : positionStat.Value.TransportMediaStatus switch
                            {
                                CheckScannerStatusClass.TransportMediaStatusEnum.Empty => XFS4IoT.Check.PositionStatusClass.TransportMediaStatusEnum.Empty,
                                CheckScannerStatusClass.TransportMediaStatusEnum.NotEmpty => XFS4IoT.Check.PositionStatusClass.TransportMediaStatusEnum.NotEmpty,
                                CheckScannerStatusClass.TransportMediaStatusEnum.Unknown => XFS4IoT.Check.PositionStatusClass.TransportMediaStatusEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected transport media status specified. {positionStat.Value.TransportMediaStatus}")
                            },
                            JammedShutterPosition: positionStat.Value.JammedShutterPosition == CheckScannerStatusClass.JammedShutterPositionEnum.NotSupported ?
                            null : positionStat.Value.JammedShutterPosition switch
                            {
                                CheckScannerStatusClass.JammedShutterPositionEnum.Open => XFS4IoT.Check.PositionStatusClass.JammedShutterPositionEnum.Open,
                                CheckScannerStatusClass.JammedShutterPositionEnum.Closed => XFS4IoT.Check.PositionStatusClass.JammedShutterPositionEnum.Closed,
                                CheckScannerStatusClass.JammedShutterPositionEnum.PartiallyOpen => XFS4IoT.Check.PositionStatusClass.JammedShutterPositionEnum.PartiallyOpen,
                                CheckScannerStatusClass.JammedShutterPositionEnum.NotJammed => XFS4IoT.Check.PositionStatusClass.JammedShutterPositionEnum.NotJammed,
                                CheckScannerStatusClass.JammedShutterPositionEnum.Unknown => XFS4IoT.Check.PositionStatusClass.JammedShutterPositionEnum.Unknown,
                                _ => throw new InternalErrorException($"Unexpected jammed shutter position status specified. {positionStat.Value.JammedShutterPosition}")
                            });

                        switch (positionStat.Key)
                        {
                            case CheckScannerCapabilitiesClass.PositionEnum.Input:
                                input = stat;
                                break;
                            case CheckScannerCapabilitiesClass.PositionEnum.Output:
                                output = stat;
                                break;
                            case CheckScannerCapabilitiesClass.PositionEnum.Refused:
                                refused = stat;
                                break;
                            default:
                                throw new InternalErrorException($"Unexpected position specified. {positionStat.Key}");
                        }
                    }
                }

                checkScanner = new(
                    Acceptor: Common.CheckScannerStatus.Acceptor switch
                    { 
                        CheckScannerStatusClass.AcceptorEnum.Ok => XFS4IoT.Check.StatusClass.AcceptorEnum.Ok,
                        CheckScannerStatusClass.AcceptorEnum.Attention => XFS4IoT.Check.StatusClass.AcceptorEnum.State,
                        CheckScannerStatusClass.AcceptorEnum.Stop => XFS4IoT.Check.StatusClass.AcceptorEnum.Stop,
                        CheckScannerStatusClass.AcceptorEnum.Unknown => XFS4IoT.Check.StatusClass.AcceptorEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected acceptor status specified. {Common.CheckScannerStatus.Acceptor}")
                    },
                    Media: Common.CheckScannerStatus.Media switch
                    { 
                        CheckScannerStatusClass.MediaEnum.Present => XFS4IoT.Check.StatusClass.MediaEnum.Present,
                        CheckScannerStatusClass.MediaEnum.NotPresent => XFS4IoT.Check.StatusClass.MediaEnum.NotPresent,
                        CheckScannerStatusClass.MediaEnum.Position => XFS4IoT.Check.StatusClass.MediaEnum.Position,
                        CheckScannerStatusClass.MediaEnum.Jammed => XFS4IoT.Check.StatusClass.MediaEnum.Jammed,
                        CheckScannerStatusClass.MediaEnum.Unknown => XFS4IoT.Check.StatusClass.MediaEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected media status specified. {Common.CheckScannerStatus.Media}")
                    },
                    Toner: Common.CheckScannerStatus.Toner == CheckScannerStatusClass.TonerEnum.NotSupported ?
                    null : Common.CheckScannerStatus.Toner switch
                    { 
                        CheckScannerStatusClass.TonerEnum.Out => XFS4IoT.Check.TonerEnum.Out,
                        CheckScannerStatusClass.TonerEnum.Full => XFS4IoT.Check.TonerEnum.Full,
                        CheckScannerStatusClass.TonerEnum.Low => XFS4IoT.Check.TonerEnum.Low,
                        CheckScannerStatusClass.TonerEnum.Unknown => XFS4IoT.Check.TonerEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected acceptor status specified. {Common.CheckScannerStatus.Toner}")
                    },
                    Ink: Common.CheckScannerStatus.Ink == CheckScannerStatusClass.InkEnum.NotSupported ?
                    null : Common.CheckScannerStatus.Ink switch
                    { 
                        CheckScannerStatusClass.InkEnum.Out => XFS4IoT.Check.InkEnum.Out,
                        CheckScannerStatusClass.InkEnum.Full => XFS4IoT.Check.InkEnum.Full,
                        CheckScannerStatusClass.InkEnum.Low => XFS4IoT.Check.InkEnum.Low,
                        CheckScannerStatusClass.InkEnum.Unknown => XFS4IoT.Check.InkEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected ink status specified. {Common.CheckScannerStatus.Ink}")
                    },
                    FrontImageScanner: Common.CheckScannerStatus.FrontImageScanner == CheckScannerStatusClass.ImageScannerEnum.NotSupported ?
                    null : Common.CheckScannerStatus.FrontImageScanner switch
                    { 
                        CheckScannerStatusClass.ImageScannerEnum.Ok => XFS4IoT.Check.FrontImageScannerEnum.Ok,
                        CheckScannerStatusClass.ImageScannerEnum.Fading => XFS4IoT.Check.FrontImageScannerEnum.Fading,
                        CheckScannerStatusClass.ImageScannerEnum.Inoperative => XFS4IoT.Check.FrontImageScannerEnum.Inoperative,
                        CheckScannerStatusClass.ImageScannerEnum.Unknown => XFS4IoT.Check.FrontImageScannerEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected front image scanner specified. {Common.CheckScannerStatus.FrontImageScanner}")
                    },
                    BackImageScanner: Common.CheckScannerStatus.BackImageScanner == CheckScannerStatusClass.ImageScannerEnum.NotSupported ?
                    null : Common.CheckScannerStatus.BackImageScanner switch
                    {
                        CheckScannerStatusClass.ImageScannerEnum.Ok => XFS4IoT.Check.BackImageScannerEnum.Ok,
                        CheckScannerStatusClass.ImageScannerEnum.Fading => XFS4IoT.Check.BackImageScannerEnum.Fading,
                        CheckScannerStatusClass.ImageScannerEnum.Inoperative => XFS4IoT.Check.BackImageScannerEnum.Inoperative,
                        CheckScannerStatusClass.ImageScannerEnum.Unknown => XFS4IoT.Check.BackImageScannerEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected back image scanner specified. {Common.CheckScannerStatus.BackImageScanner}")
                    },
                    MICRReader: Common.CheckScannerStatus.MICRReader == CheckScannerStatusClass.ImageScannerEnum.NotSupported ?
                    null : Common.CheckScannerStatus.MICRReader switch
                    {
                        CheckScannerStatusClass.ImageScannerEnum.Ok => XFS4IoT.Check.MicrReaderEnum.Ok,
                        CheckScannerStatusClass.ImageScannerEnum.Fading => XFS4IoT.Check.MicrReaderEnum.Fading,
                        CheckScannerStatusClass.ImageScannerEnum.Inoperative => XFS4IoT.Check.MicrReaderEnum.Inoperative,
                        CheckScannerStatusClass.ImageScannerEnum.Unknown => XFS4IoT.Check.MicrReaderEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected MICR reader specified. {Common.CheckScannerStatus.MICRReader}")
                    },
                    Stacker: Common.CheckScannerStatus.Stacker == CheckScannerStatusClass.StackerEnum.NotSupported ?
                    null : Common.CheckScannerStatus.Stacker switch
                    { 
                        CheckScannerStatusClass.StackerEnum.Empty => XFS4IoT.Check.StatusClass.StackerEnum.Empty,
                        CheckScannerStatusClass.StackerEnum.NotEmpty => XFS4IoT.Check.StatusClass.StackerEnum.NotEmpty,
                        CheckScannerStatusClass.StackerEnum.Full => XFS4IoT.Check.StatusClass.StackerEnum.Full,
                        CheckScannerStatusClass.StackerEnum.Inoperative => XFS4IoT.Check.StatusClass.StackerEnum.Inoperative,
                        CheckScannerStatusClass.StackerEnum.Unknown => XFS4IoT.Check.StatusClass.StackerEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected stacker status specified. {Common.CheckScannerStatus.Stacker}")
                    },
                    Rebuncher: Common.CheckScannerStatus.ReBuncher == CheckScannerStatusClass.ReBuncherEnum.NotSupported ?
                    null : Common.CheckScannerStatus.ReBuncher switch
                    { 
                        CheckScannerStatusClass.ReBuncherEnum.Empty => XFS4IoT.Check.StatusClass.RebuncherEnum.Empty,
                        CheckScannerStatusClass.ReBuncherEnum.NotEmpty => XFS4IoT.Check.StatusClass.RebuncherEnum.NotEmpty,
                        CheckScannerStatusClass.ReBuncherEnum.Full => XFS4IoT.Check.StatusClass.RebuncherEnum.Full,
                        CheckScannerStatusClass.ReBuncherEnum.Inoperative => XFS4IoT.Check.StatusClass.RebuncherEnum.Inoperative,
                        CheckScannerStatusClass.ReBuncherEnum.Unknown => XFS4IoT.Check.StatusClass.RebuncherEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected rebuncher status specified. {Common.CheckScannerStatus.ReBuncher}")
                    },
                    MediaFeeder: Common.CheckScannerStatus.MediaFeeder == CheckScannerStatusClass.MediaFeederEnum.NotSupported ?
                    null : Common.CheckScannerStatus.MediaFeeder switch
                    { 
                        CheckScannerStatusClass.MediaFeederEnum.Empty => XFS4IoT.Check.MediaFeederEnum.Empty,
                        CheckScannerStatusClass.MediaFeederEnum.NotEmpty => XFS4IoT.Check.MediaFeederEnum.NotEmpty,
                        CheckScannerStatusClass.MediaFeederEnum.Inoperative => XFS4IoT.Check.MediaFeederEnum.Inoperative,
                        CheckScannerStatusClass.MediaFeederEnum.Unknown => XFS4IoT.Check.MediaFeederEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected media feeder status specified. {Common.CheckScannerStatus.MediaFeeder}")
                    },
                    Positions: input is null && output is null && refused is null ?
                    null : new XFS4IoT.Check.StatusClass.PositionsClass(
                        Input: input,
                        Output: output,
                        Refused: refused));
            }

            XFS4IoT.MixedMedia.StatusClass mixedMedia = null;
            if (Common.MixedMediaStatus is not null)
            {
                mixedMedia = new XFS4IoT.MixedMedia.StatusClass(
                    Modes: Common.MixedMediaStatus.CurrentModes == MixedMedia.ModeTypeEnum.None ?
                        null :
                        new XFS4IoT.MixedMedia.ModesClass(
                            CashAccept: Common.MixedMediaStatus.CurrentModes.HasFlag(MixedMedia.ModeTypeEnum.Cash),
                            CheckAccept: Common.MixedMediaStatus.CurrentModes.HasFlag(MixedMedia.ModeTypeEnum.Check)
                            )
                        );
            }

            XFS4IoT.PowerManagement.StatusClass powerManagement = null;
            if (Common.PowerManagementStatus is not null)
            {
                powerManagement = new XFS4IoT.PowerManagement.StatusClass(
                    Info: new(
                        PowerInStatus: Common.PowerManagementStatus.PowerInfo.PowerInStatus switch
                        {
                            PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.Powering => XFS4IoT.PowerManagement.PowerInfoClass.PowerInStatusEnum.Powering,
                            PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.NotPower => XFS4IoT.PowerManagement.PowerInfoClass.PowerInStatusEnum.NoPower,
                            _ => throw new InternalErrorException($"Unexpected power-in status specified. {Common.PowerManagementStatus.PowerInfo.PowerInStatus}")
                        },
                        PowerOutStatus: Common.PowerManagementStatus.PowerInfo.PowerOutStatus switch
                        {
                            PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.Powering => XFS4IoT.PowerManagement.PowerInfoClass.PowerOutStatusEnum.Powering,
                            PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.NotPower => XFS4IoT.PowerManagement.PowerInfoClass.PowerOutStatusEnum.NoPower,
                            _ => throw new InternalErrorException($"Unexpected power-out status specified. {Common.PowerManagementStatus.PowerInfo.PowerOutStatus}")
                        },
                        BatteryStatus: Common.PowerManagementStatus.PowerInfo.BatteryStatus switch
                        {
                            PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Low => XFS4IoT.PowerManagement.BatteryStatusEnum.Low,
                            PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Failure => XFS4IoT.PowerManagement.BatteryStatusEnum.Failure,
                            PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Operational => XFS4IoT.PowerManagement.BatteryStatusEnum.Operational,
                            PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Full => XFS4IoT.PowerManagement.BatteryStatusEnum.Full,
                            PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Critical => XFS4IoT.PowerManagement.BatteryStatusEnum.Critical,
                            _ => null
                        },
                        BatteryChargingStatus: Common.PowerManagementStatus.PowerInfo.BatteryChargingStatus switch
                        {
                            PowerManagementStatusClass.PowerInfoClass.BatteryChargingStatusEnum.Charging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.Charging,
                            PowerManagementStatusClass.PowerInfoClass.BatteryChargingStatusEnum.NotCharging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.NotCharging,
                            PowerManagementStatusClass.PowerInfoClass.BatteryChargingStatusEnum.Discharging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.Discharging,
                            _ => null
                        }),
                    PowerSaveRecoveryTime: Common.PowerManagementStatus.PowerSaveRecoveryTime >= 0 ? Common.PowerManagementStatus.PowerSaveRecoveryTime : null
                    );
            }

            XFS4IoT.BanknoteNeutralization.StatusClass ibns = null;
            if (Common.IBNSStatus is not null)
            {
                Dictionary<string, XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass> customInputs = null;
                if (Common.IBNSStatus.CustomInputStatus is not null &&
                    Common.IBNSStatus.CustomInputStatus.Count > 0)
                {
                    customInputs = [];
                    foreach (var customInput in Common.IBNSStatus.CustomInputStatus)
                    {
                        customInputs.Add(
                            customInput.Key.ToString().ToCamelCase(),
                            new(customInput.Value.InputState switch
                            {
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Healthy => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Ok,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Fault => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Fault,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Disabled,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Triggered => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Triggered,
                                _ => throw new InternalErrorException($"Unexpected input state specified. {customInput.Key} {customInput.Value.InputState}")
                            }));
                    }
                }
                if (Common.IBNSStatus.VendorSpecificCustomInputStatus is not null &&
                    Common.IBNSStatus.VendorSpecificCustomInputStatus.Count > 0)
                {
                    foreach (var customInput in Common.IBNSStatus.VendorSpecificCustomInputStatus)
                    {
                        (customInputs ??= []).Add(
                            customInput.Key.ToCamelCase(),
                            new(customInput.Value.InputState switch
                            {
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Healthy => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Ok,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Fault => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Fault,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Disabled,
                                IBNSStatusClass.CustomInputStatusClass.InputStateEnum.Triggered => XFS4IoT.BanknoteNeutralization.StatusClass.CustomInputsClass.InputStateEnum.Triggered,
                                _ => throw new InternalErrorException($"Unexpected input state specified.{customInput.Key} {customInput.Value.InputState}")
                            }));
                    }
                }

                XFS4IoT.BanknoteNeutralization.WarningsClass warnings = Common.IBNSStatus.WarningState is null ? null :
                    new(
                        ProtectionArmingFault: Common.IBNSStatus.WarningState.ProtectionArmingFault is true ? true : null,
                        ProtectionDisarmingFault: Common.IBNSStatus.WarningState.ProtectionDisarmingFault is true ? true : null,
                        ExternalMainPowerOutage: Common.IBNSStatus.WarningState.ExternalMainPowerOutage is true ? true : null,
                        StorageUnitLowPowerSupply: Common.IBNSStatus.WarningState.StorageUnitLowPowerSupply is true ? true : null,
                        ArmedAutonomous: Common.IBNSStatus.WarningState.ArmedAutonomous is true ? true : null,
                        ArmedAlarm: Common.IBNSStatus.WarningState.ArmedAlarm is true ? true : null,
                        GasWarningLevel: Common.IBNSStatus.WarningState.GasWarningLevel is true ? true : null,
                        SeismicActivityWarningLevel: Common.IBNSStatus.WarningState.SeismicActivityWarningLevel is true ? true : null
                        );

                if (warnings is not null)
                {
                    if (warnings.ProtectionArmingFault is null && 
                        warnings.ProtectionDisarmingFault is null &&
                        warnings.ExternalMainPowerOutage is null &&
                        warnings.StorageUnitLowPowerSupply is null &&
                        warnings.ArmedAutonomous is null &&
                        warnings.ArmedAlarm is null &&
                        warnings.GasWarningLevel is null &&
                        warnings.SeismicActivityWarningLevel is null)
                    {
                        warnings = null;
                    }
                }

                XFS4IoT.BanknoteNeutralization.ErrorsClass errors = Common.IBNSStatus.ErrorState is null ? null :
                    new(
                        ProtectionEnablingFailure: Common.IBNSStatus.ErrorState.ProtectionEnablingFailure is true ? true : null,
                        ProtectionDisarmingFailure: Common.IBNSStatus.ErrorState.ProtectionDisarmingFailure is true ? true : null,
                        StorageUnitPowerSupplyFailure: Common.IBNSStatus.ErrorState.StorageUnitPowerSupplyFailure is true ? true : null,
                        BackupBatteryFailure: Common.IBNSStatus.ErrorState.BackupBatteryFailure is true ? true : null,
                        GasCriticalLevel: Common.IBNSStatus.ErrorState.GasCriticalLevel is true ? true : null,
                        Light: Common.IBNSStatus.ErrorState.Light is true ? true : null,
                        Tilted: Common.IBNSStatus.ErrorState.Tilted is true ? true : null,
                        SeismicActivityCriticalLevel: Common.IBNSStatus.ErrorState.SeismicActivityCriticalLevel is true ? true : null
                        );

                if (errors is not null)
                {
                    if (errors.ProtectionDisarmingFailure is null &&
                        errors.StorageUnitPowerSupplyFailure is null &&
                        errors.BackupBatteryFailure is null &&
                        errors.GasCriticalLevel is null &&
                        errors.Light is null &&
                        errors.Tilted is null &&
                        errors.SeismicActivityCriticalLevel is null)
                    {
                        errors = null;
                    }
                }

                if (Common.IBNSStatus.State is null)
                {
                    throw new InternalErrorException($"IBNS State object is a required property and must be set.");
                }

                ibns = new XFS4IoT.BanknoteNeutralization.StatusClass(
                    State: Common.IBNSStatus.State is null ?
                        throw new InternalErrorException($"IBNS State object is a required property and must be set.") : 
                        new XFS4IoT.BanknoteNeutralization.StateClass(
                            Mode: Common.IBNSStatus.State.Mode switch
                            {
                                IBNSStatusClass.StateClass.ModeEnum.NeutralizationTriggered => XFS4IoT.BanknoteNeutralization.StateClass.ModeEnum.NeutralizationTriggered,
                                IBNSStatusClass.StateClass.ModeEnum.Fault => XFS4IoT.BanknoteNeutralization.StateClass.ModeEnum.Fault,
                                IBNSStatusClass.StateClass.ModeEnum.Disarmed => XFS4IoT.BanknoteNeutralization.StateClass.ModeEnum.Disarmed,
                                IBNSStatusClass.StateClass.ModeEnum.Armed => XFS4IoT.BanknoteNeutralization.StateClass.ModeEnum.Armed,
                                _ => throw new InternalErrorException($"Unexpected mode state specified. {Common.IBNSStatus.State.Mode}")
                            },
                            Submode: Common.IBNSStatus.State.SubMode switch
                            {
                                IBNSStatusClass.StateClass.SubModeEnum.AllSafeSensorsIgnored => XFS4IoT.BanknoteNeutralization.StateClass.SubmodeEnum.AllSafeSensorsIgnored,
                                IBNSStatusClass.StateClass.SubModeEnum.ArmPending => XFS4IoT.BanknoteNeutralization.StateClass.SubmodeEnum.ArmPending,
                                _ => null,
                            }
                        ),
                    SafeDoor: Common.IBNSStatus.SafeDoorState switch
                    {
                        IBNSStatusClass.SafeDoorStateEnum.Fault => XFS4IoT.BanknoteNeutralization.SafeDoorStateEnum.Fault,
                        IBNSStatusClass.SafeDoorStateEnum.DoorClosed => XFS4IoT.BanknoteNeutralization.SafeDoorStateEnum.DoorClosed,
                        IBNSStatusClass.SafeDoorStateEnum.DoorOpened => XFS4IoT.BanknoteNeutralization.SafeDoorStateEnum.DoorOpened,
                        IBNSStatusClass.SafeDoorStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.SafeDoorStateEnum.Disabled,
                        _ => null,
                    },
                    SafeBolt: Common.IBNSStatus.SafeBoltState switch
                    {
                        IBNSStatusClass.SafeBoltStateEnum.Fault => XFS4IoT.BanknoteNeutralization.SafeBoltStateEnum.Fault,
                        IBNSStatusClass.SafeBoltStateEnum.BoltLocked => XFS4IoT.BanknoteNeutralization.SafeBoltStateEnum.BoltLocked,
                        IBNSStatusClass.SafeBoltStateEnum.BoltUnlocked => XFS4IoT.BanknoteNeutralization.SafeBoltStateEnum.BoltUnlocked,
                        IBNSStatusClass.SafeBoltStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.SafeBoltStateEnum.Disabled,
                        _ => null,
                    },
                    Tilt: Common.IBNSStatus.TiltState switch
                    {
                        IBNSStatusClass.TiltStateEnum.Fault => XFS4IoT.BanknoteNeutralization.TiltStateEnum.Fault,
                        IBNSStatusClass.TiltStateEnum.Tilted => XFS4IoT.BanknoteNeutralization.TiltStateEnum.Tilted,
                        IBNSStatusClass.TiltStateEnum.NotTilted => XFS4IoT.BanknoteNeutralization.TiltStateEnum.NotTilted,
                        IBNSStatusClass.TiltStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.TiltStateEnum.Disabled,
                        _ => null,
                    },
                    Light: Common.IBNSStatus.LightState switch
                    {
                        IBNSStatusClass.LightStateEnum.Fault => XFS4IoT.BanknoteNeutralization.LightStateEnum.Fault,
                        IBNSStatusClass.LightStateEnum.NotConfigured => XFS4IoT.BanknoteNeutralization.LightStateEnum.NotConfigured,
                        IBNSStatusClass.LightStateEnum.Detected => XFS4IoT.BanknoteNeutralization.LightStateEnum.Detected,
                        IBNSStatusClass.LightStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.LightStateEnum.Disabled,
                        _ => null,
                    },
                    Gas: Common.IBNSStatus.GasState switch
                    {
                        IBNSStatusClass.GasStateEnum.Fault => XFS4IoT.BanknoteNeutralization.GasStateEnum.Fault,
                        IBNSStatusClass.GasStateEnum.Initializing => XFS4IoT.BanknoteNeutralization.GasStateEnum.Initializing,
                        IBNSStatusClass.GasStateEnum.NotConfigured => XFS4IoT.BanknoteNeutralization.GasStateEnum.NotConfigured,
                        IBNSStatusClass.GasStateEnum.PartialWarningLevel => XFS4IoT.BanknoteNeutralization.GasStateEnum.PartialWarningLevel,
                        IBNSStatusClass.GasStateEnum.PartialCriticalLevel => XFS4IoT.BanknoteNeutralization.GasStateEnum.PartialCriticalLevel,
                        IBNSStatusClass.GasStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.GasStateEnum.Disabled,
                        IBNSStatusClass.GasStateEnum.WarningLevel => XFS4IoT.BanknoteNeutralization.GasStateEnum.WarningLevel,
                        IBNSStatusClass.GasStateEnum.CriticalLevel => XFS4IoT.BanknoteNeutralization.GasStateEnum.CriticalLevel,
                        IBNSStatusClass.GasStateEnum.NotDetected => XFS4IoT.BanknoteNeutralization.GasStateEnum.NotDetected,
                        _ => null,
                    },
                    Temperature: Common.IBNSStatus.TemperatureState switch
                    {
                        IBNSStatusClass.TemperatureStateEnum.Fault => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.Fault,
                        IBNSStatusClass.TemperatureStateEnum.Healthy=> XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.Ok,
                        IBNSStatusClass.TemperatureStateEnum.TooCold => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.TooCold,
                        IBNSStatusClass.TemperatureStateEnum.TooHot => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.TooHot,
                        IBNSStatusClass.TemperatureStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.TemperatureStateEnum.Disabled,
                        _ => null,
                    },
                    Seismic: Common.IBNSStatus.SeismicState switch
                    {
                        IBNSStatusClass.SeismicStateEnum.Fault => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.Fault,
                        IBNSStatusClass.SeismicStateEnum.NotConfigured => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.NotConfigured,
                        IBNSStatusClass.SeismicStateEnum.CriticalLevel => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.CriticalLevel,
                        IBNSStatusClass.SeismicStateEnum.WarningLevel => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.WarningLevel,
                        IBNSStatusClass.SeismicStateEnum.NotDetected => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.NotDetected,
                        IBNSStatusClass.SeismicStateEnum.Disabled => XFS4IoT.BanknoteNeutralization.SeismicStateEnum.Disabled,
                        _ => null,
                    },
                    CustomInputs: customInputs,
                    PowerSupply: Common.IBNSStatus.PowerInfo is null ? null:
                    new(
                        Info: new(
                        PowerInStatus: Common.IBNSStatus.PowerInfo.PowerInStatus switch
                        {
                            PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.Powering => XFS4IoT.PowerManagement.PowerInfoClass.PowerInStatusEnum.Powering,
                            PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.NotPower => XFS4IoT.PowerManagement.PowerInfoClass.PowerInStatusEnum.NoPower,
                            _ => throw new InternalErrorException($"Unexpected power-in status specified. {Common.IBNSStatus.PowerInfo.PowerInStatus}")
                        },
                        PowerOutStatus: Common.IBNSStatus.PowerInfo.PowerOutStatus switch
                        {
                            PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.Powering => XFS4IoT.PowerManagement.PowerInfoClass.PowerOutStatusEnum.Powering,
                            PowerManagementStatusClass.PowerInfoClass.PoweringStatusEnum.NotPower => XFS4IoT.PowerManagement.PowerInfoClass.PowerOutStatusEnum.NoPower,
                            _ => throw new InternalErrorException($"Unexpected power-out status specified. {Common.IBNSStatus.PowerInfo.PowerOutStatus}")
                        },
                        BatteryStatus: Common.IBNSStatus.PowerInfo.BatteryStatus switch
                        {
                            PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Low => XFS4IoT.PowerManagement.BatteryStatusEnum.Low,
                            PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Failure => XFS4IoT.PowerManagement.BatteryStatusEnum.Failure,
                            PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Operational => XFS4IoT.PowerManagement.BatteryStatusEnum.Operational,
                            PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Full => XFS4IoT.PowerManagement.BatteryStatusEnum.Full,
                            PowerManagementStatusClass.PowerInfoClass.BatteryStatusEnum.Critical => XFS4IoT.PowerManagement.BatteryStatusEnum.Critical,
                            _ => null
                        },
                        BatteryChargingStatus: Common.IBNSStatus.PowerInfo.BatteryChargingStatus switch
                        {
                            PowerManagementStatusClass.PowerInfoClass.BatteryChargingStatusEnum.Charging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.Charging,
                            PowerManagementStatusClass.PowerInfoClass.BatteryChargingStatusEnum.NotCharging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.NotCharging,
                            PowerManagementStatusClass.PowerInfoClass.BatteryChargingStatusEnum.Discharging => XFS4IoT.PowerManagement.BatteryChargingStatusEnum.Discharging,
                            _ => null
                        })),
                    Warnings: warnings,
                    Errors: errors);
            }

            XFS4IoT.Deposit.StatusClass deposit = null;
            if (Common.DepostStatus is not null)
            {
                deposit = new(
                    DepTransport: Common.DepostStatus.DepositTransport switch
                    {
                        DepositStatusClass.DepositTransportEnum.Healthy => XFS4IoT.Deposit.StatusClass.DepTransportEnum.Ok,
                        DepositStatusClass.DepositTransportEnum.Inoperative => XFS4IoT.Deposit.StatusClass.DepTransportEnum.Inoperative,
                        DepositStatusClass.DepositTransportEnum.Unknown => XFS4IoT.Deposit.StatusClass.DepTransportEnum.Unknown,
                        _ => null
                    },
                    EnvDispenser: Common.DepostStatus.EnvelopDispenser switch
                    {
                        DepositStatusClass.EnvelopDispenserEnum.Healthy => XFS4IoT.Deposit.StatusClass.EnvDispenserEnum.Ok,
                        DepositStatusClass.EnvelopDispenserEnum.Inoperative => XFS4IoT.Deposit.StatusClass.EnvDispenserEnum.Inoperative,
                        DepositStatusClass.EnvelopDispenserEnum.Unknown => XFS4IoT.Deposit.StatusClass.EnvDispenserEnum.Unknown,
                        _ => null
                    },
                    Printer: Common.DepostStatus.Printer switch
                    {
                        DepositStatusClass.PrinterEnum.Healthy => XFS4IoT.Deposit.StatusClass.PrinterEnum.Ok,
                        DepositStatusClass.PrinterEnum.Inoperative => XFS4IoT.Deposit.StatusClass.PrinterEnum.Inoperative,
                        DepositStatusClass.PrinterEnum.Unknown => XFS4IoT.Deposit.StatusClass.PrinterEnum.Unknown,
                        _ => null
                    },
                    Toner: Common.DepostStatus.Toner switch
                    {
                        DepositStatusClass.TonerEnum.Full => XFS4IoT.Deposit.StatusClass.TonerEnum.Full,
                        DepositStatusClass.TonerEnum.Low => XFS4IoT.Deposit.StatusClass.TonerEnum.Low,
                        DepositStatusClass.TonerEnum.Out => XFS4IoT.Deposit.StatusClass.TonerEnum.Out,
                        DepositStatusClass.TonerEnum.Unknown => XFS4IoT.Deposit.StatusClass.TonerEnum.Unknown,
                        _ => null
                    },
                    Shutter: Common.DepostStatus.Shutter switch
                    {
                        DepositStatusClass.ShutterEnum.Closed => XFS4IoT.Deposit.StatusClass.ShutterEnum.Closed,
                        DepositStatusClass.ShutterEnum.Open => XFS4IoT.Deposit.StatusClass.ShutterEnum.Open,
                        DepositStatusClass.ShutterEnum.Jammed => XFS4IoT.Deposit.StatusClass.ShutterEnum.Jammed,
                        DepositStatusClass.ShutterEnum.Unknown => XFS4IoT.Deposit.StatusClass.ShutterEnum.Unknown,
                        _ => null
                    },
                    DepositLocation: Common.DepostStatus.DepositLocation switch
                    {
                        DepositStatusClass.DepositLocationEnum.None => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.None,
                        DepositStatusClass.DepositLocationEnum.Container => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Container,
                        DepositStatusClass.DepositLocationEnum.Shutter => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Shutter,
                        DepositStatusClass.DepositLocationEnum.Unknown => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Unknown,
                        DepositStatusClass.DepositLocationEnum.Transport => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Transport,
                        DepositStatusClass.DepositLocationEnum.Removed => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Removed,
                        DepositStatusClass.DepositLocationEnum.Printer => XFS4IoT.Deposit.StatusClass.DepositLocationEnum.Printer,
                        _ => null
                    }
                    );
            }
            
            return Task.FromResult(
                new CommandResult<StatusCompletion.PayloadData>(
                    new StatusCompletion.PayloadData(
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
                        CashAcceptor: cashAcceptor,
                        Camera: camera,
                        Check: checkScanner,
                        MixedMedia: mixedMedia,
                        BanknoteNeutralization: ibns,
                        PowerManagement: powerManagement,
                        Deposit: deposit),
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}
