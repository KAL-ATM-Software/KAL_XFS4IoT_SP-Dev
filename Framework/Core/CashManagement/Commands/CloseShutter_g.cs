/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CloseShutter_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = CloseShutter
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashManagement.CloseShutter")]
    public sealed class CloseShutterCommand : Command<CloseShutterCommand.PayloadData>
    {
        public CloseShutterCommand(int RequestId, CloseShutterCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(PositionEnum? Position = null)
                : base()
            {
                this.Position = Position;
            }

            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

        }
    }
}
