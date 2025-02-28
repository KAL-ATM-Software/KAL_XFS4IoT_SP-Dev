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
using XFS4IoT.Keyboard;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Keyboard
{
    public partial class DataEntryHandler
    {
        private async Task<CommandResult<DataEntryCompletion.PayloadData>> HandleDataEntry(IDataEntryEvents events, DataEntryCommand dataEntry, CancellationToken cancel)
        {
            if (dataEntry.Payload.MaxLen is null)
                Logger.Warning(Constants.Framework, $"No MaxLen specified. use default 0.");

            if (dataEntry.Payload.AutoEnd is null)
                Logger.Warning(Constants.Framework, $"No AutoEnd specified. use default false.");

            if (dataEntry.Payload.ActiveKeys is null || 
                dataEntry.Payload.ActiveKeys.Count == 0)
            {
                return new(
                    new(DataEntryCompletion.PayloadData.ErrorCodeEnum.NoActivekeys),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"No active keys are specified.");
            }

            if (!Keyboard.SupportedFunctionKeys.ContainsKey(EntryModeEnum.Data))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No Data entry layout supported.");
            }

            List<ActiveKeyClass> keys = new();
            foreach (var key in dataEntry.Payload.ActiveKeys)
            {
                if (!Keyboard.SupportedFunctionKeys[EntryModeEnum.Data].Contains(key.Key))
                {
                    return new(
                        new(DataEntryCompletion.PayloadData.ErrorCodeEnum.KeyNotSupported),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Invalid key specified. {key.Key}");
                }
                keys.Add(new ActiveKeyClass(key.Key, key.Value.Terminate is not null && (bool)key.Value.Terminate));
            }

            Logger.Log(Constants.DeviceClass, "KeyboardDev.DataEntry()");

            var result = await Device.DataEntry(
                new KeyboardCommandEvents(events), 
                new(dataEntry.Payload.MaxLen is null ? 0 : (int)dataEntry.Payload.MaxLen,
                    dataEntry.Payload.AutoEnd is not null && (bool)dataEntry.Payload.AutoEnd,
                    keys), 
                cancel);

            Logger.Log(Constants.DeviceClass, $"KeyboardDev.DataEntry() -> {result.CompletionCode}, {result.ErrorCode}");

            List<KeyPressedClass> keysPressed = null;
            if (result.EnteredKeys is not null &&
                result.EnteredKeys.Count > 0)
            {
                keysPressed = new();
                foreach (var key in result.EnteredKeys)
                    keysPressed.Add(
                        new(
                            key.Completion switch
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
                            }, 
                            key.Key)
                        );
            }

            DataEntryCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.Completion is not null)
            {
                payload = new(
                    result.ErrorCode,
                    result.Keys,
                    keysPressed,
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
