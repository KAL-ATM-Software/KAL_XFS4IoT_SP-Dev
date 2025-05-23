/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using XFS4IoT;
using XFS4IoTServer;

namespace XFS4IoTFramework.Crypto
{
    public partial class GenerateAuthenticationHandler
    {
        private async Task<CommandResult<GenerateAuthenticationCompletion.PayloadData>> HandleGenerateAuthentication(IGenerateAuthenticationEvents events, GenerateAuthenticationCommand generateAuthentication, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(generateAuthentication.Payload.Key))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No key name specified.");
            }

            if (generateAuthentication.Payload.Data is null ||
                generateAuthentication.Payload.Data.Count == 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No authentication data specified.");
            }

            // Check key is stored and available
            KeyDetail keyDetail = KeyManagement.GetKeyDetail(generateAuthentication.Payload.Key);
            if (keyDetail is null)
            {
                return new(
                    new(GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key name is not found. {generateAuthentication.Payload.Key}");
            }

            if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
            {
                return new(
                    new(GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key name is not loaded. {generateAuthentication.Payload.Key}");
            }

            if (!Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$|^M[0-8]$"))
            {
                return new (
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Specified key {generateAuthentication.Payload.Key} has no valid key usage for generating authentication data.");
            }

            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$") &&
                (generateAuthentication.Payload.CryptoMethod is null &&
                 generateAuthentication.Payload.HashAlgorithm is null))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No hash algorith, or cryptoMethod specified.");
            }

            if (keyDetail.ModeOfUse != "C" &&
                keyDetail.ModeOfUse != "G" &&
                keyDetail.ModeOfUse != "S" &&
                keyDetail.ModeOfUse != "T")
            {
                return new(
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key {generateAuthentication.Payload.Key} has no mode of use for generating authentication data.");
            }

            KeyDetail ivKeyDetail = null;
            if (!string.IsNullOrEmpty(generateAuthentication.Payload.Iv?.Key))
            {
                // Check stored IV key attributes
                ivKeyDetail = KeyManagement.GetKeyDetail(generateAuthentication.Payload.Iv.Key);

                if (ivKeyDetail is null)
                {
                    return new(
                        new(GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No IV key stored. {generateAuthentication.Payload.Iv.Key}");
                }

                if (ivKeyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
                {
                    return new(
                        new(GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode, 
                        $"Specified IV key is not loaded. {generateAuthentication.Payload.Iv.Key}");
                }

                if (ivKeyDetail.KeyUsage != "I0")
                {
                    return new(
                        new(GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No IV key stored.");
                }

                if (ivKeyDetail.Algorithm != "D" &&
                    ivKeyDetail.Algorithm != "T")
                {
                    return new(
                        new(GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"The key {ivKeyDetail.KeyName} doesn't have a DES or TDES algorithm supported for decryption.");
                }

                if (ivKeyDetail.ModeOfUse != "D")
                {
                    return new(
                        new(GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.ModeOfUseNotSupported),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"The key {ivKeyDetail.KeyName} doesn't support mode of use for IV decryption.");
                }
            }

            GenerateSignatureRequest.RSASignatureAlgorithmEnum? sigAlgorithm = null;
            // Chcek the crypto capabilities
            bool verifyCryptAttrib = false;
            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$"))
            {
                if ((keyDetail.ModeOfUse == "S" ||
                     keyDetail.ModeOfUse == "T") &&
                    (Common.CryptoCapabilities.AuthenticationAttributes[keyDetail.KeyUsage].ContainsKey(keyDetail.Algorithm)))
                {
                    if (Common.CryptoCapabilities.AuthenticationAttributes[keyDetail.KeyUsage][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse))
                    {
                        sigAlgorithm = generateAuthentication.Payload.CryptoMethod switch
                        {
                            GenerateAuthenticationCommand.PayloadData.CryptoMethodEnum.RsassaPkcs1V15 => GenerateSignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                            GenerateAuthenticationCommand.PayloadData.CryptoMethodEnum.RsassaPss => GenerateSignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PSS,
                            _ => null
                        };
                        if (sigAlgorithm is not null)
                        {
                            return new(
                                new(GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported),
                                MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                $"Specified crypto method is not supported. {generateAuthentication.Payload.CryptoMethod}");
                        }

                        CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum algorithmFlag = (GenerateSignatureRequest.RSASignatureAlgorithmEnum)sigAlgorithm switch
                        {
                            GenerateSignatureRequest.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5 => CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PKCS1_V1_5,
                            _ => CryptoCapabilitiesClass.VerifyAuthenticationAttributesClass.RSASignatureAlgorithmEnum.RSASSA_PSS
                        };

                        verifyCryptAttrib = Common.CryptoCapabilities.AuthenticationAttributes[keyDetail.KeyUsage][keyDetail.Algorithm][keyDetail.ModeOfUse].CryptoMethods.HasFlag(algorithmFlag);
                    }
                }
            }
            else
            {
                if ((keyDetail.ModeOfUse == "C" ||
                    (keyDetail.ModeOfUse == "G") &&
                    Common.CryptoCapabilities.AuthenticationAttributes[keyDetail.KeyUsage].ContainsKey(keyDetail.Algorithm)))
                {
                    verifyCryptAttrib = (Common.CryptoCapabilities.AuthenticationAttributes[keyDetail.KeyUsage][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse));
                }
            }
            if (!verifyCryptAttrib)
            {
                return new(
                    new(GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The crypto attribute doesn't support specified RSA signature algorithm or unsupported mode of use for MAC.");
            }

            List<byte> ivData = null;
            string ivKeyName = string.Empty;
            if (!string.IsNullOrEmpty(generateAuthentication.Payload.Iv?.Key) ||
                generateAuthentication.Payload.Iv?.Value?.Count > 0)
            {

                if (!string.IsNullOrEmpty(generateAuthentication.Payload.Iv.Key))
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
                            new(GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"The crypto attribute doesn't support decrypt IV data with IV key.");
                    }
                }

                ivData = (new byte[8]).Select(x => x = 0).ToList();

                // Need an IV
                if (string.IsNullOrEmpty(generateAuthentication.Payload.Iv?.Key) &&
                    generateAuthentication.Payload.Iv?.Value?.Count > 0)
                {
                    // ClearIV;
                    ivData = generateAuthentication.Payload.Iv.Value;
                }
                else if (!string.IsNullOrEmpty(generateAuthentication.Payload.Iv?.Key) &&
                         generateAuthentication.Payload.Iv?.Value?.Count > 0)
                {
                    // In this last mode, the data is encrypted, so we have to decrypt
                    // it then send it as a clear IV
                    Logger.Log(Constants.DeviceClass, "CryptoDev.Crypto()");

                    var decryptResult = await Device.Crypto(
                        null,
                        new CryptoDataRequest(
                            CryptoDataRequest.CryptoModeEnum.Decrypt,
                            CryptoDataRequest.CryptoAlgorithmEnum.ECB,
                            generateAuthentication.Payload.Iv.Key,
                            KeyManagement.GetKeyDetail(generateAuthentication.Payload.Iv.Key).KeySlot,
                            generateAuthentication.Payload.Iv.Value,
                            0),
                        cancel);

                    Logger.Log(Constants.DeviceClass, $"CryptoDev.Crypto() -> {decryptResult.CompletionCode}, {decryptResult.ErrorCode}");

                    if (decryptResult.CompletionCode != MessageHeader.CompletionCodeEnum.Success)
                    {
                        GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum? errorCode = null;
                        if (decryptResult.CompletionCode == MessageHeader.CompletionCodeEnum.CommandErrorCode)
                        {
                            errorCode = decryptResult.ErrorCode switch
                            {
                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.AccessDenied => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.AccessDenied,
                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported,
                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.InvalidKeyLength,
                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNotFound => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNotFound,
                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNoValue => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.KeyNoValue,
                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.ModeOfUseNotSupported => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.ModeOfUseNotSupported,
                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.NoChipTransactionActive => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.NoChipTransactionActive,
                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation => GenerateAuthenticationCompletion.PayloadData.ErrorCodeEnum.UseViolation,
                                _ => null,
                            };
                        }

                        return new(
                            errorCode is not null ? new(errorCode) : null,
                            decryptResult.CompletionCode,
                            decryptResult.ErrorDescription);
                    }

                    ivData = decryptResult.CryptoData;
                }
                else
                {
                    ivKeyName = generateAuthentication.Payload.Iv?.Key;
                }
            }

            byte padding = (byte)(generateAuthentication.Payload.Padding is not null ? generateAuthentication.Payload.Padding : 0);

            GenerateAuthenticationDataResult result;
            if (Regex.IsMatch(keyDetail.KeyUsage, "^S[0-2]$"))
            {
                Logger.Log(Constants.DeviceClass, "CryptoDev.GenerateSignature()");

                result = await Device.GenerateSignature(
                    new CryptoCommandEvents(events),
                    new GenerateSignatureRequest(
                        generateAuthentication.Payload.Key,
                        KeyManagement.GetKeyDetail(generateAuthentication.Payload.Key).KeySlot,
                        generateAuthentication.Payload.Data,
                        padding,
                        (GenerateSignatureRequest.RSASignatureAlgorithmEnum)sigAlgorithm),
                    cancel);


                Logger.Log(Constants.DeviceClass, $"CryptoDev.GenerateSignature() -> {result.CompletionCode}, {result.ErrorCode}");
            }
            else
            {
                Logger.Log(Constants.DeviceClass, "CryptoDev.GenerateMAC()");

                result = await Device.GenerateMAC(
                    new CryptoCommandEvents(events),
                    new GenerateMACRequest(
                        keyDetail.KeyName,
                        keyDetail.KeySlot,
                        generateAuthentication.Payload.Data,
                        padding,
                        ivKeyName,
                        ivKeyDetail is not null ? ivKeyDetail.KeySlot : -1,
                        ivData),
                    cancel);


                Logger.Log(Constants.DeviceClass, $"CryptoDev.GenerateMAC() -> {result.CompletionCode}, {result.ErrorCode}");
            }

            GenerateAuthenticationCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.AuthenticationData?.Count > 0)
            {
                payload = new(
                    result.ErrorCode,
                    result.AuthenticationData);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IKeyManagementService KeyManagement { get => Provider.IsA<IKeyManagementService>(); }
    }
}
