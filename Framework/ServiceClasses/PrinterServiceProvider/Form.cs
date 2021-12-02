/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

namespace XFS4IoTFramework.Printer
{
    /// <summary>
    /// Orientation for a print job.  Text runs across the media for
    /// PORTRAIT prints and down the media for LANDSCAPE prints.
    /// </summary>
    [Flags]
    public enum FormOrientationEnum
    {
        PORTRAIT = 0x0001,
        LANDSCAPE = 0x0002,
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
    public enum FieldStyleEnum
    {
        NORMAL = 0x00000001,
        BOLD = 0x00000002,
        ITALIC = 0x00000004,
        UNDER = 0x00000008,
        DOUBLEUNDER = 0x00000010,
        DOUBLE = 0x00000020,
        TRIPLE = 0x00000040,
        QUADRUPLE = 0x00000080,
        STRIKETHROUGH = 0x00000100,
        ROTATE90 = 0x00000200,
        ROTATE270 = 0x00000400,
        UPSIDEDOWN = 0x00000800,
        PROPORTIONAL = 0x00001000,
        DOUBLEHIGH = 0x00002000,
        TRIPLEHIGH = 0x00004000,
        QUADRUPLEHIGH = 0x00008000,
        CONDENSED = 0x00010000,
        SUPERSCRIPT = 0x00020000,
        SUBSCRIPT = 0x00040000,
        OVERSCORE = 0x00080000,
        LETTERQUALITY = 0x00100000,
        NEARLETTERQUALITY = 0x00200000,
        DOUBLESTRIKE = 0x00400000,
        OPAQUE = 0x00800000,
    }

    /// <summary>
    /// Type of a print task
    /// </summary>
    [Flags]
    public enum FieldTypeEnum
    {
        TEXT = 0x0001,
        MICR = 0x0002,
        OCR = 0x0004,
        MSF = 0x0008,
        BARCODE = 0x0010,
        GRAPHIC = 0x0020,
        PAGEMARK = 0x0040,
    }

    /// <summary>
    /// Access type of a task: WRITE for tasks to print, READ for tasks to scan 
    /// </summary>
    [Flags]
    public enum FieldAccessEnum
    {
        WRITE = 0x0001,
        READ = 0x0002,
        READWRITE = 0x0004,
    }

    /// <summary>
    /// for graphic tasks indicates how they can be scaled
    /// </summary>
    [Flags]
    public enum FieldScalingEnum
    {
        BESTFIT = 0x0001,
        ASIS = 0x0002,
        MAINTAINASPECT = 0x0004
    }

    /// <summary>
    /// for barcode tasks, indicates where to print the human readable form
    /// </summary>
    [Flags]
    public enum FieldBarcodeEnum
    {
        NONE = 0x0001,
        ABOVE = 0x002,
        BELOW = 0x0004,
        BOTH = 0x0008,
    }

    /// <summary>
    /// color to use for printing
    /// </summary>
    [Flags]
    public enum FieldColorEnum
    {
        BLACK = 0x0001,
        WHITE = 0x0002,
        GRAY = 0x0004,
        RED = 0x0008,
        BLUE = 0x0010,
        GREEN = 0x0020,
        YELLOW = 0x0040,
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
                    int VersionMajor,
                    int VersionMinor,
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
            this.Fields = new();

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
            Fields.Add(Name, new(Name,
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
        public GetQueryFormCompletion.PayloadData QueryForm()
        {
            List<string> fields = new();
            fields.AddRange(from field in Fields
                            select field.Key);
            return new GetQueryFormCompletion.PayloadData(
                    XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success,
                    ErrorDescription: null,
                    FormName: Name,
                    Base: Base switch
                    {
                        BaseEnum.INCH => GetQueryFormCompletion.PayloadData.BaseEnum.Inch,
                        BaseEnum.MM => GetQueryFormCompletion.PayloadData.BaseEnum.Mm,
                        _ => GetQueryFormCompletion.PayloadData.BaseEnum.RowColumn,
                    },
                    UnitX: UnitX,
                    UnitY: UnitY,
                    Width: Width,
                    Height: Height,
                    Alignment: Alignment switch
                    { 
                        AlignmentEnum.BOTTOMLEFT => GetQueryFormCompletion.PayloadData.AlignmentEnum.BottomLeft,
                        AlignmentEnum.BOTTOMRIGHT => GetQueryFormCompletion.PayloadData.AlignmentEnum.BottomRight,
                        AlignmentEnum.TOPLEFT => GetQueryFormCompletion.PayloadData.AlignmentEnum.TopLeft,
                        _ => GetQueryFormCompletion.PayloadData.AlignmentEnum.TopRight,
                    },
                    Orientation: Orientation switch
                    { 
                        FormOrientationEnum.LANDSCAPE => GetQueryFormCompletion.PayloadData.OrientationEnum.Landscape,
                        _ => GetQueryFormCompletion.PayloadData.OrientationEnum.Portrait,
                    },
                    OffsetX: XOffset,
                    OffsetY: YOffset,
                    VersionMajor: VersionMajor,
                    VersionMinor: VersionMinor,
                    UserPrompt: Prompt,
                    Fields: fields
                );
        }

        /// <summary>
        /// Return payload structure for QueryField command
        /// </summary>
        public GetQueryFieldCompletion.PayloadData.FieldsClass QueryField(string Name)
        {
            if (!Fields.ContainsKey(Name))
            {
                Logger.Warning(Constants.Framework, $"Specified field doesn't exist. {Name}");
                return null;
            }
            return new GetQueryFieldCompletion.PayloadData.FieldsClass(
                Fields[Name].Repeat, 
                Fields[Name].Type switch
                { 
                    FieldTypeEnum.BARCODE => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Barcode,
                    FieldTypeEnum.GRAPHIC => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Graphic,
                    FieldTypeEnum.MICR => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Micr,
                    FieldTypeEnum.MSF => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Msf,
                    FieldTypeEnum.OCR => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Ocr,
                    FieldTypeEnum.PAGEMARK => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Pagemark,
                    _ => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Text,
                },
                Fields[Name].Class switch
                { 
                    FormField.ClassEnum.OPTIONAL => GetQueryFieldCompletion.PayloadData.FieldsClass.ClassEnum.Optional,
                    FormField.ClassEnum.REQUIRED => GetQueryFieldCompletion.PayloadData.FieldsClass.ClassEnum.Required,
                    _ => GetQueryFieldCompletion.PayloadData.FieldsClass.ClassEnum.Optional,
                },
                Fields[Name].Access switch
                {
                    FieldAccessEnum.READ => GetQueryFieldCompletion.PayloadData.FieldsClass.AccessEnum.Read,
                    FieldAccessEnum.WRITE => GetQueryFieldCompletion.PayloadData.FieldsClass.AccessEnum.Write,
                    _ => GetQueryFieldCompletion.PayloadData.FieldsClass.AccessEnum.ReadWrite,
                },
                Fields[Name].Overflow switch
                {
                    FormField.OverflowEnum.BESTFIT => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.BestFit,
                    FormField.OverflowEnum.OVERWRITE => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.Overwrite,
                    FormField.OverflowEnum.TERMINATE => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.Terminate,
                    FormField.OverflowEnum.TRUNCATE => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.Truncate,
                    _ => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.WordWrap,
                },
                Fields[Name].InitialValue,
                Fields[Name].Format);
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

        public int VersionMajor { get; init; }
        public int VersionMinor { get; init; }
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
