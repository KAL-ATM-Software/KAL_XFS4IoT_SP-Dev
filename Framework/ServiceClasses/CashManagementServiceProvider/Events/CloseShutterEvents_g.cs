/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CloseShutterEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashManagement
{
    internal class CloseShutterEvents : CashManagementEvents, ICloseShutterEvents
    {

        public CloseShutterEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task ShutterStatusChangedEvent(XFS4IoT.CashManagement.Events.ShutterStatusChangedEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.CashManagement.Events.ShutterStatusChangedEvent(requestId, Payload));

    }
}
