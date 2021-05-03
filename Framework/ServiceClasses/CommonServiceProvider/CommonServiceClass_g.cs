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

namespace XFS4IoTServer
{
    public partial class CommonServiceClass : ICommonServiceClass
    {
        public CommonServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
        }
        public async Task PowerSaveChangeEvent(XFS4IoT.Common.Events.PowerSaveChangeEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.PowerSaveChangeEvent(Payload));

        public async Task DevicePositionEvent(XFS4IoT.Common.Events.DevicePositionEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.DevicePositionEvent(Payload));

        private readonly IServiceProvider ServiceProvider;
    }
}
