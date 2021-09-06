/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * PinEntryEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Keyboard
{
    internal class PinEntryEvents : KeyboardEvents, IPinEntryEvents
    {

        public PinEntryEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task KeyEvent(XFS4IoT.Keyboard.Events.KeyEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Keyboard.Events.KeyEvent(requestId, Payload));

        public async Task EnterDataEvent() => await connection.SendMessageAsync(new XFS4IoT.Keyboard.Events.EnterDataEvent(requestId));

        public async Task LayoutEvent(XFS4IoT.Keyboard.Events.LayoutEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Keyboard.Events.LayoutEvent(requestId, Payload));

    }
}
