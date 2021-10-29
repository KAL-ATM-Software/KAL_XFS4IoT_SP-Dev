/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetReplenishTarget_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.GetReplenishTarget")]
    public sealed class GetReplenishTargetCompletion : Completion<GetReplenishTargetCompletion.PayloadData>
    {
        public GetReplenishTargetCompletion(int RequestId, GetReplenishTargetCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<TargetsClass> Targets = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Targets = Targets;
            }

            [DataContract]
            public sealed class TargetsClass
            {
                public TargetsClass(string Target = null)
                {
                    this.Target = Target;
                }

                /// <summary>
                /// Object name of the storage unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
                /// command) that can be used as a target.
                /// <example>unit1</example>
                /// </summary>
                [DataMember(Name = "target")]
                public string Target { get; init; }

            }

            /// <summary>
            /// Array of all suitable replenish targets. Empty if no suitable target was found.
            /// </summary>
            [DataMember(Name = "targets")]
            public List<TargetsClass> Targets { get; init; }

        }
    }
}
