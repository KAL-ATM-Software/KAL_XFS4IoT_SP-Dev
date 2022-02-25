/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT BarcodeReader interface.
 * Read_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.BarcodeReader.Completions
{
    [DataContract]
    [Completion(Name = "BarcodeReader.Read")]
    public sealed class ReadCompletion : Completion<ReadCompletion.PayloadData>
    {
        public ReadCompletion(int RequestId, ReadCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, List<ReadOutputClass> ReadOutput = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.ReadOutput = ReadOutput;
            }

            public enum ErrorCodeEnum
            {
                BarcodeInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```barcodeInvalid``` - The read operation could not be completed successfully. The barcode presented
            /// was defective or was wrongly read.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class ReadOutputClass
            {
                public ReadOutputClass(SymbologyEnum? Symbology = null, List<byte> BarcodeData = null, string SymbologyName = null)
                {
                    this.Symbology = Symbology;
                    this.BarcodeData = BarcodeData;
                    this.SymbologyName = SymbologyName;
                }

                public enum SymbologyEnum
                {
                    Ean128,
                    Ean8,
                    Ean8_2,
                    Ean8_5,
                    Ean13,
                    Ean13_2,
                    Ean13_5,
                    Jan13,
                    UpcA,
                    UpcE0,
                    UpcE0_2,
                    UpcE0_5,
                    UpcE1,
                    UpcE1_2,
                    UpcE1_5,
                    UpcA_2,
                    UpcA_5,
                    Codabar,
                    Itf,
                    Code11,
                    Code39,
                    Code49,
                    Code93,
                    Code128,
                    Msi,
                    Plessey,
                    Std2Of5,
                    Std2Of5Iata,
                    Pdf417,
                    MicroPdf417,
                    DataMatrix,
                    MaxiCode,
                    CodeOne,
                    ChannelCode,
                    TelepenOriginal,
                    TelepenAim,
                    Rss,
                    RssExpanded,
                    RssRestricted,
                    CompositeCodeA,
                    CompositeCodeB,
                    CompositeCodeC,
                    PosiCodeA,
                    PosiCodeB,
                    TriopticCode39,
                    CodablockF,
                    Code16K,
                    QrCode,
                    Aztec,
                    UkPost,
                    Planet,
                    Postnet,
                    CanadianPost,
                    NetherlandsPost,
                    AustralianPost,
                    JapanesePost,
                    ChinesePost,
                    KoreanPost,
                    SymbologyUnknown
                }

                /// <summary>
                /// Specifies the barcode symbology recognized. This contains one of the following values returned in the
                /// [symbologies](#common.capabilities.completion.properties.barcodereader.symbologies) property of the
                /// [Common.Capabilities](#common.capabilities) command. If the barcode reader is unable to
                /// recognize the symbology as one of the values reported via the device capabilities then the value
                /// for this property will be _symbologyUnknown_.
                /// 
                /// The following values are possible:
                /// * ```ean128``` - GS1-128.
                /// * ```ean8``` - EAN-8.
                /// * ```ean8_2``` - EAN-8 with 2 digit add-on.
                /// * ```ean8_5``` - EAN-8 with 5 digit add-on.
                /// * ```ean13``` - EAN-13.
                /// * ```ean13_2``` - EAN-13 with 2 digit add-on.
                /// * ```ean13_5``` -  EAN-13 with 5 digit add-on.
                /// * ```jan13``` - JAN-13.
                /// * ```upcA``` - UPC-A.
                /// * ```upcE0``` - UPC-E.
                /// * ```upcE0_2``` - UPC-E with 2 digit add-on.
                /// * ```upcE0_5``` - UPC-E with 5 digit add-on.
                /// * ```upcE1``` - UPC-E with leading 1.
                /// * ```upcE1_2``` - UPC-E with leading 1and 2 digit add-on.
                /// * ```upcE1_5``` - UPC-E with leading 1and 5 digit add-on.
                /// * ```upcA_2``` - UPC-A with2 digit add-on.
                /// * ```upcA_5``` - UPC-A with 5 digit add-on.
                /// * ```codabar``` - CODABAR (NW-7).
                /// * ```itf``` - Interleaved 2 of 5 (ITF).
                /// * ```code11``` - CODE 11 (USD-8).
                /// * ```code39``` - CODE 39.
                /// * ```code49``` - CODE 49.
                /// * ```code93``` - CODE 93.
                /// * ```code128``` - CODE 128.
                /// * ```msi``` - MSI.
                /// * ```plessey``` - PLESSEY.
                /// * ```std2Of5``` - STANDARD 2 of 5 (INDUSTRIAL 2 of 5 also).
                /// * ```std2Of5Iata``` - STANDARD 2 of 5 (IATA Version).
                /// * ```pdf417``` - PDF-417.
                /// * ```microPdf417``` - MICROPDF-417.
                /// * ```dataMatrix``` - GS1 DataMatrix.
                /// * ```maxiCode``` - MAXICODE.
                /// * ```codeOne``` - CODE ONE.
                /// * ```channelCode``` - CHANNEL CODE.
                /// * ```telepenOriginal``` - Original TELEPEN.
                /// * ```telepenAim``` - AIM version of TELEPEN.
                /// * ```rss``` - GS1 DataBar™.
                /// * ```rssExpanded``` - Expanded GS1 DataBar™.
                /// * ```rssRestricted``` - Restricted GS1 DataBar™.
                /// * ```compositeCodeA``` - Composite Code A Component.
                /// * ```compositeCodeB``` - Composite Code B Component.
                /// * ```compositeCodeC``` - Composite Code C Component.
                /// * ```posiCodeA``` - Posicode Variation A.
                /// * ```posiCodeB``` - Posicode Variation B.
                /// * ```triopticCode39``` - Trioptic Code 39.
                /// * ```codablockF``` - Codablock F.
                /// * ```code16K``` - Code 16K.
                /// * ```qrCode``` - QR Code.
                /// * ```aztec``` - Aztec Codes.
                /// * ```ukPost``` - UK Post.
                /// * ```planet``` - US Postal Planet.
                /// * ```postnet``` - US Postal Postnet.
                /// * ```canadianPost``` - Canadian Post.
                /// * ```netherlandsPost``` - Netherlands Post.
                /// * ```australianPost``` - Australian Post.
                /// * ```japanesePost``` - Japanese Post.
                /// * ```chinesePost``` - Chinese Post.
                /// * ```koreanPost``` - Korean Post.
                /// * ```symbologyUnknown``` - The barcode reader was unable to recognize the symbology.
                /// </summary>
                [DataMember(Name = "symbology")]
                public SymbologyEnum? Symbology { get; init; }

                /// <summary>
                /// Contains the Base64 encoded barcode data read from the barcode reader. The format of the data
                /// will depend on the barcode symbology read. In most cases this will be an array of bytes
                /// containing ASCII numeric digits. However, the format of the data in this property depends entirely
                /// on the symbology read, e.g. it may contain 8 bit character values where the symbol is dependent
                /// on the codepage used to encode the barcode, may contain UNICODE data, or may be a binary block
                /// of data. The application is responsible for checking the completeness and validity of the data.
                /// <example>YmFyY29kZSBkYXRh</example>
                /// </summary>
                [DataMember(Name = "barcodeData")]
                [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
                public List<byte> BarcodeData { get; init; }

                /// <summary>
                /// A vendor dependent symbology identifier for the symbology recognized.
                /// <example>code39</example>
                /// </summary>
                [DataMember(Name = "symbologyName")]
                public string SymbologyName { get; init; }

            }

            /// <summary>
            /// An array of barcode data structures, one for each barcode scanned during the read operation
            /// </summary>
            [DataMember(Name = "readOutput")]
            public List<ReadOutputClass> ReadOutput { get; init; }

        }
    }
}
