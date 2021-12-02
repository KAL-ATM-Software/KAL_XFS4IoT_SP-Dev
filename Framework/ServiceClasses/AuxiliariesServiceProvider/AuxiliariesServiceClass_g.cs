/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * AuxiliariesServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Auxiliaries;

namespace XFS4IoTServer
{
    public partial class AuxiliariesServiceClass : IAuxiliariesServiceClass
    {
        public AuxiliariesServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(AuxiliariesServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IAuxiliariesDevice>();
        }
        public async Task AuxiliaryStatusEvent(XFS4IoT.Auxiliaries.Events.AuxiliaryStatusEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Auxiliaries.Events.AuxiliaryStatusEvent(Payload));

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private IAuxiliariesDevice Device { get => ServiceProvider.Device.IsA<IAuxiliariesDevice>(); }
    }
}
