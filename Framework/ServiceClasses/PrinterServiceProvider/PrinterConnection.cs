/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Events;
using XFS4IoT.Printer.Events;

namespace Printer
{
    internal class PrinterConnection : IPrinterConnection
    {
        private readonly IConnection connection;
        private readonly string requestId;

        public PrinterConnection(IConnection connection, string requestId)
        {
            this.connection = connection;
            Contracts.IsNotNullOrWhitespace(requestId, $"Unexpected request ID is received. {requestId}");
            this.requestId = requestId;
        }

        public void RetractBinThresholdEvent(RetractBinThresholdEvent.PayloadData Payload) => connection.SendMessageAsync(new RetractBinThresholdEvent(requestId, Payload));

        public void MediaTakenEvent() => connection.SendMessageAsync(new MediaTakenEvent(requestId));

        public void PaperThresholdEvent(PaperThresholdEvent.PayloadData Payload) => connection.SendMessageAsync(new PaperThresholdEvent(requestId, Payload));

        public void TonerThresholdEvent(TonerThresholdEvent.PayloadData Payload) => connection.SendMessageAsync(new TonerThresholdEvent(requestId, Payload));

        public void InkThresholdEvent(InkThresholdEvent.PayloadData Payload) => connection.SendMessageAsync(new InkThresholdEvent(requestId, Payload));

        public void MediaPresentedEvent(MediaPresentedEvent.PayloadData Payload) => connection.SendMessageAsync(new MediaPresentedEvent(requestId, Payload));

        public void MediaAutoRetractedEvent(MediaAutoRetractedEvent.PayloadData Payload) => connection.SendMessageAsync(new MediaAutoRetractedEvent(requestId, Payload));

        public void NoMediaEvent(NoMediaEvent.PayloadData Payload) => connection.SendMessageAsync(new NoMediaEvent(requestId, Payload));

        public void MediaInsertedEvent() => connection.SendMessageAsync(new MediaInsertedEvent(requestId));

        public void FieldErrorEvent(FieldErrorEvent.PayloadData Payload) => connection.SendMessageAsync(new FieldErrorEvent(requestId, Payload));

        public void FieldWarningEvent(FieldWarningEvent.PayloadData Payload) => connection.SendMessageAsync(new FieldWarningEvent(requestId, Payload));

        public void MediaRejectedEvent(MediaRejectedEvent.PayloadData Payload) => connection.SendMessageAsync(new MediaRejectedEvent(requestId, Payload));

        public void LampThresholdEvent(LampThresholdEvent.PayloadData Payload) => connection.SendMessageAsync(new LampThresholdEvent(requestId, Payload));

        public void MediaDetectedEvent(MediaDetectedEvent.PayloadData Payload) => connection.SendMessageAsync(new MediaDetectedEvent(requestId, Payload));

        public void DefinitionLoadedEvent(DefinitionLoadedEvent.PayloadData Payload) => connection.SendMessageAsync(new DefinitionLoadedEvent(requestId, Payload));

        public void MediaInsertedUnsolicitedEvent() => connection.SendMessageAsync(new MediaInsertedUnsolicitedEvent(requestId));

        public void MediaPresentedUnsolicitedEvent(MediaPresentedUnsolicitedEvent.PayloadData Payload) => connection.SendMessageAsync(new MediaPresentedUnsolicitedEvent(requestId, Payload));

        public void RetractBinStatusEvent(RetractBinStatusEvent.PayloadData Payload) => connection.SendMessageAsync(new RetractBinStatusEvent(requestId, Payload));

        public void PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => connection.SendMessageAsync(new PowerSaveChangeEvent(requestId, Payload));

        public void DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => connection.SendMessageAsync(new DevicePositionEvent(requestId, Payload));

        public void ServiceDetailEvent(ServiceDetailEvent.PayloadData Payload) => connection.SendMessageAsync(new ServiceDetailEvent(requestId, Payload));
    }
}
