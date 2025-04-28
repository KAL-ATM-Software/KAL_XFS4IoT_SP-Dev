/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * GermanServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.German;

namespace XFS4IoTServer
{
    public partial class GermanServiceClass : IGermanServiceClass
    {

        public async Task HSMTDataChangedEvent(XFS4IoT.German.Events.HSMTDataChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.German.Events.HSMTDataChangedEvent(Payload));

        public async Task OPTRequiredEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.German.Events.OPTRequiredEvent());

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IGermanDevice Device { get => ServiceProvider.Device.IsA<IGermanDevice>(); }
    }
}
