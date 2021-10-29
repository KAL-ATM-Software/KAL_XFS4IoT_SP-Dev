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
    public partial class StartExchangeHandler
    {
        private async Task<StartExchangeCompletion.PayloadData> HandleStartExchange(IStartExchangeEvents events, StartExchangeCommand startExchange, CancellationToken cancel)
        {
            if (Storage.StorageType == StorageTypeEnum.Cash)
            {
                if (Storage.CashManagementCapabilities is null ||
                    Storage.CashManagementCapabilities.ExchangeTypes == Common.CashManagementCapabilitiesClass.ExchangeTypesEnum.NotSupported)
                {
                    return new StartExchangeCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                                 $"No capabilites reported via CashManagement for the exchange operation.");
                }
            }

            Logger.Log(Constants.DeviceClass, "StorageDev.StartExchangeAsync()");
            var result = await Device.StartExchangeAsync(cancel);
            Logger.Log(Constants.DeviceClass, $"StorageDev.StartExchangeAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            return new StartExchangeCompletion.PayloadData(result.CompletionCode,
                                                           result.ErrorDescription,
                                                           result.ErrorCode);
        }
    }
}
