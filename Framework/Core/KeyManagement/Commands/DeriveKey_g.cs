/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * DeriveKey_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = DeriveKey
    [DataContract]
    [Command(Name = "KeyManagement.DeriveKey")]
    public sealed class DeriveKeyCommand : Command<DeriveKeyCommand.PayloadData>
    {
        public DeriveKeyCommand(int RequestId, DeriveKeyCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? DerivationAlgorithm = null, string Key = null, string KeyGenKey = null, string StartValueKey = null, string StartValue = null, int? Padding = null, string InputData = null)
                : base(Timeout)
            {
                this.DerivationAlgorithm = DerivationAlgorithm;
                this.Key = Key;
                this.KeyGenKey = KeyGenKey;
                this.StartValueKey = StartValueKey;
                this.StartValue = StartValue;
                this.Padding = Padding;
                this.InputData = InputData;
            }

            /// <summary>
            /// Specifies the algorithm that is used for derivation. Possible values are: (see [derivationAlgorithms](#common.capabilities.completion.properties.keymanagement.derivationalgorithms))
            /// </summary>
            [DataMember(Name = "derivationAlgorithm")]
            public int? DerivationAlgorithm { get; init; }

            /// <summary>
            /// Specifies the name where the derived key will be stored. 
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// Specifies the name of the key generating key that is used for the derivation.
            /// </summary>
            [DataMember(Name = "keyGenKey")]
            public string KeyGenKey { get; init; }

            /// <summary>
            /// Specifies the name of the stored key used to decrypt the *startValue* to obtain the Initialization Vector.
            /// If this field is omitted, *startValue* is used as the Initialization Vector.
            /// </summary>
            [DataMember(Name = "startValueKey")]
            public string StartValueKey { get; init; }

            /// <summary>
            /// DES initialization vector for the encryption step within the derivation.
            /// </summary>
            [DataMember(Name = "startValue")]
            public string StartValue { get; init; }

            /// <summary>
            /// Specifies the padding character for the encryption step within the derivation. The valid range is 0 to 255.
            /// </summary>
            [DataMember(Name = "padding")]
            [DataTypes(Minimum = 0, Maximum = 255)]
            public int? Padding { get; init; }

            /// <summary>
            /// Data to be used for key derivation. 
            /// </summary>
            [DataMember(Name = "inputData")]
            public string InputData { get; init; }

        }
    }
}
