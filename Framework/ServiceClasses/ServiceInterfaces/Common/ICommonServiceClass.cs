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
        /// Stores CashDispenser interface capabilites internally
        /// </summary>
        CashDispenserCapabilitiesClass CashDispenserCapabilities { get => null; set { } }

        /// <summary>
        /// Stores CashManagement interface capabilites internally
        /// </summary>
        CashManagementCapabilitiesClass CashManagementCapabilities { get => null; set { } }

        /// <summary>
        /// Stores CardReader interface capabilites internally
        /// </summary>
        CardReaderCapabilitiesClass CardReaderCapabilities { get => null; set { } }

        /// <summary>
        /// Stores TextTerminal interface capabilites internally
        /// </summary>
        TextTerminalCapabilitiesClass TextTerminalCapabilities { get => null; set { } }

        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        KeyManagementCapabilitiesClass KeyManagementCapabilities { get => null; set { } }

        /// <summary>
        /// Stores Crypto interface capabilites internally
        /// </summary>
        CryptoCapabilitiesClass CryptoCapabilities { get => null; set { } }

        /// <summary>
        /// Stores PinPad interface capabilites internally
        /// </summary>
        PinPadCapabilitiesClass PinPadCapabilities { get => null; set { } }


        /// <summary>
        /// Stores Keyboard interface capabilites internally
        /// </summary>
        KeyboardCapabilitiesClass KeyboardCapabilities { get => null; set { } }
    }

    public interface ICommonServiceClass : ICommonService, ICommonUnsolicitedEvents
    {
    }
}
