/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;

namespace XFS4IoTFramework.Printer
{
    /// <summary>
    /// The device specific class must define one or more KXMediaSpecs 
    /// indicating the dimensions of the media supported.  Media dimensions
    /// are given in printer dots.
    /// If the media is a roll of paper, the Height should be set to the maximum
    /// length of media that can be printed and ejected as one document.
    /// If there is no practical limit, set the Height to zero.
    /// </summary>
    public sealed class MediaSpec
    {
        public MediaSpec(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }
        /// <summary>
        /// Width of media in dots
        /// </summary>
        public int Width { get; init; }
        /// <summary>
        /// Height of media in dots (zero if no practical limit)
        /// </summary>
        public int Height { get; init; }
    }

    /// <summary>
    /// A FormRules is returned from the device specific class property FormRules. 
    /// The object represents the capabilities of the specific class in terms of what kinds of fields it can print.
    /// </summary>
    public sealed class FormRules
    {
        public FormRules()
        {
            RowColumnOnly = false;
            ValidOrientation = FormOrientationEnum.PORTRAIT | FormOrientationEnum.LANDSCAPE;
            MinSkew = 0;
            MaxSkew = 90;
            ValidSide = FieldSideEnum.FRONT;
            ValidType = FieldTypeEnum.TEXT | 
                        FieldTypeEnum.GRAPHIC;
            ValidScaling = FieldScalingEnum.BESTFIT | 
                           FieldScalingEnum.MAINTAINASPECT | 
                           FieldScalingEnum.ASIS;
            ValidAccess = FieldAccessEnum.WRITE | FieldAccessEnum.READWRITE;
            // All styles valid as a default
            ValidStyle = FieldStyleEnum.BOLD |
                         FieldStyleEnum.CONDENSED |
                         FieldStyleEnum.DOUBLEHIGH |
                         FieldStyleEnum.DOUBLESTRIKE |
                         FieldStyleEnum.DOUBLEUNDER |
                         FieldStyleEnum.DOUBLE |
                         FieldStyleEnum.ITALIC |
                         FieldStyleEnum.LETTERQUALITY |
                         FieldStyleEnum.NEARLETTERQUALITY |
                         FieldStyleEnum.NORMAL | 
                         FieldStyleEnum.OPAQUE |
                         FieldStyleEnum.OVERSCORE |
                         FieldStyleEnum.PROPORTIONAL |
                         FieldStyleEnum.QUADRUPLEHIGH |
                         FieldStyleEnum.QUADRUPLE |
                         FieldStyleEnum.ROTATE270 |
                         FieldStyleEnum.ROTATE90 |
                         FieldStyleEnum.STRIKETHROUGH |
                         FieldStyleEnum.SUBSCRIPT |
                         FieldStyleEnum.SUPERSCRIPT |
                         FieldStyleEnum.TRIPLEHIGH |
                         FieldStyleEnum.TRIPLE |
                         FieldStyleEnum.UNDER |
                         FieldStyleEnum.UPSIDEDOWN; 
            ValidBarcode = 0;
            ValidColor = FieldColorEnum.BLACK;
            ValidFonts = "ALL";
            MinPointSize = 1;
            MaxPointSize = 1000;
            MinCPI = 1;
            MaxCPI = 100;
            MinLPI = 1;
            MaxLPI = 100;
        }

        public FormRules(bool RowColumnOnly,
                         FormOrientationEnum ValidOrientation,
                         int MinSkew,
                         int MaxSkew,
                         FieldSideEnum ValidSide,
                         FieldTypeEnum ValidType,
                         FieldScalingEnum ValidScaling,
                         FieldBarcodeEnum ValidBarcode,
                         FieldAccessEnum ValidAccess,
                         FieldStyleEnum ValidStyle,
                         FieldColorEnum ValidColor,
                         string ValidFonts,
                         int MinPointSize,
                         int MaxPointSize,
                         int MinCPI,
                         int MaxCPI,
                         int MinLPI,
                         int MaxLPI)
        {
            this.RowColumnOnly = RowColumnOnly;
            this.ValidOrientation = ValidOrientation;
            this.MinSkew = MinSkew;
            this.MaxSkew = MaxSkew;
            this.ValidSide = ValidSide;
            this.ValidType = ValidType;
            this.ValidScaling = ValidScaling;
            this.ValidAccess = ValidAccess;
            this.ValidStyle = ValidStyle;
            this.ValidBarcode = ValidBarcode;
            this.ValidColor = ValidColor;
            this.ValidFonts = ValidFonts;
            this.MinPointSize = MinPointSize;
            this.MaxPointSize = MaxPointSize;
            this.MinCPI = MinCPI;
            this.MaxCPI = MaxCPI;
            this.MinLPI = MinLPI;
            this.MaxLPI = MaxLPI;
        }

        /// <summary>
        /// Flag true if printer supports row-column based printing only
        /// </summary>
        public bool RowColumnOnly { get; init; }
        /// <summary>
        /// Valid XFSFIELD ORIENTATIONSs
        /// </summary>
        public FormOrientationEnum ValidOrientation { get; init; }
        /// <summary>
        /// Min allowed skew
        /// </summary>
        public int MinSkew { get; init; }
        /// <summary>
        /// Max allowed skew
        /// </summary>
        public int MaxSkew { get; init; }
        /// <summary>
        /// Valid XFSFIELD SIDEs
        /// </summary>
        public FieldSideEnum ValidSide { get; init; }
        /// <summary>
        /// Valid XFSFIELD TYPEs
        /// </summary>
        public FieldTypeEnum ValidType { get; init; }
        /// <summary>
        /// Valid XFSFIELD SCALINGs
        /// </summary>
        public FieldScalingEnum ValidScaling { get; init; }
        /// <summary>
        /// Valid XFSFIELD BARCODEs
        /// </summary>
        public FieldBarcodeEnum ValidBarcode { get; init; }
        /// <summary>
        /// Valid XFSFIELD ACCESSes
        /// </summary>
        public FieldAccessEnum ValidAccess { get; init; }
        /// <summary>
        /// ll valid XFSFIELD STYLEs ORred together
        /// </summary>
        public FieldStyleEnum ValidStyle { get; init; }
        /// <summary>
        /// Valid XFSFIELD COLORs
        /// </summary>
        public FieldColorEnum ValidColor { get; init; }
        /// <summary>
        /// CSV list of valid fonts (=L"ALL" if don't care)
        /// </summary>
        public string ValidFonts { get; init; }
        /// <summary>
        /// Min POINTSIZE
        /// </summary>
        public int MinPointSize { get; init; }
        /// <summary>
        /// Max POINTSIZE
        /// </summary>
        public int MaxPointSize { get; init; }
        /// <summary>
        /// Min CPI
        /// </summary>
        public int MinCPI { get; init; }
        /// <summary>
        /// Max CPI
        /// </summary>
        public int MaxCPI { get; init; }
        /// <summary>
        /// Min LPI
        /// </summary>
        public int MinLPI { get; init; }
        /// <summary>
        /// Max LPI
        /// </summary>
        public int MaxLPI { get; init; }
    }
}
