/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * SetLedHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.TextTerminal
{
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(SetLedCommand))]
    public partial class SetLedHandler : ICommandHandler
    {
        public SetLedHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetLedHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetLedHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetLedHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setLedCmd = command.IsA<SetLedCommand>($"Invalid parameter in the SetLed Handle method. {nameof(SetLedCommand)}");
            setLedCmd.Headers.RequestId.HasValue.IsTrue();

            ISetLedEvents events = new SetLedEvents(Connection, setLedCmd.Headers.RequestId.Value);

            var result = await HandleSetLed(events, setLedCmd, cancel);
            await Connection.SendMessageAsync(new SetLedCompletion(setLedCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setLedcommand = command.IsA<SetLedCommand>();
            setLedcommand.Headers.RequestId.HasValue.IsTrue();

            SetLedCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetLedCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => SetLedCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => SetLedCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetLedCompletion(setLedcommand.Headers.RequestId.Value, new SetLedCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalServiceClass TextTerminal { get; }
        private ILogger Logger { get; }
    }

}
