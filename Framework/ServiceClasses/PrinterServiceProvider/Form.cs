/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.Printer.Completions;
using Microsoft.VisualBasic.FileIO;
using System.Drawing;
using System.Reflection.PortableExecutable;
using System.Security.Policy;

namespace XFS4IoTFramework.Printer
{
    /// <summary>
    /// Orientation for a print job.  Text runs across the media for
    /// PORTRAIT prints and down the media for LANDSCAPE prints.
    /// </summary>
    [Flags]
    public enum FormOrientationEnum
    {
        PORTRAIT = 1 << 0,
        LANDSCAPE = 1 <<1,
    }

    /// <summary>
    /// The side of the form on which a task is printed.
    /// </summary>
    [Flags]
    public enum FieldSideEnum
    {
        FRONT = 0x0001,
        BACK = 0x0002,
    }

    /// <summary>
    /// Style of field
    /// </summary>
    [Flags]
    public enum FieldStyleEnum : long
    {
        NORMAL = 1 << 0,
        BOLD = 1 << 1,
        ITALIC = 1 << 2,
        UNDER = 1 << 3,
        DOUBLEUNDER = 1 << 4,
        DOUBLE = 1 << 5,
        TRIPLE = 1 << 6,
        QUADRUPLE = 1 << 7,
        STRIKETHROUGH = 1 << 8,
        ROTATE90 = 1 << 9,
        ROTATE270 = 1 << 10,
        UPSIDEDOWN = 1 << 11,
        PROPORTIONAL = 1 << 12,
        DOUBLEHIGH = 1 << 13,
        TRIPLEHIGH = 1 << 14,
        QUADRUPLEHIGH = 1 << 15,
        CONDENSED = 1 << 16,
        SUPERSCRIPT = 1 << 17,
        SUBSCRIPT = 1 << 18,
        OVERSCORE = 1 << 19,
        LETTERQUALITY = 1 << 20,
        NEARLETTERQUALITY = 1 << 21,
        DOUBLESTRIKE = 1 << 22,
        OPAQUE = 1 << 23,
    }

    /// <summary>
    /// Type of a print task
    /// </summary>
    [Flags]
    public enum FieldTypeEnum
    {
        TEXT = 1 << 0,
        MICR = 1 << 1,
        OCR = 1 << 2,
        MSF = 1 << 3,
        BARCODE = 1 << 4,
        GRAPHIC = 1 << 5,
        PAGEMARK = 1 << 6,
    }

    /// <summary>
    /// Access type of a task: WRITE for tasks to print, READ for tasks to scan 
    /// </summary>
    [Flags]
    public enum FieldAccessEnum
    {
        WRITE = 1 << 0,
        READ = 1 << 1,
        READWRITE = 1 << 2,
    }

    /// <summary>
    /// for graphic tasks indicates how they can be scaled
    /// </summary>
    [Flags]
    public enum FieldScalingEnum
    {
        BESTFIT = 1 << 0,
        ASIS = 1 << 1,
        MAINTAINASPECT = 1 << 2
    }

    /// <summary>
    /// for barcode tasks, indicates where to print the human readable form
    /// </summary>
    [Flags]
    public enum FieldBarcodeEnum
    {
        NONE = 0,
        ABOVE = 1 << 0,
        BELOW = 1 << 1,
        BOTH = 1 << 2,
    }

    /// <summary>
    /// color to use for printing
    /// </summary>
    [Flags]
    public enum FieldColorEnum
    {
        BLACK = 1 << 0,
        WHITE = 1 << 1,
        GRAY = 1 << 2,
        RED = 1 << 3,
        BLUE = 1 << 4,
        GREEN = 1 << 5,
        YELLOW = 1 << 6,
    }

    // This class represents one printer form definition.
    [Serializable]
    public sealed class Form
    {
        /// <summary>
        /// the base unit of measurement of the form
        /// </summary>
        public enum BaseEnum
        {
            INCH,
            MM,
            ROWCOLUMN,
        }

        /// <summary>
        /// the relative alignment of the form on the media 
        /// </summary>
        public enum AlignmentEnum
        {
            TOPLEFT,
            TOPRIGHT,
            BOTTOMLEFT,
            BOTTOMRIGHT,
        }

        /// <summary> 
        /// the form keywords
        /// </summary>
        public enum FormkeywordEnum
        {
            UNIT,
            SIZE,
            ALIGNMENT,
            ORIENTATION,
            SKEW,
            VERSION,
            LANGUAGE,
            COPYRIGHT,
            TITLE,
            COMMENT,
            USERPROMPT,
            XFSFIELD,
        }

