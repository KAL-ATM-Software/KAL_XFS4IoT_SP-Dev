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
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTFramework.PinPad
{
    public partial class LocalVisaHandler
    {
        private async Task<LocalVisaCompletion.PayloadData> HandleLocalVisa(ILocalVisaEvents events, LocalVisaCommand localVisa, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(localVisa.Payload.Pan))
            {
                return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No customer data specified.");
            }
            else
            {
                if (localVisa.Payload.Pan.Length != 23)
                {
                    return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Invalid length of pan specified. {localVisa.Payload.Pan.Length}");
                }

                foreach (char c in localVisa.Payload.Pan)
                {
                    if (!Char.IsDigit(c))
                    {
                        return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Offset data should be number. {localVisa.Payload.Pan}");
                    }
                }
            }
            if (string.IsNullOrEmpty(localVisa.Payload.Pvv))
            {
                return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                          $"No key name specified to verify PIN locally.");
            }
            else if (localVisa.Payload.Pvv.Length < 4)
            {
                return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Pin Valification Value must be minimum 4 digits.");
            }
            else
            {
                foreach (char c in localVisa.Payload.Pvv)
                {
                    if (!Char.IsDigit(c))
                    {
                        return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"PIN valification value should be number. {localVisa.Payload.Pvv}");
                    }
                }
            }

            KeyDetail key = PinPad.GetKeyDetail(localVisa.Payload.Key);
            if (key is null)
            {
                return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          $"Specified key is not loaded.",
                                                          LocalVisaCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
            }

            if (key.KeyUsage != "V2")
            {
                return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                          $"Specified key usage is not expected.{key.KeyUsage}",
                                                          LocalVisaCompletion.PayloadData.ErrorCodeEnum.UseViolation);
            }

            if (!string.IsNullOrEmpty(localVisa.Payload.KeyEncKey))
            {
                KeyDetail keyEncKey = PinPad.GetKeyDetail(localVisa.Payload.KeyEncKey);
                if (keyEncKey is null)
                {
                    return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Specified key encryption key is not loaded.",
                                                               LocalVisaCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
                }

                if (keyEncKey.KeyUsage != "K0" &&
                    keyEncKey.KeyUsage != "K1" &&
                    keyEncKey.KeyUsage != "K2" &&
                    keyEncKey.KeyUsage != "K3")
                {
                    return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Specified key encryption key usage is not expected.{keyEncKey.KeyUsage}",
                                                               LocalVisaCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                } 
            }

            if (string.IsNullOrEmpty(localVisa.Payload.Pvv))
            {
                return new LocalVisaCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No PIN Validation Value specified.");
            }

            Logger.Log(Constants.DeviceClass, "PinPadDev.VerifyPINLocalVISA()");

            var result = await Device.VerifyPINLocalVISA(new VerifyPINLocalVISARequest(localVisa.Payload.Pan,
                                                                                       localVisa.Payload.Pvv,
                                                                                       localVisa.Payload.Key,
                                                                                       localVisa.Payload.KeyEncKey),
                                                         cancel);

            Logger.Log(Constants.DeviceClass, $"PinPadDev.VerifyPINLocalVISA() -> {result.CompletionCode}, {result.ErrorCode}");

            return new LocalVisaCompletion.PayloadData(result.CompletionCode,
                                                       result.ErrorDescription,
                                                       result.ErrorCode switch
                                                       {
                                                          VerifyPINLocalResult.ErrorCodeEnum.AccessDenied => LocalVisaCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                                                          VerifyPINLocalResult.ErrorCodeEnum.FormatNotSupported => LocalVisaCompletion.PayloadData.ErrorCodeEnum.FormatNotSupported,
                                                          VerifyPINLocalResult.ErrorCodeEnum.InvalidKeyLength => LocalVisaCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength,
                                                          VerifyPINLocalResult.ErrorCodeEnum.KeyNotFound => LocalVisaCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                                                          VerifyPINLocalResult.ErrorCodeEnum.KeyNoValue => LocalVisaCompletion.PayloadData.ErrorCodeEnum.KeyNoValue,
                                                          VerifyPINLocalResult.ErrorCodeEnum.NoPin => LocalVisaCompletion.PayloadData.ErrorCodeEnum.NoPin,
                                                          _ => LocalVisaCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                                                       },
                                                       result.Verified);
        }
    }
}
