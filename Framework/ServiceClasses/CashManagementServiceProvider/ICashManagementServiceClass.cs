/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using XFS4IoTFramework.CashManagement;
using XFS4IoT.CashManagement.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashManagement
{
    public interface ICashManagementService
    {
        Task ItemsTakenEvent(CashManagementCapabilitiesClass.PositionEnum Position, string AdditionalBunches);

        Task ItemsInsertedEvent(CashManagementCapabilitiesClass.PositionEnum Position);

        Task ItemsPresentedEvent(CashManagementCapabilitiesClass.PositionEnum Position, string AdditionalBunches);


        /// <summary>
        /// The framework maintains cash-in status
        /// </summary>
        CashInStatusClass CashInStatusManaged { get; init; }

        /// <summary>
        /// Store cash-in in status persistently
        /// </summary>
        void StoreCashInStatus();

        /// <summary>
        /// The last status of the most recent attempt to present or return items to the customer. 
        /// </summary>
        Dictionary<CashManagementCapabilitiesClass.PositionEnum, CashManagementPresentStatus> LastCashManagementPresentStatus { get; init; }

        /// <summary>
        /// Store present status persistently
        /// </summary>
        void StoreCashManagementPresentStatus();

        /// <summary>
        /// This list provides the functionality to blacklist notes and allows additional flexibility, for example to specify that notes can be taken out of circulation
        /// by specifying them as unfit.Any items not returned in this list will be handled according to normal classification rules.
        /// </summary>
        ItemClassificationListClass ItemClassificationList { get; init; }

        /// <summary>
        /// Store classification list persistently
        /// </summary>
        void StoreItemClassificationList();
    }

    public interface ICashManagementServiceClass : ICashManagementService, ICashManagementUnsolicitedEvents
    { 
    }
}
