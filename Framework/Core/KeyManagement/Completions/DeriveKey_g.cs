/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
                AccessDenied,
                KeyNotFound,
                KeyNoValue,
                AlgorithmNotSupported,
                DuplicateKey,
                UseViolation,
                InvalidKeyLength
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            /// specific reason.
            /// * ```keyNotFound``` - The specified *keyGenKey* was not found.
            /// * ```keyNoValue``` - The specified *keyGenKey* is not loaded.
            /// * ```algorithmNotSupported``` - The specified *derivationAlgorithm* is not supported.
            /// * ```duplicateKey``` - A *key* exists with that name and cannot be overwritten.
            /// * ```useViolation``` - The specified *keyGenKey* usage does not support key derivation.
            /// * ```invalidKeyLength``` - The length of *iv* is not supported or the length of an encryption key
            /// is not compatible with the encryption operation required.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
