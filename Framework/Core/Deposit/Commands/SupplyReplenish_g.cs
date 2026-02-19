/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * SupplyReplenish_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Deposit.Commands
{
    //Original name = SupplyReplenish
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "Deposit.SupplyReplenish")]
    public sealed class SupplyReplenishCommand : Command<SupplyReplenishCommand.PayloadData>
    {
        public SupplyReplenishCommand()
            : base()
        { }

        public SupplyReplenishCommand(int RequestId, SupplyReplenishCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(bool? Envelope = null, bool? Toner = null)
                : base()
            {
                this.Envelope = Envelope;
                this.Toner = Toner;
            }

            /// <summary>
            /// The envelope supply was replenished.
            /// <example>true</example>
            /// </summary>
            [DataMember(Name = "envelope")]
            public bool? Envelope { get; init; }

            /// <summary>
            /// The toner supply was replenished.
            /// <example>false</example>
            /// </summary>
            [DataMember(Name = "toner")]
            public bool? Toner { get; init; }

        }
    }
}
