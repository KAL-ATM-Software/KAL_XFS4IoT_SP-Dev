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

            public PayloadData(int Timeout, string Source = null)
                : base(Timeout)
            {
                this.Source = Source;
            }

            /// <summary>
            /// Object name of the storage unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
            /// command) which would be used as the source of the replenishment operation.
            /// <example>unit2</example>
            /// </summary>
            [DataMember(Name = "source")]
            public string Source { get; init; }

        }
    }
}
