/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * GetMixTypes_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashDispenser.Completions
{
    [DataContract]
    [Completion(Name = "CashDispenser.GetMixTypes")]
    public sealed class GetMixTypesCompletion : Completion<GetMixTypesCompletion.PayloadData>
    {
        public GetMixTypesCompletion(int RequestId, GetMixTypesCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, Dictionary<string, MixClass> Mixes = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Mixes = Mixes;
            }

            /// <summary>
            /// Object containing mix specifications. The property name of each mix can be used as the _mix_ in the 
            /// [CashDispenser.GetMixTable](#cashdispenser.getmixtable), [CashDispenser.Dispense](#cashdispenser.dispense)
            /// and [CashDispenser.Denominate](#cashdispenser.denominate) commands.
            /// </summary>
            [DataMember(Name = "mixes")]
            public Dictionary<string, MixClass> Mixes { get; init; }

        }
    }
}
