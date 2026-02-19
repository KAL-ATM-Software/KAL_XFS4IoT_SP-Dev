/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.GetReplenishTarget")]
    public sealed class GetReplenishTargetCompletion : Completion<GetReplenishTargetCompletion.PayloadData>
    {
        public GetReplenishTargetCompletion()
            : base()
        { }

        public GetReplenishTargetCompletion(int RequestId, GetReplenishTargetCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<TargetsClass> Targets = null)
                : base()
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
                /// The name of the storage unit (as stated by the [Storage.GetStorage](#storage.getstorage)
                /// command) that can be used as a target.
                /// <example>unit1</example>
                /// </summary>
                [DataMember(Name = "target")]
                [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
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
