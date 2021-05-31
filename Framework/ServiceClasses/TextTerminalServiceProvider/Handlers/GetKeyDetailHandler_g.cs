/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * GetKeyDetailHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(GetKeyDetailCommand))]
    public partial class GetKeyDetailHandler : ICommandHandler
    {
        public GetKeyDetailHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetKeyDetailHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetKeyDetailHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetKeyDetailHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getKeyDetailCmd = command.IsA<GetKeyDetailCommand>($"Invalid parameter in the GetKeyDetail Handle method. {nameof(GetKeyDetailCommand)}");
            getKeyDetailCmd.Headers.RequestId.HasValue.IsTrue();

            IGetKeyDetailEvents events = new GetKeyDetailEvents(Connection, getKeyDetailCmd.Headers.RequestId.Value);

            var result = await HandleGetKeyDetail(events, getKeyDetailCmd, cancel);
            await Connection.SendMessageAsync(new GetKeyDetailCompletion(getKeyDetailCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getKeyDetailcommand = command.IsA<GetKeyDetailCommand>();
            getKeyDetailcommand.Headers.RequestId.HasValue.IsTrue();

            GetKeyDetailCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetKeyDetailCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetKeyDetailCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => GetKeyDetailCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetKeyDetailCompletion(getKeyDetailcommand.Headers.RequestId.Value, new GetKeyDetailCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalServiceClass TextTerminal { get; }
        private ILogger Logger { get; }
    }

}
