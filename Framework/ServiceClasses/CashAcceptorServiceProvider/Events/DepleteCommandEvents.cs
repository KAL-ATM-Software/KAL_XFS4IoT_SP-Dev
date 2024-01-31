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
using XFS4IoT.Events;

namespace XFS4IoTFramework.CashAcceptor
{
    public sealed class DepleteCommandEvents(IStorageService storage, IDepleteEvents events) : ItemErrorCommandEvents(events)
    {
        public Task IncompleteDepleteEvent(DepleteOperationResult result)
        {
            IncompleteDepleteEvent.PayloadData payload = new();

            if (result is not null)
            {
                List<IncompleteDepleteEvent.PayloadData.DepleteClass.DepleteSourceResultsClass> depleteSrouceResults = null;
                if (result.SourceResults is not null &&
                    result.SourceResults.Count > 0)
                {
                    depleteSrouceResults = new();

                    foreach (var sourceResult in result.SourceResults)
                    {
                        depleteSrouceResults.Add(new IncompleteDepleteEvent.PayloadData.DepleteClass.DepleteSourceResultsClass(sourceResult.Key,
                                                                                                                               sourceResult.Value.CashItem,
                                                                                                                               sourceResult.Value.NumberOfItemsRemoved));
                    }
                }

                payload = new(new IncompleteDepleteEvent.PayloadData.DepleteClass(result.TotalNumberOfItemsReceived,
                                                                                  result.TotalNumberOfItemsRejected,
                                                                                  depleteSrouceResults));
            }

            if (DepleteEvents is not null)
            {
                return DepleteEvents.IncompleteDepleteEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(IncompleteDepleteEvent));
        }

        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds) => StorageErrorCommandEvent?.StorageErrorEvent(Failure, CashUnitIds);

        private StorageErrorCommandEvent StorageErrorCommandEvent { get; init; } = new(storage, events);
    }
}
