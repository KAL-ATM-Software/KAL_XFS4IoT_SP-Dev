/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, Dictionary<string, string> IfmIdentifiers = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.IfmIdentifiers = IfmIdentifiers;
            }

            [DataMember(Name = "ifmIdentifiers")]
            public Dictionary<string, string> IfmIdentifiers { get; init; }

        }
    }
}
