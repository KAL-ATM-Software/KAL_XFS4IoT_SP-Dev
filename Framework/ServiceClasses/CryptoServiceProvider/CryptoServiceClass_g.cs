/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * CryptoServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Crypto;

namespace XFS4IoTServer
{
    public partial class CryptoServiceClass : ICryptoServiceClass
    {

        protected void RegisterFactory(IServiceProvider ServiceProvider)
        {
            // Add command handlers.
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Crypto.Commands.CryptoDataCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Crypto.CryptoDataHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, CryptoDataHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Crypto.Commands.DigestCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Crypto.DigestHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, DigestHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Crypto.Commands.GenerateAuthenticationCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Crypto.GenerateAuthenticationHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GenerateAuthenticationHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Crypto.Commands.GenerateRandomCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Crypto.GenerateRandomHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, GenerateRandomHandler"), logger), false);
            CommandDispatcher.AddHandler(ServiceProvider, typeof(XFS4IoT.Crypto.Commands.VerifyAuthenticationCommand), (connection, dispatcher, logger) => new XFS4IoTFramework.Crypto.VerifyAuthenticationHandler(connection, ServiceProvider.IsA<ICommandDispatcher>($"Unexpected type for ServiceProvider. {this.GetType()}, VerifyAuthenticationHandler"), logger), false);
            // Add supported message structures.
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Crypto.CryptoData", typeof(XFS4IoT.Crypto.Commands.CryptoDataCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Crypto.Digest", typeof(XFS4IoT.Crypto.Commands.DigestCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Crypto.GenerateAuthentication", typeof(XFS4IoT.Crypto.Commands.GenerateAuthenticationCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Crypto.GenerateRandom", typeof(XFS4IoT.Crypto.Commands.GenerateRandomCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Command, "Crypto.VerifyAuthentication", typeof(XFS4IoT.Crypto.Commands.VerifyAuthenticationCommand));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Crypto.CryptoData", typeof(XFS4IoT.Crypto.Completions.CryptoDataCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Crypto.Digest", typeof(XFS4IoT.Crypto.Completions.DigestCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Crypto.GenerateAuthentication", typeof(XFS4IoT.Crypto.Completions.GenerateAuthenticationCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Crypto.GenerateRandom", typeof(XFS4IoT.Crypto.Completions.GenerateRandomCompletion));
            MessageCollection.Add(MessageHeader.TypeEnum.Completion, "Crypto.VerifyAuthentication", typeof(XFS4IoT.Crypto.Completions.VerifyAuthenticationCompletion));
        }

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICryptoDevice Device { get => ServiceProvider.Device.IsA<ICryptoDevice>(); }
    }
}
