/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetDepleteSourceHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashAcceptor
{
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(GetDepleteSourceCommand))]
    public partial class GetDepleteSourceHandler : ICommandHandler
    {
        public GetDepleteSourceHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetDepleteSourceHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetDepleteSourceHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetDepleteSourceHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetDepleteSourceHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getDepleteSourceCmd = command.IsA<GetDepleteSourceCommand>($"Invalid parameter in the GetDepleteSource Handle method. {nameof(GetDepleteSourceCommand)}");
            getDepleteSourceCmd.Header.RequestId.HasValue.IsTrue();

            IGetDepleteSourceEvents events = new GetDepleteSourceEvents(Connection, getDepleteSourceCmd.Header.RequestId.Value);

            var result = await HandleGetDepleteSource(events, getDepleteSourceCmd, cancel);
            await Connection.SendMessageAsync(new GetDepleteSourceCompletion(getDepleteSourceCmd.Header.RequestId.Value, result.Payload, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getDepleteSourceCommand = command.IsA<GetDepleteSourceCommand>();
            getDepleteSourceCommand.Header.RequestId.HasValue.IsTrue();

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

            var response = new GetDepleteSourceCompletion(getDepleteSourceCommand.Header.RequestId.Value, null, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashAcceptorDevice Device { get => Provider.Device.IsA<ICashAcceptorDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashAcceptorService CashAcceptor { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
