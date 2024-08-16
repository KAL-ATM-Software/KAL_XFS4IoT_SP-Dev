/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ImportEmvPublicKey_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "KeyManagement.ImportEmvPublicKey")]
    public sealed class ImportEmvPublicKeyCompletion : Completion<ImportEmvPublicKeyCompletion.PayloadData>
    {
        public ImportEmvPublicKeyCompletion(int RequestId, ImportEmvPublicKeyCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, string ExpiryDate = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.ExpiryDate = ExpiryDate;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied,
                DuplicateKey,
                NoKeyRam,
                EmvVerifyFailed,
                KeyNotFound,
                UseViolation
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            /// specific reason.
            /// * ```duplicateKey``` - A *key* exists with that name and cannot be overwritten.
            /// * ```noKeyRam``` - There is no space left in the key RAM for a key of the specified type.
            /// * ```emvVerifyFailed``` - The verification of the imported key failed and the key was discarded.
            /// * ```keyNotFound``` - The specified *key* was not found.
            /// * ```useViolation``` - The specified *keyUsage* is not supported by this key.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Contains the expiry date of the certificate in the following format MMYY. If null, the certificate does
            /// not have an expiry date.
            /// <example>0123</example>
            /// </summary>
            [DataMember(Name = "expiryDate")]
            public string ExpiryDate { get; init; }

        }
    }
}
