/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "Common.Capabilities")]
    public sealed class CapabilitiesCompletion : Completion<CapabilitiesCompletion.PayloadData>
    {
        public CapabilitiesCompletion(int RequestId, CapabilitiesCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<InterfaceClass> Interfaces = null, CapabilityPropertiesClass Common = null, CardReader.CapabilitiesClass CardReader = null, CashAcceptor.CapabilitiesClass CashAcceptor = null, Dispenser.CapabilitiesClass CashDispenser = null, CashManagement.CapabilitiesClass CashManagement = null, Pinpad.CapabilitiesClass PinPad = null, Crypto.CapabilitiesClass Crypto = null, KeyManagement.CapabilitiesClass KeyManagement = null, Keyboard.CapabilitiesClass Keyboard = null, TextTerminal.CapabilitiesClass TextTerminal = null, Printer.CapabilitiesClass Printer = null, SensorsAndIndicators.CapabilitiesClass SensorsAndIndicators = null, CardEmbosser.CapabilitiesClass CardEmbosser = null, BarcodeReader.CapabilitiesClass BarcodeReader = null, Biometric.CapabilitiesClass Biometric = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Interfaces = Interfaces;
                this.Common = Common;
                this.CardReader = CardReader;
                this.CashAcceptor = CashAcceptor;
                this.CashDispenser = CashDispenser;
                this.CashManagement = CashManagement;
                this.PinPad = PinPad;
                this.Crypto = Crypto;
                this.KeyManagement = KeyManagement;
                this.Keyboard = Keyboard;
                this.TextTerminal = TextTerminal;
                this.Printer = Printer;
                this.SensorsAndIndicators = SensorsAndIndicators;
                this.CardEmbosser = CardEmbosser;
                this.BarcodeReader = BarcodeReader;
                this.Biometric = Biometric;
            }

            /// <summary>
            /// Array of interfaces supported by this XFS4IoT service.
            /// </summary>
            [DataMember(Name = "interfaces")]
            public List<InterfaceClass> Interfaces { get; private set; }

            /// <summary>
            /// Capability information common to all XFS4IoT services.
            /// </summary>
            [DataMember(Name = "common")]
            public CapabilityPropertiesClass Common { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the CardReader interface. This will be omitted if the CardReader interface is not supported.
            /// </summary>
            [DataMember(Name = "cardReader")]
            public CardReader.CapabilitiesClass CardReader { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the CashAcceptor interface. This will be omitted if the CashAcceptor interface is not supported.
            /// </summary>
            [DataMember(Name = "cashAcceptor")]
            public CashAcceptor.CapabilitiesClass CashAcceptor { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the CashDispenser interface. This will be omitted if the CashDispenser interface is not supported.
            /// </summary>
            [DataMember(Name = "cashDispenser")]
            public Dispenser.CapabilitiesClass CashDispenser { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the CashManagement interface. This will be omitted if the CashManagement interface is not supported.
            /// </summary>
            [DataMember(Name = "cashManagement")]
            public CashManagement.CapabilitiesClass CashManagement { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the PinPad interface. This will be omitted if the PinPad interface is not supported.
            /// </summary>
            [DataMember(Name = "pinPad")]
            public Pinpad.CapabilitiesClass PinPad { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the Crypto interface. This will be omitted if the Crypto interface is not supported.
            /// </summary>
            [DataMember(Name = "crypto")]
            public Crypto.CapabilitiesClass Crypto { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the KeyManagement interface. This will be omitted if the KeyManagement interface is not supported.
            /// </summary>
            [DataMember(Name = "keyManagement")]
            public KeyManagement.CapabilitiesClass KeyManagement { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the Keyboard interface. This will be omitted if the Keyboard interface is not supported.
            /// </summary>
            [DataMember(Name = "keyboard")]
            public Keyboard.CapabilitiesClass Keyboard { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the TextTerminal interface. This will be omitted if the TextTerminal interface is not supported.
            /// </summary>
            [DataMember(Name = "textTerminal")]
            public TextTerminal.CapabilitiesClass TextTerminal { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the Printer interface. This will be omitted if the Printer interface is not supported.
            /// </summary>
            [DataMember(Name = "printer")]
            public Printer.CapabilitiesClass Printer { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the Sensors and Indicators interface. This will be omitted if the Sensors and Indicators interface is not supported.
            /// </summary>
            [DataMember(Name = "SensorsAndIndicators")]
            public SensorsAndIndicators.CapabilitiesClass SensorsAndIndicators { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the CardEmbosser interface. This will be omitted if the CardEmbosser interface is not supported.
            /// </summary>
            [DataMember(Name = "cardEmbosser")]
            public CardEmbosser.CapabilitiesClass CardEmbosser { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the BarcodeReader interface. This will be omitted if the BarcodeReader interface is not supported.
            /// </summary>
            [DataMember(Name = "barcodeReader")]
            public BarcodeReader.CapabilitiesClass BarcodeReader { get; private set; }

            /// <summary>
            /// Capability information for XFS4IoT services implementing the Biometrics interface. This will be omitted if the Biometrics interface is not supported.
            /// </summary>
            [DataMember(Name = "biometric")]
            public Biometric.CapabilitiesClass Biometric { get; private set; }

        }
    }
}
