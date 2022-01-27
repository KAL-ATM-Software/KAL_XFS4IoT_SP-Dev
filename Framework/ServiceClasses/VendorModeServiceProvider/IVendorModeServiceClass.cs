/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoTFramework.VendorMode;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public interface IVendorModeService
    {
        /// <summary>
        /// Pending on receiving acknowledge from the clients
        /// </summary>
        List<IConnection> PendingAcknowledge { get; set; }

        /// <summary>
        /// List of registered client via Register command
        /// </summary>
        Dictionary<IConnection, string> RegisteredClients { get; set; }

        /// <summary>
        /// This event is used to indicate that the system has exited Vendor Mode
        /// </summary>
        Task ModeExitedEvent();

        /// <summary>
        /// This event is used to indicate that the system has entered Vendor Mode
        /// </summary>
        Task ModeEnteredEvent();

        /// <summary>
        /// This service event is used to indicate the request to exit Vendor Mode to all registered clients
        /// </summary>
        Task BroadcastExitModeRequestEvent();

        /// <summary>
        /// This service event is used to indicate the request to enter Vendor Mode
        /// </summary>
        Task BroadcastEnterModeRequestEvent();
    }

    public interface IVendorModeServiceClass : IVendorModeService, IVendorModeUnsolicitedEvents
    {
    }
}
