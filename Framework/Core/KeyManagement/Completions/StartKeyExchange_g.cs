/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * StartKeyExchange_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.KeyManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "KeyManagement.StartKeyExchange")]
    public sealed class StartKeyExchangeCompletion : Completion<StartKeyExchangeCompletion.PayloadData>
    {
        public StartKeyExchangeCompletion()
            : base()
        { }

        public StartKeyExchangeCompletion(int RequestId, StartKeyExchangeCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> RandomItem = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.RandomItem = RandomItem;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor
            ///   specific reason.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The randomly generated number created by the device.
            /// 
            /// This value is null if the device does not support random number generation for data authentication.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "randomItem")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> RandomItem { get; init; }

        }
    }
}
