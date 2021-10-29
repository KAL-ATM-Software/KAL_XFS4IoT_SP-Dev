/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * GetPresentStatusHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashDispenser
{
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(GetPresentStatusCommand))]
    public partial class GetPresentStatusHandler : ICommandHandler
    {
        public GetPresentStatusHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetPresentStatusHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetPresentStatusHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetPresentStatusHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getPresentStatusCmd = command.IsA<GetPresentStatusCommand>($"Invalid parameter in the GetPresentStatus Handle method. {nameof(GetPresentStatusCommand)}");
            getPresentStatusCmd.Header.RequestId.HasValue.IsTrue();

            IGetPresentStatusEvents events = new GetPresentStatusEvents(Connection, getPresentStatusCmd.Header.RequestId.Value);

            var result = await HandleGetPresentStatus(events, getPresentStatusCmd, cancel);
            await Connection.SendMessageAsync(new GetPresentStatusCompletion(getPresentStatusCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
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

            await connection.SendMessageAsync(response);
        }

        private ICashDispenserDevice Device { get => Provider.Device.IsA<ICashDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashDispenserServiceClass CashDispenser { get; }
        private ILogger Logger { get; }
    }

}
