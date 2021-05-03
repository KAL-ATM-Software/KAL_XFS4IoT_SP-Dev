/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintRawFileEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Printer
{
    internal class PrintRawFileEvents : PrinterEvents, IPrintRawFileEvents
    {

        public PrintRawFileEvents(IConnection connection, string requestId)
            : base(connection, requestId)
        { }

        public async Task MediaPresentedEvent(XFS4IoT.Printer.Events.MediaPresentedEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Printer.Events.MediaPresentedEvent(requestId, Payload));

        public async Task NoMediaEvent(XFS4IoT.Printer.Events.NoMediaEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Printer.Events.NoMediaEvent(requestId, Payload));

        public async Task MediaInsertedEvent() => await connection.SendMessageAsync(new XFS4IoT.Printer.Events.MediaInsertedEvent(requestId));

        public async Task MediaRejectedEvent(XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Printer.Events.MediaRejectedEvent(requestId, Payload));

    }
}
