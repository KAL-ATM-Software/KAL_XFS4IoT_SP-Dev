/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class PrepareDispenseHandler
    {
        private async Task<CommandResult<PrepareDispenseCompletion.PayloadData>> HandlePrepareDispense(IPrepareDispenseEvents events, PrepareDispenseCommand prepareDispense, CancellationToken cancel)
        {
            if (prepareDispense.Payload.Action is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    "Index property is set to null where the retract area is specified to retract position.");
            }

            Logger.Log(Constants.DeviceClass, "CashDispenserDev.PrepareDispenseAsync()");

            var result = await Device.PrepareDispenseAsync(new PrepareDispenseRequest((prepareDispense.Payload.Action == PrepareDispenseCommand.PayloadData.ActionEnum.Start) ? PrepareDispenseRequest.ActionEnum.Start : PrepareDispenseRequest.ActionEnum.Stop), cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.PrepareDispenseAsync() -> {result.CompletionCode}");


            return new(
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
