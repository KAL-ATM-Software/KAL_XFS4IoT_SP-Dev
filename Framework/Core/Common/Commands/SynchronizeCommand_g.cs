/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * SynchronizeCommand_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Common.Commands
{
    //Original name = SynchronizeCommand
    [DataContract]
    [Command(Name = "Common.SynchronizeCommand")]
    public sealed class SynchronizeCommandCommand : Command<SynchronizeCommandCommand.PayloadData>
    {
        public SynchronizeCommandCommand(int RequestId, SynchronizeCommandCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Command = null, object CmdData = null)
                : base(Timeout)
            {
                this.Command = Command;
                this.CmdData = CmdData;
            }

            /// <summary>
            /// The command name to be synchronized and executed next. 
            /// </summary>
            [DataMember(Name = "command")]
            public string Command { get; init; }

            /// <summary>
            /// A payload that represents the parameter that is normally associated with the command.
            /// </summary>
            [DataMember(Name = "cmdData")]
            public object CmdData { get; init; }

        }
    }
}
