/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Completion(Name = "CashDispenser.Present")]
    public sealed class PresentCompletion : Completion<PresentCompletion.PayloadData>
    {
        public PresentCompletion(int RequestId, PresentCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, CashManagement.PositionEnum? Position = null, string AdditionalBunches = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Position = Position;
                this.AdditionalBunches = AdditionalBunches;
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
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```shutterNotOpen``` - The shutter did not open when it should have. No items presented.
            /// * ```shutterOpen``` - The shutter is open when it should be closed. No items presented.
            /// * ```noItems``` - There are no items on the stacker.
            /// * ```exchangeActive``` - The device is in an exchange state (see 
            /// [CashManagement.StartExchange](#cashmanagement.startexchange)).
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
            public CashManagement.PositionEnum? Position { get; init; }

            /// <summary>
            /// Specifies how many more bunches will be required to present the request. Following values are possible:
            /// 
            ///   * ```&lt;number&gt;``` - The number of additional bunches to be presented.
            ///   * ```unknown``` - More than one additional bunch is required but the precise number is unknown.
            /// <example>1</example>
            /// </summary>
            [DataMember(Name = "additionalBunches")]
            [DataTypes(Pattern = @"^unknown$|^[0-9]*$")]
            public string AdditionalBunches { get; init; }

        }
    }
}
