/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * SecureMsgSend_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.German.Completions
{
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Completion(Name = "German.SecureMsgSend")]
    public sealed class SecureMsgSendCompletion : Completion<SecureMsgSendCompletion.PayloadData>
    {
        public SecureMsgSendCompletion()
            : base()
        { }

        public SecureMsgSendCompletion(int RequestId, SecureMsgSendCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> Msg = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Msg = Msg;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied,
                HsmStateInvalid,
                ProtocolInvalid,
                FormatInvalid,
                ContentInvalid,
                KeyNotFound,
                NoPin
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor-specific reason.
            /// * ```hsmStateInvalid``` - The HSM is not in a correct state to handle this message.
            /// * ```protocolInvalid``` - The specified protocol is invalid.
            /// * ```formatInvalid``` - The format of the message is invalid.
            /// * ```contentInvalid``` - The contents of one of the security relevant properties are invalid.
            /// * ```keyNotFound``` - No key was found for PAC/MAC generation.
            /// * ```noPin``` - No PIN or insufficient PIN-digits have been entered.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The modified message that can be sent to an authorization system or personalization system.
            /// This property is null if not applicable.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "msg")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> Msg { get; init; }

        }
    }
}
