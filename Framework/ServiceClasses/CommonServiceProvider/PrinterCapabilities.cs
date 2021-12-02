/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
            Receipt = 0x0001,
            Passbook = 0x0002,
            Journal = 0x0004,
            Document = 0x0008,
            Scanner = 0x0010,
        }

        [Flags]
        public enum ResolutionEnum
        {
            Low = 0x0001,
            Medium = 0x0002,
            High = 0x0004,
            VeryHigh = 0x0008,
        }

        [Flags]
        public enum ReadFormEnum
        {
            NotSupported = 0,
            OCR = 0x0001,
            MICR = 0x0002,
            MSF = 0x0004,
            Barcode = 0x0008,
            PageMark = 0x0010,
            Image = 0x0020,
            EmptyLine = 0x0040,
        }

        [Flags]
        public enum WriteFormEnum
        {
            NotSupported = 0,
            Text = 0x0001,
            Graphics = 0x0002,
            OCR = 0x0004,
            MICR = 0x0008,
            MSF = 0x0010,
            Barcode = 0x0020,
            Stamp = 0x0040,
        }

        [Flags]
        public enum ExtentEnum
        {
            NotSupported = 0,
            Horizontal = 0x0001,
            Vertical = 0x0002,
        }

        [Flags]
        public enum ControlEnum
        {
            NotSupported = 0,
            Eject = 0x00000001,
            Perforate = 0x00000002,
            Cut = 0x00000004,
            Skip = 0x00000008,
            Flush = 0x00000010,
            Retract = 0x00000020,
            Stack = 0x00000040,
            PartialCut = 0x00000080,
            Alarm = 0x00000100,
            PageForward = 0x00000200,
            PageBackward = 0x00000400,
            TurnMedia = 0x00000800,
            Stamp = 0x00001000,
            Park = 0x00002000,
            Expel = 0x00004000,
            EjectToTransport = 0x00008000,
            Rotate180 = 0x00010000,
            ClearBuffer = 0x00020000,
            Backward = 0x00040000,
            Forward = 0x00080000,
        }

        [Flags]
        public enum PaperSourceEnum
        {
            NotSupported = 0,
            Upper = 0x0001,
            Lower = 0x0002,
            External = 0x0004,
            AUX = 0x0008,
            AUX2 = 0x0010,
            Park = 0x0020,
        }

        [Flags]
        public enum ImageTypeEnum
        {
            NotSupported = 0,
            TIF = 0x0001,
            WMF = 0x0002,
            BMP = 0x0004,
            JPG = 0x0008,
        }

        [Flags]
        public enum FrontImageColorFormatEnum
        {
            NotSupported = 0,
            Binary = 0x0001,
            GrayScale = 0x0002,
            Full = 0x0004,
        }

        [Flags]
        public enum BackImageColorFormatEnum
        {
            NotSupported = 0,
            Binary = 0x0001,
            GrayScale = 0x0002,
            Full = 0x0004,
        }

        [Flags]
        public enum CodelineFormatEnum
        {
            NotSupported = 0,
            CMC7 = 0x0001,
            E13B = 0x0002,
            OCR = 0x0004,
        }

        [Flags]
        public enum ImageSourceTypeEnum
        {
            NotSupported = 0,
            ImageFront = 0x0001,
            ImageBack = 0x0002,
            CodeLine = 0x0004,
        }

        [Flags]
        public enum CoercivityTypeEnum
        {
            NotSupported = 0,
            Low = 0x0001,
            High = 0x0002,
            Auto = 0x0004,
        }

        [Flags]
        public enum ControlPassbookEnum
        {
            NotSupported = 0,
            TurnForward = 0x0001,
            TurnBackward = 0x0002,
            CloseForward = 0x0004,
            CloseBackward = 0x0008,
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
                                        CodelineFormatEnum CodelineFormats,
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
            this.CodelineFormats = CodelineFormats;
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
        /// Specifies the codeline format supported by this device
        /// flags.
        /// </summary>
        public CodelineFormatEnum CodelineFormats { get; init; }

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
