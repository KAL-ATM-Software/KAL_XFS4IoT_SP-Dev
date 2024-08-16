/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using XFS4IoTFramework.Storage;
using System.Drawing.Imaging;

namespace XFS4IoTFramework.Check
{
    public partial class SetMediaParametersHandler
    {
        private async Task<CommandResult<SetMediaParametersCompletion.PayloadData>> HandleSetMediaParameters(ISetMediaParametersEvents events, SetMediaParametersCommand setMediaParameters, CancellationToken cancel)
        {
            if (setMediaParameters.Payload is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Specified empty payload.");
            }

            int mediaId = 0;
            if (setMediaParameters.Payload.MediaID is not null)
            {
                mediaId = (int)setMediaParameters.Payload.MediaID;
                if (!Check.LastTransactionStatus.MediaInfo.ContainsKey(mediaId))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified media ID is not recognized. {mediaId}");
                }
            }

            if (string.IsNullOrEmpty(setMediaParameters.Payload.Destination))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No destination is specified.");
            }

            if (!Regex.IsMatch(setMediaParameters.Payload.Destination, "^customer$|^unit[0-9A-Za-z]+$"))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Invalid destination is specified. {setMediaParameters.Payload.Destination}");
            }

            SetMediaParametersRequest.DestinationEnum destination = SetMediaParametersRequest.DestinationEnum.Customer;
            string storageId = null;
            if (setMediaParameters.Payload.Destination.Contains("unit"))
            {
                if (!Storage.CheckUnits.ContainsKey(setMediaParameters.Payload.Destination))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified destination does not exist. {setMediaParameters.Payload.Destination}");
                }
                destination = SetMediaParametersRequest.DestinationEnum.Unit;
                storageId = setMediaParameters.Payload.Destination;
            }

            Dictionary<ImageSourceEnum, ImageInfo> images = null;
            if (setMediaParameters.Payload.Image is not null &&
                setMediaParameters.Payload.Image.Count > 0)
            {
                foreach (var image in setMediaParameters.Payload.Image)
                {
                    (images ??= []).Add(
                        image.Source switch
                        { 
                            XFS4IoT.Check.ImageSourceEnum.Front => ImageSourceEnum.Front,
                            XFS4IoT.Check.ImageSourceEnum.Back => ImageSourceEnum.Back,
                            _ => throw new InternalErrorException($"Unsupported source specified. {image.Source}")
                        },
                        new(ImageFormat: image.Type switch
                            {
                                XFS4IoT.Check.ImageTypeEnum.Bmp => ImageInfo.ImageFormatEnum.BMP,
                                XFS4IoT.Check.ImageTypeEnum.Wmf => ImageInfo.ImageFormatEnum.WMF,
                                XFS4IoT.Check.ImageTypeEnum.Tif => ImageInfo.ImageFormatEnum.TIF,
                                XFS4IoT.Check.ImageTypeEnum.Jpg => ImageInfo.ImageFormatEnum.JPG,
                                _ => throw new InternalErrorException($"Unsupported image type specified. {image.Type}")
                            },
                            ColorFormat: image.ColorFormat switch
                            {
                                XFS4IoT.Check.ImageColorFormatEnum.Binary => ImageInfo.ColorFormatEnum.Binary,
                                XFS4IoT.Check.ImageColorFormatEnum.GrayScale => ImageInfo.ColorFormatEnum.Grayscale,
                                XFS4IoT.Check.ImageColorFormatEnum.Full => ImageInfo.ColorFormatEnum.Full,
                                _ => throw new InternalErrorException($"Unsupported image color format specified. {image.ColorFormat}")
                            },
                            ScanColor: image.ScanColor switch
                            {
                                XFS4IoT.Check.ImageScanColorEnum.Red => ImageInfo.ScanColorEnum.Red,
                                XFS4IoT.Check.ImageScanColorEnum.Green => ImageInfo.ScanColorEnum.Green,
                                XFS4IoT.Check.ImageScanColorEnum.Blue => ImageInfo.ScanColorEnum.Blue,
                                XFS4IoT.Check.ImageScanColorEnum.Yellow => ImageInfo.ScanColorEnum.Yellow,
                                XFS4IoT.Check.ImageScanColorEnum.White => ImageInfo.ScanColorEnum.White,
                                XFS4IoT.Check.ImageScanColorEnum.InfraRed => ImageInfo.ScanColorEnum.InfraRed,
                                XFS4IoT.Check.ImageScanColorEnum.UltraViolet => ImageInfo.ScanColorEnum.UltraViolet,
                                _ => throw new InternalErrorException($"Unsupported image color format specified. {image.ScanColor}")
                            })
                        );
                }
            }

            Logger.Log(Constants.DeviceClass, "CheckDev.SetMediaParametersAsync()");

            var result = await Device.SetMediaParametersAsync(
                request: new(
                    MediaId: mediaId,
                    Destination: destination,
                    StorageId: storageId,
                    Stamp: setMediaParameters.Payload.Stamp is not null && (bool)setMediaParameters.Payload.Stamp,
                    PrintData: setMediaParameters.Payload.PrintData,
                    ImagesToRead: images),
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"CheckDev.SetMediaParametersAsync() -> {result.CompletionCode}");

            return new(
                result.ErrorCode is not null ? new(ErrorCode: result.ErrorCode) : null,
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}
