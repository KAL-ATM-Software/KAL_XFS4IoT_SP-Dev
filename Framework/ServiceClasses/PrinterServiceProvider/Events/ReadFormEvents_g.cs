/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ReadFormEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Printer
{
    internal class ReadFormEvents : PrinterEvents, IReadFormEvents
    {

        public ReadFormEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task NoMediaEvent(XFS4IoT.Printer.Events.NoMediaEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Printer.Events.NoMediaEvent(requestId, Payload));

        public async Task MediaInsertedEvent() => await connection.SendMessageAsync(new XFS4IoT.Printer.Events.MediaInsertedEvent(requestId));

        public async Task FieldErrorEvent(XFS4IoT.Printer.Events.FieldErrorEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Printer.Events.FieldErrorEvent(requestId, Payload));

        public async Task FieldWarningEvent(XFS4IoT.Printer.Events.FieldWarningEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Printer.Events.FieldWarningEvent(requestId, Payload));

        public async Task MediaRejectedEvent(XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Printer.Events.MediaRejectedEvent(requestId, Payload));

    }
}
