/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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

        public async Task CashUnitErrorEvent(XFS4IoT.CashManagement.Events.CashUnitErrorEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashManagement.Events.CashUnitErrorEvent(requestId, Payload));

        public async Task NoteErrorEvent(XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashManagement.Events.NoteErrorEvent(requestId, Payload));

        public async Task InfoAvailableEvent(XFS4IoT.CashManagement.Events.InfoAvailableEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashManagement.Events.InfoAvailableEvent(requestId, Payload));

        public async Task DelayedDispenseEvent(XFS4IoT.CashDispenser.Events.DelayedDispenseEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashDispenser.Events.DelayedDispenseEvent(requestId, Payload));

        public async Task StartDispenseEvent(XFS4IoT.CashDispenser.Events.StartDispenseEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashDispenser.Events.StartDispenseEvent(requestId, Payload));

        public async Task PartialDispenseEvent(XFS4IoT.CashDispenser.Events.PartialDispenseEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashDispenser.Events.PartialDispenseEvent(requestId, Payload));

        public async Task SubDispenseOkEvent(XFS4IoT.CashDispenser.Events.SubDispenseOkEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashDispenser.Events.SubDispenseOkEvent(requestId, Payload));

        public async Task IncompleteDispenseEvent(XFS4IoT.CashDispenser.Events.IncompleteDispenseEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashDispenser.Events.IncompleteDispenseEvent(requestId, Payload));

    }
}
