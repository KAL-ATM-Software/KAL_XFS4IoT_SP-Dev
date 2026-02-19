/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Lights.SetLight")]
    public sealed class SetLightCommand : Command<SetLightCommand.PayloadData>
    {
        public SetLightCommand()
            : base()
        { }

        public SetLightCommand(int RequestId, SetLightCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(Dictionary<string, PositionStatusClass> CardReader = null, Dictionary<string, PositionStatusClass> PinPad = null, Dictionary<string, PositionStatusClass> NotesDispenser = null, Dictionary<string, PositionStatusClass> CoinDispenser = null, Dictionary<string, PositionStatusClass> ReceiptPrinter = null, Dictionary<string, PositionStatusClass> PassbookPrinter = null, Dictionary<string, PositionStatusClass> EnvelopeDepository = null, Dictionary<string, PositionStatusClass> CheckUnit = null, Dictionary<string, PositionStatusClass> BillAcceptor = null, Dictionary<string, PositionStatusClass> EnvelopeDispenser = null, Dictionary<string, PositionStatusClass> DocumentPrinter = null, Dictionary<string, PositionStatusClass> CoinAcceptor = null, Dictionary<string, PositionStatusClass> Scanner = null, Dictionary<string, PositionStatusClass> Contactless = null, Dictionary<string, PositionStatusClass> CardReader2 = null, Dictionary<string, PositionStatusClass> NotesDispenser2 = null, Dictionary<string, PositionStatusClass> BillAcceptor2 = null, Dictionary<string, PositionStatusClass> StatusGood = null, Dictionary<string, PositionStatusClass> StatusWarning = null, Dictionary<string, PositionStatusClass> StatusBad = null, Dictionary<string, PositionStatusClass> StatusSupervisor = null, Dictionary<string, PositionStatusClass> StatusInService = null, Dictionary<string, PositionStatusClass> FasciaLight = null)
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
    }
}
