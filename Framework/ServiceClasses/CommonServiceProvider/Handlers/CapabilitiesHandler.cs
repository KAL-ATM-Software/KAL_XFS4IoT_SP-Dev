/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.Completions;
using XFS4IoT.Printer.Events;
using XFS4IoT.Lights;
using static XFS4IoTFramework.Common.LightsCapabilitiesClass;

namespace XFS4IoTFramework.Common
{
    [CommandHandlerAsync]
    public partial class CapabilitiesHandler
    {

        private Task<CommandResult<CapabilitiesCompletion.PayloadData>> HandleCapabilities(ICapabilitiesEvents events, CapabilitiesCommand capabilities, CancellationToken cancel)
        {
            if (Common.CommonCapabilities is null)
            {
                return Task.FromResult(
                    new CommandResult<CapabilitiesCompletion.PayloadData>(
                        MessageHeader.CompletionCodeEnum.InternalError,
                        $"No common capabilities is reported by the device class.")
                    );
            }

            List<InterfaceClass.NameEnum> supportedInterfaces = [];
            // Common interface must be supported for all services
            supportedInterfaces.Add(InterfaceClass.NameEnum.Common);

            // CardReader interface
            if (Common.CommonCapabilities.CardReaderInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.CardReader);
            }
            // CashDispenser interface
            if (Common.CommonCapabilities.CashDispenserInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.CashDispenser);
            }
            // CashManagement interface
            if (Common.CommonCapabilities.CashManagementInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.CashManagement);
            }
            // Crypto interface
            if (Common.CommonCapabilities.CryptoInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.Crypto);
            }
            // Keyboard interface
            if (Common.CommonCapabilities.KeyboardInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.Keyboard);
            }
            // KeyManagement interface
            if (Common.CommonCapabilities.KeyManagementInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.KeyManagement);
            }
            // PinPad interface
            if (Common.CommonCapabilities.PinPadInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.PinPad);
            }
            // TextTerminal interface
            if (Common.CommonCapabilities.TextTerminalInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.TextTerminal);
            }
            // Storage interface
            if (Common.CommonCapabilities.StorageInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.Storage);
            }
            // Printer interface
            if (Common.CommonCapabilities.PrinterInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.Printer);
            }
            // Lights interface
            if (Common.CommonCapabilities.LightsInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.Lights);
            }
            // Auxiliaries interface
            if (Common.CommonCapabilities.AuxiliariesInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.Auxiliaries);
            }
            // VendorMode interface
            if (Common.CommonCapabilities.VendorModeInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.VendorMode);
            }
            // VendorApplication interface
            if (Common.CommonCapabilities.VendorApplicationInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.VendorApplication);
            }
            // BarcodeReader interface
            if (Common.CommonCapabilities.BarcodeReaderInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.BarcodeReader);
            }
            // Biometric interface
            if (Common.CommonCapabilities.BiometricInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.Biometric);
            }
            // CashAcceptor interface
            if (Common.CommonCapabilities.CashAcceptorInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.CashAcceptor);
            }
            // Camera interface
            if (Common.CommonCapabilities.CameraInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.Camera);
            }
            // Check scanner interface
            if (Common.CommonCapabilities.CheckScannerInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.Check);
            }
            // Mixed media interface
            if (Common.CommonCapabilities.MixedMediaInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.MixedMedia);
            }
            // PowerManagement interface
            if (Common.CommonCapabilities.PowerManagementInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.PowerManagement);
            }
            // IBNS media interface
            if (Common.CommonCapabilities.BanknoteNeutralizationInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.BanknoteNeutralization);
            }
            // German specific interface
            if (Common.CommonCapabilities.GermanInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.German);
            }
            // Deposit interface
            if (Common.CommonCapabilities.DepositInterface is not null)
            {
                supportedInterfaces.Add(InterfaceClass.NameEnum.Deposit);
            }

            List<InterfaceClass> interfaces = [];
            interfaces.AddRange(from iface in supportedInterfaces
                                let devInterface = GetDeviceInterface(iface)
                                where devInterface is not null
                                select devInterface);

            List<DeviceInformationClass> deviceInformation = null;
            if (Common.CommonCapabilities.DeviceInformation?.Count > 0)
            {
                foreach (var device in Common.CommonCapabilities.DeviceInformation)
                {
                    List<FirmwareClass> firmware = null;
                    if (device.Firmware?.Count > 0)
                    {
                        foreach (var firm in device.Firmware)
                        {
                            (firmware ??= []).Add(
                                new FirmwareClass(
                                    FirmwareName: firm.FirmwareName,
                                    FirmwareVersion: firm.FirmwareVersion,
                                    HardwareRevision: firm.HardwareRevision));
                        }
                    }

                    List<SoftwareClass> software = null;
                    if (device.Software?.Count > 0)
                    {
                        foreach (var soft in device.Software)
                        {
                            (software ??= []).Add(
                                new SoftwareClass(
                                    SoftwareName: soft.SoftwareName,
                                    SoftwareVersion: soft.SoftwareVersion));
                        }
                    }

                    (deviceInformation ??= []).Add(new DeviceInformationClass(
                        ModelName: device.ModelName,
                        SerialNumber: device.SerialNumber,
                        RevisionNumber: device.RevisionNumber,
                        ModelDescription: device.ModelDescription,
                        Firmware: firmware,
                        Software: software
                        ));
                }
            }

            CapabilityPropertiesClass commonCapabilities = new(
                ServiceVersion: XFSConstants.ServiceVersion,
                DeviceInformation: deviceInformation,
                PowerSaveControl: Common.CommonCapabilities.PowerSaveControl,
                AntiFraudModule: Common.CommonCapabilities.AntiFraudModule,
                EndToEndSecurity: Common.CommonCapabilities.EndToEndSecurity is null ? null :
                    new EndToEndSecurityClass(
                        Required: Common.CommonCapabilities.EndToEndSecurity.Required switch
                        {
                            CommonCapabilitiesClass.EndToEndSecurityClass.RequiredEnum.Always => EndToEndSecurityClass.RequiredEnum.Always,
                            CommonCapabilitiesClass.EndToEndSecurityClass.RequiredEnum.IfConfigured => EndToEndSecurityClass.RequiredEnum.IfConfigured,
                            _ => null,
                        },
                        HardwareSecurityElement: Common.CommonCapabilities.EndToEndSecurity.HardwareSecurityElement,
                        ResponseSecurityEnabled: Common.CommonCapabilities.EndToEndSecurity.ResponseSecurityEnabled switch
                        {
                            CommonCapabilitiesClass.EndToEndSecurityClass.ResponseSecurityEnabledEnum.Always => EndToEndSecurityClass.ResponseSecurityEnabledEnum.Always,
                            CommonCapabilitiesClass.EndToEndSecurityClass.ResponseSecurityEnabledEnum.IfConfigured => EndToEndSecurityClass.ResponseSecurityEnabledEnum.IfConfigured,
                            _ => null,
                        },
                        Commands: null,
                        CommandNonceTimeout: Common.CommonCapabilities.EndToEndSecurity.CommandNonceTimeout
                        )
                    );

            XFS4IoT.CardReader.CapabilitiesClass cardReader = null;
            if (Common.CardReaderCapabilities is not null)
            {
                cardReader = new XFS4IoT.CardReader.CapabilitiesClass(
                    Type: Common.CardReaderCapabilities.Type switch
                    {
                        CardReaderCapabilitiesClass.DeviceTypeEnum.Contactless => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Contactless,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.Dip => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Dip,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.IntelligentContactless,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.LatchedDip => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.LatchedDip,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.Motor => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Motor,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.Permanent => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Permanent,
                        CardReaderCapabilitiesClass.DeviceTypeEnum.Swipe => XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Swipe,
                        _ => throw new InternalErrorException($"Unexpected card reader type specified {Common.CardReaderCapabilities.Type}"),
                    },
                    ReadTracks: Common.CardReaderCapabilities.ReadTracks == CardReaderCapabilitiesClass.ReadableDataTypesEnum.NotSupported ?
                    null : new(
                        Track1: Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1),
                        Track2: Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track2),
                        Track3: Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track3),
                        Watermark: Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Watermark),
                        FrontTrack1: Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1Front),
                        FrontImage: Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.FrontImage),
                        BackImage: Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.BackImage),
                        Track1JIS: Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1JIS),
                        Track3JIS: Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track3JIS),
                        Ddi: Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Ddi)
                        ),
                    WriteTracks: Common.CardReaderCapabilities.WriteTracks == CardReaderCapabilitiesClass.WritableDataTypesEnum.NotSupported ?
                    null : new(
                        Track1: Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1),
                        Track2: Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track2),
                        Track3: Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3),
                        FrontTrack1: Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1Front),
                        Track1JIS: Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1JIS),
                        Track3JIS: Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3JIS)
                        ),
                    ChipProtocols: Common.CardReaderCapabilities.ChipProtocols == CardReaderCapabilitiesClass.ChipProtocolsEnum.NotSupported ?
                    null : new(
                        ChipT0: Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.T0),
                        ChipT1: Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.T1),
                        ChipProtocolNotRequired: Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.NotRequired),
                        ChipTypeAPart3: Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeAPart3),
                        ChipTypeAPart4: Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeAPart4),
                        ChipTypeB: Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeB),
                        ChipTypeNFC: Common.CardReaderCapabilities.ChipProtocols.HasFlag(CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeNFC)
                        ),
                    SecurityType: Common.CardReaderCapabilities.SecurityType switch
                    {
                        CardReaderCapabilitiesClass.SecurityTypeEnum.Cim86 => XFS4IoT.CardReader.CapabilitiesClass.SecurityTypeEnum.Cim86,
                        CardReaderCapabilitiesClass.SecurityTypeEnum.Mm => XFS4IoT.CardReader.CapabilitiesClass.SecurityTypeEnum.Mm,
                        _ => null,
                    },
                    PowerOnOption: Common.CardReaderCapabilities.PowerOnOption switch
                    {
                        CardReaderCapabilitiesClass.PowerOptionEnum.Exit => XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.Exit,
                        CardReaderCapabilitiesClass.PowerOptionEnum.ExitThenRetain => XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.ExitThenRetain,
                        CardReaderCapabilitiesClass.PowerOptionEnum.Retain => XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.Retain,
                        CardReaderCapabilitiesClass.PowerOptionEnum.Transport => XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.Transport,
                        _ => null,
                    },
                    PowerOffOption: Common.CardReaderCapabilities.PowerOffOption switch
                    {
                        CardReaderCapabilitiesClass.PowerOptionEnum.Exit => XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.Exit,
                        CardReaderCapabilitiesClass.PowerOptionEnum.ExitThenRetain => XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.ExitThenRetain,
                        CardReaderCapabilitiesClass.PowerOptionEnum.Retain => XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.Retain,
                        CardReaderCapabilitiesClass.PowerOptionEnum.Transport => XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.Transport,
                        _ => null,
                    },
                    Common.CardReaderCapabilities.FluxSensorProgrammable,
                    Common.CardReaderCapabilities.ReadWriteAccessFollowingExit,
                    WriteMode: Common.CardReaderCapabilities.WriteMode == CardReaderCapabilitiesClass.WriteMethodsEnum.NotSupported ?
                    null : new(
                        Loco: Common.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Loco),
                        Hico: Common.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Hico),
                        Auto: Common.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Auto)),
                    ChipPower: Common.CardReaderCapabilities.ChipPower == CardReaderCapabilitiesClass.ChipPowerOptionsEnum.NotSupported ?
                    null : new(
                        Cold: Common.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Cold),
                        Warm: Common.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Warm),
                        Off: Common.CardReaderCapabilities.ChipPower.HasFlag(CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Off)),
                    MemoryChipProtocols: Common.CardReaderCapabilities.MemoryChipProtocols == CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.NotSupported ?
                    null : new(
                        Siemens4442: Common.CardReaderCapabilities.MemoryChipProtocols.HasFlag(CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.Siemens4442),
                        Gpm896: Common.CardReaderCapabilities.MemoryChipProtocols.HasFlag(CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.Gpm896)),
                    Positions: Common.CardReaderCapabilities.Positions == CardReaderCapabilitiesClass.PositionsEnum.NotSupported ?
                    null : new(
                        Exit: Common.CardReaderCapabilities.Positions.HasFlag(CardReaderCapabilitiesClass.PositionsEnum.Exit),
                        Transport: Common.CardReaderCapabilities.Positions.HasFlag(CardReaderCapabilitiesClass.PositionsEnum.Transport)),
                    CardTakenSensor: Common.CardReaderCapabilities.CardTakenSensor
                    );
            }

            XFS4IoT.CashDispenser.CapabilitiesClass cashDispenser = null;
            if (Common.CashDispenserCapabilities is not null)
            {
                cashDispenser = new XFS4IoT.CashDispenser.CapabilitiesClass(
                    Type: Common.CashDispenserCapabilities.Type switch
                    {
                        CashManagementCapabilitiesClass.TypeEnum.SelfServiceBill => XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.SelfServiceBill,
                        CashManagementCapabilitiesClass.TypeEnum.SelfServiceCoin => XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.SelfServiceCoin,
                        CashManagementCapabilitiesClass.TypeEnum.TellerBill => XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.TellerBill,
                        CashManagementCapabilitiesClass.TypeEnum.TellerCoin => XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.TellerCoin,
                        _ => throw new InternalErrorException($"Unexpected cash dispenser type specified. {Common.CashDispenserCapabilities.Type}"),
                    },
                    MaxDispenseItems: Common.CashDispenserCapabilities.MaxDispenseItems,
                    ShutterControl: Common.CashDispenserCapabilities.ShutterControl,
                    RetractAreas: Common.CashDispenserCapabilities.RetractAreas == CashManagementCapabilitiesClass.RetractAreaEnum.Default ?
                    null : new(
                        Retract: Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Retract),
                        Transport: Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Transport),
                        Stacker: Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Stacker),
                        Reject: Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Reject),
                        ItemCassette: Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette),
                        CashIn: Common.CashDispenserCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.CashIn)
                        ),
                    RetractTransportActions: Common.CashDispenserCapabilities.RetractTransportActions == CashManagementCapabilitiesClass.RetractTransportActionEnum.NotSupported ?
                    null : new(
                        Present: Common.CashDispenserCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.Present),
                        Retract: Common.CashDispenserCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.Retract),
                        Reject: Common.CashDispenserCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.Reject),
                        ItemCassette: Common.CashDispenserCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.BillCassette),
                        CashIn: Common.CashDispenserCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.CashIn)
                        ),
                RetractStackerActions: Common.CashDispenserCapabilities.RetractStackerActions == CashManagementCapabilitiesClass.RetractStackerActionEnum.NotSupported ?
                    null : new(
                        Present: Common.CashDispenserCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.Present),
                        Retract: Common.CashDispenserCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.Retract),
                        Reject: Common.CashDispenserCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.Reject),
                        ItemCassette: Common.CashDispenserCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.BillCassette),
                        CashIn: Common.CashDispenserCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.CashIn)
                        ),
                    IntermediateStacker: Common.CashDispenserCapabilities.IntermediateStacker,
                    ItemsTakenSensor: Common.CashDispenserCapabilities.ItemsTakenSensor,
                    Positions: Common.CashDispenserCapabilities.OutputPositions == CashManagementCapabilitiesClass.OutputPositionEnum.NotSupported ?
                    null : new(
                        Left: Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashManagementCapabilitiesClass.OutputPositionEnum.Left),
                        Right: Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashManagementCapabilitiesClass.OutputPositionEnum.Right),
                        Center: Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashManagementCapabilitiesClass.OutputPositionEnum.Center),
                        Top: Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashManagementCapabilitiesClass.OutputPositionEnum.Top),
                        Bottom: Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashManagementCapabilitiesClass.OutputPositionEnum.Bottom),
                        Front: Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashManagementCapabilitiesClass.OutputPositionEnum.Front),
                        Rear: Common.CashDispenserCapabilities.OutputPositions.HasFlag(CashManagementCapabilitiesClass.OutputPositionEnum.Rear)
                        ),
                    MoveItems: Common.CashDispenserCapabilities.MoveItems == CashDispenserCapabilitiesClass.MoveItemEnum.NotSupported ?
                    null : new(
                        FromCashUnit: Common.CashDispenserCapabilities.MoveItems.HasFlag(CashDispenserCapabilitiesClass.MoveItemEnum.FromCashUnit),
                        ToCashUnit: Common.CashDispenserCapabilities.MoveItems.HasFlag(CashDispenserCapabilitiesClass.MoveItemEnum.ToCashUnit),
                        ToTransport: Common.CashDispenserCapabilities.MoveItems.HasFlag(CashDispenserCapabilitiesClass.MoveItemEnum.ToTransport),
                        ToStacker: Common.CashDispenserCapabilities.MoveItems.HasFlag(CashDispenserCapabilitiesClass.MoveItemEnum.ToStacker)
                        )
                    );
            }

            XFS4IoT.CashManagement.CapabilitiesClass cashManagement = null;
            if (Common.CashManagementCapabilities is not null)
            {
                cashManagement = new(
                    CashBox: Common.CashManagementCapabilities.CashBox,
                    ExchangeType: Common.CashManagementCapabilities.ExchangeTypes == CashManagementCapabilitiesClass.ExchangeTypesEnum.NotSupported ?
                    null : new(
                        ByHand: Common.CashManagementCapabilities.ExchangeTypes.HasFlag(CashManagementCapabilitiesClass.ExchangeTypesEnum.ByHand)
                        ),
                        ItemInfoTypes: Common.CashManagementCapabilities.ItemInfoTypes == CashManagementCapabilitiesClass.ItemInfoTypesEnum.NotSupported ?
                    null : new(
                        SerialNumber: Common.CashManagementCapabilities.ItemInfoTypes.HasFlag(CashManagementCapabilitiesClass.ItemInfoTypesEnum.SerialNumber),
                        Signature: Common.CashManagementCapabilities.ItemInfoTypes.HasFlag(CashManagementCapabilitiesClass.ItemInfoTypesEnum.Signature),
                        Image: Common.CashManagementCapabilities.ItemInfoTypes.HasFlag(CashManagementCapabilitiesClass.ItemInfoTypesEnum.ImageFile)
                        ),
                    ClassificationList: Common.CashManagementCapabilities.ClassificationList);
            }

            XFS4IoT.Crypto.CapabilitiesClass crypto = null;
            if (Common.CryptoCapabilities is not null)
            {
                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.CryptoAttributesClass>>> cryptoAttrib = null;
                if (Common.CryptoCapabilities.CryptoAttributes?.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in Common.CryptoCapabilities.CryptoAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.CryptoAttributesClass>> dicAttributes = null;
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.CryptoAttributesClass> dicModeOfUse = null;
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                (dicModeOfUse ??= []).Add(
                                    key: modeOfUse,
                                    value: new(
                                        new(Ecb: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB),
                                            Cbc: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CBC),
                                            Cfb: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CFB),
                                            Ofb: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.OFB),
                                            Ctr: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CTR),
                                            Xts: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.XTS),
                                            RsaesPkcs1V15: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.RSAES_PKCS1_V1_5),
                                            RsaesOaep: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.RSAES_OAEP))
                                    ));
                            }
                            (dicAttributes ??= []).Add(algorithm, dicModeOfUse);
                        }
                        (cryptoAttrib ??= []).Add(keyUsage, dicAttributes);
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.AuthenticationAttributesClass>>> authAttrib = null;
                if (Common.CryptoCapabilities.AuthenticationAttributes?.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in Common.CryptoCapabilities.AuthenticationAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.AuthenticationAttributesClass>> dicAttributes = null;
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.AuthenticationAttributesClass> dicModeOfUse = null;
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                (dicModeOfUse ??= []).Add(
                                    key: modeOfUse,
                                    value: new(
                                        CryptoMethod: new(
                                            RsassaPkcs1V15: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5),
                                            RsassaPss: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PSS)),
                                        HashAlgorithm: new(
                                            Sha1: method?.HashAlgorithms.HasFlag(CryptoCapabilitiesClass.HashAlgorithmEnum.SHA1),
                                            Sha256: method?.HashAlgorithms.HasFlag(CryptoCapabilitiesClass.HashAlgorithmEnum.SHA256))
                                    ));
                            }
                            (dicAttributes ??= []).Add(algorithm, dicModeOfUse);
                        }
                        (authAttrib ??= []).Add(keyUsage, dicAttributes);
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.VerifyAttributesClass>>> verifyAttrib = null;
                if (Common.CryptoCapabilities.VerifyAttributes?.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in Common.CryptoCapabilities.VerifyAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.VerifyAttributesClass>> dicAttributes = null;
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.Crypto.CapabilitiesClass.VerifyAttributesClass> dicModeOfUse = null;
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                (dicModeOfUse ??= []).Add(
                                    key: modeOfUse,
                                    value: new(
                                        CryptoMethod: new(
                                            RsassaPkcs1V15: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5),
                                            RsassaPss: method?.CryptoMethods.HasFlag(CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PSS)),
                                        HashAlgorithm: new(
                                            Sha1: method?.HashAlgorithms.HasFlag(CryptoCapabilitiesClass.HashAlgorithmEnum.SHA1),
                                            Sha256: method?.HashAlgorithms.HasFlag(CryptoCapabilitiesClass.HashAlgorithmEnum.SHA256))
                                    ));
                            }
                            (dicAttributes ??= []).Add(algorithm, dicModeOfUse);
                        }
                        (verifyAttrib ??= []).Add(keyUsage, dicAttributes);
                    }
                }

                crypto = new XFS4IoT.Crypto.CapabilitiesClass(
                    EmvHashAlgorithm: new(
                        Sha1Digest: Common.CryptoCapabilities.EMVHashAlgorithms.HasFlag(CryptoCapabilitiesClass.EMVHashAlgorithmEnum.SHA1_Digest),
                        Sha256Digest: Common.CryptoCapabilities.EMVHashAlgorithms.HasFlag(CryptoCapabilitiesClass.EMVHashAlgorithmEnum.SHA256_Digest)),
                    CryptoAttributes: cryptoAttrib,
                    AuthenticationAttributes: authAttrib,
                    VerifyAttributes: verifyAttrib);
            }

            XFS4IoT.KeyManagement.CapabilitiesClass keyManagement = null;
            if (Common.KeyManagementCapabilities is not null)
            {
                Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass> loadCertOptions = null;
                if (Common.KeyManagementCapabilities.LoadCertificationOptions?.Count > 0)
                {
                    loadCertOptions = null;
                    foreach (var certOption in Common.KeyManagementCapabilities.LoadCertificationOptions)
                    {
                        if (certOption.Signer == KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.NotSupported)
                        {
                            continue;
                        }
                        (loadCertOptions ??= []).Add(
                            key: certOption.Signer switch
                            {
                                KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CertHost => "certHost",
                                KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.SigHost => "sigHost",
                                KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.HL => "hl",
                                KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CertHost_TR34 => "certHostTr34",
                                KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CA_TR34 => "caTr34",
                                KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.HL_TR34 => "hlTr34",
                                _ => throw new InternalErrorException($"Unexpected signer for the certification option is specified. {certOption.Signer}"),
                            },
                            value: new(
                                NewHost: certOption.Option.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateOptionEnum.NewHost),
                                ReplaceHost: certOption.Option.HasFlag(KeyManagementCapabilitiesClass.LoadCertificateOptionEnum.ReplaceHost)
                            ));
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.KeyAttributesClass>>> keyAttrib = null;
                if (Common.KeyManagementCapabilities.KeyAttributes?.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in Common.KeyManagementCapabilities.KeyAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.KeyAttributesClass>> dicAttributes = null;
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.KeyAttributesClass> dicModeOfUse = null;
                            foreach (var (modeOfUse, restrict) in modeOfUses)
                            {
                                (dicModeOfUse ??= []).Add(
                                    key: modeOfUse,
                                    value: new(RestrictedKeyUsage: restrict?.Restricted)
                                    );
                            }
                            (dicAttributes ??= []).Add(algorithm, dicModeOfUse);
                        }
                        (keyAttrib ??= []).Add(keyUsage, dicAttributes);
                    }
                }

                Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.DecryptAttributesClass> decryptAttrib = null;
                if (Common.KeyManagementCapabilities.DecryptAttributes?.Count > 0)
                {
                    foreach (var (algorithm, method) in Common.KeyManagementCapabilities.DecryptAttributes)
                    {
                        (decryptAttrib ??= []).Add(
                            key: algorithm,
                            value: new(
                                DecryptMethod: new(
                                    Ecb: method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.ECB),
                                    Cbc: method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CBC),
                                    Cfb: method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CFB),
                                    Ofb: method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.OFB),
                                    Ctr: method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CTR),
                                    Xts: method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.XTS),
                                    RsaesPkcs1V15: method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.RSAES_PKCS1_V1_5),
                                    RsaesOaep: method?.DecryptMethods.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.RSAES_OAEP))
                                ));
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.VerifyAttributesClass>>> verifyAttrib = null;
                if (Common.KeyManagementCapabilities.VerifyAttributes?.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in Common.KeyManagementCapabilities.VerifyAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.VerifyAttributesClass>> dicAttributes = null;
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.KeyManagement.CapabilitiesClass.VerifyAttributesClass> dicModeOfUse = null;
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                (dicModeOfUse ??= []).Add(
                                    key: modeOfUse,
                                    value: new(
                                        CryptoMethod: new(
                                            KcvNone: method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVNone),
                                            KcvSelf: method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVSelf),
                                            KcvZero: method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVZero),
                                            SigNone: method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.SignatureNone),
                                            RsassaPkcs1V15: method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.RSASSA_PKCS1_V1_5),
                                            RsassaPss: method?.CryptoMethod.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.RSASSA_PSS)),
                                        HashAlgorithm: new(
                                            Sha1: method?.HashAlgorithm.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum.SHA1),
                                            Sha256: method?.HashAlgorithm.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum.SHA256))
                                        ));
                            }
                            (dicAttributes ??= []).Add(algorithm, dicModeOfUse);
                        }
                        (verifyAttrib ??= []).Add(keyUsage, dicAttributes);
                    }
                }

                keyManagement = new XFS4IoT.KeyManagement.CapabilitiesClass(
                    KeyNum: Common.KeyManagementCapabilities.MaxKeys,
                    KeyCheckModes: new(
                        Self: Common.KeyManagementCapabilities.KeyCheckModes.HasFlag(KeyManagementCapabilitiesClass.KeyCheckModeEnum.Self),
                        Zero: Common.KeyManagementCapabilities.KeyCheckModes.HasFlag(KeyManagementCapabilitiesClass.KeyCheckModeEnum.Zero)),
                    RsaAuthenticationScheme: new(
                        TwoPartySig: Common.KeyManagementCapabilities.RSAAuthenticationScheme.HasFlag(KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum.SecondPartySignature),
                        ThreePartyCert: Common.KeyManagementCapabilities.RSAAuthenticationScheme.HasFlag(KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum.ThirdPartyCertificate),
                        ThreePartyCertTr34: Common.KeyManagementCapabilities.RSAAuthenticationScheme.HasFlag(KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum.ThirdPartyCertificateTR34)),
                    RsaSignatureAlgorithm: new(
                        Pkcs1V15: Common.KeyManagementCapabilities.RSASignatureAlgorithm.HasFlag(KeyManagementCapabilitiesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5),
                        Pss: Common.KeyManagementCapabilities.RSASignatureAlgorithm.HasFlag(KeyManagementCapabilitiesClass.RSASignatureAlgorithmEnum.RSASSA_PSS)),
                    RsaCryptAlgorithm: new(
                        Pkcs1V15: Common.KeyManagementCapabilities.RSACryptAlgorithm.HasFlag(KeyManagementCapabilitiesClass.RSACryptAlgorithmEnum.RSAES_PKCS1_V1_5),
                        Oaep: Common.KeyManagementCapabilities.RSACryptAlgorithm.HasFlag(KeyManagementCapabilitiesClass.RSACryptAlgorithmEnum.RSAES_OAEP)),
                    RsaKeyCheckMode: new(
                        Sha1: Common.KeyManagementCapabilities.RSAKeyCheckMode.HasFlag(KeyManagementCapabilitiesClass.RSAKeyCheckModeEnum.SHA1),
                        Sha256: Common.KeyManagementCapabilities.RSAKeyCheckMode.HasFlag(KeyManagementCapabilitiesClass.RSAKeyCheckModeEnum.SHA256)),
                    SignatureScheme: new(
                        RandomNumber: Common.KeyManagementCapabilities.SignatureScheme.HasFlag(KeyManagementCapabilitiesClass.SignatureSchemeEnum.RandomNumber),
                        ExportDeviceId: Common.KeyManagementCapabilities.SignatureScheme.HasFlag(KeyManagementCapabilitiesClass.SignatureSchemeEnum.ExportEPPID),
                        EnhancedRkl: Common.KeyManagementCapabilities.SignatureScheme.HasFlag(KeyManagementCapabilitiesClass.SignatureSchemeEnum.EnhancedRKL)),
                    EmvImportSchemes: new(
                        PlainCA: Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.PlainCA),
                        ChksumCA: Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.ChecksumCA),
                        EpiCA: Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.EPICA),
                        Issuer: Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.Issuer),
                        Icc: Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.ICC),
                        IccPin: Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.ICCPIN),
                        Pkcsv15CA: Common.KeyManagementCapabilities.EMVImportSchemes.HasFlag(KeyManagementCapabilitiesClass.EMVImportSchemeEnum.PKCSV1_5CA)),
                    KeyBlockImportFormats: new(
                        A: Common.KeyManagementCapabilities.KeyBlockImportFormats.HasFlag(KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKA),
                        B: Common.KeyManagementCapabilities.KeyBlockImportFormats.HasFlag(KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKB),
                        C: Common.KeyManagementCapabilities.KeyBlockImportFormats.HasFlag(KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKC),
                        D: Common.KeyManagementCapabilities.KeyBlockImportFormats.HasFlag(KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKD)),
                    KeyImportThroughParts: Common.KeyManagementCapabilities.KeyImportThroughParts,
                    DesKeyLength: new(
                        Single: Common.KeyManagementCapabilities.DESKeyLength.HasFlag(KeyManagementCapabilitiesClass.DESKeyLengthEmum.Single),
                        Double: Common.KeyManagementCapabilities.DESKeyLength.HasFlag(KeyManagementCapabilitiesClass.DESKeyLengthEmum.Double),
                        Triple: Common.KeyManagementCapabilities.DESKeyLength.HasFlag(KeyManagementCapabilitiesClass.DESKeyLengthEmum.Triple)),
                    CertificateTypes: new(
                        EncKey: Common.KeyManagementCapabilities.CertificateTypes.HasFlag(KeyManagementCapabilitiesClass.CertificateTypeEnum.EncKey),
                        VerificationKey: Common.KeyManagementCapabilities.CertificateTypes.HasFlag(KeyManagementCapabilitiesClass.CertificateTypeEnum.VerificationKey),
                        HostKey: Common.KeyManagementCapabilities.CertificateTypes.HasFlag(KeyManagementCapabilitiesClass.CertificateTypeEnum.HostKey)),
                    LoadCertOptions: loadCertOptions,
                    CrklLoadOptions: new(
                        NoRandom: Common.KeyManagementCapabilities.CRKLLoadOption.HasFlag(KeyManagementCapabilitiesClass.CRKLLoadOptionEnum.NoRandom),
                        NoRandomCrl: Common.KeyManagementCapabilities.CRKLLoadOption.HasFlag(KeyManagementCapabilitiesClass.CRKLLoadOptionEnum.NoRandomCRL),
                        Random: Common.KeyManagementCapabilities.CRKLLoadOption.HasFlag(KeyManagementCapabilitiesClass.CRKLLoadOptionEnum.Random),
                        RandomCrl: Common.KeyManagementCapabilities.CRKLLoadOption.HasFlag(KeyManagementCapabilitiesClass.CRKLLoadOptionEnum.RandomCRL)),
                    SymmetricKeyManagementMethods: new(
                        FixedKey: Common.KeyManagementCapabilities.SymmetricKeyManagementMethods.HasFlag(KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum.FixedKey),
                        MasterKey: Common.KeyManagementCapabilities.SymmetricKeyManagementMethods.HasFlag(KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum.MasterKey),
                        TdesDukpt: Common.KeyManagementCapabilities.SymmetricKeyManagementMethods.HasFlag(KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum.TripleDESDUKPT)),
                    KeyAttributes: keyAttrib,
                    DecryptAttributes: decryptAttrib,
                    VerifyAttributes: verifyAttrib);
            }

            XFS4IoT.Keyboard.CapabilitiesClass keyboard = null;
            if (Common.KeyboardCapabilities is not null)
            {
                XFS4IoT.Keyboard.CapabilitiesClass.EtsCapsClass etsCap = null;
                if (Common.KeyboardCapabilities.ETSCaps is not null)
                {
                    etsCap = new(
                        XPos: Common.KeyboardCapabilities.ETSCaps.XPos,
                        YPos: Common.KeyboardCapabilities.ETSCaps.YPos,
                        XSize: Common.KeyboardCapabilities.ETSCaps.XSize,
                        YSize: Common.KeyboardCapabilities.ETSCaps.YSize,
                        MaximumTouchFrames: Common.KeyboardCapabilities.ETSCaps.MaximumTouchFrames,
                        MaximumTouchKeys: Common.KeyboardCapabilities.ETSCaps.MaximumTouchKeys,
                        Float: new(
                            X: Common.KeyboardCapabilities.ETSCaps.FloatFlags.HasFlag(KeyboardCapabilitiesClass.ETSCap.FloatPositionEnum.FloatX),
                            Y: Common.KeyboardCapabilities.ETSCaps.FloatFlags.HasFlag(KeyboardCapabilitiesClass.ETSCap.FloatPositionEnum.FloatY))
                        );
                }

                keyboard = new XFS4IoT.Keyboard.CapabilitiesClass(
                    AutoBeep: new(
                        ActiveAvailable: Common.KeyboardCapabilities.AutoBeep.HasFlag(KeyboardCapabilitiesClass.KeyboardBeepEnum.ActiveAvailable),
                        ActiveSelectable: Common.KeyboardCapabilities.AutoBeep.HasFlag(KeyboardCapabilitiesClass.KeyboardBeepEnum.ActiveSelectable),
                        InactiveAvailable: Common.KeyboardCapabilities.AutoBeep.HasFlag(KeyboardCapabilitiesClass.KeyboardBeepEnum.InActiveAvailable),
                        InactiveSelectable: Common.KeyboardCapabilities.AutoBeep.HasFlag(KeyboardCapabilitiesClass.KeyboardBeepEnum.InActiveSelectable)),
                    EtsCaps: etsCap
                    );
            }

            XFS4IoT.PinPad.CapabilitiesClass pinPad = null;
            if (Common.PinPadCapabilities is not null)
            {
                Dictionary<string, Dictionary<string, Dictionary<string, XFS4IoT.PinPad.CapabilitiesClass.PinBlockAttributesClass>>> pinblockAttrib = null;

                if (Common.PinPadCapabilities.PinBlockAttributes?.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in Common.PinPadCapabilities.PinBlockAttributes)
                    {
                        Dictionary<string, Dictionary<string, XFS4IoT.PinPad.CapabilitiesClass.PinBlockAttributesClass>> pinAlgorithms = null;
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, XFS4IoT.PinPad.CapabilitiesClass.PinBlockAttributesClass> pinModeOfUse = null;
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                (pinModeOfUse ??= []).Add(
                                    key: modeOfUse,
                                    value: new(
                                        CryptoMethod: new(
                                            Ecb: method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.ECB),
                                            Cbc: method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.CBC),
                                            Cfb: method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.CFB),
                                            Ofb: method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.OFB),
                                            Ctr: method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.CTR),
                                            Xts: method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.XTS),
                                            RsaesPkcs1V15: method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.RSAES_PKCS1_V1_5),
                                            RsaesOaep: method?.EncryptionAlgorithm.HasFlag(PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.RSAES_OAEP))
                                    ));
                            }
                            (pinAlgorithms ??= []).Add(algorithm, pinModeOfUse);
                        }
                        (pinblockAttrib ??= []).Add(keyUsage, pinAlgorithms);
                    }
                }

                pinPad = new XFS4IoT.PinPad.CapabilitiesClass(
                    PinFormats: new(
                        Ibm3624: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.IBM3624),
                        Ansi: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.ANSI),
                        Iso0: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.ISO0),
                        Iso1: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.ISO1),
                        Eci2: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.ECI2),
                        Eci3: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.ECI3),
                        Visa: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.VISA),
                        Diebold: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.DIEBOLD),
                        DieboldCo: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.DIEBOLDCO),
                        Visa3: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.VISA3),
                        Banksys: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.BANKSYS),
                        Emv: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.EMV),
                        Iso3: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.ISO3),
                        Ap: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.AP),
                        Iso4: Common.PinPadCapabilities.PINFormat.HasFlag(PinPadCapabilitiesClass.PINFormatEnum.ISO4)
                        ),
                    PresentationAlgorithms: new(
                        PresentClear: Common.PinPadCapabilities.PresentationAlgorithm.HasFlag(PinPadCapabilitiesClass.PresentationAlgorithmEnum.PresentClear)
                        ),
                    Display: new(
                        None: Common.PinPadCapabilities.DisplayType == PinPadCapabilitiesClass.DisplayTypeEnum.NotSupported,
                        LedThrough: Common.PinPadCapabilities.DisplayType.HasFlag(PinPadCapabilitiesClass.DisplayTypeEnum.LEDThrough),
                        Display: Common.PinPadCapabilities.DisplayType.HasFlag(PinPadCapabilitiesClass.DisplayTypeEnum.Display)
                        ),
                    IdcConnect: Common.PinPadCapabilities.IDConnect,
                    ValidationAlgorithms: new(
                        Des: Common.PinPadCapabilities.ValidationAlgorithm.HasFlag(PinPadCapabilitiesClass.ValidationAlgorithmEnum.DES),
                        Visa: Common.PinPadCapabilities.ValidationAlgorithm.HasFlag(PinPadCapabilitiesClass.ValidationAlgorithmEnum.VISA)
                        ),
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
                    foreach (var resolution in Common.TextTerminalCapabilities.Resolutions)
                    {
                        (resolutions ??= []).Add(new(
                            SizeX: resolution.Width,
                            SizeY: resolution.Height)
                            );
                    }
                }

                textTerminal = new XFS4IoT.TextTerminal.CapabilitiesClass(
                    Type: Common.TextTerminalCapabilities.Type switch
                    {
                        TextTerminalCapabilitiesClass.TypeEnum.Fixed => XFS4IoT.TextTerminal.CapabilitiesClass.TypeEnum.Fixed,
                        TextTerminalCapabilitiesClass.TypeEnum.Removable => XFS4IoT.TextTerminal.CapabilitiesClass.TypeEnum.Removable,
                        _ => null
                    },
                    Resolutions: resolutions,
                    KeyLock: Common.TextTerminalCapabilities.KeyLock,
                    Cursor: Common.TextTerminalCapabilities.Cursor,
                    Forms: Common.TextTerminalCapabilities.Forms
                    );
            }

            XFS4IoT.Lights.CapabilitiesClass lights = null;
            if (Common.LightsCapabilities?.Lights is not null ||
                Common.LightsCapabilities?.CustomLights is not null)
            {
                Dictionary<string, PositionCapsClass> cardReaderLight = null;
                Dictionary<string, PositionCapsClass> pinPadLight = null;
                Dictionary<string, PositionCapsClass> notesDispenserLight = null;
                Dictionary<string, PositionCapsClass> coinDispenserLight = null;
                Dictionary<string, PositionCapsClass> receiptPrinterLight = null;
                Dictionary<string, PositionCapsClass> passbookPrinterLight = null;
                Dictionary<string, PositionCapsClass> envelopeDepositoryLight = null;
                Dictionary<string, PositionCapsClass> checkUnitLight = null;
                Dictionary<string, PositionCapsClass> billAcceptorLight = null;
                Dictionary<string, PositionCapsClass> envelopeDispenserLight = null;
                Dictionary<string, PositionCapsClass> documentPrinterLight = null;
                Dictionary<string, PositionCapsClass> coinAcceptorLight = null;
                Dictionary<string, PositionCapsClass> scannerLight = null;
                Dictionary<string, PositionCapsClass> contactlessLight = null;
                Dictionary<string, PositionCapsClass> cardReader2Light = null;
                Dictionary<string, PositionCapsClass> notesDispenser2Light = null;
                Dictionary<string, PositionCapsClass> billAcceptor2Light = null;
                Dictionary<string, PositionCapsClass> statusGoodLight = null;
                Dictionary<string, PositionCapsClass> statusWarningLight = null;
                Dictionary<string, PositionCapsClass> statusBadLight = null;
                Dictionary<string, PositionCapsClass> statusSupervisorLight = null;
                Dictionary<string, PositionCapsClass> statusInServiceLight = null;
                Dictionary<string, PositionCapsClass> fasciaLight = null;

                if (Common.LightsCapabilities?.Lights is not null &&
                    Common.LightsCapabilities.Lights.Count > 0)
                {
                    foreach (var light in Common.LightsCapabilities.Lights)
                    {
                        PositionCapsClass lightCapabilities = new(
                            FlashRate: new(
                                Off: light.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Off),
                                Slow: light.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Slow),
                                Medium: light.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Medium),
                                Quick: light.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Quick),
                                Continuous: light.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Continuous)),
                            Color: light.Value.Color == LightsCapabilitiesClass.ColorEnum.Default ? null : new(
                                Red: light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Red),
                                Green: light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Green),
                                Yellow: light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Yellow),
                                Blue: light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Blue),
                                Cyan: light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Cyan),
                                Magenta: light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Magenta),
                                White: light.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.White)),
                            Direction: light.Value.Direction == LightsCapabilitiesClass.DirectionEnum.NotSupported ? null : new(
                                Entry: light.Value.Direction.HasFlag(LightsCapabilitiesClass.DirectionEnum.Entry),
                                Exit: light.Value.Direction.HasFlag(LightsCapabilitiesClass.DirectionEnum.Exit))
                            );

                        string lightPositionName = light.Value.Position.ToString().ToCamelCase();

                        Dictionary<string, PositionCapsClass> thisLight = new()
                        {
                            { lightPositionName, lightCapabilities }
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
                if (Common.LightsCapabilities?.CustomLights is not null &&
                    Common.LightsCapabilities.CustomLights.Count > 0)
                {
                    foreach (var custom in Common.LightsCapabilities.CustomLights)
                    {
                        PositionCapsClass lightCapabilities = new(
                        FlashRate: new(
                            Off: custom.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Off),
                            Slow: custom.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Slow),
                            Medium: custom.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Medium),
                            Quick: custom.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Quick),
                            Continuous: custom.Value.FlashRate.HasFlag(LightsCapabilitiesClass.FlashRateEnum.Continuous)),
                        Color: custom.Value.Color == LightsCapabilitiesClass.ColorEnum.Default ? null : new(
                            Red: custom.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Red),
                            Green: custom.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Green),
                            Yellow: custom.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Yellow),
                            Blue: custom.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Blue),
                            Cyan: custom.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Cyan),
                            Magenta: custom.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.Magenta),
                            White: custom.Value.Color.HasFlag(LightsCapabilitiesClass.ColorEnum.White)),
                        Direction: custom.Value.Direction == LightsCapabilitiesClass.DirectionEnum.NotSupported ? null : new(
                            Entry: custom.Value.Direction.HasFlag(LightsCapabilitiesClass.DirectionEnum.Entry),
                            Exit: custom.Value.Direction.HasFlag(LightsCapabilitiesClass.DirectionEnum.Exit))
                        );

                        string lightPositionName = custom.Value.CustomPosition;
                        if (string.IsNullOrEmpty(lightPositionName))
                        {
                            Logger.Warning(Constants.Framework, "Custom light position name is not specified for capabilities. Skip this light.");
                            continue;
                        }

                        // converter is not generating extended properties for the custom lights
                        // ADD CUSTOM LIGHTS HERE
                    }
                }

                lights = new XFS4IoT.Lights.CapabilitiesClass(
                IndividualFlashRates: false,
                Lights: new(
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
                    FasciaLight: fasciaLight)
                );
            }
        
            XFS4IoT.Printer.CapabilitiesClass printer = null;
            if (Common.PrinterCapabilities is not null)
            {
                XFS4IoT.Printer.CapabilitiesClass.PaperSourcesClass paperSources = new(
                    Upper: Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.Upper),
                    Lower: Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.Lower),
                    External: Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.External),
                    Aux: Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.AUX),
                    Aux2: Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.AUX2),
                    Park: Common.PrinterCapabilities.PaperSources.HasFlag(PrinterCapabilitiesClass.PaperSourceEnum.Park));

                if (Common.PrinterCapabilities.CustomPaperSources is not null)
                {
                    paperSources.ExtendedProperties = Common.PrinterCapabilities.CustomPaperSources;
                }

                printer = new(
                    Type: new(
                        Receipt: Common.PrinterCapabilities.Types.HasFlag(PrinterCapabilitiesClass.TypeEnum.Receipt),
                        Passbook: Common.PrinterCapabilities.Types.HasFlag(PrinterCapabilitiesClass.TypeEnum.Passbook),
                        Journal: Common.PrinterCapabilities.Types.HasFlag(PrinterCapabilitiesClass.TypeEnum.Journal),
                        Document: Common.PrinterCapabilities.Types.HasFlag(PrinterCapabilitiesClass.TypeEnum.Document),
                        Scanner: Common.PrinterCapabilities.Types.HasFlag(PrinterCapabilitiesClass.TypeEnum.Scanner)),
                    Resolution: new(
                        Low: Common.PrinterCapabilities.Resolutions.HasFlag(PrinterCapabilitiesClass.ResolutionEnum.Low),
                        Medium: Common.PrinterCapabilities.Resolutions.HasFlag(PrinterCapabilitiesClass.ResolutionEnum.Medium),
                        High: Common.PrinterCapabilities.Resolutions.HasFlag(PrinterCapabilitiesClass.ResolutionEnum.High),
                        VeryHigh: Common.PrinterCapabilities.Resolutions.HasFlag(PrinterCapabilitiesClass.ResolutionEnum.VeryHigh)),
                    ReadForm: new(
                        Ocr: Common.PrinterCapabilities.ReadForms.HasFlag(PrinterCapabilitiesClass.ReadFormEnum.OCR),
                        Micr: Common.PrinterCapabilities.ReadForms.HasFlag(PrinterCapabilitiesClass.ReadFormEnum.MICR),
                        Msf: Common.PrinterCapabilities.ReadForms.HasFlag(PrinterCapabilitiesClass.ReadFormEnum.MSF),
                        Barcode: Common.PrinterCapabilities.ReadForms.HasFlag(PrinterCapabilitiesClass.ReadFormEnum.Barcode),
                        PageMark: Common.PrinterCapabilities.ReadForms.HasFlag(PrinterCapabilitiesClass.ReadFormEnum.PageMark),
                        ReadImage: Common.PrinterCapabilities.ReadForms.HasFlag(PrinterCapabilitiesClass.ReadFormEnum.Image),
                        ReadEmptyLine: Common.PrinterCapabilities.ReadForms.HasFlag(PrinterCapabilitiesClass.ReadFormEnum.EmptyLine)),
                    WriteForm: new(
                        Text: Common.PrinterCapabilities.WriteForms.HasFlag(PrinterCapabilitiesClass.WriteFormEnum.Text),
                        Graphics: Common.PrinterCapabilities.WriteForms.HasFlag(PrinterCapabilitiesClass.WriteFormEnum.Graphics),
                        Ocr: Common.PrinterCapabilities.WriteForms.HasFlag(PrinterCapabilitiesClass.WriteFormEnum.OCR),
                        Micr: Common.PrinterCapabilities.WriteForms.HasFlag(PrinterCapabilitiesClass.WriteFormEnum.MICR),
                        Msf: Common.PrinterCapabilities.WriteForms.HasFlag(PrinterCapabilitiesClass.WriteFormEnum.MSF),
                        Barcode: Common.PrinterCapabilities.WriteForms.HasFlag(PrinterCapabilitiesClass.WriteFormEnum.Barcode),
                        Stamp: Common.PrinterCapabilities.WriteForms.HasFlag(PrinterCapabilitiesClass.WriteFormEnum.Stamp)),
                    Extents: new(
                        Horizontal: Common.PrinterCapabilities.Extents.HasFlag(PrinterCapabilitiesClass.ExtentEnum.Horizontal),
                        Vertical: Common.PrinterCapabilities.Extents.HasFlag(PrinterCapabilitiesClass.ExtentEnum.Vertical)),
                    Control: new(
                        Eject: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Eject),
                        Perforate: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Perforate),
                        Cut: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Cut),
                        Skip: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Skip),
                        Flush: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Flush),
                        Retract: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Retract),
                        Stack: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Stack),
                        PartialCut: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.PartialCut),
                        Alarm: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Alarm),
                        PageForward: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.PageForward),
                        PageBackward: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.PageBackward),
                        TurnMedia: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.TurnMedia),
                        Stamp: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Stamp),
                        Park: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Park),
                        Expel: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Expel),
                        EjectToTransport: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.EjectToTransport),
                        Rotate180: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.Rotate180),
                        ClearBuffer: Common.PrinterCapabilities.Controls.HasFlag(PrinterCapabilitiesClass.ControlEnum.ClearBuffer)),
                    MaxMediaOnStacker: Common.PrinterCapabilities.MaxMediaOnStacker,
                    AcceptMedia: Common.PrinterCapabilities.AcceptMedia,
                    MultiPage: Common.PrinterCapabilities.MultiPage,
                    PaperSources: paperSources,
                    MediaTaken: Common.PrinterCapabilities.MediaTaken,
                    ImageType: new(
                        Tif: Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.TIF),
                        Wmf: Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.WMF),
                        Bmp: Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.BMP),
                        Jpg: Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.JPG)),
                    FrontImageColorFormat: new(
                        Binary: Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.FrontImageColorFormatEnum.Binary),
                        Grayscale: Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.FrontImageColorFormatEnum.GrayScale),
                        Full: Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.FrontImageColorFormatEnum.Full)),
                    BackImageColorFormat: new(
                        Binary: Common.PrinterCapabilities.BackImageColorFormats.HasFlag(PrinterCapabilitiesClass.BackImageColorFormatEnum.Binary),
                        GrayScale: Common.PrinterCapabilities.BackImageColorFormats.HasFlag(PrinterCapabilitiesClass.BackImageColorFormatEnum.GrayScale),
                        Full: Common.PrinterCapabilities.BackImageColorFormats.HasFlag(PrinterCapabilitiesClass.BackImageColorFormatEnum.Full)),
                    ImageSource: new(
                        ImageFront: Common.PrinterCapabilities.ImageSourceTypes.HasFlag(PrinterCapabilitiesClass.ImageSourceTypeEnum.ImageFront),
                        ImageBack: Common.PrinterCapabilities.ImageSourceTypes.HasFlag(PrinterCapabilitiesClass.ImageSourceTypeEnum.ImageBack)),
                    DispensePaper: Common.PrinterCapabilities.DispensePaper,
                    OsPrinter: Common.PrinterCapabilities.OSPrinter,
                    MediaPresented: Common.PrinterCapabilities.MediaPresented,
                    AutoRetractPeriod: Common.PrinterCapabilities.AutoRetractPeriod,
                    RetractToTransport: Common.PrinterCapabilities.RetractToTransport,
                    CoercivityType: new(
                        Low: Common.PrinterCapabilities.CoercivityTypes.HasFlag(PrinterCapabilitiesClass.CoercivityTypeEnum.Low),
                        High: Common.PrinterCapabilities.CoercivityTypes.HasFlag(PrinterCapabilitiesClass.CoercivityTypeEnum.High),
                        Auto: Common.PrinterCapabilities.CoercivityTypes.HasFlag(PrinterCapabilitiesClass.CoercivityTypeEnum.Auto)
                        ),
                    ControlPassbook: new(
                        TurnForward: Common.PrinterCapabilities.ControlPassbook.HasFlag(PrinterCapabilitiesClass.ControlPassbookEnum.TurnForward),
                        TurnBackward: Common.PrinterCapabilities.ControlPassbook.HasFlag(PrinterCapabilitiesClass.ControlPassbookEnum.TurnBackward),
                        CloseForward: Common.PrinterCapabilities.ControlPassbook.HasFlag(PrinterCapabilitiesClass.ControlPassbookEnum.CloseForward),
                        CloseBackward: Common.PrinterCapabilities.ControlPassbook.HasFlag(PrinterCapabilitiesClass.ControlPassbookEnum.CloseBackward)
                        ),
                    PrintSides: Common.PrinterCapabilities.PrintSides switch
                    {
                        PrinterCapabilitiesClass.PrintSidesEnum.Single => XFS4IoT.Printer.CapabilitiesClass.PrintSidesEnum.Single,
                        PrinterCapabilitiesClass.PrintSidesEnum.Dual => XFS4IoT.Printer.CapabilitiesClass.PrintSidesEnum.Dual,
                        _ => null
                    });
            }
			
			XFS4IoT.Auxiliaries.CapabilitiesClass auxiliaries = null;
            if(Common.AuxiliariesCapabilities is not null)
            {
                auxiliaries = new(
                    OperatorSwitch: Common.AuxiliariesCapabilities.OperatorSwitch == AuxiliariesCapabilitiesClass.OperatorSwitchEnum.NotAvailable ?
                    null : new(
                        Run: Common.AuxiliariesCapabilities.OperatorSwitch.HasFlag(AuxiliariesCapabilitiesClass.OperatorSwitchEnum.Run),
                        Maintenance: Common.AuxiliariesCapabilities.OperatorSwitch.HasFlag(AuxiliariesCapabilitiesClass.OperatorSwitchEnum.Maintenance),
                        Supervisor: Common.AuxiliariesCapabilities.OperatorSwitch.HasFlag(AuxiliariesCapabilitiesClass.OperatorSwitchEnum.Supervisor)
                        ),
                    TamperSensor: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.TamperSensor),
                    InternalTamperSensor: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.InternalTamperSensor),
                    SeismicSensor: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.SeismicSensor),
                    HeatSensor: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.HeatSensor),
                    ProximitySensor: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.ProximitySensor),
                    AmbientLightSensor: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.AmbientLightSensor),
                    EnhancedAudioSensor: Common.AuxiliariesCapabilities.EnhancedAudioSensor == AuxiliariesCapabilitiesClass.EnhancedAudioCapabilitiesEnum.NotAvailable ?
                    null : new(
                        Manual: Common.AuxiliariesCapabilities.EnhancedAudioSensor.HasFlag(AuxiliariesCapabilitiesClass.EnhancedAudioCapabilitiesEnum.Manual),
                        Auto: Common.AuxiliariesCapabilities.EnhancedAudioSensor.HasFlag(AuxiliariesCapabilitiesClass.EnhancedAudioCapabilitiesEnum.Auto),
                        SemiAuto: Common.AuxiliariesCapabilities.EnhancedAudioSensor.HasFlag(AuxiliariesCapabilitiesClass.EnhancedAudioCapabilitiesEnum.SemiAuto),
                    Bidirectional: Common.AuxiliariesCapabilities.EnhancedAudioSensor.HasFlag(AuxiliariesCapabilitiesClass.EnhancedAudioCapabilitiesEnum.Bidirectional)
                    ),
                    BootSwitchSensor: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.BootSwitchSensor),
                    ConsumerDisplaySensor: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.ConsumerDisplaySensor),
                    OperatorCallButtonSensor: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.OperatorCallButtonSensor),
                    HandsetSensor: Common.AuxiliariesCapabilities.HandsetSensor == AuxiliariesCapabilitiesClass.HandsetSensorCapabilities.NotAvailable ?
                    null : new(
                        Manual: Common.AuxiliariesCapabilities.HandsetSensor.HasFlag(AuxiliariesCapabilitiesClass.HandsetSensorCapabilities.Manual),
                        Auto: Common.AuxiliariesCapabilities.HandsetSensor.HasFlag(AuxiliariesCapabilitiesClass.HandsetSensorCapabilities.Auto),
                        SemiAuto: Common.AuxiliariesCapabilities.HandsetSensor.HasFlag(AuxiliariesCapabilitiesClass.HandsetSensorCapabilities.SemiAuto),
                        Microphone: Common.AuxiliariesCapabilities.HandsetSensor.HasFlag(AuxiliariesCapabilitiesClass.HandsetSensorCapabilities.Microphone)
                        ),
                    HeadsetMicrophoneSensor: Common.AuxiliariesCapabilities.HeadsetMicrophoneSensor == AuxiliariesCapabilitiesClass.HeadsetMicrophoneSensorCapabilities.NotAvailable ?
                    null : new(Manual: Common.AuxiliariesCapabilities.HeadsetMicrophoneSensor.HasFlag(AuxiliariesCapabilitiesClass.HeadsetMicrophoneSensorCapabilities.Manual),
                        Auto:Common.AuxiliariesCapabilities.HeadsetMicrophoneSensor.HasFlag(AuxiliariesCapabilitiesClass.HeadsetMicrophoneSensorCapabilities.Auto),
                        SemiAuto: Common.AuxiliariesCapabilities.HeadsetMicrophoneSensor.HasFlag(AuxiliariesCapabilitiesClass.HeadsetMicrophoneSensorCapabilities.SemiAuto)
                        ),
                    FasciaMicrophoneSensor: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.FasciaMicrophoneSensor),
                    CabinetDoor: Common.AuxiliariesCapabilities.SupportedDoorSensors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.Cabinet) is true ?
                    new(Closed: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.Cabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Closed),
                        Open: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.Cabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Open),
                        Locked: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.Cabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Locked),
                        Bolted: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.Cabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Bolted),
                        Tampered: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.Cabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Tampered))
                    : null,
                    SafeDoor: Common.AuxiliariesCapabilities.SupportedDoorSensors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.Safe) is true ?
                    new (Closed: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.Safe].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Closed),
                        Open: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.Safe].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Open),
                        Locked: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.Safe].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Locked),
                        Bolted: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.Safe].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Bolted),
                        Tampered: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.Safe].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Tampered))
                    : null,
                    VandalShield: Common.AuxiliariesCapabilities.VandalShield != AuxiliariesCapabilitiesClass.VandalShieldCapabilities.NotAvailable ?
                    new(Closed: Common.AuxiliariesCapabilities.VandalShield.HasFlag(AuxiliariesCapabilitiesClass.VandalShieldCapabilities.Closed),
                        Open: Common.AuxiliariesCapabilities.VandalShield.HasFlag(AuxiliariesCapabilitiesClass.VandalShieldCapabilities.Open),
                        Locked: Common.AuxiliariesCapabilities.VandalShield.HasFlag(AuxiliariesCapabilitiesClass.VandalShieldCapabilities.Locked),
                        Service: Common.AuxiliariesCapabilities.VandalShield.HasFlag(AuxiliariesCapabilitiesClass.VandalShieldCapabilities.Service),
                        Keyboard: Common.AuxiliariesCapabilities.VandalShield.HasFlag(AuxiliariesCapabilitiesClass.VandalShieldCapabilities.Keyboard),
                        Tampered: Common.AuxiliariesCapabilities.VandalShield.HasFlag(AuxiliariesCapabilitiesClass.VandalShieldCapabilities.Tampered))
                    : null,
                    FrontCabinet: Common.AuxiliariesCapabilities.SupportedDoorSensors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.FrontCabinet) is true ?
                    new(Closed: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.FrontCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Closed),
                        Open: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.FrontCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Open),
                        Locked: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.FrontCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Locked),
                        Bolted: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.FrontCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Bolted),
                        Tampered: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.FrontCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Tampered))
                    : null,
                    RearCabinet: Common.AuxiliariesCapabilities.SupportedDoorSensors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.RearCabinet) is true ?
                    new(Closed: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.RearCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Closed),
                        Open: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.RearCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Open),
                        Locked: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.RearCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Locked),
                        Bolted: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.RearCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Bolted),
                        Tampered: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.RearCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Tampered))
                    : null,
                    LeftCabinet: Common.AuxiliariesCapabilities.SupportedDoorSensors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.LeftCabinet) is true ?
                    new(Closed: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.LeftCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Closed),
                        Open: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.LeftCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Open),
                        Locked: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.LeftCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Locked),
                        Bolted: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.LeftCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Bolted),
                        Tampered: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.LeftCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Tampered))
                    : null,
                    RightCabinet: Common.AuxiliariesCapabilities.SupportedDoorSensors?.ContainsKey(AuxiliariesCapabilitiesClass.DoorType.RightCabinet) is true ?
                    new(Closed: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.RightCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Closed),
                        Open: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.RightCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Open),
                        Locked: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.RightCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Locked),
                        Bolted: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.RightCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Bolted),
                        Tampered: Common.AuxiliariesCapabilities.SupportedDoorSensors[AuxiliariesCapabilitiesClass.DoorType.RightCabinet].HasFlag(AuxiliariesCapabilitiesClass.DoorCapabilities.Tampered))
                    : null,
                    OpenCloseIndicator: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.OpenCloseIndicator),
                    Audio: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.Audio),
                    Heating: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.Heating),
                    ConsumerDisplayBacklight: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.ConsumerDisplayBacklight),
                    SignageDisplay: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.SignageDisplay),
                    Volume: Common.AuxiliariesCapabilities.Volume > 0 ?
                    Common.AuxiliariesCapabilities.Volume :
                    null,
                    Ups: Common.AuxiliariesCapabilities.Ups == AuxiliariesCapabilitiesClass.UpsEnum.NotAvailable ?
                    null : new(
                        Low: Common.AuxiliariesCapabilities.Ups.HasFlag(AuxiliariesCapabilitiesClass.UpsEnum.Low),
                        Engaged: Common.AuxiliariesCapabilities.Ups.HasFlag(AuxiliariesCapabilitiesClass.UpsEnum.Engaged),
                        Powering: Common.AuxiliariesCapabilities.Ups.HasFlag(AuxiliariesCapabilitiesClass.UpsEnum.Powering),
                        Recovered: Common.AuxiliariesCapabilities.Ups.HasFlag(AuxiliariesCapabilitiesClass.UpsEnum.Recovered)
                        ),
                    AudibleAlarm: Common.AuxiliariesCapabilities.AuxiliariesSupported.HasFlag(AuxiliariesCapabilitiesClass.AuxiliariesSupportedEnum.AudibleAlarm),
                    EnhancedAudioControl: Common.AuxiliariesCapabilities.EnhancedAudioControl == AuxiliariesCapabilitiesClass.EnhancedAudioControlEnum.NotAvailable ?
                    null : new(
                        HeadsetDetection: Common.AuxiliariesCapabilities.EnhancedAudioControl.HasFlag(AuxiliariesCapabilitiesClass.EnhancedAudioControlEnum.HeadsetDetection),
                        ModeControllable: Common.AuxiliariesCapabilities.EnhancedAudioControl.HasFlag(AuxiliariesCapabilitiesClass.EnhancedAudioControlEnum.ModeControllable)
                        ),
                    MicrophoneVolume: Common.AuxiliariesCapabilities.MicrophoneVolume > 0 ?
                    Common.AuxiliariesCapabilities.MicrophoneVolume :
                    null,
                    AutoStartupMode: Common.AuxiliariesCapabilities.AutoStartupMode == AuxiliariesCapabilitiesClass.AutoStartupModes.NotAvailable ?
                    null : new(
                        Specific: Common.AuxiliariesCapabilities.AutoStartupMode.HasFlag(AuxiliariesCapabilitiesClass.AutoStartupModes.Specific),
                        Daily: Common.AuxiliariesCapabilities.AutoStartupMode.HasFlag(AuxiliariesCapabilitiesClass.AutoStartupModes.Daily),
                        Weekly: Common.AuxiliariesCapabilities.AutoStartupMode.HasFlag(AuxiliariesCapabilitiesClass.AutoStartupModes.Weekly))
                    );
            }

            XFS4IoT.VendorApplication.CapabilitiesClass vendorApplication = null;
            if (Common.VendorApplicationCapabilities is not null &&
                Common.VendorApplicationCapabilities.SupportedAccessLevels != VendorApplicationCapabilitiesClass.SupportedAccessLevelEnum.NotSupported)
            {
                vendorApplication = new XFS4IoT.VendorApplication.CapabilitiesClass(
                    SupportedAccessLevels: new(
                        Basic: Common.VendorApplicationCapabilities.SupportedAccessLevels.HasFlag(VendorApplicationCapabilitiesClass.SupportedAccessLevelEnum.Basic),
                        Intermediate: Common.VendorApplicationCapabilities.SupportedAccessLevels.HasFlag(VendorApplicationCapabilitiesClass.SupportedAccessLevelEnum.Intermediate),
                        Full: Common.VendorApplicationCapabilities.SupportedAccessLevels.HasFlag(VendorApplicationCapabilitiesClass.SupportedAccessLevelEnum.Full)
                        )
                    );
            }

            XFS4IoT.BarcodeReader.CapabilitiesClass barcodeReader = null;
            if (Common.BarcodeReaderCapabilities is not null)
            {
                barcodeReader = new(
                    CanFilterSymbologies: Common.BarcodeReaderCapabilities.CanFilterSymbologies,
                    Symbologies: Common.BarcodeReaderCapabilities.Symbologies == BarcodeReaderCapabilitiesClass.SymbologiesEnum.NotSupported ?
                    null : new(
                        Ean128: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN128),
                        Ean8: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8),
                        Ean8_2: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8_2),
                        Ean8_5: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8_5),
                        Ean13: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13),
                        Ean13_2: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13_2),
                        Ean13_5: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13_5),
                        Jan13: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.JAN13),
                        UpcA: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA),
                        UpcE0: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0),
                        UpcE0_2: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0_2),
                        UpcE0_5: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0_5),
                        UpcE1: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1),
                        UpcE1_2: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1_2),
                        UpcE1_5: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1_5),
                        UpcA_2: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA_2),
                        UpcA_5: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA_5),
                        Codabar: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODABAR),
                        Itf: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.ITF),
                        Code11: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE11),
                        Code39: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE39),
                        Code49: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE49),
                        Code93: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE93),
                        Code128: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE128),
                        Msi: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.MSI),
                        Plessey: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PLESSEY),
                        Std2Of5: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.STD2OF5),
                        Std2Of5Iata: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.STD2OF5_IATA),
                        Pdf417: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PDF_417),
                        MicroPdf417: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.MICROPDF_417),
                        DataMatrix: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.DataMatrix),
                        MaxiCode: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.MAXICODE),
                        CodeOne: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODEONE),
                        ChannelCode: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CHANNELCODE),
                        TelepenOriginal: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.TELEPEN_ORIGINAL),
                        TelepenAim: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.TELEPEN_AIM),
                        Rss: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS),
                        RssExpanded: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS_EXPANDED),
                        RssRestricted: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS_RESTRICTED),
                        CompositeCodeA: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeA),
                        CompositeCodeB: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeB),
                        CompositeCodeC: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeC),
                        PosiCodeA: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PosiCodeA),
                        PosiCodeB: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PosiCodeB),
                        TriopticCode39: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.TriopticCode39),
                        CodablockF: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CodablockF),
                        Code16K: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.Code16K),
                        QrCode: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.QRCode),
                        Aztec: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.AztecCodes),
                        UkPost: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UKPost),
                        Planet: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.USPlanet),
                        Postnet: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.USPostnet),
                        CanadianPost:Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CanadianPost),
                        NetherlandsPost: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.NetherlandsPost),
                        AustralianPost: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.AustralianPost),
                        JapanesePost: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.JapanesePost),
                        ChinesePost: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.ChinesePost),
                        KoreanPost: Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.KoreanPost))
                    );
            }

            XFS4IoT.Biometric.CapabilitiesClass biometric = null;
            if(Common.BiometricCapabilities is not null)
            {
                biometric = new XFS4IoT.Biometric.CapabilitiesClass(
                    Type: new(
                        FacialFeatures: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.FacialFeatures),
                        Voice: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.Voice),
                        Fingerprint: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.FingerPrint),
                        FingerVein: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.FingerVein),
                        Iris: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.Iris),
                        Retina: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.Retina),
                        HandGeometry: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.HandGeometry),
                        ThermalFace: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.ThermalFace),
                        ThermalHand: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.ThermalHand),
                        PalmVein: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.PalmVein),
                        Signature: Common.BiometricCapabilities.Type.HasFlag(BiometricCapabilitiesClass.DeviceTypeEnum.Signature)
                        ),
                    MaxCapture: Common.BiometricCapabilities.MaxCaptures,
                    TemplateStorage: Common.BiometricCapabilities.TemplateStorageSpace,
                    DataFormats: new(
                        IsoFid: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.IsoFid),
                        IsoFmd: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.IsoFmd),
                        AnsiFid: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.AnsiFid),
                        AnsiFmd: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.AnsiFmd),
                        Qso: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.Qso),
                        Wso: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.Wso),
                        ReservedRaw1: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.ReservedRaw1),
                        ReservedTemplate1: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.ReservedTemplate1),
                        ReservedRaw2: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.ReservedRaw2),
                        ReservedTemplate2: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.ReservedTemplate2),
                        ReservedRaw3: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.ReservedRaw3),
                        ReservedTemplate3: Common.BiometricCapabilities.DataFormats.HasFlag(BiometricCapabilitiesClass.FormatEnum.ReservedTemplate3)
                        ),
                    EncryptionAlgorithms: new(
                        Ecb: Common.BiometricCapabilities.EncryptionAlgorithms.HasFlag(BiometricCapabilitiesClass.AlgorithmEnum.Ecb),
                        Cbc: Common.BiometricCapabilities.EncryptionAlgorithms.HasFlag(BiometricCapabilitiesClass.AlgorithmEnum.Cbc),
                        Cfb: Common.BiometricCapabilities.EncryptionAlgorithms.HasFlag(BiometricCapabilitiesClass.AlgorithmEnum.Cfb),
                        Rsa: Common.BiometricCapabilities.EncryptionAlgorithms.HasFlag(BiometricCapabilitiesClass.AlgorithmEnum.Rsa)
                        ),
                    Storage: new(
                        Secure: Common.BiometricCapabilities.Storage.HasFlag(BiometricCapabilitiesClass.StorageEnum.Secure),
                        Clear: Common.BiometricCapabilities.Storage.HasFlag(BiometricCapabilitiesClass.StorageEnum.Clear)
                        ),
                    PersistenceModes: new(
                        Persist: Common.BiometricCapabilities.PersistenceModes.HasFlag(BiometricCapabilitiesClass.PersistenceModesEnum.Persist),
                        Clear: Common.BiometricCapabilities.PersistenceModes.HasFlag(BiometricCapabilitiesClass.PersistenceModesEnum.Clear)
                        ),
                    MatchSupported: Common.BiometricCapabilities.MatchSupported switch
                    {
                        BiometricCapabilitiesClass.MatchModesEnum.StoredMatch => XFS4IoT.Biometric.CapabilitiesClass.MatchSupportedEnum.StoredMatch,
                        BiometricCapabilitiesClass.MatchModesEnum.CombinedMatch => XFS4IoT.Biometric.CapabilitiesClass.MatchSupportedEnum.CombinedMatch,
                        _ => null
                    },
                    ScanModes: new(
                        Scan: Common.BiometricCapabilities.ScanModes.HasFlag(BiometricCapabilitiesClass.ScanModesEnum.Scan),
                        Match: Common.BiometricCapabilities.ScanModes.HasFlag(BiometricCapabilitiesClass.ScanModesEnum.Match)
                        ),
                    CompareModes: new(
                        Verify: Common.BiometricCapabilities.CompareModes.HasFlag(BiometricCapabilitiesClass.CompareModesEnum.Verify),
                        Identity: Common.BiometricCapabilities.CompareModes.HasFlag(BiometricCapabilitiesClass.CompareModesEnum.Identity)
                        ),
                    ClearData: new(
                        ScannedData: Common.BiometricCapabilities.ClearData.HasFlag(BiometricCapabilitiesClass.ClearModesEnum.ScannedData),
                        ImportedData: Common.BiometricCapabilities.ClearData.HasFlag(BiometricCapabilitiesClass.ClearModesEnum.ImportedData),
                        SetMatchedData: Common.BiometricCapabilities.ClearData.HasFlag(BiometricCapabilitiesClass.ClearModesEnum.SetMatchedData)
                        )
                    );
            }

            XFS4IoT.CashAcceptor.CapabilitiesClass cashAcceptor = null;
            if (Common.CashAcceptorCapabilities is not null)
            {
                List<XFS4IoT.CashAcceptor.PosCapsClass> positions = null;

                foreach (var position in Common.CashAcceptorCapabilities.Positions)
                {
                    if (position.Key == CashManagementCapabilitiesClass.PositionEnum.NotSupported)
                    {
                        continue;
                    }

                    (positions ??= []).Add(new(
                        position.Key switch
                        {
                            CashManagementCapabilitiesClass.PositionEnum.OutDefault => XFS4IoT.CashManagement.PositionEnum.OutDefault,
                            CashManagementCapabilitiesClass.PositionEnum.OutLeft => XFS4IoT.CashManagement.PositionEnum.OutLeft,
                            CashManagementCapabilitiesClass.PositionEnum.OutRight => XFS4IoT.CashManagement.PositionEnum.OutRight,
                            CashManagementCapabilitiesClass.PositionEnum.OutCenter => XFS4IoT.CashManagement.PositionEnum.OutCenter,
                            CashManagementCapabilitiesClass.PositionEnum.OutTop => XFS4IoT.CashManagement.PositionEnum.OutTop,
                            CashManagementCapabilitiesClass.PositionEnum.OutBottom => XFS4IoT.CashManagement.PositionEnum.OutBottom,
                            CashManagementCapabilitiesClass.PositionEnum.OutFront => XFS4IoT.CashManagement.PositionEnum.OutFront,
                            CashManagementCapabilitiesClass.PositionEnum.OutRear => XFS4IoT.CashManagement.PositionEnum.OutRear,
                            CashManagementCapabilitiesClass.PositionEnum.InDefault => XFS4IoT.CashManagement.PositionEnum.InDefault,
                            CashManagementCapabilitiesClass.PositionEnum.InLeft => XFS4IoT.CashManagement.PositionEnum.InLeft,
                            CashManagementCapabilitiesClass.PositionEnum.InRight => XFS4IoT.CashManagement.PositionEnum.InRight,
                            CashManagementCapabilitiesClass.PositionEnum.InCenter => XFS4IoT.CashManagement.PositionEnum.InCenter,
                            CashManagementCapabilitiesClass.PositionEnum.InTop => XFS4IoT.CashManagement.PositionEnum.InTop,
                            CashManagementCapabilitiesClass.PositionEnum.InBottom => XFS4IoT.CashManagement.PositionEnum.InBottom,
                            CashManagementCapabilitiesClass.PositionEnum.InFront => XFS4IoT.CashManagement.PositionEnum.InFront,
                            CashManagementCapabilitiesClass.PositionEnum.InRear => XFS4IoT.CashManagement.PositionEnum.InRear,
                            _ => throw new InvalidDataException($"Unexpected position for the cash acceptor device class reported. {position.Key}")
                        },
                        Usage: position.Value.Usage == CashAcceptorCapabilitiesClass.PositionClass.UsageEnum.NotSupported ?
                        null : new(
                            In: position.Value.Usage.HasFlag(CashAcceptorCapabilitiesClass.PositionClass.UsageEnum.In),
                            Refuse: position.Value.Usage.HasFlag(CashAcceptorCapabilitiesClass.PositionClass.UsageEnum.Refuse),
                            Rollback: position.Value.Usage.HasFlag(CashAcceptorCapabilitiesClass.PositionClass.UsageEnum.Rollback)
                        ),
                        ShutterControl: position.Value.ShutterControl,
                        ItemsTakenSensor: position.Value.ItemsTakenSensor,
                        ItemsInsertedSensor: position.Value.ItemsInsertedSensor,
                        RetractAreas: position.Value.RetractArea == CashAcceptorCapabilitiesClass.PositionClass.RetractAreaEnum.NotSupported ?
                        null : new(
                            Retract: position.Value.RetractArea.HasFlag(CashAcceptorCapabilitiesClass.PositionClass.RetractAreaEnum.Retract),
                            Reject: position.Value.RetractArea.HasFlag(CashAcceptorCapabilitiesClass.PositionClass.RetractAreaEnum.Reject),
                            Transport: position.Value.RetractArea.HasFlag(CashAcceptorCapabilitiesClass.PositionClass.RetractAreaEnum.Transport),
                            Stacker: position.Value.RetractArea.HasFlag(CashAcceptorCapabilitiesClass.PositionClass.RetractAreaEnum.Stacker),
                            BillCassettes: position.Value.RetractArea.HasFlag(CashAcceptorCapabilitiesClass.PositionClass.RetractAreaEnum.BillCassettes),
                            CashIn: position.Value.RetractArea.HasFlag(CashAcceptorCapabilitiesClass.PositionClass.RetractAreaEnum.CashIn)
                        ),
                        PresentControl: position.Value.PresentControl,
                        PreparePresent: position.Value.PreparePresent)
                        );
                }

                cashAcceptor = new XFS4IoT.CashAcceptor.CapabilitiesClass(
                    Type: Common.CashAcceptorCapabilities.Type switch
                    {
                        CashManagementCapabilitiesClass.TypeEnum.SelfServiceBill => XFS4IoT.CashAcceptor.CapabilitiesClass.TypeEnum.SelfServiceBill,
                        CashManagementCapabilitiesClass.TypeEnum.SelfServiceCoin => XFS4IoT.CashAcceptor.CapabilitiesClass.TypeEnum.SelfServiceCoin,
                        CashManagementCapabilitiesClass.TypeEnum.TellerBill => XFS4IoT.CashAcceptor.CapabilitiesClass.TypeEnum.TellerBill,
                        CashManagementCapabilitiesClass.TypeEnum.TellerCoin => XFS4IoT.CashAcceptor.CapabilitiesClass.TypeEnum.TellerCoin,
                        _ => throw new InternalErrorException($"Unexpected cash acceptor type specified. {Common.CashAcceptorCapabilities.Type}"),
                    },
                    MaxCashInItems: Common.CashAcceptorCapabilities.MaxCashInItems,
                    Shutter: Common.CashAcceptorCapabilities.Shutter,
                    ShutterControl: Common.CashAcceptorCapabilities.ShutterControl,
                    IntermediateStacker: Common.CashAcceptorCapabilities.IntermediateStacker,
                    ItemsInsertedSensor: Common.CashAcceptorCapabilities.ItemsTakenSensor,
                    Positions: positions,
                    RetractAreas: Common.CashAcceptorCapabilities.RetractAreas == CashManagementCapabilitiesClass.RetractAreaEnum.Default ?
                    null : new(
                        Retract: Common.CashAcceptorCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Retract),
                        Transport: Common.CashAcceptorCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Transport),
                        Stacker: Common.CashAcceptorCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Stacker),
                        Reject: Common.CashAcceptorCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.Reject),
                        BillCassette: Common.CashAcceptorCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette),
                        CashIn: Common.CashAcceptorCapabilities.RetractAreas.HasFlag(CashManagementCapabilitiesClass.RetractAreaEnum.CashIn)
                        ),
                    RetractTransportActions: Common.CashAcceptorCapabilities.RetractTransportActions == CashManagementCapabilitiesClass.RetractTransportActionEnum.NotSupported ?
                    null : new(
                        Present: Common.CashAcceptorCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.Present),
                        Retract: Common.CashAcceptorCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.Retract),
                        Reject: Common.CashAcceptorCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.Reject),
                        BillCassette: Common.CashAcceptorCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.BillCassette),
                        CashIn: Common.CashAcceptorCapabilities.RetractTransportActions.HasFlag(CashManagementCapabilitiesClass.RetractTransportActionEnum.CashIn)
                        ),
                    RetractStackerActions: Common.CashAcceptorCapabilities.RetractStackerActions == CashManagementCapabilitiesClass.RetractStackerActionEnum.NotSupported ?
                    null : new(
                        Present: Common.CashAcceptorCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.Present),
                        Retract: Common.CashAcceptorCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.Retract),
                        Reject: Common.CashAcceptorCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.Reject),
                        BillCassette: Common.CashAcceptorCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.BillCassette),
                        CashIn: Common.CashAcceptorCapabilities.RetractStackerActions.HasFlag(CashManagementCapabilitiesClass.RetractStackerActionEnum.CashIn)
                        ),
                    CashInLimit: Common.CashAcceptorCapabilities.CashInLimit == CashAcceptorCapabilitiesClass.CashInLimitEnum.NotSupported ?
                    null : new(
                        ByTotalItems: Common.CashAcceptorCapabilities.CashInLimit.HasFlag(CashAcceptorCapabilitiesClass.CashInLimitEnum.ByTotalItems),
                        ByAmount: Common.CashAcceptorCapabilities.CashInLimit.HasFlag(CashAcceptorCapabilitiesClass.CashInLimitEnum.ByAmount)
                        ),
                    CountActions: Common.CashAcceptorCapabilities.CountActions == CashAcceptorCapabilitiesClass.CountActionEnum.NotSupported ?
                    null : new(
                        Individual: Common.CashAcceptorCapabilities.CountActions.HasFlag(CashAcceptorCapabilitiesClass.CountActionEnum.Individual),
                        All: Common.CashAcceptorCapabilities.CountActions.HasFlag(CashAcceptorCapabilitiesClass.CountActionEnum.All)
                        ),
                    RetainAction: Common.CashAcceptorCapabilities.RetainCounterfeitAction == CashAcceptorCapabilitiesClass.RetainCounterfeitActionEnum.NotSupported ?
                    null : new(
                        Counterfeit: Common.CashAcceptorCapabilities.RetainCounterfeitAction.HasFlag(CashAcceptorCapabilitiesClass.RetainCounterfeitActionEnum.Level2),
                        Suspect: Common.CashAcceptorCapabilities.RetainCounterfeitAction.HasFlag(CashAcceptorCapabilitiesClass.RetainCounterfeitActionEnum.Level3),
                        Inked: Common.CashAcceptorCapabilities.RetainCounterfeitAction.HasFlag(CashAcceptorCapabilitiesClass.RetainCounterfeitActionEnum.Inked)
                        )
                );
            }

            XFS4IoT.Camera.CapabilitiesClass camera = null;
            if (Common.CameraCapabilities is not null)
            {
                camera = new(
                    Cameras: Common.CameraCapabilities.Cameras is null || Common.CameraCapabilities.Cameras.Count == 0 ?
                    null : new(
                        Room: Common.CameraCapabilities.Cameras.Keys.Contains(CameraCapabilitiesClass.CameraEnum.Room) ?
                        Common.CameraCapabilities.Cameras[CameraCapabilitiesClass.CameraEnum.Room] :
                        null,
                        Person: Common.CameraCapabilities.Cameras.Keys.Contains(CameraCapabilitiesClass.CameraEnum.Person) ?
                        Common.CameraCapabilities.Cameras[CameraCapabilitiesClass.CameraEnum.Person] :
                        null,
                        ExitSlot: Common.CameraCapabilities.Cameras.Keys.Contains(CameraCapabilitiesClass.CameraEnum.ExitSlot) ?
                        Common.CameraCapabilities.Cameras[CameraCapabilitiesClass.CameraEnum.ExitSlot] :
                        null
                        ),
                    MaxPictures: Common.CameraCapabilities.MaxPictures == -1 ?
                    null :
                    Common.CameraCapabilities.MaxPictures,
                    CamData: Common.CameraCapabilities.CamData == CameraCapabilitiesClass.CamDataMethodsEnum.None ?
                    null : new(
                        AutoAdd: Common.CameraCapabilities.CamData.HasFlag(CameraCapabilitiesClass.CamDataMethodsEnum.AutoAdd),
                        ManAdd: Common.CameraCapabilities.CamData.HasFlag(CameraCapabilitiesClass.CamDataMethodsEnum.ManualAdd)
                    ),
                    MaxDataLength: Common.CameraCapabilities.MaxDataLength == -1 ?
                    null :
                    Common.CameraCapabilities.MaxDataLength);
            }

            XFS4IoT.Check.CapabilitiesClass checkScanner = null;
            if (Common.CheckScannerCapabilities is not null)
            {
                XFS4IoT.Check.PositionCapabilitiesClass input = null;
                XFS4IoT.Check.PositionCapabilitiesClass output = null;
                XFS4IoT.Check.PositionCapabilitiesClass refused = null;
                if (Common.CheckScannerCapabilities.Positions is not null)
                {
                    foreach (var positionCap in Common.CheckScannerCapabilities.Positions)
                    {
                        XFS4IoT.Check.PositionCapabilitiesClass cap = new(
                            ItemsTakenSensor: positionCap.Value.ItemsTakenSensor,
                            ItemsInsertedSensor: positionCap.Value.ItemsInsertedSensor,
                            positionCap.Value.RetractAreas == CheckScannerCapabilitiesClass.PositonCapabilities.RetractAreasEnum.None ?
                            null : new XFS4IoT.Check.PositionCapabilitiesClass.RetractAreasClass(
                                RetractBin: positionCap.Value.RetractAreas.HasFlag(CheckScannerCapabilitiesClass.PositonCapabilities.RetractAreasEnum.RetractBin),
                                Transport: positionCap.Value.RetractAreas.HasFlag(CheckScannerCapabilitiesClass.PositonCapabilities.RetractAreasEnum.Transport),
                                Stacker: positionCap.Value.RetractAreas.HasFlag(CheckScannerCapabilitiesClass.PositonCapabilities.RetractAreasEnum.Stacker),
                                Rebuncher: positionCap.Value.RetractAreas.HasFlag(CheckScannerCapabilitiesClass.PositonCapabilities.RetractAreasEnum.Rebuncher)
                                ));

                        switch (positionCap.Key)
                        {
                            case CheckScannerCapabilitiesClass.PositionEnum.Input:
                                input = cap;
                                break;
                            case CheckScannerCapabilitiesClass.PositionEnum.Output:
                                output = cap;
                                break;
                            case CheckScannerCapabilitiesClass.PositionEnum.Refused:
                                refused = cap;
                                break;
                            default:
                                throw new InternalErrorException($"Unexpected position specified. {positionCap.Key}");
                        }
                    }
                }

                checkScanner = new XFS4IoT.Check.CapabilitiesClass(
                    Type: Common.CheckScannerCapabilities.Type switch
                    { 
                        CheckScannerCapabilitiesClass.TypeEnum.Single => XFS4IoT.Check.CapabilitiesClass.TypeEnum.SingleMediaInput,
                        CheckScannerCapabilitiesClass.TypeEnum.Bunch => XFS4IoT.Check.CapabilitiesClass.TypeEnum.BunchMediaInput,
                        _ => throw new InternalErrorException($"Unexpected check scanner type specified. {Common.CheckScannerCapabilities.Type}"),
                    },
                    MaxMediaOnStacker: Common.CheckScannerCapabilities.MaxMediaOnStacker < 0 ? null : Common.CheckScannerCapabilities.MaxMediaOnStacker,
                    PrintSize: Common.CheckScannerCapabilities.PrintSize is null ? 
                    null : new(
                        Rows: Common.CheckScannerCapabilities.PrintSize.Rows, 
                        Cols: Common.CheckScannerCapabilities.PrintSize.Cols),
                    Stamp: Common.CheckScannerCapabilities.Stamp,
                    Rescan: Common.CheckScannerCapabilities.Rescan,
                    PresentControl: Common.CheckScannerCapabilities.PresentControl,
                    ApplicationRefuse: Common.CheckScannerCapabilities.ApplicationRefuse,
                    RetractLocation: Common.CheckScannerCapabilities.RetractLocations == CheckScannerCapabilitiesClass.RetractLocationEnum.Default ?
                    null : new(
                        Storage: Common.CheckScannerCapabilities.RetractLocations.HasFlag(CheckScannerCapabilitiesClass.RetractLocationEnum.Storage),
                        Transport: Common.CheckScannerCapabilities.RetractLocations.HasFlag(CheckScannerCapabilitiesClass.RetractLocationEnum.Transport),
                        Stacker: Common.CheckScannerCapabilities.RetractLocations.HasFlag(CheckScannerCapabilitiesClass.RetractLocationEnum.Stacker),
                        Rebuncher: Common.CheckScannerCapabilities.RetractLocations.HasFlag(CheckScannerCapabilitiesClass.RetractLocationEnum.ReBuncher)
                        ),
                    ResetControl: Common.CheckScannerCapabilities.ResetControls == CheckScannerCapabilitiesClass.ResetControlEnum.Default ?
                    null : new XFS4IoT.Check.CapabilitiesClass.ResetControlClass(
                        Eject: Common.CheckScannerCapabilities.ResetControls.HasFlag(CheckScannerCapabilitiesClass.ResetControlEnum.Eject),
                        StorageUnit: Common.CheckScannerCapabilities.ResetControls.HasFlag(CheckScannerCapabilitiesClass.ResetControlEnum.Storage),
                        Transport: Common.CheckScannerCapabilities.ResetControls.HasFlag(CheckScannerCapabilitiesClass.ResetControlEnum.Transport),
                        Rebuncher: Common.CheckScannerCapabilities.ResetControls.HasFlag(CheckScannerCapabilitiesClass.ResetControlEnum.ReBuncher)
                        ),
                    ImageType: Common.CheckScannerCapabilities.ImageTypes == CheckScannerCapabilitiesClass.ImageTypeEnum.None ? 
                    null : new XFS4IoT.Check.CapabilitiesClass.ImageTypeClass(
                        Tif: Common.CheckScannerCapabilities.ImageTypes.HasFlag(CheckScannerCapabilitiesClass.ImageTypeEnum.TIF),
                        Wmf: Common.CheckScannerCapabilities.ImageTypes.HasFlag(CheckScannerCapabilitiesClass.ImageTypeEnum.WMF),
                        Bmp: Common.CheckScannerCapabilities.ImageTypes.HasFlag(CheckScannerCapabilitiesClass.ImageTypeEnum.BMP),
                        Jpg: Common.CheckScannerCapabilities.ImageTypes.HasFlag(CheckScannerCapabilitiesClass.ImageTypeEnum.JPG)
                        ),
                    FrontImage: Common.CheckScannerCapabilities.FrontImage is null ?
                    null : new XFS4IoT.Check.ImageCapabilitiesClass(
                        ScanColor: new XFS4IoT.Check.ImageCapabilitiesClass.ScanColorClass(
                            Red: Common.CheckScannerCapabilities.FrontImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Red),
                            Green: Common.CheckScannerCapabilities.FrontImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Green),
                            Blue: Common.CheckScannerCapabilities.FrontImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Blue),
                            Yellow: Common.CheckScannerCapabilities.FrontImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Yellow),
                            White: Common.CheckScannerCapabilities.FrontImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.White),
                            InfraRed: Common.CheckScannerCapabilities.FrontImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.InfraRed),
                            UltraViolet: Common.CheckScannerCapabilities.FrontImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.UltraViolet)
                            ),
                        DefaultScanColor: Common.CheckScannerCapabilities.FrontImage.DefaultScanColor == CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.None ?
                        null : Common.CheckScannerCapabilities.FrontImage.DefaultScanColor switch
                        { 
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.InfraRed => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.InfraRed,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.UltraViolet => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.UltraViolet,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Red => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.Red,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Green => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.Green,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Blue => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.Blue,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.White => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.White,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Yellow => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.Yellow,
                            _ => throw new InternalErrorException($"Unexpected front image default color specified. {Common.CheckScannerCapabilities.FrontImage.DefaultScanColor}"),

                        }),
                    BackImage: Common.CheckScannerCapabilities.BackImage is null ?
                    null : new XFS4IoT.Check.ImageCapabilitiesClass(
                        ScanColor: new XFS4IoT.Check.ImageCapabilitiesClass.ScanColorClass(
                            Red: Common.CheckScannerCapabilities.BackImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Red),
                            Green: Common.CheckScannerCapabilities.BackImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Green),
                            Blue: Common.CheckScannerCapabilities.BackImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Blue),
                            Yellow: Common.CheckScannerCapabilities.BackImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Yellow),
                            White: Common.CheckScannerCapabilities.BackImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.White),
                            InfraRed: Common.CheckScannerCapabilities.BackImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.InfraRed),
                            UltraViolet: Common.CheckScannerCapabilities.BackImage.ScanColor.HasFlag(CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.UltraViolet)
                            ),
                        DefaultScanColor: Common.CheckScannerCapabilities.BackImage.DefaultScanColor == CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.None ?
                        null : Common.CheckScannerCapabilities.BackImage.DefaultScanColor switch
                        {
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.InfraRed => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.InfraRed,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.UltraViolet => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.UltraViolet,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Red => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.Red,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Green => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.Green,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Blue => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.Blue,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.White => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.White,
                            CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Yellow => XFS4IoT.Check.ImageCapabilitiesClass.DefaultScanColorEnum.Yellow,
                            _ => throw new InternalErrorException($"Unexpected front image default color specified. {Common.CheckScannerCapabilities.FrontImage.DefaultScanColor}"),
                        }),
                    CodelineFormat: Common.CheckScannerCapabilities.CodelineFormats == CheckScannerCapabilitiesClass.CodelineFormatEnum.None ?
                    null : new XFS4IoT.Check.CapabilitiesClass.CodelineFormatClass(
                        Cmc7: Common.CheckScannerCapabilities.CodelineFormats.HasFlag(CheckScannerCapabilitiesClass.CodelineFormatEnum.CMC7),
                        E13b: Common.CheckScannerCapabilities.CodelineFormats.HasFlag(CheckScannerCapabilitiesClass.CodelineFormatEnum.E13B),
                        Ocr: Common.CheckScannerCapabilities.CodelineFormats.HasFlag(CheckScannerCapabilitiesClass.CodelineFormatEnum.OCR),
                        Ocra: Common.CheckScannerCapabilities.CodelineFormats.HasFlag(CheckScannerCapabilitiesClass.CodelineFormatEnum.OCRA),
                        Ocrb: Common.CheckScannerCapabilities.CodelineFormats.HasFlag(CheckScannerCapabilitiesClass.CodelineFormatEnum.OCRB)),
                    DataSource: Common.CheckScannerCapabilities.DataSources == CheckScannerCapabilitiesClass.DataSourceEnum.None ?
                    null : new XFS4IoT.Check.CapabilitiesClass.DataSourceClass(
                        ImageFront: Common.CheckScannerCapabilities.DataSources.HasFlag(CheckScannerCapabilitiesClass.DataSourceEnum.Front),
                        ImageBack: Common.CheckScannerCapabilities.DataSources.HasFlag(CheckScannerCapabilitiesClass.DataSourceEnum.Back),
                        CodeLine: Common.CheckScannerCapabilities.DataSources.HasFlag(CheckScannerCapabilitiesClass.DataSourceEnum.Codeline)),
                    InsertOrientation: Common.CheckScannerCapabilities.InsertOrientations == CheckScannerCapabilitiesClass.InsertOrientationEnum.None ?
                    null : new XFS4IoT.Check.CapabilitiesClass.InsertOrientationClass(
                        CodeLineRight: Common.CheckScannerCapabilities.InsertOrientations.HasFlag(CheckScannerCapabilitiesClass.InsertOrientationEnum.CodelineRight),
                        CodeLineLeft: Common.CheckScannerCapabilities.InsertOrientations.HasFlag(CheckScannerCapabilitiesClass.InsertOrientationEnum.CodelineLeft),
                        CodeLineBottom: Common.CheckScannerCapabilities.InsertOrientations.HasFlag(CheckScannerCapabilitiesClass.InsertOrientationEnum.CodelineBottom),
                        CodeLineTop: Common.CheckScannerCapabilities.InsertOrientations.HasFlag(CheckScannerCapabilitiesClass.InsertOrientationEnum.CodelineTop),
                        FaceUp: Common.CheckScannerCapabilities.InsertOrientations.HasFlag(CheckScannerCapabilitiesClass.InsertOrientationEnum.FaceUp),
                        FaceDown: Common.CheckScannerCapabilities.InsertOrientations.HasFlag(CheckScannerCapabilitiesClass.InsertOrientationEnum.FaceDown)),
                    Positions: input is null && output is null && refused is null ?
                    null : new XFS4IoT.Check.CapabilitiesClass.PositionsClass(input, output, refused),
                    ImageAfterEndorse: Common.CheckScannerCapabilities.ImageAfterEndorse,
                    ReturnedItemsProcessing: Common.CheckScannerCapabilities.ReturnedItemsProcessing == CheckScannerCapabilitiesClass.ReturnedItemsProcessingEnum.None ?
                    null : new XFS4IoT.Check.CapabilitiesClass.ReturnedItemsProcessingClass(
                        Endorse: Common.CheckScannerCapabilities.ReturnedItemsProcessing.HasFlag(CheckScannerCapabilitiesClass.ReturnedItemsProcessingEnum.Endorse),
                        EndorseImage: Common.CheckScannerCapabilities.ReturnedItemsProcessing.HasFlag(CheckScannerCapabilitiesClass.ReturnedItemsProcessingEnum.EndorseImage)),
                    PrintSizeFront: Common.CheckScannerCapabilities.PrintSizeFront is null ?
                    null : new XFS4IoT.Check.PrintsizeClass(
                        Rows: Common.CheckScannerCapabilities.PrintSizeFront.Rows,
                        Cols: Common.CheckScannerCapabilities.PrintSizeFront.Cols)
                    );
            }

            XFS4IoT.MixedMedia.CapabilitiesClass mixedMedia = null;
            if (Common.MixedMediaCapabilities is not null)
            {
                mixedMedia = new XFS4IoT.MixedMedia.CapabilitiesClass(
                    Modes: Common.MixedMediaCapabilities.Modes == MixedMedia.ModeTypeEnum.None ?
                        null : new(
                            CashAccept: Common.MixedMediaCapabilities.Modes.HasFlag(MixedMedia.ModeTypeEnum.Cash),
                            CheckAccept: Common.MixedMediaCapabilities.Modes.HasFlag(MixedMedia.ModeTypeEnum.Check)
                            ),
                    Dynamic: Common.MixedMediaCapabilities.DynamicMode
                    );
            }

            XFS4IoT.PowerManagement.CapabilitiesClass powerManagement = null;
            if (Common.PowerManagementCapabilities is not null)
            {
                powerManagement = new XFS4IoT.PowerManagement.CapabilitiesClass(
                    PowerSaveControl: Common.PowerManagementCapabilities.PowerSaveControl,
                    BatteryRechargeable: Common.PowerManagementCapabilities.BatteryRechargeable
                    );
            }

            XFS4IoT.BanknoteNeutralization.CapabilitiesClass ibns = null;
            if (Common.IBNSCapabilities is not null)
            {
                Dictionary<string, XFS4IoT.BanknoteNeutralization.CapabilitiesClass.CustomInputsClass> customInputCapabilities = null;
                if (Common.IBNSCapabilities.CustomInputStatus is not null &&
                    Common.IBNSCapabilities.CustomInputStatus.Count > 0)
                {
                    customInputCapabilities = [];
                    foreach (var inputState in Common.IBNSCapabilities.CustomInputStatus)
                    {
                        customInputCapabilities.Add(inputState.Key.ToString().ToCamelCase(), new(inputState.Value.ActiveInput));
                    }
                }
                if (Common.IBNSCapabilities.VendorSpecificCustomInputStatus is not null &&
                    Common.IBNSCapabilities.VendorSpecificCustomInputStatus.Count > 0)
                {
                    foreach (var inputState in Common.IBNSCapabilities.VendorSpecificCustomInputStatus)
                    {
                        (customInputCapabilities ??= []).Add(inputState.Key.ToCamelCase(), new(inputState.Value.ActiveInput));
                    }
                }
                ibns = new XFS4IoT.BanknoteNeutralization.CapabilitiesClass(
                    Mode: Common.IBNSCapabilities.Mode switch
                    {
                        IBNSCapabilitiesClass.ModeEnum.ClientControlled => XFS4IoT.BanknoteNeutralization.CapabilitiesClass.ModeEnum.ClientControlled,
                        IBNSCapabilitiesClass.ModeEnum.VendorSpecific => XFS4IoT.BanknoteNeutralization.CapabilitiesClass.ModeEnum.VendorSpecific,
                        IBNSCapabilitiesClass.ModeEnum.Autonomous => XFS4IoT.BanknoteNeutralization.CapabilitiesClass.ModeEnum.Autonomous,
                        _ => throw new InternalErrorException($"Unexpected IBNS mode specified. {Common.IBNSCapabilities.Mode}")
                    },
                    GasSensor: Common.IBNSCapabilities.GasSensor is true ? true : null,
                    LightSensor: Common.IBNSCapabilities.LightSensor is true ? true : null,
                    SeismicSensor: Common.IBNSCapabilities.SeismicSensor is true ? true : null,
                    SafeIntrusionDetection: Common.IBNSCapabilities.SafeIntrusionDetection is true ? true : null,
                    ExternalDryContactStatusBox: Common.IBNSCapabilities.ExternalDryContactStatusBox is true ? true : null,
                    RealTimeClock: Common.IBNSCapabilities.RealTimeClock is true ? true : null,
                    PhysicalStorageUnitsAccessControl: Common.IBNSCapabilities.PhysicalStorageUnitsAccessControl is true ? true : null,
                    CustomInputs: customInputCapabilities);
            }

            XFS4IoT.German.CapabilitiesClass germanSpecific = null;
            if (Common.GermanCapabilities is not null)
            {
                germanSpecific = new(Common.GermanCapabilities.HSMVendor);
            }
            
            XFS4IoT.Deposit.CapabilitiesClass deposit = null;
            if (Common.DepositCapabilities is not null)
            {
                deposit = new XFS4IoT.Deposit.CapabilitiesClass(
                    Type: new(
                        Envelope: Common.DepositCapabilities.DeviceTypes.HasFlag(DepositCapabilitiesClass.TypesEnum.Envelop),
                        Bag: Common.DepositCapabilities.DeviceTypes.HasFlag(DepositCapabilitiesClass.TypesEnum.Bag)),
                    DepTransport: Common.DepositCapabilities.DepostTransport,
                    Printer: Common.DepositCapabilities.PrinterCapabilities is null ? 
                    null : new(
                        Toner: Common.DepositCapabilities.PrinterCapabilities.Toner,
                        PrintOnRetract: Common.DepositCapabilities.PrinterCapabilities.PrintOnRetract,
                        MaxNumChars: Common.DepositCapabilities.PrinterCapabilities.MaxNumberOfChars,
                        UnicodeSupport: Common.DepositCapabilities.PrinterCapabilities.UnicodeSupport),
                    Shutter: Common.DepositCapabilities.Shutter,
                    RetractEnvelope: Common.DepositCapabilities.RetractPosition switch
                    { 
                        DepositCapabilitiesClass.RetractPositionEnum.Container => XFS4IoT.Deposit.CapabilitiesClass.RetractEnvelopeEnum.Container,
                        DepositCapabilitiesClass.RetractPositionEnum.Dispenser => XFS4IoT.Deposit.CapabilitiesClass.RetractEnvelopeEnum.Dispenser,
                        _ => throw new InternalErrorException($"Unexpected retract position specified. {Common.DepositCapabilities.RetractPosition}")
                    });
            }
            return Task.FromResult(
                new CommandResult<CapabilitiesCompletion.PayloadData>(
                    new CapabilitiesCompletion.PayloadData(
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
                        Lights: lights,
                        Printer: printer,
				        Auxiliaries: auxiliaries,
                        VendorApplication: vendorApplication,
                        BarcodeReader: barcodeReader,
                        Biometric: biometric,
                        CashAcceptor: cashAcceptor,
                        Camera: camera,
                        Check: checkScanner,
                        MixedMedia: mixedMedia,
                        German: germanSpecific,
                        BanknoteNeutralization: ibns,
                        PowerManagement: powerManagement,
                        Deposit: deposit),
                MessageHeader.CompletionCodeEnum.Success
            ));
        }

        private InterfaceClass GetDeviceInterface(InterfaceClass.NameEnum InterfaceName)
        {
            Dictionary<string, InterfaceClass.CommandsClass> commands = null;
            Dictionary<string, InterfaceClass.EventsClass> events = null;

            foreach (var msg in Provider.GetMessagesSupported())
            {
                if (msg.Key.StartsWith($"{InterfaceName}."))
                {
                    if (msg.Value.Type == MessageTypeInfo.MessageTypeEnum.Command)
                    {
                        (commands ??= []).Add(msg.Key, new(msg.Value.Versions));
                    }
                    else if (msg.Value.Type == MessageTypeInfo.MessageTypeEnum.Event)
                    {
                        (events ??= []).Add(msg.Key, new(msg.Value.Versions));
                    }
                }
            }

            if (commands is null &&
                events is null)
            {
                return null;
            }

            return new(Name: InterfaceName,
                       Commands: commands,
                       Events: events,
                       MaximumRequests: XFSConstants.MaximumRequests);
        }
    }
}
