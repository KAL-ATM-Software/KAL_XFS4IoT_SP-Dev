/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * DeviceLockControl_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = DeviceLockControl
    [DataContract]
    [Command(Name = "CashAcceptor.DeviceLockControl")]
    public sealed class DeviceLockControlCommand : Command<DeviceLockControlCommand.PayloadData>
    {
        public DeviceLockControlCommand(int RequestId, DeviceLockControlCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, DeviceActionEnum? DeviceAction = null, CashUnitActionEnum? CashUnitAction = null, List<UnitLockControlClass> UnitLockControl = null)
                : base(Timeout)
            {
                this.DeviceAction = DeviceAction;
                this.CashUnitAction = CashUnitAction;
                this.UnitLockControl = UnitLockControl;
            }

            public enum DeviceActionEnum
            {
                Lock,
                Unlock,
                NoLockAction
            }

            /// <summary>
            /// Specifies locking or unlocking the device in its normal operating position. The following values are 
            /// possible:
            /// 
            /// * ```lock``` - Locks the device so that it cannot be removed from its normal operating position.
            /// * ```unlock``` - Unlocks the device so that it can be removed from its normal operating position.
            /// * ```noLockAction``` - No lock/unlock action will be performed on the device.
            /// </summary>
            [DataMember(Name = "deviceAction")]
            public DeviceActionEnum? DeviceAction { get; init; }

            public enum CashUnitActionEnum
            {
                LockAll,
                UnlockAll,
                LockIndividual,
                NoLockAction
            }

            /// <summary>
            /// Specifies the type of lock/unlock action on storage units. The following values are possible:
            /// 
            /// * ```lockAll``` - Locks all storage units supported.
            /// * ```unlockAll``` - Unlocks all storage units supported.
            /// * ```lockIndividual``` - Locks/unlocks storage units individually as specified in the *unitLockControl* parameter.
            /// * ```noLockAction``` - No lock/unlock action will be performed on storage units.
            /// </summary>
            [DataMember(Name = "cashUnitAction")]
            public CashUnitActionEnum? CashUnitAction { get; init; }

            [DataContract]
            public sealed class UnitLockControlClass
            {
                public UnitLockControlClass(string StorageUnit = null, UnitActionEnum? UnitAction = null)
                {
                    this.StorageUnit = StorageUnit;
                    this.UnitAction = UnitAction;
                }

                /// <summary>
                /// Name of the storage unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
                /// command) to be locked or unlocked.
                /// <example>unit1</example>
                /// </summary>
                [DataMember(Name = "storageUnit")]
                [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
                public string StorageUnit { get; init; }

                public enum UnitActionEnum
                {
                    Lock,
                    Unlock
                }

                /// <summary>
                /// Specifies whether to lock or unlock the storage unit indicated in the *storageUnit* parameter.
                /// The following values are possible:
                /// 
                /// * ```lock``` - Locks the specified storage unit so that it cannot be removed from the device. 
                /// * ```unlock``` - Unlocks the specified storage unit so that it can be removed from the device.
                /// </summary>
                [DataMember(Name = "unitAction")]
                public UnitActionEnum? UnitAction { get; init; }

            }

            /// <summary>
            /// Array of structures, one for each storage unit to be locked or unlocked. Only valid in the case where 
            /// *lockIndividual* is specified in the *cashUnitAction* property otherwise this will be ignored.
            /// </summary>
            [DataMember(Name = "unitLockControl")]
            public List<UnitLockControlClass> UnitLockControl { get; init; }

        }
    }
}
