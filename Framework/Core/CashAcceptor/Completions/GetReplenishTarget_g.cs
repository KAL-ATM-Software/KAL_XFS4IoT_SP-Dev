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

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<ReplenishTargetsClass> ReplenishTargets = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ReplenishTargets = ReplenishTargets;
            }

            [DataContract]
            public sealed class ReplenishTargetsClass
            {
                public ReplenishTargetsClass(string CashunitTarget = null)
                {
                    this.CashunitTarget = CashunitTarget;
                }

                /// <summary>
                /// Object name of the cash unit (as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
                /// command) that can be used as a target.
                /// </summary>
                [DataMember(Name = "cashunitTarget")]
                public string CashunitTarget { get; private set; }

            }

            /// <summary>
            /// Array of all suitable replenish targets. Empty if no suitable target was found.
            /// </summary>
            [DataMember(Name = "replenishTargets")]
            public List<ReplenishTargetsClass> ReplenishTargets { get; private set; }

        }
    }
}
