/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ChipIO_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = ChipIO
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CardReader.ChipIO")]
    public sealed class ChipIOCommand : Command<ChipIOCommand.PayloadData>
    {
        public ChipIOCommand(int RequestId, ChipIOCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ChipProtocolEnum? ChipProtocol = null, List<byte> ChipData = null)
                : base()
            {
                this.ChipProtocol = ChipProtocol;
                this.ChipData = ChipData;
            }

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
            /// Identifies the protocol that is used to communicate with the chip. Possible values are those described
            /// in CardReader [chipProtocols](#common.capabilities.completion.description.cardreader.chipprotocols).
            /// This property is ignored in communications with Memory Cards. The Service knows which memory
            /// card type is currently inserted and therefore there is no need for the application to manage this.
            /// 
            /// It can be one of the following:
            /// 
            /// * ```chipT0``` - Use the T=0 protocol to communicate with the chip.
            /// * ```chipT1``` - Use the T=1 protocol to communicate with the chip.
            /// * ```chipProtocolNotRequired``` - The Service will automatically determine the protocol used
            ///   to communicate with the chip.
            /// * ```chipTypeAPart3``` - Use the ISO 14443 (Part3) Type A contactless chip card protocol to
            ///   communicate with the chip.
            /// * ```chipTypeAPart4``` - Use the ISO 14443 (Part4) Type A contactless chip card protocol to
            ///   communicate with the chip.
            /// * ```chipTypeB``` - Use the ISO 14443 Type B contactless chip card protocol to
            ///   communicate with the chip.
            /// * ```chipTypeNFC``` - Use the ISO 18092 (106/212/424kbps) contactless chip card protocol to
            ///   communicate with the chip.
            /// <example>chipT1</example>
            /// </summary>
            [DataMember(Name = "chipProtocol")]
            public ChipProtocolEnum? ChipProtocol { get; init; }

            /// <summary>
            /// The Base64 encoded data to be sent to the chip.
            /// <example>wCAAAQgwMDAwMDAwMA==</example>
            /// </summary>
            [DataMember(Name = "chipData")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> ChipData { get; init; }

        }
    }
}
