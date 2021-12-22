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

            public PayloadData(int Timeout, DerivationAlgorithmEnum? DerivationAlgorithm = null, string Key = null, string KeyGenKey = null, string IvKey = null, List<byte> Iv = null, int? Padding = null, List<byte> InputData = null)
                : base(Timeout)
            {
                this.DerivationAlgorithm = DerivationAlgorithm;
                this.Key = Key;
                this.KeyGenKey = KeyGenKey;
                this.IvKey = IvKey;
                this.Iv = Iv;
                this.Padding = Padding;
                this.InputData = InputData;
            }

            public enum DerivationAlgorithmEnum
            {
                ChipZka
            }

            /// <summary>
            /// Specifies the algorithm that is used for derivation. See
            /// [derivationAlgorithms](#common.capabilities.completion.properties.keymanagement.derivationalgorithms))
            /// for the supported valued.
            /// </summary>
            [DataMember(Name = "derivationAlgorithm")]
            public DerivationAlgorithmEnum? DerivationAlgorithm { get; init; }

            /// <summary>
            /// Specifies the name where the derived key will be stored. 
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// Specifies the name of the key generating key that is used for the derivation.
            /// <example>Key02</example>
            /// </summary>
            [DataMember(Name = "keyGenKey")]
            public string KeyGenKey { get; init; }

            /// <summary>
            /// Specifies the name of the stored key used to decrypt the *iv* to obtain the Initialization
            /// Vector.
            /// 
            /// If this field is omitted, *iv* is used as the Initialization Vector.
            /// <example>IVKey01</example>
            /// </summary>
            [DataMember(Name = "ivKey")]
            public string IvKey { get; init; }

            /// <summary>
            /// DES initialization vector for the encryption step within the derivation.
            /// <example>REVTIGluaXRpYWxpemF0 ...</example>
            /// </summary>
            [DataMember(Name = "iv")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> Iv { get; init; }

            /// <summary>
            /// Specifies the padding character for the encryption step within the derivation.
            /// </summary>
            [DataMember(Name = "padding")]
            [DataTypes(Minimum = 0, Maximum = 255)]
            public int? Padding { get; init; }

            /// <summary>
            /// Data to be used for key derivation. 
            /// <example>a2V5IGRlcml2YXRpb24g ...</example>
            /// </summary>
            [DataMember(Name = "inputData")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> InputData { get; init; }

        }
    }
}
