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
            if (cryptoData.Payload.Data is null ||
                cryptoData.Payload.Data.Count == 0)
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                            $"No data specified to encrypt or decrypt.");
            }
            if (cryptoData.Payload.CryptoMethod is null)
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData, 
                                                            $"No algorith, modeOfUse or cryptoMethod specified.");
            }

            KeyDetail ivKeyDetail = null;
            if (!string.IsNullOrEmpty(cryptoData.Payload.IvKey))
            {
                // Check stored IV key attributes
                ivKeyDetail = Crypto.GetKeyDetail(cryptoData.Payload.IvKey);

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
                                                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                }
                if (ivKeyDetail.ModeOfUse != "D")
                {
                    return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                $"The key {ivKeyDetail.KeyName} doesn't support mode of use for IV decryption.",
                                                                CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation);
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

            if (!string.IsNullOrEmpty(cryptoData.Payload.ModeOfUse) &&
                cryptoData.Payload.ModeOfUse != "E" &&
                cryptoData.Payload.ModeOfUse != "D")
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                            $"Specified ModeOfUse property is incorrect. only E or D are valid.  {cryptoData.Payload.ModeOfUse}");
            }

            if (keyDetail.ModeOfUse != "E" &&
                keyDetail.ModeOfUse != "D" &&
                keyDetail.ModeOfUse != "B")
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                            $"Specified key doesn't have a right mode of use. {cryptoData.Payload.Key} {keyDetail.ModeOfUse}",
                                                            CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation);
            }

            if (!string.IsNullOrEmpty(cryptoData.Payload.ModeOfUse) &&
                keyDetail.ModeOfUse != "B")
            {
                return new CryptoDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                            $"Specified key doesn't have a mode of use 'B', however ModeOfUse property is set.  {cryptoData.Payload.Key} {cryptoData.Payload.ModeOfUse}",
                                                            CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation);
            }

            // Chcek the crypto capabilities, check must be done before loading keys. but just double checking here
            bool verifyCryptAttrib = false;
            if (Crypto.CryptoCapabilities.CryptoAttributes.ContainsKey("D0"))
            {
                if (Crypto.CryptoCapabilities.CryptoAttributes["D0"].ContainsKey(keyDetail.Algorithm))
                {
                    if (Crypto.CryptoCapabilities.CryptoAttributes["D0"][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse))
                    {
                        CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum cryptoMethod = cryptoData.Payload.CryptoMethod switch
                        {
                            CryptoDataCommand.PayloadData.CryptoMethodEnum.Cbc => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CBC,
                            CryptoDataCommand.PayloadData.CryptoMethodEnum.Cfb => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CFB,
                            CryptoDataCommand.PayloadData.CryptoMethodEnum.Ctr => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.CTR,
                            CryptoDataCommand.PayloadData.CryptoMethodEnum.Ecb => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.ECB,
                            CryptoDataCommand.PayloadData.CryptoMethodEnum.Ofb => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.OFB,
                            CryptoDataCommand.PayloadData.CryptoMethodEnum.Xts => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.XTS,
                            CryptoDataCommand.PayloadData.CryptoMethodEnum.RsaesOaep => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.RSAES_OAEP,
                            CryptoDataCommand.PayloadData.CryptoMethodEnum.RsaesPkcs1V15 => CryptoCapabilitiesClass.CryptoAttributesClass.CryptoMethodEnum.RSAES_PKCS1_V1_5,
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
            if (!string.IsNullOrEmpty(cryptoData.Payload.IvKey) ||
                cryptoData.Payload.Iv?.Count > 0)
            {
                if (!string.IsNullOrEmpty(cryptoData.Payload.IvKey))
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
                if (string.IsNullOrEmpty(cryptoData.Payload.IvKey) &&
                    cryptoData.Payload.Iv?.Count > 0)
                {
                    // ClearIV;
                    ivData = cryptoData.Payload.Iv;
                }
                else if (!string.IsNullOrEmpty(cryptoData.Payload.IvKey) &&
                         cryptoData.Payload.Iv?.Count > 0)
                {
                    // In this last mode, the data is encrypted, so we have to decrypt
                    // it then send it as a clear IV
                    Logger.Log(Constants.DeviceClass, "CryptoDev.Crypto()");

                    var decryptResult = await Device.Crypto(new CryptoCommandEvents(events), 
                                                            new CryptoDataRequest(CryptoDataRequest.CryptoModeEnum.Decrypt,
                                                                                  CryptoDataRequest.CryptoAlgorithmEnum.ECB,
                                                                                  cryptoData.Payload.IvKey,
                                                                                  Crypto.GetKeyDetail(cryptoData.Payload.IvKey).KeySlot,
                                                                                  cryptoData.Payload.Iv,
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
                    ivKeyName = cryptoData.Payload.IvKey;
                }
            }

            Logger.Log(Constants.DeviceClass, "CryptoDev.Crypto()");

            byte padding = (byte)(cryptoData.Payload.Padding is not null ? cryptoData.Payload.Padding : 0);

            CryptoDataRequest.CryptoModeEnum modeOfUse;
            if (string.IsNullOrEmpty(cryptoData.Payload.ModeOfUse))
            {
                modeOfUse = (keyDetail.ModeOfUse == "E") ? CryptoDataRequest.CryptoModeEnum.Encrypt : CryptoDataRequest.CryptoModeEnum.Decrypt;
            }
            else
            {
                modeOfUse = (cryptoData.Payload.ModeOfUse == "E") ? CryptoDataRequest.CryptoModeEnum.Encrypt : CryptoDataRequest.CryptoModeEnum.Decrypt;
            }

            var result = await Device.Crypto(new CryptoCommandEvents(events),
                                             new CryptoDataRequest(modeOfUse,
                                                                   cryptoData.Payload.CryptoMethod switch
                                                                   {
                                                                       CryptoDataCommand.PayloadData.CryptoMethodEnum.Cbc => CryptoDataRequest.CryptoAlgorithmEnum.CBC,
                                                                       CryptoDataCommand.PayloadData.CryptoMethodEnum.Cfb => CryptoDataRequest.CryptoAlgorithmEnum.CFB,
                                                                       CryptoDataCommand.PayloadData.CryptoMethodEnum.Ctr => CryptoDataRequest.CryptoAlgorithmEnum.CTR,
                                                                       CryptoDataCommand.PayloadData.CryptoMethodEnum.Ecb => CryptoDataRequest.CryptoAlgorithmEnum.ECB,
                                                                       CryptoDataCommand.PayloadData.CryptoMethodEnum.Ofb => CryptoDataRequest.CryptoAlgorithmEnum.OFB,
                                                                       CryptoDataCommand.PayloadData.CryptoMethodEnum.Xts => CryptoDataRequest.CryptoAlgorithmEnum.XTS,
                                                                       CryptoDataCommand.PayloadData.CryptoMethodEnum.RsaesOaep => CryptoDataRequest.CryptoAlgorithmEnum.RSAES_OAEP,
                                                                       _ => CryptoDataRequest.CryptoAlgorithmEnum.RSAES_OAEP,
                                                                   },
                                                                   keyDetail.KeyName,
                                                                   keyDetail.KeySlot,
                                                                   cryptoData.Payload.Data,
                                                                   padding,
                                                                   ivKeyName,
                                                                   ivKeyDetail is not null ? ivKeyDetail.KeySlot : -1,
                                                                   ivData),
                                             cancel);


            Logger.Log(Constants.DeviceClass, $"CryptoDev.Crypto() -> {result.CompletionCode}, {result.ErrorCode}");

            return new CryptoDataCompletion.PayloadData(result.CompletionCode,
                                                        result.ErrorDescription,
                                                        result.ErrorCode,
                                                        result.CryptoData);
        }
    }
}
