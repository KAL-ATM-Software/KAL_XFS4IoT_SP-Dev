/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ParkCard_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.ParkCard")]
    public sealed class ParkCardCompletion : Completion<ParkCardCompletion.PayloadData>
    {
        public ParkCardCompletion(int RequestId, ParkCardCompletion.PayloadData Payload)
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
                MediaJam,
                NoMedia,
                CardPresent,
                PositionInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```mediaJam``` - The card is jammed. Operator intervention is required.
            /// * ```noMedia``` - No card is present at the read/write, chip I/O or transport position and the *in*
            ///   option has been selected. Or no card is in the parking station and the *out* option has been
            ///   selected.
            /// * ```cardPresent``` - Another card is present and is preventing successful movement of the card
            ///   specified by [parkingStation](#cardreader.parkcard.command.properties.parkingstation).
            /// * ```positionInvalid``` - The specified parking station is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
