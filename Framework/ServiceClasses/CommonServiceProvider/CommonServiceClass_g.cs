/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * CommonServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public partial class CommonServiceClass : ICommonServiceClass
    {

        public async Task StatusChangedEvent(XFS4IoT.Common.Events.StatusChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.StatusChangedEvent(Payload));

        public async Task ErrorEvent(XFS4IoT.Common.Events.ErrorEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.ErrorEvent(Payload));

        public async Task NonceClearedEvent(XFS4IoT.Common.Events.NonceClearedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.NonceClearedEvent(Payload));

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICommonDevice Device { get => ServiceProvider.Device.IsA<ICommonDevice>(); }
    }
}
