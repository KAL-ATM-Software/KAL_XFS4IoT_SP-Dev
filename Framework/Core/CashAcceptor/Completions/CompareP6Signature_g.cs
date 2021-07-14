/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CompareP6Signature_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.CompareP6Signature")]
    public sealed class CompareP6SignatureCompletion : Completion<CompareP6SignatureCompletion.PayloadData>
    {
        public CompareP6SignatureCompletion(int RequestId, CompareP6SignatureCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, List<P6SignaturesIndexClass> P6SignaturesIndex = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.P6SignaturesIndex = P6SignaturesIndex;
            }

            public enum ErrorCodeEnum
            {
                CashInActive,
                ExchangeActive,
                InvalidReferenceSignature,
                InvalidTransactionSignature
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// "cashInActive": A cash-in transaction is active.
            /// 
            /// "exchangeActive": The device is in the exchange state.
            /// 
            /// "invalidReferenceSignature": At least one of the reference signatures is invalid. The application should 
            /// prompt the operator to carefully retry the creation of the reference signatures.
            /// 
            /// "invalidTransactionSignature": At least one of the transaction signatures is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class P6SignaturesIndexClass
            {
                public P6SignaturesIndexClass(int? Index = null, int? ConfidenceLevel = null, string ComparisonData = null)
                {
                    this.Index = Index;
                    this.ConfidenceLevel = ConfidenceLevel;
                    this.ComparisonData = ComparisonData;
                }

                /// <summary>
                /// Specifies the index (zero to #*p6Signatures* - 1) of the matching signature from the input parameter *p6Signatures*.
                /// </summary>
                [DataMember(Name = "index")]
                public int? Index { get; init; }

                /// <summary>
                /// Specifies the level of confidence for the match found. This value is in a scale 1 - 100, where 100 is the maximum confidence level. This value is zero if the Service does not support the confidence level factor.
                /// </summary>
                [DataMember(Name = "confidenceLevel")]
                public int? ConfidenceLevel { get; init; }

                /// <summary>
                /// Vendor dependent comparison result data. This data may be used as justification for the signature match or confidence level. This field is omitted if no additional comparison data is returned.
                /// </summary>
                [DataMember(Name = "comparisonData")]
                public string ComparisonData { get; init; }

            }

            /// <summary>
            /// Array of compare results. This array is empty when the compare operation completes with no match found.
            /// If there are matches found, *p6SignaturesIndex* contains the indexes of the matching signatures from the input parameter *p6Signatures*.
            /// If there is a match found but the Service does not support the confidence level factor, *p6SignaturesIndex* contains a single index with confidenceLevel set to zero.
            /// </summary>
            [DataMember(Name = "p6SignaturesIndex")]
            public List<P6SignaturesIndexClass> P6SignaturesIndex { get; init; }

        }
    }
}
