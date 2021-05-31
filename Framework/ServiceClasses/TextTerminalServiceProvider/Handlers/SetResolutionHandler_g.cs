/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * SetResolutionHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(SetResolutionCommand))]
    public partial class SetResolutionHandler : ICommandHandler
    {
        public SetResolutionHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetResolutionHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetResolutionHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetResolutionHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setResolutionCmd = command.IsA<SetResolutionCommand>($"Invalid parameter in the SetResolution Handle method. {nameof(SetResolutionCommand)}");
            setResolutionCmd.Headers.RequestId.HasValue.IsTrue();

            ISetResolutionEvents events = new SetResolutionEvents(Connection, setResolutionCmd.Headers.RequestId.Value);

            var result = await HandleSetResolution(events, setResolutionCmd, cancel);
            await Connection.SendMessageAsync(new SetResolutionCompletion(setResolutionCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setResolutioncommand = command.IsA<SetResolutionCommand>();
            setResolutioncommand.Headers.RequestId.HasValue.IsTrue();

            SetResolutionCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => SetResolutionCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetResolutionCompletion(setResolutioncommand.Headers.RequestId.Value, new SetResolutionCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalServiceClass TextTerminal { get; }
        private ILogger Logger { get; }
    }

}
