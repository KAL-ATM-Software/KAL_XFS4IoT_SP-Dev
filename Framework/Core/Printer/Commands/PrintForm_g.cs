/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = PrintForm
    [DataContract]
    [Command(Name = "Printer.PrintForm")]
    public sealed class PrintFormCommand : Command<PrintFormCommand.PayloadData>
    {
        public PrintFormCommand(int RequestId, PrintFormCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string FormName = null, string MediaName = null, AlignmentEnum? Alignment = null, int? OffsetX = null, int? OffsetY = null, ResolutionEnum? Resolution = null, MediaControlClass MediaControl = null, Dictionary<string, string> Fields = null, PaperSourceEnum? PaperSource = null)
                : base(Timeout)
            {
                this.FormName = FormName;
                this.MediaName = MediaName;
                this.Alignment = Alignment;
                this.OffsetX = OffsetX;
                this.OffsetY = OffsetY;
                this.Resolution = Resolution;
                this.MediaControl = MediaControl;
                this.Fields = Fields;
                this.PaperSource = PaperSource;
            }

            /// <summary>
            /// The form name.
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

            /// <summary>
            /// The media name. If no media definition applies, this should be empty or omitted.
            /// </summary>
            [DataMember(Name = "mediaName")]
            public string MediaName { get; init; }

            public enum AlignmentEnum
            {
                FormDefinition,
                TopLeft,
                TopRight,
                BottomLeft,
                BottomRight
            }

            /// <summary>
            /// Specifies the alignment of the form on the physical media, as one of the following values:
            /// 
            /// * ```formDefinition``` - Use the alignment specified in the form definition.
            /// * ```topLeft``` - Align form to top left of physical media.
            /// * ```topRight``` - Align form to top right of physical media.
            /// * ```bottomLeft``` - Align form to bottom left of physical media.
            /// * ```bottomRight``` - Align form to bottom right of physical media.
            /// </summary>
            [DataMember(Name = "alignment")]
            public AlignmentEnum? Alignment { get; init; }

            /// <summary>
            /// Specifies the horizontal offset of the form, relative to the horizontal alignment specified in
            /// [alignment](#printer.printform.command.properties.alignment), in horizontal resolution units (from
            /// form definition); always a positive number (i.e. if aligned to the right side of the media, means
            /// offset the form to the left). A value of *formDefinition* indicates that the *xoffset* value from the
            /// form definition should be used.
            /// </summary>
            [DataMember(Name = "offsetX")]
            [DataTypes(Minimum = 0)]
            public int? OffsetX { get; init; }

            /// <summary>
            /// Specifies the vertical offset of the form, relative to the vertical alignment specified in
            /// *alignment*, in vertical resolution units (from form definition); always a positive number (i.e. if
            /// aligned to the bottom of the media, means offset the form upward). A value of *formDefinition*
            /// indicates that the *yoffset* value from the form definition should be used.
            /// </summary>
            [DataMember(Name = "offsetY")]
            public int? OffsetY { get; init; }

            public enum ResolutionEnum
            {
                Low,
                Medium,
                High,
                VeryHigh
            }

            /// <summary>
            /// Specifies the resolution in which to print the form. Possible values are:
            /// 
            /// * ```low``` - Print form with low resolution.
            /// * ```medium``` - Print form with medium resolution.
            /// * ```high``` - Print form with high resolution.
            /// * ```veryHigh``` - Print form with very high resolution.
            /// </summary>
            [DataMember(Name = "resolution")]
            public ResolutionEnum? Resolution { get; init; }

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
            /// Specifies the manner in which the media should be handled after the printing is done, as a combination
            /// of the following flags. If no flags are set, it means do none of these actions, as when printing
            /// multiple forms on a single page. When no flags are set and the device does not support the flush
            /// capability, the data will be printed immediately. If the device supports flush, the data may be
            /// buffered and the [Printer.ControlMedia](#printer.controlmedia) command should be used to synchronize
            /// the application with the device to ensure that all data has been physically printed. The
            /// [clearBuffer](#printer.controlmedia.command.properties.mediacontrol.clearbuffer) flag is not
            /// applicable to this command. If set, the command will fail with error *invalidData*.
            /// </summary>
            [DataMember(Name = "mediaControl")]
            public MediaControlClass MediaControl { get; init; }

            /// <summary>
            /// An object containing one or more key/value pairs where the key is a field name and the value is the
            /// field value. If the field is an index field, the key must be specified as *fieldname[index]* where
            /// index specifies the zero-based element of the index field. The field names and values can contain
            /// UNICODE if supported by the service.
            /// </summary>
            [DataMember(Name = "fields")]
            public Dictionary<string, string> Fields { get; init; }

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
            /// Specifies the Paper source to use when printing this form. When the value is zero, then the paper
            /// source is determined from the media definition. This parameter is ignored if there is already paper in
            /// the print position. Possible values are:
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
            public PaperSourceEnum? PaperSource { get; init; }

        }
    }
}
