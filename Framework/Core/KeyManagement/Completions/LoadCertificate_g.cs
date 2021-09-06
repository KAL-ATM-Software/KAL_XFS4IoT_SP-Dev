/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * LoadCertificate_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [Completion(Name = "KeyManagement.LoadCertificate")]
    public sealed class LoadCertificateCompletion : Completion<LoadCertificateCompletion.PayloadData>
    {
        public LoadCertificateCompletion(int RequestId, LoadCertificateCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, RsaKeyCheckModeEnum? RsaKeyCheckMode = null, string RsaData = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.RsaKeyCheckMode = RsaKeyCheckMode;
                this.RsaData = RsaData;
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

            public enum RsaKeyCheckModeEnum
            {
                None,
                Sha1,
                Sha256
            }

            /// <summary>
            /// Defines algorithm/method used to generate the public key check value/thumb print.
            /// The check value can be used to verify that the public key has been imported correctly.
            /// 
            /// The following values are possible:
            /// * ```none``` - No check value is returned in *rsaData* property.
            /// * ```sha1``` - The *rsaData* property contains a sha-1 digest of the public key.
            /// * ```sha256``` - The *rsaData* contains a sha-256 digest of the public key.
            /// </summary>
            [DataMember(Name = "rsaKeyCheckMode")]
            public RsaKeyCheckModeEnum? RsaKeyCheckMode { get; init; }

            /// <summary>
            /// The Base64 encoded PKCS #7 structure using a Digested-data content type.
            /// The digest parameter should contain the thumb print value calculated by the algorithm 
            /// specified by rsaKeyCheckMode. If rsaKeyCheckMode is none, then this field is not be set or an empty string.
            /// </summary>
            [DataMember(Name = "rsaData")]
            public string RsaData { get; init; }

        }
    }
}
