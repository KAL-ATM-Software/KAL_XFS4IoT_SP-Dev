/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Lights.SetLight")]
    public sealed class SetLightCommand : Command<SetLightCommand.PayloadData>
    {
        public SetLightCommand(int RequestId, SetLightCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(LightStateClass CardReader = null, LightStateClass PinPad = null, LightStateClass NotesDispenser = null, LightStateClass CoinDispenser = null, LightStateClass ReceiptPrinter = null, LightStateClass PassbookPrinter = null, LightStateClass EnvelopeDepository = null, LightStateClass CheckUnit = null, LightStateClass BillAcceptor = null, LightStateClass EnvelopeDispenser = null, LightStateClass DocumentPrinter = null, LightStateClass CoinAcceptor = null, LightStateClass Scanner = null, LightStateClass Contactless = null, LightStateClass CardReader2 = null, LightStateClass NotesDispenser2 = null, LightStateClass BillAcceptor2 = null, LightStateClass StatusGood = null, LightStateClass StatusWarning = null, LightStateClass StatusBad = null, LightStateClass StatusSupervisor = null, LightStateClass StatusInService = null, LightStateClass FasciaLight = null)
                : base()
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
            public LightStateClass CardReader { get; init; }

            /// <summary>
            /// Pin Pad Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "pinPad")]
            public LightStateClass PinPad { get; init; }

            /// <summary>
            /// Notes Dispenser Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "notesDispenser")]
            public LightStateClass NotesDispenser { get; init; }

            /// <summary>
            /// Coin Dispenser Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "coinDispenser")]
            public LightStateClass CoinDispenser { get; init; }

            /// <summary>
            /// Receipt Printer Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "receiptPrinter")]
            public LightStateClass ReceiptPrinter { get; init; }

            /// <summary>
            /// Passbook Printer Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "passbookPrinter")]
            public LightStateClass PassbookPrinter { get; init; }

            /// <summary>
            /// Envelope Depository Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "envelopeDepository")]
            public LightStateClass EnvelopeDepository { get; init; }

            /// <summary>
            /// Check Unit Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "checkUnit")]
            public LightStateClass CheckUnit { get; init; }

            /// <summary>
            /// Bill Acceptor Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "billAcceptor")]
            public LightStateClass BillAcceptor { get; init; }

            /// <summary>
            /// Envelope Dispenser Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "envelopeDispenser")]
            public LightStateClass EnvelopeDispenser { get; init; }

            /// <summary>
            /// Document Printer Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "documentPrinter")]
            public LightStateClass DocumentPrinter { get; init; }

            /// <summary>
            /// Coin Acceptor Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "coinAcceptor")]
            public LightStateClass CoinAcceptor { get; init; }

            /// <summary>
            /// Scanner Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "scanner")]
            public LightStateClass Scanner { get; init; }

            /// <summary>
            /// Contactless Reader Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "contactless")]
            public LightStateClass Contactless { get; init; }

            /// <summary>
            /// Card Reader 2 Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "cardReader2")]
            public LightStateClass CardReader2 { get; init; }

            /// <summary>
            /// Notes Dispenser 2 Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "notesDispenser2")]
            public LightStateClass NotesDispenser2 { get; init; }

            /// <summary>
            /// Bill Acceptor 2 Light. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "billAcceptor2")]
            public LightStateClass BillAcceptor2 { get; init; }

            /// <summary>
            /// Status Indicator light - Good. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "statusGood")]
            public LightStateClass StatusGood { get; init; }

            /// <summary>
            /// Status Indicator light - Warning. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "statusWarning")]
            public LightStateClass StatusWarning { get; init; }

            /// <summary>
            /// Status Indicator light - Bad. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "statusBad")]
            public LightStateClass StatusBad { get; init; }

            /// <summary>
            /// Status Indicator light - Supervisor. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "statusSupervisor")]
            public LightStateClass StatusSupervisor { get; init; }

            /// <summary>
            /// Status Indicator light - In Service. This property is null if not applicable.
            /// </summary>
            [DataMember(Name = "statusInService")]
            public LightStateClass StatusInService { get; init; }

            /// <summary>
            /// Fascia Light. This property is null if not applicable.
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
