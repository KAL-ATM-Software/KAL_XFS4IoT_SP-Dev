/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using XFS4IoTServer;
using XFS4IoT.Check.Completions;
using XFS4IoTFramework.Storage;
using XFS4IoT.Completions;
using XFS4IoT;

namespace XFS4IoTFramework.Check
{
    /// <summary>
    /// This enum used for MediaPresented event
    /// </summary>
    public enum MediaPresentedPositionEnum
    {
        Input,
        Refused,
        ReBuncher
    }

    /// <summary>
    /// Unit - The media is in the storage unit.
    /// Device - The media is in the device.
    /// Position - The media is at one or more of the input, output and refused positions.
    /// Jammed - The media is jammed in the device.
    /// Customer - The media has been returned and taken by the customer.
    /// UnKnown - The media is in an unknown position.
    /// </summary>
    public enum MediaDetectedPositionEnum
    {
        Unit,
        Device,
        Position,
        Jammed,
        Customer,
        Unknown
    }

    /// <summary>
    /// Front``` - The image is for the front of the media item.
    /// Back``` - The image is for the back of the media item.
    /// </summary>
    public enum ImageSourceEnum
    {
        Front,
        Back,
    }

    public enum CodelineFomratEnum
    {
        None,
        CMC7,
        E13B,
        OCR,
        OCRA,
        OCRB,
    }

    public sealed class MediaSizeInfo(int LongEdge = 0, int ShortEdge = 0)
    {

        /// <summary>
        /// Specifies the length of the long edge of the media in millimeters, or 0 if unknown.
        /// </summary>
        public int LongEdge { get; init; } = LongEdge;

        /// <summary>
        /// Specifies the length of the short edge of the media in millimeters, or 0 if unknown.
        /// </summary>
        public int ShortEdge { get; init; } = ShortEdge;
    }

    public class ImageInfo(
        ImageInfo.ImageFormatEnum ImageFormat,
        ImageInfo.ColorFormatEnum ColorFormat,
        ImageInfo.ScanColorEnum ScanColor)
    {
        /// <summary>
        /// TIF - The image is in TIFF 6.0 format.
        /// WMF - The image is in WMF(Windows Metafile) format.
        /// BMP - The image is in Windows BMP format.
        /// JPG - The image is in JPG format.
        /// </summary>
        public enum ImageFormatEnum
        {
            BMP,
            WMF,
            TIF,
            JPG,
        }

        /// <summary>
        /// Binary - The image is binary (image contains two colors, usually the colors black and white).
        /// GrayScale - The image is gray scale(image contains multiple gray colors).
        /// Full - The image is full color(image contains colors like red, green, blue etc.).
        /// </summary>
        public enum ColorFormatEnum
        {
            Binary,
            Grayscale,
            Full,
        }

        /// <summary>
        /// Red - The image is scanned with red light.
        /// Green``` - The image is scanned with green light.
        /// Blue``` - The image is scanned with blue light.
        /// Yellow``` - The image is scanned with yellow light.
        /// White``` - The image is scanned with white light.
        /// InfraRed``` - The image is scanned with infrared light.
        /// UltraViolet``` - The image is scanned with ultraviolet light.
        /// </summary>
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
        /// Specifies the format of the image.
        /// </summary>
        public ImageFormatEnum ImageFormat { get; init; } = ImageFormat;

        /// <summary>
        /// Specifies the color format of the image.
        /// </summary>
        public ColorFormatEnum ColorFormat { get; init; } = ColorFormat;

        /// <summary>
        /// Selects the scan color.
        /// </summary>
        public ScanColorEnum ScanColor { get; init; } = ScanColor;
    }

    public class ImageDataInfo : ImageInfo
    {
        /// <summary>
        /// Ok - The data is OK.
        /// SourceNotSupported - The data source or image attributes are not supported by the Service, e.g., scan color not supported.
        /// SourceMissing - The image could not be obtained.
        /// </summary>
        public enum ImageStatusEnum
        {
            Ok,
            SourceNotSupported,
            SourceMissing,
        }

