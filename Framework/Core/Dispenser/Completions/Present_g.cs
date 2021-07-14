/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * Present_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Dispenser.Completions
{
    [DataContract]
    [Completion(Name = "Dispenser.Present")]
    public sealed class PresentCompletion : Completion<PresentCompletion.PayloadData>
    {
        public PresentCompletion(int RequestId, PresentCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, PositionEnum? Position = null, AdditionalBunchesEnum? AdditionalBunches = null, int? BunchesRemaining = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Position = Position;
                this.AdditionalBunches = AdditionalBunches;
                this.BunchesRemaining = BunchesRemaining;
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
            /// * ```exchangeActive``` - The device is in an exchange state (see CashManagement.StartExchange).
            /// * ```presentErrorNoItems``` - There was an error during the present operation - no items were presented.
            /// * ```presentErrorItems``` - There was an error during the present operation - at least some of the items were presented.
            /// * ```presentErrorUnknown``` - There was an error during the present operation - the position of the items is unknown. Intervention may be required to reconcile the cash amount totals.
            /// * ```unsupportedPosition``` - The position specified is not supported.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            public enum PositionEnum
            {
                Default,
                Left,
                Right,
                Center,
                Top,
                Bottom,
                Front,
                Rear
            }

            /// <summary>
            /// Specifies the position where the items have been presented. Following values are possible:
            /// 
            /// * ```left``` - Items presented at the left output position.
            /// * ```right``` - Items presented at the right output position.
            /// * ```center``` - Items presented at the center output position.
            /// * ```top``` - Items presented at the top output position.
            /// * ```bottom``` - Items presented at the bottom output position.
            /// * ```front``` - Items presented at the front output position.
            /// * ```rear``` - Items presented at the rear output position.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; init; }

            public enum AdditionalBunchesEnum
            {
                None,
                OneMore,
                Unknown
            }

            /// <summary>
            /// Specifies whether or not additional bunches of items are remaining to be presented as a result of the current operation. Following values are possible:
            /// 
            /// * ```none``` - No additional bunches remain.
            /// * ```oneMore``` - At least one additional bunch remains.
            /// * ```unknown``` - It is unknown whether additional bunches remain.
            /// </summary>
            [DataMember(Name = "additionalBunches")]
            public AdditionalBunchesEnum? AdditionalBunches { get; init; }

            /// <summary>
            /// If *additionalBunches* is "oneMore", specifies the number of additional bunches of items remaining to be presented as a result of the current operation. 
            /// If the number of additional bunches is at least one, but the precise number is unknown, *bunchesRemaining* will be 255 (TODO: Check if there is a better way to represent this state). 
            /// For any other value of *additionalBunches*, *bunchesRemaining* will be zero.
            /// </summary>
            [DataMember(Name = "bunchesRemaining")]
            public int? BunchesRemaining { get; init; }

        }
    }
}
