﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT;

namespace XFS4IoTFramework.KeyManagement
{
    public enum KeyAccessErrorCodeEnum
    {
        KeyNotFound,
        KeyNoValue,
        UseViolation,
        AlgorithmNotSupp
    }

    public enum CertificateChangeEnum
    {
        Secondary
    }

    public enum SignerEnum
    {
        EPP,
        Issuer,
    }

    public sealed class AuthenticationData(
        AuthenticationData.SigningMethodEnum SigningMethod,
        string Key,
        List<byte> Data)
    {
        public enum SigningMethodEnum
        {
            None,
            CertHost,
            SigHost,
            CA,
            HL,
            CBCMAC,
            CMAC,
            CertHost_TR34,
            CA_TR34,
            HL_TR34,
            Reserved1,
            Reserved2,
            Reserved3
        }

        public SigningMethodEnum SigningMethod { get; init; } = SigningMethod;

        /// <summary>
        /// If the *signer* is cbcmac or mac are specified, then this _signatureKey_ property is the name of a key with the key usage of key attribute is M0 to M8.
        /// If sigHost is specified for signer property, then this property signatureKey specifies the name of a previously loaded asymmetric key(i.e. an RSA Public Key).
        /// The default Signature Issuer public key (installed in a secure environment during manufacture) will be used, 
        /// if this signatureKey propery is omitted or contains the name of the default Signature Issuer as defined in the document [Default keys and securitry item loaded during manufacture](#keymanagement.generalinformation.rklprocess.defaultkeyandsecurity).
        /// Otherwise, this property should be omitted.
        /// </summary>
        public string Key { get; init; } = Key;

        /// <summary>
        /// This property contains the signed version of the base64 encoded data that was provided by the KeyManagement device during the previous call to the StartExchange command.
        /// The signer specified by *signer* property is used to do the signing.
        /// Both the signature and the data that was signed must be verified before the operation is performed.
        /// If certHost, ca, or hl are specified for *signer* property, then _signedData_ is a PKCS #7 structure which includes the data that was returned by the StartAuthenticate command.
        /// The optional CRL field may or may not be included in the PKCS #7 _signedData_ structure.
        /// If the signer is certHostTr34, caTr34 or hlTr34, please refer to the X9 TR34-2012 [Ref. 42] for more details.
        /// If sigHost is specified for the *signer* property specified, then s is a PKCS #7 structure which includes the data that was returned by the StartAuthenticate command.
        /// If cmcmac or cmac are specified for the *signer* property specified, then _signatureKey_ must refer to a key loaded with the key usage of key attribute is M0 to M8.
        /// </summary>
        public List<byte> Data { get; init; } = Data;
    }

    public class KeyInformationBase
    {
        public KeyInformationBase(
            string KeyVersionNumber = "00",
            string Exportability = null,
            List<byte> OptionalKeyBlockHeader = null,
            int? Generation = null,
            DateTime? ActivatingDate = null,
            DateTime? ExpiryDate = null,
            int? Version = null)
        {
            this.KeyVersionNumber = KeyVersionNumber;

            if (!string.IsNullOrEmpty(Exportability))
                Regex.IsMatch(Exportability, KeyDetail.regxExportability).IsTrue($"Invalid key usage specified. {Exportability}");
            this.Exportability = Exportability;
            this.OptionalKeyBlockHeader = OptionalKeyBlockHeader;
            this.Generation = Generation;
            this.ActivatingDate = ActivatingDate;
            this.ExpiryDate = ExpiryDate;
            this.Version = Version;
        }

        /// <summary>
        /// Specifies a two-digit ASCII character version number, which is optionally used to indicate that contents 
        /// of the key block are a component, or to prevent re-injection of old keys.
        /// See [Reference 35. ANS X9 TR-31 2018] for all possible values.
        /// </summary>
        public string KeyVersionNumber { get; init; }

        /// <summary>
        /// Specifies whether the key may be transferred outside of the cryptographic domain in which the key is found.
        /// See[Reference 35.ANS X9 TR - 31 2018] for all possible values.
        /// </summary>
        public string Exportability { get; init; }

        /// <summary>
        /// Contains any optional header blocks, as defined in [Reference 35. ANS X9 TR-31 2018].
        /// This value can be omitted if there are no optional block headers.
        /// </summary>
        public List<byte> OptionalKeyBlockHeader { get; init; }

        /// <summary>
        /// Specifies the generation of the key.
        /// Different generations might correspond to different environments(e.g.test or production environment).
        /// The content is vendor specific.
        /// </summary>
        public int? Generation { get; init; }

        /// <summary>
        /// Specifies the date when the key is activated in the format YYYYMMDD.
        /// </summary>
        public DateTime? ActivatingDate { get; init; }

        /// <summary>
        /// Specifies the date when the key expires in the format YYYYMMDD.
        /// </summary>
        public DateTime? ExpiryDate { get; init; }

        /// <summary>
        /// Specifies the version of the key (the year in which the key is valid, e.g. 1 for 2001).
        /// This value can be omitted if no such information is available for the key.
        /// </summary>
        public int? Version { get; init; }
    }

