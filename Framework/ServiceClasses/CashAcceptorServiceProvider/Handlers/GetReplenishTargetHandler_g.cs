/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetReplenishTargetHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(GetReplenishTargetCommand))]
    public partial class GetReplenishTargetHandler : ICommandHandler
    {
        public GetReplenishTargetHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetReplenishTargetHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetReplenishTargetHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetReplenishTargetHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetReplenishTargetHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getReplenishTargetCmd = command.IsA<GetReplenishTargetCommand>($"Invalid parameter in the GetReplenishTarget Handle method. {nameof(GetReplenishTargetCommand)}");
            getReplenishTargetCmd.Header.RequestId.HasValue.IsTrue();

            IGetReplenishTargetEvents events = new GetReplenishTargetEvents(Connection, getReplenishTargetCmd.Header.RequestId.Value);

            var result = await HandleGetReplenishTarget(events, getReplenishTargetCmd, cancel);
            await Connection.SendMessageAsync(new GetReplenishTargetCompletion(getReplenishTargetCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getReplenishTargetcommand = command.IsA<GetReplenishTargetCommand>();
            getReplenishTargetcommand.Header.RequestId.HasValue.IsTrue();

            GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetReplenishTargetCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetReplenishTargetCompletion(getReplenishTargetcommand.Header.RequestId.Value, new GetReplenishTargetCompletion.PayloadData(errorCode, commandException.Message));

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
