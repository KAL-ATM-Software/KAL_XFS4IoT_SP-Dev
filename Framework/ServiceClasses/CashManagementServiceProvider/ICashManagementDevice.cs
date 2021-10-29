/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/


using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of cashmanagement. 
namespace XFS4IoTFramework.CashManagement
{
    public interface ICashManagementDevice : IDevice
    {
        /// <summary>
        /// This method will retract items which may have been in customer access from an output position or from internal areas within the CashDispenser. 
        /// Retracted items will be moved to either a retract cash unit, a reject cash unit, item cash units, the transport or the intermediate stacker. 
        /// After the items are retracted the shutter is closed automatically, even if the ShutterControl capability is set to false.
        /// </summary>
        Task<RetractResult> RetractAsync(IRetractEvents events,
                                         RetractRequest request,
                                         CancellationToken cancellation);

        /// <summary>
        /// OpenCloseShutterAsync
        /// Perform shutter operation to open or close.
        /// </summary>
        Task<OpenCloseShutterResult> OpenCloseShutterAsync(OpenCloseShutterRequest request,
                                                           CancellationToken cancellation);


        /// <summary>
        /// ResetDeviceAsync
        /// Perform a hardware reset which will attempt to return the CashDispenser device to a known good state.
        /// </summary>
        Task<ResetDeviceResult> ResetDeviceAsync(IResetEvents events,
                                                 ResetDeviceRequest request,
                                                 CancellationToken cancellation);

        /// <summary>
        /// This method unlocks the safe door or starts the timedelay count down prior to unlocking the safe door, 
        /// if the device supports it. The command completes when the door is unlocked or the timer has started.
        /// </summary>
        Task<UnlockSafeResult> UnlockSafeAsync(CancellationToken cancellation);

        /// <summary>
        /// This method will cause a vendor dependent sequence of hardware events which will calibrate one or more physical cash units associated with a logical cash unit.
        /// </summary>
        Task<CalibrateCashUnitResult> CalibrateCashUnitAsync(ICalibrateCashUnitEvents events, 
                                                             CalibrateCashUnitRequest request, 
                                                             CancellationToken cancellation);
        /// <summary>
        /// This command only applies to Teller devices. It allows the application to obtain counts for each currency assigned to the teller.
        /// These counts represent the total amount of currency dispensed by the teller in all transactions.
        /// This command also enables the application to obtain the position assigned to each teller. The teller information is persistent.
        /// </summary>
        /// <returns></returns>
        Task<GetTellerInfoResult> GetTellerInfoAsync(GetTellerInfoRequest request,
                                                     CancellationToken cancellation);


        /// <summary>
        /// This command allows the application to initialize counts for each currency assigned to the teller.The values set by this command
        /// are persistent.This command only applies to Teller ATMs.
        /// </summary>
        /// <returns></returns>
        Task<SetTellerInfoResult> SetTellerInfoAsync(SetTellerInfoRequest request,
                                                     CancellationToken cancellation);
    }
}
