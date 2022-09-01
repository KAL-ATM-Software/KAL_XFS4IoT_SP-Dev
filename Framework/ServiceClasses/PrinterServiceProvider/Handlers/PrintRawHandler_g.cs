/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintRawHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(PrintRawCommand))]
    public partial class PrintRawHandler : ICommandHandler
    {
        public PrintRawHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PrintRawHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PrintRawHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PrintRawHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(PrintRawHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var printRawCmd = command.IsA<PrintRawCommand>($"Invalid parameter in the PrintRaw Handle method. {nameof(PrintRawCommand)}");
            printRawCmd.Header.RequestId.HasValue.IsTrue();

            IPrintRawEvents events = new PrintRawEvents(Connection, printRawCmd.Header.RequestId.Value);

            var result = await HandlePrintRaw(events, printRawCmd, cancel);
            await Connection.SendMessageAsync(new PrintRawCompletion(printRawCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var printRawcommand = command.IsA<PrintRawCommand>();
            printRawcommand.Header.RequestId.HasValue.IsTrue();

            PrintRawCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PrintRawCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => PrintRawCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => PrintRawCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => PrintRawCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => PrintRawCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => PrintRawCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => PrintRawCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => PrintRawCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => PrintRawCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => PrintRawCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => PrintRawCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => PrintRawCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PrintRawCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PrintRawCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PrintRawCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PrintRawCompletion(printRawcommand.Header.RequestId.Value, new PrintRawCompletion.PayloadData(errorCode, commandException.Message));

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
