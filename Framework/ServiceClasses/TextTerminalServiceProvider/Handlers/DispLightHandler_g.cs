/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * DispLightHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(DispLightCommand))]
    public partial class DispLightHandler : ICommandHandler
    {
        public DispLightHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DispLightHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DispLightHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DispLightHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var dispLightCmd = command.IsA<DispLightCommand>($"Invalid parameter in the DispLight Handle method. {nameof(DispLightCommand)}");
            dispLightCmd.Header.RequestId.HasValue.IsTrue();

            IDispLightEvents events = new DispLightEvents(Connection, dispLightCmd.Header.RequestId.Value);

            var result = await HandleDispLight(events, dispLightCmd, cancel);
            await Connection.SendMessageAsync(new DispLightCompletion(dispLightCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var dispLightcommand = command.IsA<DispLightCommand>();
            dispLightcommand.Header.RequestId.HasValue.IsTrue();

            DispLightCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DispLightCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => DispLightCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => DispLightCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DispLightCompletion(dispLightcommand.Header.RequestId.Value, new DispLightCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalServiceClass TextTerminal { get; }
        private ILogger Logger { get; }
    }

}
