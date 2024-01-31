/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT MixedMedia interface.
 * SetMode_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.MixedMedia.Commands
{
    //Original name = SetMode
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "MixedMedia.SetMode")]
    public sealed class SetModeCommand : Command<SetModeCommand.PayloadData>
    {
        public SetModeCommand(int RequestId, SetModeCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ModesClass Modes = null)
                : base()
            {
                this.Modes = Modes;
            }

            /// <summary>
            /// Specifies the required mixed media modes.
            /// </summary>
            [DataMember(Name = "modes")]
            public ModesClass Modes { get; init; }

        }
    }
}
