/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * WriteHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.TextTerminal
{
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(WriteCommand))]
    public partial class WriteHandler : ICommandHandler
    {
        public WriteHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(WriteHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(WriteHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(WriteHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(WriteHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var writeCmd = command.IsA<WriteCommand>($"Invalid parameter in the Write Handle method. {nameof(WriteCommand)}");
            writeCmd.Header.RequestId.HasValue.IsTrue();

            IWriteEvents events = new WriteEvents(Connection, writeCmd.Header.RequestId.Value);

            var result = await HandleWrite(events, writeCmd, cancel);
            await Connection.SendMessageAsync(new WriteCompletion(writeCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var writecommand = command.IsA<WriteCommand>();
            writecommand.Header.RequestId.HasValue.IsTrue();

            WriteCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => WriteCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => WriteCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => WriteCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => WriteCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => WriteCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new WriteCompletion(writecommand.Header.RequestId.Value, new WriteCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalService TextTerminal { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
