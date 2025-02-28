/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ReplaceCertificate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "KeyManagement.ReplaceCertificate")]
    public sealed class ReplaceCertificateCompletion : Completion<ReplaceCertificateCompletion.PayloadData>
    {
        public ReplaceCertificateCompletion(int RequestId, ReplaceCertificateCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> NewCertificateData = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.NewCertificateData = NewCertificateData;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied,
                FormatInvalid,
                InvalidCertificateState
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            ///   specific reason.
            /// * ```formatInvalid``` - The format of the message is invalid.
            /// * ```invalidCertificateState``` - The certificate module is in a state in which the request is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The PKCS#7 (See [[Ref. keymanagement-1](#ref-keymanagement-1)]) using a Digested-data content type. The
            /// digest parameter should contain the thumb print value.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "newCertificateData")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> NewCertificateData { get; init; }

        }
    }
}
