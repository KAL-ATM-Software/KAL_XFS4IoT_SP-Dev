/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.CashManagement;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashManagement
{
    public partial class RetractHandler
    {
        private async Task<RetractCompletion.PayloadData> HandleRetract(IRetractEvents events, RetractCommand retract, CancellationToken cancel)
        {
            if (retract.Payload.RetractArea is not null &&
                retract.Payload.OutputPosition is not null)
            {
                return new RetractCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                         $"Specified both RetractArea and OutputPosition properties, the Service Provider doesn't know where the items to be moved.");
            }

            ItemPosition itemPosition = null;

            if (retract.Payload.RetractArea is not null)
            {
                CashManagementCapabilitiesClass.RetractAreaEnum retractArea = CashManagementCapabilitiesClass.RetractAreaEnum.Default;
                retractArea = retract.Payload.RetractArea switch
                {
                    RetractAreaEnum.ItemCassette => CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette,
                    RetractAreaEnum.Reject => CashManagementCapabilitiesClass.RetractAreaEnum.Reject,
                    RetractAreaEnum.Retract => CashManagementCapabilitiesClass.RetractAreaEnum.Retract,
                    RetractAreaEnum.Stacker => CashManagementCapabilitiesClass.RetractAreaEnum.Stacker,
                    RetractAreaEnum.Transport => CashManagementCapabilitiesClass.RetractAreaEnum.Transport,
                    _ => CashManagementCapabilitiesClass.RetractAreaEnum.Default
                };

                if (retractArea != CashManagementCapabilitiesClass.RetractAreaEnum.Default &&
                    !CashManagement.CashManagementCapabilities.RetractAreas.HasFlag(retractArea))
                {
                    return new RetractCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                             $"Specified unsupported retract area. {retractArea}",
                                                             RetractCompletion.PayloadData.ErrorCodeEnum.InvalidRetractPosition);
                }

                if (retractArea == CashManagementCapabilitiesClass.RetractAreaEnum.Retract)
                {
                    if (retract.Payload.Index is null)
                    {
                        return new RetractCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"Index property is set to null where the retract area is specified to retract position.");
                    }

                    int index = (int)retract.Payload.Index;

                    // Check the index is valid
                    int totalRetractUnits = (from unit in CashManagement.CashUnits
                                             where unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashOutRetract) ||
                                                   unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.CashInRetract)
                                             select unit).Count();
                    if ((int)index > totalRetractUnits)
                    {
                        return new RetractCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                 $"Unexpected index property value is set where the retract area is specified to retract position. " +
                                                                 $"The value of index one is the first retract position and increments by one for each subsequent position. {index}");
                    }

                    itemPosition = new ItemPosition(new Retract(retractArea, index));
                }
            }
            else if (retract.Payload.OutputPosition is not null)
            {
                CashManagementCapabilitiesClass.PositionEnum position = retract.Payload.OutputPosition switch
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

                if (!CashManagement.CashDispenserCapabilities.OutputPositions.HasFlag(position))
                {
                    return new RetractCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                $"Specified unsupported output position. {position}");
                }

                itemPosition = new ItemPosition(position);
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.RetractAsync()");

            var result = await Device.RetractAsync(events,
                                                   new RetractRequest(itemPosition),
                                                   cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.RetractAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            Dictionary<string, StorageCashInClass> itemMovementResult = new();

            if (result.MovementResult != null &&
                result.MovementResult.Count > 0)
            {
                foreach (var movement in result.MovementResult)
                {
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> deposited = new();
                    foreach (var item in movement.Value.StorageCashInCount.Deposited.ItemCounts)
                        deposited.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> retracted = new();
                    foreach (var item in movement.Value.StorageCashInCount.Retracted.ItemCounts)
                        retracted.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> rejected = new();
                    foreach (var item in movement.Value.StorageCashInCount.Rejected.ItemCounts)
                        rejected.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> distributed = new();
                    foreach (var item in movement.Value.StorageCashInCount.Distributed.ItemCounts)
                        distributed.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> transport = new();
                    foreach (var item in movement.Value.StorageCashInCount.Transport.ItemCounts)
                        transport.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    itemMovementResult.Add(movement.Key, new StorageCashInClass(movement.Value.StorageCashInCount.RetractOperations,
                                                            new StorageCashCountsClass(movement.Value.StorageCashInCount.Deposited.Unrecognized, deposited),
                                                            new StorageCashCountsClass(movement.Value.StorageCashInCount.Retracted.Unrecognized, retracted),
                                                            new StorageCashCountsClass(movement.Value.StorageCashInCount.Rejected.Unrecognized, rejected),
                                                            new StorageCashCountsClass(movement.Value.StorageCashInCount.Distributed.Unrecognized, distributed),
                                                            new StorageCashCountsClass(movement.Value.StorageCashInCount.Transport.Unrecognized, transport)));
                }
            }

            await CashManagement.UpdateCashAccounting(result.MovementResult);

            return new RetractCompletion.PayloadData(result.CompletionCode,
                                                     result.ErrorDescription,
                                                     result.ErrorCode,
                                                     itemMovementResult);
        }

    }
}
