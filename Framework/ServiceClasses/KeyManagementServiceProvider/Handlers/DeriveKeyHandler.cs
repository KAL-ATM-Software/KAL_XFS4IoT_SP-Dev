/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class DeriveKeyHandler
    {
        private async Task<CommandResult<DeriveKeyCompletion.PayloadData>> HandleDeriveKey(IDeriveKeyEvents events, DeriveKeyCommand deriveKey, CancellationToken cancel)
        {
            if (deriveKey.Payload.DerivationAlgorithm is null)
            {
                return new(MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No DerivationAlgorithm specified.");
            }
            if (string.IsNullOrEmpty(deriveKey.Payload.KeyGenKey))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No key generation key specified.");
            }
            if (deriveKey.Payload.Padding is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No padding value specified.");
            }
            if (deriveKey.Payload.InputData is null ||
                deriveKey.Payload.InputData.Count == 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No derive key data specified.");
            }

            KeyDetail keyGenKeyDetail = KeyManagement.GetKeyDetail(deriveKey.Payload.KeyGenKey);
            if (keyGenKeyDetail is null)
            {
                return new(
                    new(DeriveKeyCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key generating key name is not found. {deriveKey.Payload.KeyGenKey}");
            }
            if (keyGenKeyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded &&
                keyGenKeyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Construct)
            {
                return new(
                    new(DeriveKeyCompletion.PayloadData.ErrorCodeEnum.KeyNoValue),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key generating key is not loaded. {deriveKey.Payload.KeyGenKey}");
            }

            int ivKeySlot = -1;
            if (!string.IsNullOrEmpty(deriveKey.Payload.Iv.Key))
            {
                // Verify loaded key specified to use for decrypt the Iv.Value
                // This specifies the name of a key(usage 'K0') used to decrypt the Iv.Value
                // This is only used when the Iv.Key usage is 'D0' and cryptoMethod is either CBC or CFB.
                KeyDetail keyDetail = KeyManagement.GetKeyDetail(deriveKey.Payload.Iv.Key);
                if (keyDetail is null)
                {
                    return new(
                        new(DeriveKeyCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified IV key name is not found. {deriveKey.Payload.Iv.Key}");
                }
                if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded &&
                    keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Construct)
                {
                    return new(
                        new(DeriveKeyCompletion.PayloadData.ErrorCodeEnum.KeyNoValue),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified IV key is not loaded. {deriveKey.Payload.Iv.Key}");
                }

                if (!(Regex.IsMatch(keyDetail.KeyUsage, "^D[0-1]$|^[0-9][0-9]$") &&
                      Regex.IsMatch(keyDetail.Algorithm, "^[ADT0-9]$") &&
                      Regex.IsMatch(keyDetail.ModeOfUse, "^[X0-9]$")))
                {
                    return new(
                        new(DeriveKeyCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified key doesn't have a usage to derive.");
                }

                ivKeySlot = keyDetail.KeySlot;

                // First to check capabilities of ECB decryption
                bool verifyIVAttrib = false;
                if (Common.CryptoCapabilities.CryptoAttributes.ContainsKey("D0"))
                {
                    if (Common.CryptoCapabilities.CryptoAttributes["D0"].ContainsKey("D"))
                    {
                        if (Common.CryptoCapabilities.CryptoAttributes["D0"]["D"].ContainsKey("D"))
                            verifyIVAttrib = Common.CryptoCapabilities.CryptoAttributes["D0"]["D"]["D"].CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB);
                    }
                    else if (Common.CryptoCapabilities.CryptoAttributes["D0"].ContainsKey("T"))
                    {
                        if (Common.CryptoCapabilities.CryptoAttributes["D0"]["T"].ContainsKey("D"))
                            verifyIVAttrib = Common.CryptoCapabilities.CryptoAttributes["D0"]["T"]["D"].CryptoMethods.HasFlag(CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB);
                    }
                }

                if (!verifyIVAttrib)
                {
                    return new(
                        new(DeriveKeyCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"The crypto attribute doesn't support decrypt IV data with IV key.");
                }
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.DeriveKey()");

            int keySlot = KeyManagement.FindKeySlot(deriveKey.Payload.Key);
            var result = await Device.DeriveKey(new DeriveKeyRequest(deriveKey.Payload.Key,
                                                                     keySlot,
                                                                     deriveKey.Payload.KeyGenKey,
                                                                     keyGenKeyDetail.KeySlot,
                                                                     (int)deriveKey.Payload.DerivationAlgorithm,
                                                                     deriveKey.Payload.Iv.Value,
                                                                     deriveKey.Payload.Iv.Key,
                                                                     ivKeySlot,
                                                                     (byte)deriveKey.Payload.Padding,
                                                                     deriveKey.Payload.InputData), 
                                                cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.DeriveKey() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                if (result.LoadedKeyDetail is null)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InternalError,
                        $"No key information provided from the device class.");
                }

                if (string.IsNullOrEmpty(result.LoadedKeyDetail.KeyUsage) ||
                    !Regex.IsMatch(result.LoadedKeyDetail.KeyUsage, KeyDetail.regxKeyUsage))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InternalError,
                        $"Invalid key usage specified by the device class. {result.LoadedKeyDetail?.KeyUsage}");
                }
                if (string.IsNullOrEmpty(result.LoadedKeyDetail.Algorithm) ||
                    !Regex.IsMatch(result.LoadedKeyDetail.Algorithm, KeyDetail.regxAlgorithm))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InternalError,
                        $"Invalid algorithm specified by the device class. {result.LoadedKeyDetail?.Algorithm}");
                }
                if (string.IsNullOrEmpty(result.LoadedKeyDetail.ModeOfUse) ||
                    !Regex.IsMatch(result.LoadedKeyDetail.ModeOfUse, KeyDetail.regxModeOfUse))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InternalError,
                        $"Invalid mode specified by the device class. {result.LoadedKeyDetail?.ModeOfUse}");
                }

                // Successfully loaded and add key information managing internally
                KeyManagement.AddKey(deriveKey.Payload.Key,
                                     result.UpdatedKeySlot is null ? keySlot : (int)result.UpdatedKeySlot,
                                     result.LoadedKeyDetail.KeyUsage,
                                     result.LoadedKeyDetail.Algorithm,
                                     result.LoadedKeyDetail.ModeOfUse,
                                     result.LoadedKeyDetail.KeyLength,
                                     KeyDetail.KeyStatusEnum.Loaded,
                                     false,
                                     null,
                                     result.LoadedKeyDetail.KeyVersionNumber,
                                     result.LoadedKeyDetail.Exportability,
                                     result.LoadedKeyDetail.OptionalKeyBlockHeader,
                                     result.LoadedKeyDetail.Generation,
                                     result.LoadedKeyDetail.ActivatingDate,
                                     result.LoadedKeyDetail.ExpiryDate,
                                     result.LoadedKeyDetail.Version);
            }

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
