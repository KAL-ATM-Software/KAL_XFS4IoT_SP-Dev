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
using XFS4IoT.Completions;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;

namespace XFS4IoTFramework.TextTerminal
{
    public partial class SetResolutionHandler
    {

        private async Task<SetResolutionCompletion.PayloadData> HandleSetResolution(ISetResolutionEvents events, SetResolutionCommand setResolution, CancellationToken cancel)
        {
            
            if(setResolution.Payload.Resolution is null || setResolution.Payload.Resolution.SizeX is null || setResolution.Payload.Resolution.SizeY is null)
            {
                return new SetResolutionCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Resolution is not specified.");
            }

            // Ensure the selected resolution is valid 
            bool found = false;
            foreach (var supportedRes in TextTerminal.TextTerminalCapabilities.Resolutions)
            {
                if(supportedRes.Width == setResolution.Payload.Resolution.SizeX && supportedRes.Height == setResolution.Payload.Resolution.SizeY)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                return new SetResolutionCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Selected resolution is not supported.", SetResolutionCompletion.PayloadData.ErrorCodeEnum.ResolutionNotSupported);
            }

            //Clear screen before setting resolution.
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.ClearScreenAsync()");

            var result = await Device.ClearScreenAsync(new(0, 0, Device.CurrentHeight, Device.CurrentWidth), cancel);

            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.ClearScreenAsync() -> {result.CompletionCode}");

            //Set the resolution
            Logger.Log(Constants.DeviceClass, "TextTerminalDev.SetResolutionAsync()");

            result = await Device.SetResolutionAsync(new((int)setResolution.Payload.Resolution.SizeX, (int)setResolution.Payload.Resolution.SizeY), cancel);

            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.SetResolutionAsync() -> {result.CompletionCode}");
            

            return new SetResolutionCompletion.PayloadData(result.CompletionCode, result.ErrorDescription);

        }

    }
}
