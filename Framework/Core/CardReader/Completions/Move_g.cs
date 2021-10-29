/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * Move_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.Move")]
    public sealed class MoveCompletion : Completion<MoveCompletion.PayloadData>
    {
        public MoveCompletion(int RequestId, MoveCompletion.PayloadData Payload)
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
                ShutterFail,
                NoMedia,
                Occupied,
                MediaRetained
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```mediaJam``` - The card is jammed. Operator intervention is required.
            /// * ```shutterFail``` - The open of the shutter failed due to manipulation or hardware error. Operator
            ///   intervention is required.
            /// * ```noMedia``` - No card is in the requested [*from*](#cardreader.move.command.properties.from)
            ///   position.
            /// * ```occupied``` - A card already occupies the requested
            ///   [*to*](#cardreader.move.command.properties.to) position. 
            /// * ```mediaRetained``` - The card has been retained during attempts to move it to the exit position. 
            ///   The device is clear and can be used.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}
