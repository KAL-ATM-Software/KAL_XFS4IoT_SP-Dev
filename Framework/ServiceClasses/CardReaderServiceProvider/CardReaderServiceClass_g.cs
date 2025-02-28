/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * CardReaderServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.CardReader;

namespace XFS4IoTServer
{
    public partial class CardReaderServiceClass : ICardReaderServiceClass
    {

        public async Task MediaRemovedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CardReader.Events.MediaRemovedEvent());

        public async Task CardActionEvent(XFS4IoT.CardReader.Events.CardActionEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CardReader.Events.CardActionEvent(Payload));

        public async Task MediaDetectedEvent(XFS4IoT.CardReader.Events.MediaDetectedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CardReader.Events.MediaDetectedEvent(Payload));

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICardReaderDevice Device { get => ServiceProvider.Device.IsA<ICardReaderDevice>(); }
    }
}
