/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Keyboard
{
    public partial class KeypressBeepHandler
    {
        private async Task<KeypressBeepCompletion.PayloadData> HandleKeypressBeep(IKeypressBeepEvents events, KeypressBeepCommand keypressBeep, CancellationToken cancel)
        {
            KeyboardBeepEnum beep = KeyboardBeepEnum.Off;
            if (keypressBeep.Payload.Mode?.Active is not null &&
                (bool)keypressBeep.Payload.Mode?.Active)
            {
                beep |= KeyboardBeepEnum.Active;
            }
            if (keypressBeep.Payload.Mode?.Inactive is not null &&
                (bool)keypressBeep.Payload.Mode?.Inactive)
            {
                beep |= KeyboardBeepEnum.InActive;
            }

            Logger.Log(Constants.DeviceClass, "KeyboardDev.SetKeypressBeep()");

            var result = await Device.SetKeypressBeep(beep, cancel);

            Logger.Log(Constants.DeviceClass, $"KeyboardDev.SetKeypressBeep() -> {result.CompletionCode}");

            return new KeypressBeepCompletion.PayloadData(result.CompletionCode,
                                                          result.ErrorDescription);
        }
    }
}
