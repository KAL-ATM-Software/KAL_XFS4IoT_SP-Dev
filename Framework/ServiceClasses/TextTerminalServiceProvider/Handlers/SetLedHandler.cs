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
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.TextTerminal
{
    public partial class SetLedHandler
    {

        private async Task<SetLedCompletion.PayloadData> HandleSetLed(ISetLedEvents events, SetLedCommand setLed, CancellationToken cancel)
        {

            if(setLed.Payload.Led.Value < 0 || setLed.Payload.Led.Value >= TextTerminal.TextTerminalCapabilities.LEDSupported.Count)
            {
                return new SetLedCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, "Invalid LED specified for SetLED.", SetLedCompletion.PayloadData.ErrorCodeEnum.InvalidLed);
            }

            if (setLed.Payload.Command.Off is not null && (bool)setLed.Payload.Command.Off)
            {

                Logger.Log(Constants.DeviceClass, "TextTerminalDev.LEDOffAsync()");

                var ledOffResult = await Device.LEDOffAsync(setLed.Payload.Led.Value, cancel);

                Logger.Log(Constants.DeviceClass, $"TextTerminalDev.LEDOffAsync() -> {ledOffResult.CompletionCode}");

                return new SetLedCompletion.PayloadData(ledOffResult.CompletionCode, ledOffResult.ErrorDescription);
            }


            TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum control = TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.None;
            TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum colour = TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.None;

            //Get specified control.
            if (setLed.Payload.Command.SlowFlash is not null && (bool)setLed.Payload.Command.SlowFlash)
                control = TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.SlowFlash;
            else if (setLed.Payload.Command.MediumFlash is not null && (bool)setLed.Payload.Command.MediumFlash)
                control = TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.MediumFlash;
            else if (setLed.Payload.Command.QuickFlash is not null && (bool)setLed.Payload.Command.QuickFlash)
                control = TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.QuickFlash;
            else if (setLed.Payload.Command.Continuous is not null && (bool)setLed.Payload.Command.Continuous)
                control = TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.Continuous;
            else
                return new SetLedCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "SetLED Command is not specified.");

            // Get Specified colour.
            if (setLed.Payload.Command.Red is not null && (bool)setLed.Payload.Command.Red)
                colour = TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Red;
            else if (setLed.Payload.Command.Green is not null && (bool)setLed.Payload.Command.Green)
                colour = TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Green;
            else if (setLed.Payload.Command.Yellow is not null && (bool)setLed.Payload.Command.Yellow)
                colour = TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Yellow;
            else if (setLed.Payload.Command.Blue is not null && (bool)setLed.Payload.Command.Blue)
                colour = TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Blue;
            else if (setLed.Payload.Command.Cyan is not null && (bool)setLed.Payload.Command.Cyan)
                colour = TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Cyan;
            else if (setLed.Payload.Command.Magenta is not null && (bool)setLed.Payload.Command.Magenta)
                colour = TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Magenta;
            else if (setLed.Payload.Command.White is not null && (bool)setLed.Payload.Command.White)
                colour = TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.White;


            var LedSupported = TextTerminal.TextTerminalCapabilities.LEDSupported[setLed.Payload.Led.Value];
            if (!LedSupported.LightControl.HasFlag(control))
            {
                return new SetLedCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, $"LED {setLed.Payload.Led} does not support LightControl {control}.");
            }
            else if (colour != TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.None && !LedSupported.Color.HasFlag(colour))
            {
                return new SetLedCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, $"LED {setLed.Payload.Led} does not support Colour {colour}.");
            }

            LEDOnRequest request = new LEDOnRequest(setLed.Payload.Led.Value,
                control switch
                {
                    TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.SlowFlash => LEDOnRequest.LEDCommandEnum.SlowFlash,
                    TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.MediumFlash => LEDOnRequest.LEDCommandEnum.MediumFlash,
                    TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.QuickFlash => LEDOnRequest.LEDCommandEnum.QuickFlash,
                    TextTerminalCapabilitiesClass.LEDClass.LEDLightControlsEnum.Continuous => LEDOnRequest.LEDCommandEnum.Continuous,
                    _ => throw new NotImplementedException()
                },
                colour switch
                {
                    TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Red => LEDOnRequest.LEDColorEnum.Red,
                    TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Green => LEDOnRequest.LEDColorEnum.Green,
                    TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Yellow => LEDOnRequest.LEDColorEnum.Yellow,
                    TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Blue => LEDOnRequest.LEDColorEnum.Blue,
                    TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Cyan => LEDOnRequest.LEDColorEnum.Cyan,
                    TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.Magenta => LEDOnRequest.LEDColorEnum.Magenta,
                    TextTerminalCapabilitiesClass.LEDClass.LEDColorsEnum.White => LEDOnRequest.LEDColorEnum.White,
                    _ => LEDOnRequest.LEDColorEnum.Default // Device will decide which colour is Default.
                });


            Logger.Log(Constants.DeviceClass, "TextTerminalDev.LEDOnAsync()");

            var ledOnResult = await Device.LEDOnAsync(request, cancel);

            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.LEDOnAsync() -> {ledOnResult.CompletionCode}");

            return new SetLedCompletion.PayloadData(ledOnResult.CompletionCode, ledOnResult.ErrorDescription);
        }

    }
}
