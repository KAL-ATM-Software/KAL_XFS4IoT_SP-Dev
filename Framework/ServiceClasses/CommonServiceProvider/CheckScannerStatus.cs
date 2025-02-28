/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using XFS4IoTServer;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// Check scanner status class
    /// </summary>
    public sealed class CheckScannerStatusClass(
        CheckScannerStatusClass.AcceptorEnum Acceptor,
        CheckScannerStatusClass.MediaEnum Media,
        CheckScannerStatusClass.TonerEnum Toner,
        CheckScannerStatusClass.InkEnum Ink,
        CheckScannerStatusClass.ImageScannerEnum FrontImageScanner,
        CheckScannerStatusClass.ImageScannerEnum BackImageScanner,
        CheckScannerStatusClass.ImageScannerEnum MICRReader,
        CheckScannerStatusClass.StackerEnum Stacker,
        CheckScannerStatusClass.ReBuncherEnum ReBuncher,
        CheckScannerStatusClass.MediaFeederEnum MediaFeeder,
        Dictionary<CheckScannerCapabilitiesClass.PositionEnum, CheckScannerStatusClass.PositionStatusClass> Positions) : StatusBase
    {
        public sealed class PositionStatusClass : StatusBase
        {
            public PositionStatusClass(ShutterEnum Shutter,
                                       PositionStatusEnum PositionStatus,
                                       TransportEnum Transport,
                                       TransportMediaStatusEnum TransportMediaStatus,
                                       JammedShutterPositionEnum JammedShutterPosition)
            {
                shutter = Shutter;
                positionStatus = PositionStatus;
                transport = Transport;
                transportMediaStatus = TransportMediaStatus;
                jammedShutterPosition = JammedShutterPosition;
            }

            public PositionStatusClass()
            {
            }

            /// <summary>
            /// Supported positions
            /// </summary>
            public enum PositionBitmapEnum
            {
                Input = 1 << 0,
                Output = 1 << 1,
                Refused = 1 << 2,
            }

            /// <summary>
            /// This property is set by the framework to generate status changed event
            /// </summary>
            public PositionBitmapEnum? Position { get; set; } = null;

            /// <summary>
            ///  Specifies the state of the shutter. the following values are possible:
            ///
            /// * Closed - The shutter is operational and is closed.
            /// * Open - The shutter is operational and is open.
            /// * Jammed - The shutter is jammed and is not operational.
            /// * Unknown - Due to a hardware error or other condition, the state of the shutter cannot be determined.
            /// * NotSupported
            /// </summary>
            public ShutterEnum Shutter
            {
                get { return shutter; }
                set
                {
                    if (shutter != value)
                    {
                        shutter = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private ShutterEnum shutter = ShutterEnum.NotSupported;

            /// <summary>
            /// The status of the position. the following values are possible:
            /// * Empty - The position is empty.
            /// * NotEmpty - The position is not empty.
            /// * Unknown - Due to a hardware error or other condition, the state of the position cannot be determined.
            /// * NotSupported
            /// /// </summary>
            public PositionStatusEnum PositionStatus
            {
                get { return positionStatus; }
                set
                {
                    if (positionStatus != value)
                    {
                        positionStatus = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private PositionStatusEnum positionStatus = PositionStatusEnum.NotSupported;

            /// <summary>
            /// Specifies the state of the transport mechanism. The transport is defined as any area leading to or from the
            /// position. the following values are possible:
            ///
            /// * Ok - The transport is in a good state.
            /// * Inoperative - The transport is inoperative due to a hardware failure or media jam.
            /// * Unknown - Due to a hardware error or other condition, the state of the transport cannot
            /// * NotSupported
            /// </summary>
            public TransportEnum Transport
            {
                get { return transport; }
                set
                {
                    if (transport != value)
                    {
                        transport = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private TransportEnum transport = TransportEnum.NotSupported;

            /// <summary>
            /// Returns information regarding items which may be present on the transport. the following values are possible:
            ///
            /// * Empty - The transport is empty.
            /// * NotEmpty - The transport is not empty.
            /// * Unknown - Due to a hardware error or other condition it is not known whether there are items on the transport.
            /// * NotSupported
            /// </summary>
            public TransportMediaStatusEnum TransportMediaStatus
            {
                get { return transportMediaStatus; }
                set
                {
                    if (transportMediaStatus != value)
                    {
                        transportMediaStatus = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private TransportMediaStatusEnum transportMediaStatus = TransportMediaStatusEnum.NotSupported;

            /// <summary>
            /// Returns information regarding the position of the jammed shutter. 
            /// the following values are possible:
            ///
            /// * NotJammed``` - The shutter is not jammed.
            /// * Open``` - The shutter is jammed, but fully open.
            /// * PartiallyOpen``` - The shutter is jammed, but partially open.
            /// * Closed``` - The shutter is jammed, but fully closed.
            /// * Unknown``` - The position of the shutter is unknown.
            /// * NotSupported
            /// </summary>
            public JammedShutterPositionEnum JammedShutterPosition
            {
                get { return jammedShutterPosition; }
                set
                {
                    if (jammedShutterPosition != value)
                    {
                        jammedShutterPosition = value;
                        NotifyPropertyChanged();
                    }
                }
            }
            private JammedShutterPositionEnum jammedShutterPosition = JammedShutterPositionEnum.NotSupported;
        }

        public enum ShutterEnum
        {
            Closed,
            Open,
            Jammed,
            Unknown,
            NotSupported,
        }

        public enum PositionStatusEnum
        {
            Empty,
            NotEmpty,
            Unknown,
            NotSupported,
        }

        public enum TransportEnum
        {
            Ok,
            Inoperative,
            Unknown,
            NotSupported,
        }

        public enum TransportMediaStatusEnum
        {
            Empty,
            NotEmpty,
            Unknown,
            NotSupported,
        }

        public enum JammedShutterPositionEnum
        {
            NotJammed,
            Open,
            PartiallyOpen,
            Closed,
            Unknown,
            NotSupported,
        }

        public enum AcceptorEnum
        {
            Ok,
            Attention,
            Stop,
            Unknown,
        }

        public enum MediaEnum
        {
            Present,
            NotPresent,
            Jammed,
            Unknown,
            Position,
        }

        public enum TonerEnum
        {
            Full,
            Low,
            Out,
            Unknown,
            NotSupported,
        }

        public enum InkEnum
        {
            Full,
            Low,
            Out,
            Unknown,
            NotSupported,
        }

        public enum ImageScannerEnum
        {
            Ok,
            Fading,
            Inoperative,
            Unknown,
            NotSupported,
        }

        public enum StackerEnum
        {
            Empty,
            NotEmpty,
            Full,
            Inoperative,
            Unknown,
            NotSupported,
        }

        public enum ReBuncherEnum
        {
            Empty,
            NotEmpty,
            Full,
            Inoperative,
            Unknown,
            NotSupported,
        }

        public enum MediaFeederEnum
        {
            Empty,
            NotEmpty,
            Inoperative,
            Unknown,
            NotSupported,
        }

        /// <summary>
        /// Supplies the state of the storage units for accepting cash.
        /// 
        /// * Ok - All storage units present are in a good state.
        /// * Atttention - One or more of the storage units is in a high, full or inoperative condition.Items can still
        ///                be accepted into at least one of the storage units.The status of the storage units can be obtained through the GetStorage command.
        /// * Stop - Due to a storage unit problem accepting is impossible.No items can be accepted because all of
        ///          the storage units are in a full or in an inoperative condition.
        /// * Unknown - Due to a hardware error or other condition, the state of the storage units cannot be determined.
        /// </summary>
        public AcceptorEnum Acceptor 
        {
            get { return acceptor; }
            set
            {
                if (acceptor != value)
                {
                    acceptor = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private AcceptorEnum acceptor = Acceptor;

        /// <summary>
        /// Specifies the state of the media.
        /// 
        /// * Present - Media is present in the device.
        /// * NotPresent - Media is not present in the device.
        /// * Jammed - Media is jammed in the device.
        /// * Unknown - The state of the media cannot be determined with the device in its current state.
        /// * Position - Media is at one or more of the input, output and refused positions.
        /// </summary>
        public MediaEnum Media
        {
            get { return media; }
            set
            {
                if (media != value)
                {
                    media = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private MediaEnum media = Media;


        /// <summary>
        /// Specifies the state of the toner or ink supply or the state of the ribbon of the endorser. This may be null in
        /// if the physical device does not support endorsing or the capability to report
        /// the status of the toner/ink is not supported by the device, otherwise the following values are possible:
        /// * Full - The toner or ink supply is full or the ribbon is OK.
        /// * Low - The toner or ink supply is low or the print contrast with a ribbon is weak.
        /// * Out - The toner or ink supply is empty or the print contrast with a ribbon is not sufficient any more.
        /// * Unknown - Status of toner or ink supply or the ribbon cannot be determined with the device in its current state.
        /// * NotSupported
        /// </summary>
        public TonerEnum Toner
        {
            get { return toner; }
            set
            {
                if (toner != value)
                {
                    toner = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private TonerEnum toner = Toner;

        /// <summary>
        /// Specifies the status of the stamping ink in the device.
        /// if the physical device does not support stamping or the capability to report the
        /// status of the stamp ink supply is not supported by the device, otherwise the following values are possible:
        /// * Full - Ink supply in the device is full.
        /// * Low - Ink supply in the device is low.
        /// * Out - Ink supply in the device is empty.
        /// * Unknown - Status of the stamping ink supply cannot be determined with the device in its current state.
        /// * NotSupported
        /// </summary>
        public InkEnum Ink
        {
            get { return ink; }
            set
            {
                if (ink != value)
                {
                    ink = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private InkEnum ink = Ink;

        /// <summary>
        /// Specifies the status of the image scanner that captures images of the front of the media items.
        /// if the physical device has no front scanner or the capability to report the
        /// status of the front scanner is not supported by the device, otherwise the following values are possible:
        /// * Ok - The front scanner is OK.
        /// * Fading - The front scanner performance is degraded.
        /// * Inoperative - The front scanner is inoperative.
        /// * Unknown - Status of the front scanner cannot be determined with the device in its current state.
        /// * NotSupported
        /// </summary>
        public ImageScannerEnum FrontImageScanner
        {
            get { return frontImageScanner; }
            set
            {
                if (frontImageScanner != value)
                {
                    frontImageScanner = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ImageScannerEnum frontImageScanner = FrontImageScanner;

        /// <summary>
        /// Specifies the status of the image scanner that captures images of the back of the media items.
        /// if the physical device has no back scanner or the capability to report the
        /// status of the back scanner is not supported by the device, otherwise the following values are possible:
        /// * Ok - The front scanner is OK.
        /// * Fading - The back scanner performance is degraded.
        /// * Inoperative - The back scanner is inoperative.
        /// * Unknown - Status of the back scanner cannot be determined with the device in its current state.
        /// * NotSupported
        /// </summary>
        public ImageScannerEnum BackImageScanner
        {
            get { return backImageScanner; }
            set
            {
                if (backImageScanner != value)
                {
                    backImageScanner = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ImageScannerEnum backImageScanner = BackImageScanner;

        /// <summary>
        /// Specifies the status of the MICR code line reader. 
        /// if the physical device has no MICR code line reader or 
        /// the capability to report the status of the MICR code line reader is not supported by the device, otherwise the following values are possible:
        /// * Ok - The front scanner is OK.
        /// * Fading - The back scanner performance is degraded.
        /// * Inoperative - The back scanner is inoperative.
        /// * Unknown - Status of the back scanner cannot be determined with the device in its current state.
        /// * NotSupported
        /// </summary>
        public ImageScannerEnum MICRReader
        {
            get { return micrReader; }
            set
            {
                if (micrReader != value)
                {
                    micrReader = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ImageScannerEnum micrReader = MICRReader;

        /// <summary>
        /// Supplies the state of the stacker (also known as an escrow). The stacker is where the media items are held
        /// while the application decides what to do with them.
        /// if the physical device has no stacker or the capability to report the status of the stacker is not supported by
        /// the device, otherwise the following values are possible:
        /// * Empty - The stacker is empty.
        /// * NotEmpty - The stacker is not empty.
        /// * Full - The stacker is full.This state is set if the number of media items on the stacker has
        ///          reached MaxMediaOnStacker capabilities or some physical limit has been reached.
        /// * Inoperative - The stacker is inoperative.
        /// * Unknown - Due to a hardware error or other condition, the state of the stacker cannot be determined.
        /// * NotSupported
        /// </summary>
        public StackerEnum Stacker
        {
            get { return stacker; }
            set
            {
                if (stacker != value)
                {
                    stacker = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private StackerEnum stacker = Stacker;


        /// <summary>
        /// Supplies the state of the re-buncher (return stacker). The re-buncher is where media items are re-bunched
        /// ready for return to the customer.
        /// if the physical device has no re-buncher or the capability to report the status of the re-buncher is not supported by the device,
        /// otherwise the following values are possible:
        /// * Empty - The re-buncher is empty.
        /// * NotEmpty - The re-buncher is not empty.
        /// * Full - The re-buncher is full.This state is set if the number of media items on the re-buncher
        /// has reached its physical limit.
        /// * Inoperative - The re-buncher is inoperative.
        /// * Unknown - Due to a hardware error or other condition, the state of the re-buncher cannot be determined.
        /// * NotSupported
        /// </summary>
        public ReBuncherEnum ReBuncher
        {
            get { return rebuncher; }
            set
            {
                if (rebuncher != value)
                {
                    rebuncher = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ReBuncherEnum rebuncher = ReBuncher;

        /// <summary>
        /// Supplies the state of the media feeder. This value indicates if there are items on the media feeder waiting
        /// This value can be one of the following values:
        /// * Empty - The media feeder is empty.
        /// * NotEmpty - The media feeder is not empty.
        /// * Inoperative - The media feeder is inoperative.
        /// * Unknown - Due to a hardware error or other condition, the state of the media feeder cannot be determined.
        /// * NotSupported
        /// </summary>
        public MediaFeederEnum MediaFeeder
        {
            get { return mediaFeeder; }
            set
            {
                if (mediaFeeder != value)
                {
                    mediaFeeder = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private MediaFeederEnum mediaFeeder = MediaFeeder;

        /// <summary>
        /// Array of supported position status.
        /// </summary>
        public Dictionary<CheckScannerCapabilitiesClass.PositionEnum, PositionStatusClass> Positions { get; init; } = Positions;
    }
}
