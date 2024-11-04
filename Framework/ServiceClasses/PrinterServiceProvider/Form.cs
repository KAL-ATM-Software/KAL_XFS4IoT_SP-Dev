/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
        public CommandResult<GetQueryFormCompletion.PayloadData> QueryForm(IPrinterDevice Device)
        {
            var result = ValidateForm(Device);
            if (result.Result != ValidationResultClass.ValidateResultEnum.Valid)
            {
                return new(
                    new(ErrorCode: GetQueryFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Form {Name} is not supported by the device.");
            }

            List<string> fields = [];
            fields.AddRange(from field in Fields
                            select field.Key);

            // Check all fields
            foreach (var fieldName in fields)
            {
                var fieldResult = ValidateField(fieldName, Device);
                if (fieldResult.Result != ValidationResultClass.ValidateResultEnum.Valid)
                {
                    return new(
                        new(GetQueryFormCompletion.PayloadData.ErrorCodeEnum.FormInvalid),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified field {fieldName} is invalid. {result.Reason}");
                }
            }

            return new(
                new(
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
                    Fields: fields),
                    MessageHeader.CompletionCodeEnum.Success
                );
        }

        /// <summary>
        /// Return payload structure for QueryField command
        /// </summary>
        public GetQueryFieldCompletion.PayloadData.FieldsClass QueryField(string FieldName)
        {
            Fields.ContainsKey(FieldName).IsTrue($"Specified field doesn't exist. {FieldName}");

            return new GetQueryFieldCompletion.PayloadData.FieldsClass(
                Fields[FieldName].Repeat, 
                Fields[FieldName].Type switch
                { 
                    FieldTypeEnum.BARCODE => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Barcode,
                    FieldTypeEnum.GRAPHIC => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Graphic,
                    FieldTypeEnum.MICR => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Micr,
                    FieldTypeEnum.MSF => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Msf,
                    FieldTypeEnum.OCR => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Ocr,
                    FieldTypeEnum.PAGEMARK => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Pagemark,
                    _ => GetQueryFieldCompletion.PayloadData.FieldsClass.TypeEnum.Text,
                },
                Fields[FieldName].Class switch
                { 
                    FormField.ClassEnum.OPTIONAL => GetQueryFieldCompletion.PayloadData.FieldsClass.ClassEnum.Optional,
                    FormField.ClassEnum.REQUIRED => GetQueryFieldCompletion.PayloadData.FieldsClass.ClassEnum.Required,
                    _ => GetQueryFieldCompletion.PayloadData.FieldsClass.ClassEnum.Optional,
                },
                Fields[FieldName].Access switch
                {
                    FieldAccessEnum.READ => GetQueryFieldCompletion.PayloadData.FieldsClass.AccessEnum.Read,
                    FieldAccessEnum.WRITE => GetQueryFieldCompletion.PayloadData.FieldsClass.AccessEnum.Write,
                    _ => GetQueryFieldCompletion.PayloadData.FieldsClass.AccessEnum.ReadWrite,
                },
                Fields[FieldName].Overflow switch
                {
                    FormField.OverflowEnum.BESTFIT => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.BestFit,
                    FormField.OverflowEnum.OVERWRITE => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.Overwrite,
                    FormField.OverflowEnum.TERMINATE => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.Terminate,
                    FormField.OverflowEnum.TRUNCATE => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.Truncate,
                    _ => GetQueryFieldCompletion.PayloadData.FieldsClass.OverflowEnum.WordWrap,
                },
                Fields[FieldName].InitialValue,
                Fields[FieldName].Format);
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

            if (max_x > Fields[FieldName].Width || max_y > Fields[FieldName].Height)
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