        // Constructor
        // Used by form designer for creating new forms
        public Form(ILogger Logger,
                    IPrinterDevice Device,
                    string Name,
                    BaseEnum Base,
                    int UnitX,
                    int UnitY,
                    int Width,
                    int Height,
                    int XOffset,
                    int YOffset,
                    int? VersionMajor,
                    int? VersionMinor,
                    string Date,
                    string Author,
                    string Copyright,
                    string Title,
                    string Comment,
                    string Prompt,
                    int Skew,
                    AlignmentEnum Alignment,
                    FormOrientationEnum Orientation)
        {
            this.Logger = Logger;
            this.Name = Name;
            this.Base = Base;
            this.UnitX = UnitX;
            this.UnitY = UnitY;
            this.Width = Width;
            this.Height = Height;
            this.XOffset = XOffset;
            this.YOffset = YOffset;
            this.VersionMajor = VersionMajor;
            this.VersionMinor = VersionMinor;
            this.Date = Date;
            this.Author = Author;
            this.Copyright = Copyright;
            this.Title = Title;
            this.Comment = Comment;
            this.Prompt = Prompt;
            this.Skew = Skew;
            this.Alignment = Alignment;
            this.Orientation = Orientation;
            this.Fields = [];

            // Get the appropriate conversion factor (Enum/Denom) according to the base units
            switch (Base)
            {
                case BaseEnum.MM:
                    RowEnum = Device.DotsPerMMTopY;
                    RowDenom = Device.DotsPerMMBottomY;
                    ColEnum = Device.DotsPerMMTopX;
                    ColDenom = Device.DotsPerMMBottomX;
                    break;
                case BaseEnum.INCH:
                    RowEnum = Device.DotsPerInchTopY;
                    RowDenom = Device.DotsPerInchBottomY;
                    ColEnum = Device.DotsPerInchTopX;
                    ColDenom = Device.DotsPerInchBottomX;
                    break;
                case BaseEnum.ROWCOLUMN:
                    RowEnum = Device.DotsPerRowTop;
                    RowDenom = Device.DotsPerRowBottom;
                    ColEnum = Device.DotsPerColumnTop;
                    ColDenom = Device.DotsPerColumnBottom;
                    break;
                default:
                    {
                        Contracts.Assert(false, $"Unexpected result on swith case. " + nameof(Form));
                    }
                    break;
            }

            // Switch Row & Column for Landscape prints
            if (Orientation == FormOrientationEnum.LANDSCAPE)
            {
                int Tmp = RowEnum;
                RowEnum = ColEnum;
                ColEnum = Tmp;
                Tmp = RowDenom;
                RowDenom = ColDenom;
                ColDenom = Tmp;
            }

            // So far Denoms are for 1 MM/Inch/RowCol.
            // Convert for form unit
            ColDenom *= UnitX;
            RowDenom *= UnitY;

            Contracts.Assert(RowDenom > 0 && ColDenom > 0, $"Form contains an illegal UNIT specification.  Base unit fractions must be greater than zero. {Name}");

            // Now convert all measurements
            // Round all of them up to avoid overflows due to rounding errors.
            DotWidth = XConvertToDots(Width);
            DotHeight = YConvertToDots(Height);
            DotXOffset = XConvertToDots(XOffset);
            DotYOffset = YConvertToDots(YOffset);
        }

        /// <summary>
        /// Add field structure
        /// </summary>
        public void AddField(string Name,
                             int X,
                             int Y,
                             int Width,
                             int Height,
                             string Follows,
                             int Repeat,
                             int XOffset,
                             int YOffset,
                             string Font,
                             int PointSize,
                             int CPI,
                             int LPI,
                             string Format,
                             string InitialValue,
                             FieldSideEnum Side,
                             FieldTypeEnum Type,
                             FormField.ClassEnum Class,
                             FieldAccessEnum Access,
                             FormField.OverflowEnum Overflow,
                             FieldStyleEnum Style,
                             FormField.CaseEnum Case,
                             FormField.HorizontalEnum Horizontal,
                             FormField.VerticalEnum Vertical,
                             FieldColorEnum Color,
                             FieldScalingEnum Scaling,
                             FieldBarcodeEnum Barcode)
        {
            Fields.Add(
                Name, 
                new(
                    Name,
                    X,
                    Y,
                    Width,
                    Height,
                    XConvertToDots(X),
                    YConvertToDots(Y),
                    XConvertToDots(Width),
                    YConvertToDots(Height),
                    Follows,
                    Repeat,
                    XOffset,
                    YOffset,
                    XConvertToDots(XOffset),
                    YConvertToDots(YOffset),
                    Font,
                    PointSize,
                    CPI,
                    LPI,
                    Format,
                    InitialValue,
                    Side,
                    Type,
                    Class,
                    Access,
                    Overflow,
                    Style,
                    Case,
                    Horizontal,
                    Vertical,
                    Color,
                    Scaling,
                    Barcode));
        }

        /// <summary>
        /// Return properties required for the GetQueryForm command
        /// </summary>
        /// <returns></returns>
        public CommandResult<GetQueryFormCompletion.PayloadData> QueryForm(IPrinterDevice Device)
        {
            var result = ValidateForm(Device);
            if (result.Result != ValidationResultClass.ValidateResultEnum.Valid)
            {
                return new(
                    new(ErrorCode: GetQueryFormCompletion.PayloadData.ErrorCodeEnum.FormNotFound),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Form {Name} is not supported by the device.");
            }

            Dictionary<string, GetQueryFormCompletion.PayloadData.FormClass.FieldsClass> fields = [];
            foreach (var field in Fields)
            {
                GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass style = null;
                if (field.Value.Style != FieldStyleEnum.NORMAL)
                {
                    GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.UnderlineEnum? underline = null;
                    if (field.Value.Style.HasFlag(FieldStyleEnum.UNDER))
                    {
                        underline = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.UnderlineEnum.Single;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.DOUBLEUNDER))
                    {
                        underline = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.UnderlineEnum.Double;
                    }

                    GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.WidthEnum? width = null;
                    if (field.Value.Style.HasFlag(FieldStyleEnum.DOUBLE))
                    {
                        width = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.WidthEnum.Double;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.TRIPLE))
                    {
                        width = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.WidthEnum.Triple;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.QUADRUPLE))
                    {
                        width = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.WidthEnum.Quadruple;
                    }

                    GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.StrikeEnum? strike = null;
                    if (field.Value.Style.HasFlag(FieldStyleEnum.DOUBLESTRIKE))
                    {
                        strike = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.StrikeEnum.Double;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.STRIKETHROUGH))
                    {
                        strike = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.StrikeEnum.Single;
                    }

