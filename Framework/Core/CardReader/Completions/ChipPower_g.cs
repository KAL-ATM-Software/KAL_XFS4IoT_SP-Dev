/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ChipPower_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "CardReader.ChipPower")]
    public sealed class ChipPowerCompletion : Completion<ChipPowerCompletion.PayloadData>
    {
        public ChipPowerCompletion()
            : base()
        { }

        public ChipPowerCompletion(int RequestId, ChipPowerCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<byte> ChipData = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.ChipData = ChipData;
            }

            public enum ErrorCodeEnum
            {
                ChipPowerNotSupported,
                MediaJam,
                NoMedia,
                InvalidMedia,
                InvalidData,
                AtrNotObtained
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```chipPowerNotSupported``` - The specified action is not supported by the hardware device.
            /// * ```mediaJam``` - The card is jammed (only applies to contact user chips). Operator intervention is required.
            /// * ```noMedia``` - There is no card inside the device (may not apply for contactless user chips).
            /// * ```invalidMedia``` - No chip found; card may have been inserted or pulled through the wrong way.
            /// * ```invalidData``` - An error occurred while communicating with the chip.
            /// * ```atrNotObtained``` - The ATR has not been obtained (only applies to user chips).
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The Base64 encoded data received from the chip. This property is null if no data received.
            /// <example>O2gAUACFyEARAJAC</example>
            /// </summary>
            [DataMember(Name = "chipData")]
            [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
            public List<byte> ChipData { get; init; }

        }
    }
}
