/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ChipIO_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "CardReader.ChipIO")]
    public sealed class ChipIOCompletion : Completion<ChipIOCompletion.PayloadData>
    {
        public ChipIOCompletion(int RequestId, ChipIOCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, ChipProtocolEnum? ChipProtocol = null, List<byte> ChipData = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.ChipProtocol = ChipProtocol;
                this.ChipData = ChipData;
            }

            public enum ErrorCodeEnum
            {
                MediaJam,
                NoMedia,
                InvalidMedia,
                InvalidData,
                ProtocolNotSupported,
                AtrNotObtained,
                CardCollision
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```mediaJam``` - The card is jammed. Operator intervention is required.
            /// * ```noMedia``` - There is no card inside the device.
            /// * ```invalidMedia``` - No chip found; card may have been inserted the wrong way.
            /// * ```invalidData``` - An error occurred while communicating with the chip.
            /// * ```protocolNotSupported``` - The protocol used was not supported by the Service.
            /// * ```atrNotObtained``` - The ATR has not been obtained.
            /// * ```cardCollision``` - There was an unresolved collision of two or more contactless card signals.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            public enum ChipProtocolEnum
            {
                ChipT0,
                ChipT1,
                ChipProtocolNotRequired,
                ChipTypeAPart3,
                ChipTypeAPart4,
                ChipTypeB,
                ChipTypeNFC
            }

            /// <summary>
            /// Identifies the protocol that is used to communicate with the chip. This contains the same value
            /// as the corresponding property in the payload. This property is null for Memory Card dialogs.
            /// 
            /// It can be one of the following:
            /// 
            /// * ```chipT0``` - The T=0 protocol has been used to communicate with the chip.
            /// * ```chipT1``` - The T=1 protocol has been used to communicate with the chip.
            /// * ```chipProtocolNotRequired``` - The Service has automatically determined the protocol used
            ///   to communicate with the chip.
            /// * ```chipTypeAPart3``` - The ISO 14443 (Part3) Type A contactless chip card protocol has been used to
            ///   communicate with the chip.
            /// * ```chipTypeAPart4``` - The ISO 14443 (Part4) Type A contactless chip card protocol has been used to
            ///   communicate with the chip.
            /// * ```chipTypeB``` - The ISO 14443 Type B contactless chip card protocol has been used to communicate
            ///   with the chip.
            /// * ```chipTypeNFC``` - The ISO 18092 (106/212/424kbps) contactless chip card protocol has been used to
            ///   communicate with the chip.
            /// </summary>
            [DataMember(Name = "chipProtocol")]
            public ChipProtocolEnum? ChipProtocol { get; init; }

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
