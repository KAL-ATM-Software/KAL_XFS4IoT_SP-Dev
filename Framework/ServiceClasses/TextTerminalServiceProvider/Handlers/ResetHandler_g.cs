/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * ResetHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(ResetCommand))]
    public partial class ResetHandler : ICommandHandler
    {
        public ResetHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ResetHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ResetHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ResetHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ResetHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var resetCmd = command.IsA<ResetCommand>($"Invalid parameter in the Reset Handle method. {nameof(ResetCommand)}");
            resetCmd.Header.RequestId.HasValue.IsTrue();

            IResetEvents events = new ResetEvents(Connection, resetCmd.Header.RequestId.Value);

            var result = await HandleReset(events, resetCmd, cancel);
            await Connection.SendMessageAsync(new ResetCompletion(resetCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var resetcommand = command.IsA<ResetCommand>();
            resetcommand.Header.RequestId.HasValue.IsTrue();

            ResetCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ResetCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ResetCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ResetCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ResetCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ResetCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ResetCompletion(resetcommand.Header.RequestId.Value, new ResetCompletion.PayloadData(errorCode, commandException.Message));

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
