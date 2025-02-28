/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashInEnd_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.CashInEnd")]
    public sealed class CashInEndCompletion : Completion<CashInEndCompletion.PayloadData>
    {
        public CashInEndCompletion(int RequestId, CashInEndCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, CashManagement.StorageCashInClass> Storage = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Storage = Storage;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                NoItems,
                ExchangeActive,
                NoCashInActive,
                PositionNotEmpty,
                SafeDoorOpen
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```cashUnitError``` - A problem occurred with a storage unit. A
            ///   [Storage.StorageErrorEvent](#storage.storageerrorevent) will be sent with the details.
            /// * ```noItems``` - There were no items to cash-in.
            /// * ```exchangeActive``` - The device is in an exchange state.
            /// * ```noCashInActive``` - There is no cash-in transaction active.
            /// * ```positionNotEmpty``` - The input or output position is not empty.
            /// * ```safeDoorOpen``` - The safe door is open. This device requires the safe door to be closed in order
            ///   to perform this command (see [Common.Status](#common.status.completion.properties.auxiliaries.safedoor)
            ///   property).
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
