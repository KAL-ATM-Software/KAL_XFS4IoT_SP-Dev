/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * KeyManagementSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.KeyManagement
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(EncryptionStateEnum? EncryptionState = null, CertificateStateEnum? CertificateState = null)
        {
            this.EncryptionState = EncryptionState;
            this.CertificateState = CertificateState;
        }

        public enum EncryptionStateEnum
        {
            Ready,
            NotReady,
            NotInitialized,
            Busy,
            Undefined,
            Initialized
        }

        /// <summary>
        /// Specifies the state of the encryption module.
        /// </summary>
        [DataMember(Name = "encryptionState")]
        public EncryptionStateEnum? EncryptionState { get; init; }

        public enum CertificateStateEnum
        {
            Unknown,
            Primary,
            Secondary,
            NotReady
        }

        /// <summary>
        /// Specifies the state of the public verification or encryption key in the PIN certificate modules.
        /// </summary>
        [DataMember(Name = "certificateState")]
        public CertificateStateEnum? CertificateState { get; init; }

    }


    [DataContract]
    public sealed class KeyAttributeClass
    {
        public KeyAttributeClass(string KeyUsage = null, string RestrictedKeyUsage = null, string Algorithm = null, string ModeOfUse = null, string CryptoMethod = null)
        {
            this.KeyUsage = KeyUsage;
            this.RestrictedKeyUsage = RestrictedKeyUsage;
            this.Algorithm = Algorithm;
            this.ModeOfUse = ModeOfUse;
            this.CryptoMethod = CryptoMethod;
        }

        /// <summary>
        /// Specifies the Key usages supported by [KeyManagement.ImportKey](#keymanagement.importkey) command and key usage string length must be two.
        /// The following values are possible:  
        /// 
        /// * ```B0``` - BDK Base Derivation Key. 
        /// * ```B1``` - Initial DUKPT key. 
        /// * ```B2``` - Base Key Variant Key. 
        /// * ```C0``` - CVK Card Verification Key. 
        /// * ```D0``` - Symmetric Key for Data Encryption. 
        /// * ```D1``` - Asymmetric Key for Data Encryption. 
        /// * ```D2``` - Data Encryption Key for Decimalization Table. 
        /// * ```E0``` - EMV / Chip Issuer Master Key: Application Cryptogram. 
        /// * ```E1``` - EMV / Chip Issuer Master Key: Secure Messaging for Confidentiality. 
        /// * ```E2``` - EMV / Chip Issuer Master Key: Secure Messaging for Integrity. 
        /// * ```E3``` - EMV / Chip Issuer Master Key: Data Authentication Code. 
        /// * ```E4``` - EMV / Chip Issuer Master Key: Dynamic. 
        /// * ```E5``` - EMV / Chip Issuer Master Key: Card Personalization. 
        /// * ```E6``` - EMV / Chip Issuer Master Key: Other Initialization Vector (IV). 
        /// * ```I0``` - Initialization Vector (IV). 
        /// * ```K0``` - Key Encryption or wrapping. 
        /// * ```K1``` - TR-31 Key Block Protection Key. 
        /// * ```K2``` - TR-34 Asymmetric Key. 
        /// * ```K3``` - Asymmetric Key for key agreement / key wrapping. 
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
        /// * ```S0``` - Asymmetric key pair for digital signature. 
        /// * ```S1``` - Asymmetric key pair, CA key. 
        /// * ```S2``` - Asymmetric key pair, nonX9.24 key. 
        /// * ```V0``` - PIN verification, KPV, other algorithm. 
        /// * ```V1``` - PIN verification, IBM 3624. 
        /// * ```V2``` - PIN verification, VISA PVV. 
        /// * ```V3``` - PIN verification, X9-132 algorithm 1. 
        /// * ```V4``` - PIN verification, X9-132 algorithm 2. 
        /// * ```00 - 99``` - These numeric values are reserved for proprietary use.
        /// </summary>
        [DataMember(Name = "keyUsage")]
        [DataTypes(Pattern = "^B[0-2]$|^C0$|^D[0-2]$|^E[0-6]$|^I0$|^K[0-3]$|^M[0-8]$|^P0$|^S[0-2]$|^V[0-4]$|^[0-9][0-9]$")]
        public string KeyUsage { get; init; }

        /// <summary>
        /// Specifies restricted key usage of the key associated with the keyUsage property specified.
        /// This property can be omitted if there is no restricted key usage required.
        /// Following restricted key usage can be set if the keyUsage proeprty specifies either K0 or K1.
        /// The following values are possible:
        /// 
        /// * ```B0``` - BDK Base Derivation Key. 
        /// * ```B1``` - Initial DUKPT key. 
        /// * ```B2``` - Base Key Variant Key. 
        /// * ```C0``` - CVK Card Verification Key. 
        /// * ```D0``` - Symmetric Key for Data Encryption. 
        /// * ```D1``` - Asymmetric Key for Data Encryption. 
        /// * ```D2``` - Data Encryption Key for Decimalization Table. 
        /// * ```E0``` - EMV / Chip Issuer Master Key: Application Cryptogram. 
        /// * ```E1``` - EMV / Chip Issuer Master Key: Secure Messaging for Confidentiality. 
        /// * ```E2``` - EMV / Chip Issuer Master Key: Secure Messaging for Integrity. 
        /// * ```E3``` - EMV / Chip Issuer Master Key: Data Authentication Code. 
        /// * ```E4``` - EMV / Chip Issuer Master Key: Dynamic. 
        /// * ```E5``` - EMV / Chip Issuer Master Key: Card Personalization. 
        /// * ```E6``` - EMV / Chip Issuer Master Key: Other Initialization Vector (IV). 
        /// * ```I0``` - Initialization Vector (IV). 
        /// * ```K2``` - TR-34 Asymmetric Key. 
        /// * ```K3``` - Asymmetric Key for key agreement / key wrapping. 
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
        /// * ```S0``` - Asymmetric key pair for digital signature. 
        /// * ```S1``` - Asymmetric key pair, CA key. 
        /// * ```S2``` - Asymmetric key pair, nonX9.24 key. 
        /// * ```V0``` - PIN verification, KPV, other algorithm. 
        /// * ```V1``` - PIN verification, IBM 3624. 
        /// * ```V2``` - PIN verification, VISA PVV. 
        /// * ```V3``` - PIN verification, X9-132 algorithm 1. 
        /// * ```V4``` - PIN verification, X9-132 algorithm 2. 
        /// * ```00 - 99``` - These numeric values are reserved for proprietary use.
        /// </summary>
        [DataMember(Name = "restrictedKeyUsage")]
        [DataTypes(Pattern = "^B[0-2]$|^C0$|^D[0-2]$|^E[0-6]$|^I0$|^K[2-3]$|^M[0-8]$|^P0$|^S[0-2]$|^V[0-4]$|^[0-9][0-9]$")]
        public string RestrictedKeyUsage { get; init; }

        /// <summary>
        /// Specifies the encryption algorithms supported by the import command. The following values are possible: 
        /// * ```A``` - AES.  * ```D``` - DEA.  * ```R``` - RSA.  * ```T``` - Triple DEA (also referred to as TDEA).  * ```0 - 9``` - These numeric values are reserved for proprietary use.
        /// 
        /// </summary>
        [DataMember(Name = "algorithm")]
        [DataTypes(Pattern = "^[0-9ADRT]$")]
        public string Algorithm { get; init; }

        /// <summary>
        /// Specifies the encryption modes supported by import key.
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
        /// </summary>
        [DataMember(Name = "modeOfUse")]
        [DataTypes(Pattern = "^[0-9BCDEGSTVXY]$")]
        public string ModeOfUse { get; init; }

        /// <summary>
        /// Specifies the cryptographic methods supported by the import command. 
        /// For attributes, this parameter is 0, because the key being imported is not being used yet 
        /// to perform a cryptographic method.
        /// </summary>
        [DataMember(Name = "cryptoMethod")]
        public string CryptoMethod { get; init; }

    }


    public enum DecryptCryptoMethodEnum
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


    [DataContract]
    public sealed class DecryptAttributeClass
    {
        public DecryptAttributeClass(string Algorithm = null, DecryptCryptoMethodEnum? CryptoMethod = null)
        {
            this.Algorithm = Algorithm;
            this.CryptoMethod = CryptoMethod;
        }

        /// <summary>
        /// Specifies the encryption algorithms supported by the import command.
        /// The following values are possible: 
        /// 
        /// * ```A``` - AES. 
        /// * ```D``` - DEA. 
        /// * ```R``` - RSA. 
        /// * ```T``` - Triple DEA (also referred to as TDEA). 
        /// * ```0 - 9``` - These numeric values are reserved for proprietary use.
        /// </summary>
        [DataMember(Name = "algorithm")]
        [DataTypes(Pattern = "^[0-9ADRT]$")]
        public string Algorithm { get; init; }


        [DataMember(Name = "cryptoMethod")]
        public DecryptCryptoMethodEnum? CryptoMethod { get; init; }

    }


    [DataContract]
    public sealed class VerifyAttributeClass
    {
        public VerifyAttributeClass(string KeyUsage = null, string Algorithm = null, string ModeOfUse = null, CryptoMethodEnum? CryptoMethod = null, HashAlgorithmClass HashAlgorithm = null)
        {
            this.KeyUsage = KeyUsage;
            this.Algorithm = Algorithm;
            this.ModeOfUse = ModeOfUse;
            this.CryptoMethod = CryptoMethod;
            this.HashAlgorithm = HashAlgorithm;
        }

        /// <summary>
        /// Specifies the key usages supported by the import command.
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
        /// </summary>
        [DataMember(Name = "keyUsage")]
        [DataTypes(Pattern = "^M[0-8]$|^S[0-2]$|^[0-9][0-9]$")]
        public string KeyUsage { get; init; }

        /// <summary>
        /// Specifies the encryption algorithms supported by the import command.
        /// The following values are possible: 
        /// 
        /// * ```A``` - AES. 
        /// * ```D``` - DEA. 
        /// * ```R``` - RSA. 
        /// * ```T``` - Triple DEA (also referred to as TDEA). 
        /// * ```0 - 9``` - These numeric values are reserved for proprietary use.
        /// </summary>
        [DataMember(Name = "algorithm")]
        [DataTypes(Pattern = "^[0-9ADRT]$")]
        public string Algorithm { get; init; }

        /// <summary>
        /// Specifies the encryption modes supported by the import command.
        /// The following values are possible: 
        /// 
        /// * ```V``` - Verify Only. 
        /// * ```0 - 9``` - These numeric values are reserved for proprietary use.
        /// </summary>
        [DataMember(Name = "modeOfUse")]
        [DataTypes(Pattern = "^[0-9V]$")]
        public string ModeOfUse { get; init; }

        public enum CryptoMethodEnum
        {
            KcvNone,
            KcvSelf,
            KcvZero,
            SigNone,
            RsassaPkcs1V15,
            RsassaPs
        }

        /// <summary>
        /// This parameter specifies the cryptographic method that will be used with encryption algorithm.
        /// 
        /// If algorithm is ‘A’, ‘D’, or ‘T’ and keyUsage is a MAC usage (i.e. ‘M1’), then this property cryptoMethod not be set. 
        /// 
        /// If algorithm is ‘A’, ‘D’, or ‘T’ and keyUsage is ’00’, then this property cryptoMethod can be one of the following values: 
        /// 
        /// * ```kcvNone``` - There is no key check value verification required. 
        /// * ```kcvSelf``` - The key check value (KCV) is created by an encryption of the key with itself. 
        /// * ```kcvZero``` - The key check value (KCV) is created by encrypting a zero value with the key. 
        /// 
        /// If algorithm is ‘R’ and keyUsage is not ‘00’, then this property cryptoMethod can be one of the following values: 
        /// 
        /// * ```sigNone``` - No signature algorithm specified. No signature verification will take place and the 
        /// content of verificationData must not be set. 
        /// * ```rsassaPkcs1V15``` - Use the RSASSA-PKCS1-v1.5 algorithm. 
        /// * ```rsassaPss``` - Use the RSASSA-PSS algorithm.
        /// </summary>
        [DataMember(Name = "cryptoMethod")]
        public CryptoMethodEnum? CryptoMethod { get; init; }

        [DataContract]
        public sealed class HashAlgorithmClass
        {
            public HashAlgorithmClass(bool? Sha1 = null, bool? Sha256 = null)
            {
                this.Sha1 = Sha1;
                this.Sha256 = Sha256;
            }

            /// <summary>
            /// The SHA 1 digest algorithm.
            /// </summary>
            [DataMember(Name = "sha1")]
            public bool? Sha1 { get; init; }

            /// <summary>
            /// The SHA 256 digest algorithm, as defined in ISO/IEC 10118-3:2004 and FIPS 180-2.
            /// </summary>
            [DataMember(Name = "sha256")]
            public bool? Sha256 { get; init; }

        }

        /// <summary>
        /// For asymmetric signature verification methods (keyUsage is ‘S0’, ‘S1’, or ‘S2’), this can be one of the following values to be used.
        /// If keyUsage is specified as any of the MAC usages (i.e. ‘M1’), then properties should not be not set or both 'sha1' and 'sha256' are false.
        /// </summary>
        [DataMember(Name = "hashAlgorithm")]
        public HashAlgorithmClass HashAlgorithm { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(int? KeyNum = null, IdKeyClass IdKey = null, KeyCheckModesClass KeyCheckModes = null, string HsmVendor = null, RsaAuthenticationSchemeClass RsaAuthenticationScheme = null, RsaSignatureAlgorithmClass RsaSignatureAlgorithm = null, RsaCryptAlgorithmClass RsaCryptAlgorithm = null, RsaKeyCheckModeClass RsaKeyCheckMode = null, SignatureSchemeClass SignatureScheme = null, EmvImportSchemesClass EmvImportSchemes = null, KeyBlockImportFormatsClass KeyBlockImportFormats = null, bool? KeyImportThroughParts = null, DesKeyLengthClass DesKeyLength = null, CertificateTypesClass CertificateTypes = null, List<LoadCertOptionsClass> LoadCertOptions = null, CrklLoadOptionsClass CrklLoadOptions = null, List<RestrictedKeyEncKeySupportClass> RestrictedKeyEncKeySupport = null, SymmetricKeyManagementMethodsClass SymmetricKeyManagementMethods = null, List<KeyAttributeClass> KeyAttributes = null, List<DecryptAttributeClass> DecryptAttributes = null, List<VerifyAttributeClass> VerifyAttributes = null)
        {
            this.KeyNum = KeyNum;
            this.IdKey = IdKey;
            this.KeyCheckModes = KeyCheckModes;
            this.HsmVendor = HsmVendor;
            this.RsaAuthenticationScheme = RsaAuthenticationScheme;
            this.RsaSignatureAlgorithm = RsaSignatureAlgorithm;
            this.RsaCryptAlgorithm = RsaCryptAlgorithm;
            this.RsaKeyCheckMode = RsaKeyCheckMode;
            this.SignatureScheme = SignatureScheme;
            this.EmvImportSchemes = EmvImportSchemes;
            this.KeyBlockImportFormats = KeyBlockImportFormats;
            this.KeyImportThroughParts = KeyImportThroughParts;
            this.DesKeyLength = DesKeyLength;
            this.CertificateTypes = CertificateTypes;
            this.LoadCertOptions = LoadCertOptions;
            this.CrklLoadOptions = CrklLoadOptions;
            this.RestrictedKeyEncKeySupport = RestrictedKeyEncKeySupport;
            this.SymmetricKeyManagementMethods = SymmetricKeyManagementMethods;
            this.KeyAttributes = KeyAttributes;
            this.DecryptAttributes = DecryptAttributes;
            this.VerifyAttributes = VerifyAttributes;
        }

        /// <summary>
        /// Number of the keys which can be stored in the encryption/decryption module.
        /// </summary>
        [DataMember(Name = "keyNum")]
        public int? KeyNum { get; init; }

        [DataContract]
        public sealed class IdKeyClass
        {
            public IdKeyClass(bool? Initialization = null, bool? Import = null)
            {
                this.Initialization = Initialization;
                this.Import = Import;
            }

            /// <summary>
            /// ID key is returned by the [KeyManagement.Initialization](#keymanagement.initialization) command.
            /// </summary>
            [DataMember(Name = "initialization")]
            public bool? Initialization { get; init; }

            /// <summary>
            /// ID key is required as input for the [KeyManagement.ImportKey](#keymanagement.importkey) and 
            /// [KeyManagement.DeriveKey](#keymanagement.derivekey) command.
            /// </summary>
            [DataMember(Name = "import")]
            public bool? Import { get; init; }

        }

        /// <summary>
        /// Specifies if key owner identification (in commands referenced as lpxIdent), which authorizes access 
        /// to the encryption module, is required.  A zero value is returned if the encryption module does not 
        /// support this capability.
        /// </summary>
        [DataMember(Name = "idKey")]
        public IdKeyClass IdKey { get; init; }

        [DataContract]
        public sealed class KeyCheckModesClass
        {
            public KeyCheckModesClass(bool? Self = null, bool? Zero = null)
            {
                this.Self = Self;
                this.Zero = Zero;
            }

            /// <summary>
            /// The key check value is created by an encryption of the key with itself.
            /// For a double-length or triple-length key the kcv is generated using 3DES encryption using the first 
            /// 8 bytes of the key as the source data for the encryption.
            /// </summary>
            [DataMember(Name = "self")]
            public bool? Self { get; init; }

            /// <summary>
            /// The key check value is created by encrypting a zero value with the key.
            /// </summary>
            [DataMember(Name = "zero")]
            public bool? Zero { get; init; }

        }

        /// <summary>
        /// Specifies the key check modes that are supported to check the correctness of an imported key value.
        /// </summary>
        [DataMember(Name = "keyCheckModes")]
        public KeyCheckModesClass KeyCheckModes { get; init; }

        /// <summary>
        /// Identifies the hsm Vendor. 
        /// hsmVendor is an empty string or this property is not set when the hsm Vendor is unknown or the HSM is not supported.
        /// </summary>
        [DataMember(Name = "hsmVendor")]
        public string HsmVendor { get; init; }

        [DataContract]
        public sealed class RsaAuthenticationSchemeClass
        {
            public RsaAuthenticationSchemeClass(bool? Number2partySig = null, bool? Number3partyCert = null, bool? Number3partyCertTr34 = null)
            {
                this.Number2partySig = Number2partySig;
                this.Number3partyCert = Number3partyCert;
                this.Number3partyCertTr34 = Number3partyCertTr34;
            }

            /// <summary>
            /// Two-party Signature based authentication.
            /// </summary>
            [DataMember(Name = "2partySig")]
            public bool? Number2partySig { get; init; }

            /// <summary>
            /// Three-party Certificate based authentication.
            /// </summary>
            [DataMember(Name = "3partyCert")]
            public bool? Number3partyCert { get; init; }

            /// <summary>
            /// Three-party Certificate based authentication described by X9 TR34-2012.
            /// </summary>
            [DataMember(Name = "3partyCertTr34")]
            public bool? Number3partyCertTr34 { get; init; }

        }

        /// <summary>
        /// Specifies which type of Remote Key Loading/Authentication.
        /// </summary>
        [DataMember(Name = "rsaAuthenticationScheme")]
        public RsaAuthenticationSchemeClass RsaAuthenticationScheme { get; init; }

        [DataContract]
        public sealed class RsaSignatureAlgorithmClass
        {
            public RsaSignatureAlgorithmClass(bool? Pkcs1V15 = null, bool? Pss = null)
            {
                this.Pkcs1V15 = Pkcs1V15;
                this.Pss = Pss;
            }

            /// <summary>
            /// pkcs1V15 Signatures supported.
            /// </summary>
            [DataMember(Name = "pkcs1V15")]
            public bool? Pkcs1V15 { get; init; }

            /// <summary>
            /// pss Signatures supported.
            /// </summary>
            [DataMember(Name = "pss")]
            public bool? Pss { get; init; }

        }

        /// <summary>
        /// Specifies which type of RSA Signature Algorithm.
        /// </summary>
        [DataMember(Name = "rsaSignatureAlgorithm")]
        public RsaSignatureAlgorithmClass RsaSignatureAlgorithm { get; init; }

        [DataContract]
        public sealed class RsaCryptAlgorithmClass
        {
            public RsaCryptAlgorithmClass(bool? Pkcs1V15 = null, bool? Oaep = null)
            {
                this.Pkcs1V15 = Pkcs1V15;
                this.Oaep = Oaep;
            }

            /// <summary>
            /// pkcs1V15 algorithm supported.
            /// </summary>
            [DataMember(Name = "pkcs1V15")]
            public bool? Pkcs1V15 { get; init; }

            /// <summary>
            /// oaep algorithm supported.
            /// </summary>
            [DataMember(Name = "oaep")]
            public bool? Oaep { get; init; }

        }

        /// <summary>
        /// Specifies which type of RSA Encipherment Algorithm.
        /// </summary>
        [DataMember(Name = "rsaCryptAlgorithm")]
        public RsaCryptAlgorithmClass RsaCryptAlgorithm { get; init; }

        [DataContract]
        public sealed class RsaKeyCheckModeClass
        {
            public RsaKeyCheckModeClass(bool? Sha1 = null, bool? Sha256 = null)
            {
                this.Sha1 = Sha1;
                this.Sha256 = Sha256;
            }

            /// <summary>
            /// sha1 is supported as defined in Ref. 3.
            /// </summary>
            [DataMember(Name = "sha1")]
            public bool? Sha1 { get; init; }

            /// <summary>
            /// sha256 is supported as defined in ISO/IEC 10118-3:2004 and FIPS 180-2.
            /// </summary>
            [DataMember(Name = "sha256")]
            public bool? Sha256 { get; init; }

        }

        /// <summary>
        /// Specifies which algorithm/method used to generate the public key check value/thumb print.
        /// </summary>
        [DataMember(Name = "rsaKeyCheckMode")]
        public RsaKeyCheckModeClass RsaKeyCheckMode { get; init; }

        [DataContract]
        public sealed class SignatureSchemeClass
        {
            public SignatureSchemeClass(bool? GenRsaKeyPair = null, bool? RandomNumber = null, bool? ExportEppId = null, bool? EnhancedRkl = null)
            {
                this.GenRsaKeyPair = GenRsaKeyPair;
                this.RandomNumber = RandomNumber;
                this.ExportEppId = ExportEppId;
                this.EnhancedRkl = EnhancedRkl;
            }

            /// <summary>
            /// Specifies if the Service Provider supports the RSA Signature Scheme 
            /// [KeyManagement.GenerateRSAKeyPair](#keymangement.generatersakeypair) 
            /// and [KeyManagement.ExportRSAEPPSignedItem](#keymangement.exportrsaeppsigneditem) commands.
            /// </summary>
            [DataMember(Name = "genRsaKeyPair")]
            public bool? GenRsaKeyPair { get; init; }

            /// <summary>
            /// Specifies if the Service Provider returns a random number from the StartKeyExchange GE 
            /// command within the RSA Signature Scheme.
            /// </summary>
            [DataMember(Name = "randomNumber")]
            public bool? RandomNumber { get; init; }

            /// <summary>
            /// Specifies if the Service Provider supports exporting the EPP Security Item within the RSA Signature
            /// Scheme.
            /// </summary>
            [DataMember(Name = "exportEppId")]
            public bool? ExportEppId { get; init; }

            /// <summary>
            /// Specifies that the Service Provider supports the Enhanced Signature Remote Key Scheme. This scheme allows 
            /// the customer to manage their own public keys independently of the Signature Issuer. When this mode is supported 
            /// then the key loaded signed with the Signature Issuer key is the host root public key PKROOT, rather than PKHOST.
            /// </summary>
            [DataMember(Name = "enhancedRkl")]
            public bool? EnhancedRkl { get; init; }

        }

        /// <summary>
        /// Specifies which capabilities are supported by the Signature scheme.
        /// </summary>
        [DataMember(Name = "signatureScheme")]
        public SignatureSchemeClass SignatureScheme { get; init; }

        [DataContract]
        public sealed class EmvImportSchemesClass
        {
            public EmvImportSchemesClass(bool? PlainCA = null, bool? ChksumCA = null, bool? EpiCA = null, bool? Issuer = null, bool? Icc = null, bool? IccPin = null, bool? Pkcsv15CA = null)
            {
                this.PlainCA = PlainCA;
                this.ChksumCA = ChksumCA;
                this.EpiCA = EpiCA;
                this.Issuer = Issuer;
                this.Icc = Icc;
                this.IccPin = IccPin;
                this.Pkcsv15CA = Pkcsv15CA;
            }

            /// <summary>
            /// A plain text CA public key is imported with no verification.
            /// </summary>
            [DataMember(Name = "plainCA")]
            public bool? PlainCA { get; init; }

            /// <summary>
            /// A plain text CA public key is imported using the EMV 2000 verification algorithm.
            /// </summary>
            [DataMember(Name = "chksumCA")]
            public bool? ChksumCA { get; init; }

            /// <summary>
            /// A CA public key is imported using the selfsign scheme defined in the Europay International, epi CA Module 
            /// Technical - Interface specification.\
            /// </summary>
            [DataMember(Name = "epiCA")]
            public bool? EpiCA { get; init; }

            /// <summary>
            /// An Issuer public key is imported as defined in EMV 2000 Book II.
            /// </summary>
            [DataMember(Name = "issuer")]
            public bool? Issuer { get; init; }

            /// <summary>
            /// An ICC public key is imported as defined in EMV 2000 Book II.
            /// </summary>
            [DataMember(Name = "icc")]
            public bool? Icc { get; init; }

            /// <summary>
            /// An ICC PIN public key is imported as defined in EMV 2000 Book II.
            /// </summary>
            [DataMember(Name = "iccPin")]
            public bool? IccPin { get; init; }

            /// <summary>
            /// A CA public key is imported and verified using a signature generated with a private key for 
            /// which the public key is already loaded..
            /// </summary>
            [DataMember(Name = "pkcsv15CA")]
            public bool? Pkcsv15CA { get; init; }

        }

        /// <summary>
        /// Identifies the supported emv Import Scheme(s).
        /// </summary>
        [DataMember(Name = "emvImportSchemes")]
        public EmvImportSchemesClass EmvImportSchemes { get; init; }

        [DataContract]
        public sealed class KeyBlockImportFormatsClass
        {
            public KeyBlockImportFormatsClass(bool? AnsTr31KeyBlock = null, bool? AnsTr31KeyBlockB = null, bool? AnsTr31KeyBlockC = null)
            {
                this.AnsTr31KeyBlock = AnsTr31KeyBlock;
                this.AnsTr31KeyBlockB = AnsTr31KeyBlockB;
                this.AnsTr31KeyBlockC = AnsTr31KeyBlockC;
            }

            /// <summary>
            /// Supports ANS TR-31A Keyblock format key import.
            /// </summary>
            [DataMember(Name = "ansTr31KeyBlock")]
            public bool? AnsTr31KeyBlock { get; init; }

            /// <summary>
            /// Supports ANS TR-31B Keyblock format key import.
            /// </summary>
            [DataMember(Name = "ansTr31KeyBlockB")]
            public bool? AnsTr31KeyBlockB { get; init; }

            /// <summary>
            /// Supports ANS TR-31C Keyblock format key import.
            /// </summary>
            [DataMember(Name = "ansTr31KeyBlockC")]
            public bool? AnsTr31KeyBlockC { get; init; }

        }

        /// <summary>
        /// Supported key block formats.
        /// </summary>
        [DataMember(Name = "keyBlockImportFormats")]
        public KeyBlockImportFormatsClass KeyBlockImportFormats { get; init; }

        /// <summary>
        /// Specifies whether the device is capable of importing keys in multiple parts. TRUE means the 
        /// device supports the key import in multiple parts.
        /// </summary>
        [DataMember(Name = "keyImportThroughParts")]
        public bool? KeyImportThroughParts { get; init; }

        [DataContract]
        public sealed class DesKeyLengthClass
        {
            public DesKeyLengthClass(bool? Single = null, bool? Double = null, bool? Triple = null)
            {
                this.Single = Single;
                this.Double = Double;
                this.Triple = Triple;
            }

            /// <summary>
            /// 8 byte DES keys are supported.
            /// </summary>
            [DataMember(Name = "single")]
            public bool? Single { get; init; }

            /// <summary>
            /// 16 byte DES keys are supported.
            /// </summary>
            [DataMember(Name = "double")]
            public bool? Double { get; init; }

            /// <summary>
            /// 24 byte DES keys are supported.
            /// </summary>
            [DataMember(Name = "triple")]
            public bool? Triple { get; init; }

        }

        /// <summary>
        /// Specifies which length of DES keys are supported.
        /// </summary>
        [DataMember(Name = "desKeyLength")]
        public DesKeyLengthClass DesKeyLength { get; init; }

        [DataContract]
        public sealed class CertificateTypesClass
        {
            public CertificateTypesClass(bool? EncKey = null, bool? VerificationKey = null, bool? HostKey = null)
            {
                this.EncKey = EncKey;
                this.VerificationKey = VerificationKey;
                this.HostKey = HostKey;
            }

            /// <summary>
            /// Supports the EPP public encryption certificate.
            /// </summary>
            [DataMember(Name = "encKey")]
            public bool? EncKey { get; init; }

            /// <summary>
            /// Supports the EPP public verification certificate.
            /// </summary>
            [DataMember(Name = "verificationKey")]
            public bool? VerificationKey { get; init; }

            /// <summary>
            /// Supports the Host public certificate.
            /// </summary>
            [DataMember(Name = "hostKey")]
            public bool? HostKey { get; init; }

        }

        /// <summary>
        /// Specifies supported certificate types.
        /// </summary>
        [DataMember(Name = "certificateTypes")]
        public CertificateTypesClass CertificateTypes { get; init; }

        [DataContract]
        public sealed class LoadCertOptionsClass
        {
            public LoadCertOptionsClass(SignerEnum? Signer = null, OptionClass Option = null)
            {
                this.Signer = Signer;
                this.Option = Option;
            }

            public enum SignerEnum
            {
                CertHost,
                SigHost,
                Ca,
                Hl,
                CertHostTr34,
                CaTr34,
                HlTr34
            }

            /// <summary>
            /// Specifies the signers supported by the [KeyManagement.LoadCertificate](#keymanagement.loadcertificate) command.
            /// The possible variables are:
            /// 
            /// * ```certHost``` - The current Host RSA Private Key is used to sign the token.
            /// * ```sigHost``` - The current Host RSA Private Key is used to sign the token, signature format is used.
            /// * ```hl``` - A Higher-Level Authority RSA Private Key is used to sign the token.
            /// * ```certHostTr34``` - The current Host RSA Private Key is used to sign the token, compliant with X9 TR34-2012.
            /// * ```caTr34``` - The Certificate Authority RSA Private Key is used to sign the token, compliant with X9 TR34-2012.
            /// * ```hlTr34``` - A Higher-Level Authority RSA Private Key is used to sign the token, compliant with X9 TR34-2012.\
            /// </summary>
            [DataMember(Name = "signer")]
            public SignerEnum? Signer { get; init; }

            [DataContract]
            public sealed class OptionClass
            {
                public OptionClass(bool? NewHost = null, bool? ReplaceHost = null)
                {
                    this.NewHost = NewHost;
                    this.ReplaceHost = ReplaceHost;
                }

                /// <summary>
                /// Load a new Host certificate, where one has not already been loaded.
                /// </summary>
                [DataMember(Name = "newHost")]
                public bool? NewHost { get; init; }

                /// <summary>
                /// Replace the epp to a new Host certificate, where the new Host certificate is signed by signer.
                /// </summary>
                [DataMember(Name = "replaceHost")]
                public bool? ReplaceHost { get; init; }

            }

            /// <summary>
            /// Specifies the load options supported by the [KeyManagement.LoadCertificate](#keymanagement.loadcertificate) command.
            /// </summary>
            [DataMember(Name = "option")]
            public OptionClass Option { get; init; }

        }

        /// <summary>
        /// Specifying the options supported by the [KeyManagement.LoadCertificate](#keymanagement.loadcertificate) command.
        /// </summary>
        [DataMember(Name = "loadCertOptions")]
        public List<LoadCertOptionsClass> LoadCertOptions { get; init; }

        [DataContract]
        public sealed class CrklLoadOptionsClass
        {
            public CrklLoadOptionsClass(bool? NoRandom = null, bool? NoRandomCrl = null, bool? Random = null, bool? RandomCrl = null)
            {
                this.NoRandom = NoRandom;
                this.NoRandomCrl = NoRandomCrl;
                this.Random = Random;
                this.RandomCrl = RandomCrl;
            }

            /// <summary>
            /// Import a Key Transport Key without generating and using a random number.
            /// </summary>
            [DataMember(Name = "noRandom")]
            public bool? NoRandom { get; init; }

            /// <summary>
            /// Import a Key Transport Key with a Certificate Revocation List appended to the input message.
            /// A random number is not generated nor used.
            /// </summary>
            [DataMember(Name = "noRandomCrl")]
            public bool? NoRandomCrl { get; init; }

            /// <summary>
            /// Import a Key Transport Key by generating and using a random number.
            /// </summary>
            [DataMember(Name = "random")]
            public bool? Random { get; init; }

            /// <summary>
            /// Import a Key Transport Key with a Certificate Revocation List appended to the input parameter.
            /// A random number is generated and used.
            /// </summary>
            [DataMember(Name = "randomCrl")]
            public bool? RandomCrl { get; init; }

        }

        /// <summary>
        /// Supported options to load the Key Transport Key using the Certificate Remote Key Loading protocol.
        /// </summary>
        [DataMember(Name = "crklLoadOptions")]
        public CrklLoadOptionsClass CrklLoadOptions { get; init; }

        [DataContract]
        public sealed class RestrictedKeyEncKeySupportClass
        {
            public RestrictedKeyEncKeySupportClass(LoadingMethodEnum? LoadingMethod = null, UsesClass Uses = null)
            {
                this.LoadingMethod = LoadingMethod;
                this.Uses = Uses;
            }

            public enum LoadingMethodEnum
            {
                RsaAuth2partySig,
                RsaAuth3partyCert,
                RsaAuth3partyCertTr34,
                RestrictedSecurekeyentry
            }

            /// <summary>
            /// Specifies the loading methods supported.
            /// The possible variables are:
            /// 
            /// * ```rsaAuth2partySig``` - Two-party Signature based.
            /// * ```rsaAuth3partyCert``` - Three-party Certificate based.
            /// * ```rsaAuth3partyCertTr34``` - Three- party Certificate based TR34.
            /// * ```restrictedSecurekeyentry``` - Restricted secure key entry.
            /// </summary>
            [DataMember(Name = "loadingMethod")]
            public LoadingMethodEnum? LoadingMethod { get; init; }

            [DataContract]
            public sealed class UsesClass
            {
                public UsesClass(bool? Crypt = null, bool? Function = null, bool? Macing = null, bool? Pinlocal = null, bool? Svenckey = null, bool? PinRemote = null)
                {
                    this.Crypt = Crypt;
                    this.Function = Function;
                    this.Macing = Macing;
                    this.Pinlocal = Pinlocal;
                    this.Svenckey = Svenckey;
                    this.PinRemote = PinRemote;
                }

                /// <summary>
                /// Key isused for encryption and decryption.
                /// </summary>
                [DataMember(Name = "crypt")]
                public bool? Crypt { get; init; }

                /// <summary>
                /// Key is used for Pin block creation.
                /// </summary>
                [DataMember(Name = "function")]
                public bool? Function { get; init; }

                /// <summary>
                /// Key is using for macing.
                /// </summary>
                [DataMember(Name = "macing")]
                public bool? Macing { get; init; }

                /// <summary>
                /// Key is used only for local PIN check.
                /// </summary>
                [DataMember(Name = "pinlocal")]
                public bool? Pinlocal { get; init; }

                /// <summary>
                /// Key is used as cbc start Value encryption key.
                /// </summary>
                [DataMember(Name = "svenckey")]
                public bool? Svenckey { get; init; }

                /// <summary>
                /// Key is used only for PIN block creation.
                /// </summary>
                [DataMember(Name = "pinRemote")]
                public bool? PinRemote { get; init; }

            }

            /// <summary>
            /// Specifies one or more usage flags that can be used in combination with the RestrictedKeyEncKey.
            /// </summary>
            [DataMember(Name = "uses")]
            public UsesClass Uses { get; init; }

        }

        /// <summary>
        /// A array of object specifying the loading methods that support the RestrictedKeyEncKey usage flag 
        /// and the allowable usage flag combinations.
        /// </summary>
        [DataMember(Name = "restrictedKeyEncKeySupport")]
        public List<RestrictedKeyEncKeySupportClass> RestrictedKeyEncKeySupport { get; init; }

        [DataContract]
        public sealed class SymmetricKeyManagementMethodsClass
        {
            public SymmetricKeyManagementMethodsClass(bool? FixedKey = null, bool? MasterKey = null, bool? TdesDukpt = null)
            {
                this.FixedKey = FixedKey;
                this.MasterKey = MasterKey;
                this.TdesDukpt = TdesDukpt;
            }

            /// <summary>
            /// This method of key management uses fixed keys for transaction processing.
            /// </summary>
            [DataMember(Name = "fixedKey")]
            public bool? FixedKey { get; init; }

            /// <summary>
            /// This method uses a hierarchy of Key Encrypting Keys and Transaction Keys. The highest level of 
            /// Key Encrypting Key is known as a Master Key.
            /// Transaction Keys are distributed and replaced encrypted under a Key Encrypting Key.
            /// </summary>
            [DataMember(Name = "masterKey")]
            public bool? MasterKey { get; init; }

            /// <summary>
            /// This method uses TDES Derived Unique Key Per Transaction (see reference 45).
            /// </summary>
            [DataMember(Name = "tdesDukpt")]
            public bool? TdesDukpt { get; init; }

        }

        /// <summary>
        /// Specifies the Symmentric Key Management modes.
        /// </summary>
        [DataMember(Name = "symmetricKeyManagementMethods")]
        public SymmetricKeyManagementMethodsClass SymmetricKeyManagementMethods { get; init; }

        /// <summary>
        /// Array of attributes supported by [KeyManagement.ImportKey](#keymanagement.importkey) command for the key to be loaded.
        /// </summary>
        [DataMember(Name = "keyAttributes")]
        public List<KeyAttributeClass> KeyAttributes { get; init; }

        /// <summary>
        /// Array of attributes supported by the Import command for the key used to decrypt or unwrap the key being imported.
        /// </summary>
        [DataMember(Name = "decryptAttributes")]
        public List<DecryptAttributeClass> DecryptAttributes { get; init; }

        /// <summary>
        /// Array of attributes supported by Import command for the key used for verification before importing the key."    
        /// </summary>
        [DataMember(Name = "verifyAttributes")]
        public List<VerifyAttributeClass> VerifyAttributes { get; init; }

    }


    public enum TypeDataItemToExportEnum
    {
        EppId,
        PublicKey
    }


    public enum RSASignatureAlgorithmEnum
    {
        Na,
        RsassaPkcs1V15,
        RsassaPss
    }


    [DataContract]
    public sealed class SignersClass
    {
        public SignersClass(bool? None = null, bool? Certhost = null, bool? Sighost = null, bool? Ca = null, bool? Hl = null, bool? Tr34 = null, bool? Cbcmac = null, bool? Cmac = null, bool? Reserved_1 = null, bool? Reserved_2 = null, bool? Reserved_3 = null)
        {
            this.None = None;
            this.Certhost = Certhost;
            this.Sighost = Sighost;
            this.Ca = Ca;
            this.Hl = Hl;
            this.Tr34 = Tr34;
            this.Cbcmac = Cbcmac;
            this.Cmac = Cmac;
            this.Reserved_1 = Reserved_1;
            this.Reserved_2 = Reserved_2;
            this.Reserved_3 = Reserved_3;
        }

        /// <summary>
        /// Authentication is not required.
        /// </summary>
        [DataMember(Name = "none")]
        public bool? None { get; init; }

        /// <summary>
        /// The data is signed by the current Host, using the RSA certificate-based scheme.
        /// </summary>
        [DataMember(Name = "certhost")]
        public bool? Certhost { get; init; }

        /// <summary>
        /// The data is signed by the current Host, using the RSA signature-based scheme.
        /// </summary>
        [DataMember(Name = "sighost")]
        public bool? Sighost { get; init; }

        /// <summary>
        /// The data is signed by the Certificate Authority (CA).
        /// </summary>
        [DataMember(Name = "ca")]
        public bool? Ca { get; init; }

        /// <summary>
        /// The data is signed by the Higher Level (HL) Authority.
        /// </summary>
        [DataMember(Name = "hl")]
        public bool? Hl { get; init; }

        /// <summary>
        /// The format of the data that was signed complies with the data defined in X9 TR342012 [Ref. 42]. 
        /// This value can only be used in combination with the CERTHOST, CA or HL flags.
        /// </summary>
        [DataMember(Name = "tr34")]
        public bool? Tr34 { get; init; }

        /// <summary>
        /// A MAC is calculated over the data using lpsKey and the CBC MAC algorithm.
        /// </summary>
        [DataMember(Name = "cbcmac")]
        public bool? Cbcmac { get; init; }

        /// <summary>
        /// A MAC is calculated over the data using lpsKey and the CMAC algorithm.
        /// </summary>
        [DataMember(Name = "cmac")]
        public bool? Cmac { get; init; }

        /// <summary>
        /// Reserved for a vendor-defined signing method.
        /// </summary>
        [DataMember(Name = "reserved_1")]
        public bool? Reserved_1 { get; init; }

        /// <summary>
        /// Reserved for a vendor-defined signing method.
        /// </summary>
        [DataMember(Name = "reserved_2")]
        public bool? Reserved_2 { get; init; }

        /// <summary>
        /// Reserved for a vendor-defined signing method.
        /// </summary>
        [DataMember(Name = "reserved_3")]
        public bool? Reserved_3 { get; init; }

    }


}
