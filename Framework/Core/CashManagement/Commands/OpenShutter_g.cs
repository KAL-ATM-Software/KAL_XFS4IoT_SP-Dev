/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * OpenShutter_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = OpenShutter
    [DataContract]
    [Command(Name = "CashManagement.OpenShutter")]
    public sealed class OpenShutterCommand : Command<OpenShutterCommand.PayloadData>
    {
        public OpenShutterCommand(int RequestId, OpenShutterCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, PositionEnum? Position = null)
                : base(Timeout)
            {
                this.Position = Position;
            }

            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

        }
    }
}
