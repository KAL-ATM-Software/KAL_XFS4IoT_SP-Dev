/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Printer.PrintForm")]
    public sealed class PrintFormCommand : Command<PrintFormCommand.PayloadData>
    {
        public PrintFormCommand(int RequestId, PrintFormCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string FormName = null, string MediaName = null, AlignmentEnum? Alignment = null, int? OffsetX = null, int? OffsetY = null, ResolutionEnum? Resolution = null, MediaControlNullableClass MediaControl = null, Dictionary<string, List<string>> Fields = null, string PaperSource = null)
                : base()
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
            /// <example>Form1</example>
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

            /// <summary>
            /// The media definition name. If no media definition applies, this should be null.
            /// <example>Media1</example>
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
            /// offset the form to the left). If not specified, the *x* value from the
            /// form definition should be used.
            /// </summary>
            [DataMember(Name = "offsetX")]
            [DataTypes(Minimum = 0)]
            public int? OffsetX { get; init; }

            /// <summary>
            /// Specifies the vertical offset of the form, relative to the vertical alignment specified in
            /// *alignment*, in vertical resolution units (from form definition); always a positive number (i.e. if
            /// aligned to the bottom of the media, means offset the form upward). If not specified,
            /// the *y* value from the form definition should be used.
            /// </summary>
            [DataMember(Name = "offsetY")]
            [DataTypes(Minimum = 0)]
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

            /// <summary>
            /// Specifies the manner in which the media should be handled after the printing is done.
            /// If null, it means do none of these actions, as when printing
            /// multiple forms on a single page. When no options are set and the device does not support the
            /// [flush](#common.capabilities.completion.description.printer.control.flush)
            /// capability, the data will be printed immediately. If the device supports flush, the data may be
            /// buffered and the [Printer.ControlMedia](#printer.controlmedia) command should be used to synchronize
            /// the application with the device to ensure that all data has been physically printed.
            /// 
            /// In the descriptions, *flush data* means flush any data to the printer that has not yet been printed from
            /// previous [Printer.PrintForm](#printer.printform) or [Printer.PrintNative](#printer.printnative) commands.
            /// </summary>
            [DataMember(Name = "mediaControl")]
            public MediaControlNullableClass MediaControl { get; init; }

            /// <summary>
            /// An object containing one or more fields.
            /// </summary>
            [DataMember(Name = "fields")]
            [System.Text.Json.Serialization.JsonConverter(typeof(StringOrArrayConverter))]
            public Dictionary<string, List<string>> Fields { get; init; }

            /// <summary>
            /// Specifies the paper source to be used. For commands which print, this parameter is ignored if there is already
            /// paper in the print position. It can be one of the following:
            /// 
            /// * ```upper``` - Use the only paper source or the upper paper source, if there is more than one paper
            ///                 supply.
            /// * ```lower``` - Use the lower paper source.
            /// * ```external``` - Use the external paper.
            /// * ```aux``` - Use the auxiliary paper source.
            /// * ```aux2``` - Use the second auxiliary paper source.
            /// * ```park``` - Use the parking station paper source.
            /// * ```any``` - Use any paper source, it is determined by the service.
            /// * ```[paper source identifier]``` - The vendor specific paper source.
            /// <example>lower</example>
            /// </summary>
            [DataMember(Name = "paperSource")]
            [DataTypes(Pattern = @"^upper$|^lower$|^external$|^aux$|^aux2$|^park$|^any$|^[a-zA-Z]([a-zA-Z0-9]*)$")]
            public string PaperSource { get; init; }

        }
    }
}
