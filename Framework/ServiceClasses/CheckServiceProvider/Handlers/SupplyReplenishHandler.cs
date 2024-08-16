/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;

namespace XFS4IoTFramework.Check
{
    public partial class SupplyReplenishHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleSupplyReplenish(ISupplyReplenishEvents events, SupplyReplenishCommand supplyReplenish, CancellationToken cancel)
        {
            if (supplyReplenish.Payload is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    $"No payload specified.");
            }

            if (supplyReplenish.Payload.Ink is null &&
                supplyReplenish.Payload.Toner is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    $"No ink or toner specified for a replenishment.");
            }

            SupplyReplenishRequest.SupplyEnum supplies = 0;
            if (supplyReplenish.Payload.Ink is not null && (bool)supplyReplenish.Payload.Ink)
            {
                supplies |= SupplyReplenishRequest.SupplyEnum.Ink;
            }
            if (supplyReplenish.Payload.Toner is not null && (bool)supplyReplenish.Payload.Toner)
            {
                supplies |= SupplyReplenishRequest.SupplyEnum.Toner;
            }

            Logger.Log(Constants.DeviceClass, "CheckDev.SupplyReplenishedAsync()");
            var result = await Device.SupplyReplenishAsync(new(supplies), cancel);
            Logger.Log(Constants.DeviceClass, $"CheckDev.SupplyReplenishedAsync() -> {result.CompletionCode}");

            return new(
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
