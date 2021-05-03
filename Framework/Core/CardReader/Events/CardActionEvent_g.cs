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

            public enum ActionEnum
            {
                Retained,
                Ejected,
                ReadPosition,
            }

            public enum PositionEnum
            {
                Unknown,
                Present,
                Entering,
            }


            public PayloadData(ActionEnum? Action = null, PositionEnum? Position = null)
                : base()
            {
                this.Action = Action;
                this.Position = Position;
            }

            /// <summary>
            /// Specifies which action has been performed with the card. Possible values are:
            /// 
            /// * ```retained``` - The card has been retained.
            /// * ```ejected``` - The card has been ejected.
            /// * ```readPosition``` - The card has been moved to the read position.
            /// </summary>
            [DataMember(Name = "action")] 
            public ActionEnum? Action { get; private set; }
            /// <summary>
            /// Position of card before being retained or ejected. Possible values are:
            /// 
            /// * ```unknown``` - The position of the card cannot be determined.
            /// * ```present``` - The card was present in the reader.
            /// * ```entering``` - The card was entering the reader.
            /// </summary>
            [DataMember(Name = "position")] 
            public PositionEnum? Position { get; private set; }
        }

    }
}
