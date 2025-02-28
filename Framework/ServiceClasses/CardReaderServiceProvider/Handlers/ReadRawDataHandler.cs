/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
        private async Task<CommandResult<ReadRawDataCompletion.PayloadData>> HandleReadRawData(IReadRawDataEvents events, ReadRawDataCommand readRawData, CancellationToken cancel)
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
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Specified data to read is not supported.");
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.MemoryChip) &&
                Common.CardReaderCapabilities.MemoryChipProtocols == CardReaderCapabilitiesClass.MemoryChipProtocolsEnum.NotSupported)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Memmory chip is not supported.");
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Chip) &&
                Common.CardReaderCapabilities.ChipProtocols == CardReaderCapabilitiesClass.ChipProtocolsEnum.NotSupported)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Chip is not supported.");
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Security) &&
                Common.CardReaderCapabilities.SecurityType == CardReaderCapabilitiesClass.SecurityTypeEnum.NotSupported)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"Security module is not supported.");
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.AcceptCardAsync()");

            var acceptCardResult = await Device.AcceptCardAsync(
                new ReadRawDataCommandEvents(events),
                new AcceptCardRequest(
                    dataTypes, 
                    fluxInactive, 
                    readRawData.Header.Timeout ?? 0),
                cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.AcceptCardAsync() -> {acceptCardResult.CompletionCode}, {acceptCardResult.ErrorCode}");

            if (acceptCardResult.CompletionCode != MessageHeader.CompletionCodeEnum.Success ||
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

                return new(
                    new(errorCode),
                    acceptCardResult.CompletionCode,
                    acceptCardResult.ErrorDescription);
            }

            // The device specific class completed accepting card operation check the media status must be present for motorised cardreader
            // Latch Dip can latch a card and possibly report Lached or Present status, but if the application asks to read tracks, card won't be latched and the media status is not reliable 
            if (Common.CardReaderCapabilities.Type == CardReaderCapabilitiesClass.DeviceTypeEnum.Motor &&
                (Common.CardReaderStatus.Media != CardReaderStatusClass.MediaEnum.Present  &&
                 Common.CardReaderStatus.Media != CardReaderStatusClass.MediaEnum.NotPresent))
            {
                return new(
                    new(ReadRawDataCompletion.PayloadData.ErrorCodeEnum.NoMedia),
                    MessageHeader.CompletionCodeEnum.CommandErrorCode,
                    "Accept operation is completed successfully, but the media is not present.");
            }

            // Card is accepted now and in the device, try to read card data now
            Logger.Log(Constants.DeviceClass, "CardReaderDev.ReadCardAsync()");
            var readCardDataResult = await Device.ReadCardAsync(
                new ReadCardCommandEvents(events),
                new ReadCardRequest(dataTypes),
                cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ReadCardAsync() -> {readCardDataResult.CompletionCode}, {readCardDataResult.ErrorCode}");

            if (readCardDataResult.CompletionCode != MessageHeader.CompletionCodeEnum.Success ||
                readCardDataResult.ErrorCode is not null)
            {
                return new(
                    new(
                        readCardDataResult.ErrorCode),
                        readCardDataResult.CompletionCode,
                        readCardDataResult.ErrorDescription);
            }

            // build output data
            CardDataNullableClass track1 = null;
            CardDataNullableClass track2 = null;
            CardDataNullableClass track3 = null;
            CardDataNullableClass watermark = null;
            CardDataNullableClass track1Front = null;
            List<byte> frontImage = null;
            List<byte> backImage = null;
            CardDataNullableClass track1JIS = null;
            CardDataNullableClass track3JIS = null;
            CardDataNullableClass ddi = null;
            ReadRawDataCompletion.PayloadData.SecurityClass security = null;
            ReadRawDataCompletion.PayloadData.MemoryChipClass memoryChip = null;

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track1).IsTrue("Ttrack1 data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1].DataStatus, "Unexpected track1 data status is set by the device class. DataStatus field should not be null.");
                track1 = new CardDataNullableClass(
                    Status: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1].DataStatus switch
                    {
                        ReadCardResult.CardData.DataStatusEnum.DataMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataMissing,
                        ReadCardResult.CardData.DataStatusEnum.DataInvalid => XFS4IoT.CardReader.CardDataStatusEnum.DataInvalid,
                        ReadCardResult.CardData.DataStatusEnum.DataTooLong => XFS4IoT.CardReader.CardDataStatusEnum.DataTooLong,
                        ReadCardResult.CardData.DataStatusEnum.DataTooShort => XFS4IoT.CardReader.CardDataStatusEnum.DataTooShort,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceNotSupported => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceNotSupported,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceMissing,
                        _ => null
                    },
                    Data: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track2))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track2).IsTrue("Track2 data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track2].DataStatus, "Unexpected track2 data status is set by the device class. DataStatus field should not be null.");
                track2 = new CardDataNullableClass(
                    Status: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track2].DataStatus switch
                    {
                        ReadCardResult.CardData.DataStatusEnum.DataMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataMissing,
                        ReadCardResult.CardData.DataStatusEnum.DataInvalid => XFS4IoT.CardReader.CardDataStatusEnum.DataInvalid,
                        ReadCardResult.CardData.DataStatusEnum.DataTooLong => XFS4IoT.CardReader.CardDataStatusEnum.DataTooLong,
                        ReadCardResult.CardData.DataStatusEnum.DataTooShort => XFS4IoT.CardReader.CardDataStatusEnum.DataTooShort,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceNotSupported => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceNotSupported,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceMissing,
                        _ => null
                    },
                    Data: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track2].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track3))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track3).IsTrue("Track3 data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3].DataStatus, "Unexpected track3 data status is set by the device class. DataStatus field should not be null.");
                track3 = new CardDataNullableClass(
                    Status: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3].DataStatus switch
                    {
                        ReadCardResult.CardData.DataStatusEnum.DataMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataMissing,
                        ReadCardResult.CardData.DataStatusEnum.DataInvalid => XFS4IoT.CardReader.CardDataStatusEnum.DataInvalid,
                        ReadCardResult.CardData.DataStatusEnum.DataTooLong => XFS4IoT.CardReader.CardDataStatusEnum.DataTooLong,
                        ReadCardResult.CardData.DataStatusEnum.DataTooShort => XFS4IoT.CardReader.CardDataStatusEnum.DataTooShort,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceNotSupported => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceNotSupported,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceMissing,
                        _ => null
                    },
                    Data: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Watermark))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Watermark).IsTrue("Watermark data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Watermark].DataStatus, "Unexpected watermak data status is set by the device class. DataStatus field should not be null.");
                watermark = new CardDataNullableClass(
                    Status: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Watermark].DataStatus switch
                    {
                        ReadCardResult.CardData.DataStatusEnum.DataMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataMissing,
                        ReadCardResult.CardData.DataStatusEnum.DataInvalid => XFS4IoT.CardReader.CardDataStatusEnum.DataInvalid,
                        ReadCardResult.CardData.DataStatusEnum.DataTooLong => XFS4IoT.CardReader.CardDataStatusEnum.DataTooLong,
                        ReadCardResult.CardData.DataStatusEnum.DataTooShort => XFS4IoT.CardReader.CardDataStatusEnum.DataTooShort,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceNotSupported => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceNotSupported,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceMissing,
                        _ => null
                    },
                    Data: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Watermark].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track1Front))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track1Front).IsTrue("Track1Front data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1Front].DataStatus, "Unexpected track1 front data status is set by the device class. DataStatus field should not be null.");
                track1Front = new CardDataNullableClass(
                    Status: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1Front].DataStatus switch
                    {
                        ReadCardResult.CardData.DataStatusEnum.DataMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataMissing,
                        ReadCardResult.CardData.DataStatusEnum.DataInvalid => XFS4IoT.CardReader.CardDataStatusEnum.DataInvalid,
                        ReadCardResult.CardData.DataStatusEnum.DataTooLong => XFS4IoT.CardReader.CardDataStatusEnum.DataTooLong,
                        ReadCardResult.CardData.DataStatusEnum.DataTooShort => XFS4IoT.CardReader.CardDataStatusEnum.DataTooShort,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceNotSupported => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceNotSupported,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceMissing,
                        _ => null
                    },
                    Data: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1Front].Data);
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
                track1JIS = new CardDataNullableClass(
                    Status: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1JIS].DataStatus switch
                    {
                        ReadCardResult.CardData.DataStatusEnum.DataMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataMissing,
                        ReadCardResult.CardData.DataStatusEnum.DataInvalid => XFS4IoT.CardReader.CardDataStatusEnum.DataInvalid,
                        ReadCardResult.CardData.DataStatusEnum.DataTooLong => XFS4IoT.CardReader.CardDataStatusEnum.DataTooLong,
                        ReadCardResult.CardData.DataStatusEnum.DataTooShort => XFS4IoT.CardReader.CardDataStatusEnum.DataTooShort,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceNotSupported => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceNotSupported,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceMissing,
                        _ => null
                    },
                    Data: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track1JIS].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Track3JIS))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Track3JIS).IsTrue("Track3JIS data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3JIS].DataStatus, "Unexpected track JIS3 data status is set by the device class. DataStatus field should not be null.");
                track3JIS = new CardDataNullableClass(
                    Status: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3JIS].DataStatus switch
                    {
                        ReadCardResult.CardData.DataStatusEnum.DataMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataMissing,
                        ReadCardResult.CardData.DataStatusEnum.DataInvalid => XFS4IoT.CardReader.CardDataStatusEnum.DataInvalid,
                        ReadCardResult.CardData.DataStatusEnum.DataTooLong => XFS4IoT.CardReader.CardDataStatusEnum.DataTooLong,
                        ReadCardResult.CardData.DataStatusEnum.DataTooShort => XFS4IoT.CardReader.CardDataStatusEnum.DataTooShort,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceNotSupported => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceNotSupported,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceMissing,
                        _ => null
                    },
                    Data: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Track3JIS].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Ddi))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Ddi).IsTrue("DDI data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Ddi].DataStatus, "Unexpected DDI data status is set by the device class. DataStatus field should not be null.");
                ddi = new CardDataNullableClass(
                    Status: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Ddi].DataStatus switch
                    {
                        ReadCardResult.CardData.DataStatusEnum.DataMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataMissing,
                        ReadCardResult.CardData.DataStatusEnum.DataInvalid => XFS4IoT.CardReader.CardDataStatusEnum.DataInvalid,
                        ReadCardResult.CardData.DataStatusEnum.DataTooLong => XFS4IoT.CardReader.CardDataStatusEnum.DataTooLong,
                        ReadCardResult.CardData.DataStatusEnum.DataTooShort => XFS4IoT.CardReader.CardDataStatusEnum.DataTooShort,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceNotSupported => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceNotSupported,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceMissing,
                        _ => null
                    },
                    Data: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Ddi].Data);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Security))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.Security).IsTrue("Security data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Security].DataStatus, "Unexpected security data status is set by the device class. DataStatus field should not be null.");
                security = new ReadRawDataCompletion.PayloadData.SecurityClass(
                    Status: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Security].DataStatus switch
                    {
                        ReadCardResult.CardData.DataStatusEnum.DataMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataMissing,
                        ReadCardResult.CardData.DataStatusEnum.DataInvalid => XFS4IoT.CardReader.CardDataStatusEnum.DataInvalid,
                        ReadCardResult.CardData.DataStatusEnum.DataTooLong => XFS4IoT.CardReader.CardDataStatusEnum.DataTooLong,
                        ReadCardResult.CardData.DataStatusEnum.DataTooShort => XFS4IoT.CardReader.CardDataStatusEnum.DataTooShort,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceNotSupported => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceNotSupported,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceMissing,
                        _ => null
                    },
                    Data: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.Security].SecutiryDataStatus);
            }

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.MemoryChip))
            {
                readCardDataResult.DataRead.ContainsKey(ReadCardRequest.CardDataTypesEnum.MemoryChip).IsTrue("MemoryChip data is not set by the device class.");
                Contracts.IsNotNull(readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].DataStatus, "Unexpected memocy chip data status is set by the device class. DataStatus field should not be null.");

                // Bug in the 2023-2 specification. missing pattern and format keywords in the spec.
                // Should be fixed in the next version.
                List<byte> memoryChipData = null;
                if (readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].Data.Count > 0)
                {
                    memoryChipData = readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].Data;
                }

                memoryChip = new ReadRawDataCompletion.PayloadData.MemoryChipClass(
                    Status: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].DataStatus switch
                    {
                        ReadCardResult.CardData.DataStatusEnum.DataMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataMissing,
                        ReadCardResult.CardData.DataStatusEnum.DataInvalid => XFS4IoT.CardReader.CardDataStatusEnum.DataInvalid,
                        ReadCardResult.CardData.DataStatusEnum.DataTooLong => XFS4IoT.CardReader.CardDataStatusEnum.DataTooLong,
                        ReadCardResult.CardData.DataStatusEnum.DataTooShort => XFS4IoT.CardReader.CardDataStatusEnum.DataTooShort,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceNotSupported => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceNotSupported,
                        ReadCardResult.CardData.DataStatusEnum.DataSourceMissing => XFS4IoT.CardReader.CardDataStatusEnum.DataSourceMissing,
                        _ => null
                    },
                    Protocol: readCardDataResult.DataRead[ReadCardRequest.CardDataTypesEnum.MemoryChip].MemcoryChipDataStatus,
                    Data: memoryChipData);
            }

            List<CardDataClass> chip = null;

            if (dataTypes.HasFlag(ReadCardRequest.CardDataTypesEnum.Chip) &&
                readCardDataResult.ChipATRRead is not null && 
                readCardDataResult.ChipATRRead.Count > 0)
            {
                chip = [];
                foreach (ReadCardResult.CardData atr in readCardDataResult.ChipATRRead)
                {
                    Contracts.IsNotNull(atr.DataStatus, "Unexpected chip data status is set by the device class. DataStatus field should not be null.");
                    chip.Add(new CardDataClass((CardDataStatusEnum)atr.DataStatus,
                                               atr.Data));
                }
            }

            ReadRawDataCompletion.PayloadData payload = null;
            if (readCardDataResult.ErrorCode is not null ||
                track1 is not null ||
                track2 is not null ||
                track3 is not null ||
                chip is not null ||
                security is not null ||
                watermark is not null ||
                memoryChip is not null ||
                track1Front is not null ||
                frontImage is not null ||
                backImage is not null ||
                track1JIS is not null ||
                track3JIS is not null ||
                ddi is not null)
            {
                payload = new(
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

            return new(
                payload,
                readCardDataResult.CompletionCode,
                readCardDataResult.ErrorDescription);
        }
    }
}
