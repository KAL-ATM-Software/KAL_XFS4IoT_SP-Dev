/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * PresentIDC_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.PinPad.Commands
{
    //Original name = PresentIDC
    [DataContract]
    [Command(Name = "PinPad.PresentIDC")]
    public sealed class PresentIDCCommand : Command<PresentIDCCommand.PayloadData>
    {
        public PresentIDCCommand(int RequestId, PresentIDCCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, PresentAlgorithmEnum? PresentAlgorithm = null, string ChipProtocol = null, string ChipData = null, AlgorithmDataClass AlgorithmData = null)
                : base(Timeout)
            {
                this.PresentAlgorithm = PresentAlgorithm;
                this.ChipProtocol = ChipProtocol;
                this.ChipData = ChipData;
                this.AlgorithmData = AlgorithmData;
            }

            public enum PresentAlgorithmEnum
            {
                PresentClear
            }

            /// <summary>
            /// Specifies the algorithm that is used for presentation.
            /// Possible values are: (see [presentationAlgorithms](#common.capabilities.completion.properties.pinpad.presentationAlgorithms)).
            /// </summary>
            [DataMember(Name = "presentAlgorithm")]
            public PresentAlgorithmEnum? PresentAlgorithm { get; init; }

            /// <summary>
            /// Identifies the protocol that is used to communicate with the chip. Possible values are: 
            /// (see command [chipProtocols](#common.capabilities.completion.properties.cardreader.chipProtocols) in the Identification Card Device Class Interface)
            /// </summary>
            [DataMember(Name = "chipProtocol")]
            public string ChipProtocol { get; init; }

            /// <summary>
            /// Points to the Base64 encoded data to be sent to the chip.
            /// </summary>
            [DataMember(Name = "chipData")]
            public string ChipData { get; init; }

            [DataContract]
            public sealed class AlgorithmDataClass
            {
                public AlgorithmDataClass(int? PinPointer = null, int? PinOffset = null)
                {
                    this.PinPointer = PinPointer;
                    this.PinOffset = PinOffset;
                }

                /// <summary>
                /// The byte offset where to start inserting the PIN into chipData. 
                /// The leftmost byte is numbered zero. See below for an example
                /// </summary>
                [DataMember(Name = "pinPointer")]
                public int? PinPointer { get; init; }

                /// <summary>
                /// The bit offset within the byte specified by *pinPointer* property where to start inserting the PIN. 
                /// The leftmost bit numbered zero.
                /// </summary>
                [DataMember(Name = "pinOffset")]
                public int? PinOffset { get; init; }

            }

            /// <summary>
            /// Contains the data required for the specified presentation algorithm.
            /// </summary>
            [DataMember(Name = "algorithmData")]
            public AlgorithmDataClass AlgorithmData { get; init; }

        }
    }
}
