/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetTellerInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashManagement.GetTellerInfo")]
    public sealed class GetTellerInfoCompletion : Completion<GetTellerInfoCompletion.PayloadData>
    {
        public GetTellerInfoCompletion(int RequestId, GetTellerInfoCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<TellerDetailsClass> TellerDetails = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.TellerDetails = TellerDetails;
            }

            public enum ErrorCodeEnum
            {
                InvalidCurrency,
                InvalidTellerId
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```invalidCurrency``` - Specified currency not currently available.
            /// * ```invalidTellerId``` - Invalid teller ID.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Array of teller detail objects. May be null if no teller defined.
            /// </summary>
            [DataMember(Name = "tellerDetails")]
            public List<TellerDetailsClass> TellerDetails { get; init; }

        }
    }
}
