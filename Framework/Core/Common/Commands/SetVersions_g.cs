/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * SetVersions_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Common.Commands
{
    //Original name = SetVersions
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Common.SetVersions")]
    public sealed class SetVersionsCommand : Command<SetVersionsCommand.PayloadData>
    {
        public SetVersionsCommand(int RequestId, SetVersionsCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(Dictionary<string, int> Commands = null, Dictionary<string, int> Events = null)
                : base()
            {
                this.Commands = Commands;
                this.Events = Events;
            }

            /// <summary>
            /// The commands for which a version is being set.
            /// </summary>
            [DataMember(Name = "commands")]
            public Dictionary<string, int> Commands { get; init; }

            /// <summary>
            /// The events for which a version is being set.
            /// </summary>
            [DataMember(Name = "events")]
            public Dictionary<string, int> Events { get; init; }

        }
    }
}
