/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ConfigureNotetypes_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = ConfigureNotetypes
    [DataContract]
    [Command(Name = "CashAcceptor.ConfigureNotetypes")]
    public sealed class ConfigureNotetypesCommand : Command<ConfigureNotetypesCommand.PayloadData>
    {
        public ConfigureNotetypesCommand(int RequestId, ConfigureNotetypesCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, List<int> NoteIDs = null)
                : base(Timeout)
            {
                this.NoteIDs = NoteIDs;
            }

            /// <summary>
            /// Array of unsigned integers which contains the note IDs of the banknotes the banknote reader can accept.
            /// </summary>
            [DataMember(Name = "noteIDs")]
            public List<int> NoteIDs { get; private set; }

        }
    }
}
