/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * Capabilities_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Common.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "Common.Capabilities")]
    public sealed class CapabilitiesCompletion : Completion<CapabilitiesCompletion.PayloadData>
    {
        public CapabilitiesCompletion()
            : base()
        { }

        public CapabilitiesCompletion(int RequestId, CapabilitiesCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<InterfaceClass> Interfaces = null, CapabilityPropertiesClass Common = null, CardReader.CapabilitiesClass CardReader = null, CashAcceptor.CapabilitiesClass CashAcceptor = null, CashDispenser.CapabilitiesClass CashDispenser = null, CashManagement.CapabilitiesClass CashManagement = null, Check.CapabilitiesClass Check = null, MixedMedia.CapabilitiesClass MixedMedia = null, PinPad.CapabilitiesClass PinPad = null, Crypto.CapabilitiesClass Crypto = null, KeyManagement.CapabilitiesClass KeyManagement = null, Keyboard.CapabilitiesClass Keyboard = null, TextTerminal.CapabilitiesClass TextTerminal = null, Printer.CapabilitiesClass Printer = null, BarcodeReader.CapabilitiesClass BarcodeReader = null, Biometric.CapabilitiesClass Biometric = null, Camera.CapabilitiesClass Camera = null, German.CapabilitiesClass German = null, Lights.CapabilitiesClass Lights = null, BanknoteNeutralization.CapabilitiesClass BanknoteNeutralization = null, Auxiliaries.CapabilitiesClass Auxiliaries = null, Deposit.CapabilitiesClass Deposit = null, VendorApplication.CapabilitiesClass VendorApplication = null, PowerManagement.CapabilitiesClass PowerManagement = null)
                : base()
            {
                this.Interfaces = Interfaces;
                this.Common = Common;
                this.CardReader = CardReader;
                this.CashAcceptor = CashAcceptor;
                this.CashDispenser = CashDispenser;
                this.CashManagement = CashManagement;
                this.Check = Check;
                this.MixedMedia = MixedMedia;
                this.PinPad = PinPad;
                this.Crypto = Crypto;
                this.KeyManagement = KeyManagement;
                this.Keyboard = Keyboard;
                this.TextTerminal = TextTerminal;
                this.Printer = Printer;
                this.BarcodeReader = BarcodeReader;
                this.Biometric = Biometric;
                this.Camera = Camera;
                this.German = German;
                this.Lights = Lights;
                this.BanknoteNeutralization = BanknoteNeutralization;
                this.Auxiliaries = Auxiliaries;
                this.Deposit = Deposit;
                this.VendorApplication = VendorApplication;
                this.PowerManagement = PowerManagement;
            }

            /// <summary>
            /// Array of interfaces supported by this XFS4IoT service.
            /// </summary>
            [DataMember(Name = "interfaces")]
            public List<InterfaceClass> Interfaces { get; init; }

            [DataMember(Name = "common")]
            public CapabilityPropertiesClass Common { get; init; }

            [DataMember(Name = "cardReader")]
            public CardReader.CapabilitiesClass CardReader { get; init; }

            [DataMember(Name = "cashAcceptor")]
            public CashAcceptor.CapabilitiesClass CashAcceptor { get; init; }

            [DataMember(Name = "cashDispenser")]
            public CashDispenser.CapabilitiesClass CashDispenser { get; init; }

            [DataMember(Name = "cashManagement")]
            public CashManagement.CapabilitiesClass CashManagement { get; init; }

            [DataMember(Name = "check")]
            public Check.CapabilitiesClass Check { get; init; }

            [DataMember(Name = "mixedMedia")]
            public MixedMedia.CapabilitiesClass MixedMedia { get; init; }

            [DataMember(Name = "pinPad")]
            public PinPad.CapabilitiesClass PinPad { get; init; }

            [DataMember(Name = "crypto")]
            public Crypto.CapabilitiesClass Crypto { get; init; }

            [DataMember(Name = "keyManagement")]
            public KeyManagement.CapabilitiesClass KeyManagement { get; init; }

            [DataMember(Name = "keyboard")]
            public Keyboard.CapabilitiesClass Keyboard { get; init; }

            [DataMember(Name = "textTerminal")]
            public TextTerminal.CapabilitiesClass TextTerminal { get; init; }

            [DataMember(Name = "printer")]
            public Printer.CapabilitiesClass Printer { get; init; }

            [DataMember(Name = "barcodeReader")]
            public BarcodeReader.CapabilitiesClass BarcodeReader { get; init; }

            [DataMember(Name = "biometric")]
            public Biometric.CapabilitiesClass Biometric { get; init; }

            [DataMember(Name = "camera")]
            public Camera.CapabilitiesClass Camera { get; init; }

            [DataMember(Name = "german")]
            public German.CapabilitiesClass German { get; init; }

            [DataMember(Name = "lights")]
            public Lights.CapabilitiesClass Lights { get; init; }

            [DataMember(Name = "banknoteNeutralization")]
            public BanknoteNeutralization.CapabilitiesClass BanknoteNeutralization { get; init; }

            [DataMember(Name = "auxiliaries")]
            public Auxiliaries.CapabilitiesClass Auxiliaries { get; init; }

            [DataMember(Name = "deposit")]
            public Deposit.CapabilitiesClass Deposit { get; init; }

            [DataMember(Name = "vendorApplication")]
            public VendorApplication.CapabilitiesClass VendorApplication { get; init; }

            [DataMember(Name = "powerManagement")]
            public PowerManagement.CapabilitiesClass PowerManagement { get; init; }

        }
    }
}
