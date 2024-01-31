/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
        /// Specifies the state of the encryption module. This may be null in
        /// [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
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
        /// Specifies the state of the public verification or encryption key in the PIN certificate modules. This may be null in
        /// [Common.StatusChangedEvent](#common.statuschangedevent) if unchanged.
        /// </summary>
        [DataMember(Name = "certificateState")]
        public CertificateStateEnum? CertificateState { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(int? KeyNum = null, DerivationAlgorithmsClass DerivationAlgorithms = null, KeyCheckModesClass KeyCheckModes = null, string HsmVendor = null, RsaAuthenticationSchemeClass RsaAuthenticationScheme = null, RsaSignatureAlgorithmClass RsaSignatureAlgorithm = null, RsaCryptAlgorithmClass RsaCryptAlgorithm = null, RsaKeyCheckModeClass RsaKeyCheckMode = null, SignatureSchemeClass SignatureScheme = null, EmvImportSchemesClass EmvImportSchemes = null, KeyBlockImportFormatsClass KeyBlockImportFormats = null, bool? KeyImportThroughParts = null, DesKeyLengthClass DesKeyLength = null, CertificateTypesClass CertificateTypes = null, Dictionary<string, LoadCertOptionsClass> LoadCertOptions = null, CrklLoadOptionsClass CrklLoadOptions = null, SymmetricKeyManagementMethodsClass SymmetricKeyManagementMethods = null, Dictionary<string, Dictionary<string, Dictionary<string, KeyAttributesClass>>> KeyAttributes = null, Dictionary<string, DecryptAttributesClass> DecryptAttributes = null, Dictionary<string, Dictionary<string, Dictionary<string, VerifyAttributesClass>>> VerifyAttributes = null)
        {
            this.KeyNum = KeyNum;
            this.DerivationAlgorithms = DerivationAlgorithms;
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
            this.SymmetricKeyManagementMethods = SymmetricKeyManagementMethods;
            this.KeyAttributes = KeyAttributes;
            this.DecryptAttributes = DecryptAttributes;
            this.VerifyAttributes = VerifyAttributes;
        }

        /// <summary>
        /// Number of the keys which can be stored in the encryption/decryption module.
        /// </summary>
        [DataMember(Name = "keyNum")]
        [DataTypes(Minimum = 0)]
        public int? KeyNum { get; init; }

        [DataContract]
        public sealed class DerivationAlgorithmsClass
        {
            public DerivationAlgorithmsClass(bool? ChipZka = null)
            {
                this.ChipZka = ChipZka;
            }

            /// <summary>
            /// Algorithm for the derivation of a chip card individual key as described by the German ZKA.
            /// </summary>
            [DataMember(Name = "chipZka")]
            public bool? ChipZka { get; init; }

        }

        /// <summary>
        /// Supported derivation algorithms. This property is null if not supported.
        /// </summary>
        [DataMember(Name = "derivationAlgorithms")]
        public DerivationAlgorithmsClass DerivationAlgorithms { get; init; }

        [DataContract]
        public sealed class KeyCheckModesClass
        {
            public KeyCheckModesClass(bool? Self = null, bool? Zero = null)
            {
                this.Self = Self;
                this.Zero = Zero;
            }

            /// <summary>
            /// The key check value is created by an encryption of the key with itself. For a double-length or
            /// triple-length key the KCV is generated using 3DES encryption using the first 8 bytes of the key as the
            /// source data for the encryption.
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
        /// The modes available for each key may depend on security requirements of the algorithm.
        /// The algorithm (i.e. DES, 3DES, AES) and use is determined by the algorithm of the key being checked and security requirements.
        /// This property is null if not supported.
        /// </summary>
        [DataMember(Name = "keyCheckModes")]
        public KeyCheckModesClass KeyCheckModes { get; init; }

        /// <summary>
        /// Identifies the Hardware Security Module (HSM) Vendor.
        /// 
        /// This should be null if not supported or the HSM vendor is unknown.
        /// <example>HSM Vendor</example>
        /// </summary>
        [DataMember(Name = "hsmVendor")]
        public string HsmVendor { get; init; }

        [DataContract]
        public sealed class RsaAuthenticationSchemeClass
        {
            public RsaAuthenticationSchemeClass(bool? TwoPartySig = null, bool? ThreePartyCert = null, bool? ThreePartyCertTr34 = null)
            {
                this.TwoPartySig = TwoPartySig;
                this.ThreePartyCert = ThreePartyCert;
                this.ThreePartyCertTr34 = ThreePartyCertTr34;
            }

            /// <summary>
            /// Two-party Signature based authentication.
            /// </summary>
            [DataMember(Name = "twoPartySig")]
            public bool? TwoPartySig { get; init; }

            /// <summary>
            /// Three-party Certificate based authentication.
            /// </summary>
            [DataMember(Name = "threePartyCert")]
            public bool? ThreePartyCert { get; init; }

            /// <summary>
            /// Three-party Certificate based authentication described by X9 TR34-2019
            /// [[Ref. keymanagement-9](#ref-keymanagement-9)].
            /// </summary>
            [DataMember(Name = "threePartyCertTr34")]
            public bool? ThreePartyCertTr34 { get; init; }

        }

        /// <summary>
        /// Specifies the types of Remote Key Loading/Authentication that are supported. This property is null if not supported.
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
        /// Specifies the types of RSA Signature Algorithm that are supported.
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
        /// Specifies the types of RSA Encipherment Algorithm that are supported. This property is null if not supported.
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
            /// sha1 is supported as defined in [[Ref. keymanagement-2](#ref-keymanagement-2)].
            /// </summary>
            [DataMember(Name = "sha1")]
            public bool? Sha1 { get; init; }

            /// <summary>
            /// sha256 is supported as defined in ISO/IEC 10118-3:2004 [[Ref. keymanagement-7](#ref-keymanagement-7)]
            /// and FIPS 180-2 [[Ref. keymanagement-8](#ref-keymanagement-8)].
            /// </summary>
            [DataMember(Name = "sha256")]
            public bool? Sha256 { get; init; }

        }

        /// <summary>
        /// Specifies which hash algorithms used to generate the public key check value/thumb print are supported.
        /// This property is null if not supported.
        /// </summary>
        [DataMember(Name = "rsaKeyCheckMode")]
        public RsaKeyCheckModeClass RsaKeyCheckMode { get; init; }

        [DataContract]
        public sealed class SignatureSchemeClass
        {
            public SignatureSchemeClass(bool? RandomNumber = null, bool? ExportDeviceId = null, bool? EnhancedRkl = null)
            {
                this.RandomNumber = RandomNumber;
                this.ExportDeviceId = ExportDeviceId;
                this.EnhancedRkl = EnhancedRkl;
            }

            /// <summary>
            /// Specifies if the service returns a random number from the
            /// [KeyManagement.StartKeyExchange](#keymanagement.startkeyexchange) command within the RSA Signature
            /// Scheme.
            /// </summary>
            [DataMember(Name = "randomNumber")]
            public bool? RandomNumber { get; init; }

            /// <summary>
            /// Specifies if the service supports exporting the device Security Item within the RSA Signature
            /// Scheme.
            /// </summary>
            [DataMember(Name = "exportDeviceId")]
            public bool? ExportDeviceId { get; init; }

            /// <summary>
            /// Specifies that the service supports the Enhanced Signature Remote Key Scheme. This scheme
            /// allows the customer to manage their own public keys independently of the Signature Issuer. When this
            /// mode is supported then the key loaded signed with the Signature Issuer key is the host root public key
            /// PK&lt;sub&gt;ROOT&lt;/sub&gt;, rather than PK&lt;sub&gt;HOST&lt;/sub&gt;.
            /// </summary>
            [DataMember(Name = "enhancedRkl")]
            public bool? EnhancedRkl { get; init; }

        }

        /// <summary>
        /// Specifies which capabilities are supported by the Signature scheme. This property is null if not supported.
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
            /// A plain text CA public key is imported using the EMV 2000 verification algorithm. See
            /// [[Ref. keymanagement-3](#ref-keymanagement-3)].
            /// </summary>
            [DataMember(Name = "chksumCA")]
            public bool? ChksumCA { get; init; }

            /// <summary>
            /// A CA public key is imported using the selfsign scheme defined in the Europay International, EPI CA
            /// Module Technical - Interface specification Version 1.4, [[Ref. ref-keymanagement-4](#ref-keymanagement-4)].
            /// </summary>
            [DataMember(Name = "epiCA")]
            public bool? EpiCA { get; init; }

            /// <summary>
            /// An Issuer public key is imported as defined in EMV 2000 Book II, [[Ref. keymanagement-3](#ref-keymanagement-3)].
            /// </summary>
            [DataMember(Name = "issuer")]
            public bool? Issuer { get; init; }

            /// <summary>
            /// An ICC public key is imported as defined in EMV 2000 Book II, [[Ref. keymanagement-3](#ref-keymanagement-3)].
            /// </summary>
            [DataMember(Name = "icc")]
            public bool? Icc { get; init; }

            /// <summary>
            /// An ICC PIN public key is imported as defined in EMV 2000 Book II, [[Ref. keymanagement-3](#ref-keymanagement-3)].
            /// </summary>
            [DataMember(Name = "iccPin")]
            public bool? IccPin { get; init; }

            /// <summary>
            /// A CA public key is imported and verified using a signature generated with a private key for which the
            /// public key is already loaded..
            /// </summary>
            [DataMember(Name = "pkcsv15CA")]
            public bool? Pkcsv15CA { get; init; }

        }

        /// <summary>
        /// Identifies the supported EMV Import Scheme(s). This property is null if not supported.
        /// </summary>
        [DataMember(Name = "emvImportSchemes")]
        public EmvImportSchemesClass EmvImportSchemes { get; init; }

        [DataContract]
        public sealed class KeyBlockImportFormatsClass
        {
            public KeyBlockImportFormatsClass(bool? A = null, bool? B = null, bool? C = null, bool? D = null)
            {
                this.A = A;
                this.B = B;
                this.C = C;
                this.D = D;
            }

            /// <summary>
            /// Supports X9.143 key block version ID A.
            /// </summary>
            [DataMember(Name = "A")]
            public bool? A { get; init; }

            /// <summary>
            /// Supports X9.143 key block version ID B.
            /// </summary>
            [DataMember(Name = "B")]
            public bool? B { get; init; }

            /// <summary>
            /// Supports X9.143 key block version ID C.
            /// </summary>
            [DataMember(Name = "C")]
            public bool? C { get; init; }

            /// <summary>
            /// Supports X9.143 key block version ID D.
            /// </summary>
            [DataMember(Name = "D")]
            public bool? D { get; init; }

        }

        /// <summary>
        /// Supported key block formats. This property is null if not supported.
        /// </summary>
        [DataMember(Name = "keyBlockImportFormats")]
        public KeyBlockImportFormatsClass KeyBlockImportFormats { get; init; }

        /// <summary>
        /// Specifies whether the device is capable of importing keys in multiple parts.
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
        /// Specifies which DES key lengths are supported. This property is null if not supported.
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
            /// Supports the device public encryption certificate.
            /// </summary>
            [DataMember(Name = "encKey")]
            public bool? EncKey { get; init; }

            /// <summary>
            /// Supports the device public verification certificate.
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
        /// Specifies supported certificate types. This property is null if not supported.
        /// </summary>
        [DataMember(Name = "certificateTypes")]
        public CertificateTypesClass CertificateTypes { get; init; }

        [DataContract]
        public sealed class LoadCertOptionsClass
        {
            public LoadCertOptionsClass(bool? NewHost = null, bool? ReplaceHost = null)
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
            /// Replace (or rebind) the device to a new Host certificate, where the new Host certificate is signed
            /// by *signer*.
            /// </summary>
            [DataMember(Name = "replaceHost")]
            public bool? ReplaceHost { get; init; }

        }

        /// <summary>
        /// Specifying the options supported by the [KeyManagement.LoadCertificate](#keymanagement.loadcertificate)
        /// command. This property is null if not supported.
        /// </summary>
        [DataMember(Name = "loadCertOptions")]
        public Dictionary<string, LoadCertOptionsClass> LoadCertOptions { get; init; }

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
        /// This property is null if not supported.
        /// </summary>
        [DataMember(Name = "crklLoadOptions")]
        public CrklLoadOptionsClass CrklLoadOptions { get; init; }

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
            /// This method uses TDES Derived Unique Key Per Transaction (see [[Ref. keymanagement-10](#ref-keymanagement-10)]).
            /// </summary>
            [DataMember(Name = "tdesDukpt")]
            public bool? TdesDukpt { get; init; }

        }

        /// <summary>
        /// Specifies the Symmentric Key Management modes. This property is null if not supported.
        /// </summary>
        [DataMember(Name = "symmetricKeyManagementMethods")]
        public SymmetricKeyManagementMethodsClass SymmetricKeyManagementMethods { get; init; }

        [DataContract]
        public sealed class KeyAttributesClass
        {
            public KeyAttributesClass(string RestrictedKeyUsage = null)
            {
                this.RestrictedKeyUsage = RestrictedKeyUsage;
            }

            /// <summary>
            /// If the key usage is a key encryption usage (e.g. 'K0') this specifies the key usage of the keys
            /// that can be encrypted by the key.
            /// 
            /// This property should be null if restricted key usage is not supported or required.
            /// 
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
            /// </summary>
            [DataMember(Name = "restrictedKeyUsage")]
            [DataTypes(Pattern = @"^B[0-3]$|^C0$|^D[0-3]$|^E[0-7]$|^I0$|^K[0-4]$|^M[0-8]$|^P[0-1]$|^S[0-2]$|^V[0-5]$|^[0-9][0-9]$")]
            public string RestrictedKeyUsage { get; init; }

        }

        /// <summary>
        /// Attributes supported by [KeyManagement.ImportKey](#keymanagement.importkey) command for
        /// the key to be loaded.
        /// </summary>
        [DataMember(Name = "keyAttributes")]
        public Dictionary<string, Dictionary<string, Dictionary<string, KeyAttributesClass>>> KeyAttributes { get; init; }

        [DataContract]
        public sealed class DecryptAttributesClass
        {
            public DecryptAttributesClass(DecryptMethodClass DecryptMethod = null)
            {
                this.DecryptMethod = DecryptMethod;
            }

            [DataContract]
            public sealed class DecryptMethodClass
            {
                public DecryptMethodClass(bool? Ecb = null, bool? Cbc = null, bool? Cfb = null, bool? Ofb = null, bool? Ctr = null, bool? Xts = null, bool? RsaesPkcs1V15 = null, bool? RsaesOaep = null)
                {
                    this.Ecb = Ecb;
                    this.Cbc = Cbc;
                    this.Cfb = Cfb;
                    this.Ofb = Ofb;
                    this.Ctr = Ctr;
                    this.Xts = Xts;
                    this.RsaesPkcs1V15 = RsaesPkcs1V15;
                    this.RsaesOaep = RsaesOaep;
                }

                /// <summary>
                /// The ECB encryption method is supported.
                /// </summary>
                [DataMember(Name = "ecb")]
                public bool? Ecb { get; init; }

                /// <summary>
                /// The CBC encryption method is supported.
                /// </summary>
                [DataMember(Name = "cbc")]
                public bool? Cbc { get; init; }

                /// <summary>
                /// The CFB encryption method is supported.
                /// </summary>
                [DataMember(Name = "cfb")]
                public bool? Cfb { get; init; }

                /// <summary>
                /// The OFB encryption method is supported.
                /// </summary>
                [DataMember(Name = "ofb")]
                public bool? Ofb { get; init; }

                /// <summary>
                /// The CTR method is supported and defined in NIST SP800-38A (See [[Ref. 11](#ref-keymanagement-11)]).
                /// </summary>
                [DataMember(Name = "ctr")]
                public bool? Ctr { get; init; }

                /// <summary>
                /// The XTS method is supported and defined in NIST SP800-38E (See [[Ref. keymanagement-12](#ref-keymanagement-12)]).
                /// </summary>
                [DataMember(Name = "xts")]
                public bool? Xts { get; init; }

                /// <summary>
                /// The RSAES-PKCS1-v1.5 algorithm is supported.
                /// </summary>
                [DataMember(Name = "rsaesPkcs1V15")]
                public bool? RsaesPkcs1V15 { get; init; }

                /// <summary>
                /// The RSAES-OAEP algorithm is supported.
                /// </summary>
                [DataMember(Name = "rsaesOaep")]
                public bool? RsaesOaep { get; init; }

            }

            /// <summary>
            /// Specifies the cryptographic method supported.
            /// 
            /// If the algorithm is 'A', 'D', or 'T', then one or more of the following properties must be true.
            /// 
            /// * ```ecb``` - The ECB encryption method.
            /// * ```cbc``` - The CBC encryption method.
            /// * ```cfb``` - The CFB encryption method.
            /// * ```ofb``` - The OFB encryption method.
            /// * ```ctr``` - The CTR method defined in NIST SP800-38A (See [[Ref. keymanagement-11](#ref-keymanagement-11)]).
            /// * ```xts``` - The XTS method defined in NIST SP800-38E (See [[Ref. keymanagement-12](#ref-keymanagement-12)]).
            /// 
            /// If the algorithm is 'R' then one or more of the following properties must be true.
            /// 
            /// * ```rsaesPkcs1V15``` - Use the RSAES_PKCS1-v1.5 algorithm.
            /// * ```rsaesOaep``` - Use the RSAES OAEP algorithm.
            /// </summary>
            [DataMember(Name = "decryptMethod")]
            public DecryptMethodClass DecryptMethod { get; init; }

        }

        /// <summary>
        /// Attributes supported by the [KeyManagement.ImportKey](#keymanagement.importkey) command
        /// for the key used to decrypt or unwrap the key being imported.
        /// </summary>
        [DataMember(Name = "decryptAttributes")]
        public Dictionary<string, DecryptAttributesClass> DecryptAttributes { get; init; }

        [DataContract]
        public sealed class VerifyAttributesClass
        {
            public VerifyAttributesClass(CryptoMethodClass CryptoMethod = null, HashAlgorithmClass HashAlgorithm = null)
            {
                this.CryptoMethod = CryptoMethod;
                this.HashAlgorithm = HashAlgorithm;
            }

            [DataContract]
            public sealed class CryptoMethodClass
            {
                public CryptoMethodClass(bool? KcvNone = null, bool? KcvSelf = null, bool? KcvZero = null, bool? SigNone = null, bool? RsassaPkcs1V15 = null, bool? RsassaPss = null)
                {
                    this.KcvNone = KcvNone;
                    this.KcvSelf = KcvSelf;
                    this.KcvZero = KcvZero;
                    this.SigNone = SigNone;
                    this.RsassaPkcs1V15 = RsassaPkcs1V15;
                    this.RsassaPss = RsassaPss;
                }

                /// <summary>
                /// There is no key check value (KCV) verification required.
                /// </summary>
                [DataMember(Name = "kcvNone")]
                public bool? KcvNone { get; init; }

                /// <summary>
                /// The key check value (KCV) is created by an encryption of the key with itself.
                /// </summary>
                [DataMember(Name = "kcvSelf")]
                public bool? KcvSelf { get; init; }

                /// <summary>
                /// The key check value (KCV) is created by encrypting a zero value with the key.
                /// </summary>
                [DataMember(Name = "kcvZero")]
                public bool? KcvZero { get; init; }

                /// <summary>
                /// The No signature algorithm specified. No signature verification will take place.
                /// </summary>
                [DataMember(Name = "sigNone")]
                public bool? SigNone { get; init; }

                /// <summary>
                /// The RSASSA-PKCS1-v1.5 algorithm.
                /// </summary>
                [DataMember(Name = "rsassaPkcs1V15")]
                public bool? RsassaPkcs1V15 { get; init; }

                /// <summary>
                /// The RSASSA-PSS algorithm.
                /// </summary>
                [DataMember(Name = "rsassaPss")]
                public bool? RsassaPss { get; init; }

            }

            /// <summary>
            /// This parameter specifies the cryptographic method that will be used with encryption algorithm.
            /// 
            /// If the algorithm is 'A', 'D', or 'T' and the key usage is a MAC usage (i.e. 'M1'), then all
            /// properties are false.
            /// 
            /// If the algorithm is 'A', 'D', or 'T' and the key usage is '00', then one of properties must be
            /// set true.
            /// 
            /// * ```kcvNone``` - There is no key check value (KCV) verification required.
            /// * ```kcvSelf``` - The KCV is created by an encryption of the key with itself.
            /// * ```kcvZero``` - The KCV is created by encrypting a zero value with the key.
            /// 
            /// If the algorithm is 'R' and the key usage is not '00', then one of properties must be set true.
            /// 
            /// * ```sigNone``` - No signature algorithm specified. No signature verification will take place
            /// and the content of verificationData must be set.
            /// * ```rsassaPkcs1V15``` - Use the RSASSA-PKCS1-v1.5 algorithm.
            /// * ```rsassaPss``` - Use the RSASSA-PSS algorithm.
            /// </summary>
            [DataMember(Name = "cryptoMethod")]
            public CryptoMethodClass CryptoMethod { get; init; }

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
                /// The SHA 256 digest algorithm, as defined in ISO/IEC 10118-3:2004
                /// [[Ref. keymanagement-7](#ref-keymanagement-7)] and FIPS 180-2
                /// [[Ref. keymanagement-8](#ref-keymanagement-8)].
                /// </summary>
                [DataMember(Name = "sha256")]
                public bool? Sha256 { get; init; }

            }

            /// <summary>
            /// For asymmetric signature verification methods (key usage is 'S0', 'S1', or 'S2'), then one of
            /// the following properties are true. If the key usage is any of the MAC usages (i.e. 'M1'), then
            /// both 'sha1' and 'sha256' properties are false.
            /// </summary>
            [DataMember(Name = "hashAlgorithm")]
            public HashAlgorithmClass HashAlgorithm { get; init; }

        }

        /// <summary>
        /// Attributes supported by the [KeyManagement.ImportKey](#keymanagement.importkey) for the
        /// key used for verification before importing the key.
        /// </summary>
        [DataMember(Name = "verifyAttributes")]
        public Dictionary<string, Dictionary<string, Dictionary<string, VerifyAttributesClass>>> VerifyAttributes { get; init; }

    }


    public enum AuthenticationMethodEnum
    {
        Certhost,
        SigHost,
        Ca,
        Hl,
        Cbcmac,
        Cmac,
        CertHostTr34,
        CaTr34,
        HlTr34,
        Reserved1,
        Reserved2,
        Reserved3
    }


    public enum TypeDataItemToExportEnum
    {
        DeviceId,
        PublicKey
    }


    public enum RSASignatureAlgorithmEnum
    {
        Na,
        RsassaPkcs1V15,
        RsassaPss
    }


}
