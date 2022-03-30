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
        private async Task<ReadCompletion.PayloadData> HandleRead(IReadEvents events, ReadCommand read, CancellationToken cancel)
        {
            if (read.Payload.Symbologies?.AustralianPost is not null &&
                (bool)read.Payload.Symbologies?.AustralianPost &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.AustralianPost) ||
                read.Payload.Symbologies?.Aztec is not null &&
                (bool)read.Payload.Symbologies?.Aztec &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.AztecCodes) ||
                read.Payload.Symbologies?.CanadianPost is not null &&
                (bool)read.Payload.Symbologies?.CanadianPost &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CanadianPost) ||
                read.Payload.Symbologies?.ChannelCode is not null &&
                (bool)read.Payload.Symbologies?.ChannelCode &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CHANNELCODE) ||
                read.Payload.Symbologies?.ChinesePost is not null &&
                (bool)read.Payload.Symbologies?.ChinesePost &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.ChinesePost) ||
                read.Payload.Symbologies?.Codabar is not null &&
                (bool)read.Payload.Symbologies?.Codabar &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODABAR) ||
                read.Payload.Symbologies?.CodablockF is not null &&
                (bool)read.Payload.Symbologies?.CodablockF &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CodablockF) ||
                read.Payload.Symbologies?.Code11 is not null &&
                (bool)read.Payload.Symbologies?.Code11 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE11) ||
                read.Payload.Symbologies?.Code128 is not null &&
                (bool)read.Payload.Symbologies?.Code128 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE128) ||
                read.Payload.Symbologies?.Code16K is not null &&
                (bool)read.Payload.Symbologies?.Code16K &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.Code16K) ||
                read.Payload.Symbologies?.Code39 is not null &&
                (bool)read.Payload.Symbologies?.Code39 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE39) ||
                read.Payload.Symbologies?.Code49 is not null &&
                (bool)read.Payload.Symbologies?.Code49 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE49) ||
                read.Payload.Symbologies?.Code93 is not null &&
                (bool)read.Payload.Symbologies?.Code93 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE93) ||
                read.Payload.Symbologies?.CodeOne is not null &&
                (bool)read.Payload.Symbologies?.CodeOne &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODEONE) ||
                read.Payload.Symbologies?.CompositeCodeA is not null &&
                (bool)read.Payload.Symbologies?.CompositeCodeA &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeA) ||
                read.Payload.Symbologies?.CompositeCodeB is not null &&
                (bool)read.Payload.Symbologies?.CompositeCodeB &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeB) ||
                read.Payload.Symbologies?.CompositeCodeC is not null &&
                (bool)read.Payload.Symbologies?.CompositeCodeC &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeC) ||
                read.Payload.Symbologies?.DataMatrix is not null &&
                (bool)read.Payload.Symbologies?.DataMatrix &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.DataMatrix) ||
                read.Payload.Symbologies?.Ean128 is not null &&
                (bool)read.Payload.Symbologies?.Ean128 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN128) ||
                read.Payload.Symbologies?.Ean13 is not null &&
                (bool)read.Payload.Symbologies?.Ean13 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13) ||
                read.Payload.Symbologies?.Ean13_2 is not null &&
                (bool)read.Payload.Symbologies?.Ean13_2 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13_2) ||
                read.Payload.Symbologies?.Ean13_5 is not null &&
                (bool)read.Payload.Symbologies?.Ean13_5 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13_5) ||
                read.Payload.Symbologies?.Ean8 is not null &&
                (bool)read.Payload.Symbologies?.Ean8 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8) ||
                read.Payload.Symbologies?.Ean8_2 is not null &&
                (bool)read.Payload.Symbologies?.Ean8_2 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8_2) ||
                read.Payload.Symbologies?.Ean8_5 is not null &&
                (bool)read.Payload.Symbologies?.Ean8_5 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8_5) ||
                read.Payload.Symbologies?.Itf is not null &&
                (bool)read.Payload.Symbologies?.Itf &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.ITF) ||
                read.Payload.Symbologies?.Jan13 is not null &&
                (bool)read.Payload.Symbologies?.Jan13 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.JAN13) ||
                read.Payload.Symbologies?.JapanesePost is not null &&
                (bool)read.Payload.Symbologies?.JapanesePost &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.JapanesePost) ||
                read.Payload.Symbologies?.KoreanPost is not null &&
                (bool)read.Payload.Symbologies?.KoreanPost &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.KoreanPost) ||
                read.Payload.Symbologies?.MaxiCode is not null &&
                (bool)read.Payload.Symbologies?.MaxiCode &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.MAXICODE) ||
                read.Payload.Symbologies?.MicroPdf417 is not null &&
                (bool)read.Payload.Symbologies?.MicroPdf417 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.MICROPDF_417) ||
                read.Payload.Symbologies?.Msi is not null &&
                (bool)read.Payload.Symbologies?.Msi &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.MSI) ||
                read.Payload.Symbologies?.NetherlandsPost is not null &&
                (bool)read.Payload.Symbologies?.NetherlandsPost &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.NetherlandsPost) ||
                read.Payload.Symbologies?.Pdf417 is not null &&
                (bool)read.Payload.Symbologies?.Pdf417 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PDF_417) ||
                read.Payload.Symbologies?.Planet is not null &&
                (bool)read.Payload.Symbologies?.Planet &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.USPlanet) ||
                read.Payload.Symbologies?.Plessey is not null &&
                (bool)read.Payload.Symbologies?.Plessey &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PLESSEY) ||
                read.Payload.Symbologies?.PosiCodeA is not null &&
                (bool)read.Payload.Symbologies?.PosiCodeA &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PosiCodeA) ||
                read.Payload.Symbologies?.PosiCodeB is not null &&
                (bool)read.Payload.Symbologies?.PosiCodeB &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.PosiCodeB) ||
                read.Payload.Symbologies?.Postnet is not null &&
                (bool)read.Payload.Symbologies?.Postnet &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.USPostnet) ||
                read.Payload.Symbologies?.QrCode is not null &&
                (bool)read.Payload.Symbologies?.QrCode &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.QRCode) ||
                read.Payload.Symbologies?.Rss is not null &&
                (bool)read.Payload.Symbologies?.Rss &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS) ||
                read.Payload.Symbologies?.RssExpanded is not null &&
                (bool)read.Payload.Symbologies?.RssExpanded &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS_EXPANDED) ||
                read.Payload.Symbologies?.RssRestricted is not null &&
                (bool)read.Payload.Symbologies?.RssRestricted &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS_RESTRICTED) ||
                read.Payload.Symbologies?.Std2Of5 is not null &&
                (bool)read.Payload.Symbologies?.Std2Of5 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.STD2OF5) ||
                read.Payload.Symbologies?.Std2Of5Iata is not null &&
                (bool)read.Payload.Symbologies?.Std2Of5Iata &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.STD2OF5_IATA) ||
                read.Payload.Symbologies?.TelepenAim is not null &&
                (bool)read.Payload.Symbologies?.TelepenAim &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.TELEPEN_AIM) ||
                read.Payload.Symbologies?.TelepenOriginal is not null &&
                (bool)read.Payload.Symbologies?.TelepenOriginal &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.TELEPEN_ORIGINAL) ||
                read.Payload.Symbologies?.TriopticCode39 is not null &&
                (bool)read.Payload.Symbologies?.TriopticCode39 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.TriopticCode39) ||
                read.Payload.Symbologies?.UkPost is not null &&
                (bool)read.Payload.Symbologies?.UkPost &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UKPost) ||
                read.Payload.Symbologies?.UpcA is not null &&
                (bool)read.Payload.Symbologies?.UpcA &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA) ||
                read.Payload.Symbologies?.UpcA_2 is not null &&
                (bool)read.Payload.Symbologies?.UpcA_2 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA_2) ||
                read.Payload.Symbologies?.UpcA_5 is not null &&
                (bool)read.Payload.Symbologies?.UpcA_5 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA_5) ||
                read.Payload.Symbologies?.UpcE0 is not null &&
                (bool)read.Payload.Symbologies?.UpcE0 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0) ||
                read.Payload.Symbologies?.UpcE0_2 is not null &&
                (bool)read.Payload.Symbologies?.UpcE0_2 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0_2) ||
                read.Payload.Symbologies?.UpcE0_5 is not null &&
                (bool)read.Payload.Symbologies?.UpcE0_5 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0_5) ||
                read.Payload.Symbologies?.UpcE1 is not null &&
                (bool)read.Payload.Symbologies?.UpcE1 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1) ||
                read.Payload.Symbologies?.UpcE1_2 is not null &&
                (bool)read.Payload.Symbologies?.UpcE1_2 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1_2) ||
                read.Payload.Symbologies?.UpcE1_5 is not null &&
                (bool)read.Payload.Symbologies?.UpcE1_5 &&
                !Common.BarcodeReaderCapabilities.Symbologies.HasFlag(BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1_5))
            {
                return new ReadCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                      $"Unsupported barcode {read.Payload.Symbologies}",
                                                      ReadCompletion.PayloadData.ErrorCodeEnum.BarcodeInvalid);
            }

            BarcodeReaderCapabilitiesClass.SymbologiesEnum symbologiesToRead = 0;

            if (read.Payload.Symbologies?.AustralianPost is not null &&
                (bool)read.Payload.Symbologies?.AustralianPost)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.AustralianPost;
            }
            if (read.Payload.Symbologies?.Aztec is not null &&
                (bool)read.Payload.Symbologies?.Aztec)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.AztecCodes;
            }
            if (read.Payload.Symbologies?.CanadianPost is not null &&
                (bool)read.Payload.Symbologies?.CanadianPost)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CanadianPost;
            }
            if (read.Payload.Symbologies?.ChannelCode is not null &&
                (bool)read.Payload.Symbologies?.ChannelCode)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CHANNELCODE;
            }
            if (read.Payload.Symbologies?.ChinesePost is not null &&
                (bool)read.Payload.Symbologies?.ChinesePost)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.ChinesePost;
            }
            if (read.Payload.Symbologies?.Codabar is not null &&
                (bool)read.Payload.Symbologies?.Codabar)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODABAR;
            }
            if (read.Payload.Symbologies?.CodablockF is not null &&
                (bool)read.Payload.Symbologies?.CodablockF)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CodablockF;
            }
            if (read.Payload.Symbologies?.Code11 is not null &&
                (bool)read.Payload.Symbologies?.Code11)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE11;
            }
            if (read.Payload.Symbologies?.Code128 is not null &&
                (bool)read.Payload.Symbologies?.Code128)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE128;
            }
            if (read.Payload.Symbologies?.Code16K is not null &&
                (bool)read.Payload.Symbologies?.Code16K)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.Code16K;
            }
            if (read.Payload.Symbologies?.Code39 is not null &&
                (bool)read.Payload.Symbologies?.Code39)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE39;
            }
            if (read.Payload.Symbologies?.Code49 is not null &&
                (bool)read.Payload.Symbologies?.Code49)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE49;
            }
            if (read.Payload.Symbologies?.Code93 is not null &&
                (bool)read.Payload.Symbologies?.Code93)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODE93;
            }
            if (read.Payload.Symbologies?.CodeOne is not null &&
                (bool)read.Payload.Symbologies?.CodeOne)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CODEONE;
            }
            if (read.Payload.Symbologies?.CompositeCodeA is not null &&
                (bool)read.Payload.Symbologies?.CompositeCodeA)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeA;
            }
            if (read.Payload.Symbologies?.CompositeCodeB is not null &&
                (bool)read.Payload.Symbologies?.CompositeCodeB)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeB;
            }
            if (read.Payload.Symbologies?.CompositeCodeC is not null &&
                (bool)read.Payload.Symbologies?.CompositeCodeC)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.CompositeCodeC;
            }
            if (read.Payload.Symbologies?.DataMatrix is not null &&
                (bool)read.Payload.Symbologies?.DataMatrix)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.DataMatrix;
            }
            if (read.Payload.Symbologies?.Ean128 is not null &&
                (bool)read.Payload.Symbologies?.Ean128)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN128;
            }
            if (read.Payload.Symbologies?.Ean13 is not null &&
                (bool)read.Payload.Symbologies?.Ean13)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13;
            }
            if (read.Payload.Symbologies?.Ean13_2 is not null &&
                (bool)read.Payload.Symbologies?.Ean13_2)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13_2;
            }
            if (read.Payload.Symbologies?.Ean13_5 is not null &&
                (bool)read.Payload.Symbologies?.Ean13_5)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN13_5;
            }
            if (read.Payload.Symbologies?.Ean8 is not null &&
                (bool)read.Payload.Symbologies?.Ean8)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8;
            }
            if (read.Payload.Symbologies?.Ean8_2 is not null &&
                (bool)read.Payload.Symbologies?.Ean8_2)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8_2;
            }
            if (read.Payload.Symbologies?.Ean8_5 is not null &&
                (bool)read.Payload.Symbologies?.Ean8_5)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.EAN8_5;
            }
            if (read.Payload.Symbologies?.Itf is not null &&
                (bool)read.Payload.Symbologies?.Itf)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.ITF;
            }
            if (read.Payload.Symbologies?.Jan13 is not null &&
                (bool)read.Payload.Symbologies?.Jan13)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.JAN13;
            }
            if (read.Payload.Symbologies?.JapanesePost is not null &&
                (bool)read.Payload.Symbologies?.JapanesePost)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.JapanesePost;
            }
            if (read.Payload.Symbologies?.KoreanPost is not null &&
                (bool)read.Payload.Symbologies?.KoreanPost)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.KoreanPost;
            }
            if (read.Payload.Symbologies?.MaxiCode is not null &&
                (bool)read.Payload.Symbologies?.MaxiCode)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.MAXICODE;
            }
            if (read.Payload.Symbologies?.MicroPdf417 is not null &&
                (bool)read.Payload.Symbologies?.MicroPdf417)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.MICROPDF_417;
            }
            if (read.Payload.Symbologies?.Msi is not null &&
                (bool)read.Payload.Symbologies?.Msi)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.MSI;
            }
            if (read.Payload.Symbologies?.NetherlandsPost is not null &&
                (bool)read.Payload.Symbologies?.NetherlandsPost)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.NetherlandsPost;
            }
            if (read.Payload.Symbologies?.Pdf417 is not null &&
                (bool)read.Payload.Symbologies?.Pdf417)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.PDF_417;
            }
            if (read.Payload.Symbologies?.Planet is not null &&
                (bool)read.Payload.Symbologies?.Planet)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.USPlanet;
            }
            if (read.Payload.Symbologies?.Plessey is not null &&
                (bool)read.Payload.Symbologies?.Plessey)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.PLESSEY;
            }
            if (read.Payload.Symbologies?.PosiCodeA is not null &&
                (bool)read.Payload.Symbologies?.PosiCodeA)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.PosiCodeA;
            }
            if (read.Payload.Symbologies?.PosiCodeB is not null &&
                (bool)read.Payload.Symbologies?.PosiCodeB)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.PosiCodeB;
            }
            if (read.Payload.Symbologies?.Postnet is not null &&
                (bool)read.Payload.Symbologies?.Postnet)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.USPostnet;
            }
            if (read.Payload.Symbologies?.QrCode is not null &&
                (bool)read.Payload.Symbologies?.QrCode)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.QRCode;
            }
            if (read.Payload.Symbologies?.Rss is not null &&
                (bool)read.Payload.Symbologies?.Rss)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS;
            }
            if (read.Payload.Symbologies?.RssExpanded is not null &&
                (bool)read.Payload.Symbologies?.RssExpanded)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS_EXPANDED;
            }
            if (read.Payload.Symbologies?.RssRestricted is not null &&
                (bool)read.Payload.Symbologies?.RssRestricted)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.RSS_RESTRICTED;
            }
            if (read.Payload.Symbologies?.Std2Of5 is not null &&
                (bool)read.Payload.Symbologies?.Std2Of5)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.STD2OF5;
            }
            if (read.Payload.Symbologies?.Std2Of5Iata is not null &&
                (bool)read.Payload.Symbologies?.Std2Of5Iata)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.STD2OF5_IATA;
            }
            if (read.Payload.Symbologies?.TelepenAim is not null &&
                (bool)read.Payload.Symbologies?.TelepenAim)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.TELEPEN_AIM;
            }
            if (read.Payload.Symbologies?.TelepenOriginal is not null &&
                (bool)read.Payload.Symbologies?.TelepenOriginal)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.TELEPEN_ORIGINAL;
            }
            if (read.Payload.Symbologies?.TriopticCode39 is not null &&
                (bool)read.Payload.Symbologies?.TriopticCode39)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.TriopticCode39;
            }
            if (read.Payload.Symbologies?.UkPost is not null &&
                (bool)read.Payload.Symbologies?.UkPost)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UKPost;
            }
            if (read.Payload.Symbologies?.UpcA is not null &&
                (bool)read.Payload.Symbologies?.UpcA)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA;
            }
            if (read.Payload.Symbologies?.UpcA_2 is not null &&
                (bool)read.Payload.Symbologies?.UpcA_2)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA_2;
            }
            if (read.Payload.Symbologies?.UpcA_5 is not null &&
                (bool)read.Payload.Symbologies?.UpcA_5)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCA_5;
            }
            if (read.Payload.Symbologies?.UpcE0 is not null &&
                (bool)read.Payload.Symbologies?.UpcE0)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0;
            }
            if (read.Payload.Symbologies?.UpcE0_2 is not null &&
                (bool)read.Payload.Symbologies?.UpcE0_2)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0_2;
            }
            if (read.Payload.Symbologies?.UpcE0_5 is not null &&
                (bool)read.Payload.Symbologies?.UpcE0_5)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE0_5;
            }
            if (read.Payload.Symbologies?.UpcE1 is not null &&
                (bool)read.Payload.Symbologies?.UpcE1)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1;
            }
            if (read.Payload.Symbologies?.UpcE1_2 is not null &&
                (bool)read.Payload.Symbologies?.UpcE1_2)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1_2;
            }
            if (read.Payload.Symbologies?.UpcE1_5 is not null &&
                (bool)read.Payload.Symbologies?.UpcE1_5)
            {
                symbologiesToRead |= BarcodeReaderCapabilitiesClass.SymbologiesEnum.UPCE1_5;
            }

            Logger.Log(Constants.DeviceClass, "BarcodeReaderDev.Read()");

            var result = await Device.Read(new ReadRequest(symbologiesToRead, read.Payload.Timeout), cancel);

            Logger.Log(Constants.DeviceClass, $"BarcodeReaderDev.Read() -> {result.CompletionCode} {result.ErrorCode}");

            List<ReadCompletion.PayloadData.ReadOutputClass> output = null;

            if (result.ReadData is not null)
            {
                output = new();
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
                        _ => null,
                    };
                    output.Add(new ReadCompletion.PayloadData.ReadOutputClass(symbol, data.Data, data.VendorSymbologyName));
                }
            }

            return new ReadCompletion.PayloadData(result.CompletionCode,
                                                  result.ErrorDescription,
                                                  result.ErrorCode switch
                                                  {
                                                      ReadResult.ErrorCodeEnum.BarcodeInvalid => ReadCompletion.PayloadData.ErrorCodeEnum.BarcodeInvalid,
                                                      _ => null,
                                                  },
                                                  output);
        }
    }
}
