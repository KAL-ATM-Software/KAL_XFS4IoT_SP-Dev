/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;

namespace XFS4IoTFramework.TextTerminal
{
    public partial class ReadHandler
    {

        private async Task<CommandResult<ReadCompletion.PayloadData>> HandleRead(IReadEvents events, ReadCommand read, CancellationToken cancel)
        {
            // Check for any active keys or command keys.
            if ((read.Payload.ActiveKeys is null ||
                 read.Payload.ActiveKeys.Count == 0 ) && 
                (read.Payload.ActiveCommandKeys is null || 
                 read.Payload.ActiveCommandKeys.Count == 0))
            {
                return new(
                    new(ErrorCode: ReadCompletion.PayloadData.ErrorCodeEnum.NoActiveKeys),
                    CompletionCode: MessageHeader.CompletionCodeEnum.CommandErrorCode, 
                    ErrorDescription: $"No Keys specified for Read command.");
            }

            // Get autoEnd value.
            bool autoEnd = true;
            if (read.Payload.AutoEnd is not null && !(bool)read.Payload.AutoEnd)
            {
                autoEnd = false;
            }

            // Check for either AutoEnd or TerminateCommandKeys. Read will never end if neither are specified.
            if ((read.Payload.ActiveCommandKeys?.FirstOrDefault(c => c.Value.Terminate is true) is null) && !autoEnd)
            {
                return new(
                    CompletionCode: MessageHeader.CompletionCodeEnum.InvalidData,
                    ErrorDescription: $"Device cannot determine when to end Read operation.");
            }

            // Get KeyDetails if not cached yet.
            if (TextTerminal.FirstGetKeyDetailCommand)
            {
                TextTerminal.UpdateKeyDetails();
                TextTerminal.FirstGetKeyDetailCommand = false;
            }

            // Check all ActiveKeys are supported.
            if (read.Payload.ActiveKeys is not null)
            {
                foreach (var activeKey in read.Payload.ActiveKeys)
                {
                    if (!TextTerminal.SupportedKeys.Keys.Contains(activeKey))
                    {
                        return new(
                            new(ReadCompletion.PayloadData.ErrorCodeEnum.KeyNotSupported),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode, 
                            $"Key {activeKey} is not supported by the device.");
                    }
                }
            }
            
            // Check all active command keys are supported.
            if(read.Payload.ActiveCommandKeys is not null)
            {
                foreach (var cmdKey in read.Payload.ActiveCommandKeys)
                {
                    if (!TextTerminal.SupportedKeys.CommandKeys.ContainsKey(cmdKey.Key))
                    {
                        return new(
                            new(ReadCompletion.PayloadData.ErrorCodeEnum.KeyNotSupported),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode, 
                            $"CommandKey {cmdKey} is not supported by the device.");
                    }
                }
            }

            if (read.Payload.NumOfChars is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "NumOfChars is not supplied");
            }
            else if(read.Payload.Mode is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "Mode is not supplied");
            }
            else if (read.Payload.PosX is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "PosX is not supplied");
            }
            else if (read.Payload.PosY is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "PosY is not supplied");
            }
            else if (read.Payload.Flush is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "Flush is not supplied");
            }

            ReadRequest.EchoModeEnum echo = read.Payload.EchoMode.Value switch
            {
                ReadCommand.PayloadData.EchoModeEnum.Invisible => ReadRequest.EchoModeEnum.Invisible,
                ReadCommand.PayloadData.EchoModeEnum.Password => ReadRequest.EchoModeEnum.Password,
                _ => ReadRequest.EchoModeEnum.Text,
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
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "Invalid PosX supplied. Value is outside terminal Width.");
            }
            if(PosY >= Device.CurrentHeight)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "Invalid PosY supplied. Value is outside terminal Height.");
            }

            // Check x + num chars is within the current width
            if(PosX + read.Payload.NumOfChars.Value > Device.CurrentWidth)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    "Attempted to read outside current width.");
            }

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.ReadAsync()");
            
            var result = await Device.ReadAsync(new ReadCommandEvents(events),
                                                new ReadRequest(echo, 
                                                                echoAttr, 
                                                                PosX, 
                                                                PosY, 
                                                                read.Payload.NumOfChars.Value, 
                                                                read.Payload.Visible.Value, 
                                                                read.Payload.Flush.Value, 
                                                                autoEnd, 
                                                                read.Payload.ActiveKeys ?? [], 
                                                                read.Payload.ActiveCommandKeys?.Keys.ToList() ?? [], 
                                                                read.Payload.ActiveCommandKeys?.Where(c => c.Value.Terminate is true).Select(c => c.Key).ToList() ?? []),
                                                cancel);
            
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.ReadAsync() -> {result.CompletionCode}");

            return new(
                string.IsNullOrEmpty(result.Input) ? null : new(Input: result.Input),
                result.CompletionCode, 
                result.ErrorDescription);
        }

    }
}
