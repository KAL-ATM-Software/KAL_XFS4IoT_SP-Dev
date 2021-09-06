/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.Crypto.Commands;
using XFS4IoT.Crypto.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.KeyManagement;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Crypto
{
    public partial class CryptoDataHandler
    {
        private async Task<CryptoDataCompletion.PayloadData> HandleCryptoData(ICryptoDataEvents events, CryptoDataCommand cryptoData, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(cryptoData.Payload.Key))
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                            $"No key name is specified.");
            }
            if (string.IsNullOrEmpty(cryptoData.Payload.CryptData))
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                            $"No data specified to encrypt or decrypt.");
            }
            if (cryptoData.Payload.CryptoAttributes.CryptoMethod is null)
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                            $"No algorith, modeOfUse or cryptoMethod specified.");
            }

            KeyDetail ivKeyDetail = null;
            if (!string.IsNullOrEmpty(cryptoData.Payload.StartValueKey))
            {
                // Check stored IV key attributes
                ivKeyDetail = Crypto.GetKeyDetail(cryptoData.Payload.StartValueKey);

                if (ivKeyDetail is null)
                {
                    return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, 
                                                                $"No IV key stored.", 
                                                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
                }

                if (ivKeyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
                {
                    return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"No IV key loaded.",
                                                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
                }

                if (ivKeyDetail.KeyUsage != "I0")
                {
                    return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"No IV key stored.",
                                                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                }
                if (ivKeyDetail.Algorithm != "D" &&
                    ivKeyDetail.Algorithm != "T")
                {
                    return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"The key {ivKeyDetail.KeyName} doesn't have a DES or TDES algorithm supported for IV decryption.",
                                                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported);
                }
                if (ivKeyDetail.ModeOfUse != "D")
                {
                    return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"The key {ivKeyDetail.KeyName} doesn't support mode of use for IV decryption.",
                                                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported);
                }
            }

            // Check key is stored and available
            KeyDetail keyDetail = Crypto.GetKeyDetail(cryptoData.Payload.Key);
            if (keyDetail is null)
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode, 
                                                            $"Specified key name is not found. {cryptoData.Payload.Key}", 
                                                            CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
            }
            if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                            $"Specified key is not loaded. {cryptoData.Payload.Key}",
                                                            CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
            }

            // Chcek the crypto capabilities, check must be done before loading keys. but just double checking here
            bool verifyCryptAttrib = false;
            if (Crypto.CryptoCapabilities.CryptoAttributes.ContainsKey("D0"))
            {
                if (Crypto.CryptoCapabilities.CryptoAttributes["D0"].ContainsKey(keyDetail.Algorithm))
                {
                    if (Crypto.CryptoCapabilities.CryptoAttributes["D0"][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse))
                    {
                        CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum cryptoMethod = cryptoData.Payload.CryptoAttributes.CryptoMethod switch
                        {
                            CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Cbc => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CBC,
                            CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Cfb => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CFB,
                            CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Ctr => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CTR,
                            CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Ecb => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB,
                            CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Ofb => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.OFB,
                            CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Xts => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.XTS,
                            CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.RsaesOaep => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.RSAES_OAEP,
                            CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.RsaesPkcs1V15 => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.RSAES_PKCS1_V1_5,
                            _ => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.NotSupported
                        };
                        if (cryptoMethod != CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.NotSupported)
                            verifyCryptAttrib = Crypto.CryptoCapabilities.CryptoAttributes["D0"][keyDetail.Algorithm][keyDetail.ModeOfUse].CryptoMethods.HasFlag(cryptoMethod);
                    }
                }
            }
            if (!verifyCryptAttrib)
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                            $"The crypto attribute doesn't support specified RSA signature algorithm or unsupported mode of use for MAC.",
                                                            CryptoDataCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
            }

            List<byte> ivData = null;
            string ivKeyName = string.Empty;
            if (!string.IsNullOrEmpty(cryptoData.Payload.StartValueKey) ||
                !string.IsNullOrEmpty(cryptoData.Payload.StartValue))
            {
                if (!string.IsNullOrEmpty(cryptoData.Payload.StartValueKey))
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
                        return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                    $"The crypto attribute doesn't support decrypt IV data with IV key.",
                                                                    CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                    }
                }

                ivData = (new byte[8]).Select(x => x = 0).ToList();

                // Need an IV
                if (string.IsNullOrEmpty(cryptoData.Payload.StartValueKey) &&
                    !string.IsNullOrEmpty(cryptoData.Payload.StartValue))
                {
                    // ClearIV;
                    ivData = new(Convert.FromBase64String(cryptoData.Payload.StartValue));
                }
                else if (!string.IsNullOrEmpty(cryptoData.Payload.StartValueKey) &&
                         !string.IsNullOrEmpty(cryptoData.Payload.StartValue))
                {
                    // In this last mode, the data is encrypted, so we have to decrypt
                    // it then send it as a clear IV
                    Logger.Log(Constants.DeviceClass, "CryptoDev.Crypto()");

                    var decryptResult = await Device.Crypto(events, 
                                                     new CryptoDataRequest(CryptoDataRequest.CryptoModeEnum.Decrypt,
                                                                           CryptoDataRequest.CryptoAlgorithmEnum.ECB,
                                                                           cryptoData.Payload.StartValueKey,
                                                                           Crypto.GetKeyDetail(cryptoData.Payload.StartValueKey).KeySlot,
                                                                           new(Convert.FromBase64String(cryptoData.Payload.StartValue)),
                                                                           0),
                                                     cancel);

                    Logger.Log(Constants.DeviceClass, $"CryptoDev.Crypto() -> {decryptResult.CompletionCode}, {decryptResult.ErrorCode}");

                    if (decryptResult.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
                    {
                        return new CryptoDataCompletion.PayloadData(decryptResult.CompletionCode,
                                                                    decryptResult.ErrorDescription,
                                                                    decryptResult.ErrorCode);
                    }

                    ivData = decryptResult.CryptoData;
                }
                else
                {
                    ivKeyName = cryptoData.Payload.StartValueKey;
                }
            }

            Logger.Log(Constants.DeviceClass, "CryptoDev.Crypto()");

            byte padding = (byte)(cryptoData.Payload.Padding is not null ? cryptoData.Payload.Padding : 0);

            var result = await Device.Crypto(events,
                                             new CryptoDataRequest(keyDetail.ModeOfUse == "E" ? CryptoDataRequest.CryptoModeEnum.Encrypt : CryptoDataRequest.CryptoModeEnum.Decrypt,
                                                                   cryptoData.Payload.CryptoAttributes.CryptoMethod switch
                                                                   {
                                                                       CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Cbc => CryptoDataRequest.CryptoAlgorithmEnum.CBC,
                                                                       CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Cfb => CryptoDataRequest.CryptoAlgorithmEnum.CFB,
                                                                       CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Ctr => CryptoDataRequest.CryptoAlgorithmEnum.CTR,
                                                                       CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Ecb => CryptoDataRequest.CryptoAlgorithmEnum.ECB,
                                                                       CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Ofb => CryptoDataRequest.CryptoAlgorithmEnum.OFB,
                                                                       CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.Xts => CryptoDataRequest.CryptoAlgorithmEnum.XTS,
                                                                       CryptoDataCommand.PayloadData.CryptoAttributesClass.CryptoMethodEnum.RsaesOaep => CryptoDataRequest.CryptoAlgorithmEnum.RSAES_OAEP,
                                                                       _ => CryptoDataRequest.CryptoAlgorithmEnum.RSAES_OAEP,
                                                                   },
                                                                   keyDetail.KeyName,
                                                                   keyDetail.KeySlot,
                                                                   Convert.FromBase64String(cryptoData.Payload.CryptData).ToList(),
                                                                   padding,
                                                                   ivKeyName,
                                                                   ivKeyDetail is not null ? ivKeyDetail.KeySlot : -1,
                                                                   ivData),
                                             cancel);


            Logger.Log(Constants.DeviceClass, $"CryptoDev.Crypto() -> {result.CompletionCode}, {result.ErrorCode}");

            return new CryptoDataCompletion.PayloadData(result.CompletionCode,
                                                        result.ErrorDescription,
                                                        result.ErrorCode,
                                                        result.CryptoData is null || result.CryptoData.Count == 0? null : Convert.ToBase64String(result.CryptoData.ToArray()));
        }
    }
}
