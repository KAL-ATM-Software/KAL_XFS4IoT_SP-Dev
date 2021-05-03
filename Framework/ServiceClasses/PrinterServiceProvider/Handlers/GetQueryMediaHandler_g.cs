/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryMediaHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Printer
{
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(GetQueryMediaCommand))]
    public partial class GetQueryMediaHandler : ICommandHandler
    {
        public GetQueryMediaHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetQueryMediaHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetQueryMediaHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetQueryMediaHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getQueryMediaCmd = command.IsA<GetQueryMediaCommand>($"Invalid parameter in the GetQueryMedia Handle method. {nameof(GetQueryMediaCommand)}");
            
            IGetQueryMediaEvents events = new GetQueryMediaEvents(Connection, getQueryMediaCmd.Headers.RequestId);

            var result = await HandleGetQueryMedia(events, getQueryMediaCmd, cancel);
            await Connection.SendMessageAsync(new GetQueryMediaCompletion(getQueryMediaCmd.Headers.RequestId, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getQueryMediacommand = command.IsA<GetQueryMediaCommand>();

            GetQueryMediaCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => GetQueryMediaCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetQueryMediaCompletion(getQueryMediacommand.Headers.RequestId, new GetQueryMediaCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterServiceClass Printer { get; }
        private ILogger Logger { get; }
    }

}
