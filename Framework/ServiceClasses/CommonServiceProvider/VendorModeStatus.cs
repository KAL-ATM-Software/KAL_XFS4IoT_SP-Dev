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

namespace XFS4IoTFramework.Common
{
    /// <summary>
    /// VendorModeStatusClass
    /// Store device status for the vendor mode
    /// </summary>
    public sealed class VendorModeStatusClass
    {
        public enum DeviceStatusEnum
        {
            Online,  //The Vendor Mode service is available.
            Offline, //The Vendor Mode service is not available.
        }

        public enum ServiceStatusEnum
        {
            EnterPending, //Vendor Mode enter request pending.
            Active,       //Vendor Mode active.
            ExitPending,  //Vendor Mode exit request pending.
            Inactive,     //Vendor Mode inactive.
        }


        public VendorModeStatusClass(DeviceStatusEnum DeviceStatus,
                                     ServiceStatusEnum ServiceStatus)
        {
            this.DeviceStatus = DeviceStatus;
            this.ServiceStatus = ServiceStatus;
        }

        /// <summary>
        /// Specifies the status of the Vendor Mode Service.
        /// </summary>
        public DeviceStatusEnum DeviceStatus { get; set; }


        /// <summary>
        /// Specifies the service state
        /// </summary>
        public ServiceStatusEnum ServiceStatus { get; set; }

    }
}