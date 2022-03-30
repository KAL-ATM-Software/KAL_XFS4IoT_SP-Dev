/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoT.CardReader;
using XFS4IoTFramework.Common;

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

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1) &&
                !Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1) ||
                dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track2) &&
                !Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track2) ||
                dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track3) &&
                !Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track3) ||
                dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1Front) &&
                !Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1Front) ||
                dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1JIS) &&
                !Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track1JIS) ||
                dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track3JIS) &&
                !Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Track3JIS) ||
                dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Watermark) &&
                !Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Watermark) ||
                dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Ddi) &&
                !Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.Ddi) ||
                dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.BackImage) &&
                !Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.BackImage) ||
                dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.FrontImage) &&
                !Common.CardReaderCapabilities.ReadTracks.HasFlag(CardReaderCapabilitiesClass.ReadableDataTypesEnum.FrontImage))
            {
                return new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"Specified data to read is not supported.");
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.MemoryChip) &&
                Common.CardReaderCapabilities.MemoryChipProtocols == CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.NotSupported)
            {
                return new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"Memmory chip is not supported.");
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Chip) &&
                Common.CardReaderCapabilities.ChipProtocols == CardReaderCapabilitiesClass.ChipProtocolsEnum.NotSupported)
            {
                return new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"Chip is not supported.");
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Security) &&
                Common.CardReaderCapabilities.SecurityType == CardReaderCapabilitiesClass.SecurityTypeEnum.NotSupported)
            {
                return new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                             $"Security module is not supported.");
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.AcceptCardAsync()");

            var acceptCardResult = await Device.AcceptCardAsync(new CommonCardCommandEvents(events),
                                                                new AcceptCardRequest(dataTypes, fluxInactive, readRawData.Payload.Timeout),
                                                                cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.AcceptCardAsync() -> {acceptCardResult.CompletionCode}, {acceptCardResult.ErrorCode}");

            if (acceptCardResult.CompletionCode != MessagePayload.CompletionCodeEnum.Success ||
                dataTypes == ReadCardRequest.CardDataTypesEnum.NoDataRead)
            {
                // Map to XFS error code
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
            if (Common.CardReaderCapabilities.Type == CardReaderCapabilitiesClass.DeviceTypeEnum.Motor &&
                (Common.CardReaderStatus.Media != CardReaderStatusClass.MediaEnum.Present  &&
                 Common.CardReaderStatus.Media != CardReaderStatusClass.MediaEnum.NotPresent))
            {
                return new ReadRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                             "Accept operation is completed successfully, but the media is not present.", 
                                                             ReadRawDataCompletion.PayloadData.ErrorCodeEnum.NoMedia);
            }

            // Card is accepted now and in the device, try to read card data now
            Logger.Log(Constants.DeviceClass, "CardReaderDev.ReadCardAsync()");
            var readCardDataResult = await Device.ReadCardAsync(new ReadCardCommandEvents(events),
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
            CardDataClass track1 = null;
            CardDataClass track2 = null;
            CardDataClass track3 = null;
            CardDataClass watermark = null;
            CardDataClass track1Front = null;
            List<byte> frontImage = null;
            List<byte> backImage = null;
            CardDataClass track1JIS = null;
            CardDataClass track3JIS = null;
            CardDataClass ddi = null;
            ReadRawDataCompletion.PayloadData.SecurityClass security = null;
            ReadRawDataCompletion.PayloadData.MemoryChipClass memoryChip = null;

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track1).IsTrue("Ttrack1 data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1].DataStatus, "Unexpected track1 data status is set by the device class. DataStatus field should not be null.");
                track1 = new CardDataClass((CardDataStatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1].DataStatus, 
                                           readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track2))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track2).IsTrue("Track2 data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track2].DataStatus, "Unexpected track2 data status is set by the device class. DataStatus field should not be null.");
                track2 = new CardDataClass((CardDataStatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track2].DataStatus,
                                           readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track2].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track3))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track3).IsTrue("Track3 data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3].DataStatus, "Unexpected track3 data status is set by the device class. DataStatus field should not be null.");
                track3 = new CardDataClass((CardDataStatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3].DataStatus,
                                           readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Watermark))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Watermark).IsTrue("Watermark data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Watermark].DataStatus, "Unexpected watermak data status is set by the device class. DataStatus field should not be null.");
                watermark = new CardDataClass((CardDataStatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Watermark].DataStatus,
                                              readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Watermark].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1Front))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track1Front).IsTrue("Track1Front data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1Front].DataStatus, "Unexpected track1 front data status is set by the device class. DataStatus field should not be null.");
                track1Front = new CardDataClass((CardDataStatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1Front].DataStatus,
                                                readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1Front].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.FrontImage))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.FrontImage).IsTrue("FrontImage data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.FrontImage].DataStatus, "Unexpected front image data status is set by the device class. DataStatus field should not be null.");
                frontImage = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.FrontImage].Data;
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.BackImage))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.BackImage).IsTrue("BackImage data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.BackImage].DataStatus, "Unexpected back image data status is set by the device class. DataStatus field should not be null.");
                backImage = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.BackImage].Data;
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1JIS))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track1JIS).IsTrue("Track1JIS data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1JIS].DataStatus, "Unexpected track JIS1 data status is set by the device class. DataStatus field should not be null.");
                track1JIS = new CardDataClass((CardDataStatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1JIS].DataStatus,
                                              readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1JIS].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track3JIS))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track3JIS).IsTrue("Track3JIS data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3JIS].DataStatus, "Unexpected track JIS3 data status is set by the device class. DataStatus field should not be null.");
                track3JIS = new CardDataClass((CardDataStatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3JIS].DataStatus,
                                              readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3JIS].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Ddi))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Ddi).IsTrue("DDI data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Ddi].DataStatus, "Unexpected DDI data status is set by the device class. DataStatus field should not be null.");
                ddi = new CardDataClass((CardDataStatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Ddi].DataStatus,
                                        readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Ddi].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Security))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Security).IsTrue("Security data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Security].DataStatus, "Unexpected security data status is set by the device class. DataStatus field should not be null.");
                security = new ReadRawDataCompletion.PayloadData.SecurityClass((CardDataStatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Security].DataStatus,
                                                                               readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Security].SecutiryDataStatus);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.MemoryChip))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.MemoryChip).IsTrue("MemoryChip data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].DataStatus, "Unexpected memocy chip data status is set by the device class. DataStatus field should not be null.");
                memoryChip = new ReadRawDataCompletion.PayloadData.MemoryChipClass((CardDataStatusEnum)readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].DataStatus,
                                                                                   readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].MemcoryChipDataStatus);
            }

            List<CardDataClass> chip = null;

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Chip) &&
                readCardDataResult.ChipATRRead is not null && 
                readCardDataResult.ChipATRRead.Count > 0)
            {
                chip = new List<CardDataClass>();
                foreach (ReadCardResult.CardData atr in readCardDataResult.ChipATRRead)
                {
                    Contracts.IsNotNull(atr.DataStatus, "Unexpected chip data status is set by the device class. DataStatus field should not be null.");
                    chip.Add(new CardDataClass((CardDataStatusEnum)atr.DataStatus,
                                               atr.Data));
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
