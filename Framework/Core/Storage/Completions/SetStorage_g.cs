/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * SetStorage_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Storage.Completions
{
    [DataContract]
    [Completion(Name = "Storage.SetStorage")]
    public sealed class SetStorageCompletion : Completion<SetStorageCompletion.PayloadData>
    {
        public SetStorageCompletion(int RequestId, SetStorageCompletion.PayloadData Payload)
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
                InvalidUnit,
                NoExchangeActive,
                StorageUnitError
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```invalidUnit``` - Invalid unit.
            /// * ```noExchangeActive``` - The device is not in an exchange state and a request has been made to 
            /// modify information which can only be modified in an exchange state.
            /// * ```storageUnitError``` - A problem occurred with a storage unit. A 
            /// [Storage.StorageErrorEvent](#storage.storageerrorevent) will be posted with the details.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
