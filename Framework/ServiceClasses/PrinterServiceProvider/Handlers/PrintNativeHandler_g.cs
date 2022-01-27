/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintNativeHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Printer
{
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(PrintNativeCommand))]
    public partial class PrintNativeHandler : ICommandHandler
    {
        public PrintNativeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PrintNativeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PrintNativeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PrintNativeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(PrintNativeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var printNativeCmd = command.IsA<PrintNativeCommand>($"Invalid parameter in the PrintNative Handle method. {nameof(PrintNativeCommand)}");
            printNativeCmd.Header.RequestId.HasValue.IsTrue();

            IPrintNativeEvents events = new PrintNativeEvents(Connection, printNativeCmd.Header.RequestId.Value);

            var result = await HandlePrintNative(events, printNativeCmd, cancel);
            await Connection.SendMessageAsync(new PrintNativeCompletion(printNativeCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var printNativecommand = command.IsA<PrintNativeCommand>();
            printNativecommand.Header.RequestId.HasValue.IsTrue();

            PrintNativeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PrintNativeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => PrintNativeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PrintNativeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PrintNativeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PrintNativeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PrintNativeCompletion(printNativecommand.Header.RequestId.Value, new PrintNativeCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterService Printer { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
