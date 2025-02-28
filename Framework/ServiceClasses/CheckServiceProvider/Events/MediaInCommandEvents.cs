/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Check;
using XFS4IoT.Check;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Security.Policy;
using XFS4IoT.Commands;
using System.ComponentModel;
using XFS4IoTServer;
using XFS4IoT.Events;

namespace XFS4IoTFramework.Check
{
    public class MediaInCommandEvents : MediaDataCommandEvent
    {
        public MediaInCommandEvents(ICheckService service, IMediaInEvents events) : 
            base(service, events)
        {
            MediaRefused = new(service, events);
        }
        public MediaInCommandEvents(ICheckService service, IGetNextItemEvents events) :
            base(service, events)
        {
            MediaRefused = new(service, events);
        }
        /// <summary>
        /// Long - The media was too long.
        /// Thick - The media was too thick.
        /// Double - More than one media item was detected(this value only applies to devices without a media feeder).
        /// Shutter - The media was rejected due to the shutter failing to close.
        /// Removed - The media was removed(no[Check.MediaTakenEvent](#check.mediatakenevent) is expected).
        /// Metal - Metal (e.g.staple, paperclip, etc) was detected in the input position.
        /// ForeignItems - Foreign items were detected in the input position.
        /// Other - The item was rejected for some reason not covered by the other reasons.
        /// </summary>
        public enum MediaRejectedReasonEnum
        {
            Long,
            Thick,
            Double,
            Shutter,
            Removed,
            Metal,
            ForeignItems,
            Other
        };

        public Task NoNoMediaEvent() => MediaInEvents.NoMediaEvent();

        public Task MediaInsertedEvent() => MediaInEvents.MediaInsertedEvent();

        public Task MediaRejectedEvent(MediaRejectedReasonEnum reason)
        {
            return MediaInEvents.MediaRejectedEvent(new(
                Reason: reason switch
                {
                    MediaRejectedReasonEnum.Long => XFS4IoT.Check.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Long,
                    MediaRejectedReasonEnum.Thick => XFS4IoT.Check.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Thick,
                    MediaRejectedReasonEnum.Double => XFS4IoT.Check.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Double,
                    MediaRejectedReasonEnum.Shutter => XFS4IoT.Check.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Shutter,
                    MediaRejectedReasonEnum.Removed => XFS4IoT.Check.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Removed,
                    MediaRejectedReasonEnum.Metal => XFS4IoT.Check.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Metal,
                    MediaRejectedReasonEnum.ForeignItems => XFS4IoT.Check.Events.MediaRejectedEvent.PayloadData.ReasonEnum.ForeignItems,
                    MediaRejectedReasonEnum.Other => XFS4IoT.Check.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Other,
                    _ => throw new InternalErrorException($"Unsupported rejected location specified. {reason}")
                }
                ));
        }

        public Task MediaRefusedEvent(
            MediaRefusedCommandEvent.MediaRefusedReasonEnum reason,
            MediaRefusedCommandEvent. MediaRefusedLocationEnum location,
            bool presentRequired,
            MediaSizeInfo mediaSize,
            MediaRefusedCommandEvent.MediaTypeEnum mediaType) => MediaRefused.MediaRefusedEvent(reason, location, presentRequired, mediaSize, mediaType);


        private MediaRefusedCommandEvent MediaRefused { get; init; } = null;
    }
}