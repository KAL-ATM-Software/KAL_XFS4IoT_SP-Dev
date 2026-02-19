/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * BiometricServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Biometric;

namespace XFS4IoTServer
{
    public partial class BiometricServiceClass : IBiometricServiceClass
    {

        public async Task SubjectRemovedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Biometric.Events.SubjectRemovedEvent());

        public async Task DataClearedEvent(XFS4IoT.Biometric.Events.DataClearedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Biometric.Events.DataClearedEvent(Payload));

        public async Task OrientationEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Biometric.Events.OrientationEvent());

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Biometric.Commands.ClearCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Biometric.ClearHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ClearHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Biometric.Commands.GetStorageInfoCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Biometric.GetStorageInfoHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetStorageInfoHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Biometric.Commands.ImportCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Biometric.ImportHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ImportHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Biometric.Commands.MatchCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Biometric.MatchHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, MatchHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Biometric.Commands.ReadCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Biometric.ReadHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ReadHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Biometric.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Biometric.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Biometric.Commands.SetDataPersistenceCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Biometric.SetDataPersistenceHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetDataPersistenceHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Biometric.Commands.SetMatchCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Biometric.SetMatchHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, SetMatchHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Biometric.Clear", typeof(XFS4IoT.Biometric.Commands.ClearCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Biometric.GetStorageInfo", typeof(XFS4IoT.Biometric.Commands.GetStorageInfoCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Biometric.Import", typeof(XFS4IoT.Biometric.Commands.ImportCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Biometric.Match", typeof(XFS4IoT.Biometric.Commands.MatchCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Biometric.Read", typeof(XFS4IoT.Biometric.Commands.ReadCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Biometric.Reset", typeof(XFS4IoT.Biometric.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Biometric.SetDataPersistence", typeof(XFS4IoT.Biometric.Commands.SetDataPersistenceCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Biometric.SetMatch", typeof(XFS4IoT.Biometric.Commands.SetMatchCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Biometric.Clear", typeof(XFS4IoT.Biometric.Completions.ClearCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Biometric.GetStorageInfo", typeof(XFS4IoT.Biometric.Completions.GetStorageInfoCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Biometric.Import", typeof(XFS4IoT.Biometric.Completions.ImportCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Biometric.Match", typeof(XFS4IoT.Biometric.Completions.MatchCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Biometric.Read", typeof(XFS4IoT.Biometric.Completions.ReadCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Biometric.Reset", typeof(XFS4IoT.Biometric.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Biometric.SetDataPersistence", typeof(XFS4IoT.Biometric.Completions.SetDataPersistenceCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Biometric.SetMatch", typeof(XFS4IoT.Biometric.Completions.SetMatchCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Biometric.DataClearedEvent", typeof(XFS4IoT.Biometric.Events.DataClearedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Biometric.OrientationEvent", typeof(XFS4IoT.Biometric.Events.OrientationEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Biometric.PresentSubjectEvent", typeof(XFS4IoT.Biometric.Events.PresentSubjectEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Biometric.RemoveSubjectEvent", typeof(XFS4IoT.Biometric.Events.RemoveSubjectEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Biometric.SubjectDetectedEvent", typeof(XFS4IoT.Biometric.Events.SubjectDetectedEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "Biometric.SubjectRemovedEvent", typeof(XFS4IoT.Biometric.Events.SubjectRemovedEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IBiometricDevice Device { get => ServiceProvider.Device.IsA<IBiometricDevice>(); }
    }
}
