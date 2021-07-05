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
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.Dispenser
{
    public partial class TestCashUnitsHandler
    {
        private async Task<TestCashUnitsCompletion.PayloadData> HandleTestCashUnits(ITestCashUnitsEvents events, TestCashUnitsCommand testCashUnits, CancellationToken cancel)
        {
            ItemPosition itemPosition = null;

            if (string.IsNullOrEmpty(testCashUnits.Payload.Cashunit) &&
                testCashUnits.Payload.RetractArea is null &&
                testCashUnits.Payload.OutputPosition is null)
            {
                // Default position is device decide to move items
            }
            else
            {
                if (!string.IsNullOrEmpty(testCashUnits.Payload.Cashunit) &&
                    !Dispenser.CashUnits.ContainsKey(testCashUnits.Payload.Cashunit))
                {
                    return new TestCashUnitsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Specified CashUnit location is unknown.");
                }

                if (!string.IsNullOrEmpty(testCashUnits.Payload.Cashunit))
                {
                    itemPosition = new ItemPosition(testCashUnits.Payload.Cashunit);
                }
                else
                {
                    if (testCashUnits.Payload.RetractArea is not null &&
                        testCashUnits.Payload.OutputPosition is not null)
                    {
                        return new TestCashUnitsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                       $"Specified both RetractArea and OutputPosition properties, the Service Provider doesn't know where the items to be moved.");
                    }

                    if (testCashUnits.Payload.RetractArea is not null)
                    {
                        int? index = null;
                        ResetCommand.PayloadData.RetractAreaClass retract = testCashUnits.Payload.RetractArea.IsA<ResetCommand.PayloadData.RetractAreaClass>("RetractArea object must be the RetractAreaClass.");

                        CashDispenserCapabilitiesClass.RetractAreaEnum retractArea = CashDispenserCapabilitiesClass.RetractAreaEnum.Default;
                        if (retract.RetractArea is not null)
                        {
                            retractArea = retract.RetractArea switch
                            {
                                ResetCommand.PayloadData.RetractAreaClass.RetractAreaEnum.ItemCassette => CashDispenserCapabilitiesClass.RetractAreaEnum.ItemCassette,
                                ResetCommand.PayloadData.RetractAreaClass.RetractAreaEnum.Reject => CashDispenserCapabilitiesClass.RetractAreaEnum.Reject,
                                ResetCommand.PayloadData.RetractAreaClass.RetractAreaEnum.Retract => CashDispenserCapabilitiesClass.RetractAreaEnum.Retract,
                                ResetCommand.PayloadData.RetractAreaClass.RetractAreaEnum.Stacker => CashDispenserCapabilitiesClass.RetractAreaEnum.Stacker,
                                ResetCommand.PayloadData.RetractAreaClass.RetractAreaEnum.Transport => CashDispenserCapabilitiesClass.RetractAreaEnum.Transport,
                                _ => CashDispenserCapabilitiesClass.RetractAreaEnum.Default
                            };

                            if (!Dispenser.CashDispenserCapabilities.RetractAreas[retractArea])
                            {
                                return new TestCashUnitsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                               $"Specified unsupported retract area. {retractArea}",
                                                                               TestCashUnitsCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit);
                            }
                        }

                        if (retractArea == CashDispenserCapabilitiesClass.RetractAreaEnum.Retract &&
                            retract.Index is null)
                        {
                            return new TestCashUnitsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                           $"Index property is set to null where the retract area is specified to retract position.");
                        }

                        itemPosition = new ItemPosition(new Retract(retractArea, index));
                    }
                    else if (testCashUnits.Payload.OutputPosition is not null)
                    {
                        CashDispenserCapabilitiesClass.OutputPositionEnum position = testCashUnits.Payload.OutputPosition switch
                        {
                            TestCashUnitsCommand.PayloadData.OutputPositionEnum.Bottom => CashDispenserCapabilitiesClass.OutputPositionEnum.Bottom,
                            TestCashUnitsCommand.PayloadData.OutputPositionEnum.Center => CashDispenserCapabilitiesClass.OutputPositionEnum.Center,
                            TestCashUnitsCommand.PayloadData.OutputPositionEnum.Front => CashDispenserCapabilitiesClass.OutputPositionEnum.Front,
                            TestCashUnitsCommand.PayloadData.OutputPositionEnum.Left => CashDispenserCapabilitiesClass.OutputPositionEnum.Left,
                            TestCashUnitsCommand.PayloadData.OutputPositionEnum.Rear => CashDispenserCapabilitiesClass.OutputPositionEnum.Rear,
                            TestCashUnitsCommand.PayloadData.OutputPositionEnum.Right => CashDispenserCapabilitiesClass.OutputPositionEnum.Right,
                            TestCashUnitsCommand.PayloadData.OutputPositionEnum.Top => CashDispenserCapabilitiesClass.OutputPositionEnum.Top,
                            _ => CashDispenserCapabilitiesClass.OutputPositionEnum.Default,

                        };

                        if (!Dispenser.CashDispenserCapabilities.OutputPositons[position])
                        {
                            return new TestCashUnitsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                           $"Specified unsupported output position. {position}");
                        }

                        itemPosition = new ItemPosition(position);
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.TestCashUnitsAsync()");

            var result = await Device.TestCashUnitsAsync(events,
                                                         new TestCashUnitsRequest(itemPosition),
                                                         cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.TestCashUnitsAsync() -> {result.CompletionCode}, {result.ErrorCode}");
            

            Dispenser.UpdateCashUnitAccounting(result.MovementResult);

            Dictionary<string, TestCashUnitsCompletion.PayloadData.CashunitsClass> xfsUnits = null;
            if (result.MovementResult is not null &&
                result.MovementResult.Count > 0)
            {
                xfsUnits = new();
                // Generate cash unit info updated
                foreach (var movement in result.MovementResult)
                {
                    if (string.IsNullOrEmpty(movement.Key) ||
                       !Dispenser.CashUnits.ContainsKey(movement.Key))
                        continue; // it's not moved into cash unit

                    CashUnit unit = Dispenser.CashUnits[movement.Key];

                    TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum xfsStatus = unit.Status switch
                    {
                        CashUnit.StatusEnum.Empty => TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum.Empty,
                        CashUnit.StatusEnum.Full => TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum.Full,
                        CashUnit.StatusEnum.High => TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum.High,
                        CashUnit.StatusEnum.Inoperative => TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum.Inoperative,
                        CashUnit.StatusEnum.Low => TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum.Low,
                        CashUnit.StatusEnum.Manipulated => TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum.Manipulated,
                        CashUnit.StatusEnum.Missing => TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum.Missing,
                        CashUnit.StatusEnum.NoReference => TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum.NoReference,
                        CashUnit.StatusEnum.Ok => TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum.Ok,
                        _ => TestCashUnitsCompletion.PayloadData.CashunitsClass.StatusEnum.NoValue,
                    };

                    TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum xfsType = unit.Type switch
                    {
                        CashUnit.TypeEnum.BillCassette => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.BillCassette,
                        CashUnit.TypeEnum.CashIn => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.CashIn,
                        CashUnit.TypeEnum.CoinCylinder => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.CoinCylinder,
                        CashUnit.TypeEnum.CoinDispenser => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.CoinDispenser,
                        CashUnit.TypeEnum.Coupon => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.Coupon,
                        CashUnit.TypeEnum.Document => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.Document,
                        CashUnit.TypeEnum.Recycling => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.Recycling,
                        CashUnit.TypeEnum.RejectCassette => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.RejectCassette,
                        CashUnit.TypeEnum.ReplenishmentContainer => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.ReplenishmentContainer,
                        CashUnit.TypeEnum.RetractCassette => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.RetractCassette,
                        _ => TestCashUnitsCompletion.PayloadData.CashunitsClass.TypeEnum.NotApplicable,
                    };

                    List<TestCashUnitsCompletion.PayloadData.CashunitsClass.NoteNumberListClass.NoteNumberClass> xfsNoteNumber = new();
                    foreach (BankNoteNumber bn in unit.BankNoteNumberList)
                        xfsNoteNumber.Add(new TestCashUnitsCompletion.PayloadData.CashunitsClass.NoteNumberListClass.NoteNumberClass(bn.NoteID, bn.Count));

                    xfsUnits.Add(movement.Key, new TestCashUnitsCompletion.PayloadData.CashunitsClass(xfsStatus,
                                                                                                      xfsType,
                                                                                                      unit.CurrencyID,
                                                                                                      unit.Value,
                                                                                                      unit.LogicalCount,
                                                                                                      unit.Maximum,
                                                                                                      unit.AppLock,
                                                                                                      unit.CashUnitName,
                                                                                                      unit.InitialCount,
                                                                                                      unit.DispensedCount,
                                                                                                      unit.PresentedCount,
                                                                                                      unit.RetractedCount,
                                                                                                      unit.RejectCount,
                                                                                                      unit.Minimum,
                                                                                                      unit.PhysicalPositionName,
                                                                                                      unit.UnitID,
                                                                                                      unit.Count,
                                                                                                      unit.MaximumCapacity,
                                                                                                      unit.HardwareSensor,
                                                                                                      new TestCashUnitsCompletion.PayloadData.CashunitsClass.ItemTypeClass((unit.ItemTypes & CashUnit.ItemTypesEnum.All) == CashUnit.ItemTypesEnum.All,
                                                                                                                                                                           (unit.ItemTypes & CashUnit.ItemTypesEnum.Unfit) == CashUnit.ItemTypesEnum.Unfit,
                                                                                                                                                                           (unit.ItemTypes & CashUnit.ItemTypesEnum.Individual) == CashUnit.ItemTypesEnum.Individual,
                                                                                                                                                                           (unit.ItemTypes & CashUnit.ItemTypesEnum.Level1) == CashUnit.ItemTypesEnum.Level1,
                                                                                                                                                                           (unit.ItemTypes & CashUnit.ItemTypesEnum.Level2) == CashUnit.ItemTypesEnum.Level2,
                                                                                                                                                                           (unit.ItemTypes & CashUnit.ItemTypesEnum.Level3) == CashUnit.ItemTypesEnum.Level3,
                                                                                                                                                                           (unit.ItemTypes & CashUnit.ItemTypesEnum.ItemProcessor) == CashUnit.ItemTypesEnum.ItemProcessor,
                                                                                                                                                                           (unit.ItemTypes & CashUnit.ItemTypesEnum.UnfitIndividual) == CashUnit.ItemTypesEnum.UnfitIndividual),
                                                                                                      unit.CashInCount,
                                                                                                      new TestCashUnitsCompletion.PayloadData.CashunitsClass.NoteNumberListClass(xfsNoteNumber),
                                                                                                      unit.BanknoteIDs));
                }
            }

            return new TestCashUnitsCompletion.PayloadData(result.CompletionCode,
                                                           result.ErrorDescription,
                                                           result.ErrorCode,
                                                           xfsUnits);
        }
    }
}
