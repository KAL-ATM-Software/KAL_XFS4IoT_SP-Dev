/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * StartAuthenticate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "KeyManagement.StartAuthenticate")]
    public sealed class StartAuthenticateCompletion : Completion<StartAuthenticateCompletion.PayloadData>
    {
        public StartAuthenticateCompletion(int RequestId, StartAuthenticateCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<byte> DataToSign = null, SignersClass Signers = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.DataToSign = DataToSign;
                this.Signers = Signers;
            }

            /// <summary>
            /// The data that must be authenticated by one of the authorities indicated by *methods* before the
            /// *command* can be executed. If the *command* does not require authentication, this property is null
            /// and the command result is success.
            /// <example>QXV0aGVudGljYXRpb24g ...</example>
            /// </summary>
            [DataMember(Name = "dataToSign")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> DataToSign { get; init; }

            [DataContract]
            public sealed class SignersClass
            {
                public SignersClass(bool? CertHost = null, bool? SigHost = null, bool? Ca = null, bool? Hl = null, bool? Cbcmac = null, bool? Cmac = null, bool? CertHostTr34 = null, bool? CaTr34 = null, bool? HlTr34 = null, bool? Reserved1 = null, bool? Reserved2 = null, bool? Reserved3 = null)
                {
                    this.CertHost = CertHost;
                    this.SigHost = SigHost;
                    this.Ca = Ca;
                    this.Hl = Hl;
                    this.Cbcmac = Cbcmac;
                    this.Cmac = Cmac;
                    this.CertHostTr34 = CertHostTr34;
                    this.CaTr34 = CaTr34;
                    this.HlTr34 = HlTr34;
                    this.Reserved1 = Reserved1;
                    this.Reserved2 = Reserved2;
                    this.Reserved3 = Reserved3;
                }

                /// <summary>
                /// The current host can be used to generate authentication data, using the RSA certificate-based
                /// scheme.
                /// </summary>
                [DataMember(Name = "certHost")]
                public bool? CertHost { get; init; }

                /// <summary>
                /// The current host can be used to generate authentication data, using the RSA signature-based scheme.
                /// </summary>
                [DataMember(Name = "sigHost")]
                public bool? SigHost { get; init; }

                /// <summary>
                /// The Certificate Authority (CA) can be used to generate authentication data.
                /// </summary>
                [DataMember(Name = "ca")]
                public bool? Ca { get; init; }

                /// <summary>
                /// The Higher Level (HL) Authority can be used to generate authentication data.
                /// </summary>
                [DataMember(Name = "hl")]
                public bool? Hl { get; init; }

                /// <summary>
                /// A CBC MAC key can be used to generate authentication data.
                /// </summary>
                [DataMember(Name = "cbcmac")]
                public bool? Cbcmac { get; init; }

                /// <summary>
                /// A CMAC key can be used to generate authentication data.
                /// </summary>
                [DataMember(Name = "cmac")]
                public bool? Cmac { get; init; }

                /// <summary>
                /// The current host can be used to generate authentication data compliant with TR-34.
                /// </summary>
                [DataMember(Name = "certHostTr34")]
                public bool? CertHostTr34 { get; init; }

                /// <summary>
                /// The Certificate Authority (CA) can be used to generate the authentication data compliant with TR-34.
                /// </summary>
                [DataMember(Name = "caTr34")]
                public bool? CaTr34 { get; init; }

                /// <summary>
                /// The Higher Level (HL) Authority can be used to generate authentication data compliant with TR-34.
                /// </summary>
                [DataMember(Name = "hlTr34")]
                public bool? HlTr34 { get; init; }

                /// <summary>
                /// The authentication data is generated using a vendor specific generation method.
                /// </summary>
                [DataMember(Name = "reserved1")]
                public bool? Reserved1 { get; init; }

                /// <summary>
                /// The authentication data is generated using a vendor specific generation method.
                /// </summary>
                [DataMember(Name = "reserved2")]
                public bool? Reserved2 { get; init; }

                /// <summary>
                /// The authentication data is generated using a vendor specific generation method.
                /// </summary>
                [DataMember(Name = "reserved3")]
                public bool? Reserved3 { get; init; }

            }

            /// <summary>
            /// Specifies the methods which may be used to generate authentication data. If *dataToSign* is not null,
            /// at least one method must be true.
            /// </summary>
            [DataMember(Name = "signers")]
            public SignersClass Signers { get; init; }

        }
    }
}
