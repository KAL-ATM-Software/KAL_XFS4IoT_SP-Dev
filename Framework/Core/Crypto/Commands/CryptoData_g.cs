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

            public PayloadData(int Timeout, string Key = null, string StartValueKey = null, string StartValue = null, int? Padding = null, string CryptData = null, CryptoAttributesClass CryptoAttributes = null)
                : base(Timeout)
            {
                this.Key = Key;
                this.StartValueKey = StartValueKey;
                this.StartValue = StartValue;
                this.Padding = Padding;
                this.CryptData = CryptData;
                this.CryptoAttributes = CryptoAttributes;
            }

            /// <summary>
            /// Specifies the name of the stored key.
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// If startValue specifies an Initialization Vector (IV), then this parameter specifies the name of the
            /// stored key used to decrypt the startValue to obtain the IV. If startValue is omitted and this
            /// parameter is set, then this parameter specifies the name of the IV that has been previously imported
            /// via TR-31. If this parameter is not set, *startValue* is used as the Initialization Vector.
            /// </summary>
            [DataMember(Name = "startValueKey")]
            public string StartValueKey { get; init; }

            /// <summary>
            /// The Base64 encoded initialization vector for CBC / CFB encryption. 
            /// If this property and *startValueKey* are both omitted the default value for CBC / CFB is all zeroes.
            /// </summary>
            [DataMember(Name = "startValue")]
            public string StartValue { get; init; }

            /// <summary>
            /// Specifies the padding character. The valid range is 0 to 255.
            /// </summary>
            [DataMember(Name = "padding")]
            [DataTypes(Minimum = 0, Maximum = 255)]
            public int? Padding { get; init; }

            /// <summary>
            /// The Base64 encoded data to be encrypted or decrypted.
            /// </summary>
            [DataMember(Name = "cryptData")]
            public string CryptData { get; init; }

            [DataContract]
            public sealed class CryptoAttributesClass
            {
                public CryptoAttributesClass(CryptoMethodEnum? CryptoMethod = null)
                {
                    this.CryptoMethod = CryptoMethod;
                }

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
                /// Specifies the cryptographic method supported by the [Crypto.CryptoData](#crypto.cryptodata) command. For symmetric encryption
                /// methods (keyUsage is ‘D0’), this can be one of the following values:
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
                public CryptoMethodEnum? CryptoMethod { get; init; }

            }

            /// <summary>
            /// This parameter specifies the encryption algorithm, cryptographic method, and mode to be used for this
            /// command. For a list of valid values see [cryptoAttributes](#common.capabilities.completion.properties.crypto.cryptoattributes) capability. The values specified must be
            /// compatible with the key identified by Key.
            /// </summary>
            [DataMember(Name = "cryptoAttributes")]
            public CryptoAttributesClass CryptoAttributes { get; init; }

        }
    }
}
