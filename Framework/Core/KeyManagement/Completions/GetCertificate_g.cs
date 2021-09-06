/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "KeyManagement.GetCertificate")]
    public sealed class GetCertificateCompletion : Completion<GetCertificateCompletion.PayloadData>
    {
        public GetCertificateCompletion(int RequestId, GetCertificateCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string Certificate = null)
                : base(CompletionCode, ErrorDescription)
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
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific reason.
            /// * ```invalidCertificateState``` - The certificate module is in a state in which the request is invalid.
            /// * ```keyNotFound``` - The specified public key was not found.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Contains the certificate that is to be loaded represented in DER encoded ASN.1 notation.
            /// This data should be in a binary encoded PKCS #7 using the degenerate certificate only case of the signed-data content 
            /// type in which the inner content's data file is omitted and there are no signers.
            /// </summary>
            [DataMember(Name = "certificate")]
            public string Certificate { get; init; }

        }
    }
}
