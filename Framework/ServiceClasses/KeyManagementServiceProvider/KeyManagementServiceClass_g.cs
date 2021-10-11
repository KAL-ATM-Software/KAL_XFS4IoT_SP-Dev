/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * KeyManagementServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTServer
{
    public partial class KeyManagementServiceClass : IKeyManagementServiceClass
    {
        public KeyManagementServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(KeyManagementServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IKeyManagementDevice>();
        }
        public async Task InitializedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.KeyManagement.Events.InitializedEvent());

        public async Task IllegalKeyAccessEvent(XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent(Payload));

        public async Task CertificateChangeEvent(XFS4IoT.KeyManagement.Events.CertificateChangeEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.KeyManagement.Events.CertificateChangeEvent(Payload));

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private IKeyManagementDevice Device { get => ServiceProvider.Device.IsA<IKeyManagementDevice>(); }
    }
}
