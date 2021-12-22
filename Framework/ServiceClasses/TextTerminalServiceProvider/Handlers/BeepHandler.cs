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
            if (beep.Payload.Beep.Continuous is not null)
            {
                action = (bool)beep.Payload.Beep.Continuous ? BeepRequest.BeepActionEnum.Continuous : BeepRequest.BeepActionEnum.Off;
            }

            BeepRequest.BeepTypeEnum type = BeepRequest.BeepTypeEnum.None;
            if (beep.Payload.Beep.BeepType is not null)
            {
                type = beep.Payload.Beep.BeepType switch
                {
                    BeepCommand.PayloadData.BeepClass.BeepTypeEnum.Critical => BeepRequest.BeepTypeEnum.Critical,
                    BeepCommand.PayloadData.BeepClass.BeepTypeEnum.Error => BeepRequest.BeepTypeEnum.Error,
                    BeepCommand.PayloadData.BeepClass.BeepTypeEnum.Exclamation => BeepRequest.BeepTypeEnum.Exclamation,
                    BeepCommand.PayloadData.BeepClass.BeepTypeEnum.KeyPress => BeepRequest.BeepTypeEnum.KeyPress,
                    BeepCommand.PayloadData.BeepClass.BeepTypeEnum.Warning => BeepRequest.BeepTypeEnum.Warning,
                    _ => BeepRequest.BeepTypeEnum.None,
                };
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
