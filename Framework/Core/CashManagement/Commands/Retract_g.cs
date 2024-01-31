/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * Retract_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = Retract
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashManagement.Retract")]
    public sealed class RetractCommand : Command<RetractCommand.PayloadData>
    {
        public RetractCommand(int RequestId, RetractCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(RetractClass Location = null)
                : base()
            {
                this.Location = Location;
            }

            /// <summary>
            /// Specifies where items are to be retracted from and where they are to be retracted to.
            /// </summary>
            [DataMember(Name = "location")]
            public RetractClass Location { get; init; }

        }
    }
}
