/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
        private async Task<CommandResult<RetractCompletion.PayloadData>> HandleRetract(IRetractEvents events, RetractCommand retract, CancellationToken cancel)
        {
            if (retract.Payload.Location.RetractArea is not null &&
                retract.Payload.Location.OutputPosition is not null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Specified both RetractArea and OutputPosition properties, the Service Provider doesn't know where the items to be moved.");
            }

            RetractPosition retractPosition = null;

            if (retract.Payload.Location.RetractArea is not null)
            {
                CashManagementCapabilitiesClass.RetractAreaEnum retractArea = CashManagementCapabilitiesClass.RetractAreaEnum.Default;
                retractArea = retract.Payload.Location.RetractArea switch
                {
                    RetractAreaEnum.ItemCassette => CashManagementCapabilitiesClass.RetractAreaEnum.ItemCassette,
                    RetractAreaEnum.Reject => CashManagementCapabilitiesClass.RetractAreaEnum.Reject,
                    RetractAreaEnum.Retract => CashManagementCapabilitiesClass.RetractAreaEnum.Retract,
                    RetractAreaEnum.Stacker => CashManagementCapabilitiesClass.RetractAreaEnum.Stacker,
                    RetractAreaEnum.Transport => CashManagementCapabilitiesClass.RetractAreaEnum.Transport,
                    _ => CashManagementCapabilitiesClass.RetractAreaEnum.Default
                };

                if (retractArea != CashManagementCapabilitiesClass.RetractAreaEnum.Default &&
                    !Common.CashManagementCapabilities.RetractAreas.HasFlag(retractArea))
                {
                    return new(
                        new(RetractCompletion.PayloadData.ErrorCodeEnum.InvalidRetractPosition),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified unsupported retract area. {retractArea}");
                }

                if (retractArea == CashManagementCapabilitiesClass.RetractAreaEnum.Retract)
                {
                    int index = 0;
                    if (retract.Payload.Location.Index is null)
                    {
                        Logger.Warning(Constants.Framework, $"Index property is set to null where the retract area is specified to retract position. default to zero.");
                    }
                    else
                    {
                        index = (int)retract.Payload.Location.Index;
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
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Unexpected index property value is set where the retract area is specified to retract position. " +
                            $"The value of index one is the first retract position and increments by one for each subsequent position. {index}");
                    }

                    retractPosition = new RetractPosition(new Retract(retractArea, index));
                }
                else
                {
                    retractPosition = new RetractPosition(new Retract(retractArea));
                }
            }
            else if (retract.Payload.Location.OutputPosition is not null)
            {
                CashManagementCapabilitiesClass.PositionEnum position = retract.Payload.Location.OutputPosition switch
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

                if (!Common.CashDispenserCapabilities.OutputPositions.HasFlag(position))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified unsupported output position. {position}");
                }

                retractPosition = new RetractPosition(position);
            }

            // Clear TotalReturnedItems for the present status
            foreach (var presentStatus in CashManagement.LastCashManagementPresentStatus)
            {
                presentStatus.Value.TotalReturnedItems = new();
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.RetractAsync()");

            var result = await Device.RetractAsync(
                new RetractCommandEvents(Storage, events),
                new RetractRequest(retractPosition),
                cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.RetractAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            // Ending cash-in operation
            CashManagement.CashInStatusManaged.Status = CashInStatusClass.StatusEnum.Retract;
            CashManagement.StoreCashInStatus();

            Dictionary<string, StorageCashInClass> itemMovementResult = null;

            if (result.MovementResult?.Count > 0)
            {
                itemMovementResult = [];
                foreach (var movement in result.MovementResult)
                {
                    if (movement.Value.StorageCashInCount is null)
                    {
                        // Ignore if the device class reports no cash count information.
                        continue;
                    }

                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> deposited = new();
                    foreach (var item in movement.Value.StorageCashInCount.Deposited.ItemCounts)
                    {
                        deposited.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    }
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> retracted = new();
                    foreach (var item in movement.Value.StorageCashInCount.Retracted.ItemCounts)
                    {
                        retracted.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    }
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> rejected = new();
                    foreach (var item in movement.Value.StorageCashInCount.Rejected.ItemCounts)
                    {
                        rejected.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    }
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> distributed = new();
                    foreach (var item in movement.Value.StorageCashInCount.Distributed.ItemCounts)
                    {
                        distributed.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    }
                    Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> transport = new();
                    foreach (var item in movement.Value.StorageCashInCount.Transport.ItemCounts)
                    {
                        transport.Add(item.Key, new XFS4IoT.CashManagement.StorageCashCountClass(item.Value.Fit, item.Value.Unfit, item.Value.Suspect, item.Value.Counterfeit, item.Value.Inked));
                    }

                    StorageCashCountsClass depositedCount = new(movement.Value.StorageCashInCount.Deposited.Unrecognized)
                    {
                        ExtendedProperties = deposited
                    };
                    StorageCashCountsClass retractedCount = new(movement.Value.StorageCashInCount.Retracted.Unrecognized)
                    {
                        ExtendedProperties = retracted
                    };
                    StorageCashCountsClass rejectedCount = new(movement.Value.StorageCashInCount.Rejected.Unrecognized)
                    {
                        ExtendedProperties = rejected
                    };
                    StorageCashCountsClass distributedCount = new(movement.Value.StorageCashInCount.Distributed.Unrecognized)
                    {
                        ExtendedProperties = distributed
                    };
                    StorageCashCountsClass transportCount = new(movement.Value.StorageCashInCount.Transport.Unrecognized)
                    {
                        ExtendedProperties = transport
                    };

                    itemMovementResult.Add(movement.Key, 
                        new StorageCashInClass
                        (
                            movement.Value.StorageCashInCount.RetractOperations,
                            depositedCount,
                            retractedCount,
                            rejectedCount,
                            distributedCount,
                            transportCount
                        ));
                }
            }

            await Storage.UpdateCashAccounting(result.MovementResult);

            RetractCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                itemMovementResult is not null)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    Storage: itemMovementResult);
            }

            return new(
                payload,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
