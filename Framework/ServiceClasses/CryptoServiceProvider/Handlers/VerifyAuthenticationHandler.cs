/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTServer;
using XFS4IoT;

namespace XFS4IoTFramework.Crypto
{
    public partial class VerifyAuthenticationHandler
    {
        private async Task<CommandResult<VerifyAuthenticationCompletion.PayloadData>> HandleVerifyAuthentication(IVerifyAuthenticationEvents events, VerifyAuthenticationCommand verifyAuthentication, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(verifyAuthentication.Payload.Key))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    $"No key name is specified.");
            }
            if ((verifyAuthentication.Payload.Data is null ||
                 verifyAuthentication.Payload.Data.Count == 0) ||
                (verifyAuthentication.Payload.VerifyData is null ||
                 verifyAuthentication.Payload.VerifyData.Count == 0))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No data specified to verify.");
            }

            // Check key is stored and available
            KeyDetail keyDetail = KeyManagement.GetKeyDetail(verifyAuthentication.Payload.Key);
            if (keyDetail is null)
            {
                return new(
                    new(VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key name is not found. {verifyAuthentication.Payload.Key}");
            }
            if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
            {
                return new(
                    new(VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key name is not loaded. {verifyAuthentication.Payload.Key}");
            }

            if (!Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$|^M[0-8]$"))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Specified key {verifyAuthentication.Payload.Key} has no valid key usage for verify authentication data.");
            }

            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$") &&
                (verifyAuthentication.Payload.CryptoMethod is null &&
                 verifyAuthentication.Payload.HashAlgorithm is null))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData, 
                    $"No hash algorith, or cryptoMethod specified.");
            }

            if (keyDetail.ModeOfUse != "V")
            {
                return new(
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key {verifyAuthentication.Payload.Key} has no mode of use for generating authentication data.");
            }

            KeyDetail ivKeyDetail = null;
            if (!string.IsNullOrEmpty(verifyAuthentication.Payload.Iv?.Key))
            {
                // Check stored IV key attributes
                ivKeyDetail = KeyManagement.GetKeyDetail(verifyAuthentication.Payload.Iv.Key);

                if (ivKeyDetail is null)
                {
                    return new(
                        new(VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No IV key is found. {verifyAuthentication.Payload.Iv.Key}");
                }
                if (ivKeyDetail is null)
                {
                    return new(
                        new(VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No IV key loaded. {verifyAuthentication.Payload.Iv.Key}");
                }

                if (ivKeyDetail.KeyUsage != "I0")
                {
                    return new(
                        new(VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No IV key stored.");
                }
                if (ivKeyDetail.Algorithm != "D" &&
                    ivKeyDetail.Algorithm != "T")
                {
                    return new(
                        new(VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"The key {ivKeyDetail.KeyName} doesn't have a DES or TDES algorithm supported for decryption.");
                }

                if (ivKeyDetail.ModeOfUse != "D")
                {
                    return new(
                        new(VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.ModeOfUseNotSupported),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"The key {ivKeyDetail.KeyName} doesn't support mode of use for IV decryption.");
                }
            }

            // Chcek the crypto capabilities
            VerifySignatureRequest.RSASignatureAlgorithmEnum? sigAlgorithm = null;
            // Chcek the crypto capabilities
            bool verifyCryptAttrib = false;
            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$"))
            {
                if (Common.CryptoCapabilities.VerifyAttributes[keyDetail.KeyUsage].ContainsKey(keyDetail.Algorithm))
                {
                    if (Common.CryptoCapabilities.VerifyAttributes[keyDetail.KeyUsage][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse))
                    {
                        sigAlgorithm = verifyAuthentication.Payload.CryptoMethod switch
                        {
                            VerifyAuthenticationCommand.PayloadData.CryptoMethodEnum.RsassaPkcs1V15 => VerifySignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                            VerifyAuthenticationCommand.PayloadData.CryptoMethodEnum.RsassaPss => VerifySignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PSS,
                            _ => null
                        };
                        if (sigAlgorithm is not null)
                        {
                            return new(
                                new(VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported),
                                MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                $"Specified crypto method is not supported. {verifyAuthentication.Payload.CryptoMethod}");
                        }

                        CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum algorithmFlag = (GenerateSignatureRequest.RSASignatureAlgorithmEnum)sigAlgorithm switch
                        {
                            GenerateSignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5 => CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                            _ => CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PSS
                        };

                        verifyCryptAttrib = Common.CryptoCapabilities.VerifyAttributes[keyDetail.KeyUsage][keyDetail.Algorithm][keyDetail.ModeOfUse].CryptoMethods.HasFlag(algorithmFlag);
                    }
                }
            }
            else
            {
                if (Common.CryptoCapabilities.VerifyAttributes[keyDetail.KeyUsage].ContainsKey(keyDetail.Algorithm))
                {
                    verifyCryptAttrib = (Common.CryptoCapabilities.VerifyAttributes[keyDetail.KeyUsage][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse));
                }
            }
            if (!verifyCryptAttrib)
            {
                return new(
                    new(VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The verify attribute doesn't support specified RSA signature algorithm or unsupported mode of use for MAC.");
            }

            List<byte> ivData = null;
            string ivKeyName = string.Empty;
            if (!string.IsNullOrEmpty(verifyAuthentication.Payload.Iv?.Key) ||
                verifyAuthentication.Payload.Iv?.Value?.Count > 0)
            {
                if (!string.IsNullOrEmpty(verifyAuthentication.Payload.Iv?.Key))
                {
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
                            new(VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"The crypto attribute doesn't support decrypt IV data with IV key.");
                    }
                }

                ivData = (new byte[8]).Select(x => x = 0).ToList();

                // Need an IV
                if (string.IsNullOrEmpty(verifyAuthentication.Payload.Iv?.Key) &&
                    verifyAuthentication.Payload.Iv?.Value?.Count > 0)
                {
                    // ClearIV;
                    ivData = verifyAuthentication.Payload.Iv.Value;
                }
                else if (!string.IsNullOrEmpty(verifyAuthentication.Payload.Iv?.Key) &&
                         verifyAuthentication.Payload.Iv?.Value?.Count > 0)
                {
                    // In this last mode, the data is encrypted, so we have to decrypt
                    // it then send it as a clear IV
                    Logger.Log(Constants.DeviceClass, "CryptoDev.Crypto()");

                    var decryptResult = await Device.Crypto(null,
                                                            new CryptoDataRequest(CryptoDataRequest.CryptoModeEnum.Decrypt,
                                                            CryptoDataRequest.CryptoAlgorithmEnum.ECB,
                                                            verifyAuthentication.Payload.Iv.Key,
                                                            KeyManagement.GetKeyDetail(verifyAuthentication.Payload.Iv?.Key).KeySlot,
                                                            verifyAuthentication.Payload.Iv.Value,
                                                            0),
                                                     cancel);

                    Logger.Log(Constants.DeviceClass, $"CryptoDev.Crypto() -> {decryptResult.CompletionCode}, {decryptResult.ErrorCode}");

                    if (decryptResult.CompletionCode != MessageHeader.CompletionCodeEnum.Success)
                    {
                        return new(
                            new(
                                decryptResult.ErrorCode switch
                                {
                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.AccessDenied => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported,
                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength,
                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNotFound => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNoValue => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue,
                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.ModeOfUseNotSupported => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.ModeOfUseNotSupported,
                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.NoChipTransactionActive => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.NoChipTransactionActive,
                                    _ => VerifyAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation,
                                }),
                            decryptResult.CompletionCode,
                            decryptResult.ErrorDescription);
                    }

                    ivData = decryptResult.CryptoData;
                }
                else
                {
                    ivKeyName = verifyAuthentication.Payload.Iv?.Key;
                }
            }

            byte padding = (byte)(verifyAuthentication.Payload.Padding is not null ? verifyAuthentication.Payload.Padding : 0);

            VerifyAuthenticationDataResult result;
            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$"))
            {
                Logger.Log(Constants.DeviceClass, "CryptoDev.VerifySignature()");

                result = await Device.VerifySignature(new CryptoCommandEvents(events),
                                                      new VerifySignatureRequest(verifyAuthentication.Payload.Key,
                                                                                 KeyManagement.GetKeyDetail(verifyAuthentication.Payload.Key).KeySlot,
                                                                                 verifyAuthentication.Payload.Data,
                                                                                 verifyAuthentication.Payload.VerifyData,
                                                                                 (VerifySignatureRequest.RSASignatureAlgorithmEnum)sigAlgorithm,
                                                                                  padding),
                                                      cancel);


                Logger.Log(Constants.DeviceClass, $"CryptoDev.VerifySignature() -> {result.CompletionCode}, {result.ErrorCode}");
            }
            else
            {
                Logger.Log(Constants.DeviceClass, "CryptoDev.VerifyMAC()");

                result = await Device.VerifyMAC(new CryptoCommandEvents(events),
                                                new VerifyMACRequest(keyDetail.KeyName,
                                                                     keyDetail.KeySlot,
                                                                     verifyAuthentication.Payload.Data,
                                                                     verifyAuthentication.Payload.VerifyData,
                                                                     padding,
                                                                     ivKeyName,
                                                                     ivKeyDetail is not null ? ivKeyDetail.KeySlot : -1,
                                                                     ivData),
                                                cancel);


                Logger.Log(Constants.DeviceClass, $"CryptoDev.VerifyMAC() -> {result.CompletionCode}, {result.ErrorCode}");
            }

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IKeyManagementService KeyManagement { get => Provider.IsA<IKeyManagementService>(); }
    }
}
