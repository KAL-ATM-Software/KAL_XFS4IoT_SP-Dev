/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT DK interface.
 * DKSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.DK
{

    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(string HsmVendor = null, bool? HsmJournaling = null)
        {
            this.HsmVendor = HsmVendor;
            this.HsmJournaling = HsmJournaling;
        }

        /// <summary>
        /// Identifies the hsm Vendor. hsmVendor is an empty string or this property is omitted when the 
        /// hsm Vendor is unknown or the HSM is not supported.
        /// </summary>
        [DataMember(Name = "hsmVendor")]
        public string HsmVendor { get; init; }

        /// <summary>
        /// Specifies whether the hsm supports journaling by the [DK.GetJournal](#dk.getjournal) command.
        /// The value of this parameter is either TRUE or FALSE. TRUE means the hsm supports journaling by 
        /// [DK.GetJournal](#dk.getjournal).
        /// </summary>
        [DataMember(Name = "hsmJournaling")]
        public bool? HsmJournaling { get; init; }

    }


}
