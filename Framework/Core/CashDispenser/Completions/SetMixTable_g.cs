/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * SetMixTable_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashDispenser.Completions
{
    [DataContract]
    [Completion(Name = "CashDispenser.SetMixTable")]
    public sealed class SetMixTableCompletion : Completion<SetMixTableCompletion.PayloadData>
    {
        public SetMixTableCompletion(int RequestId, SetMixTableCompletion.PayloadData Payload)
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
                InvalidMixNumber,
                InvalidMixTable
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```invalidMixNumber``` - The *mixNumber* is reserved for a predefined mix algorithm.
            /// * ```invalidMixTable``` - The contents of at least one of the defined rows of the mix table is incorrect.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
