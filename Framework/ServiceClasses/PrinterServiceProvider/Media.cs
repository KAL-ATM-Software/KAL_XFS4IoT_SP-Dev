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
            ANY = 0,
            UPPER = 1 << 0,
            LOWER = 1 << 1,
            EXTERNAL = 1 << 2,
            AUX = 1 << 3,
            AUX2 = 1 << 4,
            PARK = 1 << 5,
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
                    SourceEnum Sources,
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
                    Dictionary<string, bool> CustomSources = null)
        {
            this.Logger = Logger;
            this.Name = Name;
            this.Type = Type;
            this.Sources = Sources;
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
            this.CustomSources = CustomSources;

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
        public GetQueryMediaCompletion.PayloadData QueryMedia()
        {
            XFS4IoT.Printer.PaperSourcesClass paperSource = null;

            if (Sources != SourceEnum.ANY)
            {
                paperSource = new(Sources.HasFlag(SourceEnum.UPPER),
                                  Sources.HasFlag(SourceEnum.LOWER),
                                  Sources.HasFlag(SourceEnum.EXTERNAL),
                                  Sources.HasFlag(SourceEnum.AUX),
                                  Sources.HasFlag(SourceEnum.AUX2),
                                  Sources.HasFlag(SourceEnum.PARK));

                if (CustomSources is not null)
                {
                    paperSource.ExtendedProperties = CustomSources;
                }
            }
            

            return new GetQueryMediaCompletion.PayloadData(
                    XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success,
                    ErrorDescription: null,
                    MediaType: Type switch
                    {
                        TypeEnum.GENERIC => GetQueryMediaCompletion.PayloadData.MediaTypeEnum.Generic,
                        TypeEnum.MULTIPART => GetQueryMediaCompletion.PayloadData.MediaTypeEnum.Multipart,
                        _ => GetQueryMediaCompletion.PayloadData.MediaTypeEnum.Passbook,
                    },
                    Base: Base switch
                    {
                        BaseEnum.INCH => GetQueryMediaCompletion.PayloadData.BaseEnum.Inch,
                        BaseEnum.MM => GetQueryMediaCompletion.PayloadData.BaseEnum.Mm,
                        _ => GetQueryMediaCompletion.PayloadData.BaseEnum.Rowcolumn,
                    },
                    UnitX: UnitX,
                    UnitY: UnitY,
                    SizeWidth: Width,
                    SizeHeight: Height,
                    PageCount: Pages,
                    LineCount: Lines,
                    PrintAreaX: PrintAreaX,
                    PrintAreaY: PrintAreaY,
                    PrintAreaWidth: PrintAreaWidth,
                    PrintAreaHeight: PrintAreaHeight,
                    RestrictedAreaX: RestrictedAreaX,
                    RestrictedAreaY: RestrictedAreaY,
                    RestrictedAreaWidth: RestrictedAreaWidth,
                    RestrictedAreaHeight: RestrictedAreaHeight,
                    Stagger: Staggering,
                    FoldType: Fold switch
                    {
                        FoldEnum.HORIZONTAL => GetQueryMediaCompletion.PayloadData.FoldTypeEnum.Horizontal,
                        FoldEnum.VERTICAL => GetQueryMediaCompletion.PayloadData.FoldTypeEnum.Vertical,
                        _ => GetQueryMediaCompletion.PayloadData.FoldTypeEnum.None,
                    },
                    PaperSources: paperSource
                );
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
        public SourceEnum Sources { get; init; }
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
        public Dictionary<string, bool> CustomSources { get; init; }

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
