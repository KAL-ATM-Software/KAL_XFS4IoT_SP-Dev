/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "KeyManagement.ReplaceCertificate")]
    public sealed class ReplaceCertificateCompletion : Completion<ReplaceCertificateCompletion.PayloadData>
    {
        public ReplaceCertificateCompletion(int RequestId, ReplaceCertificateCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string NewCertificateData = null)
                : base(CompletionCode, ErrorDescription)
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
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific reason.
            /// * ```formatInvalid``` - The format of the message is invalid.
            /// * ```invalidCertificateState``` - The certificate module is in a state in which the request is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The Base64 encoded PKCS #7 using a Digested-data content type.
            /// The digest parameter should contain the thumb print value.
            /// </summary>
            [DataMember(Name = "newCertificateData")]
            public string NewCertificateData { get; init; }

        }
    }
}
