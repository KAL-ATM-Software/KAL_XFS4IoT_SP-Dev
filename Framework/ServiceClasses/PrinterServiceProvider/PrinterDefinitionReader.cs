/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Printer;
using XFS4IoTServer;

namespace XFS4IoTFramework.Printer
{
    /// <summary>
    /// Class to parse Printer specific Media and Form definitions
    /// </summary>
    internal class PrinterDefinitionReader
    {
        public PrinterDefinitionReader(ILogger logger, IPrinterDevice device)
        {
            this.Logger = logger.IsNotNull();
            this.Device = device.IsNotNull();
        }

        public ILogger Logger { get; init; }
        public IPrinterDevice Device { get; init; }

        /// <summary>
        /// Read a printer media from the XFSFormReader
        /// </summary>
        public Media ReadPrinterMedia(XFSFormReader reader)
        {
            reader.FormReaderAssertToken(XFSFormReader.TokenType.XFSMEDIA);

            string Name = reader.ReadString();
            Media.TypeEnum Type = Media.TypeEnum.GENERIC;
            Media.SourceEnum Source = Media.SourceEnum.ANY;
            Media.BaseEnum? Base = null;
            int? UnitX = null, UnitY = null;
            int? Width = null, Height = null;
            int PrintAreaX = 0, PrintAreaY = 0;
            int PrintAreaWidth = -1, PrintAreaHeight = -1;
            int RestrictedAreaX = 0, RestrictedAreaY = 0;
            int RestrictedAreaWidth = 0, RestrictedAreaHeight = 0;
            Media.FoldEnum Fold = Media.FoldEnum.NONE;
            int Staggering = 0, Pages = 0, Lines = 0;

            reader.FormReaderAssertToken(XFSFormReader.TokenType.BEGIN, true);

            while (reader.ReadNextToken() != XFSFormReader.TokenType.END)
            {
                reader.FormReaderAssertToken(XFSFormReader.TokenType.KEYWORD);
                switch (reader.CurrentKeyword())
                {
                    case XFSFormReader.FormKeyword.TYPE:
                        Type = reader.ReadEnum<Media.TypeEnum>();
                        break;

                    case XFSFormReader.FormKeyword.SOURCE:
                        Source = reader.ReadEnum<Media.SourceEnum>();
                        break;

                    case XFSFormReader.FormKeyword.UNIT:
                        Base = reader.ReadEnum<Media.BaseEnum>();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        UnitX = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        UnitY = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.SIZE:
                        Width = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        Height = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.PRINTAREA:
                        PrintAreaX = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        PrintAreaY = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        PrintAreaWidth = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        PrintAreaHeight = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.RESTRICTED:
                        RestrictedAreaX = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        RestrictedAreaY = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        RestrictedAreaWidth = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        RestrictedAreaHeight = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.FOLD:
                        Fold = reader.ReadEnum<Media.FoldEnum>();
                        break;

                    case XFSFormReader.FormKeyword.STAGGERING:
                        Staggering = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.PAGE:
                        Pages = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.LINES:
                        Lines = reader.ReadInt();
                        break;

                    default:
                        reader.FormReaderAssert(false, "Unexpected current token in ReadPrinterMedia.");
                        break;
                }
            }

            reader.FormReaderAssert(!(Base is null || UnitX is null || UnitY is null), "Expected required UNIT element in XFSMedia definition.");
            reader.FormReaderAssert(!(Width is null || Height is null), "Expected required SIZE element in XFSMedia definition.");
            
            if (PrintAreaWidth == -1) PrintAreaWidth = Width.IsNotNull();
            if (PrintAreaHeight == -1) PrintAreaHeight = Height.IsNotNull();

            return new Media(Logger, Device, Name, 
                Type, Source, Base.IsNotNull(), UnitX.IsNotNull(), UnitY.IsNotNull(), Width.IsNotNull(), Height.IsNotNull(), 
                PrintAreaX, PrintAreaY, PrintAreaWidth, PrintAreaHeight, RestrictedAreaX, RestrictedAreaY, RestrictedAreaWidth, RestrictedAreaHeight, 
                Fold, Staggering, Pages, Lines);
        }

