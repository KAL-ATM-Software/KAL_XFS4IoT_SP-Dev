/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * DeriveKey_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [Completion(Name = "KeyManagement.DeriveKey")]
    public sealed class DeriveKeyCompletion : Completion<DeriveKeyCompletion.PayloadData>
    {
        public DeriveKeyCompletion(int RequestId, DeriveKeyCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                KeyNotFound,
                AccessDenied,
                InvalidId,
                DuplicateKey,
                KeyNoValue,
                UseViolation,
                InvalidKeyLength,
                AlgorithmNotSupported
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```keyNotFound``` - The specified key was not found.
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific reason.
            /// * ```invalidId``` - The ID passed was not valid.
            /// * ```duplicateKey``` - A key exists with that name and cannot be overwritten.
            /// * ```keyNoValue``` - The specified key is not loaded.
            /// * ```useViolation``` - The specified use is not supported by this key.
            /// * ```invalidKeyLength``` - The length of startValue is not supported or the length of an encryption key is not compatible 
            /// with the encryption operation required.
            /// * ```algorithmNotSupported``` - The specified algorithm is not supported.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
