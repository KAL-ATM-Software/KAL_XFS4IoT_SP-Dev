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
using XFS4IoTFramework.CashManagement;
using XFS4IoT.Events;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashDispenser
{
    public sealed class DispenseCommandEvents(IStorageService storage, IDispenseEvents events) : ItemErrorCommandEvents(events)
    {
        public Task DelayedDispenseEvent(double Delay)
        {
            DispenseEvents.IsNotNull($"No IDispenseEvents interface is set. " + nameof(DelayedDispenseEvent));
            return DispenseEvents.DelayedDispenseEvent(new(Delay));
        }

        public Task StartDispenseEvent()
        {
            DispenseEvents.IsNotNull($"No IDispenseEvents interface is set. " + nameof(StartDispenseEvent));
            return DispenseEvents.StartDispenseEvent();
        }

        public Task IncompleteDispenseEvent(Dictionary<string, double> Currencies, Dictionary<string, int> Values, Dictionary<string, double> CashBox)
        {
            DispenseEvents.IsNotNull($"No IDispenseEvents interface is set. " + nameof(IncompleteDispenseEvent));
            return DispenseEvents.IncompleteDispenseEvent(
                new(Currencies: Currencies.Count == 0 ?
                        null :
                        Currencies,
                    Values: Values.Count == 0 ? 
                        null :
                        Values,
                    CashBox: CashBox.Count == 0 ?
                        null :
                        new(CashBox))
                );
        }

        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds) => StorageErrorCommandEvent?.StorageErrorEvent(Failure, CashUnitIds);

        private StorageErrorCommandEvent StorageErrorCommandEvent { get; init; } = new(storage, events);
    } 
}
