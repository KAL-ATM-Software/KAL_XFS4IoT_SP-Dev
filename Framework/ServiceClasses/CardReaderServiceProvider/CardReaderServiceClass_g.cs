/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * CardReaderServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.CardReader;

namespace XFS4IoTServer
{
    public partial class CardReaderServiceClass : ICardReaderServiceClass
    {
        public CardReaderServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CardReaderServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICardReaderDevice>();
        }
        public async Task MediaRemovedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CardReader.Events.MediaRemovedEvent());

        public async Task RetainBinThresholdEvent(XFS4IoT.CardReader.Events.RetainBinThresholdEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CardReader.Events.RetainBinThresholdEvent(Payload));

        public async Task CardActionEvent(XFS4IoT.CardReader.Events.CardActionEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CardReader.Events.CardActionEvent(Payload));

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private ICardReaderDevice Device { get => ServiceProvider.Device.IsA<ICardReaderDevice>(); }
    }
}
