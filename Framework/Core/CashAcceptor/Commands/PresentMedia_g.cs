/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * PresentMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = PresentMedia
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashAcceptor.PresentMedia")]
    public sealed class PresentMediaCommand : Command<PresentMediaCommand.PayloadData>
    {
        public PresentMediaCommand(int RequestId, PresentMediaCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CashManagement.PositionEnum? Position = null)
                : base()
            {
                this.Position = Position;
            }

            [DataMember(Name = "position")]
            public CashManagement.PositionEnum? Position { get; init; }

        }
    }
}
