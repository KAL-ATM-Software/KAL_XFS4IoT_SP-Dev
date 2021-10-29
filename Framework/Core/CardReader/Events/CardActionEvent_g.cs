/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * CardActionEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CardReader.Events
{

    [DataContract]
    [Event(Name = "CardReader.CardActionEvent")]
    public sealed class CardActionEvent : UnsolicitedEvent<CardActionEvent.PayloadData>
    {

        public CardActionEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string To = null, string From = null)
                : base()
            {
                this.To = To;
                this.From = From;
            }

            /// <summary>
            /// Position where the card was moved to. Possible values are:
            /// 
            /// * ```exit``` - The card was moved to the exit position.
            /// * ```transport``` - The card was moved to the transport position. 
            /// * ```&lt;storage unit identifier&gt;``` - The card was moved to the storage unit with matching
            ///   [identifier](#storage.getstorage.completion.properties.storage.unit1). The storage unit type must be
            ///   *retain*.
            /// <example>retn1</example>
            /// </summary>
            [DataMember(Name = "to")]
            [DataTypes(Pattern = @"^exit$|^transport$|^.{1,5}$")]
            public string To { get; init; }

            /// <summary>
            /// Position where the card was moved from. Possible values are:
            /// 
            /// * ```unknown``` - The position of the card cannot be determined.
            /// * ```exit``` - The card was in the exit position.
            /// * ```transport``` - The card was moved in the transport position. 
            /// * ```&lt;storage unit identifier&gt;``` - The card was in a storage unit with matching
            ///   [identifier](#storage.getstorage.completion.properties.storage.unit1). The storage unit type must be
            ///   *park*.
            /// <example>transport</example>
            /// </summary>
            [DataMember(Name = "from")]
            [DataTypes(Pattern = @"^unknown$|^exit$|^transport$|^.{1,5}$")]
            public string From { get; init; }

        }

    }
}
