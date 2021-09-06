/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * PinPadServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.PinPad;

namespace XFS4IoTServer
{
    public partial class PinPadServiceClass : IPinPadServiceClass
    {
        public PinPadServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(PinPadServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IPinPadDevice>();
        }
        public async Task IllegalKeyAccessEvent(XFS4IoT.PinPad.Events.IllegalKeyAccessEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.PinPad.Events.IllegalKeyAccessEvent(Payload));

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private IPinPadDevice Device { get => ServiceProvider.Device.IsA<IPinPadDevice>(); }
    }
}
