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
    public partial class LocalDESHandler
    {
        private async Task<CommandResult<LocalDESCompletion.PayloadData>> HandleLocalDES(ILocalDESEvents events, LocalDESCommand localDES, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(localDES.Payload.ValidationData))
            {
                return new(MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No validation data specified.");
            }

            if (string.IsNullOrEmpty(localDES.Payload.Key))
            {
                return new(MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No key name specified to verify PIN locally.");
            }

            KeyDetail key = KeyManagement.GetKeyDetail(localDES.Payload.Key);
            if (key is null)
            {
                return new(
                    new(LocalDESCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key is not loaded.");
            }

            if (key.KeyUsage != "V0")
            {
                return new(
                    new(LocalDESCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key usage is not expected.{key.KeyUsage}");
            }

            if (!string.IsNullOrEmpty(localDES.Payload.KeyEncKey))
            {
                KeyDetail keyEncKey = KeyManagement.GetKeyDetail(localDES.Payload.KeyEncKey);
                if (keyEncKey is null)
                {
                    return new(
                        new(LocalDESCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified key encryption key is not loaded.");
                }

                if (keyEncKey.KeyUsage != "K0" &&
                    keyEncKey.KeyUsage != "K1" &&
                    keyEncKey.KeyUsage != "K2" &&
                    keyEncKey.KeyUsage != "K3")
                {
                    return new(
                        new(LocalDESCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified key encryption key usage is not expected.{keyEncKey.KeyUsage}");
                }
            }

            if (string.IsNullOrEmpty(localDES.Payload.DecTable))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No ASCII decimalization table specified.");
            }
            else
            {
                if (localDES.Payload.DecTable.Length != 16)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Decimal table must be 16 digits. {localDES.Payload.DecTable.Length}");
                }

                foreach (char c in localDES.Payload.Offset)
                {
                    if (!Char.IsDigit(c))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Offset data should be number. {localDES.Payload.Offset}");
                    }
                }
            }

            if (!string.IsNullOrEmpty(localDES.Payload.Offset))
            {
                foreach (char c in localDES.Payload.Offset)
                {
                    if (!Uri.IsHexDigit(c))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Offset data should be in hexstring. {localDES.Payload.Offset}");
                    }
                }
            }

            if (localDES.Payload.Padding is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No padding data specified.");
            }

            if (localDES.Payload.Padding.Length != 1 &&
                localDES.Payload.Padding.Length != 2)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Invalid length of padding. it should be length 1 or 2. {localDES.Payload.Padding.Length}");
            }

            byte padding = Convert.ToByte(localDES.Payload.Padding, 16);
            if (padding > 0xf)
            {
                if (padding >= '0' && padding <= '9')
                    padding -= (byte)'0';
                else if (padding >= 'A' && padding <= 'F')
                {
                    padding -= (byte)'A';
                    padding += 10;
                }
                else if (padding >= 'a' && padding <= 'f')
                {
                    padding -= (byte)'a';
                    padding += 10;
                }
                else
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Invalid value of padding. {localDES.Payload.Padding}");
                }
            }

            if (localDES.Payload.MaxPIN is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No max PIN specified.");
            }
            if (localDES.Payload.NoLeadingZero is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No NoLeadingZero specified.");
            }
            if (localDES.Payload.ValDigits is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No ValDigits specified.");
            }
            else
            {
                if (localDES.Payload.ValDigits > 16)
                {
                    return new (
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"ValDigits must be under 16 digits.");
                }
            }

            Logger.Log(Constants.DeviceClass, "PinPadDev.VerifyPINLocalDES()");

            var result = await Device.VerifyPINLocalDES(
                new VerifyPINLocalDESRequest(
                    localDES.Payload.ValidationData,
                    localDES.Payload.Offset,
                    padding,
                    (int)localDES.Payload.MaxPIN,
                    (int)localDES.Payload.ValDigits,
                    localDES.Payload.NoLeadingZero is null ? 
                    false : 
                    (bool)localDES.Payload.NoLeadingZero,
                    localDES.Payload.Key,
                    localDES.Payload.KeyEncKey,
                    localDES.Payload.DecTable),
                cancel);

            Logger.Log(Constants.DeviceClass, $"PinPadDev.VerifyPINLocalDES() -> {result.CompletionCode}, {result.ErrorCode}");

            LocalDESCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                payload = new(
                    result.ErrorCode switch
                    {
                        VerifyPINLocalResult.ErrorCodeEnum.AccessDenied => LocalDESCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                        VerifyPINLocalResult.ErrorCodeEnum.FormatNotSupported => LocalDESCompletion.PayloadData.ErrorCodeEnum.FormatNotSupported,
                        VerifyPINLocalResult.ErrorCodeEnum.InvalidKeyLength => LocalDESCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength,
                        VerifyPINLocalResult.ErrorCodeEnum.KeyNotFound => LocalDESCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                        VerifyPINLocalResult.ErrorCodeEnum.KeyNoValue => LocalDESCompletion.PayloadData.ErrorCodeEnum.KeyNoValue,
                        VerifyPINLocalResult.ErrorCodeEnum.NoPin => LocalDESCompletion.PayloadData.ErrorCodeEnum.NoPin,
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
