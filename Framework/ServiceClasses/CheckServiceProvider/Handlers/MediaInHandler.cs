/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using XFS4IoT.Check;

namespace XFS4IoTFramework.Check
{
    public partial class MediaInHandler
    {
        private async Task<MediaInCompletion.PayloadData> HandleMediaIn(IMediaInEvents events, MediaInCommand mediaIn, CancellationToken cancel)
        {
            if (mediaIn.Payload is null)
            {
                return new MediaInCompletion.PayloadData(
                    MessagePayload.CompletionCodeEnum.InvalidData,
                    $"No payload specified.");
            }

            CodelineFomratEnum codelineFormat = mediaIn.Payload.CodelineFormat switch
            {
                XFS4IoT.Check.CodelineFormatEnum.Cmc7 => CodelineFomratEnum.CMC7,
                XFS4IoT.Check.CodelineFormatEnum.E13b => CodelineFomratEnum.E13B,
                XFS4IoT.Check.CodelineFormatEnum.Ocr => CodelineFomratEnum.OCR,
                XFS4IoT.Check.CodelineFormatEnum.Ocra => CodelineFomratEnum.OCRA,
                XFS4IoT.Check.CodelineFormatEnum.Ocrb => CodelineFomratEnum.OCRB,
                _ => CodelineFomratEnum.None,
            };

            if (codelineFormat != CodelineFomratEnum.None)
            {
                if (!Common.CheckScannerCapabilities.DataSources.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.DataSourceEnum.Codeline))
                {
                    return new MediaInCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"Codeline format is specified but the device doesn't support codeline data source. check capabilities.dataSource. {mediaIn.Payload.CodelineFormat}");
                }

