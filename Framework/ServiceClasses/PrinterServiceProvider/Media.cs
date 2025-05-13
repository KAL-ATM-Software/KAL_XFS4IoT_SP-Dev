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
    // This class represents one printer media definition.
    [Serializable]
    public sealed class Media
    {
        /// <summary>
        /// the base unit of measurement of the media
        /// </summary>
        public enum TypeEnum
        {
            GENERIC,
            MULTIPART,
            PASSBOOK,
        }

        [Flags]
        public enum SourceEnum
        {
            ANY,
            UPPER,
            LOWER,
            EXTERNAL,
            AUX,
            AUX2,
            PARK,
            CUSTOM,
        }

        public enum BaseEnum
        {
            MM,
            INCH,
            ROWCOLUMN,
        }

        public enum FoldEnum
        {
            NONE,
            HORIZONTAL,
            VERTICAL,
        }

        // Constructor
        // Used by form designer for creating new forms
        public Media(ILogger Logger,
                     IPrinterDevice Device,
                     string Name,
                     TypeEnum Type,
                     SourceEnum Source,
                     BaseEnum Base,
                     int UnitX,
                     int UnitY,
                     int Width,
                     int Height,
                     int PrintAreaX,
                     int PrintAreaY,
                     int PrintAreaWidth,
                     int PrintAreaHeight,
                     int RestrictedAreaX,
                     int RestrictedAreaY,
                     int RestrictedAreaWidth,
                     int RestrictedAreaHeight,
                     FoldEnum Fold,
                     int Staggering,
                     int Pages,
                     int Lines,
                     string CustomSource = null)
        {
            this.Logger = Logger;
            this.Name = Name;
            this.Type = Type;
            this.Source = Source;
            this.Base = Base;
            this.UnitX = UnitX;
            this.UnitY = UnitY;
            this.Width = Width;
            this.Height = Height;
            this.PrintAreaX = PrintAreaX;
            this.PrintAreaY = PrintAreaY;
            this.PrintAreaWidth = PrintAreaWidth;
            this.PrintAreaHeight = PrintAreaHeight;
            this.RestrictedAreaX = RestrictedAreaX;
            this.RestrictedAreaY = RestrictedAreaY;
            this.RestrictedAreaWidth = RestrictedAreaWidth;
            this.RestrictedAreaHeight = RestrictedAreaHeight;
            this.Fold = Fold;
            this.Staggering = Staggering;
            this.Pages = Pages;
            this.Lines = Lines;
            this.CustomSource = CustomSource;

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

            // So far Denoms are for 1 MM/Inch/RowCol.
            // Convert for form unit
            ColDenom *= UnitX;
            RowDenom *= UnitY;

            Contracts.Assert(RowDenom > 0 && ColDenom > 0, $"Form contains an illegal UNIT specification.  Base unit fractions must be greater than zero. {Name}");

            // Now convert all measurements
            // Round all of them up to avoid overflows due to rounding errors.
            DotWidth = XConvertToDots(Width);
            DotHeight = YConvertToDots(Height);
            DotPrintAreaX = XConvertToDots(PrintAreaX);
            DotPrintAreaY = YConvertToDots(PrintAreaY);
            DotPrintAreaWidth = XConvertToDots(PrintAreaWidth);
            DotPrintAreaHeight = YConvertToDots(PrintAreaHeight);
            DotRestrictedAreaX = XConvertToDots(RestrictedAreaX);
            DotRestrictedAreaY = YConvertToDots(RestrictedAreaY);
            DotRestrictedAreaWidth = XConvertToDots(RestrictedAreaWidth);
            DotRestrictedAreaHeight = YConvertToDots(RestrictedAreaHeight);
        }

        /// <summary>
        /// Return properties required for the GetQueryForm command
        /// </summary>
        /// <returns></returns>
        public CommandResult<GetQueryMediaCompletion.PayloadData> QueryMedia(IPrinterDevice Device)
        {
            var result = ValidateMedia(Device);
            if (result.Result != ValidationResultClass.ValidateResultEnum.Valid)
            {
                return new(
                    new(ErrorCode: GetQueryMediaCompletion.PayloadData.ErrorCodeEnum.MediaNotFound),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    $"Media {Name} is invalid. {result.Reason}");
            }

            string paperSource;
            if (Source != SourceEnum.CUSTOM)
            {
                paperSource = Source.ToString().ToLower();
            }
            else
            {
                paperSource = CustomSource;
            }

            GetQueryMediaCompletion.PayloadData payload = new(
                Media: new(
                    MediaType: Type switch
                    {
                        TypeEnum.GENERIC => GetQueryMediaCompletion.PayloadData.MediaClass.MediaTypeEnum.Generic,
                        TypeEnum.MULTIPART => GetQueryMediaCompletion.PayloadData.MediaClass.MediaTypeEnum.Multipart,
                        _ => GetQueryMediaCompletion.PayloadData.MediaClass.MediaTypeEnum.Passbook,
                    },
                    Source: paperSource,
                    Unit: new XFS4IoT.Printer.UnitClass(
                        Base: Base switch
                        {
                            BaseEnum.INCH => XFS4IoT.Printer.UnitClass.BaseEnum.Inch,
                            BaseEnum.MM => XFS4IoT.Printer.UnitClass.BaseEnum.Mm,
                            _ => XFS4IoT.Printer.UnitClass.BaseEnum.RowColumn,
                        },
                        X: UnitX,
                        Y: UnitY),
                    Size: new XFS4IoT.Printer.SizeClass(
                        Width: Width,
                        Height: Height),
                    PrintArea: (PrintAreaX < 0 || PrintAreaY < 0 || PrintAreaWidth < 0 || PrintAreaHeight < 0) ? 
                        null :
                        new XFS4IoT.Printer.AreaClass(
                            X: PrintAreaX,
                            Y: PrintAreaY,
                            Width: PrintAreaWidth,
                            Height: PrintAreaHeight),
                    Restricted: (RestrictedAreaX < 0 || RestrictedAreaY < 0 || RestrictedAreaWidth < 0 || RestrictedAreaHeight < 0) ? 
                        null :
                        new XFS4IoT.Printer.AreaClass(
                            X: RestrictedAreaX,
                            Y: RestrictedAreaY,
                            Width: RestrictedAreaWidth,
                            Height: RestrictedAreaHeight),
                    Fold: Fold switch
                    {
                        FoldEnum.HORIZONTAL => GetQueryMediaCompletion.PayloadData.MediaClass.FoldEnum.Horizontal,
                        FoldEnum.VERTICAL => GetQueryMediaCompletion.PayloadData.MediaClass.FoldEnum.Vertical,
                        _ => null,
                    },
                    Staggering: Staggering,
                    Page: Pages,
                    Lines: Lines)
                );

            return new(
                Payload: payload,
                CompletionCode: MessageHeader.CompletionCodeEnum.Success
                );
        }

        /// <summary>
        /// Validate media against the printer device supports
        /// </summary>
        public ValidationResultClass ValidateMedia(IPrinterDevice Device)
        {
            List<MediaSpec> mediaSpecs = Device.MediaSpecs;

            int i = 0;
            for (; i < mediaSpecs.Count; i++)
            {
                if (mediaSpecs[i].Width >= DotWidth &&
                    (mediaSpecs[i].Height == 0 ||
                     mediaSpecs[i].Height >= DotHeight))
                {
                    break;
                }
            }

            // Check width
            if (i == mediaSpecs.Count)
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"Size of media is greater than any media supported by the device.");
            }

            if (Fold != Media.FoldEnum.NONE)
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"FOLD not supported for this printer type.");
            }

            if (Staggering != 0)
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"STAGGERING not supported for this printer type.");
            }

            if (Pages != 0)
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"PAGE not supported for this printer type.");
            }

            if (Lines != 0)
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"LINES not supported for this printer type.");
            }

            if (PrintAreaX < 0 ||
                PrintAreaX + PrintAreaWidth > Width ||
                PrintAreaY < 0 ||
                PrintAreaY + PrintAreaHeight > Height)
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"PRINTAREA not within extent of media. PrintAreaX:{PrintAreaX}, PrintAreaY:{PrintAreaY}");
            }

            if (RestrictedAreaX < 0 ||
                RestrictedAreaX + RestrictedAreaWidth > Width ||
                RestrictedAreaY < 0 ||
                RestrictedAreaY + RestrictedAreaHeight > Height)
            {
                return new(ValidationResultClass.ValidateResultEnum.Invalid,
                           $"RESTRICTED area not within extent of media. RestrictedAreaX:{RestrictedAreaX}, RestrictedAreaY:{RestrictedAreaY}");
            }

            return new(ValidationResultClass.ValidateResultEnum.Valid);
        }

        /// <summary>
        /// Name of the media
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Specifies the type of media
        /// </summary>
        public TypeEnum Type { get; init; }

        /// <summary>
        /// Specifies the source of media
        /// </summary>
        public SourceEnum Source { get; init; }
        /// <summary>
        /// Specifies the base unit of measurement of the media
        /// </summary>
        public BaseEnum Base { get; init; }
        /// <summary>
        /// Size of the form
        /// </summary>
        public int UnitX { get; init; }
        public int UnitY { get; init; }

        /// <summary>
        /// vendor specific paper source
        /// </summary>
        public string CustomSource { get; init; }

        /// <summary>
        /// Size of media in Units
        /// and in printer dots
        /// </summary>
        public int Width { get; init; }
        public int Height { get; init; }
        public int DotWidth { get; init; }
        public int DotHeight { get; init; }

        /// <summary>
        /// Size of media area in Units
        /// and in printer dots
        /// </summary>
        public int PrintAreaX { get; init; }
        public int PrintAreaY { get; init; }
        public int PrintAreaWidth { get; init; }
        public int PrintAreaHeight { get; init; }

        public int DotPrintAreaX { get; init; }
        public int DotPrintAreaY { get; init; }
        public int DotPrintAreaWidth { get; init; }
        public int DotPrintAreaHeight { get; init; }

        /// <summary>
        /// Size of restricted media area in Units
        /// and in printer dots
        /// </summary>
        public int RestrictedAreaX { get; init; }
        public int RestrictedAreaY { get; init; }
        public int RestrictedAreaWidth { get; init; }
        public int RestrictedAreaHeight { get; init; }

        public int DotRestrictedAreaX { get; init; }
        public int DotRestrictedAreaY { get; init; }
        public int DotRestrictedAreaWidth { get; init; }
        public int DotRestrictedAreaHeight { get; init; }

        public FoldEnum Fold { get; init; }

        public int Staggering { get; init; }
        public int Pages { get; init; }
        public int Lines { get; init; }

        /// <summary>
        /// Conversion factors for converting to dots
        /// </summary>
        private int RowEnum { get; init; }
        private int ColEnum { get; init; }
        private int RowDenom { get; init; }
        private int ColDenom { get; init; }

        /// <summary>
        /// X/YConvertToDots
        /// Convert an X and Y measurement in Form units to printer dots
        /// </summary>
        private int XConvertToDots(int X)
        {
            int e = X * ColEnum + ColDenom - 1;
            return e / ColDenom;
        }
        private int YConvertToDots(int Y)
        {
            int e = Y * RowEnum + RowDenom - 1;
            return e / RowDenom;
        }

        private ILogger Logger { get; }
    }
}
