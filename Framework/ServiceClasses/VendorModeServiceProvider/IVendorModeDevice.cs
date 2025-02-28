/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoTFramework.Common;

// KAL specific implementation of vendormode. 
namespace XFS4IoTFramework.VendorMode
{
    public interface IVendorModeDevice : IDevice
    {
        /// <summary>
        /// This method is called after all registered clients are acknowledged to enter vendor mode
        /// </summary>
        Task<DeviceResult> EnterVendorMode(CancellationToken cancellation);

        /// <summary>
        /// This method is called after all registered clients are acknowledged to exit vendor mode
        /// </summary>
        Task<DeviceResult> ExitVendorMode(CancellationToken cancellation);
    }
}
