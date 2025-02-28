﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTFramework.CardReader
{
    public abstract class CommonCardCommandEvents
    {
        public CommonCardCommandEvents(IReadRawDataEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(CommonCardCommandEvents));
            events.IsA<IReadRawDataEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(CommonCardCommandEvents));
            ReadRawDataEvents = events;
        }
        public CommonCardCommandEvents(IWriteRawDataEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(CommonCardCommandEvents));
            events.IsA<IWriteRawDataEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(CommonCardCommandEvents));
            WriteRawDataEvents = events;
        }

        public Task InsertCardEvent()
        {
            if (ReadRawDataEvents is not null)
            {
                return ReadRawDataEvents.InsertCardEvent();
            }
            if (WriteRawDataEvents is not null)
            {
                return WriteRawDataEvents.InsertCardEvent();
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(InsertCardEvent));
        }

        public Task MediaInsertedEvent()
        {
            if (ReadRawDataEvents is not null)
            {
                return ReadRawDataEvents.MediaInsertedEvent();
            }
            if (WriteRawDataEvents is not null)
            {
                return WriteRawDataEvents.MediaInsertedEvent();
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(MediaInsertedEvent));
        }

        public Task InvalidMediaEvent()
        {
            if (ReadRawDataEvents is not null)
            {
                return ReadRawDataEvents.InvalidMediaEvent();
            }
            if (WriteRawDataEvents is not null)
            {
                return WriteRawDataEvents.InvalidMediaEvent();
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(InvalidMediaEvent));
        }

        protected IReadRawDataEvents ReadRawDataEvents { get; init; } = null;
        protected IWriteRawDataEvents WriteRawDataEvents { get; init; } = null;
    }
}
