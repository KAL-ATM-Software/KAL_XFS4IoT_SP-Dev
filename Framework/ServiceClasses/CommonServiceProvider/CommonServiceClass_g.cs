/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * CommonServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public partial class CommonServiceClass : ICommonServiceClass
    {

        public async Task StatusChangedEvent(XFS4IoT.Common.Events.StatusChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.StatusChangedEvent(Payload));

        public async Task ErrorEvent(XFS4IoT.Common.Events.ErrorEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.ErrorEvent(Payload));

        public async Task NonceClearedEvent(XFS4IoT.Common.Events.NonceClearedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Common.Events.NonceClearedEvent(Payload));

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Common.Commands.CapabilitiesCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Common.CapabilitiesHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CapabilitiesHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Common.Commands.ClearCommandNonceCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Common.ClearCommandNonceHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ClearCommandNonceHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Common.Commands.GetCommandNonceCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Common.GetCommandNonceHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetCommandNonceHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Common.Commands.GetTransactionStateCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Common.GetTransactionStateHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetTransactionStateHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Common.Commands.PowerSaveControlCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Common.PowerSaveControlHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PowerSaveControlHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Common.Commands.SetTransactionStateCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Common.SetTransactionStateHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetTransactionStateHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Common.Commands.SetVersionsCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Common.SetVersionsHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetVersionsHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Common.Commands.StatusCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Common.StatusHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, StatusHandler"), logger), true);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Common.Cancel", typeof(XFS4IoT.Common.Commands.CancelCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Common.Capabilities", typeof(XFS4IoT.Common.Commands.CapabilitiesCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Common.ClearCommandNonce", typeof(XFS4IoT.Common.Commands.ClearCommandNonceCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Common.GetCommandNonce", typeof(XFS4IoT.Common.Commands.GetCommandNonceCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Common.GetTransactionState", typeof(XFS4IoT.Common.Commands.GetTransactionStateCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Common.PowerSaveControl", typeof(XFS4IoT.Common.Commands.PowerSaveControlCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Common.SetTransactionState", typeof(XFS4IoT.Common.Commands.SetTransactionStateCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Common.SetVersions", typeof(XFS4IoT.Common.Commands.SetVersionsCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Common.Status", typeof(XFS4IoT.Common.Commands.StatusCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Common.Cancel", typeof(XFS4IoT.Common.Completions.CancelCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Common.Capabilities", typeof(XFS4IoT.Common.Completions.CapabilitiesCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Common.ClearCommandNonce", typeof(XFS4IoT.Common.Completions.ClearCommandNonceCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Common.GetCommandNonce", typeof(XFS4IoT.Common.Completions.GetCommandNonceCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Common.GetTransactionState", typeof(XFS4IoT.Common.Completions.GetTransactionStateCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Common.PowerSaveControl", typeof(XFS4IoT.Common.Completions.PowerSaveControlCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Common.SetTransactionState", typeof(XFS4IoT.Common.Completions.SetTransactionStateCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Common.SetVersions", typeof(XFS4IoT.Common.Completions.SetVersionsCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Common.Status", typeof(XFS4IoT.Common.Completions.StatusCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Common.ErrorEvent", typeof(XFS4IoT.Common.Events.ErrorEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Common.NonceClearedEvent", typeof(XFS4IoT.Common.Events.NonceClearedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Common.StatusChangedEvent", typeof(XFS4IoT.Common.Events.StatusChangedEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICommonDevice Device { get => ServiceProvider.Device.IsA<ICommonDevice>(); }
    }
}
