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

namespace XFS4IoTFramework.CashDispenser
{
    public sealed class DispenseCommandEvents : ItemErrorCommandEvents
    {
        public DispenseCommandEvents(IDispenseEvents events) :
            base(events)
        { }

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
            return DispenseEvents.IncompleteDispenseEvent(new(Currencies, Values, CashBox));
        }
    }
}
