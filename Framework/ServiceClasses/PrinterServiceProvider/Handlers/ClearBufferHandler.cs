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
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Printer
{
    public partial class ClearBufferHandler
    {
        private async Task<CommandResult<MessagePayloadBase>> HandleClearBuffer(IClearBufferEvents events, ClearBufferCommand clearBuffer, CancellationToken cancel)
        {
            if (!Common.PrinterCapabilities.Controls.HasFlag(XFS4IoTFramework.Common.PrinterCapabilitiesClass.ControlEnum.ClearBuffer))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    "The device has no capability to clear the buffer.");
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.ControlMediaAsync()");
            var result = await Device.ControlMediaAsync(
                new ControlMediaEvent(events),
                new ControlMediaRequest(PrinterCapabilitiesClass.ControlEnum.ClearBuffer),
                cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ControlMediaAsync() -> {result.CompletionCode}, {result.ErrorCode}");


            return new(
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
