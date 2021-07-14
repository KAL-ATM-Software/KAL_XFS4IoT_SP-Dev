/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * SetMode_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = SetMode
    [DataContract]
    [Command(Name = "CashAcceptor.SetMode")]
    public sealed class SetModeCommand : Command<SetModeCommand.PayloadData>
    {
        public SetModeCommand(int RequestId, SetModeCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, MixedModeEnum? MixedMode = null)
                : base(Timeout)
            {
                this.MixedMode = MixedMode;
            }

            public enum MixedModeEnum
            {
                MixedMediaNotActive,
                MixedMedia
            }

            /// <summary>
            /// Specifies the Mixed Media mode of the device. Following values are possible:
            /// 
            /// "mixedMediaNotActive": Mixed Media transactions are deactivated. This is the default mode.
            /// 
            /// "mixedMedia": Mixed Media transactions are activated in combination with the ItemProcessor interface as defined by the capability *mixedMode*.
            /// </summary>
            [DataMember(Name = "mixedMode")]
            public MixedModeEnum? MixedMode { get; init; }

        }
    }
}
