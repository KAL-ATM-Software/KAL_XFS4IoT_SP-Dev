/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "Printer.Reset")]
    public sealed class ResetCommand : Command<ResetCommand.PayloadData>
    {
        public ResetCommand(string RequestId, ResetCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum MediaControlEnum
            {
                Eject,
                Retract,
                Expel,
            }


            public PayloadData(int Timeout, MediaControlEnum? MediaControl = null, int? RetractBinNumber = null)
                : base(Timeout)
            {
                this.MediaControl = MediaControl;
                this.RetractBinNumber = RetractBinNumber;
            }

            /// <summary>
            /// Specifies the manner in which the media should be handled, as one of the following:
            /// 
            /// * ```eject``` - Eject the media.
            /// * ```retract``` - Retract the media to retract bin number specified.
            /// * ```expel``` - Throw the media out of the exit slot.
            /// </summary>
            [DataMember(Name = "mediaControl")] 
            public MediaControlEnum? MediaControl { get; private set; }
            /// <summary>
            /// Number of the retract bin the media is retracted to. This number has to be between one and the
            /// [number of bins](#common.capabilities.completion.properties.printer.retractbins) supported by this
            /// device. It is only relevant if [mediaControl](#printer.reset.command.properties.mediacontrol) is
            /// *retract*.
            /// </summary>
            [DataMember(Name = "retractBinNumber")] 
            public int? RetractBinNumber { get; private set; }

        }
    }
}
