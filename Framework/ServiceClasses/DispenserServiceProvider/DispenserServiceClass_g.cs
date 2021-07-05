/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * DispenserServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Dispenser;

namespace XFS4IoTServer
{
    public partial class DispenserServiceClass : IDispenserServiceClass
    {
        public DispenserServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(DispenserServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IDispenserDevice>();
        }
        public async Task ItemsTakenEvent(XFS4IoT.Dispenser.Events.ItemsTakenEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Dispenser.Events.ItemsTakenEvent(Payload));

        public async Task ShutterStatusChangedEvent(XFS4IoT.Dispenser.Events.ShutterStatusChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Dispenser.Events.ShutterStatusChangedEvent(Payload));

        public async Task MediaDetectedEvent(XFS4IoT.Dispenser.Events.MediaDetectedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Dispenser.Events.MediaDetectedEvent(Payload));

        public async Task ItemsPresentedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Dispenser.Events.ItemsPresentedEvent());

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private IDispenserDevice Device { get => ServiceProvider.Device.IsA<IDispenserDevice>(); }
    }
}
