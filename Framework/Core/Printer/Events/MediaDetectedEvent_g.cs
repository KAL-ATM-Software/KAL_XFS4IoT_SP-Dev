/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Event(Name = "Printer.MediaDetectedEvent")]
    public sealed class MediaDetectedEvent : UnsolicitedEvent<MediaDetectedEvent.PayloadData>
    {

        public MediaDetectedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(PositionEnum? Position = null, int? RetractBinNumber = null)
                : base()
            {
                this.Position = Position;
                this.RetractBinNumber = RetractBinNumber;
            }

            public enum PositionEnum
            {
                Retracted,
                Present,
                Entering,
                Jammed,
                Unknown,
                Expelled
            }

            /// <summary>
            /// Specifies the media position after the reset operation, as one of the following values:
            /// 
            /// * ```retracted``` - The media was retracted during the reset operation.
            /// * ```present``` - The media is in the print position or on the stacker.
            /// * ```entering``` - The media is in the exit slot.
            /// * ```jammed``` - The media is jammed in the device.
            /// * ```unknown``` - The media is in an unknown position.
            /// * ```expelled``` - The media was expelled during the reset operation.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionEnum? Position { get; private set; }

            /// <summary>
            /// Number of the retract bin the media was retracted to. This number has to be between one and the 
            /// [number of bins](#common.capabilities.completion.properties.printer.retractbins) supported by this device.
            /// It is only relevant if [position](#printer.mediadetectedevent.event.properties.position) is *retracted*.
            /// </summary>
            [DataMember(Name = "retractBinNumber")]
            public int? RetractBinNumber { get; private set; }

        }

    }
}
