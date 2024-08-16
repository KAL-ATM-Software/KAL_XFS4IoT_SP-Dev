/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.Completions;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;
using System.Collections.Generic;

namespace XFS4IoTFramework.CashManagement
{
    public partial class ResetHandler
    {
        private async Task<CommandResult<ResetCompletion.PayloadData>> HandleReset(IResetEvents events, ResetCommand reset, CancellationToken cancel)
        {
            ItemDestination destination = new();

            if (reset.Payload.Position is null ||
                reset.Payload.Position.Target is null)
            {
                // Default position is device decide to move items
            }
            else
            {
                if (reset.Payload.Position.Target == ItemTargetEnumEnum.SingleUnit &&
                    string.IsNullOrEmpty(reset.Payload.Position.Unit))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified location to {reset.Payload.Position.Target}, but target unit {reset.Payload.Position.Unit} property is an empty string.");
                }

                if (reset.Payload.Position.Target == ItemTargetEnumEnum.SingleUnit &&
                    !string.IsNullOrEmpty(reset.Payload.Position.Unit) &&
                    !Storage.CashUnits.ContainsKey(reset.Payload.Position.Unit))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified CashUnit location is unknown. {reset.Payload.Position.Unit}");
                }

                if (reset.Payload.Position.Target == ItemTargetEnumEnum.SingleUnit)
                {
                    destination = new ItemDestination(CashUnit: reset.Payload.Position.Unit);
                }
                else
                {
                    if (reset.Payload.Position.Target == ItemTargetEnumEnum.Retract ||
                        reset.Payload.Position.Target == ItemTargetEnumEnum.Transport ||
                        reset.Payload.Position.Target == ItemTargetEnumEnum.Stacker ||
                        reset.Payload.Position.Target == ItemTargetEnumEnum.ItemCassette ||
                        reset.Payload.Position.Target == ItemTargetEnumEnum.CashIn)
                    {
                        CashManagementCapabilitiesClass.RetractAreaEnum retractArea = reset.Payload.Position.Target switch
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
                            return new(
                                new(ResetCompletion.PayloadData.ErrorCodeEnum.InvalidRetractPosition),
                                MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                $"Specified unsupported retract area. {retractArea}");
                        }

                        if (retractArea == CashManagementCapabilitiesClass.RetractAreaEnum.Retract)
                        {
                            int index = 0;
                            if (reset.Payload.Position.Index is null)
                            {
                                Logger.Warning(Constants.Framework, $"Index property is set to null where the retract area is specified to retract position. default to zero.");
                            }
                            else
                            {
                                index = (int)reset.Payload.Position.Index;
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

                            destination = new ItemDestination(ItemTargetEnum.Retract, index);
                        }
                        else
                        {
                            destination = new ItemDestination(
                               reset.Payload.Position.Target switch
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
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Specified unsupported output position. {reset.Payload.Position.Target}");
                        }

                        CashManagementCapabilitiesClass.PositionEnum position = reset.Payload.Position.Target switch
                        {
                            ItemTargetEnumEnum.OutBottom => CashManagementCapabilitiesClass.PositionEnum.OutBottom,
                            ItemTargetEnumEnum.OutCenter => CashManagementCapabilitiesClass.PositionEnum.OutCenter,
                            ItemTargetEnumEnum.OutDefault => CashManagementCapabilitiesClass.PositionEnum.OutDefault,
                            ItemTargetEnumEnum.OutFront => CashManagementCapabilitiesClass.PositionEnum.OutFront,
                            ItemTargetEnumEnum.OutLeft => CashManagementCapabilitiesClass.PositionEnum.OutLeft,
                            ItemTargetEnumEnum.OutRear => CashManagementCapabilitiesClass.PositionEnum.OutRear,
                            ItemTargetEnumEnum.OutRight => CashManagementCapabilitiesClass.PositionEnum.OutRight,
                            ItemTargetEnumEnum.OutTop => CashManagementCapabilitiesClass.PositionEnum.OutTop,
                            _ => throw new InvalidDataException($"Unsupported output position is specified. {reset.Payload.Position.Target}"),
                        };

                        if (!Common.CashManagementCapabilities.Positions.HasFlag(position))
                        {
                            return new(
                                MessageHeader.CompletionCodeEnum.InvalidData,
                                $"Specified unsupported output position. {reset.Payload.Position.Target}");
                        }

                        destination = new ItemDestination(
                               reset.Payload.Position.Target switch
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

            // Clear TotalReturnedItems for the present status
            foreach (var presentStatus in CashManagement.LastCashManagementPresentStatus)
            {
                presentStatus.Value.TotalReturnedItems = new();
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.ResetDeviceAsync()");

            var result = await Device.ResetDeviceAsync(new ResetCommandEvents(Storage, events),
                                                       new ResetDeviceRequest(destination),
                                                       cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.ResetDeviceAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            // Ending cash-in operation
            CashManagement.CashInStatusManaged.Status = CashInStatusClass.StatusEnum.Reset;
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

                    itemMovementResult.Add(
                        movement.Key, 
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

            ResetCompletion.PayloadData payload = null;
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
