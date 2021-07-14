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

namespace XFS4IoTFramework.TextTerminal
{
    public partial class BeepHandler
    {
        private async Task<BeepCompletion.PayloadData> HandleBeep(IBeepEvents events, BeepCommand beep, CancellationToken cancel)
        {
            if (beep.Payload.Beep is null)
            {
                return new BeepCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                      $"No beep flag is specified.");
            }

            BeepRequest.BeepActionEnum action = BeepRequest.BeepActionEnum.On;
            if (beep.Payload.Beep.Continuous is not null &&
                (bool)beep.Payload.Beep.Continuous)
            {
                action = BeepRequest.BeepActionEnum.Continuous;
            }
            else if (beep.Payload.Beep.Off is not null &&
                     (bool)beep.Payload.Beep.Off)
            {
                action = BeepRequest.BeepActionEnum.Off;
            }

            BeepRequest.BeepTypeEnum type = BeepRequest.BeepTypeEnum.None;
            if (beep.Payload.Beep.KeyPress is not null &&
                (bool)beep.Payload.Beep.KeyPress)
            {
                type = BeepRequest.BeepTypeEnum.KeyPress;
            }
            else if (beep.Payload.Beep.Warning is not null &&
                     (bool)beep.Payload.Beep.Warning)
            {
                type = BeepRequest.BeepTypeEnum.Warning;
            }
            else if (beep.Payload.Beep.Error is not null &&
                     (bool)beep.Payload.Beep.Error)
            {
                type = BeepRequest.BeepTypeEnum.Error;
            }
            else if (beep.Payload.Beep.Exclamation is not null &&
                     (bool)beep.Payload.Beep.Exclamation)
            {
                type = BeepRequest.BeepTypeEnum.Exclamation;
            }
            else if (beep.Payload.Beep.Critical is not null &&
                    (bool)beep.Payload.Beep.Critical)
            {
                type = BeepRequest.BeepTypeEnum.Critical;
            }

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.BeepAsync()");
            
            var result = await Device.BeepAsync(new BeepRequest(type, action), 
                                                cancel);
            
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.BeepAsync() -> {result.CompletionCode}");

            return new BeepCompletion.PayloadData(result.CompletionCode,
                                                  result.ErrorDescription);
        }
    }
}
