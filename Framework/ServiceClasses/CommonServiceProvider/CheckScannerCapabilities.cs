/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Commands;
using XFS4IoTServer;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// CashManagementCapabilities
    /// Store device capabilites for the cash management interface for cash devices
    /// </summary>
    public sealed class CheckScannerCapabilitiesClass(
        CheckScannerCapabilitiesClass.TypeEnum Type,
        int MaxMediaOnStacker,
        CheckScannerCapabilitiesClass.PrintSizeClass PrintSize,
        bool Stamp,
        bool Rescan,
        bool PresentControl,
        bool ApplicationRefuse,
        CheckScannerCapabilitiesClass.RetractLocationEnum RetractLocations,
        CheckScannerCapabilitiesClass.ResetControlEnum ResetControls,
        CheckScannerCapabilitiesClass.ImageTypeEnum ImageTypes,
        CheckScannerCapabilitiesClass.ImageCapabilities FrontImage,
        CheckScannerCapabilitiesClass.ImageCapabilities BackImage,
        CheckScannerCapabilitiesClass.CodelineFormatEnum CodelineFormats,
        CheckScannerCapabilitiesClass.DataSourceEnum DataSources,
        CheckScannerCapabilitiesClass.InsertOrientationEnum InsertOrientations,
        Dictionary<CheckScannerCapabilitiesClass.PositionEnum, CheckScannerCapabilitiesClass.PositonCapabilities> Positions,
        bool ImageAfterEndorse,
        CheckScannerCapabilitiesClass.ReturnedItemsProcessingEnum ReturnedItemsProcessing,
        CheckScannerCapabilitiesClass.PrintSizeClass PrintSizeFront)
    {
        /// <summary>
        /// Supported positions
        /// </summary>
        public enum PositionEnum
        {
            Input,
            Output,
            Refused,
        }

        /// <summary>
        /// Common output shutter position
        /// Single, Device accepts a single media item from the customer.
        /// Bunch Device accepts a bunch of media items from the customer.
        /// </summary>
        public enum TypeEnum
        {
            Single,
            Bunch,
        }

        /// <summary>
        /// Storage - Retract the media to a storage unit
        /// Transport - Retract the media to the transport
        /// Stacker - Retract the media to the stacker
        /// Rebuncher - Retract the media to the rebuncher
        /// </summary>
        [Flags]
        public enum RetractLocationEnum
        {
            Default = 0,
            Storage = 1 << 0,
            Transport = 1 << 1,
            Stacker = 1 << 2,
            ReBuncher = 1 << 3,
        }

        /// <summary>
        /// Eject - Eject media to the output location
        /// Storage - Retract the media to a storage unit
        /// Transport - Retract the media to the transport
        /// Rebuncher - Retract the media to the rebuncher
        /// </summary>
        [Flags]
        public enum ResetControlEnum
        {
            Default = 0,
            Eject = 1 << 0,
            Storage = 1 << 1,
            Transport = 1 << 2,
            ReBuncher = 1 << 3,
        }

        /// <summary>
        /// Tif - Scanned images in TIFF 6.0 format 
        /// Wmf - Scanned images in WMF (Windows Metafile) format.
        /// Bmp - Scanned images in windows BMP format.
        /// Jpg - Scanned images in windows BMP format.
        /// </summary>
        [Flags]
        public enum ImageTypeEnum
        {
            None = 0,
            TIF = 1 << 0,
            WMF = 1 << 1,
            BMP = 1 << 2,
            JPG = 1 << 3,
        }

        /// <summary>
        /// CMC7 - The device can read MICR CMC7
        /// E13B - The device can read MICR E13B
        /// OCR - The device can read code lines using Optical Character Recognition.
        /// OCRA - The device can read code lines using Optical Character Recognition font A.
        /// OCRB - The device can read code lines using Optical Character Recognition font B.
        /// </summary>
        [Flags]
        public enum CodelineFormatEnum
        {
            None = 0,
            CMC7 = 1 << 0,
            E13B = 1 << 1,
            OCR = 1 << 2,
            OCRA = 1 << 3,
            OCRB = 1 << 4,
        }

        /// <summary>
        /// ImageFront - The device can scan the front image of the document.
        /// ImageBack - The device can scan the back image of the document.
        /// Codeline - The device can recognize the code line.
        /// </summary>
        [Flags]
        public enum DataSourceEnum
        {
            None = 0,
            Front = 1 << 0,
            Back = 1 << 1,
            Codeline = 1 << 2,
        }

        /// <summary>
        /// CodelineRight - The media item should be inserted short edge first with the code line to the right.
        /// CodelineLeft - The media item should be inserted short edge first with the code line to the left.
        /// CodelineBottom - The media item should be inserted long edge first with the code line to the bottom.
        /// CodelineTop - The media item should be inserted long edge first with the code line to the top.
        /// FaceUp - The media item should be inserted with the front of the media item facing up.
        /// FaceDown - The media item should be inserted with the front of the media item facing down.
        /// </summary>
        [Flags]
        public enum InsertOrientationEnum
        {
            None = 0,
            CodelineRight = 1 << 0,
            CodelineLeft = 1 << 1,
            CodelineBottom = 1 << 2,
            CodelineTop = 1 << 3,
            FaceUp = 1 << 4,
            FaceDown = 1 << 5,
        }

        /// <summary>
        /// Endorse - This device can endorse a media item to be returned to the customer
        /// EndorseImage - This device can generate an image of a media item to be returned to the customer after it has been endorsed
        /// </summary>
        [Flags]
        public enum ReturnedItemsProcessingEnum
        {
            None = 0,
            Endorse = 1 << 0,
            EndorseImage = 1 << 1,
        }

        /// <summary>
        /// Size of printing area
        /// </summary>
        public sealed class PrintSizeClass(
            int Rows,
            int Cols)
        {

            /// <summary>
            /// Specifies the maximum number of rows of text that the device can print on the back of a media item. 
            /// This value is one for single line printers.
            /// </summary>
            public int Rows { get; init; } = Rows;

            /// <summary>
            /// Specifies the maximum number of characters that can be printed on a row.
            /// </summary>
            public int Cols { get; init; } = Cols;
        }

        /// <summary>
        /// Capabilities for scanning image
        /// </summary>
        public sealed class ImageCapabilities(
            ImageCapabilities.ColorFormatEnum ColorFormats,
            ImageCapabilities.ScanColorFlagEnum ScanColor,
            ImageCapabilities.ScanColorEnum DefaultScanColor)
        {
            /// <summary>
            /// Binary - scanned images in binary.
            /// GrayScale - scanned images in grayscale.
            /// Full - scanned images in full color.
            /// </summary>
            [Flags]
            public enum ColorFormatEnum
            {
                None = 0,
                Binary = 1 << 0,
                Grayscale = 1 << 1,
                Full = 1 << 2,
            }

            /// <summary>
            /// Scanning color supported
            /// </summary>
            [Flags]
            public enum ScanColorFlagEnum
            {
                None = 0,
                Red = 1 << 0,
                Green = 1 << 1,
                Blue = 1 << 2,
                Yellow = 1 << 3,
                White = 1 << 4,
                InfraRed = 1 << 5,
                UltraViolet = 1 << 6,
            }

            public enum ScanColorEnum
            {
                None,
                Red,
                Green,
                Blue,
                Yellow,
                White,
                InfraRed,
                UltraViolet,
            }

            /// <summary>
            /// Specifies the image color formats supported by this device.
            /// </summary>
            public ColorFormatEnum ColorFormats { get; init; } = ColorFormats;

            /// <summary>
            /// Specifies the image scan colors supported by this device and individually controllable by the
            /// application. Scan colors are used to enhance the scanning results on colored scan media.
            /// </summary>
            public ScanColorFlagEnum ScanColor { get; init; } = ScanColor;

            /// <summary>
            /// Specifies the default image color format used by this device.
            /// </summary>
            public ScanColorEnum DefaultScanColor { get; init; } = DefaultScanColor;
        }

        /// <summary>
        /// Capabilities for positions
        /// </summary>
        public sealed class PositonCapabilities(
            bool ItemsTakenSensor,
            bool ItemsInsertedSensor,
            PositonCapabilities.RetractAreasEnum RetractAreas)
        {
            /// <summary>
            /// RetractBin - Can retract items in this position to a retract storage unit.
            /// Transport -Can retract items in this position to the transport.
            /// Stacker - Can retract items in this position to the stacker.
            /// Rebuncher - Can retract items in this position to the stacker.
            /// </summary>
            [Flags]
            public enum RetractAreasEnum
            {
                None = 0,
                RetractBin = 1 << 0,
                Transport = 1 << 1,
                Stacker = 1 << 2,
                Rebuncher = 1 << 3,
            }

            /// <summary>
            /// Specifies whether or not the described position can detect when items at the exit position are taken by the user.
            /// If true the Service generates an accompanying MediaTakenEvent.
            /// If false this event is not generated. This relates to output and refused positions.
            /// </summary>
            public bool ItemsTakenSensor { get; init; } = ItemsTakenSensor;

            /// <summary>
            /// Specifies whether the described position has the ability to detect when items have been inserted by the user.
            /// If true the Service generates an accompanying MediaInsertedEvent.
            /// If false this event is not generated. This relates to all input positions
            /// </summary>
            public bool ItemsInsertedSensor { get; init; } = ItemsInsertedSensor;

            /// <summary>
            /// Specifies the areas to which items may be retracted from this position.
            /// </summary>
            public RetractAreasEnum RetractAreas { get; init; } = RetractAreas;
        }

        /// <summary>
        /// Specifies the type of the physical device driven by the service.
        /// </summary>
        public TypeEnum Type { get; init; } = Type;

        /// <summary>
        /// Specifies the maximum number of media items that the stacker can hold 
        /// (zero if the device does not have a stacker). 
        /// If the device has a bunch media input capability and the stacker is not present or 
        /// has a capacity of one then the application must process each item inserted sequentially 
        /// as described in section Multi-Feed Devices without a Stacker.
        /// </summary>
        public int MaxMediaOnStacker { get; init; } = MaxMediaOnStacker;

        /// <summary>
        /// This is representing the back side of the check, null if device has no back side printing capabilities. 
        /// If the media item is inserted in one of the orientations specified in InsertOrientation, 
        /// the Service Provider will print on the back side of the media. 
        /// If the media item is inserted in a different orientation to those specified in InsertOrientation then 
        /// printing may occur on the front side, upside down or both.
        /// </summary>
        public PrintSizeClass PrintSize { get; init; } = PrintSize;

        /// <summary>
        /// Specifies whether the device has stamping capabilities. 
        /// If the media item is inserted in one of the orientations specified in InsertOrientation, the device will stamp on the front side of the media.
        /// If the media item is inserted in a different orientation to those specified in InsertOrientation then
        /// stamping may occur on the back, upside down or both.
        /// </summary>
        public bool Stamp { get; init; } = Stamp;

        /// <summary>
        /// Specifies whether the device has the capability to either physically rescan media items after
        /// they have been inserted into the device or is able to generate any image supported by the device during the
        /// ReadImage]command (regardless of the images requested during the
        /// MediaIn command). 
        /// If true then the item can be rescanned or the images can be
        /// generated using the parameters passed in the ReadImage command .If false then all images required
        /// (various color, file format, bit depth) must be gathered during execution of the MediaIn command.
        /// </summary>
        public bool Rescan { get; init; } = Rescan;

        /// <summary>
        /// Specifies how the presenting of media items is controlled during the
        /// MediaInEnd and MediaInRollback commands.
        /// If true the presenting is controlled implicitly by the Service.If false the presenting must
        /// be controlled explicitly by the application using the PresentMedia command.
        /// This applies to all positions.
        /// </summary>
        public bool PresentControl { get; init; } = PresentControl;

        /// <summary>
        /// Specifies if the Device supports the MediaIn command mode where the
        /// application decides to accept or refuse each media item that has successfully been accepted by the device.
        /// If true then the Service supports this mode. If false then the Service does not
        /// support this mode(or the device does not have a stacker).
        /// </summary>
        public bool ApplicationRefuse { get; init; } = ApplicationRefuse;

        /// <summary>
        /// Specifies the locations to which the media can be retracted using the RetractMedia command.
        /// </summary>
        public RetractLocationEnum RetractLocations { get; init; } = RetractLocations;

        /// <summary>
        /// Specifies the manner in which the media can be handled on Reset command.
        /// </summary>
        public ResetControlEnum ResetControls { get; init; } = ResetControls;

        /// <summary>
        /// Specifies the image format supported by this device.
        /// </summary>
        public ImageTypeEnum ImageTypes { get; init; } = ImageTypes;

        /// <summary>
        /// Specifies the capabilities of the front image supported by this device. 
        /// null if front images are not supported.
        /// </summary>
        public ImageCapabilities FrontImage { get; init; } = FrontImage;

        /// <summary>
        /// Specifies the capabilities of the back image supported by this device. 
        /// null if front images are not supported.
        /// </summary>
        public ImageCapabilities BackImage { get; init; } = BackImage;

        /// <summary>
        /// Specifies the code line formats supported by this device.
        /// </summary>
        public CodelineFormatEnum CodelineFormats { get; init; } = CodelineFormats;

        /// <summary>
        /// Specifies the reading/imaging capabilities supported by this device.
        /// </summary>
        public DataSourceEnum DataSources { get; init; } = DataSources;

        /// <summary>
        /// Specifies the media item insertion orientations supported by the Device such that hardware
        /// features such as MICR reading, endorsing and stamping will be aligned with the correct edges and sides
        /// of the media item.Devices may still return code lines and images even if one of these orientations is not
        /// used during media insertion.If the media items are inserted in one of the orientations defined in this
        /// capability then any printing or stamping will be on the correct side of the media item.If the media is
        /// inserted in a different orientation then any printing or stamping may be on the wrong side, upside down
        /// or both. This value is reported based on the customer's perspective.
        /// </summary>
        public InsertOrientationEnum InsertOrientations { get; init; } = InsertOrientations;

        /// <summary>
        /// Array of supported position status.
        /// </summary>
        public Dictionary<PositionEnum, PositonCapabilities> Positions { get; init; } = Positions;

        /// <summary>
        /// Specifies whether the device can generate an image after text is printed on the media item. 
        /// If true then the generation of the image can be specified using the SetMediaParameters command.
        /// If false, this functionality is not available. This capability applies to media items whose
        /// destination is a storage unit; the ReturnedItemsProcessing capability indicates whether this functionality
        /// is supported for media items that are to be returned to the customer.
        /// </summary>
        public bool ImageAfterEndorse { get; init; } = ImageAfterEndorse;

        /// <summary>
        /// Specifies the processing that this device supports for media items that are identified to be returned to
        /// the customer using the SetMediaParameters command
        /// </summary>
        public ReturnedItemsProcessingEnum ReturnedItemsProcessing { get; init; } = ReturnedItemsProcessing;

        /// <summary>
        /// Reports the printing capabilities of the device on the front side of the check, 
        /// null if device has no front printing capabilities.
        /// If the media item is inserted in one of the orientations specified in InsertOrientation, the device
        /// will print on the front side of the media.If the media item is inserted in a different orientation to those
        /// specified in InsertOrientation then printing may occur on the back side, upside down or both.
        /// </summary>
        public PrintSizeClass PrintSizeFront { get; init; } = PrintSizeFront;

    }
}
