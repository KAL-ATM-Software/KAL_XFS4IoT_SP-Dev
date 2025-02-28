/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Printer
{
    public partial class ReadImageHandler
    {
        private async Task<CommandResult<ReadImageCompletion.PayloadData>> HandleReadImage(IReadImageEvents events, ReadImageCommand readImage, CancellationToken cancel)
        {

            if (readImage.Payload.FrontImage?.ImageType is null &&
                readImage.Payload.FrontImage?.ColorFormat is null &&
                readImage.Payload.BackImage?.ImageType is null &&
                readImage.Payload.BackImage?.ColorFormat is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No image type and color format specified.");
            }

            if ((readImage.Payload.FrontImage?.ImageType is null &&
                 readImage.Payload.FrontImage?.ColorFormat is not null) ||
                (readImage.Payload.FrontImage?.ImageType is not null &&
                 readImage.Payload.FrontImage?.ColorFormat is null) ||
                (readImage.Payload.BackImage?.ImageType is null &&
                 readImage.Payload.BackImage?.ColorFormat is not null) ||
                (readImage.Payload.BackImage?.ImageType is not null &&
                 readImage.Payload.BackImage?.ColorFormat is null))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No image type or color format specified.");
            }

            if (!((readImage.Payload.FrontImage?.ImageType is not null &&
                   readImage.Payload.FrontImage?.ColorFormat is not null) ||
                  (readImage.Payload.BackImage?.ImageType is not null &&
                   readImage.Payload.BackImage?.ColorFormat is not null)))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No image type or color format specified.");
            }

            Dictionary<AcceptAndReadImageRequest.SourceTypeEnum, AcceptAndReadImageRequest.ReadData> DataToRead = [];

            if (readImage.Payload.FrontImage?.ImageType is not null)
            {
                if (!Common.PrinterCapabilities.ImageSourceTypes.HasFlag(PrinterCapabilitiesClass.ImageSourceTypeEnum.ImageFront))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"No front image source supported by the device.");
                }

                if (readImage.Payload.FrontImage.ImageType == ReadImageCommand.PayloadData.FrontImageClass.ImageTypeEnum.Bmp &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.BMP) ||
                    readImage.Payload.FrontImage.ImageType == ReadImageCommand.PayloadData.FrontImageClass.ImageTypeEnum.Jpg &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.JPG) ||
                    readImage.Payload.FrontImage.ImageType == ReadImageCommand.PayloadData.FrontImageClass.ImageTypeEnum.Tif &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.TIF) ||
                    readImage.Payload.FrontImage.ImageType == ReadImageCommand.PayloadData.FrontImageClass.ImageTypeEnum.Wmf &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.WMF))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified front image type supported by the device. {readImage.Payload.FrontImage.ImageType}");
                }

                if ((readImage.Payload.FrontImage.ColorFormat == ReadImageCommand.PayloadData.FrontImageClass.ColorFormatEnum.Binary &&
                    !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.FrontImageColorFormatEnum.Binary)) ||
                    (readImage.Payload.FrontImage.ColorFormat == ReadImageCommand.PayloadData.FrontImageClass.ColorFormatEnum.Fullcolor &&
                    !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.FrontImageColorFormatEnum.Full)) ||
                    (readImage.Payload.FrontImage.ColorFormat == ReadImageCommand.PayloadData.FrontImageClass.ColorFormatEnum.Grayscale &&
                    !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.FrontImageColorFormatEnum.GrayScale)))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Unsupported color image format specified.");
                }

                DataToRead.Add(
                    AcceptAndReadImageRequest.SourceTypeEnum.Front, 
                    new AcceptAndReadImageRequest.ReadData(
                        AcceptAndReadImageRequest.SourceTypeEnum.Front,
                        readImage.Payload.FrontImage.ImageType switch
                        {
                            ReadImageCommand.PayloadData.FrontImageClass.ImageTypeEnum.Tif => AcceptAndReadImageRequest.ReadData.DataFormatEnum.TIF,
                            ReadImageCommand.PayloadData.FrontImageClass.ImageTypeEnum.Wmf => AcceptAndReadImageRequest.ReadData.DataFormatEnum.WMF,
                            ReadImageCommand.PayloadData.FrontImageClass.ImageTypeEnum.Bmp => AcceptAndReadImageRequest.ReadData.DataFormatEnum.BMP,
                            _ => AcceptAndReadImageRequest.ReadData.DataFormatEnum.JPG,
                        },
                        readImage.Payload.FrontImage.ColorFormat is null ? AcceptAndReadImageRequest.ReadData.ColorFormatEnum.None :
                        readImage.Payload.FrontImage.ColorFormat switch
                        {
                            ReadImageCommand.PayloadData.FrontImageClass.ColorFormatEnum.Binary => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Binary,
                            ReadImageCommand.PayloadData.FrontImageClass.ColorFormatEnum.Grayscale => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Grayscale,
                            _ => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Fullcolor,
                        }));
            }

            if (readImage.Payload.BackImage?.ImageType is not null)
            {
                if (!Common.PrinterCapabilities.ImageSourceTypes.HasFlag(PrinterCapabilitiesClass.ImageSourceTypeEnum.ImageBack))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"No back image source supported by the device.");
                }

                if (readImage.Payload.BackImage.ImageType == ReadImageCommand.PayloadData.BackImageClass.ImageTypeEnum.Bmp &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.BMP) ||
                    readImage.Payload.BackImage.ImageType == ReadImageCommand.PayloadData.BackImageClass.ImageTypeEnum.Jpg &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.JPG) ||
                    readImage.Payload.BackImage.ImageType == ReadImageCommand.PayloadData.BackImageClass.ImageTypeEnum.Tif &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.TIF) ||
                    readImage.Payload.BackImage.ImageType == ReadImageCommand.PayloadData.BackImageClass.ImageTypeEnum.Wmf &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.WMF))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified back image type supported by the device. {readImage.Payload.BackImage.ImageType}");
                }

                if (readImage.Payload.BackImage?.ColorFormat is not null)
                {
                    if ((readImage.Payload.BackImage.ColorFormat == ReadImageCommand.PayloadData.BackImageClass.ColorFormatEnum.Binary &&
                        !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.BackImageColorFormatEnum.Binary)) ||
                        (readImage.Payload.BackImage.ColorFormat == ReadImageCommand.PayloadData.BackImageClass.ColorFormatEnum.Fullcolor &&
                        !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.BackImageColorFormatEnum.Full)) ||
                        (readImage.Payload.BackImage.ColorFormat == ReadImageCommand.PayloadData.BackImageClass.ColorFormatEnum.Grayscale &&
                        !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.BackImageColorFormatEnum.GrayScale)))
                    {
                        return new(
                            MessageHeader.CompletionCodeEnum.InvalidData,
                            $"Unsupported color image format specified.");
                    }
                }

                DataToRead.Add(
                    AcceptAndReadImageRequest.SourceTypeEnum.Back, 
                    new AcceptAndReadImageRequest.ReadData(
                        AcceptAndReadImageRequest.SourceTypeEnum.Back,
                        readImage.Payload.BackImage.ImageType switch
                        {
                            ReadImageCommand.PayloadData.BackImageClass.ImageTypeEnum.Tif => AcceptAndReadImageRequest.ReadData.DataFormatEnum.TIF,
                            ReadImageCommand.PayloadData.BackImageClass.ImageTypeEnum.Wmf => AcceptAndReadImageRequest.ReadData.DataFormatEnum.WMF,
                            ReadImageCommand.PayloadData.BackImageClass.ImageTypeEnum.Bmp => AcceptAndReadImageRequest.ReadData.DataFormatEnum.BMP,
                            _ => AcceptAndReadImageRequest.ReadData.DataFormatEnum.JPG,
                        },
                        readImage.Payload.BackImage.ColorFormat is null ? AcceptAndReadImageRequest.ReadData.ColorFormatEnum.None :
                        readImage.Payload.BackImage.ColorFormat switch
                        {
                            ReadImageCommand.PayloadData.BackImageClass.ColorFormatEnum.Binary => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Binary,
                            ReadImageCommand.PayloadData.BackImageClass.ColorFormatEnum.Grayscale => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Grayscale,
                            _ => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Fullcolor,
                        }));
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.AcceptAndReadImageAsync()");
            var result = await Device.AcceptAndReadImageAsync(
                new ReadImageCommandEvents(events),
                new AcceptAndReadImageRequest(DataToRead),
                cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.AcceptAndReadImageAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            ReadImageCompletion.PayloadData.ImagesClass dataRead = null;
            if (result.Data?.Count > 0)
            {
                ReadImageCompletion.PayloadData.ImagesClass.FrontClass front = null;
                if (result.Data.ContainsKey(AcceptAndReadImageRequest.SourceTypeEnum.Front))
                {
                    front = new(result.Data[AcceptAndReadImageRequest.SourceTypeEnum.Front].Status switch
                    { 
                        AcceptAndReadImageResult.DataRead.StatusEnum.Ok => ReadImageCompletion.PayloadData.ImagesClass.FrontClass.StatusEnum.Ok,
                        AcceptAndReadImageResult.DataRead.StatusEnum.Missing => ReadImageCompletion.PayloadData.ImagesClass.FrontClass.StatusEnum.Missing,
                        _ => null,
                    },
                    result.Data[AcceptAndReadImageRequest.SourceTypeEnum.Front].Data);
                }
                ReadImageCompletion.PayloadData.ImagesClass.BackClass back = null;
                if (result.Data.ContainsKey(AcceptAndReadImageRequest.SourceTypeEnum.Back))
                {
                    back = new(result.Data[AcceptAndReadImageRequest.SourceTypeEnum.Back].Status switch
                    {
                        AcceptAndReadImageResult.DataRead.StatusEnum.Ok => ReadImageCompletion.PayloadData.ImagesClass.BackClass.StatusEnum.Ok,
                        AcceptAndReadImageResult.DataRead.StatusEnum.Missing => ReadImageCompletion.PayloadData.ImagesClass.BackClass.StatusEnum.Missing,
                        _ => null,
                    },
                    result.Data[AcceptAndReadImageRequest.SourceTypeEnum.Back].Data);
                }
                dataRead = new(front, back);
            }

            ReadImageCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                dataRead is not null)
            {
                payload = new(
                    ErrorCode: result.ErrorCode,
                    Images: dataRead);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }
    }
}
