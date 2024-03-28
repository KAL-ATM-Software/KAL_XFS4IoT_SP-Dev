/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using System.Collections.Generic;
using XFS4IoT.Check;

namespace XFS4IoTFramework.Check
{
    public partial class GetTransactionStatusHandler
    {
        private Task<GetTransactionStatusCompletion.PayloadData> HandleGetTransactionStatus(IGetTransactionStatusEvents events, GetTransactionStatusCommand getTransactionStatus, CancellationToken cancel)
        {
            List<XFS4IoT.Check.MediaStatusClass> mediaInfo = null;

            if (Check.LastTransactionStatus.MediaInfo is not null &&
                Check.LastTransactionStatus.MediaInfo.Count > 0)
            {
                foreach (var media in Check.LastTransactionStatus.MediaInfo)
                {
                    List<ImageDataClass> images = null;
                    if (media.Value.Images is not null &&
                        media.Value.Images.Count > 0)
                    {
                        foreach (var image in media.Value.Images)
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

                    string mediaLocation = null;

                    (mediaInfo ??= []).Add(new(
                        MediaID: media.Key,
                        MediaLocation: mediaLocation,
                        CodelineData: media.Value.CodelineData,
                        MagneticReadIndicator: media.Value.MagneticReadIndicator switch
                        {
                            MediaDataInfo.MagneticReadIndicatorEnum.MICR => XFS4IoT.Check.MagneticReadIndicatorEnum.Micr,
                            MediaDataInfo.MagneticReadIndicatorEnum.Not_MICR => XFS4IoT.Check.MagneticReadIndicatorEnum.NotMicr,
                            MediaDataInfo.MagneticReadIndicatorEnum.No_MICR => XFS4IoT.Check.MagneticReadIndicatorEnum.NoMicr,
                            MediaDataInfo.MagneticReadIndicatorEnum.NotRead => XFS4IoT.Check.MagneticReadIndicatorEnum.NotRead,
                            MediaDataInfo.MagneticReadIndicatorEnum.Not_MICR_Format => XFS4IoT.Check.MagneticReadIndicatorEnum.NotMicrFormat,
                            MediaDataInfo.MagneticReadIndicatorEnum.Unknown => XFS4IoT.Check.MagneticReadIndicatorEnum.Unknown,
                            _ => throw new InternalErrorException($"Unexpected magnetic read indicator specified. {media.Value.MagneticReadIndicator}")

                        },
                        Image: images,
                        InsertOrientation: media.Value.MediaOrientation == MediaDataInfo.MediaOrientationEnum.None &&
                                           media.Value.CodelineOrientation == MediaDataInfo.CodelineOrientationEnum.None ?
                                           null :
                                           new InsertOrientationClass(
                                               Codeline: media.Value.CodelineOrientation switch
                                               {
                                                   MediaDataInfo.CodelineOrientationEnum.Right => InsertOrientationClass.CodelineEnum.Right,
                                                   MediaDataInfo.CodelineOrientationEnum.Left => InsertOrientationClass.CodelineEnum.Left,
                                                   MediaDataInfo.CodelineOrientationEnum.Top => InsertOrientationClass.CodelineEnum.Top,
                                                   MediaDataInfo.CodelineOrientationEnum.Bottom => InsertOrientationClass.CodelineEnum.Bottom,
                                                   _ => null,
                                               },
                                               Media: media.Value.MediaOrientation switch
                                               {
                                                   MediaDataInfo.MediaOrientationEnum.Up => InsertOrientationClass.MediaEnum.Up,
                                                   MediaDataInfo.MediaOrientationEnum.Down => InsertOrientationClass.MediaEnum.Down,
                                                   _ => null,
                                               }),
                        MediaSize: media.Value.MediaSize is null ? null :
                        new MediaSizeClass(LongEdge: media.Value.MediaSize.LongEdge,
                                           ShortEdge: media.Value.MediaSize.ShortEdge),
                        MediaValidity: media.Value.MediaValidity switch
                        { 
                            MediaDataInfo.MediaValidityEnum.Ok => XFS4IoT.Check.MediaValidityEnum.Ok,
                            MediaDataInfo.MediaValidityEnum.Suspect => XFS4IoT.Check.MediaValidityEnum.Suspect,
                            MediaDataInfo.MediaValidityEnum.Unknown => XFS4IoT.Check.MediaValidityEnum.Unknown,
                            MediaDataInfo.MediaValidityEnum.NoValidation => XFS4IoT.Check.MediaValidityEnum.NoValidation,
                            _ => throw new InternalErrorException($"Unexpected media validity specified. {media.Value.MediaValidity}")
                        },
                        CustomerAccess: media.Value.CustomerAccess switch 
                        {
                            MediaStatus.CustomerAccessEnum.Customer => XFS4IoT.Check.MediaStatusClass.CustomerAccessEnum.Customer,
                            MediaStatus.CustomerAccessEnum.Unknown => XFS4IoT.Check.MediaStatusClass.CustomerAccessEnum.Unknown,
                            MediaStatus.CustomerAccessEnum.None => XFS4IoT.Check.MediaStatusClass.CustomerAccessEnum.None,
                            _ => throw new InternalErrorException($"Unexpected customer access specified. {media.Value.CustomerAccess}")
                        }
                        ));
                }
            }

            return Task.FromResult(
                new GetTransactionStatusCompletion.PayloadData(
                    CompletionCode: XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success,
                    ErrorDescription: null,
                    MediaInTransaction: Check.LastTransactionStatus.MediaInTransactionState switch
                    { 
                        TransactionStatus.MediaInTransactionStateEnum.Ok => GetTransactionStatusCompletion.PayloadData .MediaInTransactionEnum.Ok,
                        TransactionStatus.MediaInTransactionStateEnum.Active => GetTransactionStatusCompletion.PayloadData.MediaInTransactionEnum.Active,
                        TransactionStatus.MediaInTransactionStateEnum.Rollback => GetTransactionStatusCompletion.PayloadData.MediaInTransactionEnum.Rollback,
                        TransactionStatus.MediaInTransactionStateEnum.RollbackAfterDeposit => GetTransactionStatusCompletion.PayloadData.MediaInTransactionEnum.RollbackAfterDeposit,
                        TransactionStatus.MediaInTransactionStateEnum.Retract => GetTransactionStatusCompletion.PayloadData.MediaInTransactionEnum.Retract,
                        TransactionStatus.MediaInTransactionStateEnum.Failure => GetTransactionStatusCompletion.PayloadData.MediaInTransactionEnum.Failure,
                        TransactionStatus.MediaInTransactionStateEnum.Unknown => GetTransactionStatusCompletion.PayloadData.MediaInTransactionEnum.Unknown,
                        TransactionStatus.MediaInTransactionStateEnum.Reset => GetTransactionStatusCompletion.PayloadData.MediaInTransactionEnum.Reset,
                        _ => throw new InternalErrorException($"Unexpected transaction status is specified. {Check.LastTransactionStatus.MediaInTransactionState}")
                    },
                    MediaOnStacker: Check.LastTransactionStatus.MediaOnStacker < 0 ?
                        "unknown" :
                        Check.LastTransactionStatus.MediaOnStacker.ToString(),
                    LastMediaInTotal: Check.LastTransactionStatus.LastMediaInTotal < 0 ?
                        "unknown" :
                        Check.LastTransactionStatus.LastMediaInTotal.ToString(),
                    LastMediaAddedToStacker: Check.LastTransactionStatus.LastMediaAddedToStacker < 0 ?
                        "unknown" :
                        Check.LastTransactionStatus.LastMediaAddedToStacker.ToString(),
                    TotalItems: Check.LastTransactionStatus.TotalItems < 0 ?
                        "unknown" :
                        Check.LastTransactionStatus.TotalItems.ToString(),
                    TotalItemsRefused: Check.LastTransactionStatus.TotalItemsRefused < 0 ?
                        "unknown" :
                        Check.LastTransactionStatus.TotalItemsRefused.ToString(),
                    TotalBunchesRefused: Check.LastTransactionStatus.TotalBunchesRefused < 0 ?
                        "unknown" :
                        Check.LastTransactionStatus.TotalBunchesRefused.ToString(),
                    MediaInfo: mediaInfo)
                );
        }
    }
}
