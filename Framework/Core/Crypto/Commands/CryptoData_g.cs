/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * CryptoData_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Crypto.Commands
{
    //Original name = CryptoData
    [DataContract]
    [Command(Name = "Crypto.CryptoData")]
    public sealed class CryptoDataCommand : Command<CryptoDataCommand.PayloadData>
    {
        public CryptoDataCommand(int RequestId, CryptoDataCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Key = null, string IvKey = null, List<byte> Iv = null, int? Padding = null, string ModeOfUse = null, CryptoMethodEnum? CryptoMethod = null, List<byte> Data = null)
                : base(Timeout)
            {
                this.Key = Key;
                this.IvKey = IvKey;
                this.Iv = Iv;
                this.Padding = Padding;
                this.ModeOfUse = ModeOfUse;
                this.CryptoMethod = CryptoMethod;
                this.Data = Data;
            }

            /// <summary>
            /// Specifies the name of the encryption key. The key
            /// [usage](#common.capabilities.completion.properties.crypto.cryptoattributes.d0) must one of the
            /// supported *cryptoAttributes*.
            /// <example>Key001</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

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
            /// The *key* [Mode of Use](#common.capabilities.completion.properties.crypto.cryptoattributes.d0.d.d)
            /// qualifier.
            /// 
            /// If the *key* Mode Of Use is 'B', this qualifies the Mode of Use as one of the following values: 
            /// 
            /// * ```D``` - Decrypt / Unwrap Only. 
            /// * ```E``` - Encrypt / Wrap Only. 
            /// 
            /// If the *key* Mode of Use is not 'B', this should be omitted.
            /// <example>E</example>
            /// </summary>
            [DataMember(Name = "modeOfUse")]
            [DataTypes(Pattern = @"^[DE]$")]
            public string ModeOfUse { get; init; }

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
            /// Specifies the cryptographic method to use.
            /// 
            /// If the *key* usage is 'D0', this can be one of the following values:
            /// 
            /// * ```ecb``` - The ECB encryption method.
            /// * ```cbc``` - The CBC encryption method.
            /// * ```cfb``` - The CFB encryption method.
            /// * ```ofb``` - The OFB encryption method.
            /// * ```ctr``` - The CTR method defined in NIST SP800-38A.
            /// * ```xts``` - The XTS method defined in NIST SP800-38E.
            /// 
            /// If the *key* usage is 'D1', this can be one of the following values:
            /// 
            /// * ```rsaesPkcs1V15``` - Use the RSAES_PKCS1-v1.5 algorithm.
            /// * ```rsaesOaep``` - Use the RSAES OAEP algorithm.
            /// </summary>
            [DataMember(Name = "cryptoMethod")]
            public CryptoMethodEnum? CryptoMethod { get; init; }

            /// <summary>
            /// The data to be encrypted or decrypted.
            /// <example>U2FtcGxlIERhdGE=</example>
            /// </summary>
            [DataMember(Name = "data")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> Data { get; init; }

        }
    }
}
