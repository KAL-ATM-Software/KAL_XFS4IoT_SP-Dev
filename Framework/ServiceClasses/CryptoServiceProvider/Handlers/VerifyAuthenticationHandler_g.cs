/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * VerifyAuthenticationHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Crypto, typeof(VerifyAuthenticationCommand))]
    public partial class VerifyAuthenticationHandler : ICommandHandler
    {
        public VerifyAuthenticationHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(VerifyAuthenticationHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(VerifyAuthenticationHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICryptoDevice>();

            Crypto = Provider.IsA<ICryptoServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(VerifyAuthenticationHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var verifyAuthenticationCmd = command.IsA<VerifyAuthenticationCommand>($"Invalid parameter in the VerifyAuthentication Handle method. {nameof(VerifyAuthenticationCommand)}");
            verifyAuthenticationCmd.Header.RequestId.HasValue.IsTrue();

            IVerifyAuthenticationEvents events = new VerifyAuthenticationEvents(Connection, verifyAuthenticationCmd.Header.RequestId.Value);

            var result = await HandleVerifyAuthentication(events, verifyAuthenticationCmd, cancel);
            await Connection.SendMessageAsync(new VerifyAuthenticationCompletion(verifyAuthenticationCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var verifyAuthenticationcommand = command.IsA<VerifyAuthenticationCommand>();
            verifyAuthenticationcommand.Header.RequestId.HasValue.IsTrue();

            VerifyAuthenticationCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => VerifyAuthenticationCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => VerifyAuthenticationCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => VerifyAuthenticationCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => VerifyAuthenticationCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new VerifyAuthenticationCompletion(verifyAuthenticationcommand.Header.RequestId.Value, new VerifyAuthenticationCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICryptoDevice Device { get => Provider.Device.IsA<ICryptoDevice>(); }
        private IServiceProvider Provider { get; }
        private ICryptoServiceClass Crypto { get; }
        private ILogger Logger { get; }
    }

}
