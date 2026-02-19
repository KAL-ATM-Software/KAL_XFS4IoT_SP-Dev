/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * DepositServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Deposit;

namespace XFS4IoTServer
{
    public partial class DepositServiceClass : IDepositServiceClass
    {

        public async Task EnvTakenEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.EnvTakenEvent());

        public async Task EnvDepositedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.EnvDepositedEvent());

        public async Task DepositErrorEvent(XFS4IoT.Deposit.Events.DepositErrorEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.DepositErrorEvent(Payload));

        public async Task EnvInsertedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.EnvInsertedEvent());

        public async Task InsertDepositEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.InsertDepositEvent());

        public async Task MediaDetectedEvent(XFS4IoT.Deposit.Events.MediaDetectedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.MediaDetectedEvent(Payload));

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Deposit.Commands.DispenseCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Deposit.DispenseHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DispenseHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Deposit.Commands.EntryCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Deposit.EntryHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, EntryHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Deposit.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Deposit.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Deposit.Commands.RetractCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Deposit.RetractHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, RetractHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Deposit.Commands.SupplyReplenishCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Deposit.SupplyReplenishHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SupplyReplenishHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Deposit.Dispense", typeof(XFS4IoT.Deposit.Commands.DispenseCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Deposit.Entry", typeof(XFS4IoT.Deposit.Commands.EntryCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Deposit.Reset", typeof(XFS4IoT.Deposit.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Deposit.Retract", typeof(XFS4IoT.Deposit.Commands.RetractCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Deposit.SupplyReplenish", typeof(XFS4IoT.Deposit.Commands.SupplyReplenishCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Deposit.Dispense", typeof(XFS4IoT.Deposit.Completions.DispenseCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Deposit.Entry", typeof(XFS4IoT.Deposit.Completions.EntryCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Deposit.Reset", typeof(XFS4IoT.Deposit.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Deposit.Retract", typeof(XFS4IoT.Deposit.Completions.RetractCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Deposit.SupplyReplenish", typeof(XFS4IoT.Deposit.Completions.SupplyReplenishCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Deposit.DepositErrorEvent", typeof(XFS4IoT.Deposit.Events.DepositErrorEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Deposit.EnvDepositedEvent", typeof(XFS4IoT.Deposit.Events.EnvDepositedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Deposit.EnvInsertedEvent", typeof(XFS4IoT.Deposit.Events.EnvInsertedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Deposit.EnvTakenEvent", typeof(XFS4IoT.Deposit.Events.EnvTakenEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Deposit.InsertDepositEvent", typeof(XFS4IoT.Deposit.Events.InsertDepositEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Deposit.MediaDetectedEvent", typeof(XFS4IoT.Deposit.Events.MediaDetectedEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IDepositDevice Device { get => ServiceProvider.Device.IsA<IDepositDevice>(); }
    }
}
