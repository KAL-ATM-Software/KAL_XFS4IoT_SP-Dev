/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryField_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "Printer.GetQueryField")]
    public sealed class GetQueryFieldCompletion : Completion<GetQueryFieldCompletion.PayloadData>
    {
        public GetQueryFieldCompletion()
            : base()
        { }

        public GetQueryFieldCompletion(int RequestId, GetQueryFieldCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, FieldsClass> Fields = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Fields = Fields;
            }

            public enum ErrorCodeEnum
            {
                FormNotFound,
                FieldNotFound
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```formNotFound``` - The specified form cannot be found.
            /// * ```fieldNotFound``` - The specified field cannot be found.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

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
            /// Details of the field(s) requested. For each object, the property name is the field name. This property
            /// is null if the form or field(s) cannot be found.
            /// </summary>
            [DataMember(Name = "fields")]
            public Dictionary<string, FieldsClass> Fields { get; init; }

        }
    }
}
