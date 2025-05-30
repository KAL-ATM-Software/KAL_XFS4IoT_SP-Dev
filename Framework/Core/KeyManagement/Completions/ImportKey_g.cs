/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ImportKey_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "KeyManagement.ImportKey")]
    public sealed class ImportKeyCompletion : Completion<ImportKeyCompletion.PayloadData>
    {
        public ImportKeyCompletion(int RequestId, ImportKeyCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> VerificationData = null, VerifyAttributesClass VerifyAttributes = null, int? KeyLength = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.VerificationData = VerificationData;
                this.VerifyAttributes = VerifyAttributes;
                this.KeyLength = KeyLength;
            }

            public enum ErrorCodeEnum
            {
                KeyNotFound,
                AccessDenied,
                DuplicateKey,
                KeyNoValue,
                UseViolation,
                FormatNotSupported,
                InvalidKeyLength,
                NoKeyRam,
                SignatureNotSupported,
                SignatureInvalid,
                RandomInvalid,
                AlgorithmNotSupported,
                ModeNotSupported,
                CryptoMethodNotSupported,
                InvalidValue,
                FormatInvalid,
                ContentInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```keyNotFound``` - One of the keys specified was not found.
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            ///   specific reason.
            /// * ```duplicateKey``` - A key exists with that name and cannot be overwritten.
            /// * ```keyNoValue``` - One of the specified keys is not loaded.
            /// * ```useViolation``` - The use specified by *keyUsage* is not supported or conflicts with a previously
            ///   loaded key with the same name as *key* or the usage of
            ///   [decryptKey](#keymanagement.importkey.command.properties.decryptkey)
            ///   is not supported.
            /// * ```formatNotSupported``` - The specified format is not supported.
            /// * ```invalidKeyLength``` - The length of value is not supported.
            /// * ```noKeyRam``` - There is no space left in the key RAM for a key of the specified type.
            /// * ```signatureNotSupported``` - The *cryptoMethod* of the *verifyAttributes* is not supported. The key
            ///   is not stored in the device.
            /// * ```signatureInvalid``` - The verification data in *verificationData* the input data is invalid. The
            ///   key is not stored in the device.
            /// * ```randomInvalid``` - The encrypted random number in the input data does not match the one previously
            ///   provided by the device. The key is not stored in the device.
            /// * ```algorithmNotSupported``` - The algorithm specified by *algorithm* is not supported by this command.
            /// * ```modeNotSupported``` - The mode specified by *modeOfUse* is not supported.
            /// * ```cryptoMethodNotSupported``` - The cryptographic method specified by *cryptoMethod* for *keyAttributes*
            ///   or *verifyAttributes* is not supported.
            /// * ```invalidValue``` - The key [value](#keymanagement.importkey.command.properties.value) contains a key
            ///   block which failed its authentication check. The key is not stored in the device.
            /// * ```formatInvalid``` - The format of the key block is invalid.
            /// * ```contentInvalid``` - The content of the key block is invalid.
            /// * ```formatNotSupported``` - The key block version or content is not supported.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The verification data.
            /// 
            /// This property can be null if there is no verification data.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "verificationData")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> VerificationData { get; init; }

            [DataContract]
            public sealed class VerifyAttributesClass
            {
                public VerifyAttributesClass(string KeyUsage = null, string Algorithm = null, string ModeOfUse = null, CryptoMethodEnum? CryptoMethod = null, HashAlgorithmEnum? HashAlgorithm = null)
                {
                    this.KeyUsage = KeyUsage;
                    this.Algorithm = Algorithm;
                    this.ModeOfUse = ModeOfUse;
                    this.CryptoMethod = CryptoMethod;
                    this.HashAlgorithm = HashAlgorithm;
                }

                /// <summary>
                /// Specifies the key usage.
                /// The following values are possible:
                /// 
                /// * ```M0``` - ISO 16609 MAC Algorithm 1 (using TDEA).
                /// * ```M1``` - ISO 9797-1 MAC Algorithm 1.
                /// * ```M2``` - ISO 9797-1 MAC Algorithm 2.
                /// * ```M3``` - ISO 9797-1 MAC Algorithm 3.
                /// * ```M4``` - ISO 9797-1 MAC Algorithm 4.
                /// * ```M5``` - ISO 9797-1:1999 MAC Algorithm 5.
                /// * ```M6``` - ISO 9797-1:2011 MAC Algorithm 5 / CMAC.
                /// * ```M7``` - HMAC.
                /// * ```M8``` - ISO 9797-1:2011 MAC Algorithm 6.
                /// * ```S0``` - Asymmetric key pair or digital signature.
                /// * ```S1``` - Asymmetric key pair, CA key.
                /// * ```S2``` - Asymmetric key pair, nonX9.24 key.
                /// * ```00 - 99``` - These numeric values are reserved for proprietary use.
                /// <example>M0</example>
                /// </summary>
                [DataMember(Name = "keyUsage")]
                [DataTypes(Pattern = @"^M[0-8]$|^S[0-2]$|^[0-9][0-9]$")]
                public string KeyUsage { get; init; }

                /// <summary>
                /// Specifies the encryption algorithm.
                /// The following values are possible:
                /// 
                /// * ```A``` - AES.
                /// * ```D``` - DEA.
                /// * ```R``` - RSA.
                /// * ```T``` - Triple DEA (also referred to as TDEA).
                /// * ```"0" - "9"``` - These numeric values are reserved for proprietary use.
                /// <example>T</example>
                /// </summary>
                [DataMember(Name = "algorithm")]
                [DataTypes(Pattern = @"^[0-9ADRT]$")]
                public string Algorithm { get; init; }

                /// <summary>
                /// Specifies the encryption mode.
                /// The following values are possible:
                /// 
                /// * ```S``` - Signature.
                /// * ```V``` - Verify Only.
                /// * ```0 - 9``` - These numeric values are reserved for proprietary use.
                /// <example>V</example>
                /// </summary>
                [DataMember(Name = "modeOfUse")]
                [DataTypes(Pattern = @"^[0-9SV]$")]
                public string ModeOfUse { get; init; }

                public enum CryptoMethodEnum
                {
                    KcvNone,
                    KcvSelf,
                    KcvZero,
                    SigNone,
                    RsassaPkcs1V15,
                    RsassaPss
                }

                /// <summary>
                /// This parameter specifies the cryptographic method
                /// [cryptomethod](#common.capabilities.completion.properties.keymanagement.verifyattributes.m0.t.v.cryptomethod)
                /// that will be used with encryption algorithm.
                /// 
                /// If the *algorithm* property is
                /// ['A', 'D', or 'T'](#common.capabilities.completion.properties.keymanagement.verifyattributes.m0.t)
                /// and specified *keyUsage* property is MAC key usage (i.e.
                /// ['M1'](#common.capabilities.completion.properties.keymanagement.keyattributes.m0)), this property
                /// can be null.
                /// 
                /// If the *algorithm* property is
                /// ['A', 'D', or 'T'](#common.capabilities.completion.properties.keymanagement.verifyattributes.m0.t)
                /// and specified *keyUsage* property is
                /// ['00'](#common.capabilities.completion.properties.keymanagement.keyattributes.m0), this property
                /// can be one of the following values:
                /// 
                /// * ```kcvNone``` - There is no key check value verification required.
                /// * ```kcvSelf``` - The key check value (KCV) is created by an encryption of the key with itself.
                /// * ```kcvZero``` - The key check value (KCV) is created by encrypting a zero value with the key.
                /// 
                /// If the *algorithm* property is
                /// ['R'](#common.capabilities.completion.properties.keymanagement.verifyattributes.m0.t) and specified
                /// *keyUsage* property is not ['00'](#common.capabilities.completion.properties.keymanagement.keyattributes.m0),
                /// this property can be one of the following values:
                /// 
                /// * ```sigNone``` - No signature algorithm specified. No signature verification will take place and
                ///   the content of *verificationData* is not required.
                /// * ```rsassaPkcs1V15``` - Use the RSASSA-PKCS1-v1.5 algorithm.
                /// * ```rsassaPss``` - Use the RSASSA-PSS algorithm.
                /// </summary>
                [DataMember(Name = "cryptoMethod")]
                public CryptoMethodEnum? CryptoMethod { get; init; }

                public enum HashAlgorithmEnum
                {
                    Sha1,
                    Sha256
                }

                /// <summary>
                /// For asymmetric signature verification methods (Specified *keyUsage* property is ['S0', 'S1', or
                /// 'S2'](#common.capabilities.completion.properties.keymanagement.keyattributes.k1)), this can be one
                /// of the following values:
                /// 
                /// * ```sha1``` - The SHA 1 digest algorithm.
                /// * ```sha256``` - The SHA 256 digest algorithm, as defined in ISO/IEC 10118-3:2004
                /// [[Ref. keymanagement-7](#ref-keymanagement-7)] and FIPS 180-2
                /// [[Ref. keymanagement-8](#ref-keymanagement-8)].
                /// 
                /// If the *keyUsage* property is any of the MAC usages (e.g.
                /// ['M1'](#common.capabilities.completion.properties.keymanagement.keyattributes.k1)), this property
                /// can be null.
                /// </summary>
                [DataMember(Name = "hashAlgorithm")]
                public HashAlgorithmEnum? HashAlgorithm { get; init; }

            }

            /// <summary>
            /// This parameter specifies the encryption algorithm, cryptographic method, and mode used to verify this
            /// command.  For a list of valid values see the
            /// [verifyAttributes](#common.capabilities.completion.properties.keymanagement.verifyattributes)
            /// capability properties.
            /// 
            /// This property should be null if there is no verification data.
            /// </summary>
            [DataMember(Name = "verifyAttributes")]
            public VerifyAttributesClass VerifyAttributes { get; init; }

            /// <summary>
            /// Specifies the length, in bits, of the key. Zero if the key length is unknown.
            /// </summary>
            [DataMember(Name = "keyLength")]
            [DataTypes(Minimum = 0)]
            public int? KeyLength { get; init; }

        }
    }
}
