/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetMediaListHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(GetMediaListCommand))]
    public partial class GetMediaListHandler : ICommandHandler
    {
        public GetMediaListHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetMediaListHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetMediaListHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetMediaListHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getMediaListCmd = command.IsA<GetMediaListCommand>($"Invalid parameter in the GetMediaList Handle method. {nameof(GetMediaListCommand)}");
            getMediaListCmd.Header.RequestId.HasValue.IsTrue();

            IGetMediaListEvents events = new GetMediaListEvents(Connection, getMediaListCmd.Header.RequestId.Value);

            var result = await HandleGetMediaList(events, getMediaListCmd, cancel);
            await Connection.SendMessageAsync(new GetMediaListCompletion(getMediaListCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getMediaListcommand = command.IsA<GetMediaListCommand>();
            getMediaListcommand.Header.RequestId.HasValue.IsTrue();

            GetMediaListCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetMediaListCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetMediaListCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => GetMediaListCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => GetMediaListCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetMediaListCompletion(getMediaListcommand.Header.RequestId.Value, new GetMediaListCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterServiceClass Printer { get; }
        private ILogger Logger { get; }
    }

}
