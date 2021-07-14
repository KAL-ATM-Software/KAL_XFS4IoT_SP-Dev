/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.TextTerminal
{
    public partial class DispLightHandler
    {
        private async Task<DispLightCompletion.PayloadData> HandleDispLight(IDispLightEvents events, DispLightCommand dispLight, CancellationToken cancel)
        {
            if (!TextTerminal.TextTerminalCapabilities.DisplayLight)
            {
                return new DispLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand,
                                                           $"The DisplayLight command is not supported. see capabilities.");
            }

            if(dispLight.Payload.Mode is null)
            {
                return new DispLightCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "DispLight Mode is not specified.");
            }

            DeviceResult result;

            if ((bool)dispLight.Payload.Mode)
            {
                Logger.Log(Constants.DeviceClass, "TextTerminalDev.DispLightOnAsync()");
                result = await Device.DispLightOnAsync(cancel);
                Logger.Log(Constants.DeviceClass, $"TextTerminalDev.DispLightOnAsync() -> {result.CompletionCode}");
            }
            else
            {
                Logger.Log(Constants.DeviceClass, "TextTerminalDev.DispLightOffAsync()");
                result = await Device.DispLightOffAsync(cancel);
                Logger.Log(Constants.DeviceClass, $"TextTerminalDev.DispLightOffAsync() -> {result.CompletionCode}");
            }

            return new DispLightCompletion.PayloadData(result.CompletionCode,
                                                       result.ErrorDescription);
        }
    }
}
