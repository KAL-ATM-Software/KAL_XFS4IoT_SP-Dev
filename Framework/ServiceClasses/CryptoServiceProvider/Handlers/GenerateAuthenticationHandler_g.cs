/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * GenerateAuthenticationHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Crypto.Commands;
using XFS4IoT.Crypto.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Crypto
{
    [CommandHandler(XFSConstants.ServiceClass.Crypto, typeof(GenerateAuthenticationCommand))]
    public partial class GenerateAuthenticationHandler : ICommandHandler
    {
        public GenerateAuthenticationHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GenerateAuthenticationHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GenerateAuthenticationHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICryptoDevice>();

            Crypto = Provider.IsA<ICryptoServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GenerateAuthenticationHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var generateAuthenticationCmd = command.IsA<GenerateAuthenticationCommand>($"Invalid parameter in the GenerateAuthentication Handle method. {nameof(GenerateAuthenticationCommand)}");
            generateAuthenticationCmd.Header.RequestId.HasValue.IsTrue();

            IGenerateAuthenticationEvents events = new GenerateAuthenticationEvents(Connection, generateAuthenticationCmd.Header.RequestId.Value);

            var result = await HandleGenerateAuthentication(events, generateAuthenticationCmd, cancel);
            await Connection.SendMessageAsync(new GenerateAuthenticationCompletion(generateAuthenticationCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var generateAuthenticationcommand = command.IsA<GenerateAuthenticationCommand>();
            generateAuthenticationcommand.Header.RequestId.HasValue.IsTrue();

            GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GenerateAuthenticationCompletion(generateAuthenticationcommand.Header.RequestId.Value, new GenerateAuthenticationCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICryptoDevice Device { get => Provider.Device.IsA<ICryptoDevice>(); }
        private IServiceProvider Provider { get; }
        private ICryptoServiceClass Crypto { get; }
        private ILogger Logger { get; }
    }

}
