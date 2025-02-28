/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "TextTerminal.SetResolution")]
    public sealed class SetResolutionCommand : Command<SetResolutionCommand.PayloadData>
    {
        public SetResolutionCommand(int RequestId, SetResolutionCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ResolutionClass Resolution = null)
                : base()
            {
                this.Resolution = Resolution;
            }

            /// <summary>
            /// This must be one of the supported [resolutions](#common.capabilities.completion.properties.textterminal.resolutions).
            /// </summary>
            [DataMember(Name = "resolution")]
            public ResolutionClass Resolution { get; init; }

        }
    }
}
