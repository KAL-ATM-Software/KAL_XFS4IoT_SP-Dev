/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * GetMixTable_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashDispenser.Completions
{
    [DataContract]
    [Completion(Name = "CashDispenser.GetMixTable")]
    public sealed class GetMixTableCompletion : Completion<GetMixTableCompletion.PayloadData>
    {
        public GetMixTableCompletion(int RequestId, GetMixTableCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, int? MixNumber = null, string Name = null, List<Dictionary<string, int>> MixRows = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.MixNumber = MixNumber;
                this.Name = Name;
                this.MixRows = MixRows;
            }

            public enum ErrorCodeEnum
            {
                InvalidMix
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```invalidMix``` - The _mix_ property does not correspond to a defined mix table.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Number identifying the house mix table.
            /// <example>21</example>
            /// </summary>
            [DataMember(Name = "mixNumber")]
            [DataTypes(Minimum = 1)]
            public int? MixNumber { get; init; }

            /// <summary>
            /// Name of the house mix table.
            /// <example>House mix 21</example>
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

            /// <summary>
            /// Array of rows of the mix table.
            /// </summary>
            [DataMember(Name = "mixRows")]
            public List<Dictionary<string, int>> MixRows { get; init; }

        }
    }
}
