/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ReadImageHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(ReadImageCommand))]
    public partial class ReadImageHandler : ICommandHandler
    {
        public ReadImageHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadImageHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ReadImageHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ReadImageHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var readImageCmd = command.IsA<ReadImageCommand>($"Invalid parameter in the ReadImage Handle method. {nameof(ReadImageCommand)}");
            readImageCmd.Header.RequestId.HasValue.IsTrue();

            IReadImageEvents events = new ReadImageEvents(Connection, readImageCmd.Header.RequestId.Value);

            var result = await HandleReadImage(events, readImageCmd, cancel);
            await Connection.SendMessageAsync(new ReadImageCompletion(readImageCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var readImagecommand = command.IsA<ReadImageCommand>();
            readImagecommand.Header.RequestId.HasValue.IsTrue();

            ReadImageCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ReadImageCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ReadImageCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ReadImageCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ReadImageCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ReadImageCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ReadImageCompletion(readImagecommand.Header.RequestId.Value, new ReadImageCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterServiceClass Printer { get; }
        private ILogger Logger { get; }
    }

}
