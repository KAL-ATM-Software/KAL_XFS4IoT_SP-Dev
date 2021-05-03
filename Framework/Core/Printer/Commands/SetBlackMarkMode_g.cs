/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * SetBlackMarkMode_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = SetBlackMarkMode
    [DataContract]
    [Command(Name = "Printer.SetBlackMarkMode")]
    public sealed class SetBlackMarkModeCommand : Command<SetBlackMarkModeCommand.PayloadData>
    {
        public SetBlackMarkModeCommand(string RequestId, SetBlackMarkModeCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum BlackMarkModeEnum
            {
                On,
                Off,
            }


            public PayloadData(int Timeout, BlackMarkModeEnum? BlackMarkMode = null)
                : base(Timeout)
            {
                this.BlackMarkMode = BlackMarkMode;
            }

            /// <summary>
            /// Specifies the desired black mark detection mode as one of the following:
            /// 
            /// * ```on``` - Turns the black mark detection and associated functionality on.
            /// * ```off``` - Turns the black mark detection and associated functionality off.
            /// </summary>
            [DataMember(Name = "blackMarkMode")] 
            public BlackMarkModeEnum? BlackMarkMode { get; private set; }

        }
    }
}
