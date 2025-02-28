/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = Reset
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Printer.Reset")]
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
            /// Specifies the manner in which the media should be handled, as one of the following:
            /// 
            /// * ```eject``` - Eject the media.
            /// * ```expel``` - Throw the media out of the exit slot.
            /// * ```[storage unit identifier]``` - A storage unit as specified by
            ///   [identifier](#storage.getstorage.completion.properties.storage.unit1).
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "mediaControl")]
            [DataTypes(Pattern = @"^eject$|^expel$|^unit[0-9A-Za-z]+$")]
            public string MediaControl { get; init; }

        }
    }
}
