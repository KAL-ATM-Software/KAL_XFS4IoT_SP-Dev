/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashDispenser.GetMixTable")]
    public sealed class GetMixTableCompletion : Completion<GetMixTableCompletion.PayloadData>
    {
        public GetMixTableCompletion(int RequestId, GetMixTableCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, int? MixNumber = null, string Name = null, List<MixRowClass> MixRows = null)
                : base()
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
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```invalidMix``` - The *mix* property does not correspond to a defined mix table.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Number identifying the house mix table (optional).
            /// <example>21</example>
            /// </summary>
            [DataMember(Name = "mixNumber")]
            [DataTypes(Minimum = 1)]
            public int? MixNumber { get; init; }

            /// <summary>
            /// Name of the house mix table. Null if not defined.
            /// <example>House mix 21</example>
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

            /// <summary>
            /// Array of rows of the mix table.
            /// </summary>
            [DataMember(Name = "mixRows")]
            public List<MixRowClass> MixRows { get; init; }

        }
    }
}
