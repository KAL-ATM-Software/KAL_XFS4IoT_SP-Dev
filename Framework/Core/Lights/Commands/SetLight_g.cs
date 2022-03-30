/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Lights interface.
 * SetLight_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Lights.Commands
{
    //Original name = SetLight
    [DataContract]
    [Command(Name = "Lights.SetLight")]
    public sealed class SetLightCommand : Command<SetLightCommand.PayloadData>
    {
        public SetLightCommand(int RequestId, SetLightCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, LightStateClass CardReader = null, LightStateClass PinPad = null, LightStateClass NotesDispenser = null, LightStateClass CoinDispenser = null, LightStateClass ReceiptPrinter = null, LightStateClass PassbookPrinter = null, LightStateClass EnvelopeDepository = null, LightStateClass BillAcceptor = null, LightStateClass EnvelopeDispenser = null, LightStateClass DocumentPrinter = null, LightStateClass CoinAcceptor = null, LightStateClass Scanner = null, LightStateClass Contactless = null, LightStateClass CardReader2 = null, LightStateClass NotesDispenser2 = null, LightStateClass BillAcceptor2 = null, LightStateClass StatusGood = null, LightStateClass StatusWarning = null, LightStateClass StatusBad = null, LightStateClass StatusSupervisor = null, LightStateClass StatusInService = null, LightStateClass FasciaLight = null)
                : base(Timeout)
            {
                this.CardReader = CardReader;
                this.PinPad = PinPad;
                this.NotesDispenser = NotesDispenser;
                this.CoinDispenser = CoinDispenser;
                this.ReceiptPrinter = ReceiptPrinter;
                this.PassbookPrinter = PassbookPrinter;
                this.EnvelopeDepository = EnvelopeDepository;
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
            /// Scanner Light.
            /// </summary>
            [DataMember(Name = "scanner")]
            public LightStateClass Scanner { get; init; }

            /// <summary>
            /// Contactless Reader Light.
            /// </summary>
            [DataMember(Name = "contactless")]
            public LightStateClass Contactless { get; init; }

            /// <summary>
            /// Card Reader 2 Light.
            /// </summary>
            [DataMember(Name = "cardReader2")]
            public LightStateClass CardReader2 { get; init; }

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
            /// Status Indicator light - Good.
            /// </summary>
            [DataMember(Name = "statusGood")]
            public LightStateClass StatusGood { get; init; }

            /// <summary>
            /// Status Indicator light - Warning.
            /// </summary>
            [DataMember(Name = "statusWarning")]
            public LightStateClass StatusWarning { get; init; }

            /// <summary>
            /// Status Indicator light - Bad.
            /// </summary>
            [DataMember(Name = "statusBad")]
            public LightStateClass StatusBad { get; init; }

            /// <summary>
            /// Status Indicator light - Supervisor.
            /// </summary>
            [DataMember(Name = "statusSupervisor")]
            public LightStateClass StatusSupervisor { get; init; }

            /// <summary>
            /// Status Indicator light - In Service.
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
    }
}
