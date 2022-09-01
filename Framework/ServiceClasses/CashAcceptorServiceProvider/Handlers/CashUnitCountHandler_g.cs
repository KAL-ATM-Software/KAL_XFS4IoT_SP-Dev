/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashUnitCountHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(CashUnitCountCommand))]
    public partial class CashUnitCountHandler : ICommandHandler
    {
        public CashUnitCountHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CashUnitCountHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashUnitCountHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CashUnitCountHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(CashUnitCountHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var cashUnitCountCmd = command.IsA<CashUnitCountCommand>($"Invalid parameter in the CashUnitCount Handle method. {nameof(CashUnitCountCommand)}");
            cashUnitCountCmd.Header.RequestId.HasValue.IsTrue();

            ICashUnitCountEvents events = new CashUnitCountEvents(Connection, cashUnitCountCmd.Header.RequestId.Value);

            var result = await HandleCashUnitCount(events, cashUnitCountCmd, cancel);
            await Connection.SendMessageAsync(new CashUnitCountCompletion(cashUnitCountCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var cashUnitCountcommand = command.IsA<CashUnitCountCommand>();
            cashUnitCountcommand.Header.RequestId.HasValue.IsTrue();

            CashUnitCountCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => CashUnitCountCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CashUnitCountCompletion(cashUnitCountcommand.Header.RequestId.Value, new CashUnitCountCompletion.PayloadData(errorCode, commandException.Message));

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
