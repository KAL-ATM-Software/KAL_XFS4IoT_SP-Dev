/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ImportEmvPublicKey_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = ImportEmvPublicKey
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "KeyManagement.ImportEmvPublicKey")]
    public sealed class ImportEmvPublicKeyCommand : Command<ImportEmvPublicKeyCommand.PayloadData>
    {
        public ImportEmvPublicKeyCommand(int RequestId, ImportEmvPublicKeyCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Key = null, string KeyUsage = null, ImportSchemeEnum? ImportScheme = null, List<byte> Value = null, string VerifyKey = null)
                : base()
            {
                this.Key = Key;
                this.KeyUsage = KeyUsage;
                this.ImportScheme = ImportScheme;
                this.Value = Value;
                this.VerifyKey = VerifyKey;
            }

            /// <summary>
            /// Specifies the name of key being loaded.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// Specifies the type of access for which the *key* can be used. The following values are possible:
            /// 
            /// * ```E0``` - EMV / Chip Issuer Master Key: Application Cryptogram.
            /// * ```E1``` - EMV / Chip Issuer Master Key: Secure Messaging for Confidentiality.
            /// * ```E2``` - EMV / Chip Issuer Master Key: Secure Messaging for Integrity.
            /// * ```E3``` - EMV / Chip Issuer Master Key: Data Authentication Code.
            /// * ```E4``` - EMV / Chip Issuer Master Key: Dynamic.
            /// * ```E5``` - EMV / Chip Issuer Master Key: Card Personalization.
            /// * ```E6``` - EMV / Chip Issuer Master Key: Other Initialization Vector (IV).
            /// * ```E7``` - EMV / Chip Asymmetric Key Pair for EMV/Smart Card based PIN/PIN Block Encryption.
            /// * ```00 - 99``` - These numeric values are reserved for proprietary use.
            /// <example>E0</example>
            /// </summary>
            [DataMember(Name = "keyUsage")]
            [DataTypes(Pattern = @"^E[0-7]$|^[0-9][0-9]$")]
            public string KeyUsage { get; init; }

            public enum ImportSchemeEnum
            {
                PlainCA,
                ChecksumCA,
                EpiCA,
                Issuer,
                Icc,
                IccPIN,
                PkcsV1_5_CA
            }

            /// <summary>
            /// Defines the import scheme used. The following values are possible:
            /// 
            /// * ```plainCA``` - This scheme is used by VISA. A plain text CA public key is imported with no verification.
            ///   The two parts of the key (modulus and exponent) are passed in clear mode as a DER encoded PKCS#1 public key.
            ///   The key is loaded directly in the security module.
            /// * ```checksumCA``` - This scheme is used by VISA. A plain text CA public key is imported using the EMV 2000
            ///   Book II verification algorithm and it is verified before being loaded in the security module.
            /// * ```epiCA``` - This scheme is used by MasterCard Europe. A CA public key is imported using the self-signed
            ///   scheme.
            /// * ```issuer``` - An Issuer public key is imported as defined in EMV 2000 Book II.
            /// * ```icc``` - An ICC public key is imported as defined in EMV 2000 Book II.
            /// * ```iccPIN``` - An ICC PIN public key is imported as defined in EMV 2000 Book II.
            /// * ```pkcsV1_5_CA``` - A CA public key is imported and verified using a signature generated with a private
            ///   key for which the public key is already loaded.
            /// </summary>
            [DataMember(Name = "importScheme")]
            public ImportSchemeEnum? ImportScheme { get; init; }

            /// <summary>
            /// Contains all the necessary data to complete the import using the scheme specified within *importScheme*.
            /// 
            /// If *importScheme* is *plainCA* then *value* contains a DER encoded PKCS#1 public key. No verification is
            /// possible. *verifyKey* is ignored.
            /// 
            /// If *importScheme* is *checksumCA* then *value* contains table 23 data, as specified in EMV 2000 Book 2 (See
            /// [[Ref. keymanagement-3](#ref-keymanagement-3)]). The plain text key is verified as defined within EMV 2000
            /// Book 2, page 73. *verifyKey* is ignored (See [[Ref. keymanagement-3](#ref-keymanagement-3)]).
            /// 
            /// If *importScheme* is *epiCA* then *value* contains the concatenation of tables 4 and 13,
            /// as specified in [[Ref. keymanagement-4](#ref-keymanagement-4)], Europay International, EPI CA Module
            /// Technical – Interface specification Version 1.4. These tables are also described in the EMV Support
            /// Appendix. The self-signed public key is verified as defined by the reference document. *verifyKey* is ignored.
            /// 
            /// If *importScheme* is *issuer* then *value* contains the EMV public key certificate. Within the following
            /// descriptions tags are documented to indicate the source of the data, but they are not sent down to the
            /// service. The data consists of the concatenation of: the key exponent length (1 byte), the key exponent value
            /// (variable length – EMV Tag value: ‘9F32’), the EMV certificate length (1 byte), the EMV certificate value
            /// (variable length – EMV Tag value: ‘90’), the remainder length (1 byte). The remainder value (variable
            /// length – EMV Tag value: ‘92’), the PAN length (1 byte) and the PAN value (variable length – EMV Tag
            /// value: ‘5A’). The service will compare the leftmost three to eight hex digits (where each byte consists of
            /// two hex digits) of the PAN to the Issuer Identification Number retrieved from the certificate. For more
            /// explanations, the reader can refer to EMVCo, Book2 – Security &amp; Key Management Version 4.0, Table 4 (See
            /// [[Ref. keymanagement-3](#ref-keymanagement-3)]). *verifyKey* defines the previously loaded key used to
            /// verify the signature.
            /// 
            /// If *importScheme* is *icc* then *value* contains the EMV public key certificate. Within the following
            /// descriptions tags are documented to indicate the source of the data, but they are not sent down to the
            /// service. The data consists of the concatenation of: the key exponent length (1 byte), the key exponent
            /// value (variable length– EMV Tag value: ‘9F47’), the EMV certificate length (1 byte), the EMV certificate
            /// value (variable length – EMV Tag value:’9F46’), the remainder length (1 byte), the remainder value
            /// (variable length – EMV Tag value: ‘9F48’), the SDA length (1 byte), the SDA value (variable length), the
            /// PAN length (1 byte) and the PAN value (variable length – EMV Tag value: ‘5A’). The service will compare the
            /// PAN to the PAN retrieved from the certificate. For more explanations, the reader can refer to EMVCo,
            /// Book2 – Security &amp; Key Management Version 4.0, Table 9 (See [[Ref. keymanagement-3](#ref-keymanagement-3)]).
            /// *verifyKey* defines the previously loaded key used to verify the signature.
            /// 
            /// If *importScheme* is *iccPIN* then *value* contains the EMV public key certificate. Within the following
            /// descriptions tags are documented to indicate the source of the data, but they are not sent down to the
            /// service. The data consists of the concatenation of: the key exponent length (1 byte), the key exponent
            /// value (variable length – EMV Tag value: ‘9F2E’), the EMV certificate length (1 byte), the EMV certificate
            /// value (variable length – EMV Tag value:’9F2D’), the remainder length (1 byte), the remainder value
            /// (variable length – EMV Tag value: ‘9F2F’), the SDA length (1 byte), the SDA value (variable length), the
            /// PAN length (1 byte) and the PAN value (variable length – EMV Tag value: ‘5A’). The service will compare the
            /// PAN to the PAN retrieved from the certificate. For more explanations, the reader can refer to EMVCo,
            /// Book2 – Security &amp; Key Management Version 4.0, Table 9 (See [[Ref. keymanagement-3](#ref-keymanagement-3)]).
            /// *verifyKey* defines the previously loaded key used to verify the signature.
            /// 
            /// If *importScheme* is *pkcsV1_5_CA* then *value* contains the CA public key signed with the previously
            /// loaded public key specified in *verifyKey*. *value* consists of the concatenation of EMV 2000 Book II
            /// Table 23 + 8 byte random number + Signature (See [[Ref. keymanagement-3](#ref-keymanagement-3)]). The
            /// 8-byte random number is not used for validation; it is used to ensure the signature is unique. The
            /// Signature consists of all the bytes in the *value* buffer after table 23 and the 8-byte random number.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "value")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Value { get; init; }

            /// <summary>
            /// Specifies the name of the previously loaded key used to verify the signature, as detailed in the
            /// descriptions above.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "verifyKey")]
            public string VerifyKey { get; init; }

        }
    }
}
