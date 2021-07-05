/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashManagement
{
    public partial class SetCashUnitInfoHandler
    {
        private async Task<SetCashUnitInfoCompletion.PayloadData> HandleSetCashUnitInfo(ISetCashUnitInfoEvents events, SetCashUnitInfoCommand setCashUnitInfo, CancellationToken cancel)
        {
            if (setCashUnitInfo.Payload.Cashunits is null ||
                setCashUnitInfo.Payload.Cashunits.Count == 0)
            {
                return new SetCashUnitInfoCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"No cash unit information is set in the payload to update cash unit counts or configuration.");
            }

            Dictionary<string, SetCashUnitInfoRequest.SetCashUnitConfiguration> cashUnitConfigs = new();
            Dictionary<string, SetCashUnitInfoRequest.SetCashUnitAccounting> cashUnitAccountings = new();
            foreach (var unit in setCashUnitInfo.Payload.Cashunits)
            {
                if (string.IsNullOrEmpty(unit.Key))
                {
                    return new SetCashUnitInfoCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                     $"No key name is set to update cash unit information. {unit.Key}");
                }

                CashUnit.ItemTypesEnum? itemTypes = null;
                if (unit.Value.ItemType is not null)
                {
                    if ((bool)unit.Value.ItemType.All)
                    {
                        itemTypes |= CashUnit.ItemTypesEnum.All;
                    }
                    if ((bool)unit.Value.ItemType.Individual)
                    {
                        itemTypes |= CashUnit.ItemTypesEnum.Individual;
                    }
                    if ((bool)unit.Value.ItemType.ItemProcessor)
                    {
                        itemTypes |= CashUnit.ItemTypesEnum.ItemProcessor;
                    }
                    if ((bool)unit.Value.ItemType.Level1)
                    {
                        itemTypes |= CashUnit.ItemTypesEnum.Level1;
                    }
                    if ((bool)unit.Value.ItemType.Level2)
                    {
                        itemTypes |= CashUnit.ItemTypesEnum.Level2;
                    }
                    if ((bool)unit.Value.ItemType.Level3)
                    {
                        itemTypes |= CashUnit.ItemTypesEnum.Level3;
                    }
                    if ((bool)unit.Value.ItemType.Unfit)
                    {
                        itemTypes |= CashUnit.ItemTypesEnum.Unfit;
                    }
                    if ((bool)unit.Value.ItemType.UnfitIndividual)
                    {
                        itemTypes |= CashUnit.ItemTypesEnum.UnfitIndividual;
                    }
                }

                cashUnitConfigs.Add(unit.Key, new SetCashUnitInfoRequest.SetCashUnitConfiguration(unit.Value.Type is null ? null : unit.Value.Type switch
                                                                                                 {
                                                                                                     SetCashUnitInfoCommand.PayloadData.CashunitsClass.TypeEnum.BillCassette => CashUnit.TypeEnum.BillCassette,
                                                                                                     SetCashUnitInfoCommand.PayloadData.CashunitsClass.TypeEnum.CashIn => CashUnit.TypeEnum.CashIn,
                                                                                                     SetCashUnitInfoCommand.PayloadData.CashunitsClass.TypeEnum.CoinCylinder => CashUnit.TypeEnum.CoinCylinder,
                                                                                                     SetCashUnitInfoCommand.PayloadData.CashunitsClass.TypeEnum.CoinDispenser => CashUnit.TypeEnum.CoinDispenser,
                                                                                                     SetCashUnitInfoCommand.PayloadData.CashunitsClass.TypeEnum.Coupon => CashUnit.TypeEnum.Coupon,
                                                                                                     SetCashUnitInfoCommand.PayloadData.CashunitsClass.TypeEnum.Document => CashUnit.TypeEnum.Document,
                                                                                                     SetCashUnitInfoCommand.PayloadData.CashunitsClass.TypeEnum.Recycling => CashUnit.TypeEnum.Recycling,
                                                                                                     SetCashUnitInfoCommand.PayloadData.CashunitsClass.TypeEnum.RejectCassette => CashUnit.TypeEnum.RejectCassette,
                                                                                                     SetCashUnitInfoCommand.PayloadData.CashunitsClass.TypeEnum.ReplenishmentContainer => CashUnit.TypeEnum.ReplenishmentContainer,
                                                                                                     SetCashUnitInfoCommand.PayloadData.CashunitsClass.TypeEnum.RetractCassette => CashUnit.TypeEnum.RetractCassette,
                                                                                                     _ => CashUnit.TypeEnum.NotApplicable,
                                                                                                 },
                                                                                                 unit.Value.CurrencyID,
                                                                                                 unit.Value.Value,
                                                                                                 unit.Value.Maximum,
                                                                                                 unit.Value.AppLock,
                                                                                                 unit.Value.CashUnitName,
                                                                                                 unit.Value.Minimum,
                                                                                                 unit.Value.PhysicalPositionName,
                                                                                                 unit.Value.UnitID,
                                                                                                 unit.Value.MaximumCapacity,
                                                                                                 unit.Value.HardwareSensor,
                                                                                                 itemTypes,
                                                                                                 unit.Value.NoteIDs));

                List<BankNoteNumber> bankNoteList = null;
                if (unit.Value.NoteNumberList is not null &&
                    unit.Value.NoteNumberList.NoteNumber is not null &&
                    unit.Value.NoteNumberList.NoteNumber.Count > 0)
                {
                    foreach (var bk in unit.Value.NoteNumberList.NoteNumber)
                    {
                        if (bk.NoteID is null ||
                            bk.Count is null)
                        {
                            continue;
                        }

                        bankNoteList.Add(new BankNoteNumber((int)bk.NoteID, (int)bk.Count));
                    }
                }

                cashUnitAccountings.Add(unit.Key, new SetCashUnitInfoRequest.SetCashUnitAccounting(unit.Value.LogicalCount,
                                                                                                   unit.Value.InitialCount,
                                                                                                   unit.Value.DispensedCount,
                                                                                                   unit.Value.PresentedCount,
                                                                                                   unit.Value.RetractedCount,
                                                                                                   unit.Value.RejectCount,
                                                                                                   unit.Value.Count,
                                                                                                   unit.Value.CashInCount,
                                                                                                   bankNoteList));
            }

            // Keep old accounts
            Dictionary<string, CashUnitAccounting> oldAccounting = new();
            foreach (var unit in CashManagement.CashUnits)
            {
                oldAccounting.Add(unit.Key, unit.Value.Accounting);
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.SetCashUnitInfoAsync()");

            var result = await Device.SetCashUnitInfoAsync(events, 
                                                           new SetCashUnitInfoRequest(cashUnitConfigs, 
                                                                                      cashUnitAccountings), 
                                                           cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.SetCashUnitInfoAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            // Update cash unit configuration updated
            CashManagement.ConstructCashUnits();

            if (cashUnitAccountings is not null &&
                cashUnitAccountings.Count > 0)
            {
                // Update cash unit counts internally
                Dictionary<string, CashUnitAccounting> newAccounting = new();
                foreach (var unit in cashUnitAccountings)
                {
                    CashUnitAccounting account = new();
                    if (oldAccounting.ContainsKey(unit.Key))
                    {
                        account = oldAccounting[unit.Key];
                    }

                    if (unit.Value.CashInCount is not null)
                        account.CashInCount = (int)unit.Value.CashInCount;
                    if (unit.Value.Count is not null)
                        account.Count = (int)unit.Value.Count;
                    if (unit.Value.DispensedCount is not null)
                        account.DispensedCount = (int)unit.Value.DispensedCount;
                    if (unit.Value.InitialCount is not null)
                        account.InitialCount = (int)unit.Value.InitialCount;
                    if (unit.Value.LogicalCount is not null)
                        account.LogicalCount = (int)unit.Value.LogicalCount;
                    if (unit.Value.PresentedCount is not null)
                        account.PresentedCount = (int)unit.Value.PresentedCount;
                    if (unit.Value.RejectCount is not null)
                        account.RejectCount = (int)unit.Value.RejectCount;
                    if (unit.Value.RetractedCount is not null)
                        account.RetractedCount = (int)unit.Value.RetractedCount;
                    if (unit.Value.BankNoteNumberList is not null &&
                        unit.Value.BankNoteNumberList.Count > 0)
                    {
                        account.BankNoteNumberList = (from BankNoteNumber bk in unit.Value.BankNoteNumberList
                                                      select new BankNoteNumber(bk.NoteID, bk.Count)).ToList();
                    }

                    newAccounting.Add(unit.Key, account);
                }

                foreach (var unit in newAccounting)
                {
                    if (!CashManagement.CashUnits.ContainsKey(unit.Key))
                        continue;
                    CashManagement.CashUnits[unit.Key].Accounting = unit.Value;
                }

                CashManagement.UpdateCashUnitAccounting();
            }

            return new SetCashUnitInfoCompletion.PayloadData(result.CompletionCode,
                                                             result.ErrorDescription,
                                                             result.ErrorCode);
        }
    }
}
