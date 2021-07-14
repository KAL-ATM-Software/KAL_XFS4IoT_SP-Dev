/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * MediaAutoRetractedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [Event(Name = "Printer.MediaAutoRetractedEvent")]
    public sealed class MediaAutoRetractedEvent : UnsolicitedEvent<MediaAutoRetractedEvent.PayloadData>
    {

        public MediaAutoRetractedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(RetractResultEnum? RetractResult = null, int? BinNumber = null)
                : base()
            {
                this.RetractResult = RetractResult;
                this.BinNumber = BinNumber;
            }

            public enum RetractResultEnum
            {
                Ok,
                Jammed
            }

            /// <summary>
            /// Specifies the result of the automatic retraction, as one of the following values:
            /// 
            /// * ```ok``` - The media was retracted successfully.
            /// * ```jammed``` - The media is jammed.
            /// </summary>
            [DataMember(Name = "retractResult")]
            public RetractResultEnum? RetractResult { get; init; }

            /// <summary>
            /// Number of the retract bin the media was retracted to or zero if the media is retracted to the transport.
            /// This number has to be between zero and the number of bins supported by this device. This value is also
            /// zero if [retractResult](#printer.mediaautoretractedevent.event.properties.retractresult) is *jammed*.
            /// </summary>
            [DataMember(Name = "binNumber")]
            public int? BinNumber { get; init; }

        }

    }
}
