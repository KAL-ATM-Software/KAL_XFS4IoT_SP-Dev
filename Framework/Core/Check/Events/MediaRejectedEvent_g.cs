/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaRejectedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Check.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Check.MediaRejectedEvent")]
    public sealed class MediaRejectedEvent : Event<MediaRejectedEvent.PayloadData>
    {

        public MediaRejectedEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(ReasonEnum? Reason = null)
                : base()
            {
                this.Reason = Reason;
            }

            public enum ReasonEnum
            {
                Long,
                Thick,
                Double,
                Transport,
                Shutter,
                Removed,
                Metal,
                ForeignItems,
                Other
            }

            /// <summary>
            /// Specified the reason why the media was rejected. Specified as one of the following values:
            /// 
            /// * ```long``` - The media was too long.
            /// * ```thick``` - The media was too thick.
            /// * ```double``` - More than one media item was detected (this value only applies to devices without a media feeder).
            /// * ```transport``` - The media could not be moved inside the device.
            /// * ```shutter``` - The media was rejected due to the shutter failing to close.
            /// * ```removed``` - The media was removed (no [Check.MediaTakenEvent](#check.mediatakenevent) is expected).
            /// * ```metal``` - Metal (e.g. staple, paperclip, etc.) was detected in the input position.
            /// * ```foreignItems``` - Foreign items were detected in the input position.
            /// * ```other``` - The item was rejected for some reason not covered by the other reasons.
            /// <example>metal</example>
            /// </summary>
            [DataMember(Name = "reason")]
            public ReasonEnum? Reason { get; init; }

        }

    }
}
