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
using XFS4IoT.Events;
using XFS4IoTFramework.CashAcceptor;
using XFS4IoTFramework.CashDispenser;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashManagement
{
    public abstract class StorageErrorCommonCommandEvents : ItemInfoAvailableCommandEvent
    {
        public StorageErrorCommonCommandEvents(IStorageService storage, IResetEvents events) : 
            base(events)
        {
            StorageErrorCommandEvent = new(storage, events);
        }

        public StorageErrorCommonCommandEvents(IStorageService storage, ICashInRollbackEvents events) :
            base(events)
        {
            StorageErrorCommandEvent = new(storage, events);
        }

        public StorageErrorCommonCommandEvents(IStorageService storage, IPreparePresentEvents events) :
            base(events)
        {
            StorageErrorCommandEvent = new(storage, events);
        }

        public StorageErrorCommonCommandEvents(IStorageService storage, IRejectEvents events) :
            base(events)
        {
            StorageErrorCommandEvent = new(storage, events);
        }

        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds) => StorageErrorCommandEvent?.StorageErrorEvent(Failure, CashUnitIds);

        private StorageErrorCommandEvent StorageErrorCommandEvent { get; init; } = null;
    }
}
