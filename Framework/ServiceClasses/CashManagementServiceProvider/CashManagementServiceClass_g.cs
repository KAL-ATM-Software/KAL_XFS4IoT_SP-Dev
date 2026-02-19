/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CashManagementServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTServer
{
    public partial class CashManagementServiceClass : ICashManagementServiceClass
    {

        public async Task TellerInfoChangedEvent(XFS4IoT.CashManagement.Events.TellerInfoChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.TellerInfoChangedEvent(Payload));

        public async Task ItemsTakenEvent(XFS4IoT.CashManagement.Events.ItemsTakenEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.ItemsTakenEvent(Payload));

        public async Task ItemsInsertedEvent(XFS4IoT.CashManagement.Events.ItemsInsertedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.ItemsInsertedEvent(Payload));

        public async Task ItemsPresentedEvent(XFS4IoT.CashManagement.Events.ItemsPresentedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.ItemsPresentedEvent(Payload));

        public async Task MediaDetectedEvent(XFS4IoT.CashManagement.Events.MediaDetectedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.MediaDetectedEvent(Payload));

        public async Task ShutterStatusChangedEvent(XFS4IoT.CashManagement.Events.ShutterStatusChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.ShutterStatusChangedEvent(Payload));

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.CalibrateCashUnitCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.CalibrateCashUnitHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CalibrateCashUnitHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.CloseShutterCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.CloseShutterHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CloseShutterHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.GetBankNoteTypesCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.GetBankNoteTypesHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetBankNoteTypesHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.GetClassificationListCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.GetClassificationListHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetClassificationListHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.GetItemInfoCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.GetItemInfoHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetItemInfoHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.GetTellerInfoCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.GetTellerInfoHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetTellerInfoHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.OpenShutterCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.OpenShutterHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, OpenShutterHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.RetractCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.RetractHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, RetractHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.SetClassificationListCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.SetClassificationListHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetClassificationListHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashManagement.Commands.SetTellerInfoCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashManagement.SetTellerInfoHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetTellerInfoHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.CalibrateCashUnit", typeof(XFS4IoT.CashManagement.Commands.CalibrateCashUnitCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.CloseShutter", typeof(XFS4IoT.CashManagement.Commands.CloseShutterCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.GetBankNoteTypes", typeof(XFS4IoT.CashManagement.Commands.GetBankNoteTypesCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.GetClassificationList", typeof(XFS4IoT.CashManagement.Commands.GetClassificationListCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.GetItemInfo", typeof(XFS4IoT.CashManagement.Commands.GetItemInfoCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.GetTellerInfo", typeof(XFS4IoT.CashManagement.Commands.GetTellerInfoCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.OpenShutter", typeof(XFS4IoT.CashManagement.Commands.OpenShutterCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.Reset", typeof(XFS4IoT.CashManagement.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.Retract", typeof(XFS4IoT.CashManagement.Commands.RetractCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.SetClassificationList", typeof(XFS4IoT.CashManagement.Commands.SetClassificationListCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashManagement.SetTellerInfo", typeof(XFS4IoT.CashManagement.Commands.SetTellerInfoCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.CalibrateCashUnit", typeof(XFS4IoT.CashManagement.Completions.CalibrateCashUnitCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.CloseShutter", typeof(XFS4IoT.CashManagement.Completions.CloseShutterCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.GetBankNoteTypes", typeof(XFS4IoT.CashManagement.Completions.GetBankNoteTypesCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.GetClassificationList", typeof(XFS4IoT.CashManagement.Completions.GetClassificationListCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.GetItemInfo", typeof(XFS4IoT.CashManagement.Completions.GetItemInfoCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.GetTellerInfo", typeof(XFS4IoT.CashManagement.Completions.GetTellerInfoCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.OpenShutter", typeof(XFS4IoT.CashManagement.Completions.OpenShutterCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.Reset", typeof(XFS4IoT.CashManagement.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.Retract", typeof(XFS4IoT.CashManagement.Completions.RetractCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.SetClassificationList", typeof(XFS4IoT.CashManagement.Completions.SetClassificationListCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashManagement.SetTellerInfo", typeof(XFS4IoT.CashManagement.Completions.SetTellerInfoCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashManagement.IncompleteRetractEvent", typeof(XFS4IoT.CashManagement.Events.IncompleteRetractEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashManagement.InfoAvailableEvent", typeof(XFS4IoT.CashManagement.Events.InfoAvailableEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashManagement.ItemsInsertedEvent", typeof(XFS4IoT.CashManagement.Events.ItemsInsertedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashManagement.ItemsPresentedEvent", typeof(XFS4IoT.CashManagement.Events.ItemsPresentedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashManagement.ItemsTakenEvent", typeof(XFS4IoT.CashManagement.Events.ItemsTakenEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashManagement.MediaDetectedEvent", typeof(XFS4IoT.CashManagement.Events.MediaDetectedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashManagement.NoteErrorEvent", typeof(XFS4IoT.CashManagement.Events.NoteErrorEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashManagement.ShutterStatusChangedEvent", typeof(XFS4IoT.CashManagement.Events.ShutterStatusChangedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashManagement.TellerInfoChangedEvent", typeof(XFS4IoT.CashManagement.Events.TellerInfoChangedEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICashManagementDevice Device { get => ServiceProvider.Device.IsA<ICashManagementDevice>(); }
    }
}
