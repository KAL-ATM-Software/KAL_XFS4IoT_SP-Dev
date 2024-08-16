/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Printer
{
    public partial class SupplyReplenishHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleSupplyReplenish(ISupplyReplenishEvents events, SupplyReplenishCommand supplyReplenish, CancellationToken cancel)
        {
            SupplyReplenishedRequest.SupplyEnum supplies = SupplyReplenishedRequest.SupplyEnum.NotSupported;

            if (supplyReplenish.Payload.Aux is not null &&
                (bool)supplyReplenish.Payload.Aux)
            {
                supplies |= SupplyReplenishedRequest.SupplyEnum.AUX;
            }
            if (supplyReplenish.Payload.Aux2 is not null &&
                (bool)supplyReplenish.Payload.Aux2)
            {
                supplies |= SupplyReplenishedRequest.SupplyEnum.AUX2;
            }
            if (supplyReplenish.Payload.Upper is not null &&
                (bool)supplyReplenish.Payload.Upper)
            {
                supplies |= SupplyReplenishedRequest.SupplyEnum.Upper;
            }
            if (supplyReplenish.Payload.Lower is not null &&
                (bool)supplyReplenish.Payload.Lower)
            {
                supplies |= SupplyReplenishedRequest.SupplyEnum.Lower;
            }
            if (supplyReplenish.Payload.Toner is not null &&
                (bool)supplyReplenish.Payload.Toner)
            {
                supplies |= SupplyReplenishedRequest.SupplyEnum.Toner;
            }
            if (supplyReplenish.Payload.Ink is not null &&
                (bool)supplyReplenish.Payload.Ink)
            {
                supplies |= SupplyReplenishedRequest.SupplyEnum.Ink;
            }
            if (supplyReplenish.Payload.Lamp is not null &&
                (bool)supplyReplenish.Payload.Lamp)
            {
                supplies |= SupplyReplenishedRequest.SupplyEnum.Lamp;
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.SupplyReplenishedAsync()");
            var result = await Device.SupplyReplenishedAsync(new (supplies), cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.SupplyReplenishedAsync() -> {result.CompletionCode}");

            return new(
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
