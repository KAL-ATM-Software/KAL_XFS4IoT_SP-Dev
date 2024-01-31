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
using XFS4IoT.Events;
using XFS4IoTFramework.CashAcceptor;
using XFS4IoTFramework.CashDispenser;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashManagement
{
    public abstract class StorageItemErrorCommandEvents : ItemErrorCommandEvents
    {
        public StorageItemErrorCommandEvents(IStorageService storage, ICalibrateCashUnitEvents events) :
            base(events)
        {
            StorageErrorCommandEvent = new(storage, events);
        }

        public StorageItemErrorCommandEvents(IStorageService storage, ICashInEndEvents events) :
            base(events)
        {
            StorageErrorCommandEvent = new(storage, events);
        }

        public StorageItemErrorCommandEvents(IStorageService storage, ICashUnitCountEvents events) :
            base(events)
        {
            StorageErrorCommandEvent = new(storage, events);
        }

        public StorageItemErrorCommandEvents(IStorageService storage, ICountEvents events) :
            base(events)
        {
            StorageErrorCommandEvent = new(storage, events);
        }

        public StorageItemErrorCommandEvents(IStorageService storage, ITestCashUnitsEvents events) :
            base(events)
        {
            StorageErrorCommandEvent = new(storage, events);
        }
        
        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds) => StorageErrorCommandEvent?.StorageErrorEvent(Failure, CashUnitIds);

        private StorageErrorCommandEvent StorageErrorCommandEvent { get; init; } = null;
    }
}
