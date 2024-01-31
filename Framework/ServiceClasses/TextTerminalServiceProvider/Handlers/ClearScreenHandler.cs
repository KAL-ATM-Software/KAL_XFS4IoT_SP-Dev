/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
            if (clearScreen.Payload.Screen is null ||
                (clearScreen.Payload.Screen.PositionX is null && 
                 clearScreen.Payload.Screen.PositionY is null && 
                 clearScreen.Payload.Screen.Width is null && 
                 clearScreen.Payload.Screen.Height is null))
            {
                request = new ClearScreenRequest(0, 0, Device.CurrentHeight, Device.CurrentWidth);
            }
            // Check if PositionX is null
            else if (clearScreen.Payload.Screen.PositionX is null)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "No PositionX is specified.");
            }
            // Check if PositionY is null
            else if (clearScreen.Payload.Screen.PositionY is null)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "No PositionY is specified.");
            }
            // Check if Width is null
            else if (clearScreen.Payload.Screen.Width is null)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "No Width is specified.");
            }
            // Check if Height is null
            else if (clearScreen.Payload.Screen.Height is null)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "No Height is specified.");
            }
            //Ensure values are positive
            else if (clearScreen.Payload.Screen.PositionX < 0 || 
                     clearScreen.Payload.Screen.PositionY < 0 || 
                     clearScreen.Payload.Screen.Width < 0 || 
                     clearScreen.Payload.Screen.Height < 0)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "PositionX,PositionY,Width,Height cannot be negative.");
            }
            // Check rectangle is within the current resolution
            else if (clearScreen.Payload.Screen.PositionX + clearScreen.Payload.Screen.Width > Device.CurrentWidth ||
                     clearScreen.Payload.Screen.PositionY + clearScreen.Payload.Screen.Height > Device.CurrentHeight)
            {
                return new ClearScreenCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Invalid rectangle specified for the current resolution in ClearScreen.");
            }
            // Else we have a valid rectangle to use
            else
            {
                request = new ClearScreenRequest((int)clearScreen.Payload.Screen.PositionX, (int)clearScreen.Payload.Screen.PositionY, (int)clearScreen.Payload.Screen.Height, (int)clearScreen.Payload.Screen.Width);
            }

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.ClearScreenAsync()");

            var result = await Device.ClearScreenAsync(request, cancel);

            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.ClearScreenAsync() -> {result.CompletionCode}");

            return new ClearScreenCompletion.PayloadData(result.CompletionCode,
                                                         result.ErrorDescription);

        }

    }
}
