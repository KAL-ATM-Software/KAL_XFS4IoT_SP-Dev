/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = Reset
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CardReader.Reset")]
    public sealed class ResetCommand : Command<ResetCommand.PayloadData>
    {
        public ResetCommand(int RequestId, ResetCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ToEnum? To = null, string StorageId = null)
                : base()
            {
                this.To = To;
                this.StorageId = StorageId;
            }

            public enum ToEnum
            {
                Exit,
                Retain,
                CurrentPosition,
                Auto
            }

            /// <summary>
            /// Specifies the position a card in the transport or exit position should be moved to as one of the
            /// following:
            /// 
            /// * ```exit``` - Move the card to the exit position. If the card is already at the exit, it may be moved
            ///   to ensure it is in the correct position to be taken.
            /// * ```retain``` - Move the card to a retain storage unit.
            /// * ```currentPosition``` - Keep the card in its current position. If the card is in the transport, it
            ///   may be moved in the transport to verify it is not jammed.
            /// * ```auto``` - The service will select the position to which the card will be moved based on device
            /// capabilities, retain storage units available and service specific configuration.
            /// <example>retain</example>
            /// </summary>
            [DataMember(Name = "to")]
            public ToEnum? To { get; init; }

            /// <summary>
            /// If the card is to be moved to a *retain* storage unit, this indicates the retain storage unit to which
            /// the card should be moved.
            /// 
            /// If null, the Service will select the retain storage unit based on the number of retain storage
            /// units available and service specific configuration.
            /// <example>unit4</example>
            /// </summary>
            [DataMember(Name = "storageId")]
            [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
            public string StorageId { get; init; }

        }
    }
}
