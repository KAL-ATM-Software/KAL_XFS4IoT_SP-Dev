/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashInStartHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(CashInStartCommand))]
    public partial class CashInStartHandler : ICommandHandler
    {
        public CashInStartHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CashInStartHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashInStartHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CashInStartHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(CashInStartHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var cashInStartCmd = command.IsA<CashInStartCommand>($"Invalid parameter in the CashInStart Handle method. {nameof(CashInStartCommand)}");
            cashInStartCmd.Header.RequestId.HasValue.IsTrue();

            ICashInStartEvents events = new CashInStartEvents(Connection, cashInStartCmd.Header.RequestId.Value);

            var result = await HandleCashInStart(events, cashInStartCmd, cancel);
            await Connection.SendMessageAsync(new CashInStartCompletion(cashInStartCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var cashInStartcommand = command.IsA<CashInStartCommand>();
            cashInStartcommand.Header.RequestId.HasValue.IsTrue();

            CashInStartCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CashInStartCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => CashInStartCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => CashInStartCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => CashInStartCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => CashInStartCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => CashInStartCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => CashInStartCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => CashInStartCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => CashInStartCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => CashInStartCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => CashInStartCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => CashInStartCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => CashInStartCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => CashInStartCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => CashInStartCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CashInStartCompletion(cashInStartcommand.Header.RequestId.Value, new CashInStartCompletion.PayloadData(errorCode, commandException.Message));

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
