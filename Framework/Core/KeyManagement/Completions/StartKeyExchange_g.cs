/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "KeyManagement.StartKeyExchange")]
    public sealed class StartKeyExchangeCompletion : Completion<StartKeyExchangeCompletion.PayloadData>
    {
        public StartKeyExchangeCompletion(int RequestId, StartKeyExchangeCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string RandomItem = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.RandomItem = RandomItem;
            }

            public enum ErrorCodeEnum
            {
                AccessDenied
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific reason.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The Base64 encoded randomly generated number created by the encryptor.
            /// If the device does not support random number generation and verification, a zero length random number is returned 
            /// and an empty string is returned.
            /// </summary>
            [DataMember(Name = "randomItem")]
            public string RandomItem { get; init; }

        }
    }
}
