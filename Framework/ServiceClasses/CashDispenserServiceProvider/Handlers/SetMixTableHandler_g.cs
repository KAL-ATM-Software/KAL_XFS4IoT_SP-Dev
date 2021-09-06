/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * SetMixTableHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(SetMixTableCommand))]
    public partial class SetMixTableHandler : ICommandHandler
    {
        public SetMixTableHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetMixTableHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetMixTableHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetMixTableHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setMixTableCmd = command.IsA<SetMixTableCommand>($"Invalid parameter in the SetMixTable Handle method. {nameof(SetMixTableCommand)}");
            setMixTableCmd.Header.RequestId.HasValue.IsTrue();

            ISetMixTableEvents events = new SetMixTableEvents(Connection, setMixTableCmd.Header.RequestId.Value);

            var result = await HandleSetMixTable(events, setMixTableCmd, cancel);
            await Connection.SendMessageAsync(new SetMixTableCompletion(setMixTableCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setMixTablecommand = command.IsA<SetMixTableCommand>();
            setMixTablecommand.Header.RequestId.HasValue.IsTrue();

            SetMixTableCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => SetMixTableCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => SetMixTableCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetMixTableCompletion(setMixTablecommand.Header.RequestId.Value, new SetMixTableCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashDispenserDevice Device { get => Provider.Device.IsA<ICashDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashDispenserServiceClass CashDispenser { get; }
        private ILogger Logger { get; }
    }

}
