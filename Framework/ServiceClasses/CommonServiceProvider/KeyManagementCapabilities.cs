/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// Constructor
    /// </summary>
    public sealed class KeyManagementCapabilitiesClass(
        int MaxKeys,
        KeyManagementCapabilitiesClass.KeyCheckModeEnum KeyCheckModes,
        string HSMVendor,
        KeyManagementCapabilitiesClass.RSAAuthenticationSchemeEnum RSAAuthenticationScheme,
        KeyManagementCapabilitiesClass.RSASignatureAlgorithmEnum RSASignatureAlgorithm,
        KeyManagementCapabilitiesClass.RSACryptAlgorithmEnum RSACryptAlgorithm,
        KeyManagementCapabilitiesClass.RSAKeyCheckModeEnum RSAKeyCheckMode,
        KeyManagementCapabilitiesClass.SignatureSchemeEnum SignatureScheme,
        KeyManagementCapabilitiesClass.EMVImportSchemeEnum EMVImportSchemes,
        KeyManagementCapabilitiesClass.KeyBlockImportFormatEmum KeyBlockImportFormats,
        bool KeyImportThroughParts,
        KeyManagementCapabilitiesClass.DESKeyLengthEmum DESKeyLength,
        KeyManagementCapabilitiesClass.CertificateTypeEnum CertificateTypes,
        List<KeyManagementCapabilitiesClass.SingerCapabilities> LoadCertificationOptions,
        KeyManagementCapabilitiesClass.CRKLLoadOptionEnum CRKLLoadOption,
        KeyManagementCapabilitiesClass.SymmetricKeyManagementMethodEnum SymmetricKeyManagementMethods,
        Dictionary<string, Dictionary<string, Dictionary<string, KeyManagementCapabilitiesClass.KeyAttributeOptionClass>>> KeyAttributes,
        Dictionary<string, KeyManagementCapabilitiesClass.DecryptMethodClass> DecryptAttributes,
        Dictionary<string, Dictionary<string, Dictionary<string, KeyManagementCapabilitiesClass.VerifyMethodClass>>> VerifyAttributes)
    {
        [Flags]
        public enum KeyCheckModeEnum
        {
            NotSupported = 0,
            Self = 1 << 0,
            Zero = 1 << 1,
        }

        [Flags]
        public enum RSAAuthenticationSchemeEnum
        {
            NotSupported = 0,
            SecondPartySignature = 1 << 0,      // Two-party Signature based authentication.
            ThirdPartyCertificate = 1 << 1,     // Three-party Certificate based authentication.
            ThirdPartyCertificateTR34 = 1 << 2, // Three-party Certificate based authentication described by X9 TR34-2012 
        }

        [Flags]
        public enum RSASignatureAlgorithmEnum
        {
            NotSupported = 0,
            RSASSA_PKCS1_V1_5 = 1 << 0,     // SSA_PKCS_V1_5 Signatures supported
            RSASSA_PSS = 1 << 1,            // SSA_PSS Signatures supported
        }

        [Flags]
        public enum RSACryptAlgorithmEnum
        {
            NotSupported = 0,
            RSAES_PKCS1_V1_5 = 1 << 0,      // AES_PKCS_V1_5 algorithm supported
            RSAES_OAEP = 1 << 1,            // AES_OAEP algorithm supported
        }

        [Flags]
        public enum RSAKeyCheckModeEnum
        {
            NotSupported = 0,
            SHA1 = 1 << 0,
            SHA256 = 1 << 1,
        }

        [Flags]
        public enum SignatureSchemeEnum
        {
            NotSupported = 0,
            RandomNumber = 1 << 0, // Specifies if the Service Provider returns a random number from the StartKeyExchange command within the RSA Signature Scheme.
            ExportEPPID = 1 << 1,  // Specifies if the Service Provider supports exporting the EPP Security Item within the RSA Signature Scheme.
            EnhancedRKL = 1 << 2,  // Specifies that the Service Provider supports the Enhanced Signature Remote Key Scheme. This scheme allows the customer to manage their own public keys independently of the Signature Issuer. 
        }

        [Flags]
        public enum EMVImportSchemeEnum
        {
            NotSupported = 0,
            PlainCA = 1 << 0,    //A plain text CA public key is imported with no verification
            ChecksumCA = 1 << 1, //A plain text CA public key is imported using the EMV 2000 verification algorithm
            EPICA = 1 << 2,      //A CA public key is imported using the self-sign scheme defined in the Europay International, EPI CA Module Technical - Interface specification Version 
            Issuer = 1 << 3,     //An Issuer public key is imported as defined in EMV 2000 Book II
            ICC = 1 << 4,        //An ICC public key is imported as defined in EMV 2000 Book II
            ICCPIN = 1 << 5,     //An ICC PIN public key is imported as defined in EMV 2000 Book II
            PKCSV1_5CA = 1 << 6, //A CA public key is imported and verified using a signature generated with a private key for which the public key is already loaded
        }

        [Flags]
        public enum KeyBlockImportFormatEmum
        {
            NotSupported = 0,
            KEYBLOCKA = 1 << 0, //ANSI X9.143 version ID A of the keyblock
            KEYBLOCKB = 1 << 1, //ANSI X9.143 version ID B of the keyblock
            KEYBLOCKC = 1 << 2, //ANSI X9.143 version ID C of the keyblock
            KEYBLOCKD = 1 << 3, //ANSI X9.143 version ID D of the keyblock
        }

        [Flags]
        public enum DESKeyLengthEmum
        {
            NotSupported = 0,
            Single = 1 << 0,
            Double = 1 << 1,
            Triple = 1 << 2,
        }

        [Flags]
        public enum CertificateTypeEnum
        {
            NotSupported = 0,
            EncKey = 1 << 0,
            VerificationKey = 1 << 1,
            HostKey = 1 << 2,
        }

        [Flags]
        public enum SignerEnum
        {
            NotSupported = 0,
            EncKey = 1 << 0,           //Supports the EPP public encryption certificate
            VerificationKey = 1 << 1,  //Supports the EPP public verification certificate
            HostKey = 1 << 2,          //Supports the Host public certificate
        }

        [Flags]
        public enum LoadCertificateSignerEnum
        {
            NotSupported = 0,
            CertHost, //The current Host RSA Private Key is used to sign the token
            SigHost,  //The current Host RSA Private Key is used to sign the token, signature format is used.
            HL,       //A Higher-Level Authority RSA Private Key is used to sign the token
            CertHost_TR34, //The current Host RSA Private Key is used to sign the token, compliant with X9 TR34-2019
            CA_TR34, // The Certificate Authority RSA Private Key is used to sign the token, compliant with X9 TR34-2019
            HL_TR34, // A Higher-Level Authority RSA Private Key is used to sign the token, compliant with X9 TR34-2019
            CA, // Obsolete since 2023-2
        }

        [Flags]
        public enum LoadCertificateOptionEnum
        {
            NotSupported = 0,
            NewHost = 1 << 0,     //The current Host RSA Private Key is used to sign the token
            ReplaceHost = 1 << 1, //The current Host RSA Private Key is used to sign the token, signature format is used.
        }

        [Flags]
        public enum CRKLLoadOptionEnum
        {
            NotSupported = 0,
            NoRandom = 1 << 0,        //Import a Key Transport Key without generating and using a random number.
            NoRandomCRL = 1 << 1,     //Import a Key Transport Key with a Certificate Revocation List appended to the input message. A random number is not generated nor used.
            Random = 1 << 2,          //Import a Key Transport Key by generating and using a random number.
            RandomCRL = 1 << 3,       //Import a Key Transport Key with a Certificate Revocation List appended to the input parameter. A random number is generated and used.
        }

        [Flags]
        public enum SymmetricKeyManagementMethodEnum
        {
            NotSupported = 0,
            FixedKey = 1 << 0,        //This method of key management uses fixed keys for transaction processing
            MasterKey = 1 << 1,       //This method uses a hierarchy of Key Encrypting Keys and Transaction Keys. The highest level of Key Encrypting Key is known as a Master Key. Transaction Keys are distributed and replaced encrypted under a Key Encrypting Key
            TripleDESDUKPT = 1 << 2,  //This method uses TDES Derived Unique Key Per Transaction
        }

        /// <summary>
        /// Number of the keys which can be stored in the encryption/decryption module.
        /// </summary>
        public int MaxKeys { get; init; } = MaxKeys;

        /// <summary>
        /// Specifies the key check modes that are supported to check the correctness of an imported key value. 
        /// The encryption algorithm used (i.e. DES, 3DES, AES) is determined by the type of key being checked. 
        /// If the key size is larger than the algorithm block size, then only the first block will be used
        /// </summary>
        public KeyCheckModeEnum KeyCheckModes { get; init; } = KeyCheckModes;

        /// <summary>
        /// Identifies the hsm Vendor. 
        /// hsmVendor is an empty string or this property is not set when the hsm Vendor is unknown or the HSM is not supported.
        /// </summary>
        public string HSMVendor { get; init; } = HSMVendor;

        /// <summary>
        /// Specifies which type(s) of Remote Key Loading/Authentication is supported.
        /// </summary>
        public RSAAuthenticationSchemeEnum RSAAuthenticationScheme { get; init; } = RSAAuthenticationScheme;

        /// <summary>
        /// Specifies which type(s) of RSA Signature Algorithm(s) is supported.
        /// </summary>
        public RSASignatureAlgorithmEnum RSASignatureAlgorithm { get; init; } = RSASignatureAlgorithm;

        /// <summary>
        /// Specifies which type of RSA Encipherment Algorithm.
        /// </summary>
        public RSACryptAlgorithmEnum RSACryptAlgorithm { get; init; } = RSACryptAlgorithm;

        /// <summary>
        /// Specifies which algorithm/method used to generate the public key check value/thumb print.
        /// </summary>
        public RSAKeyCheckModeEnum RSAKeyCheckMode { get; init; } = RSAKeyCheckMode;

        /// <summary>
        /// Specifies which capabilities are supported by the Signature scheme.
        /// </summary>
        public SignatureSchemeEnum SignatureScheme { get; init; } = SignatureScheme;

        /// <summary>
        /// Identifies the supported emv Import Scheme(s).
        /// </summary>
        public EMVImportSchemeEnum EMVImportSchemes { get; init; } = EMVImportSchemes;


        /// <summary>
        /// Supported TR31 key block formats.
        /// </summary>
        public KeyBlockImportFormatEmum KeyBlockImportFormats { get; init; } = KeyBlockImportFormats;

        /// <summary>
        /// Specifies whether the device is capable of importing keys in multiple parts. TRUE means the 
        /// device supports the key import in multiple parts.
        /// </summary>
        public bool KeyImportThroughParts { get; init; } = KeyImportThroughParts;

        /// <summary>
        /// Specifies which length of DES keys are supported.
        /// </summary>
        public DESKeyLengthEmum DESKeyLength { get; init; } = DESKeyLength;

        /// <summary>
        /// Specifies supported certificate types.
        /// </summary>
        public CertificateTypeEnum CertificateTypes { get; init; } = CertificateTypes;

        public sealed class SingerCapabilities
        {
            public SingerCapabilities(LoadCertificateSignerEnum Signer,
                                      LoadCertificateOptionEnum Option,
                                      bool TR34)
            {
                this.Signer = Signer;
                this.Option = Option;
                this.TR34 = TR34;
            }

            /// <summary>
            /// Specifies the signer supported by the LoadCertificate command 
            /// </summary>
            public LoadCertificateSignerEnum Signer { get; init; }

            /// <summary>
            /// Specifies the load option supported by the LoadCertificate command 
            /// </summary>
            public LoadCertificateOptionEnum Option { get; init; }

            /// <summary>
            /// Specifies the TR34 certificate or not 
            /// </summary>
            public bool TR34 { get; init; }
        }

        /// <summary>
        /// There is one structure for each signer that is supported by the Service Provider. 
        /// In each structure, there will be a Signers parameter with one bit set to indicate which signer the structure is referencing, 
        /// and there will be a Options parameter with one or more bits set to indicate all of the options that the Service Provider supports with the signer specified by signer.
        /// </summary>
        public List<SingerCapabilities> LoadCertificationOptions { get; init; } = LoadCertificationOptions;

        /// <summary>
        /// Supported options to load the Key Transport Key using the Certificate Remote Key Loading protocol.
        /// </summary>
        public CRKLLoadOptionEnum CRKLLoadOption { get; init; } = CRKLLoadOption;

        /// <summary>
        /// Specifies the Symmentric Key Management modes.
        /// </summary>
        public SymmetricKeyManagementMethodEnum SymmetricKeyManagementMethods { get; init; } = SymmetricKeyManagementMethods;

        public sealed class KeyAttributeOptionClass
        {
            public KeyAttributeOptionClass(string Restricted = null)
            {
                this.Restricted = Restricted;
            }

            /// <summary>
            /// Specifies restricted key usage of the key associated with the key usage.
            /// This property can be omitted if there is no restricted key usage required.
            /// Following restricted key usage can be set if the key Usage is either 'K0' or 'K1'.
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
            public string Restricted { get; init; }
        }

        /// <summary>
        /// Key-value pair of attributes supported by Importkey command for the key to be loaded.
        /// </summary>
        public Dictionary<string, Dictionary<string, Dictionary<string, KeyAttributeOptionClass>>> KeyAttributes { get; init; } = KeyAttributes;

        public sealed class DecryptMethodClass
        {
            [Flags]
            public enum DecryptMethodEnum
            {
                NotSupported = 0,
                ECB = 1 << 0,
                CBC = 1 << 1,
                CFB = 1 << 2,
                OFB = 1 << 3,
                CTR = 1 << 4,
                XTS = 1 << 5,
                RSAES_PKCS1_V1_5 = 1 << 6,
                RSAES_OAEP = 1 << 7,
            }

            public DecryptMethodClass(DecryptMethodEnum DecryptMethods)
            {
                this.DecryptMethods = DecryptMethods;
            }

            /// <summary>
            /// Specifies the cryptographic method supported.
            /// If the algorithm is 'A', 'D', or 'T', then one of following property must be true and both rsaesPkcs1V15, rsaesOaep properties are false.
            /// 
            /// * ```ecb``` - The ECB encryption method. 
            /// * ```cbc``` - The CBC encryption method. 
            /// * ```cfb``` - The CFB encryption method. 
            /// * ```ofb``` - The OFB encryption method. 
            /// * ```ctr``` - The CTR method defined in NIST SP800-38A. 
            /// * ```xts``` - The XTS method defined in NIST SP800-38E. 
            /// 
            /// If the algorithm is 'R', then one of following property must be true and ecb, cbc, cfb, ofb, ctr, xts must be all false.
            /// 
            /// * ```rsaesPkcs1V15``` - Use the RSAES_PKCS1-v1.5 algorithm. 
            /// * ```rsaesOaep``` - Use the RSAES OAEP algorithm. 
            /// </summary>
            public DecryptMethodEnum DecryptMethods { get; init; }

        }

        /// <summary>
        /// Key-value pair of attributes supported by the ImportKey command for the key used to decrypt or unwrap the key being imported.
        /// </summary>
        public Dictionary<string, DecryptMethodClass> DecryptAttributes { get; init; } = DecryptAttributes;


        public sealed class VerifyMethodClass
        {
            [Flags]
            public enum CryptoMethodEnum
            {
                NotSupported = 0,
                KCVNone = 1 << 0,
                KCVSelf = 1 << 1,
                KCVZero = 1 << 2,
                SignatureNone = 1 << 3,
                RSASSA_PKCS1_V1_5 = 1 << 4,
                RSASSA_PSS = 1 << 5,
            }

            [Flags]
            public enum HashAlgorithmEnum
            {
                NotSupported = 0,
                SHA1 = 1 << 0,
                SHA256 = 1 << 1,
            }

            public VerifyMethodClass(CryptoMethodEnum CryptoMethod, HashAlgorithmEnum HashAlgorithm = HashAlgorithmEnum.NotSupported)
            {
                this.CryptoMethod = CryptoMethod;
                this.HashAlgorithm = HashAlgorithm;
            }

            /// <summary>
            /// This parameter specifies the cryptographic method that will be used with encryption algorithm.
            /// 
            /// If the algorithm is 'A', 'D', or 'T' and the key usage is a MAC usage (i.e. 'M1'), then all properties are false. 
            /// 
            /// If the algorithm is 'A', 'D', or 'T' and the key usage is '00', then one of properties must be set true. 
            /// 
            /// * ```kcvNone``` - There is no key check value verification required. 
            /// * ```kcvSelf``` - The key check value (KCV) is created by an encryption of the key with itself. 
            /// * ```kcvZero``` - The key check value (KCV) is created by encrypting a zero value with the key. 
            /// 
            /// If the algorithm is 'R' and the key usage is not '00', then one of properties must be set true. 
            /// 
            /// * ```sigNone``` - No signature algorithm specified. No signature verification will take place and the 
            /// content of verificationData must be set. 
            /// * ```rsassaPkcs1V15``` - Use the RSASSA-PKCS1-v1.5 algorithm. 
            /// * ```rsassaPss``` - Use the RSASSA-PSS algorithm.
            /// </summary>
            public CryptoMethodEnum CryptoMethod { get; init; }

            /// <summary>
            /// For asymmetric signature verification methods (key usage is 'S0', 'S1', or 'S2'), then one of the following properties are true.
            /// If the key usage is any of the MAC usages (i.e. 'M1'), then properties both 'sha1' and 'sha256' are false.
            /// </summary>
            public HashAlgorithmEnum HashAlgorithm { get; init; }

        }

        /// <summary>
        /// Key-value pair of attributes supported by the ImportKey for the key used for verification before importing the key.
        /// </summary>
        public Dictionary<string, Dictionary<string, Dictionary<string, VerifyMethodClass>>> VerifyAttributes { get; init; } = VerifyAttributes;

    }
}
