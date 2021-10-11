/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
        private async Task<PinEntryCompletion.PayloadData> HandlePinEntry(IPinEntryEvents events, PinEntryCommand pinEntry, CancellationToken cancel)
        {
            if (pinEntry.Payload.MaxLen is null)
                Logger.Warning(Constants.Framework, $"No MaxLen specified. use default 0.");

            if (pinEntry.Payload.MinLen is null)
                Logger.Warning(Constants.Framework, $"No MinLen specified. use default 0.");

            if (pinEntry.Payload.AutoEnd is null)
                Logger.Warning(Constants.Framework, $"No AutoEnd specified. use default false.");

            if (!Keyboard.SupportedFunctionKeys.ContainsKey(EntryModeEnum.Pin))
            {
                return new PinEntryCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                            $"No Pin entry layout supported.");
            }

            List<ActiveKeyCalss> keys = new();
            foreach (var key in pinEntry.Payload.ActiveKeys)
            {
                if (!Keyboard.SupportedFunctionKeys[EntryModeEnum.Pin].Contains(key.Key))
                {
                    return new PinEntryCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                              $"Invalid key specified. {key.Key}");
                }
                keys.Add(new ActiveKeyCalss(key.Key, key.Value.Terminate is not null && (bool)key.Value.Terminate));
            }

            Logger.Log(Constants.DeviceClass, "KeyboardDev.PinEntry()");

            var result = await Device.PinEntry(events, 
                                               new(pinEntry.Payload.MinLen is null ? 0 : (int)pinEntry.Payload.MinLen,
                                                   pinEntry.Payload.MaxLen is null ? 0 : (int)pinEntry.Payload.MaxLen,
                                                   pinEntry.Payload.AutoEnd is not null && (bool)pinEntry.Payload.AutoEnd,
                                                   pinEntry.Payload.Echo,
                                                   keys), 
                                               cancel);

            Logger.Log(Constants.DeviceClass, $"KeyboardDev.PinEntry() -> {result.CompletionCode}, {result.ErrorCode}");

            return new PinEntryCompletion.PayloadData(result.CompletionCode,
                                                      result.ErrorDescription,
                                                      result.ErrorCode,
                                                      result.Digits,
                                                      result.Completion);
        }
    }
}