        public ImageDataInfo():
            base(ImageInfo.ImageFormatEnum.BMP,
                 ImageInfo.ColorFormatEnum.Binary,
                 ImageInfo.ScanColorEnum.Red)
        {
            ImageStatus = ImageStatusEnum.SourceNotSupported;
            ImageData.Clear();
        }

        public ImageDataInfo(
            ImageInfo.ImageFormatEnum ImageType,
            ImageInfo.ColorFormatEnum ColorFormat,
            ImageInfo.ScanColorEnum ScanColor,
            ImageStatusEnum ImageStatus,
            List<byte> ImageData) :
            base(ImageType,
                 ColorFormat,
                 ScanColor)
        {
            this.ImageStatus = ImageStatus;
            this.ImageData = ImageData;
        }

        /// <summary>
        /// Status of the image data.
        /// </summary>
        public ImageStatusEnum ImageStatus { get; init; }

        /// <summary>
        /// Scanned image data
        /// </summary>
        public List<byte> ImageData { get; init; } = [];
    }

    public class MediaDataInfo(
        MediaDataInfo.MagneticReadIndicatorEnum MagneticReadIndicator,
        string CodelineData = null,
        Dictionary<ImageSourceEnum, ImageDataInfo> Images = null,
        MediaDataInfo.CodelineOrientationEnum CodelineOrientation = MediaDataInfo.CodelineOrientationEnum.None,
        MediaDataInfo.MediaOrientationEnum MediaOrientation = MediaDataInfo.MediaOrientationEnum.None,
        MediaSizeInfo MediaSize = null,
        MediaDataInfo.MediaValidityEnum MediaValidity = MediaDataInfo.MediaValidityEnum.Unknown)
    {
        /// <summary>
        /// MICR - The MICR code line was read using MICR technology and MICR characters were present.
        /// Not_MICR - The MICR code line was NOT read using MICR technology.
        /// No_MICR - The MICR code line was read using MICR technology and no magnetic characters were read.
        /// Unknown - It is unknown how the MICR code line was read.
        /// Not_MICR_Format - The code line is not a MICR format code line.
        /// NotRead - No code line was read.
        /// </summary>
        public enum MagneticReadIndicatorEnum
        {
            MICR,
            Not_MICR,
            No_MICR,
            Unknown,
            Not_MICR_Format,
            NotRead,
        }

        /// <summary>
        /// Right - The code line is to the right.
        /// Left - The code line is to the left.
        /// Bottom - The code line is to the bottom.
        /// Top - The code line is to the top.
        /// </summary>
        public enum CodelineOrientationEnum
        {
            None,
            Right,
            Left,
            Bottom,
            Top,
        }

        /// <summary>
        /// Up - The front of the media (the side with the code line) is facing up.
        /// Down - The front of the media(the side with the code line) is facing down.
        /// </summary>
        public enum MediaOrientationEnum
        {
            None,
            Up,
            Down,
        }

        /// <summary>
        /// Ok - The media item is valid.
        /// Suspect - The validity of the media item is suspect.
        /// Unknown - The validity of the media item is unknown.
        /// NoValidation - No specific security features were evaluated.
        /// </summary>
        public enum MediaValidityEnum
        {
            Ok,
            Suspect,
            Unknown,
            NoValidation
        }

        /// <summary>
        /// Specifies the code line data.
        /// </summary>
        public string CodelineData { get; set; } = CodelineData;

        /// <summary>
        /// Specifies the type of technology used to read a MICR code line.
        /// </summary>
        public MagneticReadIndicatorEnum MagneticReadIndicator { get; set; } = MagneticReadIndicator;

        /// <summary>
        /// Specify the orientation of codeline
        /// </summary>
        public CodelineOrientationEnum CodelineOrientation { get; set; } = CodelineOrientation;

        /// <summary>
        /// Specify the orientation of media
        /// </summary>
        public MediaOrientationEnum MediaOrientation { get; set; } = MediaOrientation;

        /// <summary>
        /// Specifies the size of the media item. Will be null if the device does not support media size measurement or
        /// no size measurements are known.
        /// </summary>
        public MediaSizeInfo MediaSize { get; set; } = MediaSize;

        /// <summary>
        /// Media items may have special security features which can be detected by the device. This specifies
        /// whether the media item is suspect or valid, allowing the application the choice in how to further
        /// process a media item that could not be confirmed as being valid.
        /// </summary>
        public MediaValidityEnum MediaValidity { get; set; } = MediaValidity;

        /// <summary>
        /// If the Device has determined the orientation of the media, then all images returned are in the standard orientation and the images will match the image source
        /// requested by the application.This means that images will be returned with the code line at the bottom,
        /// and the image of the front and rear of the media item will be returned in the structures associated with
        /// the image sources respectively.
        /// </summary>
        public Dictionary<ImageSourceEnum, ImageDataInfo> Images { get; set; } = Images;
    }

