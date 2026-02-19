/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * SetForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = SetForm
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "Printer.SetForm")]
    public sealed class SetFormCommand : Command<SetFormCommand.PayloadData>
    {
        public SetFormCommand()
            : base()
        { }

        public SetFormCommand(int RequestId, SetFormCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Name = null, FormClass Form = null)
                : base()
            {
                this.Name = Name;
                this.Form = Form;
            }

            /// <summary>
            /// The name of the form.
            /// <example>Form10</example>
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

            [DataContract]
            public sealed class FormClass
            {
                public FormClass(UnitClass Unit = null, SizeClass Size = null, AlignmentClass Alignment = null, OrientationEnum? Orientation = null, int? Skew = null, VersionClass Version = null, int? Cpi = null, int? Lpi = null, int? PointSize = null, string Copyright = null, string Title = null, string Comment = null, string UserPrompt = null, Dictionary<string, FieldsClass> Fields = null, Dictionary<string, FramesClass> Frames = null, Dictionary<string, SubFormsClass> SubForms = null)
                {
                    this.Unit = Unit;
                    this.Size = Size;
                    this.Alignment = Alignment;
                    this.Orientation = Orientation;
                    this.Skew = Skew;
                    this.Version = Version;
                    this.Cpi = Cpi;
                    this.Lpi = Lpi;
                    this.PointSize = PointSize;
                    this.Copyright = Copyright;
                    this.Title = Title;
                    this.Comment = Comment;
                    this.UserPrompt = UserPrompt;
                    this.Fields = Fields;
                    this.Frames = Frames;
                    this.SubForms = SubForms;
                }

                [DataMember(Name = "unit")]
                public UnitClass Unit { get; init; }

                [DataMember(Name = "size")]
                public SizeClass Size { get; init; }

                [DataContract]
                public sealed class AlignmentClass
                {
                    public AlignmentClass(RelativeEnum? Relative = null, int? X = null, int? Y = null)
                    {
                        this.Relative = Relative;
                        this.X = X;
                        this.Y = Y;
                    }

                    public enum RelativeEnum
                    {
                        TopLeft,
                        TopRight,
                        BottomLeft,
                        BottomRight
                    }

                    /// <summary>
                    /// The relative position as one of the following:
                    /// 
                    /// * ```topLeft``` - Align to the top left of the physical media.
                    /// * ```topRight``` - Align to the top right of the physical media.
                    /// * ```bottomLeft``` - Align to the bottom left of the physical media.
                    /// * ```bottomRight``` - Align to the bottom right of the physical media.
                    /// <example>bottomRight</example>
                    /// </summary>
                    [DataMember(Name = "relative")]
                    public RelativeEnum? Relative { get; init; }

                    /// <summary>
                    /// Horizontal offset relative to the horizontal alignment specified by *relative* in the base unit
                    /// of the form. Always specified as a
                    /// positive value (i.e. if aligned to the right side of the media, means offset the form to the left).
                    /// <example>10</example>
                    /// </summary>
                    [DataMember(Name = "x")]
                    [DataTypes(Minimum = 0)]
                    public int? X { get; init; }

                    /// <summary>
                    /// Vertical offset relative to the vertical alignment specified by *relative* in the base unit
                    /// of the form. Always specified as a
                    /// positive value (i.e. if aligned to the bottom of the media, means offset the form upward).
                    /// <example>10</example>
                    /// </summary>
                    [DataMember(Name = "y")]
                    [DataTypes(Minimum = 0)]
                    public int? Y { get; init; }

                }

                /// <summary>
                /// The alignment of the form on the physical media. If null, the default values are used.
                /// </summary>
                [DataMember(Name = "alignment")]
                public AlignmentClass Alignment { get; init; }

                public enum OrientationEnum
                {
                    Portrait,
                    Landscape
                }

                /// <summary>
                /// Orientation of the form as one of the following:
                /// 
                /// * ```portrait``` - Short edge horizontal.
                /// * ```landscape``` - Long edge horizontal.
                /// <example>landscape</example>
                /// </summary>
                [DataMember(Name = "orientation")]
                public OrientationEnum? Orientation { get; init; }

                /// <summary>
                /// Maximum skew factor in degrees.
                /// </summary>
                [DataMember(Name = "skew")]
                [DataTypes(Minimum = 0, Maximum = 359)]
                public int? Skew { get; init; }

                [DataContract]
                public sealed class VersionClass
                {
                    public VersionClass(string Version = null, string Date = null, string Author = null)
                    {
                        this.Version = Version;
                        this.Date = Date;
                        this.Author = Author;
                    }

                    /// <summary>
                    /// Specifies the major and optional minor version of the form. This may be null if the version is not
                    /// required in the form.
                    /// <example>1.2</example>
                    /// </summary>
                    [DataMember(Name = "version")]
                    [DataTypes(Pattern = @"^[1-9][0-9]*(\.[0-9]+)?$")]
                    public string Version { get; init; }

                    /// <summary>
                    /// The creation/modification date of the form. May be null if not required.
                    /// <example>2018-11-13</example>
                    /// </summary>
                    [DataMember(Name = "date")]
                    public string Date { get; init; }

                    /// <summary>
                    /// The author of the form. May be null if not required.
                    /// <example>S. Currie</example>
                    /// </summary>
                    [DataMember(Name = "author")]
                    public string Author { get; init; }

                }

                /// <summary>
                /// The version of the form. May be null if not specified or required.
                /// </summary>
                [DataMember(Name = "version")]
                public VersionClass Version { get; init; }

                /// <summary>
                /// Characters per inch. This value specifies the default CPI within the form.
                /// When the *rowColumn* unit is used, the form CPI and LPI are used to calculate the
                /// position and size of all fields within a form, irrespective of the CPI and LPI of
                /// the fields themselves. If null, the printer's default value is used.
                /// </summary>
                [DataMember(Name = "cpi")]
                [DataTypes(Minimum = 1)]
                public int? Cpi { get; init; }

                /// <summary>
                /// Lines per inch. This value specifies the default LPI within the form.
                /// When the *rowColumn* unit is used, the form CPI and LPI are used to calculate the
                /// position and size of all fields within a form, irrespective of the CPI and LPI of
                /// the fields themselves. If null, the printer's default value is used.
                /// </summary>
                [DataMember(Name = "lpi")]
                [DataTypes(Minimum = 1)]
                public int? Lpi { get; init; }

                /// <summary>
                /// This value specifies the default point size within the form. If null, the printer's default value is used.
                /// </summary>
                [DataMember(Name = "pointSize")]
                [DataTypes(Minimum = 1)]
                public int? PointSize { get; init; }

                /// <summary>
                /// Copyright entry. May be null if not required.
                /// <example>Copyright (c) XYZ Corp.</example>
                /// </summary>
                [DataMember(Name = "copyright")]
                public string Copyright { get; init; }

                /// <summary>
                /// The title of the form. May be null if not required.
                /// <example>Form 1</example>
                /// </summary>
                [DataMember(Name = "title")]
                public string Title { get; init; }

                /// <summary>
                /// Additional comments about the form. May be null if not required.
                /// <example>This form is for purpose x</example>
                /// </summary>
                [DataMember(Name = "comment")]
                public string Comment { get; init; }

                /// <summary>
                /// Prompt string for user interaction. May be null if not required.
                /// <example>Please take the receipt when presented.</example>
                /// </summary>
                [DataMember(Name = "userPrompt")]
                public string UserPrompt { get; init; }

                [DataContract]
                public sealed class FieldsClass
                {
                    public FieldsClass(int? Order = null, PositionClass Position = null, string Follows = null, string Header = null, string Footer = null, SideEnum? Side = null, SizeClass Size = null, IndexClass Index = null, FieldTypeEnum? FieldType = null, ScalingEnum? Scaling = null, BarcodeEnum? Barcode = null, CoercivityEnum? Coercivity = null, ClassEnum? Class = null, AccessEnum? Access = null, OverflowEnum? Overflow = null, StyleClass Style = null, CaseEnum? Case = null, HorizontalEnum? Horizontal = null, VerticalEnum? Vertical = null, string Color = null, FontClass Font = null, string Format = null, string InitialValue = null, List<byte> InitialGraphic = null)
                    {
                        this.Order = Order;
                        this.Position = Position;
                        this.Follows = Follows;
                        this.Header = Header;
                        this.Footer = Footer;
                        this.Side = Side;
                        this.Size = Size;
                        this.Index = Index;
                        this.FieldType = FieldType;
                        this.Scaling = Scaling;
                        this.Barcode = Barcode;
                        this.Coercivity = Coercivity;
                        this.Class = Class;
                        this.Access = Access;
                        this.Overflow = Overflow;
                        this.Style = Style;
                        this.Case = Case;
                        this.Horizontal = Horizontal;
                        this.Vertical = Vertical;
                        this.Color = Color;
                        this.Font = Font;
                        this.Format = Format;
                        this.InitialValue = InitialValue;
                        this.InitialGraphic = InitialGraphic;
                    }

                    /// <summary>
                    /// Specifies the order in which the field is to be printed. Required as there is no inherent
                    /// order to JSON properties in an object. May be overridden by *follows*. If two fields are defined with the
                    /// same *order* in a given form, then the form is invalid.
                    /// <example>10</example>
                    /// </summary>
                    [DataMember(Name = "order")]
                    [DataTypes(Minimum = 0)]
                    public int? Order { get; init; }

                    [DataMember(Name = "position")]
                    public PositionClass Position { get; init; }

                    /// <summary>
                    /// Print this field directly following the field with the specified name;
                    /// positioning information is ignored. If null, then fields are printed according to *order*.
                    /// <example>Field01</example>
                    /// </summary>
                    [DataMember(Name = "follows")]
                    public string Follows { get; init; }

                    /// <summary>
                    /// This field is a form/sub-form header field.
                    /// 
                    /// - N represents a form/sub-form page number (relative to 0) the header field is to print within.
                    /// - N-N represents a form/sub-form page number range the header field is to print within.
                    /// - ALL indicates that header field is to be printed on all pages of form/sub-form.
                    /// 
                    /// Combinations of N and N-N may exist separated by commas.
                    /// 
                    /// The form/sub-form page number is intended to supplement the *z* property of the *position*.
                    /// For example 0,2-4,6 indicates that the header field is to print on relative form/sub-form pages 0, 2, 3, 4, and 6.
                    /// <example>0,2-5,7</example>
                    /// </summary>
                    [DataMember(Name = "header")]
                    [DataTypes(Pattern = @"^(([0-9]+|[0-9]+-[0-9]+),)*([0-9]+|[0-9]+-[0-9]+)+$|^ALL$")]
                    public string Header { get; init; }

                    /// <summary>
                    /// This field is a form/sub-form footer field.
                    /// 
                    /// - N represents a form/sub-form page number (relative to 0) the footer field is to print within.
                    /// - N-N represents a form/sub-form page number range the footer field is to print within.
                    /// - ALL indicates that footer field is to be printed on all pages of form/sub-form.
                    /// 
                    /// Combinations of N and N-N may exist separated by commas.
                    /// 
                    /// The form/sub-form page number is intended to supplement the *z* property of the *position*.
                    /// For example 0,2-4,6 indicates that the footer field is to print on relative form/sub-form pages 0, 2, 3, 4, and 6.
                    /// <example>0,2-5,7</example>
                    /// </summary>
                    [DataMember(Name = "footer")]
                    [DataTypes(Pattern = @"^(([0-9]+|[0-9]+-[0-9]+),)*([0-9]+|[0-9]+-[0-9]+)+$|^ALL$")]
                    public string Footer { get; init; }

                    public enum SideEnum
                    {
                        Front,
                        Back
                    }

                    /// <summary>
                    /// The side of the form as one of the following:
                    /// 
                    /// * ```front``` - the front side of the paper/media.
                    /// * ```back``` - the back side of the paper/media.
                    /// </summary>
                    [DataMember(Name = "side")]
                    public SideEnum? Side { get; init; }

                    [DataMember(Name = "size")]
                    public SizeClass Size { get; init; }

                    [DataContract]
                    public sealed class IndexClass
                    {
                        public IndexClass(int? RepeatCount = null, int? X = null, int? Y = null)
                        {
                            this.RepeatCount = RepeatCount;
                            this.X = X;
                            this.Y = Y;
                        }

                        /// <summary>
                        /// How often this field is repeated.
                        /// </summary>
                        [DataMember(Name = "repeatCount")]
                        [DataTypes(Minimum = 1)]
                        public int? RepeatCount { get; init; }

                        /// <summary>
                        /// Horizontal offset for next field
                        /// </summary>
                        [DataMember(Name = "x")]
                        [DataTypes(Minimum = 0)]
                        public int? X { get; init; }

                        /// <summary>
                        /// Vertical offset for next field
                        /// </summary>
                        [DataMember(Name = "y")]
                        [DataTypes(Minimum = 0)]
                        public int? Y { get; init; }

                    }

                    /// <summary>
                    /// Specifies that the field is to be repeated in the form/sub-form. May be null if not required.
                    /// </summary>
                    [DataMember(Name = "index")]
                    public IndexClass Index { get; init; }

                    public enum FieldTypeEnum
                    {
                        Text,
                        Micr,
                        Ocr,
                        Msf,
                        Barcode,
                        Graphic,
                        Pagemark
                    }

                    /// <summary>
                    /// The type of the field as one of the following:
                    /// 
                    /// * ```text``` - text field.
                    /// * ```micr``` - MICR field.
                    /// * ```ocr``` - OCR field.
                    /// * ```msf``` - MSF field.
                    /// * ```barcode``` - Barcode field.
                    /// * ```graphic``` - Graphic field.
                    /// * ```pagemark``` - Page mark field.
                    /// </summary>
                    [DataMember(Name = "fieldType")]
                    public FieldTypeEnum? FieldType { get; init; }

                    public enum ScalingEnum
                    {
                        BestFit,
                        AsIs,
                        MaintainAspect
                    }

                    /// <summary>
                    /// Information on how to size the graphic within the field as one of the following. Only relevant where
                    /// *fieldType* is *graphic*, therefore will be ignored for other cases:
                    /// 
                    /// * ```bestFit``` - scale to size indicated.
                    /// * ```asIs``` - render at native size.
                    /// * ```maintainAspect``` - scale as close as possible to size indicated while maintaining
                    ///   the aspect ratio and not losing graphic information.
                    /// </summary>
                    [DataMember(Name = "scaling")]
                    public ScalingEnum? Scaling { get; init; }

                    public enum BarcodeEnum
                    {
                        None,
                        Above,
                        Below,
                        Both
                    }

                    /// <summary>
                    /// Position of the HRI (Human Readable Interpretation) characters as one of the following:
                    /// 
                    /// * ```none``` - no characters or not applicable.
                    /// * ```above``` - above the barcode.
                    /// * ```below``` - below the barcode.
                    /// * ```both``` - both above and below the barcode.
                    /// </summary>
                    [DataMember(Name = "barcode")]
                    public BarcodeEnum? Barcode { get; init; }

                    public enum CoercivityEnum
                    {
                        Auto,
                        Low,
                        High
                    }

                    /// <summary>
                    /// Specifies the coercivity to be used for writing the magnetic stripe as one of the following. May be null
                    /// as this is only applicable to *msf* fields:
                    /// 
                    /// * ```auto``` - The coercivity is decided by the service or the hardware.
                    /// * ```low``` - A low coercivity is to be used for writing the magnetic stripe.
                    /// * ```high``` - A high coercivity is to be used for writing the magnetic stripe.
                    /// </summary>
                    [DataMember(Name = "coercivity")]
                    public CoercivityEnum? Coercivity { get; init; }

                    public enum ClassEnum
                    {
                        Optional,
                        Static,
                        Required
                    }

                    /// <summary>
                    /// Field class as one of the following:
                    /// 
                    /// * ```optional``` - The field is optional.
                    /// * ```static``` - The field is static.
                    /// * ```required``` - The field is required.
                    /// </summary>
                    [DataMember(Name = "class")]
                    public ClassEnum? Class { get; init; }

                    public enum AccessEnum
                    {
                        Read,
                        Write,
                        ReadWrite
                    }

                    /// <summary>
                    /// Specifies the field access as one of the following:
                    /// 
                    /// * ```read``` - The field is used for input.
                    /// * ```write``` - The field is used for output.
                    /// * ```readWrite``` - The field is used for both input and output.
                    /// </summary>
                    [DataMember(Name = "access")]
                    public AccessEnum? Access { get; init; }

                    public enum OverflowEnum
                    {
                        Terminate,
                        Truncate,
                        BestFit,
                        Overwrite,
                        WordWrap
                    }

                    /// <summary>
                    /// Specifies how an overflow of field data should be handled as one of the following:
                    /// 
                    /// * ```terminate``` - Return an error and terminate printing of the form.
                    /// * ```truncate``` - Truncate the field data to fit in the field.
                    /// * ```bestFit``` - Fit the text in the field.
                    /// * ```overwrite``` - Print the field data beyond the extents of the field boundary.
                    /// * ```wordWrap``` - If the field can hold more than one line the text is wrapped around. Wrapping
                    ///   is performed, where possible, by splitting the line on a space character or a hyphen character
                    ///   or any other character which is used to join two words together.
                    /// </summary>
                    [DataMember(Name = "overflow")]
                    public OverflowEnum? Overflow { get; init; }

                    [DataContract]
                    public sealed class StyleClass
                    {
                        public StyleClass(bool? Bold = null, bool? Italic = null, UnderlineEnum? Underline = null, WidthEnum? Width = null, StrikeEnum? Strike = null, RotateEnum? Rotate = null, CharacterSpacingEnum? CharacterSpacing = null, LineSpacingEnum? LineSpacing = null, ScriptEnum? Script = null, bool? Overscore = null, QualityEnum? Quality = null, bool? Opaque = null)
                        {
                            this.Bold = Bold;
                            this.Italic = Italic;
                            this.Underline = Underline;
                            this.Width = Width;
                            this.Strike = Strike;
                            this.Rotate = Rotate;
                            this.CharacterSpacing = CharacterSpacing;
                            this.LineSpacing = LineSpacing;
                            this.Script = Script;
                            this.Overscore = Overscore;
                            this.Quality = Quality;
                            this.Opaque = Opaque;
                        }

                        /// <summary>
                        /// Bold style.
                        /// <example>true</example>
                        /// </summary>
                        [DataMember(Name = "bold")]
                        public bool? Bold { get; init; }

                        /// <summary>
                        /// Italic style.
                        /// </summary>
                        [DataMember(Name = "italic")]
                        public bool? Italic { get; init; }

                        public enum UnderlineEnum
                        {
                            None,
                            Single,
                            Double
                        }

                        /// <summary>
                        /// Underline style as one of the following:
                        /// 
                        /// * ```none``` - No underline.
                        /// * ```single``` - Single underline.
                        /// * ```double``` - Double underline.
                        /// <example>double</example>
                        /// </summary>
                        [DataMember(Name = "underline")]
                        public UnderlineEnum? Underline { get; init; }

                        public enum WidthEnum
                        {
                            Single,
                            Double,
                            Triple,
                            Quadruple
                        }

                        /// <summary>
                        /// Width style as one of the following:
                        /// 
                        /// * ```single``` - Single width.
                        /// * ```double``` - Double width.
                        /// * ```triple``` - Triple width.
                        /// * ```quadruple``` - Quadruple width.
                        /// <example>triple</example>
                        /// </summary>
                        [DataMember(Name = "width")]
                        public WidthEnum? Width { get; init; }

                        public enum StrikeEnum
                        {
                            None,
                            Single,
                            Double
                        }

                        /// <summary>
                        /// Strike through style as one of the following:
                        /// 
                        /// * ```none``` - No strike through.
                        /// * ```single``` - Single strike through.
                        /// * ```double``` - Double strike through.
                        /// </summary>
                        [DataMember(Name = "strike")]
                        public StrikeEnum? Strike { get; init; }

                        public enum RotateEnum
                        {
                            None,
                            Ninety,
                            UpsideDown,
                            TwoSeventy
                        }

                        /// <summary>
                        /// Rotation angle as one of the following:
                        /// 
                        /// * ```none``` - No rotation.
                        /// * ```ninety``` - Rotate 90 degrees clockwise.
                        /// * ```upsideDown``` - Upside down.
                        /// * ```twoSeventy``` - Rotate 270 degrees clockwise.
                        /// </summary>
                        [DataMember(Name = "rotate")]
                        public RotateEnum? Rotate { get; init; }

                        public enum CharacterSpacingEnum
                        {
                            Standard,
                            Proportional,
                            Condensed
                        }

                        /// <summary>
                        /// Character spacing as one of the following:
                        /// 
                        /// * ```standard``` - Standard character spacing.
                        /// * ```proportional``` - Proportional character spacing.
                        /// * ```condensed``` - Condensed character spacing.
                        /// </summary>
                        [DataMember(Name = "characterSpacing")]
                        public CharacterSpacingEnum? CharacterSpacing { get; init; }

                        public enum LineSpacingEnum
                        {
                            Standard,
                            Double,
                            Triple,
                            Quadruple
                        }

                        /// <summary>
                        /// Line spacing as one of the following:
                        /// 
                        /// * ```standard``` - Standard line spacing.
                        /// * ```double``` - Double line spacing.
                        /// * ```triple``` - Triple line spacing.
                        /// * ```quadruple``` - Quadruple line spacing.
                        /// </summary>
                        [DataMember(Name = "lineSpacing")]
                        public LineSpacingEnum? LineSpacing { get; init; }

                        public enum ScriptEnum
                        {
                            Standard,
                            Superscript,
                            Subscript
                        }

                        /// <summary>
                        /// Character script type as one of the following:
                        /// 
                        /// * ```standard``` - Standard script.
                        /// * ```superscript``` - Superscript.
                        /// * ```subscript``` - Superscript.
                        /// </summary>
                        [DataMember(Name = "script")]
                        public ScriptEnum? Script { get; init; }

                        /// <summary>
                        /// Overscore style.
                        /// </summary>
                        [DataMember(Name = "overscore")]
                        public bool? Overscore { get; init; }

                        public enum QualityEnum
                        {
                            Standard,
                            Letter,
                            NearLetter
                        }

                        /// <summary>
                        /// Letter quality as one of the following:
                        /// 
                        /// * ```standard``` - Standard quality.
                        /// * ```letter``` - Letter quality.
                        /// * ```nearLetter``` - Near letter quality.
                        /// </summary>
                        [DataMember(Name = "quality")]
                        public QualityEnum? Quality { get; init; }

                        /// <summary>
                        /// If true, the printing is opaque; if false, transparent.
                        /// </summary>
                        [DataMember(Name = "opaque")]
                        public bool? Opaque { get; init; }

                    }

                    /// <summary>
                    /// Style attributes using a combination of the following. Some of the styles may be mutually exclusive; they 
                    /// may provide unexpected results if combined. This field is optional and may be null, in which case the 
                    /// default normal style will be used.
                    /// </summary>
                    [DataMember(Name = "style")]
                    public StyleClass Style { get; init; }

                    public enum CaseEnum
                    {
                        NoChange,
                        Upper,
                        Lower
                    }

                    /// <summary>
                    /// Convert field contents to one of the following:
                    /// 
                    /// * ```noChange``` - No change.
                    /// * ```upper``` - Convert to upper case.
                    /// * ```lower``` - Convert to lower case.
                    /// </summary>
                    [DataMember(Name = "case")]
                    public CaseEnum? Case { get; init; }

                    public enum HorizontalEnum
                    {
                        Left,
                        Right,
                        Center,
                        Justify
                    }

                    /// <summary>
                    /// Horizontal alignment of field contents as one of the following:
                    /// 
                    /// * ```left``` - Align to the left.
                    /// * ```right``` - Align to the right.
                    /// * ```center``` - Align to the center.
                    /// * ```justify``` - Justify the contents.
                    /// </summary>
                    [DataMember(Name = "horizontal")]
                    public HorizontalEnum? Horizontal { get; init; }

                    public enum VerticalEnum
                    {
                        Bottom,
                        Center,
                        Top
                    }

                    /// <summary>
                    /// Vertical alignment of field contents as one of the following:
                    /// 
                    /// * ```bottom``` - Align to the bottom.
                    /// * ```center``` - Align to the center.
                    /// * ```top``` - Align to the top.
                    /// </summary>
                    [DataMember(Name = "vertical")]
                    public VerticalEnum? Vertical { get; init; }

                    /// <summary>
                    /// Specifies the color. Following values are possible:
                    /// 
                    /// * ```black```
                    /// * ```white```
                    /// * ```gray```
                    /// * ```red```
                    /// * ```blue```
                    /// * ```green```
                    /// * ```yellow```
                    /// * ```[R,G,B]``` - Red, blue, green colors in the range 0-255.
                    /// <example>242,44,97</example>
                    /// </summary>
                    [DataMember(Name = "color")]
                    [DataTypes(Pattern = @"^black$|^white$|^gray$|^red$|^blue$|^green$|^yellow$|^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$")]
                    public string Color { get; init; }

                    [DataContract]
                    public sealed class FontClass
                    {
                        public FontClass(string Name = null, int? PointSize = null, int? Cpi = null, int? Lpi = null)
                        {
                            this.Name = Name;
                            this.PointSize = PointSize;
                            this.Cpi = Cpi;
                            this.Lpi = Lpi;
                        }

                        /// <summary>
                        /// Font name: This property is interpreted by the service. In some cases, it may indicate printer resident
                        /// fonts, and in others it may indicate the name of a downloadable font. For *barcode* fields it represents
                        /// the barcode font name.
                        /// 
                        /// In some cases, this pre-defines other properties in this object.
                        /// <example>Courier New</example>
                        /// </summary>
                        [DataMember(Name = "name")]
                        public string Name { get; init; }

                        /// <summary>
                        /// Point size. If 0, the point size defaults to the *pointSize* defined for the form.
                        /// <example>10</example>
                        /// </summary>
                        [DataMember(Name = "pointSize")]
                        [DataTypes(Minimum = 0)]
                        public int? PointSize { get; init; }

                        /// <summary>
                        /// Characters per inch. If 0, the CPI defaults to the CPI defined for the form.
                        /// <example>5</example>
                        /// </summary>
                        [DataMember(Name = "cpi")]
                        [DataTypes(Minimum = 0)]
                        public int? Cpi { get; init; }

                        /// <summary>
                        /// Lines per inch. If 0, the LPI defaults to the LPI defined for the form.
                        /// <example>5</example>
                        /// </summary>
                        [DataMember(Name = "lpi")]
                        [DataTypes(Minimum = 0)]
                        public int? Lpi { get; init; }

                    }

                    /// <summary>
                    /// The font to be used. If null, the default font is used.
                    /// </summary>
                    [DataMember(Name = "font")]
                    public FontClass Font { get; init; }

                    /// <summary>
                    /// This is an application defined input field describing how the application should format the data. This may be interpreted by the service.
                    /// <example>Application defined information</example>
                    /// </summary>
                    [DataMember(Name = "format")]
                    public string Format { get; init; }

                    /// <summary>
                    /// Initial value, which may be changed dynamically at run-time. May be null if no initial value is required.
                    /// Ignored for *graphic* fields, which are specified in *initialGraphic*.
                    /// <example>Default text</example>
                    /// </summary>
                    [DataMember(Name = "initialValue")]
                    public string InitialValue { get; init; }

                    /// <summary>
                    /// Initial graphic in Base64 format, which may be changed dynamically at run-time. May be null if no initial
                    /// graphic is required. Ignored for non-*graphic* fields, which are specified in *initialValue*.
                    /// <example>O2gAUACFyEARAJAC</example>
                    /// </summary>
                    [DataMember(Name = "initialGraphic")]
                    [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                    public List<byte> InitialGraphic { get; init; }

                }

                /// <summary>
                /// One field definition for each field in the form. For each object, the property name is the field name. The
                /// field name within a form and its optional sub-forms must be unique.
                /// </summary>
                [DataMember(Name = "fields")]
                public Dictionary<string, FieldsClass> Fields { get; init; }

                [DataContract]
                public sealed class FramesClass
                {
                    public FramesClass(PositionClass Position = null, string Frames = null, string Header = null, string Footer = null, SideEnum? Side = null, SizeClass Size = null, RepeatClass Repeat = null, FieldTypeEnum? FieldType = null, ClassEnum? Class = null, OverflowEnum? Overflow = null, StyleEnum? Style = null, string Color = null, string FillColor = null, FillStyleEnum? FillStyle = null, string SubstSign = null, string Title = null, HorizontalEnum? Horizontal = null, VerticalEnum? Vertical = null)
                    {
                        this.Position = Position;
                        this.Frames = Frames;
                        this.Header = Header;
                        this.Footer = Footer;
                        this.Side = Side;
                        this.Size = Size;
                        this.Repeat = Repeat;
                        this.FieldType = FieldType;
                        this.Class = Class;
                        this.Overflow = Overflow;
                        this.Style = Style;
                        this.Color = Color;
                        this.FillColor = FillColor;
                        this.FillStyle = FillStyle;
                        this.SubstSign = SubstSign;
                        this.Title = Title;
                        this.Horizontal = Horizontal;
                        this.Vertical = Vertical;
                    }

                    [DataMember(Name = "position")]
                    public PositionClass Position { get; init; }

                    /// <summary>
                    /// Frames the specified field, positioning and size information are ignored. The frame surrounds the
                    /// complete field, not just the printed data. If the field is repeated, the frame surrounds the first and last
                    /// fields that are printed.
                    /// <example>Field01</example>
                    /// </summary>
                    [DataMember(Name = "frames")]
                    public string Frames { get; init; }

                    /// <summary>
                    /// This frame is a form/sub-form header frame.
                    /// 
                    /// - N represents a form/sub-form page number (relative to 0) the header frame is to print within.
                    /// - N-N represents a form/sub-form page number range the header frame is to print within.
                    /// - ALL indicates that header frame is to be printed on all pages of form/sub-form.
                    /// 
                    /// Combinations of N and N-N may exist separated by commas.
                    /// 
                    /// The form/sub-form page number is intended to supplement the *z* property of the *position*.
                    /// For example 0,2-4,6 indicates that the header frame is to print on relative form/sub-form pages 0, 2, 3, 4, and 6.
                    /// <example>0,2-5,7</example>
                    /// </summary>
                    [DataMember(Name = "header")]
                    [DataTypes(Pattern = @"^(([0-9]+|[0-9]+-[0-9]+),)*([0-9]+|[0-9]+-[0-9]+)+$|^ALL$")]
                    public string Header { get; init; }

                    /// <summary>
                    /// This frame is a form/sub-form footer frame.
                    /// 
                    /// - N represents a form/sub-form page number (relative to 0) the footer frame is to print within.
                    /// - N-N represents a form/sub-form page number range the footer frame is to print within.
                    /// - ALL indicates that footer frame is to be printed on all pages of form/sub-form.
                    /// 
                    /// Combinations of N and N-N may exist separated by commas.
                    /// 
                    /// The form/sub-form page number is intended to supplement the *z* property of the *position*.
                    /// For example 0,2-4,6 indicates that the footer frame is to print on relative form/sub-form pages 0, 2, 3, 4, and 6.
                    /// <example>0,2-5,7</example>
                    /// </summary>
                    [DataMember(Name = "footer")]
                    [DataTypes(Pattern = @"^(([0-9]+|[0-9]+-[0-9]+),)*([0-9]+|[0-9]+-[0-9]+)+$|^ALL$")]
                    public string Footer { get; init; }

                    public enum SideEnum
                    {
                        Front,
                        Back
                    }

                    /// <summary>
                    /// The side of the form where this frame is positioned as one of the following:
                    /// 
                    /// * ```front``` - the front side of the paper/media.
                    /// * ```back``` - the back side of the paper/media.
                    /// <example>back</example>
                    /// </summary>
                    [DataMember(Name = "side")]
                    public SideEnum? Side { get; init; }

                    [DataMember(Name = "size")]
                    public SizeClass Size { get; init; }

                    [DataContract]
                    public sealed class RepeatClass
                    {
                        public RepeatClass(int? RepeatCountX = null, int? OffsetX = null, int? RepeatCountY = null, int? OffsetY = null)
                        {
                            this.RepeatCountX = RepeatCountX;
                            this.OffsetX = OffsetX;
                            this.RepeatCountY = RepeatCountY;
                            this.OffsetY = OffsetY;
                        }

                        /// <summary>
                        /// How often this frame is repeated horizontally in the form.
                        /// <example>2</example>
                        /// </summary>
                        [DataMember(Name = "repeatCountX")]
                        [DataTypes(Minimum = 0)]
                        public int? RepeatCountX { get; init; }

                        /// <summary>
                        /// Horizontal offset for next frame.
                        /// <example>4</example>
                        /// </summary>
                        [DataMember(Name = "offsetX")]
                        [DataTypes(Minimum = 0)]
                        public int? OffsetX { get; init; }

                        /// <summary>
                        /// How often this frame is repeated vertically in the form.
                        /// <example>1</example>
                        /// </summary>
                        [DataMember(Name = "repeatCountY")]
                        [DataTypes(Minimum = 0)]
                        public int? RepeatCountY { get; init; }

                        /// <summary>
                        /// Vertical offset for next frame.
                        /// <example>3</example>
                        /// </summary>
                        [DataMember(Name = "offsetY")]
                        [DataTypes(Minimum = 0)]
                        public int? OffsetY { get; init; }

                    }

                    /// <summary>
                    /// Specifies that the frame is to be repeated in the form/sub-form. May be null if not required.
                    /// </summary>
                    [DataMember(Name = "repeat")]
                    public RepeatClass Repeat { get; init; }

                    public enum FieldTypeEnum
                    {
                        Rectangle,
                        RoundedCorner,
                        Ellipse
                    }

                    /// <summary>
                    /// The type of the frame as one of the following:
                    /// 
                    /// * ```rectangle``` - Rectangle.
                    /// * ```roundedCorner``` - Rounded corner.
                    /// * ```ellipse``` - Ellipse.
                    /// <example>ellipse</example>
                    /// </summary>
                    [DataMember(Name = "fieldType")]
                    public FieldTypeEnum? FieldType { get; init; }

                    public enum ClassEnum
                    {
                        Static,
                        Optional
                    }

                    /// <summary>
                    /// Frame class as one of the following:
                    /// 
                    /// * ```static``` - The frame is static.
                    /// * ```optional``` - The frame is optional; it is printed only if its name appears in the list of field
                    ///   names given as parameter to the command. In this case, the name of the frame must be different from all
                    ///   the names of the fields.
                    /// <example>optional</example>
                    /// </summary>
                    [DataMember(Name = "class")]
                    public ClassEnum? Class { get; init; }

                    public enum OverflowEnum
                    {
                        Terminate,
                        Truncate,
                        BestFit
                    }

                    /// <summary>
                    /// Action on frame overflowing the form as one of the following:
                    /// 
                    /// * ```terminate``` - Return an error and terminate printing of the form.
                    /// * ```truncate``` - Truncate to fit in the field.
                    /// * ```bestFit``` - Fit the text in the field.
                    /// <example>truncate</example>
                    /// </summary>
                    [DataMember(Name = "overflow")]
                    public OverflowEnum? Overflow { get; init; }

                    public enum StyleEnum
                    {
                        SingleThin,
                        DoubleThin,
                        SingleThick,
                        DoubleThick,
                        Dotted
                    }

                    /// <summary>
                    /// Frame line attributes as one of the following:
                    /// 
                    /// * ```singleThin``` - A single thin line.
                    /// * ```doubleThin``` - A double thin line.
                    /// * ```singleThick``` - A single thick line.
                    /// * ```doubleThick``` - A double thick line.
                    /// * ```dotted``` - A dotted line.
                    /// <example>doubleThick</example>
                    /// </summary>
                    [DataMember(Name = "style")]
                    public StyleEnum? Style { get; init; }

                    /// <summary>
                    /// Specifies the color for the frame lines. Following values are possible:
                    /// 
                    /// * ```black```
                    /// * ```white```
                    /// * ```gray```
                    /// * ```red```
                    /// * ```blue```
                    /// * ```green```
                    /// * ```yellow```
                    /// * ```[R,G,B]``` - Red, blue, green colors in the range 0-255.
                    /// <example>242,44,97</example>
                    /// </summary>
                    [DataMember(Name = "color")]
                    [DataTypes(Pattern = @"^black$|^white$|^gray$|^red$|^blue$|^green$|^yellow$|^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$")]
                    public string Color { get; init; }

                    /// <summary>
                    /// Specifies the color for the interior of the frame. Following values are possible:
                    /// 
                    /// * ```black```
                    /// * ```white```
                    /// * ```gray```
                    /// * ```red```
                    /// * ```blue```
                    /// * ```green```
                    /// * ```yellow```
                    /// * ```[R,G,B]``` - Red, blue, green colors in the range 0-255.
                    /// <example>242,44,97</example>
                    /// </summary>
                    [DataMember(Name = "fillColor")]
                    [DataTypes(Pattern = @"^black$|^white$|^gray$|^red$|^blue$|^green$|^yellow$|^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$")]
                    public string FillColor { get; init; }

                    public enum FillStyleEnum
                    {
                        None,
                        Solid,
                        Bdiagonal,
                        Cross,
                        Diagcross,
                        Fdiagonal,
                        Horizontal,
                        Vertical
                    }

                    /// <summary>
                    /// Specifies the style for filling the interior of the frame. Following values are possible:
                    /// 
                    /// * ```none``` - No filling.
                    /// * ```solid``` - Solid color.
                    /// * ```bdiagonal``` - Downward hatch (left to right) at 45 degrees.
                    /// * ```cross``` - Horizontal and vertical crosshatch.
                    /// * ```diagcross``` - Crosshatch at 45 degrees.
                    /// * ```fdiagonal``` - Upward hatch (left to right) at 45 degrees.
                    /// * ```horizontal``` - Horizontal hatch.
                    /// * ```vertical``` - Vertical hatch.
                    /// <example>cross</example>
                    /// </summary>
                    [DataMember(Name = "fillStyle")]
                    public FillStyleEnum? FillStyle { get; init; }

                    /// <summary>
                    /// Character that is used as substitute sign when a character in a read field cannot be read.
                    /// <example>?</example>
                    /// </summary>
                    [DataMember(Name = "substSign")]
                    [DataTypes(Pattern = @".")]
                    public string SubstSign { get; init; }

                    /// <summary>
                    /// Uses the specified *field* as the title of the frame. Positioning information of the field is ignored.
                    /// <example>ExampleTitleField</example>
                    /// </summary>
                    [DataMember(Name = "title")]
                    public string Title { get; init; }

                    public enum HorizontalEnum
                    {
                        Left,
                        Right,
                        Center
                    }

                    /// <summary>
                    /// Horizontal alignment of the frame title as one of the following:
                    /// 
                    /// * ```left``` - Align to the left.
                    /// * ```right``` - Align to the right.
                    /// * ```center``` - Align to the center.
                    /// <example>center</example>
                    /// </summary>
                    [DataMember(Name = "horizontal")]
                    public HorizontalEnum? Horizontal { get; init; }

                    public enum VerticalEnum
                    {
                        Bottom,
                        Top
                    }

                    /// <summary>
                    /// Vertical alignment of the frame title as one of the following:
                    /// 
                    /// * ```bottom``` - Align to the bottom.
                    /// * ```top``` - Align to the top.
                    /// <example>bottom</example>
                    /// </summary>
                    [DataMember(Name = "vertical")]
                    public VerticalEnum? Vertical { get; init; }

                }

                /// <summary>
                /// One frame definition for each frame in the form. For each object, the property name is the frame name. The
                /// frame name within a form and its optional sub-forms must be unique. Frames are optional therefore this may be null.
                /// </summary>
                [DataMember(Name = "frames")]
                public Dictionary<string, FramesClass> Frames { get; init; }

                [DataContract]
                public sealed class SubFormsClass
                {
                    public SubFormsClass(PositionClass Position = null, SizeClass Size = null, Dictionary<string, FieldsClass> Fields = null, Dictionary<string, FramesClass> Frames = null)
                    {
                        this.Position = Position;
                        this.Size = Size;
                        this.Fields = Fields;
                        this.Frames = Frames;
                    }

                    [DataMember(Name = "position")]
                    public PositionClass Position { get; init; }

                    [DataMember(Name = "size")]
                    public SizeClass Size { get; init; }

                    [DataContract]
                    public sealed class FieldsClass
                    {
                        public FieldsClass(int? Order = null, PositionClass Position = null, string Follows = null, string Header = null, string Footer = null, SideEnum? Side = null, SizeClass Size = null, IndexClass Index = null, FieldTypeEnum? FieldType = null, ScalingEnum? Scaling = null, BarcodeEnum? Barcode = null, CoercivityEnum? Coercivity = null, ClassEnum? Class = null, AccessEnum? Access = null, OverflowEnum? Overflow = null, StyleClass Style = null, CaseEnum? Case = null, HorizontalEnum? Horizontal = null, VerticalEnum? Vertical = null, string Color = null, FontClass Font = null, string Format = null, string InitialValue = null, List<byte> InitialGraphic = null)
                        {
                            this.Order = Order;
                            this.Position = Position;
                            this.Follows = Follows;
                            this.Header = Header;
                            this.Footer = Footer;
                            this.Side = Side;
                            this.Size = Size;
                            this.Index = Index;
                            this.FieldType = FieldType;
                            this.Scaling = Scaling;
                            this.Barcode = Barcode;
                            this.Coercivity = Coercivity;
                            this.Class = Class;
                            this.Access = Access;
                            this.Overflow = Overflow;
                            this.Style = Style;
                            this.Case = Case;
                            this.Horizontal = Horizontal;
                            this.Vertical = Vertical;
                            this.Color = Color;
                            this.Font = Font;
                            this.Format = Format;
                            this.InitialValue = InitialValue;
                            this.InitialGraphic = InitialGraphic;
                        }

                        /// <summary>
                        /// Specifies the order in which the field is to be printed. Required as there is no inherent
                        /// order to JSON properties in an object. May be overridden by *follows*. If two fields are defined with the
                        /// same *order* in a given form, then the form is invalid.
                        /// <example>10</example>
                        /// </summary>
                        [DataMember(Name = "order")]
                        [DataTypes(Minimum = 0)]
                        public int? Order { get; init; }

                        [DataMember(Name = "position")]
                        public PositionClass Position { get; init; }

                        /// <summary>
                        /// Print this field directly following the field with the specified name;
                        /// positioning information is ignored. If null, then fields are printed according to *order*.
                        /// <example>Field01</example>
                        /// </summary>
                        [DataMember(Name = "follows")]
                        public string Follows { get; init; }

                        /// <summary>
                        /// This field is a form/sub-form header field.
                        /// 
                        /// - N represents a form/sub-form page number (relative to 0) the header field is to print within.
                        /// - N-N represents a form/sub-form page number range the header field is to print within.
                        /// - ALL indicates that header field is to be printed on all pages of form/sub-form.
                        /// 
                        /// Combinations of N and N-N may exist separated by commas.
                        /// 
                        /// The form/sub-form page number is intended to supplement the *z* property of the *position*.
                        /// For example 0,2-4,6 indicates that the header field is to print on relative form/sub-form pages 0, 2, 3, 4, and 6.
                        /// <example>0,2-5,7</example>
                        /// </summary>
                        [DataMember(Name = "header")]
                        [DataTypes(Pattern = @"^(([0-9]+|[0-9]+-[0-9]+),)*([0-9]+|[0-9]+-[0-9]+)+$|^ALL$")]
                        public string Header { get; init; }

                        /// <summary>
                        /// This field is a form/sub-form footer field.
                        /// 
                        /// - N represents a form/sub-form page number (relative to 0) the footer field is to print within.
                        /// - N-N represents a form/sub-form page number range the footer field is to print within.
                        /// - ALL indicates that footer field is to be printed on all pages of form/sub-form.
                        /// 
                        /// Combinations of N and N-N may exist separated by commas.
                        /// 
                        /// The form/sub-form page number is intended to supplement the *z* property of the *position*.
                        /// For example 0,2-4,6 indicates that the footer field is to print on relative form/sub-form pages 0, 2, 3, 4, and 6.
                        /// <example>0,2-5,7</example>
                        /// </summary>
                        [DataMember(Name = "footer")]
                        [DataTypes(Pattern = @"^(([0-9]+|[0-9]+-[0-9]+),)*([0-9]+|[0-9]+-[0-9]+)+$|^ALL$")]
                        public string Footer { get; init; }

                        public enum SideEnum
                        {
                            Front,
                            Back
                        }

                        /// <summary>
                        /// The side of the form as one of the following:
                        /// 
                        /// * ```front``` - the front side of the paper/media.
                        /// * ```back``` - the back side of the paper/media.
                        /// </summary>
                        [DataMember(Name = "side")]
                        public SideEnum? Side { get; init; }

                        [DataMember(Name = "size")]
                        public SizeClass Size { get; init; }

                        [DataContract]
                        public sealed class IndexClass
                        {
                            public IndexClass(int? RepeatCount = null, int? X = null, int? Y = null)
                            {
                                this.RepeatCount = RepeatCount;
                                this.X = X;
                                this.Y = Y;
                            }

                            /// <summary>
                            /// How often this field is repeated.
                            /// </summary>
                            [DataMember(Name = "repeatCount")]
                            [DataTypes(Minimum = 1)]
                            public int? RepeatCount { get; init; }

                            /// <summary>
                            /// Horizontal offset for next field
                            /// </summary>
                            [DataMember(Name = "x")]
                            [DataTypes(Minimum = 0)]
                            public int? X { get; init; }

                            /// <summary>
                            /// Vertical offset for next field
                            /// </summary>
                            [DataMember(Name = "y")]
                            [DataTypes(Minimum = 0)]
                            public int? Y { get; init; }

                        }

                        /// <summary>
                        /// Specifies that the field is to be repeated in the form/sub-form. May be null if not required.
                        /// </summary>
                        [DataMember(Name = "index")]
                        public IndexClass Index { get; init; }

                        public enum FieldTypeEnum
                        {
                            Text,
                            Micr,
                            Ocr,
                            Msf,
                            Barcode,
                            Graphic,
                            Pagemark
                        }

                        /// <summary>
                        /// The type of the field as one of the following:
                        /// 
                        /// * ```text``` - text field.
                        /// * ```micr``` - MICR field.
                        /// * ```ocr``` - OCR field.
                        /// * ```msf``` - MSF field.
                        /// * ```barcode``` - Barcode field.
                        /// * ```graphic``` - Graphic field.
                        /// * ```pagemark``` - Page mark field.
                        /// </summary>
                        [DataMember(Name = "fieldType")]
                        public FieldTypeEnum? FieldType { get; init; }

                        public enum ScalingEnum
                        {
                            BestFit,
                            AsIs,
                            MaintainAspect
                        }

                        /// <summary>
                        /// Information on how to size the graphic within the field as one of the following. Only relevant where
                        /// *fieldType* is *graphic*, therefore will be ignored for other cases:
                        /// 
                        /// * ```bestFit``` - scale to size indicated.
                        /// * ```asIs``` - render at native size.
                        /// * ```maintainAspect``` - scale as close as possible to size indicated while maintaining
                        ///   the aspect ratio and not losing graphic information.
                        /// </summary>
                        [DataMember(Name = "scaling")]
                        public ScalingEnum? Scaling { get; init; }

                        public enum BarcodeEnum
                        {
                            None,
                            Above,
                            Below,
                            Both
                        }

                        /// <summary>
                        /// Position of the HRI (Human Readable Interpretation) characters as one of the following:
                        /// 
                        /// * ```none``` - no characters or not applicable.
                        /// * ```above``` - above the barcode.
                        /// * ```below``` - below the barcode.
                        /// * ```both``` - both above and below the barcode.
                        /// </summary>
                        [DataMember(Name = "barcode")]
                        public BarcodeEnum? Barcode { get; init; }

                        public enum CoercivityEnum
                        {
                            Auto,
                            Low,
                            High
                        }

                        /// <summary>
                        /// Specifies the coercivity to be used for writing the magnetic stripe as one of the following. May be null
                        /// as this is only applicable to *msf* fields:
                        /// 
                        /// * ```auto``` - The coercivity is decided by the service or the hardware.
                        /// * ```low``` - A low coercivity is to be used for writing the magnetic stripe.
                        /// * ```high``` - A high coercivity is to be used for writing the magnetic stripe.
                        /// </summary>
                        [DataMember(Name = "coercivity")]
                        public CoercivityEnum? Coercivity { get; init; }

                        public enum ClassEnum
                        {
                            Optional,
                            Static,
                            Required
                        }

                        /// <summary>
                        /// Field class as one of the following:
                        /// 
                        /// * ```optional``` - The field is optional.
                        /// * ```static``` - The field is static.
                        /// * ```required``` - The field is required.
                        /// </summary>
                        [DataMember(Name = "class")]
                        public ClassEnum? Class { get; init; }

                        public enum AccessEnum
                        {
                            Read,
                            Write,
                            ReadWrite
                        }

                        /// <summary>
                        /// Specifies the field access as one of the following:
                        /// 
                        /// * ```read``` - The field is used for input.
                        /// * ```write``` - The field is used for output.
                        /// * ```readWrite``` - The field is used for both input and output.
                        /// </summary>
                        [DataMember(Name = "access")]
                        public AccessEnum? Access { get; init; }

                        public enum OverflowEnum
                        {
                            Terminate,
                            Truncate,
                            BestFit,
                            Overwrite,
                            WordWrap
                        }

                        /// <summary>
                        /// Specifies how an overflow of field data should be handled as one of the following:
                        /// 
                        /// * ```terminate``` - Return an error and terminate printing of the form.
                        /// * ```truncate``` - Truncate the field data to fit in the field.
                        /// * ```bestFit``` - Fit the text in the field.
                        /// * ```overwrite``` - Print the field data beyond the extents of the field boundary.
                        /// * ```wordWrap``` - If the field can hold more than one line the text is wrapped around. Wrapping
                        ///   is performed, where possible, by splitting the line on a space character or a hyphen character
                        ///   or any other character which is used to join two words together.
                        /// </summary>
                        [DataMember(Name = "overflow")]
                        public OverflowEnum? Overflow { get; init; }

                        [DataContract]
                        public sealed class StyleClass
                        {
                            public StyleClass(bool? Bold = null, bool? Italic = null, UnderlineEnum? Underline = null, WidthEnum? Width = null, StrikeEnum? Strike = null, RotateEnum? Rotate = null, CharacterSpacingEnum? CharacterSpacing = null, LineSpacingEnum? LineSpacing = null, ScriptEnum? Script = null, bool? Overscore = null, QualityEnum? Quality = null, bool? Opaque = null)
                            {
                                this.Bold = Bold;
                                this.Italic = Italic;
                                this.Underline = Underline;
                                this.Width = Width;
                                this.Strike = Strike;
                                this.Rotate = Rotate;
                                this.CharacterSpacing = CharacterSpacing;
                                this.LineSpacing = LineSpacing;
                                this.Script = Script;
                                this.Overscore = Overscore;
                                this.Quality = Quality;
                                this.Opaque = Opaque;
                            }

                            /// <summary>
                            /// Bold style.
                            /// <example>true</example>
                            /// </summary>
                            [DataMember(Name = "bold")]
                            public bool? Bold { get; init; }

                            /// <summary>
                            /// Italic style.
                            /// </summary>
                            [DataMember(Name = "italic")]
                            public bool? Italic { get; init; }

                            public enum UnderlineEnum
                            {
                                None,
                                Single,
                                Double
                            }

                            /// <summary>
                            /// Underline style as one of the following:
                            /// 
                            /// * ```none``` - No underline.
                            /// * ```single``` - Single underline.
                            /// * ```double``` - Double underline.
                            /// <example>double</example>
                            /// </summary>
                            [DataMember(Name = "underline")]
                            public UnderlineEnum? Underline { get; init; }

                            public enum WidthEnum
                            {
                                Single,
                                Double,
                                Triple,
                                Quadruple
                            }

                            /// <summary>
                            /// Width style as one of the following:
                            /// 
                            /// * ```single``` - Single width.
                            /// * ```double``` - Double width.
                            /// * ```triple``` - Triple width.
                            /// * ```quadruple``` - Quadruple width.
                            /// <example>triple</example>
                            /// </summary>
                            [DataMember(Name = "width")]
                            public WidthEnum? Width { get; init; }

                            public enum StrikeEnum
                            {
                                None,
                                Single,
                                Double
                            }

                            /// <summary>
                            /// Strike through style as one of the following:
                            /// 
                            /// * ```none``` - No strike through.
                            /// * ```single``` - Single strike through.
                            /// * ```double``` - Double strike through.
                            /// </summary>
                            [DataMember(Name = "strike")]
                            public StrikeEnum? Strike { get; init; }

                            public enum RotateEnum
                            {
                                None,
                                Ninety,
                                UpsideDown,
                                TwoSeventy
                            }

                            /// <summary>
                            /// Rotation angle as one of the following:
                            /// 
                            /// * ```none``` - No rotation.
                            /// * ```ninety``` - Rotate 90 degrees clockwise.
                            /// * ```upsideDown``` - Upside down.
                            /// * ```twoSeventy``` - Rotate 270 degrees clockwise.
                            /// </summary>
                            [DataMember(Name = "rotate")]
                            public RotateEnum? Rotate { get; init; }

                            public enum CharacterSpacingEnum
                            {
                                Standard,
                                Proportional,
                                Condensed
                            }

                            /// <summary>
                            /// Character spacing as one of the following:
                            /// 
                            /// * ```standard``` - Standard character spacing.
                            /// * ```proportional``` - Proportional character spacing.
                            /// * ```condensed``` - Condensed character spacing.
                            /// </summary>
                            [DataMember(Name = "characterSpacing")]
                            public CharacterSpacingEnum? CharacterSpacing { get; init; }

                            public enum LineSpacingEnum
                            {
                                Standard,
                                Double,
                                Triple,
                                Quadruple
                            }

                            /// <summary>
                            /// Line spacing as one of the following:
                            /// 
                            /// * ```standard``` - Standard line spacing.
                            /// * ```double``` - Double line spacing.
                            /// * ```triple``` - Triple line spacing.
                            /// * ```quadruple``` - Quadruple line spacing.
                            /// </summary>
                            [DataMember(Name = "lineSpacing")]
                            public LineSpacingEnum? LineSpacing { get; init; }

                            public enum ScriptEnum
                            {
                                Standard,
                                Superscript,
                                Subscript
                            }

                            /// <summary>
                            /// Character script type as one of the following:
                            /// 
                            /// * ```standard``` - Standard script.
                            /// * ```superscript``` - Superscript.
                            /// * ```subscript``` - Superscript.
                            /// </summary>
                            [DataMember(Name = "script")]
                            public ScriptEnum? Script { get; init; }

                            /// <summary>
                            /// Overscore style.
                            /// </summary>
                            [DataMember(Name = "overscore")]
                            public bool? Overscore { get; init; }

                            public enum QualityEnum
                            {
                                Standard,
                                Letter,
                                NearLetter
                            }

                            /// <summary>
                            /// Letter quality as one of the following:
                            /// 
                            /// * ```standard``` - Standard quality.
                            /// * ```letter``` - Letter quality.
                            /// * ```nearLetter``` - Near letter quality.
                            /// </summary>
                            [DataMember(Name = "quality")]
                            public QualityEnum? Quality { get; init; }

                            /// <summary>
                            /// If true, the printing is opaque; if false, transparent.
                            /// </summary>
                            [DataMember(Name = "opaque")]
                            public bool? Opaque { get; init; }

                        }

                        /// <summary>
                        /// Style attributes using a combination of the following. Some of the styles may be mutually exclusive; they 
                        /// may provide unexpected results if combined. This field is optional and may be null, in which case the 
                        /// default normal style will be used.
                        /// </summary>
                        [DataMember(Name = "style")]
                        public StyleClass Style { get; init; }

                        public enum CaseEnum
                        {
                            NoChange,
                            Upper,
                            Lower
                        }

                        /// <summary>
                        /// Convert field contents to one of the following:
                        /// 
                        /// * ```noChange``` - No change.
                        /// * ```upper``` - Convert to upper case.
                        /// * ```lower``` - Convert to lower case.
                        /// </summary>
                        [DataMember(Name = "case")]
                        public CaseEnum? Case { get; init; }

                        public enum HorizontalEnum
                        {
                            Left,
                            Right,
                            Center,
                            Justify
                        }

                        /// <summary>
                        /// Horizontal alignment of field contents as one of the following:
                        /// 
                        /// * ```left``` - Align to the left.
                        /// * ```right``` - Align to the right.
                        /// * ```center``` - Align to the center.
                        /// * ```justify``` - Justify the contents.
                        /// </summary>
                        [DataMember(Name = "horizontal")]
                        public HorizontalEnum? Horizontal { get; init; }

                        public enum VerticalEnum
                        {
                            Bottom,
                            Center,
                            Top
                        }

                        /// <summary>
                        /// Vertical alignment of field contents as one of the following:
                        /// 
                        /// * ```bottom``` - Align to the bottom.
                        /// * ```center``` - Align to the center.
                        /// * ```top``` - Align to the top.
                        /// </summary>
                        [DataMember(Name = "vertical")]
                        public VerticalEnum? Vertical { get; init; }

                        /// <summary>
                        /// Specifies the color. Following values are possible:
                        /// 
                        /// * ```black```
                        /// * ```white```
                        /// * ```gray```
                        /// * ```red```
                        /// * ```blue```
                        /// * ```green```
                        /// * ```yellow```
                        /// * ```[R,G,B]``` - Red, blue, green colors in the range 0-255.
                        /// <example>242,44,97</example>
                        /// </summary>
                        [DataMember(Name = "color")]
                        [DataTypes(Pattern = @"^black$|^white$|^gray$|^red$|^blue$|^green$|^yellow$|^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$")]
                        public string Color { get; init; }

                        [DataContract]
                        public sealed class FontClass
                        {
                            public FontClass(string Name = null, int? PointSize = null, int? Cpi = null, int? Lpi = null)
                            {
                                this.Name = Name;
                                this.PointSize = PointSize;
                                this.Cpi = Cpi;
                                this.Lpi = Lpi;
                            }

                            /// <summary>
                            /// Font name: This property is interpreted by the service. In some cases, it may indicate printer resident
                            /// fonts, and in others it may indicate the name of a downloadable font. For *barcode* fields it represents
                            /// the barcode font name.
                            /// 
                            /// In some cases, this pre-defines other properties in this object.
                            /// <example>Courier New</example>
                            /// </summary>
                            [DataMember(Name = "name")]
                            public string Name { get; init; }

                            /// <summary>
                            /// Point size. If 0, the point size defaults to the *pointSize* defined for the form.
                            /// <example>10</example>
                            /// </summary>
                            [DataMember(Name = "pointSize")]
                            [DataTypes(Minimum = 0)]
                            public int? PointSize { get; init; }

                            /// <summary>
                            /// Characters per inch. If 0, the CPI defaults to the CPI defined for the form.
                            /// <example>5</example>
                            /// </summary>
                            [DataMember(Name = "cpi")]
                            [DataTypes(Minimum = 0)]
                            public int? Cpi { get; init; }

                            /// <summary>
                            /// Lines per inch. If 0, the LPI defaults to the LPI defined for the form.
                            /// <example>5</example>
                            /// </summary>
                            [DataMember(Name = "lpi")]
                            [DataTypes(Minimum = 0)]
                            public int? Lpi { get; init; }

                        }

                        /// <summary>
                        /// The font to be used. If null, the default font is used.
                        /// </summary>
                        [DataMember(Name = "font")]
                        public FontClass Font { get; init; }

                        /// <summary>
                        /// This is an application defined input field describing how the application should format the data. This may be interpreted by the service.
                        /// <example>Application defined information</example>
                        /// </summary>
                        [DataMember(Name = "format")]
                        public string Format { get; init; }

                        /// <summary>
                        /// Initial value, which may be changed dynamically at run-time. May be null if no initial value is required.
                        /// Ignored for *graphic* fields, which are specified in *initialGraphic*.
                        /// <example>Default text</example>
                        /// </summary>
                        [DataMember(Name = "initialValue")]
                        public string InitialValue { get; init; }

                        /// <summary>
                        /// Initial graphic in Base64 format, which may be changed dynamically at run-time. May be null if no initial
                        /// graphic is required. Ignored for non-*graphic* fields, which are specified in *initialValue*.
                        /// <example>O2gAUACFyEARAJAC</example>
                        /// </summary>
                        [DataMember(Name = "initialGraphic")]
                        [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                        public List<byte> InitialGraphic { get; init; }

                    }

                    /// <summary>
                    /// One field definition for each field in the sub-form. The field name within a form and its optional sub-forms
                    /// must be unique.
                    /// </summary>
                    [DataMember(Name = "fields")]
                    public Dictionary<string, FieldsClass> Fields { get; init; }

                    [DataContract]
                    public sealed class FramesClass
                    {
                        public FramesClass(PositionClass Position = null, string Frames = null, string Header = null, string Footer = null, SideEnum? Side = null, SizeClass Size = null, RepeatClass Repeat = null, FieldTypeEnum? FieldType = null, ClassEnum? Class = null, OverflowEnum? Overflow = null, StyleEnum? Style = null, string Color = null, string FillColor = null, FillStyleEnum? FillStyle = null, string SubstSign = null, string Title = null, HorizontalEnum? Horizontal = null, VerticalEnum? Vertical = null)
                        {
                            this.Position = Position;
                            this.Frames = Frames;
                            this.Header = Header;
                            this.Footer = Footer;
                            this.Side = Side;
                            this.Size = Size;
                            this.Repeat = Repeat;
                            this.FieldType = FieldType;
                            this.Class = Class;
                            this.Overflow = Overflow;
                            this.Style = Style;
                            this.Color = Color;
                            this.FillColor = FillColor;
                            this.FillStyle = FillStyle;
                            this.SubstSign = SubstSign;
                            this.Title = Title;
                            this.Horizontal = Horizontal;
                            this.Vertical = Vertical;
                        }

                        [DataMember(Name = "position")]
                        public PositionClass Position { get; init; }

                        /// <summary>
                        /// Frames the specified field, positioning and size information are ignored. The frame surrounds the
                        /// complete field, not just the printed data. If the field is repeated, the frame surrounds the first and last
                        /// fields that are printed.
                        /// <example>Field01</example>
                        /// </summary>
                        [DataMember(Name = "frames")]
                        public string Frames { get; init; }

                        /// <summary>
                        /// This frame is a form/sub-form header frame.
                        /// 
                        /// - N represents a form/sub-form page number (relative to 0) the header frame is to print within.
                        /// - N-N represents a form/sub-form page number range the header frame is to print within.
                        /// - ALL indicates that header frame is to be printed on all pages of form/sub-form.
                        /// 
                        /// Combinations of N and N-N may exist separated by commas.
                        /// 
                        /// The form/sub-form page number is intended to supplement the *z* property of the *position*.
                        /// For example 0,2-4,6 indicates that the header frame is to print on relative form/sub-form pages 0, 2, 3, 4, and 6.
                        /// <example>0,2-5,7</example>
                        /// </summary>
                        [DataMember(Name = "header")]
                        [DataTypes(Pattern = @"^(([0-9]+|[0-9]+-[0-9]+),)*([0-9]+|[0-9]+-[0-9]+)+$|^ALL$")]
                        public string Header { get; init; }

                        /// <summary>
                        /// This frame is a form/sub-form footer frame.
                        /// 
                        /// - N represents a form/sub-form page number (relative to 0) the footer frame is to print within.
                        /// - N-N represents a form/sub-form page number range the footer frame is to print within.
                        /// - ALL indicates that footer frame is to be printed on all pages of form/sub-form.
                        /// 
                        /// Combinations of N and N-N may exist separated by commas.
                        /// 
                        /// The form/sub-form page number is intended to supplement the *z* property of the *position*.
                        /// For example 0,2-4,6 indicates that the footer frame is to print on relative form/sub-form pages 0, 2, 3, 4, and 6.
                        /// <example>0,2-5,7</example>
                        /// </summary>
                        [DataMember(Name = "footer")]
                        [DataTypes(Pattern = @"^(([0-9]+|[0-9]+-[0-9]+),)*([0-9]+|[0-9]+-[0-9]+)+$|^ALL$")]
                        public string Footer { get; init; }

                        public enum SideEnum
                        {
                            Front,
                            Back
                        }

                        /// <summary>
                        /// The side of the form where this frame is positioned as one of the following:
                        /// 
                        /// * ```front``` - the front side of the paper/media.
                        /// * ```back``` - the back side of the paper/media.
                        /// <example>back</example>
                        /// </summary>
                        [DataMember(Name = "side")]
                        public SideEnum? Side { get; init; }

                        [DataMember(Name = "size")]
                        public SizeClass Size { get; init; }

                        [DataContract]
                        public sealed class RepeatClass
                        {
                            public RepeatClass(int? RepeatCountX = null, int? OffsetX = null, int? RepeatCountY = null, int? OffsetY = null)
                            {
                                this.RepeatCountX = RepeatCountX;
                                this.OffsetX = OffsetX;
                                this.RepeatCountY = RepeatCountY;
                                this.OffsetY = OffsetY;
                            }

                            /// <summary>
                            /// How often this frame is repeated horizontally in the form.
                            /// <example>2</example>
                            /// </summary>
                            [DataMember(Name = "repeatCountX")]
                            [DataTypes(Minimum = 0)]
                            public int? RepeatCountX { get; init; }

                            /// <summary>
                            /// Horizontal offset for next frame.
                            /// <example>4</example>
                            /// </summary>
                            [DataMember(Name = "offsetX")]
                            [DataTypes(Minimum = 0)]
                            public int? OffsetX { get; init; }

                            /// <summary>
                            /// How often this frame is repeated vertically in the form.
                            /// <example>1</example>
                            /// </summary>
                            [DataMember(Name = "repeatCountY")]
                            [DataTypes(Minimum = 0)]
                            public int? RepeatCountY { get; init; }

                            /// <summary>
                            /// Vertical offset for next frame.
                            /// <example>3</example>
                            /// </summary>
                            [DataMember(Name = "offsetY")]
                            [DataTypes(Minimum = 0)]
                            public int? OffsetY { get; init; }

                        }

                        /// <summary>
                        /// Specifies that the frame is to be repeated in the form/sub-form. May be null if not required.
                        /// </summary>
                        [DataMember(Name = "repeat")]
                        public RepeatClass Repeat { get; init; }

                        public enum FieldTypeEnum
                        {
                            Rectangle,
                            RoundedCorner,
                            Ellipse
                        }

                        /// <summary>
                        /// The type of the frame as one of the following:
                        /// 
                        /// * ```rectangle``` - Rectangle.
                        /// * ```roundedCorner``` - Rounded corner.
                        /// * ```ellipse``` - Ellipse.
                        /// <example>ellipse</example>
                        /// </summary>
                        [DataMember(Name = "fieldType")]
                        public FieldTypeEnum? FieldType { get; init; }

                        public enum ClassEnum
                        {
                            Static,
                            Optional
                        }

                        /// <summary>
                        /// Frame class as one of the following:
                        /// 
                        /// * ```static``` - The frame is static.
                        /// * ```optional``` - The frame is optional; it is printed only if its name appears in the list of field
                        ///   names given as parameter to the command. In this case, the name of the frame must be different from all
                        ///   the names of the fields.
                        /// <example>optional</example>
                        /// </summary>
                        [DataMember(Name = "class")]
                        public ClassEnum? Class { get; init; }

                        public enum OverflowEnum
                        {
                            Terminate,
                            Truncate,
                            BestFit
                        }

                        /// <summary>
                        /// Action on frame overflowing the form as one of the following:
                        /// 
                        /// * ```terminate``` - Return an error and terminate printing of the form.
                        /// * ```truncate``` - Truncate to fit in the field.
                        /// * ```bestFit``` - Fit the text in the field.
                        /// <example>truncate</example>
                        /// </summary>
                        [DataMember(Name = "overflow")]
                        public OverflowEnum? Overflow { get; init; }

                        public enum StyleEnum
                        {
                            SingleThin,
                            DoubleThin,
                            SingleThick,
                            DoubleThick,
                            Dotted
                        }

                        /// <summary>
                        /// Frame line attributes as one of the following:
                        /// 
                        /// * ```singleThin``` - A single thin line.
                        /// * ```doubleThin``` - A double thin line.
                        /// * ```singleThick``` - A single thick line.
                        /// * ```doubleThick``` - A double thick line.
                        /// * ```dotted``` - A dotted line.
                        /// <example>doubleThick</example>
                        /// </summary>
                        [DataMember(Name = "style")]
                        public StyleEnum? Style { get; init; }

                        /// <summary>
                        /// Specifies the color for the frame lines. Following values are possible:
                        /// 
                        /// * ```black```
                        /// * ```white```
                        /// * ```gray```
                        /// * ```red```
                        /// * ```blue```
                        /// * ```green```
                        /// * ```yellow```
                        /// * ```[R,G,B]``` - Red, blue, green colors in the range 0-255.
                        /// <example>242,44,97</example>
                        /// </summary>
                        [DataMember(Name = "color")]
                        [DataTypes(Pattern = @"^black$|^white$|^gray$|^red$|^blue$|^green$|^yellow$|^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$")]
                        public string Color { get; init; }

                        /// <summary>
                        /// Specifies the color for the interior of the frame. Following values are possible:
                        /// 
                        /// * ```black```
                        /// * ```white```
                        /// * ```gray```
                        /// * ```red```
                        /// * ```blue```
                        /// * ```green```
                        /// * ```yellow```
                        /// * ```[R,G,B]``` - Red, blue, green colors in the range 0-255.
                        /// <example>242,44,97</example>
                        /// </summary>
                        [DataMember(Name = "fillColor")]
                        [DataTypes(Pattern = @"^black$|^white$|^gray$|^red$|^blue$|^green$|^yellow$|^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]),([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$")]
                        public string FillColor { get; init; }

                        public enum FillStyleEnum
                        {
                            None,
                            Solid,
                            Bdiagonal,
                            Cross,
                            Diagcross,
                            Fdiagonal,
                            Horizontal,
                            Vertical
                        }

                        /// <summary>
                        /// Specifies the style for filling the interior of the frame. Following values are possible:
                        /// 
                        /// * ```none``` - No filling.
                        /// * ```solid``` - Solid color.
                        /// * ```bdiagonal``` - Downward hatch (left to right) at 45 degrees.
                        /// * ```cross``` - Horizontal and vertical crosshatch.
                        /// * ```diagcross``` - Crosshatch at 45 degrees.
                        /// * ```fdiagonal``` - Upward hatch (left to right) at 45 degrees.
                        /// * ```horizontal``` - Horizontal hatch.
                        /// * ```vertical``` - Vertical hatch.
                        /// <example>cross</example>
                        /// </summary>
                        [DataMember(Name = "fillStyle")]
                        public FillStyleEnum? FillStyle { get; init; }

                        /// <summary>
                        /// Character that is used as substitute sign when a character in a read field cannot be read.
                        /// <example>?</example>
                        /// </summary>
                        [DataMember(Name = "substSign")]
                        [DataTypes(Pattern = @".")]
                        public string SubstSign { get; init; }

                        /// <summary>
                        /// Uses the specified *field* as the title of the frame. Positioning information of the field is ignored.
                        /// <example>ExampleTitleField</example>
                        /// </summary>
                        [DataMember(Name = "title")]
                        public string Title { get; init; }

                        public enum HorizontalEnum
                        {
                            Left,
                            Right,
                            Center
                        }

                        /// <summary>
                        /// Horizontal alignment of the frame title as one of the following:
                        /// 
                        /// * ```left``` - Align to the left.
                        /// * ```right``` - Align to the right.
                        /// * ```center``` - Align to the center.
                        /// <example>center</example>
                        /// </summary>
                        [DataMember(Name = "horizontal")]
                        public HorizontalEnum? Horizontal { get; init; }

                        public enum VerticalEnum
                        {
                            Bottom,
                            Top
                        }

                        /// <summary>
                        /// Vertical alignment of the frame title as one of the following:
                        /// 
                        /// * ```bottom``` - Align to the bottom.
                        /// * ```top``` - Align to the top.
                        /// <example>bottom</example>
                        /// </summary>
                        [DataMember(Name = "vertical")]
                        public VerticalEnum? Vertical { get; init; }

                    }

                    /// <summary>
                    /// One frame definition for each frame in the sub-form. For each object, the property name is the frame name.
                    /// The frame name within a form and its optional sub-forms must be unique. Frames are optional therefore this
                    /// may be null.
                    /// </summary>
                    [DataMember(Name = "frames")]
                    public Dictionary<string, FramesClass> Frames { get; init; }

                }

                /// <summary>
                /// One sub-form definition for each sub-form in the form. For each object, the property name is the frame name.
                /// The sub-form name within a form and must be unique. Sub-forms are optional therefore this may be null.
                /// </summary>
                [DataMember(Name = "subForms")]
                public Dictionary<string, SubFormsClass> SubForms { get; init; }

            }

            /// <summary>
            /// The details of the form. May be null if the form is to be deleted.
            /// </summary>
            [DataMember(Name = "form")]
            public FormClass Form { get; init; }

        }
    }
}
