/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
            await Connection.SendMessageAsync(new GetDepleteSourceCompletion(getDepleteSourceCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getDepleteSourcecommand = command.IsA<GetDepleteSourceCommand>();
            getDepleteSourcecommand.Header.RequestId.HasValue.IsTrue();

            GetDepleteSourceCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetDepleteSourceCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => GetDepleteSourceCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetDepleteSourceCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetDepleteSourceCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetDepleteSourceCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetDepleteSourceCompletion(getDepleteSourcecommand.Header.RequestId.Value, new GetDepleteSourceCompletion.PayloadData(errorCode, commandException.Message));

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
