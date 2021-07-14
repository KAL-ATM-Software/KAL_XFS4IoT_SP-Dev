/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetDevicelockStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.GetDevicelockStatus")]
    public sealed class GetDevicelockStatusCompletion : Completion<GetDevicelockStatusCompletion.PayloadData>
    {
        public GetDevicelockStatusCompletion(int RequestId, GetDevicelockStatusCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, DeviceLockStatusEnum? DeviceLockStatus = null, List<CashUnitLockClass> CashUnitLock = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.DeviceLockStatus = DeviceLockStatus;
                this.CashUnitLock = CashUnitLock;
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
            /// "lock": The device is physically locked.
            /// 
            /// "unlock": The device is physically unlocked.
            /// 
            /// "lockUnknown": Due to a hardware error or other condition, the physical lock/unlock status of the device cannot be determined.
            /// 
            /// "lockNotSupported": The Service does not support physical lock/unlock control of the device.
            /// </summary>
            [DataMember(Name = "deviceLockStatus")]
            public DeviceLockStatusEnum? DeviceLockStatus { get; init; }

            [DataContract]
            public sealed class CashUnitLockClass
            {
                public CashUnitLockClass(string PhysicalPositionName = null, CashUnitLockStatusEnum? CashUnitLockStatus = null)
                {
                    this.PhysicalPositionName = PhysicalPositionName;
                    this.CashUnitLockStatus = CashUnitLockStatus;
                }

                /// <summary>
                /// A name identifying the physical location of the cash unit within the CashAcceptor.  This name is the same as the "physicalPositionName" in the CashManagement.CashUnitInfo command.
                /// 
                /// </summary>
                [DataMember(Name = "physicalPositionName")]
                public string PhysicalPositionName { get; init; }

                public enum CashUnitLockStatusEnum
                {
                    Lock,
                    Unlock,
                    LockUnknown
                }

                /// <summary>
                /// Specifies the physical lock/unlock status of cash units supported. Following values are possible:
                /// 
                /// "lock": The cash unit is physically locked.
                /// 
                /// "unlock": The cash unit is physically unlocked.
                /// 
                /// "lockunknown": Due to a hardware error or other condition, the physical lock/unlock status of the cash unit cannot be determined.
                /// </summary>
                [DataMember(Name = "cashUnitLockStatus")]
                public CashUnitLockStatusEnum? CashUnitLockStatus { get; init; }

            }

            /// <summary>
            /// Array specifying the physical lock/unlock status of cash units. Cash units that do not support the  physical lock/unlock control are not contained in the array. If there are no cash units that support  physical lock/unlock control this will be empty.
            /// 
            /// </summary>
            [DataMember(Name = "cashUnitLock")]
            public List<CashUnitLockClass> CashUnitLock { get; init; }

        }
    }
}
