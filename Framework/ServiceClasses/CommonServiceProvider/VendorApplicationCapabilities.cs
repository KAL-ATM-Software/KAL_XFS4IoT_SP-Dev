/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// VendorApplicationCapabilities
    /// Store device capabilites for the vendor mode application interface for VDM
    /// </summary>
    public sealed class VendorApplicationCapabilitiesClass
    {

        [Flags]
        public enum SupportedAccessLevelEnum
        {
            NotSupported = 0,
            Basic = 1 << 0,
            Intermediate = 1 << 1,
            Full = 1 << 2,
        }

        
        public VendorApplicationCapabilitiesClass(SupportedAccessLevelEnum SupportedAccessLevels)
        {
            this.SupportedAccessLevels = SupportedAccessLevels;
        }

        /// <summary>
        /// Specifies the supported access levels. This allows the application to show a user interface with
        /// reduced or extended functionality depending on the access levels.The exact meaning or functionalities
        /// definition is left to the vendor.If no access levels are supported this property will be omitted.
        /// Otherwise this will report the supported access levels respectively.
        /// </summary>
        public SupportedAccessLevelEnum SupportedAccessLevels { get; init; }
    }
}
