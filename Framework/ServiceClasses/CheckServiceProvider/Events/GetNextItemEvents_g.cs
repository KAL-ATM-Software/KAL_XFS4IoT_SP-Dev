/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * GetNextItemEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Check
{
    internal class GetNextItemEvents : CheckEvents, IGetNextItemEvents
    {

        public GetNextItemEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task MediaRefusedEvent(XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Check.Events.MediaRefusedEvent(requestId, Payload));

        public async Task MediaDataEvent(XFS4IoT.Check.Events.MediaDataEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Check.Events.MediaDataEvent(requestId, Payload));

    }
}
