/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Crypto.CryptoData")]
    public sealed class CryptoDataCommand : Command<CryptoDataCommand.PayloadData>
    {
        public CryptoDataCommand(int RequestId, CryptoDataCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Key = null, string StoredKey = null, IvClass Iv = null, int? Padding = null, string ModeOfUse = null, CryptoMethodEnum? CryptoMethod = null, List<byte> Data = null)
                : base()
            {
                this.Key = Key;
                this.StoredKey = StoredKey;
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
            /// The *key* [Mode of Use](#common.capabilities.completion.properties.crypto.cryptoattributes.d0.d.d)
            /// qualifier.
            /// 
            /// If the *key* Mode Of Use is 'B', this qualifies the Mode of Use as one of the following values:
            /// 
            /// * ```D``` - Decrypt / Unwrap Only.
            /// * ```E``` - Encrypt / Wrap Only.
            /// 
            /// If the *key* Mode of Use is not 'B', this should be null.
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
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "data")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Data { get; init; }

        }
    }
}