    /// <summary>
    /// Perform hardware reset operation and if the media is detected, move it into the specified location
    /// </summary>
    public sealed class ResetDeviceRequest(
        ResetDeviceRequest.MediaControlEnum MediaControl,
        string StorageId)
    {

        public enum MediaControlEnum
        {
            Default,
            Eject,
            Transport,
            ReBuncher,
            Unit,
        }

        /// <summary>
        /// Specifies the manner in which the media should be handled
        /// </summary>
        public MediaControlEnum MediaControl { get; init; } = MediaControl;

        /// <summary>
        /// The media item is in a storage unit as specified by the identifier
        /// </summary>
        public string StorageId { get; init; } = StorageId;
    }

    /// <summary>
    /// ResetResult
    /// Return result of reset operation
    /// </summary>
    public sealed class ResetDeviceResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        ResetCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
        Dictionary<string, CheckUnitCountClass> MovementResult = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on reset operation
        /// </summary>
        public ResetCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;

        /// <summary>
        /// Specifies the detailed note movement while in reset operation.
        /// </summary>
        public Dictionary<string, CheckUnitCountClass> MovementResult { get; init; } = MovementResult;
    }

    /// <summary>
    /// Replenish supplies
    /// </summary>
    public sealed class SupplyReplenishRequest(SupplyReplenishRequest.SupplyEnum Supplies)
    {
        public enum SupplyEnum
        {
            Toner = 1 << 0,
            Ink = 1 << 1,
        }

        /// <summary>
        /// Specifies which supplies were replenished.
        /// </summary>
        public SupplyEnum Supplies { get; init; } = Supplies;
    }

    /// <summary>
    /// ExpelMediaResult
    /// Return result of expel media operation
    /// </summary>
    public sealed class ExpelMediaResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        ExpelMediaCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on expel media operation
        /// </summary>
        public ExpelMediaCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    /// <summary>
    /// Present media to specified location
    /// </summary>
    public sealed class PresentMediaRequest(PresentMediaRequest.PositionEnum Position)
    {
        public enum PositionEnum
        {
            All,
            Input,
            Refused,
            ReBuncher
        }

        /// <summary>
        /// Specifies the position to move items.
        /// </summary>
        public PositionEnum Position { get; init; } = Position;
    }

    public sealed class PresentMediaResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        PresentMediaCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on presenting media operation
        /// </summary>
        public PresentMediaCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    /// <summary>
    /// Scan check image
    /// </summary>
    public sealed class ReadImageRequest(
        int MediaId,
        CodelineFomratEnum CodelineFormat,
        Dictionary<ImageSourceEnum, ImageInfo> ImageInfo)
    {
        /// <summary>
        /// Specifies the sequence number (starting from 1) of a media item.
        /// </summary>
        public int MediaId { get; init; } = MediaId;

        /// <summary>
        /// Specifies the code line format. None if reading codeline is not required.
        /// </summary>
        public CodelineFomratEnum CodelineFormat { get; init; } = CodelineFormat;

        /// <summary>
        /// Image information to scan checks
        /// </summary>
        public Dictionary<ImageSourceEnum, ImageInfo> ImageInfo { get; init; } = ImageInfo;
    }

    /// <summary>
    /// Result of the image scanning operation
    /// </summary>
    public sealed class ReadImageResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        ReadImageCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
        Dictionary<ImageSourceEnum, ImageDataInfo> ImageData = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on scanning image data
        /// </summary>
        public ReadImageCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;

        /// <summary>
        /// Store Image data read.
        /// </summary>
        public Dictionary<ImageSourceEnum, ImageDataInfo> ImageData { get; init; } = ImageData;
    }

    /// <summary>
    /// RetractRequest
    /// The parameter class for the retract operation
    /// </summary>
    /// <remarks>
    /// ResetRequest
    /// The parameter class for the retract operation
    /// </remarks>
    public sealed class RetractMediaRequest(
        RetractMediaRequest.LocationEnum Location,
        string StorageId)
    {
        public enum LocationEnum
        {
            Default,
            Stacker,
            Transport,
            ReBuncher,
            Unit,
        }

        /// <summary>
        /// Specifies the location where to be stored when items are detected while in reset operation.
        /// </summary>
        public LocationEnum Location { get; init; } = Location;

        /// <summary>
        /// If the Location property is set Unit, this property is set to move items, otherwise an empty or null string.
        /// </summary>
        public string StorageId { get; init; } = StorageId;
    }

    /// <summary>
    /// RetractResult
    /// Return result of retract items
    /// </summary>
    /// <remarks>
    /// ResetDeviceResult
    /// Return result of retract items
    /// </remarks>
    public sealed class RetractMediaResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        RetractMediaCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
        Dictionary<string, CheckUnitCountClass> MovementResult = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public RetractMediaCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;

        /// <summary>
        /// Specifies the detailed note movement while in reset operation.
        /// </summary>
        public Dictionary<string, CheckUnitCountClass> MovementResult { get; init; } = MovementResult;
    }

    /// <summary>
    /// RetractRequest
    /// The parameter class for the retract operation
    /// </summary>
    public sealed class SetMediaParametersRequest(
        int MediaId,
        SetMediaParametersRequest.DestinationEnum Destination,
        string StorageId,
        bool Stamp,
        string PrintData,
        Dictionary<ImageSourceEnum, ImageInfo> ImagesToRead)
    {
        public enum DestinationEnum
        {
            Customer,
            Unit,
        }

        /// <summary>
        /// Specifies the sequence number of a media item. 
        /// Valid IDs are 1 to the maximum media ID assigned within the transaction
        /// if the value is 0, all media items are selected.
        /// </summary>
        public int MediaId { get; init; } = MediaId;

        /// <summary>
        /// Specifies where the item(s) specified by MediaIds are to be moved to.
        /// If the destination is Unit, refer to StorageId property.
        /// </summary>
        public DestinationEnum Destination { get; init; } = Destination;

        /// <summary>
        /// Storage Id where the medias are to be stored.
        /// </summary>
        public string StorageId { get; init; } = StorageId;

        /// <summary>
        /// Specifies whether the media will be stamped.
        /// </summary>
        public bool Stamp { get; init; } = Stamp;

        /// <summary>
        /// Specifies the data that will be printed on the media item that is entered by the customer. If a
        /// character is not supported by the device it will be replaced by a vendor dependent substitution
        /// character.If not specified, no text will be printed.
        /// For devices that can print multiple lines, each line is separated by a Carriage Return(Unicode 0x000D) and Line
        /// Feed(Unicode 0x000A) sequence.For devices that can print on both sides, the front and back print data are
        /// separated by a Carriage Return(Unicode 0x000D) and a Form Feed(Unicode 0x000C) sequence.In this case the data
        /// to be printed on the back is the first set of data, and the front is the second set of data.
        /// </summary>
        public string PrintData { get; init; } = PrintData;

        /// <summary>
        /// Specifies the images required to be read.
        /// </summary>
        public Dictionary<ImageSourceEnum, ImageInfo> ImagesToRead { get; init; } = ImagesToRead;
    }

    /// <summary>
    /// SetMediaParametersResult
    /// Return result of set actions
    /// </summary>
    public sealed class SetMediaParametersResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        SetMediaParametersCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public SetMediaParametersCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    /// <summary>
    /// MediaInRequest
    /// The parameter class for the media-in operation
    /// </summary>
    public sealed class MediaInRequest(
        CodelineFomratEnum CodelineFormat,
        Dictionary<ImageSourceEnum, ImageInfo> ImagesToRead,
        int MaxMediaOnStacker,
        bool ApplicationRefuse)
    { 
        /// <summary>
        /// Specifies the images required to be read.
        /// </summary>
        public CodelineFomratEnum CodelineFormat { get; init; } = CodelineFormat;

        /// <summary>
        /// Specifies the images required to be read.
        /// </summary>
        public Dictionary<ImageSourceEnum, ImageInfo> ImagesToRead { get; init; } = ImagesToRead;

        /// <summary>
        /// Maximum number of media items allowed on the stacker during the media-in transaction. This value is used to
        /// limit the total number of media items on the stacker. When this limit is reached all further media items
        /// will be refused and a Check.MediaRefusedEvent message will be generated
        /// reporting StackerFull.
        /// Ignored unless specified on the first MediaIn operation within a single
        /// media-in transaction.
        /// Ignored on devices without stackers.
        /// </summary>
        public int MaxMediaOnStacker { get; init; } = MaxMediaOnStacker;

        /// <summary>
        /// Specifies if the application wants to make the decision to accept or refuse each media item that has
        /// successfully been accepted by the device.
        /// - If true then the application must decide to accept or refuse each item.
        /// - If false then any decision on whether an item should be refused is left to the device/Service.
        /// - Ignored unless specified on the first MediaIn operation within a single
        /// media-in transaction.
        /// - Ignored if applicationRefuse capability is false.
        /// </summary>
        public bool ApplicationRefuse { get; init; } = ApplicationRefuse;
    }

    /// <summary>
    /// MediaInResult
    /// Return result of media-in operation
    /// </summary>
    public sealed class MediaInResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        MediaInCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
        int MediaOnStacker = -1,
        int LastMedia = -1,
        int LastMediaOnStacker = -1) 
        : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public MediaInCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;

        /// <summary>
        /// Contains the total number of media items on the stacker. could be -1 if it is
        /// unknown or the device does not have a stacker.
        /// </summary>
        public int MediaOnStacker { get; init; } = MediaOnStacker;

        /// <summary>
        /// Contains the number of media items processed by this instance of the operation. could be -1 if it is
        /// unknown or the device does not have a stacker.
        /// </summary>
        public int LastMedia { get; init; } = LastMedia;

        /// <summary>
        /// Contains the number of media items on the stacker successfully accepted by this instance of the command
        /// execution. could be -1 if it is unknown or the device does not have a stacker.
        /// </summary>
        public int LastMediaOnStacker { get; init; } = LastMediaOnStacker;
    }

    /// <summary>
    /// The parameter class for the rollback operation.
    /// Empty oject and for future extention.
    /// </summary>
    public sealed class MediaInRollbackRequest
    {
        public MediaInRollbackRequest()
        { }
    }

    public sealed class MediaInEndCountClass(
        int ItemsReturned = 0,
        int ItemsRefused = 0,
        int BunchesRefused = 0,
        Dictionary<string, CheckUnitCountClass> MovementResult = null)
    {

        /// <summary>
        /// Contains the number of media items that were returned to the customer by application selection through the
        /// SetMediaParameters during the current transaction. This does not include 
        /// items that were refused.
        /// Specify -1 if it's unknown.
        /// </summary>
        public int ItemsReturned { get; init; } = ItemsReturned;

        /// <summary>
        /// Contains the total number of items automatically returned to the customer during the 
        /// execution of the whole transaction. This count does not include bunches of items which are 
        /// refused as a single entity without being processed as single items.
        /// Specify -1 if it's unknown.
        /// </summary>
        public int ItemsRefused { get; init; } = ItemsRefused;

        /// <summary>
        /// Contains the total number of refused bunches of items that were automatically 
        /// returned to the customer without being processed as single items.
        /// Specify -1 if it's unknown.
        /// </summary>
        public int BunchesRefused { get; init; } = BunchesRefused;

        /// <summary>
        /// Specifies the detailed note movement while in reset operation.
        /// </summary>
        public Dictionary<string, CheckUnitCountClass> MovementResult { get; init; } = MovementResult;
    }

    /// <summary>
    /// MediaInRollbackResult
    /// Return result of rollback operation
    /// </summary>
    public sealed class MediaInRollbackResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        MediaInRollbackCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
        MediaInEndCountClass MediaInEndCount = null) : DeviceResult(CompletionCode, ErrorDescription)

    {

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public MediaInRollbackCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;

        /// <summary>
        /// Result of counts at the end of operation.
        /// </summary>
        public MediaInEndCountClass MediaInEndCount { get; init; } = MediaInEndCount;
    }

    /// <summary>
    /// The parameter class for completing media-in operation.
    /// Empty oject and for future extention.
    /// </summary>
    public sealed class MediaInEndRequest
    {
        public MediaInEndRequest()
        { }
    }

    /// <summary>
    /// MediaInEndResult
    /// Return result of completing media-in operation
    /// </summary>
    public sealed class MediaInEndResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        MediaInEndCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
        MediaInEndCountClass MediaInEndCount = null) : DeviceResult(CompletionCode, ErrorDescription)

    {

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public MediaInEndCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;

        /// <summary>
        /// Result of counts at the end of operation.
        /// </summary>
        public MediaInEndCountClass MediaInEndCount { get; init; } = MediaInEndCount;
    }

    /// <summary>
    /// The parameter class to get the next item from the multi-item feed unit and capture the item data.
    /// Empty oject and for future extention.
    /// </summary>
    public sealed class GetNextItemRequest
    {
        public GetNextItemRequest()
        { }
    }

    /// <summary>
    /// Return result of processing next item operation.
    /// </summary>
    public sealed class GetNextItemResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        GetNextItemCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public GetNextItemCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    /// <summary>
    /// Take predefined actions (move item to destination, stamping, endorsing, re-imaging) to be
    /// executed on the current media item.This command only applies to devices without stackers 
    /// and on devices with stackers this command is not supported.
    /// Empty oject and for future extention.
    /// </summary>
    public sealed class ActionItemRequest
    {
        public ActionItemRequest()
        { }
    }

    /// <summary>
    /// Return result of action to move items in predefined actions.
    /// </summary>
    public sealed class ActionItemResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        ActionItemCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
        Dictionary<string, CheckUnitCountClass> MovementResult = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public ActionItemCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;

        /// <summary>
        /// Specifies the detailed note movement while in reset operation.
        /// </summary>
        public Dictionary<string, CheckUnitCountClass> MovementResult { get; init; } = MovementResult;
    }

    /// <summary>
    /// The applications indicates if the current media item should be accepted or refused. 
    /// Applications only use this operation when the media-in operation is used in the mode 
    /// where the application can decide if each physically acceptable media item should be 
    /// accepted or refused.
    /// </summary>
    public sealed class AcceptItemRequest(bool AcceptItem)
    {

        /// <summary>
        /// Specifies if the item should be accepted or refused. 
        /// If true then the item is accepted and moved to the stacker.
        /// If false then the item is moved to the re-buncher/refuse position.
        /// </summary>
        public bool AcceptItem { get; init; } = AcceptItem;
    }

    /// <summary>
    /// Return result of action to accept or refuse current media item.
    /// </summary>
    public sealed class AcceptItemResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        AcceptItemCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)

    {

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public AcceptItemCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }
}