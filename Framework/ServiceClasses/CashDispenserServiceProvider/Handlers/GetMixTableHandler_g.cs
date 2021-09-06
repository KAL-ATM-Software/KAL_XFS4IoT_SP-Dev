/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * GetMixTableHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(GetMixTableCommand))]
    public partial class GetMixTableHandler : ICommandHandler
    {
        public GetMixTableHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetMixTableHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetMixTableHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetMixTableHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getMixTableCmd = command.IsA<GetMixTableCommand>($"Invalid parameter in the GetMixTable Handle method. {nameof(GetMixTableCommand)}");
            getMixTableCmd.Header.RequestId.HasValue.IsTrue();

            IGetMixTableEvents events = new GetMixTableEvents(Connection, getMixTableCmd.Header.RequestId.Value);

            var result = await HandleGetMixTable(events, getMixTableCmd, cancel);
            await Connection.SendMessageAsync(new GetMixTableCompletion(getMixTableCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getMixTablecommand = command.IsA<GetMixTableCommand>();
            getMixTablecommand.Header.RequestId.HasValue.IsTrue();

            GetMixTableCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => GetMixTableCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => GetMixTableCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetMixTableCompletion(getMixTablecommand.Header.RequestId.Value, new GetMixTableCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashDispenserDevice Device { get => Provider.Device.IsA<ICashDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashDispenserServiceClass CashDispenser { get; }
        private ILogger Logger { get; }
    }

}
