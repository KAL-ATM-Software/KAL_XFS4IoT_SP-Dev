﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * StorageServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    public partial class StorageServiceClass : IStorageServiceClass
    {

        public async Task CountsChangedEvent(XFS4IoT.Storage.Events.CountsChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Storage.Events.CountsChangedEvent(Payload));

        public async Task StorageChangedEvent(XFS4IoT.Storage.Events.StorageChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Storage.Events.StorageChangedEvent(Payload));

        public async Task StorageThresholdEvent(XFS4IoT.Storage.Events.StorageThresholdEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Storage.Events.StorageThresholdEvent(Payload));

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IStorageDevice Device { get => ServiceProvider.Device.IsA<IStorageDevice>(); }
    }
}
