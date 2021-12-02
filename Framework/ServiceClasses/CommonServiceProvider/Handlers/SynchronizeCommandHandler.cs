/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Common
{
    public partial class SynchronizeCommandHandler
    {

        private async Task<SynchronizeCommandCompletion.PayloadData> HandleSynchronizeCommand(ISynchronizeCommandEvents events, SynchronizeCommandCommand synchronizeCommand, CancellationToken cancel)
        {
            CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum? auxiliariesCommand = synchronizeCommand.Payload.Command switch
            {
                "Auxiliaries.ClearAutoStartUpTime" => CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum.ClearAutoStartUpTime,
                "Auxiliaries.GetAutoStartUpTime" => CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum.GetAutoStartUpTime,
                "Auxiliaries.Register" => CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum.Register,
                "Auxiliaries.SetAutoStartUpTime" => CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum.SetAutoStartUpTime,
                "Auxiliaries.SetAuxiliaries" => CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum.SetAuxiliaries,
                _ => null
            };
            if (auxiliariesCommand is not null)
            {
                if (!Common.CommonCapabilities.AuxiliariesInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.AuxiliariesInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum? cardReaderCommand = synchronizeCommand.Payload.Command switch
            {
                "CardReader.ChipIO" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.ChipIO,
                "CardReader.ChipPower" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.ChipPower,
                "CardReader.EMVClessConfigure" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.EMVClessConfigure,
                "CardReader.EMVClessIssuerUpdate" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.EMVClessIssuerUpdate,
                "CardReader.EMVClessPerformTransaction" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.EMVClessPerformTransaction,
                "CardReader.EMVClessQueryApplications" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.EMVClessQueryApplications,
                "CardReader.Move" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.Move,
                "CardReader.QueryIFMIdentifier" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.QueryIFMIdentifier,
                "CardReader.ReadRawData" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.ReadRawData,
                "CardReader.Reset" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.Reset,
                "CardReader.SetKey" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.SetKey,
                "CardReader.WriteRawData" => CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum.WriteRawData,
                _ => null
            };
            if (cardReaderCommand is not null)
            {
                if (!Common.CommonCapabilities.CardReaderInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.CardReaderInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum? cashDispenserCommand = synchronizeCommand.Payload.Command switch
            {
                "CashDispenser.Count" => CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum.Count,
                "CashDispenser.Denominate" => CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum.Denominate,
                "CashDispenser.Dispense" => CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum.Dispense,
                "CashDispenser.GetMixTable" => CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum.GetMixTable,
                "CashDispenser.GetMixTypes" => CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum.GetMixTypes,
                "CashDispenser.GetPresentStatus" => CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum.GetPresentStatus,
                "CashDispenser.PrepareDispense" => CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum.PrepareDispense,
                "CashDispenser.Present" => CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum.Present,
                "CashDispenser.Reject" => CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum.Reject,
                "CashDispenser.SetMixTable" => CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum.SetMixTable,
                _ => null
            };
            if (cashDispenserCommand is not null)
            {
                if (!Common.CommonCapabilities.CashDispenserInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.CashDispenserInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum? cashManagementCommand = synchronizeCommand.Payload.Command switch
            {
                "CashManagement.CalibrateCashUnit" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.CalibrateCashUnit,
                "CashManagement.CloseShutter" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.CloseShutter,
                "CashManagement.GetBankNoteTypes" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.GetBankNoteTypes,
                "CashManagement.GetClassificationList" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.GetClassificationList,
                "CashManagement.GetItemInfo" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.GetItemInfo,
                "CashManagement.GetTellerInfo" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.GetTellerInfo,
                "CashManagement.OpenSafeDoor" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.OpenSafeDoor,
                "CashManagement.OpenShutter" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.OpenShutter,
                "CashManagement.Reset" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.Reset,
                "CashManagement.Retract" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.Retract,
                "CashManagement.SetClassificationList" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.SetClassificationList,
                "CashManagement.SetTellerInfo" => CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum.SetTellerInfo,
                _ => null
            };
            if (cashManagementCommand is not null)
            {
                if (!Common.CommonCapabilities.CashManagementInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.CashManagementInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum? commonCommand = synchronizeCommand.Payload.Command switch
            {
                "Common.Capabilities" => CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.Capabilities,
                "Common.Status" => CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.Status,
                "Common.ClearCommandNonce" => CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.ClearCommandNonce,
                "Common.GetCommandNonce" => CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.GetCommandNonce,
                "Common.GetTransactionState" => CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.GetTransactionState,
                "Common.PowerSaveControl" => CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.PowerSaveControl,
                "Common.SetTransactionState" => CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.SetTransactionState,
                "Common.SetVersions" => CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.SetVersions,
                "Common.SynchronizeCommand" => CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum.SynchronizeCommand,
                _ => null
            };
            if (commonCommand is not null)
            {
                if (!Common.CommonCapabilities.CommonInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.CommonInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.CryptoInterfaceClass.CommandEnum? cryptoCommand = synchronizeCommand.Payload.Command switch
            {
                "Crypto.CryptoData" => CommonCapabilitiesClass.CryptoInterfaceClass.CommandEnum.CryptoData,
                "Crypto.Digest" => CommonCapabilitiesClass.CryptoInterfaceClass.CommandEnum.Digest,
                "Crypto.GenerateAuthentication" => CommonCapabilitiesClass.CryptoInterfaceClass.CommandEnum.GenerateAuthentication,
                "Crypto.GenerateRandom" => CommonCapabilitiesClass.CryptoInterfaceClass.CommandEnum.GenerateRandom,
                "Crypto.VerifyAuthentication" => CommonCapabilitiesClass.CryptoInterfaceClass.CommandEnum.VerifyAuthentication,
                _ => null
            };
            if (cryptoCommand is not null)
            {
                if (!Common.CommonCapabilities.CryptoInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.CryptoInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum? keyboardCommand = synchronizeCommand.Payload.Command switch
            {
                "Keyboard.DataEntry" => CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum.DataEntry,
                "Keyboard.DefineLayout" => CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum.DefineLayout,
                "Keyboard.GetLayout" => CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum.GetLayout,
                "Keyboard.KeypressBeep" => CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum.KeypressBeep,
                "Keyboard.PinEntry" => CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum.PinEntry,
                "Keyboard.Reset" => CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum.Reset,
                "Keyboard.SecureKeyEntry" => CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum.SecureKeyEntry,
                _ => null
            };
            if (keyboardCommand is not null)
            {
                if (!Common.CommonCapabilities.KeyboardInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.KeyboardInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum? keyManagementCommand = synchronizeCommand.Payload.Command switch
            {
                "KeyManagement.DeleteKey" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.DeleteKey,
                "KeyManagement.DeriveKey" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.DeriveKey,
                "KeyManagement.ExportRSAEPPSignedItem" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.ExportRSAEPPSignedItem,
                "KeyManagement.ExportRSAIssuerSignedItem" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.ExportRSAIssuerSignedItem,
                "KeyManagement.GenerateKCV" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.GenerateKCV,
                "KeyManagement.GenerateRSAKeyPair" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.GenerateRSAKeyPair,
                "KeyManagement.GetCertificate" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.GetCertificate,
                "KeyManagement.GetKeyDetail" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.GetKeyDetail,
                "KeyManagement.ImportKey" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.ImportKey,
                "KeyManagement.Initialization" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.Initialization,
                "KeyManagement.LoadCertificate" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.LoadCertificate,
                "KeyManagement.ReplaceCertificate" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.ReplaceCertificate,
                "KeyManagement.Reset" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.Reset,
                "KeyManagement.StartAuthenticate" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.StartAuthenticate,
                "KeyManagement.StartKeyExchange" => CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum.StartKeyExchange,
                _ => null
            };
            if (keyManagementCommand is not null)
            {
                if (!Common.CommonCapabilities.KeyManagementInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.KeyManagementInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.LightsInterfaceClass.CommandEnum? lightsCommand = synchronizeCommand.Payload.Command switch
            {
                "Lights.SetLight" => CommonCapabilitiesClass.LightsInterfaceClass.CommandEnum.SetLight,
                _ => null
            };
            if (lightsCommand is not null)
            {
                if (!Common.CommonCapabilities.LightsInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.LightsInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum? pinPadCommand = synchronizeCommand.Payload.Command switch
            {
                "PinPad.GetPinBlock" => CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum.GetPinBlock,
                "PinPad.GetQueryPCIPTSDeviceId" => CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum.GetQueryPCIPTSDeviceId,
                "PinPad.LocalDES" => CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum.LocalDES,
                "PinPad.LocalVisa" => CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum.LocalVisa,
                "PinPad.MaintainPin" => CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum.MaintainPin,
                "PinPad.PresentIDC" => CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum.PresentIDC,
                "PinPad.Reset" => CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum.Reset,
                "PinPad.SetPinBlockData" => CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum.SetPinBlockData,
                _ => null
            };
            if (pinPadCommand is not null)
            {
                if (!Common.CommonCapabilities.PinPadInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.PinPadInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum? printerCommand = synchronizeCommand.Payload.Command switch
            {
                "Printer.ControlMedia" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.ControlMedia,
                "Printer.ControlPassbook" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.ControlPassbook,
                "Printer.DispensePaper" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.DispensePaper,
                "Printer.GetCodelineMapping" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.GetCodelineMapping,
                "Printer.GetFormList" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.GetFormList,
                "Printer.GetMediaList" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.GetMediaList,
                "Printer.GetQueryField" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.GetQueryField,
                "Printer.GetQueryForm" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.GetQueryForm,
                "Printer.GetQueryMedia" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.GetQueryMedia,
                "Printer.LoadDefinition" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.LoadDefinition,
                "Printer.MediaExtents" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.MediaExtents,
                "Printer.PrintForm" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.PrintForm,
                "Printer.PrintNative" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.PrintNative,
                "Printer.PrintRaw" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.PrintRaw,
                "Printer.ReadForm" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.ReadForm,
                "Printer.ReadImage" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.ReadImage,
                "Printer.Reset" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.Reset,
                "Printer.ResetCount" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.ResetCount,
                "Printer.RetractMedia" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.RetractMedia,
                "Printer.SetBlackMarkMode" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.SetBlackMarkMode,
                "Printer.SupplyReplenish" => CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum.SupplyReplenish,
                _ => null
            };
            if (printerCommand is not null)
            {
                if (!Common.CommonCapabilities.PrinterInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.PrinterInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.StorageInterfaceClass.CommandEnum? storageCommand = synchronizeCommand.Payload.Command switch
            {
                "Storage.EndExchange" => CommonCapabilitiesClass.StorageInterfaceClass.CommandEnum.EndExchange,
                "Storage.GetStorage" => CommonCapabilitiesClass.StorageInterfaceClass.CommandEnum.GetStorage,
                "Storage.SetStorage" => CommonCapabilitiesClass.StorageInterfaceClass.CommandEnum.SetStorage,
                "Storage.StartExchange" => CommonCapabilitiesClass.StorageInterfaceClass.CommandEnum.StartExchange,
                _ => null
            };
            if (storageCommand is not null)
            {
                if (!Common.CommonCapabilities.StorageInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.StorageInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum? textTerminalCommand = synchronizeCommand.Payload.Command switch
            {
                "TextTerminal.Beep" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.Beep,
                "TextTerminal.ClearScreen" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.ClearScreen,
                "TextTerminal.DefineKeys" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.DefineKeys,
                "TextTerminal.GetFormList" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.GetFormList,
                "TextTerminal.GetKeyDetail" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.GetKeyDetail,
                "TextTerminal.GetQueryField" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.GetQueryField,
                "TextTerminal.GetQueryForm" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.GetQueryForm,
                "TextTerminal.Read" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.Read,
                "TextTerminal.ReadForm" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.ReadForm,
                "TextTerminal.Reset" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.Reset,
                "TextTerminal.SetResolution" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.SetResolution,
                "TextTerminal.Write" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.Write,
                "TextTerminal.WriteForm" => CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum.WriteForm,
                _ => null
            };
            if (textTerminalCommand is not null)
            {
                if (!Common.CommonCapabilities.TextTerminalInterface.SynchronizableCommands.Contains((CommonCapabilitiesClass.TextTerminalInterfaceClass.CommandEnum)auxiliariesCommand))
                {
                    return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
                }
            }

            if (auxiliariesCommand is null &&
                cardReaderCommand is null &&
                cashDispenserCommand is null &&
                cashManagementCommand is null &&
                commonCommand is null &&
                cryptoCommand is null &&
                keyboardCommand is null && 
                keyManagementCommand is null &&
                lightsCommand is null &&
                pinPadCommand is null &&
                printerCommand is null &&
                storageCommand is null &&
                textTerminalCommand is null)
            {
                return new SynchronizeCommandCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                    $"Specified command is not supported. {synchronizeCommand.Payload.Command}");
            }

            Logger.Log(Constants.DeviceClass, "CommonDev.SynchronizeCommand()");
            var result = await Device.SynchronizeCommand(new SynchronizeCommandRequest(auxiliariesCommand,
                                                                                       cardReaderCommand,
                                                                                       cashDispenserCommand,
                                                                                       cashManagementCommand,
                                                                                       commonCommand,
                                                                                       cryptoCommand,
                                                                                       keyboardCommand,
                                                                                       keyManagementCommand,
                                                                                       lightsCommand,
                                                                                       pinPadCommand,
                                                                                       printerCommand,
                                                                                       storageCommand,
                                                                                       textTerminalCommand,
                                                                                       synchronizeCommand.Payload.CmdData));
            Logger.Log(Constants.DeviceClass, $"CommonDev.SynchronizeCommand() -> {result.CompletionCode}");

            return new SynchronizeCommandCompletion.PayloadData(result.CompletionCode,
                                                                result.ErrorDescription);
        }
    }
}
