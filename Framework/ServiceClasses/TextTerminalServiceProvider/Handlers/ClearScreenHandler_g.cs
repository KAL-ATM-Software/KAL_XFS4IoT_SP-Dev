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
using XFS4IoTFramework.Common;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.TextTerminal
{
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(ClearScreenCommand))]
    public partial class ClearScreenHandler : ICommandHandler
    {
        public ClearScreenHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ClearScreenHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ClearScreenHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ClearScreenHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ClearScreenHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var clearScreenCmd = command.IsA<ClearScreenCommand>($"Invalid parameter in the ClearScreen Handle method. {nameof(ClearScreenCommand)}");
            clearScreenCmd.Header.RequestId.HasValue.IsTrue();

            IClearScreenEvents events = new ClearScreenEvents(Connection, clearScreenCmd.Header.RequestId.Value);

            var result = await HandleClearScreen(events, clearScreenCmd, cancel);
            await Connection.SendMessageAsync(new ClearScreenCompletion(clearScreenCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var clearScreencommand = command.IsA<ClearScreenCommand>();
            clearScreencommand.Header.RequestId.HasValue.IsTrue();

            ClearScreenCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ClearScreenCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ClearScreenCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ClearScreenCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ClearScreenCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ClearScreenCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ClearScreenCompletion(clearScreencommand.Header.RequestId.Value, new ClearScreenCompletion.PayloadData(errorCode, commandException.Message));

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
