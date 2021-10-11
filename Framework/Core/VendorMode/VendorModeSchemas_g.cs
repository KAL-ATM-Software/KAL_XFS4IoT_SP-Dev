/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * VendorModeSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.VendorMode
{

    [DataContract]
    public sealed class StatusClass
    {
        public StatusClass(DeviceEnum? Device = null, ServiceEnum? Service = null)
        {
            this.Device = Device;
            this.Service = Service;
        }

        public enum DeviceEnum
        {
            Online,
            Offline
        }

        /// <summary>
        /// Specifies the status of the Vendor Mode Service Provider. Status will be one of the following
        /// values:
        /// 
        /// * ```online``` - The Vendor Mode service is available.
        /// * ```offline``` - The Vendor Mode service is not available.
        /// </summary>
        [DataMember(Name = "device")]
        public DeviceEnum? Device { get; init; }

        public enum ServiceEnum
        {
            EnterPending,
            Active,
            ExitPending,
            Inactive
        }

        /// <summary>
        /// 
        /// Specifies the service state as one of the following values:
        /// * ```enterPending``` - Vendor Mode enter request pending.
        /// * ```active``` - Vendor Mode active.
        /// * ```exitPending``` - Vendor Mode exit request pending.
        /// * ```inactive``` - Vendor Mode inactive.
        /// </summary>
        [DataMember(Name = "service")]
        public ServiceEnum? Service { get; init; }

    }


}
