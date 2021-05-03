/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * ClearScreenHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(ClearScreenCommand))]
    public partial class ClearScreenHandler : ICommandHandler
    {
        public ClearScreenHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ClearScreenHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ClearScreenHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ClearScreenHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var clearScreenCmd = command.IsA<ClearScreenCommand>($"Invalid parameter in the ClearScreen Handle method. {nameof(ClearScreenCommand)}");
            
            IClearScreenEvents events = new ClearScreenEvents(Connection, clearScreenCmd.Headers.RequestId);

            var result = await HandleClearScreen(events, clearScreenCmd, cancel);
            await Connection.SendMessageAsync(new ClearScreenCompletion(clearScreenCmd.Headers.RequestId, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var clearScreencommand = command.IsA<ClearScreenCommand>();

            ClearScreenCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ClearScreenCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => ClearScreenCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => ClearScreenCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ClearScreenCompletion(clearScreencommand.Headers.RequestId, new ClearScreenCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalServiceClass TextTerminal { get; }
        private ILogger Logger { get; }
    }

}
