/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * DeviceLockControl_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.DeviceLockControl")]
    public sealed class DeviceLockControlCompletion : Completion<DeviceLockControlCompletion.PayloadData>
    {
        public DeviceLockControlCompletion(int RequestId, DeviceLockControlCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                InvalidCashUnit,
                CashInActive,
                ExchangeActive,
                DeviceLockFailure
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```invalidCashUnit``` - The storage unit type specified is invalid.
            /// * ```cashInActive``` - A cash-in transaction is active.
            /// * ```exchangeActive``` - The device is in the exchange state.
            /// * ```deviceLockFailure``` - The device and/or the storage units specified could not be locked/unlocked,
            ///   e.g., the lock action could not be performed because the storage unit specified to be locked had been
            ///   removed.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
