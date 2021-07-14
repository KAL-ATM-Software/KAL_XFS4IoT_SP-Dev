/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * QueryIFMIdentifier_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.QueryIFMIdentifier")]
    public sealed class QueryIFMIdentifierCompletion : Completion<QueryIFMIdentifierCompletion.PayloadData>
    {
        public QueryIFMIdentifierCompletion(int RequestId, QueryIFMIdentifierCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, IfmAuthorityEnum? IfmAuthority = null, string IfmIdentifier = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.IfmAuthority = IfmAuthority;
                this.IfmIdentifier = IfmIdentifier;
            }

            public enum IfmAuthorityEnum
            {
                Emv,
                Europay,
                Visa,
                Giecb
            }

            /// <summary>
            /// Specifies the IFM authority that issued the IFM identifier:
            /// 
            /// * ```emv``` - The Level 1 Type Approval IFM identifier assigned by EMVCo.
            /// * ```europay``` - The Level 1 Type Approval IFM identifier assigned by Europay.
            /// * ```visa``` - The Level 1 Type Approval IFM identifier assigned by VISA.
            /// * ```giecb``` - The IFM identifier assigned by GIE Cartes Bancaires.
            /// </summary>
            [DataMember(Name = "ifmAuthority")]
            public IfmAuthorityEnum? IfmAuthority { get; init; }

            /// <summary>
            /// The IFM Identifier of the chip card reader (or IFM) as assigned by the specified authority.
            /// </summary>
            [DataMember(Name = "ifmIdentifier")]
            public string IfmIdentifier { get; init; }

        }
    }
}
