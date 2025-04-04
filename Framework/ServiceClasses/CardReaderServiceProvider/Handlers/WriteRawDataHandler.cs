/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CardReader
{
    public partial class WriteRawDataHandler
    {
        private async Task<CommandResult<WriteRawDataCompletion.PayloadData>> HandleWriteRawData(IWriteRawDataEvents events, WriteRawDataCommand writeRawData, CancellationToken cancel)
        {
            Dictionary<WriteCardRequest.DestinationEnum, WriteCardRequest.CardData> dataToWrite = [];
            if (writeRawData.Payload.Track1 is null &&
                writeRawData.Payload.Track2 is null &&
                writeRawData.Payload.Track3 is null &&
                writeRawData.Payload.Track1JIS is null &&
                writeRawData.Payload.Track1Front is null &&
                writeRawData.Payload.Track3JIS is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    "No track data is supplied.");
            }


            if (writeRawData.Payload.Track1 is not null)
            {
                dataToWrite.Add(
                    WriteCardRequest.DestinationEnum.Track1,
                    new WriteCardRequest.CardData(
                        WriteMethod: writeRawData.Payload.Track1.WriteMethod switch
                        {
                            WriteRawDataCommand.PayloadData.Track1Class.WriteMethodEnum.Hico => WriteCardRequest.CardData.WriteMethodEnum.Hico,
                            WriteRawDataCommand.PayloadData.Track1Class.WriteMethodEnum.Loco => WriteCardRequest.CardData.WriteMethodEnum.Loco,
                            _ => WriteCardRequest.CardData.WriteMethodEnum.Auto,
                        },
                        Data: writeRawData.Payload.Track1.Data)
                    );
            }
            if (writeRawData.Payload.Track2 is not null)
            {
                dataToWrite.Add(
                    WriteCardRequest.DestinationEnum.Track2,
                    new WriteCardRequest.CardData(
                        WriteMethod: writeRawData.Payload.Track2.WriteMethod switch
                        {
                            WriteRawDataCommand.PayloadData.Track2Class.WriteMethodEnum.Hico => WriteCardRequest.CardData.WriteMethodEnum.Hico,
                            WriteRawDataCommand.PayloadData.Track2Class.WriteMethodEnum.Loco => WriteCardRequest.CardData.WriteMethodEnum.Loco,
                            _ => WriteCardRequest.CardData.WriteMethodEnum.Auto,
                        },
                        Data: writeRawData.Payload.Track2.Data)
                    );
            }
            if (writeRawData.Payload.Track3 is not null)
            {
                dataToWrite.Add(
                    WriteCardRequest.DestinationEnum.Track3,
                    new WriteCardRequest.CardData(
                        WriteMethod: writeRawData.Payload.Track3.WriteMethod switch
                        {
                            WriteRawDataCommand.PayloadData.Track3Class.WriteMethodEnum.Hico => WriteCardRequest.CardData.WriteMethodEnum.Hico,
                            WriteRawDataCommand.PayloadData.Track3Class.WriteMethodEnum.Loco => WriteCardRequest.CardData.WriteMethodEnum.Loco,
                            _ => WriteCardRequest.CardData.WriteMethodEnum.Auto,
                        },
                        Data: writeRawData.Payload.Track3.Data)
                    );
            }
            if (writeRawData.Payload.Track1JIS is not null)
            {
                dataToWrite.Add(
                    WriteCardRequest.DestinationEnum.Track1JIS,
                    new WriteCardRequest.CardData(
                        WriteMethod: writeRawData.Payload.Track1JIS.WriteMethod switch
                        {
                            WriteRawDataCommand.PayloadData.Track1JISClass.WriteMethodEnum.Hico => WriteCardRequest.CardData.WriteMethodEnum.Hico,
                            WriteRawDataCommand.PayloadData.Track1JISClass.WriteMethodEnum.Loco => WriteCardRequest.CardData.WriteMethodEnum.Loco,
                            _ => WriteCardRequest.CardData.WriteMethodEnum.Auto,
                        },
                        Data: writeRawData.Payload.Track1JIS.Data)
                    );
}
            if (writeRawData.Payload.Track1Front is not null)
            {
                dataToWrite.Add(
                    WriteCardRequest.DestinationEnum.Track1Front,
                    new WriteCardRequest.CardData(
                        WriteMethod: writeRawData.Payload.Track1Front.WriteMethod switch
                        {
                            WriteRawDataCommand.PayloadData.Track1FrontClass.WriteMethodEnum.Hico => WriteCardRequest.CardData.WriteMethodEnum.Hico,
                            WriteRawDataCommand.PayloadData.Track1FrontClass.WriteMethodEnum.Loco => WriteCardRequest.CardData.WriteMethodEnum.Loco,
                            _ => WriteCardRequest.CardData.WriteMethodEnum.Auto,
                        },
                        Data: writeRawData.Payload.Track1Front.Data)
                    );
            }
            if (writeRawData.Payload.Track3JIS is not null)
            {
                dataToWrite.Add(
                    WriteCardRequest.DestinationEnum.Track3JIS,
                    new WriteCardRequest.CardData(
                        WriteMethod: writeRawData.Payload.Track3JIS.WriteMethod switch
                        {
                            WriteRawDataCommand.PayloadData.Track3JISClass.WriteMethodEnum.Hico => WriteCardRequest.CardData.WriteMethodEnum.Hico,
                            WriteRawDataCommand.PayloadData.Track3JISClass.WriteMethodEnum.Loco => WriteCardRequest.CardData.WriteMethodEnum.Loco,
                            _ => WriteCardRequest.CardData.WriteMethodEnum.Auto,
                        },
                        Data: writeRawData.Payload.Track3JIS.Data)
                    );
            }

            foreach (var data in dataToWrite)
            {
                // First data check
                if (data.Key == WriteCardRequest.DestinationEnum.Track1 &&
                    !Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1) ||
                    data.Key == WriteCardRequest.DestinationEnum.Track2 &&
                    !Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track2) ||
                    data.Key == WriteCardRequest.DestinationEnum.Track3 &&
                    !Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3) ||
                    data.Key == WriteCardRequest.DestinationEnum.Track1Front &&
                    !Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1Front) ||
                    data.Key == WriteCardRequest.DestinationEnum.Track1JIS &&
                    !Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1JIS) ||
                    data.Key == WriteCardRequest.DestinationEnum.Track3JIS &&
                    !Common.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3JIS))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified destination is not supported. {data.Key}");
                }

                if (data.Value.WriteMethod == WriteCardRequest.CardData.WriteMethodEnum.Auto &&
                    !Common.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Auto) ||
                    data.Value.WriteMethod == WriteCardRequest.CardData.WriteMethodEnum.Loco &&
                    !Common.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Loco) ||
                    data.Value.WriteMethod == WriteCardRequest.CardData.WriteMethodEnum.Hico &&
                    !Common.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Hico))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Specified write methods is not supported. {data.Value.WriteMethod}");
                }

                if (data.Value.Data is null ||
                    data.Value.Data.Count == 0)
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        "No data specified to write track.");
                }

                List<byte> writeData = data.Value.Data;
                if (!ValidateData(data.Key, writeData))
                {
                    return new(
                        MessageHeader.CompletionCodeEnum.InvalidData,
                        $"Invalid track data is specified for the {data.Key}");
                }
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.AcceptCardAsync()");

            var acceptCardResult = await Device.AcceptCardAsync(
                new ReadRawDataCommandEvents(events),
                new AcceptCardRequest(
                    ReadCardRequest.CardDataTypesEnum.NoDataRead, 
                    false, 
                    writeRawData.Header.Timeout ?? 0),
                cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.AcceptCardAsync() -> {acceptCardResult.CompletionCode}, {acceptCardResult.ErrorCode}");

            if (acceptCardResult.CompletionCode != MessageHeader.CompletionCodeEnum.Success)
            {
                // Map to XFS erro code
                WriteRawDataCompletion.PayloadData.ErrorCodeEnum? errorCode = acceptCardResult.ErrorCode switch
                {
                    AcceptCardResult.ErrorCodeEnum.MediaJam => WriteRawDataCompletion.PayloadData.ErrorCodeEnum.MediaJam,
                    AcceptCardResult.ErrorCodeEnum.ShutterFail => WriteRawDataCompletion.PayloadData.ErrorCodeEnum.ShutterFail,
                    AcceptCardResult.ErrorCodeEnum.NoMedia => WriteRawDataCompletion.PayloadData.ErrorCodeEnum.NoMedia,
                    AcceptCardResult.ErrorCodeEnum.InvalidMedia => WriteRawDataCompletion.PayloadData.ErrorCodeEnum.InvalidMedia,
                    AcceptCardResult.ErrorCodeEnum.CardTooShort => WriteRawDataCompletion.PayloadData.ErrorCodeEnum.CardTooShort,
                    AcceptCardResult.ErrorCodeEnum.CardTooLong => WriteRawDataCompletion.PayloadData.ErrorCodeEnum.CardTooLong,
                    _ => null
                };


                return new(
                    new(errorCode),
                    acceptCardResult.CompletionCode,
                    acceptCardResult.ErrorDescription);
            }

            // The device specific class completed accepting card operation check the media status must be present for motorised cardreader before writing data.
            if (Common.CardReaderCapabilities.Type == CardReaderCapabilitiesClass.DeviceTypeEnum.Motor &&
                (Common.CardReaderStatus.Media != CardReaderStatusClass.MediaEnum.Present &&
                 Common.CardReaderStatus.Media != CardReaderStatusClass.MediaEnum.NotSupported))
            {
                return new(
                    new(WriteRawDataCompletion.PayloadData.ErrorCodeEnum.NoMedia),
                    MessageHeader.CompletionCodeEnum.HardwareError,
                    "Accept operation is completed successfully, but the media is not present.");
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.WriteCardDataAsync()");
            var writeCardDataResult = await Device.WriteCardAsync(new WriteRawDataCommandEvents(events),
                                                                  new WriteCardRequest(dataToWrite),
                                                                  cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.WriteCardDataAsync() -> {writeCardDataResult.CompletionCode}, {writeCardDataResult.ErrorCode}");

            return new(
                writeCardDataResult.ErrorCode is not null ? new(writeCardDataResult.ErrorCode) : null,
                writeCardDataResult.CompletionCode,
                writeCardDataResult.ErrorDescription);
        }

        /// <summary>
        /// ValidateData
        /// Validate track data
        /// </summary>
        /// <param name="Track">Track data to validate</param>
        /// <param name="Data">Data to validate</param>
        /// <returns></returns>
        private bool ValidateData(WriteCardRequest.DestinationEnum Track, List<byte> Data)
        {
            if (validTrackDataRange.TryGetValue(Track, out ValidTrackDataRange value))
            {
                if (Data.Count > value.MaxLength)
                    return false;

                for (int i=0; i<Data.Count; i++)
                {
                    if (Data[i] < value.MinLegal ||
                        Data[i] > value.MaxLegal)
                    {
                        return false;
                    }

                    if (i < Data.Count - 1 && Data[i] == value.EndSentinel)
                        return false;
                }
            }

            return true;
        }

        private class ValidTrackDataRange(
            int MaxLength, 
            byte MinLegal, 
            byte MaxLegal, 
            byte StartSentinel, 
            byte EndSentinel)
        {
            public int MaxLength { get; private set; } = MaxLength;
            public byte MinLegal { get; private set; } = MinLegal;
            public byte MaxLegal { get; private set; } = MaxLegal;
            public byte StartSentinel { get; private set; } = StartSentinel;
            public byte EndSentinel { get; private set; } = EndSentinel;
        }

        private readonly Dictionary<WriteCardRequest.DestinationEnum, ValidTrackDataRange> validTrackDataRange = new()
        {
            { WriteCardRequest.DestinationEnum.Track1, new ValidTrackDataRange(78,  0x20, 0x5f, 0x25, 0x3f) },
            { WriteCardRequest.DestinationEnum.Track2, new ValidTrackDataRange(39,  0x30, 0x3e, 0x3b, 0x3f)},
            { WriteCardRequest.DestinationEnum.Track3, new ValidTrackDataRange(106, 0x30, 0x3e, 0x3b, 0x3f)},
        };
    }
}
