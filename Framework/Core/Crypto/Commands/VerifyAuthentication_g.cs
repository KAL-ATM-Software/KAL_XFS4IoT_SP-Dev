/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * VerifyAuthentication_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Crypto.Commands
{
    //Original name = VerifyAuthentication
    [DataContract]
    [Command(Name = "Crypto.VerifyAuthentication")]
    public sealed class VerifyAuthenticationCommand : Command<VerifyAuthenticationCommand.PayloadData>
    {
        public VerifyAuthenticationCommand(int RequestId, VerifyAuthenticationCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Key = null, List<byte> Data = null, List<byte> VerifyData = null, string IvKey = null, List<byte> Iv = null, int? Padding = null, string Compression = null, CryptoMethodEnum? CryptoMethod = null, HashAlgorithmEnum? HashAlgorithm = null)
                : base(Timeout)
            {
                this.Key = Key;
                this.Data = Data;
                this.VerifyData = VerifyData;
                this.IvKey = IvKey;
                this.Iv = Iv;
                this.Padding = Padding;
                this.Compression = Compression;
                this.CryptoMethod = CryptoMethod;
                this.HashAlgorithm = HashAlgorithm;
            }

            /// <summary>
            /// Specifies the name of the verification key. The key
            /// [usage](#common.capabilities.completion.properties.crypto.verifyattributes.m0) must one of the
            /// supported *verifyAttributes*.
            /// <example>Key001</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// The data to be authenticated. The service will generate authentication data (MAC or signature) using
            /// the *key*, *cryptoMethod* and "hashAlgorithm* then compare with *verifyData*.
            /// <example>R2VuZXJhdGUgYSBNQUMg ...</example>
            /// </summary>
            [DataMember(Name = "data")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> Data { get; init; }

            /// <summary>
            /// The authentication data to verify.
            /// <example>RGF0YSB0byBiZSB2ZXJp ...</example>
            /// </summary>
            [DataMember(Name = "verifyData")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> VerifyData { get; init; }

            /// <summary>
            /// The name of a key used to decrypt the *iv* or the name of an Initialization Vector (IV).
            /// 
            /// If *iv* is included, this specifies the name of a key (usage 'K0') used to decrypt the *iv*.
            /// 
            /// If *iv* is omitted, this specifies the name of an IV (usage 'I0').
            /// 
            /// This is only used when the *key* usage is a symmetric encryption key and *cryptoMethod* is either CBC or
            /// CFB.            
            /// 
            /// <example>IVKey</example>
            /// </summary>
            [DataMember(Name = "ivKey")]
            public string IvKey { get; init; }

            /// <summary>
            /// The plaintext or encrypted IV for use with the CBC or CFB encryption methods.
            /// 
            /// If *iv* and *ivKey* properties are omitted the default IV is all zeroes.
            /// <example>VGhlIGluaXRpYWxpemF0 ...</example>
            /// </summary>
            [DataMember(Name = "iv")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> Iv { get; init; }

            /// <summary>
            /// Specifies the padding character to use for symmetric key encryption.
            /// <example>255</example>
            /// </summary>
            [DataMember(Name = "padding")]
            [DataTypes(Minimum = 0, Maximum = 255)]
            public int? Padding { get; init; }

            /// <summary>
            /// Specifies whether the data is to be compressed (blanks removed) before building the MAC. If this property is
            /// omitted compression is not applied. Otherwise this property value is the  blank character (e.g. ' ' in ASCII
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
            /// [cryptographic method](#common.capabilities.completion.properties.crypto.verifyattributes.m0.t.v.cryptomethod)
            /// to use.
            /// 
            /// If the *key* usage is an asymmetric key pair signature usage (e.g. 'S0') this can be one of the
            /// following values:
            /// 
            /// * ```rsassaPkcs1V15``` - Use the RSASSA-PKCS1-v1.5 algorithm. 
            /// * ```rsassaPss``` - Use the RSASSA-PSS algorithm. 
            /// 
            /// If the *key* usage is a MAC usage (e.g. 'M0') this property should be omitted.
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
            /// [hash algorithm](#common.capabilities.completion.properties.crypto.verifyattributes.m0.t.v.hashalgorithm)
            /// to use.
            /// 
            /// If the *key* usage is an asymmetric key pair signature usage (e.g. 'S0') this can be one of the
            /// following values:
            /// 
            /// * ```sha1``` - The SHA1 digest algorithm.
            /// * ```sha256``` - The SHA 256 digest algorithm, as defined in ISO/IEC 10118-3:2004
            /// [[Ref. crypto-1](#ref-crypto-1)] and FIPS 180-2 [[Ref. crypto-2](#ref-crypto-2)].
            /// 
            /// If the *key* usage is a MAC usage (e.g. 'M0') this property should be omitted.)].
            /// </summary>
            [DataMember(Name = "hashAlgorithm")]
            public HashAlgorithmEnum? HashAlgorithm { get; init; }

        }
    }
}
