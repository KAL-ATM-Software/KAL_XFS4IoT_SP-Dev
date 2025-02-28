/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashManagement
{
    public sealed class RetractCommandEvents(IStorageService storage, IRetractEvents events) : ItemErrorCommandEvents(events)
    {
        public enum IncompleteRetractReasonEnum
        {
            RetractFailure,
            RetractAreaFull,
            ForeignItemsDetected,
            InvalidBunch
        }

        public Task IncompleteRetractEvent(Dictionary<string, CashUnitCountClass> movements, IncompleteRetractReasonEnum reason)
        {
            if (RetractEvents is not null)
            {
                Dictionary<string, XFS4IoT.CashManagement.StorageCashInClass> itemMovementResult = null;
                if (movements?.Count > 0)
                {
                    itemMovementResult = [];
                    foreach (var movement in movements)
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

                        XFS4IoT.CashManagement.StorageCashCountsClass depositedCount = new(movement.Value.StorageCashInCount.Deposited.Unrecognized);
                        depositedCount.ExtendedProperties = deposited;
                        XFS4IoT.CashManagement.StorageCashCountsClass retractedCount = new(movement.Value.StorageCashInCount.Retracted.Unrecognized);
                        retractedCount.ExtendedProperties = retracted;
                        XFS4IoT.CashManagement.StorageCashCountsClass rejectedCount = new(movement.Value.StorageCashInCount.Rejected.Unrecognized);
                        rejectedCount.ExtendedProperties = rejected;
                        XFS4IoT.CashManagement.StorageCashCountsClass distributedCount = new(movement.Value.StorageCashInCount.Distributed.Unrecognized);
                        distributedCount.ExtendedProperties = distributed;
                        XFS4IoT.CashManagement.StorageCashCountsClass transportCount = new(movement.Value.StorageCashInCount.Transport.Unrecognized);
                        transportCount.ExtendedProperties = transport;

                        itemMovementResult.Add(movement.Key, new XFS4IoT.CashManagement.StorageCashInClass
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

                return RetractEvents.IncompleteRetractEvent(
                        new XFS4IoT.CashManagement.Events.IncompleteRetractEvent.PayloadData(itemMovementResult,
                        reason switch
                        {
                            IncompleteRetractReasonEnum.ForeignItemsDetected => XFS4IoT.CashManagement.Events.IncompleteRetractEvent.PayloadData.ReasonEnum.ForeignItemsDetected,
                            IncompleteRetractReasonEnum.InvalidBunch => XFS4IoT.CashManagement.Events.IncompleteRetractEvent.PayloadData.ReasonEnum.InvalidBunch,
                            IncompleteRetractReasonEnum.RetractAreaFull => XFS4IoT.CashManagement.Events.IncompleteRetractEvent.PayloadData.ReasonEnum.RetractAreaFull,
                            _ => XFS4IoT.CashManagement.Events.IncompleteRetractEvent.PayloadData.ReasonEnum.RetractFailure,
                        })
                );
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(IncompleteRetractEvent));
        }

        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds) => StorageErrorCommandEvent?.StorageErrorEvent(Failure, CashUnitIds);

        private StorageErrorCommandEvent StorageErrorCommandEvent { get; init; } = new(storage, events);
    }
}
