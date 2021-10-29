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

            public PayloadData(int Timeout, string Key = null, string StartValueKey = null, string StartValue = null, int? Padding = null, bool? Compression = null, string AuthenticationData = null, string VerifyData = null, VerifyAttributesClass VerifyAttributes = null)
                : base(Timeout)
            {
                this.Key = Key;
                this.StartValueKey = StartValueKey;
                this.StartValue = StartValue;
                this.Padding = Padding;
                this.Compression = Compression;
                this.AuthenticationData = AuthenticationData;
                this.VerifyData = VerifyData;
                this.VerifyAttributes = VerifyAttributes;
            }

            /// <summary>
            /// Specifies the name of the stored key.
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// If startValue specifies an Initialization Vector (IV), then this property specifies the name of the
            /// stored key used to decrypt the startValue to obtain the IV. If *startValue* is not set and this property
            /// is also not set, then this property specifies the name of the IV that has been previously imported via
            /// X9.143. If this property is not set, *startValue* is used as the Initialization Vector.
            /// </summary>
            [DataMember(Name = "startValueKey")]
            public string StartValueKey { get; init; }

            /// <summary>
            /// The initialization vector for CBC / CFB encryption. If this parameter and *startValueKey* are both not
            /// set the default value for CBC / CFB is all zeroes.
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
            /// Specifies whether data is to be compressed (blanks removed) before building the MAC. If compression is
            /// 0x00 no compression is selected, otherwise compression holds the representation of the blank character
            /// (e.g. 0x20 in ASCII or 0x40 in EBCDIC).
            /// </summary>
            [DataMember(Name = "compression")]
            public bool? Compression { get; init; }

            /// <summary>
            /// The device will either generate a MAC or sign the Base64 encoded *authenticationData* and compare with *verifyData* property.
            /// </summary>
            [DataMember(Name = "authenticationData")]
            public string AuthenticationData { get; init; }

            /// <summary>
            /// The Base64 encoded authentication data to be verified by MAC or signature.
            /// </summary>
            [DataMember(Name = "verifyData")]
            public string VerifyData { get; init; }

            [DataContract]
            public sealed class VerifyAttributesClass
            {
                public VerifyAttributesClass(CryptoMethodEnum? CryptoMethod = null, HashAlgorithmEnum? HashAlgorithm = null)
                {
                    this.CryptoMethod = CryptoMethod;
                    this.HashAlgorithm = HashAlgorithm;
                }

                public enum CryptoMethodEnum
                {
                    RsassaPkcs1V15,
                    RsassaPss
                }

                /// <summary>
                /// Specifies the cryptographic method [cryptoMethod](#common.capabilities.completion.properties.crypto.verifyattributes.s0.r.s.cryptomethod) supported. For asymmetric
                /// signature verification methods (Specified [key](#crypto.verifyauthentication.command.properties.key) is key usage ['S0', 'S1', or 'S2'](#common.capabilities.completion.properties.crypto.verifyattributes.s0)), this can be one of the
                /// following values. 
                /// 
                /// * ```rsassaPkcs1V15``` - Use the RSASSA-PKCS1-v1.5 algorithm. 
                /// * ```rsassaPss``` - Use the RSASSA-PSS algorithm. 
                /// 
                /// If the specified [key](#crypto.verifyauthentication.command.properties.key) is any of the MAC usages (i.e. ['M1'](#common.capabilities.completion.properties.crypto.verifyattributes.s0)), then this property can be omitted.
                /// </summary>
                [DataMember(Name = "cryptoMethod")]
                public CryptoMethodEnum? CryptoMethod { get; init; }

                public enum HashAlgorithmEnum
                {
                    Sha1,
                    Sha256
                }

                /// <summary>
                /// For asymmetric signature verification methods (Specified [key](#crypto.verifyauthentication.command.properties.key) is key usage ['S0', 'S1', or 'S2'](#common.capabilities.completion.properties.crypto.verifyattributes.s0)), this can be one
                /// of the following values to be used. If the specified [key](#crypto.verifyauthentication.command.properties.key) is any of the MAC usages (i.e. ['M1'](#common.capabilities.completion.properties.crypto.verifyattributes.s0)),
                /// then this property can be omitted.
                /// 
                /// * ```sha1``` - The SHA 1 digest algorithm.
                /// * ```sha256``` - The SHA 256 digest algorithm, as defined in ISO/IEC 10118-3:2004 and FIPS 180-2.
                /// </summary>
                [DataMember(Name = "hashAlgorithm")]
                public HashAlgorithmEnum? HashAlgorithm { get; init; }

            }

            /// <summary>
            /// This parameter specifies the encryption algorithm, cryptographic method, and mode to be used for this
            /// command. For a list of valid values see the Attributes [verifyAttributes](#common.capabilities.completion.properties.crypto.verifyattributes) 
            /// property. The values specified must be compatible with the key identified by Key.
            /// </summary>
            [DataMember(Name = "verifyAttributes")]
            public VerifyAttributesClass VerifyAttributes { get; init; }

        }
    }
}
