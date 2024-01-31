/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * MediaDetectedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Printer.MediaDetectedEvent")]
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
            /// Specifies the media position after the reset operation, as one of the following values:
            /// 
            /// * ```present``` - The media is in the print position or on the stacker.
            /// * ```entering``` - The media is in the exit slot.
            /// * ```jammed``` - The media is jammed in the device.
            /// * ```unknown``` - The media is in an unknown position.
            /// * ```expelled``` - The media was expelled during the reset operation.
            /// * ```unit&lt;retract bin number&gt;``` - Media was retracted to the retract bin specified. The bin number
            /// is between 1 and the [number of bins](#common.capabilities.completion.properties.printer.retractbins)
            /// supported by this device.
            /// <example>unit2</example>
            /// </summary>
            [DataMember(Name = "position")]
            [DataTypes(Pattern = @"^present$|^entering$|^jammed$|^unknown$|^expelled$|^unit[0-9]+$")]
            public string Position { get; init; }

        }

    }
}
