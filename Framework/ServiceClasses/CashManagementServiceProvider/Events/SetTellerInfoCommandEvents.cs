/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoTFramework.CashManagement;
using XFS4IoT.CashManagement.Events;

namespace XFS4IoTFramework.CashManagement
{
    public sealed class SetTellerInfoCommandEvents
    {
        public SetTellerInfoCommandEvents(ISetTellerInfoEvents events)
        {
            SetTellerInfoEvents = events;
        }

        public Task TellerInfoChangedEvent(int TellerID) => SetTellerInfoEvents.TellerInfoChangedEvent(new TellerInfoChangedEvent.PayloadData(TellerID));

        private ISetTellerInfoEvents SetTellerInfoEvents { get; init; }
    }
}
