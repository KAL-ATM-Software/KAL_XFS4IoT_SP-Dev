/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "CashAcceptor.CashInEnd")]
    public sealed class CashInEndCompletion : Completion<CashInEndCompletion.PayloadData>
    {
        public CashInEndCompletion(int RequestId, CashInEndCompletion.PayloadData Payload)
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
                NoItems,
                ExchangeActive,
                NoCashInActive,
                PositionNotEmpty,
                SafeDoorOpen
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```cashUnitError``` - A problem occurred with a cash unit. A 
            /// [Storage.StorageErrorEvent](#storage.storageerrorevent) will be sent with the details.
            /// * ```noItems``` - There were no items to cash-in.
            /// * ```exchangeActive``` - The device is in an exchange state.
            /// * ```noCashInActive``` - There is no cash-in transaction active.
            /// * ```positionNotEmpty``` - The input or output position is not empty.
            /// * ```safeDoorOpen``` - The safe door is open. This device requires the safe door to be closed in order
            /// to perform this command.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// List of storage units that have taken items and the type of items they have taken during the current 
            /// transaction. This only contains data related to the current transaction.
            /// </summary>
            [DataMember(Name = "storage")]
            public Dictionary<string, CashManagement.StorageCashInClass> Storage { get; init; }

        }
    }
}
