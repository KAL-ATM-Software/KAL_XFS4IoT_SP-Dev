/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Keyboard
{
    public partial class PinEntryHandler
    {
        private async Task<CommandResult<PinEntryCompletion.PayloadData>> HandlePinEntry(IPinEntryEvents events, PinEntryCommand pinEntry, CancellationToken cancel)
        {
            if (pinEntry.Payload.MaxLen is null)
                Logger.Warning(Constants.Framework, $"No MaxLen specified. use default 0.");

            if (pinEntry.Payload.MinLen is null)
                Logger.Warning(Constants.Framework, $"No MinLen specified. use default 0.");

            if (pinEntry.Payload.AutoEnd is null)
                Logger.Warning(Constants.Framework, $"No AutoEnd specified. use default false.");

            if (!Keyboard.SupportedFunctionKeys.ContainsKey(EntryModeEnum.Pin))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No Pin entry layout supported.");
            }

            List<ActiveKeyClass> keys = new();
            foreach (var key in pinEntry.Payload.ActiveKeys)
            {
                if (!Keyboard.SupportedFunctionKeys[EntryModeEnum.Pin].Contains(key.Key))
                {
                    return new(
                        new(PinEntryCompletion.PayloadData.ErrorCodeEnum.KeyNotSupported),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Invalid key specified. {key.Key}");
                }
                keys.Add(new ActiveKeyClass(key.Key, key.Value.Terminate is not null && (bool)key.Value.Terminate));
            }

            Logger.Log(Constants.DeviceClass, "KeyboardDev.PinEntry()");

            var result = await Device.PinEntry(
                new KeyboardCommandEvents(events), 
                new(pinEntry.Payload.MinLen is null ? 0 : (int)pinEntry.Payload.MinLen,
                    pinEntry.Payload.MaxLen is null ? 0 : (int)pinEntry.Payload.MaxLen,
                    pinEntry.Payload.AutoEnd is not null && (bool)pinEntry.Payload.AutoEnd,
                    pinEntry.Payload.Echo,
                    keys), 
                cancel);

            Logger.Log(Constants.DeviceClass, $"KeyboardDev.PinEntry() -> {result.CompletionCode}, {result.ErrorCode}");

            PinEntryCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.Completion is not null)
            {
                payload = new(
                    result.ErrorCode,
                    result.Digits,
                    result.Completion switch
                    {
                        EntryCompletionEnum.Auto => XFS4IoT.Keyboard.EntryCompletionEnum.Auto,
                        EntryCompletionEnum.Enter => XFS4IoT.Keyboard.EntryCompletionEnum.Enter,
                        EntryCompletionEnum.Cancel => XFS4IoT.Keyboard.EntryCompletionEnum.Cancel,
                        EntryCompletionEnum.Continue => XFS4IoT.Keyboard.EntryCompletionEnum.Continue,
                        EntryCompletionEnum.Clear => XFS4IoT.Keyboard.EntryCompletionEnum.Clear,
                        EntryCompletionEnum.Backspace => XFS4IoT.Keyboard.EntryCompletionEnum.Backspace,
                        EntryCompletionEnum.FDK => XFS4IoT.Keyboard.EntryCompletionEnum.Fdk,
                        EntryCompletionEnum.Help => XFS4IoT.Keyboard.EntryCompletionEnum.Help,
                        EntryCompletionEnum.FK => XFS4IoT.Keyboard.EntryCompletionEnum.Fk,
                        EntryCompletionEnum.ContinueFDK => XFS4IoT.Keyboard.EntryCompletionEnum.ContFdk,
                        _ => null,
                    });
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
