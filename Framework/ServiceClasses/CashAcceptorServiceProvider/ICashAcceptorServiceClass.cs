/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoTFramework.CashAcceptor;
using XFS4IoTFramework.CashManagement;
using XFS4IoTFramework.Common;
using XFS4IoT.CashAcceptor.Events;

namespace XFS4IoTServer
{
    public interface ICashAcceptorService
    {
        /// <summary>
        /// The information about the status of the currently active cash-in transaction or 
        /// in the case where no cash-in transaction is active the status of the most recently ended cash-in transaction. 
        /// </summary>
        CashInStatusClass CashInStatus { get; init; }

        /// <summary>
        /// The physical lock/unlock status of the CashAcceptor device and storages.
        /// </summary>
        DeviceLockStatusClass DeviceLockStatus { get; init; }

        /// <summary>
        /// The deplete target and destination information
        /// Key - The storage id can be used for target of the depletion operation.
        /// Value - List of storage id can be used for source of the depletion operation
        /// </summary>
        Dictionary<string, List<string>> DepleteCashUnitSources { get; init; }

        /// <summary>
        /// Which storage units can be specified as targets for a given source storage unit with the CashAcceptor.Replenish command
        /// </summary>
        List<string> ReplenishTargets { get; init; }
    }

    public interface ICashAcceptorServiceClass : ICashAcceptorUnsolicitedEvents
    {
    }
}
