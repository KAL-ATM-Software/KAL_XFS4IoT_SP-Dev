/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * SetTellerInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashManagement.SetTellerInfo")]
    public sealed class SetTellerInfoCompletion : Completion<SetTellerInfoCompletion.PayloadData>
    {
        public SetTellerInfoCompletion(int RequestId, SetTellerInfoCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                InvalidCurrency,
                InvalidTellerId,
                UnsupportedPosition,
                ExchangeActive
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```invalidCurrency``` - The specified currency is not currently available.
            /// * ```invalidTellerId``` - The teller ID is invalid.
            /// * ```unsupportedPosition``` - The position specified is not supported.
            /// * ```exchangeActive``` - The target teller is currently in the middle of an exchange operation.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
