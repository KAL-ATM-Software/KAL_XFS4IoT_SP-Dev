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
using XFS4IoT.CashAcceptor.Events;

namespace XFS4IoTFramework.CashAcceptor
{
    public sealed class CashInCommandEvents(IStorageService storage, ICashInEvents events) : CashInCommonCommandEvents(events)
    {
        public Task SubCashInEvent(int? unrecognized, Dictionary<string, CashItemCountClass> itemCounts)
        {
            CashInEvents.IsNotNull($"No ICashInEvents interface is set. " + nameof(SubCashInEvent));

            SubCashInEvent.PayloadData payload = null;
            if (itemCounts?.Count > 0)
            {
                Dictionary<string, XFS4IoT.CashManagement.StorageCashCountClass> itemMovementResult = null;
                foreach (var itemCount in itemCounts)
                {
                    (itemMovementResult ??= []).Add(itemCount.Key, new XFS4IoT.CashManagement.StorageCashCountClass(itemCount.Value.Fit, itemCount.Value.Unfit, itemCount.Value.Suspect, itemCount.Value.Counterfeit, itemCount.Value.Inked));
                }
                payload = new(unrecognized)
                {
                    ExtendedProperties = itemMovementResult
                };
            }

            return CashInEvents.SubCashInEvent(payload);
        }

        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds) => StorageErrorCommandEvent?.StorageErrorEvent(Failure, CashUnitIds);

        private StorageErrorCommandEvent StorageErrorCommandEvent { get; init; } = new(storage, events);
    }
}
