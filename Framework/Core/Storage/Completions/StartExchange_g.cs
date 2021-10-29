/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * StartExchange_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Storage.Completions
{
    [DataContract]
    [Completion(Name = "Storage.StartExchange")]
    public sealed class StartExchangeCompletion : Completion<StartExchangeCompletion.PayloadData>
    {
        public StartExchangeCompletion(int RequestId, StartExchangeCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                StorageUnitError,
                ExchangeActive,
                TransactionActive
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```storageUnitError``` - An error occurred with a storage unit while performing the exchange 
            /// operation. A [Storage.StorageErrorEvent](#storage.storageerrorevent) will be sent with the 
            /// details.
            /// * ```exchangeActive``` - The device is already in an exchange state.
            /// * ```transactionActive``` - A transaction is active.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
