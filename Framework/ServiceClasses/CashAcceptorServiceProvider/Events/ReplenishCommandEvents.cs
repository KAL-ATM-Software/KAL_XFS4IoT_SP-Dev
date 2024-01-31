/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Storage;
using XFS4IoT.CashAcceptor.Events;

namespace XFS4IoTFramework.CashAcceptor
{
    public sealed class ReplenishCommandEvents(IStorageService storage, IReplenishEvents events) : ItemErrorCommandEvents(events)
    {
        public Task IncompleteReplenishEvent(ReplenishOperationResult result)
        {
            IncompleteReplenishEvent.PayloadData payload = new();

            if (result is not null)
            {
                List<IncompleteReplenishEvent.PayloadData.ReplenishClass.ReplenishTargetResultsClass> targetResults = null;
                if (result.TargetResults is not null &&
                    result.TargetResults.Count > 0)
                {
                    targetResults = [];
                    foreach (var targetResult in result.TargetResults)
                    {
                        targetResults.Add(new IncompleteReplenishEvent.PayloadData.ReplenishClass.ReplenishTargetResultsClass(targetResult.Key,
                                                                                                                              targetResult.Value.CashItem,
                                                                                                                              targetResult.Value.NumberOfItemsReceived));
                    }
                }

                payload = new(new IncompleteReplenishEvent.PayloadData.ReplenishClass(result.NumberOfItemsRemoved,
                                                                                      result.TotalNumberOfItemsRejected,
                                                                                      targetResults));
            }

            if (ReplenishEvents is not null)
            {
                return ReplenishEvents.IncompleteReplenishEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(IncompleteDepleteEvent));
        }

        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds) => StorageErrorCommandEvent?.StorageErrorEvent(Failure, CashUnitIds);

        private StorageErrorCommandEvent StorageErrorCommandEvent { get; init; } = new(storage, events);
    }
}
