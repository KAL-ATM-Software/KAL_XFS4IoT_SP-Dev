/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "CardReader.ChipIO")]
    public sealed class ChipIOCommand : Command<ChipIOCommand.PayloadData>
    {
        public ChipIOCommand(int RequestId, ChipIOCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string ChipProtocol = null, string ChipData = null)
                : base(Timeout)
            {
                this.ChipProtocol = ChipProtocol;
                this.ChipData = ChipData;
            }

            /// <summary>
            /// Identifies the protocol that is used to communicate with the chip. Possible values are those described
            /// in CardReader.Capabilities. This field is ignored in communications with Memory Cards. The Service
            /// Provider knows which memory card type is currently inserted and therefore there is no need for the
            /// application to manage this.
            /// </summary>
            [DataMember(Name = "chipProtocol")]
            public string ChipProtocol { get; init; }

            /// <summary>
            /// The Base64 encoded data to be sent to the chip.
            /// </summary>
            [DataMember(Name = "chipData")]
            public string ChipData { get; init; }

        }
    }
}
