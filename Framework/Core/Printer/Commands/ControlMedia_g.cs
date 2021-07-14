/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ControlMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = ControlMedia
    [DataContract]
    [Command(Name = "Printer.ControlMedia")]
    public sealed class ControlMediaCommand : Command<ControlMediaCommand.PayloadData>
    {
        public ControlMediaCommand(int RequestId, ControlMediaCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, MediaControlClass MediaControl = null)
                : base(Timeout)
            {
                this.MediaControl = MediaControl;
            }

            [DataContract]
            public sealed class MediaControlClass
            {
                public MediaControlClass(bool? Eject = null, bool? Perforate = null, bool? Cut = null, bool? Skip = null, bool? Flush = null, bool? Retract = null, bool? Stack = null, bool? PartialCut = null, bool? Alarm = null, bool? Forward = null, bool? Backward = null, bool? TurnMedia = null, bool? Stamp = null, bool? Park = null, bool? Expel = null, bool? EjectToTransport = null, bool? Rotate180 = null, bool? ClearBuffer = null)
                {
                    this.Eject = Eject;
                    this.Perforate = Perforate;
                    this.Cut = Cut;
                    this.Skip = Skip;
                    this.Flush = Flush;
                    this.Retract = Retract;
                    this.Stack = Stack;
                    this.PartialCut = PartialCut;
                    this.Alarm = Alarm;
                    this.Forward = Forward;
                    this.Backward = Backward;
                    this.TurnMedia = TurnMedia;
                    this.Stamp = Stamp;
                    this.Park = Park;
                    this.Expel = Expel;
                    this.EjectToTransport = EjectToTransport;
                    this.Rotate180 = Rotate180;
                    this.ClearBuffer = ClearBuffer;
                }

                /// <summary>
                /// Flush any data to the printer that has not yet been printed from previous
                /// [Printer.PrintForm](#printer.printform) or [Printer.PrintRawFile](#printer.printrawfile) commands, then
                /// eject the media.
                /// </summary>
                [DataMember(Name = "eject")]
                public bool? Eject { get; init; }

                /// <summary>
                /// Flush data as per eject, then perforate the media.
                /// </summary>
                [DataMember(Name = "perforate")]
                public bool? Perforate { get; init; }

                /// <summary>
                /// Flush data as per eject, then cut the media. For printers which have the ability to stack multiple cut
                /// sheets and deliver them as a single bundle to the customer, cut causes the media to be stacked and eject
                /// causes the bundle to be moved to the exit slot.
                /// </summary>
                [DataMember(Name = "cut")]
                public bool? Cut { get; init; }

                /// <summary>
                /// Flush data as per eject, then skip the media to mark.
                /// </summary>
                [DataMember(Name = "skip")]
                public bool? Skip { get; init; }

                /// <summary>
                /// Flush any data to the printer that has not yet been physically printed from previous *Printer.PrintForm* or
                /// *Printer.PrintRawFile* commands. This will synchronize the application with the device to ensure that all
                /// data has been physically printed.
                /// </summary>
                [DataMember(Name = "flush")]
                public bool? Flush { get; init; }

                /// <summary>
                /// Flush data as per flush, then retract the media to retract bin number one. For devices with more than one
                /// bin the command [Printer.RetractMedia](#printer.retractmedia) should be used if the media should be
                /// retracted to another bin than bin number one.
                /// </summary>
                [DataMember(Name = "retract")]
                public bool? Retract { get; init; }

                /// <summary>
                /// Flush data as per flush, then move the media item on the internal stacker.
                /// </summary>
                [DataMember(Name = "stack")]
                public bool? Stack { get; init; }

                /// <summary>
                /// Flush the data as per flush, then partially cut the media.
                /// </summary>
                [DataMember(Name = "partialCut")]
                public bool? PartialCut { get; init; }

                /// <summary>
                /// Cause the printer to ring a bell, beep, or otherwise sound an audible alarm.
                /// </summary>
                [DataMember(Name = "alarm")]
                public bool? Alarm { get; init; }

                /// <summary>
                /// Flush the data as per flush, then turn one page forward.
                /// </summary>
                [DataMember(Name = "forward")]
                public bool? Forward { get; init; }

                /// <summary>
                /// Flush the data as per flush, then turn one page backward.
                /// </summary>
                [DataMember(Name = "backward")]
                public bool? Backward { get; init; }

                /// <summary>
                /// Flush the data as per flush, then turn inserted media.
                /// </summary>
                [DataMember(Name = "turnMedia")]
                public bool? TurnMedia { get; init; }

                /// <summary>
                /// Flush the data as per flush, then stamp on inserted media.
                /// </summary>
                [DataMember(Name = "stamp")]
                public bool? Stamp { get; init; }

                /// <summary>
                /// Park the media in the parking station.
                /// </summary>
                [DataMember(Name = "park")]
                public bool? Park { get; init; }

                /// <summary>
                /// Flush the data as per flush, then throw the media out of the exit slot.
                /// </summary>
                [DataMember(Name = "expel")]
                public bool? Expel { get; init; }

                /// <summary>
                /// Flush the data as per flush, then move the media to a position on the transport just behind the exit slot.
                /// </summary>
                [DataMember(Name = "ejectToTransport")]
                public bool? EjectToTransport { get; init; }

                /// <summary>
                /// Flush the data as per flush, then rotate media 180 degrees in the printing plane.
                /// </summary>
                [DataMember(Name = "rotate180")]
                public bool? Rotate180 { get; init; }

                /// <summary>
                /// Clear any data that has not yet been physically printed from previous *Pinter.PrintForm* or
                /// *Printer.PrintRawFile* commands.
                /// </summary>
                [DataMember(Name = "clearBuffer")]
                public bool? ClearBuffer { get; init; }

            }

            /// <summary>
            /// Specifies the manner in which the media should be handled, as a combination of the following flags:
            /// 
            /// It is not possible to combine the flags eject, retract, park, expel and ejectToTransport with each
            /// other otherwise the command completes with errInvalidData.
            /// 
            /// It is not possible to combine the flag
            /// [clearBuffer](#printer.controlmedia.command.properties.mediacontrol.clearbuffer) with any other flags,
            /// otherwise the command completes with *invalidData*.
            /// 
            /// An application should be aware that the sequence of the actions is not guaranteed if more than one
            /// flag is specified in this parameter.
            /// </summary>
            [DataMember(Name = "mediaControl")]
            public MediaControlClass MediaControl { get; init; }

        }
    }
}
