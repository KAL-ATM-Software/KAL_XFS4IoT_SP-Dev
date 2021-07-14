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
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = GetReplenishTarget
    [DataContract]
    [Command(Name = "CashAcceptor.GetReplenishTarget")]
    public sealed class GetReplenishTargetCommand : Command<GetReplenishTargetCommand.PayloadData>
    {
        public GetReplenishTargetCommand(int RequestId, GetReplenishTargetCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string CashunitSource = null)
                : base(Timeout)
            {
                this.CashunitSource = CashunitSource;
            }

            /// <summary>
            /// Object name of the cash unit (as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
            /// command) which would be used as the source of the replenishment operation.
            /// </summary>
            [DataMember(Name = "cashunitSource")]
            public string CashunitSource { get; init; }

        }
    }
}
