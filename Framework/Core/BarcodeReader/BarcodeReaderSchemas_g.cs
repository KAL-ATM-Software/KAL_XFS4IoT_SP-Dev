/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT BarcodeReader interface.
 * BarcodeReaderSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.BarcodeReader
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(ScannerEnum? Scanner = null)
        {
            this.Scanner = Scanner;
        }

        public enum ScannerEnum
        {
            On,
            Off,
            Inoperative,
            Unknown
        }

        /// <summary>
        /// Specifies the scanner status (laser, camera or other technology) as one of the following:
        /// 
        /// * ```on``` - Scanner is enabled for reading.
        /// * ```off``` - Scanner is disabled.
        /// * ```inoperative``` - Scanner is inoperative due to a hardware error.
        /// * ```unknown``` - Scanner status cannot be determined.
        /// </summary>
        [DataMember(Name = "scanner")]
        public ScannerEnum? Scanner { get; init; }

    }


    [DataContract]
    public sealed class SymbologiesPropertiesClass
    {
        public SymbologiesPropertiesClass(bool? Ean128 = null, bool? Ean8 = null, bool? Ean8_2 = null, bool? Ean8_5 = null, bool? Ean13 = null, bool? Ean13_2 = null, bool? Ean13_5 = null, bool? Jan13 = null, bool? UpcA = null, bool? UpcE0 = null, bool? UpcE0_2 = null, bool? UpcE0_5 = null, bool? UpcE1 = null, bool? UpcE1_2 = null, bool? UpcE1_5 = null, bool? UpcA_2 = null, bool? UpcA_5 = null, bool? Codabar = null, bool? Itf = null, bool? Code11 = null, bool? Code39 = null, bool? Code49 = null, bool? Code93 = null, bool? Code128 = null, bool? Msi = null, bool? Plessey = null, bool? Std2Of5 = null, bool? Std2Of5Iata = null, bool? Pdf417 = null, bool? MicroPdf417 = null, bool? DataMatrix = null, bool? MaxiCode = null, bool? CodeOne = null, bool? ChannelCode = null, bool? TelepenOriginal = null, bool? TelepenAim = null, bool? Rss = null, bool? RssExpanded = null, bool? RssRestricted = null, bool? CompositeCodeA = null, bool? CompositeCodeB = null, bool? CompositeCodeC = null, bool? PosiCodeA = null, bool? PosiCodeB = null, bool? TriopticCode39 = null, bool? CodablockF = null, bool? Code16K = null, bool? QrCode = null, bool? Aztec = null, bool? UkPost = null, bool? Planet = null, bool? Postnet = null, bool? CanadianPost = null, bool? NetherlandsPost = null, bool? AustralianPost = null, bool? JapanesePost = null, bool? ChinesePost = null, bool? KoreanPost = null)
        {
            this.Ean128 = Ean128;
            this.Ean8 = Ean8;
            this.Ean8_2 = Ean8_2;
            this.Ean8_5 = Ean8_5;
            this.Ean13 = Ean13;
            this.Ean13_2 = Ean13_2;
            this.Ean13_5 = Ean13_5;
            this.Jan13 = Jan13;
            this.UpcA = UpcA;
            this.UpcE0 = UpcE0;
            this.UpcE0_2 = UpcE0_2;
            this.UpcE0_5 = UpcE0_5;
            this.UpcE1 = UpcE1;
            this.UpcE1_2 = UpcE1_2;
            this.UpcE1_5 = UpcE1_5;
            this.UpcA_2 = UpcA_2;
            this.UpcA_5 = UpcA_5;
            this.Codabar = Codabar;
            this.Itf = Itf;
            this.Code11 = Code11;
            this.Code39 = Code39;
            this.Code49 = Code49;
            this.Code93 = Code93;
            this.Code128 = Code128;
            this.Msi = Msi;
            this.Plessey = Plessey;
            this.Std2Of5 = Std2Of5;
            this.Std2Of5Iata = Std2Of5Iata;
            this.Pdf417 = Pdf417;
            this.MicroPdf417 = MicroPdf417;
            this.DataMatrix = DataMatrix;
            this.MaxiCode = MaxiCode;
            this.CodeOne = CodeOne;
            this.ChannelCode = ChannelCode;
            this.TelepenOriginal = TelepenOriginal;
            this.TelepenAim = TelepenAim;
            this.Rss = Rss;
            this.RssExpanded = RssExpanded;
            this.RssRestricted = RssRestricted;
            this.CompositeCodeA = CompositeCodeA;
            this.CompositeCodeB = CompositeCodeB;
            this.CompositeCodeC = CompositeCodeC;
            this.PosiCodeA = PosiCodeA;
            this.PosiCodeB = PosiCodeB;
            this.TriopticCode39 = TriopticCode39;
            this.CodablockF = CodablockF;
            this.Code16K = Code16K;
            this.QrCode = QrCode;
            this.Aztec = Aztec;
            this.UkPost = UkPost;
            this.Planet = Planet;
            this.Postnet = Postnet;
            this.CanadianPost = CanadianPost;
            this.NetherlandsPost = NetherlandsPost;
            this.AustralianPost = AustralianPost;
            this.JapanesePost = JapanesePost;
            this.ChinesePost = ChinesePost;
            this.KoreanPost = KoreanPost;
        }

        /// <summary>
        /// GS1-128
        /// </summary>
        [DataMember(Name = "ean128")]
        public bool? Ean128 { get; init; }

        /// <summary>
        /// EAN-8
        /// </summary>
        [DataMember(Name = "ean8")]
        public bool? Ean8 { get; init; }

        /// <summary>
        /// EAN-8 with 2 digit add-on
        /// </summary>
        [DataMember(Name = "ean8_2")]
        public bool? Ean8_2 { get; init; }

        /// <summary>
        /// EAN-8 with 5 digit add-on
        /// </summary>
        [DataMember(Name = "ean8_5")]
        public bool? Ean8_5 { get; init; }

        /// <summary>
        /// EAN13
        /// </summary>
        [DataMember(Name = "ean13")]
        public bool? Ean13 { get; init; }

        /// <summary>
        /// EAN-13 with 2 digit add-on
        /// </summary>
        [DataMember(Name = "ean13_2")]
        public bool? Ean13_2 { get; init; }

        /// <summary>
        /// EAN-13 with 5 digit add-on
        /// </summary>
        [DataMember(Name = "ean13_5")]
        public bool? Ean13_5 { get; init; }

        /// <summary>
        /// jan-13
        /// </summary>
        [DataMember(Name = "jan13")]
        public bool? Jan13 { get; init; }

        /// <summary>
        /// UPC-A
        /// </summary>
        [DataMember(Name = "upcA")]
        public bool? UpcA { get; init; }

        /// <summary>
        /// UPC-E
        /// </summary>
        [DataMember(Name = "upcE0")]
        public bool? UpcE0 { get; init; }

        /// <summary>
        /// UPC-E with 2 digit add-on
        /// </summary>
        [DataMember(Name = "upcE0_2")]
        public bool? UpcE0_2 { get; init; }

        /// <summary>
        /// UPC-E with 5 digit add-on
        /// </summary>
        [DataMember(Name = "upcE0_5")]
        public bool? UpcE0_5 { get; init; }

        /// <summary>
        /// UPC-E with leading 1
        /// </summary>
        [DataMember(Name = "upcE1")]
        public bool? UpcE1 { get; init; }

        /// <summary>
        /// UPC-E with leading 1and 2 digit add-on
        /// </summary>
        [DataMember(Name = "upcE1_2")]
        public bool? UpcE1_2 { get; init; }

        /// <summary>
        /// UPC-E with leading 1and 5 digit add-on
        /// </summary>
        [DataMember(Name = "upcE1_5")]
        public bool? UpcE1_5 { get; init; }

        /// <summary>
        /// UPC-A with2 digit add-on
        /// </summary>
        [DataMember(Name = "upcA_2")]
        public bool? UpcA_2 { get; init; }

        /// <summary>
        /// UPC-A with 5 digit add-on
        /// </summary>
        [DataMember(Name = "upcA_5")]
        public bool? UpcA_5 { get; init; }

        /// <summary>
        /// CODABAR (NW-7)
        /// </summary>
        [DataMember(Name = "codabar")]
        public bool? Codabar { get; init; }

        /// <summary>
        /// Interleaved 2 of 5 (ITF)
        /// </summary>
        [DataMember(Name = "itf")]
        public bool? Itf { get; init; }

        /// <summary>
        /// CODE 11 (USD-8)
        /// </summary>
        [DataMember(Name = "code11")]
        public bool? Code11 { get; init; }

        /// <summary>
        /// CODE 39
        /// </summary>
        [DataMember(Name = "code39")]
        public bool? Code39 { get; init; }

        /// <summary>
        /// CODE 49
        /// </summary>
        [DataMember(Name = "code49")]
        public bool? Code49 { get; init; }

        /// <summary>
        /// CODE 93
        /// </summary>
        [DataMember(Name = "code93")]
        public bool? Code93 { get; init; }

        /// <summary>
        /// CODE 128
        /// </summary>
        [DataMember(Name = "code128")]
        public bool? Code128 { get; init; }

        /// <summary>
        /// MSI
        /// </summary>
        [DataMember(Name = "msi")]
        public bool? Msi { get; init; }

        /// <summary>
        /// PLESSEY
        /// </summary>
        [DataMember(Name = "plessey")]
        public bool? Plessey { get; init; }

        /// <summary>
        /// STANDARD 2 of 5 (INDUSTRIAL 2 of 5 also)
        /// </summary>
        [DataMember(Name = "std2Of5")]
        public bool? Std2Of5 { get; init; }

        /// <summary>
        /// STANDARD 2 of 5 (IATA Version)
        /// </summary>
        [DataMember(Name = "std2Of5Iata")]
        public bool? Std2Of5Iata { get; init; }

        /// <summary>
        /// PDF-417
        /// </summary>
        [DataMember(Name = "pdf417")]
        public bool? Pdf417 { get; init; }

        /// <summary>
        /// MICROPDF-417
        /// </summary>
        [DataMember(Name = "microPdf417")]
        public bool? MicroPdf417 { get; init; }

        /// <summary>
        /// GS1 DataMatrix
        /// </summary>
        [DataMember(Name = "dataMatrix")]
        public bool? DataMatrix { get; init; }

        /// <summary>
        /// MAXICODE
        /// </summary>
        [DataMember(Name = "maxiCode")]
        public bool? MaxiCode { get; init; }

        /// <summary>
        /// CODE ONE
        /// </summary>
        [DataMember(Name = "codeOne")]
        public bool? CodeOne { get; init; }

        /// <summary>
        /// CHANNEL CODE
        /// </summary>
        [DataMember(Name = "channelCode")]
        public bool? ChannelCode { get; init; }

        /// <summary>
        /// Original TELEPEN
        /// </summary>
        [DataMember(Name = "telepenOriginal")]
        public bool? TelepenOriginal { get; init; }

        /// <summary>
        /// AIM version of TELEPEN
        /// </summary>
        [DataMember(Name = "telepenAim")]
        public bool? TelepenAim { get; init; }

        /// <summary>
        /// GS1 DataBar™
        /// </summary>
        [DataMember(Name = "rss")]
        public bool? Rss { get; init; }

        /// <summary>
        /// Expanded GS1 DataBar™
        /// </summary>
        [DataMember(Name = "rssExpanded")]
        public bool? RssExpanded { get; init; }

        /// <summary>
        /// Restricted GS1 DataBar™
        /// </summary>
        [DataMember(Name = "rssRestricted")]
        public bool? RssRestricted { get; init; }

        /// <summary>
        /// Composite Code A Component
        /// </summary>
        [DataMember(Name = "compositeCodeA")]
        public bool? CompositeCodeA { get; init; }

        /// <summary>
        /// Composite Code B Component
        /// </summary>
        [DataMember(Name = "compositeCodeB")]
        public bool? CompositeCodeB { get; init; }

        /// <summary>
        /// Composite Code C Component
        /// </summary>
        [DataMember(Name = "compositeCodeC")]
        public bool? CompositeCodeC { get; init; }

        /// <summary>
        /// Posicode Variation A
        /// </summary>
        [DataMember(Name = "posiCodeA")]
        public bool? PosiCodeA { get; init; }

        /// <summary>
        /// Posicode Variation B
        /// </summary>
        [DataMember(Name = "posiCodeB")]
        public bool? PosiCodeB { get; init; }

        /// <summary>
        /// Trioptic Code 39
        /// </summary>
        [DataMember(Name = "triopticCode39")]
        public bool? TriopticCode39 { get; init; }

        /// <summary>
        /// Codablock F
        /// </summary>
        [DataMember(Name = "codablockF")]
        public bool? CodablockF { get; init; }

        /// <summary>
        /// Code 16K
        /// </summary>
        [DataMember(Name = "code16K")]
        public bool? Code16K { get; init; }

        /// <summary>
        /// QR Code
        /// </summary>
        [DataMember(Name = "qrCode")]
        public bool? QrCode { get; init; }

        /// <summary>
        /// Aztec Codes
        /// </summary>
        [DataMember(Name = "aztec")]
        public bool? Aztec { get; init; }

        /// <summary>
        /// UK Post
        /// </summary>
        [DataMember(Name = "ukPost")]
        public bool? UkPost { get; init; }

        /// <summary>
        /// US Postal Planet
        /// </summary>
        [DataMember(Name = "planet")]
        public bool? Planet { get; init; }

        /// <summary>
        /// US Postal Postnet
        /// </summary>
        [DataMember(Name = "postnet")]
        public bool? Postnet { get; init; }

        /// <summary>
        /// Canadian Post
        /// </summary>
        [DataMember(Name = "canadianPost")]
        public bool? CanadianPost { get; init; }

        /// <summary>
        /// Netherlands Post
        /// </summary>
        [DataMember(Name = "netherlandsPost")]
        public bool? NetherlandsPost { get; init; }

        /// <summary>
        /// Australian Post
        /// </summary>
        [DataMember(Name = "australianPost")]
        public bool? AustralianPost { get; init; }

        /// <summary>
        /// Japanese Post
        /// </summary>
        [DataMember(Name = "japanesePost")]
        public bool? JapanesePost { get; init; }

        /// <summary>
        /// Chinese Post
        /// </summary>
        [DataMember(Name = "chinesePost")]
        public bool? ChinesePost { get; init; }

        /// <summary>
        /// Korean Post
        /// </summary>
        [DataMember(Name = "koreanPost")]
        public bool? KoreanPost { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(bool? CanFilterSymbologies = null, SymbologiesPropertiesClass Symbologies = null)
        {
            this.CanFilterSymbologies = CanFilterSymbologies;
            this.Symbologies = Symbologies;
        }

        /// <summary>
        /// Specifies whether the device is capable of discriminating between the presented barcode symbologies such
        /// that only the desired symbologies are recognized/reported
        /// </summary>
        [DataMember(Name = "canFilterSymbologies")]
        public bool? CanFilterSymbologies { get; init; }

        /// <summary>
        /// Specifies the barcode symbologies readable by the scanner. This will be omitted if the supported barcode
        /// symbologies can not be determined.
        /// </summary>
        [DataMember(Name = "symbologies")]
        public SymbologiesPropertiesClass Symbologies { get; init; }

    }


}
