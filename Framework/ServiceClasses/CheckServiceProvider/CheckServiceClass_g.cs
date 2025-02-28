/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * CheckServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Check;

namespace XFS4IoTServer
{
    public partial class CheckServiceClass : ICheckServiceClass
    {

        public async Task MediaTakenEvent(XFS4IoT.Check.Events.MediaTakenEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Check.Events.MediaTakenEvent(Payload));

        public async Task MediaDetectedEvent(XFS4IoT.Check.Events.MediaDetectedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Check.Events.MediaDetectedEvent(Payload));

        public async Task ShutterStatusChangedEvent(XFS4IoT.Check.Events.ShutterStatusChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Check.Events.ShutterStatusChangedEvent(Payload));

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICheckDevice Device { get => ServiceProvider.Device.IsA<ICheckDevice>(); }
    }
}
