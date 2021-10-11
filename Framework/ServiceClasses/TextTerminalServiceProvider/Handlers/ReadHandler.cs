/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/


using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;

namespace XFS4IoTFramework.TextTerminal
{
    public partial class ReadHandler
    {

        private async Task<ReadCompletion.PayloadData> HandleRead(IReadEvents events, ReadCommand read, CancellationToken cancel)
        {
            // Check for any active keys or command keys.
            if (string.IsNullOrWhiteSpace(read.Payload.ActiveKeys) && 
                (read.Payload.ActiveCommandKeys is null || read.Payload.ActiveCommandKeys.Count == 0))
            {
                return new ReadCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode, "No Keys specified for Read command.", ReadCompletion.PayloadData.ErrorCodeEnum.NoActiveKeys);
            }

            // Get autoEnd value.
            bool autoEnd = read.Payload.AutoEnd is not null && (bool)read.Payload.AutoEnd;

            // Check for either AutoEnd or TerminateCommandKeys. Read will never end if neither are specified.
            if((read.Payload.ActiveCommandKeys?.FirstOrDefault(c => c.Value.Terminate is true) is null) && !autoEnd)
            {
                return new ReadCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "Device cannot determine when to end Read operation.");
            }

            // Get KeyDetails if not cached yet.
            if (TextTerminal.FirstGetKeyDetailCommand)
            {
                TextTerminal.UpdateKeyDetails();
                TextTerminal.FirstGetKeyDetailCommand = false;
            }

            // Check all ActiveKeys are supported.
            if (!string.IsNullOrEmpty(read.Payload.ActiveKeys))
            {
                foreach (char c in read.Payload.ActiveKeys)
                {
                    if (!TextTerminal.SupportedKeys.Keys.Contains(c))
                    {
                        return new ReadCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, $"Key {c} is not supported by the device.", ReadCompletion.PayloadData.ErrorCodeEnum.KeyNotSupported);
                    }
                }
            }
            
            // Check all active command keys are supported.
            if(read.Payload.ActiveCommandKeys is not null)
            {
                foreach (var cmdKey in read.Payload.ActiveCommandKeys)
                {
                    if (!TextTerminal.SupportedKeys.CommandKeys.Contains(cmdKey.Key))
                    {
                        return new ReadCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, $"CommandKey {cmdKey} is not supported by the device.", ReadCompletion.PayloadData.ErrorCodeEnum.KeyNotSupported);
                    }
                }
            }

            if (read.Payload.NumOfChars is null)
            {
                return new ReadCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "NumOfChars is not supplied");
            }
            else if(read.Payload.Mode is null)
            {
                return new ReadCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "Mode is not supplied");
            }
            else if (read.Payload.PosX is null)
            {
                return new ReadCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "PosX is not supplied");
            }
            else if (read.Payload.PosY is null)
            {
                return new ReadCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "PosY is not supplied");
            }
            else if (read.Payload.Echo is null)
            {
                return new ReadCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "Echo is not supplied");
            }
            else if (read.Payload.Flush is null)
            {
                return new ReadCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "Flush is not supplied");
            }
            else if (read.Payload.EchoAttr is null)
            {
                return new ReadCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "EchoAttr is not supplied");
            }
            else if (read.Payload.EchoMode is null)
            {
                return new ReadCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.InvalidData, "EchoMode is not supplied");
            }

            ReadRequest.EchoModeEnum echo = read.Payload.EchoMode.Value switch
            {
                ReadCommand.PayloadData.EchoModeEnum.Text => ReadRequest.EchoModeEnum.Text,
                ReadCommand.PayloadData.EchoModeEnum.Invisible => ReadRequest.EchoModeEnum.Invisible,
                ReadCommand.PayloadData.EchoModeEnum.Password => ReadRequest.EchoModeEnum.Password,
                _ => throw new InvalidDataException("Unknown EchoMode value. " + read.Payload.EchoMode.Value)
            };

            ReadRequest.TextAttributesEnum echoAttr = ReadRequest.TextAttributesEnum.None;
            if (read.Payload.EchoAttr.Underline is not null && (bool)read.Payload.EchoAttr.Underline)
                echoAttr |= ReadRequest.TextAttributesEnum.Underline;
            if (read.Payload.EchoAttr.Inverted is not null && (bool)read.Payload.EchoAttr.Inverted)
                echoAttr |= ReadRequest.TextAttributesEnum.Inverted;
            if (read.Payload.EchoAttr.Flash is not null && (bool)read.Payload.EchoAttr.Flash)
                echoAttr |= ReadRequest.TextAttributesEnum.Flash;

            int PosX, PosY;

            // Calculate X,Y for absolute or relative
            if(read.Payload.Mode.Value == XFS4IoT.TextTerminal.ModesEnum.Absolute)
            {

                PosX = read.Payload.PosX.Value;
                PosY = read.Payload.PosY.Value;
            }
            else
            {
                PosX = Device.CurrentX + read.Payload.PosX.Value;
                PosY = Device.CurrentY + read.Payload.PosY.Value;
            }

            // Check x,y are valid
            if(PosX >= Device.CurrentWidth)
            {
                return new ReadCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Invalid PosX supplied. Value is outside terminal Width.");
            }
            if(PosY >= Device.CurrentHeight)
            {
                return new ReadCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Invalid PosY supplied. Value is outside terminal Height.");
            }

            // Check x + num chars is within the current width
            if(PosX + read.Payload.NumOfChars.Value > Device.CurrentWidth)
            {
                return new ReadCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, "Attempted to read outside current width.");
            }

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.ReadAsync()");
            
            var result = await Device.ReadAsync(new(echo, echoAttr, PosX, PosY, read.Payload.NumOfChars.Value, read.Payload.Echo.Value, read.Payload.Flush.Value, autoEnd, read.Payload.ActiveKeys ?? string.Empty, read.Payload.ActiveCommandKeys?.Keys.ToList() ?? new(), read.Payload.ActiveCommandKeys?.Where(c => c.Value.Terminate is true).Select(c => c.Key).ToList() ?? new()), cancel);
            
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.ReadAsync() -> {result.CompletionCode}");

            return new ReadCompletion.PayloadData(result.CompletionCode, result.ErrorDescription, null, result.Input);
        }

    }
}
