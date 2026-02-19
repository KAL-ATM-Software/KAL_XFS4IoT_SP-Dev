/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * KeyManagementServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTServer
{
    public partial class KeyManagementServiceClass : IKeyManagementServiceClass
    {

        public async Task InitializedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.KeyManagement.Events.InitializedEvent());

        public async Task IllegalKeyAccessEvent(XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent(Payload));

        public async Task CertificateChangeEvent(XFS4IoT.KeyManagement.Events.CertificateChangeEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.KeyManagement.Events.CertificateChangeEvent(Payload));

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.DeleteKeyCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.DeleteKeyHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DeleteKeyHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.ExportRSADeviceSignedItemCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.ExportRSADeviceSignedItemHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ExportRSADeviceSignedItemHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.ExportRSAIssuerSignedItemCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.ExportRSAIssuerSignedItemHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ExportRSAIssuerSignedItemHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.GenerateKCVCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.GenerateKCVHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GenerateKCVHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.GenerateRSAKeyPairCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.GenerateRSAKeyPairHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GenerateRSAKeyPairHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.GetCertificateCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.GetCertificateHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetCertificateHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.GetKeyDetailCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.GetKeyDetailHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GetKeyDetailHandler"), logger), true);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.ImportEmvPublicKeyCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.ImportEmvPublicKeyHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ImportEmvPublicKeyHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.ImportKeyCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.ImportKeyHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ImportKeyHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.ImportKeyTokenCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.ImportKeyTokenHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ImportKeyTokenHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.InitializationCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.InitializationHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, InitializationHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.LoadCertificateCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.LoadCertificateHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, LoadCertificateHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.ReplaceCertificateCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.ReplaceCertificateHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ReplaceCertificateHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.ResetCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.ResetHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, ResetHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.StartAuthenticateCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.StartAuthenticateHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, StartAuthenticateHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.KeyManagement.Commands.StartKeyExchangeCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.KeyManagement.StartKeyExchangeHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, StartKeyExchangeHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.DeleteKey", typeof(XFS4IoT.KeyManagement.Commands.DeleteKeyCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.ExportRSADeviceSignedItem", typeof(XFS4IoT.KeyManagement.Commands.ExportRSADeviceSignedItemCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.ExportRSAIssuerSignedItem", typeof(XFS4IoT.KeyManagement.Commands.ExportRSAIssuerSignedItemCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.GenerateKCV", typeof(XFS4IoT.KeyManagement.Commands.GenerateKCVCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.GenerateRSAKeyPair", typeof(XFS4IoT.KeyManagement.Commands.GenerateRSAKeyPairCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.GetCertificate", typeof(XFS4IoT.KeyManagement.Commands.GetCertificateCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.GetKeyDetail", typeof(XFS4IoT.KeyManagement.Commands.GetKeyDetailCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.ImportEmvPublicKey", typeof(XFS4IoT.KeyManagement.Commands.ImportEmvPublicKeyCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.ImportKey", typeof(XFS4IoT.KeyManagement.Commands.ImportKeyCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.ImportKeyToken", typeof(XFS4IoT.KeyManagement.Commands.ImportKeyTokenCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.Initialization", typeof(XFS4IoT.KeyManagement.Commands.InitializationCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.LoadCertificate", typeof(XFS4IoT.KeyManagement.Commands.LoadCertificateCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.ReplaceCertificate", typeof(XFS4IoT.KeyManagement.Commands.ReplaceCertificateCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.Reset", typeof(XFS4IoT.KeyManagement.Commands.ResetCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.StartAuthenticate", typeof(XFS4IoT.KeyManagement.Commands.StartAuthenticateCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "KeyManagement.StartKeyExchange", typeof(XFS4IoT.KeyManagement.Commands.StartKeyExchangeCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.DeleteKey", typeof(XFS4IoT.KeyManagement.Completions.DeleteKeyCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.ExportRSADeviceSignedItem", typeof(XFS4IoT.KeyManagement.Completions.ExportRSADeviceSignedItemCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.ExportRSAIssuerSignedItem", typeof(XFS4IoT.KeyManagement.Completions.ExportRSAIssuerSignedItemCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.GenerateKCV", typeof(XFS4IoT.KeyManagement.Completions.GenerateKCVCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.GenerateRSAKeyPair", typeof(XFS4IoT.KeyManagement.Completions.GenerateRSAKeyPairCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.GetCertificate", typeof(XFS4IoT.KeyManagement.Completions.GetCertificateCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.GetKeyDetail", typeof(XFS4IoT.KeyManagement.Completions.GetKeyDetailCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.ImportEmvPublicKey", typeof(XFS4IoT.KeyManagement.Completions.ImportEmvPublicKeyCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.ImportKey", typeof(XFS4IoT.KeyManagement.Completions.ImportKeyCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.ImportKeyToken", typeof(XFS4IoT.KeyManagement.Completions.ImportKeyTokenCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.Initialization", typeof(XFS4IoT.KeyManagement.Completions.InitializationCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.LoadCertificate", typeof(XFS4IoT.KeyManagement.Completions.LoadCertificateCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.ReplaceCertificate", typeof(XFS4IoT.KeyManagement.Completions.ReplaceCertificateCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.Reset", typeof(XFS4IoT.KeyManagement.Completions.ResetCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.StartAuthenticate", typeof(XFS4IoT.KeyManagement.Completions.StartAuthenticateCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "KeyManagement.StartKeyExchange", typeof(XFS4IoT.KeyManagement.Completions.StartKeyExchangeCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "KeyManagement.CertificateChangeEvent", typeof(XFS4IoT.KeyManagement.Events.CertificateChangeEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "KeyManagement.DUKPTKSNEvent", typeof(XFS4IoT.KeyManagement.Events.DUKPTKSNEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "KeyManagement.IllegalKeyAccessEvent", typeof(XFS4IoT.KeyManagement.Events.IllegalKeyAccessEvent));
            MessageCollection.Add(MessageHeader.TypeEnum.Event, "KeyManagement.InitializedEvent", typeof(XFS4IoT.KeyManagement.Events.InitializedEvent));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IKeyManagementDevice Device { get => ServiceProvider.Device.IsA<IKeyManagementDevice>(); }
    }
}
