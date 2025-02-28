/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using XFS4IoTServer;
using XFS4IoT;

namespace XFS4IoTFramework.Crypto
{
    public partial class CryptoDataHandler
    {
        private async Task<CommandResult<CryptoDataCompletion.PayloadData>> HandleCryptoData(ICryptoDataEvents events, CryptoDataCommand cryptoData, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(cryptoData.Payload.Key))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No key name is specified.");
            }
            if (cryptoData.Payload.Data is null ||
                cryptoData.Payload.Data.Count == 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No data specified to encrypt or decrypt.");
            }
            if (cryptoData.Payload.CryptoMethod is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No algorith, modeOfUse or cryptoMethod specified.");
            }

            KeyDetail ivKeyDetail = null;
            if (!string.IsNullOrEmpty(cryptoData.Payload.Iv?.Key))
            {
                // Check stored IV key attributes
                ivKeyDetail = KeyManagement.GetKeyDetail(cryptoData.Payload.Iv.Key);

                if (ivKeyDetail is null)
                {
                    return new(
                        new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No IV key stored.");
                }

                if (ivKeyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
                {
                    return new(
                        new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNoValue),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No IV key loaded.");
                }

                if (ivKeyDetail.KeyUsage != "I0")
                {
                    return new(
                        new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"No IV key stored.");
                }
                if (ivKeyDetail.Algorithm != "D" &&
                    ivKeyDetail.Algorithm != "T")
                {
                    return new(
                        new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"The key {ivKeyDetail.KeyName} doesn't have a DES or TDES algorithm supported for IV decryption.");
                }
                if (ivKeyDetail.ModeOfUse != "D")
                {
                    return new(
                        new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"The key {ivKeyDetail.KeyName} doesn't support mode of use for IV decryption.");
                }
            }

            // Check key is stored and available
            KeyDetail keyDetail = KeyManagement.GetKeyDetail(cryptoData.Payload.Key);
            if (keyDetail is null)
            {
                return new(
                    new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNotFound),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key name is not found. {cryptoData.Payload.Key}");
            }
            if (keyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
            {
                return new(
                    new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.KeyNoValue),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key is not loaded. {cryptoData.Payload.Key}");
            }

            if (!string.IsNullOrEmpty(cryptoData.Payload.ModeOfUse) &&
                cryptoData.Payload.ModeOfUse != "E" &&
                cryptoData.Payload.ModeOfUse != "D")
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Specified ModeOfUse property is incorrect. only E or D are valid.  {cryptoData.Payload.ModeOfUse}");
            }

            if (keyDetail.ModeOfUse != "E" &&
                keyDetail.ModeOfUse != "D" &&
                keyDetail.ModeOfUse != "B")
            {
                return new(
                    new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key doesn't have a right mode of use. {cryptoData.Payload.Key} {keyDetail.ModeOfUse}");
            }

            if (!string.IsNullOrEmpty(cryptoData.Payload.ModeOfUse) &&
                keyDetail.ModeOfUse != "B" && keyDetail.ModeOfUse != cryptoData.Payload.ModeOfUse)
            {
                return new(
                    new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Specified key doesn't have a mode of use 'B', however ModeOfUse property is set.  {cryptoData.Payload.Key} {cryptoData.Payload.ModeOfUse}");
            }

            // Chcek the crypto capabilities, check must be done before loading keys. but just double checking here
            bool verifyCryptAttrib = false;
            if (Common.CryptoCapabilities.CryptoAttributes.ContainsKey("D0"))
            {
                if (Common.CryptoCapabilities.CryptoAttributes["D0"].ContainsKey(keyDetail.Algorithm))
                {
                    if (Common.CryptoCapabilities.CryptoAttributes["D0"][keyDetail.Algorithm].ContainsKey(keyDetail.ModeOfUse))
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
                            verifyCryptAttrib = Common.CryptoCapabilities.CryptoAttributes["D0"][keyDetail.Algorithm][keyDetail.ModeOfUse].CryptoMethods.HasFlag(cryptoMethod);
                    }
                }
            }
            if (!verifyCryptAttrib)
            {
                return new(
                    new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"The crypto attribute doesn't support specified RSA signature algorithm or unsupported mode of use for MAC.");
            }

            List<byte> ivData = null;
            string ivKeyName = string.Empty;
            if (!string.IsNullOrEmpty(cryptoData.Payload.Iv?.Key) ||
                cryptoData.Payload.Iv?.Value?.Count > 0)
            {
                if (!string.IsNullOrEmpty(cryptoData.Payload.Iv.Key))
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
                            new(CryptoDataCompletion.PayloadData.ErrorCodeEnum.UseViolation),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"The crypto attribute doesn't support decrypt IV data with IV key.");
                    }
                }

                ivData = (new byte[8]).Select(x => x = 0).ToList();

                // Need an IV
                if (string.IsNullOrEmpty(cryptoData.Payload.Iv?.Key) &&
                    cryptoData.Payload.Iv?.Value?.Count > 0)
                {
                    // ClearIV;
                    ivData = cryptoData.Payload.Iv.Value;
                }
                else if (!string.IsNullOrEmpty(cryptoData.Payload.Iv?.Key) &&
                         cryptoData.Payload.Iv?.Value?.Count > 0)
                {
                    // In this last mode, the data is encrypted, so we have to decrypt
                    // it then send it as a clear IV
                    Logger.Log(Constants.DeviceClass, "CryptoDev.Crypto()");

                    var decryptResult = await Device.Crypto(
                        new CryptoCommandEvents(events), 
                        new CryptoDataRequest(
                            CryptoDataRequest.CryptoModeEnum.Decrypt,
                            CryptoDataRequest.CryptoAlgorithmEnum.ECB,
                            cryptoData.Payload.Iv.Key,
                            KeyManagement.GetKeyDetail(cryptoData.Payload.Iv.Key).KeySlot,
                            cryptoData.Payload.Iv.Value,
                            0),
                        cancel);

                    Logger.Log(Constants.DeviceClass, $"CryptoDev.Crypto() -> {decryptResult.CompletionCode}, {decryptResult.ErrorCode}");

                    if (decryptResult.CompletionCode != MessageHeader.CompletionCodeEnum.Success)
                    {
                        return new(
                            decryptResult.ErrorCode is not null ? new(decryptResult.ErrorCode) : null,
                            decryptResult.CompletionCode,
                            decryptResult.ErrorDescription);
                    }

                    ivData = decryptResult.CryptoData;
                }
                else
                {
                    ivKeyName = cryptoData.Payload.Iv?.Key;
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

            var result = await Device.Crypto(
                new CryptoCommandEvents(events),
                new CryptoDataRequest(
                    modeOfUse,
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

            CryptoDataCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.CryptoData?.Count > 0)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    Data: result.CryptoData);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IKeyManagementService KeyManagement { get => Provider.IsA<IKeyManagementService>(); }
    }
}
