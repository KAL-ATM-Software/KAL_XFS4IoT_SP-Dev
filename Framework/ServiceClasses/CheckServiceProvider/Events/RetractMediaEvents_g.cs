/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * RetractMediaEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Check
{
    internal class RetractMediaEvents : CheckEvents, IRetractMediaEvents
    {

        public RetractMediaEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task StorageErrorEvent(XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Storage.Events.StorageErrorEvent(requestId, Payload));

    }
}
