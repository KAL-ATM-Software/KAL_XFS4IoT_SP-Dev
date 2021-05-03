/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * GetQueryFieldHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(GetQueryFieldCommand))]
    public partial class GetQueryFieldHandler : ICommandHandler
    {
        public GetQueryFieldHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetQueryFieldHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetQueryFieldHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetQueryFieldHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getQueryFieldCmd = command.IsA<GetQueryFieldCommand>($"Invalid parameter in the GetQueryField Handle method. {nameof(GetQueryFieldCommand)}");
            
            IGetQueryFieldEvents events = new GetQueryFieldEvents(Connection, getQueryFieldCmd.Headers.RequestId);

            var result = await HandleGetQueryField(events, getQueryFieldCmd, cancel);
            await Connection.SendMessageAsync(new GetQueryFieldCompletion(getQueryFieldCmd.Headers.RequestId, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getQueryFieldcommand = command.IsA<GetQueryFieldCommand>();

            GetQueryFieldCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetQueryFieldCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetQueryFieldCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => GetQueryFieldCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetQueryFieldCompletion(getQueryFieldcommand.Headers.RequestId, new GetQueryFieldCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalServiceClass TextTerminal { get; }
        private ILogger Logger { get; }
    }

}
