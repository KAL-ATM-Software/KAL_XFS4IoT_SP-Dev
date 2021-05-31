/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * MediaPresentedUnsolicitedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [Event(Name = "Printer.MediaPresentedUnsolicitedEvent")]
    public sealed class MediaPresentedUnsolicitedEvent : UnsolicitedEvent<MediaPresentedUnsolicitedEvent.PayloadData>
    {

        public MediaPresentedUnsolicitedEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(int? WadIndex = null, int? TotalWads = null)
                : base()
            {
                this.WadIndex = WadIndex;
                this.TotalWads = TotalWads;
            }

            /// <summary>
            /// Specifies the index (starting from one) of the presented wad, where a Wad is a bunch of one or more pages
            /// presented as a bunch.
            /// </summary>
            [DataMember(Name = "wadIndex")]
            public int? WadIndex { get; private set; }

            /// <summary>
            /// Specifies the total number of wads in the print job, zero if the total number of wads is not known.
            /// </summary>
            [DataMember(Name = "totalWads")]
            public int? TotalWads { get; private set; }

        }

    }
}
