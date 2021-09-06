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

            public PayloadData(int Timeout, int? MixNumber = null)
                : base(Timeout)
            {
                this.MixNumber = MixNumber;
            }

            /// <summary>
            /// Number of the requested house mix table.
            /// </summary>
            [DataMember(Name = "mixNumber")]
            public int? MixNumber { get; init; }

        }
    }
}
