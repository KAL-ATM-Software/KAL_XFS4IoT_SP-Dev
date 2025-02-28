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
using XFS4IoT.GermanSpecific.Commands;
using XFS4IoT.GermanSpecific.Completions;
using System.Text.RegularExpressions;

namespace XFS4IoTFramework.GermanSpecific
{
    public partial class SetHSMTDataHandler
    {
        private async Task<CommandResult<SetHSMTDataCompletion.PayloadData>> HandleSetHSMTData(ISetHSMTDataEvents events, SetHSMTDataCommand setHSMTData, CancellationToken cancel)
        {
            if (setHSMTData.Payload is null)
            {
                throw new InvalidDataException($"No payload is set.");
            }

            if (!Regex.IsMatch(setHSMTData.Payload.TerminalId, @"^[0-9]{8}$"))
            {
                throw new InternalErrorException($"Specified TerminalId contains invalid characters. {setHSMTData.Payload.TerminalId}");
            }
            if (!Regex.IsMatch(setHSMTData.Payload.BankCode, @"^[0-9]{8}$"))
            {
                throw new InternalErrorException($"Specified BankCode invalid characters. {setHSMTData.Payload.BankCode}");
            }
            if (!Regex.IsMatch(setHSMTData.Payload.OnlineDateAndTime, @"^20\\d{2}(0[1-9]|1[0,1,2])(0[1-9]|[12][0-9]|3[01])(0[0-9]|1[0-9]|2[0-3])[0-5][0-9][0-5][0-9]$"))
            {
                throw new InternalErrorException($"OnlineDateAndTime contains invalid characters. {setHSMTData.Payload.OnlineDateAndTime}");
            }

            Logger.Log(Constants.DeviceClass, "GermanSpecificDev.SetHSMTData()");
            var result = await Device.SetHSMTData(
                new(
                    TerminalId: setHSMTData.Payload.TerminalId,
                    BankCode: setHSMTData.Payload.BankCode,
                    OnlineDateAndTime: setHSMTData.Payload.OnlineDateAndTime),
                cancel);
            Logger.Log(Constants.DeviceClass, $"GermanSpecificDev.SetHSMTData() -> {result.CompletionCode}");

            return new(
                Payload: result.ErrorCode is null ? null :
                new(ErrorCode: result.ErrorCode switch
                {
                    SetHSMTDataResponse.ErrorCodeEnum.HSMStateInvalid => SetHSMTDataCompletion.PayloadData.ErrorCodeEnum.HsmStateInvalid,
                    SetHSMTDataResponse.ErrorCodeEnum.AccessDenied => SetHSMTDataCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                    _ => throw new InternalErrorException($"Unexpected error code. {result.ErrorCode}"),
                }),
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
