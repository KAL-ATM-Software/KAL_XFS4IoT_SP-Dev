/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * GenerateAuthentication_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Crypto.Commands
{
    //Original name = GenerateAuthentication
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Crypto.GenerateAuthentication")]
    public sealed class GenerateAuthenticationCommand : Command<GenerateAuthenticationCommand.PayloadData>
    {
        public GenerateAuthenticationCommand(int RequestId, GenerateAuthenticationCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Key = null, List<byte> Data = null, string StoredKey = null, IvClass Iv = null, int? Padding = null, string Compression = null, CryptoMethodEnum? CryptoMethod = null, HashAlgorithmEnum? HashAlgorithm = null, int? AuthenticationDatalength = null)
                : base()
            {
                this.Key = Key;
                this.Data = Data;
                this.StoredKey = StoredKey;
                this.Iv = Iv;
                this.Padding = Padding;
                this.Compression = Compression;
                this.CryptoMethod = CryptoMethod;
                this.HashAlgorithm = HashAlgorithm;
                this.AuthenticationDatalength = AuthenticationDatalength;
            }

            /// <summary>
            /// Specifies the name of a key. The key
            /// [usage](#common.capabilities.completion.properties.crypto.authenticationattributes.s0) must one of the
            /// supported *authenticationAttributes*.
            /// <example>Key001</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// The data used to generate the authentication data.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "data")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Data { get; init; }

            /// <summary>
            /// This specifies the name of a key (usage 'I0') used as the Initialization Vector (IV).
            /// This property is null if not required.
            /// 
            /// <example>StoredIVKey</example>
            /// </summary>
            [DataMember(Name = "storedKey")]
            public string StoredKey { get; init; }

            [DataContract]
            public sealed class IvClass
            {
                public IvClass(string Key = null, List<byte> Value = null)
                {
                    this.Key = Key;
                    this.Value = Value;
                }

                /// <summary>
                /// The name of a key used to decrypt the *value*.
                /// This specifies the name of a key (usage 'K0') used to decrypt the *value*.
                /// This is only used when the *key* usage is 'D0' and *cryptoMethod* is either CBC or
                /// CFB. If this property is null, *value* is used as the Initialization Vector.
                /// 
                /// <example>KeyToDecrypt</example>
                /// </summary>
                [DataMember(Name = "key")]
                public string Key { get; init; }

                /// <summary>
                /// The plaintext or encrypted IV for use with the CBC or CFB encryption methods.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "value")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Value { get; init; }

            }

            /// <summary>
            /// Specifies the Initialization Vector. This property is null if *storedKey* is used.
            /// </summary>
            [DataMember(Name = "iv")]
            public IvClass Iv { get; init; }

            /// <summary>
            /// Specifies the padding character to use for symmetric key encryption.
            /// <example>255</example>
            /// </summary>
            [DataMember(Name = "padding")]
            [DataTypes(Minimum = 0, Maximum = 255)]
            public int? Padding { get; init; }

            /// <summary>
            /// Specifies whether the data is to be compressed (blanks removed) before building the MAC. If this property is
            /// null, the compression is not applied. Otherwise this property value is the blank character (e.g. ' ' in ASCII
            /// or '@' in EBCDIC).
            /// <example>@</example>
            /// </summary>
            [DataMember(Name = "compression")]
            [DataTypes(Pattern = @"^[ @]$")]
            public string Compression { get; init; }

            public enum CryptoMethodEnum
            {
                RsassaPkcs1V15,
                RsassaPss
            }

            /// <summary>
            /// Specifies the.
            /// [cryptographic method](#common.capabilities.completion.properties.crypto.authenticationattributes.m0.t.g.cryptomethod)
            /// to use.
            /// 
            /// If the *key* usage is an asymmetric key pair signature usage (e.g. 'S0') this can be one of the
            /// following values:
            /// 
            /// * ```rsassaPkcs1V15``` - Use the RSASSA-PKCS1-v1.5 algorithm.
            /// * ```rsassaPss``` - Use the RSASSA-PSS algorithm.
            /// 
            /// If the *key* usage is a MAC usage (e.g. 'M0') this property should be null.
            /// </summary>
            [DataMember(Name = "cryptoMethod")]
            public CryptoMethodEnum? CryptoMethod { get; init; }

            public enum HashAlgorithmEnum
            {
                Sha1,
                Sha256
            }

            /// <summary>
            /// Specifies the
            /// [hash algorithm](#common.capabilities.completion.properties.crypto.authenticationattributes.m0.t.g.hashalgorithm)
            /// to use.
            /// 
            /// If the *key* usage is an asymmetric key pair signature usage (e.g. 'S0') this can be one of the
            /// following values:
            /// 
            /// * ```sha1``` - The SHA1 digest algorithm.
            /// * ```sha256``` - The SHA 256 digest algorithm, as defined in ISO/IEC 10118-3:2004.
            /// [[Ref. crypto-1](#ref-crypto-1)] and FIPS 180-2 [[Ref. crypto-2](#ref-crypto-2)].
            /// 
            /// If the *key* usage is a MAC usage (e.g. 'M0') this property will be ignored. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "hashAlgorithm")]
            public HashAlgorithmEnum? HashAlgorithm { get; init; }

            /// <summary>
            /// The required authentication data length.
            /// 
            /// If the *key* usage is an asymmetric key pair signature usage (e.g. 'S0') this property will be ignored.
            /// </summary>
            [DataMember(Name = "authenticationDatalength")]
            [DataTypes(Minimum = 4, Maximum = 8)]
            public int? AuthenticationDatalength { get; init; }

        }
    }
}
