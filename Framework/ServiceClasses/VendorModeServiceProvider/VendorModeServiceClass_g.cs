/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * VendorModeServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.VendorMode;

namespace XFS4IoTServer
{
    public partial class VendorModeServiceClass : IVendorModeServiceClass
    {
        public VendorModeServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(VendorModeServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IVendorModeDevice>();
        }
        public async Task ExitModeRequestEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.ExitModeRequestEvent());

        public async Task ModeEnteredEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.ModeEnteredEvent());

        public async Task ModeExitedEvent(XFS4IoT.VendorMode.Events.ModeExitedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.ModeExitedEvent(Payload));

        public async Task EnterModeRequestEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.EnterModeRequestEvent());

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private IVendorModeDevice Device { get => ServiceProvider.Device.IsA<IVendorModeDevice>(); }
    }
}
