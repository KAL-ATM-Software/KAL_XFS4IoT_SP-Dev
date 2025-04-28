/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.German.Commands;
using XFS4IoT.German.Completions;
using System.Text.RegularExpressions;

namespace XFS4IoTFramework.German
{
    public partial class HSMInitHandler
    {
        private async Task<CommandResult<HSMInitCompletion.PayloadData>> HandleHSMInit(IHSMInitEvents events, HSMInitCommand hSMInit, CancellationToken cancel)
        {
            if (hSMInit.Payload is null)
            {
                throw new InvalidDataException($"No payload is set.");
            }

            if (!string.IsNullOrEmpty(hSMInit.Payload.OnlineTime) && 
                !Regex.IsMatch(hSMInit.Payload.OnlineTime, @"^[0-9]{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9][0-5][0-9]$"))
            {
                throw new InvalidDataException($"Invalid OnlineTime value. {hSMInit.Payload.OnlineTime}");
            }

            Logger.Log(Constants.DeviceClass, "GermanDev.HSMInit()");
            var result = await Device.HSMInitAsync(
                new(
                    InitMode: hSMInit.Payload.InitMode switch
                    {
                        HSMInitCommand.PayloadData.InitModeEnum.Temp => HSMInitRequest.InitModeEnum.Temp,
                        HSMInitCommand.PayloadData.InitModeEnum.Definite => HSMInitRequest.InitModeEnum.Definite,
                        HSMInitCommand.PayloadData.InitModeEnum.Irreversible => HSMInitRequest.InitModeEnum.Irreversible,
                        _ => throw new InvalidDataException($"Invalid InitMode value. {hSMInit.Payload.InitMode}")
                    },
                    OnlineTime: hSMInit.Payload.OnlineTime),
                cancel);
            Logger.Log(Constants.DeviceClass, $"GermanDev.HSMInit() -> {result.CompletionCode}");

            return new(
                Payload: result.ErrorCode is null ? null: 
                new(ErrorCode: result.ErrorCode switch
                {
                    HSMInitResponse.ErrorCodeEnum.ModeNotSupported => HSMInitCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported,
                    HSMInitResponse.ErrorCodeEnum.HSMStateInvalid => HSMInitCompletion.PayloadData.ErrorCodeEnum.HsmStateInvalid,
                    _ => throw new InternalErrorException($"Unexpected error code. {result.ErrorCode}"),
                }),
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
