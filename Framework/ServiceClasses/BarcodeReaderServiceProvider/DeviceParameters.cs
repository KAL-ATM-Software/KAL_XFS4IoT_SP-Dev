/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.BarcodeReader
{
    /// The classes used by the device interface for an Input/Output parameters

    /// <summary>
    /// ReadRequest
    /// Information contains to perform operation for reading barcode data
    /// </summary>
    public sealed class ReadRequest(
        BarcodeReaderCapabilitiesClass.SymbologiesEnum SymbologiesToRead,
        int Timeout)
    {

        /// <summary>
        /// If the value is zero, enable all symbologies, otherwise enable to read specified format
        /// </summary>
        public BarcodeReaderCapabilitiesClass.SymbologiesEnum SymbologiesToRead { get; init; } = SymbologiesToRead;

        /// <summary>
        /// Timeout for waiting barcode to be read
        /// </summary>
        public int Timeout { get; init; } = Timeout;
    }

    /// <summary>
    /// ReadResult
    /// Return result of reading barcode
    /// </summary>
    public sealed class ReadResult : DeviceResult
    {
        public sealed class ReadBarcodeData
        {
            public ReadBarcodeData(
                SymbologyEnum SymbologyRead,
                List<byte> Data)
            {
                VendorSymbologyName = string.Empty;
                this.SymbologyRead = SymbologyRead;
                this.Data = Data;
            }
            public ReadBarcodeData(
                string VendorSymbologyName,
                List<byte> Data)
            {
                this.SymbologyRead = SymbologyEnum.NotRead;
                this.VendorSymbologyName = VendorSymbologyName;
                this.Data = Data;
            }

            /// <summary>
            /// Read symbolody barcode data
            /// </summary>
            public SymbologyEnum SymbologyRead { get; init; }

            /// <summary>
            /// Read barcode data
            /// </summary>
            public List<byte> Data { get; init; }

            /// <summary>
            /// Vendor specific symbolody read
            /// </summary>
            public string VendorSymbologyName { get; init; }
        }

        public enum SymbologyEnum : Int64
        {
            NotRead,
            EAN128,
            EAN8,
            EAN8_2,
            EAN8_5,
            EAN13,
            EAN13_2,
            EAN13_5,
            JAN13,
            UPCA,
            UPCE0,
            UPCE0_2,
            UPCE0_5,
            UPCE1,
            UPCE1_2,
            UPCE1_5,
            UPCA_2,
            UPCA_5,
            CODABAR,
            ITF,
            CODE11,
            CODE39,
            CODE49,
            CODE93,
            CODE128,
            MSI,
            PLESSEY,
            STD2OF5,
            STD2OF5_IATA,
            PDF_417,
            MICROPDF_417,
            DataMatrix,
            MAXICODE,
            CODEONE,
            CHANNELCODE,
            TELEPEN_ORIGINAL,
            TELEPEN_AIM,
            RSS,
            RSS_EXPANDED,
            RSS_RESTRICTED,
            CompositeCodeA,
            CompositeCodeB,
            CompositeCodeC,
            PosiCodeA,
            PosiCodeB,
            TriopticCode39,
            CodablockF,
            Code16K,
            QRCode,
            AztecCodes,
            UKPost,
            USPlanet,
            USPostnet,
            CanadianPost,
            NetherlandsPost,
            AustralianPost,
            JapanesePost,
            ChinesePost,
            KoreanPost,
            SymbologyUnknown
        }

        public ReadResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public ReadResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<ReadBarcodeData> ReadData)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.ReadData = ReadData;
        }

        public enum ErrorCodeEnum
        {
            BarcodeInvalid
        }

        /// <summary>
        /// Specifies the error code on reading barcode
        /// </summary>
        public ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Read barcode data
        /// </summary>
        public List<ReadBarcodeData> ReadData { get; init; }
    }
}
