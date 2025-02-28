/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashAcceptor.ConfigureNoteReader")]
    public sealed class ConfigureNoteReaderCommand : Command<ConfigureNoteReaderCommand.PayloadData>
    {
        public ConfigureNoteReaderCommand(int RequestId, ConfigureNoteReaderCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(bool? LoadAlways = null)
                : base()
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
