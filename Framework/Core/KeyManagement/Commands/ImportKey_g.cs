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
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = ImportKey
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "KeyManagement.ImportKey")]
    public sealed class ImportKeyCommand : Command<ImportKeyCommand.PayloadData>
    {
        public ImportKeyCommand(int RequestId, ImportKeyCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Key = null, KeyAttributesClass KeyAttributes = null, List<byte> Value = null, bool? Constructing = null, string DecryptKey = null, DecryptMethodEnum? DecryptMethod = null, List<byte> VerificationData = null, string VerifyKey = null, VerifyAttributesClass VerifyAttributes = null, string VendorAttributes = null)
                : base()
            {
                this.Key = Key;
                this.KeyAttributes = KeyAttributes;
                this.Value = Value;
                this.Constructing = Constructing;
                this.DecryptKey = DecryptKey;
                this.DecryptMethod = DecryptMethod;
                this.VerificationData = VerificationData;
                this.VerifyKey = VerifyKey;
                this.VerifyAttributes = VerifyAttributes;
                this.VendorAttributes = VendorAttributes;
            }

            /// <summary>
            /// Specifies the name of key being loaded.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            [DataContract]
            public sealed class KeyAttributesClass
            {
                public KeyAttributesClass(string KeyUsage = null, string Algorithm = null, string ModeOfUse = null, string RestrictedKeyUsage = null)
                {
                    this.KeyUsage = KeyUsage;
                    this.Algorithm = Algorithm;
                    this.ModeOfUse = ModeOfUse;
                    this.RestrictedKeyUsage = RestrictedKeyUsage;
                }

                /// <summary>
                /// Specifies the key usage.
                /// The following values are possible:
                /// 
                /// * ```B0``` - BDK Base Derivation Key.
                /// * ```B1``` - Initial DUKPT key.
                /// * ```B2``` - Base Key Variant Key.
                /// * ```B3``` - Key Derivation Key (Non ANSI X9.24).
                /// * ```C0``` - CVK Card Verification Key.
                /// * ```D0``` - Symmetric Key for Data Encryption.
                /// * ```D1``` - Asymmetric Key for Data Encryption.
                /// * ```D2``` - Data Encryption Key for Decimalization Table.
                /// * ```D3``` - Data Encryption Key for Sensitive Data.
                /// * ```E0``` - EMV / Chip Issuer Master Key: Application Cryptogram.
                /// * ```E1``` - EMV / Chip Issuer Master Key: Secure Messaging for Confidentiality.
                /// * ```E2``` - EMV / Chip Issuer Master Key: Secure Messaging for Integrity.
                /// * ```E3``` - EMV / Chip Issuer Master Key: Data Authentication Code.
                /// * ```E4``` - EMV / Chip Issuer Master Key: Dynamic.
                /// * ```E5``` - EMV / Chip Issuer Master Key: Card Personalization.
                /// * ```E6``` - EMV / Chip Issuer Master Key: Other Initialization Vector (IV).
                /// * ```E7``` - EMV / Chip Asymmetric Key Pair for EMV/Smart Card based PIN/PIN Block Encryption.
                /// * ```I0``` - Initialization Vector (IV).
                /// * ```K0``` - Key Encryption or wrapping.
                /// * ```K1``` - X9.143 Key Block Protection Key.
                /// * ```K2``` - TR-34 Asymmetric Key.
                /// * ```K3``` - Asymmetric Key for key agreement / key wrapping.
                /// * ```K4``` - Key Block Protection Key, ISO 20038.
                /// * ```M0``` - ISO 16609 MAC algorithm 1 (using TDEA).
                /// * ```M1``` - ISO 9797-1 MAC Algorithm 1.
                /// * ```M2``` - ISO 9797-1 MAC Algorithm 2.
                /// * ```M3``` - ISO 9797-1 MAC Algorithm 3.
                /// * ```M4``` - ISO 9797-1 MAC Algorithm 4.
                /// * ```M5``` - ISO 9797-1:2011 MAC Algorithm 5.
                /// * ```M6``` - ISO 9797-1:2011 MAC Algorithm 5 / CMAC.
                /// * ```M7``` - HMAC.
                /// * ```M8``` - ISO 9797-1:2011 MAC Algorithm 6.
                /// * ```P0``` - PIN Encryption.
                /// * ```P1``` - PIN Generation Key (reserved for ANSI X9.132-202x).
                /// * ```S0``` - Asymmetric key pair for digital signature.
                /// * ```S1``` - Asymmetric key pair, CA key.
                /// * ```S2``` - Asymmetric key pair, nonX9.24 key.
                /// * ```V0``` - PIN verification, KPV, other algorithm.
                /// * ```V1``` - PIN verification, IBM 3624.
                /// * ```V2``` - PIN verification, VISA PVV.
                /// * ```V3``` - PIN verification, X9-132 algorithm 1.
                /// * ```V4``` - PIN verification, X9-132 algorithm 2.
                /// * ```V5``` - PIN Verification Key, ANSI X9.132 algorithm 3.
                /// * ```00 - 99``` - These numeric values are reserved for proprietary use.
                /// <example>P0</example>
                /// </summary>
                [DataMember(Name = "keyUsage")]
                [DataTypes(Pattern = @"^B[0-3]$|^C0$|^D[0-3]$|^E[0-7]$|^I0$|^K[0-4]$|^M[0-8]$|^P[0-1]$|^S[0-2]$|^V[0-5]$|^[0-9][0-9]$")]
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
                /// * ```B``` - Both Encrypt and Decrypt / Wrap and unwrap.
                /// * ```C``` - Both Generate and Verify.
                /// * ```D``` - Decrypt / Unwrap Only.
                /// * ```E``` - Encrypt / Wrap Only.
                /// * ```G``` - Generate Only.
                /// * ```S``` - Signature Only.
                /// * ```T``` - Both Sign and Decrypt.
                /// * ```V``` - Verify Only.
                /// * ```X``` - Key used to derive other keys(s).
                /// * ```Y``` - Key used to create key variants.
                /// * ```0 - 9``` - These numeric values are reserved for proprietary use.
                /// <example>G</example>
                /// </summary>
                [DataMember(Name = "modeOfUse")]
                [DataTypes(Pattern = @"^[0-9BCDEGSTVXY]$")]
                public string ModeOfUse { get; init; }

                /// <summary>
                /// This property should only be included if the
                /// [keyUsage](#keymanagement.importkey.command.properties.keyattributes.keyusage) is a key encryption
                /// key usage (K* e.g. 'K0') and the key can only be used as the *decryptKey* for keys with one of the
                /// following usages if the keyUsage is a key encryption key usage.
                /// 
                /// * ```B0``` - BDK Base Derivation Key.
                /// * ```B1``` - Initial DUKPT key.
                /// * ```B2``` - Base Key Variant Key.
                /// * ```B3``` - Key Derivation Key (Non ANSI X9.24).
                /// * ```C0``` - CVK Card Verification Key.
                /// * ```D0``` - Symmetric Key for Data Encryption.
                /// * ```D1``` - Asymmetric Key for Data Encryption.
                /// * ```D2``` - Data Encryption Key for Decimalization Table.
                /// * ```D3``` - Data Encryption Key for Sensitive Data.
                /// * ```E0``` - EMV / Chip Issuer Master Key: Application Cryptogram.
                /// * ```E1``` - EMV / Chip Issuer Master Key: Secure Messaging for Confidentiality.
                /// * ```E2``` - EMV / Chip Issuer Master Key: Secure Messaging for Integrity.
                /// * ```E3``` - EMV / Chip Issuer Master Key: Data Authentication Code.
                /// * ```E4``` - EMV / Chip Issuer Master Key: Dynamic.
                /// * ```E5``` - EMV / Chip Issuer Master Key: Card Personalization.
                /// * ```E6``` - EMV / Chip Issuer Master Key: Other Initialization Vector (IV).
                /// * ```E7``` - EMV / Chip Asymmetric Key Pair for EMV/Smart Card based PIN/PIN Block Encryption.
                /// * ```I0``` - Initialization Vector (IV).
                /// * ```K0``` - Key Encryption or wrapping.
                /// * ```K1``` - X9.143 Key Block Protection Key.
                /// * ```K2``` - TR-34 Asymmetric Key.
                /// * ```K3``` - Asymmetric Key for key agreement / key wrapping.
                /// * ```K4``` - Key Block Protection Key, ISO 20038.
                /// * ```M0``` - ISO 16609 MAC algorithm 1 (using TDEA).
                /// * ```M1``` - ISO 9797-1 MAC Algorithm 1.
                /// * ```M2``` - ISO 9797-1 MAC Algorithm 2.
                /// * ```M3``` - ISO 9797-1 MAC Algorithm 3.
                /// * ```M4``` - ISO 9797-1 MAC Algorithm 4.
                /// * ```M5``` - ISO 9797-1:2011 MAC Algorithm 5.
                /// * ```M6``` - ISO 9797-1:2011 MAC Algorithm 5 / CMAC.
                /// * ```M7``` - HMAC.
                /// * ```M8``` - ISO 9797-1:2011 MAC Algorithm 6.
                /// * ```P0``` - PIN Encryption.
                /// * ```P1``` - PIN Generation Key (reserved for ANSI X9.132-202x).
                /// * ```S0``` - Asymmetric key pair for digital signature.
                /// * ```S1``` - Asymmetric key pair, CA key.
                /// * ```S2``` - Asymmetric key pair, nonX9.24 key.
                /// * ```V0``` - PIN verification, KPV, other algorithm.
                /// * ```V1``` - PIN verification, IBM 3624.
                /// * ```V2``` - PIN verification, VISA PVV.
                /// * ```V3``` - PIN verification, X9-132 algorithm 1.
                /// * ```V4``` - PIN verification, X9-132 algorithm 2.
                /// * ```V5``` - PIN Verification Key, ANSI X9.132 algorithm 3.
                /// * ```00 - 99``` - These numeric values are reserved for proprietary use.
                /// <example>K0</example>
                /// </summary>
                [DataMember(Name = "restrictedKeyUsage")]
                [DataTypes(Pattern = @"^B[0-2]$|^C0$|^D[0-2]$|^E[0-6]$|^I0$|^K[0-4]$|^M[0-8]$|^P0$|^S[0-2]$|^V[0-4]$|^[0-9][0-9]$")]
                public string RestrictedKeyUsage { get; init; }

            }

            /// <summary>
            /// This parameter specifies the encryption algorithm, cryptographic method, and mode to be used for the key
            /// imported by this command. For a list of valid values see the
            /// [keyAttribute](#common.capabilities.completion.properties.keymanagement.keyattributes) capability. The
            /// values specified must be compatible with the key identified by key.
            /// If a keyblock is being imported, this property can be null.
            /// </summary>
            [DataMember(Name = "keyAttributes")]
            public KeyAttributesClass KeyAttributes { get; init; }

            /// <summary>
            /// Specifies the Base64 encoded value of key to be loaded.
            /// If it is an RSA key the first 4 bytes contain the exponent and the following 128 the modulus.
            /// This property is not required for secure key entry and can be null.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "value")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Value { get; init; }

            /// <summary>
            /// If the key is under construction through the import of multiple parts from a secure encryption key entry
            /// buffer, then this property is set to true.
            /// </summary>
            [DataMember(Name = "constructing")]
            public bool? Constructing { get; init; }

            /// <summary>
            /// Specifies the name of the key used to decrypt the key being loaded.
            /// 
            /// If value contains a X9.143 key block, then *decryptKey* is the name of the key block protection key that
            /// is used to verify and decrypt the key block. This property is null if the data in *value* is not
            /// encrypted or the *constructing* property is true.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "decryptKey")]
            public string DecryptKey { get; init; }

            public enum DecryptMethodEnum
            {
                Ecb,
                Cbc,
                Cfb,
                Ofb,
                Ctr,
                Xts,
                RsaesPkcs1V15,
                RsaesOaep
            }

            /// <summary>
            /// Specifies the cryptographic method that shall be used with the key specified by *decryptKey*.
            /// 
            /// This property is not required if a keyblock is being imported, as the decrypt method is contained within
            /// the keyblock.
            /// 
            /// This property specifies the cryptographic method that will be used to decrypt the encrypted value.
            /// 
            /// This property should be null if the *constructing* property is true or if *decryptKey* is null.
            /// 
            /// For a list of valid values see this property in the
            /// [decryptAttribute](#common.capabilities.completion.properties.keymanagement.decryptattributes)
            /// capability.
            /// 
            /// If the *decryptKey* algorithm is
            /// ['A', 'D', or 'T'](#common.capabilities.completion.properties.keymanagement.decryptattributes.a), then
            /// this property can be one of the following values:
            /// 
            /// * ```ecb``` - The ECB encryption method.
            /// * ```cbc``` - The CBC encryption method.
            /// * ```cfb``` - The CFB encryption method.
            /// * ```ofb``` - The OFB encryption method.
            /// * ```ctr``` - The CTR method defined in NIST SP800-38A (See [[Ref. keymanagement-11](#ref-keymanagement-11)]).
            /// * ```xts``` - The XTS method defined in NIST SP800-38E (See [[Ref. keymanagement-12](#ref-keymanagement-12)]).
            /// 
            /// If the *decryptKey* algorithm is
            /// ['R'](#common.capabilities.completion.properties.keymanagement.decryptattributes.a), then this property
            /// can be one of the following values:
            /// 
            /// * ```rsaesPkcs1V15``` - Use the RSAES_PKCS1-v1.5 algorithm.
            /// * ```rsaesOaep``` - Use the RSAES OAEP algorithm.
            /// 
            /// If the specified [decryptKey](#keymanagement.importkey.command.properties.decryptkey) is key usage
            /// ['K1'](#common.capabilities.completion.properties.keymanagement.keyattributes.m0), then this property
            /// can be null.
            /// X9.143 defines the cryptographic methods used for each key block version.
            /// </summary>
            [DataMember(Name = "decryptMethod")]
            public DecryptMethodEnum? DecryptMethod { get; init; }

            /// <summary>
            /// Contains the data to be verified before importing.
            /// 
            /// This property can be null if no verification is needed before importing the key, the *constructing*
            /// property is true or *value* contains verification data.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "verificationData")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> VerificationData { get; init; }

            /// <summary>
            /// Specifies the name of the previously loaded key which will be used to verify the *verificationData*.
            /// This property can be null when no verification is needed before importing the key or the
            /// *constructing* property is true.
            /// <example>VerifyKey01</example>
            /// </summary>
            [DataMember(Name = "verifyKey")]
            public string VerifyKey { get; init; }

            [DataContract]
            public sealed class VerifyAttributesClass
            {
                public VerifyAttributesClass(CryptoMethodEnum? CryptoMethod = null, HashAlgorithmEnum? HashAlgorithm = null)
                {
                    this.CryptoMethod = CryptoMethod;
                    this.HashAlgorithm = HashAlgorithm;
                }

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
                /// If the verifyKey algorithm is
                /// ['A', 'D', or 'T'](#common.capabilities.completion.properties.keymanagement.verifyattributes.m0.t)
                /// and specified
                /// [verifyKey](#keymanagement.importkey.command.properties.verifykey) is MAC key usage (i.e.
                /// ['M1'](#common.capabilities.completion.properties.keymanagement.keyattributes.m0)),  this property
                /// can be null.
                /// 
                /// If the verifyKey algorithm is
                /// ['A', 'D', or 'T'](#common.capabilities.completion.properties.keymanagement.verifyattributes.m0.t)
                /// and specified [verifyKey](#keymanagement.importkey.command.properties.verifykey) is key usage
                /// ['00'](#common.capabilities.completion.properties.keymanagement.keyattributes.m0), this property can
                /// be one of the following values:
                /// 
                /// * ```kcvNone``` - There is no key check value verification required.
                /// * ```kcvSelf``` - The key check value (KCV) is created by an encryption of the key with itself.
                /// * ```kcvZero``` - The key check value (KCV) is created by encrypting a zero value with the key.
                /// 
                /// If the verifyKey algorithm is
                /// ['R'](#common.capabilities.completion.properties.keymanagement.verifyattributes.m0.t) and specified
                /// [verifyKey](#keymanagement.importkey.command.properties.verifykey) is not key usage
                /// ['00'](#common.capabilities.completion.properties.keymanagement.keyattributes.m0), then this
                /// property can be one of the following values:
                /// 
                /// * ```sigNone``` - No signature algorithm specified. No signature verification will take place and
                ///   the content of verificationData is not required.
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
                /// For asymmetric signature verification methods (Specified
                /// [verifyKey](#keymanagement.importkey.command.properties.verifykey) usage is
                /// ['S0', 'S1', or 'S2'](#common.capabilities.completion.properties.keymanagement.keyattributes.k1)),
                /// this can be one of the following values:
                /// 
                /// * ```sha1``` - The SHA 1 digest algorithm.
                /// * ```sha256``` - The SHA 256 digest algorithm, as defined in ISO/IEC 10118-3:2004
                /// [[Ref. keymanagement-7](#ref-keymanagement-7)] and FIPS 180-2
                /// [[Ref. keymanagement-8](#ref-keymanagement-8)].
                /// 
                /// If the specified [verifyKey](#keymanagement.importkey.command.properties.verifykey) is key usage any
                /// of the MAC usages (i.e. ['M1'](#common.capabilities.completion.properties.keymanagement.keyattributes.k1)),
                /// then this property can be null.
                /// </summary>
                [DataMember(Name = "hashAlgorithm")]
                public HashAlgorithmEnum? HashAlgorithm { get; init; }

            }

            /// <summary>
            /// This parameter specifies the encryption algorithm, cryptographic method, and mode to be used to verify
            /// this command or to generate verification output data. Verifying input data will result in no
            /// verification output data. For a list of valid values see the
            /// [verifyAttributes](#common.capabilities.completion.properties.keymanagement.verifyattributes)
            /// capability.
            /// 
            /// This property can be null if *verificationData* is not required or the *constructing* property is
            /// true.
            /// </summary>
            [DataMember(Name = "verifyAttributes")]
            public VerifyAttributesClass VerifyAttributes { get; init; }

            /// <summary>
            /// Specifies the vendor attributes of the key to be imported.
            /// Refer to vendor documentation for details.
            /// <example>See vendor documentation</example>
            /// </summary>
            [DataMember(Name = "vendorAttributes")]
            public string VendorAttributes { get; init; }

        }
    }
}
