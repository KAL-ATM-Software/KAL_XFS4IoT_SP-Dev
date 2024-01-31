/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoT.Completions;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Printer
{
    public partial class ReadImageHandler
    {
        private async Task<ReadImageCompletion.PayloadData> HandleReadImage(IReadImageEvents events, ReadImageCommand readImage, CancellationToken cancel)
        {

            if (readImage.Payload.FrontImageType is null &&
                readImage.Payload.FrontImageColorFormat is null &&
                readImage.Payload.BackImageType is null &&
                readImage.Payload.BackImageColorFormat is null)
            {
                return new ReadImageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No image type and color format specified.");
            }

            if ((readImage.Payload.FrontImageType is null &&
                 readImage.Payload.FrontImageColorFormat is not null) ||
                (readImage.Payload.FrontImageType is not null &&
                 readImage.Payload.FrontImageColorFormat is null) ||
                (readImage.Payload.BackImageType is null &&
                 readImage.Payload.BackImageColorFormat is not null) ||
                (readImage.Payload.BackImageType is not null &&
                 readImage.Payload.BackImageColorFormat is null))
            {
                return new ReadImageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No image type or color format specified.");
            }

            if (!((readImage.Payload.FrontImageType is not null &&
                   readImage.Payload.FrontImageColorFormat is not null) ||
                  (readImage.Payload.BackImageType is not null &&
                   readImage.Payload.BackImageColorFormat is not null)))
            {
                return new ReadImageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                           $"No image type or color format specified.");
            }

            Dictionary<AcceptAndReadImageRequest.SourceTypeEnum, AcceptAndReadImageRequest.ReadData> DataToRead = [];

            if (readImage.Payload.FrontImageType is not null)
            {
                if (!Common.PrinterCapabilities.ImageSourceTypes.HasFlag(PrinterCapabilitiesClass.ImageSourceTypeEnum.ImageFront))
                {
                    return new ReadImageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"No front image source supported by the device.");
                }

                if (readImage.Payload.FrontImageType == ReadImageCommand.PayloadData.FrontImageTypeEnum.Bmp &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.BMP) ||
                    readImage.Payload.FrontImageType == ReadImageCommand.PayloadData.FrontImageTypeEnum.Jpg &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.JPG) ||
                    readImage.Payload.FrontImageType == ReadImageCommand.PayloadData.FrontImageTypeEnum.Tif &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.TIF) ||
                    readImage.Payload.FrontImageType == ReadImageCommand.PayloadData.FrontImageTypeEnum.Wmf &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.WMF))
                {
                    return new ReadImageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Specified front image type supported by the device. {readImage.Payload.FrontImageType}");
                }

                if ((readImage.Payload.FrontImageColorFormat == ReadImageCommand.PayloadData.FrontImageColorFormatEnum.Binary &&
                    !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.FrontImageColorFormatEnum.Binary)) ||
                    (readImage.Payload.FrontImageColorFormat == ReadImageCommand.PayloadData.FrontImageColorFormatEnum.Fullcolor &&
                    !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.FrontImageColorFormatEnum.Full)) ||
                    (readImage.Payload.FrontImageColorFormat == ReadImageCommand.PayloadData.FrontImageColorFormatEnum.Grayscale &&
                    !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.FrontImageColorFormatEnum.GrayScale)))
                {
                    return new ReadImageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                $"Unsupported color image format specified.");
                }

                DataToRead.Add(AcceptAndReadImageRequest.SourceTypeEnum.Front, new AcceptAndReadImageRequest.ReadData(AcceptAndReadImageRequest.SourceTypeEnum.Front,
                                                                                                                      readImage.Payload.FrontImageType switch
                                                                                                                      {
                                                                                                                          ReadImageCommand.PayloadData.FrontImageTypeEnum.Tif => AcceptAndReadImageRequest.ReadData.DataFormatEnum.TIF,
                                                                                                                          ReadImageCommand.PayloadData.FrontImageTypeEnum.Wmf => AcceptAndReadImageRequest.ReadData.DataFormatEnum.WMF,
                                                                                                                          ReadImageCommand.PayloadData.FrontImageTypeEnum.Bmp => AcceptAndReadImageRequest.ReadData.DataFormatEnum.BMP,
                                                                                                                          _ => AcceptAndReadImageRequest.ReadData.DataFormatEnum.JPG,
                                                                                                                      },
                                                                                                                      readImage.Payload.FrontImageColorFormat is null ? AcceptAndReadImageRequest.ReadData.ColorFormatEnum.None :
                                                                                                                      readImage.Payload.FrontImageColorFormat switch
                                                                                                                      {
                                                                                                                          ReadImageCommand.PayloadData.FrontImageColorFormatEnum.Binary => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Binary,
                                                                                                                          ReadImageCommand.PayloadData.FrontImageColorFormatEnum.Grayscale => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Grayscale,
                                                                                                                          _ => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Fullcolor,
                                                                                                                      }));
            }
            if (readImage.Payload.BackImageType is not null)
            {
                if (!Common.PrinterCapabilities.ImageSourceTypes.HasFlag(PrinterCapabilitiesClass.ImageSourceTypeEnum.ImageBack))
                {
                    return new ReadImageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"No back image source supported by the device.");
                }

                if (readImage.Payload.BackImageType == ReadImageCommand.PayloadData.BackImageTypeEnum.Bmp &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.BMP) ||
                    readImage.Payload.BackImageType == ReadImageCommand.PayloadData.BackImageTypeEnum.Jpg &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.JPG) ||
                    readImage.Payload.BackImageType == ReadImageCommand.PayloadData.BackImageTypeEnum.Tif &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.TIF) ||
                    readImage.Payload.BackImageType == ReadImageCommand.PayloadData.BackImageTypeEnum.Wmf &&
                    !Common.PrinterCapabilities.ImageTypes.HasFlag(PrinterCapabilitiesClass.ImageTypeEnum.WMF))
                {
                    return new ReadImageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                               $"Specified back image type supported by the device. {readImage.Payload.FrontImageType}");
                }

                if (readImage.Payload.FrontImageColorFormat is not null)
                {
                    if ((readImage.Payload.BackImageColorFormat == ReadImageCommand.PayloadData.BackImageColorFormatEnum.Binary &&
                        !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.BackImageColorFormatEnum.Binary)) ||
                        (readImage.Payload.BackImageColorFormat == ReadImageCommand.PayloadData.BackImageColorFormatEnum.Fullcolor &&
                        !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.BackImageColorFormatEnum.Full)) ||
                        (readImage.Payload.BackImageColorFormat == ReadImageCommand.PayloadData.BackImageColorFormatEnum.Grayscale &&
                        !Common.PrinterCapabilities.FrontImageColorFormats.HasFlag(PrinterCapabilitiesClass.BackImageColorFormatEnum.GrayScale)))
                    {
                        return new ReadImageCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                   $"Unsupported color image format specified.");
                    }
                }

                DataToRead.Add(AcceptAndReadImageRequest.SourceTypeEnum.Back, new AcceptAndReadImageRequest.ReadData(AcceptAndReadImageRequest.SourceTypeEnum.Back,
                                                                                                                     readImage.Payload.BackImageType switch
                                                                                                                     {
                                                                                                                         ReadImageCommand.PayloadData.BackImageTypeEnum.Tif => AcceptAndReadImageRequest.ReadData.DataFormatEnum.TIF,
                                                                                                                         ReadImageCommand.PayloadData.BackImageTypeEnum.Wmf => AcceptAndReadImageRequest.ReadData.DataFormatEnum.WMF,
                                                                                                                         ReadImageCommand.PayloadData.BackImageTypeEnum.Bmp => AcceptAndReadImageRequest.ReadData.DataFormatEnum.BMP,
                                                                                                                         _ => AcceptAndReadImageRequest.ReadData.DataFormatEnum.JPG,
                                                                                                                     },
                                                                                                                     readImage.Payload.BackImageColorFormat is null ? AcceptAndReadImageRequest.ReadData.ColorFormatEnum.None :
                                                                                                                     readImage.Payload.BackImageColorFormat switch
                                                                                                                     {
                                                                                                                         ReadImageCommand.PayloadData.BackImageColorFormatEnum.Binary => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Binary,
                                                                                                                         ReadImageCommand.PayloadData.BackImageColorFormatEnum.Grayscale => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Grayscale,
                                                                                                                         _ => AcceptAndReadImageRequest.ReadData.ColorFormatEnum.Fullcolor,
                                                                                                                     }));
            }

            Logger.Log(Constants.DeviceClass, "PrinterDev.AcceptAndReadImageAsync()");
            var result = await Device.AcceptAndReadImageAsync(new ReadImageCommandEvents(events),
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

            return new ReadImageCompletion.PayloadData(result.CompletionCode,
                                                       result.ErrorDescription,
                                                       result.ErrorCode,
                                                       dataRead);
        }
    }
}
