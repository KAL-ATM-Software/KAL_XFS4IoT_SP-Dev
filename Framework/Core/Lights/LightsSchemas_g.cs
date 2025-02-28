/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Lights interface.
 * LightsSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Lights
{

    [DataContract]
    public sealed class PositionStatusClass
    {
        public PositionStatusClass(FlashRateEnum? FlashRate = null, ColorEnum? Color = null, DirectionEnum? Direction = null)
        {
            this.FlashRate = FlashRate;
            this.Color = Color;
            this.Direction = Direction;
        }

        public enum FlashRateEnum
        {
            Off,
            Slow,
            Medium,
            Quick,
            Continuous
        }

        /// <summary>
        /// The light flash rate. This may be null in a [Common.StatusChangedEvent](#common.statuschangedevent) if
        /// unchanged, otherwise one of the following values:
        /// 
        /// * ```off``` - The light is turned off.
        /// * ```slow``` - The light is flashing slowly.
        /// * ```medium``` - The light is flashing at medium frequency.
        /// * ```quick``` - The light is flashing quickly.
        /// * ```continuous``` - The light is continuous (steady).
        /// </summary>
        [DataMember(Name = "flashRate")]
        public FlashRateEnum? FlashRate { get; init; }

        public enum ColorEnum
        {
            Red,
            Green,
            Yellow,
            Blue,
            Cyan,
            Magenta,
            White
        }

        /// <summary>
        /// The light color. This property can be null if not supported, only supports a single color
        /// or in a [Common.StatusChangedEvent](#common.statuschangedevent) if
        /// unchanged, otherwise one of the following values:
        /// 
        /// * ```red``` - The light is red.
        /// * ```green``` - The light is green.
        /// * ```yellow``` - The light is yellow.
        /// * ```blue``` - The light is blue.
        /// * ```cyan``` - The light is cyan.
        /// * ```magenta``` - The light is magenta.
        /// * ```white``` - The light is white.
        /// </summary>
        [DataMember(Name = "color")]
        public ColorEnum? Color { get; init; }

        public enum DirectionEnum
        {
            Entry,
            Exit
        }

        /// <summary>
        /// The light direction. This property can be null if not supported or in a [Common.StatusChangedEvent](#common.statuschangedevent) if
        /// unchanged, otherwise one of the following values:
        /// 
        /// * ```entry``` -  The light is indicating entry.
        /// * ```exit``` -  The light is indicating exit.
        /// </summary>
        [DataMember(Name = "direction")]
        public DirectionEnum? Direction { get; init; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(Dictionary<string, PositionStatusClass> CardReader = null, Dictionary<string, PositionStatusClass> PinPad = null, Dictionary<string, PositionStatusClass> NotesDispenser = null, Dictionary<string, PositionStatusClass> CoinDispenser = null, Dictionary<string, PositionStatusClass> ReceiptPrinter = null, Dictionary<string, PositionStatusClass> PassbookPrinter = null, Dictionary<string, PositionStatusClass> EnvelopeDepository = null, Dictionary<string, PositionStatusClass> CheckUnit = null, Dictionary<string, PositionStatusClass> BillAcceptor = null, Dictionary<string, PositionStatusClass> EnvelopeDispenser = null, Dictionary<string, PositionStatusClass> DocumentPrinter = null, Dictionary<string, PositionStatusClass> CoinAcceptor = null, Dictionary<string, PositionStatusClass> Scanner = null, Dictionary<string, PositionStatusClass> Contactless = null, Dictionary<string, PositionStatusClass> CardReader2 = null, Dictionary<string, PositionStatusClass> NotesDispenser2 = null, Dictionary<string, PositionStatusClass> BillAcceptor2 = null, Dictionary<string, PositionStatusClass> StatusGood = null, Dictionary<string, PositionStatusClass> StatusWarning = null, Dictionary<string, PositionStatusClass> StatusBad = null, Dictionary<string, PositionStatusClass> StatusSupervisor = null, Dictionary<string, PositionStatusClass> StatusInService = null, Dictionary<string, PositionStatusClass> FasciaLight = null)
        {
            this.CardReader = CardReader;
            this.PinPad = PinPad;
            this.NotesDispenser = NotesDispenser;
            this.CoinDispenser = CoinDispenser;
            this.ReceiptPrinter = ReceiptPrinter;
            this.PassbookPrinter = PassbookPrinter;
            this.EnvelopeDepository = EnvelopeDepository;
            this.CheckUnit = CheckUnit;
            this.BillAcceptor = BillAcceptor;
            this.EnvelopeDispenser = EnvelopeDispenser;
            this.DocumentPrinter = DocumentPrinter;
            this.CoinAcceptor = CoinAcceptor;
            this.Scanner = Scanner;
            this.Contactless = Contactless;
            this.CardReader2 = CardReader2;
            this.NotesDispenser2 = NotesDispenser2;
            this.BillAcceptor2 = BillAcceptor2;
            this.StatusGood = StatusGood;
            this.StatusWarning = StatusWarning;
            this.StatusBad = StatusBad;
            this.StatusSupervisor = StatusSupervisor;
            this.StatusInService = StatusInService;
            this.FasciaLight = FasciaLight;
        }

        /// <summary>
        /// Card Reader Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "cardReader")]
        public Dictionary<string, PositionStatusClass> CardReader { get; init; }

        /// <summary>
        /// Pin Pad Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "pinPad")]
        public Dictionary<string, PositionStatusClass> PinPad { get; init; }

        /// <summary>
        /// Notes Dispenser Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "notesDispenser")]
        public Dictionary<string, PositionStatusClass> NotesDispenser { get; init; }

        /// <summary>
        /// Coin Dispenser Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "coinDispenser")]
        public Dictionary<string, PositionStatusClass> CoinDispenser { get; init; }

        /// <summary>
        /// Receipt Printer Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "receiptPrinter")]
        public Dictionary<string, PositionStatusClass> ReceiptPrinter { get; init; }

        /// <summary>
        /// Passbook Printer Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "passbookPrinter")]
        public Dictionary<string, PositionStatusClass> PassbookPrinter { get; init; }

        /// <summary>
        /// Envelope Depository Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "envelopeDepository")]
        public Dictionary<string, PositionStatusClass> EnvelopeDepository { get; init; }

        /// <summary>
        /// Check Unit Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "checkUnit")]
        public Dictionary<string, PositionStatusClass> CheckUnit { get; init; }

        /// <summary>
        /// Bill Acceptor Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "billAcceptor")]
        public Dictionary<string, PositionStatusClass> BillAcceptor { get; init; }

        /// <summary>
        /// Envelope Dispenser Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "envelopeDispenser")]
        public Dictionary<string, PositionStatusClass> EnvelopeDispenser { get; init; }

        /// <summary>
        /// Document Printer Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "documentPrinter")]
        public Dictionary<string, PositionStatusClass> DocumentPrinter { get; init; }

        /// <summary>
        /// Coin Acceptor Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "coinAcceptor")]
        public Dictionary<string, PositionStatusClass> CoinAcceptor { get; init; }

        /// <summary>
        /// Scanner Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "scanner")]
        public Dictionary<string, PositionStatusClass> Scanner { get; init; }

        /// <summary>
        /// Contactless Reader Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "contactless")]
        public Dictionary<string, PositionStatusClass> Contactless { get; init; }

        /// <summary>
        /// Card Reader 2 Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "cardReader2")]
        public Dictionary<string, PositionStatusClass> CardReader2 { get; init; }

        /// <summary>
        /// Notes Dispenser 2 Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "notesDispenser2")]
        public Dictionary<string, PositionStatusClass> NotesDispenser2 { get; init; }

        /// <summary>
        /// Bill Acceptor 2 Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "billAcceptor2")]
        public Dictionary<string, PositionStatusClass> BillAcceptor2 { get; init; }

        /// <summary>
        /// Status Indicator light - Good. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "statusGood")]
        public Dictionary<string, PositionStatusClass> StatusGood { get; init; }

        /// <summary>
        /// Status Indicator light - Warning. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "statusWarning")]
        public Dictionary<string, PositionStatusClass> StatusWarning { get; init; }

        /// <summary>
        /// Status Indicator light - Bad. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "statusBad")]
        public Dictionary<string, PositionStatusClass> StatusBad { get; init; }

        /// <summary>
        /// Status Indicator light - Supervisor. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "statusSupervisor")]
        public Dictionary<string, PositionStatusClass> StatusSupervisor { get; init; }

        /// <summary>
        /// Status Indicator light - In Service. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "statusInService")]
        public Dictionary<string, PositionStatusClass> StatusInService { get; init; }

        /// <summary>
        /// Fascia Light. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "fasciaLight")]
        public Dictionary<string, PositionStatusClass> FasciaLight { get; init; }

    }


    [DataContract]
    public sealed class PositionCapsClass
    {
        public PositionCapsClass(FlashRateClass FlashRate = null, ColorClass Color = null, DirectionClass Direction = null)
        {
            this.FlashRate = FlashRate;
            this.Color = Color;
            this.Direction = Direction;
        }

        [DataContract]
        public sealed class FlashRateClass
        {
            public FlashRateClass(bool? Off = null, bool? Slow = null, bool? Medium = null, bool? Quick = null, bool? Continuous = null)
            {
                this.Off = Off;
                this.Slow = Slow;
                this.Medium = Medium;
                this.Quick = Quick;
                this.Continuous = Continuous;
            }

            /// <summary>
            /// The light can be turned off.
            /// </summary>
            [DataMember(Name = "off")]
            public bool? Off { get; init; }

            /// <summary>
            /// The light can flash slowly.
            /// </summary>
            [DataMember(Name = "slow")]
            public bool? Slow { get; init; }

            /// <summary>
            /// The light can flash at medium frequency.
            /// </summary>
            [DataMember(Name = "medium")]
            public bool? Medium { get; init; }

            /// <summary>
            /// The light can flash quickly.
            /// </summary>
            [DataMember(Name = "quick")]
            public bool? Quick { get; init; }

            /// <summary>
            /// The light can be turned on.
            /// </summary>
            [DataMember(Name = "continuous")]
            public bool? Continuous { get; init; }

        }

        /// <summary>
        /// Indicates the light flash rate.
        /// </summary>
        [DataMember(Name = "flashRate")]
        public FlashRateClass FlashRate { get; init; }

        [DataContract]
        public sealed class ColorClass
        {
            public ColorClass(bool? Red = null, bool? Green = null, bool? Yellow = null, bool? Blue = null, bool? Cyan = null, bool? Magenta = null, bool? White = null)
            {
                this.Red = Red;
                this.Green = Green;
                this.Yellow = Yellow;
                this.Blue = Blue;
                this.Cyan = Cyan;
                this.Magenta = Magenta;
                this.White = White;
            }

            /// <summary>
            /// The light can be red.
            /// </summary>
            [DataMember(Name = "red")]
            public bool? Red { get; init; }

            /// <summary>
            /// The light can be green.
            /// </summary>
            [DataMember(Name = "green")]
            public bool? Green { get; init; }

            /// <summary>
            /// The light can be yellow.
            /// </summary>
            [DataMember(Name = "yellow")]
            public bool? Yellow { get; init; }

            /// <summary>
            /// The light can be blue.
            /// </summary>
            [DataMember(Name = "blue")]
            public bool? Blue { get; init; }

            /// <summary>
            /// The light can be cyan.
            /// </summary>
            [DataMember(Name = "cyan")]
            public bool? Cyan { get; init; }

            /// <summary>
            /// The light can be magenta.
            /// </summary>
            [DataMember(Name = "magenta")]
            public bool? Magenta { get; init; }

            /// <summary>
            /// The light can be white.
            /// </summary>
            [DataMember(Name = "white")]
            public bool? White { get; init; }

        }

        /// <summary>
        /// Indicates the light color. This property can be null if the guidance light indicator 
        /// only supports one color. 
        /// </summary>
        [DataMember(Name = "color")]
        public ColorClass Color { get; init; }

        [DataContract]
        public sealed class DirectionClass
        {
            public DirectionClass(bool? Entry = null, bool? Exit = null)
            {
                this.Entry = Entry;
                this.Exit = Exit;
            }

            /// <summary>
            /// The light can indicate entry.
            /// </summary>
            [DataMember(Name = "entry")]
            public bool? Entry { get; init; }

            /// <summary>
            /// The light can indicate exit.
            /// </summary>
            [DataMember(Name = "exit")]
            public bool? Exit { get; init; }

        }

        /// <summary>
        /// Indicates the light direction. This property is null if not applicable.
        /// </summary>
        [DataMember(Name = "direction")]
        public DirectionClass Direction { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(bool? IndividualFlashRates = null, LightsClass Lights = null)
        {
            this.IndividualFlashRates = IndividualFlashRates;
            this.Lights = Lights;
        }

        /// <summary>
        /// Indicates flash rates of the lights are individually controllable.
        /// If true, excluding off, indicates the flash rate of each light may be different. 
        /// If false, excluding off, indicates all lights flash at the same rate.
        /// </summary>
        [DataMember(Name = "individualFlashRates")]
        public bool? IndividualFlashRates { get; init; }

        [DataContract]
        public sealed class LightsClass
        {
            public LightsClass(Dictionary<string, PositionCapsClass> CardReader = null, Dictionary<string, PositionCapsClass> PinPad = null, Dictionary<string, PositionCapsClass> NotesDispenser = null, Dictionary<string, PositionCapsClass> CoinDispenser = null, Dictionary<string, PositionCapsClass> ReceiptPrinter = null, Dictionary<string, PositionCapsClass> PassbookPrinter = null, Dictionary<string, PositionCapsClass> EnvelopeDepository = null, Dictionary<string, PositionCapsClass> CheckUnit = null, Dictionary<string, PositionCapsClass> BillAcceptor = null, Dictionary<string, PositionCapsClass> EnvelopeDispenser = null, Dictionary<string, PositionCapsClass> DocumentPrinter = null, Dictionary<string, PositionCapsClass> CoinAcceptor = null, Dictionary<string, PositionCapsClass> Scanner = null, Dictionary<string, PositionCapsClass> Contactless = null, Dictionary<string, PositionCapsClass> CardReader2 = null, Dictionary<string, PositionCapsClass> NotesDispenser2 = null, Dictionary<string, PositionCapsClass> BillAcceptor2 = null, Dictionary<string, PositionCapsClass> StatusGood = null, Dictionary<string, PositionCapsClass> StatusWarning = null, Dictionary<string, PositionCapsClass> StatusBad = null, Dictionary<string, PositionCapsClass> StatusSupervisor = null, Dictionary<string, PositionCapsClass> StatusInService = null, Dictionary<string, PositionCapsClass> FasciaLight = null)
            {
                this.CardReader = CardReader;
                this.PinPad = PinPad;
                this.NotesDispenser = NotesDispenser;
                this.CoinDispenser = CoinDispenser;
                this.ReceiptPrinter = ReceiptPrinter;
                this.PassbookPrinter = PassbookPrinter;
                this.EnvelopeDepository = EnvelopeDepository;
                this.CheckUnit = CheckUnit;
                this.BillAcceptor = BillAcceptor;
                this.EnvelopeDispenser = EnvelopeDispenser;
                this.DocumentPrinter = DocumentPrinter;
                this.CoinAcceptor = CoinAcceptor;
                this.Scanner = Scanner;
                this.Contactless = Contactless;
                this.CardReader2 = CardReader2;
                this.NotesDispenser2 = NotesDispenser2;
                this.BillAcceptor2 = BillAcceptor2;
                this.StatusGood = StatusGood;
                this.StatusWarning = StatusWarning;
                this.StatusBad = StatusBad;
                this.StatusSupervisor = StatusSupervisor;
                this.StatusInService = StatusInService;
                this.FasciaLight = FasciaLight;
            }

            /// <summary>
            /// Card Reader Light.
            /// </summary>
            [DataMember(Name = "cardReader")]
            public Dictionary<string, PositionCapsClass> CardReader { get; init; }

            /// <summary>
            /// Pin Pad Light.
            /// </summary>
            [DataMember(Name = "pinPad")]
            public Dictionary<string, PositionCapsClass> PinPad { get; init; }

            /// <summary>
            /// Notes Dispenser Light.
            /// </summary>
            [DataMember(Name = "notesDispenser")]
            public Dictionary<string, PositionCapsClass> NotesDispenser { get; init; }

            /// <summary>
            /// Coin Dispenser Light.
            /// </summary>
            [DataMember(Name = "coinDispenser")]
            public Dictionary<string, PositionCapsClass> CoinDispenser { get; init; }

            /// <summary>
            /// Receipt Printer Light.
            /// </summary>
            [DataMember(Name = "receiptPrinter")]
            public Dictionary<string, PositionCapsClass> ReceiptPrinter { get; init; }

            /// <summary>
            /// Passbook Printer Light.
            /// </summary>
            [DataMember(Name = "passbookPrinter")]
            public Dictionary<string, PositionCapsClass> PassbookPrinter { get; init; }

            /// <summary>
            /// Envelope Depository Light.
            /// </summary>
            [DataMember(Name = "envelopeDepository")]
            public Dictionary<string, PositionCapsClass> EnvelopeDepository { get; init; }

            /// <summary>
            /// Check Unit Light.
            /// </summary>
            [DataMember(Name = "checkUnit")]
            public Dictionary<string, PositionCapsClass> CheckUnit { get; init; }

            /// <summary>
            /// Bill Acceptor Light.
            /// </summary>
            [DataMember(Name = "billAcceptor")]
            public Dictionary<string, PositionCapsClass> BillAcceptor { get; init; }

            /// <summary>
            /// Envelope Dispenser Light.
            /// </summary>
            [DataMember(Name = "envelopeDispenser")]
            public Dictionary<string, PositionCapsClass> EnvelopeDispenser { get; init; }

            /// <summary>
            /// Document Printer Light.
            /// </summary>
            [DataMember(Name = "documentPrinter")]
            public Dictionary<string, PositionCapsClass> DocumentPrinter { get; init; }

            /// <summary>
            /// Coin Acceptor Light.
            /// </summary>
            [DataMember(Name = "coinAcceptor")]
            public Dictionary<string, PositionCapsClass> CoinAcceptor { get; init; }

            /// <summary>
            /// Scanner Light.
            /// </summary>
            [DataMember(Name = "scanner")]
            public Dictionary<string, PositionCapsClass> Scanner { get; init; }

            /// <summary>
            /// Contactless Reader Light.
            /// </summary>
            [DataMember(Name = "contactless")]
            public Dictionary<string, PositionCapsClass> Contactless { get; init; }

            /// <summary>
            /// Card Reader 2 Light.
            /// </summary>
            [DataMember(Name = "cardReader2")]
            public Dictionary<string, PositionCapsClass> CardReader2 { get; init; }

            /// <summary>
            /// Notes Dispenser 2 Light.
            /// </summary>
            [DataMember(Name = "notesDispenser2")]
            public Dictionary<string, PositionCapsClass> NotesDispenser2 { get; init; }

            /// <summary>
            /// Bill Acceptor 2 Light.
            /// </summary>
            [DataMember(Name = "billAcceptor2")]
            public Dictionary<string, PositionCapsClass> BillAcceptor2 { get; init; }

            /// <summary>
            /// Status indicator light - Good.
            /// </summary>
            [DataMember(Name = "statusGood")]
            public Dictionary<string, PositionCapsClass> StatusGood { get; init; }

            /// <summary>
            /// Status indicator light - Warning.
            /// </summary>
            [DataMember(Name = "statusWarning")]
            public Dictionary<string, PositionCapsClass> StatusWarning { get; init; }

            /// <summary>
            /// Status indicator light - Bad.
            /// </summary>
            [DataMember(Name = "statusBad")]
            public Dictionary<string, PositionCapsClass> StatusBad { get; init; }

            /// <summary>
            /// Status indicator light - Supervisor.
            /// </summary>
            [DataMember(Name = "statusSupervisor")]
            public Dictionary<string, PositionCapsClass> StatusSupervisor { get; init; }

            /// <summary>
            /// Status indicator light - In Service.
            /// </summary>
            [DataMember(Name = "statusInService")]
            public Dictionary<string, PositionCapsClass> StatusInService { get; init; }

            /// <summary>
            /// Fascia light.
            /// </summary>
            [DataMember(Name = "fasciaLight")]
            public Dictionary<string, PositionCapsClass> FasciaLight { get; init; }

        }

        /// <summary>
        /// Indicates the lights supported.
        /// </summary>
        [DataMember(Name = "lights")]
        public LightsClass Lights { get; init; }

    }


}
