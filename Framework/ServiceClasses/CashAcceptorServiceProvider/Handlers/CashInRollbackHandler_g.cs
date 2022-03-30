/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashInRollbackHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(CashInRollbackCommand))]
    public partial class CashInRollbackHandler : ICommandHandler
    {
        public CashInRollbackHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CashInRollbackHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashInRollbackHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CashInRollbackHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(CashInRollbackHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var cashInRollbackCmd = command.IsA<CashInRollbackCommand>($"Invalid parameter in the CashInRollback Handle method. {nameof(CashInRollbackCommand)}");
            cashInRollbackCmd.Header.RequestId.HasValue.IsTrue();

            ICashInRollbackEvents events = new CashInRollbackEvents(Connection, cashInRollbackCmd.Header.RequestId.Value);

            var result = await HandleCashInRollback(events, cashInRollbackCmd, cancel);
            await Connection.SendMessageAsync(new CashInRollbackCompletion(cashInRollbackCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var cashInRollbackcommand = command.IsA<CashInRollbackCommand>();
            cashInRollbackcommand.Header.RequestId.HasValue.IsTrue();

            CashInRollbackCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CashInRollbackCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => CashInRollbackCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => CashInRollbackCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => CashInRollbackCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => CashInRollbackCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CashInRollbackCompletion(cashInRollbackcommand.Header.RequestId.Value, new CashInRollbackCompletion.PayloadData(errorCode, commandException.Message));

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
