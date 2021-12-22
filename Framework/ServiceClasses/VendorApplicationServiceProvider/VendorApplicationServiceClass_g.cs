/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * VendorApplicationServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.VendorApplication;

namespace XFS4IoTServer
{
    public partial class VendorApplicationServiceClass : IVendorApplicationServiceClass
    {
        public VendorApplicationServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(VendorApplicationServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IVendorApplicationDevice>();
        }
        public async Task InterfaceChangedEvent(XFS4IoT.VendorApplication.Events.InterfaceChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorApplication.Events.InterfaceChangedEvent(Payload));

        public async Task VendorAppExitedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorApplication.Events.VendorAppExitedEvent());

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private IVendorApplicationDevice Device { get => ServiceProvider.Device.IsA<IVendorApplicationDevice>(); }
    }
}
