/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * GetMixTable_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Dispenser.Completions
{
    [DataContract]
    [Completion(Name = "Dispenser.GetMixTable")]
    public sealed class GetMixTableCompletion : Completion<GetMixTableCompletion.PayloadData>
    {
        public GetMixTableCompletion(int RequestId, GetMixTableCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, int? MixNumber = null, string Name = null, List<double> MixHeader = null, List<MixRowsClass> MixRows = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.MixNumber = MixNumber;
                this.Name = Name;
                this.MixHeader = MixHeader;
                this.MixRows = MixRows;
            }

            public enum ErrorCodeEnum
            {
                InvalidMixNumber
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```invalidMixNumber``` - The *mixNumber* parameter does not correspond to a defined mix table.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Number identifying the house mix table.
            /// </summary>
            [DataMember(Name = "mixNumber")]
            public int? MixNumber { get; init; }

            /// <summary>
            /// Name of the house mix table.
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

            /// <summary>
            /// Array of floating point numbers; each element defines the value of the item corresponding to its respective column.
            /// </summary>
            [DataMember(Name = "mixHeader")]
            public List<double> MixHeader { get; init; }

            [DataContract]
            public sealed class MixRowsClass
            {
                public MixRowsClass(double? Amount = null, List<int> Mixture = null)
                {
                    this.Amount = Amount;
                    this.Mixture = Mixture;
                }

                /// <summary>
                /// Amount denominated by this mix row.
                /// </summary>
                [DataMember(Name = "amount")]
                public double? Amount { get; init; }

                /// <summary>
                /// A mix row, an array of integers; each element defines the quantity of each item denomination in the mix used in the denomination of *amount*. 
                /// The value of each array element is defined by the *mixHeader*.
                /// </summary>
                [DataMember(Name = "mixture")]
                public List<int> Mixture { get; init; }

            }

            /// <summary>
            /// Array of rows of the mix table.
            /// </summary>
            [DataMember(Name = "mixRows")]
            public List<MixRowsClass> MixRows { get; init; }

        }
    }
}
