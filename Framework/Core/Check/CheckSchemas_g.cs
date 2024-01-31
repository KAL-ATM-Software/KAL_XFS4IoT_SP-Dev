/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * CheckSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Check
{

    public enum TonerEnum
    {
        Full,
        Low,
        Out,
        Unknown
    }


    public enum InkEnum
    {
        Full,
        Low,
        Out,
        Unknown
    }


    public enum FrontImageScannerEnum
    {
        Ok,
        Fading,
        Inoperative,
        Unknown
    }


    public enum BackImageScannerEnum
    {
        Ok,
        Fading,
        Inoperative,
        Unknown
    }


    public enum MicrReaderEnum
    {
        Ok,
        Fading,
        Inoperative,
        Unknown
    }


    public enum MediaFeederEnum
    {
        Empty,
        NotEmpty,
        Inoperative,
        Unknown
    }


    public enum ShutterStateEnum
    {
        Closed,
        Open,
        Jammed,
        Unknown
    }


    [DataContract]
    public sealed class PositionStatusClass
    {
        public PositionStatusClass(ShutterStateEnum? Shutter = null, PositionStatusEnum? PositionStatus = null, TransportEnum? Transport = null, TransportMediaStatusEnum? TransportMediaStatus = null, JammedShutterPositionEnum? JammedShutterPosition = null)
        {
            this.Shutter = Shutter;
            this.PositionStatus = PositionStatus;
            this.Transport = Transport;
            this.TransportMediaStatus = TransportMediaStatus;
            this.JammedShutterPosition = JammedShutterPosition;
        }

        [DataMember(Name = "shutter")]
        public ShutterStateEnum? Shutter { get; init; }

        public enum PositionStatusEnum
        {
            Empty,
            NotEmpty,
            Unknown
        }

        /// <summary>
        /// The status of the position. This property is null in [Common.Status](#common.status) if the physical device
        /// is not capable of reporting whether or not items are at the position, otherwise the following values are
        /// possible:
        /// 
        /// * ```empty``` - The position is empty.
        /// * ```notEmpty``` - The position is not empty.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the position cannot be determined.
        /// </summary>
        [DataMember(Name = "positionStatus")]
        public PositionStatusEnum? PositionStatus { get; init; }

        public enum TransportEnum
        {
            Ok,
            Inoperative,
            Unknown
        }

        /// <summary>
        /// Specifies the state of the transport mechanism. The transport is defined as any area leading to or from the
        /// position. This property is null in [Common.Status](#common.status) if the physical device has no transport
        /// or transport state reporting is not supported, otherwise the following values are possible:
        /// 
        /// * ```ok``` - The transport is in a good state.
        /// * ```inoperative``` - The transport is inoperative due to a hardware failure or media jam.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the transport cannot be determined.
        /// </summary>
        [DataMember(Name = "transport")]
        public TransportEnum? Transport { get; init; }

        public enum TransportMediaStatusEnum
        {
            Empty,
            NotEmpty,
            Unknown
        }

        /// <summary>
        /// Returns information regarding items which may be present on the transport. This property is null in
        /// [Common.Status](#common.status) if the physical device is not capable of reporting whether or not items are
        /// on the transport, otherwise the following values are possible:
        /// 
        /// * ```empty``` - The transport is empty.
        /// * ```notEmpty``` - The transport is not empty.
        /// * ```unknown``` - Due to a hardware error or other condition it is not known whether there are items on the transport.
        /// </summary>
        [DataMember(Name = "transportMediaStatus")]
        public TransportMediaStatusEnum? TransportMediaStatus { get; init; }

        public enum JammedShutterPositionEnum
        {
            NotJammed,
            Open,
            PartiallyOpen,
            Closed,
            Unknown
        }

        /// <summary>
        /// Returns information regarding the position of the jammed shutter. This property is null in
        /// [Common.Status](#common.status) if the physical device has no shutter or the reporting of the position of a
        /// jammed shutter is not supported, otherwise the following values are possible:
        /// 
        /// * ```notJammed``` - The shutter is not jammed.
        /// * ```open``` - The shutter is jammed, but fully open.
        /// * ```partiallyOpen``` - The shutter is jammed, but partially open.
        /// * ```closed``` - The shutter is jammed, but fully closed.
        /// * ```unknown``` - The position of the shutter is unknown.
        /// </summary>
        [DataMember(Name = "jammedShutterPosition")]
        public JammedShutterPositionEnum? JammedShutterPosition { get; init; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(AcceptorEnum? Acceptor = null, MediaEnum? Media = null, TonerEnum? Toner = null, InkEnum? Ink = null, FrontImageScannerEnum? FrontImageScanner = null, BackImageScannerEnum? BackImageScanner = null, MicrReaderEnum? MICRReader = null, StackerEnum? Stacker = null, RebuncherEnum? Rebuncher = null, MediaFeederEnum? MediaFeeder = null, PositionsClass Positions = null)
        {
            this.Acceptor = Acceptor;
            this.Media = Media;
            this.Toner = Toner;
            this.Ink = Ink;
            this.FrontImageScanner = FrontImageScanner;
            this.BackImageScanner = BackImageScanner;
            this.MICRReader = MICRReader;
            this.Stacker = Stacker;
            this.Rebuncher = Rebuncher;
            this.MediaFeeder = MediaFeeder;
            this.Positions = Positions;
        }

        public enum AcceptorEnum
        {
            Ok,
            State,
            Stop,
            Unknown
        }

        /// <summary>
        /// Supplies the state of the overall acceptor storage units. This may be null in
        /// [Common.StatusChangedEvent](#common.statuschangedevent) if the state has not changed. The following values
        /// are possible:
        /// 
        /// * ```ok``` - All storage units present are in a good state.
        /// * ```state``` - One or more of the storage units is in a high, full or inoperative condition. Items can still
        /// be accepted into at least one of the storage units. The status of the storage units can be obtained through the
        /// [Storage.GetStorage](#storage.getstorage) command.
        /// * ```stop``` - Due to a storage unit problem accepting is impossible. No items can be accepted because all of
        /// the storage units are in a full or in an inoperative condition.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the storage units cannot be determined.
        /// </summary>
        [DataMember(Name = "acceptor")]
        public AcceptorEnum? Acceptor { get; init; }

        public enum MediaEnum
        {
            Present,
            NotPresent,
            Jammed,
            Unknown,
            Position
        }

        /// <summary>
        /// Specifies the state of the media. This may be null in [Common.Status](#common.status) if the capability to
        /// report the state of the media is not supported by the device, otherwise the following values are possible:
        /// 
        /// * ```present``` - Media is present in the device.
        /// * ```notPresent``` - Media is not present in the device.
        /// * ```jammed``` - Media is jammed in the device.
        /// * ```unknown``` - The state of the media cannot be determined with the device in its current state.
        /// * ```position``` - Media is at one or more of the input, output and refused positions.
        /// </summary>
        [DataMember(Name = "media")]
        public MediaEnum? Media { get; init; }

        [DataMember(Name = "toner")]
        public TonerEnum? Toner { get; init; }

        [DataMember(Name = "ink")]
        public InkEnum? Ink { get; init; }

        [DataMember(Name = "frontImageScanner")]
        public FrontImageScannerEnum? FrontImageScanner { get; init; }

        [DataMember(Name = "backImageScanner")]
        public BackImageScannerEnum? BackImageScanner { get; init; }

        [DataMember(Name = "mICRReader")]
        public MicrReaderEnum? MICRReader { get; init; }

        public enum StackerEnum
        {
            Empty,
            NotEmpty,
            Full,
            Inoperative,
            Unknown
        }

        /// <summary>
        /// Supplies the state of the stacker (also known as an escrow). The stacker is where the media items are held
        /// while the application decides what to do with them. This may be null in [Common.Status](#common.status) if
        /// the physical device has no stacker or the capability to report the status of the stacker is not supported by
        /// the device, otherwise the following values are possible:
        /// 
        /// * ```empty``` - The stacker is empty.
        /// * ```notEmpty``` - The stacker is not empty.
        /// * ```full``` - The stacker is full. This state is set if the number of media items on the stacker has
        /// reached [maxMediaOnStacker](#common.capabilities.completion.description.check.maxmediaonstacker)
        /// or some physical limit has been reached.
        /// * ```inoperative``` - The stacker is inoperative.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the stacker cannot be determined.
        /// </summary>
        [DataMember(Name = "stacker")]
        public StackerEnum? Stacker { get; init; }

        public enum RebuncherEnum
        {
            Empty,
            NotEmpty,
            Full,
            Inoperative,
            Unknown
        }

        /// <summary>
        /// Supplies the state of the re-buncher (return stacker). The re-buncher is where media items are re-bunched
        /// ready for return to the customer. This may be null in [Common.Status](#common.status) if the physical device
        /// has no re-buncher or the capability to report the status of the re-buncher is not supported by the device,
        /// otherwise the following values are possible:
        /// 
        /// * ```empty``` - The re-buncher is empty.
        /// * ```notEmpty``` - The re-buncher is not empty.
        /// * ```full``` - The re-buncher is full. This state is set if the number of media items on the re-buncher
        /// has reached its physical limit.
        /// * ```inoperative``` - The re-buncher is inoperative.
        /// * ```unknown``` - Due to a hardware error or other condition, the state of the re-buncher cannot be determined.
        /// </summary>
        [DataMember(Name = "rebuncher")]
        public RebuncherEnum? Rebuncher { get; init; }

        [DataMember(Name = "mediaFeeder")]
        public MediaFeederEnum? MediaFeeder { get; init; }

        [DataContract]
        public sealed class PositionsClass
        {
            public PositionsClass(PositionStatusClass Input = null, PositionStatusClass Output = null, PositionStatusClass Refused = null)
            {
                this.Input = Input;
                this.Output = Output;
                this.Refused = Refused;
            }

            /// <summary>
            /// Specifies the status of the input position. This may be null in
            /// [Common.StatusChangedEvent](#common.statuschangedevent) if no states have changed for the position.
            /// </summary>
            [DataMember(Name = "input")]
            public PositionStatusClass Input { get; init; }

            /// <summary>
            /// Specifies the status of the output position. This may be null in
            /// [Common.StatusChangedEvent](#common.statuschangedevent) if no states have changed for the position.
            /// </summary>
            [DataMember(Name = "output")]
            public PositionStatusClass Output { get; init; }

            /// <summary>
            /// Specifies the status of the refused position. This may be null in
            /// [Common.StatusChangedEvent](#common.statuschangedevent) if no states have changed for the position.
            /// </summary>
            [DataMember(Name = "refused")]
            public PositionStatusClass Refused { get; init; }

        }

        /// <summary>
        /// Specifies the status of the input, output and refused positions. This may be null in
        /// [Common.StatusChangedEvent](#common.statuschangedevent) if no position states have changed.
        /// </summary>
        [DataMember(Name = "positions")]
        public PositionsClass Positions { get; init; }

    }


    [DataContract]
    public sealed class PrintsizeClass
    {
        public PrintsizeClass(int? Rows = null, int? Cols = null)
        {
            this.Rows = Rows;
            this.Cols = Cols;
        }

        /// <summary>
        /// Specifies the maximum number of rows of text that the device can print on the media
        /// item. This value is 1 for single line printers.
        /// </summary>
        [DataMember(Name = "rows")]
        [DataTypes(Minimum = 0)]
        public int? Rows { get; init; }

        /// <summary>
        /// Specifies the maximum number of characters that can be printed on a row.
        /// </summary>
        [DataMember(Name = "cols")]
        [DataTypes(Minimum = 0)]
        public int? Cols { get; init; }

    }


    [DataContract]
    public sealed class ImageCapabilitiesClass
    {
        public ImageCapabilitiesClass(ColorFormatClass ColorFormat = null, ScanColorClass ScanColor = null, DefaultScanColorEnum? DefaultScanColor = null)
        {
            this.ColorFormat = ColorFormat;
            this.ScanColor = ScanColor;
            this.DefaultScanColor = DefaultScanColor;
        }

        [DataContract]
        public sealed class ColorFormatClass
        {
            public ColorFormatClass(bool? Binary = null, bool? GrayScale = null, bool? Full = null)
            {
                this.Binary = Binary;
                this.GrayScale = GrayScale;
                this.Full = Full;
            }

            /// <summary>
            /// The device can return scanned images in binary.
            /// </summary>
            [DataMember(Name = "binary")]
            public bool? Binary { get; init; }

            /// <summary>
            /// The device can return scanned images in gray scale.
            /// </summary>
            [DataMember(Name = "grayScale")]
            public bool? GrayScale { get; init; }

            /// <summary>
            /// The device can return scanned images in full color.
            /// </summary>
            [DataMember(Name = "full")]
            public bool? Full { get; init; }

        }

        /// <summary>
        /// Specifies the image color formats supported by this device, as a combination of these properties:
        /// </summary>
        [DataMember(Name = "colorFormat")]
        public ColorFormatClass ColorFormat { get; init; }

        [DataContract]
        public sealed class ScanColorClass
        {
            public ScanColorClass(bool? Red = null, bool? Green = null, bool? Blue = null, bool? Yellow = null, bool? White = null, bool? InfraRed = null, bool? UltraViolet = null)
            {
                this.Red = Red;
                this.Green = Green;
                this.Blue = Blue;
                this.Yellow = Yellow;
                this.White = White;
                this.InfraRed = InfraRed;
                this.UltraViolet = UltraViolet;
            }

            /// <summary>
            /// The device can return images scanned with red light.
            /// </summary>
            [DataMember(Name = "red")]
            public bool? Red { get; init; }

            /// <summary>
            /// The device can return images scanned with green light.
            /// </summary>
            [DataMember(Name = "green")]
            public bool? Green { get; init; }

            /// <summary>
            /// The device can return images scanned with blue light.
            /// </summary>
            [DataMember(Name = "blue")]
            public bool? Blue { get; init; }

            /// <summary>
            /// The device can return images scanned with yellow light.
            /// </summary>
            [DataMember(Name = "yellow")]
            public bool? Yellow { get; init; }

            /// <summary>
            /// The device can return images scanned with white light.
            /// </summary>
            [DataMember(Name = "white")]
            public bool? White { get; init; }

            /// <summary>
            /// The device can return images scanned with infrared light.
            /// </summary>
            [DataMember(Name = "infraRed")]
            public bool? InfraRed { get; init; }

            /// <summary>
            /// The device can return images scanned with ultraviolet light.
            /// </summary>
            [DataMember(Name = "ultraViolet")]
            public bool? UltraViolet { get; init; }

        }

        /// <summary>
        /// Specifies the image scan colors supported by this device and individually controllable by the
        /// application. Scan colors are used to enhance the scanning results on colored scan media.
        /// This value is specified as a combination of these properties:
        /// </summary>
        [DataMember(Name = "scanColor")]
        public ScanColorClass ScanColor { get; init; }

        public enum DefaultScanColorEnum
        {
            Red,
            Green,
            Blue,
            Yellow,
            White,
            InfraRed,
            UltraViolet
        }

        /// <summary>
        /// Specifies the default image color format used by this device (i.e. when not explicitly set).
        /// The following values are possible:
        /// 
        /// * ```red``` - The default color is red light.
        /// * ```green``` - The default color is green light.
        /// * ```blue``` - The default color is blue light.
        /// * ```yellow``` - The default color is yellow light.
        /// * ```white``` - The default color is white light.
        /// * ```infraRed``` - The default color is infrared light.
        /// * ```ultraViolet``` - The default color is ultraviolet light.
        /// </summary>
        [DataMember(Name = "defaultScanColor")]
        public DefaultScanColorEnum? DefaultScanColor { get; init; }

    }


    [DataContract]
    public sealed class PositionCapabilitiesClass
    {
        public PositionCapabilitiesClass(bool? ItemsTakenSensor = null, bool? ItemsInsertedSensor = null, RetractAreasClass RetractAreas = null)
        {
            this.ItemsTakenSensor = ItemsTakenSensor;
            this.ItemsInsertedSensor = ItemsInsertedSensor;
            this.RetractAreas = RetractAreas;
        }

        /// <summary>
        /// Specifies whether or not the described position can detect when items at the exit position are taken by the
        /// user. If true the Service generates an accompanying [MediaTakenEvent](#check.mediatakenevent).
        /// If false this event is not generated. This relates to output and refused positions, so will
        /// be null for input positions.
        /// </summary>
        [DataMember(Name = "itemsTakenSensor")]
        public bool? ItemsTakenSensor { get; init; }

        /// <summary>
        /// Specifies whether the described position has the ability to detect when items have been inserted by the user.
        /// If true the Service generates an accompanying [MediaInsertedEvent](#check.mediainsertedevent).
        /// If false this event is not generated. This relates to all input positions, so will always be
        /// null for input positions.
        /// </summary>
        [DataMember(Name = "itemsInsertedSensor")]
        public bool? ItemsInsertedSensor { get; init; }

        [DataContract]
        public sealed class RetractAreasClass
        {
            public RetractAreasClass(bool? RetractBin = null, bool? Transport = null, bool? Stacker = null, bool? Rebuncher = null)
            {
                this.RetractBin = RetractBin;
                this.Transport = Transport;
                this.Stacker = Stacker;
                this.Rebuncher = Rebuncher;
            }

            /// <summary>
            /// Can retract items in this position to a retract storage unit.
            /// </summary>
            [DataMember(Name = "retractBin")]
            public bool? RetractBin { get; init; }

            /// <summary>
            /// Can retract items in this position to the transport.
            /// </summary>
            [DataMember(Name = "transport")]
            public bool? Transport { get; init; }

            /// <summary>
            /// Can retract items in this position to the stacker.
            /// </summary>
            [DataMember(Name = "stacker")]
            public bool? Stacker { get; init; }

            /// <summary>
            /// Can retract items in this position to the re-buncher.
            /// </summary>
            [DataMember(Name = "rebuncher")]
            public bool? Rebuncher { get; init; }

        }

        /// <summary>
        /// Specifies the areas to which items may be retracted from this position. May be null if items can not
        /// be retracted from this position.
        /// </summary>
        [DataMember(Name = "retractAreas")]
        public RetractAreasClass RetractAreas { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(TypeEnum? Type = null, int? MaxMediaOnStacker = null, PrintsizeClass PrintSize = null, bool? Stamp = null, bool? Rescan = null, bool? PresentControl = null, bool? ApplicationRefuse = null, RetractLocationClass RetractLocation = null, ResetControlClass ResetControl = null, ImageTypeClass ImageType = null, ImageCapabilitiesClass FrontImage = null, ImageCapabilitiesClass BackImage = null, CodelineFormatClass CodelineFormat = null, DataSourceClass DataSource = null, InsertOrientationClass InsertOrientation = null, PositionsClass Positions = null, bool? ImageAfterEndorse = null, ReturnedItemsProcessingClass ReturnedItemsProcessing = null, PrintsizeClass PrintSizeFront = null)
        {
            this.Type = Type;
            this.MaxMediaOnStacker = MaxMediaOnStacker;
            this.PrintSize = PrintSize;
            this.Stamp = Stamp;
            this.Rescan = Rescan;
            this.PresentControl = PresentControl;
            this.ApplicationRefuse = ApplicationRefuse;
            this.RetractLocation = RetractLocation;
            this.ResetControl = ResetControl;
            this.ImageType = ImageType;
            this.FrontImage = FrontImage;
            this.BackImage = BackImage;
            this.CodelineFormat = CodelineFormat;
            this.DataSource = DataSource;
            this.InsertOrientation = InsertOrientation;
            this.Positions = Positions;
            this.ImageAfterEndorse = ImageAfterEndorse;
            this.ReturnedItemsProcessing = ReturnedItemsProcessing;
            this.PrintSizeFront = PrintSizeFront;
        }

        public enum TypeEnum
        {
            SingleMediaInput,
            BunchMediaInput
        }

        /// <summary>
        /// Specifies the type of the physical device. The following values are possible:
        /// 
        /// * ```singleMediaInput``` - Device accepts a single media item from the customer.
        /// * ```bunchMediaInput``` - Device accepts a bunch of media items from the customer.
        /// </summary>
        [DataMember(Name = "type")]
        public TypeEnum? Type { get; init; }

        /// <summary>
        /// Specifies the maximum number of media items that the stacker can hold (zero if the device does not have a
        /// stacker). If the device has a bunch media input capability and the stacker is not present or has a capacity
        /// of one then the application must process each item inserted sequentially as described in section Multi-Feed
        /// Devices without a Stacker.
        /// </summary>
        [DataMember(Name = "maxMediaOnStacker")]
        [DataTypes(Minimum = 0)]
        public int? MaxMediaOnStacker { get; init; }

        /// <summary>
        /// Specifies the maximum print capabilities on the back side of the check, null if device has no
        /// back printing capabilities. If the
        /// media item is inserted in one of the orientations specified in *insertOrientation*, the device
        /// will print on the back side of the media. If the media item is inserted in a different orientation to those
        /// specified in *insertOrientation* then printing may occur on the front side, upside down or both.
        /// </summary>
        [DataMember(Name = "printSize")]
        public PrintsizeClass PrintSize { get; init; }

        /// <summary>
        /// Specifies whether the device has stamping capabilities. If the media item is inserted in one of the
        /// orientations specified in *insertOrientation*, the device will stamp on the front side of the media.
        /// If the media item is inserted in a different orientation to those specified in *insertOrientation* then
        /// stamping may occur on the back, upside down or both.
        /// </summary>
        [DataMember(Name = "stamp")]
        public bool? Stamp { get; init; }

        /// <summary>
        /// Specifies whether the device has the capability to either physically rescan media items after
        /// they have been inserted into the device or is able to generate any image supported by the device during the
        /// [ReadImage](#check.readimage) command (regardless of the images requested during the
        /// [MediaIn](#check.mediain) command). If true then the item can be rescanned or the images can be
        /// generated using the parameters passed in the *ReadImage* command. If false then all images required
        /// (various color, file format, bit depth) must be gathered during execution of the *MediaIn* command.
        /// </summary>
        [DataMember(Name = "rescan")]
        public bool? Rescan { get; init; }

        /// <summary>
        /// Specifies how the presenting of media items is controlled during the
        /// [MediaInEnd](#check.mediainend) and [MediaInRollback](#check.mediainrollback) commands.
        /// If true the presenting is controlled implicitly by the Service. If false the presenting must
        /// be controlled explicitly by the application using the [PresentMedia](#check.presentmedia) command.
        /// This applies to all positions.
        /// </summary>
        [DataMember(Name = "presentControl")]
        public bool? PresentControl { get; init; }

        /// <summary>
        /// Specifies if the Device supports the [MediaIn](#check.mediain) command mode where the
        /// application decides to accept or refuse each media item that has successfully been accepted by the device.
        /// If true then the Service supports this mode. If false then the Service does not
        /// support this mode (or the device does not have a stacker).
        /// </summary>
        [DataMember(Name = "applicationRefuse")]
        public bool? ApplicationRefuse { get; init; }

        [DataContract]
        public sealed class RetractLocationClass
        {
            public RetractLocationClass(bool? Storage = null, bool? Transport = null, bool? Stacker = null, bool? Rebuncher = null)
            {
                this.Storage = Storage;
                this.Transport = Transport;
                this.Stacker = Stacker;
                this.Rebuncher = Rebuncher;
            }

            /// <summary>
            /// Retract the media to a storage unit.
            /// </summary>
            [DataMember(Name = "storage")]
            public bool? Storage { get; init; }

            /// <summary>
            /// Retract the media to the transport.
            /// </summary>
            [DataMember(Name = "transport")]
            public bool? Transport { get; init; }

            /// <summary>
            /// Retract the media to the stacker.
            /// </summary>
            [DataMember(Name = "stacker")]
            public bool? Stacker { get; init; }

            /// <summary>
            /// Retract the media to the re-buncher.
            /// </summary>
            [DataMember(Name = "rebuncher")]
            public bool? Rebuncher { get; init; }

        }

        /// <summary>
        /// Specifies the locations to which the media can be retracted using the [Check.RetractMedia](#check.retractmedia) command,
        /// as a combination of these properties. May be null if not supported:
        /// </summary>
        [DataMember(Name = "retractLocation")]
        public RetractLocationClass RetractLocation { get; init; }

        [DataContract]
        public sealed class ResetControlClass
        {
            public ResetControlClass(bool? Eject = null, bool? StorageUnit = null, bool? Transport = null, bool? Rebuncher = null)
            {
                this.Eject = Eject;
                this.StorageUnit = StorageUnit;
                this.Transport = Transport;
                this.Rebuncher = Rebuncher;
            }

            /// <summary>
            /// Eject the media.
            /// </summary>
            [DataMember(Name = "eject")]
            public bool? Eject { get; init; }

            /// <summary>
            /// Retract the media to retract storage unit.
            /// </summary>
            [DataMember(Name = "storageUnit")]
            public bool? StorageUnit { get; init; }

            /// <summary>
            /// Retract the media to the transport.
            /// </summary>
            [DataMember(Name = "transport")]
            public bool? Transport { get; init; }

            /// <summary>
            /// Retract the media to the re-buncher.
            /// </summary>
            [DataMember(Name = "rebuncher")]
            public bool? Rebuncher { get; init; }

        }

        /// <summary>
        /// Specifies the manner in which the media can be handled on [Reset](#check.reset), as a combination
        /// of these properties. May be null if the command is not supported:
        /// </summary>
        [DataMember(Name = "resetControl")]
        public ResetControlClass ResetControl { get; init; }

        [DataContract]
        public sealed class ImageTypeClass
        {
            public ImageTypeClass(bool? Tif = null, bool? Wmf = null, bool? Bmp = null, bool? Jpg = null)
            {
                this.Tif = Tif;
                this.Wmf = Wmf;
                this.Bmp = Bmp;
                this.Jpg = Jpg;
            }

            /// <summary>
            /// The device can return scanned images in TIFF 6.0 format.
            /// </summary>
            [DataMember(Name = "tif")]
            public bool? Tif { get; init; }

            /// <summary>
            /// The device can return scanned images in WMF (Windows Metafile) format.
            /// </summary>
            [DataMember(Name = "wmf")]
            public bool? Wmf { get; init; }

            /// <summary>
            /// The device can return scanned images in windows BMP format.
            /// </summary>
            [DataMember(Name = "bmp")]
            public bool? Bmp { get; init; }

            /// <summary>
            /// The device can return scanned images in JPG format.
            /// </summary>
            [DataMember(Name = "jpg")]
            public bool? Jpg { get; init; }

        }

        /// <summary>
        /// Specifies the image format supported by this device, as a combination of these properties. May be null
        /// if not supported.
        /// </summary>
        [DataMember(Name = "imageType")]
        public ImageTypeClass ImageType { get; init; }

        /// <summary>
        /// Specifies the capabilities of the front image supported by this device. May be null if front images are not
        /// supported.
        /// </summary>
        [DataMember(Name = "frontImage")]
        public ImageCapabilitiesClass FrontImage { get; init; }

        /// <summary>
        /// Specifies the capabilities of the back image supported by this device. May be null if back images are not
        /// supported.
        /// </summary>
        [DataMember(Name = "backImage")]
        public ImageCapabilitiesClass BackImage { get; init; }

        [DataContract]
        public sealed class CodelineFormatClass
        {
            public CodelineFormatClass(bool? Cmc7 = null, bool? E13b = null, bool? Ocr = null, bool? Ocra = null, bool? Ocrb = null)
            {
                this.Cmc7 = Cmc7;
                this.E13b = E13b;
                this.Ocr = Ocr;
                this.Ocra = Ocra;
                this.Ocrb = Ocrb;
            }

            /// <summary>
            /// The device can read MICR CMC7 [[Ref. check-4](#ref-check-4)] code lines.
            /// </summary>
            [DataMember(Name = "cmc7")]
            public bool? Cmc7 { get; init; }

            /// <summary>
            /// The device can read MICR E13B [[Ref. check-3](#ref-check-3)] code lines.
            /// </summary>
            [DataMember(Name = "e13b")]
            public bool? E13b { get; init; }

            /// <summary>
            /// The device can read code lines using Optical Character Recognition.
            /// The default or pre-configured OCR font will be used.
            /// </summary>
            [DataMember(Name = "ocr")]
            public bool? Ocr { get; init; }

            /// <summary>
            /// The device can read code lines using Optical Character Recognition font A.
            /// The ASCII codes will conform to [[Ref. check-1](#ref-check-1)].
            /// </summary>
            [DataMember(Name = "ocra")]
            public bool? Ocra { get; init; }

            /// <summary>
            /// The device can read code lines using Optical Character Recognition font B.
            /// The ASCII codes will conform to [[Ref. check-2](#ref-check-2)].
            /// </summary>
            [DataMember(Name = "ocrb")]
            public bool? Ocrb { get; init; }

        }

        /// <summary>
        /// Specifies the code line formats supported by this device, as a combination of these properties:
        /// </summary>
        [DataMember(Name = "codelineFormat")]
        public CodelineFormatClass CodelineFormat { get; init; }

        [DataContract]
        public sealed class DataSourceClass
        {
            public DataSourceClass(bool? ImageFront = null, bool? ImageBack = null, bool? CodeLine = null)
            {
                this.ImageFront = ImageFront;
                this.ImageBack = ImageBack;
                this.CodeLine = CodeLine;
            }

            /// <summary>
            /// The device can scan the front image of the document.
            /// </summary>
            [DataMember(Name = "imageFront")]
            public bool? ImageFront { get; init; }

            /// <summary>
            /// The device can scan the back image of the document.
            /// </summary>
            [DataMember(Name = "imageBack")]
            public bool? ImageBack { get; init; }

            /// <summary>
            /// The device can recognize the code line.
            /// </summary>
            [DataMember(Name = "codeLine")]
            public bool? CodeLine { get; init; }

        }

        /// <summary>
        /// Specifies the reading/imaging capabilities supported by this device, as a combination of these properties:
        /// </summary>
        [DataMember(Name = "dataSource")]
        public DataSourceClass DataSource { get; init; }

        [DataContract]
        public sealed class InsertOrientationClass
        {
            public InsertOrientationClass(bool? CodeLineRight = null, bool? CodeLineLeft = null, bool? CodeLineBottom = null, bool? CodeLineTop = null, bool? FaceUp = null, bool? FaceDown = null)
            {
                this.CodeLineRight = CodeLineRight;
                this.CodeLineLeft = CodeLineLeft;
                this.CodeLineBottom = CodeLineBottom;
                this.CodeLineTop = CodeLineTop;
                this.FaceUp = FaceUp;
                this.FaceDown = FaceDown;
            }

            /// <summary>
            /// The media item should be inserted short edge first with the code line to the right.
            /// </summary>
            [DataMember(Name = "codeLineRight")]
            public bool? CodeLineRight { get; init; }

            /// <summary>
            /// The media item should be inserted short edge first with the code line to the left.
            /// </summary>
            [DataMember(Name = "codeLineLeft")]
            public bool? CodeLineLeft { get; init; }

            /// <summary>
            /// The media item should be inserted long edge first with the code line to the bottom.
            /// </summary>
            [DataMember(Name = "codeLineBottom")]
            public bool? CodeLineBottom { get; init; }

            /// <summary>
            /// The media item should be inserted long edge first with the code line to the top.
            /// </summary>
            [DataMember(Name = "codeLineTop")]
            public bool? CodeLineTop { get; init; }

            /// <summary>
            /// The media item should be inserted with the front of the media item facing up.
            /// </summary>
            [DataMember(Name = "faceUp")]
            public bool? FaceUp { get; init; }

            /// <summary>
            /// The media item should be inserted with the front of the media item facing down.
            /// </summary>
            [DataMember(Name = "faceDown")]
            public bool? FaceDown { get; init; }

        }

        /// <summary>
        /// Specifies the media item insertion orientations supported by the Device such that hardware
        /// features such as MICR reading, endorsing and stamping will be aligned with the correct edges and sides
        /// of the media item. Devices may still return code lines and images even if one of these orientations is not
        /// used during media insertion. If the media items are inserted in one of the orientations defined in this
        /// capability then any printing or stamping will be on the correct side of the media item. If the media is
        /// inserted in a different orientation then any printing or stamping may be on the wrong side, upside down
        /// or both. This value is reported based on the customer's perspective.
        /// This value is a combination of these properties:
        /// </summary>
        [DataMember(Name = "insertOrientation")]
        public InsertOrientationClass InsertOrientation { get; init; }

        [DataContract]
        public sealed class PositionsClass
        {
            public PositionsClass(PositionCapabilitiesClass Input = null, PositionCapabilitiesClass Output = null, PositionCapabilitiesClass Refused = null)
            {
                this.Input = Input;
                this.Output = Output;
                this.Refused = Refused;
            }

            /// <summary>
            /// Structure that specifies the capabilities of the input position.
            /// </summary>
            [DataMember(Name = "input")]
            public PositionCapabilitiesClass Input { get; init; }

            /// <summary>
            /// Structure that specifies the capabilities of the output position.
            /// </summary>
            [DataMember(Name = "output")]
            public PositionCapabilitiesClass Output { get; init; }

            /// <summary>
            /// Structure that specifies the capabilities of the refused position.
            /// </summary>
            [DataMember(Name = "refused")]
            public PositionCapabilitiesClass Refused { get; init; }

        }

        /// <summary>
        /// Specifies the capabilities of up to three logical position types.
        /// </summary>
        [DataMember(Name = "positions")]
        public PositionsClass Positions { get; init; }

        /// <summary>
        /// Specifies whether the device can generate an image after text is printed on the media item. If true then
        /// the generation of the image can be specified using the [SetMediaParameters](#check.setmediaparameters)
        /// command. If false, this functionality is not available. This capability applies to media items whose
        /// destination is a storage unit; the *returnedItemsProcessing* capability indicates whether this functionality
        /// is supported for media items that are to be returned to the customer.
        /// </summary>
        [DataMember(Name = "imageAfterEndorse")]
        public bool? ImageAfterEndorse { get; init; }

        [DataContract]
        public sealed class ReturnedItemsProcessingClass
        {
            public ReturnedItemsProcessingClass(bool? Endorse = null, bool? EndorseImage = null)
            {
                this.Endorse = Endorse;
                this.EndorseImage = EndorseImage;
            }

            /// <summary>
            /// This device can endorse a media item to be returned to the customer; the endorsement is specified
            /// using the [SetMediaParameters](#check.setmediaparameters) command.
            /// </summary>
            [DataMember(Name = "endorse")]
            public bool? Endorse { get; init; }

            /// <summary>
            /// This device can generate an image of a media item to be returned to the customer after it has been
            /// endorsed; the request for the image is specified using the [SetMediaParameters](#check.setmediaparameters) command.
            /// </summary>
            [DataMember(Name = "endorseImage")]
            public bool? EndorseImage { get; init; }

        }

        /// <summary>
        /// Specifies the processing that this device supports for media items that are identified to be returned to
        /// the customer using the [SetMediaParameters](#check.setmediaparameters) command, as a combination of these
        /// properties or null if none apply:
        /// </summary>
        [DataMember(Name = "returnedItemsProcessing")]
        public ReturnedItemsProcessingClass ReturnedItemsProcessing { get; init; }

        /// <summary>
        /// Reports the printing capabilities of the device on the front side of the check, null if device has no
        /// front printing capabilities.
        /// If the media item is inserted in one of the orientations specified in *insertOrientation*, the device
        /// will print on the front side of the media. If the media item is inserted in a different orientation to those
        /// specified in *insertOrientation* then printing may occur on the back side, upside down or both.
        /// </summary>
        [DataMember(Name = "printSizeFront")]
        public PrintsizeClass PrintSizeFront { get; init; }

    }


    [DataContract]
    public sealed class StorageTypesClass
    {
        public StorageTypesClass(bool? MediaIn = null, bool? Retract = null)
        {
            this.MediaIn = MediaIn;
            this.Retract = Retract;
        }

        /// <summary>
        /// The unit can accept items during Media In transactions. May be null in command data and events
        /// if not changing.
        /// <example>true</example>
        /// </summary>
        [DataMember(Name = "mediaIn")]
        public bool? MediaIn { get; init; }

        /// <summary>
        /// Retract unit. Items can be retracted into this unit using [Check.RetractMedia](#check.retractmedia).
        /// May be null in command data and events
        /// if not changing.
        /// </summary>
        [DataMember(Name = "retract")]
        public bool? Retract { get; init; }

    }


    [DataContract]
    public sealed class StorageSensorsClass
    {
        public StorageSensorsClass(bool? Empty = null, bool? High = null, bool? Full = null)
        {
            this.Empty = Empty;
            this.High = High;
            this.Full = Full;
        }

        /// <summary>
        /// The unit contains a hardware sensor which reports when the unit is empty. May be null in command data and events
        /// if not changing.
        /// </summary>
        [DataMember(Name = "empty")]
        public bool? Empty { get; init; }

        /// <summary>
        /// The unit contains a hardware sensor which reports when the unit is nearly full. May be null in command data and events
        /// if not changing.
        /// </summary>
        [DataMember(Name = "high")]
        public bool? High { get; init; }

        /// <summary>
        /// The unit contains a hardware sensor which reports when the unit is full. May be null in command data and events
        /// if not changing.
        /// </summary>
        [DataMember(Name = "full")]
        public bool? Full { get; init; }

    }


    [DataContract]
    public sealed class StorageCapabilitiesClass
    {
        public StorageCapabilitiesClass(StorageTypesClass Types = null, StorageSensorsClass Sensors = null)
        {
            this.Types = Types;
            this.Sensors = Sensors;
        }

        [DataMember(Name = "types")]
        public StorageTypesClass Types { get; init; }

        [DataMember(Name = "sensors")]
        public StorageSensorsClass Sensors { get; init; }

    }


    [DataContract]
    public sealed class StorageConfigurationClass
    {
        public StorageConfigurationClass(StorageTypesClass Types = null, string BinID = null, int? HighThreshold = null, int? RetractHighThreshold = null)
        {
            this.Types = Types;
            this.BinID = BinID;
            this.HighThreshold = HighThreshold;
            this.RetractHighThreshold = RetractHighThreshold;
        }

        [DataMember(Name = "types")]
        public StorageTypesClass Types { get; init; }

        /// <summary>
        /// An application defined Storage Unit Identifier. This may be null in events if not changing.
        /// <example>My check bin</example>
        /// </summary>
        [DataMember(Name = "binID")]
        public string BinID { get; init; }

        /// <summary>
        /// If specified,
        /// [replenishmentStatus](#storage.getstorage.completion.properties.storage.unit1.check.status.replenishmentstatus)
        /// is set to *high* if the total number of items in the storage unit is greater than this number.
        /// May be null in command data and events if not being modified.
        /// <example>500</example>
        /// </summary>
        [DataMember(Name = "highThreshold")]
        [DataTypes(Minimum = 1)]
        public int? HighThreshold { get; init; }

        /// <summary>
        /// If specified and the storage unit is configured as *retract*,
        /// [replenishmentStatus](#storage.getstorage.completion.properties.storage.unit1.check.status.replenishmentstatus)
        /// is set to *high* if the total number of retract operations in the storage unit is greater than this number.
        /// May be null in command data and events if not being modified.
        /// <example>5</example>
        /// </summary>
        [DataMember(Name = "retractHighThreshold")]
        [DataTypes(Minimum = 0)]
        public int? RetractHighThreshold { get; init; }

    }


    public enum ReplenishmentStatusEnum
    {
        Ok,
        Full,
        High,
        Empty
    }


    [DataContract]
    public sealed class StorageStatusClass
    {
        public StorageStatusClass(int? Index = null, InitialClass Initial = null, InClass In = null, ReplenishmentStatusEnum? ReplenishmentStatus = null)
        {
            this.Index = Index;
            this.Initial = Initial;
            this.In = In;
            this.ReplenishmentStatus = ReplenishmentStatus;
        }

        /// <summary>
        /// Assigned by the Service. Will be a unique number which can be used to determine
        /// *usBinNumber* in XFS 3.x migration. This can change as storage units are added and removed
        /// from the storage collection.
        /// <example>4</example>
        /// </summary>
        [DataMember(Name = "index")]
        [DataTypes(Minimum = 1)]
        public int? Index { get; init; }

        [DataContract]
        public sealed class InitialClass
        {
            public InitialClass(int? MediaInCount = null, int? Count = null, int? RetractOperations = null)
            {
                this.MediaInCount = MediaInCount;
                this.Count = Count;
                this.RetractOperations = RetractOperations;
            }

            /// <summary>
            /// Count of items added to the storage unit due to Check operations. If the number of items is not counted
            /// this is not reported and *retractOperations* is incremented as items are added to the unit. May be null
            /// in command data and events if not changing.
            /// <example>100</example>
            /// </summary>
            [DataMember(Name = "mediaInCount")]
            [DataTypes(Minimum = 0)]
            public int? MediaInCount { get; init; }

            /// <summary>
            /// Total number of items added to the storage unit due to any operations. If the number of items is not counted
            /// this is not reported and *retractOperations* is incremented as items are added to the unit. May be null
            /// in command data and events if not changing.
            /// <example>150</example>
            /// </summary>
            [DataMember(Name = "count")]
            [DataTypes(Minimum = 0)]
            public int? Count { get; init; }

            /// <summary>
            /// Total number of operations which resulted in items being retracted to the storage unit. May be null
            /// in command data and events if not changing.
            /// <example>15</example>
            /// </summary>
            [DataMember(Name = "retractOperations")]
            [DataTypes(Minimum = 0)]
            public int? RetractOperations { get; init; }

        }

        /// <summary>
        /// The check related counts as set at the last replenishment. May be null in events where status has not changed.
        /// </summary>
        [DataMember(Name = "initial")]
        public InitialClass Initial { get; init; }

        [DataContract]
        public sealed class InClass
        {
            public InClass(int? MediaInCount = null, int? Count = null, int? RetractOperations = null)
            {
                this.MediaInCount = MediaInCount;
                this.Count = Count;
                this.RetractOperations = RetractOperations;
            }

            /// <summary>
            /// Count of items added to the storage unit due to Check operations. If the number of items is not counted
            /// this is not reported and *retractOperations* is incremented as items are added to the unit. May be null
            /// in command data and events if not changing.
            /// <example>100</example>
            /// </summary>
            [DataMember(Name = "mediaInCount")]
            [DataTypes(Minimum = 0)]
            public int? MediaInCount { get; init; }

            /// <summary>
            /// Total number of items added to the storage unit due to any operations. If the number of items is not counted
            /// this is not reported and *retractOperations* is incremented as items are added to the unit. May be null
            /// in command data and events if not changing.
            /// <example>150</example>
            /// </summary>
            [DataMember(Name = "count")]
            [DataTypes(Minimum = 0)]
            public int? Count { get; init; }

            /// <summary>
            /// Total number of operations which resulted in items being retracted to the storage unit. May be null
            /// in command data and events if not changing.
            /// <example>15</example>
            /// </summary>
            [DataMember(Name = "retractOperations")]
            [DataTypes(Minimum = 0)]
            public int? RetractOperations { get; init; }

        }

        /// <summary>
        /// The check items added to the unit since the last replenishment. May be null in events where status has not changed.
        /// </summary>
        [DataMember(Name = "in")]
        public InClass In { get; init; }

        [DataMember(Name = "replenishmentStatus")]
        public ReplenishmentStatusEnum? ReplenishmentStatus { get; init; }

    }


    [DataContract]
    public sealed class StorageClass
    {
        public StorageClass(StorageCapabilitiesClass Capabilities = null, StorageConfigurationClass Configuration = null, StorageStatusClass Status = null)
        {
            this.Capabilities = Capabilities;
            this.Configuration = Configuration;
            this.Status = Status;
        }

        [DataMember(Name = "capabilities")]
        public StorageCapabilitiesClass Capabilities { get; init; }

        [DataMember(Name = "configuration")]
        public StorageConfigurationClass Configuration { get; init; }

        [DataMember(Name = "status")]
        public StorageStatusClass Status { get; init; }

    }


    public enum MagneticReadIndicatorEnum
    {
        Micr,
        NotMicr,
        NoMicr,
        Unknown,
        NotMicrFormat,
        NotRead
    }


    public enum ImageSourceEnum
    {
        Front,
        Back
    }


    public enum ImageTypeEnum
    {
        Tif,
        Wmf,
        Bmp,
        Jpg
    }


    public enum ImageColorFormatEnum
    {
        Binary,
        GrayScale,
        Full
    }


    public enum ImageScanColorEnum
    {
        Red,
        Green,
        Blue,
        Yellow,
        White,
        InfraRed,
        UltraViolet
    }


    public enum ImageStatusEnum
    {
        Ok,
        SourceNotSupported,
        SourceMissing
    }


    [DataContract]
    public sealed class ImageDataClass
    {
        public ImageDataClass(ImageSourceEnum? ImageSource = null, ImageTypeEnum? ImageType = null, ImageColorFormatEnum? ImageColorFormat = null, ImageScanColorEnum? ImageScanColor = null, ImageStatusEnum? ImageStatus = null, List<byte> Image = null)
        {
            this.ImageSource = ImageSource;
            this.ImageType = ImageType;
            this.ImageColorFormat = ImageColorFormat;
            this.ImageScanColor = ImageScanColor;
            this.ImageStatus = ImageStatus;
            this.Image = Image;
        }

        [DataMember(Name = "imageSource")]
        public ImageSourceEnum? ImageSource { get; init; }

        [DataMember(Name = "imageType")]
        public ImageTypeEnum? ImageType { get; init; }

        [DataMember(Name = "imageColorFormat")]
        public ImageColorFormatEnum? ImageColorFormat { get; init; }

        [DataMember(Name = "imageScanColor")]
        public ImageScanColorEnum? ImageScanColor { get; init; }

        [DataMember(Name = "imageStatus")]
        public ImageStatusEnum? ImageStatus { get; init; }

        /// <summary>
        /// Base64 encoded image. May be null if no image was obtained.
        /// <example>wCAAAQgwMDAwMDAwMA==</example>
        /// </summary>
        [DataMember(Name = "image")]
        [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
        public List<byte> Image { get; init; }

    }


    [DataContract]
    public sealed class InsertOrientationClass
    {
        public InsertOrientationClass(CodelineEnum? Codeline = null, MediaEnum? Media = null)
        {
            this.Codeline = Codeline;
            this.Media = Media;
        }

        public enum CodelineEnum
        {
            Right,
            Left,
            Bottom,
            Top
        }

        /// <summary>
        /// Specifies the orientation of the code line. The following values are possible, or null if unknown.
        /// 
        /// * ```right``` - The code line is to the right.
        /// * ```left``` - The code line is to the left.
        /// * ```bottom``` - The code line is to the bottom.
        /// * ```top``` - The code line is to the top.
        /// <example>top</example>
        /// </summary>
        [DataMember(Name = "codeline")]
        public CodelineEnum? Codeline { get; init; }

        public enum MediaEnum
        {
            Up,
            Down
        }

        /// <summary>
        /// Specifies the orientation of the media. The following values are possible, or null if unknown:
        /// 
        /// * ```up``` - The front of the media (the side with the code line) is facing up.
        /// * ```down``` - The front of the media (the side with the code line) is facing down.
        /// <example>down</example>
        /// </summary>
        [DataMember(Name = "media")]
        public MediaEnum? Media { get; init; }

    }


    [DataContract]
    public sealed class MediaSizeClass
    {
        public MediaSizeClass(int? LongEdge = null, int? ShortEdge = null)
        {
            this.LongEdge = LongEdge;
            this.ShortEdge = ShortEdge;
        }

        /// <summary>
        /// Specifies the length of the long edge of the media in millimeters, or 0 if unknown.
        /// <example>205</example>
        /// </summary>
        [DataMember(Name = "longEdge")]
        [DataTypes(Minimum = 0)]
        public int? LongEdge { get; init; }

        /// <summary>
        /// Specifies the length of the short edge of the media in millimeters, or 0 if unknown.
        /// <example>103</example>
        /// </summary>
        [DataMember(Name = "shortEdge")]
        [DataTypes(Minimum = 0)]
        public int? ShortEdge { get; init; }

    }


    public enum MediaValidityEnum
    {
        Ok,
        Suspect,
        Unknown,
        NoValidation
    }


    [DataContract]
    public sealed class MediaStatusClass
    {
        public MediaStatusClass(int? MediaID = null, string MediaLocation = null, string CodelineData = null, MagneticReadIndicatorEnum? MagneticReadIndicator = null, List<ImageDataClass> Image = null, InsertOrientationClass InsertOrientation = null, MediaSizeClass MediaSize = null, MediaValidityEnum? MediaValidity = null, CustomerAccessEnum? CustomerAccess = null)
        {
            this.MediaID = MediaID;
            this.MediaLocation = MediaLocation;
            this.CodelineData = CodelineData;
            this.MagneticReadIndicator = MagneticReadIndicator;
            this.Image = Image;
            this.InsertOrientation = InsertOrientation;
            this.MediaSize = MediaSize;
            this.MediaValidity = MediaValidity;
            this.CustomerAccess = CustomerAccess;
        }

        /// <summary>
        /// Specifies the sequence number (starting from 1) of a media item.
        /// <example>4</example>
        /// </summary>
        [DataMember(Name = "mediaID")]
        [DataTypes(Minimum = 1)]
        public int? MediaID { get; init; }

        /// <summary>
        /// Specifies the location of the media item. This value can change outside of a media-in
        /// transaction as the media moves within the device. The following values are possible:
        /// 
        /// * ```device``` - The media item is inside the device in some position other than a storage unit.
        /// * ```&lt;storage unit identifier&gt;``` - The media item is in a storage unit as specified by
        ///   [identifier](#storage.getstorage.completion.properties.storage.unit1).
        /// * ```customer``` - The media item has been returned to the customer.
        /// * ```unknown``` - The media item location is unknown.
        /// <example>device</example>
        /// </summary>
        [DataMember(Name = "mediaLocation")]
        [DataTypes(Pattern = @"^device$|^customer$|^unknown$|^unit[0-9A-Za-z]+$")]
        public string MediaLocation { get; init; }

        /// <summary>
        /// Specifies the code line data. See [Code line Characters](#check.generalinformation.codelinecharacters).
        /// <example>⑈22222⑈⑆123456⑆</example>
        /// </summary>
        [DataMember(Name = "codelineData")]
        public string CodelineData { get; init; }

        [DataMember(Name = "magneticReadIndicator")]
        public MagneticReadIndicatorEnum? MagneticReadIndicator { get; init; }

        /// <summary>
        /// Array of image data. If the Device has
        /// determined the orientation of the media (i.e. *insertOrientation* is defined and not set to \\"unknown\\"),
        /// then all images returned are in the standard orientation and the images will match the image source
        /// requested by the application. This means that images will be returned with the code line at the bottom,
        /// and the image of the front and rear of the media item will be returned in the structures associated with
        /// the \\"front\\" and \\"back\\" image sources respectively.
        /// </summary>
        [DataMember(Name = "image")]
        public List<ImageDataClass> Image { get; init; }

        [DataMember(Name = "insertOrientation")]
        public InsertOrientationClass InsertOrientation { get; init; }

        [DataMember(Name = "mediaSize")]
        public MediaSizeClass MediaSize { get; init; }

        [DataMember(Name = "mediaValidity")]
        public MediaValidityEnum? MediaValidity { get; init; }

        public enum CustomerAccessEnum
        {
            Unknown,
            Customer,
            None
        }

        /// <summary>
        /// Specifies if the media item has been in customer access since it was first deposited, e.g. it has been
        /// retracted from a position with customer access. This value can change outside of a media-in transaction as
        /// the media moves within the device. The following values are possible:
        /// 
        /// * ```unknown``` - It is not known if the media item has been in a position with customer access.
        /// * ```customer``` - The media item has been in a position with customer access.
        /// * ```none``` - The media item has not been in a position with customer access.
        /// <example>customer</example>
        /// </summary>
        [DataMember(Name = "customerAccess")]
        public CustomerAccessEnum? CustomerAccess { get; init; }

    }


    public enum CodelineFormatEnum
    {
        Cmc7,
        E13b,
        Ocr,
        Ocra,
        Ocrb
    }


    [DataContract]
    public sealed class ImageRequestClass
    {
        public ImageRequestClass(ImageSourceEnum? Source = null, ImageTypeEnum? Type = null, ImageColorFormatEnum? ColorFormat = null, ImageScanColorEnum? ScanColor = null)
        {
            this.Source = Source;
            this.Type = Type;
            this.ColorFormat = ColorFormat;
            this.ScanColor = ScanColor;
        }

        [DataMember(Name = "source")]
        public ImageSourceEnum? Source { get; init; }

        [DataMember(Name = "type")]
        public ImageTypeEnum? Type { get; init; }

        [DataMember(Name = "colorFormat")]
        public ImageColorFormatEnum? ColorFormat { get; init; }

        [DataMember(Name = "scanColor")]
        public ImageScanColorEnum? ScanColor { get; init; }

    }


    [DataContract]
    public sealed class MediainClass
    {
        public MediainClass(int? MediaOnStacker = null, int? LastMedia = null, int? LastMediaOnStacker = null, MediaFeederEnum? MediaFeeder = null)
        {
            this.MediaOnStacker = MediaOnStacker;
            this.LastMedia = LastMedia;
            this.LastMediaOnStacker = LastMediaOnStacker;
            this.MediaFeeder = MediaFeeder;
        }

        /// <summary>
        /// Contains the total number of media items on the stacker (including *lastMediaOnStacker*). May be null if it is
        /// unknown or the device does not have a stacker.
        /// <example>10</example>
        /// </summary>
        [DataMember(Name = "mediaOnStacker")]
        [DataTypes(Minimum = 0)]
        public int? MediaOnStacker { get; init; }

        /// <summary>
        /// Contains the number of media items processed by this instance of the command execution. May be null if it is
        /// unknown or the device does not have a stacker.
        /// <example>5</example>
        /// </summary>
        [DataMember(Name = "lastMedia")]
        [DataTypes(Minimum = 0)]
        public int? LastMedia { get; init; }

        /// <summary>
        /// Contains the number of media items on the stacker successfully accepted by this instance of the command
        /// execution. May be null if it is unknown or the device does not have a stacker.
        /// 
        /// The number of refused media items can be determined by *lastMedia* - *lastMediaOnStacker*. This is only
        /// possible if these values contain known values, and would not be possible if a bunch of items were refused
        /// as a single entity.
        /// <example>3</example>
        /// </summary>
        [DataMember(Name = "lastMediaOnStacker")]
        [DataTypes(Minimum = 0)]
        public int? LastMediaOnStacker { get; init; }

        [DataMember(Name = "mediaFeeder")]
        public MediaFeederEnum? MediaFeeder { get; init; }

    }


    [DataContract]
    public sealed class MediaInEndDataClass
    {
        public MediaInEndDataClass(int? ItemsReturned = null, int? ItemsRefused = null, int? BunchesRefused = null, StorageClass Storage = null)
        {
            this.ItemsReturned = ItemsReturned;
            this.ItemsRefused = ItemsRefused;
            this.BunchesRefused = BunchesRefused;
            this.Storage = Storage;
        }

        /// <summary>
        /// Contains the number of media items that were returned to the customer by application selection through the
        /// [Check.SetMediaParameters](#check.setmediaparameters) command during the current transaction. This does not include
        /// items that were refused.
        /// <example>2</example>
        /// </summary>
        [DataMember(Name = "itemsReturned")]
        [DataTypes(Minimum = 0)]
        public int? ItemsReturned { get; init; }

        /// <summary>
        /// Contains the total number of items automatically returned to the customer during the execution of the whole
        /// transaction. This count does not include bunches of items which are refused as a single entity without being
        /// processed as single items.
        /// <example>3</example>
        /// </summary>
        [DataMember(Name = "itemsRefused")]
        [DataTypes(Minimum = 0)]
        public int? ItemsRefused { get; init; }

        /// <summary>
        /// Contains the total number of refused bunches of items that were automatically returned to the customer
        /// without being processed as single items.
        /// <example>1</example>
        /// </summary>
        [DataMember(Name = "bunchesRefused")]
        [DataTypes(Minimum = 0)]
        public int? BunchesRefused { get; init; }

        [DataContract]
        public sealed class StorageClass
        {
            public StorageClass(Dictionary<string, Storage.StorageUnitClass> Storage = null)
            {
                this.Storage = Storage;
            }

            /// <summary>
            /// Object containing storage unit information. The property name is the storage unit identifier.
            /// </summary>
            [DataMember(Name = "storage")]
            public Dictionary<string, Storage.StorageUnitClass> Storage { get; init; }

        }

        /// <summary>
        /// List of storage units that have taken items, and the type of items they have taken, during the current
        /// transaction. This only contains data related to the current transaction.
        /// </summary>
        [DataMember(Name = "storage")]
        public StorageClass Storage { get; init; }

    }


    public enum PositionEnum
    {
        Input,
        Refused,
        Rebuncher
    }


    [DataContract]
    public sealed class MediaDataClass
    {
        public MediaDataClass(int? MediaID = null, string CodelineData = null, MagneticReadIndicatorEnum? MagneticReadIndicator = null, List<ImageDataClass> Image = null, InsertOrientationClass InsertOrientation = null, MediaSizeClass MediaSize = null, MediaValidityEnum? MediaValidity = null)
        {
            this.MediaID = MediaID;
            this.CodelineData = CodelineData;
            this.MagneticReadIndicator = MagneticReadIndicator;
            this.Image = Image;
            this.InsertOrientation = InsertOrientation;
            this.MediaSize = MediaSize;
            this.MediaValidity = MediaValidity;
        }

        /// <summary>
        /// Specifies the sequence number (starting from 1) of a media item.
        /// <example>4</example>
        /// </summary>
        [DataMember(Name = "mediaID")]
        [DataTypes(Minimum = 1)]
        public int? MediaID { get; init; }

        /// <summary>
        /// Specifies the code line data. See [Code line Characters](#check.generalinformation.codelinecharacters).
        /// <example>⑈22222⑈⑆123456⑆</example>
        /// </summary>
        [DataMember(Name = "codelineData")]
        public string CodelineData { get; init; }

        [DataMember(Name = "magneticReadIndicator")]
        public MagneticReadIndicatorEnum? MagneticReadIndicator { get; init; }

        /// <summary>
        /// Array of image data. If the Device has
        /// determined the orientation of the media (i.e. *insertOrientation* is defined and not set to \\"unknown\\"),
        /// then all images returned are in the standard orientation and the images will match the image source
        /// requested by the application. This means that images will be returned with the code line at the bottom,
        /// and the image of the front and rear of the media item will be returned in the structures associated with
        /// the \\"front\\" and \\"back\\" image sources respectively.
        /// </summary>
        [DataMember(Name = "image")]
        public List<ImageDataClass> Image { get; init; }

        [DataMember(Name = "insertOrientation")]
        public InsertOrientationClass InsertOrientation { get; init; }

        [DataMember(Name = "mediaSize")]
        public MediaSizeClass MediaSize { get; init; }

        [DataMember(Name = "mediaValidity")]
        public MediaValidityEnum? MediaValidity { get; init; }

    }


    [DataContract]
    public sealed class StorageStatusSetClass
    {
        public StorageStatusSetClass(InitialClass Initial = null)
        {
            this.Initial = Initial;
        }

        [DataContract]
        public sealed class InitialClass
        {
            public InitialClass(int? MediaInCount = null, int? Count = null, int? RetractOperations = null)
            {
                this.MediaInCount = MediaInCount;
                this.Count = Count;
                this.RetractOperations = RetractOperations;
            }

            /// <summary>
            /// Count of items added to the storage unit due to Check operations. If the number of items is not counted
            /// this is not reported and *retractOperations* is incremented as items are added to the unit. May be null
            /// in command data and events if not changing.
            /// <example>100</example>
            /// </summary>
            [DataMember(Name = "mediaInCount")]
            [DataTypes(Minimum = 0)]
            public int? MediaInCount { get; init; }

            /// <summary>
            /// Total number of items added to the storage unit due to any operations. If the number of items is not counted
            /// this is not reported and *retractOperations* is incremented as items are added to the unit. May be null
            /// in command data and events if not changing.
            /// <example>150</example>
            /// </summary>
            [DataMember(Name = "count")]
            [DataTypes(Minimum = 0)]
            public int? Count { get; init; }

            /// <summary>
            /// Total number of operations which resulted in items being retracted to the storage unit. May be null
            /// in command data and events if not changing.
            /// <example>15</example>
            /// </summary>
            [DataMember(Name = "retractOperations")]
            [DataTypes(Minimum = 0)]
            public int? RetractOperations { get; init; }

        }

        /// <summary>
        /// The check related counts as set at the last replenishment.
        /// </summary>
        [DataMember(Name = "initial")]
        public InitialClass Initial { get; init; }

    }


    [DataContract]
    public sealed class StorageSetClass
    {
        public StorageSetClass(StorageConfigurationClass Configuration = null, StorageStatusSetClass Status = null)
        {
            this.Configuration = Configuration;
            this.Status = Status;
        }

        [DataMember(Name = "configuration")]
        public StorageConfigurationClass Configuration { get; init; }

        [DataMember(Name = "status")]
        public StorageStatusSetClass Status { get; init; }

    }


}
