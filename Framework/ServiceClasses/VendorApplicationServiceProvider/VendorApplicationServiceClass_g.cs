/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * VendorApplicationServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.VendorApplication;

namespace XFS4IoTServer
{
    public partial class VendorApplicationServiceClass : IVendorApplicationServiceClass
    {

        public async Task InterfaceChangedEvent(XFS4IoT.VendorApplication.Events.InterfaceChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorApplication.Events.InterfaceChangedEvent(Payload));

        public async Task VendorAppExitedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorApplication.Events.VendorAppExitedEvent());

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IVendorApplicationDevice Device { get => ServiceProvider.Device.IsA<IVendorApplicationDevice>(); }
    }
}
