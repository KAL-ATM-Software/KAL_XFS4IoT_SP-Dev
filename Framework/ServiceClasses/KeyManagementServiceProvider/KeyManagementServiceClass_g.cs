/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * KeyManagementServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTServer
{
    public partial class KeyManagementServiceClass : IKeyManagementServiceClass
    {

        public async Task InitializedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.KeyManagement.Events.InitializedEvent());

        public async Task IllegalKeyAccessEvent(XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent(Payload));

        public async Task CertificateChangeEvent(XFS4IoT.KeyManagement.Events.CertificateChangeEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.KeyManagement.Events.CertificateChangeEvent(Payload));

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IKeyManagementDevice Device { get => ServiceProvider.Device.IsA<IKeyManagementDevice>(); }
    }
}
