/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// BarcodeReaderCapabilitiesClass
    /// Store device capabilites for the barcode reader device
    /// </summary>
    public sealed class BarcodeReaderCapabilitiesClass
    {
        [Flags]
        public enum SymbologiesEnum : long
        {
            NotSupported = 0,
            EAN128 = 1L << 0,
            EAN8 = 1L << 1,
            EAN8_2 = 1L << 2,
            EAN8_5 = 1L << 3,
            EAN13 = 1L << 4,
            EAN13_2 = 1L << 5,
            EAN13_5 = 1L << 6,
            JAN13 = 1L << 7,
            UPCA = 1L << 8,
            UPCE0 = 1L << 9,
            UPCE0_2 = 1L << 10,
            UPCE0_5 = 1L << 11,
            UPCE1 = 1L << 12,
            UPCE1_2 = 1L << 13,
            UPCE1_5 = 1L << 14,
            UPCA_2 = 1L << 15,
            UPCA_5 = 1L << 16,
            CODABAR = 1L << 17,
            ITF = 1L << 18,
            CODE11 = 1L << 19,
            CODE39 = 1L << 20,
            CODE49 = 1L << 21,
            CODE93 = 1L << 22,
            CODE128 = 1L << 23,
            MSI = 1L << 24,
            PLESSEY = 1L << 25,
            STD2OF5 = 1L << 26,
            STD2OF5_IATA = 1L << 27,
            PDF_417 = 1L << 28,
            MICROPDF_417 = 1L << 29,
            DataMatrix = 1L << 30,
            MAXICODE = 1L << 31,
            CODEONE = 1L << 32,
            CHANNELCODE = 1L << 33,
            TELEPEN_ORIGINAL = 1L << 34,
            TELEPEN_AIM = 1L << 35,
            RSS = 1L << 36,
            RSS_EXPANDED = 1L << 37,
            RSS_RESTRICTED = 1L << 38,
            CompositeCodeA = 1L << 39,
            CompositeCodeB = 1L << 40,
            CompositeCodeC = 1L << 41,
            PosiCodeA = 1L << 42,
            PosiCodeB = 1L << 43,
            TriopticCode39 = 1L << 44,
            CodablockF = 1L << 45,
            Code16K = 1L << 46,
            QRCode = 1L << 47,
            AztecCodes = 1L << 48,
            UKPost = 1L << 49,
            USPlanet = 1L << 50,
            USPostnet = 1L << 51,
            CanadianPost = 1L << 52,
            NetherlandsPost = 1L << 53,
            AustralianPost = 1L << 54,
            JapanesePost = 1L << 55,
            ChinesePost = 1L << 56,
            KoreanPost = 1L << 57,
        }

        public BarcodeReaderCapabilitiesClass(bool CanFilterSymbologies,
                                              SymbologiesEnum Symbologies = SymbologiesEnum.NotSupported)
        {
            this.CanFilterSymbologies = CanFilterSymbologies;
            this.Symbologies = Symbologies;
        }

        /// <summary>
        /// Specifies whether the device is capable of discriminating between the presented barcode symbologies such
        /// that only the desired symbologies are recognized/reported
        /// </summary>
        public bool CanFilterSymbologies { get; init; }

        /// <summary>
        /// Specifies the barcode symbologies readable by the scanner.
        /// </summary>
        public SymbologiesEnum Symbologies { get; init; }
    }
}
