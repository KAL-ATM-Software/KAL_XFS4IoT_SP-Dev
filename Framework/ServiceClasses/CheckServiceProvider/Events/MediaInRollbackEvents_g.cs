/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaInRollbackEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Check
{
    internal class MediaInRollbackEvents : CheckEvents, IMediaInRollbackEvents
    {

        public MediaInRollbackEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task MediaPresentedEvent(XFS4IoT.Check.Events.MediaPresentedEvent.PayloadData Payload) => await connection.SendMessageAsync(new XFS4IoT.Check.Events.MediaPresentedEvent(requestId, Payload));

    }
}
