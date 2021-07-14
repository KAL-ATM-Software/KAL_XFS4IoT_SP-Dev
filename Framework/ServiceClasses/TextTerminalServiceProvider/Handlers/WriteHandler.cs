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
using System.Collections.Generic;
using System.Text;

namespace XFS4IoTFramework.TextTerminal
{
    public partial class WriteHandler
    {

        private async Task<WriteCompletion.PayloadData> HandleWrite(IWriteEvents events, WriteCommand write, CancellationToken cancel)
        {
            if (write.Payload.Mode is null)
            {
                return new WriteCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Write Mode is not supplied.");
            }

            else if (write.Payload.PosX is null)
            {
                return new WriteCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Write PosX is not supplied.");
            }
            else if (write.Payload.PosY is null)
            {
                return new WriteCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Write PosY is not supplied.");
            }
            else if (write.Payload.PosX < 0 || write.Payload.PosY < 0)
            {
                return new WriteCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Write PosX,PosY cannot be negative.");
            }
            else if (write.Payload.TextAttr is null)
            {
                return new WriteCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Write TextAttr is not supplied.");
            }
            else if (string.IsNullOrEmpty(write.Payload.Text))
            {
                return new WriteCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Write Text is not supplied.");
            }


            int PosX, PosY;
            // Calculate X,Y for absolute or relative
            if (write.Payload.Mode.Value == XFS4IoT.TextTerminal.ModesEnum.Absolute)
            {

                PosX = write.Payload.PosX.Value;
                PosY = write.Payload.PosY.Value;
            }
            else
            {
                PosX = Device.CurrentX + write.Payload.PosX.Value;
                PosY = Device.CurrentY + write.Payload.PosY.Value;
            }
            // Check x,y are valid
            if (PosX < 0 || PosX >= Device.CurrentWidth)
            {
                return new WriteCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "PositionX is outside the current resolution width.");
            }
            else if (PosY < 0 || PosY >= Device.CurrentHeight)
            {
                return new WriteCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "PositionY is outside the current resolution width.");
            }

            // Evaluate the text attributes.
            WriteRequest.TextAttributesEnum textAttributes = WriteRequest.TextAttributesEnum.None;

            if (write.Payload.TextAttr.Underline is not null && (bool)write.Payload.TextAttr.Underline)
                textAttributes |= WriteRequest.TextAttributesEnum.Underline;
            if (write.Payload.TextAttr.Inverted is not null && (bool)write.Payload.TextAttr.Inverted)
                textAttributes |= WriteRequest.TextAttributesEnum.Inverted;
            if (write.Payload.TextAttr.Flash is not null && (bool)write.Payload.TextAttr.Flash)
                textAttributes |= WriteRequest.TextAttributesEnum.Flash;

            string text = write.Payload.Text;
            StringBuilder buffer = new(Device.CurrentWidth);

            DeviceResult result = new(MessagePayload.CompletionCodeEnum.Success);
            bool flush;

            for(int i = 0; i < text.Length && result.CompletionCode == MessagePayload.CompletionCodeEnum.Success; i++)
            {
                if (text[i] == '\n' || text[i] == '\r') // Check for new line within text. \n or \r
                {
                    if (i + 1 < text.Length && (text[i + 1] == '\n' || text[i + 1] == '\r')) ++i; //Skip next char in case CR + LF or LF + CR
                    flush = true;
                }
                else
                {
                    buffer.Append(text[i]); //Update buffer.

                    // Flush the buffer if this is the last char or we have reached the device width.
                    if (i == text.Length - 1 || PosX + buffer.Length == Device.CurrentWidth) flush = true;
                    else flush = false;
                }

                if (flush) //Write the line within buffer.
                {
                    //Check if we need to scroll the text displayed.
                    if(PosY >= Device.CurrentHeight)
                    {
                        //Check if scrolling is supported by the device
                        if (Device.ScrollingSupported)
                        {
                            Logger.Log(Constants.DeviceClass, "TextTerminalDev.ScrollAsync()");

                            result = await Device.ScrollAsync(cancel);

                            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.ScrollAsync() -> {result.CompletionCode}");

                            if (result.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                            {
                                break; // Scroll failed.
                            }

                            PosY = Device.CurrentHeight - 1; //Write to last line on the display now we have scrolled to make space.
                        }
                        else
                        {
                            Logger.Log(Constants.DeviceClass, "Scrolling is not supported by the device. Write operation will end with unwritten text.");
                            break; // Stop write operation.
                        }
                        
                    }


                    Logger.Log(Constants.DeviceClass, "TextTerminalDev.WriteAsync()");

                    result = await Device.WriteAsync(new WriteRequest(PosX, PosY, buffer.ToString(), textAttributes), cancel);

                    Logger.Log(Constants.DeviceClass, $"TextTerminalDev.WriteAsync() -> {result.CompletionCode}");

                    //Move onto the start of the next line.
                    PosX = 0;
                    ++PosY;
                    buffer.Clear();
                }
            }

            return new WriteCompletion.PayloadData(result.CompletionCode, result.ErrorDescription);
        }

    }
}
