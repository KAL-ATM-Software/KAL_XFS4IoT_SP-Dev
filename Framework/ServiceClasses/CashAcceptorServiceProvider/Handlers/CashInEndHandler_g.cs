/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashInEndHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(CashInEndCommand))]
    public partial class CashInEndHandler : ICommandHandler
    {
        public CashInEndHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CashInEndHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashInEndHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CashInEndHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(CashInEndHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var cashInEndCmd = command.IsA<CashInEndCommand>($"Invalid parameter in the CashInEnd Handle method. {nameof(CashInEndCommand)}");
            cashInEndCmd.Header.RequestId.HasValue.IsTrue();

            ICashInEndEvents events = new CashInEndEvents(Connection, cashInEndCmd.Header.RequestId.Value);

            var result = await HandleCashInEnd(events, cashInEndCmd, cancel);
            await Connection.SendMessageAsync(new CashInEndCompletion(cashInEndCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var cashInEndcommand = command.IsA<CashInEndCommand>();
            cashInEndcommand.Header.RequestId.HasValue.IsTrue();

            CashInEndCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CashInEndCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => CashInEndCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => CashInEndCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => CashInEndCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => CashInEndCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => CashInEndCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => CashInEndCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => CashInEndCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => CashInEndCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => CashInEndCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => CashInEndCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => CashInEndCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => CashInEndCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => CashInEndCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => CashInEndCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CashInEndCompletion(cashInEndcommand.Header.RequestId.Value, new CashInEndCompletion.PayloadData(errorCode, commandException.Message));

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
