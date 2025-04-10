/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * GenerateRSAKeyPair_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.KeyManagement.Commands
{
    //Original name = GenerateRSAKeyPair
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "KeyManagement.GenerateRSAKeyPair")]
    public sealed class GenerateRSAKeyPairCommand : Command<GenerateRSAKeyPairCommand.PayloadData>
    {
        public GenerateRSAKeyPairCommand(int RequestId, GenerateRSAKeyPairCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Key = null, UseEnum? Use = null, int? ModulusLength = null, ExponentValueEnum? ExponentValue = null)
                : base()
            {
                this.Key = Key;
                this.Use = Use;
                this.ModulusLength = ModulusLength;
                this.ExponentValue = ExponentValue;
            }

            /// <summary>
            /// Specifies the name of the new key pair to be generated. Details of the generated key pair can be
            /// obtained through the [KeyManagement.GetKeyDetail](#keymanagement.getkeydetail) command.
            /// <example>Key02</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            public enum UseEnum
            {
                RsaPrivate,
                RsaPrivateSign
            }

            /// <summary>
            /// Specifies what the private key component of the key pair can be used for. The public key part can only
            /// be used for the inverse function. For example, if the *rsaPrivateSign* use is specified, then the
            /// private key can only be used for signature  generation and the partner public key can only be used for
            /// verification. The following values are possible:
            /// 
            /// * ```rsaPrivate``` - Key is used as a private key for RSA decryption.
            /// * ```rsaPrivateSign``` - Key is used as a private key for RSA Signature generation. Only data generated
            ///   within the device can be signed.
            /// </summary>
            [DataMember(Name = "use")]
            public UseEnum? Use { get; init; }

            /// <summary>
            /// Specifies the number of bits for the modulus of the RSA key pair to be generated.
            /// When zero is specified then the device will be responsible for defining the length.
            /// </summary>
            [DataMember(Name = "modulusLength")]
            [DataTypes(Minimum = 0)]
            public int? ModulusLength { get; init; }

            public enum ExponentValueEnum
            {
                Device,
                Exponent1,
                Exponent4,
                Exponent16
            }

            /// <summary>
            /// Specifies the value of the exponent of the RSA key pair to be generated.
            /// The following values are possible:
            /// 
            /// * ```device``` - The device will decide the exponent.
            /// * ```exponent1``` - Exponent of 2[sup]1[/sup]+1 (3).
            /// * ```exponent4``` - Exponent of 2[sup]4[/sup]+1 (17).
            /// * ```exponent16``` - Exponent of 2[sup]16[/sup]+1 (65537).
            /// </summary>
            [DataMember(Name = "exponentValue")]
            public ExponentValueEnum? ExponentValue { get; init; }

        }
    }
}
