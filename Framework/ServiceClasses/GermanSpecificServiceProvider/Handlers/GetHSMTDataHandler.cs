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
    public partial class GetHSMTDataHandler
    {
        private async Task<CommandResult<GetHSMTDataCompletion.PayloadData>> HandleGetHSMTData(IGetHSMTDataEvents events, GetHSMTDataCommand getHSMTData, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "GermanSpecificDev.GetHSMTData()");
            var result = await Device.GetHSMTData(cancel);
            Logger.Log(Constants.DeviceClass, $"GermanSpecificDev.GetHSMTData() -> {result.CompletionCode}");

            GetHSMTDataCompletion.PayloadData payload = null;
            if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                if (!Regex.IsMatch(result.HSMTDataRead.TerminalId, @"^[0-9]{8}$"))
                {
                    throw new InternalErrorException($"Specified TerminalId contains invalid characters. {result.HSMTDataRead.TerminalId}");
                }
                if (!Regex.IsMatch(result.HSMTDataRead.BankCode, @"^[0-9]{8}$"))
                {
                    throw new InternalErrorException($"Specified BankCode invalid characters. {result.HSMTDataRead.BankCode}");
                }
                if (!Regex.IsMatch(result.HSMTDataRead.OnlineDateAndTime, @"^20\\d{2}(0[1-9]|1[0,1,2])(0[1-9]|[12][0-9]|3[01])(0[0-9]|1[0-9]|2[0-3])[0-5][0-9][0-5][0-9]$"))
                {
                    throw new InternalErrorException($"OnlineDateAndTime contains invalid characters. {result.HSMTDataRead.OnlineDateAndTime}");
                }
                if (!Regex.IsMatch(result.HSMTDataRead.ZAKId, @"^.{16}$"))
                {
                    throw new InternalErrorException($"ZAKId contains invalid characters. {result.HSMTDataRead.ZAKId}");
                }
                if (!Regex.IsMatch(result.HSMTDataRead.HSMManufacturerId, @"^.{6,}$"))
                {
                    throw new InternalErrorException($"HSMManufacturerId contains invalid characters. {result.HSMTDataRead.HSMManufacturerId}");
                }
                if (!Regex.IsMatch(result.HSMTDataRead.HSMSerialNumber, @"^.{10}$"))
                {
                    throw new InternalErrorException($"HSMSerialNumber contains invalid characters. {result.HSMTDataRead.HSMSerialNumber}");
                }
                payload = new(
                    TerminalId: result.HSMTDataRead.TerminalId,
                    BankCode: result.HSMTDataRead.BankCode,
                    OnlineDateAndTime: result.HSMTDataRead.OnlineDateAndTime,
                    ZkaId: result.HSMTDataRead.ZAKId,
                    HsmStatus: (int)result.HSMTDataRead.HSMStatus,
                    HsmManufacturerId: result.HSMTDataRead.HSMManufacturerId,
                    HsmSerialNumber: result.HSMTDataRead.HSMSerialNumber);
            }

            return new(
                Payload: payload,
                result.CompletionCode, 
                result.ErrorDescription);
        }
    }
}
