/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/

using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashManagement
{
    public interface ICashManagementDevice : IDevice
    {
        /// <summary>
        /// This method will retract items which may have been in customer access from an output position or from internal areas within the CashDispenser. 
        /// Retracted items will be moved to either a retract cash unit, a reject cash unit, item cash units, the transport or the intermediate stacker. 
        /// After the items are retracted the shutter is closed automatically, even if the ShutterControl capability is set to false.
        /// </summary>
        Task<RetractResult> RetractAsync(RetractCommandEvents events,
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
        Task<ResetDeviceResult> ResetDeviceAsync(ResetCommandEvents events,
                                                 ResetDeviceRequest request,
                                                 CancellationToken cancellation);


        /// <summary>
        /// This method will cause a vendor dependent sequence of hardware events which will calibrate one or more physical cash units associated with a logical cash unit.
        /// </summary>
        Task<CalibrateCashUnitResult> CalibrateCashUnitAsync(CalibrateCashUnitCommandEvents events, 
                                                             CalibrateCashUnitRequest request, 
                                                             CancellationToken cancellation);
        /// <summary>
        /// This command only applies to Teller devices. It allows the application to obtain counts for each currency assigned to the teller.
        /// These counts represent the total amount of currency dispensed by the teller in all transactions.
        /// This command also enables the application to obtain the position assigned to each teller. The teller information is persistent.
        /// </summary>
        Task<GetTellerInfoResult> GetTellerInfoAsync(GetTellerInfoRequest request,
                                                     CancellationToken cancellation);


        /// <summary>
        /// This command allows the application to initialize counts for each currency assigned to the teller.The values set by this command
        /// are persistent.This command only applies to Teller ATMs.
        /// </summary>
        Task<SetTellerInfoResult> SetTellerInfoAsync(SetTellerInfoRequest request,
                                                     CancellationToken cancellation);

        /// <summary>
        /// This command is used to get information about detected items. It can be used to get information about individual items,
        /// all items of a certain classification, or all items that have information available.This information is available from
        /// the point where the first CashManagement.InfoAvailableEvent is generated until a
        /// transaction or replenishment command is executed including the following:
        /// 
        /// CashAcceptor.CashInStart
        /// CashAcceptor.CashIn
        /// CashAcceptor.CashInEnd
        /// CashAcceptor.CashInRollback
        /// CashAcceptor.CreateSignature
        /// CashAcceptor.Replenish
        /// CashAcceptor.CashUnitCount
        /// CashAcceptor.Deplete
        /// CashManagement.Retract
        /// CashManagement.Reset
        /// CashManagement.OpenShutter
        /// CashManagement.CloseShutter
        /// CashManagement.CalibrateCashUnit
        /// CashDispenser.Dispense
        /// CashDispenser.Present
        /// CashDispenser.Reject
        /// CashDispenser.Count
        /// CashDispenser.TestCashUnits
        /// Storage.StartExchange
        /// Storage.EndExchange
        /// 
        /// In addition, since the item information is not cumulative and can be replaced by any command that can move notes, it is
        /// recommended that applications that are interested in the available information should query for it following the
        /// CashManagement.InfoAvailableEvent* but before any other command is executed.
        /// </summary>
        GetItemInfoResult GetItemInfoInfo(GetItemInfoRequest request);

        /// <summary>
        /// CashManagement Status
        /// </summary>
        CashManagementStatusClass CashManagementStatus { get; set; }

        /// <summary>
        /// CashManagement Capabilities
        /// </summary>
        CashManagementCapabilitiesClass CashManagementCapabilities { get; set; }
    }
}
