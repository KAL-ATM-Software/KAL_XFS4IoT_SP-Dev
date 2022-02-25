/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetPresentStatusHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(GetPresentStatusCommand))]
    public partial class GetPresentStatusHandler : ICommandHandler
    {
        public GetPresentStatusHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetPresentStatusHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetPresentStatusHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetPresentStatusHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetPresentStatusHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getPresentStatusCmd = command.IsA<GetPresentStatusCommand>($"Invalid parameter in the GetPresentStatus Handle method. {nameof(GetPresentStatusCommand)}");
            getPresentStatusCmd.Header.RequestId.HasValue.IsTrue();

            IGetPresentStatusEvents events = new GetPresentStatusEvents(Connection, getPresentStatusCmd.Header.RequestId.Value);

            var result = await HandleGetPresentStatus(events, getPresentStatusCmd, cancel);
            await Connection.SendMessageAsync(new GetPresentStatusCompletion(getPresentStatusCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getPresentStatuscommand = command.IsA<GetPresentStatusCommand>();
            getPresentStatuscommand.Header.RequestId.HasValue.IsTrue();

            GetPresentStatusCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetPresentStatusCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GetPresentStatusCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetPresentStatusCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetPresentStatusCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetPresentStatusCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetPresentStatusCompletion(getPresentStatuscommand.Header.RequestId.Value, new GetPresentStatusCompletion.PayloadData(errorCode, commandException.Message));

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
