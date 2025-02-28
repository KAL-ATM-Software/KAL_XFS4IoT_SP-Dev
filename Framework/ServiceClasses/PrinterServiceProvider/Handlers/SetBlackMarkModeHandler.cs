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
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Printer
{
    public partial class SetBlackMarkModeHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleSetBlackMarkMode(ISetBlackMarkModeEvents events, SetBlackMarkModeCommand setBlackMarkMode, CancellationToken cancel)
        {
            if (setBlackMarkMode.Payload.BlackMarkMode is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No black mark mode specified.");
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.SetBlackMarkModeAsync()");
            var result = await Device.SetBlackMarkModeAsync(
                setBlackMarkMode.Payload.BlackMarkMode is not null && (bool)setBlackMarkMode.Payload.BlackMarkMode ? BlackMarkModeEnum.On : BlackMarkModeEnum.Off, 
                cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.SetBlackMarkModeAsync() -> {result.CompletionCode}");

            return new(
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
