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

namespace XFS4IoTFramework.Check
{
    public class MediaPresentedCommandEvent
    {
        public enum PositionEnum
        {
            Input,
            Output,
            Refused
        }

        public MediaPresentedCommandEvent(IResetEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IResetEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaPresentedCommandEvent));
            ResetEvents = events;
        }
        public MediaPresentedCommandEvent(IActionItemEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IActionItemEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaPresentedCommandEvent));
            ActionItemEvents = events;
        }
        public MediaPresentedCommandEvent(IMediaInEndEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IMediaInEndEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaPresentedCommandEvent));
            MediaInEndEvents = events;
        }
        public MediaPresentedCommandEvent(IMediaInRollbackEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IMediaInRollbackEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaPresentedCommandEvent));
            MediaInRollbackEvents = events;
        }
        public MediaPresentedCommandEvent(IPresentMediaEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IPresentMediaEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaPresentedCommandEvent));
            PresentMediaEvents = events;
        }

        /// <summary>
        /// This indicates that media has been presented to the customer for removal.
        /// </summary>
        /// <param name="Position">Specifies the index 
        /// Specifies the position refused items are returned.
        /// </param>
        /// <param name="BunchIndex">Specifies the index 
        /// (starting from one) of the presented bunch (one or more items presented as a bunch).
        /// </param>
        /// <param name="TotalBunches">
        /// Specifies the total number of bunches to be returned from all positions. 
        /// The total represents the number of bunches that will be returned as a 
        /// result of a single command that presents media.
        /// Specify -1 if it's unknown.
        /// </param>
        public Task MediaPresentedEvent(
            PositionEnum Position,
            int BunchIndex,
            int TotalBunches = -1)
        {
            XFS4IoT.Check.Events.MediaPresentedEvent.PayloadData payload = new(
                Position: Position switch
                { 
                    PositionEnum.Input => XFS4IoT.Check.PositionEnum.Input,
                    PositionEnum.Refused => XFS4IoT.Check.PositionEnum.Refused,
                    PositionEnum.Output => XFS4IoT.Check.PositionEnum.Output,
                    _ => throw new InternalErrorException($"Unsupported position is specified. {Position}")
                },
                BunchIndex: BunchIndex,
                TotalBunches: TotalBunches == -1 ? 
                "unknown" :
                TotalBunches.ToString()
                );

            if (ResetEvents is not null)
            {
                return ResetEvents.MediaPresentedEvent(payload);
            }
            if (ActionItemEvents is not null)
            {
                return ActionItemEvents.MediaPresentedEvent(payload);
            }
            if (MediaInEndEvents is not null)
            {
                return MediaInEndEvents.MediaPresentedEvent(payload);
            }
            if (MediaInRollbackEvents is not null)
            {
                return MediaInRollbackEvents.MediaPresentedEvent(payload);
            }
            if (PresentMediaEvents is not null)
            {
                return PresentMediaEvents.MediaPresentedEvent(payload);
            }
            
            throw new InvalidOperationException($"Unreachable code. " + nameof(MediaPresentedEvent));
        }

        protected IResetEvents ResetEvents { get; init; } = null;
        protected IActionItemEvents ActionItemEvents { get; init; } = null;
        protected IMediaInEndEvents MediaInEndEvents { get; init; } = null;
        protected IMediaInRollbackEvents MediaInRollbackEvents { get; init; } = null;
        protected IPresentMediaEvents PresentMediaEvents { get; init; } = null;
 
    }
}