        /// <summary>
        /// Read a printer Form from the XFSFormReader
        /// </summary>
        public Form ReadPrinterForm(XFSFormReader reader)
        {
            reader.FormReaderAssertToken(XFSFormReader.TokenType.XFSFORM);

            string Name = reader.ReadString();
            Form.BaseEnum? Base = null;
            int? UnitX = null, UnitY = null;
            int? Width = null, Height = null;
            int XOffset = 0, YOffset = 0;
            int VersionMajor = 0, VersionMinor = 0;
            string Date = null, Author = null;
            string Copyright = null, Title = null;
            string Comment = null, Prompt = null;
            int Skew = 0;
            Form.AlignmentEnum Alignment = Form.AlignmentEnum.TOPLEFT;
            FormOrientationEnum Orientation = FormOrientationEnum.PORTRAIT;

            List<FormField> fields = new(); 

            reader.FormReaderAssertToken(XFSFormReader.TokenType.BEGIN, true);

            while (reader.ReadNextToken() != XFSFormReader.TokenType.END)
            {
                reader.FormReaderAssert(reader.CurrentTokenType is XFSFormReader.TokenType.KEYWORD or XFSFormReader.TokenType.XFSFIELD, "Expected Keyword or XFSField within XFSForm definition.");
                if (reader.CurrentTokenType is XFSFormReader.TokenType.XFSFIELD)
                    fields.Add(ReadPrinterField(reader));
                else
                {
                    switch (reader.CurrentKeyword())
                    {
                        case XFSFormReader.FormKeyword.UNIT:
                            Base = reader.ReadEnum<Form.BaseEnum>();
                            reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                            UnitX = reader.ReadInt();
                            reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                            UnitY = reader.ReadInt();
                            break;

                        case XFSFormReader.FormKeyword.SIZE:
                            Width = reader.ReadInt();
                            reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                            Height = reader.ReadInt();
                            break;

                        case XFSFormReader.FormKeyword.ALIGNMENT:
                            Alignment = reader.ReadEnum<Form.AlignmentEnum>();
                            reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                            XOffset = reader.ReadInt();
                            reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                            YOffset = reader.ReadInt();
                            break;

                        case XFSFormReader.FormKeyword.VERSION:
                            VersionMajor = reader.ReadInt();
                            reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                            VersionMinor = reader.ReadInt();
                            reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                            Date = reader.ReadString();
                            reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                            Author = reader.ReadString();
                            break;

                        case XFSFormReader.FormKeyword.COPYRIGHT:
                            Copyright = reader.ReadString();
                            break;

                        case XFSFormReader.FormKeyword.TITLE:
                            Title = reader.ReadString();
                            break;

                        case XFSFormReader.FormKeyword.COMMENT:
                            Comment = reader.ReadString();
                            break;

                        case XFSFormReader.FormKeyword.USERPROMPT:
                            Prompt = reader.ReadString();
                            break;

                        case XFSFormReader.FormKeyword.SKEW:
                            Skew = reader.ReadInt();
                            break;

                        case XFSFormReader.FormKeyword.ORIENTATION:
                            Orientation = reader.ReadEnum<FormOrientationEnum>();
                            break;

                        default:
                            reader.FormReaderAssert(false, "Unexpected current token in ReadPrinterForm.");
                            break;
                    }
                }
            }

            reader.FormReaderAssert(!(Base is null || UnitX is null || UnitY is null), "Expected required UNIT element in XFSForm definition.");
            reader.FormReaderAssert(!(Width is null || Height is null), "Expected required SIZE element in XFSForm definition.");

            Form form = new(Logger, Device, Name, Base.IsNotNull(), UnitX.IsNotNull(), UnitY.IsNotNull(), Width.IsNotNull(), Height.IsNotNull(),
            XOffset, YOffset, VersionMajor, VersionMinor, Date, Author, Copyright, Title, Comment, Prompt, Skew, Alignment, Orientation);

            foreach (var field in fields)
                form.AddField(field.Name, field.X, field.Y, field.Width, field.Height, field.Follows, field.Repeat, field.XOffset, field.YOffset, field.Font, field.PointSize, field.CPI, field.LPI, field.Format, field.InitialValue, 
                    field.Side, field.Type, field.Class, field.Access, field.Overflow, field.Style, field.Case, field.Horizontal, field.Vertical, field.Color, field.Scaling, field.Barcode);

            return form;
        }