                if (codelineFormat == CodelineFomratEnum.CMC7 &&
                    !Common.CheckScannerCapabilities.CodelineFormats.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.CodelineFormatEnum.CMC7) ||
                    codelineFormat == CodelineFomratEnum.E13B &&
                    !Common.CheckScannerCapabilities.CodelineFormats.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.CodelineFormatEnum.E13B) ||
                    codelineFormat == CodelineFomratEnum.OCR &&
                    !Common.CheckScannerCapabilities.CodelineFormats.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.CodelineFormatEnum.OCR) ||
                    codelineFormat == CodelineFomratEnum.OCRA &&
                    !Common.CheckScannerCapabilities.CodelineFormats.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.CodelineFormatEnum.OCRA) ||
                    codelineFormat == CodelineFomratEnum.OCRB &&
                    !Common.CheckScannerCapabilities.CodelineFormats.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.CodelineFormatEnum.OCRB))
                {
                    return new MediaInCompletion.PayloadData(
                        MessagePayload.CompletionCodeEnum.InvalidData,
                        $"Unsupported codeline format is specified. {mediaIn.Payload.CodelineFormat}");
                }
            }

            Dictionary<ImageSourceEnum, ImageInfo> imageInfo = null;
            if (mediaIn.Payload.Image is not null &&
                mediaIn.Payload.Image.Count > 0)
            {
                foreach (var info in mediaIn.Payload.Image)
                {
                    info.IsNotNull($"Unexpected object is specified in the Image payload.");

                    if (info.Type is null ||
                        info.Source is null ||
                        info.ScanColor is null ||
                        info.ColorFormat is null)
                    {
                        return new MediaInCompletion.PayloadData(
                            MessagePayload.CompletionCodeEnum.InvalidData,
                            $"One of required field for {nameof(mediaIn.Payload.Image)} is missing.");
                    }

                    ImageSourceEnum imageSource = info.Source switch
                    {
                        XFS4IoT.Check.ImageSourceEnum.Front => ImageSourceEnum.Front,
                        XFS4IoT.Check.ImageSourceEnum.Back => ImageSourceEnum.Back,
                        _ => throw new InternalErrorException($"Unsupported image source is specified. {info.Source}"),
                    };

                    if ((imageSource == ImageSourceEnum.Front &&
                         !Common.CheckScannerCapabilities.DataSources.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.DataSourceEnum.Front)) ||
                        (imageSource == ImageSourceEnum.Back &&
                         !Common.CheckScannerCapabilities.DataSources.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.DataSourceEnum.Back)))
                    {
                        return new MediaInCompletion.PayloadData(
                            MessagePayload.CompletionCodeEnum.InvalidData,
                            $"Specified data source is not supported by the device. {info.Source}");
                    }

                    ImageInfo.ImageFormatEnum imageType = info.Type switch
                    {
                        ImageTypeEnum.Jpg => ImageInfo.ImageFormatEnum.JPG,
                        ImageTypeEnum.Wmf => ImageInfo.ImageFormatEnum.WMF,
                        ImageTypeEnum.Tif => ImageInfo.ImageFormatEnum.TIF,
                        ImageTypeEnum.Bmp => ImageInfo.ImageFormatEnum.BMP,
                        _ => throw new InternalErrorException($"Unsupported image format is specified. {info.Type}"),
                    };

                    if (imageType == ImageInfo.ImageFormatEnum.JPG &&
                        !Common.CheckScannerCapabilities.ImageTypes.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageTypeEnum.JPG) ||
                        imageType == ImageInfo.ImageFormatEnum.WMF &&
                        !Common.CheckScannerCapabilities.ImageTypes.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageTypeEnum.WMF) ||
                        imageType == ImageInfo.ImageFormatEnum.BMP &&
                        !Common.CheckScannerCapabilities.ImageTypes.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageTypeEnum.BMP) ||
                        imageType == ImageInfo.ImageFormatEnum.TIF &&
                        !Common.CheckScannerCapabilities.ImageTypes.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageTypeEnum.TIF))
                    {
                        return new MediaInCompletion.PayloadData(
                            MessagePayload.CompletionCodeEnum.InvalidData,
                            $"Specified image format is not supported by the device. {info.Type}");
                    }

                    ImageInfo.ScanColorEnum scanColor = info.ScanColor switch
                    {
                        ImageScanColorEnum.Red => ImageInfo.ScanColorEnum.Red,
                        ImageScanColorEnum.Green => ImageInfo.ScanColorEnum.Green,
                        ImageScanColorEnum.Blue => ImageInfo.ScanColorEnum.Blue,
                        ImageScanColorEnum.Yellow => ImageInfo.ScanColorEnum.Yellow,
                        ImageScanColorEnum.White => ImageInfo.ScanColorEnum.White,
                        ImageScanColorEnum.InfraRed => ImageInfo.ScanColorEnum.InfraRed,
                        ImageScanColorEnum.UltraViolet => ImageInfo.ScanColorEnum.UltraViolet,
                        _ => throw new InternalErrorException($"Unexpected scan color is specified. {info.ScanColor}"),
                    };

                    XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum scanColorCap = scanColor switch
                    {
                        ImageInfo.ScanColorEnum.Red => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Red,
                        ImageInfo.ScanColorEnum.Green => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Green,
                        ImageInfo.ScanColorEnum.Blue => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Blue,
                        ImageInfo.ScanColorEnum.Yellow => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.Yellow,
                        ImageInfo.ScanColorEnum.White => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.White,
                        ImageInfo.ScanColorEnum.InfraRed => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.InfraRed,
                        _ => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorFlagEnum.UltraViolet
                    };

                    if ((imageSource == ImageSourceEnum.Front &&
                         !Common.CheckScannerCapabilities.FrontImage.ScanColor.HasFlag(scanColorCap)) ||
                        (imageSource == ImageSourceEnum.Back &&
                         !Common.CheckScannerCapabilities.BackImage.ScanColor.HasFlag(scanColorCap)))
                    {
                        return new MediaInCompletion.PayloadData(
                            MessagePayload.CompletionCodeEnum.InvalidData,
                            $"Specified scan color is not supported by the device. {info.ScanColor}");
                    }

                    ImageInfo.ColorFormatEnum colorFormat = info.ColorFormat switch
                    {
                        ImageColorFormatEnum.Binary => ImageInfo.ColorFormatEnum.Binary,
                        ImageColorFormatEnum.GrayScale => ImageInfo.ColorFormatEnum.Grayscale,
                        ImageColorFormatEnum.Full => ImageInfo.ColorFormatEnum.Full,
                        _ => throw new InternalErrorException($"Unexpected color format is specified. {info.ColorFormat}"),
                    };

                    XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ColorFormatEnum colorFormatCap = colorFormat switch
                    {
                        ImageInfo.ColorFormatEnum.Binary => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ColorFormatEnum.Binary,
                        ImageInfo.ColorFormatEnum.Grayscale => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ColorFormatEnum.Grayscale,
                        _ => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ColorFormatEnum.Full
                    };

                    if (imageSource == ImageSourceEnum.Front &&
                        !Common.CheckScannerCapabilities.FrontImage.ColorFormats.HasFlag(colorFormatCap) ||
                        imageSource == ImageSourceEnum.Back &&
                        !Common.CheckScannerCapabilities.BackImage.ColorFormats.HasFlag(colorFormatCap))
                    {
                        return new MediaInCompletion.PayloadData(
                            MessagePayload.CompletionCodeEnum.InvalidData,
                            $"Specified scan color is not supported by the device. {info.ScanColor}");
                    }

                    if (imageInfo is not null &&
                        imageInfo.ContainsKey(imageSource))
                    {
                        return new MediaInCompletion.PayloadData(
                            MessagePayload.CompletionCodeEnum.InvalidData,
                            $"Specified same data source already exist. {imageSource}");
                    }
                    (imageInfo ??= []).Add(imageSource, new(imageType, colorFormat, scanColor));
                }
            }

            int maxMediaOnStacker;
            if (mediaIn.Payload.MaxMediaOnStacker is null ||
                mediaIn.Payload.MaxMediaOnStacker == 0)
            {
                maxMediaOnStacker = Common.CheckScannerCapabilities.MaxMediaOnStacker;
            }
            else
            {
                maxMediaOnStacker = (int)mediaIn.Payload.MaxMediaOnStacker;
            }

            // Clear last transaction status
            if (Check.LastTransactionStatus.MediaInTransactionState != TransactionStatus.MediaInTransactionStateEnum.Active)
            {
                Check.LastTransactionStatus.NewTransaction();
            }

            Logger.Log(Constants.DeviceClass, "CheckDev.MediaInAsync()");

            var result = await Device.MediaInAsync(
                events: new MediaInCommandEvents(Check, events),
                request: new(CodelineFormat: codelineFormat,
                             ImagesToRead: imageInfo,
                             MaxMediaOnStacker: maxMediaOnStacker,
                             ApplicationRefuse: mediaIn.Payload.ApplicationRefuse is not null && (bool)mediaIn.Payload.ApplicationRefuse),
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"CheckDev.MediaInAsync() -> {result.CompletionCode}");

            Check.LastTransactionStatus.LastMediaAddedToStacker = result.LastMediaOnStacker;

            if (result.MediaOnStacker >= 0)
            {
                Check.LastTransactionStatus.MediaOnStacker = result.MediaOnStacker;
            }
            else if (Check.LastTransactionStatus.LastMediaAddedToStacker > 0)
            {
                if (Check.LastTransactionStatus.MediaOnStacker < 0)
                {
                    Check.LastTransactionStatus.MediaOnStacker = Check.LastTransactionStatus.LastMediaAddedToStacker;
                }
                else
                {
                    Check.LastTransactionStatus.MediaOnStacker += Check.LastTransactionStatus.LastMediaAddedToStacker;
                }
            }
            else
            {
                Check.LastTransactionStatus.MediaOnStacker = -1;
            }

            if (result.LastMedia >= 0)
            {
                Check.LastTransactionStatus.LastMediaInTotal += result.LastMedia;
            }
            else
            {
                Check.LastTransactionStatus.LastMediaInTotal = -1;
            }

            Check.LastTransactionStatus.MediaInTransactionState = TransactionStatus.MediaInTransactionStateEnum.Failure;
            if (result.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                Check.LastTransactionStatus.MediaInTransactionState = TransactionStatus.MediaInTransactionStateEnum.Active;
            }

            Check.StoreTransactionStatus();

            MediainClass mediaInResult = null;
            if (result.LastMediaOnStacker >= 0 ||
                result.MediaOnStacker >= 0 ||
                result.LastMedia >= 0)
            {
                mediaInResult = new(
                    MediaOnStacker: result.MediaOnStacker < 0 ?
                        null :
                        result.MediaOnStacker,
                    LastMedia: result.LastMedia < 0 ?
                        null :
                        result.LastMedia,
                    LastMediaOnStacker: result.LastMediaOnStacker < 0 ?
                        null :
                        result.LastMediaOnStacker,
                    MediaFeeder: Device.CheckScannerStatus.MediaFeeder switch
                    {
                        XFS4IoTFramework.Common.CheckScannerStatusClass.MediaFeederEnum.Empty => MediaFeederEnum.Empty,
                        XFS4IoTFramework.Common.CheckScannerStatusClass.MediaFeederEnum.NotEmpty => MediaFeederEnum.NotEmpty,
                        XFS4IoTFramework.Common.CheckScannerStatusClass.MediaFeederEnum.Inoperative => MediaFeederEnum.Inoperative,
                        XFS4IoTFramework.Common.CheckScannerStatusClass.MediaFeederEnum.Unknown => MediaFeederEnum.Unknown,
                        _ => null,
                    });
            }

            return new MediaInCompletion.PayloadData(
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription,
                ErrorCode: result.ErrorCode,
                MediaIn: mediaInResult);
        }
    }
}
