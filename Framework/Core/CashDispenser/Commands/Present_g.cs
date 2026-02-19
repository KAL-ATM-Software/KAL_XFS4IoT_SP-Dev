/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * Present_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashDispenser.Commands
{
    //Original name = Present
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashDispenser.Present")]
    public sealed class PresentCommand : Command<PresentCommand.PayloadData>
    {
        public PresentCommand()
            : base()
        { }

        public PresentCommand(int RequestId, PresentCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CashManagement.OutputPositionEnum? Position = null)
                : base()
            {
                this.Position = Position;
            }

            [DataMember(Name = "position")]
            public CashManagement.OutputPositionEnum? Position { get; init; }

        }
    }
}
