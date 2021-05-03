/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using XFS4IoT;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;

namespace XFS4IoTFramework.CardReader
{
    public partial class ReadRawDataHandler
    {
        private async Task<ReadRawDataCompletion.PayloadData> HandleReadRawData(IReadRawDataEvents events, ReadRawDataCommand readRawData, CancellationToken cancel)
        {
            ReadCardRequest.CardDataTypesEnum dataTypes = ReadCardRequest.CardDataTypesEnum.NoDataRead;

            if (readRawData.Payload.Track1 is not null && (bool)readRawData.Payload.Track1)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.Track1;
            if (readRawData.Payload.Track2 is not null && (bool)readRawData.Payload.Track2)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.Track2;
            if (readRawData.Payload.Track3 is not null && (bool)readRawData.Payload.Track3)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.Track3;
            if (readRawData.Payload.Chip is not null && (bool)readRawData.Payload.Chip)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.Chip;
            if (readRawData.Payload.Security is not null && (bool)readRawData.Payload.Security)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.Security;
            if (readRawData.Payload.MemoryChip is not null && (bool)readRawData.Payload.MemoryChip)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.MemoryChip;
            if (readRawData.Payload.Track1Front is not null && (bool)readRawData.Payload.Track1Front)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.Track1Front;
            if (readRawData.Payload.FrontImage is not null && (bool)readRawData.Payload.FrontImage)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.FrontImage;
            if (readRawData.Payload.BackImage is not null && (bool)readRawData.Payload.BackImage)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.BackImage;
            if (readRawData.Payload.Track1JIS is not null && (bool)readRawData.Payload.Track1JIS)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.Track1JIS;
            if (readRawData.Payload.Track3JIS is not null && (bool)readRawData.Payload.Track3JIS)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.Track3JIS;
            if (readRawData.Payload.Ddi is not null && (bool)readRawData.Payload.Ddi)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.Ddi;
            if (readRawData.Payload.Watermark is not null && (bool)readRawData.Payload.Watermark)
                dataTypes |= ReadCardRequest.CardDataTypesEnum.Watermark;

            bool fluxInactive = readRawData.Payload.FluxInactive ?? true;
            if (readRawData.Payload.FluxInactive is not null)
                fluxInactive = (bool)readRawData.Payload.FluxInactive;

            Logger.Log(Constants.DeviceClass, "CardReaderDev.AcceptCardAsync()");

            IAcceptCardEvents newEvents = new AcceptCardEvents((ReadRawDataEvents)events);
            newEvents.IsNotNull($"Invalid event reference. {nameof(AcceptCardEvents)}");

            var acceptCardResult = await Device.AcceptCardAsync(newEvents,
                                                                new AcceptCardRequest(dataTypes, fluxInactive, readRawData.Payload.Timeout),
                                                                cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.AcceptCardAsync() -> {acceptCardResult.CompletionCode}, {acceptCardResult.ErrorCode}");

            if (acceptCardResult.CompletionCode != MessagePayload.CompletionCodeEnum.Success ||
                dataTypes == ReadCardRequest.CardDataTypesEnum.NoDataRead)
            {
                // Map to XFS erro code
                ReadRawDataCompletion.PayloadData.ErrorCodeEnum? errorCode = acceptCardResult.ErrorCode switch
                {
                    AcceptCardResult.ErrorCodeEnum.MediaJam => ReadRawDataCompletion.PayloadData.ErrorCodeEnum.MediaJam,
                    AcceptCardResult.ErrorCodeEnum.ShutterFail => ReadRawDataCompletion.PayloadData.ErrorCodeEnum.ShutterFail,
                    AcceptCardResult.ErrorCodeEnum.NoMedia => ReadRawDataCompletion.PayloadData.ErrorCodeEnum.NoMedia,
                    AcceptCardResult.ErrorCodeEnum.InvalidMedia => ReadRawDataCompletion.PayloadData.ErrorCodeEnum.InvalidMedia,
                    AcceptCardResult.ErrorCodeEnum.CardTooShort => ReadRawDataCompletion.PayloadData.ErrorCodeEnum.CardTooShort,
                    AcceptCardResult.ErrorCodeEnum.CardTooLong => ReadRawDataCompletion.PayloadData.ErrorCodeEnum.CardTooLong,
                    _ => null
                };

                return new ReadRawDataCompletion.PayloadData(acceptCardResult.CompletionCode,
                                                             acceptCardResult.ErrorDescription,
                                                             errorCode);
            }

            // The device specific class completed accepting card operation check the media status must be present for motorised cardreader
            // Latch Dip can latch a card and possibly report Lached or Present status, but if the application asks to read tracks, card won't be latched and the media status is not reliable 
            if (Device.DeviceType == DeviceTypeEnum.Motor &&
                (Device.MediaStatus != MediaStatusEnum.Present &&
                 Device.MediaStatus != MediaStatusEnum.NotSupported))
            {
                return new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.HardwareError,
                                                             "Accept operation is completed successfully, but the media is not present.", 
                                                             ReadRawDataCompletion.PayloadData.ErrorCodeEnum.NoMedia);
            }

            // Card is accepted now and in the device, try to read card data now
            Logger.Log(Constants.DeviceClass, "CardReaderDev.ReadCardAsync()");
            var readCardDataResult = await Device.ReadCardAsync(events,
                                                                new ReadCardRequest(dataTypes),
                                                                cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ReadCardAsync() -> {readCardDataResult.CompletionCode}, {readCardDataResult.ErrorCode}");

            if (readCardDataResult.CompletionCode != MessagePayload.CompletionCodeEnum.Success ||
                readCardDataResult.ErrorCode is not null)
            {
                return new ReadRawDataCompletion.PayloadData(readCardDataResult.CompletionCode,
                                                             readCardDataResult.ErrorDescription,
                                                             readCardDataResult.ErrorCode);
            }

            // build output data
            ReadRawDataCompletion.PayloadData.Track1Class track1 = null;
            ReadRawDataCompletion.PayloadData.Track2Class track2 = null;
            ReadRawDataCompletion.PayloadData.Track3Class track3 = null;
            ReadRawDataCompletion.PayloadData.WatermarkClass watermark = null;
            ReadRawDataCompletion.PayloadData.Track1FrontClass track1Front = null;
            ReadRawDataCompletion.PayloadData.FrontImageClass frontImage = null;
            ReadRawDataCompletion.PayloadData.BackImageClass backImage = null;
            ReadRawDataCompletion.PayloadData.Track1JISClass track1JIS = null;
            ReadRawDataCompletion.PayloadData.Track3JISClass track3JIS = null;
            ReadRawDataCompletion.PayloadData.DdiClass ddi = null;
            ReadRawDataCompletion.PayloadData.SecurityClass security = null;
            ReadRawDataCompletion.PayloadData.MemoryChipClass memoryChip = null;

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track1).IsTrue("Ttrack1 data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1].DataStatus, "Unexpected track1 data status is set by the device class. DataStatus field should not be null.");
                byte[] data = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1].Data?.ToArray();
                track1 = new ReadRawDataCompletion.PayloadData.Track1Class((ReadRawDataCompletion.PayloadData.Track1Class.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1].DataStatus,
                                                                           (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : null);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track2))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track2).IsTrue("Track2 data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track2].DataStatus, "Unexpected track2 data status is set by the device class. DataStatus field should not be null.");
                byte[] data = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track2].Data?.ToArray();
                track2 = new ReadRawDataCompletion.PayloadData.Track2Class((ReadRawDataCompletion.PayloadData.Track2Class.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track2].DataStatus,
                                                                           (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : null);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track3))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track3).IsTrue("Track3 data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3].DataStatus, "Unexpected track3 data status is set by the device class. DataStatus field should not be null.");
                byte[] data = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3].Data?.ToArray();
                track3 = new ReadRawDataCompletion.PayloadData.Track3Class((ReadRawDataCompletion.PayloadData.Track3Class.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3].DataStatus,
                                                                           (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : null);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Watermark))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Watermark).IsTrue("Watermark data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Watermark].DataStatus, "Unexpected watermak data status is set by the device class. DataStatus field should not be null.");
                byte[] data = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Watermark].Data?.ToArray();
                watermark = new ReadRawDataCompletion.PayloadData.WatermarkClass((ReadRawDataCompletion.PayloadData.WatermarkClass.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Watermark].DataStatus,
                                                                                 (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : null);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1Front))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track1Front).IsTrue("Track1Front data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1Front].DataStatus, "Unexpected track1 front data status is set by the device class. DataStatus field should not be null.");
                byte[] data = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1Front].Data?.ToArray();
                track1Front = new ReadRawDataCompletion.PayloadData.Track1FrontClass((ReadRawDataCompletion.PayloadData.Track1FrontClass.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1Front].DataStatus,
                                                                           (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : null);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.FrontImage))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.FrontImage).IsTrue("FrontImage data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.FrontImage].DataStatus, "Unexpected front image data status is set by the device class. DataStatus field should not be null.");
                byte[] data = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.FrontImage].Data?.ToArray();
                frontImage = new ReadRawDataCompletion.PayloadData.FrontImageClass((ReadRawDataCompletion.PayloadData.FrontImageClass.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.FrontImage].DataStatus,
                                                                                   (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : string.Empty);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.BackImage))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.BackImage).IsTrue("BackImage data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.BackImage].DataStatus, "Unexpected back image data status is set by the device class. DataStatus field should not be null.");
                byte[] data = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.BackImage].Data?.ToArray();
                backImage = new ReadRawDataCompletion.PayloadData.BackImageClass((ReadRawDataCompletion.PayloadData.BackImageClass.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.BackImage].DataStatus,
                                                                              (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : null);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1JIS))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track1JIS).IsTrue("Track1JIS data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1JIS].DataStatus, "Unexpected track JIS1 data status is set by the device class. DataStatus field should not be null.");
                byte[] data = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1JIS].Data?.ToArray();
                track1JIS = new ReadRawDataCompletion.PayloadData.Track1JISClass((ReadRawDataCompletion.PayloadData.Track1JISClass.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1JIS].DataStatus,
                                                                                  (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : null);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track3JIS))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track3JIS).IsTrue("Track3JIS data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3JIS].DataStatus, "Unexpected track JIS3 data status is set by the device class. DataStatus field should not be null.");
                byte[] data = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3JIS].Data?.ToArray();
                track3JIS = new ReadRawDataCompletion.PayloadData.Track3JISClass((ReadRawDataCompletion.PayloadData.Track3JISClass.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3JIS].DataStatus,
                                                                                 (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : null);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Ddi))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Ddi).IsTrue("DDI data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Ddi].DataStatus, "Unexpected DDI data status is set by the device class. DataStatus field should not be null.");
                byte[] data = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Ddi].Data?.ToArray();
                ddi = new ReadRawDataCompletion.PayloadData.DdiClass((ReadRawDataCompletion.PayloadData.DdiClass.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Ddi].DataStatus,
                                                                     (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : null);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Security))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Security).IsTrue("Security data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Security].DataStatus, "Unexpected security data status is set by the device class. DataStatus field should not be null.");
                security = new ReadRawDataCompletion.PayloadData.SecurityClass((ReadRawDataCompletion.PayloadData.SecurityClass.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Security].DataStatus,
                                                                               readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Security].SecutiryDataStatus);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.MemoryChip))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.MemoryChip).IsTrue("MemoryChip data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].DataStatus, "Unexpected memocy chip data status is set by the device class. DataStatus field should not be null.");
                memoryChip = new ReadRawDataCompletion.PayloadData.MemoryChipClass((ReadRawDataCompletion.PayloadData.MemoryChipClass.StatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].DataStatus,
                                                                                   readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].MemcoryChipDataStatus);
            }

            List<ReadRawDataCompletion.PayloadData.ChipClass> chip = null;

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Chip) &&
                readCardDataResult.ChipATRRead is not null && 
                readCardDataResult.ChipATRRead.Count > 0)
            {
                chip = new List<ReadRawDataCompletion.PayloadData.ChipClass>();
                foreach (ReadCardResult.CardData atr in readCardDataResult.ChipATRRead)
                {
                    Contracts.IsNotNull(atr.DataStatus, "Unexpected chip data status is set by the device class. DataStatus field should not be null.");
                    byte[] data = atr.Data?.ToArray();
                    chip.Add(new ReadRawDataCompletion.PayloadData.ChipClass((ReadRawDataCompletion.PayloadData.ChipClass.StatusEnum)atr.DataStatus,
                                                                             (data is not null && data.Length > 0) ? Convert.ToBase64String(data) : string.Empty));
                }
            }

            return new ReadRawDataCompletion.PayloadData(readCardDataResult.CompletionCode,
                                                         readCardDataResult.ErrorDescription,
                                                         readCardDataResult.ErrorCode,
                                                         track1,
                                                         track2,
                                                         track3,
                                                         chip,
                                                         security,
                                                         watermark,
                                                         memoryChip,
                                                         track1Front,
                                                         frontImage,
                                                         backImage,
                                                         track1JIS,
                                                         track3JIS,
                                                         ddi);
        }
    }
}
