/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public partial class CommonServiceClass
    {

        /// <summary>
        /// Stores CashDispenser interface capabilites internally
        /// </summary>
        public CashDispenserCapabilitiesClass CashDispenserCapabilities { get; set; }

        /// <summary>
        /// Stores CashManagement interface capabilites internally
        /// </summary>
        public CashManagementCapabilitiesClass CashManagementCapabilities { get; set; }

        /// <summary>
        /// Stores CardReader interface capabilites internally
        /// </summary>
        public CardReaderCapabilitiesClass CardReaderCapabilities { get; set; }

        /// <summary>
        /// Stores TextTerminal interface capabilites internally
        /// </summary>
        public TextTerminalCapabilitiesClass TextTerminalCapabilities { get; set; }

        /// <summary>
        /// Stores KeyManagement interface capabilites internally
        /// </summary>
        public KeyManagementCapabilitiesClass KeyManagementCapabilities { get; set; }

        /// <summary>
        /// Stores Crypto interface capabilites internally
        /// </summary>
        public CryptoCapabilitiesClass CryptoCapabilities { get; set; }
    }
}
