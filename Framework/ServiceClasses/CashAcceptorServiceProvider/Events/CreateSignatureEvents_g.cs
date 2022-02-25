/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CreateSignatureEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashAcceptor
{
    internal class CreateSignatureEvents : CashAcceptorEvents, ICreateSignatureEvents
    {

        public CreateSignatureEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task NoteErrorEvent(XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashManagement.Events.NoteErrorEvent(requestId, Payload));

        public async Task InfoAvailableEvent(XFS4IoT.CashManagement.Events.InfoAvailableEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashManagement.Events.InfoAvailableEvent(requestId, Payload));

        public async Task InputRefuseEvent(XFS4IoT.CashAcceptor.Events.InputRefuseEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashAcceptor.Events.InputRefuseEvent(requestId, Payload));

        public async Task InsertItemsEvent() => await connection.SendMessageAsync(new XFS4IoT.CashAcceptor.Events.InsertItemsEvent(requestId));

    }
}
