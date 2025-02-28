/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * AcceptItem_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Check.Commands
{
    //Original name = AcceptItem
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Check.AcceptItem")]
    public sealed class AcceptItemCommand : Command<AcceptItemCommand.PayloadData>
    {
        public AcceptItemCommand(int RequestId, AcceptItemCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(bool? Accept = null)
                : base()
            {
                this.Accept = Accept;
            }

            /// <summary>
            /// Specifies if the item should be accepted or refused. If true, then the item is accepted and moved to the
            /// stacker. If false, then the item is moved to the re-buncher/refuse position.
            /// <example>true</example>
            /// </summary>
            [DataMember(Name = "accept")]
            public bool? Accept { get; init; }

        }
    }
}
