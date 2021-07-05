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

namespace XFS4IoTFramework.CashManagement
{
    public partial class GetCashUnitInfoHandler
    {
        private Task<GetCashUnitInfoCompletion.PayloadData> HandleGetCashUnitInfo(IGetCashUnitInfoEvents events, GetCashUnitInfoCommand getCashUnitInfo, CancellationToken cancel)
        {
            if (CashManagement.FirstCashUnitInfoCommand)
            {
                CashManagement.ConstructCashUnits();
                CashManagement.FirstCashUnitInfoCommand = false;
            }

            CashManagement.UpdateCashUnitAccounting();

            Dictionary<string, GetCashUnitInfoCompletion.PayloadData.CashunitsClass> xfsUnits = new();
            foreach (var unit in CashManagement.CashUnits)
            {
                GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum xfsStatus = unit.Value.Status switch
                {
                    CashUnit.StatusEnum.Empty => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum.Empty,
                    CashUnit.StatusEnum.Full => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum.Full,
                    CashUnit.StatusEnum.High => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum.High,
                    CashUnit.StatusEnum.Inoperative => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum.Inoperative,
                    CashUnit.StatusEnum.Low => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum.Low,
                    CashUnit.StatusEnum.Manipulated => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum.Manipulated,
                    CashUnit.StatusEnum.Missing => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum.Missing,
                    CashUnit.StatusEnum.NoReference => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum.NoReference,
                    CashUnit.StatusEnum.Ok => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum.Ok,
                    _ => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.StatusEnum.NoValue,
                };

                GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum xfsType = unit.Value.Type switch
                {
                    CashUnit.TypeEnum.BillCassette => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.BillCassette,
                    CashUnit.TypeEnum.CashIn => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.CashIn,
                    CashUnit.TypeEnum.CoinCylinder => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.CoinCylinder,
                    CashUnit.TypeEnum.CoinDispenser => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.CoinDispenser,
                    CashUnit.TypeEnum.Coupon => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.Coupon,
                    CashUnit.TypeEnum.Document => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.Document,
                    CashUnit.TypeEnum.Recycling => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.Recycling,
                    CashUnit.TypeEnum.RejectCassette => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.RejectCassette,
                    CashUnit.TypeEnum.ReplenishmentContainer => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.ReplenishmentContainer,
                    CashUnit.TypeEnum.RetractCassette => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.RetractCassette,
                    _ => GetCashUnitInfoCompletion.PayloadData.CashunitsClass.TypeEnum.NotApplicable,
                };

                List<GetCashUnitInfoCompletion.PayloadData.CashunitsClass.NoteNumberListClass.NoteNumberClass> xfsNoteNumber = new();
                foreach (BankNoteNumber bn in unit.Value.BankNoteNumberList)
                    xfsNoteNumber.Add(new GetCashUnitInfoCompletion.PayloadData.CashunitsClass.NoteNumberListClass.NoteNumberClass(bn.NoteID, bn.Count));

                xfsUnits.Add(unit.Key, new GetCashUnitInfoCompletion.PayloadData.CashunitsClass(xfsStatus,
                                                                                                xfsType,
                                                                                                unit.Value.CurrencyID,
                                                                                                unit.Value.Value,
                                                                                                unit.Value.LogicalCount,
                                                                                                unit.Value.Maximum,
                                                                                                unit.Value.AppLock,
                                                                                                unit.Value.CashUnitName,
                                                                                                unit.Value.InitialCount,
                                                                                                unit.Value.DispensedCount,
                                                                                                unit.Value.PresentedCount,
                                                                                                unit.Value.RetractedCount,
                                                                                                unit.Value.RejectCount,
                                                                                                unit.Value.Minimum,
                                                                                                unit.Value.PhysicalPositionName,
                                                                                                unit.Value.UnitID,
                                                                                                unit.Value.Count,
                                                                                                unit.Value.MaximumCapacity,
                                                                                                unit.Value.HardwareSensor,
                                                                                                new GetCashUnitInfoCompletion.PayloadData.CashunitsClass.ItemTypeClass((unit.Value.ItemTypes & CashUnit.ItemTypesEnum.All) == CashUnit.ItemTypesEnum.All,
                                                                                                                                                                       (unit.Value.ItemTypes & CashUnit.ItemTypesEnum.Unfit) == CashUnit.ItemTypesEnum.Unfit,
                                                                                                                                                                       (unit.Value.ItemTypes & CashUnit.ItemTypesEnum.Individual) == CashUnit.ItemTypesEnum.Individual,
                                                                                                                                                                       (unit.Value.ItemTypes & CashUnit.ItemTypesEnum.Level1) == CashUnit.ItemTypesEnum.Level1,
                                                                                                                                                                       (unit.Value.ItemTypes & CashUnit.ItemTypesEnum.Level2) == CashUnit.ItemTypesEnum.Level2,
                                                                                                                                                                       (unit.Value.ItemTypes & CashUnit.ItemTypesEnum.Level3) == CashUnit.ItemTypesEnum.Level3,
                                                                                                                                                                       (unit.Value.ItemTypes & CashUnit.ItemTypesEnum.ItemProcessor) == CashUnit.ItemTypesEnum.ItemProcessor,
                                                                                                                                                                       (unit.Value.ItemTypes & CashUnit.ItemTypesEnum.UnfitIndividual) == CashUnit.ItemTypesEnum.UnfitIndividual),
                                                                                                unit.Value.CashInCount,
                                                                                                new GetCashUnitInfoCompletion.PayloadData.CashunitsClass.NoteNumberListClass(xfsNoteNumber),
                                                                                                unit.Value.BanknoteIDs));

            }
            return Task.FromResult(new GetCashUnitInfoCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                             null,
                                                                             xfsUnits));
        }
    }
}
