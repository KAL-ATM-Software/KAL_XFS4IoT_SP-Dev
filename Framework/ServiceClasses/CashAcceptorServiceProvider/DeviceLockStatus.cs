/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashAcceptor
{
    public sealed class DeviceLockStatusClass
    {
        /// <summary>
        /// Specifies the physical lock/unlock status of the CashAcceptor device. The following values are possible:
        /// 
        /// * ```Lock``` - The device is physically locked.
        /// * ```Unlock``` - The device is physically unlocked.
        /// * ```LockUnknown``` - Due to a hardware error or other condition, the physical lock/unlock status of the 
        /// device cannot be determined.
        /// * ```LockNotSupported``` - The Service does not support reporting the physical lock/unlock status of the 
        /// device.
        /// </summary>
        public enum DeviceLockStatusEnum
        {
            Lock,
            Unlock,
            LockUnknown,
            LockNotSupported,
        }

        /// <summary>
        /// Specifies the physical lock/unlock status of storage units supported. The following values are possible:
        /// 
        /// * ```Lock``` - The storage unit is physically locked.
        /// * ```Unlock``` - The storage unit is physically unlocked.
        /// * ```LockUnknown``` - Due to a hardware error or other condition, the physical lock/unlock status of the 
        /// storage unit cannot be determined.
        /// </summary>
        public enum UnitLockStatusEnum
        {
            Lock,
            Unlock,
            LockUnknown,
        }

        public DeviceLockStatusClass(DeviceLockStatusEnum DeviceLockStatus,
                                     Dictionary<string, UnitLockStatusEnum> UnitLock)
        {
            this.LockStatus = DeviceLockStatus;
            this.UnitLock = UnitLock;
        }

        public DeviceLockStatusClass()
        {
            LockStatus = DeviceLockStatusEnum.LockNotSupported;
            UnitLock = null;
        }

        public DeviceLockStatusEnum LockStatus { get; set; }

        /// <summary>
        /// Specifying the physical lock/unlock status of storage units. Units that do not support the physical
        /// lock/unlock control are not contained in the dictionary. If there are no units that support physical
        /// lock/unlock control this will be empty.
        /// </summary>
        public Dictionary<string, UnitLockStatusEnum> UnitLock { get; set; }
    }
}
