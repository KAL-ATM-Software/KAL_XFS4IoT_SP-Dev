/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ChipPower_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = ChipPower
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CardReader.ChipPower")]
    public sealed class ChipPowerCommand : Command<ChipPowerCommand.PayloadData>
    {
        public ChipPowerCommand(int RequestId, ChipPowerCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ChipPowerEnum? ChipPower = null)
                : base()
            {
                this.ChipPower = ChipPower;
            }

            public enum ChipPowerEnum
            {
                Cold,
                Warm,
                Off
            }

            /// <summary>
            /// Specifies the action to perform as one of the following:
            /// 
            /// * ```cold``` - The chip is powered on and reset.
            /// * ```warm``` - The chip is reset.
            /// * ```off``` - The chip is powered off.
            /// </summary>
            [DataMember(Name = "chipPower")]
            public ChipPowerEnum? ChipPower { get; init; }

        }
    }
}
