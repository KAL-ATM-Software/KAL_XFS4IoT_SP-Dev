/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT.Common.Events;

namespace XFS4IoTFramework.Common
{
    public interface ICommonService
    {
        /// <summary>
        /// Stores Common Capabilities
        /// </summary>
        CommonCapabilitiesClass CommonCapabilities { get; set; }

        /// <summary>
        /// Stores CashDispenser capabilites for an internal use
        /// </summary>
        CashDispenserCapabilitiesClass CashDispenserCapabilities { get => null; set { } }

        /// <summary>
        /// Stores CashManagement capabilites for an internal use
        /// </summary>
        CashManagementCapabilitiesClass CashManagementCapabilities { get => null; set { } }

        /// <summary>
        /// Stores CardReader capabilites for an internal use
        /// </summary>
        CardReaderCapabilitiesClass CardReaderCapabilities { get => null; set { } }

        /// <summary>
        /// Stores TextTerminal capabilites for an internal use
        /// </summary>
        TextTerminalCapabilitiesClass TextTerminalCapabilities { get => null; set { } }

        /// <summary>
        /// Stores KeyManagement capabilites for an internal use
        /// </summary> 
        KeyManagementCapabilitiesClass KeyManagementCapabilities { get => null; set { } }

        /// <summary>
        /// Stores Crypto capabilites for an internal use
        /// </summary>
        CryptoCapabilitiesClass CryptoCapabilities { get => null; set { } }

        /// <summary>
        /// Stores PinPad capabilites for an internal use
        /// </summary> 
        PinPadCapabilitiesClass PinPadCapabilities { get => null; set { } }

        /// <summary>
        /// Stores Keyboard capabilites for an internal use
        /// </summary>
        KeyboardCapabilitiesClass KeyboardCapabilities { get => null; set { } }

        /// <summary>
        /// Stores Lights capabilities for an internal use
        /// </summary>
        LightsCapabilitiesClass LightsCapabilities { get => null; set { } }

        /// <summary>
        /// Stores Printer capabilities for an internal use
        /// </summary>
        PrinterCapabilitiesClass PrinterCapabilities { get => null; set { } }

        /// <summary>
        /// Stores Auxiliaries capabilities for an internal use
        /// </summary>
        AuxiliariesCapabilities AuxiliariesCapabilities { get => null; set { } }

        /// <summary>
        /// Stores vendor application capabilites
        /// </summary>
        VendorApplicationCapabilitiesClass VendorApplicationCapabilities { get => null; set { } }

        /// <summary>
        /// Stores Commons status
        /// </summary>
        CommonStatusClass CommonStatus { get; set; }

        /// <summary>
        /// Stores CardReader status
        /// </summary>
        CardReaderStatusClass CardReaderStatus { get => null; set { } }

        /// <summary>
        /// Stores CashDispenser status
        /// </summary>
        CashDispenserStatusClass CashDispenserStatus { get => null; set { } }

        /// <summary>
        /// Stores CashManagement status
        /// </summary>
        CashManagementStatusClass CashManagementStatus { get => null; set { } }

        /// <summary>
        /// Stores KeyManagement status
        /// </summary>
        KeyManagementStatusClass KeyManagementStatus { get => null; set { } }

        /// <summary>
        /// Stores Keyboard status
        /// </summary>
        KeyboardStatusClass KeyboardStatus { get => null; set { } }

        /// <summary>
        /// Stores CardReader status
        /// </summary>
        TextTerminalStatusClass TextTerminalStatus { get => null; set { } }

        /// <summary>
        /// Stores light status
        /// </summary>
        LightsStatusClass LightsStatus { get => null; set { } }

        /// <summary>
        /// Stores printer status
        /// </summary>
        PrinterStatusClass PrinterStatus { get => null; set { } }
		
		/// <summary>
        /// Stores auxiliaries status
        /// </summary>
        AuxiliariesStatus AuxiliariesStatus { get => null; set { } }

        /// <summary>
        /// Stores vendor mode status
        /// </summary>
        VendorModeStatusClass VendorModeStatus { get => null; set { } }

        /// <summary>
        /// Stores vendor application status
        /// </summary>
        VendorApplicationStatusClass VendorApplicationStatus { get => null; set { } }

        /// <summary>
        /// Status changed event
        /// </summary>
        Task StatusChangedEvent(CommonStatusClass.DeviceEnum? Device,
                                CommonStatusClass.PositionStatusEnum? Position,
                                int? PowerSaveRecoveryTime,
                                CommonStatusClass.AntiFraudModuleEnum? AntiFraudModule,
                                CommonStatusClass.ExchangeEnum? Exchange,
                                CommonStatusClass.EndToEndSecurityEnum? EndToEndSecurity);

        /// <summary>
        /// Nonce cleared event
        /// </summary>
        Task NonceClearedEvent(string ReasonDescription);

        /// <summary>
        /// Error event
        /// </summary>
        Task ErrorEvent(CommonStatusClass.ErrorEventIdEnum EventId,
                        CommonStatusClass.ErrorActionEnum Action,
                        string VendorDescription);
    }

    public interface ICommonServiceClass : ICommonService, ICommonUnsolicitedEvents
    {
    }
}
