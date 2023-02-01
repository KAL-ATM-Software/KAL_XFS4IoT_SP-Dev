/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * PrintFormHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(PrintFormCommand))]
    public partial class PrintFormHandler : ICommandHandler
    {
        public PrintFormHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PrintFormHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PrintFormHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PrintFormHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(PrintFormHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var printFormCmd = command.IsA<PrintFormCommand>($"Invalid parameter in the PrintForm Handle method. {nameof(PrintFormCommand)}");
            printFormCmd.Header.RequestId.HasValue.IsTrue();

            IPrintFormEvents events = new PrintFormEvents(Connection, printFormCmd.Header.RequestId.Value);

            var result = await HandlePrintForm(events, printFormCmd, cancel);
            await Connection.SendMessageAsync(new PrintFormCompletion(printFormCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var printFormcommand = command.IsA<PrintFormCommand>();
            printFormcommand.Header.RequestId.HasValue.IsTrue();

            PrintFormCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PrintFormCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => PrintFormCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => PrintFormCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => PrintFormCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => PrintFormCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => PrintFormCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => PrintFormCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => PrintFormCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => PrintFormCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => PrintFormCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => PrintFormCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => PrintFormCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PrintFormCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PrintFormCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PrintFormCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PrintFormCompletion(printFormcommand.Header.RequestId.Value, new PrintFormCompletion.PayloadData(errorCode, commandException.Message));

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