    public class ImportKeyBaseRequest(
        string KeyName,
        string KeyUsage,
        string Algorithm,
        string ModeOfUse,
        string RestrictedKeyUsage = null,
        ImportKeyBaseRequest.VerifyAttributeClass VerifyAttribute = null,
        string VendorAttribute = null)
    {
        public sealed class VerifyAttributeClass(
            string KeyName,
            VerifyAttributeClass.VerifyMethodEnum VerifyMethod,
            VerifyAttributeClass.HashAlgorithmEnum? HashAlgorithm = null)
        {
            public enum VerifyMethodEnum
            {
                KCVNone,
                KCVSelf,
                KCVZero,
                SignatureNone,
                RSASSA_PKCS1_V1_5,
                RSASSA_PSS,
            }

            public enum HashAlgorithmEnum
            {
                SHA1,
                SHA256,
            }

            /// <summary>
            /// Key name for verification to use
            /// </summary>
            public string KeyName { get; init; } = KeyName;

            /// <summary>
            /// Data to verify
            /// </summary>
            public List<byte> VerificationData { get; init; }

            /// <summary>
            /// Cryptographic method to use
            /// </summary>
            public VerifyMethodEnum VerifyMethod { get; init; } = VerifyMethod;

            /// <summary>
            /// Hash algorithm to use
            /// </summary>
            public HashAlgorithmEnum? HashAlgorithm { get; init; } = HashAlgorithm;
        }

        /// <summary>
        /// Specifies the key name to store
        /// </summary>
        public string KeyName { get; init; } = KeyName;

        /// <summary>
        /// Key usage associated with the key to be stored
        /// </summary>
        public string KeyUsage { get; init; } = KeyUsage;

        /// <summary>
        /// Algorithm associated with key usage
        /// </summary>
        public string Algorithm { get; init; } = Algorithm;

        /// <summary>
        /// Mode of use associated with the Algorithm
        /// </summary>
        public string ModeOfUse { get; init; } = ModeOfUse;

        /// <summary>
        /// Restricted key usage
        /// </summary>
        public string RestrictedKeyUsage { get; init; } = RestrictedKeyUsage;

        /// <summary>
        /// Verify data if it's requested
        /// </summary>
        public VerifyAttributeClass VerifyAttribute { get; init; } = VerifyAttribute;

        /// <summary>
        /// Vendor specific attributes
        /// </summary>
        public string VendorAttribute { get; init; } = VendorAttribute;
    }

    public sealed class ImportKeyPartRequest(
        string KeyName,
        int ComponentNumber,
        string KeyUsage,
        string Algorithm,
        string ModeOfUse,
        string RestrictedKeyUsage = null) : ImportKeyBaseRequest(KeyName, KeyUsage, Algorithm, ModeOfUse, RestrictedKeyUsage: RestrictedKeyUsage)
    {

        /// <summary>
        /// Number of component to store temporarily
        /// </summary>
        public int ComponentNumber { get; init; } = ComponentNumber;
    }

    public sealed class AssemblyKeyPartsRequest(
        string KeyName,
        int KeySlot,
        string KeyUsage,
        string Algorithm,
        string ModeOfUse,
        string RestrictedKeyUsage = null,
        ImportKeyBaseRequest.VerifyAttributeClass VerifyAttribute = null,
        string VendorAttribute = null)
        : ImportKeyBaseRequest(KeyName, KeyUsage, Algorithm, ModeOfUse, RestrictedKeyUsage, VerifyAttribute, VendorAttribute)
    {

        /// <summary>
        /// Key slot to use, if the device class needs to use specific number, update it in the result
        /// </summary>
        public int KeySlot { get; init; } = KeySlot;
    }

    public sealed class ImportKeyRequest(
        string KeyName,
        int KeySlot,
        List<byte> KeyData,
        string KeyUsage,
        string Algorithm,
        string ModeOfUse,
        string RestrictedKeyUsage = null,
        ImportKeyBaseRequest.VerifyAttributeClass VerifyAttribute = null,
        ImportKeyRequest.DecryptAttributeClass DecryptAttribute = null,
        string VendorAttribute = null) 
        : ImportKeyBaseRequest(KeyName, KeyUsage, Algorithm, ModeOfUse, RestrictedKeyUsage, VerifyAttribute, VendorAttribute)
    {
        public sealed class DecryptAttributeClass(
            string KeyName,
            DecryptAttributeClass.DecryptMethodEnum DecryptoMethod)
        {
            public enum DecryptMethodEnum
            {
                ECB,
                CBC,
                CFB,
                OFB,
                CTR,
                XTS,
                RSAES_PKCS1_V1_5,
                RSAES_OAEP,
                TR31 // decrypt method is in the TR31 keyblock
            }

            /// <summary>
            /// Key name for verification to use
            /// </summary>
            public string KeyName { get; init; } = KeyName;

            /// <summary>
            /// Cryptographic method to use
            /// </summary>
            public DecryptMethodEnum DecryptoMethod { get; init; } = DecryptoMethod;
        }

        /// <summary>
        /// Key slot to use, if the device class needs to use specific number, update it in the result
        /// </summary>
        public int KeySlot { get; init; } = KeySlot;

        /// <summary>
        /// Key data to load
        /// </summary>
        public List<byte> KeyData { get; init; } = KeyData;

        /// <summary>
        /// Decrypt key before loading key specified
        /// </summary>
        public DecryptAttributeClass DecryptAttribute { get; init; } = DecryptAttribute;
    }

    public sealed class ImportKeyResult : DeviceResult
    {
        public sealed class VerifyAttributeClass
        {
            public VerifyAttributeClass(
                string KeyUsage,
                string Algorithm,
                string ModeOfUse,
                ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum VerifyMethod,
                ImportKeyRequest.VerifyAttributeClass.HashAlgorithmEnum? HashAlgorithm = null)
            {
                this.KeyUsage = KeyUsage;
                Regex.IsMatch(this.KeyUsage, KeyDetail.regxVerifyKeyUsage).IsTrue($"Invalid key usage specified. {this.KeyUsage}");
                this.Algorithm = Algorithm;
                Regex.IsMatch(this.Algorithm, KeyDetail.regxAlgorithm).IsTrue($"Invalid algorithm specified. {this.Algorithm}");
                this.ModeOfUse = ModeOfUse;
                Regex.IsMatch(this.ModeOfUse, KeyDetail.regxVerifyModeOfUse).IsTrue($"Invalid mode specified. {this.KeyUsage}");
                this.VerifyMethod = VerifyMethod;
                this.HashAlgorithm = HashAlgorithm;
            }

            /// <summary>
            /// Key usage to verify data
            /// </summary>
            public string KeyUsage { get; init; }

            /// <summary>
            /// Algorithm to use to verify data
            /// </summary>
            public string Algorithm { get; init; }

