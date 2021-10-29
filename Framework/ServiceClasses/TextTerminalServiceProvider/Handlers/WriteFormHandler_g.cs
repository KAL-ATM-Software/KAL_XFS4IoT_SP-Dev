/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * WriteFormHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(WriteFormCommand))]
    public partial class WriteFormHandler : ICommandHandler
    {
        public WriteFormHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(WriteFormHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(WriteFormHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(WriteFormHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var writeFormCmd = command.IsA<WriteFormCommand>($"Invalid parameter in the WriteForm Handle method. {nameof(WriteFormCommand)}");
            writeFormCmd.Header.RequestId.HasValue.IsTrue();

            IWriteFormEvents events = new WriteFormEvents(Connection, writeFormCmd.Header.RequestId.Value);

            var result = await HandleWriteForm(events, writeFormCmd, cancel);
            await Connection.SendMessageAsync(new WriteFormCompletion(writeFormCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var writeFormcommand = command.IsA<WriteFormCommand>();
            writeFormcommand.Header.RequestId.HasValue.IsTrue();

            WriteFormCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => WriteFormCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => WriteFormCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => WriteFormCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => WriteFormCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => WriteFormCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new WriteFormCompletion(writeFormcommand.Header.RequestId.Value, new WriteFormCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalServiceClass TextTerminal { get; }
        private ILogger Logger { get; }
    }

}
