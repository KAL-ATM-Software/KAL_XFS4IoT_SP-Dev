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
        /// This method is called when the client application send CashUnitInfo command first time since executable runs or while exchange is in progress.
        /// Return true if the cash unit configuration is being changed since last call, otherwise false
        /// The key representing physical position name associated with the CashUnit structure.
        /// The key name should be unique to identify Physical Cash Unit
        /// </summary>
        bool GetCashUnitConfiguration(out Dictionary<string, CashUnitConfiguration> CashUnits);

        /// <summary>
        /// This method is called after device operation is completed involving cash movement
        /// returning false if the device doesn't support maintaining counters and framework will maintain counters.
        /// However if the device doesn't support maintaining counters, the counts are not guaranteed.
        /// The key name should be used for the GetCashUnitConfiguration output.
        /// </summary>
        Dictionary<string, CashUnitAccounting> GetCashUnitAccounting();

        /// <summary>
        /// This method is called after device operation is completed involving cash movement
        /// Return false if the device doesn't support handware sensor to detect cash unit status.
        /// The framework will use decide the cash unit status from the counts maintained by the framework.
        /// The key name should be used for the GetCashUnitConfiguration output.
        /// </summary>
        Dictionary<string, CashUnit.StatusEnum> GetCashUnitStatus();


        /// <summary>
        /// This method is used to adjust information about the status and contents of the cash units present in the CashDispenser or CashAcceptor device.
        /// </summary>
        Task<SetCashUnitInfoResult> SetCashUnitInfoAsync(ISetCashUnitInfoEvents events, 
                                                         SetCashUnitInfoRequest setCashUnitInfo, 
                                                         CancellationToken cancellation);

        /// <summary>
        /// This method unlocks the safe door or starts the timedelay count down prior to unlocking the safe door, 
        /// if the device supports it. The command completes when the door is unlocked or the timer has started.
        /// </summary>
        Task<UnlockSafeResult> UnlockSafeAsync(CancellationToken cancellation);

        /// <summary>
        /// InitiateExchange
        /// This method is called when the application initiated cash unit exchange by hand
        /// </summary>
        Task<InitiateExchangeResult> InitiateExchangeAsync(IStartExchangeEvents events,
                                                           InitiateExchangeRequest exchangeInfo, 
                                                           CancellationToken cancellation);

        /// <summary>
        /// InitiateClearRecyclerRequest
        /// This method is called when the application initiated to empty recycler units
        /// </summary>
        Task<InitiateExchangeResult> InitiateExchangeClearRecyclerAsync(IStartExchangeEvents events,
                                                                        InitiateClearRecyclerRequest exchangeInfo,
                                                                        CancellationToken cancellation);


        /// <summary>
        /// InitiateExchangeDepositIntoAsync
        /// This method is called when the application initiated to filling cash into the cash units via cash-in operation.
        /// Items will be moved from the deposit entrance to the bill cash units.
        /// </summary>
        Task<InitiateExchangeResult> InitiateExchangeDepositIntoAsync(IStartExchangeEvents events,
                                                                      CancellationToken cancellation);

        /// <summary>
        /// CompleteExchangeAsync
        /// This method will end the exchange state
        /// </summary>
        Task<CompleteExchangeResult> CompleteExchangeAsync(IEndExchangeEvents events, 
                                                           CompleteExchangeRequest exchangeInfo, 
                                                           CancellationToken cancellation);

        /// <summary>
        /// This method will cause a vendor dependent sequence of hardware events which will calibrate one or more physical cash units associated with a logical cash unit.
        /// </summary>
        Task<CalibrateCashUnitResult> CalibrateCashUnitAsync(ICalibrateCashUnitEvents events, 
                                                             CalibrateCashUnitRequest calibrationInfo, 
                                                             CancellationToken cancellation);

    }
}
