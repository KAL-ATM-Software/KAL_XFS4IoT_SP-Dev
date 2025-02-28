/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT GermanSpecific interface.
 * GermanSpecificSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.GermanSpecific
{

    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(string HsmVendor = null)
        {
            this.HsmVendor = HsmVendor;
        }

        /// <summary>
        /// Identifies the HSM Vendor. This property is null when the 
        /// HSM Vendor is unknown or the HSM is not supported.
        /// <example>HSM Vendor</example>
        /// </summary>
        [DataMember(Name = "hsmVendor")]
        public string HsmVendor { get; init; }

    }


}
