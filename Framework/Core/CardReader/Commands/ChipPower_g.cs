/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ChipPower_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = ChipPower
    [DataContract]
    [Command(Name = "CardReader.ChipPower")]
    public sealed class ChipPowerCommand : Command<ChipPowerCommand.PayloadData>
    {
        public ChipPowerCommand(string RequestId, ChipPowerCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ChipPowerEnum
            {
                Cold,
                Warm,
                Off,
            }


            public PayloadData(int Timeout, ChipPowerEnum? ChipPower = null)
                : base(Timeout)
            {
                this.ChipPower = ChipPower;
            }

            /// <summary>
            ///Specifies the action to perform as one of the following:**cold**
            ////The chip is powered on and reset.**warm**
            ////The chip is reset.**off**
            ////The chip is powered off.
            /// </summary>
            [DataMember(Name = "chipPower")] 
            public ChipPowerEnum? ChipPower { get; private set; }

        }
    }
}
