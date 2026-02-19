/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT BanknoteNeutralization interface.
 * BanknoteNeutralizationServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.BanknoteNeutralization;

namespace XFS4IoTServer
{
    public partial class BanknoteNeutralizationServiceClass : IBanknoteNeutralizationServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.BanknoteNeutralization.Commands.SetProtectionCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.BanknoteNeutralization.SetProtectionHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetProtectionHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.BanknoteNeutralization.Commands.TriggerNeutralizationCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.BanknoteNeutralization.TriggerNeutralizationHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, TriggerNeutralizationHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "BanknoteNeutralization.SetProtection", typeof(XFS4IoT.BanknoteNeutralization.Commands.SetProtectionCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "BanknoteNeutralization.TriggerNeutralization", typeof(XFS4IoT.BanknoteNeutralization.Commands.TriggerNeutralizationCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "BanknoteNeutralization.SetProtection", typeof(XFS4IoT.BanknoteNeutralization.Completions.SetProtectionCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "BanknoteNeutralization.TriggerNeutralization", typeof(XFS4IoT.BanknoteNeutralization.Completions.TriggerNeutralizationCompletion));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IBanknoteNeutralizationDevice Device { get => ServiceProvider.Device.IsA<IBanknoteNeutralizationDevice>(); }
    }
}
