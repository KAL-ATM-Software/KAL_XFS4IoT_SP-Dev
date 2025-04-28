/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT German interface.
 * SecureMsgReceiveHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.German.Commands;
using XFS4IoT.German.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.German
{
    [CommandHandler(XFSConstants.ServiceClass.German, typeof(SecureMsgReceiveCommand))]
    public partial class SecureMsgReceiveHandler : ICommandHandler
    {
        public SecureMsgReceiveHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SecureMsgReceiveHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SecureMsgReceiveHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IGermanDevice>();

            German = Provider.IsA<IGermanService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SecureMsgReceiveHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SecureMsgReceiveHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var secureMsgReceiveCmd = command.IsA<SecureMsgReceiveCommand>($"Invalid parameter in the SecureMsgReceive Handle method. {nameof(SecureMsgReceiveCommand)}");
            secureMsgReceiveCmd.Header.RequestId.HasValue.IsTrue();

            ISecureMsgReceiveEvents events = new SecureMsgReceiveEvents(Connection, secureMsgReceiveCmd.Header.RequestId.Value);

            var result = await HandleSecureMsgReceive(events, secureMsgReceiveCmd, cancel);
            await Connection.SendMessageAsync(new SecureMsgReceiveCompletion(secureMsgReceiveCmd.Header.RequestId.Value, result.Payload, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var secureMsgReceiveCommand = command.IsA<SecureMsgReceiveCommand>();
            secureMsgReceiveCommand.Header.RequestId.HasValue.IsTrue();

            MessageHeader.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MessageHeader.CompletionCodeEnum.InvalidData,
                InternalErrorException => MessageHeader.CompletionCodeEnum.InternalError,
                UnsupportedDataException => MessageHeader.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => MessageHeader.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => MessageHeader.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => MessageHeader.CompletionCodeEnum.HardwareError,
                UserErrorException => MessageHeader.CompletionCodeEnum.UserError,
                FraudAttemptException => MessageHeader.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => MessageHeader.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => MessageHeader.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => MessageHeader.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => MessageHeader.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => MessageHeader.CompletionCodeEnum.TimeOut,
                _ => MessageHeader.CompletionCodeEnum.InternalError
            };

            var response = new SecureMsgReceiveCompletion(secureMsgReceiveCommand.Header.RequestId.Value, null, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IGermanDevice Device { get => Provider.Device.IsA<IGermanDevice>(); }
        private IServiceProvider Provider { get; }
        private IGermanService German { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
