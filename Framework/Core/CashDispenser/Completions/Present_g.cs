/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * Present_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashDispenser.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashDispenser.Present")]
    public sealed class PresentCompletion : Completion<PresentCompletion.PayloadData>
    {
        public PresentCompletion(int RequestId, PresentCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, CashManagement.PositionInfoNullableClass Position = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Position = Position;
            }

            public enum ErrorCodeEnum
            {
                ShutterNotOpen,
                ShutterOpen,
                NoItems,
                ExchangeActive,
                PresentErrorNoItems,
                PresentErrorItems,
                PresentErrorUnknown,
                UnsupportedPosition
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. Following values are possible:
            /// 
            /// * ```shutterNotOpen``` - The shutter did not open when it should have. No items presented.
            /// * ```shutterOpen``` - The shutter is open when it should be closed. No items presented.
            /// * ```noItems``` - There are no items on the stacker.
            /// * ```exchangeActive``` - The device is in an exchange state (see
            /// [Storage.StartExchange](#storage.startexchange)).
            /// * ```presentErrorNoItems``` - There was an error during the present operation - no items were
            /// presented.
            /// * ```presentErrorItems``` - There was an error during the present operation - at least some of the
            /// items were presented.
            /// * ```presentErrorUnknown``` - There was an error during the present operation - the position of the
            /// items is unknown. Intervention may be required to reconcile the cash amount totals.
            /// * ```unsupportedPosition``` - The position specified is not supported.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataMember(Name = "position")]
            public CashManagement.PositionInfoNullableClass Position { get; init; }

        }
    }
}
