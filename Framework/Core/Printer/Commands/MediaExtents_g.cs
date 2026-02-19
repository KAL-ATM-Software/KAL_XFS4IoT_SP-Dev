/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * MediaExtents_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = MediaExtents
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Printer.MediaExtents")]
    public sealed class MediaExtentsCommand : Command<MediaExtentsCommand.PayloadData>
    {
        public MediaExtentsCommand()
            : base()
        { }

        public MediaExtentsCommand(int RequestId, MediaExtentsCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(UnitClass Unit = null)
                : base()
            {
                this.Unit = Unit;
            }

            [DataMember(Name = "unit")]
            public UnitClass Unit { get; init; }

        }
    }
}
