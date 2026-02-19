/***********************************************************************************************\
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

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Storage.Commands.EndExchangeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Storage.EndExchangeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, EndExchangeHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Storage.Commands.GetStorageCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Storage.GetStorageHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetStorageHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Storage.Commands.SetStorageCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Storage.SetStorageHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetStorageHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Storage.Commands.StartExchangeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Storage.StartExchangeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, StartExchangeHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Storage.EndExchange", typeof(XFS4IoT.Storage.Commands.EndExchangeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Storage.GetStorage", typeof(XFS4IoT.Storage.Commands.GetStorageCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Storage.SetStorage", typeof(XFS4IoT.Storage.Commands.SetStorageCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Storage.StartExchange", typeof(XFS4IoT.Storage.Commands.StartExchangeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Storage.EndExchange", typeof(XFS4IoT.Storage.Completions.EndExchangeCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Storage.GetStorage", typeof(XFS4IoT.Storage.Completions.GetStorageCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Storage.SetStorage", typeof(XFS4IoT.Storage.Completions.SetStorageCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Storage.StartExchange", typeof(XFS4IoT.Storage.Completions.StartExchangeCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Storage.CountsChangedEvent", typeof(XFS4IoT.Storage.Events.CountsChangedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Storage.StorageChangedEvent", typeof(XFS4IoT.Storage.Events.StorageChangedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Storage.StorageErrorEvent", typeof(XFS4IoT.Storage.Events.StorageErrorEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Storage.StorageThresholdEvent", typeof(XFS4IoT.Storage.Events.StorageThresholdEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IStorageDevice Device { get => ServiceProvider.Device.IsA<IStorageDevice>(); }
    }
}
