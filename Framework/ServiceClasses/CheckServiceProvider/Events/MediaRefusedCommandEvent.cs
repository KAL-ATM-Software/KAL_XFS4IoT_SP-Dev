/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
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
using XFS4IoT.Check.Events;

namespace XFS4IoTFramework.Check
{
    public class MediaRefusedCommandEvent
    {
        /// <summary>
        /// ForeignItems - Foreign items were detected in the input position.
        /// StackerFull - The stacker is full or the maximum number of items that the application wants to be allowed on the stacker has been reached(see maxMediaOnStacker input parameter in the MediaIn command).
        /// CodelineInvalid - The code line data was found but was invalid.
        /// InvalidMedia - The media item is not a check, and in the case of Mixed Media processing not a cash item either.
        /// TooLong - The media item(or bunch of items) long edge was too long.
        /// TooShort - The media item(or bunch of items) long edge was too short.
        /// TooWide - The media item(or bunch of items) short edge was too wide.
        /// TooNarrow - The media item(or bunch of items) short edge was too narrow.
        /// TooThick - The media item was too thick.
        /// InvalidOrientation - The media item was inserted in an invalid orientation.
        /// DoubleDetect - The media items could not be separated.
        /// RefusePosFull - There are too many items in the refuse area.The refused items must be returned to the customer before any additional media items can be accepted.
        /// ReturnBlocked - Processing of the items did not take place as the bunch of items is blocking the return of other items.
        /// InvalidBunch - Processing of the items did not take place as the bunch of items presented is invalid, e.g.it is too large or was presented incorrectly.
        /// OtherItem - The item was refused for some reason not covered by the other reasons.
        /// OtherBunch - The bunch was refused for some reason not covered by the other reasons.
        /// Jamming - The media item is causing a jam.
        /// Metal - Metal (e.g.staple, paperclip, etc) was detected in the input position.
        /// </summary>
        public enum MediaRefusedReasonEnum
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
            Metal,
        };

        /// <summary>
        /// Input - The input position.
        /// Refused - The refused media position.
        /// ReBuncher - The refuse/return re-buncher.
        /// Stacker - The stacker.
        /// </summary>
        public enum MediaRefusedLocationEnum
        {
            Input,
            Refused,
            ReBuncher,
            Stacker,
        };

        /// <summary>
        /// Type of refused item.
        /// Item - Refused single item
        /// Bunch - Refused bunch of media
        /// </summary>
        public enum MediaTypeEnum
        {
            Item,
            Bunch,
        }

        public MediaRefusedCommandEvent(ICheckService checkService, IMediaInEvents MediaInEvents)
        {
            this.MediaInEvents = MediaInEvents;
            checkService.IsNotNull($"Invalid reference to check service passed in" + nameof(MediaPresentedCommandEvent));
            CheckScanner = checkService;
        }
        public MediaRefusedCommandEvent(ICheckService checkService, IGetNextItemEvents GetNextItemEvents)
        {
            this.GetNextItemEvents = GetNextItemEvents;
            checkService.IsNotNull($"Invalid reference to check service passed in" + nameof(MediaPresentedCommandEvent));
            CheckScanner = checkService;
        }

        public Task MediaRefusedEvent(
            MediaRefusedReasonEnum reason,
            MediaRefusedLocationEnum location,
            bool presentRequired,
            MediaSizeInfo mediaSize,
            MediaTypeEnum MediaType)
        {
            if (MediaType == MediaTypeEnum.Item)
            {
                CheckScanner.LastTransactionStatus.TotalItemsRefused++;
            }
            else
            {
                CheckScanner.LastTransactionStatus.TotalBunchesRefused++;
            }

            XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData payload = new(
                Reason: reason switch
                {
                    MediaRefusedReasonEnum.ForeignItems => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.ForeignItems,
                    MediaRefusedReasonEnum.StackerFull => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.StackerFull,
                    MediaRefusedReasonEnum.CodelineInvalid => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.CodelineInvalid,
                    MediaRefusedReasonEnum.InvalidMedia => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.InvalidMedia,
                    MediaRefusedReasonEnum.TooLong => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.TooLong,
                    MediaRefusedReasonEnum.TooShort => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.TooShort,
                    MediaRefusedReasonEnum.TooWide => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.TooWide,
                    MediaRefusedReasonEnum.TooNarrow => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.TooNarrow,
                    MediaRefusedReasonEnum.TooThick => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.TooThick,
                    MediaRefusedReasonEnum.InvalidOrientation => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.InvalidOrientation,
                    MediaRefusedReasonEnum.DoubleDetect => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.DoubleDetect,
                    MediaRefusedReasonEnum.RefusePosFull => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.RefusePosFull,
                    MediaRefusedReasonEnum.ReturnBlocked => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.ReturnBlocked,
                    MediaRefusedReasonEnum.InvalidBunch => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.InvalidBunch,
                    MediaRefusedReasonEnum.OtherItem => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.OtherItem,
                    MediaRefusedReasonEnum.OtherBunch => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.OtherBunch,
                    MediaRefusedReasonEnum.Jamming => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.Jamming,
                    MediaRefusedReasonEnum.Metal => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.ReasonEnum.Metal,
                    _ => throw new InternalErrorException($"Unsupported refused reason specified. {reason}")
                },
                Location: location switch
                {
                    MediaRefusedLocationEnum.Input => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.LocationEnum.Input,
                    MediaRefusedLocationEnum.Refused => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.LocationEnum.Refused,
                    MediaRefusedLocationEnum.ReBuncher => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.LocationEnum.Rebuncher,
                    MediaRefusedLocationEnum.Stacker => XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData.LocationEnum.Stacker,
                    _ => throw new InternalErrorException($"Unsupported refused location specified. {location}")
                },
                PresentRequired: presentRequired,
                MediaSize: mediaSize is null ?
                    null :
                    new(LongEdge: mediaSize.LongEdge,
                        ShortEdge: mediaSize.ShortEdge));

            if (MediaInEvents is not null)
            {
                return MediaInEvents.MediaRefusedEvent(payload);
            }
            if(GetNextItemEvents is not null)
            {
                return GetNextItemEvents.MediaRefusedEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(MediaDataEvent));
        }

        protected IMediaInEvents MediaInEvents { get; init; } = null;
        protected IGetNextItemEvents GetNextItemEvents { get; init; } = null;

        private ICheckService CheckScanner { get; init; } = null;
    }
}
