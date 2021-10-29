/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ConfigureNoteReader_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = ConfigureNoteReader
    [DataContract]
    [Command(Name = "CashAcceptor.ConfigureNoteReader")]
    public sealed class ConfigureNoteReaderCommand : Command<ConfigureNoteReaderCommand.PayloadData>
    {
        public ConfigureNoteReaderCommand(int RequestId, ConfigureNoteReaderCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, bool? LoadAlways = null)
                : base(Timeout)
            {
                this.LoadAlways = LoadAlways;
            }

            /// <summary>
            /// If set to true, the Service loads the currency description data into the note reader, even if it is already
            /// loaded.
            /// </summary>
            [DataMember(Name = "loadAlways")]
            public bool? LoadAlways { get; init; }

        }
    }
}
