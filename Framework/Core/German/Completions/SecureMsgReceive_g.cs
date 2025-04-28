/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * SecureMsgReceive_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.German.Completions
{
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Completion(Name = "German.SecureMsgReceive")]
    public sealed class SecureMsgReceiveCompletion : Completion<SecureMsgReceiveCompletion.PayloadData>
    {
        public SecureMsgReceiveCompletion(int RequestId, SecureMsgReceiveCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                AccessDenied,
                HsmStateInvalid,
                MacInvalid,
                ProtocolInvalid,
                FormatInvalid,
                ContentInvalid,
                KeyNotFound
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor-specific reason.
            /// * ```hsmStateInvalid``` - The HSM is not in a correct state to handle this message.
            /// * ```macInvalid``` - The MAC of the message is not correct.
            /// * ```protocolInvalid``` - The specified protocol is invalid.
            /// * ```formatInvalid``` - The format of the message is invalid.
            /// * ```contentInvalid``` - The contents of one of the security relevant properties are invalid.
            /// * ```keyNotFound``` - No key was found for PAC/MAC generation.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
