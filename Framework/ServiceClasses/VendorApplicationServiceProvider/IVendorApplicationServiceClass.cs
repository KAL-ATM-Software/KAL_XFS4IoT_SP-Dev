/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.VendorApplication;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public interface IVendorApplicationService
    {
        /// <summary>
        /// This event is used to indicate the vendor dependent application has exited, 
        /// allowing an application the opportunity to exit Vendor Mode.
        /// </summary>
        Task VendorAppExitedEvent();

        /// <summary>
        /// This event is used to indicate that the required interface has changed. 
        /// </summary>
        Task InterfaceChangedEvent(ActiveInterfaceEnum ActiveInterface);
    }

    public interface IVendorApplicationServiceClass : IVendorApplicationService, IVendorApplicationUnsolicitedEvents
    {
    }
}
