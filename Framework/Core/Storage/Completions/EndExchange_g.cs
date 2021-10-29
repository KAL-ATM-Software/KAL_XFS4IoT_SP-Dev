/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * EndExchange_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Storage.Completions
{
    [DataContract]
    [Completion(Name = "Storage.EndExchange")]
    public sealed class EndExchangeCompletion : Completion<EndExchangeCompletion.PayloadData>
    {
        public EndExchangeCompletion(int RequestId, EndExchangeCompletion.PayloadData Payload)
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
                NoExchangeActive
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```storageUnitError``` - A storage unit problem occurred that meant no storage units could be 
            /// updated. One or more [Storage.StorageErrorEvent](#storage.storageerrorevent) events will be sent with
            /// the details.
            /// * ```noExchangeActive``` - There is no exchange active.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
