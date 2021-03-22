/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.TextTerminal.Events;
using XFS4IoT.Common.Events;

namespace TextTerminal
{
    internal class TextTerminalConnection : ITextTerminalConnection
    {
        private readonly IConnection connection;
        private readonly string requestId;

        public TextTerminalConnection(IConnection connection, string requestId)
        {
            this.connection = connection;
            Contracts.IsNotNullOrWhitespace(requestId, $"Unexpected request ID is received. {requestId}");
            this.requestId = requestId;
        }

        public void FieldErrorEvent(FieldErrorEvent.PayloadData Payload) => connection.SendMessageAsync(new XFS4IoT.TextTerminal.Events.FieldErrorEvent(requestId, Payload));

        public void FieldWarningEvent() => connection.SendMessageAsync(new XFS4IoT.TextTerminal.Events.FieldWarningEvent(requestId));

        public void KeyEvent(XFS4IoT.TextTerminal.Events.KeyEvent.PayloadData Payload) => connection.SendMessageAsync(new XFS4IoT.TextTerminal.Events.KeyEvent(requestId, Payload));

        public void PowerSaveChangeEvent(PowerSaveChangeEvent.PayloadData Payload) => connection.SendMessageAsync(new XFS4IoT.Common.Events.PowerSaveChangeEvent(requestId, Payload));

        public void DevicePositionEvent(DevicePositionEvent.PayloadData Payload) => connection.SendMessageAsync(new XFS4IoT.Common.Events.DevicePositionEvent(requestId, Payload));

        public void ServiceDetailEvent(ServiceDetailEvent.PayloadData Payload) => connection.SendMessageAsync(new XFS4IoT.Common.Events.ServiceDetailEvent(requestId, Payload));
    }
}
