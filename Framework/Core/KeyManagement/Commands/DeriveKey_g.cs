/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "KeyManagement.DeriveKey")]
    public sealed class DeriveKeyCommand : Command<DeriveKeyCommand.PayloadData>
    {
        public DeriveKeyCommand(int RequestId, DeriveKeyCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(DerivationAlgorithmEnum? DerivationAlgorithm = null, string Key = null, string KeyGenKey = null, string StoredKey = null, IvClass Iv = null, int? Padding = null, List<byte> InputData = null)
                : base()
            {
                this.DerivationAlgorithm = DerivationAlgorithm;
                this.Key = Key;
                this.KeyGenKey = KeyGenKey;
                this.StoredKey = StoredKey;
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
                /// CFB. if this property is null, *value* is used as the Initialization Vector.
                /// 
                /// <example>KeyToDecrypt</example>
                /// </summary>
                [DataMember(Name = "key")]
                public string Key { get; init; }

                /// <summary>
                /// The plaintext or encrypted IV for use with the CBC or CFB encryption methods.
                /// <example>VGhlIGluaXRpYWxpemF0 ...</example>
                /// </summary>
                [DataMember(Name = "value")]
                [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
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
            /// Data to be used for key derivation.
            /// <example>a2V5IGRlcml2YXRpb24g ...</example>
            /// </summary>
            [DataMember(Name = "inputData")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> InputData { get; init; }

        }
    }
}
