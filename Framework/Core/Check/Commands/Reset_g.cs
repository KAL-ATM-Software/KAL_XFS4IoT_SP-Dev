/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Check.Commands
{
    //Original name = Reset
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Check.Reset")]
    public sealed class ResetCommand : Command<ResetCommand.PayloadData>
    {
        public ResetCommand(int RequestId, ResetCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string MediaControl = null)
                : base()
            {
                this.MediaControl = MediaControl;
            }

            /// <summary>
            /// Specifies the manner in which the media should be handled, as one of the following values. See
            /// [resetControl](#common.capabilities.completion.description.check.resetcontrol) to determine the
            /// supported options.
            /// 
            /// * ```eject``` - Eject the media, i.e. return the media to the customer. Note that more than one position may
            ///   be used to return media.
            /// * ```&lt;storage unit identifier&gt;``` - The media item is in a storage unit as specified by
            ///   [identifier](#storage.getstorage.completion.properties.storage.unit1).
            /// * ```transport``` - Retract the media to the transport.
            /// * ```rebuncher``` - Retract the media to the rebuncher.
            /// <example>transport</example>
            /// </summary>
            [DataMember(Name = "mediaControl")]
            [DataTypes(Pattern = @"^eject$|^transport$|^rebuncher$|^unit[0-9A-Za-z]+$")]
            public string MediaControl { get; init; }

        }
    }
}
