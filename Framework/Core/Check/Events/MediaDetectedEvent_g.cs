/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaDetectedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Check.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Check.MediaDetectedEvent")]
    public sealed class MediaDetectedEvent : UnsolicitedEvent<MediaDetectedEvent.PayloadData>
    {

        public MediaDetectedEvent()
            : base()
        { }

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
            /// Specifies the media position after the operation, as one of the following values:
            /// 
            /// * ```[storage unit identifier]``` - The media item was retracted to a storage unit as specified by
            ///   [identifier](#storage.getstorage.completion.properties.storage.unit1).
            /// * ```device``` - The media is in the device.
            /// * ```position``` - The media is at one or more of the input, output and refused positions.
            /// * ```jammed``` - The media is jammed in the device.
            /// * ```customer``` - The media has been returned and taken by the customer.
            /// * ```unknown``` - The media is in an unknown position.
            /// <example>customer</example>
            /// </summary>
            [DataMember(Name = "position")]
            [DataTypes(Pattern = @"^device$|^position$|^jammed$|^customer$|^unknown$|^unit[0-9A-Za-z]+$")]
            public string Position { get; init; }

        }

    }
}
