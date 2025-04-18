/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashUnitCount_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.CashUnitCount")]
    public sealed class CashUnitCountCompletion : Completion<CashUnitCountCompletion.PayloadData>
    {
        public CashUnitCountCompletion(int RequestId, CashUnitCountCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                TooManyItemsToCount,
                CountPositionNotEmpty,
                CashUnitError
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```invalidCashUnit``` - At least one of the storage units specified is either invalid or does not
            ///   support being counted. No storage units have been counted.
            /// * ```cashInActive``` - A cash-in transaction is active.
            /// * ```exchangeActive``` - The device is in the exchange state.
            /// * ```tooManyItemsToCount``` - There were too many items. The required internal position may have been
            ///   of insufficient size.
            ///   All items should be returned to the storage unit from which they originated.
            /// * ```countPositionNotEmpty``` - A required internal position is not empty so a storage unit count is
            ///   not possible.
            /// * ```cashUnitError``` - A storage unit caused a problem. A
            ///   [Storage.StorageErrorEvent](#storage.storageerrorevent) will be posted with the details.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
