/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTServer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XFS4IoTFramework.Common
{
    public sealed class DepositCapabilitiesClass(
        DepositCapabilitiesClass.TypesEnum DeviceTypes,
        bool DepostTransport,
        bool Shutter,
        DepositCapabilitiesClass.RetractPositionEnum RetractPosition,
        DepositCapabilitiesClass.PrinterCapabilitiesClass PrinterCapabilities)
    {
        public enum TypesEnum
        {
            Envelop = 1 << 0, //The depository accepts envelopes.
            Bag = 1 << 1,     //The depository accepts bags.
        }

        public enum RetractPositionEnum
        {
            Container, //Retracted envelopes are put in the deposit container.
            Dispenser, //Retracted envelopes are retracted back to the envelope dispenser.
        }

        public sealed class PrinterCapabilitiesClass(
            bool Toner,
            bool PrintOnRetract,
            int MaxNumberOfChars,
            bool UnicodeSupport)
        {

            /// <summary>
            /// Specifies whether the printer has a toner (or ink) cassette.
            /// </summary>
            public bool Toner { get; init; } = Toner;

            /// <summary>
            /// Specifies whether the device can print the data with retract operation on retracted envelopes.
            /// </summary>
            public bool PrintOnRetract { get; init; } = PrintOnRetract;

            /// <summary>
            /// pecifies the maximum number of non-proportional ASCII characters that can be printed 
            /// on the envelope. Any  attempt to print more characters than this will result in failure response.
            /// This property is related to the printable area supported by the device, 
            /// therefore the actual number of characters which can be printed will be affected 
            /// by the characters to be printed and the size of the media being printed on, 
            /// therefore it is possible that the print data may be truncated.
            /// </summary>
            public int MaxNumberOfChars { get; init; } = MaxNumberOfChars;

            /// <summary>
            /// Specifies whether the range of characters that can be printed on the
            /// envelope may include Unicode characters.
            /// Note that print data is always supplied in Unicode, but some devices may not be able to 
            /// print a full range of characters and are restricted to the ASCII character range.
            /// 
            /// If true, characters in the Unicode range can be printed. 
            /// If false, only characters in the ASCII range can be printed.
            /// Regardless of this capability, the device may not be able to print all of the characters 
            /// in either specified range.If a character is not supported by the device 
            /// it will be replaced by a vendor dependent substitution character.
            /// It is the responsibility of the vendor to supply information about which characters
            /// are supported on a given device.
            /// </summary>
            public bool UnicodeSupport { get; init; } = UnicodeSupport;
        }

        /// <summary>
        /// Specifies the type of the depository device.
        /// </summary>
        public TypesEnum DeviceTypes { get; init; } = DeviceTypes;

        /// <summary>
        /// Specifies whether a deposit transport mechanism is available.
        /// </summary>
        public bool DepostTransport { get; init; } = DepostTransport;

        /// <summary>
        /// Specifies whether a deposit transport shutter is available.
        /// </summary>
        public bool Shutter { get; init; } = Shutter;

        /// <summary>
        /// Specifies the ability of the envelope dispenser to retract envelopes.
        /// 
        /// Possible values are:
        /// * ```container``` - Retracted envelopes are put in the deposit container.
        /// * ```dispenser``` - Retracted envelopes are retracted back to the envelope dispenser.
        /// </summary>
        public RetractPositionEnum RetractPosition { get; init; } = RetractPosition;

        /// <summary>
        /// Specifies printer capabilities supported.
        /// </summary>
        public PrinterCapabilitiesClass PrinterCapabilities { get; init; } = PrinterCapabilities;
    }
}