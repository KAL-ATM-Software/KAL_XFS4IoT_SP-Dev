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
    public partial class GenerateAuthenticationHandler
    {
        private async Task<GenerateAuthenticationCompletion.PayloadData> HandleGenerateAuthentication(IGenerateAuthenticationEvents events, GenerateAuthenticationCommand generateAuthentication, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(generateAuthentication.Payload.Key))
            {
                return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"No key name specified.");
            }

            if (string.IsNullOrEmpty(generateAuthentication.Payload.AuthenticationData))
            {
                return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"No authentication data specified.");
            }

            // Check key is stored and available
            KeyDetail keyDetail = Crypto.GetKeyDetail(generateAuthentication.Payload.Key);
            if (keyDetail is null)
            {
                return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                        $"Specified key name is not found. {generateAuthentication.Payload.Key}",
                                                                        GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
            }

            if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
            {
                return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                        $"Specified key name is not loaded. {generateAuthentication.Payload.Key}",
                                                                        GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
            }

            if (!Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$|^M[0-8]$"))
            {
                return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                        $"Specified key {generateAuthentication.Payload.Key} has no valid key usage for generating authentication data.");
            }

            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$") &&
                (generateAuthentication.Payload.AuthenticationAttribute is null ||
                (generateAuthentication.Payload.AuthenticationAttribute.CryptoMethod is null &&
                 generateAuthentication.Payload.AuthenticationAttribute.HashAlgorithm is null)))
            {
                return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                                        $"No hash algorith, or cryptoMethod specified.");
            }

            if (keyDetail.ModeOfUse != "C" &&
                keyDetail.ModeOfUse != "G" &&
                keyDetail.ModeOfUse != "S" &&
                keyDetail.ModeOfUse != "T")
            {
                return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, 
                                                                        $"Specified key {generateAuthentication.Payload.Key} has no mode of use for generating authentication data.");
            }

            KeyDetail ivKeyDetail = null;
            if (!string.IsNullOrEmpty(generateAuthentication.Payload.StartValueKey))
            {
                // Check stored IV key attributes
                ivKeyDetail = Crypto.GetKeyDetail(generateAuthentication.Payload.StartValueKey);

                if (ivKeyDetail is null)
                {
                    return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                            $"No IV key stored. {generateAuthentication.Payload.StartValueKey}",
                                                                            GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
                }

                if (ivKeyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
                {
                    return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                            $"Specified IV key is not loaded. {generateAuthentication.Payload.StartValueKey}",
                                                                            GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
                }

                if (ivKeyDetail.KeyUsage != "I0")
                {
                    return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"No IV key stored.",
                                                                GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                }

                if (ivKeyDetail.Algorithm != "D" &&
                    ivKeyDetail.Algorithm != "T")
                {
                    return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"The key {ivKeyDetail.KeyName} doesn't have a DES or TDES algorithm supported for decryption.",
                                                                GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported);
                }

                if (ivKeyDetail.ModeOfUse != "D")
                {
                    return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"The key {ivKeyDetail.KeyName} doesn't support mode of use for IV decryption.",
                                                                GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported);
                }
            }

            GenerateSignatureRequest.RSASignatureAlgorithmEnum? sigAlgorithm = null;
            // Chcek the crypto capabilities
            bool verifyCryptAttrib = false;
            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$"))
            {
                if ((keyDetail.ModeOfUse == "S" ||
                     keyDetail.ModeOfUse == "T") &&
                    (Crypto.CryptoCapabilities.AuthenticationAttributes[keyDetail.KeyUsage].ContainsKey(keyDetail.Algorithm)))
                {
                    if (Crypto.CryptoCapabilities.AuthenticationAttributes[keyDetail.KeyUsage][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse))
                    {
                        sigAlgorithm = generateAuthentication.Payload.AuthenticationAttribute.CryptoMethod switch
                        {
                            GenerateAuthenticationCommand.PayloadData.AuthenticationAttributeClass.CryptoMethodEnum.RsassaPkcs1V15 => GenerateSignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                            GenerateAuthenticationCommand.PayloadData.AuthenticationAttributeClass.CryptoMethodEnum.RsassaPss => GenerateSignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PSS,
                            _ => null
                        };
                        if (sigAlgorithm is not null)
                        {
                            return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                                    $"Specified crypto method is not supported. {generateAuthentication.Payload.AuthenticationAttribute.CryptoMethod}",
                                                                                    GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
                        }

                        CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum algorithmFlag = (GenerateSignatureRequest.RSASignatureAlgorithmEnum)sigAlgorithm switch
                        {
                            GenerateSignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5 => CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                            _ => CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PSS
                        };

                        verifyCryptAttrib = Crypto.CryptoCapabilities.AuthenticationAttributes[keyDetail.KeyUsage][keyDetail.Algorithm][keyDetail.ModeOfUse].CryptoMethods.HasFlag(algorithmFlag);
                    }
                }
            }
            else
            {
                if ((keyDetail.ModeOfUse == "C" ||
                    (keyDetail.ModeOfUse == "G") &&
                    Crypto.CryptoCapabilities.AuthenticationAttributes[keyDetail.KeyUsage].ContainsKey(keyDetail.Algorithm)))
                {
                    verifyCryptAttrib = (Crypto.CryptoCapabilities.AuthenticationAttributes[keyDetail.KeyUsage][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse));
                }
            }
            if (!verifyCryptAttrib)
            {
                return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                        $"The crypto attribute doesn't support specified RSA signature algorithm or unsupported mode of use for MAC.",
                                                                        GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
            }

            List<byte> ivData = null;
            string ivKeyName = string.Empty;
            if (!string.IsNullOrEmpty(generateAuthentication.Payload.StartValueKey) ||
                !string.IsNullOrEmpty(generateAuthentication.Payload.StartValue))
            {

                if (!string.IsNullOrEmpty(generateAuthentication.Payload.StartValueKey))
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
                        return new GenerateAuthenticationCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                                $"The crypto attribute doesn't support decrypt IV data with IV key.",
                                                                                GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                    }
                }

                ivData = (new byte[8]).Select(x => x = 0).ToList();

                // Need an IV
                if (string.IsNullOrEmpty(generateAuthentication.Payload.StartValueKey) &&
                    !string.IsNullOrEmpty(generateAuthentication.Payload.StartValue))
                {
                    // ClearIV;
                    ivData = new(Convert.FromBase64String(generateAuthentication.Payload.StartValue));
                }
                else if (!string.IsNullOrEmpty(generateAuthentication.Payload.StartValueKey) &&
                         !string.IsNullOrEmpty(generateAuthentication.Payload.StartValue))
                {
                    // In this last mode, the data is encrypted, so we have to decrypt
                    // it then send it as a clear IV
                    Logger.Log(Constants.DeviceClass, "CryptoDev.Crypto()");

                    var decryptResult = await Device.Crypto(null,
                                                            new CryptoDataRequest(CryptoDataRequest.CryptoModeEnum.Decrypt,
                                                                                  CryptoDataRequest.CryptoAlgorithmEnum.ECB,
                                                                                  generateAuthentication.Payload.StartValueKey,
                                                                                  Crypto.GetKeyDetail(generateAuthentication.Payload.StartValueKey).KeySlot,
                                                                                  new(Convert.FromBase64String(generateAuthentication.Payload.StartValue)),
                                                                                  0),
                                                            cancel);

                    Logger.Log(Constants.DeviceClass, $"CryptoDev.Crypto() -> {decryptResult.CompletionCode}, {decryptResult.ErrorCode}");

                    if (decryptResult.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                    {
                        return new GenerateAuthenticationCompletion.PayloadData(decryptResult.CompletionCode,
                                                                                decryptResult.ErrorDescription,
                                                                                decryptResult.ErrorCode switch
                                                                                {
                                                                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.AccessDenied => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                                                                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported,
                                                                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported,
                                                                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength,
                                                                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNotFound => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                                                                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNoValue => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue,
                                                                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported,
                                                                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.NoChipTransactionActive => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.NoChipTransactionActive,
                                                                                    _ => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation,


                                                                                });
                    }

                    ivData = decryptResult.CryptoData;
                }
                else
                {
                    ivKeyName = generateAuthentication.Payload.StartValueKey;
                }
            }

            byte padding = (byte)(generateAuthentication.Payload.Padding is not null ? generateAuthentication.Payload.Padding : 0);

            GenerateAuthenticationDataResult result;
            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$"))
            {
                Logger.Log(Constants.DeviceClass, "CryptoDev.GenerateSignature()");

                result = await Device.GenerateSignature(events,
                                                        new GenerateSignatureRequest(generateAuthentication.Payload.Key,
                                                                                     Crypto.GetKeyDetail(generateAuthentication.Payload.Key).KeySlot,
                                                                                     Convert.FromBase64String(generateAuthentication.Payload.AuthenticationData).ToList(),
                                                                                     padding,
                                                                                     (GenerateSignatureRequest.RSASignatureAlgorithmEnum)sigAlgorithm),
                                                        cancel);


                Logger.Log(Constants.DeviceClass, $"CryptoDev.GenerateSignature() -> {result.CompletionCode}, {result.ErrorCode}");
            }
            else
            {
                Logger.Log(Constants.DeviceClass, "CryptoDev.GenerateMAC()");

                result = await Device.GenerateMAC(events,
                                                  new GenerateMACRequest(keyDetail.KeyName,
                                                                         keyDetail.KeySlot,
                                                                         Convert.FromBase64String(generateAuthentication.Payload.AuthenticationData).ToList(),
                                                                         padding,
                                                                         ivKeyName,
                                                                         ivKeyDetail is not null ? ivKeyDetail.KeySlot : -1,
                                                                         ivData),
                                                  cancel);


                Logger.Log(Constants.DeviceClass, $"CryptoDev.GenerateMAC() -> {result.CompletionCode}, {result.ErrorCode}");
            }

            return new GenerateAuthenticationCompletion.PayloadData(result.CompletionCode,
                                                                    result.ErrorDescription,
                                                                    result.ErrorCode,
                                                                    result.AuthenticationData is null || result.AuthenticationData.Count == 0 ? null : Convert.ToBase64String(result.AuthenticationData.ToArray()));
        }
    }
}
