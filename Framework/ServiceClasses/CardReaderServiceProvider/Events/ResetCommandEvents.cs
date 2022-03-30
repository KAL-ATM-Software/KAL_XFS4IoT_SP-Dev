/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using XFS4IoTServer;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.CardReader.Events;

namespace XFS4IoTFramework.CardReader
{
    public sealed class ResetCommandEvents
    {
        public ResetCommandEvents(IResetEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ResetCommandEvents));
            events.IsA<IResetEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ResetCommandEvents));
            ResetEvents = events;
        }

        public Task MediaDetectedEvent(MovePosition Position)
        {
            ResetEvents.IsNotNull($"Unexpected interface specified. " + nameof(MediaDetectedEvent));
            return ResetEvents.MediaDetectedEvent(new MediaDetectedEvent.PayloadData(Position.Position switch
                                                                                     {
                                                                                         MovePosition.MovePositionEnum.Exit => "exit",
                                                                                         MovePosition.MovePositionEnum.Transport => "transport",
                                                                                         _ => Position.StorageId,
                                                                                     }));
        }

        private IResetEvents ResetEvents { get; init; } = null;
    }
}
