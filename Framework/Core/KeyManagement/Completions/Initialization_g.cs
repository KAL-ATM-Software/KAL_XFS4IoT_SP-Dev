/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * Initialization_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [Completion(Name = "KeyManagement.Initialization")]
    public sealed class InitializationCompletion : Completion<InitializationCompletion.PayloadData>
    {
        public InitializationCompletion(int RequestId, InitializationCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string Identification = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Identification = Identification;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied,
                InvalidId
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific reason.
            /// * ```invalidId``` - The ID passed was not valid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The Base64 encoded value of the ID key encrypted by the encryption key.
            /// This value can be used as authorization for the [KeyManagement.ImportKey](#keymanagement.importkey) command, this 
            /// field is not set if no authorization required.
            /// 
            /// </summary>
            [DataMember(Name = "identification")]
            public string Identification { get; init; }

        }
    }
}