                    GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.RotateEnum? rotate = null;
                    if (field.Value.Style.HasFlag(FieldStyleEnum.ROTATE90))
                    {
                        rotate = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.RotateEnum.Ninety;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.ROTATE270))
                    {
                        rotate = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.RotateEnum.TwoSeventy;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.UPSIDEDOWN))
                    {
                        rotate = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.RotateEnum.UpsideDown;
                    }

                    GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.CharacterSpacingEnum? characterSpacing = null;
                    if (field.Value.Style.HasFlag(FieldStyleEnum.CONDENSED))
                    {
                        characterSpacing = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.CharacterSpacingEnum.Condensed;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.PROPORTIONAL))
                    {
                        characterSpacing = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.CharacterSpacingEnum.Proportional;
                    }

                    GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.LineSpacingEnum? lineSpacing = null;
                    if (field.Value.Style.HasFlag(FieldStyleEnum.DOUBLEHIGH))
                    {
                        lineSpacing = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.LineSpacingEnum.Double;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.TRIPLEHIGH))
                    {
                        lineSpacing = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.LineSpacingEnum.Triple;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.QUADRUPLEHIGH))
                    {
                        lineSpacing = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.LineSpacingEnum.Quadruple;
                    }

                    GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.ScriptEnum? script = null;
                    if (field.Value.Style.HasFlag(FieldStyleEnum.SUPERSCRIPT))
                    {
                        script = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.ScriptEnum.Superscript;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.SUBSCRIPT))
                    {
                        script = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.ScriptEnum.Subscript;
                    }

                    GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.QualityEnum? quality = null;
                    if (field.Value.Style.HasFlag(FieldStyleEnum.LETTERQUALITY))
                    {
                        quality = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.QualityEnum.Letter;
                    }
                    else if (field.Value.Style.HasFlag(FieldStyleEnum.NEARLETTERQUALITY))
                    {
                        quality = GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.StyleClass.QualityEnum.NearLetter;
                    }

                    style = new(
                    Bold: field.Value.Style.HasFlag(FieldStyleEnum.BOLD) ? true : null,
                    Italic: field.Value.Style.HasFlag(FieldStyleEnum.ITALIC) ? true : null,
                    Underline: underline,
                    Width: width,
                    Strike: strike,
                    Rotate: rotate,
                    CharacterSpacing: characterSpacing,
                    LineSpacing: lineSpacing,
                    Script: script,
                    Overscore: field.Value.Style.HasFlag(FieldStyleEnum.OVERSCORE) ? true : null,
                    Quality: quality,
                    Opaque: field.Value.Style.HasFlag(FieldStyleEnum.OPAQUE) ? true : null);
                }

                fields.Add(
                    field.Key,
                    new GetQueryFormCompletion.PayloadData.FormClass.FieldsClass(
                        Position: new(
                            X: field.Value.X,
                            Y: field.Value.Y),
                        Follows: string.IsNullOrEmpty(field.Value.Follows) ? null : field.Value.Follows,
                        Side: field.Value.Side switch
                        {
                            FieldSideEnum.FRONT => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.SideEnum.Front,
                            FieldSideEnum.BACK => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.SideEnum.Back,
                            _ => null,
                        },
                        Size: new(
                            Width: field.Value.Width,
                            Height: field.Value.Height),
                        Index: field.Value.Repeat > 0 ? new(
                            RepeatCount: field.Value.Repeat,
                            X: field.Value.XOffset,
                            Y: field.Value.YOffset) : null,
                        FieldType: field.Value.Type switch
                        {
                            FieldTypeEnum.BARCODE => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.FieldTypeEnum.Barcode,
                            FieldTypeEnum.GRAPHIC => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.FieldTypeEnum.Graphic,
                            FieldTypeEnum.MICR => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.FieldTypeEnum.Micr,
                            FieldTypeEnum.MSF => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.FieldTypeEnum.Msf,
                            FieldTypeEnum.OCR => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.FieldTypeEnum.Ocr,
                            FieldTypeEnum.PAGEMARK => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.FieldTypeEnum.Pagemark,
                            FieldTypeEnum.TEXT => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.FieldTypeEnum.Text,
                            _ => null,
                        },
                        Scaling: field.Value.Scaling switch
                        {
                            FieldScalingEnum.ASIS => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.ScalingEnum.AsIs,
                            FieldScalingEnum.BESTFIT => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.ScalingEnum.BestFit,
                            FieldScalingEnum.MAINTAINASPECT => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.ScalingEnum.MaintainAspect,
                            _ => null,
                        },
                        Barcode: field.Value.Barcode switch
                        {
                            FieldBarcodeEnum.ABOVE => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.BarcodeEnum.Above,
                            FieldBarcodeEnum.BELOW => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.BarcodeEnum.Below,
                            FieldBarcodeEnum.BOTH => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.BarcodeEnum.Both,
                            _ => null,
                        },
                        Coercivity: null,
                        Class: field.Value.Class switch
                        {
                            FormField.ClassEnum.OPTIONAL => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.ClassEnum.Optional,
                            FormField.ClassEnum.REQUIRED => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.ClassEnum.Required,
                            FormField.ClassEnum.STATIC => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.ClassEnum.Static,
                            _ => null,
                        },
                        Access: field.Value.Access switch
                        {
                            FieldAccessEnum.READ => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.AccessEnum.Read,
                            FieldAccessEnum.WRITE => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.AccessEnum.Write,
                            FieldAccessEnum.READWRITE => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.AccessEnum.ReadWrite,
                            _ => null,
                        },
                        Overflow: field.Value.Overflow switch
                        {
                            FormField.OverflowEnum.BESTFIT => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.OverflowEnum.BestFit,
                            FormField.OverflowEnum.OVERWRITE => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.OverflowEnum.Overwrite,
                            FormField.OverflowEnum.TERMINATE => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.OverflowEnum.Terminate,
                            FormField.OverflowEnum.TRUNCATE => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.OverflowEnum.Truncate,
                            _ => null,
                        },
                        Style: style,
                        Case: field.Value.Case switch
                        {
                            FormField.CaseEnum.LOWER => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.CaseEnum.Lower,
                            FormField.CaseEnum.UPPER => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.CaseEnum.Upper,
                            _ => null,
                        },
                        Horizontal: field.Value.Horizontal switch
                        {
                            FormField.HorizontalEnum.CENTER => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.HorizontalEnum.Center,
                            FormField.HorizontalEnum.JUSTIFY => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.HorizontalEnum.Justify,
                            FormField.HorizontalEnum.LEFT => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.HorizontalEnum.Left,
                            FormField.HorizontalEnum.RIGHT => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.HorizontalEnum.Right,
                            _ => null,
                        },
                        Vertical: field.Value.Vertical switch
                        {
                            FormField.VerticalEnum.BOTTOM => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.VerticalEnum.Bottom,
                            FormField.VerticalEnum.CENTER => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.VerticalEnum.Center,
                            FormField.VerticalEnum.TOP => GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.VerticalEnum.Top,
                            _ => null,
                        }, 
                        Color: field.Value.Color > 0 ? field.Value.Color.ToString().ToLower() : null,
                        Font: string.IsNullOrEmpty(field.Value.Font) ? null : new GetQueryFormCompletion.PayloadData.FormClass.FieldsClass.FontClass(
                            Name: field.Value.Font,
                            PointSize: field.Value.PointSize > 0 ? field.Value.PointSize : null,
                            Cpi: field.Value.CPI > 0 ? field.Value.CPI : null,
                            Lpi: field.Value.LPI > 0 ? field.Value.LPI : null),
                        Format: string.IsNullOrEmpty(field.Value.Format) ? null : field.Value.Format,
                        InitialValue: string.IsNullOrEmpty(field.Value.InitialValue) ? null : field.Value.InitialValue)
                    );
            }

            // Check all fields
            foreach (var field in fields)
            {
                var fieldResult = ValidateField(field.Key, Device);
                if (fieldResult.Result != ValidationResultClass.ValidateResultEnum.Valid)
                {
                    return new(
                        new(GetQueryFormCompletion.PayloadData.ErrorCodeEnum.FormNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified field {field.Key} is invalid. {result.Reason}");
                }
            }

            GetQueryFormCompletion.PayloadData payload = new(
                Form: new(
                    Unit: new(
                        Base: Base switch
                        {
                            BaseEnum.INCH => XFS4IoT.Printer.UnitClass.BaseEnum.Inch,
                            BaseEnum.MM => XFS4IoT.Printer.UnitClass.BaseEnum.Mm,
                            _ => XFS4IoT.Printer.UnitClass.BaseEnum.RowColumn,
                        },
                        X: UnitX,
                        Y: UnitY),
                    Size: new(
                        Width: Width,
                        Height: Height),
                    Alignment: new(
                        Relative: Alignment switch
                        {
                            AlignmentEnum.BOTTOMLEFT => GetQueryFormCompletion.PayloadData.FormClass.AlignmentClass.RelativeEnum.BottomLeft,
                            AlignmentEnum.BOTTOMRIGHT => GetQueryFormCompletion.PayloadData.FormClass.AlignmentClass.RelativeEnum.BottomRight,
                            AlignmentEnum.TOPLEFT => GetQueryFormCompletion.PayloadData.FormClass.AlignmentClass.RelativeEnum.TopLeft,
                            _ => GetQueryFormCompletion.PayloadData.FormClass.AlignmentClass.RelativeEnum.TopRight,
                        },
                        X: XOffset,
                        Y: YOffset),
                    Orientation: Orientation switch
                    {
                        FormOrientationEnum.LANDSCAPE => GetQueryFormCompletion.PayloadData.FormClass.OrientationEnum.Landscape,
                        _ => GetQueryFormCompletion.PayloadData.FormClass.OrientationEnum.Portrait,
                    },
                    Skew: Skew >= 0 ? Skew : null,
                    Version: (VersionMinor is null && VersionMajor is null && string.IsNullOrEmpty(Date) && string.IsNullOrEmpty(Author)) ? null 
                        : new(
                            Version: VersionMajor is null || VersionMinor is null ? null : $"{VersionMajor}.{VersionMinor}",
                            Date: string.IsNullOrEmpty(Date) ? null : Date,
                            Author: string.IsNullOrEmpty(Author) ? null : Author),
                    Copyright: string.IsNullOrEmpty(Copyright) ? null : Copyright,
                    Title: string.IsNullOrEmpty(Title) ? null : Title,
                    Comment: string.IsNullOrEmpty(Comment) ? null : Comment,
                    UserPrompt: string.IsNullOrEmpty(Prompt) ? null : Prompt,
                    Fields: fields)
                );

            return new CommandResult<GetQueryFormCompletion.PayloadData>(payload, MessageHeader.CompletionCodeEnum.Success);
        }

        /// <summary>
        /// Return payload structure for QueryField command
        /// </summary>
        public GetQueryFieldCompletion.PayloadData.FieldsClass QueryField(string FieldName)
        {
            Fields.ContainsKey(FieldName).IsTrue($"Specified field doesn't exist. {FieldName}");

            GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass style = null;
            if (Fields[FieldName].Style > 0)
            {
                GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.UnderlineEnum? underline = null;
                if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.UNDER))
                {
                    underline = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.UnderlineEnum.Single;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.DOUBLEUNDER))
                {
                    underline = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.UnderlineEnum.Double;
                }

                GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.WidthEnum? width = null;
                if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.DOUBLE))
                {
                    width = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.WidthEnum.Double;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.TRIPLE))
                {
                    width = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.WidthEnum.Triple;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.QUADRUPLE))
                {
                    width = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.WidthEnum.Quadruple;
                }

                GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.StrikeEnum? strike = null;
                if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.DOUBLESTRIKE))
                {
                    strike = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.StrikeEnum.Double;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.STRIKETHROUGH))
                {
                    strike = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.StrikeEnum.Single;
                }

                GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.RotateEnum? rotate = null;
                if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.ROTATE90))
                {
                    rotate = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.RotateEnum.Ninety;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.ROTATE270))
                {
                    rotate = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.RotateEnum.TwoSeventy;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.UPSIDEDOWN))
                {
                    rotate = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.RotateEnum.UpsideDown;
                }

                GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.CharacterSpacingEnum? characterSpacing = null;
                if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.CONDENSED))
                {
                    characterSpacing = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.CharacterSpacingEnum.Condensed;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.PROPORTIONAL))
                {
                    characterSpacing = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.CharacterSpacingEnum.Proportional;
                }

                GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.LineSpacingEnum? lineSpacing = null;
                if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.DOUBLEHIGH))
                {
                    lineSpacing = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.LineSpacingEnum.Double;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.TRIPLEHIGH))
                {
                    lineSpacing = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.LineSpacingEnum.Triple;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.QUADRUPLEHIGH))
                {
                    lineSpacing = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.LineSpacingEnum.Quadruple;
                }

                GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.ScriptEnum? script = null;
                if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.SUPERSCRIPT))
                {
                    script = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.ScriptEnum.Superscript;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.SUBSCRIPT))
                {
                    script = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.ScriptEnum.Subscript;
                }

                GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.QualityEnum? quality = null;
                if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.LETTERQUALITY))
                {
                    quality = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.QualityEnum.Letter;
                }
                else if (Fields[FieldName].Style.HasFlag(FieldStyleEnum.NEARLETTERQUALITY))
                {
                    quality = GetQueryFieldCompletion.PayloadData.FieldsClass.StyleClass.QualityEnum.NearLetter;
                }

                style = new(
                Bold: Fields[FieldName].Style.HasFlag(FieldStyleEnum.BOLD) ? true : null,
                Italic: Fields[FieldName].Style.HasFlag(FieldStyleEnum.ITALIC) ? true : null,
                Underline: underline,
                Width: width,
                Strike: strike,
                Rotate: rotate,
                CharacterSpacing: characterSpacing,
                LineSpacing: lineSpacing,
                Script: script,
                Overscore: Fields[FieldName].Style.HasFlag(FieldStyleEnum.OVERSCORE) ? true : null,
                Quality: quality,
                Opaque: Fields[FieldName].Style.HasFlag(FieldStyleEnum.OPAQUE) ? true : null);
            }

            return new GetQueryFieldCompletion.PayloadData.FieldsClass(
                    Position: new(
                        X: Fields[FieldName].X,
                        Y: Fields[FieldName].Y),
                    Follows: string.IsNullOrEmpty(Fields[FieldName].Follows) ? null : Fields[FieldName].Follows,
                    Side: Fields[FieldName].Side switch
                    {
                        FieldSideEnum.FRONT => GetQueryFieldCompletion.PayloadData.FieldsClass.SideEnum.Front,
                        FieldSideEnum.BACK => GetQueryFieldCompletion.PayloadData.FieldsClass.SideEnum.Back,
                        _ => null,
                    },
                    Size: new(
                        Width: Fields[FieldName].Width,
                        Height: Fields[FieldName].Height),
                    Index: Fields[FieldName].Repeat > 0 ? new(
                        RepeatCount: Fields[FieldName].Repeat,
                        X: Fields[FieldName].XOffset,
                        Y: Fields[FieldName].YOffset) : null,
                    FieldType: Fields[FieldName].Type switch
                    {
                        FieldTypeEnum.BARCODE => GetQueryFieldCompletion.PayloadData.FieldsClass.FieldTypeEnum.Barcode,
                        FieldTypeEnum.GRAPHIC => GetQueryFieldCompletion.PayloadData.FieldsClass.FieldTypeEnum.Graphic,
                        FieldTypeEnum.MICR => GetQueryFieldCompletion.PayloadData.FieldsClass.FieldTypeEnum.Micr,
                        FieldTypeEnum.MSF => GetQueryFieldCompletion.PayloadData.FieldsClass.FieldTypeEnum.Msf,
                        FieldTypeEnum.OCR => GetQueryFieldCompletion.PayloadData.FieldsClass.FieldTypeEnum.Ocr,
                        FieldTypeEnum.PAGEMARK => GetQueryFieldCompletion.PayloadData.FieldsClass.FieldTypeEnum.Pagemark,
                        FieldTypeEnum.TEXT => GetQueryFieldCompletion.PayloadData.FieldsClass.FieldTypeEnum.Text,
                        _ => null,
                    },
                    Scaling: Fields[FieldName].Scaling switch
                    {
                        FieldScalingEnum.ASIS => GetQueryFieldCompletion.PayloadData.FieldsClass.ScalingEnum.AsIs,
                        FieldScalingEnum.BESTFIT => GetQueryFieldCompletion.PayloadData.FieldsClass.ScalingEnum.BestFit,
                        FieldScalingEnum.MAINTAINASPECT => GetQueryFieldCompletion.PayloadData.FieldsClass.ScalingEnum.MaintainAspect,
                        _ => null,
                    },
                    Barcode: Fields[FieldName].Barcode switch
                    {
                        FieldBarcodeEnum.ABOVE => GetQueryFieldCompletion.PayloadData.FieldsClass.BarcodeEnum.Above,
                        FieldBarcodeEnum.BELOW => GetQueryFieldCompletion.PayloadData.FieldsClass.BarcodeEnum.Below,
                        FieldBarcodeEnum.BOTH => GetQueryFieldCompletion.PayloadData.FieldsClass.BarcodeEnum.Both,
                        _ => null,
                    },
                    Coercivity: null,
                    Class: Fields[FieldName].Class switch
                    {
                        FormField.ClassEnum.OPTIONAL => GetQueryFieldCompletion.PayloadData.FieldsClass.ClassEnum.Optional,
                        FormField.ClassEnum.REQUIRED => GetQueryFieldCompletion.PayloadData.FieldsClass.ClassEnum.Required,
                        FormField.ClassEnum.STATIC => GetQueryFieldCompletion.PayloadData.FieldsClass.ClassEnum.Static,
                        _ => null,
                    },
                    Access: Fields[FieldName].Access switch
                    {
                        FieldAccessEnum.READ => GetQueryFieldCompletion.PayloadData.FieldsClass.AccessEnum.Read,
                        FieldAccessEnum.WRITE => GetQueryFieldCompletion.PayloadData.FieldsClass.AccessEnum.Write,
                        FieldAccessEnum.READWRITE => GetQueryFieldCompletion.PayloadData.FieldsClass.AccessEnum.ReadWrite,
                        _ => null,
                    },
                    Overflow: Fields[FieldName].Overflow switch
                    {
                        FormField.OverflowEnum.BESTFIT => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.BestFit,
                        FormField.OverflowEnum.OVERWRITE => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.Overwrite,
                        FormField.OverflowEnum.TERMINATE => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.Terminate,
                        FormField.OverflowEnum.TRUNCATE => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.Truncate,
                        _ => null,
                    },
                    Style: style,
                    Case: Fields[FieldName].Case switch
                    {
                        FormField.CaseEnum.LOWER => GetQueryFieldCompletion.PayloadData.FieldsClass.CaseEnum.Lower,
                        FormField.CaseEnum.UPPER => GetQueryFieldCompletion.PayloadData.FieldsClass.CaseEnum.Upper,
                        _ => null,
                    },
                    Horizontal: Fields[FieldName].Horizontal switch
                    {
                        FormField.HorizontalEnum.CENTER => GetQueryFieldCompletion.PayloadData.FieldsClass.HorizontalEnum.Center,
                        FormField.HorizontalEnum.JUSTIFY => GetQueryFieldCompletion.PayloadData.FieldsClass.HorizontalEnum.Justify,
                        FormField.HorizontalEnum.LEFT => GetQueryFieldCompletion.PayloadData.FieldsClass.HorizontalEnum.Left,
                        FormField.HorizontalEnum.RIGHT => GetQueryFieldCompletion.PayloadData.FieldsClass.HorizontalEnum.Right,
                        _ => null,
                    },
                    Vertical: Fields[FieldName].Vertical switch
                    {
                        FormField.VerticalEnum.BOTTOM => GetQueryFieldCompletion.PayloadData.FieldsClass.VerticalEnum.Bottom,
                        FormField.VerticalEnum.CENTER => GetQueryFieldCompletion.PayloadData.FieldsClass.VerticalEnum.Center,
                        FormField.VerticalEnum.TOP => GetQueryFieldCompletion.PayloadData.FieldsClass.VerticalEnum.Top,
                        _ => null,
                    },
                    Color: Fields[FieldName].Color > 0 ? Fields[FieldName].Color.ToString().ToLower() : null,
                    Font: string.IsNullOrEmpty(Fields[FieldName].Font) ? null : new GetQueryFieldCompletion.PayloadData.FieldsClass.FontClass(
                        Name: Fields[FieldName].Font,
                        PointSize: Fields[FieldName].PointSize > 0 ? Fields[FieldName].PointSize : null,
                        Cpi: Fields[FieldName].CPI > 0 ? Fields[FieldName].CPI : null,
                        Lpi: Fields[FieldName].LPI > 0 ? Fields[FieldName].LPI : null),
                    Format: string.IsNullOrEmpty(Fields[FieldName].Format) ? null : Fields[FieldName].Format,
                    InitialValue: string.IsNullOrEmpty(Fields[FieldName].InitialValue) ? null : Fields[FieldName].InitialValue
                );
        }

        /// <summary>
        /// Validate form against the printer device supports
        /// </summary>
        public ValidationResultClass ValidateForm(IPrinterDevice Device)
        {
            FormRules rules = Device.FormRules;

            if (!rules.ValidOrientation.HasFlag(Orientation))
            { 
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"Orientation is not valid for the device. {Orientation}");
            }

            if (rules.RowColumnOnly &&
                Base != Form.BaseEnum.ROWCOLUMN)
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"UNIT is not valid for this printer - only ROWCOLUMN supported. {Base}, {Name}");
            }

            return new(ValidationResultClass.ValidateResultEnum.Valid);
        }

        /// <summary>
        /// Validate field against the printer device supports
        /// </summary>
        public ValidationResultClass ValidateField(string FieldName, IPrinterDevice Device)
        {
            FormRules rules = Device.FormRules;

            if (!Fields.ContainsKey(FieldName))
            {
                return new(ValidationResultClass.ValidateResultEnum.Missing,
                           $"Specified field doesn't exist. {FieldName}");
            }

            // Check the field is entirely inside the form
            // Do this using printer units not dots: otherwise rounding may cause
            // us to reject valid forms.
            int max_x, max_y;

            if (Fields[FieldName].Repeat == 0)
            {
                max_x = Fields[FieldName].X + Fields[FieldName].Width;
                max_y = Fields[FieldName].Y + Fields[FieldName].Height;
            }
            else
            {
                max_x = Fields[FieldName].X + (Fields[FieldName].Repeat - 1) * Fields[FieldName].XOffset +
                        Fields[FieldName].Width;
                max_y = Fields[FieldName].Y + (Fields[FieldName].Repeat - 1) * Fields[FieldName].YOffset +
                        Fields[FieldName].Height;
            }

            if (max_x > Width || max_y > Height)
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"Extent of field is larger than form.");
            }

            // Check SIDE
            if (!rules.ValidSide.HasFlag(Fields[FieldName].Side))
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"SIDE is not valid.");
            }

            // Check TYPE
            if (!rules.ValidType.HasFlag(Fields[FieldName].Type))
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"TYPE is not valid.");
            }

            // Check SCALING for GRAPHIC fields
            if (Fields[FieldName].Type == FieldTypeEnum.GRAPHIC)
            {
                if (!rules.ValidScaling.HasFlag(Fields[FieldName].Scaling))
                {
                    return new(ValidationResultClass.ValidateResultEnum.Invalid,
                               $"SCALING is not valid.");
                }

                if (!string.IsNullOrEmpty(Fields[FieldName].Format))
                {
                    if (Fields[FieldName].Format != "JPG" &&
                        Fields[FieldName].Format != "BMP")
                    {
                        return new(ValidationResultClass.ValidateResultEnum.Invalid,
                                   $"FORMAT is not valid for graphic field. {Fields[FieldName].Format}");
                    }
                }
                else
                {
                    Logger.Log(Constants.Framework, $"No FORMAT field defined, the device class could sniff image format and decide how to deal with it.");
                }
            }

            // Check BARCODE for BARCODE fields
            if (Fields[FieldName].Type == FieldTypeEnum.BARCODE)
            {
                if (!rules.ValidBarcode.HasFlag(Fields[FieldName].Barcode))
                {
                    return new(ValidationResultClass.ValidateResultEnum.Invalid,
                               $"BARCODE is not valid.");
                }
            }

            // Check ACCESS
            if (!rules.ValidAccess.HasFlag(Fields[FieldName].Access))
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"SIDE is not valid.");
            }

            // Check OVERWRITE
            if (Fields[FieldName].Overflow == FormField.OverflowEnum.OVERWRITE)
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"OVERFLOW is not supported.");
            }

            // Check STYLE
            if (!rules.ValidStyle.HasFlag(Fields[FieldName].Style))
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"STYLE is not valid.");
            }

            // Check COLOR
            if (!rules.ValidColor.HasFlag(Fields[FieldName].Color))
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"COLOR is not valid.");
            }

            // Check FONT

            if (Fields[FieldName].Type == FieldTypeEnum.TEXT)
            {
                if (rules.ValidFonts != "ALL")
                {
                    if (!rules.ValidFonts.Contains(Fields[FieldName].Font, StringComparison.OrdinalIgnoreCase))
                    {
                        return new(ValidationResultClass.ValidateResultEnum.Invalid,
                                   $"FONT is not valid.");
                    }
                }
            }

            // Check point size
            if (Fields[FieldName].PointSize != -1 &&
                (Fields[FieldName].PointSize > rules.MaxPointSize ||
                 Fields[FieldName].PointSize < rules.MinPointSize))
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"POINTSIZE is not valid.");
            }

            // Check CPI
            if (Fields[FieldName].CPI != -1 &&
                (Fields[FieldName].CPI > rules.MaxCPI ||
                 Fields[FieldName].CPI < rules.MinCPI))
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"CPI is not valid.");
            }

            // Check LPI
            if (Fields[FieldName].LPI != -1 &&
                (Fields[FieldName].LPI > rules.MaxLPI ||
                 Fields[FieldName].LPI < rules.MinLPI))
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"LPI is not valid.");
            }

            if (Fields[FieldName].Class == FormField.ClassEnum.REQUIRED &&
                !string.IsNullOrEmpty(Fields[FieldName].InitialValue))
            {
                Logger.Warning(Constants.Framework, $"INITIALVALUE for REQUIRED field ignored. {Fields[FieldName].Name}");
            }

            if (Fields[FieldName].Class == FormField.ClassEnum.STATIC &&
                string.IsNullOrEmpty(Fields[FieldName].InitialValue))
            {
                Logger.Warning(Constants.Framework, $"INITIALVALUE for STATIC field. {Fields[FieldName].Name}");
            }

            return new(ValidationResultClass.ValidateResultEnum.Valid);
        }

        /// <summary>
        /// Name of the form
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Specifies the base unit of measurement of the form
        /// </summary>
        public BaseEnum Base { get; init; }

        /// <summary>
        /// Size of the form
        /// </summary>
        public int UnitX { get; init; }
        public int UnitY { get; init; }

        /// <summary>
        /// Size in Units
        /// and in printer dots
        /// </summary>
        public int Width { get; init; }
        public int Height { get; init; }
        public int DotWidth { get; init; }
        public int DotHeight { get; init; }

        /// <summary>
        /// Offset from top-left of media in Units
        /// and in printer dots
        /// </summary>
        public int XOffset { get; init; }
        public int YOffset { get; init; }
        public int DotXOffset { get; init; }
        public int DotYOffset { get; init; }

        public int? VersionMajor { get; init; }
        public int? VersionMinor { get; init; }
        public string Date { get; init; }
        public string Author { get; init; }

        public string Copyright { get; init; }
        public string Title { get; init; }
        public string Comment { get; init; }
        public string Prompt { get; init; }

        /// <summary>
        /// Maximum skew factor in degrees 
        /// </summary>
        public int Skew { get; init; }

        public AlignmentEnum Alignment { get; init; }

        public FormOrientationEnum Orientation { get; init; }

        public Dictionary<string, FormField> Fields { get; init; }

        /// <summary>
        /// X/YConvertToDots
        /// Convert an X and Y measurement in Form units to printer dots
        /// </summary>
        public int XConvertToDots(int X)
        {
            int e = X * ColEnum + ColDenom - 1;
            return e / ColDenom;
        }
        public int YConvertToDots(int Y)
        {
            int e = Y * RowEnum + RowDenom - 1;
            return e / RowDenom;
        }

        /// <summary>
        /// Conversion factors for converting to dots
        /// </summary>
        private int RowEnum { get; init; }
        private int ColEnum { get; init; }
        private int RowDenom { get; init; }
        private int ColDenom { get; init; }

        private ILogger Logger { get; }
    }

    /// <summary>
    /// This class represents one printer field definition.
    /// </summary>
    [Serializable]
    public sealed record FormField
    {
        public enum ClassEnum
        {
            OPTIONAL,
            STATIC,
            REQUIRED,
        }

        public enum OverflowEnum
        {
            TERMINATE,
            TRUNCATE,
            BESTFIT,
            OVERWRITE,
            WORDWRAP,
        }

        public enum CaseEnum
        {
            NOCHANGE,
            UPPER,
            LOWER
        }

        public enum HorizontalEnum
        {
            LEFT,
            RIGHT,
            CENTER,
            JUSTIFY,
        }

        public enum VerticalEnum
        {
            BOTTOM,
            CENTER,
            TOP,
        }

        public FormField(string Name,
                         int X,
                         int Y,
                         int Width,
                         int Height,
                         int DotX,
                         int DotY,
                         int DotWidth,
                         int DotHeight,
                         string Follows,
                         int Repeat,
                         int XOffset,
                         int YOffset,
                         int DotXOffset,
                         int DotYOffset,
                         string Font,
                         int PointSize,
                         int CPI,
                         int LPI,
                         string Format,
                         string InitialValue,
                         FieldSideEnum Side,
                         FieldTypeEnum Type,
                         ClassEnum Class,
                         FieldAccessEnum Access,
                         OverflowEnum Overflow,
                         FieldStyleEnum Style,
                         CaseEnum Case,
                         HorizontalEnum Horizontal,
                         VerticalEnum Vertical,
                         FieldColorEnum Color,
                         FieldScalingEnum Scaling,
                         FieldBarcodeEnum Barcode)
        {
            this.Name = Name;
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.DotX = DotX;
            this.DotY = DotY;
            this.DotWidth = DotWidth;
            this.DotHeight = DotHeight;
            this.Follows = Follows;
            this.Repeat = Repeat;
            this.XOffset = XOffset;
            this.YOffset = YOffset;
            this.DotXOffset = DotXOffset;
            this.DotYOffset = DotYOffset;
            this.Font = Font;
            this.PointSize = PointSize;
            this.CPI = CPI;
            this.LPI = LPI;
            this.Format = Format;
            this.InitialValue = InitialValue;
            this.Side = Side;
            this.Type = Type;
            this.Class = Class;
            this.Access = Access;
            this.Overflow = Overflow;
            this.Style = Style;
            this.Case = Case;
            this.Horizontal = Horizontal;
            this.Vertical = Vertical;
            this.Color = Color;
            this.Scaling = Scaling;
            this.Barcode = Barcode;
        }

        /// <summary>
        /// Name of field
        /// </summary>
        public string Name { get; init; }
        /// <summary>
        /// X position
        /// </summary>
        public int X { get; init; }
        /// <summary>
        /// Y position
        /// </summary>
        public int Y { get; init; }
        /// <summary>
        /// Width of field
        /// </summary>
        public int Width { get; init; }
        /// <summary>
        /// height of field
        /// </summary>
        public int Height { get; init; }
        /// <summary>
        /// X position in dots
        /// </summary>
        public int DotX { get; init; }
        /// <summary>
        /// Y position in dots
        /// </summary>
        public int DotY { get; init; }
        /// <summary>
        /// Width in dots
        /// </summary>
        public int DotWidth { get; init; }
        /// <summary>
        /// Height in dots
        /// </summary>
        public int DotHeight { get; init; }
        /// <summary>
        /// Name of field this one follows
        /// </summary>
        public string Follows { get; init; }
        /// <summary>
        /// Number of occurrences. Zero if not an index field.
        /// </summary>
        public int Repeat { get; init; }
        /// <summary>
        /// Offset to next occurrence for X(Index fields only).
        /// </summary>
        public int XOffset { get; init; }
        /// <summary>
        /// Offset to next occurrence for Y(Index fields only).
        /// </summary>
        public int YOffset { get; init; }
        /// <summary>
        /// Offset to next occurrence for X in dots
        /// </summary>
        public int DotXOffset { get; init; }
        /// <summary>
        /// Offset to next occurrence for Y in dots
        /// </summary>
        public int DotYOffset { get; init; }
        /// <summary>
        /// Font name
        /// </summary>
        public string Font { get; init; }
        /// <summary>
        /// Point size
        /// </summary>
        public int PointSize { get; init; }
        /// <summary>
        /// Chars per inch
        /// </summary>
        public int CPI { get; init; }
        /// <summary>
        /// Lines per inch
        /// </summary>
        public int LPI { get; init; }
        /// <summary>
        /// Format of the field
        /// </summary>
        public string Format { get; init; }
        /// <summary>
        /// Initialvalue of field
        /// </summary>
        public string InitialValue { get; init; }
        public FieldSideEnum Side { get; init; }
        public FieldTypeEnum Type { get; init; }
        public ClassEnum Class { get; init; }
        public FieldAccessEnum Access { get; init; }
        public OverflowEnum Overflow { get; init; }
        public FieldStyleEnum Style { get; init; }
        public CaseEnum Case { get; init; }
        public HorizontalEnum Horizontal { get; init; }
        public VerticalEnum Vertical { get; init; }
        public FieldColorEnum Color { get; init; }
        public FieldScalingEnum Scaling { get; init; }
        public FieldBarcodeEnum Barcode { get; init; }
    }
}
