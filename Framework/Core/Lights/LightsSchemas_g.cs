/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    public sealed class LightStateClass
    {
        public LightStateClass(PositionEnum? Position = null, FlashRateEnum? FlashRate = null, ColourEnum? Colour = null, DirectionEnum? Direction = null)
        {
            this.Position = Position;
            this.FlashRate = FlashRate;
            this.Colour = Colour;
            this.Direction = Direction;
        }

        public enum PositionEnum
        {
            Left,
            Right,
            Center,
            Top,
            Bottom,
            Front,
            Rear
        }

        /// <summary>
        /// The light position. If omitted then the default position is used. One of the following values:
        /// * ```left``` -  The left position.
        /// * ```right``` -  The right position.
        /// * ```center``` -  The center position.
        ///  * ```top``` -  The top position.
        /// * ```bottom``` -  The bottom position.
        /// * ```front``` -  The front position.
        /// * ```rear``` -  The rear position.
        /// </summary>
        [DataMember(Name = "position")]
        public PositionEnum? Position { get; init; }

        public enum FlashRateEnum
        {
            Off,
            Slow,
            Medium,
            Quick,
            Continuous
        }

        /// <summary>
        /// The light flash rate as one of the following values:
        /// * ```off``` -  The light is turned off.
        /// * ```slow``` -  The light is flashing slowly.
        /// * ```medium``` -  The light is flashing medium frequency.
        /// * ```quick``` -  The light is flashing quickly.
        /// * ```continuous``` - The light is continuous (steady).
        /// </summary>
        [DataMember(Name = "flashRate")]
        public FlashRateEnum? FlashRate { get; init; }

        public enum ColourEnum
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
        /// The light color as one of the following values:
        /// * ```red``` -  The light is red.
        /// * ```green``` -  The light is green.
        /// * ```yellow``` -  The light us yellow.
        /// * ```blue``` -  The light is blue.
        /// * ```cyan``` - The light is cyan.
        /// * ```magenta``` -  The light is magenta.
        /// * ```white``` - The light is white.
        /// </summary>
        [DataMember(Name = "colour")]
        public ColourEnum? Colour { get; init; }

        public enum DirectionEnum
        {
            Entry,
            Exit
        }

        /// <summary>
        /// The light direction as one of the following values:
        /// * ```entry``` -  The light is indicating entry.
        /// * ```exit``` -  The light is indicating exit.
        /// </summary>
        [DataMember(Name = "direction")]
        public DirectionEnum? Direction { get; init; }

    }


    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(LightStateClass CardReader = null, LightStateClass PinPad = null, LightStateClass NotesDispenser = null, LightStateClass CoinDispenser = null, LightStateClass ReceiptPrinter = null, LightStateClass PassbookPrinter = null, LightStateClass EnvelopeDepository = null, LightStateClass ChequeUnit = null, LightStateClass BillAcceptor = null, LightStateClass EnvelopeDispenser = null, LightStateClass DocumentPrinter = null, LightStateClass CoinAcceptor = null, LightStateClass Scanner = null, LightStateClass Contactless = null, LightStateClass CardUnit2 = null, LightStateClass NotesDispenser2 = null, LightStateClass BillAcceptor2 = null, LightStateClass StatusGood = null, LightStateClass StatusWarning = null, LightStateClass StatusBad = null, LightStateClass StatusSupervisor = null, LightStateClass StatusInService = null, LightStateClass FasciaLight = null)
        {
            this.CardReader = CardReader;
            this.PinPad = PinPad;
            this.NotesDispenser = NotesDispenser;
            this.CoinDispenser = CoinDispenser;
            this.ReceiptPrinter = ReceiptPrinter;
            this.PassbookPrinter = PassbookPrinter;
            this.EnvelopeDepository = EnvelopeDepository;
            this.ChequeUnit = ChequeUnit;
            this.BillAcceptor = BillAcceptor;
            this.EnvelopeDispenser = EnvelopeDispenser;
            this.DocumentPrinter = DocumentPrinter;
            this.CoinAcceptor = CoinAcceptor;
            this.Scanner = Scanner;
            this.Contactless = Contactless;
            this.CardUnit2 = CardUnit2;
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
        /// Card Unit Light.
        /// </summary>
        [DataMember(Name = "cardReader")]
        public LightStateClass CardReader { get; init; }

        /// <summary>
        /// Pin Pad Light.
        /// </summary>
        [DataMember(Name = "pinPad")]
        public LightStateClass PinPad { get; init; }

        /// <summary>
        /// Notes Dispenser Light.
        /// </summary>
        [DataMember(Name = "notesDispenser")]
        public LightStateClass NotesDispenser { get; init; }

        /// <summary>
        /// Coin Dispenser Light.
        /// </summary>
        [DataMember(Name = "coinDispenser")]
        public LightStateClass CoinDispenser { get; init; }

        /// <summary>
        /// Receipt Printer Light.
        /// </summary>
        [DataMember(Name = "receiptPrinter")]
        public LightStateClass ReceiptPrinter { get; init; }

        /// <summary>
        /// Passbook Printer Light.
        /// </summary>
        [DataMember(Name = "passbookPrinter")]
        public LightStateClass PassbookPrinter { get; init; }

        /// <summary>
        /// Envelope Depository Light.
        /// </summary>
        [DataMember(Name = "envelopeDepository")]
        public LightStateClass EnvelopeDepository { get; init; }

        /// <summary>
        /// Cheque Unit Light.
        /// </summary>
        [DataMember(Name = "chequeUnit")]
        public LightStateClass ChequeUnit { get; init; }

        /// <summary>
        /// Bill Acceptor Light.
        /// </summary>
        [DataMember(Name = "billAcceptor")]
        public LightStateClass BillAcceptor { get; init; }

        /// <summary>
        /// Envelope Dispenser Light.
        /// </summary>
        [DataMember(Name = "envelopeDispenser")]
        public LightStateClass EnvelopeDispenser { get; init; }

        /// <summary>
        /// Document Printer Light.
        /// </summary>
        [DataMember(Name = "documentPrinter")]
        public LightStateClass DocumentPrinter { get; init; }

        /// <summary>
        /// Coin Acceptor Light.
        /// </summary>
        [DataMember(Name = "coinAcceptor")]
        public LightStateClass CoinAcceptor { get; init; }

        /// <summary>
        /// scanner Light.
        /// </summary>
        [DataMember(Name = "scanner")]
        public LightStateClass Scanner { get; init; }

        /// <summary>
        /// Contactless Light.
        /// </summary>
        [DataMember(Name = "contactless")]
        public LightStateClass Contactless { get; init; }

        /// <summary>
        /// Card Unit 2 Light.
        /// </summary>
        [DataMember(Name = "cardUnit2")]
        public LightStateClass CardUnit2 { get; init; }

        /// <summary>
        /// Notes Dispenser 2 Light.
        /// </summary>
        [DataMember(Name = "notesDispenser2")]
        public LightStateClass NotesDispenser2 { get; init; }

        /// <summary>
        /// Bill Acceptor 2 Light.
        /// </summary>
        [DataMember(Name = "billAcceptor2")]
        public LightStateClass BillAcceptor2 { get; init; }

        /// <summary>
        /// Status indicator light - Good.
        /// </summary>
        [DataMember(Name = "statusGood")]
        public LightStateClass StatusGood { get; init; }

        /// <summary>
        /// Status indicator light - Warning.
        /// </summary>
        [DataMember(Name = "statusWarning")]
        public LightStateClass StatusWarning { get; init; }

        /// <summary>
        /// Status indicator light - Bad.
        /// </summary>
        [DataMember(Name = "statusBad")]
        public LightStateClass StatusBad { get; init; }

        /// <summary>
        /// Status indicator light - Supervisor.
        /// </summary>
        [DataMember(Name = "statusSupervisor")]
        public LightStateClass StatusSupervisor { get; init; }

        /// <summary>
        /// Status indicator light - In Service.
        /// </summary>
        [DataMember(Name = "statusInService")]
        public LightStateClass StatusInService { get; init; }

        /// <summary>
        /// Fascia Light.
        /// </summary>
        [DataMember(Name = "fasciaLight")]
        public LightStateClass FasciaLight { get; init; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public Dictionary<string, System.Text.Json.JsonElement> ExtensionData { get; set; } = new();

        [System.Text.Json.Serialization.JsonIgnore]
        public Dictionary<string, LightStateClass> ExtendedProperties
        {
            get => MessageBase.ParseExtendedProperties<LightStateClass>(ExtensionData);
            set => ExtensionData = MessageBase.CreateExtensionData<LightStateClass>(value);
        }

    }


    [DataContract]
    public sealed class LightCapabilitiesClass
    {
        public LightCapabilitiesClass(FlashRateClass FlashRate = null, ColorClass Color = null, DirectionClass Direction = null, PositionClass Position = null)
        {
            this.FlashRate = FlashRate;
            this.Color = Color;
            this.Direction = Direction;
            this.Position = Position;
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
            /// The light can flash medium frequency.
            /// </summary>
            [DataMember(Name = "medium")]
            public bool? Medium { get; init; }

            /// <summary>
            /// The light can flash quickly.
            /// </summary>
            [DataMember(Name = "quick")]
            public bool? Quick { get; init; }

            /// <summary>
            /// The light can flash continuous (steady).
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
            /// The light can be white .
            /// </summary>
            [DataMember(Name = "white")]
            public bool? White { get; init; }

        }

        /// <summary>
        /// Indicates the light color.
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
            /// The light can  indicate entry.
            /// </summary>
            [DataMember(Name = "entry")]
            public bool? Entry { get; init; }

            /// <summary>
            /// The light can  indicate exit.
            /// </summary>
            [DataMember(Name = "exit")]
            public bool? Exit { get; init; }

        }

        /// <summary>
        /// Indicates the light direction.
        /// </summary>
        [DataMember(Name = "direction")]
        public DirectionClass Direction { get; init; }

        [DataContract]
        public sealed class PositionClass
        {
            public PositionClass(bool? Left = null, bool? Right = null, bool? Center = null, bool? Top = null, bool? Bottom = null, bool? Front = null, bool? Rear = null)
            {
                this.Left = Left;
                this.Right = Right;
                this.Center = Center;
                this.Top = Top;
                this.Bottom = Bottom;
                this.Front = Front;
                this.Rear = Rear;
            }

            /// <summary>
            /// The left position.
            /// </summary>
            [DataMember(Name = "left")]
            public bool? Left { get; init; }

            /// <summary>
            /// The right position.
            /// </summary>
            [DataMember(Name = "right")]
            public bool? Right { get; init; }

            /// <summary>
            /// The center position.
            /// </summary>
            [DataMember(Name = "center")]
            public bool? Center { get; init; }

            /// <summary>
            /// The top position.
            /// </summary>
            [DataMember(Name = "top")]
            public bool? Top { get; init; }

            /// <summary>
            /// The bottom position.
            /// </summary>
            [DataMember(Name = "bottom")]
            public bool? Bottom { get; init; }

            /// <summary>
            /// The front position.
            /// </summary>
            [DataMember(Name = "front")]
            public bool? Front { get; init; }

            /// <summary>
            /// The rear position.
            /// </summary>
            [DataMember(Name = "rear")]
            public bool? Rear { get; init; }

        }

        /// <summary>
        /// Indicates the light position.
        /// </summary>
        [DataMember(Name = "position")]
        public PositionClass Position { get; init; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(LightCapabilitiesClass CardReader = null, LightCapabilitiesClass PinPad = null, LightCapabilitiesClass NotesDispenser = null, LightCapabilitiesClass CoinDispenser = null, LightCapabilitiesClass ReceiptPrinter = null, LightCapabilitiesClass PassbookPrinter = null, LightCapabilitiesClass EnvelopeDepository = null, LightCapabilitiesClass ChequeUnit = null, LightCapabilitiesClass BillAcceptor = null, LightCapabilitiesClass EnvelopeDispenser = null, LightCapabilitiesClass DocumentPrinter = null, LightCapabilitiesClass CoinAcceptor = null, LightCapabilitiesClass Scanner = null, LightCapabilitiesClass Contactless = null, LightCapabilitiesClass CardUnit2 = null, LightCapabilitiesClass NotesDispenser2 = null, LightCapabilitiesClass BillAcceptor2 = null, LightCapabilitiesClass StatusGood = null, LightCapabilitiesClass StatusWarning = null, LightCapabilitiesClass StatusBad = null, LightCapabilitiesClass StatusSupervisor = null, LightCapabilitiesClass StatusInService = null, LightCapabilitiesClass FasciaLight = null)
        {
            this.CardReader = CardReader;
            this.PinPad = PinPad;
            this.NotesDispenser = NotesDispenser;
            this.CoinDispenser = CoinDispenser;
            this.ReceiptPrinter = ReceiptPrinter;
            this.PassbookPrinter = PassbookPrinter;
            this.EnvelopeDepository = EnvelopeDepository;
            this.ChequeUnit = ChequeUnit;
            this.BillAcceptor = BillAcceptor;
            this.EnvelopeDispenser = EnvelopeDispenser;
            this.DocumentPrinter = DocumentPrinter;
            this.CoinAcceptor = CoinAcceptor;
            this.Scanner = Scanner;
            this.Contactless = Contactless;
            this.CardUnit2 = CardUnit2;
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
        /// Card Unit Light.
        /// </summary>
        [DataMember(Name = "cardReader")]
        public LightCapabilitiesClass CardReader { get; init; }

        /// <summary>
        /// Pin Pad Light.
        /// </summary>
        [DataMember(Name = "pinPad")]
        public LightCapabilitiesClass PinPad { get; init; }

        /// <summary>
        /// Notes Dispenser Light.
        /// </summary>
        [DataMember(Name = "notesDispenser")]
        public LightCapabilitiesClass NotesDispenser { get; init; }

        /// <summary>
        /// Coin Dispenser Light.
        /// </summary>
        [DataMember(Name = "coinDispenser")]
        public LightCapabilitiesClass CoinDispenser { get; init; }

        /// <summary>
        /// Receipt Printer Light.
        /// </summary>
        [DataMember(Name = "receiptPrinter")]
        public LightCapabilitiesClass ReceiptPrinter { get; init; }

        /// <summary>
        /// Passbook Printer Light.
        /// </summary>
        [DataMember(Name = "passbookPrinter")]
        public LightCapabilitiesClass PassbookPrinter { get; init; }

        /// <summary>
        /// Envelope Depository Light.
        /// </summary>
        [DataMember(Name = "EnvelopeDepository")]
        public LightCapabilitiesClass EnvelopeDepository { get; init; }

        /// <summary>
        /// Cheque Unit Light.
        /// </summary>
        [DataMember(Name = "chequeUnit")]
        public LightCapabilitiesClass ChequeUnit { get; init; }

        /// <summary>
        /// Bill Acceptor Light.
        /// </summary>
        [DataMember(Name = "billAcceptor")]
        public LightCapabilitiesClass BillAcceptor { get; init; }

        /// <summary>
        /// Envelope Dispenser Light.
        /// </summary>
        [DataMember(Name = "envelopeDispenser")]
        public LightCapabilitiesClass EnvelopeDispenser { get; init; }

        /// <summary>
        /// Document Printer Light.
        /// </summary>
        [DataMember(Name = "documentPrinter")]
        public LightCapabilitiesClass DocumentPrinter { get; init; }

        /// <summary>
        /// Coin Acceptor Light.
        /// </summary>
        [DataMember(Name = "coinAcceptor")]
        public LightCapabilitiesClass CoinAcceptor { get; init; }

        /// <summary>
        /// Scanner Light.
        /// </summary>
        [DataMember(Name = "scanner")]
        public LightCapabilitiesClass Scanner { get; init; }

        /// <summary>
        /// Contactless Light.
        /// </summary>
        [DataMember(Name = "contactless")]
        public LightCapabilitiesClass Contactless { get; init; }

        /// <summary>
        /// Card Unit 2 Light.
        /// </summary>
        [DataMember(Name = "cardUnit2")]
        public LightCapabilitiesClass CardUnit2 { get; init; }

        /// <summary>
        /// Notes Dispenser 2 Light.
        /// </summary>
        [DataMember(Name = "notesDispenser2")]
        public LightCapabilitiesClass NotesDispenser2 { get; init; }

        /// <summary>
        /// Bill Acceptor 2 Light.
        /// </summary>
        [DataMember(Name = "billAcceptor2")]
        public LightCapabilitiesClass BillAcceptor2 { get; init; }

        /// <summary>
        /// Status indicator light - Good.
        /// </summary>
        [DataMember(Name = "statusGood")]
        public LightCapabilitiesClass StatusGood { get; init; }

        /// <summary>
        /// Status indicator light - Warning.
        /// </summary>
        [DataMember(Name = "statusWarning")]
        public LightCapabilitiesClass StatusWarning { get; init; }

        /// <summary>
        /// Status indicator light - Bad.
        /// </summary>
        [DataMember(Name = "statusBad")]
        public LightCapabilitiesClass StatusBad { get; init; }

        /// <summary>
        /// Status indicator light - Supervisor.
        /// </summary>
        [DataMember(Name = "statusSupervisor")]
        public LightCapabilitiesClass StatusSupervisor { get; init; }

        /// <summary>
        /// Status indicator light - In Service.
        /// </summary>
        [DataMember(Name = "statusInService")]
        public LightCapabilitiesClass StatusInService { get; init; }

        /// <summary>
        /// Fascia light.
        /// </summary>
        [DataMember(Name = "fasciaLight")]
        public LightCapabilitiesClass FasciaLight { get; init; }

        [System.Text.Json.Serialization.JsonExtensionData]
        public Dictionary<string, System.Text.Json.JsonElement> ExtensionData { get; set; } = new();

        [System.Text.Json.Serialization.JsonIgnore]
        public Dictionary<string, LightCapabilitiesClass> ExtendedProperties
        {
            get => MessageBase.ParseExtendedProperties<LightCapabilitiesClass>(ExtensionData);
            set => ExtensionData = MessageBase.CreateExtensionData<LightCapabilitiesClass>(value);
        }

    }


}
