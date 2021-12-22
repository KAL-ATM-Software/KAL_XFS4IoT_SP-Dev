/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * IVendorApplicationDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of vendorapplication. 
namespace XFS4IoTFramework.VendorApplication
{
    public interface IVendorApplicationDevice : IDevice
    {

        /// <summary>
        /// This command is issued by an application to start a local application which provides vendor dependent services. It can be used in conjunction with the Vendor Mode interface to manage vendor independent services and start vendor specific services, e.g. maintenance oriented applications.
        /// </summary>
        Task<XFS4IoT.VendorApplication.Completions.StartLocalApplicationCompletion.PayloadData> StartLocalApplication(IStartLocalApplicationEvents events, 
                                                                                                                      XFS4IoT.VendorApplication.Commands.StartLocalApplicationCommand.PayloadData payload, 
                                                                                                                      CancellationToken cancellation);

        /// <summary>
        /// |-  This command is used to retrieve the interface that should be used by the vendor dependent application.
        /// </summary>
        Task<XFS4IoT.VendorApplication.Completions.GetActiveInterfaceCompletion.PayloadData> GetActiveInterface(IGetActiveInterfaceEvents events, 
                                                                                                                XFS4IoT.VendorApplication.Commands.GetActiveInterfaceCommand.PayloadData payload, 
                                                                                                                CancellationToken cancellation);

        /// <summary>
        /// This command is used to indicate which interface should be displayed by a vendor dependent application. An application can issue this command to ensure that a vendor dependent application starts on the correct interface, or to change the interface while running.
        /// </summary>
        Task<XFS4IoT.VendorApplication.Completions.SetActiveInterfaceCompletion.PayloadData> SetActiveInterface(ISetActiveInterfaceEvents events, 
                                                                                                                XFS4IoT.VendorApplication.Commands.SetActiveInterfaceCommand.PayloadData payload, 
                                                                                                                CancellationToken cancellation);

    }
}
