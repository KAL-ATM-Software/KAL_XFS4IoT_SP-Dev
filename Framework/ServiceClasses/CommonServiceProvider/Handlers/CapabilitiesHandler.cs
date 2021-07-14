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
using XFS4IoT.TextTerminal;

namespace XFS4IoTFramework.Common
{
    [CommandHandlerAsync]
    public partial class CapabilitiesHandler
    {

        private Task<CapabilitiesCompletion.PayloadData> HandleCapabilities(ICapabilitiesEvents events, CapabilitiesCommand capabilities, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CommonDev.Capabilities()");
            var result = Device.Capabilities();
            Logger.Log(Constants.DeviceClass, $"CommonDev.Capabilities() -> {result.CompletionCode}");

            if (result.CashDispenser is not null)
            {
                Dictionary<CashDispenserCapabilitiesClass.RetractAreaEnum, bool> retractAreas = new()
                {
                    { CashDispenserCapabilitiesClass.RetractAreaEnum.ItemCassette, false },
                    { CashDispenserCapabilitiesClass.RetractAreaEnum.Reject, false },
                    { CashDispenserCapabilitiesClass.RetractAreaEnum.Retract, false },
                    { CashDispenserCapabilitiesClass.RetractAreaEnum.Stacker, false },
                    { CashDispenserCapabilitiesClass.RetractAreaEnum.Transport, false },
                    { CashDispenserCapabilitiesClass.RetractAreaEnum.Default, true }
                };

                if (result.CashDispenser.RetractAreas?.ItemCassette is not null &&
                    (bool)result.CashDispenser.RetractAreas?.ItemCassette)
                {
                    retractAreas[CashDispenserCapabilitiesClass.RetractAreaEnum.ItemCassette] = true;
                }
                if (result.CashDispenser.RetractAreas?.Reject is not null &&
                    (bool)result.CashDispenser.RetractAreas?.Reject)
                {
                    retractAreas[CashDispenserCapabilitiesClass.RetractAreaEnum.Reject] = true;
                }
                if (result.CashDispenser.RetractAreas?.Retract is not null &&
                    (bool)result.CashDispenser.RetractAreas?.Retract)
                {
                    retractAreas[CashDispenserCapabilitiesClass.RetractAreaEnum.Retract] = true;
                }
                if (result.CashDispenser.RetractAreas?.Stacker is not null &&
                    (bool)result.CashDispenser.RetractAreas?.Stacker)
                {
                    retractAreas[CashDispenserCapabilitiesClass.RetractAreaEnum.Stacker] = true;
                }
                if (result.CashDispenser.RetractAreas?.Transport is not null &&
                    (bool)result.CashDispenser.RetractAreas?.Transport)
                {
                    retractAreas[CashDispenserCapabilitiesClass.RetractAreaEnum.Transport] = true;
                }

                Dictionary<CashDispenserCapabilitiesClass.RetractStackerActionEnum, bool> retractStackerActions = new()
                {
                    { CashDispenserCapabilitiesClass.RetractStackerActionEnum.ItemCassette, false },
                    { CashDispenserCapabilitiesClass.RetractStackerActionEnum.Present, false },
                    { CashDispenserCapabilitiesClass.RetractStackerActionEnum.Reject, false },
                    { CashDispenserCapabilitiesClass.RetractStackerActionEnum.Retract, false }
                };

                if (result.CashDispenser.RetractStackerActions?.ItemCassette is not null &&
                    (bool)result.CashDispenser.RetractStackerActions?.ItemCassette)
                {
                    retractStackerActions[CashDispenserCapabilitiesClass.RetractStackerActionEnum.ItemCassette] = true;
                }
                if (result.CashDispenser.RetractStackerActions?.Present is not null &&
                    (bool)result.CashDispenser.RetractStackerActions?.Present)
                {
                    retractStackerActions[CashDispenserCapabilitiesClass.RetractStackerActionEnum.Present] = true;
                }
                if (result.CashDispenser.RetractStackerActions?.Reject is not null &&
                    (bool)result.CashDispenser.RetractStackerActions?.Reject)
                {
                    retractStackerActions[CashDispenserCapabilitiesClass.RetractStackerActionEnum.Reject] = true;
                }
                if (result.CashDispenser.RetractStackerActions?.Retract is not null &&
                    (bool)result.CashDispenser.RetractStackerActions?.Retract)
                {
                    retractStackerActions[CashDispenserCapabilitiesClass.RetractStackerActionEnum.Retract] = true;
                }

                Dictionary<CashDispenserCapabilitiesClass.RetractTransportActionEnum, bool> retractTransportActions = new()
                {
                    { CashDispenserCapabilitiesClass.RetractTransportActionEnum.ItemCassette, false },
                    { CashDispenserCapabilitiesClass.RetractTransportActionEnum.Present, false },
                    { CashDispenserCapabilitiesClass.RetractTransportActionEnum.Reject, false },
                    { CashDispenserCapabilitiesClass.RetractTransportActionEnum.Retract, false }
                };

                if (result.CashDispenser.RetractTransportActions?.ItemCassette is not null &&
                    (bool)result.CashDispenser.RetractTransportActions?.ItemCassette)
                {
                    retractTransportActions[CashDispenserCapabilitiesClass.RetractTransportActionEnum.ItemCassette] = true;
                }
                if (result.CashDispenser.RetractTransportActions?.Present is not null &&
                    (bool)result.CashDispenser.RetractTransportActions?.Present)
                {
                    retractTransportActions[CashDispenserCapabilitiesClass.RetractTransportActionEnum.Present] = true;
                }
                if (result.CashDispenser.RetractTransportActions?.Reject is not null &&
                    (bool)result.CashDispenser.RetractTransportActions?.Reject)
                {
                    retractTransportActions[CashDispenserCapabilitiesClass.RetractTransportActionEnum.Reject] = true;
                }
                if (result.CashDispenser.RetractTransportActions?.Retract is not null &&
                    (bool)result.CashDispenser.RetractTransportActions?.Retract)
                {
                    retractTransportActions[CashDispenserCapabilitiesClass.RetractTransportActionEnum.Retract] = true;
                }

                Dictionary<CashDispenserCapabilitiesClass.OutputPositionEnum, bool> outputPositions = new()
                {
                    { CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom, false },
                    { CashDispenserCapabilitiesClass.OutputPositionEnum.Center, false },
                    { CashDispenserCapabilitiesClass.OutputPositionEnum.Default, true },
                    { CashDispenserCapabilitiesClass.OutputPositionEnum.Front, false },
                    { CashDispenserCapabilitiesClass.OutputPositionEnum.Left, false },
                    { CashDispenserCapabilitiesClass.OutputPositionEnum.Rear, false },
                    { CashDispenserCapabilitiesClass.OutputPositionEnum.Right, false },
                    { CashDispenserCapabilitiesClass.OutputPositionEnum.Top, false }
                };

                if (result.CashDispenser.Positions?.Bottom is not null &&
                    (bool)result.CashDispenser.Positions?.Bottom)
                {
                    outputPositions[CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom] = true;
                }
                if (result.CashDispenser.Positions?.Center is not null &&
                    (bool)result.CashDispenser.Positions?.Center)
                {
                    outputPositions[CashDispenserCapabilitiesClass.OutputPositionEnum.Center] = true;
                }
                if (result.CashDispenser.Positions?.Front is not null &&
                    (bool)result.CashDispenser.Positions?.Front)
                {
                    outputPositions[CashDispenserCapabilitiesClass.OutputPositionEnum.Front] = true;
                }
                if (result.CashDispenser.Positions?.Left is not null &&
                    (bool)result.CashDispenser.Positions?.Left)
                {
                    outputPositions[CashDispenserCapabilitiesClass.OutputPositionEnum.Left] = true;
                }
                if (result.CashDispenser.Positions?.Rear is not null &&
                    (bool)result.CashDispenser.Positions?.Rear)
                {
                    outputPositions[CashDispenserCapabilitiesClass.OutputPositionEnum.Rear] = true;
                }
                if (result.CashDispenser.Positions?.Right is not null &&
                    (bool)result.CashDispenser.Positions?.Right)
                {
                    outputPositions[CashDispenserCapabilitiesClass.OutputPositionEnum.Right] = true;
                }
                if (result.CashDispenser.Positions?.Top is not null &&
                    (bool)result.CashDispenser.Positions?.Top)
                {
                    outputPositions[CashDispenserCapabilitiesClass.OutputPositionEnum.Top] = true;
                }

                Dictionary<CashDispenserCapabilitiesClass.MoveItemEnum, bool> moveItems = new()
                {
                    { CashDispenserCapabilitiesClass.MoveItemEnum.FromCashUnit, false },
                    { CashDispenserCapabilitiesClass.MoveItemEnum.ToCashUnit, false },
                    { CashDispenserCapabilitiesClass.MoveItemEnum.ToStacker, false },
                    { CashDispenserCapabilitiesClass.MoveItemEnum.ToTransport, false }
                };

                if (result.CashDispenser.MoveItems?.FromCashUnit is not null &&
                    (bool)result.CashDispenser.MoveItems?.FromCashUnit)
                {
                    moveItems[CashDispenserCapabilitiesClass.MoveItemEnum.FromCashUnit] = true;
                }
                if (result.CashDispenser.MoveItems?.ToCashUnit is not null &&
                    (bool)result.CashDispenser.MoveItems?.ToCashUnit)
                {
                    moveItems[CashDispenserCapabilitiesClass.MoveItemEnum.ToCashUnit] = true;
                }
                if (result.CashDispenser.MoveItems?.ToStacker is not null &&
                    (bool)result.CashDispenser.MoveItems?.ToStacker)
                {
                    moveItems[CashDispenserCapabilitiesClass.MoveItemEnum.ToStacker] = true;
                }
                if (result.CashDispenser.MoveItems?.ToTransport is not null &&
                    (bool)result.CashDispenser.MoveItems?.ToTransport)
                {
                    moveItems[CashDispenserCapabilitiesClass.MoveItemEnum.ToTransport] = true;
                }

                // Store internal object for other interfaces can be used
                Common.CashDispenserCapabilities = new CashDispenserCapabilitiesClass(result.CashDispenser.Type switch
                                                                                      {
                                                                                          XFS4IoT.Dispenser.CapabilitiesClass.TypeEnum.SelfServiceBill => CashDispenserCapabilitiesClass.TypeEnum.selfServiceBill,
                                                                                          XFS4IoT.Dispenser.CapabilitiesClass.TypeEnum.SelfServiceCoin => CashDispenserCapabilitiesClass.TypeEnum.selfServiceCoin,
                                                                                          XFS4IoT.Dispenser.CapabilitiesClass.TypeEnum.TellerBill => CashDispenserCapabilitiesClass.TypeEnum.tellerBill,
                                                                                          _ => CashDispenserCapabilitiesClass.TypeEnum.tellerCoin
                                                                                      },
                                                                                      result.CashDispenser.MaxDispenseItems is null ? 0 : (int)result.CashDispenser.MaxDispenseItems,
                                                                                      result.CashDispenser.Shutter is not null && (bool)result.CashDispenser.Shutter,
                                                                                      result.CashDispenser.ShutterControl is not null && (bool)result.CashDispenser.ShutterControl,
                                                                                      retractAreas,
                                                                                      retractTransportActions,
                                                                                      retractStackerActions,
                                                                                      result.CashDispenser.IntermediateStacker is not null && (bool)result.CashDispenser.IntermediateStacker,
                                                                                      result.CashDispenser.ItemsTakenSensor is not null && (bool)result.CashDispenser.ItemsTakenSensor,
                                                                                      outputPositions,
                                                                                      moveItems,
                                                                                      result.CashDispenser.PrepareDispense is not null && (bool)result.CashDispenser.PrepareDispense);
            }

            if (result.CashManagement is not null)
            {
                CashManagementCapabilitiesClass.ExchangeTypesEnum exchangeType = CashManagementCapabilitiesClass.ExchangeTypesEnum.NotSupported;
                if (result.CashManagement.ExchangeType?.ByHand is not null && (bool)result.CashManagement.ExchangeType?.ByHand)
                    exchangeType |= CashManagementCapabilitiesClass.ExchangeTypesEnum.ByHand;
                if (result.CashManagement.ExchangeType?.ClearRecycler is not null && (bool)result.CashManagement.ExchangeType?.ClearRecycler)
                    exchangeType |= CashManagementCapabilitiesClass.ExchangeTypesEnum.ClearRecycler;
                if (result.CashManagement.ExchangeType?.DepositInto is not null && (bool)result.CashManagement.ExchangeType?.DepositInto)
                    exchangeType |= CashManagementCapabilitiesClass.ExchangeTypesEnum.DepositInto;
                if (result.CashManagement.ExchangeType?.ToCassettes is not null && (bool)result.CashManagement.ExchangeType?.ToCassettes)
                    exchangeType |= CashManagementCapabilitiesClass.ExchangeTypesEnum.ToCassettes;


                CashManagementCapabilitiesClass.ItemInfoTypesEnum itemInfo = CashManagementCapabilitiesClass.ItemInfoTypesEnum.NotSupported;
                if (result.CashManagement.ItemInfoTypes?.SerialNumber is not null && (bool)result.CashManagement.ItemInfoTypes?.SerialNumber)
                    itemInfo |= CashManagementCapabilitiesClass.ItemInfoTypesEnum.SerialNumber;
                if (result.CashManagement.ItemInfoTypes?.Signature is not null && (bool)result.CashManagement.ItemInfoTypes?.Signature)
                    itemInfo |= CashManagementCapabilitiesClass.ItemInfoTypesEnum.Signature;
                if (result.CashManagement.ItemInfoTypes?.ImageFile is not null && (bool)result.CashManagement.ItemInfoTypes?.ImageFile)
                    itemInfo |= CashManagementCapabilitiesClass.ItemInfoTypesEnum.ImageFile;

                Common.CashManagementCapabilities = new CashManagementCapabilitiesClass(exchangeType,
                                                                                        itemInfo,
                                                                                        result.CashManagement.SafeDoor is not null && (bool)result.CashManagement.SafeDoor,
                                                                                        result.CashManagement.CashBox is not null && (bool)result.CashManagement.CashBox,
                                                                                        result.CashManagement.ClassificationList is not null && (bool)result.CashManagement.ClassificationList,
                                                                                        result.CashManagement.PhysicalNoteList is not null && (bool)result.CashManagement.PhysicalNoteList);
            }

            if (result.CardReader is not null)
            {
                CardReaderCapabilitiesClass.DeviceTypeEnum type = result.CardReader.Type switch
                {
                    XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Contactless => CardReaderCapabilitiesClass.DeviceTypeEnum.Motor,
                    XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Dip => CardReaderCapabilitiesClass.DeviceTypeEnum.Dip,
                    XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.IntelligentContactless => CardReaderCapabilitiesClass.DeviceTypeEnum.IntelligentContactless,
                    XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.LatchedDip => CardReaderCapabilitiesClass.DeviceTypeEnum.LatchedDip,
                    XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Motor => CardReaderCapabilitiesClass.DeviceTypeEnum.Motor,
                    XFS4IoT.CardReader.CapabilitiesClass.TypeEnum.Permanent => CardReaderCapabilitiesClass.DeviceTypeEnum.Permanent,
                    _ => CardReaderCapabilitiesClass.DeviceTypeEnum.Swipe
                };

                CardReaderCapabilitiesClass.ReadableDataTypesEnum readableData = CardReaderCapabilitiesClass.ReadableDataTypesEnum.NotSupported;
                if (result.CardReader.ReadTracks.BackImage is not null && (bool)result.CardReader.ReadTracks.BackImage)
                    readableData |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.BackImage;
                if (result.CardReader.ReadTracks.Ddi is not null && (bool)result.CardReader.ReadTracks.Ddi)
                    readableData |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Ddi;
                if (result.CardReader.ReadTracks.FrontImage is not null && (bool)result.CardReader.ReadTracks.FrontImage)
                    readableData |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.BackImage;
                if (result.CardReader.ReadTracks.FrontTrack1 is not null && (bool)result.CardReader.ReadTracks.FrontTrack1)
                    readableData |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1Front;
                if (result.CardReader.ReadTracks.Track1 is not null && (bool)result.CardReader.ReadTracks.Track1)
                    readableData |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1;
                if (result.CardReader.ReadTracks.Track1JIS is not null && (bool)result.CardReader.ReadTracks.Track1JIS)
                    readableData |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1JIS;
                if (result.CardReader.ReadTracks.Track2 is not null && (bool)result.CardReader.ReadTracks.Track2)
                    readableData |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track2;
                if (result.CardReader.ReadTracks.Track3 is not null && (bool)result.CardReader.ReadTracks.Track3)
                    readableData |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track3;
                if (result.CardReader.ReadTracks.Track3JIS is not null && (bool)result.CardReader.ReadTracks.Track3JIS)
                    readableData |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track3JIS;
                if (result.CardReader.ReadTracks.Watermark is not null && (bool)result.CardReader.ReadTracks.Watermark)
                    readableData |= CardReaderCapabilitiesClass.ReadableDataTypesEnum.Watermark;


                CardReaderCapabilitiesClass.WritableDataTypesEnum writableData = CardReaderCapabilitiesClass.WritableDataTypesEnum.NotSupported;
                if (result.CardReader.WriteTracks?.FrontTrack1 is not null && (bool)result.CardReader.WriteTracks?.FrontTrack1)
                    writableData |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1Front;
                if (result.CardReader.WriteTracks?.Track1 is not null && (bool)result.CardReader.WriteTracks?.Track1)
                    writableData |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1;
                if (result.CardReader.WriteTracks?.Track1JIS is not null && (bool)result.CardReader.WriteTracks?.Track1JIS)
                    writableData |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1JIS;
                if (result.CardReader.WriteTracks?.Track2 is not null && (bool)result.CardReader.WriteTracks?.Track2)
                    writableData |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track2;
                if (result.CardReader.WriteTracks?.Track3 is not null && (bool)result.CardReader.WriteTracks?.Track3)
                    writableData |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3;
                if (result.CardReader.WriteTracks?.Track3JIS is not null && (bool)result.CardReader.WriteTracks?.Track3JIS)
                    writableData |= CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3JIS;

                CardReaderCapabilitiesClass.ChipProtocolsEnum chipProtocols = CardReaderCapabilitiesClass.ChipProtocolsEnum.NotSupported;
                if (result.CardReader.ChipProtocols?.ChipProtocolNotRequired is not null && (bool)result.CardReader.ChipProtocols?.ChipProtocolNotRequired)
                    chipProtocols |= CardReaderCapabilitiesClass.ChipProtocolsEnum.NotRequired;
                if (result.CardReader.ChipProtocols?.ChipT0 is not null && (bool)result.CardReader.ChipProtocols?.ChipT0)
                    chipProtocols |= CardReaderCapabilitiesClass.ChipProtocolsEnum.T0;
                if (result.CardReader.ChipProtocols?.ChipT1 is not null && (bool)result.CardReader.ChipProtocols?.ChipT1)
                    chipProtocols |= CardReaderCapabilitiesClass.ChipProtocolsEnum.T1;
                if (result.CardReader.ChipProtocols?.ChipTypeAPart3 is not null && (bool)result.CardReader.ChipProtocols?.ChipTypeAPart3)
                    chipProtocols |= CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeAPart3;
                if (result.CardReader.ChipProtocols?.ChipTypeAPart4 is not null && (bool)result.CardReader.ChipProtocols?.ChipTypeAPart4)
                    chipProtocols |= CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeAPart4;
                if (result.CardReader.ChipProtocols?.ChipTypeB is not null && (bool)result.CardReader.ChipProtocols?.ChipTypeB)
                    chipProtocols |= CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeB;
                if (result.CardReader.ChipProtocols?.ChipTypeNFC is not null && (bool)result.CardReader.ChipProtocols?.ChipTypeNFC)
                    chipProtocols |= CardReaderCapabilitiesClass.ChipProtocolsEnum.TypeNFC;

                CardReaderCapabilitiesClass.SecurityTypeEnum securityType = CardReaderCapabilitiesClass.SecurityTypeEnum.NotSupported;
                if (result.CardReader.SecurityType is not null)
                {
                    securityType = result.CardReader.SecurityType switch
                    {
                        XFS4IoT.CardReader.CapabilitiesClass.SecurityTypeEnum.Cim86 => CardReaderCapabilitiesClass.SecurityTypeEnum.Cim86,
                        XFS4IoT.CardReader.CapabilitiesClass.SecurityTypeEnum.Mm => CardReaderCapabilitiesClass.SecurityTypeEnum.Mm,
                        _ => CardReaderCapabilitiesClass.SecurityTypeEnum.NotSupported,
                    };
                }

                CardReaderCapabilitiesClass.PowerOptionEnum powerOnAction = CardReaderCapabilitiesClass.PowerOptionEnum.NoAction;
                if (result.CardReader.PowerOnOption is not null)
                {
                    powerOnAction = result.CardReader.PowerOnOption switch
                    {
                        XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.Eject => CardReaderCapabilitiesClass.PowerOptionEnum.Eject,
                        XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.EjectThenRetain => CardReaderCapabilitiesClass.PowerOptionEnum.EjectThenRetain,
                        XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.ReadPosition => CardReaderCapabilitiesClass.PowerOptionEnum.ReadPosition,
                        XFS4IoT.CardReader.CapabilitiesClass.PowerOnOptionEnum.Retain => CardReaderCapabilitiesClass.PowerOptionEnum.Retain,
                        _ => CardReaderCapabilitiesClass.PowerOptionEnum.NoAction,
                    };
                }

                CardReaderCapabilitiesClass.PowerOptionEnum powerOffAction = CardReaderCapabilitiesClass.PowerOptionEnum.NoAction;
                if (result.CardReader.PowerOffOption is not null)
                {
                    powerOnAction = result.CardReader.PowerOffOption switch
                    {
                        XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.Eject => CardReaderCapabilitiesClass.PowerOptionEnum.Eject,
                        XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.EjectThenRetain => CardReaderCapabilitiesClass.PowerOptionEnum.EjectThenRetain,
                        XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.ReadPosition => CardReaderCapabilitiesClass.PowerOptionEnum.ReadPosition,
                        XFS4IoT.CardReader.CapabilitiesClass.PowerOffOptionEnum.Retain => CardReaderCapabilitiesClass.PowerOptionEnum.Retain,
                        _ => CardReaderCapabilitiesClass.PowerOptionEnum.NoAction,
                    };
                }

                CardReaderCapabilitiesClass.WriteMethodsEnum writeModes = CardReaderCapabilitiesClass.WriteMethodsEnum.NotSupported;
                if (result.CardReader.WriteMode?.Auto is not null && (bool)result.CardReader.WriteMode?.Auto)
                    writeModes |= CardReaderCapabilitiesClass.WriteMethodsEnum.Auto;
                if (result.CardReader.WriteMode?.Hico is not null && (bool)result.CardReader.WriteMode?.Hico)
                    writeModes |= CardReaderCapabilitiesClass.WriteMethodsEnum.Hico;
                if (result.CardReader.WriteMode?.Loco is not null && (bool)result.CardReader.WriteMode?.Loco)
                    writeModes |= CardReaderCapabilitiesClass.WriteMethodsEnum.Loco;

                CardReaderCapabilitiesClass.ChipPowerOptionsEnum chipPowers = CardReaderCapabilitiesClass.ChipPowerOptionsEnum.NotSupported;
                if (result.CardReader.ChipPower?.Cold is not null && (bool)result.CardReader.ChipPower?.Cold)
                    chipPowers |= CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Cold;
                if (result.CardReader.ChipPower?.Warm is not null && (bool)result.CardReader.ChipPower?.Warm)
                    chipPowers |= CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Warm;
                if (result.CardReader.ChipPower?.Off is not null && (bool)result.CardReader.ChipPower?.Off)
                    chipPowers |= CardReaderCapabilitiesClass.ChipPowerOptionsEnum.Off;

                CardReaderCapabilitiesClass.MemoryChipProtocolsEnum memChipProtocols = CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.NotSupported;
                if (result.CardReader.MemoryChipProtocols?.Gpm896 is not null && (bool)result.CardReader?.MemoryChipProtocols.Gpm896)
                    memChipProtocols |= CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.Gpm896;
                if (result.CardReader.MemoryChipProtocols?.Siemens4442 is not null && (bool)result.CardReader?.MemoryChipProtocols.Siemens4442)
                    memChipProtocols |= CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.Siemens4442;

                CardReaderCapabilitiesClass.EjectPositionsEnum ejectPositions = CardReaderCapabilitiesClass.EjectPositionsEnum.NotSupported;
                if (result.CardReader.EjectPosition?.Exit is not null && (bool)result.CardReader.EjectPosition?.Exit)
                    ejectPositions |= CardReaderCapabilitiesClass.EjectPositionsEnum.Exit;
                if (result.CardReader.EjectPosition?.Transport is not null && (bool)result.CardReader.EjectPosition?.Transport)
                    ejectPositions |= CardReaderCapabilitiesClass.EjectPositionsEnum.Transport;


                Common.CardReaderCapabilities = new CardReaderCapabilitiesClass(type,
                                                                                readableData,
                                                                                writableData,
                                                                                chipProtocols,
                                                                                result.CardReader.MaxCardCount is null ? 0 : (int)result.CardReader.MaxCardCount,
                                                                                securityType,
                                                                                powerOnAction,
                                                                                powerOffAction,
                                                                                result.CardReader.FluxSensorProgrammable is not null && (bool)result.CardReader.FluxSensorProgrammable,
                                                                                result.CardReader.ReadWriteAccessFollowingEject is not null && (bool)result.CardReader.ReadWriteAccessFollowingEject,
                                                                                writeModes,
                                                                                chipPowers,
                                                                                memChipProtocols,
                                                                                ejectPositions,
                                                                                result.CardReader.NumberParkingStations is null ? 0 : (int)result.CardReader.NumberParkingStations);
            }

            if (result.TextTerminal is not null)
            {
                if (result.TextTerminal.Type is null)
                {
                    Task.FromResult(new CapabilitiesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError,
                                                                           $"The type proeprty is not set."));
                }

                List<Size> resolutions = new();
                if (result.TextTerminal.Resolutions is not null)
                {
                    foreach (var resolution in result.TextTerminal.Resolutions)
                    {
                        if (resolution.SizeX is null ||
                            resolution.SizeY is null)
                        {
                            Task.FromResult(new CapabilitiesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError,
                                                                                   $"The no resolution size specified."));
                        }
                        resolutions.Add(new Size((int)resolution.SizeX, (int)resolution.SizeY));
                    }
                }

