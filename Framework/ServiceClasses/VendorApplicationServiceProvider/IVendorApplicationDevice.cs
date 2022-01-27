/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoTFramework.Common;

// KAL specific implementation of vendorapplication. 
namespace XFS4IoTFramework.VendorApplication
{
    public interface IVendorApplicationDevice : IDevice
    {
        /// <summary>
        /// This command is issued by an application to start a local application which provides vendor dependent services. 
        /// It can be used in conjunction with the Vendor Mode interface to manage vendor independent services and start vendor specific services, 
        /// e.g. maintenance oriented applications.
        /// </summary>
        Task<DeviceResult> StartLocalApplication(StartLocalApplicationRequest request, 
                                                 CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve the interface that should be used by the vendor dependent application.
        /// </summary>
        GetActiveInterfaceResult GetActiveInterface();

        /// <summary>
        /// This command is used to indicate which interface should be displayed by a vendor dependent application.
        /// An application can issue this command to ensure that a vendor dependent application starts on the correct interface, 
        /// or to change the interface while running.
        /// </summary>
        Task<DeviceResult> SetActiveInterface(SetActiveInterfaceRequest request, 
                                              CancellationToken cancellation);

        /// <summary>
        /// Stores vendor application capabilites
        /// </summary>
        VendorApplicationCapabilitiesClass VendorApplicationCapabilities { get; set; }

    }
}
