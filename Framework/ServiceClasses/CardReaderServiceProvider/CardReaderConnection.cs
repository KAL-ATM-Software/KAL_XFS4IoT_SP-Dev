/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CardReader.Events;
using XFS4IoT.Common.Events;

namespace CardReader
{
    internal class CardReaderConnection : ICardReaderConnection
    {
        private readonly IConnection connection;
        private readonly string requestId;

        public CardReaderConnection(IConnection connection, string requestId)
        {
            this.connection = connection;
            Contracts.IsNotNullOrWhitespace(requestId, $"Unexpected request ID is received. {requestId}");
            this.requestId = requestId;
        }

        public void InsertCardEvent() => connection.SendMessageAsync(new InsertCardEvent(requestId));

        public void MediaInsertedEvent() => connection.SendMessageAsync(new MediaInsertedEvent(requestId));

        public void MediaRemovedEvent() => connection.SendMessageAsync(new MediaRemovedEvent(requestId));

        public void InvalidTrackDataEvent(InvalidTrackDataEvent.PayloadData Payload) => connection.SendMessageAsync(new InvalidTrackDataEvent(requestId, Payload));

        public void InvalidMediaEvent() => connection.SendMessageAsync(new InvalidMediaEvent(requestId));

        public void TrackDetectedEvent(TrackDetectedEvent.PayloadData Payload) => connection.SendMessageAsync(new TrackDetectedEvent(requestId, Payload));

        public void RetainBinThresholdEvent(RetainBinThresholdEvent.PayloadData Payload) => connection.SendMessageAsync(new RetainBinThresholdEvent(requestId, Payload));

        public void MediaRetainedEvent() => connection.SendMessageAsync(new MediaRetainedEvent(requestId));

        public void MediaDetectedEvent(MediaDetectedEvent.PayloadData Payload) => connection.SendMessageAsync(new MediaDetectedEvent(requestId, Payload));

        public void EMVClessReadStatusEvent(EMVClessReadStatusEvent.PayloadData Payload) => connection.SendMessageAsync(new EMVClessReadStatusEvent(requestId, Payload));

        public void CardActionEvent(CardActionEvent.PayloadData Payload) => connection.SendMessageAsync(new CardActionEvent(requestId, Payload));

        public void PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => connection.SendMessageAsync(new PowerSaveChangeEvent(requestId, Payload));

        public void DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => connection.SendMessageAsync(new DevicePositionEvent(requestId, Payload));

        public void ServiceDetailEvent(ServiceDetailEvent.PayloadData Payload) => connection.SendMessageAsync(new ServiceDetailEvent(requestId, Payload));
    }
}
