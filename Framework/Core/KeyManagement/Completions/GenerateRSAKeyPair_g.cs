/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * GenerateRSAKeyPair_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [Completion(Name = "KeyManagement.GenerateRSAKeyPair")]
    public sealed class GenerateRSAKeyPairCompletion : Completion<GenerateRSAKeyPairCompletion.PayloadData>
    {
        public GenerateRSAKeyPairCompletion(int RequestId, GenerateRSAKeyPairCompletion.PayloadData Payload)
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
                InvalidModulusLength,
                UseViolation,
                DuplicateKey,
                KeyGenerationError
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific reason.
            /// * ```invalidModulusLength``` - The modulus length specified is invalid.
            /// * ```useViolation``` - The specified use is not supported by this key.
            /// * ```duplicateKey``` - A key exists with that name and cannot be overwritten.
            /// * ```keyGenerationError``` - The EPP is unable to generate a key pair.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
