/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    public sealed class PrinterCapabilitiesClass
    {
        [Flags]
        public enum TypeEnum
        {
            Receipt = 1 << 0,
            Passbook = 1 << 1,
            Journal = 1 << 2,
            Document = 1 << 3,
            Scanner = 1 << 4,
        }

        [Flags]
        public enum ResolutionEnum
        {
            Low = 1 << 0,
            Medium = 1 << 1,
            High = 1 << 2,
            VeryHigh = 1 << 3,
        }

        [Flags]
        public enum ReadFormEnum
        {
            NotSupported = 0,
            OCR = 1 << 0,
            MICR = 1 << 1,
            MSF = 1 << 2,
            Barcode = 1 << 3,
            PageMark = 1 << 4,
            Image = 1 << 5,
            EmptyLine = 1 << 6,
        }

        [Flags]
        public enum WriteFormEnum
        {
            NotSupported = 0,
            Text = 1 << 0,
            Graphics = 1 << 1,
            OCR = 1 << 2,
            MICR = 1 << 3,
            MSF = 1 << 4,
            Barcode = 1 << 5,
            Stamp = 1 << 6,
        }

        [Flags]
        public enum ExtentEnum
        {
            NotSupported = 0,
            Horizontal = 1 << 0,
            Vertical = 1 << 1,
        }

        [Flags]
        public enum ControlEnum
        {
            NotSupported = 0,
            Eject = 1 << 0,
            Perforate = 1 << 1,
            Cut = 1 << 2,
            Skip = 1 << 3,
            Flush = 1 << 4,
            Retract = 1 << 5,
            Stack = 1 << 6,
            PartialCut = 1 << 7,
            Alarm = 1 << 8,
            PageForward = 1 << 9,
            PageBackward = 1 << 10,
            TurnMedia = 1 << 11,
            Stamp = 1 << 12,
            Park = 1 << 13,
            Expel = 1 << 14,
            EjectToTransport = 1 << 15,
            Rotate180 = 1 << 16,
            ClearBuffer = 1 << 17,
            Backward = 1 << 18,
            Forward = 1 << 19,
        }

        [Flags]
        public enum PaperSourceEnum
        {
            NotSupported = 0,
            Upper = 1 << 0,
            Lower = 1 << 1,
            External = 1 << 2,
            AUX = 1 << 3,
            AUX2 = 1 << 4,
            Park = 1 << 5,
        }

        [Flags]
        public enum ImageTypeEnum
        {
            NotSupported = 0,
            TIF = 1 << 0,
            WMF = 1 << 1,
            BMP = 1 << 2,
            JPG = 1 << 3,
        }

        [Flags]
        public enum FrontImageColorFormatEnum
        {
            NotSupported = 0,
            Binary = 1 << 0,
            GrayScale = 1 << 1,
            Full = 1 << 2,
        }

        [Flags]
        public enum BackImageColorFormatEnum
        {
            NotSupported = 0,
            Binary = 1 << 0,
            GrayScale = 1 << 1,
            Full = 1 << 2,
        }

        [Flags]
        public enum ImageSourceTypeEnum
        {
            NotSupported = 0,
            ImageFront = 1 << 0,
            ImageBack = 1 << 1,
        }

        [Flags]
        public enum CoercivityTypeEnum
        {
            NotSupported = 0,
            Low = 1 << 0,
            High = 1 << 1,
            Auto = 1 << 2,
        }

        [Flags]
        public enum ControlPassbookEnum
        {
            NotSupported = 0,
            TurnForward = 1 << 0,
            TurnBackward = 1 << 1,
            CloseForward = 1 << 2,
            CloseBackward = 1 << 3,
        }

        public enum PrintSidesEnum
        {
            NotSupported,
            Single,
            Dual
        }

        public PrinterCapabilitiesClass(TypeEnum Types,
                                        ResolutionEnum Resolutions,
                                        ReadFormEnum ReadForms,
                                        WriteFormEnum WriteForms,
                                        ExtentEnum Extents,
                                        ControlEnum Controls,
                                        int MaxMediaOnStacker,
                                        bool AcceptMedia,
                                        bool MultiPage,
                                        PaperSourceEnum PaperSources,
                                        bool MediaTaken,
                                        int RetractBins,
                                        List<int> MaxRetract,
                                        ImageTypeEnum ImageTypes,
                                        FrontImageColorFormatEnum FrontImageColorFormats,
                                        BackImageColorFormatEnum BackImageColorFormats,
                                        ImageSourceTypeEnum ImageSourceTypes,
                                        bool DispensePaper,
                                        string OSPrinter,
                                        bool MediaPresented,
                                        int AutoRetractPeriod,
                                        bool RetractToTransport,
                                        CoercivityTypeEnum CoercivityTypes,
                                        ControlPassbookEnum ControlPassbook,
                                        PrintSidesEnum PrintSides,
                                        Dictionary<string, bool> CustomPaperSources = null)
        {
            this.Types = Types;
            this.Resolutions = Resolutions;
            this.ReadForms = ReadForms;
            this.WriteForms = WriteForms;
            this.Extents = Extents;
            this.Controls = Controls;
            this.MaxMediaOnStacker = MaxMediaOnStacker;
            this.AcceptMedia = AcceptMedia;
            this.MultiPage = MultiPage;
            this.PaperSources = PaperSources;
            this.MediaTaken = MediaTaken;
            this.RetractBins = RetractBins;
            this.MaxRetract = MaxRetract;
            this.ImageTypes = ImageTypes;
            this.FrontImageColorFormats = FrontImageColorFormats;
            this.BackImageColorFormats = BackImageColorFormats;
            this.ImageSourceTypes = ImageSourceTypes;
            this.DispensePaper = DispensePaper;
            this.OSPrinter = OSPrinter;
            this.MediaPresented = MediaPresented;
            this.AutoRetractPeriod = AutoRetractPeriod;
            this.RetractToTransport = RetractToTransport;
            this.CoercivityTypes = CoercivityTypes;
            this.ControlPassbook = ControlPassbook;
            this.PrintSides = PrintSides;
            this.CustomPaperSources = CustomPaperSources;
        }

        /// <summary>
        /// Specifies the type(s) of the physical device driven by the logical service.
        /// </summary>
        public TypeEnum Types { get; init; }

        /// <summary>
        /// Specifies at which resolution(s) the physical device can print. Used by the application to select the level
        /// of print quality desired; does not imply any absolute level of resolution, only relative.
        /// </summary>
        public ResolutionEnum Resolutions { get; init; }

        /// <summary>
        /// Specifies whether the device can read data from media
        /// </summary>
        public ReadFormEnum ReadForms { get; init; }

        /// <summary>
        /// Specifies whether the device can write data to the media
        /// </summary>
        public WriteFormEnum WriteForms { get; init; }

        /// <summary>
        /// Specifies whether the device is able to measure the inserted media
        /// </summary>
        public ExtentEnum Extents { get; init; }

        /// <summary>
        /// Specifies the manner in which media can be controlled
        /// </summary>
        public ControlEnum Controls { get; init; }

        /// <summary>
        /// Specifies the maximum number of media items that the stacker can hold (zero if not available).
        /// </summary>
        public int MaxMediaOnStacker { get; init; }

        /// <summary>
        /// Specifies whether the device is able to accept media while no execute command is running that is waiting
        /// explicitly for media to be inserted.
        /// </summary>
        public bool AcceptMedia { get; init; }

        /// <summary>
        /// Specifies whether the device is able to support multiple page print jobs.
        /// </summary>
        public bool MultiPage { get; init; }

        /// <summary>
        /// Specifies the Paper sources available for this printer
        /// </summary>
        public PaperSourceEnum PaperSources { get; init; }

        /// <summary>
        /// Specifies whether the device is able to detect when the media is taken from the exit slot. If false, the
        /// [Printer.MediaTakenEvent](#printer.mediatakenevent) event is not fired.
        /// </summary>
        public bool MediaTaken { get; init; }

        /// <summary>
        /// Specifies the number of retract bins (zero if not supported).
        /// </summary>
        public int RetractBins { get; init; }

        /// <summary>
        /// An array of the length [retractBins](#common.capabilities.completion.properties.printer.retractbins) with
        /// the maximum number of media items that each retract bin can hold (one count for each supported bin, starting
        /// from zero for bin number one to retractBins - 1 for bin number retractBins). This will be omitted if there
        /// are no retract bins.
        /// </summary>
        public List<int> MaxRetract { get; init; }

        /// <summary>
        /// Specifies the image format supported by this device
        /// </summary>
        public ImageTypeEnum ImageTypes { get; init; }

        /// <summary>
        /// Specifies the front image color formats supported by this device
        /// </summary>
        public FrontImageColorFormatEnum FrontImageColorFormats { get; init; }

        /// <summary>
        /// Specifies the back image color formats supported by this device
        /// </summary>
        public BackImageColorFormatEnum BackImageColorFormats { get; init; }

        /// <summary>
        /// Specifies the source for the read image command supported by this device
        /// flags.
        /// </summary>
        public ImageSourceTypeEnum ImageSourceTypes { get; init; }

        /// <summary>
        /// Specifies whether the device is able to dispense paper.
        /// </summary>
        public bool DispensePaper { get; init; }

        /// <summary>
        /// Specifies the name of the default logical operating system printer that is associated with this Service
        /// Provider. Applications should use this printer name to generate native printer files to be printed through
        /// the Printer.PrintRawFile command. This value will be omitted if the Service
        /// Provider does not support the Printer.PrintRawFile command.
        /// </summary>
        public string OSPrinter { get; init; }

        /// <summary>
        /// Specifies whether the device is able to detect when the media is presented to the user for removal. If true,
        /// the Printer.MediaPresentedEvent event is fired. If false, the Printer.MediaPresentedEvent event is not fired.
        /// </summary>
        public bool MediaPresented { get; init; }

        /// <summary>
        /// Specifies the number of seconds before the device will automatically retract the presented media. If the
        /// command that generated the media is still active when the media is automatically retracted, the command will
        /// complete with an error. If the device does not retract media automatically this value will be zero.
        /// </summary>
        public int AutoRetractPeriod { get; init; }

        /// <summary>
        /// Specifies whether the device is able to retract the previously ejected media to the transport.
        /// </summary>
        public bool RetractToTransport { get; init; }

        /// <summary>
        /// Specifies the form write modes supported by this device
        /// </summary>
        public CoercivityTypeEnum CoercivityTypes { get; init; }

        /// <summary>
        /// Specifies how the passbook can be controlled with the [Printer.ControlPassbook](#printer.controlpassbook)
        /// command, as a combination of the following flags.
        /// </summary>
        public ControlPassbookEnum ControlPassbook { get; init; }

        /// <summary>
        /// Specifies on which sides of the media this device can print as one of the following values.
        /// 
        /// * ```NotSupported``` - The device is not capable of printing on any sides of the media.
        /// * ```Single``` - The device is capable of printing on one side of the media.
        /// * ```Dual``` - The device is capable of printing on two sides of the media.
        /// </summary>
        public PrintSidesEnum PrintSides { get; init; }

        /// <summary>
        /// Custom vendor specific paper sources
        /// </summary>
        public Dictionary<string, bool> CustomPaperSources { get; init; }
    }
}
