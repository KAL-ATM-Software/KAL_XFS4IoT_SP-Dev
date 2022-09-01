/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTFramework.Common;
using XFS4IoT.Crypto.Commands;
using XFS4IoT.Crypto.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Crypto
{
    [CommandHandler(XFSConstants.ServiceClass.Crypto, typeof(GenerateAuthenticationCommand))]
    public partial class GenerateAuthenticationHandler : ICommandHandler
    {
        public GenerateAuthenticationHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GenerateAuthenticationHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GenerateAuthenticationHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICryptoDevice>();

            Crypto = Provider.IsA<ICryptoService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GenerateAuthenticationHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GenerateAuthenticationHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var generateAuthenticationCmd = command.IsA<GenerateAuthenticationCommand>($"Invalid parameter in the GenerateAuthentication Handle method. {nameof(GenerateAuthenticationCommand)}");
            generateAuthenticationCmd.Header.RequestId.HasValue.IsTrue();

            IGenerateAuthenticationEvents events = new GenerateAuthenticationEvents(Connection, generateAuthenticationCmd.Header.RequestId.Value);

            var result = await HandleGenerateAuthentication(events, generateAuthenticationCmd, cancel);
            await Connection.SendMessageAsync(new GenerateAuthenticationCompletion(generateAuthenticationCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var generateAuthenticationcommand = command.IsA<GenerateAuthenticationCommand>();
            generateAuthenticationcommand.Header.RequestId.HasValue.IsTrue();

            GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GenerateAuthenticationCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GenerateAuthenticationCompletion(generateAuthenticationcommand.Header.RequestId.Value, new GenerateAuthenticationCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICryptoDevice Device { get => Provider.Device.IsA<ICryptoDevice>(); }
        private IServiceProvider Provider { get; }
        private ICryptoService Crypto { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
