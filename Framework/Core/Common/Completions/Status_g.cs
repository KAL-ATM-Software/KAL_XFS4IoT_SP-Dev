/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * Status_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Common.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "Common.Status")]
    public sealed class StatusCompletion : Completion<StatusCompletion.PayloadData>
    {
        public StatusCompletion(int RequestId, StatusCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(StatusPropertiesClass Common = null, CardReader.StatusClass CardReader = null, CashAcceptor.StatusClass CashAcceptor = null, CashDispenser.StatusClass CashDispenser = null, CashManagement.StatusClass CashManagement = null, Check.StatusClass Check = null, MixedMedia.StatusClass MixedMedia = null, KeyManagement.StatusClass KeyManagement = null, Keyboard.StatusClass Keyboard = null, TextTerminal.StatusClass TextTerminal = null, Printer.StatusClass Printer = null, BarcodeReader.StatusClass BarcodeReader = null, Biometric.StatusClass Biometric = null, Camera.StatusClass Camera = null, Lights.StatusClass Lights = null, IntelligentBanknoteNeutralization.StatusClass BanknoteNeutralization = null, Auxiliaries.StatusClass Auxiliaries = null, Deposit.StatusClass Deposit = null, VendorMode.StatusClass VendorMode = null, VendorApplication.StatusClass VendorApplication = null, PowerManagement.StatusClass PowerManagement = null)
                : base()
            {
                this.Common = Common;
                this.CardReader = CardReader;
                this.CashAcceptor = CashAcceptor;
                this.CashDispenser = CashDispenser;
                this.CashManagement = CashManagement;
                this.Check = Check;
                this.MixedMedia = MixedMedia;
                this.KeyManagement = KeyManagement;
                this.Keyboard = Keyboard;
                this.TextTerminal = TextTerminal;
                this.Printer = Printer;
                this.BarcodeReader = BarcodeReader;
                this.Biometric = Biometric;
                this.Camera = Camera;
                this.Lights = Lights;
                this.BanknoteNeutralization = BanknoteNeutralization;
                this.Auxiliaries = Auxiliaries;
                this.Deposit = Deposit;
                this.VendorMode = VendorMode;
                this.VendorApplication = VendorApplication;
                this.PowerManagement = PowerManagement;
            }

            [DataMember(Name = "common")]
            public StatusPropertiesClass Common { get; init; }

            [DataMember(Name = "cardReader")]
            public CardReader.StatusClass CardReader { get; init; }

            [DataMember(Name = "cashAcceptor")]
            public CashAcceptor.StatusClass CashAcceptor { get; init; }

            [DataMember(Name = "cashDispenser")]
            public CashDispenser.StatusClass CashDispenser { get; init; }

            [DataMember(Name = "cashManagement")]
            public CashManagement.StatusClass CashManagement { get; init; }

            [DataMember(Name = "check")]
            public Check.StatusClass Check { get; init; }

            [DataMember(Name = "mixedMedia")]
            public MixedMedia.StatusClass MixedMedia { get; init; }

            [DataMember(Name = "keyManagement")]
            public KeyManagement.StatusClass KeyManagement { get; init; }

            [DataMember(Name = "keyboard")]
            public Keyboard.StatusClass Keyboard { get; init; }

            [DataMember(Name = "textTerminal")]
            public TextTerminal.StatusClass TextTerminal { get; init; }

            [DataMember(Name = "printer")]
            public Printer.StatusClass Printer { get; init; }

            [DataMember(Name = "barcodeReader")]
            public BarcodeReader.StatusClass BarcodeReader { get; init; }

            [DataMember(Name = "biometric")]
            public Biometric.StatusClass Biometric { get; init; }

            [DataMember(Name = "camera")]
            public Camera.StatusClass Camera { get; init; }

            [DataMember(Name = "lights")]
            public Lights.StatusClass Lights { get; init; }

            [DataMember(Name = "banknoteNeutralization")]
            public IntelligentBanknoteNeutralization.StatusClass BanknoteNeutralization { get; init; }

            [DataMember(Name = "auxiliaries")]
            public Auxiliaries.StatusClass Auxiliaries { get; init; }

            [DataMember(Name = "deposit")]
            public Deposit.StatusClass Deposit { get; init; }

            [DataMember(Name = "vendorMode")]
            public VendorMode.StatusClass VendorMode { get; init; }

            [DataMember(Name = "vendorApplication")]
            public VendorApplication.StatusClass VendorApplication { get; init; }

            [DataMember(Name = "powerManagement")]
            public PowerManagement.StatusClass PowerManagement { get; init; }

        }
    }
}
