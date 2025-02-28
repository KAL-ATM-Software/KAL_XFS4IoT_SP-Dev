/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace XFS4IoTFramework.Common
{
    public sealed class PowerManagementCapabilitiesClass(
        bool PowerSaveControl = false,
        bool BatteryRechargeable = false) : StatusBase
    {
        /// <summary>
        /// Specifies whether power saving control is available.
        /// </summary>
        public bool PowerSaveControl = PowerSaveControl;

        /// <summary>
        /// Specifies whether the battery is rechargeable or not.
        /// </summary>
        public bool BatteryRechargeable = BatteryRechargeable;
    }
}
