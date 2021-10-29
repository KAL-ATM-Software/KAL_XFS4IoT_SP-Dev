/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetDeviceLockStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.GetDeviceLockStatus")]
    public sealed class GetDeviceLockStatusCompletion : Completion<GetDeviceLockStatusCompletion.PayloadData>
    {
        public GetDeviceLockStatusCompletion(int RequestId, GetDeviceLockStatusCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, DeviceLockStatusEnum? DeviceLockStatus = null, List<UnitLockClass> UnitLock = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.DeviceLockStatus = DeviceLockStatus;
                this.UnitLock = UnitLock;
            }

            public enum DeviceLockStatusEnum
            {
                Lock,
                Unlock,
                LockUnknown,
                LockNotSupported
            }

            /// <summary>
            /// Specifies the physical lock/unlock status of the CashAcceptor device. Following values are possible:
            /// 
            /// * ```lock``` - The device is physically locked.
            /// * ```unlock``` - The device is physically unlocked.
            /// * ```lockUnknown``` - Due to a hardware error or other condition, the physical lock/unlock status of the device cannot be determined.
            /// * ```lockNotSupported``` - The Service does not support physical lock/unlock control of the device.
            /// </summary>
            [DataMember(Name = "deviceLockStatus")]
            public DeviceLockStatusEnum? DeviceLockStatus { get; init; }

            [DataContract]
            public sealed class UnitLockClass
            {
                public UnitLockClass(string StorageUnit = null, UnitLockStatusEnum? UnitLockStatus = null)
                {
                    this.StorageUnit = StorageUnit;
                    this.UnitLockStatus = UnitLockStatus;
                }

                /// <summary>
                /// Object name of the storage unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
                /// command) from which items are to be removed.
                /// <example>unit1</example>
                /// </summary>
                [DataMember(Name = "storageUnit")]
                public string StorageUnit { get; init; }

                public enum UnitLockStatusEnum
                {
                    Lock,
                    Unlock,
                    LockUnknown
                }

                /// <summary>
                /// Specifies the physical lock/unlock status of cash units supported. Following values are possible:
                /// 
                /// * ```lock``` - The cash unit is physically locked.
                /// * ```unlock``` - The cash unit is physically unlocked.
                /// * ```lockUnknown``` - Due to a hardware error or other condition, the physical lock/unlock status of the cash unit cannot be determined.
                /// </summary>
                [DataMember(Name = "unitLockStatus")]
                public UnitLockStatusEnum? UnitLockStatus { get; init; }

            }

            /// <summary>
            /// Array specifying the physical lock/unlock status of storage units. Units that do not support the physical
            /// lock/unlock control are not contained in the array. If there are no units that support physical
            /// lock/unlock control this will be empty.
            /// </summary>
            [DataMember(Name = "unitLock")]
            public List<UnitLockClass> UnitLock { get; init; }

        }
    }
}
