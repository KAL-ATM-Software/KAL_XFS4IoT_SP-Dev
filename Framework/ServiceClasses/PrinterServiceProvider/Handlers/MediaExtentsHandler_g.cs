/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * MediaExtentsHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(MediaExtentsCommand))]
    public partial class MediaExtentsHandler : ICommandHandler
    {
        public MediaExtentsHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(MediaExtentsHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(MediaExtentsHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(MediaExtentsHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var mediaExtentsCmd = command.IsA<MediaExtentsCommand>($"Invalid parameter in the MediaExtents Handle method. {nameof(MediaExtentsCommand)}");
            
            IMediaExtentsEvents events = new MediaExtentsEvents(Connection, mediaExtentsCmd.Headers.RequestId);

            var result = await HandleMediaExtents(events, mediaExtentsCmd, cancel);
            await Connection.SendMessageAsync(new MediaExtentsCompletion(mediaExtentsCmd.Headers.RequestId, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var mediaExtentscommand = command.IsA<MediaExtentsCommand>();

            MediaExtentsCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MediaExtentsCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => MediaExtentsCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => MediaExtentsCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new MediaExtentsCompletion(mediaExtentscommand.Headers.RequestId, new MediaExtentsCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterServiceClass Printer { get; }
        private ILogger Logger { get; }
    }

}
