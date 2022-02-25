/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * PreparePresentHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(PreparePresentCommand))]
    public partial class PreparePresentHandler : ICommandHandler
    {
        public PreparePresentHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PreparePresentHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PreparePresentHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PreparePresentHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(PreparePresentHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var preparePresentCmd = command.IsA<PreparePresentCommand>($"Invalid parameter in the PreparePresent Handle method. {nameof(PreparePresentCommand)}");
            preparePresentCmd.Header.RequestId.HasValue.IsTrue();

            IPreparePresentEvents events = new PreparePresentEvents(Connection, preparePresentCmd.Header.RequestId.Value);

            var result = await HandlePreparePresent(events, preparePresentCmd, cancel);
            await Connection.SendMessageAsync(new PreparePresentCompletion(preparePresentCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var preparePresentcommand = command.IsA<PreparePresentCommand>();
            preparePresentcommand.Header.RequestId.HasValue.IsTrue();

            PreparePresentCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PreparePresentCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => PreparePresentCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PreparePresentCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PreparePresentCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PreparePresentCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PreparePresentCompletion(preparePresentcommand.Header.RequestId.Value, new PreparePresentCompletion.PayloadData(errorCode, commandException.Message));

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
