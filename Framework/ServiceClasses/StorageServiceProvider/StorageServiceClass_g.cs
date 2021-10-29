/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * StorageServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    public partial class StorageServiceClass : IStorageServiceClass
    {
        public StorageServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(StorageServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IStorageDevice>();
        }
        public async Task StorageThresholdEvent(XFS4IoT.Storage.Events.StorageThresholdEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Storage.Events.StorageThresholdEvent(Payload));

        public async Task StorageChangedEvent(XFS4IoT.Storage.Events.StorageChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Storage.Events.StorageChangedEvent(Payload));

        public async Task StorageErrorEvent(XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Storage.Events.StorageErrorEvent(Payload));

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private IStorageDevice Device { get => ServiceProvider.Device.IsA<IStorageDevice>(); }
    }
}
