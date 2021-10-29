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
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.CashManagement;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class TestCashUnitsHandler
    {
        private async Task<TestCashUnitsCompletion.PayloadData> HandleTestCashUnits(ITestCashUnitsEvents events, TestCashUnitsCommand testCashUnits, CancellationToken cancel)
        {
            ItemPosition itemPosition = null;

            if (string.IsNullOrEmpty(testCashUnits.Payload.Unit) &&
                testCashUnits.Payload.RetractArea is null &&
                testCashUnits.Payload.OutputPosition is null)
            {
                // Default position is device decide to move items
            }
            else
            {
                if (!string.IsNullOrEmpty(testCashUnits.Payload.Unit) &&
                    !CashDispenser.CashUnits.ContainsKey(testCashUnits.Payload.Unit))
                {
                    return new TestCashUnitsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Specified CashUnit location is unknown.");
                }

                if (!string.IsNullOrEmpty(testCashUnits.Payload.Unit))
                {
                    itemPosition = new ItemPosition(testCashUnits.Payload.Unit);
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
                        RetractClass retract = testCashUnits.Payload.RetractArea.IsA<RetractClass>("RetractArea object must be the RetractAreaClass.");

                        CashManagementCapabilitiesClass.RetractAreaEnum retractArea = CashManagementCapabilitiesClass.RetractAreaEnum.Default;
                        if (retract.RetractArea is not null)
                        {
                            retractArea = retract.RetractArea switch
                            {
                                RetractAreaEnum.ItemCassette => CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette,
                                RetractAreaEnum.Reject => CashManagementCapabilitiesClass.RetractAreaEnum.Reject,
                                RetractAreaEnum.Retract => CashManagementCapabilitiesClass.RetractAreaEnum.Retract,
                                RetractAreaEnum.Stacker => CashManagementCapabilitiesClass.RetractAreaEnum.Stacker,
                                RetractAreaEnum.Transport => CashManagementCapabilitiesClass.RetractAreaEnum.Transport,
                                _ => CashManagementCapabilitiesClass.RetractAreaEnum.Default
                            };

                            if (retractArea != CashManagementCapabilitiesClass.RetractAreaEnum.Default &&
                                !CashDispenser.CashManagementCapabilities.RetractAreas.HasFlag(retractArea))
                            {
                                return new TestCashUnitsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                               $"Specified unsupported retract area. {retractArea}",
                                                                               TestCashUnitsCompletion.PayloadData.ErrorCodeEnum.InvalidCashUnit);
                            }
                        }

                        if (retractArea == CashManagementCapabilitiesClass.RetractAreaEnum.Retract &&
                            retract.Index is null)
                        {
                            return new TestCashUnitsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                           $"Index property is set to null where the retract area is specified to retract position.");
                        }

                        itemPosition = new ItemPosition(new Retract(retractArea, index));
                    }
                    else if (testCashUnits.Payload.OutputPosition is not null)
                    {
                        CashManagementCapabilitiesClass.PositionEnum position = testCashUnits.Payload.OutputPosition switch
                        {
                            OutputPositionEnum.OutBottom => CashManagementCapabilitiesClass.PositionEnum.OutBottom,
                            OutputPositionEnum.OutCenter => CashManagementCapabilitiesClass.PositionEnum.OutCenter,
                            OutputPositionEnum.OutDefault => CashManagementCapabilitiesClass.PositionEnum.OutDefault,
                            OutputPositionEnum.OutFront => CashManagementCapabilitiesClass.PositionEnum.OutFront,
                            OutputPositionEnum.OutLeft => CashManagementCapabilitiesClass.PositionEnum.OutLeft,
                            OutputPositionEnum.OutRear => CashManagementCapabilitiesClass.PositionEnum.OutRear,
                            OutputPositionEnum.OutRight => CashManagementCapabilitiesClass.PositionEnum.OutRight,
                            OutputPositionEnum.OutTop => CashManagementCapabilitiesClass.PositionEnum.OutTop,
                            _ => CashManagementCapabilitiesClass.PositionEnum.NotSupported,

                        };

                        if (position == CashManagementCapabilitiesClass.PositionEnum.NotSupported ||
                            !CashDispenser.CashDispenserCapabilities.OutputPositions.HasFlag(position))
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


            await CashDispenser.UpdateCashAccounting(result.MovementResult);


            return new TestCashUnitsCompletion.PayloadData(result.CompletionCode,
                                                           result.ErrorDescription,
                                                           result.ErrorCode);
        }
    }
}
