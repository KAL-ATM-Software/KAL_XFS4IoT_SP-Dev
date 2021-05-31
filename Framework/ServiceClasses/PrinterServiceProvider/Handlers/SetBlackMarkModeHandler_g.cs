/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * SetBlackMarkModeHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Printer
{
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(SetBlackMarkModeCommand))]
    public partial class SetBlackMarkModeHandler : ICommandHandler
    {
        public SetBlackMarkModeHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetBlackMarkModeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetBlackMarkModeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetBlackMarkModeHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setBlackMarkModeCmd = command.IsA<SetBlackMarkModeCommand>($"Invalid parameter in the SetBlackMarkMode Handle method. {nameof(SetBlackMarkModeCommand)}");
            setBlackMarkModeCmd.Headers.RequestId.HasValue.IsTrue();

            ISetBlackMarkModeEvents events = new SetBlackMarkModeEvents(Connection, setBlackMarkModeCmd.Headers.RequestId.Value);

            var result = await HandleSetBlackMarkMode(events, setBlackMarkModeCmd, cancel);
            await Connection.SendMessageAsync(new SetBlackMarkModeCompletion(setBlackMarkModeCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setBlackMarkModecommand = command.IsA<SetBlackMarkModeCommand>();
            setBlackMarkModecommand.Headers.RequestId.HasValue.IsTrue();

            SetBlackMarkModeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetBlackMarkModeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => SetBlackMarkModeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => SetBlackMarkModeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetBlackMarkModeCompletion(setBlackMarkModecommand.Headers.RequestId.Value, new SetBlackMarkModeCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterServiceClass Printer { get; }
        private ILogger Logger { get; }
    }

}
