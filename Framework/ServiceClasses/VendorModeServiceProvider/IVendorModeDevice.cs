/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * IVendorModeDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of vendormode. 
namespace XFS4IoTFramework.VendorMode
{
    public interface IVendorModeDevice : IDevice
    {

        /// <summary>
        /// |-  This command is issued by an application to register that it wants to participate in Vendor Mode.
        /// </summary>
        Task<XFS4IoT.VendorMode.Completions.RegisterCompletion.PayloadData> Register(IRegisterEvents events, 
                                                                                     XFS4IoT.VendorMode.Commands.RegisterCommand.PayloadData payload, 
                                                                                     CancellationToken cancellation);

        /// <summary>
        /// This command is issued by an application to indicate a logical request to enter Vendor Mode. The Servicewill then indicate the request to all registered applications by sending a[VendorMode.EnterModeRequestEvent](#vendormode.entermoderequestevent) and then wait foran acknowledgement back from each registered application before putting the system into Vendor Mode. The [service](#common.status.completion.description.vendormode.service) will change to *enterPending* on receipt of this command and will prevail untilall applications have acknowledged, at which time *service* will change to *active* and the commandcompletes.If the command fails when *service* is *enterPending*, *service* is changed to *inactive* and[VendorMode.ModeExitedEvent](#VendorMode.modeexitedevent) is sent to all registeredapplications.
        /// </summary>
        Task<XFS4IoT.VendorMode.Completions.EnterModeRequestCompletion.PayloadData> EnterModeRequest(IEnterModeRequestEvents events, 
                                                                                                     XFS4IoT.VendorMode.Commands.EnterModeRequestCommand.PayloadData payload, 
                                                                                                     CancellationToken cancellation);

        /// <summary>
        /// This command is issued by a registered application as an acknowledgement to the[VendorMode.EnterModeRequestEvent](#vendormode.entermoderequestevent) and it indicatesthat it is ready for the system to enter Vendor Mode. All registered applications must respond before Vendor Mode can be entered. Completion of this command is immediate.If this command is executed outwith a request for Vendor Mode entry, or if the acknowledge has already been sent from this connection then the command completes with a [sequenceError](#api.generalinformation.messagetypes.completionmessages.completioncodes) error code.Note: Applications must be prepared to allow the vendor dependent application to display on the active interface. This means that applications should no longer try to be the foreground or topmost window to ensure that the vendor dependent application is visible.
        /// </summary>
        Task<XFS4IoT.VendorMode.Completions.EnterModeAcknowledgeCompletion.PayloadData> EnterModeAcknowledge(IEnterModeAcknowledgeEvents events, 
                                                                                                             XFS4IoT.VendorMode.Commands.EnterModeAcknowledgeCommand.PayloadData payload, 
                                                                                                             CancellationToken cancellation);

        /// <summary>
        /// This command is issued by an application to indicate a request to exit Vendor Mode. The Service will then indicate the request to all registered applications by sending a [VendorMode.ExitModeRequestEvent](#vendormode.exitmoderequestevent) and then wait for anacknowledgement back from each registered application before removing the system from Vendor Mode. Thestatus will change to exitPending on receipt of this command and will prevail untilall applications have acknowledged, at which time the status will change to inactive and the ExitModeRequest command completes.If the command fails when the status is exitPending, the status is changed to active and a[VendorMode.ModeEnteredEvent](#vendormode.modeenteredevent) is sent to all registeredapplications.
        /// </summary>
        Task<XFS4IoT.VendorMode.Completions.ExitModeRequestCompletion.PayloadData> ExitModeRequest(IExitModeRequestEvents events, 
                                                                                                   XFS4IoT.VendorMode.Commands.ExitModeRequestCommand.PayloadData payload, 
                                                                                                   CancellationToken cancellation);

        /// <summary>
        /// This command is issued by a registered application as an acknowledgement to the[VendorMode.ExitModeRequest](#vendormode.exitmoderequest) command and it indicates thatthe application is ready for the system to exit Vendor Mode. All registered applications (including theapplication that issued the request to exit Vendor Mode) must respond before Vendor Mode willbe exited. Completion of this command is immediate.
        /// </summary>
        Task<XFS4IoT.VendorMode.Completions.ExitModeAcknowledgeCompletion.PayloadData> ExitModeAcknowledge(IExitModeAcknowledgeEvents events, 
                                                                                                           XFS4IoT.VendorMode.Commands.ExitModeAcknowledgeCommand.PayloadData payload, 
                                                                                                           CancellationToken cancellation);

    }
}
