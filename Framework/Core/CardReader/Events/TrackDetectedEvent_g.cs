/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * TrackDetectedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.CardReader.Events
{

    [DataContract]
    [Event(Name = "CardReader.TrackDetectedEvent")]
    public sealed class TrackDetectedEvent : Event<TrackDetectedEvent.PayloadData>
    {

        public TrackDetectedEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(bool? Track1 = null, bool? Track2 = null, bool? Track3 = null, bool? Watermark = null, bool? FrontTrack1 = null)
                : base()
            {
                this.Track1 = Track1;
                this.Track2 = Track2;
                this.Track3 = Track3;
                this.Watermark = Watermark;
                this.FrontTrack1 = FrontTrack1;
            }

            /// <summary>
            /// The card reader has track 1.
            /// </summary>
            [DataMember(Name = "track1")]
            public bool? Track1 { get; private set; }

            /// <summary>
            /// The card reader has track 2.
            /// </summary>
            [DataMember(Name = "track2")]
            public bool? Track2 { get; private set; }

            /// <summary>
            /// The card reader has track 3.
            /// </summary>
            [DataMember(Name = "track3")]
            public bool? Track3 { get; private set; }

            /// <summary>
            /// The card reader has the Swedish watermark track.
            /// </summary>
            [DataMember(Name = "watermark")]
            public bool? Watermark { get; private set; }

            /// <summary>
            /// The card reader has front track 1.
            /// </summary>
            [DataMember(Name = "frontTrack1")]
            public bool? FrontTrack1 { get; private set; }

        }

    }
}