            /// <summary>
            /// Algorithm to use to verify data
            /// </summary>
            public string ModeOfUse { get; init; }

            /// <summary>
            /// Cryptographic method to use
            /// </summary>
            public ImportKeyRequest.VerifyAttributeClass.VerifyMethodEnum VerifyMethod { get; init; }

            /// <summary>
            /// Hash algorithm to use
            /// </summary>
            public ImportKeyRequest.VerifyAttributeClass.HashAlgorithmEnum? HashAlgorithm { get; init; }
        }

        public ImportKeyResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            ImportKeyCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.VerificationData = null;
            this.KeyLength = 0;
        }

        public ImportKeyResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            KeyInformationBase KeyInformation,
            List<byte> VerificationData,
            VerifyAttributeClass VerifyAttribute,
            int KeyLength,
            int? UpdatedKeySlot = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.KeyInformation = KeyInformation;
            this.VerificationData = VerificationData;
            this.VerifyAttribute = VerifyAttribute;
            this.KeyLength = KeyLength;
            this.UpdatedKeySlot = UpdatedKeySlot;
        }

        public ImportKeyCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// The KeySlot number is provided by the framework as an input parameter.
        /// It's possible to assign different KeySlot number by the device class
        /// </summary>
        public int? UpdatedKeySlot { get; init; }

        /// <summary>
        /// Store key information loaded by the device class
        /// </summary>
        public KeyInformationBase KeyInformation { get; init; }

        /// <summary>
        /// This parameter specifies the encryption algorithm, cryptographic method, and mode used to verify this command
        /// </summary>
        public List<byte> VerificationData { get; init; }

        /// <summary>
        /// Verify attribute
        /// </summary>
        public VerifyAttributeClass VerifyAttribute { get; init; }

        /// <summary>
        /// Specifies the length, in bits, of the key. 0 if the key length is unknown.
        /// </summary>
        public int KeyLength { get; init; }
    }

    public sealed class InitializationRequest(AuthenticationData Authentication = null)
    {

        /// <summary>
        /// Authentication data required for initializing device
        /// </summary>
        public AuthenticationData Authentication { get; init; } = Authentication;
    }

    public sealed class InitializationResult : DeviceResult
    {
        public InitializationResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            InitializationCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Identification = null;
        }

        public InitializationResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> Identification)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Identification = Identification;
        }

        public InitializationCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// ID key encrypted by the encryption key
        /// </summary>
        public List<byte> Identification { get; init; }
    }

    public sealed class DeleteKeyRequest(
        string KeyName,
        AuthenticationData Authentication = null)
    {

        /// <summary>
        /// Key name to delete, if the value is null or empty string, all key to be deleted
        /// </summary>
        public string KeyName { get; init; } = KeyName;

        /// <summary>
        /// Authentication data required to delete key
        /// </summary>
        public AuthenticationData Authentication { get; set; } = Authentication;
    }

    public sealed class GenerateKCVRequest(
        string KeyName,
        GenerateKCVRequest.KeyCheckValueEnum KCVMode)
    {
        public enum KeyCheckValueEnum
        {
            Self,
            Zero,
        }

        /// <summary>
        /// Key name to generate KCV
        /// </summary>
        public string KeyName { get; init; } = KeyName;

        /// <summary>
        /// KCV mode to generate
        /// </summary>
        public KeyCheckValueEnum KVCMode { get; init; }
    }

    public sealed class GenerateKCVResult : DeviceResult
    {
        public GenerateKCVResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            GenerateKCVCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.KCV = null;
        }

        public GenerateKCVResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> KCV)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.KCV = KCV;
        }

        public GenerateKCVCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Key check value generated
        /// </summary>
        public List<byte> KCV { get; init; }
    }

    [Obsolete("This method is no longer available in XFS4IoT 2024-2. This interface will be removed after version 4.")]
    public sealed class DeriveKeyRequest(
        string KeyName,
        int KeySlot,
        string KeyGeneratingKey,
        int KeyGeneratingKeySlot,
        int DerivationAlgorithm,
        List<byte> IVData,
        string IVKey,
        int IVKeySlot,
        byte Padding,
        List<byte> Data)
    {

        /// <summary>
        /// Key name to derive key
        /// </summary>
        public string KeyName { get; init; } = KeyName;

        /// <summary>
        /// The key slot of the derived key
        /// </summary>
        public int KeySlot { get; init; } = KeySlot;

        /// <summary>
        /// Specifies the name of the key generating key that is used for the derivation.
        /// </summary>
        public string KeyGeneratingKey { get; init; } = KeyGeneratingKey;

        /// <summary>
        /// The key slot of the key generating key
        /// </summary>
        public int KeyGeneratingKeySlot { get; init; } = KeyGeneratingKeySlot;

        /// <summary>
        /// Specifies the algorithm that is used for derivation.
        /// </summary>
        public int DerivationAlgorithm { get; init; } = DerivationAlgorithm;

        /// <summary>
        /// Data of the initialization vector
        /// </summary>
        public List<byte> IVData { get; init; } = IVData;

        /// <summary>
        /// The key name of the initialization vector
        /// </summary>
        public string IVKey { get; init; } = IVKey;

        /// <summary>
        /// The key slot of the initialization vector
        /// </summary>
        public int IVKeySlot { get; init; } = IVKeySlot;


        /// <summary>
        /// Specifies the padding character for the encryption step within the derivation. The valid range is 0 to 255
        /// </summary>
        public byte Padding { get; init; } = Padding;

        /// <summary>
        /// Data to be used for key derivation.
        /// </summary>
        public List<byte> Data { get; init; } = Data;
    }

    [Obsolete("This method is no longer available in XFS4IoT 2024-2. This interface will be removed after version 4.")]
    public sealed class DeriveKeyResult : DeviceResult
    {
        public sealed class LoadedKeyInformation(
            string KeyUsage,
            string Algorithm,
            string ModeOfUse,
            int KeyLength,
            string KeyVersionNumber,
            string Exportability,
            List<byte> OptionalKeyBlockHeader,
            int? Generation = null,
            DateTime? ActivatingDate = null,
            DateTime? ExpiryDate = null,
            int? Version = null) 
            : KeyInformationBase(KeyVersionNumber, Exportability, OptionalKeyBlockHeader, Generation, ActivatingDate, ExpiryDate, Version)
        {

            /// <summary>
            /// Key usage to load
            /// </summary>
            public string KeyUsage { get; init; } = KeyUsage;

            /// <summary>
            /// Algorithm associated with key usage
            /// </summary>
            public string Algorithm { get; init; } = Algorithm;

            /// <summary>
            /// Mode of use associated with the Algorithm
            /// </summary>
            public string ModeOfUse { get; init; } = ModeOfUse;

            /// <summary>
            /// Specifies the length, in bits, of the key. 0 if the key length is unknown.
            /// </summary>
            public int KeyLength { get; init; } = KeyLength;
        }

        public DeriveKeyResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            DeriveKeyCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.LoadedKeyDetail = null;
        }
        public DeriveKeyResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            LoadedKeyInformation LoadedKeyDetail,
            int? UpdatedKeySlot)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.LoadedKeyDetail = LoadedKeyDetail;
            this.UpdatedKeySlot = UpdatedKeySlot;
        }

        public DeriveKeyCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// if the key slot number is changed from an input parameter
        /// </summary>
        public int? UpdatedKeySlot { get; init; }

        /// <summary>
        /// Loaded derived key information
        /// </summary>
        public LoadedKeyInformation LoadedKeyDetail { get; init; }
    }

    public sealed class ExportEPPIdRequest(
        SignerEnum Signer,
        ExportEPPIdRequest.RSASignatureAlgorithmEnum SignatureAlgorithm = ExportEPPIdRequest.RSASignatureAlgorithmEnum.Default,
        string SignatureKeyName = null)
    {
        public enum RSASignatureAlgorithmEnum
        {
            Default,
            NoSignature,
            RSASSA_PKCS1_V1_5,     // SSA_PKCS_V1_5 Signatures supported
            RSASSA_PSS,            // SSA_PSS Signatures supported
        }

        /// <summary>
        /// Signer either EPP or offline Signature Issuer
        /// </summary>
        public SignerEnum Signer { get; init; } = Signer;

        /// <summary>
        /// Specifies the name of the private key to use to sign the exported item. 
        /// This property is null or empty string if the Signer is set to Issuer
        /// </summary>
        public string SignatureKeyName { get; init; } = SignatureKeyName;

        /// <summary>
        /// RSA signature algorithm to sign
        /// </summary>
        public RSASignatureAlgorithmEnum SignatureAlgorithm { get; init; } = SignatureAlgorithm;
    }

    public sealed class ExportRSAPublicKeyRequest(
        SignerEnum Signer,
        string KeyName,
        ExportRSAPublicKeyRequest.RSASignatureAlgorithmEnum SignatureAlgorithm = ExportRSAPublicKeyRequest.RSASignatureAlgorithmEnum.Default,
        string SignatureKeyName = null)
    {
        public enum RSASignatureAlgorithmEnum
        {
            Default,
            NoSignature,
            RSASSA_PKCS1_V1_5,     // SSA_PKCS_V1_5 Signatures supported
            RSASSA_PSS,            // SSA_PSS Signatures supported
        }

        /// <summary>
        /// Signer either EPP or offline Signature Issuer
        /// </summary>
        public SignerEnum Signer { get; init; } = Signer;

        /// <summary>
        /// Specifies the name of the public key to be exported. 
        /// The private/public key pair was installed during manufacture (Default Keys and Security Item loaded during manufacture) for a definition of these default keys. 
        /// If this value is null or empty, then the default EPP public key that is used for symmetric key encryption is exported
        /// </summary>
        public string KeyName { get; init; } = KeyName;

        /// <summary>
        /// Specifies the name of the private key to use to sign the exported item. 
        /// This property is null or empty string if the Signer is set to Issuer
        /// </summary>
        public string SignatureKeyName { get; init; } = SignatureKeyName;

        /// <summary>
        /// RSA signature algorithm to sign
        /// </summary>
        public RSASignatureAlgorithmEnum SignatureAlgorithm { get; init; } = SignatureAlgorithm;
    }

    public sealed class RSASignedItemResult : DeviceResult
    {
        public enum RSASignatureAlgorithmEnum
        {
            NoSignature,
            RSASSA_PKCS1_V1_5,     // SSA_PKCS_V1_5 Signatures supported
            RSASSA_PSS,            // SSA_PSS Signatures supported
        }

        public enum ErrorCodeEnum
        {
            NoRSAKeyPair,
            AccessDenied,
            KeyNotFound
        }

        public RSASignedItemResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            ErrorCodeEnum? ErrorCode = null)
           : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Data = null;
            this.SignatureAlgorithm = RSASignatureAlgorithmEnum.NoSignature;
            this.SelfSignature = null;
            this.Signature = null;
        }

        public RSASignedItemResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> Data,
            RSASignatureAlgorithmEnum SignatureAlgorithm,
            List<byte> Signature = null,
            List<byte> SelfSignature = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Data = Data;
            this.SignatureAlgorithm = SignatureAlgorithm;
            this.SelfSignature = SelfSignature;
            this.Signature = Signature;
        }

        public ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// EPP ID or Public key
        /// </summary>
        public List<byte> Data { get; init; }

        /// <summary>
        /// If a public key was requested then this property contains the RSA signature of the public key exported, 
        /// generated with the key-pair’s private component. this property can be null when key Self-Signatures are not supported or required.
        /// This property is only set if requested Signer property is EPP
        /// </summary>
        public List<byte> SelfSignature { get; init; }
        
        /// <summary>
        /// Signed signature data
        /// </summary>
        public List<byte> Signature { get; init; }


        /// <summary>
        /// RSA signature algorithm to signed
        /// </summary>
        public RSASignatureAlgorithmEnum SignatureAlgorithm { get; init; }
    }

    public sealed class ExportCertificateRequest(ExportCertificateRequest.CertificateTypeEnum Type)
    {
        public enum CertificateTypeEnum
        {
            EncryptionKey,
            VerificationKey,
            HostKey,
        }

        /// <summary>
        /// Specifies which public key certificate is requested.
        /// If the Status command indicates Primary Certificates are accepted, then the Primary Public Encryption Key or the Primary Public Verification Key will be read out.
        /// If the Status command indicates Secondary Certificates are accepted, then the Secondary Public Encryption Key or the Secondary Public Verification Key will be read out.
        /// </summary>
        public CertificateTypeEnum Type { get; init; } = Type;
    }

    public sealed class ExportCertificateResult : DeviceResult
    {

        public ExportCertificateResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            GetCertificateCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
           : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Certificate = null;
        }

        public ExportCertificateResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> Certificate)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Certificate = Certificate;
        }

        public GetCertificateCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Exported certificate data
        /// </summary>
        public List<byte> Certificate { get; init; }
    }

    public sealed class GenerateRSAKeyPairRequest(
        string KeyName,
        int KeySlot,
        GenerateRSAKeyPairRequest.ModeOfUseEnum PrivateKeyUsage,
        int ModulusLength,
        GenerateRSAKeyPairRequest.ExponentEnum Exponent)
    {
        public enum ExponentEnum
        {
            Default,
            Exponent1,
            Exponent4,
            Exponent16,
        }

        public enum ModeOfUseEnum
        {
            S,
            T,
        }

        /// <summary>
        /// Specifies the name of the new key-pair to be generated. 
        /// </summary>
        public string KeyName { get; init; } = KeyName;

        /// <summary>
        /// Key slot number to use
        /// </summary>
        public int KeySlot { get; init; } = KeySlot;

        /// <summary>
        /// Specifies mode of use
        /// S - Signature Only.
        /// T - Both Sign and Decrypt
        /// </summary>
        public ModeOfUseEnum PrivateKeyUsage { get; init; } = PrivateKeyUsage;

        /// <summary>
        /// Specifies the number of bits for the modulus of the RSA key pair to be generated. 
        /// When zero is specified then the PIN device will be responsible for defining the length
        /// </summary>
        public int ModulusLength { get; init; } = ModulusLength;

        /// <summary>
        /// Specifies the value of the exponent of the RSA key pair to be generated
        /// </summary>
        public ExponentEnum Exponent { get; init; } = Exponent;
    }

    public sealed class GenerateRSAKeyPairResult : DeviceResult
    {
        public sealed class LoadedKeyInformation(
            string KeyUsage,
            string Algorithm,
            string ModeOfUse,
            int KeyLength,
            string KeyVersionNumber = "00",
            string Exportability = null,
            List<byte> OptionalKeyBlockHeader = null,
            int? Generation = null,
            DateTime? ActivatingDate = null,
            DateTime? ExpiryDate = null,
            int? Version = null) 
            : KeyInformationBase(KeyVersionNumber, Exportability, OptionalKeyBlockHeader, Generation, ActivatingDate, ExpiryDate, Version)
        {

            /// <summary>
            /// Key usage for the generated asymmetric key
            /// It should be S0 to S2 or 00 - 99
            /// </summary>
            public string KeyUsage { get; init; } = KeyUsage;

            /// <summary>
            /// Algorithm
            /// It should be R
            /// or 0 - 9
            /// </summary>
            public string Algorithm { get; init; } = Algorithm;

            /// <summary>
            /// Mode of use
            /// It should be S ot T
            /// or 0 - 9
            /// </summary>

            public string ModeOfUse { get; init; } = ModeOfUse;

            /// <summary>
            /// Specifies the length, in bits, of the key. 0 if the key length is unknown.
            /// </summary>
            public int KeyLength { get; init; } = KeyLength;
        }

        public GenerateRSAKeyPairResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            GenerateRSAKeyPairCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
           : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.UpdatedKeySlot = null;
        }

        public GenerateRSAKeyPairResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            LoadedKeyInformation LoadedKeyDetail,
            int? UpdatedKeySlot = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.LoadedKeyDetail = LoadedKeyDetail;
            this.UpdatedKeySlot = UpdatedKeySlot;
        }

        public GenerateRSAKeyPairCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// if the key slot number is changed from an input parameter
        /// </summary>
        public int? UpdatedKeySlot { get; init; }

        /// <summary>
        /// Loaded derived key information
        /// </summary>
        public LoadedKeyInformation LoadedKeyDetail { get; init; }
    }

    public sealed class ReplaceCertificateRequest(List<byte> Certificate)
    {
        public enum CertificateTypeEnum
        {
            EncryptionKey,
            VerificationKey,
            HostKey,
        }

        /// <summary>
        /// Pointer to the PKCS # 7 message that will replace the current Certificate Authority. 
        /// The outer content uses the Signed-data content type, the inner content is a degenerate certificate only content containing the new CA certificate and Inner Signed Data type The certificate should be in a format represented in DER encoded ASN.1 notation.
        /// </summary>
        public List<byte> Certificate { get; init; } = Certificate;
    }

    public sealed class ReplaceCertificateResult : DeviceResult
    {
        public ReplaceCertificateResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            ReplaceCertificateCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
           : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Digest = null;
        }

        public ReplaceCertificateResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> Digest)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Digest = Digest;
        }

        public ReplaceCertificateCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// A PKCS #7 structure using a Digested-data content type. 
        /// The digest parameter should contain the thumb print value.
        /// </summary>
        public List<byte> Digest { get; init; }
    }

    public sealed class ImportCertificateRequest(
        ImportCertificateRequest.LoadOptionEnum LoadOption,
        ImportCertificateRequest.SignerEnum Signer,
        List<byte> Certificate)
    {
        public enum LoadOptionEnum
        {
            NewHost,
            ReplaceHost,
        }

        public enum SignerEnum
        {
            Host,
            CA,
            HL
        }

        /// <summary>
        /// Specifies the method to use to load the certificate
        /// </summary>
        public LoadOptionEnum LoadOption { get; init; } = LoadOption;

        /// <summary>
        /// Specifies the signer of the certificate to be loaded
        /// </summary>
        public SignerEnum Signer { get; init; } = Signer;

        /// <summary>
        /// The certificate that is to be loaded represented in DER encoded ASN.1 notation in DER encoded ASN.1 notation.
        /// </summary>
        public List<byte> Certificate { get; init; } = Certificate;
    }

    public sealed class ImportCertificateResult : DeviceResult
    {
        public enum RSAKeyCheckModeEnum
        {
            None,
            SHA1,
            SHA256,
        }

        public ImportCertificateResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            LoadCertificateCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
           : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.RSAData = null;
        }

        public ImportCertificateResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            RSAKeyCheckModeEnum KeyCheckMode,
            List<byte> RSAData)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.RSAKeyCheckMode = RSAKeyCheckMode;
            this.RSAData = RSAData;
        }

        public LoadCertificateCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Defines algorithm/method used to generate the public key check value/thumb print.
        /// The check value can be used to verify that the public key has been imported correctly.
        /// </summary>
        public RSAKeyCheckModeEnum RSAKeyCheckMode { get; init; }
        
        /// <summary>
        /// A PKCS #7 structure using a Digested-data content type. 
        /// The digest parameter should contain the thumb print value calculated by the algorithm specified by RSAKeyCheckMode. 
        /// If RSAKeyCheckMode is None, then this property can be set to null or empty list.
        /// </summary>
        public List<byte> RSAData { get; init; }
    }

    public sealed class StartKeyExchangeResult : DeviceResult
    {
        public enum RSAKeyCheckModeEnum
        {
            None,
            SHA1,
            SHA256,
        }

        public StartKeyExchangeResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            StartKeyExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
           : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.RandomItem = null;
        }

        public StartKeyExchangeResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> RandomItem)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.RandomItem = RandomItem;
        }

        public StartKeyExchangeCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// A randomly generated number created by the encryptor. 
        /// If the PIN device does not support random number generation and verification, this property can be null or zero length of list
        /// </summary>
        public List<byte> RandomItem { get; init; }
    }


    public sealed class StartAuthenticateRequest
    {
        public enum CommandEnum
        {
            Deletekey,
            Initialization,
        }

        public sealed class DeleteKeyInput(string Key)
        {

            /// <summary>
            /// Key name to delete
            /// </summary>
            public string Key { get; init; } = Key;
        }

        public sealed class InitializationInput
        {
            public InitializationInput()
            { }
        }

        public StartAuthenticateRequest(
            CommandEnum Command,
            DeleteKeyInput DeleteKeyCommandParam)
        {
            Contracts.Assert(Command == CommandEnum.Deletekey, $"Command enum must be delete key. {Command}");
            this.Command = Command;
            this.DeleteKeyCommandParam = DeleteKeyCommandParam;
            this.InitializationCommandParam = null;
        }
        public StartAuthenticateRequest(
            CommandEnum Command,
            InitializationInput InitializationCommandParam)
        {
            Contracts.Assert(Command == CommandEnum.Initialization, $"Command enum must be initialization. {Command}");
            this.Command = Command;
            this.InitializationCommandParam = InitializationCommandParam;
            this.DeleteKeyCommandParam = null;
        }

        /// <summary>
        /// Command type to get authentication data to sign
        /// </summary>
        public CommandEnum Command { get; init; }

        /// <summary>
        /// Command parameter for singing data
        /// </summary>
        public DeleteKeyInput DeleteKeyCommandParam { get; init; }

        public InitializationInput InitializationCommandParam { get; init; } 
    }

    public sealed class StartAuthenticateResult : DeviceResult
    {
        public StartAuthenticateResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.DataToSign = null;
            this.SigningMethod =  AuthenticationData.SigningMethodEnum.None;
        }

        public StartAuthenticateResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> DataToSign,
            AuthenticationData.SigningMethodEnum SigningMethod)
            : base(CompletionCode, null)
        {
            this.DataToSign = DataToSign;
            this.SigningMethod = SigningMethod;
        }

        public List<byte> DataToSign { get; init; }

        public AuthenticationData.SigningMethodEnum SigningMethod { get; init; }
    }

    public sealed class ImportKeyTokenRequest(
        string KeyName,
        string KeyUsage,
        List<byte> KeyToken,
        ImportKeyTokenRequest.LoadOptionEnum LoadOption)
    {
        public enum LoadOptionEnum
        {
            NoRandom,
            Random,
            NoRandom_CRL,
            Random_CRL
        }

        /// <summary>
        /// Name of the key to load
        /// </summary>
        public string KeyName { get; init; } = KeyName;

        /// <summary>
        /// If NoRandom_CRL or Random_CRL, KeyUsage is an empty string as the key usage is embedded in the KeyToken.
        /// </summary>
        public string KeyUsage { get; init; } = KeyUsage;

        /// <summary>
        /// The binary encoded PKCS #7 represented in DER encoded ASN.1 notation. This allows the Host to
        /// verify that key was imported correctly and to the correct device.The message has an outer Signed-data
        /// content type with the SignerInfo encryptedDigest field containing the HOST’s signature.The inner content is
        /// an Enveloped-data content type.The device identifier is included as the issuerAndSerialNumber within the RecipientInfo.
        /// </summary>
        public List<byte> KeyToken { get; init; } = KeyToken;

        /// <summary>
        ///* ```NoRandom``` Import a key without generating a using a random number.
        ///* ```Random``` Import a key by generating and using a random number.
        ///* ```NoRandom_CRL``` Import a key with a Certificate Revocation List included in the token. A random
        ///                    number is not generated nor used. This option is used for the
        ///                    One - Pass Protocol described in X9 TR34-2019 [[Ref. keymanagement-9](#ref-keymanagement-9)]
        ///* ```Random_CRL``` Import a key with a Certificate Revocation List included in the token. A random number
        ///                   is generated and used.This option is used for the Two - Pass Protocol described in X9 TR34-2019.
        ///                   If Random* or Random_CRL, the random number is included as an authenticated attribute within SignerInfo SignedAttributes.
        /// </summary>
        public LoadOptionEnum LoadOption { get; init; } = LoadOption;
    }

    public sealed class ImportKeyTokenResult : DeviceResult
    {
        public enum AuthenticationAlgorithmEnum
        {
            None,
            SHA1,
            SHA256,
        }

        public enum KeyCheckValueEnum
        {
            None,
            Self,
            Zero,
        }

        public ImportKeyTokenResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            int KeyLength,
            AuthenticationAlgorithmEnum AuthenticationAlgorithm,
            List<byte> AuthenticationData,
            KeyCheckValueEnum KCVMode,
            List<byte> KCV)
            : base(CompletionCode, null)
        {
            ErrorCode = null;
            this.KeyLength = KeyLength;
            this.AuthenticationAlgorithm = AuthenticationAlgorithm;
            this.AuthenticationData = AuthenticationData;
            this.KCVMode = KCVMode;
            this.KCV = KCV;
        }

        public ImportKeyTokenResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            ImportKeyTokenCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
           : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            AuthenticationAlgorithm = AuthenticationAlgorithmEnum.None;
            AuthenticationData = null;
            KCVMode = KeyCheckValueEnum.None;
            KCV = null;
            KeyLength = 0;
        }

        public ImportKeyTokenCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Loaded key length. if it's unknown, specify 0.
        /// </summary>
        public int KeyLength { get; init; }

        /// <summary>
        /// The algorithm used to generate the signature contained in the AuthenticationData property
        /// sent to the host.
        /// </summary>
        public AuthenticationAlgorithmEnum AuthenticationAlgorithm { get; init; }

        /// <summary>
        /// If LoadOption is Random or Random_CRL, this data is a binary encoded PKCS #7, represented
        /// in DER encoded ASN.1 notation.The message has an outer Signed-data content type with the SignerInfo
        /// encryptedDigest field containing the ATM’s signature.The random numbers are included as
        /// authenticatedAttributes within the SignerInfo. The inner content is a data content type, which contains the
        /// HOST identifier as an issuerAndSerialNumber sequence.
        /// If AuthenticationAlgorithm is None, then this will also be null or empty.
        /// </summary>
        public List<byte> AuthenticationData { get; init; }

        /// <summary>
        /// KCV mode is used to create the key check value.
        /// </summary>
        public KeyCheckValueEnum KCVMode { get; init; }

        /// <summary>
        /// key check value for the key verification code data that can be used for verification of the loaded key.
        /// if KVCMode is None, this property is null or an empty.
        /// </summary>
        public List<byte> KCV { get; init; }
    }

    public sealed class ImportEMVPublicKeyRequest(
        string KeyName,
        string KeyUsage,
        ImportEMVPublicKeyRequest.ImportSchemeEnum ImportScheme,
        List<byte> ImportData,
        string VerificationKeyName)
    {
        public enum ImportSchemeEnum
        {
            PlainText_CA,
            PlainText_Checksum_CA,
            EPI_CA,
            Issuer,
            ICC,
            ICC_PIN,
            PKCSV1_5_CA
        }

        /// <summary>
        /// Name of the key to load
        /// </summary>
        public string KeyName { get; init; } = KeyName;

        /// <summary>
        /// Specify the type of key usage for which the KeyName property can be used.
        /// * ```E0``` - EMV / Chip Issuer Master Key: Application Cryptogram.
        /// * ```E1``` - EMV / Chip Issuer Master Key: Secure Messaging for Confidentiality.
        /// * ```E2``` - EMV / Chip Issuer Master Key: Secure Messaging for Integrity.
        /// * ```E3``` - EMV / Chip Issuer Master Key: Data Authentication Code.
        /// * ```E4``` - EMV / Chip Issuer Master Key: Dynamic.
        /// * ```E5``` - EMV / Chip Issuer Master Key: Card Personalization.
        /// * ```E6``` - EMV / Chip Issuer Master Key: Other Initialization Vector (IV).
        /// * ```E7``` - EMV / Chip Asymmetric Key Pair for EMV/Smart Card based PIN/PIN Block Encryption.
        /// * ```00 - 99``` - These numeric values are reserved for proprietary use.
        /// </summary>
        public string KeyUsage { get; init; } = KeyUsage;

        /// <summary>
        /// The binary encoded PKCS #7 represented in DER encoded ASN.1 notation. This allows the Host to
        /// verify that key was imported correctly and to the correct device.The message has an outer Signed-data
        /// content type with the SignerInfo encryptedDigest field containing the HOST’s signature.The inner content is
        /// an Enveloped-data content type.The device identifier is included as the issuerAndSerialNumber within the RecipientInfo.
        /// </summary>
        public List<byte> KeyToken { get; init; }

        /// <summary>
        /// * ```PlainText_CA``` This scheme is used by VISA. A plain text CA public key is imported with no verification.
        ///                 The two parts of the key (modulus and exponent) are passed in clear mode as a DER encoded PKCS#1 public key.
        ///                 The key is loaded directly in the security module.
        /// * ```PlainText_Checksum_CA```  This scheme is used by VISA. A plain text CA public key is imported using the EMV 2000
        ///                     Book II verification algorithm and it is verified before being loaded in the security module.
        /// * ```EPI_CA``` This scheme is used by MasterCard Europe. A CA public key is imported using the self-signed
        /// scheme.
        /// * ```Issuer``` An Issuer public key is imported as defined in EMV 2000 Book II.
        /// * ```ICC``` An ICC public key is imported as defined in EMV 2000 Book II.
        /// * ```ICC_PIN``` An ICC PIN public key is imported as defined in EMV 2000 Book II.
        /// * ```PKCSV1_5_CA``` A CA public key is imported and verified using a signature generated with a private
        ///                     key for which the public key is already loaded.
        /// </summary>
        public ImportSchemeEnum ImportScheme { get; init; } = ImportScheme;

        /// <summary>
        /// Contains all the necessary data to complete the import using the scheme specified within ImportScheme.
        /// 
        /// If ImportScheme is PlainText_CA then ImportData contains a DER encoded PKCS#1 public key. No verification is
        /// possible. VerificationKeyName is ignored.
        /// 
        /// If ImportScheme is PlainText_Checksum_CA then ImportData contains table 23 data, as specified in EMV 2000 Book 2 (See
        /// [[Ref. keymanagement-3](#ref-keymanagement-3)]). The plain text key is verified as defined within EMV 2000
        /// Book 2, page 73. VerificationKeyName is ignored (See [[Ref. keymanagement-3](#ref-keymanagement-3)]).
        /// 
        /// If ImportScheme is EPI_CA then ImportScheme contains the concatenation of tables 4 and 13,
        /// as specified in [[Ref. keymanagement-4](#ref-keymanagement-4)], Europay International, EPI CA Module
        /// Technical – Interface specification Version 1.4. These tables are also described in the EMV Support
        /// Appendix. The self-signed public key is verified as defined by the reference document. KeyName is ignored.
        /// 
        /// If ImportScheme is Issure then ImportData contains the EMV public key certificate. Within the following
        /// descriptions tags are documented to indicate the source of the data, but they are not sent down to the
        /// service. The data consists of the concatenation of: the key exponent length (1 byte), the key exponent value
        /// (variable length – EMV Tag value: ‘9F32’), the EMV certificate length (1 byte), the EMV certificate value
        /// (variable length – EMV Tag value: ‘90’), the remainder length (1 byte). The remainder value (variable
        /// length – EMV Tag value: ‘92’), the PAN length (1 byte) and the PAN value (variable length – EMV Tag
        /// value: ‘5A’). The service will compare the leftmost three to eight hex digits (where each byte consists of
        /// two hex digits) of the PAN to the Issuer Identification Number retrieved from the certificate. For more
        /// explanations, the reader can refer to EMVCo, Book2 – Security &amp; Key Management Version 4.0, Table 4 (See
        /// [[Ref. keymanagement-3](#ref-keymanagement-3)]). VerificationKeyName defines the previously loaded key used to
        /// verify the signature.
        /// 
        /// If ImportScheme is ICC then ImportData contains the EMV public key certificate. Within the following
        /// descriptions tags are documented to indicate the source of the data, but they are not sent down to the
        /// service. The data consists of the concatenation of: the key exponent length (1 byte), the key exponent
        /// value (variable length– EMV Tag value: ‘9F47’), the EMV certificate length (1 byte), the EMV certificate
        /// value (variable length – EMV Tag value:’9F46’), the remainder length (1 byte), the remainder value
        /// (variable length – EMV Tag value: ‘9F48’), the SDA length (1 byte), the SDA value (variable length), the
        /// PAN length (1 byte) and the PAN value (variable length – EMV Tag value: ‘5A’). The service will compare the
        /// PAN to the PAN retrieved from the certificate. For more explanations, the reader can refer to EMVCo,
        /// Book2 – Security &amp; Key Management Version 4.0, Table 9 (See [[Ref. keymanagement-3](#ref-keymanagement-3)]).
        /// VerificationKeyName defines the previously loaded key used to verify the signature.
        /// 
        /// If ImportScheme is ICC_PIN then ImportData contains the EMV public key certificate. Within the following
        /// descriptions tags are documented to indicate the source of the data, but they are not sent down to the
        /// service. The data consists of the concatenation of: the key exponent length (1 byte), the key exponent
        /// value (variable length – EMV Tag value: ‘9F2E’), the EMV certificate length (1 byte), the EMV certificate
        /// value (variable length – EMV Tag value:’9F2D’), the remainder length (1 byte), the remainder value
        /// (variable length – EMV Tag value: ‘9F2F’), the SDA length (1 byte), the SDA value (variable length), the
        /// PAN length (1 byte) and the PAN value (variable length – EMV Tag value: ‘5A’). The service will compare the
        /// PAN to the PAN retrieved from the certificate. For more explanations, the reader can refer to EMVCo,
        /// Book2 – Security &amp; Key Management Version 4.0, Table 9 (See [[Ref. keymanagement-3](#ref-keymanagement-3)]).
        /// VerificationKeyName defines the previously loaded key used to verify the signature.
        /// 
        /// If ImportScheme is PKCSV1_5_CA then ImportData contains the CA public key signed with the previously
        /// loaded public key specified in VerificationKeyName. ImportData consists of the concatenation of EMV 2000 Book II
        /// Table 23 + 8 byte random number + Signature (See [[Ref. keymanagement-3](#ref-keymanagement-3)]). The
        /// 8-byte random number is not used for validation; it is used to ensure the signature is unique. The
        /// Signature consists of all the bytes in the ImportData buffer after table 23 and the 8-byte random number.
        /// </summary>
        public List<byte> ImportData { get; init; } = ImportData;

        /// <summary>
        /// The name of the previously loaded key used to verify the signature.
        /// This property is null or empty string if verification is not required.
        /// </summary>
        public string VerificationKeyName { get; init; } = VerificationKeyName;
    }

    public sealed class ImportEMVPublicKeyResult : DeviceResult
    {
        public ImportEMVPublicKeyResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ExpiryDate)
            : base(CompletionCode, null)
        {
            ErrorCode = null;
            this.ExpiryDate = ExpiryDate;
        }

        public ImportEMVPublicKeyResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            ImportEmvPublicKeyCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
           : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            ExpiryDate = null;
        }

        public ImportEmvPublicKeyCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Contains the expiry date of the certificate in the following format MMYY. If null, the certificate does
        /// not have an expiry date.
        /// <example>0123</example>
        /// </summary>
        public string ExpiryDate { get; init; }
    }
}
