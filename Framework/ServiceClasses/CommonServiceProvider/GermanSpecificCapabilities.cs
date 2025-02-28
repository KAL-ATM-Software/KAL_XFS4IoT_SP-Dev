/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// GermanCapabilitiesClass
    /// The German interface is regional specific requirements for DK certification.
    /// </summary>
    public sealed class GermanSpecificCapabilitiesClass(
        string HSMVendor = null)
    {
        /// <summary>
        /// Specifies whether the HSM Vendor.
        /// </summary>
        public string HSMVendor { get; init; } = HSMVendor;
    }
}
