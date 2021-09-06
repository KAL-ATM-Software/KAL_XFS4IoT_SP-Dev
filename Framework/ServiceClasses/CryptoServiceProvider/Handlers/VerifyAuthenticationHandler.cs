/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Crypto.Commands;
using XFS4IoT.Crypto.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Crypto
{
    public partial class VerifyAuthenticationHandler
    {
        private async Task<VerifyAuthenticationCompletion.PayloadData> HandleVerifyAuthentication(IVerifyAuthenticationEvents events, VerifyAuthenticationCommand verifyAuthentication, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(verifyAuthentication.Payload.Key))
            {
                return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                                      $"No key name is specified.");
            }
            if (string.IsNullOrEmpty(verifyAuthentication.Payload.AuthenticationData) ||
                string.IsNullOrEmpty(verifyAuthentication.Payload.VerifyData))
            {
                return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                      $"No data specified to verify.");
            }

            // Check key is stored and available
            KeyDetail keyDetail = Crypto.GetKeyDetail(verifyAuthentication.Payload.Key);
            if (keyDetail is null)
            {
                return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, 
                                                                      $"Specified key name is not found. {verifyAuthentication.Payload.Key}", 
                                                                      VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
            }
            if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
            {
                return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                      $"Specified key name is not loaded. {verifyAuthentication.Payload.Key}",
                                                                      VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
            }

            if (!Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$|^M[0-8]$"))
            {
                return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                                      $"Specified key {verifyAuthentication.Payload.Key} has no valid key usage for verify authentication data.");
            }

            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$") &&
                (verifyAuthentication.Payload.VerifyAttributes is null ||
                (verifyAuthentication.Payload.VerifyAttributes.CryptoMethod is null &&
                 verifyAuthentication.Payload.VerifyAttributes.HashAlgorithm is null)))
            {
                return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, $"No hash algorith, or cryptoMethod specified.");
            }

            if (keyDetail.ModeOfUse != "V")
            {
                return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, 
                                                                      $"Specified key {verifyAuthentication.Payload.Key} has no mode of use for generating authentication data.");
            }

            KeyDetail ivKeyDetail = null;
            if (!string.IsNullOrEmpty(verifyAuthentication.Payload.StartValueKey))
            {
                // Check stored IV key attributes
                ivKeyDetail = Crypto.GetKeyDetail(verifyAuthentication.Payload.StartValueKey);

                if (ivKeyDetail is null)
                {
                    return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                            $"No IV key is found. {verifyAuthentication.Payload.StartValueKey}",
                                                                            VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
                }
                if (ivKeyDetail is null)
                {
                    return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                            $"No IV key loaded. {verifyAuthentication.Payload.StartValueKey}",
                                                                            VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
                }

                if (ivKeyDetail.KeyUsage != "I0")
                {
                    return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"No IV key stored.",
                                                                VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                }
                if (ivKeyDetail.Algorithm != "D" &&
                    ivKeyDetail.Algorithm != "T")
                {
                    return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"The key {ivKeyDetail.KeyName} doesn't have a DES or TDES algorithm supported for decryption.",
                                                                VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported);
                }

                if (ivKeyDetail.ModeOfUse != "D")
                {
                    return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"The key {ivKeyDetail.KeyName} doesn't support mode of use for IV decryption.",
                                                                VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported);
                }
            }

            // Chcek the crypto capabilities
            VerifySignatureRequest.RSASignatureAlgorithmEnum? sigAlgorithm = null;
            // Chcek the crypto capabilities
            bool verifyCryptAttrib = false;
            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$"))
            {
                if (Crypto.CryptoCapabilities.VerifyAttributes[keyDetail.KeyUsage].ContainsKey(keyDetail.Algorithm))
                {
                    if (Crypto.CryptoCapabilities.VerifyAttributes[keyDetail.KeyUsage][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse))
                    {
                        sigAlgorithm = verifyAuthentication.Payload.VerifyAttributes.CryptoMethod switch
                        {
                            VerifyAuthenticationCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.RsassaPkcs1V15 => VerifySignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                            VerifyAuthenticationCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.RsassaPss => VerifySignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PSS,
                            _ => null
                        };
                        if (sigAlgorithm is not null)
                        {
                            return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                                    $"Specified crypto method is not supported. {verifyAuthentication.Payload.VerifyAttributes.CryptoMethod}",
                                                                                    VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
                        }

                        CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum algorithmFlag = (GenerateSignatureRequest.RSASignatureAlgorithmEnum)sigAlgorithm switch
                        {
                            GenerateSignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5 => CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                            _ => CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PSS
                        };

                        verifyCryptAttrib = Crypto.CryptoCapabilities.VerifyAttributes[keyDetail.KeyUsage][keyDetail.Algorithm][keyDetail.ModeOfUse].CryptoMethods.HasFlag(algorithmFlag);
                    }
                }
            }
            else
            {
                if (Crypto.CryptoCapabilities.VerifyAttributes[keyDetail.KeyUsage].ContainsKey(keyDetail.Algorithm))
                {
                    verifyCryptAttrib = (Crypto.CryptoCapabilities.VerifyAttributes[keyDetail.KeyUsage][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse));
                }
            }
            if (!verifyCryptAttrib)
            {
                return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                      $"The verify attribute doesn't support specified RSA signature algorithm or unsupported mode of use for MAC.",
                                                                      VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
            }

            List<byte> ivData = null;
            string ivKeyName = string.Empty;
            if (!string.IsNullOrEmpty(verifyAuthentication.Payload.StartValueKey) ||
                !string.IsNullOrEmpty(verifyAuthentication.Payload.StartValue))
            {
                if (!string.IsNullOrEmpty(verifyAuthentication.Payload.StartValueKey))
                {
                    // First to check capabilities of ECB decryption
                    bool verifyIVAttrib = false;
                    if (Crypto.CryptoCapabilities.CryptoAttributes.ContainsKey("D0"))
                    {
                        if (Crypto.CryptoCapabilities.CryptoAttributes["D0"].ContainsKey("D"))
                        {
                            if (Crypto.CryptoCapabilities.CryptoAttributes["D0"]["D"].ContainsKey("D"))
                                verifyIVAttrib = Crypto.CryptoCapabilities.CryptoAttributes["D0"]["D"]["D"].CryptoMethods.HasFlag(Common.CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB);
                        }
                        else if (Crypto.CryptoCapabilities.CryptoAttributes["D0"].ContainsKey("T"))
                        {
                            if (Crypto.CryptoCapabilities.CryptoAttributes["D0"]["T"].ContainsKey("D"))
                                verifyIVAttrib = Crypto.CryptoCapabilities.CryptoAttributes["D0"]["T"]["D"].CryptoMethods.HasFlag(Common.CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB);
                        }
                    }

                    if (!verifyIVAttrib)
                    {
                        return new VerifyAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                               $"The crypto attribute doesn't support decrypt IV data with IV key.",
                                                                               VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                    }
                }

                ivData = (new byte[8]).Select(x => x = 0).ToList();

                // Need an IV
                if (string.IsNullOrEmpty(verifyAuthentication.Payload.StartValueKey) &&
                    !string.IsNullOrEmpty(verifyAuthentication.Payload.StartValue))
                {
                    // ClearIV;
                    ivData = new(Convert.FromBase64String(verifyAuthentication.Payload.StartValue));
                }
                else if (!string.IsNullOrEmpty(verifyAuthentication.Payload.StartValueKey) &&
                         !string.IsNullOrEmpty(verifyAuthentication.Payload.StartValue))
                {
                    // In this last mode, the data is encrypted, so we have to decrypt
                    // it then send it as a clear IV
                    Logger.Log(Constants.DeviceClass, "CryptoDev.Crypto()");

                    var decryptResult = await Device.Crypto(null,
                                                            new CryptoDataRequest(CryptoDataRequest.CryptoModeEnum.Decrypt,
                                                            CryptoDataRequest.CryptoAlgorithmEnum.ECB,
                                                            verifyAuthentication.Payload.StartValueKey,
                                                            Crypto.GetKeyDetail(verifyAuthentication.Payload.StartValueKey).KeySlot,
                                                            new(Convert.FromBase64String(verifyAuthentication.Payload.StartValue)),
                                                            0),
                                                     cancel);

                    Logger.Log(Constants.DeviceClass, $"CryptoDev.Crypto() -> {decryptResult.CompletionCode}, {decryptResult.ErrorCode}");

                    if (decryptResult.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                    {
                        return new VerifyAuthenticationCompletion.PayloadData(decryptResult.CompletionCode,
                                                                              decryptResult.ErrorDescription,
                                                                              decryptResult.ErrorCode switch
                                                                              {
                                                                                  CryptoDataCompletion.PayloadData.ErrorCodeEnum.AccessDenied => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                                                                                  CryptoDataCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported,
                                                                                  CryptoDataCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported,
                                                                                  CryptoDataCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength,
                                                                                  CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNotFound => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                                                                                  CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNoValue => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue,
                                                                                  CryptoDataCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported,
                                                                                  CryptoDataCompletion.PayloadData.ErrorCodeEnum.NoChipTransactionActive => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.NoChipTransactionActive,
                                                                                  _ => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation,
                                                                                });
                    }

                    ivData = decryptResult.CryptoData;
                }
                else
                {
                    ivKeyName = verifyAuthentication.Payload.StartValueKey;
                }
            }

            byte padding = (byte)(verifyAuthentication.Payload.Padding is not null ? verifyAuthentication.Payload.Padding : 0);

            VerifyAuthenticationDataResult result;
            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$"))
            {
                Logger.Log(Constants.DeviceClass, "CryptoDev.VerifySignature()");

                result = await Device.VerifySignature(events,
                                                        new VerifySignatureRequest(verifyAuthentication.Payload.Key,
                                                                                   Crypto.GetKeyDetail(verifyAuthentication.Payload.Key).KeySlot,
                                                                                   Convert.FromBase64String(verifyAuthentication.Payload.AuthenticationData).ToList(),
                                                                                   Convert.FromBase64String(verifyAuthentication.Payload.VerifyData).ToList(),
                                                                                   (VerifySignatureRequest.RSASignatureAlgorithmEnum)sigAlgorithm,
                                                                                   padding),
                                                        cancel);


                Logger.Log(Constants.DeviceClass, $"CryptoDev.VerifySignature() -> {result.CompletionCode}, {result.ErrorCode}");
            }
            else
            {
                Logger.Log(Constants.DeviceClass, "CryptoDev.VerifyMAC()");

                result = await Device.VerifyMAC(events,
                                                  new VerifyMACRequest(keyDetail.KeyName,
                                                                       keyDetail.KeySlot,
                                                                       Convert.FromBase64String(verifyAuthentication.Payload.AuthenticationData).ToList(),
                                                                       Convert.FromBase64String(verifyAuthentication.Payload.VerifyData).ToList(),
                                                                       padding,
                                                                       ivKeyName,
                                                                       ivKeyDetail is not null ? ivKeyDetail.KeySlot : -1,
                                                                       ivData),
                                                  cancel);


                Logger.Log(Constants.DeviceClass, $"CryptoDev.VerifyMAC() -> {result.CompletionCode}, {result.ErrorCode}");
            }

            return new VerifyAuthenticationCompletion.PayloadData(result.CompletionCode,
                                                                  result.ErrorDescription,
                                                                  result.ErrorCode);
        }
    }
}
