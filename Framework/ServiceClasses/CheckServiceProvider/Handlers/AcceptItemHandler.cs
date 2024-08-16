/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    public partial class AcceptItemHandler
    {
        private async Task<CommandResult<AcceptItemCompletion.PayloadData>> HandleAcceptItem(IAcceptItemEvents events, AcceptItemCommand acceptItem, CancellationToken cancel)
        {
            if (acceptItem.Payload is null ||
                acceptItem.Payload.Accept is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Required property Accept is not specified.");
            }

            Logger.Log(Constants.DeviceClass, "CheckDev.AcceptItemAsync()");

            var result = await Device.AcceptItemAsync(
                request: new((bool)acceptItem.Payload.Accept),
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"CheckDev.AcceptItemAsync() -> {result.CompletionCode}");

            return new(
                result.ErrorCode is not null ? new(ErrorCode: result.ErrorCode) : null,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
