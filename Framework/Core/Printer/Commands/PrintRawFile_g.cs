/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintRawFile_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = PrintRawFile
    [DataContract]
    [Command(Name = "Printer.PrintRawFile")]
    public sealed class PrintRawFileCommand : Command<PrintRawFileCommand.PayloadData>
    {
        public PrintRawFileCommand(int RequestId, PrintRawFileCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string FileName = null, MediaControlClass MediaControl = null, PaperSourceEnum? PaperSource = null)
                : base(Timeout)
            {
                this.FileName = FileName;
                this.MediaControl = MediaControl;
                this.PaperSource = PaperSource;
            }

            /// <summary>
            /// This is the full path and file name of the file to be printed. This value cannot contain UNICODE
            /// characters.
            /// </summary>
            [DataMember(Name = "fileName")]
            public string FileName { get; private set; }

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
                public bool? Eject { get; private set; }

                /// <summary>
                /// Flush data as per eject, then perforate the media.
                /// </summary>
                [DataMember(Name = "perforate")]
                public bool? Perforate { get; private set; }

                /// <summary>
                /// Flush data as per eject, then cut the media. For printers which have the ability to stack multiple cut
                /// sheets and deliver them as a single bundle to the customer, cut causes the media to be stacked and eject
                /// causes the bundle to be moved to the exit slot.
                /// </summary>
                [DataMember(Name = "cut")]
                public bool? Cut { get; private set; }

                /// <summary>
                /// Flush data as per eject, then skip the media to mark.
                /// </summary>
                [DataMember(Name = "skip")]
                public bool? Skip { get; private set; }

                /// <summary>
                /// Flush any data to the printer that has not yet been physically printed from previous *Printer.PrintForm* or
                /// *Printer.PrintRawFile* commands. This will synchronize the application with the device to ensure that all
                /// data has been physically printed.
                /// </summary>
                [DataMember(Name = "flush")]
                public bool? Flush { get; private set; }

                /// <summary>
                /// Flush data as per flush, then retract the media to retract bin number one. For devices with more than one
                /// bin the command [Printer.RetractMedia](#printer.retractmedia) should be used if the media should be
                /// retracted to another bin than bin number one.
                /// </summary>
                [DataMember(Name = "retract")]
                public bool? Retract { get; private set; }

                /// <summary>
                /// Flush data as per flush, then move the media item on the internal stacker.
                /// </summary>
                [DataMember(Name = "stack")]
                public bool? Stack { get; private set; }

                /// <summary>
                /// Flush the data as per flush, then partially cut the media.
                /// </summary>
                [DataMember(Name = "partialCut")]
                public bool? PartialCut { get; private set; }

                /// <summary>
                /// Cause the printer to ring a bell, beep, or otherwise sound an audible alarm.
                /// </summary>
                [DataMember(Name = "alarm")]
                public bool? Alarm { get; private set; }

                /// <summary>
                /// Flush the data as per flush, then turn one page forward.
                /// </summary>
                [DataMember(Name = "forward")]
                public bool? Forward { get; private set; }

                /// <summary>
                /// Flush the data as per flush, then turn one page backward.
                /// </summary>
                [DataMember(Name = "backward")]
                public bool? Backward { get; private set; }

                /// <summary>
                /// Flush the data as per flush, then turn inserted media.
                /// </summary>
                [DataMember(Name = "turnMedia")]
                public bool? TurnMedia { get; private set; }

                /// <summary>
                /// Flush the data as per flush, then stamp on inserted media.
                /// </summary>
                [DataMember(Name = "stamp")]
                public bool? Stamp { get; private set; }

                /// <summary>
                /// Park the media in the parking station.
                /// </summary>
                [DataMember(Name = "park")]
                public bool? Park { get; private set; }

                /// <summary>
                /// Flush the data as per flush, then throw the media out of the exit slot.
                /// </summary>
                [DataMember(Name = "expel")]
                public bool? Expel { get; private set; }

                /// <summary>
                /// Flush the data as per flush, then move the media to a position on the transport just behind the exit slot.
                /// </summary>
                [DataMember(Name = "ejectToTransport")]
                public bool? EjectToTransport { get; private set; }

                /// <summary>
                /// Flush the data as per flush, then rotate media 180 degrees in the printing plane.
                /// </summary>
                [DataMember(Name = "rotate180")]
                public bool? Rotate180 { get; private set; }

                /// <summary>
                /// Clear any data that has not yet been physically printed from previous *Pinter.PrintForm* or
                /// *Printer.PrintRawFile* commands.
                /// </summary>
                [DataMember(Name = "clearBuffer")]
                public bool? ClearBuffer { get; private set; }

            }

            /// <summary>
            /// Specifies the manner in which the media should be handled after each page is printed, as a combination
            /// of the following flags. If no flags are set, no actions will be performed, as when printing multiple
            /// pages on a single media item. Note that the
            /// [clearBuffer](#printer.controlmedia.command.properties.mediacontrol.clearbuffer) flag is not
            /// applicable to this this command and will be ignored.
            /// </summary>
            [DataMember(Name = "mediaControl")]
            public MediaControlClass MediaControl { get; private set; }

            public enum PaperSourceEnum
            {
                Any,
                Upper,
                Lower,
                External,
                Aux,
                Aux2,
                Park
            }

            /// <summary>
            /// Specifies the paper source to use when printing. If omitted, the Service Provider will determine the
            /// paper source that will be used. This parameter is ignored if there is already paper in the print
            /// position. Possible values are:
            /// 
            /// * ```any``` - Any paper source can be used; it is determined by the service.
            /// * ```upper``` - Use the only paper source or the upper paper source, if there is more than one paper
            ///   supply.
            /// * ```lower``` - Use the lower paper source.
            /// * ```external``` - Use the external paper source (such as envelope tray or single sheet feed).
            /// * ```aux``` - Use the auxiliary paper source.
            /// * ```aux2``` - Use the second auxiliary paper source.
            /// * ```park``` - Use the parking station.
            /// </summary>
            [DataMember(Name = "paperSource")]
            public PaperSourceEnum? PaperSource { get; private set; }

        }
    }
}
