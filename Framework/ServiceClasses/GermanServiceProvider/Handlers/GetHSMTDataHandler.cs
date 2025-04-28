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
    public partial class GetHSMTDataHandler
    {
        private async Task<CommandResult<GetHSMTDataCompletion.PayloadData>> HandleGetHSMTData(IGetHSMTDataEvents events, GetHSMTDataCommand getHSMTData, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "GermanDev.GetHSMTData()");
            var result = await Device.GetHSMTDataAsync(cancel);
            Logger.Log(Constants.DeviceClass, $"GermanDev.GetHSMTData() -> {result.CompletionCode}");

            GetHSMTDataCompletion.PayloadData payload = null;
            if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                if (result.HSMTDataRead is not null)
                {
                    if (!string.IsNullOrEmpty(result.HSMTDataRead.TerminalId) &&
                        !Regex.IsMatch(result.HSMTDataRead.TerminalId, @"^[0-9]{8}$"))
                    {
                        throw new InternalErrorException($"Specified TerminalId contains invalid characters. {result.HSMTDataRead.TerminalId}");
                    }
                    if (!string.IsNullOrEmpty(result.HSMTDataRead.BankCode) && 
                        !Regex.IsMatch(result.HSMTDataRead.BankCode, @"^[0-9]{8}$"))
                    {
                        throw new InternalErrorException($"Specified BankCode invalid characters. {result.HSMTDataRead.BankCode}");
                    }
                    if (!string.IsNullOrEmpty(result.HSMTDataRead.OnlineDateAndTime) && 
                        !Regex.IsMatch(result.HSMTDataRead.OnlineDateAndTime, @"^20\d{2}(0[1-9]|1[0,1,2])(0[1-9]|[12][0-9]|3[01])(0[0-9]|1[0-9]|2[0-3])[0-5][0-9][0-5][0-9]$"))
                    {
                        throw new InternalErrorException($"OnlineDateAndTime contains invalid characters. {result.HSMTDataRead.OnlineDateAndTime}");
                    }
                    if (!string.IsNullOrEmpty(result.HSMTDataRead.ZAKId) && 
                        !Regex.IsMatch(result.HSMTDataRead.ZAKId, @"^.{16}$"))
                    {
                        throw new InternalErrorException($"ZAKId contains invalid characters. {result.HSMTDataRead.ZAKId}");
                    }
                    if (!string.IsNullOrEmpty(result.HSMTDataRead.HSMManufacturerId) && 
                        !Regex.IsMatch(result.HSMTDataRead.HSMManufacturerId, @"^.{6,}$"))
                    {
                        throw new InternalErrorException($"HSMManufacturerId contains invalid characters. {result.HSMTDataRead.HSMManufacturerId}");
                    }
                    if (!string.IsNullOrEmpty(result.HSMTDataRead.HSMSerialNumber) && 
                        !Regex.IsMatch(result.HSMTDataRead.HSMSerialNumber, @"^.{10}$"))
                    {
                        throw new InternalErrorException($"HSMSerialNumber contains invalid characters. {result.HSMTDataRead.HSMSerialNumber}");
                    }
                    payload = new(
                        TerminalId: string.IsNullOrEmpty(result.HSMTDataRead.TerminalId) ? null : result.HSMTDataRead.TerminalId,
                        BankCode: string.IsNullOrEmpty(result.HSMTDataRead.BankCode) ? null : result.HSMTDataRead.BankCode,
                        OnlineDateAndTime: string.IsNullOrEmpty(result.HSMTDataRead.OnlineDateAndTime) ? null : result.HSMTDataRead.OnlineDateAndTime,
                        ZkaId: string.IsNullOrEmpty(result.HSMTDataRead.ZAKId) ? null : result.HSMTDataRead.ZAKId,
                        HsmStatus: (int?)result.HSMTDataRead.HSMStatus,
                        HsmManufacturerId: string.IsNullOrEmpty(result.HSMTDataRead.HSMManufacturerId) ? null : result.HSMTDataRead.HSMManufacturerId,
                        HsmSerialNumber: string.IsNullOrEmpty(result.HSMTDataRead.HSMSerialNumber) ? null : result.HSMTDataRead.HSMSerialNumber);
                }
            }

            return new(
                Payload: payload,
                result.CompletionCode, 
                result.ErrorDescription);
        }
    }
}
