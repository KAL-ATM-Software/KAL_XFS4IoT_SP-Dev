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
        public PrintFormCommand(string RequestId, PrintFormCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum AlignmentEnum
            {
                FormDefinition,
                TopLeft,
                TopRight,
                BottomLeft,
                BottomRight,
            }

            public enum ResolutionEnum
            {
                Low,
                Medium,
                High,
                VeryHigh,
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
            public class MediaControlClass
            {
                [DataMember(Name = "eject")] 
                public bool? Eject { get; private set; }
                [DataMember(Name = "perforate")] 
                public bool? Perforate { get; private set; }
                [DataMember(Name = "cut")] 
                public bool? Cut { get; private set; }
                [DataMember(Name = "skip")] 
                public bool? Skip { get; private set; }
                [DataMember(Name = "flush")] 
                public bool? Flush { get; private set; }
                [DataMember(Name = "retract")] 
                public bool? Retract { get; private set; }
                [DataMember(Name = "stack")] 
                public bool? Stack { get; private set; }
                [DataMember(Name = "partialCut")] 
                public bool? PartialCut { get; private set; }
                [DataMember(Name = "alarm")] 
                public bool? Alarm { get; private set; }
                [DataMember(Name = "forward")] 
                public bool? Forward { get; private set; }
                [DataMember(Name = "backward")] 
                public bool? Backward { get; private set; }
                [DataMember(Name = "turnMedia")] 
                public bool? TurnMedia { get; private set; }
                [DataMember(Name = "stamp")] 
                public bool? Stamp { get; private set; }
                [DataMember(Name = "park")] 
                public bool? Park { get; private set; }
                [DataMember(Name = "expel")] 
                public bool? Expel { get; private set; }
                [DataMember(Name = "ejectToTransport")] 
                public bool? EjectToTransport { get; private set; }
                [DataMember(Name = "rotate180")] 
                public bool? Rotate180 { get; private set; }
                [DataMember(Name = "clearBuffer")] 
                public bool? ClearBuffer { get; private set; }

                public MediaControlClass (bool? Eject, bool? Perforate, bool? Cut, bool? Skip, bool? Flush, bool? Retract, bool? Stack, bool? PartialCut, bool? Alarm, bool? Forward, bool? Backward, bool? TurnMedia, bool? Stamp, bool? Park, bool? Expel, bool? EjectToTransport, bool? Rotate180, bool? ClearBuffer)
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


            }

            /// <summary>
            /// An object containing one or more key/value pairs where the key is a field name and the value is the
            /// field value. If the field is an index field, the key must be specified as *fieldname[index]* where
            /// index specifies the zero-based element of the index field. The field names and values can contain
            /// UNICODE if supported by the service.
            /// </summary>
            public class FieldsClass
            {

                public FieldsClass ()
                {
                }


            }

            public enum PaperSourceEnum
            {
                Any,
                Upper,
                Lower,
                External,
                Aux,
                Aux2,
                Park,
            }


            public PayloadData(int Timeout, string FormName = null, string MediaName = null, AlignmentEnum? Alignment = null, int? OffsetX = null, int? OffsetY = null, ResolutionEnum? Resolution = null, object MediaControl = null, object Fields = null, PaperSourceEnum? PaperSource = null)
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
            public string FormName { get; private set; }
            /// <summary>
            /// The media name. If no media definition applies, this should be empty or omitted.
            /// </summary>
            [DataMember(Name = "mediaName")] 
            public string MediaName { get; private set; }
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
            public AlignmentEnum? Alignment { get; private set; }
            /// <summary>
            /// Specifies the horizontal offset of the form, relative to the horizontal alignment specified in
            /// [alignment](#printer.printform.command.properties.alignment), in horizontal resolution units (from
            /// form definition); always a positive number (i.e. if aligned to the right side of the media, means
            /// offset the form to the left). A value of *formDefinition* indicates that the *xoffset* value from the
            /// form definition should be used.
            /// </summary>
            [DataMember(Name = "offsetX")] 
            public int? OffsetX { get; private set; }
            /// <summary>
            /// Specifies the vertical offset of the form, relative to the vertical alignment specified in
            /// *alignment*, in vertical resolution units (from form definition); always a positive number (i.e. if
            /// aligned to the bottom of the media, means offset the form upward). A value of *formDefinition*
            /// indicates that the *yoffset* value from the form definition should be used.
            /// </summary>
            [DataMember(Name = "offsetY")] 
            public int? OffsetY { get; private set; }
            /// <summary>
            /// Specifies the resolution in which to print the form. Possible values are:
            /// 
            /// * ```low``` - Print form with low resolution.
            /// * ```medium``` - Print form with medium resolution.
            /// * ```high``` - Print form with high resolution.
            /// * ```veryHigh``` - Print form with very high resolution.
            /// </summary>
            [DataMember(Name = "resolution")] 
            public ResolutionEnum? Resolution { get; private set; }
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
            public object MediaControl { get; private set; }
            /// <summary>
            /// An object containing one or more key/value pairs where the key is a field name and the value is the
            /// field value. If the field is an index field, the key must be specified as *fieldname[index]* where
            /// index specifies the zero-based element of the index field. The field names and values can contain
            /// UNICODE if supported by the service.
            /// </summary>
            [DataMember(Name = "fields")] 
            public object Fields { get; private set; }
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
            public PaperSourceEnum? PaperSource { get; private set; }

        }
    }
}
