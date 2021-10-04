/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using XFS4IoTFramework.PinPad;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public interface IPinPadService : IKeyManagementService, ICommonService
    {
        /// <summary>
        /// List of PCI Security Standards Council PIN transaction security (PTS) certification held by the PIN device
        /// </summary>
        PCIPTSDeviceIdClass PCIPTSDeviceId { get; set; }
    }

    public interface IPinPadServiceClass : IPinPadService, IPinPadUnsolicitedEvents
    {
    }
}
