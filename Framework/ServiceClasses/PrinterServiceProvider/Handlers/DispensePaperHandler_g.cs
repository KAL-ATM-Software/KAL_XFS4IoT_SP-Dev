/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * DispensePaperHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(DispensePaperCommand))]
    public partial class DispensePaperHandler : ICommandHandler
    {
        public DispensePaperHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DispensePaperHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DispensePaperHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DispensePaperHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var dispensePaperCmd = command.IsA<DispensePaperCommand>($"Invalid parameter in the DispensePaper Handle method. {nameof(DispensePaperCommand)}");
            dispensePaperCmd.Header.RequestId.HasValue.IsTrue();

            IDispensePaperEvents events = new DispensePaperEvents(Connection, dispensePaperCmd.Header.RequestId.Value);

            var result = await HandleDispensePaper(events, dispensePaperCmd, cancel);
            await Connection.SendMessageAsync(new DispensePaperCompletion(dispensePaperCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var dispensePapercommand = command.IsA<DispensePaperCommand>();
            dispensePapercommand.Header.RequestId.HasValue.IsTrue();

            DispensePaperCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DispensePaperCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => DispensePaperCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => DispensePaperCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => DispensePaperCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DispensePaperCompletion(dispensePapercommand.Header.RequestId.Value, new DispensePaperCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterServiceClass Printer { get; }
        private ILogger Logger { get; }
    }

}
