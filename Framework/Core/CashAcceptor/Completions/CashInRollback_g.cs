/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashInRollback_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.CashInRollback")]
    public sealed class CashInRollbackCompletion : Completion<CashInRollbackCompletion.PayloadData>
    {
        public CashInRollbackCompletion(int RequestId, CashInRollbackCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, Dictionary<string, CashManagement.StorageCashInClass> Storage = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Storage = Storage;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                ShutterNotOpen,
                ExchangeActive,
                NoCashInActive,
                PositionNotEmpty,
                NoItems
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// 
            /// * ```cashUnitError``` - A problem occurred with a storage unit. A
            /// [Storage.StorageErrorEvent](#storage.storageerrorevent) will be sent with the details.
            /// * ```shutterNotOpen``` - The shutter failed to open. In the case of explicit shutter control
            /// the application may have failed to open the shutter before issuing the command.
            /// * ```exchangeActive``` - The device is in an exchange state.
            /// * ```noCashInActive``` - There is no cash-in transaction active.
            /// * ```positionNotEmpty``` - The input or output position is not empty.
            /// * ```noItems``` - There were no items to rollback.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Object containing the storage units which have had items inserted during the associated operation or
            /// transaction. Only storage units whose contents have been modified are included.
            /// </summary>
            [DataMember(Name = "storage")]
            public Dictionary<string, CashManagement.StorageCashInClass> Storage { get; init; }

        }
    }
}
