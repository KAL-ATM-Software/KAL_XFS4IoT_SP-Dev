/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetDepleteSource_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.GetDepleteSource")]
    public sealed class GetDepleteSourceCompletion : Completion<GetDepleteSourceCompletion.PayloadData>
    {
        public GetDepleteSourceCompletion(int RequestId, GetDepleteSourceCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<DepleteSourcesClass> DepleteSources = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.DepleteSources = DepleteSources;
            }

            [DataContract]
            public sealed class DepleteSourcesClass
            {
                public DepleteSourcesClass(string CashunitSource = null)
                {
                    this.CashunitSource = CashunitSource;
                }

                /// <summary>
                /// Object name of the cash unit (as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
                /// command) that can be used as a source.
                /// </summary>
                [DataMember(Name = "cashunitSource")]
                public string CashunitSource { get; init; }

            }

            /// <summary>
            /// Array of all suitable deplete sources. Empty if no suitable source was found.
            /// </summary>
            [DataMember(Name = "depleteSources")]
            public List<DepleteSourcesClass> DepleteSources { get; init; }

        }
    }
}
