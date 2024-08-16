/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * GetKeyDetail_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "KeyManagement.GetKeyDetail")]
    public sealed class GetKeyDetailCompletion : Completion<GetKeyDetailCompletion.PayloadData>
    {
        public GetKeyDetailCompletion(int RequestId, GetKeyDetailCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, KeyDetailsClass> KeyDetails = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.KeyDetails = KeyDetails;
            }

            public enum ErrorCodeEnum
            {
                KeyNotFound
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```keyNotFound``` - The specified key name is not found.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class KeyDetailsClass
            {
                public KeyDetailsClass(int? Generation = null, int? Version = null, string ActivatingDate = null, string ExpiryDate = null, LoadedEnum? Loaded = null, KeyBlockInfoClass KeyBlockInfo = null)
                {
                    this.Generation = Generation;
                    this.Version = Version;
                    this.ActivatingDate = ActivatingDate;
                    this.ExpiryDate = ExpiryDate;
                    this.Loaded = Loaded;
                    this.KeyBlockInfo = KeyBlockInfo;
                }

                /// <summary>
                /// Specifies the generation of the key.
                /// Different generations might correspond to different environments (e.g. test or production environment).
                /// The content is vendor specific. This value can be null if no such information is available for the key.
                /// </summary>
                [DataMember(Name = "generation")]
                [DataTypes(Minimum = 0, Maximum = 99)]
                public int? Generation { get; init; }

                /// <summary>
                /// Specifies the version of the key (the year in which the key is valid, e.g. 1 for 2001).
                /// This value can be null if no such information is available for the key.
                /// </summary>
                [DataMember(Name = "version")]
                [DataTypes(Minimum = 0, Maximum = 99)]
                public int? Version { get; init; }

                /// <summary>
                /// Specifies the date when the key is activated in the format YYYYMMDD.
                /// This value can be null if no such information is available for the key.
                /// <example>20210101</example>
                /// </summary>
                [DataMember(Name = "activatingDate")]
                [DataTypes(Pattern = @"^[0-9]{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])$")]
                public string ActivatingDate { get; init; }

                /// <summary>
                /// Specifies the date when the key expires in the format YYYYMMDD.
                /// This value can be null if no such information is available for the key.
                /// <example>20220101</example>
                /// </summary>
                [DataMember(Name = "expiryDate")]
                [DataTypes(Pattern = @"^[0-9]{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])$")]
                public string ExpiryDate { get; init; }

                public enum LoadedEnum
                {
                    No,
                    Yes,
                    Unknown,
                    Construct
                }

                /// <summary>
                /// Specifies whether the key has been loaded (imported from Application or locally from Operator).
                /// * ```no``` - The key is not loaded.
                /// * ```yes``` - The key is loaded and ready to be used in cryptographic operations.
                /// * ```unknown``` -\tThe State of the key is unknown.
                /// * ```construct``` -\tThe key is under construction, meaning that at least one key part has been loaded but
                /// the key is not activated and ready to be used in other cryptographic operations.
                /// </summary>
                [DataMember(Name = "loaded")]
                public LoadedEnum? Loaded { get; init; }

                [DataContract]
                public sealed class KeyBlockInfoClass
                {
                    public KeyBlockInfoClass(string KeyUsage = null, string RestrictedKeyUsage = null, string Algorithm = null, string ModeOfUse = null, string KeyVersionNumber = null, string Exportability = null, string OptionalBlockHeader = null, int? KeyLength = null)
                    {
                        this.KeyUsage = KeyUsage;
                        this.RestrictedKeyUsage = RestrictedKeyUsage;
                        this.Algorithm = Algorithm;
                        this.ModeOfUse = ModeOfUse;
                        this.KeyVersionNumber = KeyVersionNumber;
                        this.Exportability = Exportability;
                        this.OptionalBlockHeader = OptionalBlockHeader;
                        this.KeyLength = KeyLength;
                    }

                    /// <summary>
                    /// Specifies the intended function of the key.
                    /// The following values are possible - See [[Ref. keymanagement-6](#ref-keymanagement-6)] :
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
                    [DataMember(Name = "keyUsage")]
                    [DataTypes(Pattern = @"^B[0-3]$|^C0$|^D[0-3]$|^E[0-7]$|^I0$|^K[0-4]$|^M[0-8]$|^P[0-1]$|^S[0-2]$|^V[0-5]$|^[0-9][0-9]$")]
                    public string KeyUsage { get; init; }

                    /// <summary>
                    /// If the *keyUsage* is a key encryption usage (e.g. 'K0') this specifies the key usage of the keys that
                    /// can be encrypted by the key.
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
                    /// 
                    /// This value can be null if the key usage is not an key encryption usage or restricted key encryption keys
                    /// are not supported or required.
                    /// <example>D0</example>
                    /// </summary>
                    [DataMember(Name = "restrictedKeyUsage")]
                    [DataTypes(Pattern = @"^B[0-3]$|^C0$|^D[0-3]$|^E[0-7]$|^I0$|^K[0-4]$|^M[0-8]$|^P[0-1]$|^S[0-2]$|^V[0-5]$|^[0-9][0-9]$")]
                    public string RestrictedKeyUsage { get; init; }

                    /// <summary>
                    /// Specifies the algorithm for which the key can be used.
                    /// See [[Ref. keymanagement-6](#ref-keymanagement-6)] for all possible values:
                    /// * ```A``` - AES.
                    /// * ```D``` - DEA.
                    /// * ```E``` - Elliptic Curve.
                    /// * ```H``` - HMAC.
                    /// * ```R``` - RSA.
                    /// * ```S``` - DSA.
                    /// * ```T``` - Triple DEA (also referred to as TDEA).
                    /// * ```0 - 9``` - These numeric values are reserved for proprietary use.
                    /// <example>T</example>
                    /// </summary>
                    [DataMember(Name = "algorithm")]
                    [DataTypes(Pattern = @"^[0-9ADEHRST]$")]
                    public string Algorithm { get; init; }

                    /// <summary>
                    /// Specifies the operation that the key can perform.
                    /// See [[Ref. keymanagement-6](#ref-keymanagement-6)] for all possible values:
                    /// * ```B``` - Both Encrypt and Decrypt / Wrap and unwrap.
                    /// * ```C``` - Both Generate and Verify.
                    /// * ```D``` - Decrypt / Unwrap Only.
                    /// * ```E``` - Encrypt / Wrap Only.
                    /// * ```G``` - Generate Only.
                    /// * ```N``` - No special restrictions.
                    /// * ```S``` - Signature Only.
                    /// * ```T``` - Both Sign and Decrypt.
                    /// * ```V``` - Verify Only.
                    /// * ```X``` - Key used to derive other keys(s).
                    /// * ```Y``` - Key used to create key variants.
                    /// * ```0 - 9``` - These numeric values are reserved for proprietary use.
                    /// <example>B</example>
                    /// </summary>
                    [DataMember(Name = "modeOfUse")]
                    [DataTypes(Pattern = @"^[0-9BCDEGNSTVXY]$")]
                    public string ModeOfUse { get; init; }

                    /// <summary>
                    /// Specifies a two-digit ASCII character version number, which is optionally used to indicate that contents
                    /// of the key block are a component, or to prevent re-injection of old keys.
                    /// See [[Ref. keymanagement-6](#ref-keymanagement-6)] for all possible values.
                    /// This value can be null if Key versioning is not used.
                    /// <example>01</example>
                    /// </summary>
                    [DataMember(Name = "keyVersionNumber")]
                    [DataTypes(Pattern = @"^[0-9a-zA-Z][0-9a-zA-Z]$")]
                    public string KeyVersionNumber { get; init; }

                    /// <summary>
                    /// Specifies whether the key may be transferred outside of the cryptographic domain in which the key is
                    /// found. See [[Ref. keymanagement-6](#ref-keymanagement-6)] for all possible values:
                    /// * ```E``` - Exportable under a KEK in a form meeting the requirements of X9.24 Parts 1 or 2.
                    /// * ```N``` - Non-exportable by the receiver of the key block, or from storage.
                    ///             Does not preclude exporting keys derived from a non-exportable key.
                    /// * ```S``` - Sensitive, Exportable under a KEK in a form not necessarily meeting the requirements of
                    /// X9.24 Parts 1 or 2.
                    /// * ```0 - 9``` - These numeric values are reserved for proprietary use.
                    /// <example>N</example>
                    /// </summary>
                    [DataMember(Name = "exportability")]
                    [DataTypes(Pattern = @"^[0-9ESN]$")]
                    public string Exportability { get; init; }

                    /// <summary>
                    /// Contains any optional header blocks, as defined in [[Ref. keymanagement-6](#ref-keymanagement-6)].
                    /// This value can be null if there are no optional block headers.
                    /// <example>HM0621</example>
                    /// </summary>
                    [DataMember(Name = "optionalBlockHeader")]
                    public string OptionalBlockHeader { get; init; }

                    /// <summary>
                    /// Specifies the length, in bits, of the key. 0 if the key length is unknown.
                    /// </summary>
                    [DataMember(Name = "keyLength")]
                    [DataTypes(Minimum = 0)]
                    public int? KeyLength { get; init; }

                }

                /// <summary>
                /// Specifies the key attributes using X9.143 keyblock header definitions.
                /// </summary>
                [DataMember(Name = "keyBlockInfo")]
                public KeyBlockInfoClass KeyBlockInfo { get; init; }

            }

            /// <summary>
            /// This property contains key/value pairs where the key is a name of key and the value is the key detail.
            /// If there are no key details, this property is an empty object.
            /// </summary>
            [DataMember(Name = "keyDetails")]
            public Dictionary<string, KeyDetailsClass> KeyDetails { get; init; }

        }
    }
}
