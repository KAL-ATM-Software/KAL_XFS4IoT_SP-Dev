/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Completions;
using XFS4IoT.Storage.Commands;
using XFS4IoT.Storage.Completions;

namespace XFS4IoTFramework.Storage
{
    public partial class EndExchangeHandler
    {
        private async Task<EndExchangeCompletion.PayloadData> HandleEndExchange(IEndExchangeEvents events, EndExchangeCommand endExchange, CancellationToken cancel)
        {
            if (Storage.StorageType == StorageTypeEnum.Cash)
            {
                if (Storage.CashManagementCapabilities is null ||
                    Storage.CashManagementCapabilities.ExchangeTypes == Common.CashManagementCapabilitiesClass.ExchangeTypesEnum.NotSupported)
                {
                    return new EndExchangeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                 $"No capabilites reported via CashManagement for the exchange operation.");
                }
            }

            Logger.Log(Constants.DeviceClass, "StorageDev.EndExchangeAsync()");
            var result = await Device.EndExchangeAsync(cancel);
            Logger.Log(Constants.DeviceClass, $"StorageDev.EndExchangeAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new EndExchangeCompletion.PayloadData(result.CompletionCode,
                                                         result.ErrorDescription,
                                                         result.ErrorCode);
        }
    }
}
