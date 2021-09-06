/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * MaintainPin_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.PinPad.Commands
{
    //Original name = MaintainPin
    [DataContract]
    [Command(Name = "PinPad.MaintainPin")]
    public sealed class MaintainPinCommand : Command<MaintainPinCommand.PayloadData>
    {
        public MaintainPinCommand(int RequestId, MaintainPinCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, bool? MaintainPIN = null)
                : base(Timeout)
            {
                this.MaintainPIN = MaintainPIN;
            }

            /// <summary>
            /// Specifies if the PIN should be maintained after a PIN processing command. Once set, this setting 
            /// applies until changed through another call to this command
            /// </summary>
            [DataMember(Name = "maintainPIN")]
            public bool? MaintainPIN { get; init; }

        }
    }
}
