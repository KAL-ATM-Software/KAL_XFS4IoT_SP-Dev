/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * WriteRawDataEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CardReader
{
    internal class WriteRawDataEvents : CardReaderEvents, IWriteRawDataEvents
    {

        public WriteRawDataEvents(IConnection connection, string requestId)
            : base(connection, requestId)
        { }

        public async Task InsertCardEvent() => await connection.SendMessageAsync(new XFS4IoT.CardReader.Events.InsertCardEvent(requestId));

        public async Task MediaInsertedEvent() => await connection.SendMessageAsync(new XFS4IoT.CardReader.Events.MediaInsertedEvent(requestId));

        public async Task InvalidMediaEvent() => await connection.SendMessageAsync(new XFS4IoT.CardReader.Events.InvalidMediaEvent(requestId));

        public async Task InvalidTrackDataEvent(XFS4IoT.CardReader.Events.InvalidTrackDataEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CardReader.Events.InvalidTrackDataEvent(requestId, Payload));

    }
}
