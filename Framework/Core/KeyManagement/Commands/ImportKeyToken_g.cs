/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ImportKeyToken_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = ImportKeyToken
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "KeyManagement.ImportKeyToken")]
    public sealed class ImportKeyTokenCommand : Command<ImportKeyTokenCommand.PayloadData>
    {
        public ImportKeyTokenCommand()
            : base()
        { }

        public ImportKeyTokenCommand(int RequestId, ImportKeyTokenCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<byte> KeyToken = null, string Key = null, string KeyUsage = null, LoadOptionEnum? LoadOption = null)
                : base()
            {
                this.KeyToken = KeyToken;
                this.Key = Key;
                this.KeyUsage = KeyUsage;
                this.LoadOption = LoadOption;
            }

            /// <summary>
            /// Pointer to a binary encoded PKCS #7 represented in DER encoded ASN.1 notation. This allows the Host to
            /// verify that key was imported correctly and to the correct device. The message has an outer Signed-data
            /// content type with the SignerInfo encryptedDigest field containing the HOST’s signature. The inner content is
            /// an Enveloped-data content type. The device identifier is included as the issuerAndSerialNumber within the
            /// RecipientInfo.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "keyToken")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> KeyToken { get; init; }

            /// <summary>
            /// Specifies the name of the key to be stored.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

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

            public enum LoadOptionEnum
            {
                NoRandom,
                Random,
                NoRandomCrl,
                RandomCrl
            }

            /// <summary>
            /// Specifies the method to use to load the key token as one of the following values:
            /// 
            /// * ```noRandom``` - Import a key without generating a using a random number.
            /// * ```random``` - Import a key by generating and using a random number. This option is used for
            ///   [Remote Key Exchange](#keymanagement.generalinformation.crklsummary.keyexchange)
            /// * ```noRandomCrl``` - Import a key with a Certificate Revocation List included in the token. A random
            ///   number is not generated nor used. This option is used for the
            ///   [One-Pass Protocol](#keymanagement.generalinformation.tr34rkl.tr34keytransport.tr34keytransportonepass)
            ///   described in X9 TR34-2019 [[Ref. keymanagement-9](#ref-keymanagement-9)]
            /// * ```randomCrl``` - Import a key with a Certificate Revocation List included in the token. A random number
            ///   is generated and used. This option is used for the
            ///   [Two-Pass Protocol](#keymanagement.generalinformation.tr34rkl.tr34keytransport.tr34keytransporttwopass)
            ///   described in X9 TR34-2019 [[Ref. keymanagement-9](#ref-keymanagement-9)]
            /// 
            /// If *random* or *randomCrl*, the random number is included as an authenticated attribute within SignerInfo
            /// SignedAttributes.
            /// 
            /// If *noRandom* or *noRandomCrl*, a timestamp is included as an authenticated attribute within SignerInfo
            /// SignedAttributes.
            /// 
            /// If *noRandomCrl* or *randomCrl*, *keyUsage* must be null as the key usage is embedded in the *keyToken*.
            /// </summary>
            [DataMember(Name = "loadOption")]
            public LoadOptionEnum? LoadOption { get; init; }

        }
    }
}
