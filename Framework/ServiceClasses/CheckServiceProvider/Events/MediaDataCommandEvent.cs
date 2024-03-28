/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Check;

namespace XFS4IoTFramework.Check
{
    public class MediaDataCommandEvent
    {
        public MediaDataCommandEvent(ICheckService checkService, IActionItemEvents events)
        {
            events.IsNotNull($"Invalid reference to the event interface passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IActionItemEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaDataCommandEvent));
            ActionItemEvents = events;

            checkService.IsNotNull($"Invalid reference to check service passed in" + nameof(MediaPresentedCommandEvent));
            CheckScanner = checkService;
        }
        public MediaDataCommandEvent(ICheckService checkService, IMediaInEndEvents events)
        {
            events.IsNotNull($"Invalid reference to the event interface passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IMediaInEndEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaPresentedCommandEvent));
            MediaInEndEvents = events;

            checkService.IsNotNull($"Invalid reference to check service passed in" + nameof(MediaPresentedCommandEvent));
            CheckScanner = checkService;
        }
        public MediaDataCommandEvent(ICheckService checkService, IMediaInEvents events)
        {
            events.IsNotNull($"Invalid reference to the event interface passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IMediaInEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaPresentedCommandEvent));
            MediaInEvents = events;

            checkService.IsNotNull($"Invalid reference to check service passed in" + nameof(MediaPresentedCommandEvent));
            CheckScanner = checkService;
        }
        public MediaDataCommandEvent(ICheckService checkService, IGetNextItemEvents events)
        {
            events.IsNotNull($"Invalid reference to the event interface passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IGetNextItemEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaPresentedCommandEvent));
            GetNextItemEvents = events;

            checkService.IsNotNull($"Invalid reference to check service passed in" + nameof(MediaPresentedCommandEvent));
            CheckScanner = checkService;
        }

        public Task MediaDataEvent(
            int MediaID,
            string CodelineData,
            MediaDataInfo.MagneticReadIndicatorEnum MagneticReadIndicator,
            Dictionary<ImageSourceEnum, ImageDataInfo> Images,
            MediaDataInfo.CodelineOrientationEnum CodelineOrientation,
            MediaDataInfo.MediaOrientationEnum MediaOrientation,
            MediaSizeInfo MediaSize,
            MediaDataInfo. MediaValidityEnum MediaValidity)
        {
            // Update transaction status info
            if (CheckScanner.LastTransactionStatus.MediaInfo.ContainsKey(MediaID))
            {
                CheckScanner.LastTransactionStatus.MediaInfo[MediaID].CodelineData = CodelineData;
                CheckScanner.LastTransactionStatus.MediaInfo[MediaID].MagneticReadIndicator = MagneticReadIndicator;
                CheckScanner.LastTransactionStatus.MediaInfo[MediaID].CodelineOrientation = CodelineOrientation;
                CheckScanner.LastTransactionStatus.MediaInfo[MediaID].MediaOrientation = MediaOrientation;
                CheckScanner.LastTransactionStatus.MediaInfo[MediaID].MediaSize = MediaSize;
                CheckScanner.LastTransactionStatus.MediaInfo[MediaID].MediaValidity = MediaValidity;
                CheckScanner.LastTransactionStatus.MediaInfo[MediaID].Images = Images;
            }
            else
            {
                CheckScanner.LastTransactionStatus.MediaInfo.Add(
                    MediaID, 
                    new(
                        CodelineData: CodelineData,
                        MagneticReadIndicator: MagneticReadIndicator,
                        Images: Images,
                        CodelineOrientation: CodelineOrientation,
                        MediaOrientation: MediaOrientation,
                        MediaSize: MediaSize,
                        MediaValidity: MediaValidity
                        )
                    );
            }

            List<ImageDataClass> images = null;
            if (CheckScanner.LastTransactionStatus.MediaInfo[MediaID].Images is not null &&
                CheckScanner.LastTransactionStatus.MediaInfo[MediaID].Images.Count > 0)
            {
                foreach (var image in CheckScanner.LastTransactionStatus.MediaInfo[MediaID].Images)
                {
                    (images ??= []).Add(new(
                        ImageSource: image.Key switch
                        {
                            ImageSourceEnum.Front => XFS4IoT.Check.ImageSourceEnum.Front,
                            ImageSourceEnum.Back => XFS4IoT.Check.ImageSourceEnum.Back,
                            _ => throw new InternalErrorException($"Unexpected image source specified. {image.Key}")
                        },
                        ImageType: image.Value.ImageFormat switch
                        {
                            ImageInfo.ImageFormatEnum.BMP => XFS4IoT.Check.ImageTypeEnum.Bmp,
                            ImageInfo.ImageFormatEnum.WMF => XFS4IoT.Check.ImageTypeEnum.Wmf,
                            ImageInfo.ImageFormatEnum.TIF => XFS4IoT.Check.ImageTypeEnum.Tif,
                            ImageInfo.ImageFormatEnum.JPG => XFS4IoT.Check.ImageTypeEnum.Jpg,
                            _ => throw new InternalErrorException($"Unexpected image format specified. {image.Value.ImageFormat}")
                        },
                        ImageColorFormat: image.Value.ColorFormat switch
                        {
                            ImageInfo.ColorFormatEnum.Binary => XFS4IoT.Check.ImageColorFormatEnum.Binary,
                            ImageInfo.ColorFormatEnum.Grayscale => XFS4IoT.Check.ImageColorFormatEnum.GrayScale,
                            ImageInfo.ColorFormatEnum.Full => XFS4IoT.Check.ImageColorFormatEnum.Full,
                            _ => throw new InternalErrorException($"Unexpected image color format specified. {image.Value.ColorFormat}")
                        },
                        ImageScanColor: image.Value.ScanColor switch
                        {
                            ImageInfo.ScanColorEnum.Red => XFS4IoT.Check.ImageScanColorEnum.Red,
                            ImageInfo.ScanColorEnum.Blue => XFS4IoT.Check.ImageScanColorEnum.Blue,
                            ImageInfo.ScanColorEnum.Green => XFS4IoT.Check.ImageScanColorEnum.Green,
                            ImageInfo.ScanColorEnum.Yellow => XFS4IoT.Check.ImageScanColorEnum.Yellow,
                            ImageInfo.ScanColorEnum.White => XFS4IoT.Check.ImageScanColorEnum.White,
                            ImageInfo.ScanColorEnum.InfraRed => XFS4IoT.Check.ImageScanColorEnum.InfraRed,
                            ImageInfo.ScanColorEnum.UltraViolet => XFS4IoT.Check.ImageScanColorEnum.UltraViolet,
                            _ => throw new InternalErrorException($"Unexpected image scan color format specified. {image.Value.ScanColor}")
                        },
                        ImageStatus: image.Value.ImageStatus switch
                        {
                            ImageDataInfo.ImageStatusEnum.Ok => XFS4IoT.Check.ImageStatusEnum.Ok,
                            ImageDataInfo.ImageStatusEnum.SourceMissing => XFS4IoT.Check.ImageStatusEnum.SourceMissing,
                            ImageDataInfo.ImageStatusEnum.SourceNotSupported => XFS4IoT.Check.ImageStatusEnum.SourceNotSupported,
                            _ => throw new InternalErrorException($"Unexpected image status specified. {image.Value.ImageStatus}")
                        },
                        Image: (image.Value.ImageData is null || image.Value.ImageData.Count == 0) ?
                            null : image.Value.ImageData
                        ));
                }
            }

            XFS4IoT.Check.Events.MediaDataEvent.PayloadData payload = new(
                    MediaID: MediaID <= 0 ?
                        null :
                        MediaID,
                    CodelineData: CodelineData,
                    MagneticReadIndicator: MagneticReadIndicator switch
                    {
                        MediaDataInfo.MagneticReadIndicatorEnum.MICR => XFS4IoT.Check.MagneticReadIndicatorEnum.Micr,
                        MediaDataInfo.MagneticReadIndicatorEnum.Not_MICR => XFS4IoT.Check.MagneticReadIndicatorEnum.NotMicr,
                        MediaDataInfo.MagneticReadIndicatorEnum.No_MICR => XFS4IoT.Check.MagneticReadIndicatorEnum.NoMicr,
                        MediaDataInfo.MagneticReadIndicatorEnum.NotRead => XFS4IoT.Check.MagneticReadIndicatorEnum.NotRead,
                        MediaDataInfo.MagneticReadIndicatorEnum.Not_MICR_Format => XFS4IoT.Check.MagneticReadIndicatorEnum.NotMicrFormat,
                        MediaDataInfo.MagneticReadIndicatorEnum.Unknown => XFS4IoT.Check.MagneticReadIndicatorEnum.Unknown,
                        _ => throw new InternalErrorException($"Unexpected magnetic read indicator specified. {MagneticReadIndicator}")

                    },
                    Image: images,
                    InsertOrientation: MediaOrientation == MediaDataInfo.MediaOrientationEnum.None &&
                                       CodelineOrientation == MediaDataInfo.CodelineOrientationEnum.None ?
                                           null :
                                           new InsertOrientationClass(
                                               Codeline: CodelineOrientation switch
                                               {
                                                   MediaDataInfo.CodelineOrientationEnum.Right => InsertOrientationClass.CodelineEnum.Right,
                                                   MediaDataInfo.CodelineOrientationEnum.Left => InsertOrientationClass.CodelineEnum.Left,
                                                   MediaDataInfo.CodelineOrientationEnum.Top => InsertOrientationClass.CodelineEnum.Top,
                                                   MediaDataInfo.CodelineOrientationEnum.Bottom => InsertOrientationClass.CodelineEnum.Bottom,
                                                   _ => null,
                                               },
                                               Media: MediaOrientation switch
                                               {
                                                   MediaDataInfo.MediaOrientationEnum.Up => InsertOrientationClass.MediaEnum.Up,
                                                   MediaDataInfo.MediaOrientationEnum.Down => InsertOrientationClass.MediaEnum.Down,
                                                   _ => null,
                                               }),
                    MediaSize: MediaSize is null ? null :
                        new MediaSizeClass(LongEdge: MediaSize.LongEdge,
                                           ShortEdge: MediaSize.ShortEdge),
                    MediaValidity: MediaValidity switch
                    {
                        MediaDataInfo.MediaValidityEnum.Ok => XFS4IoT.Check.MediaValidityEnum.Ok,
                        MediaDataInfo.MediaValidityEnum.Suspect => XFS4IoT.Check.MediaValidityEnum.Suspect,
                        MediaDataInfo.MediaValidityEnum.Unknown => XFS4IoT.Check.MediaValidityEnum.Unknown,
                        MediaDataInfo.MediaValidityEnum.NoValidation => XFS4IoT.Check.MediaValidityEnum.NoValidation,
                        _ => throw new InternalErrorException($"Unexpected media validity specified. {MediaValidity}")
                    }
                );

            if (ActionItemEvents is not null)
            {
                return ActionItemEvents.MediaDataEvent(payload);
            }
            if (MediaInEndEvents is not null)
            {
                return MediaInEndEvents.MediaDataEvent(payload);
            }
            if (MediaInEvents is not null)
            {
                return MediaInEvents.MediaDataEvent(payload);
            }
            if (GetNextItemEvents is not null)
            {
                return GetNextItemEvents.MediaDataEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(MediaDataEvent));
        }

        protected IActionItemEvents ActionItemEvents { get; init; } = null;
        protected IMediaInEndEvents MediaInEndEvents { get; init; } = null;
        protected IMediaInEvents MediaInEvents { get; init; } = null;
        protected IGetNextItemEvents GetNextItemEvents { get; init; } = null;
        
        private ICheckService CheckScanner { get; init; } = null;
    }
}