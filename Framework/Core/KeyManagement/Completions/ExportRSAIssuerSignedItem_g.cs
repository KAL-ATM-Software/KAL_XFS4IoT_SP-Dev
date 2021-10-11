/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ExportRSAIssuerSignedItem_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [Completion(Name = "KeyManagement.ExportRSAIssuerSignedItem")]
    public sealed class ExportRSAIssuerSignedItemCompletion : Completion<ExportRSAIssuerSignedItemCompletion.PayloadData>
    {
        public ExportRSAIssuerSignedItemCompletion(int RequestId, ExportRSAIssuerSignedItemCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string Value = null, RSASignatureAlgorithmEnum? RsaSignatureAlgorithm = null, string Signature = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Value = Value;
                this.RsaSignatureAlgorithm = RsaSignatureAlgorithm;
                this.Signature = Signature;
            }

            public enum ErrorCodeEnum
            {
                NoRSAKeyPair,
                AccessDenied,
                KeyNotFound
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```noRSAKeyPair``` - The device does not have a private key.
            /// * ```accessDenied``` - The device is either not initialized or not ready for any vendor specific reason.
            /// * ```keyNotFound``` - The data item identified by name was not found.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// If a public key was requested then value contains the PKCS #1 formatted RSA public key represented in
            /// DER encoded ASN.1 format. If the security item was requested then value contains the PIN's Security
            /// Item, which may be vendor specific.
            /// </summary>
            [DataMember(Name = "value")]
            public string Value { get; init; }

            /// <summary>
            /// Specifies the algorithm, used to generate the Signature returned in signature, as one of the following:
            /// 
            /// * ```na``` - No signature algorithm used, no signature will be provided in signature, the data item may 
            ///   still be exported.  
            /// * ```rsassaPkcs1V15``` - RSASSA-PKCS1-v1.5 algorithm used.  
            /// * ```rsassaPss``` - RSASSA-PSS algorithm used.
            /// </summary>
            [DataMember(Name = "rsaSignatureAlgorithm")]
            public RSASignatureAlgorithmEnum? RsaSignatureAlgorithm { get; init; }

            /// <summary>
            /// Specifies the RSA signature of the data item exported formatted in base64. An empty sting can be returned when key signature are not supported. 
            /// </summary>
            [DataMember(Name = "signature")]
            public string Signature { get; init; }

        }
    }
}
