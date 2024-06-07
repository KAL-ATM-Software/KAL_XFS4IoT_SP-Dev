/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTFramework.Storage;
using System.Text.RegularExpressions;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class TestCashUnitsHandler
    {
        private async Task<TestCashUnitsCompletion.PayloadData> HandleTestCashUnits(ITestCashUnitsEvents events, TestCashUnitsCommand testCashUnits, CancellationToken cancel)
        {
            ItemDestination destination = new();

            if (testCashUnits.Payload.Target is null)
            {
                // Default position is device decide to move items
            }
            else
            {
                if (testCashUnits.Payload.Target.Target == ItemTargetEnumEnum.SingleUnit &&
                    string.IsNullOrEmpty(testCashUnits.Payload.Target.Unit))
                {
                    return new TestCashUnitsCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"Specified location to {testCashUnits.Payload.Target.Target}, but target unit {testCashUnits.Payload.Target.Unit} property is an empty string.");
                }

                if (testCashUnits.Payload.Target.Target == ItemTargetEnumEnum.SingleUnit &&
                    !string.IsNullOrEmpty(testCashUnits.Payload.Target.Unit) &&
                    !Storage.CashUnits.ContainsKey(testCashUnits.Payload.Target.Unit))
                {
                    return new TestCashUnitsCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"Specified CashUnit location is unknown. {testCashUnits.Payload.Target.Unit}");
                }

                if (testCashUnits.Payload.Target.Target == ItemTargetEnumEnum.SingleUnit)
                {
                    destination = new ItemDestination(CashUnit: testCashUnits.Payload.Target.Unit);
                }
                else
                {
                    if (testCashUnits.Payload.Target.Target == ItemTargetEnumEnum.Retract ||
                        testCashUnits.Payload.Target.Target == ItemTargetEnumEnum.Transport ||
                        testCashUnits.Payload.Target.Target == ItemTargetEnumEnum.Stacker ||
                        testCashUnits.Payload.Target.Target == ItemTargetEnumEnum.ItemCassette ||
                        testCashUnits.Payload.Target.Target == ItemTargetEnumEnum.CashIn ||
                        testCashUnits.Payload.Target.Target == ItemTargetEnumEnum.Reject)
                    {
                        CashManagementCapabilitiesClass.RetractAreaEnum retractArea = testCashUnits.Payload.Target.Target switch
                        {
                            ItemTargetEnumEnum.ItemCassette => CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette,
                            ItemTargetEnumEnum.CashIn => CashManagementCapabilitiesClass.RetractAreaEnum.CashIn,
                            ItemTargetEnumEnum.Retract => CashManagementCapabilitiesClass.RetractAreaEnum.Retract,
                            ItemTargetEnumEnum.Stacker => CashManagementCapabilitiesClass.RetractAreaEnum.Stacker,
                            ItemTargetEnumEnum.Reject => CashManagementCapabilitiesClass.RetractAreaEnum.Reject,
                            _ => CashManagementCapabilitiesClass.RetractAreaEnum.Transport,
                        };

                        if (!Common.CashManagementCapabilities.RetractAreas.HasFlag(retractArea))
                        {
                            return new TestCashUnitsCompletion.PayloadData(
                                MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                $"Specified unsupported retract area. {retractArea}",
                                TestCashUnitsCompletion.PayloadData.ErrorCodeEnum.UnsupportedPosition);
                        }

                        if (retractArea == CashManagementCapabilitiesClass.RetractAreaEnum.Retract)
                        {
                            int index = 0;
                            if (testCashUnits.Payload.Target.Index is null)
                            {
                                Logger.Warning(Constants.Framework, $"Index property is set to null where the retract area is specified to retract position. default to zero.");
                            }
                            else
                            {
                                index = (int)testCashUnits.Payload.Target.Index;
                            }

                            if (index < 0)
                            {
                                Logger.Warning(Constants.Framework, $"Index property is set negative value {index}. default to zero.");
                                index = 0;
                            }

                            // Check the index is valid
                            int totalRetractUnits = (from unit in Storage.CashUnits
                                                     where unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract) ||
                                                         unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract)
                                                     select unit).Count();
                            if ((int)index > totalRetractUnits)
                            {
                                return new TestCashUnitsCompletion.PayloadData(
                                    MessagePayload.CompletionCodeEnum.InvalidData,
                                    $"Unexpected index property value is set where the retract area is specified to retract position. " +
                                    $"The value of index one is the first retract position and increments by one for each subsequent position. {index}");
                            }

                            destination = new ItemDestination(ItemTargetEnum.Retract, index);
                        }
                        else
                        {
                            destination = new ItemDestination(
                            testCashUnits.Payload.Target.Target switch
                               {
                                   ItemTargetEnumEnum.ItemCassette => ItemTargetEnum.ItemCassette,
                                   ItemTargetEnumEnum.CashIn => ItemTargetEnum.CashIn,
                                   ItemTargetEnumEnum.Stacker => ItemTargetEnum.Stacker,
                                   ItemTargetEnumEnum.Reject => ItemTargetEnum.Reject,
                                   _ => ItemTargetEnum.Transport,
                               });
                        }
                    }
                    else
                    {
                        if (Common.CashManagementCapabilities.Positions == CashManagementCapabilitiesClass.PositionEnum.NotSupported)
                        {
                            return new TestCashUnitsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Specified unsupported output position. {testCashUnits.Payload.Target.Target}");
                        }

                        CashManagementCapabilitiesClass.PositionEnum position = testCashUnits.Payload.Target.Target switch
                        {
                            ItemTargetEnumEnum.OutBottom => CashManagementCapabilitiesClass.PositionEnum.OutBottom,
                            ItemTargetEnumEnum.OutCenter => CashManagementCapabilitiesClass.PositionEnum.OutCenter,
                            ItemTargetEnumEnum.OutDefault => CashManagementCapabilitiesClass.PositionEnum.OutDefault,
                            ItemTargetEnumEnum.OutFront => CashManagementCapabilitiesClass.PositionEnum.OutFront,
                            ItemTargetEnumEnum.OutLeft => CashManagementCapabilitiesClass.PositionEnum.OutLeft,
                            ItemTargetEnumEnum.OutRear => CashManagementCapabilitiesClass.PositionEnum.OutRear,
                            ItemTargetEnumEnum.OutRight => CashManagementCapabilitiesClass.PositionEnum.OutRight,
                            ItemTargetEnumEnum.OutTop => CashManagementCapabilitiesClass.PositionEnum.OutTop,
                            _ => throw new InvalidDataException($"Unsupported output position is specified. {testCashUnits.Payload.Target.Target}"),
                        };

                        if (!Common.CashManagementCapabilities.Positions.HasFlag(position))
                        {
                            return new TestCashUnitsCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Specified unsupported output position. {testCashUnits.Payload.Target.Target}");
                        }

                        destination = new ItemDestination(
                        testCashUnits.Payload.Target.Target switch
                               {
                                   ItemTargetEnumEnum.OutBottom => ItemTargetEnum.OutBottom,
                                   ItemTargetEnumEnum.OutCenter => ItemTargetEnum.OutCenter,
                                   ItemTargetEnumEnum.OutDefault => ItemTargetEnum.OutDefault,
                                   ItemTargetEnumEnum.OutFront => ItemTargetEnum.OutFront,
                                   ItemTargetEnumEnum.OutLeft => ItemTargetEnum.OutLeft,
                                   ItemTargetEnumEnum.OutRear => ItemTargetEnum.OutRear,
                                   ItemTargetEnumEnum.OutRight => ItemTargetEnum.OutRight,
                                   _ => ItemTargetEnum.OutTop,
                               });
                    }
                }
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.TestCashUnitsAsync()");

            var result = await Device.TestCashUnitsAsync(new TestCashUnitsCommandEvents(Storage, events),
                                                         new TestCashUnitsRequest(destination),
                                                         cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.TestCashUnitsAsync() -> {result.CompletionCode}, {result.ErrorCode}");


            await Storage.UpdateCashAccounting(result.MovementResult);


            return new TestCashUnitsCompletion.PayloadData(result.CompletionCode,
                                                           result.ErrorDescription,
                                                           result.ErrorCode);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
