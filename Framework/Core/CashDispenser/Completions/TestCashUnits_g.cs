/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * TestCashUnits_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashDispenser.Completions
{
    [DataContract]
    [Completion(Name = "CashDispenser.TestCashUnits")]
    public sealed class TestCashUnitsCompletion : Completion<TestCashUnitsCompletion.PayloadData>
    {
        public TestCashUnitsCompletion(int RequestId, TestCashUnitsCompletion.PayloadData Payload)
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
                CashUnitError,
                UnsupportedPosition,
                ShutterNotOpen,
                ShutterOpen,
                InvalidCashUnit,
                ExchangeActive,
                PresentErrorNoItems,
                PresentErrorItems,
                PresentErrorUnknown
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```cashUnitError``` - A cash unit caused a problem that meant all cash units could not be tested or 
            /// no cash units were testable. One or more [Storage.StorageErrorEvent](#storage.storageerrorevent) 
            /// events will be posted with the details.
            /// * ```unsupportedPosition``` - The position specified is not supported.
            /// * ```shutterNotOpen``` - The shutter is not open or did not open when it should have. No items presented.
            /// * ```shutterOpen``` - The shutter is open when it should be closed. No items presented.
            /// * ```invalidCashUnit``` - The cash unit number specified is not valid.
            /// * ```exchangeActive``` - The device is in an exchange state (see
            /// [CashManagement.StartExchange](#cashmanagement.startexchange)).
            /// * ```presentErrorNoItems``` - There was an error during the present operation - no items were presented.
            /// * ```presentErrorItems``` - There was an error during the present operation - at least some of the items 
            /// were presented.
            /// * ```presentErrorUnknown``` - There was an error during the present operation - the position of the items 
            /// is unknown. Intervention may be required to reconcile the cash amount totals.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
