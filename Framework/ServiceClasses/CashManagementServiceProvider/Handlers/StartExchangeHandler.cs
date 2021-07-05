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
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashManagement
{
    public partial class StartExchangeHandler
    {
        private async Task<StartExchangeCompletion.PayloadData> HandleStartExchange(IStartExchangeEvents events, StartExchangeCommand startExchange, CancellationToken cancel)
        {
            if (CashManagement.CashManagementCapabilities.ExchangeTypes == CashManagementCapabilitiesClass.ExchangeTypesEnum.NotSupported)
            {
                return new StartExchangeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedData,
                                                               "No exchange types define in the capabilites and the device doesn't support exchange operation.");
            }

            if (startExchange.Payload.ExchangeType == StartExchangeCommand.PayloadData.ExchangeTypeEnum.ByHand &&
                (CashManagement.CashManagementCapabilities.ExchangeTypes & CashManagementCapabilitiesClass.ExchangeTypesEnum.ByHand) != CashManagementCapabilitiesClass.ExchangeTypesEnum.ByHand ||
                startExchange.Payload.ExchangeType == StartExchangeCommand.PayloadData.ExchangeTypeEnum.ClearRecycler &&
                (CashManagement.CashManagementCapabilities.ExchangeTypes & CashManagementCapabilitiesClass.ExchangeTypesEnum.ClearRecycler) != CashManagementCapabilitiesClass.ExchangeTypesEnum.ClearRecycler ||
                startExchange.Payload.ExchangeType == StartExchangeCommand.PayloadData.ExchangeTypeEnum.DepositInto &&
                (CashManagement.CashManagementCapabilities.ExchangeTypes & CashManagementCapabilitiesClass.ExchangeTypesEnum.DepositInto) != CashManagementCapabilitiesClass.ExchangeTypesEnum.DepositInto ||
                startExchange.Payload.ExchangeType == StartExchangeCommand.PayloadData.ExchangeTypeEnum.ToCassettes &&
                (CashManagement.CashManagementCapabilities.ExchangeTypes & CashManagementCapabilitiesClass.ExchangeTypesEnum.ToCassettes) != CashManagementCapabilitiesClass.ExchangeTypesEnum.ToCassettes)
            {
                return new StartExchangeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Invalid exchange type specified where the capabilites doesn't support. {startExchange.Payload.ExchangeType}");
            }

            InitiateExchangeResult result = null;
            if (startExchange.Payload.ExchangeType == StartExchangeCommand.PayloadData.ExchangeTypeEnum.ByHand)
            {
                Logger.Log(Constants.DeviceClass, "CashManagementDev.InitiateExchangeAsync()");

                result = await Device.InitiateExchangeAsync(events, new InitiateExchangeRequest(startExchange.Payload.CashunitList), cancel);

                Logger.Log(Constants.DeviceClass, $"CashManagementDev.InitiateExchangeAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            }
            else if (startExchange.Payload.ExchangeType == StartExchangeCommand.PayloadData.ExchangeTypeEnum.ClearRecycler)
            {
                if (startExchange.Payload.Output is null)
                {
                    return new StartExchangeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Not output location specified for the clear recycler type. {startExchange.Payload.ExchangeType}");
                }

                CashDispenserCapabilitiesClass.OutputPositionEnum outpos = CashDispenserCapabilitiesClass.OutputPositionEnum.Default;
                if (startExchange.Payload.Output.Position.Bottom is not null && (bool)startExchange.Payload.Output.Position.Bottom)
                {
                    outpos = CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom;
                }
                else if (startExchange.Payload.Output.Position.Center is not null && (bool)startExchange.Payload.Output.Position.Center)
                {
                    outpos = CashDispenserCapabilitiesClass.OutputPositionEnum.Center;
                }
                else if (startExchange.Payload.Output.Position.Front is not null && (bool)startExchange.Payload.Output.Position.Front)
                {
                    outpos = CashDispenserCapabilitiesClass.OutputPositionEnum.Front;
                }
                else if (startExchange.Payload.Output.Position.Left is not null && (bool)startExchange.Payload.Output.Position.Left)
                {
                    outpos = CashDispenserCapabilitiesClass.OutputPositionEnum.Left;
                }
                else if (startExchange.Payload.Output.Position.Rear is not null && (bool)startExchange.Payload.Output.Position.Rear)
                {
                    outpos = CashDispenserCapabilitiesClass.OutputPositionEnum.Rear;
                }
                else if (startExchange.Payload.Output.Position.Right is not null && (bool)startExchange.Payload.Output.Position.Right)
                {
                    outpos = CashDispenserCapabilitiesClass.OutputPositionEnum.Right;
                }
                else if (startExchange.Payload.Output.Position.Top is not null && (bool)startExchange.Payload.Output.Position.Top)
                {
                    outpos = CashDispenserCapabilitiesClass.OutputPositionEnum.Top;
                }
                else
                {
                    outpos = CashDispenserCapabilitiesClass.OutputPositionEnum.Default;
                }

                Logger.Log(Constants.DeviceClass, "CashManagementDev.InitiateExchangeClearRecyclerAsync()");

                result = await Device.InitiateExchangeClearRecyclerAsync(events, new InitiateClearRecyclerRequest(startExchange.Payload.Output.Cashunit, outpos, startExchange.Payload.Output.TargetCashunit), cancel);

                Logger.Log(Constants.DeviceClass, $"CashManagementDev.InitiateExchangeClearRecyclerAsync() -> {result.CompletionCode}, {result.ErrorCode}");
            }
            else if (startExchange.Payload.ExchangeType == StartExchangeCommand.PayloadData.ExchangeTypeEnum.DepositInto)
            {
                Logger.Log(Constants.DeviceClass, "CashManagementDev.InitiateExchangeDepositIntoAsync()");

                result = await Device.InitiateExchangeDepositIntoAsync(events, cancel);

                Logger.Log(Constants.DeviceClass, $"CashManagementDev.InitiateExchangeDepositIntoAsync() -> {result.CompletionCode}, {result.ErrorCode}");
            }
            else if (startExchange.Payload.ExchangeType == StartExchangeCommand.PayloadData.ExchangeTypeEnum.ToCassettes)
            {
                // NOT SUPPORTED: PREVIEW 4
                // Need another type as we don't know cash move from replenishment unit to cash unit or other way round.
                // This flag is used both XFS CDM/CIM 3.X.
                // CIM Move Cash from Replenishment => Cash Units
                // CDM Move Cash from Cash Units => Replenishment
                return new StartExchangeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                               $"Exchnage type {startExchange.Payload.ExchangeType} is not supported yet.");
            }
            else
            {
                Contracts.Assert(false, $"Unexpected exchange type received. {startExchange.Payload.ExchangeType}");
            }

            if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
            {
                return new StartExchangeCompletion.PayloadData(result.CompletionCode,
                                                               result.ErrorDescription,
                                                               result.ErrorCode);
            }

            Dictionary<string, StartExchangeCompletion.PayloadData.CashunitsClass> xfsUnits = null;

            if (result.CashUnits is not null)
            {
                xfsUnits = new();
                foreach (string unit in result.CashUnits)
                {
                    if (!CashManagement.CashUnits.ContainsKey(unit))
                        continue;
                    StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum xfsStatus = CashManagement.CashUnits[unit].Status switch
                    {
                        CashUnit.StatusEnum.Empty => StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum.Empty,
                        CashUnit.StatusEnum.Full => StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum.Full,
                        CashUnit.StatusEnum.High => StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum.High,
                        CashUnit.StatusEnum.Inoperative => StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum.Inoperative,
                        CashUnit.StatusEnum.Low => StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum.Low,
                        CashUnit.StatusEnum.Manipulated => StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum.Manipulated,
                        CashUnit.StatusEnum.Missing => StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum.Missing,
                        CashUnit.StatusEnum.NoReference => StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum.NoReference,
                        CashUnit.StatusEnum.Ok => StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum.Ok,
                        _ => StartExchangeCompletion.PayloadData.CashunitsClass.StatusEnum.NoValue,
                    };

                    StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum xfsType = CashManagement.CashUnits[unit].Type switch
                    {
                        CashUnit.TypeEnum.BillCassette => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.BillCassette,
                        CashUnit.TypeEnum.CashIn => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.CashIn,
                        CashUnit.TypeEnum.CoinCylinder => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.CoinCylinder,
                        CashUnit.TypeEnum.CoinDispenser => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.CoinDispenser,
                        CashUnit.TypeEnum.Coupon => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.Coupon,
                        CashUnit.TypeEnum.Document => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.Document,
                        CashUnit.TypeEnum.Recycling => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.Recycling,
                        CashUnit.TypeEnum.RejectCassette => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.RejectCassette,
                        CashUnit.TypeEnum.ReplenishmentContainer => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.ReplenishmentContainer,
                        CashUnit.TypeEnum.RetractCassette => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.RetractCassette,
                        _ => StartExchangeCompletion.PayloadData.CashunitsClass.TypeEnum.NotApplicable,
                    };

                    List<StartExchangeCompletion.PayloadData.CashunitsClass.NoteNumberListClass.NoteNumberClass> xfsNoteNumber = new();
                    foreach (BankNoteNumber bn in CashManagement.CashUnits[unit].BankNoteNumberList)
                        xfsNoteNumber.Add(new StartExchangeCompletion.PayloadData.CashunitsClass.NoteNumberListClass.NoteNumberClass(bn.NoteID, bn.Count));

                    xfsUnits.Add(unit, new StartExchangeCompletion.PayloadData.CashunitsClass(xfsStatus,
                                                                                              xfsType,
                                                                                              CashManagement.CashUnits[unit].CurrencyID,
                                                                                              CashManagement.CashUnits[unit].Value,
                                                                                              CashManagement.CashUnits[unit].LogicalCount,
                                                                                              CashManagement.CashUnits[unit].Maximum,
                                                                                              CashManagement.CashUnits[unit].AppLock,
                                                                                              CashManagement.CashUnits[unit].CashUnitName,
                                                                                              CashManagement.CashUnits[unit].InitialCount,
                                                                                              CashManagement.CashUnits[unit].DispensedCount,
                                                                                              CashManagement.CashUnits[unit].PresentedCount,
                                                                                              CashManagement.CashUnits[unit].RetractedCount,
                                                                                              CashManagement.CashUnits[unit].RejectCount,
                                                                                              CashManagement.CashUnits[unit].Minimum,
                                                                                              CashManagement.CashUnits[unit].PhysicalPositionName,
                                                                                              CashManagement.CashUnits[unit].UnitID,
                                                                                              CashManagement.CashUnits[unit].Count,
                                                                                              CashManagement.CashUnits[unit].MaximumCapacity,
                                                                                              CashManagement.CashUnits[unit].HardwareSensor,
                                                                                              new StartExchangeCompletion.PayloadData.CashunitsClass.ItemTypeClass((CashManagement.CashUnits[unit].ItemTypes & CashUnit.ItemTypesEnum.All) == CashUnit.ItemTypesEnum.All,
                                                                                                                                                                   (CashManagement.CashUnits[unit].ItemTypes & CashUnit.ItemTypesEnum.Unfit) == CashUnit.ItemTypesEnum.Unfit,
                                                                                                                                                                   (CashManagement.CashUnits[unit].ItemTypes & CashUnit.ItemTypesEnum.Individual) == CashUnit.ItemTypesEnum.Individual,
                                                                                                                                                                   (CashManagement.CashUnits[unit].ItemTypes & CashUnit.ItemTypesEnum.Level1) == CashUnit.ItemTypesEnum.Level1,
                                                                                                                                                                   (CashManagement.CashUnits[unit].ItemTypes & CashUnit.ItemTypesEnum.Level2) == CashUnit.ItemTypesEnum.Level2,
                                                                                                                                                                   (CashManagement.CashUnits[unit].ItemTypes & CashUnit.ItemTypesEnum.Level3) == CashUnit.ItemTypesEnum.Level3,
                                                                                                                                                                   (CashManagement.CashUnits[unit].ItemTypes & CashUnit.ItemTypesEnum.ItemProcessor) == CashUnit.ItemTypesEnum.ItemProcessor,
                                                                                                                                                                   (CashManagement.CashUnits[unit].ItemTypes & CashUnit.ItemTypesEnum.UnfitIndividual) == CashUnit.ItemTypesEnum.UnfitIndividual),
                                                                                                CashManagement.CashUnits[unit].CashInCount,
                                                                                                new StartExchangeCompletion.PayloadData.CashunitsClass.NoteNumberListClass(xfsNoteNumber),
                                                                                                CashManagement.CashUnits[unit].BanknoteIDs));
                }
            }

            return new StartExchangeCompletion.PayloadData(result.CompletionCode,
                                                           result.ErrorDescription,
                                                           result.ErrorCode,
                                                           xfsUnits);
        }
    }
}
