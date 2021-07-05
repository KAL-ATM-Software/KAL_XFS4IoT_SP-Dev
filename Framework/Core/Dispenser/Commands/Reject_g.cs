/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * Reject_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Dispenser.Commands
{
    //Original name = Reject
    [DataContract]
    [Command(Name = "Dispenser.Reject")]
    public sealed class RejectCommand : Command<RejectCommand.PayloadData>
    {
        public RejectCommand(int RequestId, RejectCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout)
                : base(Timeout)
            {
            }

        }
    }
}
