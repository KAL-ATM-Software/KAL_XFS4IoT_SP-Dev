/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTFramework.Printer
{
    public abstract class MediaPresentedCommandEvent
    {
        public MediaPresentedCommandEvent(IPrintRawEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IPrintRawEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaPresentedCommandEvent));
            PrintRawEvents = events;
        }
        public MediaPresentedCommandEvent(IDispensePaperEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(MediaPresentedCommandEvent));
            events.IsA<IDispensePaperEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(MediaPresentedCommandEvent));
            DispensePaperEvents = events;
        }

        public Task MediaPresentedEvent(int WadIndex, int TotalWads)
        {
            if (PrintRawEvents is not null)
            {
                return PrintRawEvents.MediaPresentedEvent(new(WadIndex, TotalWads));
            }
            if (DispensePaperEvents is not null)
            {
                return DispensePaperEvents.MediaPresentedEvent(new(WadIndex, TotalWads));
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(MediaPresentedEvent));
        }

        private IPrintRawEvents PrintRawEvents { get; init; } = null;
        private IDispensePaperEvents DispensePaperEvents { get; init; } = null;
    }
}
