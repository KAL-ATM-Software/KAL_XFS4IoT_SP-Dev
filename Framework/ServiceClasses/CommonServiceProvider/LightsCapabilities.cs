﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// LightsCapabilitiesClass
    /// Store device capabilites for the lights supported by the device
    /// </summary>
    public sealed class LightsCapabilitiesClass(
        Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsCapabilitiesClass.Light> Lights,
        Dictionary<string, LightsCapabilitiesClass.Light> CustomLights = null,
        bool IndividualFlashRates = false)
    {
        public enum DeviceEnum
        {
            CardReader,
            PinPad,
            NotesDispenser,
            CoinDispenser,
            ReceiptPrinter,
            PassbookPrinter,
            EnvelopeDepository,
            BillAcceptor,
            EnvelopeDispenser,
            DocumentPrinter,
            CoinAcceptor,
            Scanner,
            Contactless,
            CardReader2,
            NotesDispenser2,
            BillAcceptor2,
            StatusGoodIndicator,
            StatusWarningIndicator,
            StatusBadIndicator,
            StatusSupervisorIndicator,
            StatusInServiceIndicator,
            FasciaLight,
            CheckUnit,
        }

        [Flags]
        public enum FlashRateEnum
        {
            Off = 1 << 0,
            Slow = 1 << 1,
            Medium = 1 << 2,
            Quick = 1 << 3,
            Continuous = 1 << 4,
        }

        [Flags]
        public enum ColorEnum
        {
            Default = 0,
            Red = 1 << 0,
            Green = 1 << 1,
            Yellow = 1 << 2,
            Blue = 1 << 3,
            Cyan = 1 << 4,
            Magenta = 1 << 5,
            White = 1 << 6,
        }

        [Flags]
        public enum DirectionEnum
        {
            NotSupported = 0,
            Entry = 1 << 0,
            Exit = 1 << 1,
        }

        [Flags]
        public enum LightPostionEnum
        {
            Default = 0,
            Left = 1 << 0,
            Right = 1 << 1,
            Center = 1 << 2,
            Top = 1 << 3,
            Bottom = 1 << 4,
            Front = 1 << 5,
            Rear = 1 << 6,
        }

        /// <summary>
        /// Light capabilities
        /// </summary>
        public sealed class Light(
            LightsCapabilitiesClass.FlashRateEnum FlashRate,
            LightsCapabilitiesClass.ColorEnum Color,
            LightsCapabilitiesClass.DirectionEnum Direction,
            LightsCapabilitiesClass.LightPostionEnum Position,
            string customPosition = null)
        {
            public FlashRateEnum FlashRate { get; init; } = FlashRate;

            public ColorEnum Color { get; init; } = Color;

            public DirectionEnum Direction { get; init; } = Direction;

            public LightPostionEnum Position { get; init; } = Position;

            public string CustomPosition { get; init; } = customPosition;
        }

        /// <summary>
        /// Pre-defined lights device components in the XFS specification
        /// </summary>
        public Dictionary<DeviceEnum, Light> Lights { get; init; } = Lights;

        /// <summary>
        /// Vendor specific type of lights
        /// </summary>
        public Dictionary<string, Light> CustomLights { get; init; } = CustomLights;

        /// <summary>
        /// Indicates flash rates of the lights are individually controllable.
        /// If true, excluding off, indicates the flash rate of each light may be different.
        /// If false, excluding off, indicates all lights flash at the same rate.
        /// </summary>
        bool IndividualFlashRates { get; init; } = IndividualFlashRates;
    }
}
