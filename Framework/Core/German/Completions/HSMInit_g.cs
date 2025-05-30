/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * HSMInit_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.German.Completions
{
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Completion(Name = "German.HSMInit")]
    public sealed class HSMInitCompletion : Completion<HSMInitCompletion.PayloadData>
    {
        public HSMInitCompletion(int RequestId, HSMInitCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                ModeNotSupported,
                HsmStateInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```modeNotSupported``` - The specified init mode is not supported.
            /// * ```hsmStateInvalid``` - The HSM is not in a correct state to handle this command.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
