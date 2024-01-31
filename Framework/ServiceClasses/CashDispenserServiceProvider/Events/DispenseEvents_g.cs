/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * DispenseEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashDispenser
{
    internal class DispenseEvents : CashDispenserEvents, IDispenseEvents
    {

        public DispenseEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task StorageErrorEvent(XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Storage.Events.StorageErrorEvent(requestId, Payload));

        public async Task NoteErrorEvent(XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashManagement.Events.NoteErrorEvent(requestId, Payload));

        public async Task InfoAvailableEvent(XFS4IoT.CashManagement.Events.InfoAvailableEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashManagement.Events.InfoAvailableEvent(requestId, Payload));

        public async Task DelayedDispenseEvent(XFS4IoT.CashDispenser.Events.DelayedDispenseEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashDispenser.Events.DelayedDispenseEvent(requestId, Payload));

        public async Task StartDispenseEvent() => await connection.SendMessageAsync(new XFS4IoT.CashDispenser.Events.StartDispenseEvent(requestId));

        public async Task IncompleteDispenseEvent(XFS4IoT.CashDispenser.Events.IncompleteDispenseEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashDispenser.Events.IncompleteDispenseEvent(requestId, Payload));

    }
}
