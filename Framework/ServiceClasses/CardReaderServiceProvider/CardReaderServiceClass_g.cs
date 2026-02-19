/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * CardReaderServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.CardReader;

namespace XFS4IoTServer
{
    public partial class CardReaderServiceClass : ICardReaderServiceClass
    {

        public async Task MediaRemovedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CardReader.Events.MediaRemovedEvent());

        public async Task CardActionEvent(XFS4IoT.CardReader.Events.CardActionEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CardReader.Events.CardActionEvent(Payload));

        public async Task MediaDetectedEvent(XFS4IoT.CardReader.Events.MediaDetectedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CardReader.Events.MediaDetectedEvent(Payload));

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.ChipIOCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.ChipIOHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ChipIOHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.ChipPowerCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.ChipPowerHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ChipPowerHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.EMVClessConfigureCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.EMVClessConfigureHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, EMVClessConfigureHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.EMVClessIssuerUpdateCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.EMVClessIssuerUpdateHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, EMVClessIssuerUpdateHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.EMVClessPerformTransactionCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.EMVClessPerformTransactionHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, EMVClessPerformTransactionHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.EMVClessQueryApplicationsCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.EMVClessQueryApplicationsHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, EMVClessQueryApplicationsHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.MoveCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.MoveHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, MoveHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.QueryIFMIdentifierCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.QueryIFMIdentifierHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, QueryIFMIdentifierHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.ReadRawDataCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.ReadRawDataHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ReadRawDataHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.SetKeyCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.SetKeyHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetKeyHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.CardReader.Commands.WriteRawDataCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.CardReader.WriteRawDataHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, WriteRawDataHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.ChipIO", typeof(XFS4IoT.CardReader.Commands.ChipIOCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.ChipPower", typeof(XFS4IoT.CardReader.Commands.ChipPowerCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.EMVClessConfigure", typeof(XFS4IoT.CardReader.Commands.EMVClessConfigureCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.EMVClessIssuerUpdate", typeof(XFS4IoT.CardReader.Commands.EMVClessIssuerUpdateCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.EMVClessPerformTransaction", typeof(XFS4IoT.CardReader.Commands.EMVClessPerformTransactionCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.EMVClessQueryApplications", typeof(XFS4IoT.CardReader.Commands.EMVClessQueryApplicationsCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.Move", typeof(XFS4IoT.CardReader.Commands.MoveCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.QueryIFMIdentifier", typeof(XFS4IoT.CardReader.Commands.QueryIFMIdentifierCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.ReadRawData", typeof(XFS4IoT.CardReader.Commands.ReadRawDataCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.Reset", typeof(XFS4IoT.CardReader.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.SetKey", typeof(XFS4IoT.CardReader.Commands.SetKeyCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "CardReader.WriteRawData", typeof(XFS4IoT.CardReader.Commands.WriteRawDataCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.ChipIO", typeof(XFS4IoT.CardReader.Completions.ChipIOCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.ChipPower", typeof(XFS4IoT.CardReader.Completions.ChipPowerCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.EMVClessConfigure", typeof(XFS4IoT.CardReader.Completions.EMVClessConfigureCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.EMVClessIssuerUpdate", typeof(XFS4IoT.CardReader.Completions.EMVClessIssuerUpdateCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.EMVClessPerformTransaction", typeof(XFS4IoT.CardReader.Completions.EMVClessPerformTransactionCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.EMVClessQueryApplications", typeof(XFS4IoT.CardReader.Completions.EMVClessQueryApplicationsCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.Move", typeof(XFS4IoT.CardReader.Completions.MoveCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.QueryIFMIdentifier", typeof(XFS4IoT.CardReader.Completions.QueryIFMIdentifierCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.ReadRawData", typeof(XFS4IoT.CardReader.Completions.ReadRawDataCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.Reset", typeof(XFS4IoT.CardReader.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.SetKey", typeof(XFS4IoT.CardReader.Completions.SetKeyCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "CardReader.WriteRawData", typeof(XFS4IoT.CardReader.Completions.WriteRawDataCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CardReader.CardActionEvent", typeof(XFS4IoT.CardReader.Events.CardActionEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CardReader.EMVClessReadStatusEvent", typeof(XFS4IoT.CardReader.Events.EMVClessReadStatusEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CardReader.InsertCardEvent", typeof(XFS4IoT.CardReader.Events.InsertCardEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CardReader.InvalidMediaEvent", typeof(XFS4IoT.CardReader.Events.InvalidMediaEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CardReader.MediaDetectedEvent", typeof(XFS4IoT.CardReader.Events.MediaDetectedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CardReader.MediaInsertedEvent", typeof(XFS4IoT.CardReader.Events.MediaInsertedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CardReader.MediaRemovedEvent", typeof(XFS4IoT.CardReader.Events.MediaRemovedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "CardReader.TrackDetectedEvent", typeof(XFS4IoT.CardReader.Events.TrackDetectedEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICardReaderDevice Device { get => ServiceProvider.Device.IsA<ICardReaderDevice>(); }
    }
}
