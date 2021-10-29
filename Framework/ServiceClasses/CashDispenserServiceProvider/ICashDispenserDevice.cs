/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoTFramework.Common;

// KAL specific implementation of dispenser. 
namespace XFS4IoTFramework.CashDispenser
{
    public interface ICashDispenserDevice : IDevice
    {
        /// <summary>
        /// This method performs the dispensing of items to the customer. 
        /// </summary>
        Task<DispenseResult> DispenseAsync(IDispenseEvents events, 
                                           DispenseRequest request, 
                                           CancellationToken cancellation);

        /// <summary>
        /// PresentCashAsync
        /// This method will move items to the exit position for removal by the user. 
        /// If a shutter exists, then it will be implicitly controlled during the present operation, even if the ShutterControl capability is set to false.
        /// The shutter will be closed when the user removes the items or the items are retracted. 
        /// </summary>
        Task<PresentCashResult> PresentCashAsync(IPresentEvents events, 
                                                 PresentCashRequest request, 
                                                 CancellationToken cancellation);

        /// <summary>
        /// This method will move items from the intermediatestacker and transport them to a reject cash unit (i.e. a cash unit with type rejectCassette). 
        /// The count field of the reject cash unit is incremented by the number of items that were thought to be present at the time of the reject or the number counted by the device during the reject.
        /// Note that the reject bin countis unreliable.
        /// </summary>
        Task<RejectResult> RejectAsync(IRejectEvents events, 
                                       CancellationToken cancellation);

        /// <summary>
        /// This method is used to test cash units following replenishment.
        /// All physical cash units which are testable (i.e. that have a status of ok or low and no application lock in the the physical cash unit) are tested.
        /// If the hardware is able to do so tests are continued even if an error occurs while testing one of the cash units. 
        /// The method completes with success if the device successfully manages to test all of the testable cash units regardless of the outcome of the test. 
        /// This is the case if all testable cash units could be tested and a dispense was possible from at least one of the cash units.
        /// </summary>
        Task<TestCashUnitsResult> TestCashUnitsAsync(ITestCashUnitsEvents events, 
                                                     TestCashUnitsRequest request, 
                                                     CancellationToken cancellation);

        /// <summary>
        /// CountAsync
        /// Perform count operation to empty the specified physical cash unit(s). 
        /// All items dispensed from the cash unit are counted and moved to the specified output location.
        /// </summary>
        Task<CountResult> CountAsync(ICountEvents events, 
                                     CountRequest request,
                                     CancellationToken cancellation);

        /// <summary>
        /// PrepareDispenseAsync
        /// On some hardware it can take a significant amount of time for the dispenser to get ready to dispense media. 
        /// On this type of hardware the this method can be used to improve transaction performance.
        /// </summary>
        Task<PrepareDispenseResult> PrepareDispenseAsync(PrepareDispenseRequest request,
                                                         CancellationToken cancellation);


        /// <summary>
        /// GetPresentStatus
        /// This method returns the status of the most recent attempt to dispense and/or present items to the customer from a specified output position.
        /// Throw NotImplementedException if the device specific class doesn't support to manage present status.
        /// </summary>
       PresentStatus GetPresentStatus(CashDispenserCapabilitiesClass.OutputPositionEnum position);
    }
}
