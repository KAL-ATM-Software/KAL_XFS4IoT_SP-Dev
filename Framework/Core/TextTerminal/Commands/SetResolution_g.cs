/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * SetResolution_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = SetResolution
    [DataContract]
    [Command(Name = "TextTerminal.SetResolution")]
    public sealed class SetResolutionCommand : Command<SetResolutionCommand.PayloadData>
    {
        public SetResolutionCommand(int RequestId, SetResolutionCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, ResolutionClass Resolution = null)
                : base(Timeout)
            {
                this.Resolution = Resolution;
            }

            /// <summary>
            /// Specifies the horizontal size of the display of the text terminal unit.
            /// </summary>
            [DataMember(Name = "resolution")]
            public ResolutionClass Resolution { get; private set; }

        }
    }
}
