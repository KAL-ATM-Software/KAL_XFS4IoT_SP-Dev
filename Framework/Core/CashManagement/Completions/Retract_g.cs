/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * Retract_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [Completion(Name = "CashManagement.Retract")]
    public sealed class RetractCompletion : Completion<RetractCompletion.PayloadData>
    {
        public RetractCompletion(int RequestId, RetractCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, Dictionary<string, StorageCashInClass> Storage = null)
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
                ShutterNotClosed,
                ItemsTaken,
                InvalidRetractPosition,
                NotRetractArea,
                ForeignItemsDetected
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```cashUnitError``` - A problem occurred with a cash unit. A 
            /// [Storage.StorageErrorEvent](#storage.storageerrorevent) will be sent with the details.
            /// * ```noItems``` - There were no items to retract.
            /// * ```exchangeActive``` - The device is in an exchange state.
            /// * ```shutterNotClosed``` - The shutter failed to close.
            /// * ```itemsTaken``` - Items were present at the output position at the start of the operation, but were 
            /// removed before the operation was complete - some or all of the items were not retracted.
            /// * ```invalidRetractPosition``` - The *index* is not supported.
            /// * ```notRetractArea``` - The retract area specified in *retractArea* is not supported.
            /// * ```foreignItemsDetected``` - Foreign items have been detected inside the input position.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// List of storage units that have taken items and the type of items they have taken during the current 
            /// command.
            /// </summary>
            [DataMember(Name = "storage")]
            public Dictionary<string, StorageCashInClass> Storage { get; init; }

        }
    }
}
