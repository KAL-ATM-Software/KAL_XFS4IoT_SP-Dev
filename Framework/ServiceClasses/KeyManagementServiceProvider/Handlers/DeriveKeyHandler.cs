/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
        private async Task<DeriveKeyCompletion.PayloadData> HandleDeriveKey(IDeriveKeyEvents events, DeriveKeyCommand deriveKey, CancellationToken cancel)
        {
            if (deriveKey.Payload.DerivationAlgorithm is null)
            {
                return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No DerivationAlgorithm specified.");
            }
            if (string.IsNullOrEmpty(deriveKey.Payload.KeyGenKey))
            {
                return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No key generation key specified.");
            }
            if (deriveKey.Payload.Padding is null)
            {
                return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No padding value specified.");
            }
            if (string.IsNullOrEmpty(deriveKey.Payload.InputData))
            {
                return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"No derive key data specified.");
            }

            if (KeyManagement.KeyManagementCapabilities.IDKey.HasFlag(KeyManagementCapabilitiesClass.IDKeyEnum.Import) &&
                string.IsNullOrEmpty(deriveKey.Payload.Ident))
            {
                return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"No identification data provided.",
                                                           DeriveKeyCompletion.PayloadData.ErrorCodeEnum.AccessDenied);

            }
            KeyDetail keyGenKeyDetail = KeyManagement.GetKeyDetail(deriveKey.Payload.KeyGenKey);
            if (keyGenKeyDetail is null)
            {
                return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                            $"Specified key generating key name is not found. {deriveKey.Payload.KeyGenKey}",
                                                            DeriveKeyCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
            }
            if (keyGenKeyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded &&
                keyGenKeyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Construct)
            {
                return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                            $"Specified key generating key is not loaded. {deriveKey.Payload.KeyGenKey}",
                                                            DeriveKeyCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
            }

            int ivKeySlot = -1;
            if (!string.IsNullOrEmpty(deriveKey.Payload.StartValueKey))
            {
                KeyDetail keyDetail = KeyManagement.GetKeyDetail(deriveKey.Payload.StartValueKey);
                if (keyDetail is null)
                {
                    return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Specified IV key name is not found. {deriveKey.Payload.StartValueKey}",
                                                               DeriveKeyCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
                }
                if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded &&
                    keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Construct)
                {
                    return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Specified IV key is not loaded. {deriveKey.Payload.StartValueKey}",
                                                               DeriveKeyCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
                }

                if (!(Regex.IsMatch(keyDetail.KeyUsage, "^D[0-1]$|^[0-9][0-9]$") &&
                      Regex.IsMatch(keyDetail.Algorithm, "^[ADT0-9]$") &&
                      Regex.IsMatch(keyDetail.ModeOfUse, "^[X0-9]$")))
                {
                    return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Specified key doesn't have a usage to derive.",
                                                               DeriveKeyCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                }

                ivKeySlot = keyDetail.KeySlot;

                // First to check capabilities of ECB decryption
                bool verifyIVAttrib = false;
                if (KeyManagement.CryptoCapabilities.CryptoAttributes.ContainsKey("D0"))
                {
                    if (KeyManagement.CryptoCapabilities.CryptoAttributes["D0"].ContainsKey("D"))
                    {
                        if (KeyManagement.CryptoCapabilities.CryptoAttributes["D0"]["D"].ContainsKey("D"))
                            verifyIVAttrib = KeyManagement.CryptoCapabilities.CryptoAttributes["D0"]["D"]["D"].CryptoMethods.HasFlag(Common.CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB);
                    }
                    else if (KeyManagement.CryptoCapabilities.CryptoAttributes["D0"].ContainsKey("T"))
                    {
                        if (KeyManagement.CryptoCapabilities.CryptoAttributes["D0"]["T"].ContainsKey("D"))
                            verifyIVAttrib = KeyManagement.CryptoCapabilities.CryptoAttributes["D0"]["T"]["D"].CryptoMethods.HasFlag(Common.CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB);
                    }
                }

                if (!verifyIVAttrib)
                {
                    return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"The crypto attribute doesn't support decrypt IV data with IV key.",
                                                               DeriveKeyCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                }
            }

            Logger.Log(Constants.DeviceClass, "KeyManagementDev.DeriveKey()");

            int keySlot = KeyManagement.FindKeySlot(deriveKey.Payload.Key);
            var result = await Device.DeriveKey(new DeriveKeyRequest(deriveKey.Payload.Key,
                                                                     keySlot,
                                                                     deriveKey.Payload.KeyGenKey,
                                                                     keyGenKeyDetail.KeySlot,
                                                                     (int)deriveKey.Payload.DerivationAlgorithm,
                                                                     deriveKey.Payload.StartValue is not null && deriveKey.Payload.StartValue.Length > 0 ? Convert.FromBase64String(deriveKey.Payload.StartValue).ToList() : null,
                                                                     deriveKey.Payload.StartValueKey,
                                                                     ivKeySlot,
                                                                     (byte)deriveKey.Payload.Padding,
                                                                     Convert.FromBase64String(deriveKey.Payload.InputData).ToList(),
                                                                     deriveKey.Payload.Ident is not null && deriveKey.Payload.Ident.Length > 0 ? Convert.FromBase64String(deriveKey.Payload.Ident).ToList() : null), 
                                                cancel);

            Logger.Log(Constants.DeviceClass, $"KeyManagementDev.DeriveKey() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                if (result.LoadedKeyDetail is null)
                {
                    return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError,
                                                               $"No key information provided from the device class.");
                }

                if (string.IsNullOrEmpty(result.LoadedKeyDetail.KeyUsage) ||
                    !Regex.IsMatch(result.LoadedKeyDetail.KeyUsage, KeyDetail.regxKeyUsage))
                {
                    return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError,
                                                               $"Invalid key usage specified by the device class. {result.LoadedKeyDetail?.KeyUsage}");
                }
                if (string.IsNullOrEmpty(result.LoadedKeyDetail.Algorithm) ||
                    !Regex.IsMatch(result.LoadedKeyDetail.Algorithm, KeyDetail.regxAlgorithm))
                {
                    return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError,
                                                               $"Invalid algorithm specified by the device class. {result.LoadedKeyDetail?.Algorithm}");
                }
                if (string.IsNullOrEmpty(result.LoadedKeyDetail.ModeOfUse) ||
                    !Regex.IsMatch(result.LoadedKeyDetail.ModeOfUse, KeyDetail.regxModeOfUse))
                {
                    return new DeriveKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InternalError,
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

            return new DeriveKeyCompletion.PayloadData(result.CompletionCode,
                                                       result.ErrorDescription,
                                                       result.ErrorCode);
        }
    }
}
