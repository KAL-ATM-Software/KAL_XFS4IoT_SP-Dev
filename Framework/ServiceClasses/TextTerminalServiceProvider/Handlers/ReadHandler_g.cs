/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * ReadHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(ReadCommand))]
    public partial class ReadHandler : ICommandHandler
    {
        public ReadHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ReadHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ReadHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var readCmd = command.IsA<ReadCommand>($"Invalid parameter in the Read Handle method. {nameof(ReadCommand)}");
            readCmd.Header.RequestId.HasValue.IsTrue();

            IReadEvents events = new ReadEvents(Connection, readCmd.Header.RequestId.Value);

            var result = await HandleRead(events, readCmd, cancel);
            await Connection.SendMessageAsync(new ReadCompletion(readCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var readcommand = command.IsA<ReadCommand>();
            readcommand.Header.RequestId.HasValue.IsTrue();

            ReadCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ReadCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => ReadCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => ReadCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ReadCompletion(readcommand.Header.RequestId.Value, new ReadCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalServiceClass TextTerminal { get; }
        private ILogger Logger { get; }
    }

}
