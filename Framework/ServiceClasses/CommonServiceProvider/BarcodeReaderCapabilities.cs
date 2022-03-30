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
        public enum SymbologiesEnum : Int64
        {
            NotSupported = 0,
            EAN128 = 1 << 0,
            EAN8 = 1 << 1,
            EAN8_2 = 1 << 2,
            EAN8_5 = 1 << 3,
            EAN13 = 1 << 4,
            EAN13_2 = 1 << 5,
            EAN13_5 = 1 << 6,
            JAN13 = 1 << 7,
            UPCA = 1 << 8,
            UPCE0 = 1 << 9,
            UPCE0_2 = 1 << 10,
            UPCE0_5 = 1 << 11,
            UPCE1 = 1 << 12,
            UPCE1_2 = 1 << 13,
            UPCE1_5 = 1 << 14,
            UPCA_2 = 1 << 15,
            UPCA_5 = 1 << 16,
            CODABAR = 1 << 17,
            ITF = 1 << 18,
            CODE11 = 1 << 19,
            CODE39 = 1 << 20,
            CODE49 = 1 << 21,
            CODE93 = 1 << 22,
            CODE128 = 1 << 23,
            MSI = 1 << 24,
            PLESSEY = 1 << 25,
            STD2OF5 = 1 << 26,
            STD2OF5_IATA = 1 << 27,
            PDF_417 = 1 << 28,
            MICROPDF_417 = 1 << 29,
            DataMatrix = 1 << 30,
            MAXICODE = 1 << 31,
            CODEONE = 1 << 32,
            CHANNELCODE = 1 << 33,
            TELEPEN_ORIGINAL = 1 << 34,
            TELEPEN_AIM = 1 << 35,
            RSS = 1 << 36,
            RSS_EXPANDED = 1 << 37,
            RSS_RESTRICTED = 1 << 38,
            CompositeCodeA = 1 << 39,
            CompositeCodeB = 1 << 40,
            CompositeCodeC = 1 << 41,
            PosiCodeA = 1 << 42,
            PosiCodeB = 1 << 43,
            TriopticCode39 = 1 << 44,
            CodablockF = 1 << 45,
            Code16K = 1 << 46,
            QRCode = 1 << 47,
            AztecCodes = 1 << 48,
            UKPost = 1 << 49,
            USPlanet = 1 << 50,
            USPostnet = 1 << 51,
            CanadianPost = 1 << 52,
            NetherlandsPost = 1 << 53,
            AustralianPost = 1 << 54,
            JapanesePost = 1 << 55,
            ChinesePost = 1 << 56,
            KoreanPost = 1 << 57,
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
