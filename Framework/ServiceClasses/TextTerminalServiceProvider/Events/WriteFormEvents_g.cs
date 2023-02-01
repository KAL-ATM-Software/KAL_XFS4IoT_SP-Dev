/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * WriteFormEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.TextTerminal
{
    internal class WriteFormEvents : TextTerminalEvents, IWriteFormEvents
    {

        public WriteFormEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task FieldErrorEvent(XFS4IoT.TextTerminal.Events.FieldErrorEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.TextTerminal.Events.FieldErrorEvent(requestId, Payload));

        public async Task FieldWarningEvent() => await connection.SendMessageAsync(new XFS4IoT.TextTerminal.Events.FieldWarningEvent(requestId));

    }
}
