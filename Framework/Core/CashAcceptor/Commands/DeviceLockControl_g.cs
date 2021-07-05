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

            public PayloadData(int Timeout, DeviceActionEnum? DeviceAction = null, int? CashUnitAction = null, List<UnitLockControlClass> UnitLockControl = null)
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
            /// \"lock\": Locks the device so that it cannot be removed from its normal operating position.
            /// 
            /// \"unlock\": Unlocks the device so that it can be removed from its normal operating position.
            /// 
            /// \"noLockAction\": No lock/unlock action will be performed on the device.
            /// </summary>
            [DataMember(Name = "deviceAction")]
            public DeviceActionEnum? DeviceAction { get; private set; }

            /// <summary>
            /// Specifies the type of lock/unlock action on cash units. Following values are possible:
            /// 
            /// \"lockAll\": Locks all cash units supported.
            /// 
            /// \"unlockAll\": Unlocks all cash units supported.
            /// 
            /// \"lockIndividual\": Locks/unlocks cash units individually as specified in the *unitLockControl* parameter.
            /// 
            /// \"noLockAction\": 
            /// </summary>
            [DataMember(Name = "cashUnitAction")]
            public int? CashUnitAction { get; private set; }

            [DataContract]
            public sealed class UnitLockControlClass
            {
                public UnitLockControlClass(string PhysicalPositionName = null, UnitActionEnum? UnitAction = null)
                {
                    this.PhysicalPositionName = PhysicalPositionName;
                    this.UnitAction = UnitAction;
                }

                /// <summary>
                /// Specifies which cash unit is to be locked/unlocked. This name is the same as  the *physicalPositionName* in the CashUnitInfo structure. Only cash units reported by the  CashAcceptor.DeviceLockStatus command can be specified.
                /// 
                /// </summary>
                [DataMember(Name = "physicalPositionName")]
                public string PhysicalPositionName { get; private set; }

                public enum UnitActionEnum
                {
                    Lock,
                    Unlock
                }

                /// <summary>
                /// Specifies whether to lock or unlock the cash unit indicated in the *physicalPositionName* parameter. Following values are possible:
                /// \"lock\": Locks the specified cash unit so that it cannot be removed from the device.
                /// \"unlock\": Unlocks the specified cash unit so that it can be removed from the device.
                /// 
                /// </summary>
                [DataMember(Name = "unitAction")]
                public UnitActionEnum? UnitAction { get; private set; }

            }

            /// <summary>
            /// Array of UnitLockControl structures; only valid in the case where \"lockIndividual\" is specified in the *cashUnitAction* field. 
            /// Otherwise this field will be ignored. Each element specifies one cash unit to be locked/unlocked.
            /// </summary>
            [DataMember(Name = "unitLockControl")]
            public List<UnitLockControlClass> UnitLockControl { get; private set; }

        }
    }
}
