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

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.AcceptItemCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.AcceptItemHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, AcceptItemHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.ActionItemCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.ActionItemHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ActionItemHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.ExpelMediaCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.ExpelMediaHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ExpelMediaHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.GetNextItemCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.GetNextItemHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetNextItemHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.GetTransactionStatusCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.GetTransactionStatusHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetTransactionStatusHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.MediaInCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.MediaInHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, MediaInHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.MediaInEndCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.MediaInEndHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, MediaInEndHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.MediaInRollbackCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.MediaInRollbackHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, MediaInRollbackHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.PresentMediaCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.PresentMediaHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PresentMediaHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.ReadImageCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.ReadImageHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ReadImageHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.RetractMediaCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.RetractMediaHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, RetractMediaHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.SetMediaParametersCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.SetMediaParametersHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetMediaParametersHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Check.Commands.SupplyReplenishCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Check.SupplyReplenishHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SupplyReplenishHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.AcceptItem", typeof(XFS4IoT.Check.Commands.AcceptItemCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.ActionItem", typeof(XFS4IoT.Check.Commands.ActionItemCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.ExpelMedia", typeof(XFS4IoT.Check.Commands.ExpelMediaCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.GetNextItem", typeof(XFS4IoT.Check.Commands.GetNextItemCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.GetTransactionStatus", typeof(XFS4IoT.Check.Commands.GetTransactionStatusCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.MediaIn", typeof(XFS4IoT.Check.Commands.MediaInCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.MediaInEnd", typeof(XFS4IoT.Check.Commands.MediaInEndCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.MediaInRollback", typeof(XFS4IoT.Check.Commands.MediaInRollbackCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.PresentMedia", typeof(XFS4IoT.Check.Commands.PresentMediaCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.ReadImage", typeof(XFS4IoT.Check.Commands.ReadImageCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.Reset", typeof(XFS4IoT.Check.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.RetractMedia", typeof(XFS4IoT.Check.Commands.RetractMediaCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.SetMediaParameters", typeof(XFS4IoT.Check.Commands.SetMediaParametersCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Check.SupplyReplenish", typeof(XFS4IoT.Check.Commands.SupplyReplenishCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.AcceptItem", typeof(XFS4IoT.Check.Completions.AcceptItemCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.ActionItem", typeof(XFS4IoT.Check.Completions.ActionItemCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.ExpelMedia", typeof(XFS4IoT.Check.Completions.ExpelMediaCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.GetNextItem", typeof(XFS4IoT.Check.Completions.GetNextItemCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.GetTransactionStatus", typeof(XFS4IoT.Check.Completions.GetTransactionStatusCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.MediaIn", typeof(XFS4IoT.Check.Completions.MediaInCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.MediaInEnd", typeof(XFS4IoT.Check.Completions.MediaInEndCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.MediaInRollback", typeof(XFS4IoT.Check.Completions.MediaInRollbackCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.PresentMedia", typeof(XFS4IoT.Check.Completions.PresentMediaCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.ReadImage", typeof(XFS4IoT.Check.Completions.ReadImageCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.Reset", typeof(XFS4IoT.Check.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.RetractMedia", typeof(XFS4IoT.Check.Completions.RetractMediaCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.SetMediaParameters", typeof(XFS4IoT.Check.Completions.SetMediaParametersCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Check.SupplyReplenish", typeof(XFS4IoT.Check.Completions.SupplyReplenishCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Check.MediaDataEvent", typeof(XFS4IoT.Check.Events.MediaDataEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Check.MediaDetectedEvent", typeof(XFS4IoT.Check.Events.MediaDetectedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Check.MediaInsertedEvent", typeof(XFS4IoT.Check.Events.MediaInsertedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Check.MediaPresentedEvent", typeof(XFS4IoT.Check.Events.MediaPresentedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Check.MediaRefusedEvent", typeof(XFS4IoT.Check.Events.MediaRefusedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Check.MediaRejectedEvent", typeof(XFS4IoT.Check.Events.MediaRejectedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Check.MediaTakenEvent", typeof(XFS4IoT.Check.Events.MediaTakenEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Check.NoMediaEvent", typeof(XFS4IoT.Check.Events.NoMediaEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Check.ShutterStatusChangedEvent", typeof(XFS4IoT.Check.Events.ShutterStatusChangedEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICheckDevice Device { get => ServiceProvider.Device.IsA<ICheckDevice>(); }
    }
}
