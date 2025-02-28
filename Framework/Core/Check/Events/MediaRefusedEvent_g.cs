/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaRefusedEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Check.Events
{

    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Event(Name = "Check.MediaRefusedEvent")]
    public sealed class MediaRefusedEvent : Event<MediaRefusedEvent.PayloadData>
    {

        public MediaRefusedEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(ReasonEnum? Reason = null, LocationEnum? Location = null, bool? PresentRequired = null, MediaSizeClass MediaSize = null)
                : base()
            {
                this.Reason = Reason;
                this.Location = Location;
                this.PresentRequired = PresentRequired;
                this.MediaSize = MediaSize;
            }

            public enum ReasonEnum
            {
                ForeignItems,
                StackerFull,
                CodelineInvalid,
                InvalidMedia,
                TooLong,
                TooShort,
                TooWide,
                TooNarrow,
                TooThick,
                InvalidOrientation,
                DoubleDetect,
                RefusePosFull,
                ReturnBlocked,
                InvalidBunch,
                OtherItem,
                OtherBunch,
                Jamming,
                Metal
            }

            /// <summary>
            /// Specified the reason why the media was refused. Specified as one of the following values:
            /// 
            /// * ```foreignItems``` - Foreign items were detected in the input position.
            /// * ```stackerFull``` - The stacker is full or the maximum number of items that the application wants to be allowed on the stacker has been reached.
            /// * ```codelineInvalid``` - The code line data was found but was invalid.
            /// * ```invalidMedia``` - The media item is not a check, and in the case of Mixed Media processing not a cash item either.
            /// * ```tooLong``` - The media item (or bunch of items) long edge was too long.
            /// * ```tooShort``` - The media item (or bunch of items) long edge was too short.
            /// * ```tooWide``` - The media item (or bunch of items) short edge was too wide.
            /// * ```tooNarrow``` - The media item (or bunch of items) short edge was too narrow.
            /// * ```tooThick``` - The media item was too thick.
            /// * ```invalidOrientation``` - The media item was inserted in an invalid orientation.
            /// * ```doubleDetect``` - The media items could not be separated.
            /// * ```refusePosFull``` - There are too many items in the refuse area. The refused items must be returned to the customer before any additional media items can be accepted.
            /// * ```returnBlocked``` - Processing of the items did not take place as the bunch of items is blocking the return of other items.
            /// * ```invalidBunch``` - Processing of the items did not take place as the bunch of items presented is invalid, e.g. it is too large or was presented incorrectly.
            /// * ```otherItem``` - The item was refused for some reason not covered by the other reasons.
            /// * ```otherBunch``` - The bunch was refused for some reason not covered by the other reasons.
            /// * ```jamming``` - The media item is causing a jam.
            /// * ```metal``` - Metal (e.g. staple, paperclip, etc.) was detected in the input position.
            /// <example>metal</example>
            /// </summary>
            [DataMember(Name = "reason")]
            public ReasonEnum? Reason { get; init; }

            public enum LocationEnum
            {
                Input,
                Refused,
                Rebuncher,
                Stacker
            }

            /// <summary>
            /// Specifies where the refused media should be presented to the customer from. It can be one of the following
            /// values:
            /// 
            /// * ```input``` - The input position.
            /// * ```refused``` - The refused media position.
            /// * ```rebuncher``` - The refuse/return re-buncher.
            /// * ```stacker``` - The stacker.
            /// <example>refused</example>
            /// </summary>
            [DataMember(Name = "location")]
            public LocationEnum? Location { get; init; }

            /// <summary>
            /// This indicates whether the media needs to be presented to the customer before any additional media movement
            /// commands can be executed. If true, then the media must be presented to the customer via the
            /// [Check.PresentMedia](#check.presentmedia) command before further media movement commands can be executed. If
            /// false, then the device can continue without the media being returned to the customer.
            /// <example>true</example>
            /// </summary>
            [DataMember(Name = "presentRequired")]
            public bool? PresentRequired { get; init; }

            [DataMember(Name = "mediaSize")]
            public MediaSizeClass MediaSize { get; init; }

        }

    }
}
