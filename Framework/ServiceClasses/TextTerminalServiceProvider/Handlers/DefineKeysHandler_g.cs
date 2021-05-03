/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * DefineKeysHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(DefineKeysCommand))]
    public partial class DefineKeysHandler : ICommandHandler
    {
        public DefineKeysHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DefineKeysHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DefineKeysHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DefineKeysHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var defineKeysCmd = command.IsA<DefineKeysCommand>($"Invalid parameter in the DefineKeys Handle method. {nameof(DefineKeysCommand)}");
            
            IDefineKeysEvents events = new DefineKeysEvents(Connection, defineKeysCmd.Headers.RequestId);

            var result = await HandleDefineKeys(events, defineKeysCmd, cancel);
            await Connection.SendMessageAsync(new DefineKeysCompletion(defineKeysCmd.Headers.RequestId, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var defineKeyscommand = command.IsA<DefineKeysCommand>();

            DefineKeysCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => DefineKeysCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DefineKeysCompletion(defineKeyscommand.Headers.RequestId, new DefineKeysCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalServiceClass TextTerminal { get; }
        private ILogger Logger { get; }
    }

}
