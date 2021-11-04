/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Common
{
    [CommandHandlerAsync]
    public partial class CapabilitiesHandler
    {

        private Task<CapabilitiesCompletion.PayloadData> HandleCapabilities(ICapabilitiesEvents events, CapabilitiesCommand capabilities, CancellationToken cancel)
        {
            if (Common.CommonCapabilities is null)
            {
                return Task.FromResult(new CapabilitiesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError, $"No common capabilities is reported by the device class."));
            }

            List<XFS4IoT.Common.InterfaceClass> interfaces = new();
            foreach (var commonInterface in Common.CommonCapabilities.Interfaces)
            {
                Dictionary<string, XFS4IoT.Common.InterfaceClass.CommandsClass> supportedCommands = null;
                if ( commonInterface.Commands?.Count > 0)
                {
                    supportedCommands = new();
                    foreach (var cmd in commonInterface.Commands)
                    {
                        supportedCommands.Add(cmd.Key, new XFS4IoT.Common.InterfaceClass.CommandsClass(cmd.Value?.Versions));
                    }
                }

                Dictionary<string, XFS4IoT.Common.InterfaceClass.EventsClass> supportedEvents = null;
                if (commonInterface.Events?.Count > 0)
                {
                    supportedEvents = new();
                    foreach (var ev in commonInterface.Events)
                    {
                        supportedEvents.Add(ev.Key, new XFS4IoT.Common.InterfaceClass.EventsClass(ev.Value?.Versions));
                    }
                }

                interfaces.Add(new XFS4IoT.Common.InterfaceClass(
                    Name: commonInterface.Name switch
                    {
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.Auxiliaries => XFS4IoT.Common.InterfaceClass.NameEnum.Auxiliaries,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.BarcodeReader => XFS4IoT.Common.InterfaceClass.NameEnum.BarcodeReader,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.Camera => XFS4IoT.Common.InterfaceClass.NameEnum.Camera,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.CardEmbosser => XFS4IoT.Common.InterfaceClass.NameEnum.CardEmbosser,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.CardReader => XFS4IoT.Common.InterfaceClass.NameEnum.CardReader,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.CashAcceptor => XFS4IoT.Common.InterfaceClass.NameEnum.CashAcceptor,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.CashDispenser => XFS4IoT.Common.InterfaceClass.NameEnum.CashDispenser,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.CashManagement => XFS4IoT.Common.InterfaceClass.NameEnum.CashManagement,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.Common => XFS4IoT.Common.InterfaceClass.NameEnum.Common,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.Crypto => XFS4IoT.Common.InterfaceClass.NameEnum.Crypto,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.Keyboard => XFS4IoT.Common.InterfaceClass.NameEnum.Keyboard,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.KeyManagement => XFS4IoT.Common.InterfaceClass.NameEnum.KeyManagement,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.Lights => XFS4IoT.Common.InterfaceClass.NameEnum.Lights,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.PinPad => XFS4IoT.Common.InterfaceClass.NameEnum.PinPad,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.Printer => XFS4IoT.Common.InterfaceClass.NameEnum.Printer,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.Storage => XFS4IoT.Common.InterfaceClass.NameEnum.Storage,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.TextTerminal => XFS4IoT.Common.InterfaceClass.NameEnum.TextTerminal,
                        CommonCapabilitiesClass.InterfaceClass.NameEnum.VendorApplication => XFS4IoT.Common.InterfaceClass.NameEnum.VendorApplication,
                        _ => XFS4IoT.Common.InterfaceClass.NameEnum.VendorMode,
                    },
                    Commands: supportedCommands,
                    Events: supportedEvents,
                    MaximumRequests: commonInterface.MaximumRequests,
                    AuthenticationRequired: commonInterface.AuthenticationRequired
                    ));
            }

            List<XFS4IoT.Common.DeviceInformationClass> deviceInformation = null;
            if (Common.CommonCapabilities.DeviceInformation?.Count > 0)
            {
                deviceInformation = new();
                foreach (var device in Common.CommonCapabilities.DeviceInformation)
                {
                    List<XFS4IoT.Common.FirmwareClass> firmware = null;
                    if (device.Firmware?.Count > 0)
                    {
                        firmware = new();
                        foreach (var firm in device.Firmware)
                        {
                            firmware.Add(new XFS4IoT.Common.FirmwareClass(
                                FirmwareName: firm.FirmwareName,
                                FirmwareVersion: firm.FirmwareVersion,
                                HardwareRevision: firm.HardwareRevision
                                ));
                        }
                    }

                    List<XFS4IoT.Common.SoftwareClass> software = null;
                    if (device.Software?.Count > 0)
                    {
                        software = new();
                        foreach (var soft in device.Software)
                        {
                            software.Add(new XFS4IoT.Common.SoftwareClass(
                                SoftwareName: soft.SoftwareName,
                                SoftwareVersion: soft.SoftwareVersion
                                ));
                        }
                    }

                    deviceInformation.Add(new XFS4IoT.Common.DeviceInformationClass(
                        ModelName: device.ModelName,
                        SerialNumber: device.SerialNumber,
                        RevisionNumber: device.RevisionNumber,
                        ModelDescription: device.ModelDescription,
                        Firmware: firmware,
                        Software: software
                        ));
                }
            }

            XFS4IoT.Common.CapabilityPropertiesClass commonCapabilities = new(
                ServiceVersion: Common.CommonCapabilities.ServiceVersion,
                DeviceInformation: deviceInformation,
                VendorModeIformation: Common.CommonCapabilities.VendorModeIformation is null ? null :
                new XFS4IoT.Common.VendorModeInfoClass(
                    Common.CommonCapabilities.VendorModeIformation.AllowOpenSessions,
                    Common.CommonCapabilities.VendorModeIformation.AllowedExecuteCommands
                    ),
                PowerSaveControl: Common.CommonCapabilities.PowerSaveControl,
                AntiFraudModule: Common.CommonCapabilities.AntiFraudModule,
                SynchronizableCommands: Common.CommonCapabilities.SynchronizableCommands,
                EndToEndSecurity: Common.CommonCapabilities.EndToEndSecurity,
                HardwareSecurityElement: Common.CommonCapabilities.HardwareSecurityElement,
                ResponseSecurityEnabled: Common.CommonCapabilities.ResponseSecurityEnabled,
                CommandNonceTimeout: Common.CommonCapabilities.CommandNonceTimeout
                );

            XFS4IoT.CardReader.CapabilitiesClass cardReader = null;
            if (Common.CardReaderCapabilities is not null)
            {
                cardReader = new XFS4IoT.CardReader.CapabilitiesClass(
                    Common.CardReaderCapabilities.Type switch
                    {
                        CardReaderCapabilitiesClass.DeviceTypeEnum.Contactless => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Contactless,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.Dip => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Dip,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.IntelligentContactless,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.LatchedDip => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.LatchedDip,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.Motor => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Motor,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.Permanent => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Permanent,
                        _ => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Swipe,
                    },
                    ReadTracks: new XFS4IoT.CardReader.CapabilitiesClass.ReadTracksClass(
                        Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1),
                        Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track2),
                        Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track3),
                        Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Watermark),
                        Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1Front),
                        Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.FrontImage),
                        Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.BackImage),
                        Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1JIS),
                        Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track3JIS),
                        Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Ddi)
                        ),
                    WriteTracks: new XFS4IoT.CardReader.CapabilitiesClass.WriteTracksClass(
                        Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1),
                        Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track2),
                        Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3),
                        Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1Front),
                        Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1JIS),
                        Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3JIS)
                        ),
                    ChipProtocols: new XFS4IoT.CardReader.CapabilitiesClass.ChipProtocolsClass(
                        Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.T0),
                        Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.T1),
                        Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.NotRequired),
                        Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeAPart3),
                        Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeAPart4),
                        Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeB),
                        Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeNFC)
                        ),
                    SecurityType: Common.CardReaderCapabilities.SecurityType switch
                    {
                        CardReaderCapabilitiesClass.SecurityTypeEnum.Cim86 => XFS4IoT.CardReader.CapabilitiesClass.SecurityTypeEnum.Cim86,
                        CardReaderCapabilitiesClass.SecurityTypeEnum.Mm => XFS4IoT.CardReader.CapabilitiesClass.SecurityTypeEnum.Mm,
                        _ => XFS4IoT.CardReader.CapabilitiesClass.SecurityTypeEnum.NotSupported,
                    },
                    PowerOnOption: Common.CardReaderCapabilities.PowerOnOption switch
                    {
                        CardReaderCapabilitiesClass.PowerOptionEnum.Exit => XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.Exit,
                        CardReaderCapabilitiesClass.PowerOptionEnum.ExitThenRetain => XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.ExitThenRetain,
                        CardReaderCapabilitiesClass.PowerOptionEnum.Retain => XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.Retain,
                        CardReaderCapabilitiesClass.PowerOptionEnum.Transport => XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.Transport,
                        _ => XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.NotSupported,
                    },
                    PowerOffOption: Common.CardReaderCapabilities.PowerOffOption switch
                    {
                        CardReaderCapabilitiesClass.PowerOptionEnum.Exit => XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.Exit,
                        CardReaderCapabilitiesClass.PowerOptionEnum.ExitThenRetain => XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.ExitThenRetain,
                        CardReaderCapabilitiesClass.PowerOptionEnum.Retain => XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.Retain,
                        CardReaderCapabilitiesClass.PowerOptionEnum.Transport => XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.Transport,
                        _ => XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.NotSupported,
                    },
                    Common.CardReaderCapabilities.FluxSensorProgrammable,
                    Common.CardReaderCapabilities.ReadWriteAccessFollowingExit,
                    WriteMode: new XFS4IoT.CardReader.CapabilitiesClass.WriteModeClass(
                        Common.CardReaderCapabilities.WriteMode == CardReaderCapabilitiesClass.WriteMethodsEnum.NotSupported,
                        Common.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Loco),
                        Common.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Hico),
                        Common.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Auto)),
                    ChipPower: new XFS4IoT.CardReader.CapabilitiesClass.ChipPowerClass(
                        Common.CardReaderCapabilities.ChipPower == CardReaderCapabilitiesClass.ChipPowerOptionsEnum.NotSupported,
                        Common.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Cold),
                        Common.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Warm),
                        Common.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Off)),
                    MemoryChipProtocols: new XFS4IoT.CardReader.CapabilitiesClass.MemoryChipProtocolsClass(
                        Common.CardReaderCapabilities.MemoryChipProtocols.HasFlag(CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.Siemens4442),
                        Common.CardReaderCapabilities.MemoryChipProtocols.HasFlag(CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.Gpm896)),
                    Positions: new XFS4IoT.CardReader.CapabilitiesClass.PositionsClass(
                        Common.CardReaderCapabilities.Positions.HasFlag(CardReaderCapabilitiesClass.PositionsEnum.Exit),
                        Common.CardReaderCapabilities.Positions.HasFlag(CardReaderCapabilitiesClass.PositionsEnum.Transport)),
                    CardTakenSensor: Common.CardReaderCapabilities.CardTakenSensor
                    );
            }

            XFS4IoT.CashDispenser.CapabilitiesClass cashDispenser = null;
            if (Common.CashDispenserCapabilities is not null)
            {
                cashDispenser = new XFS4IoT.CashDispenser.CapabilitiesClass(
                    Type: Common.CashDispenserCapabilities.Type switch
                    {
                        CashDispenserCapabilitiesClass.TypeEnum.SelfServiceBill => XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.SelfServiceBill,
                        CashDispenserCapabilitiesClass.TypeEnum.SelfServiceCoin => XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.SelfServiceCoin,
                        CashDispenserCapabilitiesClass.TypeEnum.TellerBill => XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.TellerBill,
                        _ => XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.TellerCoin
                    },
                    MaxDispenseItems: Common.CashDispenserCapabilities.MaxDispenseItems,
                    ShutterControl: Common.CashDispenserCapabilities.ShutterControl,
                    RetractAreas: new XFS4IoT.CashDispenser.CapabilitiesClass.RetractAreasClass(
                        Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Retract),
                        Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Transport),
                        Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Stacker),
                        Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Reject),
                        Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette)
                        ),
                    RetractTransportActions: new XFS4IoT.CashDispenser.CapabilitiesClass.RetractTransportActionsClass(
                        Common.CashDispenserCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.Present),
                        Common.CashDispenserCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.Retract),
                        Common.CashDispenserCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.Reject),
                        Common.CashDispenserCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.ItemCassette)
                        ),
                    RetractStackerActions: new XFS4IoT.CashDispenser.CapabilitiesClass.RetractStackerActionsClass(
                        Common.CashDispenserCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.Present),
                        Common.CashDispenserCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.Retract),
                        Common.CashDispenserCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.Reject),
                        Common.CashDispenserCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.ItemCassette)
                        ),
                    IntermediateStacker: Common.CashDispenserCapabilities.IntermediateStacker,
                    ItemsTakenSensor: Common.CashDispenserCapabilities.ItemsTakenSensor,
                    Positions: new XFS4IoT.CashDispenser.CapabilitiesClass.PositionsClass(
                        Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashDispenserCapabilitiesClass.OutputPositionEnum.Left),
                        Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashDispenserCapabilitiesClass.OutputPositionEnum.Right),
                        Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashDispenserCapabilitiesClass.OutputPositionEnum.Center),
                        Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashDispenserCapabilitiesClass.OutputPositionEnum.Top),
                        Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom),
                        Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashDispenserCapabilitiesClass.OutputPositionEnum.Front),
                        Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashDispenserCapabilitiesClass.OutputPositionEnum.Rear)
                        ),
                    MoveItems: new XFS4IoT.CashDispenser.CapabilitiesClass.MoveItemsClass(
                        Common.CashDispenserCapabilities.MoveItems.HasFlag(CashDispenserCapabilitiesClass.MoveItemEnum.FromCashUnit),
                        Common.CashDispenserCapabilities.MoveItems.HasFlag(CashDispenserCapabilitiesClass.MoveItemEnum.ToCashUnit),
                        Common.CashDispenserCapabilities.MoveItems.HasFlag(CashDispenserCapabilitiesClass.MoveItemEnum.ToTransport),
                        Common.CashDispenserCapabilities.MoveItems.HasFlag(CashDispenserCapabilitiesClass.MoveItemEnum.ToStacker)
                        )
                    );
            }

            XFS4IoT.CashManagement.CapabilitiesClass cashManagement = null;
            if (Common.CashManagementCapabilities is not null)
            {
                cashManagement = new XFS4IoT.CashManagement.CapabilitiesClass(
                    SafeDoor: Common.CashManagementCapabilities.SafeDoor,
                    CashBox: Common.CashManagementCapabilities.CashBox,
                    ExchangeType: new XFS4IoT.CashManagement.CapabilitiesClass.ExchangeTypeClass(Common.CashManagementCapabilities.ExchangeTypes.HasFlag(CashManagementCapabilitiesClass.ExchangeTypesEnum.ByHand)),
                    ItemInfoTypes: new XFS4IoT.CashManagement.CapabilitiesClass.ItemInfoTypesClass(
                        Common.CashManagementCapabilities.ItemInfoTypes.HasFlag(CashManagementCapabilitiesClass.ItemInfoTypesEnum.SerialNumber),
                        Common.CashManagementCapabilities.ItemInfoTypes.HasFlag(CashManagementCapabilitiesClass.ItemInfoTypesEnum.Signature),
                        Common.CashManagementCapabilities.ItemInfoTypes.HasFlag(CashManagementCapabilitiesClass.ItemInfoTypesEnum.ImageFile)),
                    ClassificationList: Common.CashManagementCapabilities.ClassificationList
                    );
            }

            XFS4IoT.Crypto.CapabilitiesClass crypto = null;
            if (Common.CryptoCapabilities is not null)
            {
                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.CryptoAttributesClass>>> cryptoAttrib = null;
                if (Common.CryptoCapabilities.CryptoAttributes?.Count > 0)
                {
                    cryptoAttrib = new();
                    foreach (var (keyUsage, algorithms) in Common.CryptoCapabilities.CryptoAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.CryptoAttributesClass>> dicAttributes = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.CryptoAttributesClass> dicModeOfUse = new();
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                dicModeOfUse.Add(modeOfUse, new XFS4IoT.Crypto.CapabilitiesClass.CryptoAttributesClass(
                                    new XFS4IoT.Crypto.CapabilitiesClass.CryptoAttributesClass.CryptoMethodClass(
                                        method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB),
                                        method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CBC),
                                        method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CFB),
                                        method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.OFB),
                                        method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CTR),
                                        method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.XTS),
                                        method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.RSAES_PKCS1_V1_5),
                                        method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.RSAES_OAEP))
                                    ));
                            }
                            dicAttributes.Add(algorithm, dicModeOfUse);
                        }
                        cryptoAttrib.Add(keyUsage, dicAttributes);
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.AuthenticationAttributesClass>>> authAttrib = null;
                if ( Common.CryptoCapabilities.AuthenticationAttributes?.Count > 0)
                {
                    authAttrib = new();
                    foreach (var (keyUsage, algorithms) in Common.CryptoCapabilities.AuthenticationAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.AuthenticationAttributesClass>> dicAttributes = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.AuthenticationAttributesClass> dicModeOfUse = new();
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                dicModeOfUse.Add(modeOfUse, new XFS4IoT.Crypto.CapabilitiesClass.AuthenticationAttributesClass(
                                    new XFS4IoT.Crypto.CapabilitiesClass.AuthenticationAttributesClass.CryptoMethodClass(method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5),
                                                                                                                         method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PSS)), 
                                    new XFS4IoT.Crypto.CapabilitiesClass.AuthenticationAttributesClass.HashAlgorithmClass(method?.HashAlgorithms.HasFlag(CryptoCapabilitiesClass.HashAlgorithmEnum.SHA1),
                                                                                                                          method?.HashAlgorithms.HasFlag(CryptoCapabilitiesClass.HashAlgorithmEnum.SHA256))
                                    ));
                            }
                            dicAttributes.Add(algorithm, dicModeOfUse);
                        }
                        authAttrib.Add(keyUsage, dicAttributes);
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.VerifyAttributesClass>>> verifyAttrib = null;
                if (Common.CryptoCapabilities.VerifyAttributes?.Count > 0)
                {
                    verifyAttrib = new();
                    foreach (var (keyUsage, algorithms) in Common.CryptoCapabilities.VerifyAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.VerifyAttributesClass>> dicAttributes = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.VerifyAttributesClass> dicModeOfUse = new();
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                dicModeOfUse.Add(modeOfUse, new XFS4IoT.Crypto.CapabilitiesClass.VerifyAttributesClass(
                                    new XFS4IoT.Crypto.CapabilitiesClass.VerifyAttributesClass.CryptoMethodClass(method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5),
                                                                                                                 method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PSS)),
                                    new XFS4IoT.Crypto.CapabilitiesClass.VerifyAttributesClass.HashAlgorithmClass(method?.HashAlgorithms.HasFlag(CryptoCapabilitiesClass.HashAlgorithmEnum.SHA1),
                                                                                                                  method?.HashAlgorithms.HasFlag(CryptoCapabilitiesClass.HashAlgorithmEnum.SHA256))
                                    ));
                            }
                            dicAttributes.Add(algorithm, dicModeOfUse);
                        }
                        verifyAttrib.Add(keyUsage, dicAttributes);
                    }
                }

                crypto = new XFS4IoT.Crypto.CapabilitiesClass(
                    EmvHashAlgorithm: new XFS4IoT.Crypto.CapabilitiesClass.EmvHashAlgorithmClass(Common.CryptoCapabilities.EMVHashAlgorithms.HasFlag(CryptoCapabilitiesClass.EMVHashAlgorithmEnum.SHA1_Digest),
                                                                               Common.CryptoCapabilities.EMVHashAlgorithms.HasFlag(CryptoCapabilitiesClass.EMVHashAlgorithmEnum.SHA256_Digest)),
                    CryptoAttributes: cryptoAttrib,
                    AuthenticationAttributes: authAttrib,
                    VerifyAttributes: verifyAttrib);
            }

            XFS4IoT.KeyManagement.CapabilitiesClass keyManagement = null;
            if (Common.KeyManagementCapabilities is not null)
            {
                List<XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass> loadCertOptions = null;
                if (Common.KeyManagementCapabilities.LoadCertificationOptions?.Count > 0)
                {
                    loadCertOptions = new();
                    foreach (var certOption in Common.KeyManagementCapabilities.LoadCertificationOptions)
                    {
                        loadCertOptions.Add(new XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass(
                            certOption.Signer switch
                            {
                                KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CA => XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.Ca,
                                KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CertHost => XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.CertHost,
                                KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.HL => XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.Hl,
                                _ => XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.SigHost,
                            },
                            new XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.OptionClass(
                                certOption.Option.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateOptionEnum.NewHost),
                                certOption.Option.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateOptionEnum.ReplaceHost)
                                )
                            )); 
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.KeyAttributesClass>>> keyAttrib = null;
                if (Common.KeyManagementCapabilities.KeyAttributes?.Count > 0)
                {
                    keyAttrib = new();
                    foreach (var (keyUsage, algorithms) in Common.KeyManagementCapabilities.KeyAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.KeyAttributesClass>> dicAttributes = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.KeyAttributesClass> dicModeOfUse = new();
                            foreach (var (modeOfUse, restrict) in modeOfUses)
                            {
                                dicModeOfUse.Add(modeOfUse, new XFS4IoT.KeyManagement.CapabilitiesClass.KeyAttributesClass(restrict?.Restricted));
                            }
                            dicAttributes.Add(algorithm, dicModeOfUse);
                        }
                        keyAttrib.Add(keyUsage, dicAttributes);
                    }
                }

                Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.DecryptAttributesClass> decryptAttrib = null;
                if (Common.KeyManagementCapabilities.DecryptAttributes?.Count > 0)
                {
                    decryptAttrib = new();
                    foreach (var (algorithm, method) in Common.KeyManagementCapabilities.DecryptAttributes)
                    {
                        decryptAttrib.Add(algorithm, new XFS4IoT.KeyManagement.CapabilitiesClass.DecryptAttributesClass(
                            new XFS4IoT.KeyManagement.CapabilitiesClass.DecryptAttributesClass.DecryptMethodClass(
                                method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.ECB),
                                method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CBC),
                                method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CFB),
                                method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.OFB),
                                method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CTR),
                                method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.XTS),
                                method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.RSAES_PKCS1_V1_5),
                                method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.RSAES_OAEP))
                                )
                            );
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.VerifyAttributesClass>>> verifyAttrib = new();
                if (Common.KeyManagementCapabilities.VerifyAttributes?.Count > 0)
                {
                    verifyAttrib = new();
                    foreach (var (keyUsage, algorithms) in Common.KeyManagementCapabilities.VerifyAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.VerifyAttributesClass>> dicAttributes = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.VerifyAttributesClass> dicModeOfUse = new();
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                dicModeOfUse.Add(modeOfUse, new XFS4IoT.KeyManagement.CapabilitiesClass.VerifyAttributesClass(
                                    new XFS4IoT.KeyManagement.CapabilitiesClass.VerifyAttributesClass.CryptoMethodClass(
                                        method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVNone),
                                        method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVSelf),
                                        method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVZero),
                                        method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.SignatureNone),
                                        method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.RSASSA_PKCS1_V1_5),
                                        method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.RSASSA_PSS)),
                                    new XFS4IoT.KeyManagement.CapabilitiesClass.VerifyAttributesClass.HashAlgorithmClass(
                                        method?.HashAlgorithm.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum.SHA1),
                                        method?.HashAlgorithm.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum.SHA256))
                                        )
                                    );
                            }
                            dicAttributes.Add(algorithm, dicModeOfUse);
                        }
                        verifyAttrib.Add(keyUsage, dicAttributes);
                    }
                }

                keyManagement = new XFS4IoT.KeyManagement.CapabilitiesClass(
                    KeyNum: Common.KeyManagementCapabilities.MaxKeys,
                    DerivationAlgorithms: new XFS4IoT.KeyManagement.CapabilitiesClass.DerivationAlgorithmsClass(
                        ChipZka: false),
                    KeyCheckModes: new XFS4IoT.KeyManagement.CapabilitiesClass.KeyCheckModesClass(
                        Common.KeyManagementCapabilities.KeyCheckModes.HasFlag(KeyManagementCapabilitiesClass.KeyCheckModeEnum.Self),
                        Common.KeyManagementCapabilities.KeyCheckModes.HasFlag(KeyManagementCapabilitiesClass.KeyCheckModeEnum.Zero)),
                    HsmVendor: Common.KeyManagementCapabilities.HSMVendor,
                    RsaAuthenticationScheme: new XFS4IoT.KeyManagement.CapabilitiesClass.RsaAuthenticationSchemeClass(
                        Common.KeyManagementCapabilities.RSAAuthenticationScheme.HasFlag(KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum.SecondPartySignature),
                        Common.KeyManagementCapabilities.RSAAuthenticationScheme.HasFlag(KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum.ThirdPartyCertificate),
                        Common.KeyManagementCapabilities.RSAAuthenticationScheme.HasFlag(KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum.ThirdPartyCertificateTR34)),
                    RsaSignatureAlgorithm: new XFS4IoT.KeyManagement.CapabilitiesClass.RsaSignatureAlgorithmClass(
                        Common.KeyManagementCapabilities.RSASignatureAlgorithm.HasFlag(KeyManagementCapabilitiesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5),
                        Common.KeyManagementCapabilities.RSASignatureAlgorithm.HasFlag(KeyManagementCapabilitiesClass.RSASignatureAlgorithmEnum.RSASSA_PSS)),
                    RsaCryptAlgorithm: new XFS4IoT.KeyManagement.CapabilitiesClass.RsaCryptAlgorithmClass(
                        Common.KeyManagementCapabilities.RSACryptAlgorithm.HasFlag(KeyManagementCapabilitiesClass.RSACryptAlgorithmEnum.RSAES_PKCS1_V1_5),
                        Common.KeyManagementCapabilities.RSACryptAlgorithm.HasFlag(KeyManagementCapabilitiesClass.RSACryptAlgorithmEnum.RSAES_OAEP)),
                    RsaKeyCheckMode: new XFS4IoT.KeyManagement.CapabilitiesClass.RsaKeyCheckModeClass(
                        Common.KeyManagementCapabilities.RSAKeyCheckMode.HasFlag(KeyManagementCapabilitiesClass.RSAKeyCheckModeEnum.SHA1),
                        Common.KeyManagementCapabilities.RSAKeyCheckMode.HasFlag(KeyManagementCapabilitiesClass.RSAKeyCheckModeEnum.SHA256)),
                    SignatureScheme: new XFS4IoT.KeyManagement.CapabilitiesClass.SignatureSchemeClass(
                        Common.KeyManagementCapabilities.SignatureScheme.HasFlag(KeyManagementCapabilitiesClass.SignatureSchemeEnum.RSAKeyPair),
                        Common.KeyManagementCapabilities.SignatureScheme.HasFlag(KeyManagementCapabilitiesClass.SignatureSchemeEnum.RandomNumber),
                        Common.KeyManagementCapabilities.SignatureScheme.HasFlag(KeyManagementCapabilitiesClass.SignatureSchemeEnum.ExportEPPID),
                        Common.KeyManagementCapabilities.SignatureScheme.HasFlag(KeyManagementCapabilitiesClass.SignatureSchemeEnum.EnhancedRKL)),
                    EmvImportSchemes: new XFS4IoT.KeyManagement.CapabilitiesClass.EmvImportSchemesClass(
                        Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.PlainCA),
                        Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.ChecksumCA),
                        Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.EPICA),
                        Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.Issuer),
                        Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.ICC),
                        Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.ICCPIN),
                        Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.PKCSV1_5CA)),
                    KeyBlockImportFormats: new XFS4IoT.KeyManagement.CapabilitiesClass.KeyBlockImportFormatsClass(
                        Common.KeyManagementCapabilities.KeyBlockImportFormats.HasFlag(KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKA),
                        Common.KeyManagementCapabilities.KeyBlockImportFormats.HasFlag(KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKB),
                        Common.KeyManagementCapabilities.KeyBlockImportFormats.HasFlag(KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKC),
                        Common.KeyManagementCapabilities.KeyBlockImportFormats.HasFlag(KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKD)),
                    KeyImportThroughParts: Common.KeyManagementCapabilities.KeyImportThroughParts,
                    DesKeyLength: new XFS4IoT.KeyManagement.CapabilitiesClass.DesKeyLengthClass(
                        Common.KeyManagementCapabilities.DESKeyLength.HasFlag(KeyManagementCapabilitiesClass.DESKeyLengthEmum.Single),
                        Common.KeyManagementCapabilities.DESKeyLength.HasFlag(KeyManagementCapabilitiesClass.DESKeyLengthEmum.Double),
                        Common.KeyManagementCapabilities.DESKeyLength.HasFlag(KeyManagementCapabilitiesClass.DESKeyLengthEmum.Triple)),
                    CertificateTypes: new XFS4IoT.KeyManagement.CapabilitiesClass.CertificateTypesClass(
                        Common.KeyManagementCapabilities.CertificateTypes.HasFlag(KeyManagementCapabilitiesClass.CertificateTypeEnum.EncKey),
                        Common.KeyManagementCapabilities.CertificateTypes.HasFlag(KeyManagementCapabilitiesClass.CertificateTypeEnum.VerificationKey),
                        Common.KeyManagementCapabilities.CertificateTypes.HasFlag(KeyManagementCapabilitiesClass.CertificateTypeEnum.HostKey)),
                    LoadCertOptions: loadCertOptions,
                    CrklLoadOptions: new XFS4IoT.KeyManagement.CapabilitiesClass.CrklLoadOptionsClass(
                        Common.KeyManagementCapabilities.CRKLLoadOption.HasFlag(KeyManagementCapabilitiesClass.CRKLLoadOptionEnum.NoRandom),
                        Common.KeyManagementCapabilities.CRKLLoadOption.HasFlag(KeyManagementCapabilitiesClass.CRKLLoadOptionEnum.NoRandomCRL),
                        Common.KeyManagementCapabilities.CRKLLoadOption.HasFlag(KeyManagementCapabilitiesClass.CRKLLoadOptionEnum.Random),
                        Common.KeyManagementCapabilities.CRKLLoadOption.HasFlag(KeyManagementCapabilitiesClass.CRKLLoadOptionEnum.RandomCRL)),
                    SymmetricKeyManagementMethods: new XFS4IoT.KeyManagement.CapabilitiesClass.SymmetricKeyManagementMethodsClass(
                        Common.KeyManagementCapabilities.SymmetricKeyManagementMethods.HasFlag(KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum.FixedKey),
                        Common.KeyManagementCapabilities.SymmetricKeyManagementMethods.HasFlag(KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum.MasterKey),
                        Common.KeyManagementCapabilities.SymmetricKeyManagementMethods.HasFlag(KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum.TripleDESDUKPT)),
                    KeyAttributes: keyAttrib,
                    DecryptAttributes: decryptAttrib,
                    VerifyAttributes: verifyAttrib);
            }

            XFS4IoT.Keyboard.CapabilitiesClass keyboard = null;
            if (Common.KeyboardCapabilities is not null)
            {
                List<XFS4IoT.Keyboard.CapabilitiesClass.EtsCapsClass> etsCaps = null;
                if (Common.KeyboardCapabilities.ETSCaps?.Count > 0)
                {
                    etsCaps = new();
                    foreach (var etc in Common.KeyboardCapabilities.ETSCaps)
                    {
                        etsCaps.Add(new XFS4IoT.Keyboard.CapabilitiesClass.EtsCapsClass(
                            etc.XPos,
                            etc.YPos,
                            etc.XSize,
                            etc.YSize,
                            etc.MaximumTouchFrames,
                            etc.MaximumTouchKeys,
                            new XFS4IoT.Keyboard.CapabilitiesClass.EtsCapsClass.FloatClass(etc.FloatFlags.HasFlag(KeyboardCapabilitiesClass.ETSCap.FloatPositionEnum.FloatX),
                                                                                           etc.FloatFlags.HasFlag(KeyboardCapabilitiesClass.ETSCap.FloatPositionEnum.FloatY))
                            ));
                    }
                }

                keyboard = new XFS4IoT.Keyboard.CapabilitiesClass(
                    AutoBeep: new XFS4IoT.Keyboard.CapabilitiesClass.AutoBeepClass(Common.KeyboardCapabilities.AutoBeep.HasFlag(KeyboardCapabilitiesClass.KeyboardBeepEnum.ActiveAvailable),
                                                                                   Common.KeyboardCapabilities.AutoBeep.HasFlag(KeyboardCapabilitiesClass.KeyboardBeepEnum.ActiveSelectable),
                                                                                   Common.KeyboardCapabilities.AutoBeep.HasFlag(KeyboardCapabilitiesClass.KeyboardBeepEnum.InActiveAvailable),
                                                                                   Common.KeyboardCapabilities.AutoBeep.HasFlag(KeyboardCapabilitiesClass.KeyboardBeepEnum.InActiveSelectable)),
                    EtsCaps: etsCaps
                    );
            }

            XFS4IoT.PinPad.CapabilitiesClass pinPad = null;
            if (Common.PinPadCapabilities is not null)
            {
                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.PinPad.CapabilitiesClass.PinBlockAttributesClass>>> pinblockAttrib = new();

                if (Common.PinPadCapabilities.PinBlockAttributes?.Count > 0)
                {
                    pinblockAttrib = new();
                    foreach (var (keyUsage, algorithms) in Common.PinPadCapabilities.PinBlockAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.PinPad.CapabilitiesClass.PinBlockAttributesClass>> pinAlgorithms = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.PinPad.CapabilitiesClass.PinBlockAttributesClass> pinModeOfUse = new();
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                pinModeOfUse.Add(modeOfUse, new XFS4IoT.PinPad.CapabilitiesClass.PinBlockAttributesClass(
                                    new XFS4IoT.PinPad.CapabilitiesClass.PinBlockAttributesClass.CryptoMethodClass(
                                        method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.ECB),
                                        method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.CBC),
                                        method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.CFB),
                                        method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.OFB),
                                        method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.CTR),
                                        method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.XTS),
                                        method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.RSAES_PKCS1_V1_5),
                                        method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.RSAES_OAEP))
                                    ));
                            }
                            pinAlgorithms.Add(algorithm, pinModeOfUse);
                        }
                        pinblockAttrib.Add(keyUsage, pinAlgorithms);
                    }
                }

                pinPad = new XFS4IoT.PinPad.CapabilitiesClass(
                    PinFormats: new XFS4IoT.PinPad.CapabilitiesClass.PinFormatsClass(),
                    PresentationAlgorithms: new XFS4IoT.PinPad.CapabilitiesClass.PresentationAlgorithmsClass(),
                    Display: new XFS4IoT.PinPad.CapabilitiesClass.DisplayClass(),
                    IdcConnect: Common.PinPadCapabilities.IDConnect,
                    ValidationAlgorithms: new XFS4IoT.PinPad.CapabilitiesClass.ValidationAlgorithmsClass(),
                    PinCanPersistAfterUse: Common.PinPadCapabilities.PinCanPersistAfterUse,
                    TypeCombined: Common.PinPadCapabilities.TypeCombined,
                    SetPinblockDataRequired: Common.PinPadCapabilities.SetPinblockDataRequired,
                    PinBlockAttributes: pinblockAttrib
                    );
            }

            XFS4IoT.TextTerminal.CapabilitiesClass textTerminal = null;
            if (Common.TextTerminalCapabilities is not null)
            {
                List<XFS4IoT.TextTerminal.ResolutionClass> resolutions = null;
                if (Common.TextTerminalCapabilities.Resolutions?.Count > 0)
                {
                    resolutions = new();
                    foreach (var resolution in Common.TextTerminalCapabilities.Resolutions)
                    {
                        resolutions.Add(new XFS4IoT.TextTerminal.ResolutionClass(
                            resolution.Width,
                            resolution.Height
                            ));
                    }
                }

                textTerminal = new XFS4IoT.TextTerminal.CapabilitiesClass(
                    Type: Common.TextTerminalCapabilities.Type switch
                    {
                        TextTerminalCapabilitiesClass.TypeEnum.Fixed => XFS4IoT.TextTerminal.CapabilitiesClass.TypeEnum.Fixed,
                        _ => XFS4IoT.TextTerminal.CapabilitiesClass.TypeEnum.Removable
                    },
                    Resolutions: resolutions, 
                    KeyLock: Common.TextTerminalCapabilities.KeyLock,
                    Cursor: Common.TextTerminalCapabilities.Cursor,
                    Forms: Common.TextTerminalCapabilities.Forms
                    );
            }

            Dictionary<string, XFS4IoT.Lights.LightCapabilitiesClass> lights = null;
            if (Common.LightsCapabilities?.Lights?.Count > 0)
            {
                lights = new();
                foreach (var light in Common.LightsCapabilities.Lights)
                {
                    lights.Add(light.Key, new XFS4IoT.Lights.LightCapabilitiesClass(
                                            new XFS4IoT.Lights.LightCapabilitiesClass.FlashRateClass(light.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Off),
                                                                                                     light.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Slow),
                                                                                                     light.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Medium),
                                                                                                     light.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Quick),
                                                                                                     light.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Continuous)),
                                            new XFS4IoT.Lights.LightCapabilitiesClass.ColorClass(light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Red),
                                                                                                 light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Green),
                                                                                                 light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Yellow),
                                                                                                 light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Blue),
                                                                                                 light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Cyan),
                                                                                                 light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Magenta),
                                                                                                 light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.White)),
                                            new XFS4IoT.Lights.LightCapabilitiesClass.DirectionClass(light.Value.Direction.HasFlag(LightsCapabilitiesClass.DirectionEnum.Entry),
                                                                                                     light.Value.Direction.HasFlag(LightsCapabilitiesClass.DirectionEnum.Exit)),
                                            new XFS4IoT.Lights.LightCapabilitiesClass.PositionClass(light.Value.Position.HasFlag(LightsCapabilitiesClass.LightPostionEnum.Left),
                                                                                                    light.Value.Position.HasFlag(LightsCapabilitiesClass.LightPostionEnum.Right),
                                                                                                    light.Value.Position.HasFlag(LightsCapabilitiesClass.LightPostionEnum.Center),
                                                                                                    light.Value.Position.HasFlag(LightsCapabilitiesClass.LightPostionEnum.Top),
                                                                                                    light.Value.Position.HasFlag(LightsCapabilitiesClass.LightPostionEnum.Bottom),
                                                                                                    light.Value.Position.HasFlag(LightsCapabilitiesClass.LightPostionEnum.Front),
                                                                                                    light.Value.Position.HasFlag(LightsCapabilitiesClass.LightPostionEnum.Rear))


                                            ));
                }
            }

            return Task.FromResult(
                new CapabilitiesCompletion.PayloadData(
                    MessagePayload.CompletionCodeEnum.Success,
                    string.Empty,
                    Interfaces: interfaces,
                    Common: commonCapabilities,
                    CardReader: cardReader,
                    CashDispenser: cashDispenser,
                    CashManagement: cashManagement,
                    PinPad: pinPad,
                    Crypto: crypto,
                    KeyManagement: keyManagement,
                    Keyboard: keyboard,
                    TextTerminal: textTerminal,
                    Lights: lights)
                );  ;
        }
    }
}
