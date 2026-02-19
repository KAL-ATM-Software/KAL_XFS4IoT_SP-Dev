/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * VendorApplicationServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.VendorApplication;

namespace XFS4IoTServer
{
    public partial class VendorApplicationServiceClass : IVendorApplicationServiceClass
    {

        public async Task VendorAppExitedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorApplication.Events.VendorAppExitedEvent());

        public async Task InterfaceChangedEvent(XFS4IoT.VendorApplication.Events.InterfaceChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.VendorApplication.Events.InterfaceChangedEvent(Payload));

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.VendorApplication.Commands.GetActiveInterfaceCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.VendorApplication.GetActiveInterfaceHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetActiveInterfaceHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.VendorApplication.Commands.SetActiveInterfaceCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.VendorApplication.SetActiveInterfaceHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetActiveInterfaceHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.VendorApplication.Commands.StartLocalApplicationCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.VendorApplication.StartLocalApplicationHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, StartLocalApplicationHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "VendorApplication.GetActiveInterface", typeof(XFS4IoT.VendorApplication.Commands.GetActiveInterfaceCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "VendorApplication.SetActiveInterface", typeof(XFS4IoT.VendorApplication.Commands.SetActiveInterfaceCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "VendorApplication.StartLocalApplication", typeof(XFS4IoT.VendorApplication.Commands.StartLocalApplicationCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "VendorApplication.GetActiveInterface", typeof(XFS4IoT.VendorApplication.Completions.GetActiveInterfaceCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "VendorApplication.SetActiveInterface", typeof(XFS4IoT.VendorApplication.Completions.SetActiveInterfaceCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "VendorApplication.StartLocalApplication", typeof(XFS4IoT.VendorApplication.Completions.StartLocalApplicationCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "VendorApplication.InterfaceChangedEvent", typeof(XFS4IoT.VendorApplication.Events.InterfaceChangedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "VendorApplication.VendorAppExitedEvent", typeof(XFS4IoT.VendorApplication.Events.VendorAppExitedEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IVendorApplicationDevice Device { get => ServiceProvider.Device.IsA<IVendorApplicationDevice>(); }
    }
}
