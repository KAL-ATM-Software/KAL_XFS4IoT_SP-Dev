/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * SupplyReplenish_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Check.Commands
{
    //Original name = SupplyReplenish
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Check.SupplyReplenish")]
    public sealed class SupplyReplenishCommand : Command<SupplyReplenishCommand.PayloadData>
    {
        public SupplyReplenishCommand(int RequestId, SupplyReplenishCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(bool? Toner = null, bool? Ink = null)
                : base()
            {
                this.Toner = Toner;
                this.Ink = Ink;
            }

            /// <summary>
            /// Specifies whether the toner supply was replenished.
            /// <example>true</example>
            /// </summary>
            [DataMember(Name = "toner")]
            public bool? Toner { get; init; }

            /// <summary>
            /// Specifies whether the ink supply was replenished.
            /// <example>false</example>
            /// </summary>
            [DataMember(Name = "ink")]
            public bool? Ink { get; init; }

        }
    }
}
