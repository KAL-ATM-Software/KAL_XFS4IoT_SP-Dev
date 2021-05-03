/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/


using System.Threading.Tasks;
using XFS4IoT;

namespace XFS4IoTFramework.CardReader
{
    internal class AcceptCardEvents : IAcceptCardEvents
    {
        public AcceptCardEvents(ReadRawDataEvents events)
        {
            readRawDataEvents = events;
            writeRawDataEvents = null;
        }
        public AcceptCardEvents(WriteRawDataEvents events)
        {
            readRawDataEvents = null;
            writeRawDataEvents = events;
        }

        public async Task InsertCardEvent()
        {
            Contracts.Assert(readRawDataEvents is not null || writeRawDataEvents is not null, $"Invalid event reference is set." + nameof(AcceptCardEvents));

            if (readRawDataEvents is not null)
                await readRawDataEvents.InsertCardEvent();
            else
                await writeRawDataEvents.InsertCardEvent();
        }

        public async Task MediaInsertedEvent()
        {
            Contracts.Assert(readRawDataEvents is not null || writeRawDataEvents is not null, $"Invalid event reference is set." + nameof(AcceptCardEvents));

            if (readRawDataEvents is not null)
                await readRawDataEvents.MediaInsertedEvent();
            else
                await writeRawDataEvents.MediaInsertedEvent();
        }

        public async Task InvalidMediaEvent()
        {
            Contracts.Assert(readRawDataEvents is not null || writeRawDataEvents is not null, $"Invalid event reference is set." + nameof(AcceptCardEvents));

            if (readRawDataEvents is not null)
                await readRawDataEvents.InvalidMediaEvent();
            else
                await writeRawDataEvents.InvalidMediaEvent();
        }

        private readonly ReadRawDataEvents readRawDataEvents;
        private readonly WriteRawDataEvents writeRawDataEvents;
    }
}
