/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * RetractMediaHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(RetractMediaCommand))]
    public partial class RetractMediaHandler : ICommandHandler
    {
        public RetractMediaHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(RetractMediaHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(RetractMediaHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(RetractMediaHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(RetractMediaHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var retractMediaCmd = command.IsA<RetractMediaCommand>($"Invalid parameter in the RetractMedia Handle method. {nameof(RetractMediaCommand)}");
            retractMediaCmd.Header.RequestId.HasValue.IsTrue();

            IRetractMediaEvents events = new RetractMediaEvents(Connection, retractMediaCmd.Header.RequestId.Value);

            var result = await HandleRetractMedia(events, retractMediaCmd, cancel);
            await Connection.SendMessageAsync(new RetractMediaCompletion(retractMediaCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var retractMediacommand = command.IsA<RetractMediaCommand>();
            retractMediacommand.Header.RequestId.HasValue.IsTrue();

            RetractMediaCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => RetractMediaCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => RetractMediaCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => RetractMediaCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => RetractMediaCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => RetractMediaCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new RetractMediaCompletion(retractMediacommand.Header.RequestId.Value, new RetractMediaCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterService Printer { get; }
        private ILogger Logger { get; }
    }

}