                List<TextTerminalCapabilitiesClass.LEDClass> LEDsSupported = new();
                if (result.TextTerminal.Leds is not null)
                {
                    foreach (var led in result.TextTerminal.Leds)
                    {
                        TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum LEDColor = TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.None;
                        if (led.Blue is not null && (bool)led.Blue)
                            LEDColor |= TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Blue;
                        if (led.Cyan is not null && (bool)led.Cyan)
                            LEDColor |= TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Cyan;
                        if (led.Green is not null && (bool)led.Green)
                            LEDColor |= TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Green;
                        if (led.Magenta is not null && (bool)led.Magenta)
                            LEDColor |= TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Magenta;
                        if (led.Red is not null && (bool)led.Red)
                            LEDColor |= TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Red;
                        if (led.White is not null && (bool)led.White)
                            LEDColor |= TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.White;
                        if (led.Yellow is not null && (bool)led.Yellow)
                            LEDColor |= TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Yellow;

                        TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum LEDLight = TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.None;
                        if (led.Continuous is not null && (bool)led.Continuous)
                            LEDLight |= TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.Continuous;
                        if (led.MediumFlash is not null && (bool)led.MediumFlash)
                            LEDLight |= TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.MediumFlash;
                        if (led.Off is not null && (bool)led.Off)
                            LEDLight |= TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.Off;
                        if (led.QuickFlash is not null && (bool)led.QuickFlash)
                            LEDLight |= TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.QuickFlash;
                        if (led.SlowFlash is not null && (bool)led.SlowFlash)
                            LEDLight |= TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.SlowFlash;

                        LEDsSupported.Add(new TextTerminalCapabilitiesClass.LEDClass(LEDColor, LEDLight));
                    }
                }

                Common.TextTerminalCapabilities = new TextTerminalCapabilitiesClass(result.TextTerminal.Type == CapabilitiesClass.TypeEnum.Fixed ? TextTerminalCapabilitiesClass.TypeEnum.Fixed : TextTerminalCapabilitiesClass.TypeEnum.Removable,
                                                                                    resolutions,
                                                                                    result.TextTerminal.KeyLock is not null && (bool)result.TextTerminal.KeyLock,
                                                                                    result.TextTerminal.DisplayLight is not null && (bool)result.TextTerminal.DisplayLight,
                                                                                    result.TextTerminal.Cursor is not null && (bool)result.TextTerminal.Cursor,
                                                                                    result.TextTerminal.Forms is not null && (bool)result.TextTerminal.Forms,
                                                                                    LEDsSupported);
            }

            return Task.FromResult(result);
        }
    }
}
