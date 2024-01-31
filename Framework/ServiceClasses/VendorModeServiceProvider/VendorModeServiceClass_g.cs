/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * VendorModeServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.VendorMode;

namespace XFS4IoTServer
{
    public partial class VendorModeServiceClass : IVendorModeServiceClass
    {

        public async Task EnterModeRequestEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.EnterModeRequestEvent());

        public async Task ExitModeRequestEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.ExitModeRequestEvent());

        public async Task ModeEnteredEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.ModeEnteredEvent());

        public async Task ModeExitedEvent(XFS4IoT.VendorMode.Events.ModeExitedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.ModeExitedEvent(Payload));

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IVendorModeDevice Device { get => ServiceProvider.Device.IsA<IVendorModeDevice>(); }
    }
}
