/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CompareSignature_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.CompareSignature")]
    public sealed class CompareSignatureCompletion : Completion<CompareSignatureCompletion.PayloadData>
    {
        public CompareSignatureCompletion(int RequestId, CompareSignatureCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<SignaturesIndexClass> SignaturesIndex = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.SignaturesIndex = SignaturesIndex;
            }

            public enum ErrorCodeEnum
            {
                CashInActive,
                ExchangeActive,
                InvalidReferenceSignature,
                InvalidTransactionSignature
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```cashInActive``` - A cash-in transaction is active. This device requires that no cash-in
            ///   transaction is active in order to perform the command.
            /// * ```exchangeActive``` - The device is in the exchange state.
            /// * ```invalidReferenceSignature``` - At least one of the reference signatures is invalid. The application should
            ///   prompt the operator to carefully retry the creation of the reference signatures.
            /// * ```invalidTransactionSignature``` - At least one of the transaction signatures is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class SignaturesIndexClass
            {
                public SignaturesIndexClass(int? Index = null, int? ConfidenceLevel = null, string ComparisonData = null)
                {
                    this.Index = Index;
                    this.ConfidenceLevel = ConfidenceLevel;
                    this.ComparisonData = ComparisonData;
                }

                /// <summary>
                /// Specifies the index (0 to #*signatures* - 1) of the matching signature from the input property *signatures*.
                /// </summary>
                [DataMember(Name = "index")]
                [DataTypes(Minimum = 0)]
                public int? Index { get; init; }

                /// <summary>
                /// Specifies the level of confidence for the match found. This value is in a scale 1 - 100, where 100 is the
                /// maximum confidence level. This value is 0 if the Service does not support the confidence level factor.
                /// <example>95</example>
                /// </summary>
                [DataMember(Name = "confidenceLevel")]
                [DataTypes(Minimum = 0, Maximum = 100)]
                public int? ConfidenceLevel { get; init; }

                /// <summary>
                /// Vendor dependent comparison result data. This data may be used as justification for the signature match or
                /// confidence level. This property is null if no additional comparison data is returned.
                /// <example>Example comparison data.</example>
                /// </summary>
                [DataMember(Name = "comparisonData")]
                public string ComparisonData { get; init; }

            }

            /// <summary>
            /// Array of compare results. This is null when the compare operation completes with no matches found.
            /// If there are matches found, *signaturesIndex* contains the indices of the matching signatures from the
            /// input property *signatures*.
            /// If there is a match found but the Service does not support the confidence level factor, *signaturesIndex*
            /// contains a single index with confidenceLevel set to 0.
            /// </summary>
            [DataMember(Name = "signaturesIndex")]
            public List<SignaturesIndexClass> SignaturesIndex { get; init; }

        }
    }
}
