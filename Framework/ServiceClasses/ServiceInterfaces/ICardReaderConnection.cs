/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using XFS4IoTFramework.Common;

namespace CardReader
{
    public interface ICardReaderConnection : ICommonConnection
    {

        void InsertCardEvent();

        void MediaInsertedEvent();

        void MediaRemovedEvent();

        void InvalidTrackDataEvent(XFS4IoT.CardReader.Events.InvalidTrackDataEvent.PayloadData Payload);

        void InvalidMediaEvent();

        void TrackDetectedEvent(XFS4IoT.CardReader.Events.TrackDetectedEvent.PayloadData Payload);

        void RetainBinThresholdEvent(XFS4IoT.CardReader.Events.RetainBinThresholdEvent.PayloadData Payload);

        void MediaRetainedEvent();

        void MediaDetectedEvent(XFS4IoT.CardReader.Events.MediaDetectedEvent.PayloadData Payload);

        void EMVClessReadStatusEvent(XFS4IoT.CardReader.Events.EMVClessReadStatusEvent.PayloadData Payload);

        void CardActionEvent(XFS4IoT.CardReader.Events.CardActionEvent.PayloadData Payload);

        void PowerSaveChangeEvent(XFS4IoT.Common.Events.PowerSaveChangeEvent.PayloadData Payload);

        void DevicePositionEvent(XFS4IoT.Common.Events.DevicePositionEvent.PayloadData Payload);

    }
}
