/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * GetCertificate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "KeyManagement.GetCertificate")]
    public sealed class GetCertificateCompletion : Completion<GetCertificateCompletion.PayloadData>
    {
        public GetCertificateCompletion(int RequestId, GetCertificateCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> Certificate = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Certificate = Certificate;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied,
                InvalidCertificateState,
                KeyNotFound
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            ///   specific reason.
            /// * ```invalidCertificateState``` - The certificate module is in a state in which the request is invalid.
            /// * ```keyNotFound``` - The specified public key was not found.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Contains the certificate that is to be loaded represented in DER encoded ASN.1 notation.
            /// This data should be in a binary encoded PKCS#7 (See [[Ref. keymanagement-1](#ref-keymanagement-1)])
            /// using the degenerate certificate only case of the signed-data content type in which the inner content's
            /// data file is omitted and there are no signers.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "certificate")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Certificate { get; init; }

        }
    }
}
