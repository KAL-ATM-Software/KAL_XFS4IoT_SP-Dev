/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * Move_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = Move
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CardReader.Move")]
    public sealed class MoveCommand : Command<MoveCommand.PayloadData>
    {
        public MoveCommand(int RequestId, MoveCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string From = null, string To = null)
                : base()
            {
                this.From = From;
                this.To = To;
            }

            /// <summary>
            /// Specifies where the card should be moved from as one of the following:
            /// 
            /// * ```exit``` - The card will be moved from the exit position.
            /// * ```transport``` - The card will be moved from the transport position. This is the only value
            ///   applicable to latched dip card readers.
            /// * ```[storage unit identifier]``` - The card will be moved from the storage unit with matching
            ///   [identifier](#storage.getstorage.completion.properties.storage.unit1). The storage unit type must be
            ///   either *dispense* or *park*.
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "from")]
            [DataTypes(Pattern = @"^exit$|^transport$|^unit[0-9A-Za-z]+$")]
            public string From { get; init; }

            /// <summary>
            /// Specifies where the card should be moved to as one of the following:
            /// 
            /// * ```exit``` - The card will be moved to the exit. This is the only value applicable to latched dip
            ///   card readers.
            /// * ```transport``` - The card will be moved to the transport just behind the exit slot.
            /// * ```[storage unit identifier]``` - The card will be moved to the storage unit with matching
            ///   [identifier](#storage.getstorage.completion.properties.storage.unit1). The storage unit type must be
            ///   either *retain* or *park*.
            /// <example>exit</example>
            /// </summary>
            [DataMember(Name = "to")]
            [DataTypes(Pattern = @"^exit$|^transport$|^unit[0-9A-Za-z]+$")]
            public string To { get; init; }

        }
    }
}
