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
    public partial class ClearScreenHandler
    {

        private async Task<ClearScreenCompletion.PayloadData> HandleClearScreen(IClearScreenEvents events, ClearScreenCommand clearScreen, CancellationToken cancel)
        {
            ClearScreenRequest request;

            // First check if all values are null - then clear entire screen. 
            if(clearScreen.Payload.PositionX is null && clearScreen.Payload.PositionY is null && clearScreen.Payload.Width is null && clearScreen.Payload.Height is null)
            {
                request = new ClearScreenRequest(0, 0, Device.CurrentHeight, Device.CurrentWidth);
            }
            // Check if PositionX is null
            else if (clearScreen.Payload.PositionX is null)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "No PositionX is specified.");
            }
            // Check if PositionY is null
            else if (clearScreen.Payload.PositionY is null)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "No PositionY is specified.");
            }
            // Check if Width is null
            else if (clearScreen.Payload.Width is null)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "No Width is specified.");
            }
            // Check if Height is null
            else if (clearScreen.Payload.Height is null)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "No Height is specified.");
            }
            //Ensure values are positive
            else if (clearScreen.Payload.PositionX < 0 || clearScreen.Payload.PositionY < 0 || clearScreen.Payload.Width < 0 || clearScreen.Payload.Height < 0)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "PositionX,PositionY,Width,Height cannot be negative.");
            }
            // Check rectangle is within the current resolution
            else if (clearScreen.Payload.PositionX + clearScreen.Payload.Width > Device.CurrentWidth ||
                     clearScreen.Payload.PositionY + clearScreen.Payload.Height > Device.CurrentHeight)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Invalid rectangle specified for the current resolution in ClearScreen.");
            }
            // Else we have a valid rectangle to use
            else
            {
                request = new ClearScreenRequest((int)clearScreen.Payload.PositionX, (int)clearScreen.Payload.PositionY, (int)clearScreen.Payload.Height, (int)clearScreen.Payload.Width);
            }

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.ClearScreenAsync()");

            var result = await Device.ClearScreenAsync(request, cancel);

            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.ClearScreenAsync() -> {result.CompletionCode}");

            return new ClearScreenCompletion.PayloadData(result.CompletionCode,
                                                         result.ErrorDescription);

        }

    }
}
