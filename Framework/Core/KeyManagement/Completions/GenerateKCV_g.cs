/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * GenerateKCV_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [Completion(Name = "KeyManagement.GenerateKCV")]
    public sealed class GenerateKCVCompletion : Completion<GenerateKCVCompletion.PayloadData>
    {
        public GenerateKCVCompletion(int RequestId, GenerateKCVCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string Kcv = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Kcv = Kcv;
            }

            public enum ErrorCodeEnum
            {
                KeyNotFound,
                KeyNoValue,
                AccessDenied,
                ModeNotSupported
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```keyNotFound``` - The specified key encryption key was not found.
            /// * ```keyNoValue``` - The specified key exists but has no value loaded.
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific reason.
            /// * ```modeNotSupported``` - The KCV mode is not supported.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Contains the Base64 encoded key check value data that can be used for verification of the key. 
            /// </summary>
            [DataMember(Name = "kcv")]
            public string Kcv { get; init; }

        }
    }
}
