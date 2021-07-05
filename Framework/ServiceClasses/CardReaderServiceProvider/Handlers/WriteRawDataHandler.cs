/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
        private async Task<WriteRawDataCompletion.PayloadData> HandleWriteRawData(IWriteRawDataEvents events, WriteRawDataCommand writeRawData, CancellationToken cancel)
        {
            Dictionary<WriteRawDataCommand.PayloadData.DataClass.DestinationEnum, WriteCardRequest.CardData> dataToWrite = new ();
            foreach (WriteRawDataCommand.PayloadData.DataClass data in writeRawData.Payload.Data)
            {
                // First data check
                if (data.Destination is null)
                {
                    return new WriteRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  "No destination specified to write track data.");
                }
                
                if (data.Destination == WriteRawDataCommand.PayloadData.DataClass.DestinationEnum.Track1 &&
                    !CardReader.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1) ||
                    data.Destination == WriteRawDataCommand.PayloadData.DataClass.DestinationEnum.Track2 &&
                    !CardReader.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track2) ||
                    data.Destination == WriteRawDataCommand.PayloadData.DataClass.DestinationEnum.Track3 &&
                    !CardReader.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3) ||
                    data.Destination == WriteRawDataCommand.PayloadData.DataClass.DestinationEnum.Track1Front &&
                    !CardReader.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1Front) ||
                    data.Destination == WriteRawDataCommand.PayloadData.DataClass.DestinationEnum.Track1JIS &&
                    !CardReader.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track1JIS) ||
                    data.Destination == WriteRawDataCommand.PayloadData.DataClass.DestinationEnum.Track3JIS &&
                    !CardReader.CardReaderCapabilities.WriteTracks.HasFlag(CardReaderCapabilitiesClass.WritableDataTypesEnum.Track3JIS))
                {
                    return new WriteRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Specified destination is not supported. {data.Destination}");
                }

                if (data.WriteMethod is not null)
                {
                    if (data.WriteMethod == WriteRawDataCommand.PayloadData.DataClass.WriteMethodEnum.Auto &&
                        !CardReader.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Auto) ||
                        data.WriteMethod == WriteRawDataCommand.PayloadData.DataClass.WriteMethodEnum.Loco &&
                        !CardReader.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Loco) ||
                        data.WriteMethod == WriteRawDataCommand.PayloadData.DataClass.WriteMethodEnum.Hico &&
                        !CardReader.CardReaderCapabilities.WriteMode.HasFlag(CardReaderCapabilitiesClass.WriteMethodsEnum.Hico))
                    {
                        return new WriteRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                      $"Specified write methods is not supported. {data.WriteMethod}");
                    }
                }

                if (string.IsNullOrEmpty(data.Data))
                {
                    return new WriteRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  "No data specified to write track.");
                }

                List<byte> writeData = new (Convert.FromBase64String(data.Data));
                if (!ValidateData((WriteRawDataCommand.PayloadData.DataClass.DestinationEnum)data.Destination, writeData))
                {
                    return new WriteRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                  $"Invalid track data is specified for the {data.Destination}");
                }

                // Data seems to be ok and add it in the list to write data to the device class
                dataToWrite.Add((WriteRawDataCommand.PayloadData.DataClass.DestinationEnum)data.Destination,
                                 new WriteCardRequest.CardData(writeData, data.WriteMethod));
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.AcceptCardAsync()");

            IAcceptCardEvents newEvents = new AcceptCardEvents((WriteRawDataEvents)events);
            newEvents.IsNotNull($"Invalid event reference. {nameof(AcceptCardEvents)}");

            var acceptCardResult = await Device.AcceptCardAsync(newEvents,
                                                                new AcceptCardRequest(ReadCardRequest.CardDataTypesEnum.NoDataRead, false, writeRawData.Payload.Timeout),
                                                                cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.AcceptCardAsync() -> {acceptCardResult.CompletionCode}, {acceptCardResult.ErrorCode}");

            if (acceptCardResult.CompletionCode != MessagePayload.CompletionCodeEnum.Success)
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


                return new WriteRawDataCompletion.PayloadData(acceptCardResult.CompletionCode,
                                                              acceptCardResult.ErrorDescription,
                                                              errorCode);
            }

            // The device specific class completed accepting card operation check the media status must be present for motorised cardreader before writing data.
            if (Device.DeviceType == DeviceTypeEnum.Motor &&
                (Device.MediaStatus != MediaStatusEnum.Present &&
                 Device.MediaStatus != MediaStatusEnum.NotSupported))
            {
                return new WriteRawDataCompletion.PayloadData(MessagePayload.CompletionCodeEnum.HardwareError,
                                                              "Accept operation is completed successfully, but the media is not present.",
                                                              WriteRawDataCompletion.PayloadData.ErrorCodeEnum.NoMedia);
            }

            Logger.Log(Constants.DeviceClass, "CardReaderDev.WriteCardDataAsync()");
            var writeCardDataResult = await Device.WriteCardAsync(events,
                                                                  new WriteCardRequest(dataToWrite),
                                                                  cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.WriteCardDataAsync() -> {writeCardDataResult.CompletionCode}, {writeCardDataResult.ErrorCode}");

            return new WriteRawDataCompletion.PayloadData(writeCardDataResult.CompletionCode,
                                                          writeCardDataResult.ErrorDescription,
                                                          writeCardDataResult.ErrorCode);
        }

        /// <summary>
        /// ValidateData
        /// Validate track data
        /// </summary>
        /// <param name="Track">Track data to validate</param>
        /// <param name="Data">Data to validate</param>
        /// <returns></returns>
        private bool ValidateData(WriteRawDataCommand.PayloadData.DataClass.DestinationEnum Track, List<byte> Data)
        {
            if (validTrackDataRange.ContainsKey(Track))
            {
                if (Data.Count > validTrackDataRange[Track].MaxLength)
                    return false;

                for (int i=0; i<Data.Count; i++)
                {
                    if (Data[i] < validTrackDataRange[Track].MinLegal ||
                        Data[i] > validTrackDataRange[Track].MaxLegal)
                    {
                        return false;
                    }

                    if (i < Data.Count - 1 && Data[i] == validTrackDataRange[Track].EndSentinel)
                        return false;
                }
            }

            return true;
        }

        private class ValidTrackDataRange
        {
            public ValidTrackDataRange(int MaxLength, byte MinLegal, byte MaxLegal, byte StartSentinel, byte EndSentinel)
            {
                this.MaxLength = MaxLength;
                this.MinLegal = MinLegal;
                this.MaxLegal = MaxLegal;
                this.StartSentinel = StartSentinel;
                this.EndSentinel = EndSentinel;
            }

            public int MaxLength { get; private set; }
            public byte MinLegal { get; private set; }
            public byte MaxLegal { get; private set; }
            public byte StartSentinel { get; private set; }
            public byte EndSentinel { get; private set; }
        }

        private readonly Dictionary<WriteRawDataCommand.PayloadData.DataClass.DestinationEnum, ValidTrackDataRange> validTrackDataRange = new()
        {
            { WriteRawDataCommand.PayloadData.DataClass.DestinationEnum.Track1, new ValidTrackDataRange(78,  0x20, 0x5f, 0x25, 0x3f) },
            { WriteRawDataCommand.PayloadData.DataClass.DestinationEnum.Track2, new ValidTrackDataRange(39,  0x30, 0x3e, 0x3b, 0x3f)},
            { WriteRawDataCommand.PayloadData.DataClass.DestinationEnum.Track3, new ValidTrackDataRange(106, 0x30, 0x3e, 0x3b, 0x3f)},
        };
    }
}
