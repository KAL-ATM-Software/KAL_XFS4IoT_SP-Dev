/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * MediaPresentedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Printer.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Printer.MediaPresentedEvent")]
    public sealed class MediaPresentedEvent : Event<MediaPresentedEvent.PayloadData>
    {

        public MediaPresentedEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
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
            /// Specifies the index (starting from 1) of the presented wad, where a Wad is a bunch of one or more pages
            /// presented as a bunch.
            /// </summary>
            [DataMember(Name = "wadIndex")]
            [DataTypes(Minimum = 1)]
            public int? WadIndex { get; init; }

            /// <summary>
            /// Specifies the total number of wads in the print job, 0 if not known.
            /// </summary>
            [DataMember(Name = "totalWads")]
            [DataTypes(Minimum = 0)]
            public int? TotalWads { get; init; }

        }

    }
}
