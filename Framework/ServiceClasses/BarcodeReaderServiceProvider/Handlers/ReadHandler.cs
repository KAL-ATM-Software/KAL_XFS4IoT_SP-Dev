/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.BarcodeReader.Commands;
using XFS4IoT.BarcodeReader.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.BarcodeReader
{
    public partial class ReadHandler
    {
        private async Task<CommandResult<ReadCompletion.PayloadData>> HandleRead(IReadEvents events, ReadCommand read, CancellationToken cancel)
        {
            if (Common.BarcodeReaderCapabilities.Symbologies == BarcodeReaderCapabilitiesClass.SymbologiesEnum.NotSupported)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.UnsupportedCommand, 
                    $"No barcode symbologies are supported by the device.");
            }

            BarcodeReaderCapabilitiesClass.SymbologiesEnum symbologiesToRead = 0;

            if (read.Payload.Symbologies is null)
            {
                //When Symbologies is null, read all Symbologies supported by the device.
                symbologiesToRead = Common.BarcodeReaderCapabilities.Symbologies;
            }

            else if (read.Payload.Symbologies?.AustralianPost is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.AustralianPost) ||
                read.Payload.Symbologies?.Aztec is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.AztecCodes) ||
                read.Payload.Symbologies?.CanadianPost is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CanadianPost) ||
                read.Payload.Symbologies?.ChannelCode is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CHANNELCODE) ||
                read.Payload.Symbologies?.ChinesePost is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.ChinesePost) ||
                read.Payload.Symbologies?.Codabar is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODABAR) ||
                read.Payload.Symbologies?.CodablockF is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CodablockF) ||
                read.Payload.Symbologies?.Code11 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE11) ||
                read.Payload.Symbologies?.Code128 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE128) ||
                read.Payload.Symbologies?.Code16K is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.Code16K) ||
                read.Payload.Symbologies?.Code39 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE39) ||
                read.Payload.Symbologies?.Code49 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE49) ||
                read.Payload.Symbologies?.Code93 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE93) ||
                read.Payload.Symbologies?.CodeOne is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODEONE) ||
                read.Payload.Symbologies?.CompositeCodeA is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeA) ||
                read.Payload.Symbologies?.CompositeCodeB is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeB) ||
                read.Payload.Symbologies?.CompositeCodeC is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeC) ||
                read.Payload.Symbologies?.DataMatrix is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.DataMatrix) ||
                read.Payload.Symbologies?.Ean128 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN128) ||
                read.Payload.Symbologies?.Ean13 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13) ||
                read.Payload.Symbologies?.Ean13_2 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13_2) ||
                read.Payload.Symbologies?.Ean13_5 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13_5) ||
                read.Payload.Symbologies?.Ean8 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8) ||
                read.Payload.Symbologies?.Ean8_2 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8_2) ||
                read.Payload.Symbologies?.Ean8_5 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8_5) ||
                read.Payload.Symbologies?.Itf is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.ITF) ||
                read.Payload.Symbologies?.Jan13 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.JAN13) ||
                read.Payload.Symbologies?.JapanesePost is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.JapanesePost) ||
                read.Payload.Symbologies?.KoreanPost is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.KoreanPost) ||
                read.Payload.Symbologies?.MaxiCode is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.MAXICODE) ||
                read.Payload.Symbologies?.MicroPdf417 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.MICROPDF_417) ||
                read.Payload.Symbologies?.Msi is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.MSI) ||
                read.Payload.Symbologies?.NetherlandsPost is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.NetherlandsPost) ||
                read.Payload.Symbologies?.Pdf417 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PDF_417) ||
                read.Payload.Symbologies?.Planet is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.USPlanet) ||
                read.Payload.Symbologies?.Plessey is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PLESSEY) ||
                read.Payload.Symbologies?.PosiCodeA is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PosiCodeA) ||
                read.Payload.Symbologies?.PosiCodeB is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PosiCodeB) ||
                read.Payload.Symbologies?.Postnet is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.USPostnet) ||
                read.Payload.Symbologies?.QrCode is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.QRCode) ||
                read.Payload.Symbologies?.Rss is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS) ||
                read.Payload.Symbologies?.RssExpanded is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS_EXPANDED) ||
                read.Payload.Symbologies?.RssRestricted is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS_RESTRICTED) ||
                read.Payload.Symbologies?.Std2Of5 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.STD2OF5) ||
                read.Payload.Symbologies?.Std2Of5Iata is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.STD2OF5_IATA) ||
                read.Payload.Symbologies?.TelepenAim is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.TELEPEN_AIM) ||
                read.Payload.Symbologies?.TelepenOriginal is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.TELEPEN_ORIGINAL) ||
                read.Payload.Symbologies?.TriopticCode39 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.TriopticCode39) ||
                read.Payload.Symbologies?.UkPost is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UKPost) ||
                read.Payload.Symbologies?.UpcA is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA) ||
                read.Payload.Symbologies?.UpcA_2 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA_2) ||
                read.Payload.Symbologies?.UpcA_5 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA_5) ||
                read.Payload.Symbologies?.UpcE0 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0) ||
                read.Payload.Symbologies?.UpcE0_2 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0_2) ||
                read.Payload.Symbologies?.UpcE0_5 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0_5) ||
                read.Payload.Symbologies?.UpcE1 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1) ||
                read.Payload.Symbologies?.UpcE1_2 is true &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1_2) ||
                read.Payload.Symbologies?.UpcE1_5 is true && 
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1_5))
            {
                return new(MessageHeader.CompletionCodeEnum.UnsupportedData,
                           $"One or more barcode symbologies specified are not supported by the device. {read.Payload.Symbologies}");
            }

            if (read.Payload.Symbologies?.AustralianPost is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.AustralianPost;
            }
            if (read.Payload.Symbologies?.Aztec is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.AztecCodes;
            }
            if (read.Payload.Symbologies?.CanadianPost is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CanadianPost;
            }
            if (read.Payload.Symbologies?.ChannelCode is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CHANNELCODE;
            }
            if (read.Payload.Symbologies?.ChinesePost is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.ChinesePost;
            }
            if (read.Payload.Symbologies?.Codabar is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODABAR;
            }
            if (read.Payload.Symbologies?.CodablockF is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CodablockF;
            }
            if (read.Payload.Symbologies?.Code11 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE11;
            }
            if (read.Payload.Symbologies?.Code128 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE128;
            }
            if (read.Payload.Symbologies?.Code16K is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.Code16K;
            }
            if (read.Payload.Symbologies?.Code39 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE39;
            }
            if (read.Payload.Symbologies?.Code49 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE49;
            }
            if (read.Payload.Symbologies?.Code93 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE93;
            }
            if (read.Payload.Symbologies?.CodeOne is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODEONE;
            }
            if (read.Payload.Symbologies?.CompositeCodeA is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeA;
            }
            if (read.Payload.Symbologies?.CompositeCodeB is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeB;
            }
            if (read.Payload.Symbologies?.CompositeCodeC is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeC;
            }
            if (read.Payload.Symbologies?.DataMatrix is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.DataMatrix;
            }
            if (read.Payload.Symbologies?.Ean128 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN128;
            }
            if (read.Payload.Symbologies?.Ean13 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13;
            }
            if (read.Payload.Symbologies?.Ean13_2 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13_2;
            }
            if (read.Payload.Symbologies?.Ean13_5 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13_5;
            }
            if (read.Payload.Symbologies?.Ean8 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8;
            }
            if (read.Payload.Symbologies?.Ean8_2 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8_2;
            }
            if (read.Payload.Symbologies?.Ean8_5 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8_5;
            }
            if (read.Payload.Symbologies?.Itf is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.ITF;
            }
            if (read.Payload.Symbologies?.Jan13 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.JAN13;
            }
            if (read.Payload.Symbologies?.JapanesePost is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.JapanesePost;
            }
            if (read.Payload.Symbologies?.KoreanPost is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.KoreanPost;
            }
            if (read.Payload.Symbologies?.MaxiCode is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.MAXICODE;
            }
            if (read.Payload.Symbologies?.MicroPdf417 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.MICROPDF_417;
            }
            if (read.Payload.Symbologies?.Msi is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.MSI;
            }
            if (read.Payload.Symbologies?.NetherlandsPost is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.NetherlandsPost;
            }
            if (read.Payload.Symbologies?.Pdf417 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.PDF_417;
            }
            if (read.Payload.Symbologies?.Planet is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.USPlanet;
            }
            if (read.Payload.Symbologies?.Plessey is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.PLESSEY;
            }
            if (read.Payload.Symbologies?.PosiCodeA is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.PosiCodeA;
            }
            if (read.Payload.Symbologies?.PosiCodeB is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.PosiCodeB;
            }
            if (read.Payload.Symbologies?.Postnet is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.USPostnet;
            }
            if (read.Payload.Symbologies?.QrCode is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.QRCode;
            }
            if (read.Payload.Symbologies?.Rss is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS;
            }
            if (read.Payload.Symbologies?.RssExpanded is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS_EXPANDED;
            }
            if (read.Payload.Symbologies?.RssRestricted is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS_RESTRICTED;
            }
            if (read.Payload.Symbologies?.Std2Of5 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.STD2OF5;
            }
            if (read.Payload.Symbologies?.Std2Of5Iata is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.STD2OF5_IATA;
            }
            if (read.Payload.Symbologies?.TelepenAim is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.TELEPEN_AIM;
            }
            if (read.Payload.Symbologies?.TelepenOriginal is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.TELEPEN_ORIGINAL;
            }
            if (read.Payload.Symbologies?.TriopticCode39 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.TriopticCode39;
            }
            if (read.Payload.Symbologies?.UkPost is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UKPost;
            }
            if (read.Payload.Symbologies?.UpcA is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA;
            }
            if (read.Payload.Symbologies?.UpcA_2 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA_2;
            }
            if (read.Payload.Symbologies?.UpcA_5 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA_5;
            }
            if (read.Payload.Symbologies?.UpcE0 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0;
            }
            if (read.Payload.Symbologies?.UpcE0_2 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0_2;
            }
            if (read.Payload.Symbologies?.UpcE0_5 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0_5;
            }
            if (read.Payload.Symbologies?.UpcE1 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1;
            }
            if (read.Payload.Symbologies?.UpcE1_2 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1_2;
            }
            if (read.Payload.Symbologies?.UpcE1_5 is true)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1_5;
            }

            Logger.Log(Constants.DeviceClass, "BarcodeReaderDev.Read()");

            var result = await Device.Read(new ReadRequest(symbologiesToRead, read.Header.Timeout ?? 0), cancel);

            Logger.Log(Constants.DeviceClass, $"BarcodeReaderDev.Read() -> {result.CompletionCode} {result.ErrorCode}");

            List<ReadCompletion.PayloadData.ReadOutputClass> output = null;

            if (result.ReadData is not null)
            {
                output = [];
                foreach (ReadResult.ReadBarcodeData data in result.ReadData)
                {
                    ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum? symbol = data.SymbologyRead switch
                    {
                        ReadResult.SymbologyEnum.AustralianPost => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.AustralianPost,
                        ReadResult.SymbologyEnum.AztecCodes => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Aztec,
                        ReadResult.SymbologyEnum.CanadianPost => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.CanadianPost,
                        ReadResult.SymbologyEnum.CHANNELCODE => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.ChannelCode,
                        ReadResult.SymbologyEnum.ChinesePost => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.ChinesePost,
                        ReadResult.SymbologyEnum.CODABAR => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Codabar,
                        ReadResult.SymbologyEnum.CodablockF => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.CodablockF,
                        ReadResult.SymbologyEnum.CODE11 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Code11,
                        ReadResult.SymbologyEnum.CODE128 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Code128,
                        ReadResult.SymbologyEnum.Code16K => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Code16K,
                        ReadResult.SymbologyEnum.CODE39 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Code39,
                        ReadResult.SymbologyEnum.CODE49 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Code49,
                        ReadResult.SymbologyEnum.CODE93 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Code93,
                        ReadResult.SymbologyEnum.CODEONE => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.CodeOne,
                        ReadResult.SymbologyEnum.CompositeCodeA => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.CompositeCodeA,
                        ReadResult.SymbologyEnum.CompositeCodeB => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.CompositeCodeB,
                        ReadResult.SymbologyEnum.CompositeCodeC => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.CompositeCodeC,
                        ReadResult.SymbologyEnum.DataMatrix => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.DataMatrix,
                        ReadResult.SymbologyEnum.EAN128 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Ean128,
                        ReadResult.SymbologyEnum.EAN13 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Ean13,
                        ReadResult.SymbologyEnum.EAN13_2 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Ean13_2,
                        ReadResult.SymbologyEnum.EAN13_5 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Ean13_5,
                        ReadResult.SymbologyEnum.EAN8 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Ean8,
                        ReadResult.SymbologyEnum.EAN8_2 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Ean8_2,
                        ReadResult.SymbologyEnum.EAN8_5 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Ean8_5,
                        ReadResult.SymbologyEnum.ITF => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Itf,
                        ReadResult.SymbologyEnum.JAN13 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Jan13,
                        ReadResult.SymbologyEnum.JapanesePost => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.JapanesePost,
                        ReadResult.SymbologyEnum.KoreanPost => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.KoreanPost,
                        ReadResult.SymbologyEnum.MAXICODE => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.MaxiCode,
                        ReadResult.SymbologyEnum.MICROPDF_417 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.MicroPdf417,
                        ReadResult.SymbologyEnum.MSI => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Msi,
                        ReadResult.SymbologyEnum.NetherlandsPost => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.NetherlandsPost,
                        ReadResult.SymbologyEnum.PDF_417 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Pdf417,
                        ReadResult.SymbologyEnum.PLESSEY => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Plessey,
                        ReadResult.SymbologyEnum.PosiCodeA => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.PosiCodeA,
                        ReadResult.SymbologyEnum.PosiCodeB => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.PosiCodeB,
                        ReadResult.SymbologyEnum.QRCode => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.QrCode,
                        ReadResult.SymbologyEnum.RSS => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Rss,
                        ReadResult.SymbologyEnum.RSS_EXPANDED => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.RssExpanded,
                        ReadResult.SymbologyEnum.RSS_RESTRICTED => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.RssRestricted,
                        ReadResult.SymbologyEnum.STD2OF5 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Std2Of5,
                        ReadResult.SymbologyEnum.STD2OF5_IATA => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Std2Of5Iata,
                        ReadResult.SymbologyEnum.TELEPEN_AIM => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.TelepenAim,
                        ReadResult.SymbologyEnum.TELEPEN_ORIGINAL => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.TelepenOriginal,
                        ReadResult.SymbologyEnum.TriopticCode39 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.TriopticCode39,
                        ReadResult.SymbologyEnum.UKPost => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.UkPost,
                        ReadResult.SymbologyEnum.UPCA => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.UpcA,
                        ReadResult.SymbologyEnum.UPCA_2 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.UpcA_2,
                        ReadResult.SymbologyEnum.UPCA_5 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.UpcA_5,
                        ReadResult.SymbologyEnum.UPCE0 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.UpcE0,
                        ReadResult.SymbologyEnum.UPCE0_2 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.UpcE0_2,
                        ReadResult.SymbologyEnum.UPCE0_5 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.UpcE0_5,
                        ReadResult.SymbologyEnum.UPCE1 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.UpcE1,
                        ReadResult.SymbologyEnum.UPCE1_2 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.UpcE1_2,
                        ReadResult.SymbologyEnum.UPCE1_5 => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.UpcE1_5,
                        ReadResult.SymbologyEnum.USPlanet => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Planet,
                        ReadResult.SymbologyEnum.USPostnet => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.Postnet,
                        ReadResult.SymbologyEnum.SymbologyUnknown => ReadCompletion.PayloadData.ReadOutputClass.SymbologyEnum.SymbologyUnknown,
                        _ => null,
                    };
                    output.Add(new ReadCompletion.PayloadData.ReadOutputClass(symbol, data.Data, data.VendorSymbologyName));
                }
            }

            return new(
                new(
                    result.ErrorCode is not null ? result.ErrorCode switch
                    {
                        ReadResult.ErrorCodeEnum.BarcodeInvalid => ReadCompletion.PayloadData.ErrorCodeEnum.BarcodeInvalid,
                        _ => throw new InternalErrorException($"Unsupported read command error code specified. {result.ErrorCode}"),
                    } : null, 
                    ReadOutput: output),
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}
