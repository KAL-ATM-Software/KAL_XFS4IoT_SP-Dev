/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * GetMixTable_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashDispenser.Commands
{
    //Original name = GetMixTable
    [DataContract]
    [Command(Name = "CashDispenser.GetMixTable")]
    public sealed class GetMixTableCommand : Command<GetMixTableCommand.PayloadData>
    {
        public GetMixTableCommand(int RequestId, GetMixTableCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Mix = null)
                : base(Timeout)
            {
                this.Mix = Mix;
            }

            /// <summary>
            /// A house mix table as defined by one of the [mixes](#cashdispenser.getmixtypes.completion.properties.) reported by [CashDispenser.GetMixTypes](#cashdispenser.getmixtypes).
            /// <example>table21</example>
            /// </summary>
            [DataMember(Name = "mix")]
            public string Mix { get; init; }

        }
    }
}
