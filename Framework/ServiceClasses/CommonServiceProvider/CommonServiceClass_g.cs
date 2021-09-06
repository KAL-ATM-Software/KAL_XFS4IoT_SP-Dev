/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * CommonServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public partial class CommonServiceClass : ICommonServiceClass
    {
        public CommonServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CommonServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICommonDevice>();
        }
        public async Task PowerSaveChangeEvent(XFS4IoT.Common.Events.PowerSaveChangeEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.PowerSaveChangeEvent(Payload));

        public async Task DevicePositionEvent(XFS4IoT.Common.Events.DevicePositionEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.DevicePositionEvent(Payload));

        public async Task NonceClearedEvent(XFS4IoT.Common.Events.NonceClearedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.NonceClearedEvent(Payload));

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private ICommonDevice Device { get => ServiceProvider.Device.IsA<ICommonDevice>(); }
    }
}