        /// <summary>
        /// Read a printer form field from the XFSFormReader
        /// </summary>
        public FormField ReadPrinterField(XFSFormReader reader)
        {
            reader.FormReaderAssertToken(XFSFormReader.TokenType.XFSFIELD);

            string Name = reader.ReadString();
            int? X = null, Y = null;
            int? Width = null, Height = null;
            string Follows = null;
            int Repeat = 0, XOffset = 0, YOffset = 0;
            string Font = null;
            int PointSize = -1;
            int CPI = -1;
            int LPI = -1;
            string Format = null;
            string InitialValue = null;
            FieldSideEnum Side = FieldSideEnum.FRONT;
            FieldTypeEnum Type = FieldTypeEnum.TEXT;
            FormField.ClassEnum Class = FormField.ClassEnum.OPTIONAL;
            FieldAccessEnum Access = FieldAccessEnum.WRITE;
            FormField.OverflowEnum Overflow = FormField.OverflowEnum.TERMINATE;
            FieldStyleEnum Style = FieldStyleEnum.NORMAL;
            FormField.CaseEnum Case = FormField.CaseEnum.NOCHANGE;
            FormField.HorizontalEnum Horizontal = FormField.HorizontalEnum.LEFT;
            FormField.VerticalEnum Vertical = FormField.VerticalEnum.BOTTOM;
            FieldColorEnum Color = FieldColorEnum.BLACK;
            FieldScalingEnum Scaling = FieldScalingEnum.BESTFIT;
            FieldBarcodeEnum Barcode = FieldBarcodeEnum.NONE;

            reader.FormReaderAssertToken(XFSFormReader.TokenType.BEGIN, true);

            while (reader.ReadNextToken() != XFSFormReader.TokenType.END)
            {
                reader.FormReaderAssertToken(XFSFormReader.TokenType.KEYWORD);

                switch (reader.CurrentKeyword())
                {
                    case XFSFormReader.FormKeyword.POSITION:
                        X = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        Y = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.SIZE:
                        Width = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        Height = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.FOLLOWS:
                        Follows = reader.ReadString();
                        break;

                    case XFSFormReader.FormKeyword.INDEX:
                        Repeat = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        XOffset = reader.ReadInt();
                        reader.FormReaderAssertToken(XFSFormReader.TokenType.COMMA, true);
                        YOffset = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.FONT:
                        Font = reader.ReadString();
                        break;

                    case XFSFormReader.FormKeyword.POINTSIZE:
                        PointSize = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.CPI:
                        CPI = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.LPI:
                        LPI = reader.ReadInt();
                        break;

                    case XFSFormReader.FormKeyword.FORMAT:
                        Format = reader.ReadString();
                        break;

                    case XFSFormReader.FormKeyword.INITIALVALUE:
                        InitialValue = reader.ReadString();
                        break;

                    case XFSFormReader.FormKeyword.SIDE:
                        Side = reader.ReadEnum<FieldSideEnum>();
                        break;

                    case XFSFormReader.FormKeyword.TYPE:
                        Type = reader.ReadEnum<FieldTypeEnum>();
                        break;

                    case XFSFormReader.FormKeyword.CLASS:
                        Class = reader.ReadEnum<FormField.ClassEnum>();
                        break;

                    case XFSFormReader.FormKeyword.ACCESS:
                        Access = reader.ReadEnum<FieldAccessEnum>();
                        break;

                    case XFSFormReader.FormKeyword.OVERFLOW:
                        Overflow = reader.ReadEnum<FormField.OverflowEnum>();
                        break;

                    case XFSFormReader.FormKeyword.STYLE:
                        Style = reader.ReadEnum<FieldStyleEnum>();
                        while (reader.PeekNextToken() is XFSFormReader.TokenType.OR)
                        {
                            reader.FormReaderAssertToken(XFSFormReader.TokenType.OR, true);
                            Style |= reader.ReadEnum<FieldStyleEnum>();
                        }
                        break;

                    case XFSFormReader.FormKeyword.CASE:
                        Case = reader.ReadEnum<FormField.CaseEnum>();
                        break;

                    case XFSFormReader.FormKeyword.HORIZONTAL:
                        Horizontal = reader.ReadEnum<FormField.HorizontalEnum>();
                        break;

                    case XFSFormReader.FormKeyword.VERTICAL:
                        Vertical = reader.ReadEnum<FormField.VerticalEnum>();
                        break;

                    case XFSFormReader.FormKeyword.COLOR:
                        Color = reader.ReadEnum<FieldColorEnum>();
                        break;

                    case XFSFormReader.FormKeyword.SCALING:
                        Scaling = reader.ReadEnum<FieldScalingEnum>();
                        break;

                    case XFSFormReader.FormKeyword.BARCODE:
                        Barcode = reader.ReadEnum<FieldBarcodeEnum>();
                        break;

                    default:
                        reader.FormReaderAssert(false, "Unexpected current token in ReadPrinterField.");
                        break;
                }
            }


            reader.FormReaderAssert(!(X is null || Y is null), "Expected required POSITION element in XFSField definition.");
            reader.FormReaderAssert(!(Width is null || Height is null), "Expected required SIZE element in XFSField definition.");

            return new FormField(Name, X.IsNotNull(), Y.IsNotNull(), Width.IsNotNull(), Height.IsNotNull(), -1, -1, -1, -1, Follows, Repeat, XOffset, YOffset, -1, -1, Font, PointSize, CPI, LPI, Format, InitialValue, Side, Type, Class, Access, Overflow, Style, Case, Horizontal, Vertical, Color, Scaling, Barcode);
        }

    }
}
