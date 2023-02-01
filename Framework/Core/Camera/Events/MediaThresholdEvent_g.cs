/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Camera interface.
 * MediaThresholdEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Camera.Events
{

    [DataContract]
    [Event(Name = "Camera.MediaThresholdEvent")]
    public sealed class MediaThresholdEvent : UnsolicitedEvent<MediaThresholdEvent.PayloadData>
    {

        public MediaThresholdEvent(PayloadData Payload)
            : base(Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(MediaThresholdEnum? MediaThreshold = null)
                : base()
            {
                this.MediaThreshold = MediaThreshold;
            }

            public enum MediaThresholdEnum
            {
                Ok,
                High,
                Full
            }

            /// <summary>
            /// Specified as one of the following.
            /// * ```ok``` - The recording media is a good state.
            /// * ```high``` - The recording media is almost full.
            /// * ```full``` - The recording media is full.
            /// </summary>
            [DataMember(Name = "mediaThreshold")]
            public MediaThresholdEnum? MediaThreshold { get; init; }

        }

    }
}
