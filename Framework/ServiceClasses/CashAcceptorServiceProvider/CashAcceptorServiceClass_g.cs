/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashAcceptorServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.CashAcceptor;

namespace XFS4IoTServer
{
    public partial class CashAcceptorServiceClass : ICashAcceptorServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.CashInCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.CashInHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CashInHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.CashInEndCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.CashInEndHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CashInEndHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.CashInRollbackCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.CashInRollbackHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CashInRollbackHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.CashInStartCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.CashInStartHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CashInStartHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.CashUnitCountCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.CashUnitCountHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CashUnitCountHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.CompareSignatureCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.CompareSignatureHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CompareSignatureHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.ConfigureNoteReaderCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.ConfigureNoteReaderHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ConfigureNoteReaderHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.ConfigureNoteTypesCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.ConfigureNoteTypesHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ConfigureNoteTypesHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.CreateSignatureCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.CreateSignatureHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CreateSignatureHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.DepleteCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.DepleteHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DepleteHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.DeviceLockControlCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.DeviceLockControlHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DeviceLockControlHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.GetCashInStatusCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.GetCashInStatusHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetCashInStatusHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.GetDepleteSourceCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.GetDepleteSourceHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetDepleteSourceHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.GetDeviceLockStatusCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.GetDeviceLockStatusHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetDeviceLockStatusHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.GetPresentStatusCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.GetPresentStatusHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetPresentStatusHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.GetReplenishTargetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.GetReplenishTargetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetReplenishTargetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.PreparePresentCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.PreparePresentHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PreparePresentHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.PresentMediaCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.PresentMediaHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, PresentMediaHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CashAcceptor.Commands.ReplenishCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CashAcceptor.ReplenishHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ReplenishHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.CashIn", typeof(XFS4IoT.CashAcceptor.Commands.CashInCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.CashInEnd", typeof(XFS4IoT.CashAcceptor.Commands.CashInEndCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.CashInRollback", typeof(XFS4IoT.CashAcceptor.Commands.CashInRollbackCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.CashInStart", typeof(XFS4IoT.CashAcceptor.Commands.CashInStartCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.CashUnitCount", typeof(XFS4IoT.CashAcceptor.Commands.CashUnitCountCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.CompareSignature", typeof(XFS4IoT.CashAcceptor.Commands.CompareSignatureCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.ConfigureNoteReader", typeof(XFS4IoT.CashAcceptor.Commands.ConfigureNoteReaderCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.ConfigureNoteTypes", typeof(XFS4IoT.CashAcceptor.Commands.ConfigureNoteTypesCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.CreateSignature", typeof(XFS4IoT.CashAcceptor.Commands.CreateSignatureCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.Deplete", typeof(XFS4IoT.CashAcceptor.Commands.DepleteCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.DeviceLockControl", typeof(XFS4IoT.CashAcceptor.Commands.DeviceLockControlCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.GetCashInStatus", typeof(XFS4IoT.CashAcceptor.Commands.GetCashInStatusCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.GetDepleteSource", typeof(XFS4IoT.CashAcceptor.Commands.GetDepleteSourceCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.GetDeviceLockStatus", typeof(XFS4IoT.CashAcceptor.Commands.GetDeviceLockStatusCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.GetPresentStatus", typeof(XFS4IoT.CashAcceptor.Commands.GetPresentStatusCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.GetReplenishTarget", typeof(XFS4IoT.CashAcceptor.Commands.GetReplenishTargetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.PreparePresent", typeof(XFS4IoT.CashAcceptor.Commands.PreparePresentCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.PresentMedia", typeof(XFS4IoT.CashAcceptor.Commands.PresentMediaCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CashAcceptor.Replenish", typeof(XFS4IoT.CashAcceptor.Commands.ReplenishCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.CashIn", typeof(XFS4IoT.CashAcceptor.Completions.CashInCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.CashInEnd", typeof(XFS4IoT.CashAcceptor.Completions.CashInEndCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.CashInRollback", typeof(XFS4IoT.CashAcceptor.Completions.CashInRollbackCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.CashInStart", typeof(XFS4IoT.CashAcceptor.Completions.CashInStartCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.CashUnitCount", typeof(XFS4IoT.CashAcceptor.Completions.CashUnitCountCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.CompareSignature", typeof(XFS4IoT.CashAcceptor.Completions.CompareSignatureCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.ConfigureNoteReader", typeof(XFS4IoT.CashAcceptor.Completions.ConfigureNoteReaderCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.ConfigureNoteTypes", typeof(XFS4IoT.CashAcceptor.Completions.ConfigureNoteTypesCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.CreateSignature", typeof(XFS4IoT.CashAcceptor.Completions.CreateSignatureCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.Deplete", typeof(XFS4IoT.CashAcceptor.Completions.DepleteCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.DeviceLockControl", typeof(XFS4IoT.CashAcceptor.Completions.DeviceLockControlCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.GetCashInStatus", typeof(XFS4IoT.CashAcceptor.Completions.GetCashInStatusCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.GetDepleteSource", typeof(XFS4IoT.CashAcceptor.Completions.GetDepleteSourceCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.GetDeviceLockStatus", typeof(XFS4IoT.CashAcceptor.Completions.GetDeviceLockStatusCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.GetPresentStatus", typeof(XFS4IoT.CashAcceptor.Completions.GetPresentStatusCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.GetReplenishTarget", typeof(XFS4IoT.CashAcceptor.Completions.GetReplenishTargetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.PreparePresent", typeof(XFS4IoT.CashAcceptor.Completions.PreparePresentCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.PresentMedia", typeof(XFS4IoT.CashAcceptor.Completions.PresentMediaCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CashAcceptor.Replenish", typeof(XFS4IoT.CashAcceptor.Completions.ReplenishCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashAcceptor.IncompleteDepleteEvent", typeof(XFS4IoT.CashAcceptor.Events.IncompleteDepleteEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashAcceptor.IncompleteReplenishEvent", typeof(XFS4IoT.CashAcceptor.Events.IncompleteReplenishEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashAcceptor.InputRefuseEvent", typeof(XFS4IoT.CashAcceptor.Events.InputRefuseEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashAcceptor.InsertItemsEvent", typeof(XFS4IoT.CashAcceptor.Events.InsertItemsEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CashAcceptor.SubCashInEvent", typeof(XFS4IoT.CashAcceptor.Events.SubCashInEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICashAcceptorDevice Device { get => ServiceProvider.Device.IsA<ICashAcceptorDevice>(); }
    }
}
