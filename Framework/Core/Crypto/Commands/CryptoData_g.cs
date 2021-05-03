/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * CryptoData_g.cs uses automatically generated parts. 
 * created at 4/20/2021 12:28:05 PM
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
        public CryptoDataCommand(string RequestId, CryptoDataCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            /// <summary>
            /// This parameter specifies the encryption algorithm, cryptographic method, and mode to be used for this
            /// command. For a list of valid values see the [Capability.Attributes](#common.capabilities.completion.properties.crypto.cryptoattributes) field. The values specified must be
            /// compatible with the key identified by Key.
            /// </summary>
            public class CryptoAttributesClass
            {
                public enum AlgorithmEnum
                {
                    A,
                    D,
                    R,
                    T,
                }
                [DataMember(Name = "algorithm")] 
                public AlgorithmEnum? Algorithm { get; private set; }
                public enum ModeOfUseEnum
                {
                    D,
                    E,
                }
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
                    RsaesOaep,
                }
                [DataMember(Name = "cryptoMethod")] 
                public CryptoMethodEnum? CryptoMethod { get; private set; }

                public CryptoAttributesClass (AlgorithmEnum? Algorithm, ModeOfUseEnum? ModeOfUse, CryptoMethodEnum? CryptoMethod)
                {
                    this.Algorithm = Algorithm;
                    this.ModeOfUse = ModeOfUse;
                    this.CryptoMethod = CryptoMethod;
                }


            }


            public PayloadData(int Timeout, string Key = null, string StartValueKey = null, string StartValue = null, int? Padding = null, string CryptData = null, object CryptoAttributes = null)
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
            public string Key { get; private set; }
            /// <summary>
            /// If startValue specifies an Initialization Vector (IV), then this parameter specifies the name of the
            /// stored key used to decrypt the startValue to obtain the IV. If startValue is not set and this
            /// parameter is set, then this parameter specifies the name of the IV that has been previously imported
            /// via TR-31. If this parameter is not set, startValue is used as the Initialization Vector.
            /// </summary>
            [DataMember(Name = "startValueKey")] 
            public string StartValueKey { get; private set; }
            /// <summary>
            /// The initialization vector for CBC / CFB encryption. If this parameter and startValueKey are both not
            /// set the default value for CBC / CFB is all zeroes.
            /// </summary>
            [DataMember(Name = "startValue")] 
            public string StartValue { get; private set; }
            /// <summary>
            /// Commonly used padding data.
            /// </summary>
            [DataMember(Name = "padding")] 
            public int? Padding { get; private set; }
            /// <summary>
            /// The data to be encrypted or decrypted formatted in Base64.
            /// </summary>
            [DataMember(Name = "cryptData")] 
            public string CryptData { get; private set; }
            /// <summary>
            /// This parameter specifies the encryption algorithm, cryptographic method, and mode to be used for this
            /// command. For a list of valid values see the [Capability.Attributes](#common.capabilities.completion.properties.crypto.cryptoattributes) field. The values specified must be
            /// compatible with the key identified by Key.
            /// </summary>
            [DataMember(Name = "cryptoAttributes")] 
            public object CryptoAttributes { get; private set; }

        }
    }
}
