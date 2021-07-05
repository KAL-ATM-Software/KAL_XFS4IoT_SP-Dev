/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
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
    }

    public interface ICommonServiceClass : ICommonService, ICommonUnsolicitedEvents
    {
    }
}
