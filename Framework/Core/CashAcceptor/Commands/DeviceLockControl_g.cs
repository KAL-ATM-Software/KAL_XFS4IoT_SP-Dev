/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
            /// Specifies to lock or unlock the device in its normal operating position. Following values are possible:
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
            /// Specifies the type of lock/unlock action on cash units. Following values are possible:
            /// 
            /// * ```lockAll``` - Locks all cash units supported.
            /// * ```unlockAll``` - Unlocks all cash units supported.
            /// * ```lockIndividual``` - Locks/unlocks cash units individually as specified in the _unitLockControl_ parameter.
            /// * ```noLockAction``` - No lock/unlock action will be performed on cash units.
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
                /// Object name of the storage unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
                /// command) from which items are to be removed.
                /// <example>unit1</example>
                /// </summary>
                [DataMember(Name = "storageUnit")]
                public string StorageUnit { get; init; }

                public enum UnitActionEnum
                {
                    Lock,
                    Unlock
                }

                /// <summary>
                /// Specifies whether to lock or unlock the cash unit indicated in the *physicalPositionName* parameter.
                /// Following values are possible:
                /// 
                /// * ```lock``` - Locks the specified cash unit so that it cannot be removed from the device. 
                /// * ```unlock``` - Unlocks the specified cash unit so that it can be removed from the device.
                /// </summary>
                [DataMember(Name = "unitAction")]
                public UnitActionEnum? UnitAction { get; init; }

            }

            /// <summary>
            /// Array of UnitLockControl structures; only valid in the case where ```lockIndividual``` is specified in the
            /// _cashUnitAction_ field otherwise this field will be ignored. Each element specifies one cash unit to be 
            /// locked/unlocked.
            /// </summary>
            [DataMember(Name = "unitLockControl")]
            public List<UnitLockControlClass> UnitLockControl { get; init; }

        }
    }
}
