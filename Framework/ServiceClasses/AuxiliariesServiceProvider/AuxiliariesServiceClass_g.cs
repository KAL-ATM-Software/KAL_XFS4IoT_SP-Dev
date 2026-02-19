/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * AuxiliariesServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Auxiliaries;

namespace XFS4IoTServer
{
    public partial class AuxiliariesServiceClass : IAuxiliariesServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Auxiliaries.Commands.ClearAutoStartupTimeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Auxiliaries.ClearAutoStartupTimeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ClearAutoStartupTimeHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Auxiliaries.Commands.GetAutoStartupTimeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Auxiliaries.GetAutoStartupTimeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetAutoStartupTimeHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Auxiliaries.Commands.SetAutoStartupTimeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Auxiliaries.SetAutoStartupTimeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetAutoStartupTimeHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Auxiliaries.Commands.SetAuxiliariesCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Auxiliaries.SetAuxiliariesHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetAuxiliariesHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Auxiliaries.ClearAutoStartupTime", typeof(XFS4IoT.Auxiliaries.Commands.ClearAutoStartupTimeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Auxiliaries.GetAutoStartupTime", typeof(XFS4IoT.Auxiliaries.Commands.GetAutoStartupTimeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Auxiliaries.Register", typeof(XFS4IoT.Auxiliaries.Commands.RegisterCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Auxiliaries.SetAutoStartupTime", typeof(XFS4IoT.Auxiliaries.Commands.SetAutoStartupTimeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Auxiliaries.SetAuxiliaries", typeof(XFS4IoT.Auxiliaries.Commands.SetAuxiliariesCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Auxiliaries.ClearAutoStartupTime", typeof(XFS4IoT.Auxiliaries.Completions.ClearAutoStartupTimeCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Auxiliaries.GetAutoStartupTime", typeof(XFS4IoT.Auxiliaries.Completions.GetAutoStartupTimeCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Auxiliaries.Register", typeof(XFS4IoT.Auxiliaries.Completions.RegisterCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Auxiliaries.SetAutoStartupTime", typeof(XFS4IoT.Auxiliaries.Completions.SetAutoStartupTimeCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Auxiliaries.SetAuxiliaries", typeof(XFS4IoT.Auxiliaries.Completions.SetAuxiliariesCompletion));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IAuxiliariesDevice Device { get => ServiceProvider.Device.IsA<IAuxiliariesDevice>(); }
    }
}
