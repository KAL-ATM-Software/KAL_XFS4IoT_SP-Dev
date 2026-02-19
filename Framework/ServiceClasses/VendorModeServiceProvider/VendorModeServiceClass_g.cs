/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * VendorModeServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.VendorMode;

namespace XFS4IoTServer
{
    public partial class VendorModeServiceClass : IVendorModeServiceClass
    {

        public async Task EnterModeRequestEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.EnterModeRequestEvent());

        public async Task ExitModeRequestEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.ExitModeRequestEvent());

        public async Task ModeEnteredEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.ModeEnteredEvent());

        public async Task ModeExitedEvent(XFS4IoT.VendorMode.Events.ModeExitedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorMode.Events.ModeExitedEvent(Payload));

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.VendorMode.Commands.EnterModeAcknowledgeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.VendorMode.EnterModeAcknowledgeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, EnterModeAcknowledgeHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.VendorMode.Commands.EnterModeRequestCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.VendorMode.EnterModeRequestHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, EnterModeRequestHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.VendorMode.Commands.ExitModeAcknowledgeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.VendorMode.ExitModeAcknowledgeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ExitModeAcknowledgeHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.VendorMode.Commands.ExitModeRequestCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.VendorMode.ExitModeRequestHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ExitModeRequestHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.VendorMode.Commands.RegisterCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.VendorMode.RegisterHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, RegisterHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "VendorMode.EnterModeAcknowledge", typeof(XFS4IoT.VendorMode.Commands.EnterModeAcknowledgeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "VendorMode.EnterModeRequest", typeof(XFS4IoT.VendorMode.Commands.EnterModeRequestCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "VendorMode.ExitModeAcknowledge", typeof(XFS4IoT.VendorMode.Commands.ExitModeAcknowledgeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "VendorMode.ExitModeRequest", typeof(XFS4IoT.VendorMode.Commands.ExitModeRequestCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "VendorMode.Register", typeof(XFS4IoT.VendorMode.Commands.RegisterCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "VendorMode.EnterModeAcknowledge", typeof(XFS4IoT.VendorMode.Completions.EnterModeAcknowledgeCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "VendorMode.EnterModeRequest", typeof(XFS4IoT.VendorMode.Completions.EnterModeRequestCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "VendorMode.ExitModeAcknowledge", typeof(XFS4IoT.VendorMode.Completions.ExitModeAcknowledgeCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "VendorMode.ExitModeRequest", typeof(XFS4IoT.VendorMode.Completions.ExitModeRequestCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "VendorMode.Register", typeof(XFS4IoT.VendorMode.Completions.RegisterCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "VendorMode.EnterModeRequestEvent", typeof(XFS4IoT.VendorMode.Events.EnterModeRequestEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "VendorMode.ExitModeRequestEvent", typeof(XFS4IoT.VendorMode.Events.ExitModeRequestEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "VendorMode.ModeEnteredEvent", typeof(XFS4IoT.VendorMode.Events.ModeEnteredEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "VendorMode.ModeExitedEvent", typeof(XFS4IoT.VendorMode.Events.ModeExitedEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IVendorModeDevice Device { get => ServiceProvider.Device.IsA<IVendorModeDevice>(); }
    }
}
