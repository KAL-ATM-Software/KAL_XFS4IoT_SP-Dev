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
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTServer;

namespace XFS4IoTFramework.PinPad
{
    public partial class LocalVisaHandler
    {
        private async Task<CommandResult<LocalVisaCompletion.PayloadData>> HandleLocalVisa(ILocalVisaEvents events, LocalVisaCommand localVisa, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(localVisa.Payload.Pan))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No customer data specified.");
            }
            else
            {
                if (localVisa.Payload.Pan.Length != 23)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Invalid length of pan specified. {localVisa.Payload.Pan.Length}");
                }

                foreach (char c in localVisa.Payload.Pan)
                {
                    if (!Char.IsDigit(c))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Offset data should be number. {localVisa.Payload.Pan}");
                    }
                }
            }
            if (string.IsNullOrEmpty(localVisa.Payload.Pvv))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No key name specified to verify PIN locally.");
            }
            else if (localVisa.Payload.Pvv.Length < 4)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Pin Valification Value must be minimum 4 digits.");
            }
            else
            {
                foreach (char c in localVisa.Payload.Pvv)
                {
                    if (!Char.IsDigit(c))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"PIN valification value should be number. {localVisa.Payload.Pvv}");
                    }
                }
            }

            KeyDetail key = KeyManagement.GetKeyDetail(localVisa.Payload.Key);
            if (key is null)
            {
                return new(
                    new(LocalVisaCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key is not loaded.");
            }

            if (localVisa.Payload.KeyEncKey is null ||
                localVisa.Payload.KeyEncKey.Count == 0)
            {
                // Loaded key is to veirfy PIN using imported key
                if (key.KeyUsage != "V2")
                {
                    return new(
                        new(LocalVisaCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified key usage is not having expected key usage to verify PIN.{key.KeyUsage}");
                }
            }
            else
            {
                // Loaded key is used to decrypt data
                if (key.KeyUsage != "D0" &&
                    key.KeyUsage != "D2")
                {
                    return new(
                        new(LocalVisaCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified key encryption key usage is not expected to decrypt KeyEncKey enctrypted key.{key.KeyUsage}");
                } 
            }

            if (string.IsNullOrEmpty(localVisa.Payload.Pvv))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No PIN Validation Value specified.");
            }

            Logger.Log(Constants.DeviceClass, "PinPadDev.VerifyPINLocalVISA()");

            var result = await Device.VerifyPINLocalVISA(
                new VerifyPINLocalVISARequest(
                    localVisa.Payload.Pan,
                    localVisa.Payload.Pvv,
                    localVisa.Payload.Key,
                    localVisa.Payload.KeyEncKey),
                cancel);

            Logger.Log(Constants.DeviceClass, $"PinPadDev.VerifyPINLocalVISA() -> {result.CompletionCode}, {result.ErrorCode}");

            LocalVisaCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                payload = new(
                     result.ErrorCode switch
                     {
                         VerifyPINLocalResult.ErrorCodeEnum.AccessDenied => LocalVisaCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                         VerifyPINLocalResult.ErrorCodeEnum.FormatNotSupported => LocalVisaCompletion.PayloadData.ErrorCodeEnum.FormatNotSupported,
                         VerifyPINLocalResult.ErrorCodeEnum.InvalidKeyLength => LocalVisaCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength,
                         VerifyPINLocalResult.ErrorCodeEnum.KeyNotFound => LocalVisaCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                         VerifyPINLocalResult.ErrorCodeEnum.KeyNoValue => LocalVisaCompletion.PayloadData.ErrorCodeEnum.KeyNoValue,
                         VerifyPINLocalResult.ErrorCodeEnum.NoPin => LocalVisaCompletion.PayloadData.ErrorCodeEnum.NoPin,
                         _ => null,
                     },
                     result.Verified);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IKeyManagementService KeyManagement { get => Provider.IsA<IKeyManagementService>(); }
    }
}
