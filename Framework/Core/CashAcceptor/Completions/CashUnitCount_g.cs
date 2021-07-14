/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "CashAcceptor.CashUnitCount")]
    public sealed class CashUnitCountCompletion : Completion<CashUnitCountCompletion.PayloadData>
    {
        public CashUnitCountCompletion(int RequestId, CashUnitCountCompletion.PayloadData Payload)
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
                InvalidCashUnit,
                CashInActive,
                ExchangeActive,
                TooManyItemsToCount,
                CountPositionNotEmpty,
                CashUnitError
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// "invalidCashUnit": At least one of the cash units specified is either invalid or does not support being counted. No cash units have been counted.
            /// 
            /// "cashInActive": A cash-in transaction is active.
            /// 
            /// "exchangeActive": The device is in the exchange state.
            /// 
            /// "tooManyItemsToCount": There were too many items. The required internal position may have been of insufficient size. 
            /// All items should be returned to the cash unit from which they originated.
            /// 
            /// "countPositionNotEmpty": A required internal position is not empty so a cash unit count is not possible.
            /// 
            /// "cashUnitError": A cash unit caused a problem. A CashManagement.CashUnitErrorEvent will be posted with the details.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
