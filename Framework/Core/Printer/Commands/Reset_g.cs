/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
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
            /// * ```unit&lt;retract bin number&gt;``` - Retract the media to retract bin number specified. This number has
            /// to be between 1 and the [number of bins](#common.capabilities.completion.properties.printer.retractbins)
            /// supported by this device.
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "mediaControl")]
            [DataTypes(Pattern = @"^eject$|^expel$|^unit[0-9]+$")]
            public string MediaControl { get; init; }

        }
    }
}
