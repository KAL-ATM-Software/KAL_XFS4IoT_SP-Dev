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
using System.Text.RegularExpressions;
using XFS4IoT;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.KeyManagement
{
    public partial class ImportKeyHandler
    {
        private async Task<ImportKeyCompletion.PayloadData> HandleImportKey(IImportKeyEvents events, ImportKeyCommand importKey, CancellationToken cancel)
        {
            if (string.IsNullOrEmpty(importKey.Payload.Key))
            {
                return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No key name specified.",
                                                           ImportKeyCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
            }

            if (importKey.Payload.KeyAttributes is null)
            {
                return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No key attribute specified.");
            }

            if (string.IsNullOrEmpty(importKey.Payload.KeyAttributes.KeyUsage))
            {
                return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No key usage specified.");
            }
            if (!Regex.IsMatch(importKey.Payload.KeyAttributes.KeyUsage, KeyDetail.regxKeyUsage))
            {
                return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Invalid key usage specified. {importKey.Payload.KeyAttributes.KeyUsage}");
            }

            if (string.IsNullOrEmpty(importKey.Payload.KeyAttributes.Algorithm))
            {
                return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No key algorith specified.");
            }
            if (!Regex.IsMatch(importKey.Payload.KeyAttributes.Algorithm, KeyDetail.regxAlgorithm))
            {
                return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Invalid algorith specified. {importKey.Payload.KeyAttributes.Algorithm}");
            }

            if (string.IsNullOrEmpty(importKey.Payload.KeyAttributes.ModeOfUse))
            {
                return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No key mode specified.");
            }
            if (!Regex.IsMatch(importKey.Payload.KeyAttributes.ModeOfUse, KeyDetail.regxModeOfUse))
            {
                return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"Invalid key mode specified.");
            }

            // Check key attributes supported
            List<string> keyUsages = new() { importKey.Payload.KeyAttributes.KeyUsage };
            for (int i = 0; i < 100; i++)
                keyUsages.Add(i.ToString("00"));
            bool keyAttribSupported = false;
            foreach (string keyUsage in keyUsages)
            {
                if (KeyManagement.KeyManagementCapabilities.KeyAttributes.ContainsKey(keyUsage))
                {
                    List<string> algorithms = new() { importKey.Payload.KeyAttributes.Algorithm };
                    for (int i = 0; i < 10; i++)
                        algorithms.Add(i.ToString("0"));
                    foreach (string algorithm in algorithms)
                    {
                        if (KeyManagement.KeyManagementCapabilities.KeyAttributes[keyUsage].ContainsKey(algorithm))
                        {
                            List<string> modes = new() { importKey.Payload.KeyAttributes.ModeOfUse };
                            for (int i = 0; i < 10; i++)
                                modes.Add(i.ToString("0"));
                            foreach (string mode in modes)
                            {
                                keyAttribSupported = KeyManagement.KeyManagementCapabilities.KeyAttributes[keyUsage][algorithm].ContainsKey(mode);
                                if (keyAttribSupported)
                                    break;
                            }
                        }
                        if (keyAttribSupported)
                            break;
                    }
                }
                if (keyAttribSupported)
                    break;
            }

            if (!keyAttribSupported)
            {
                return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"Specified key attribute is not supported.",
                                                           ImportKeyCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported);
            }

            int keySlot = KeyManagement.FindKeySlot(importKey.Payload.Key);
            SecureKeyEntryStatusClass secureKeyEntryStatus = KeyManagement.GetSecureKeyEntryStatus();
            bool assemblyParts;

            if (importKey.Payload.Constructing is not null &&
                (bool)importKey.Payload.Constructing)
            {
                if (!KeyManagement.KeyManagementCapabilities.KeyImportThroughParts)
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"The constructing property is enabled, but the device doesn't support secure key entry.");
                }

                if (!secureKeyEntryStatus.KeyBuffered)
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"There are not key component buffered.",
                                                               ImportKeyCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
                }

                if (!string.IsNullOrEmpty(importKey.Payload.Value))
                {
                    Logger.Warning(Constants.Framework, "Key value is set to buffer key components. the key data is ignored.");
                }

                int componentNumber;
                Dictionary<SecureKeyEntryStatusClass.KeyPartEnum, KeyComponentStatus> keyStatus = secureKeyEntryStatus.GetKeyStatus();
                if (!keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.First].Stored)
                {
                    keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.First].Stored.IsFalse($"First part of key is not loaded, but the second part of key flag is enabled.");
                    componentNumber = (int)SecureKeyEntryStatusClass.KeyPartEnum.First;
                }
                else
                {
                    // first part of key component is already loaded
                    if (keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.Second].Stored)
                    {
                        return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Both 2 parts of key components already stored.");
                    }

                    if (keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.First].KeyName != importKey.Payload.Key)
                    {
                        return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Invalid key name specified. first part stored {keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.First].KeyName}, attempt to load {importKey.Payload.Key}");
                    }

                    componentNumber = (int)SecureKeyEntryStatusClass.KeyPartEnum.Second;
                }

                Logger.Log(Constants.DeviceClass, "KeyManagementDev.ImportKeyPart()");

                var importKeyPartResult = await Device.ImportKeyPart(new ImportKeyPartRequest(importKey.Payload.Key,
                                                                                              componentNumber,
                                                                                              importKey.Payload.KeyAttributes.KeyUsage,
                                                                                              importKey.Payload.KeyAttributes.Algorithm,
                                                                                              importKey.Payload.KeyAttributes.ModeOfUse,
                                                                                              importKey.Payload.KeyAttributes.Restricted),
                                                                     cancel);

                Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ImportKeyPart() -> {importKeyPartResult.CompletionCode}, {importKeyPartResult.ErrorCode}");

                ImportKeyCompletion.PayloadData.VerifyAttributesClass verifyAttribute = null;
                if (importKeyPartResult.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
                {
                    secureKeyEntryStatus.KeyPartLoaded((SecureKeyEntryStatusClass.KeyPartEnum)componentNumber, importKey.Payload.Key);
                    // Successfully loaded and add key information managing internally
                    KeyManagement.AddKey(importKey.Payload.Key,
                                         keySlot,
                                         importKey.Payload.KeyAttributes.KeyUsage,
                                         importKey.Payload.KeyAttributes.Algorithm,
                                         importKey.Payload.KeyAttributes.ModeOfUse,
                                         importKeyPartResult.KeyLength,
                                         KeyDetail.KeyStatusEnum.Construct,
                                         false,
                                         importKey.Payload.KeyAttributes.Restricted);

                    if (importKeyPartResult.VerifyAttribute is not null)
                    {
                        verifyAttribute = new(
                            importKeyPartResult.VerifyAttribute.KeyUsage,
                            importKeyPartResult.VerifyAttribute.Algorithm,
                            importKeyPartResult.VerifyAttribute.ModeOfUse,
                            importKeyPartResult.VerifyAttribute.VerifyMethod switch 
                            {
                                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.KCVNone => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvNone,
                                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.KCVSelf => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvSelf,
                                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.KCVZero => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvZero,
                                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.RSASSA_PKCS1_V1_5 => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.RsassaPkcs1V15,
                                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.RSASSA_PSS => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.RsassaPs,
                                _ => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.SigNone
                            },
                            importKeyPartResult.VerifyAttribute.HashAlgorithm is null ? null : importKeyPartResult.VerifyAttribute.HashAlgorithm switch 
                                                                                               {
                                                                                                   ImportKeyRequest.VerifyAttributeClass.HashAlgorithmEnum.SHA1 => ImportKeyCompletion.PayloadData.VerifyAttributesClass.HashAlgorithmEnum.Sha1,
                                                                                                   _ => ImportKeyCompletion.PayloadData.VerifyAttributesClass.HashAlgorithmEnum.Sha256
                                                                                               });
                    }
                }

                return new ImportKeyCompletion.PayloadData(importKeyPartResult.CompletionCode,
                                                           importKeyPartResult.ErrorDescription,
                                                           importKeyPartResult.ErrorCode,
                                                           importKeyPartResult.VerificationData is not null && importKeyPartResult.VerificationData.Count > 0 ? Convert.ToBase64String(importKeyPartResult.VerificationData.ToArray()) : null,
                                                           verifyAttribute,
                                                           importKeyPartResult.KeyLength);
            }
            else
            {
                Dictionary<SecureKeyEntryStatusClass.KeyPartEnum, KeyComponentStatus> keyStatus = secureKeyEntryStatus.GetKeyStatus();
                assemblyParts = (keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.First].Stored && keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.Second].Stored);
                
                if (assemblyParts)
                {
                    if (!string.IsNullOrEmpty(importKey.Payload.Value))
                    {
                        Logger.Warning(Constants.Framework, "Key value is set for assembly buffered key components. the key data is ignored.");
                    }

                    Contracts.Assert(keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.First].KeyName == keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.Second].KeyName, $"Stored key component name should be identical. {keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.First].KeyName}, {keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.Second].KeyName}");


                    if (keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.First].KeyName != importKey.Payload.Key)
                    {
                        // Some of firmware allows to load different encryption key while secure key components are already loaded, however the SP framework returns error.
                        return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.SequenceError,
                                                                   $"Invalid sequence for importing encryption key as the secure key entry process is enabled. the key name preserved for the key components different on assemblying key components.");
                    }
                }
                else
                {
                    if (keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.First].Stored ||
                        keyStatus[SecureKeyEntryStatusClass.KeyPartEnum.Second].Stored)
                    {
                        return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.SequenceError,
                                                                   $"Invalid sequence for importing encryption key as the secure key entry process is enabled.");
                    }

                    if (string.IsNullOrEmpty(importKey.Payload.Value))
                    {
                        return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"No key value specified. {importKey.Payload.Key}");
                    }

                }
            }

            ImportKeyRequest.DecryptAttributeClass decryptKeyAttribute = null;
            if (!string.IsNullOrEmpty(importKey.Payload.DecryptKey))
            {
                KeyDetail decryptKeyDetail = KeyManagement.GetKeyDetail(importKey.Payload.DecryptKey);
                if (decryptKeyDetail is null)
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"Specified decrypt key is not stored. {importKey.Payload.DecryptKey}",
                                                           ImportKeyCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
                }

                if (decryptKeyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"Specified decrypt key is not loaded. {importKey.Payload.DecryptKey}",
                                                           ImportKeyCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
                }

                if (!KeyManagement.KeyManagementCapabilities.DecryptAttributes.ContainsKey(decryptKeyDetail.Algorithm))
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Specified decrypt key doesn't support required algorithm. {importKey.Payload.DecryptKey}",
                                                               ImportKeyCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported);
                }

                KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum decryptMethodCap = KeyManagement.KeyManagementCapabilities.DecryptAttributes[decryptKeyDetail.Algorithm].DecryptMethods;
                if (decryptMethodCap == KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.NotSupported)
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"No decrypt method is not supported. {importKey.Payload.DecryptMethod}",
                                                               ImportKeyCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
                }

                ImportKeyRequest.DecryptAttributeClass.DecryptMethodEnum decryptMethod = ImportKeyRequest.DecryptAttributeClass.DecryptMethodEnum.TR31;
                if (importKey.Payload.DecryptMethod is not null)
                {
                    if (decryptKeyDetail.Algorithm != "R")
                    {
                        if ((importKey.Payload.DecryptMethod == ImportKeyCommand.PayloadData.DecryptMethodEnum.Cbc &&
                             !decryptMethodCap.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CBC)) ||
                            (importKey.Payload.DecryptMethod == ImportKeyCommand.PayloadData.DecryptMethodEnum.Cfb &&
                             !decryptMethodCap.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CFB)) ||
                            (importKey.Payload.DecryptMethod == ImportKeyCommand.PayloadData.DecryptMethodEnum.Ctr &&
                             !decryptMethodCap.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.CTR)) ||
                            (importKey.Payload.DecryptMethod == ImportKeyCommand.PayloadData.DecryptMethodEnum.Ecb &&
                             !decryptMethodCap.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.ECB)) ||
                            (importKey.Payload.DecryptMethod == ImportKeyCommand.PayloadData.DecryptMethodEnum.Ofb &&
                             !decryptMethodCap.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.OFB)) ||
                            (importKey.Payload.DecryptMethod == ImportKeyCommand.PayloadData.DecryptMethodEnum.Xts &&
                             !decryptMethodCap.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.XTS)))
                        {
                            return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                       $"No decrypt method is not supported. {importKey.Payload.DecryptMethod}",
                                                                       ImportKeyCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
                        }

                        decryptMethod = importKey.Payload.DecryptMethod switch
                        {
                            ImportKeyCommand.PayloadData.DecryptMethodEnum.Cbc =>  ImportKeyRequest.DecryptAttributeClass.DecryptMethodEnum.CBC,
                            ImportKeyCommand.PayloadData.DecryptMethodEnum.Cfb =>  ImportKeyRequest.DecryptAttributeClass.DecryptMethodEnum.CFB,
                            ImportKeyCommand.PayloadData.DecryptMethodEnum.Ctr =>  ImportKeyRequest.DecryptAttributeClass.DecryptMethodEnum.CTR,
                            ImportKeyCommand.PayloadData.DecryptMethodEnum.Ecb =>  ImportKeyRequest.DecryptAttributeClass.DecryptMethodEnum.ECB,
                            ImportKeyCommand.PayloadData.DecryptMethodEnum.Ofb =>  ImportKeyRequest.DecryptAttributeClass.DecryptMethodEnum.OFB,
                            _ =>  ImportKeyRequest.DecryptAttributeClass.DecryptMethodEnum.XTS
                        };
                    }
                    else
                    {
                        if ((importKey.Payload.DecryptMethod == ImportKeyCommand.PayloadData.DecryptMethodEnum.RsaesOaep &&
                             !decryptMethodCap.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.RSAES_OAEP)) ||
                            (importKey.Payload.DecryptMethod == ImportKeyCommand.PayloadData.DecryptMethodEnum.RsaesPkcs1V15 &&
                             !decryptMethodCap.HasFlag(KeyManagementCapabilitiesClass.DecryptMethodClass.DecryptMethodEnum.RSAES_PKCS1_V1_5)))
                        {
                            return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                       $"No decrypt method is supported. {importKey.Payload.DecryptMethod}",
                                                                       ImportKeyCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
                        }

                        decryptMethod = importKey.Payload.DecryptMethod switch
                        {
                            ImportKeyCommand.PayloadData.DecryptMethodEnum.RsaesOaep => ImportKeyRequest.DecryptAttributeClass.DecryptMethodEnum.RSAES_OAEP,
                            _ =>  ImportKeyRequest.DecryptAttributeClass.DecryptMethodEnum.RSAES_PKCS1_V1_5
                        };
                    }
                }

                decryptKeyAttribute = new ImportKeyRequest.DecryptAttributeClass(importKey.Payload.DecryptKey, decryptMethod);
            }

            ImportKeyRequest.VerifyAttributeClass verifyKeyAttribute = null;
            if (!string.IsNullOrEmpty(importKey.Payload.VerifyKey))
            {
                KeyDetail verifyKeyDetail = KeyManagement.GetKeyDetail(importKey.Payload.VerifyKey);
                if (verifyKeyDetail is null)
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"Specified verify key is not stored. {importKey.Payload.VerifyKey}",
                                                           ImportKeyCompletion.PayloadData.ErrorCodeEnum.KeyNotFound);
                }

                if (verifyKeyDetail.KeyStatus != KeyDetail.KeyStatusEnum.Loaded)
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                           $"Specified verify key is not loaded. {importKey.Payload.VerifyKey}",
                                                           ImportKeyCompletion.PayloadData.ErrorCodeEnum.KeyNoValue);
                }

                if (verifyKeyDetail.KeyName != "_HostCert" &&
                    importKey.Payload.VerificationData is null &&
                    importKey.Payload.VerifyAttributes is null)
                {
                    // _HostCert is a public key loaded with the host token to verify key token, key token contains vertification data and no need to specify verification data and attribute
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"No verification data and verify attribute specified.");
                }

                if (!KeyManagement.KeyManagementCapabilities.VerifyAttributes.ContainsKey(verifyKeyDetail.KeyUsage))
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Specified verify key doesn't support required key usage. {importKey.Payload.VerifyKey}",
                                                               ImportKeyCompletion.PayloadData.ErrorCodeEnum.UseViolation);
                }

                if (!KeyManagement.KeyManagementCapabilities.VerifyAttributes[verifyKeyDetail.KeyUsage].ContainsKey(verifyKeyDetail.Algorithm))
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Specified verify key doesn't support required algorithm. {importKey.Payload.VerifyKey}",
                                                               ImportKeyCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported);
                }

                if (KeyManagement.KeyManagementCapabilities.VerifyAttributes[verifyKeyDetail.KeyUsage][verifyKeyDetail.Algorithm].ContainsKey("V"))
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Specified verify key doesn't support required mode of use. {importKey.Payload.VerifyKey}",
                                                               ImportKeyCompletion.PayloadData.ErrorCodeEnum.ModeNotSupported);
                }

                if (importKey.Payload.VerifyAttributes is null)
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"No verify attributes specified when verify key name is specified. {importKey.Payload.VerifyKey}");
                }

                if (importKey.Payload.VerifyAttributes.CryptoMethod is null)
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"No cryptoMethod specified when verify key name is specified. {importKey.Payload.VerifyKey}");
                }

                KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum cryptoMethodCap = KeyManagement.KeyManagementCapabilities.VerifyAttributes[verifyKeyDetail.KeyUsage][verifyKeyDetail.Algorithm]["V"].CryptoMethod;
                if (cryptoMethodCap == KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.NotSupported)
                {
                    return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                               $"Crypto method is not supported. {importKey.Payload.VerifyAttributes.CryptoMethod}",
                                                               ImportKeyCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
                }

                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum verifyMethod;
                ImportKeyRequest.VerifyAttributeClass.HashAlgorithmEnum? hashAlgorithm = null;
                if (verifyKeyDetail.Algorithm != "R")
                {
                    if ((importKey.Payload.VerifyAttributes.CryptoMethod == ImportKeyCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvNone &&
                         !cryptoMethodCap.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVNone)) ||
                        (importKey.Payload.VerifyAttributes.CryptoMethod == ImportKeyCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvSelf &&
                         !cryptoMethodCap.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVSelf)) ||
                        (importKey.Payload.VerifyAttributes.CryptoMethod == ImportKeyCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvZero &&
                         !cryptoMethodCap.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.KCVZero)))
                    {
                        return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                   $"No crypto method is supported. {importKey.Payload.VerifyAttributes.CryptoMethod}",
                                                                   ImportKeyCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
                    }

                    verifyMethod = importKey.Payload.VerifyAttributes.CryptoMethod switch
                    {
                        ImportKeyCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvSelf => ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.KCVSelf,
                        ImportKeyCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvZero => ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.KCVZero,
                        _ =>  ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.KCVNone
                    };
                }
                else
                {
                    if ((importKey.Payload.VerifyAttributes.CryptoMethod == ImportKeyCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.SigNone &&
                         !cryptoMethodCap.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.SignatureNone)) ||
                        (importKey.Payload.VerifyAttributes.CryptoMethod == ImportKeyCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.RsassaPkcs1V15 &&
                         !cryptoMethodCap.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.RSASSA_PKCS1_V1_5)) ||
                        (importKey.Payload.VerifyAttributes.CryptoMethod == ImportKeyCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.RsassaPs &&
                         !cryptoMethodCap.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.CryptoMethodEnum.RSASSA_PSS)))
                    {
                        return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                   $"No decrypt method is not supported. {importKey.Payload.VerifyAttributes.CryptoMethod}",
                                                                   ImportKeyCompletion.PayloadData.ErrorCodeEnum.CryptoMethodNotSupported);
                    }

                    verifyMethod = importKey.Payload.VerifyAttributes.CryptoMethod switch
                    {
                        ImportKeyCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.RsassaPkcs1V15 => ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.RSASSA_PKCS1_V1_5,
                        ImportKeyCommand.PayloadData.VerifyAttributesClass.CryptoMethodEnum.RsassaPs => ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.RSASSA_PSS,
                        _ =>  ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.SignatureNone
                    };

                    if (importKey.Payload.VerifyAttributes.HashAlgorithm is null &&
                        verifyMethod != ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.SignatureNone)
                    {
                        return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"No hash algorithm specified when verify method is {importKey.Payload.VerifyAttributes.CryptoMethod}.");
                    }

                    if (verifyMethod != ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.SignatureNone)
                    {
                        KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum hashAlgorithmCap = KeyManagement.KeyManagementCapabilities.VerifyAttributes[verifyKeyDetail.KeyUsage][verifyKeyDetail.Algorithm]["V"].HashAlgorithm;
                        if (hashAlgorithmCap == KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum.NotSupported)
                        {
                            return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                       $"No hash algorithm supported to verify.",
                                                                       ImportKeyCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported);
                        }

                        if ((importKey.Payload.VerifyAttributes.HashAlgorithm == ImportKeyCommand.PayloadData.VerifyAttributesClass.HashAlgorithmEnum.Sha1 &&
                             !hashAlgorithmCap.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum.SHA1)) ||
                            (importKey.Payload.VerifyAttributes.HashAlgorithm == ImportKeyCommand.PayloadData.VerifyAttributesClass.HashAlgorithmEnum.Sha256 &&
                             !hashAlgorithmCap.HasFlag(KeyManagementCapabilitiesClass.VerifyMethodClass.HashAlgorithmEnum.SHA256)))
                        {
                            return new ImportKeyCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                       $"No hash algorithm supported to verify. {importKey.Payload.VerifyAttributes.HashAlgorithm}",
                                                                       ImportKeyCompletion.PayloadData.ErrorCodeEnum.AlgorithmNotSupported);
                        }
                    }
                }

                verifyKeyAttribute = new ImportKeyRequest.VerifyAttributeClass(importKey.Payload.VerifyKey, verifyMethod, hashAlgorithm);
            }

            ImportKeyResult result;
            if (assemblyParts)
            {
                Logger.Log(Constants.DeviceClass, "KeyManagementDev.AssemblyKeyParts()");

                result = await Device.AssemblyKeyParts(new AssemblyKeyPartsRequest(importKey.Payload.Key,
                                                                                   keySlot,
                                                                                   importKey.Payload.KeyAttributes.KeyUsage,
                                                                                   importKey.Payload.KeyAttributes.Algorithm,
                                                                                   importKey.Payload.KeyAttributes.ModeOfUse,
                                                                                   importKey.Payload.KeyAttributes.Restricted,
                                                                                   verifyKeyAttribute,
                                                                                   importKey.Payload.VendorAttributes),
                                                       cancel);

                Logger.Log(Constants.DeviceClass, $"KeyManagementDev.AssemblyKeyParts() -> {result.CompletionCode}, {result.ErrorCode}");
            }
            else
            {
                Logger.Log(Constants.DeviceClass, "KeyManagementDev.ImportKey()");

                result = await Device.ImportKey(new ImportKeyRequest(importKey.Payload.Key,
                                                                     keySlot,
                                                                     Convert.FromBase64String(importKey.Payload.Value).ToList(),
                                                                     importKey.Payload.KeyAttributes.KeyUsage,
                                                                     importKey.Payload.KeyAttributes.Algorithm,
                                                                     importKey.Payload.KeyAttributes.ModeOfUse,
                                                                     importKey.Payload.KeyAttributes.Restricted,
                                                                     verifyKeyAttribute,
                                                                     decryptKeyAttribute,
                                                                     importKey.Payload.VendorAttributes),
                                                cancel);

                Logger.Log(Constants.DeviceClass, $"KeyManagementDev.ImportKey() -> {result.CompletionCode}, {result.ErrorCode}");
            }

            ImportKeyCompletion.PayloadData.VerifyAttributesClass importKeyVerifyAttib = null;
            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                secureKeyEntryStatus.Reset();

                if (result.VerifyAttribute is not null)
                {
                    importKeyVerifyAttib = new(
                            result.VerifyAttribute.KeyUsage,
                            result.VerifyAttribute.Algorithm,
                            result.VerifyAttribute.ModeOfUse,
                            result.VerifyAttribute.VerifyMethod switch 
                            {
                                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.KCVNone => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvNone,
                                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.KCVSelf => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvSelf,
                                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.KCVZero => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.KcvZero,
                                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.RSASSA_PKCS1_V1_5 => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.RsassaPkcs1V15,
                                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum.RSASSA_PSS => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.RsassaPs,
                                _ => ImportKeyCompletion.PayloadData.VerifyAttributesClass.CryptoMethodEnum.SigNone
                            },
                            result.VerifyAttribute.HashAlgorithm is null ? null : result.VerifyAttribute.HashAlgorithm switch 
                                                                                  {
                                                                                      ImportKeyRequest.VerifyAttributeClass.HashAlgorithmEnum.SHA1 => ImportKeyCompletion.PayloadData.VerifyAttributesClass.HashAlgorithmEnum.Sha1,
                                                                                      _ => ImportKeyCompletion.PayloadData.VerifyAttributesClass.HashAlgorithmEnum.Sha256
                                                                                  });
                }
                // Successfully loaded and add key information managing internally
                KeyManagement.AddKey(importKey.Payload.Key,
                                     result.UpdatedKeySlot is null ? keySlot : (int)result.UpdatedKeySlot,
                                     importKey.Payload.KeyAttributes.KeyUsage,
                                     importKey.Payload.KeyAttributes.Algorithm,
                                     importKey.Payload.KeyAttributes.ModeOfUse,
                                     result.KeyLength,
                                     KeyDetail.KeyStatusEnum.Loaded,
                                     false,
                                     importKey.Payload.KeyAttributes.Restricted,
                                     result.KeyInformation?.KeyVersionNumber,
                                     result.KeyInformation?.Exportability,
                                     result.KeyInformation?.OptionalKeyBlockHeader,
                                     result.KeyInformation?.Generation,
                                     result.KeyInformation?.ActivatingDate,
                                     result.KeyInformation?.ExpiryDate,
                                     result.KeyInformation?.Version);
            }

            return new ImportKeyCompletion.PayloadData(result.CompletionCode,
                                                       result.ErrorDescription,
                                                       result.ErrorCode,
                                                       result.VerificationData is not null && result.VerificationData.Count > 0 ? Convert.ToBase64String(result.VerificationData.ToArray()) : null,
                                                       importKeyVerifyAttib,
                                                       result.KeyLength);
        }
    }
}
