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
using XFS4IoT.Storage.Commands;
using XFS4IoT.Storage.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Storage
{
    public partial class EndExchangeHandler
    {
        private async Task<CommandResult<EndExchangeCompletion.PayloadData>> HandleEndExchange(IEndExchangeEvents events, EndExchangeCommand endExchange, CancellationToken cancel)
        {
            if (Storage.StorageType.HasFlag(StorageTypeEnum.Cash))
            {
                if (Common.CashManagementCapabilities is null ||
                    Common.CashManagementCapabilities.ExchangeTypes == CashManagementCapabilitiesClass.ExchangeTypesEnum.NotSupported)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                        $"No capabilites reported via CashManagement for the exchange operation.");
                }
            }

            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.NotSupported)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                    $"The exchange command is not supported.");
            }

            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Inactive)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.Success,
                    $"The exchange state is already in active.");
            }

            Logger.Log(Constants.DeviceClass, "StorageDev.EndExchangeAsync()");
            var result = await Device.EndExchangeAsync(cancel);
            Logger.Log(Constants.DeviceClass, $"StorageDev.EndExchangeAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                Common.CommonStatus.Exchange = CommonStatusClass.ExchangeEnum.Inactive;
            }

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
