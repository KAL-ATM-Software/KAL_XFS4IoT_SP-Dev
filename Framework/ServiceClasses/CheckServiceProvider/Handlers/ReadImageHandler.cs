/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
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
    public partial class ReadImageHandler
    {
        private async Task<CommandResult<ReadImageCompletion.PayloadData>> HandleReadImage(IReadImageEvents events, ReadImageCommand readImage, CancellationToken cancel)
        {
            if (readImage.Payload is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No payload specified.");
            }

            if (readImage.Payload.MediaID is null ||
                readImage.Payload.MediaID <= 0)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Invalid MediaID specified. {readImage.Payload.MediaID}");
            }

            CodelineFomratEnum codelineFormat = readImage.Payload.CodelineFormat switch
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
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Codeline format is specified but the device doesn't support codeline data source. check capabilities.dataSource. {readImage.Payload.CodelineFormat}");
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
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported codeline format is specified. {readImage.Payload.CodelineFormat}");
                }
            }

            Dictionary<ImageSourceEnum, ImageInfo> imageInfo = null;
            if (readImage.Payload.Image is not null &&
                readImage.Payload.Image.Count > 0)
            {
                foreach (var info in readImage.Payload.Image)
                {
                    info.IsNotNull($"Unexpected object is specified in the Image payload.");

                    if (info.Type is null ||
                        info.Source is null ||
                        info.ScanColor is null ||
                        info.ColorFormat is null)
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"One of required field for {nameof(readImage.Payload.Image)} is missing.");
                    }

                    ImageSourceEnum imageSource = info.Source switch
                    {
                        XFS4IoT.Check.ImageSourceEnum.Front => ImageSourceEnum.Front,
                        XFS4IoT.Check.ImageSourceEnum.Back => ImageSourceEnum.Back,
                        _ => throw new InternalErrorException($"Unsupported image source is specified. {info.Source}"),
                    };

                    if (imageSource == ImageSourceEnum.Front &&
                        !Common.CheckScannerCapabilities.DataSources.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.DataSourceEnum.Front) ||
                        imageSource == ImageSourceEnum.Back &&
                        !Common.CheckScannerCapabilities.DataSources.HasFlag(XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.DataSourceEnum.Back))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
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
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
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

                    XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum scanColorCap = scanColor switch
                    {
                        ImageInfo.ScanColorEnum.Red => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Red,
                        ImageInfo.ScanColorEnum.Green => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Green,
                        ImageInfo.ScanColorEnum.Blue => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Blue,
                        ImageInfo.ScanColorEnum.Yellow => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.Yellow,
                        ImageInfo.ScanColorEnum.White => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.White,
                        ImageInfo.ScanColorEnum.InfraRed => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.InfraRed,
                        _ => XFS4IoTFramework.Common.CheckScannerCapabilitiesClass.ImageCapabilities.ScanColorEnum.UltraViolet
                    };

                    if (imageSource == ImageSourceEnum.Front && 
                        !Common.CheckScannerCapabilities.FrontImage.ScanColor.HasFlag(scanColorCap) ||
                        imageSource == ImageSourceEnum.Back &&
                        !Common.CheckScannerCapabilities.BackImage.ScanColor.HasFlag(scanColorCap))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
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
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Specified scan color is not supported by the device. {info.ScanColor}");
                    }

                    if (imageInfo is not null &&
                        imageInfo.ContainsKey(imageSource))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Specified same data source already exist. {imageSource}");
                    }
                    (imageInfo ??= []).Add(imageSource, new(imageType, colorFormat, scanColor));
                }
            }

            Logger.Log(Constants.DeviceClass, "CheckDev.ReadImageAsync()");

            var result = await Device.ReadImageAsync(
                request: new(MediaId: (int)readImage.Payload.MediaID,
                             CodelineFormat: codelineFormat,
                             ImageInfo: imageInfo),
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"CheckDev.ReadImageAsync() -> {result.CompletionCode}");

            if (result.ImageData is not null)
            {
                MediaStatus mediaStatus = new(result.ImageData);

                if (Check.LastTransactionStatus.MediaInfo.ContainsKey((int)readImage.Payload.MediaID))
                {
                    Check.LastTransactionStatus.MediaInfo.Remove((int)readImage.Payload.MediaID);
                }
                Check.LastTransactionStatus.MediaInfo.Add((int)readImage.Payload.MediaID, mediaStatus);
            }

            Check.StoreTransactionStatus();

            return new(
                result.ErrorCode is not null ? new(ErrorCode: result.ErrorCode) : null,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
