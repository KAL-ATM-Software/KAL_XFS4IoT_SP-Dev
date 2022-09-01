/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetDeviceLockStatusHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(GetDeviceLockStatusCommand))]
    public partial class GetDeviceLockStatusHandler : ICommandHandler
    {
        public GetDeviceLockStatusHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetDeviceLockStatusHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetDeviceLockStatusHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetDeviceLockStatusHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetDeviceLockStatusHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getDeviceLockStatusCmd = command.IsA<GetDeviceLockStatusCommand>($"Invalid parameter in the GetDeviceLockStatus Handle method. {nameof(GetDeviceLockStatusCommand)}");
            getDeviceLockStatusCmd.Header.RequestId.HasValue.IsTrue();

            IGetDeviceLockStatusEvents events = new GetDeviceLockStatusEvents(Connection, getDeviceLockStatusCmd.Header.RequestId.Value);

            var result = await HandleGetDeviceLockStatus(events, getDeviceLockStatusCmd, cancel);
            await Connection.SendMessageAsync(new GetDeviceLockStatusCompletion(getDeviceLockStatusCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getDeviceLockStatuscommand = command.IsA<GetDeviceLockStatusCommand>();
            getDeviceLockStatuscommand.Header.RequestId.HasValue.IsTrue();

            GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetDeviceLockStatusCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetDeviceLockStatusCompletion(getDeviceLockStatuscommand.Header.RequestId.Value, new GetDeviceLockStatusCompletion.PayloadData(errorCode, commandException.Message));

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
