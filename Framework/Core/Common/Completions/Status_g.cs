/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "Common.Status")]
    public sealed class StatusCompletion : Completion<StatusCompletion.PayloadData>
    {
        public StatusCompletion(int RequestId, StatusCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, StatusPropertiesClass Common = null, CardReader.StatusClass CardReader = null, CashAcceptor.StatusClass CashAcceptor = null, CashDispenser.StatusClass CashDispenser = null, CashManagement.StatusClass CashManagement = null, KeyManagement.StatusClass KeyManagement = null, Keyboard.StatusClass Keyboard = null, TextTerminal.StatusClass TextTerminal = null, Printer.StatusClass Printer = null, CardEmbosser.StatusClass CardEmbosser = null, BarcodeReader.StatusClass BarcodeReader = null, Biometric.StatusClass Biometric = null, Dictionary<string, Lights.LightStateClass> Lights = null, Auxiliaries.StatusClass Auxiliaries = null, VendorMode.StatusClass VendorMode = null, VendorApplication.StatusClass VendorApplication = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Common = Common;
                this.CardReader = CardReader;
                this.CashAcceptor = CashAcceptor;
                this.CashDispenser = CashDispenser;
                this.CashManagement = CashManagement;
                this.KeyManagement = KeyManagement;
                this.Keyboard = Keyboard;
                this.TextTerminal = TextTerminal;
                this.Printer = Printer;
                this.CardEmbosser = CardEmbosser;
                this.BarcodeReader = BarcodeReader;
                this.Biometric = Biometric;
                this.Lights = Lights;
                this.Auxiliaries = Auxiliaries;
                this.VendorMode = VendorMode;
                this.VendorApplication = VendorApplication;
            }

            /// <summary>
            /// Status information common to all XFS4IoT services.
            /// </summary>
            [DataMember(Name = "common")]
            public StatusPropertiesClass Common { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the CardReader interface. This will be omitted if the
            /// CardReader interface is not supported.
            /// </summary>
            [DataMember(Name = "cardReader")]
            public CardReader.StatusClass CardReader { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the CashAcceptor interface. This will be omitted if the
            /// CashAcceptor interface is not supported.
            /// </summary>
            [DataMember(Name = "cashAcceptor")]
            public CashAcceptor.StatusClass CashAcceptor { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the CashDispenser interface. This will be omitted if
            /// the CashDispenser interface is not supported.
            /// </summary>
            [DataMember(Name = "cashDispenser")]
            public CashDispenser.StatusClass CashDispenser { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the CashManagement interface. This will be omitted if
            /// the CashManagement interface is not supported.
            /// </summary>
            [DataMember(Name = "cashManagement")]
            public CashManagement.StatusClass CashManagement { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the KeyManagement interface. This will be omitted if
            /// the KeyManagement interface is not supported.
            /// </summary>
            [DataMember(Name = "keyManagement")]
            public KeyManagement.StatusClass KeyManagement { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the Keyboard interface. This will be omitted if the
            /// Keyboard interface is not supported.
            /// </summary>
            [DataMember(Name = "keyboard")]
            public Keyboard.StatusClass Keyboard { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the TextTerminal interface. This will be omitted if the
            /// TextTerminal interface is not supported.
            /// </summary>
            [DataMember(Name = "textTerminal")]
            public TextTerminal.StatusClass TextTerminal { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the Printer interface. This will be omitted if the
            /// Printer interface is not supported.
            /// </summary>
            [DataMember(Name = "printer")]
            public Printer.StatusClass Printer { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the CardEmbosser interface. This will be omitted if the
            /// CardEmbosser interface is not supported.
            /// </summary>
            [DataMember(Name = "cardEmbosser")]
            public CardEmbosser.StatusClass CardEmbosser { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the BarcodeReader interface. This will be omitted if
            /// the BarcodeReader interface is not supported.
            /// </summary>
            [DataMember(Name = "barcodeReader")]
            public BarcodeReader.StatusClass BarcodeReader { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the Biometrics interface. This will be omitted if the
            /// Biometrics interface is not supported.
            /// </summary>
            [DataMember(Name = "biometric")]
            public Biometric.StatusClass Biometric { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the Lights interface. This will be omitted if the
            /// Lights interface is not supported.
            /// </summary>
            [DataMember(Name = "lights")]
            public Dictionary<string, Lights.LightStateClass> Lights { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the Auxiliaries interface. This will be omitted if the
            /// Auxiliaries interface is not supported.
            /// </summary>
            [DataMember(Name = "auxiliaries")]
            public Auxiliaries.StatusClass Auxiliaries { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the VendorMode interface. This will be omitted if the
            /// VendorMode interface is not supported.
            /// </summary>
            [DataMember(Name = "vendorMode")]
            public VendorMode.StatusClass VendorMode { get; init; }

            /// <summary>
            /// Status information for XFS4IoT services implementing the Vendor Application interface. This 
            /// will be omitted if the Vendor Mode interface is not supported.
            /// </summary>
            [DataMember(Name = "vendorApplication")]
            public VendorApplication.StatusClass VendorApplication { get; init; }

        }
    }
}
