/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * MediaDetectedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CardReader.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "CardReader.MediaDetectedEvent")]
    public sealed class MediaDetectedEvent : UnsolicitedEvent<MediaDetectedEvent.PayloadData>
    {

        public MediaDetectedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(string Position = null)
                : base()
            {
                this.Position = Position;
            }

            /// <summary>
            /// Specifies a card position or jammed state as one of the following:
            /// 
            /// * ```exit``` - A card is at the exit position.
            /// * ```transport``` - A card is in the transport position.
            /// * ```[storage unit identifier]``` - A card is in the
            ///   [identified](#storage.getstorage.completion.properties.storage.unit1) *retain* or *park* storage unit.
            /// * ```jammed``` - A card is jammed in the device.
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "position")]
            [DataTypes(Pattern = @"^exit$|^transport$|^jammed$|^unit[0-9A-Za-z]+$")]
            public string Position { get; init; }

        }

    }
}
