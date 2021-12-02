/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTFramework.Printer
{
    public sealed class ReadImageCommandEvents
    {
        public enum ReasonEnum
        {
            Short,
            Long,
            Multiple,
            Align,
            MoveToAlign,
            Shutter,
            Escrow,
            Thick,
            Other
        }

        public ReadImageCommandEvents(IReadImageEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ReadImageCommandEvents));
            events.IsA<IReadImageEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ReadImageCommandEvents));
            ReadImageEvents = events;
        }

        public Task NoMediaEvent(string UserPrompt)
        {
            ReadImageEvents.IsNotNull($"Unexpected interface specified. " + nameof(NoMediaEvent));
            return ReadImageEvents.NoMediaEvent(new(UserPrompt));
        }

        public Task MediaInsertedEvent()
        {
            ReadImageEvents.IsNotNull($"Unexpected interface specified. " + nameof(MediaInsertedEvent));
            return ReadImageEvents.MediaInsertedEvent();
        }

        public Task MediaRejectedEvent(ReasonEnum Reason)
        {
            ReadImageEvents.IsNotNull($"Unexpected interface specified. " + nameof(MediaInsertedEvent));
            return ReadImageEvents.MediaRejectedEvent(new(Reason switch
                                                          {
                                                              ReasonEnum.Short => XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Short,
                                                              ReasonEnum.Long => XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Long,
                                                              ReasonEnum.Multiple => XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Multiple,
                                                              ReasonEnum.Align => XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Align,
                                                              ReasonEnum.MoveToAlign => XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData.ReasonEnum.MoveToAlign,
                                                              ReasonEnum.Shutter => XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Shutter,
                                                              ReasonEnum.Escrow => XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Escrow,
                                                              ReasonEnum.Thick => XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Thick,
                                                              ReasonEnum.Other => XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData.ReasonEnum.Other,
                                                              _ => null
                                                          }));
        }

        private IReadImageEvents ReadImageEvents { get; init; } = null;
    }
}
