/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * CryptoSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Crypto
{

    [DataContract]
    public sealed class CryptoAttributeClass
    {
        public CryptoAttributeClass(KeyUsageEnum? KeyUsage = null, AlgorithmEnum? Algorithm = null, ModeOfUseEnum? ModeOfUse = null, CryptoMethodEnum? CryptoMethod = null)
        {
            this.KeyUsage = KeyUsage;
            this.Algorithm = Algorithm;
            this.ModeOfUse = ModeOfUse;
            this.CryptoMethod = CryptoMethod;
        }

        public enum KeyUsageEnum
        {
            D0,
            D1
        }

        /// <summary>
        /// Specifies the key usage supported by the [Crypto.CryptoData](#crypto.cryptodata) command.
        /// The following values are possible: 
        /// 
        /// * ```D0``` - Symmetric data encryption.  
        /// * ```D1``` - Asymmetric data encryption.
        /// </summary>
        [DataMember(Name = "keyUsage")]
        public KeyUsageEnum? KeyUsage { get; private set; }

        public enum AlgorithmEnum
        {
            A,
            D,
            R,
            T
        }

        /// <summary>
        /// Specifies the encryption algorithms supported by [Crypto.CryptoData](#crypto.cryptodata) command.
        /// The following values are possible:  
        /// 
        /// * ```A``` - AES. 
        /// * ```D``` - DEA. 
        /// * ```R``` - RSA. 
        /// * ```T``` - Triple DEA (also referred to as TDEA).
        /// </summary>
        [DataMember(Name = "algorithm")]
        public AlgorithmEnum? Algorithm { get; private set; }

        public enum ModeOfUseEnum
        {
            D,
            E
        }

        /// <summary>
        /// Specifies the encryption mode supported by [Crypto.CryptoData](#crypto.cryptodata) command.
        /// The following values are possible: 
        /// 
        /// * ```D``` - Decrypt  
        /// * ```E``` - Encrypt  
        /// </summary>
        [DataMember(Name = "modeOfUse")]
        public ModeOfUseEnum? ModeOfUse { get; private set; }

        public enum CryptoMethodEnum
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
        /// Specifies the cryptographic method supported by the [Crypto.CryptoData](#crypto.cryptodata) command.
        /// For symmetric encryption methods (keyUsage is ‘D0’), this can be one of the following values: 
        /// 
        /// * ```ecb``` - The ECB encryption method. 
        /// * ```cbc``` - The CBC encryption method. 
        /// * ```cfb``` - The CFB encryption method. 
        /// * ```ofb``` - The OFB encryption method. 
        /// * ```ctr``` - The CTR method defined in NIST SP800-38A. 
        /// * ```xts``` - The XTS method defined in NIST SP800-38E. 
        /// 
        /// For asymmetric encryption methods (keyUsage is ‘D1’), this can be one of the following values: 
        /// 
        /// * ```rsaesPkcs1V15``` - Use the RSAES_PKCS1-v1.5 algorithm. 
        /// * ```rsaesOaep``` - Use the RSAES OAEP algorithm.
        /// </summary>
        [DataMember(Name = "cryptoMethod")]
        public CryptoMethodEnum? CryptoMethod { get; private set; }

    }


    [DataContract]
    public sealed class AuthenticationAttributeClass
    {
        public AuthenticationAttributeClass(KeyUsageEnum? KeyUsage = null, AlgorithmEnum? Algorithm = null, ModeOfUseEnum? ModeOfUse = null, CryptoMethodEnum? CryptoMethod = null, HashAlgorithmClass HashAlgorithm = null)
        {
            this.KeyUsage = KeyUsage;
            this.Algorithm = Algorithm;
            this.ModeOfUse = ModeOfUse;
            this.CryptoMethod = CryptoMethod;
            this.HashAlgorithm = HashAlgorithm;
        }

        public enum KeyUsageEnum
        {
            M0,
            M1,
            M2,
            M3,
            M4,
            M5,
            M6,
            M7,
            M8,
            S0,
            S1,
            S2
        }

        /// <summary>
        /// Specifies the key usage supported by the [Crypto.GenerateAuthentication](#crypto.generateauthentication) command.
        /// The following values are possible:
        /// 
        /// * ```M0``` - ISO 16609 MAC Algorithm 1 (using TDEA).
        /// * ```M1```- ISO 9797-1 MAC Algorithm 1.
        /// * ```M2``` - ISO 9797-1 MAC Algorithm 2. 
        /// * ```M3``` - ISO 9797-1 MAC Algorithm 3. 
        /// * ```M4``` - ISO 9797-1 MAC Algorithm 4. 
        /// * ```M5``` - ISO 9797-1:1999 MAC Algorithm 5. 
        /// * ```M6``` - 9797-1:2011 MAC Algorithm 5/CMAC. 
        /// * ```M7``` - HMAC. 
        /// * ```M8``` - ISO 9797-1:2011 MAC Algorithm 6. 
        /// * ```S0``` - Asymmetric key pair for digital signature. 
        /// * ```S1``` - Asymmetric key pair, CA. 
        /// * ```S2``` - Asymmetric key pair, nonX9.24 key.
        /// </summary>
        [DataMember(Name = "keyUsage")]
        public KeyUsageEnum? KeyUsage { get; private set; }

        public enum AlgorithmEnum
        {
            A,
            D,
            R,
            T
        }

        /// <summary>
        /// Specifies the encryption algorithms supported by [Crypto.GenerateAuthentication](#crypto.generateauthentication) command.
        /// The following values are possible: 
        /// 
        /// * ```A``` - AES. 
        /// * ```D``` - DEA. 
        /// * ```R``` - RSA. 
        /// * ```T``` - Triple DEA (also referred to as TDEA).
        /// </summary>
        [DataMember(Name = "algorithm")]
        public AlgorithmEnum? Algorithm { get; private set; }

        public enum ModeOfUseEnum
        {
            G,
            S
        }

        /// <summary>
        /// Specifies the encryption mode supported by [Crypto.GenerateAuthentication](#crypto.generateauthentication) command.
        /// The following values are possible: 
        /// 
        /// * ```G``` - Generate. This be used to generate a MAC. 
        /// * ```S``` - Signature
        /// </summary>
        [DataMember(Name = "modeOfUse")]
        public ModeOfUseEnum? ModeOfUse { get; private set; }

        public enum CryptoMethodEnum
        {
            RsassaPkcs1V15,
            RsassaPss
        }

        /// <summary>
        /// Specifies the cryptographic method supported by the [Crypto.GenerateAuthentication](#crypto.generateauthentication) command.
        /// For asymmetric signature verification methods (bKeyUsage is ‘S0’, ‘S1’, or ‘S2’), this can be one of the following values: 
        /// 
        /// * ```rsassaPkcs1V15``` - Use the RSASSA-PKCS1-v1.5 algorithm. 
        /// * ```rsassaPss``` - Use the RSASSA-PSS algorithm. 
        /// If keyUsage is specified as any of the MAC usages (i.e. ‘M1’), then this proeprty should not be not set.
        /// </summary>
        [DataMember(Name = "cryptoMethod")]
        public CryptoMethodEnum? CryptoMethod { get; private set; }

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
            public bool? Sha1 { get; private set; }

            /// <summary>
            /// The SHA 256 digest algorithm, as defined in ISO/IEC 10118-3:2004 and FIPS 180-2.
            /// </summary>
            [DataMember(Name = "sha256")]
            public bool? Sha256 { get; private set; }

        }

        /// <summary>
        /// For asymmetric signature verification methods (keyUsage is ‘S0’, ‘S1’, or ‘S2’), this can be one of the following values to be used.
        /// If keyUsage is specified as any of the MAC usages (i.e. ‘M1’), then properties should not be not set or both 'sha1' and 'sha256' are false.
        /// </summary>
        [DataMember(Name = "hashAlgorithm")]
        public HashAlgorithmClass HashAlgorithm { get; private set; }

    }


    [DataContract]
    public sealed class VerifyAttributeClass
    {
        public VerifyAttributeClass(KeyUsageEnum? KeyUsage = null, AlgorithmEnum? Algorithm = null, ModeOfUseEnum? ModeOfUse = null, CryptoMethodEnum? CryptoMethod = null, HashAlgorithmClass HashAlgorithm = null)
        {
            this.KeyUsage = KeyUsage;
            this.Algorithm = Algorithm;
            this.ModeOfUse = ModeOfUse;
            this.CryptoMethod = CryptoMethod;
            this.HashAlgorithm = HashAlgorithm;
        }

        public enum KeyUsageEnum
        {
            M0,
            M1,
            M2,
            M3,
            M4,
            M5,
            M6,
            M7,
            M8,
            S0,
            S1,
            S2
        }

        /// <summary>
        /// Specifies the key usage supported by the [Crypto.VerifyAuthentication](#crypto.verifyauthentication) command.
        /// The following values are possible: 
        /// 
        /// * ```M0``` - ISO 16609 MAC Algorithm 1 (using TDEA).
        /// * ```M1``` - ISO 9797-1 MAC Algorithm 1. 
        /// * ```M2``` - ISO 9797-1 MAC Algorithm 2.
        /// * ```M3``` - ISO 9797-1 MAC Algorithm 3. 
        /// * ```M4``` - ISO 9797-1 MAC Algorithm 4. 
        /// * ```M5``` - ISO 9797-1:1999 MAC Algorithm 5. 
        /// * ```M6``` - 9797-1:2011 MAC Algorithm 5/CMAC. 
        /// * ```M7``` - HMAC. 
        /// * ```M8``` - ISO 9797-1:2011 MAC Algorithm 6. 
        /// * ```S0``` - Asymmetric key pair for digital signature. 
        /// * ```S1``` - Asymmetric key pair, CA. 
        /// * ```S2``` - Asymmetric key pair, nonX9.24 key.
        /// </summary>
        [DataMember(Name = "keyUsage")]
        public KeyUsageEnum? KeyUsage { get; private set; }

        public enum AlgorithmEnum
        {
            A,
            D,
            R,
            T
        }

        /// <summary>
        /// Specifies the encryption algorithms supported by [Crypto.VerifyAuthentication](#crypto.verifyauthentication) command.
        /// The following values are possible: 
        /// 
        /// * ```A``` - AES. 
        /// * ```D``` - DEA. 
        /// * ```R``` - RSA. 
        /// * ```T``` - Triple DEA (also referred to as TDEA).
        /// </summary>
        [DataMember(Name = "algorithm")]
        public AlgorithmEnum? Algorithm { get; private set; }

        public enum ModeOfUseEnum
        {
            S,
            V
        }

        /// <summary>
        /// Specifies the encryption mode supported by [Crypto.VerifyAuthentication](#crypto.verifyauthentication) command.
        /// The following values are possible: 
        /// 
        /// * ```S``` - Signature.  
        /// * ```V``` - Verify. This be used to verify a MAC.
        /// </summary>
        [DataMember(Name = "modeOfUse")]
        public ModeOfUseEnum? ModeOfUse { get; private set; }

        public enum CryptoMethodEnum
        {
            RsassaPkcs1V15,
            RsassaPss
        }

        /// <summary>
        /// Specifies the cryptographic method supported by the [Crypto.VerifyAuthentication](#crypto.verifyauthentication) command.
        /// For asymmetric signature verification methods (bKeyUsage is ‘S0’, ‘S1’, or ‘S2’), this can be one of the following values. 
        /// 
        /// * ```rsassaPkcs1V15``` - Use the RSASSA-PKCS1-v1.5 algorithm. 
        /// * ```rsassaPss``` - Use the RSASSA-PSS algorithm. 
        /// 
        /// If keyUsage is specified as any of the MAC usages (i.e. ‘M1’), then this proeprty should not be not set.
        /// </summary>
        [DataMember(Name = "cryptoMethod")]
        public CryptoMethodEnum? CryptoMethod { get; private set; }

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
            public bool? Sha1 { get; private set; }

            /// <summary>
            /// The SHA 256 digest algorithm, as defined in ISO/IEC 10118-3:2004 and FIPS 180-2.
            /// </summary>
            [DataMember(Name = "sha256")]
            public bool? Sha256 { get; private set; }

        }

        /// <summary>
        /// For asymmetric signature verification methods (keyUsage is ‘S0’, ‘S1’, or ‘S2’), this can be one of the following values to be used.
        /// If keyUsage is specified as any of the MAC usages (i.e. ‘M1’), then properties should not be not set or both 'sha1' and 'sha256' are false.
        /// </summary>
        [DataMember(Name = "hashAlgorithm")]
        public HashAlgorithmClass HashAlgorithm { get; private set; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(AlgorithmsClass Algorithms = null, EmvHashAlgorithmClass EmvHashAlgorithm = null, List<CryptoAttributeClass> CryptoAttributes = null, List<AuthenticationAttributeClass> AuthenticationAttributes = null, List<VerifyAttributeClass> VerifyAttributes = null)
        {
            this.Algorithms = Algorithms;
            this.EmvHashAlgorithm = EmvHashAlgorithm;
            this.CryptoAttributes = CryptoAttributes;
            this.AuthenticationAttributes = AuthenticationAttributes;
            this.VerifyAttributes = VerifyAttributes;
        }

        [DataContract]
        public sealed class AlgorithmsClass
        {
            public AlgorithmsClass(bool? Ecb = null, bool? Cbc = null, bool? Cfb = null, bool? Rsa = null, bool? Cma = null, bool? DesMac = null, bool? TriDesEcb = null, bool? TriDesCbc = null, bool? TriDesCfb = null, bool? TriDesMac = null, bool? MaaMac = null, bool? TriDesMac2805 = null, bool? Sm4 = null, bool? Sm4Mac = null)
            {
                this.Ecb = Ecb;
                this.Cbc = Cbc;
                this.Cfb = Cfb;
                this.Rsa = Rsa;
                this.Cma = Cma;
                this.DesMac = DesMac;
                this.TriDesEcb = TriDesEcb;
                this.TriDesCbc = TriDesCbc;
                this.TriDesCfb = TriDesCfb;
                this.TriDesMac = TriDesMac;
                this.MaaMac = MaaMac;
                this.TriDesMac2805 = TriDesMac2805;
                this.Sm4 = Sm4;
                this.Sm4Mac = Sm4Mac;
            }

            /// <summary>
            /// Electronic Code Book.
            /// </summary>
            [DataMember(Name = "ecb")]
            public bool? Ecb { get; private set; }

            /// <summary>
            /// Cipher Block Chaining.
            /// </summary>
            [DataMember(Name = "cbc")]
            public bool? Cbc { get; private set; }

            /// <summary>
            /// Cipher Feed Back.
            /// </summary>
            [DataMember(Name = "cfb")]
            public bool? Cfb { get; private set; }

            /// <summary>
            /// RSA Encryption.
            /// </summary>
            [DataMember(Name = "rsa")]
            public bool? Rsa { get; private set; }

            /// <summary>
            /// ECMA Encryption.
            /// </summary>
            [DataMember(Name = "cma")]
            public bool? Cma { get; private set; }

            /// <summary>
            /// MAC calculation using CBC.
            /// </summary>
            [DataMember(Name = "desMac")]
            public bool? DesMac { get; private set; }

            /// <summary>
            /// Triple DES with Electronic Code Book.
            /// </summary>
            [DataMember(Name = "triDesEcb")]
            public bool? TriDesEcb { get; private set; }

            /// <summary>
            /// Triple DES with Cipher Block Chaining.
            /// </summary>
            [DataMember(Name = "triDesCbc")]
            public bool? TriDesCbc { get; private set; }

            /// <summary>
            /// Triple DES with Cipher Feed Back.
            /// </summary>
            [DataMember(Name = "triDesCfb")]
            public bool? TriDesCfb { get; private set; }

            /// <summary>
            /// Last Block Triple DES MAC as defined in ISO/IEC 9797-1:1999 [Ref. 32], using: 
            /// block length n=64, padding Method 1 (when padding=0), MAC Algorithm 3, MAC length m where 32<=m<=64.
            /// </summary>
            [DataMember(Name = "triDesMac")]
            public bool? TriDesMac { get; private set; }

            /// <summary>
            /// MAC calculation using the Message authenticator algorithm as defined in ISO 8731-2.
            /// </summary>
            [DataMember(Name = "maaMac")]
            public bool? MaaMac { get; private set; }

            /// <summary>
            /// Triple DES MAC calculation as defined in ISO 16609:2004 and and Australian Standard 2805.4.
            /// </summary>
            [DataMember(Name = "triDesMac2805")]
            public bool? TriDesMac2805 { get; private set; }

            /// <summary>
            /// SM4 block cipher algorithm as defined in Password industry standard of the People's Republic of China GM/T 0002-2012.
            /// </summary>
            [DataMember(Name = "sm4")]
            public bool? Sm4 { get; private set; }

            /// <summary>
            /// EMAC calculation using the Message authenticator algorithm as defined in as defined in Password 
            /// industry standard of the People's Republic of China GM/T 0002-2012.
            /// and and in PBOC3.0 JR/T 0025.17-2013.
            /// </summary>
            [DataMember(Name = "sm4Mac")]
            public bool? Sm4Mac { get; private set; }

        }

        /// <summary>
        /// Supported encryption modes.
        /// </summary>
        [DataMember(Name = "algorithms")]
        public AlgorithmsClass Algorithms { get; private set; }

        [DataContract]
        public sealed class EmvHashAlgorithmClass
        {
            public EmvHashAlgorithmClass(bool? Sha1Digest = null, bool? Sha256Digest = null)
            {
                this.Sha1Digest = Sha1Digest;
                this.Sha256Digest = Sha256Digest;
            }

            /// <summary>
            /// The SHA 1 digest algorithm is supported by the [Crypto.Digest](#crypto.digest) command.
            /// </summary>
            [DataMember(Name = "sha1Digest")]
            public bool? Sha1Digest { get; private set; }

            /// <summary>
            /// The SHA 256 digest algorithm, as defined in ISO/IEC 10118-3:2004 and FIPS 180-2, is supported 
            /// by the [Crypto.Digest](#crypto.digest) command.
            /// </summary>
            [DataMember(Name = "sha256Digest")]
            public bool? Sha256Digest { get; private set; }

        }

        /// <summary>
        /// Specifies which hash algorithm is supported for the calculation of the HASH.
        /// </summary>
        [DataMember(Name = "emvHashAlgorithm")]
        public EmvHashAlgorithmClass EmvHashAlgorithm { get; private set; }

        /// <summary>
        /// Array of attributes supported by the [Crypto.CryptoData](#crypto.cryptodata) command.
        /// </summary>
        [DataMember(Name = "cryptoAttributes")]
        public List<CryptoAttributeClass> CryptoAttributes { get; private set; }

        /// <summary>
        /// Array of attributes supported by the [Crypto.GenerateAuthentication](#crypto.generateauthentication) command.
        /// </summary>
        [DataMember(Name = "authenticationAttributes")]
        public List<AuthenticationAttributeClass> AuthenticationAttributes { get; private set; }

        /// <summary>
        /// Array of attributes supported by the [Crypto.VerifyAuthentication](#crypto.verifyauthentication) command.
        /// </summary>
        [DataMember(Name = "verifyAttributes")]
        public List<VerifyAttributeClass> VerifyAttributes { get; private set; }

    }


}
