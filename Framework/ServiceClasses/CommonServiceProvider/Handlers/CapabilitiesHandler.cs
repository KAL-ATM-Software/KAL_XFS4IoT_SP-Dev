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
                                                                                          XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.SelfServiceBill => CashDispenserCapabilitiesClass.TypeEnum.selfServiceBill,
                                                                                          XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.SelfServiceCoin => CashDispenserCapabilitiesClass.TypeEnum.selfServiceCoin,
                                                                                          XFS4IoT.CashDispenser.CapabilitiesClass.TypeEnum.TellerBill => CashDispenserCapabilitiesClass.TypeEnum.tellerBill,
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

                Common.TextTerminalCapabilities = new TextTerminalCapabilitiesClass(result.TextTerminal.Type == CapabilitiesClass.TypeEnum.Fixed ? TextTerminalCapabilitiesClass.TypeEnum.Fixed : TextTerminalCapabilitiesClass.TypeEnum.Removable,
                                                                                    resolutions,
                                                                                    result.TextTerminal.KeyLock is not null && (bool)result.TextTerminal.KeyLock,
                                                                                    result.TextTerminal.Cursor is not null && (bool)result.TextTerminal.Cursor,
                                                                                    result.TextTerminal.Forms is not null && (bool)result.TextTerminal.Forms);
            }


            if (result.KeyManagement is not null)
            {
                KeyManagementCapabilitiesClass.KeyCheckModeEnum keyCheckModes = KeyManagementCapabilitiesClass.KeyCheckModeEnum.NotSupported;
                if (result.KeyManagement.KeyCheckModes?.Self is not null && (bool)result.KeyManagement.KeyCheckModes.Self)
                    keyCheckModes |= KeyManagementCapabilitiesClass.KeyCheckModeEnum.Self;
                if (result.KeyManagement.KeyCheckModes?.Zero is not null && (bool)result.KeyManagement.KeyCheckModes.Zero)
                    keyCheckModes |= KeyManagementCapabilitiesClass.KeyCheckModeEnum.Zero;

                KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum rsaAuthenticationScheme = KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum.NotSupported;
                if (result.KeyManagement.RsaAuthenticationScheme?.Number2partySig is not null && (bool)result.KeyManagement.RsaAuthenticationScheme.Number2partySig)
                    rsaAuthenticationScheme |= KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum.SecondPartySignature;
                if (result.KeyManagement.RsaAuthenticationScheme?.Number3partyCert is not null && (bool)result.KeyManagement.RsaAuthenticationScheme.Number3partyCert)
                    rsaAuthenticationScheme |= KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum.ThirdPartyCertificate;
                if (result.KeyManagement.RsaAuthenticationScheme?.Number3partyCertTr34 is not null && (bool)result.KeyManagement.RsaAuthenticationScheme.Number3partyCertTr34)
                    rsaAuthenticationScheme |= KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum.ThirdPartyCertificateTR34;

                KeyManagementCapabilitiesClass.RSASignatureAlgorithmEnum rsaSignatureAlgorithms = KeyManagementCapabilitiesClass.RSASignatureAlgorithmEnum.NotSupported;
                if (result.KeyManagement.RsaSignatureAlgorithm?.Pkcs1V15 is not null && (bool)result.KeyManagement.RsaSignatureAlgorithm.Pkcs1V15)
                    rsaSignatureAlgorithms |= KeyManagementCapabilitiesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5;
                if (result.KeyManagement.RsaSignatureAlgorithm?.Pss is not null && (bool)result.KeyManagement.RsaSignatureAlgorithm.Pss)
                    rsaSignatureAlgorithms |= KeyManagementCapabilitiesClass.RSASignatureAlgorithmEnum.RSASSA_PSS;

                KeyManagementCapabilitiesClass.RSACryptAlgorithmEnum rsaCryptAlgorithms = KeyManagementCapabilitiesClass.RSACryptAlgorithmEnum.NotSupported;
                if (result.KeyManagement.RsaCryptAlgorithm?.Pkcs1V15 is not null && (bool)result.KeyManagement.RsaCryptAlgorithm.Pkcs1V15)
                    rsaCryptAlgorithms |= KeyManagementCapabilitiesClass.RSACryptAlgorithmEnum.RSAES_PKCS1_V1_5;
                if (result.KeyManagement.RsaCryptAlgorithm?.Oaep is not null && (bool)result.KeyManagement.RsaCryptAlgorithm.Oaep)
                    rsaCryptAlgorithms |= KeyManagementCapabilitiesClass.RSACryptAlgorithmEnum.RSAES_OAEP;

                KeyManagementCapabilitiesClass.RSAKeyCheckModeEnum rsaKeyCheckModes = KeyManagementCapabilitiesClass.RSAKeyCheckModeEnum.NotSupported;
                if (result.KeyManagement.RsaKeyCheckMode?.Sha1 is not null && (bool)result.KeyManagement.RsaKeyCheckMode.Sha1)
                    rsaKeyCheckModes |= KeyManagementCapabilitiesClass.RSAKeyCheckModeEnum.SHA1;
                if (result.KeyManagement.RsaKeyCheckMode?.Sha256 is not null && (bool)result.KeyManagement.RsaKeyCheckMode.Sha256)
                    rsaKeyCheckModes |= KeyManagementCapabilitiesClass.RSAKeyCheckModeEnum.SHA256;

                KeyManagementCapabilitiesClass.SignatureSchemeEnum signatureScheme = KeyManagementCapabilitiesClass.SignatureSchemeEnum.NotSupported;
                if (result.KeyManagement.SignatureScheme?.EnhancedRkl is not null && (bool)result.KeyManagement.SignatureScheme.EnhancedRkl)
                    signatureScheme |= KeyManagementCapabilitiesClass.SignatureSchemeEnum.EnhancedRKL; 
                if (result.KeyManagement.SignatureScheme?.ExportEppId is not null && (bool)result.KeyManagement.SignatureScheme.ExportEppId)
                    signatureScheme |= KeyManagementCapabilitiesClass.SignatureSchemeEnum.ExportEPPID;
                if (result.KeyManagement.SignatureScheme?.GenRsaKeyPair is not null && (bool)result.KeyManagement.SignatureScheme.GenRsaKeyPair)
                    signatureScheme |= KeyManagementCapabilitiesClass.SignatureSchemeEnum.RSAKeyPair;
                if (result.KeyManagement.SignatureScheme?.RandomNumber is not null && (bool)result.KeyManagement.SignatureScheme.RandomNumber)
                    signatureScheme |= KeyManagementCapabilitiesClass.SignatureSchemeEnum.RandomNumber;

                KeyManagementCapabilitiesClass.EMVImportSchemeEnum emvImportScheme = KeyManagementCapabilitiesClass.EMVImportSchemeEnum.NotSupported;
                if (result.KeyManagement.EmvImportSchemes?.ChksumCA is not null && (bool)result.KeyManagement.EmvImportSchemes.ChksumCA)
                    emvImportScheme |= KeyManagementCapabilitiesClass.EMVImportSchemeEnum.ChecksumCA;
                if (result.KeyManagement.EmvImportSchemes?.EpiCA is not null && (bool)result.KeyManagement.EmvImportSchemes.EpiCA)
                    emvImportScheme |= KeyManagementCapabilitiesClass.EMVImportSchemeEnum.EPICA;
                if (result.KeyManagement.EmvImportSchemes?.Icc is not null && (bool)result.KeyManagement.EmvImportSchemes.Icc)
                    emvImportScheme |= KeyManagementCapabilitiesClass.EMVImportSchemeEnum.ICC;
                if (result.KeyManagement.EmvImportSchemes?.IccPin is not null && (bool)result.KeyManagement.EmvImportSchemes.IccPin)
                    emvImportScheme |= KeyManagementCapabilitiesClass.EMVImportSchemeEnum.ICCPIN;
                if (result.KeyManagement.EmvImportSchemes?.Issuer is not null && (bool)result.KeyManagement.EmvImportSchemes.Issuer)
                    emvImportScheme |= KeyManagementCapabilitiesClass.EMVImportSchemeEnum.Issuer;
                if (result.KeyManagement.EmvImportSchemes?.Pkcsv15CA is not null && (bool)result.KeyManagement.EmvImportSchemes.Pkcsv15CA)
                    emvImportScheme |= KeyManagementCapabilitiesClass.EMVImportSchemeEnum.PKCSV1_5CA;
                if (result.KeyManagement.EmvImportSchemes?.PlainCA is not null && (bool)result.KeyManagement.EmvImportSchemes.PlainCA)
                    emvImportScheme |= KeyManagementCapabilitiesClass.EMVImportSchemeEnum.PlainCA;

                KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum keyblockImportformats = KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.NotSupported;
                if (result.KeyManagement.KeyBlockImportFormats?.AnsTr31KeyBlock is not null && (bool)result.KeyManagement.KeyBlockImportFormats.AnsTr31KeyBlock)
                    keyblockImportformats |= KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKA;
                if (result.KeyManagement.KeyBlockImportFormats?.AnsTr31KeyBlockB is not null && (bool)result.KeyManagement.KeyBlockImportFormats.AnsTr31KeyBlockB)
                    keyblockImportformats |= KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKB;
                if (result.KeyManagement.KeyBlockImportFormats?.AnsTr31KeyBlockC is not null && (bool)result.KeyManagement.KeyBlockImportFormats.AnsTr31KeyBlockC)
                    keyblockImportformats |= KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum.KEYBLOCKC;

                KeyManagementCapabilitiesClass.DESKeyLengthEmum desKeyLength = KeyManagementCapabilitiesClass.DESKeyLengthEmum.NotSupported;
                if (result.KeyManagement.DesKeyLength?.Double is not null && (bool)result.KeyManagement.DesKeyLength.Double)
                    desKeyLength |= KeyManagementCapabilitiesClass.DESKeyLengthEmum.Double;
                if (result.KeyManagement.DesKeyLength?.Single is not null && (bool)result.KeyManagement.DesKeyLength.Single)
                    desKeyLength |= KeyManagementCapabilitiesClass.DESKeyLengthEmum.Single;
                if (result.KeyManagement.DesKeyLength?.Triple is not null && (bool)result.KeyManagement.DesKeyLength.Triple)
                    desKeyLength |= KeyManagementCapabilitiesClass.DESKeyLengthEmum.Triple;

                KeyManagementCapabilitiesClass.CertificateTypeEnum certTypes = KeyManagementCapabilitiesClass.CertificateTypeEnum.NotSupported;
                if (result.KeyManagement.CertificateTypes?.EncKey is not null && (bool)result.KeyManagement.CertificateTypes.EncKey)
                    certTypes |= KeyManagementCapabilitiesClass.CertificateTypeEnum.EncKey;
                if (result.KeyManagement.CertificateTypes?.HostKey is not null && (bool)result.KeyManagement.CertificateTypes.HostKey)
                    certTypes |= KeyManagementCapabilitiesClass.CertificateTypeEnum.HostKey;
                if (result.KeyManagement.CertificateTypes?.VerificationKey is not null && (bool)result.KeyManagement.CertificateTypes.VerificationKey)
                    certTypes |= KeyManagementCapabilitiesClass.CertificateTypeEnum.VerificationKey;

                List<KeyManagementCapabilitiesClass.SingerCapabilities> loadCertOptions = new ();
                if (result.KeyManagement is not null)
                {
                    foreach (var option in result.KeyManagement.LoadCertOptions)
                    {
                        if (option.Signer is not null && option.Option is not null)
                        {
                            KeyManagementCapabilitiesClass.LoadCertificateSignerEnum singer = option.Signer switch
                            {
                                XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.Ca => KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CA,
                                XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.CaTr34 => KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CA,
                                XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.CertHost => KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CertHost,
                                XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.CertHostTr34 => KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.CertHost,
                                XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.Hl => KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.HL,
                                XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.HlTr34 => KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.HL,
                                XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.SigHost => KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.SigHost,
                                _ => KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.NotSupported,
                            };

                            if (singer == KeyManagementCapabilitiesClass.LoadCertificateSignerEnum.NotSupported)
                            {
                                return Task.FromResult(new CapabilitiesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError,
                                                                                              $"Invalid signer specified for loading certificate. {option.Signer}"));
                            }
                            bool tr34 = (option.Signer == XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.CaTr34 ||
                                         option.Signer == XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.CertHostTr34 ||
                                         option.Signer == XFS4IoT.KeyManagement.CapabilitiesClass.LoadCertOptionsClass.SignerEnum.HlTr34);

                            KeyManagementCapabilitiesClass.LoadCertificateOptionEnum loadCertCapOptions = KeyManagementCapabilitiesClass.LoadCertificateOptionEnum.NotSupported;
                            if (option.Option.NewHost is not null && (bool)option.Option.NewHost)
                                loadCertCapOptions |= KeyManagementCapabilitiesClass.LoadCertificateOptionEnum.NewHost;
                            if (option.Option.ReplaceHost is not null && (bool)option.Option.ReplaceHost)
                                loadCertCapOptions |= KeyManagementCapabilitiesClass.LoadCertificateOptionEnum.ReplaceHost;

                            if (loadCertCapOptions == KeyManagementCapabilitiesClass.LoadCertificateOptionEnum.NotSupported)
                            {
                                return Task.FromResult(new CapabilitiesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError,
                                                                                              $"Invalid option specified for loading certificate. {option.Option}"));
                            }

                            loadCertOptions.Add(new KeyManagementCapabilitiesClass.SingerCapabilities(singer, loadCertCapOptions, tr34));
                        }
                    }
                }

                KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum symmetricKeyMethods = KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum.NotSupported;
                if (result.KeyManagement.SymmetricKeyManagementMethods?.FixedKey is not null && (bool)result.KeyManagement.SymmetricKeyManagementMethods.FixedKey)
                    symmetricKeyMethods |= KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum.FixedKey;
                if (result.KeyManagement.SymmetricKeyManagementMethods?.MasterKey is not null && (bool)result.KeyManagement.SymmetricKeyManagementMethods.MasterKey)
                    symmetricKeyMethods |= KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum.MasterKey;
                if (result.KeyManagement.SymmetricKeyManagementMethods?.TdesDukpt is not null && (bool)result.KeyManagement.SymmetricKeyManagementMethods.TdesDukpt)
                    symmetricKeyMethods |= KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum.TripleDESDUKPT;

                Dictionary<string, Dictionary<string, Dictionary<string, KeyManagementCapabilitiesClass.KeyAttributeOptionClass>>> keyAttributes = new();
                
                if (result.KeyManagement.KeyAttributes is not null && result.KeyManagement.KeyAttributes.Count > 0)
                { 
                    foreach (var (keyUsage, algorithms) in result.KeyManagement.KeyAttributes)
                    {
                        Dictionary<string, Dictionary<string, KeyManagementCapabilitiesClass.KeyAttributeOptionClass>> dicAttributes = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, KeyManagementCapabilitiesClass.KeyAttributeOptionClass> dicModeOfUse = new();
                            foreach (var (modeOfUse, restrict) in modeOfUses)
                            {
                                KeyManagementCapabilitiesClass.KeyAttributeOptionClass restricted = new(string.Empty);
                                if (restrict is not null)
                                {
                                    restricted = new(restrict.Restricted);
                                }

                                dicModeOfUse.Add(modeOfUse, restricted);
                            }
                            dicAttributes.Add(algorithm, dicModeOfUse);
                        }
                        keyAttributes.Add(keyUsage, dicAttributes);
                    }
                }
                
                Dictionary<string, KeyManagementCapabilitiesClass.DecryptMethodClass> decryptAttributes = new();
                if (result.KeyManagement.DecryptAttributes is not null && result.KeyManagement.DecryptAttributes.Count > 0)
                {
                    foreach (var (algorithm, method) in result.KeyManagement.DecryptAttributes)
                    {
                        KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum decryptMethod = KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.NotSupported;
                        if (method?.DecryptMethod?.Cbc is not null && (bool)method?.DecryptMethod?.Cbc)
                            decryptMethod |= KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CBC;
                        if (method?.DecryptMethod?.Cfb is not null && (bool)method?.DecryptMethod?.Cfb)
                            decryptMethod |= KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CFB;
                        if (method?.DecryptMethod?.Ctr is not null && (bool)method?.DecryptMethod?.Ctr)
                            decryptMethod |= KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CTR;
                        if (method?.DecryptMethod?.Ecb is not null && (bool)method?.DecryptMethod?.Ecb)
                            decryptMethod |= KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.ECB;
                        if (method?.DecryptMethod?.Ofb is not null && (bool)method?.DecryptMethod?.Ofb)
                            decryptMethod |= KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.OFB;
                        if (method?.DecryptMethod?.RsaesOaep is not null && (bool)method?.DecryptMethod?.RsaesOaep)
                            decryptMethod |= KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.RSAES_OAEP;
                        if (method?.DecryptMethod?.RsaesPkcs1V15 is not null && (bool)method?.DecryptMethod?.RsaesPkcs1V15)
                            decryptMethod |= KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.RSAES_PKCS1_V1_5;
                        if (method?.DecryptMethod?.Xts is not null && (bool)method?.DecryptMethod?.Xts)
                            decryptMethod |= KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.XTS;

                        decryptAttributes.Add(algorithm, new KeyManagementCapabilitiesClass.DecryptMethodClass(decryptMethod));
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, KeyManagementCapabilitiesClass.VerifyMethodClass>>> verifyAttributes = new();
                if (result.KeyManagement.VerifyAttributes is not null && result.KeyManagement.VerifyAttributes.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in result.KeyManagement.VerifyAttributes)
                    {
                        Dictionary<string, Dictionary<string, KeyManagementCapabilitiesClass.VerifyMethodClass>> dicAttributes = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, KeyManagementCapabilitiesClass.VerifyMethodClass> dicModeOfUse = new();
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                            KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum cryptoMethod = KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.NotSupported;
                            if (method?.CryptoMethod?.KcvNone is not null && (bool)method?.CryptoMethod?.KcvNone)
                                cryptoMethod |= KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVNone;
                            if (method?.CryptoMethod?.KcvSelf is not null && (bool)method?.CryptoMethod?.KcvSelf)
                                cryptoMethod |= KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVSelf;
                            if (method?.CryptoMethod?.KcvZero is not null && (bool)method?.CryptoMethod?.KcvZero)
                                cryptoMethod |= KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVZero;
                            if (method?.CryptoMethod?.RsassaPkcs1V15 is not null && (bool)method?.CryptoMethod?.RsassaPkcs1V15)
                                cryptoMethod |= KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.RSASSA_PKCS1_V1_5;
                            if (method?.CryptoMethod?.RsassaPss is not null && (bool)method?.CryptoMethod?.RsassaPss)
                                cryptoMethod |= KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.RSASSA_PSS;
                            if (method?.CryptoMethod?.SigNone is not null && (bool)method?.CryptoMethod?.SigNone)
                                cryptoMethod |= KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.SignatureNone;

                            KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum hashAlgorithm = KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum.NotSupported;
                            if (method?.HashAlgorithm?.Sha1 is not null && (bool)method?.HashAlgorithm?.Sha1)
                                hashAlgorithm |= KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum.SHA1;
                            if (method?.HashAlgorithm?.Sha256 is not null && (bool)method?.HashAlgorithm?.Sha256)
                                hashAlgorithm |= KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum.SHA256;

                                dicModeOfUse.Add(modeOfUse, new KeyManagementCapabilitiesClass.VerifyMethodClass(cryptoMethod, hashAlgorithm));
                            }
                            dicAttributes.Add(algorithm, dicModeOfUse);
                        }
                        verifyAttributes.Add(keyUsage, dicAttributes);
                    }
                }

                Common.KeyManagementCapabilities = new KeyManagementCapabilitiesClass(result.KeyManagement.KeyNum is not null ? (int)result.KeyManagement.KeyNum : 0,
                                                                                      keyCheckModes,
                                                                                      result.KeyManagement.HsmVendor,
                                                                                      rsaAuthenticationScheme,
                                                                                      rsaSignatureAlgorithms,
                                                                                      rsaCryptAlgorithms,
                                                                                      rsaKeyCheckModes,
                                                                                      signatureScheme,
                                                                                      emvImportScheme,
                                                                                      keyblockImportformats,
                                                                                      result.KeyManagement.KeyImportThroughParts is not null ? (bool)result.KeyManagement.KeyImportThroughParts : false,
                                                                                      desKeyLength,
                                                                                      certTypes,
                                                                                      loadCertOptions,
                                                                                      symmetricKeyMethods,
                                                                                      keyAttributes,
                                                                                      decryptAttributes,
                                                                                      verifyAttributes);
            }

            if (result.Crypto is not null)
            {
                CryptoCapabilitiesClass.EMVHashAlgorithmEnum emvHashAlgorithms = CryptoCapabilitiesClass.EMVHashAlgorithmEnum.NotSupported;
                if (result.Crypto.EmvHashAlgorithm?.Sha1Digest is not null && (bool)result.Crypto.EmvHashAlgorithm.Sha1Digest)
                    emvHashAlgorithms |= CryptoCapabilitiesClass.EMVHashAlgorithmEnum.SHA1_Digest;
                if (result.Crypto.EmvHashAlgorithm?.Sha256Digest is not null && (bool)result.Crypto.EmvHashAlgorithm.Sha256Digest)
                    emvHashAlgorithms |= CryptoCapabilitiesClass.EMVHashAlgorithmEnum.SHA256_Digest;


                Dictionary<string, Dictionary<string, Dictionary<string, CryptoCapabilitiesClass.CryptoAttributesClass>>> cryptoAttributes = new();
                
                if (result.Crypto.CryptoAttributes is not null && result.Crypto.CryptoAttributes.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in result.Crypto.CryptoAttributes)
                    {
                        Dictionary<string, Dictionary<string, CryptoCapabilitiesClass.CryptoAttributesClass>> dicAttributes = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, CryptoCapabilitiesClass.CryptoAttributesClass> dicModeOfUse = new();
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum cryptoMethod = CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.NotSupported;
                                if (method?.CryptoMethod?.Cbc is not null && (bool)method?.CryptoMethod?.Cbc)
                                    cryptoMethod |= CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CBC;
                                if (method?.CryptoMethod?.Cfb is not null && (bool)method?.CryptoMethod?.Cfb)
                                    cryptoMethod |= CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CFB;
                                if (method?.CryptoMethod?.Ctr is not null && (bool)method?.CryptoMethod?.Ctr)
                                    cryptoMethod |= CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CTR;
                                if (method?.CryptoMethod?.Ecb is not null && (bool)method?.CryptoMethod?.Ecb)
                                    cryptoMethod |= CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB;
                                if (method?.CryptoMethod?.Ofb is not null && (bool)method?.CryptoMethod?.Ofb)
                                    cryptoMethod |= CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.OFB;
                                if (method?.CryptoMethod?.RsaesOaep is not null && (bool)method?.CryptoMethod?.RsaesOaep)
                                    cryptoMethod |= CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.RSAES_OAEP;
                                if (method?.CryptoMethod?.RsaesPkcs1V15 is not null && (bool)method?.CryptoMethod?.RsaesPkcs1V15)
                                    cryptoMethod |= CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.RSAES_PKCS1_V1_5;
                                if (method?.CryptoMethod?.Xts is not null && (bool)method?.CryptoMethod?.Xts)
                                    cryptoMethod |= CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.XTS;

                                dicModeOfUse.Add(modeOfUse, new CryptoCapabilitiesClass.CryptoAttributesClass(cryptoMethod));
                            }
                            dicAttributes.Add(algorithm, dicModeOfUse);
                        }
                        cryptoAttributes.Add(keyUsage, dicAttributes);
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass>>> authenticationAttributes = new();
                if (result.Crypto.AuthenticationAttributes is not null && result.Crypto.AuthenticationAttributes.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in result.Crypto.AuthenticationAttributes)
                    {
                        Dictionary<string, Dictionary<string, CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass>> dicAttributes = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass> dicModeOfUse = new();
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum cryptoAlgorithm = CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.NotSupported;
                                if (method?.CryptoMethod?.RsassaPkcs1V15 is not null && (bool)method?.CryptoMethod?.RsassaPkcs1V15)
                                    cryptoAlgorithm |= CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5;
                                if (method?.CryptoMethod?.RsassaPss is not null && (bool)method?.CryptoMethod?.RsassaPss)
                                    cryptoAlgorithm |= CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PSS;
                                
                                CryptoCapabilitiesClass.HashAlgorithmEnum hashAlgorithm = CryptoCapabilitiesClass.HashAlgorithmEnum.NotSupported;

                                dicModeOfUse.Add(modeOfUse, new CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass(cryptoAlgorithm, hashAlgorithm));
                            }
                            dicAttributes.Add(algorithm, dicModeOfUse);
                        }
                        authenticationAttributes.Add(keyUsage, dicAttributes);
                    }
                }

                Dictionary<string, Dictionary<string, Dictionary<string, CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass>>> verifyAttributes = new();
                if (result.Crypto.VerifyAttributes is not null && result.Crypto.VerifyAttributes.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in result.Crypto.VerifyAttributes)
                    {
                        Dictionary<string, Dictionary<string, CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass>> dicAttributes = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass> dicModeOfUse = new();
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum cryptoAlgorithm = CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.NotSupported;
                                if (method?.CryptoMethod?.RsassaPkcs1V15 is not null && (bool)method?.CryptoMethod?.RsassaPkcs1V15)
                                    cryptoAlgorithm |= CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5;
                                if (method?.CryptoMethod?.RsassaPss is not null && (bool)method?.CryptoMethod?.RsassaPss)
                                    cryptoAlgorithm |= CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PSS;

                                CryptoCapabilitiesClass.HashAlgorithmEnum hashAlgorithm = CryptoCapabilitiesClass.HashAlgorithmEnum.NotSupported;

                                dicModeOfUse.Add(modeOfUse, new CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass(cryptoAlgorithm, hashAlgorithm));
                            }
                            dicAttributes.Add(algorithm, dicModeOfUse);
                        }
                        verifyAttributes.Add(keyUsage, dicAttributes);
                    }
                }

                Common.CryptoCapabilities = new CryptoCapabilitiesClass(emvHashAlgorithms,
                                                                        cryptoAttributes,
                                                                        authenticationAttributes,
                                                                        verifyAttributes);
            }

            if (result.PinPad is not null)
            {
                PinPadCapabilitiesClass.PINFormatEnum formats = PinPadCapabilitiesClass.PINFormatEnum.NotSupported;
                if (result.PinPad.PinFormats?.Ansi is not null && (bool)result.PinPad.PinFormats.Ansi)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.ANSI;
                if (result.PinPad.PinFormats?.Ap is not null && (bool)result.PinPad.PinFormats.Ap)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.AP;
                if (result.PinPad.PinFormats?.Diebold is not null && (bool)result.PinPad.PinFormats.Diebold)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.DIEBOLD;
                if (result.PinPad.PinFormats?.DieboldCo is not null && (bool)result.PinPad.PinFormats.DieboldCo)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.DIEBOLDCO;
                if (result.PinPad.PinFormats?.Eci2 is not null && (bool)result.PinPad.PinFormats.Eci2)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.ECI2;
                if (result.PinPad.PinFormats?.Eci3 is not null && (bool)result.PinPad.PinFormats.Eci3)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.ECI3;
                if (result.PinPad.PinFormats?.Emv is not null && (bool)result.PinPad.PinFormats.Emv)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.EMV;
                if (result.PinPad.PinFormats?.Ibm3624 is not null && (bool)result.PinPad.PinFormats.Ibm3624)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.IBM3624;
                if (result.PinPad.PinFormats?.Iso0 is not null && (bool)result.PinPad.PinFormats.Iso0)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.ISO0;
                if (result.PinPad.PinFormats?.Iso1 is not null && (bool)result.PinPad.PinFormats.Iso1)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.ISO1;
                if (result.PinPad.PinFormats?.Iso3 is not null && (bool)result.PinPad.PinFormats.Iso3)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.ISO3;
                if (result.PinPad.PinFormats?.Visa is not null && (bool)result.PinPad.PinFormats.Visa)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.VISA;
                if (result.PinPad.PinFormats?.Visa3 is not null && (bool)result.PinPad.PinFormats.Visa3)
                    formats |= PinPadCapabilitiesClass.PINFormatEnum.VISA3;

                PinPadCapabilitiesClass.PresentationAlgorithmEnum presentationAlgorithms = PinPadCapabilitiesClass.PresentationAlgorithmEnum.NotSupported;
                if (result.PinPad.PresentationAlgorithms?.PresentClear is not null && (bool)result.PinPad.PresentationAlgorithms.PresentClear)
                    presentationAlgorithms |= PinPadCapabilitiesClass.PresentationAlgorithmEnum.PresentClear;

                PinPadCapabilitiesClass.DisplayTypeEnum displayTypes = PinPadCapabilitiesClass.DisplayTypeEnum.NotSupported;
                if (result.PinPad.Display?.LedThrough is not null && (bool)result.PinPad.Display.LedThrough)
                    displayTypes |= PinPadCapabilitiesClass.DisplayTypeEnum.LEDThrough;
                if (result.PinPad.Display?.Display is not null && (bool)result.PinPad.Display.Display)
                    displayTypes |= PinPadCapabilitiesClass.DisplayTypeEnum.Display;

                PinPadCapabilitiesClass.ValidationAlgorithmEnum validationAlgorithms = PinPadCapabilitiesClass.ValidationAlgorithmEnum.NotSupported;
                if (result.PinPad.ValidationAlgorithms?.Des is not null && (bool)result.PinPad.ValidationAlgorithms.Des)
                    validationAlgorithms |= PinPadCapabilitiesClass.ValidationAlgorithmEnum.DES;
                if (result.PinPad.ValidationAlgorithms?.Visa is not null && (bool)result.PinPad.ValidationAlgorithms.Visa)
                    validationAlgorithms |= PinPadCapabilitiesClass.ValidationAlgorithmEnum.VISA;

                Dictionary<string, Dictionary<string, Dictionary<string, PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm>>> pinBlockAttributes = new();
                if (result.PinPad.PinBlockAttributes is not null && result.PinPad.PinBlockAttributes.Count > 0)
                {
                    foreach (var (keyUsage, algorithms) in result.PinPad.PinBlockAttributes)
                    {
                        Dictionary<string, Dictionary<string, PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm>> pinAlgorithms = new();
                        foreach (var (algorithm, modeOfUses) in algorithms)
                        {
                            Dictionary<string, PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm> pinModeOfUse = new();
                            foreach (var (modeOfUse, method) in modeOfUses)
                            {
                                PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum encAlgorithm = PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.NotSupported;
                                if (method?.CryptoMethod?.Cbc is not null && (bool)method?.CryptoMethod?.Cbc)
                                    encAlgorithm |= PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.CBC;
                                if (method?.CryptoMethod?.Cfb is not null && (bool)method?.CryptoMethod?.Cfb)
                                    encAlgorithm |= PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.CFB;
                                if (method?.CryptoMethod?.Ctr is not null && (bool)method?.CryptoMethod?.Ctr)
                                    encAlgorithm |= PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.CTR;
                                if (method?.CryptoMethod?.Ecb is not null && (bool)method?.CryptoMethod?.Ecb)
                                    encAlgorithm |= PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.ECB;
                                if (method?.CryptoMethod?.Ofb is not null && (bool)method?.CryptoMethod?.Ofb)
                                    encAlgorithm |= PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.OFB;
                                if (method?.CryptoMethod?.Xts is not null && (bool)method?.CryptoMethod?.Xts)
                                    encAlgorithm |= PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.XTS;
                                if (method?.CryptoMethod?.RsaesOaep is not null && (bool)method?.CryptoMethod?.RsaesOaep)
                                    encAlgorithm |= PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.RSAES_OAEP;
                                if (method?.CryptoMethod?.RsaesPkcs1V15 is not null && (bool)method?.CryptoMethod?.RsaesPkcs1V15)
                                    encAlgorithm |= PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm.EncryptionAlgorithmEnum.RSAES_PKCS1_V1_5;

                                pinModeOfUse.Add(modeOfUse, new PinPadCapabilitiesClass.PinBlockEncryptionAlgorithm(encAlgorithm));
                            }
                            pinAlgorithms.Add(algorithm, pinModeOfUse);
                        }
                        pinBlockAttributes.Add(keyUsage, pinAlgorithms);
                    }
                }

                Common.PinPadCapabilities = new PinPadCapabilitiesClass(formats,
                                                                        presentationAlgorithms,
                                                                        displayTypes,
                                                                        result.PinPad.IdcConnect is not null && (bool)result.PinPad.IdcConnect,
                                                                        validationAlgorithms,
                                                                        result.PinPad.PinCanPersistAfterUse is not null && (bool)result.PinPad.PinCanPersistAfterUse,
                                                                        result.PinPad.TypeCombined is not null && (bool)result.PinPad.TypeCombined,
                                                                        result.PinPad.SetPinblockDataRequired is not null && (bool)result.PinPad.SetPinblockDataRequired,
                                                                        pinBlockAttributes);
            }

            if (result.Keyboard is not null)
            {
            }

            return Task.FromResult(result);
        }
    }
}
