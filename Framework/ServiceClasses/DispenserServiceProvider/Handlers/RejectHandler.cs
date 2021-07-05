/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.CashManagement;
using System.Linq;

namespace XFS4IoTFramework.Dispenser
{
    public partial class RejectHandler
    {
        private async Task<RejectCompletion.PayloadData> HandleReject(IRejectEvents events, RejectCommand reject, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.RejectAsync()");

            // Find reject unit
            bool foundDestination = false;
            foreach (var _ in from unit in Dispenser.CashUnits
                              where unit.Value.Type == CashUnit.TypeEnum.RejectCassette
                              select new { })
            {
                foundDestination = true;
            }

            if (!foundDestination)
            {
                return new RejectCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                        $"No reject units supported for this device.");
            }

            var result = await Device.RejectAsync(events, cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.RejectAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            Dispenser.UpdateCashUnitAccounting(result.MovementResult);

            return new RejectCompletion.PayloadData(result.CompletionCode, 
                                                    result.ErrorDescription, 
                                                    result.ErrorCode);
        }
    }
}
