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
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;

namespace XFS4IoTFramework.CashManagement
{
    public partial class EndExchangeHandler
    {
        private async Task<EndExchangeCompletion.PayloadData> HandleEndExchange(IEndExchangeEvents events, EndExchangeCommand endExchange, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CashManagementDev.CompleteExchangeAsync()");

            var result = await Device.CompleteExchangeAsync(events, new CompleteExchangeRequest(), cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.CompleteExchangeAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new EndExchangeCompletion.PayloadData(result.CompletionCode,
                                                         result.ErrorDescription,
                                                         result.ErrorCode);
        }
    }
}
