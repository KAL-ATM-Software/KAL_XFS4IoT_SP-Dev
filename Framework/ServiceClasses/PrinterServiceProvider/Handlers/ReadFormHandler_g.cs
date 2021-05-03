/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ReadFormHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(ReadFormCommand))]
    public partial class ReadFormHandler : ICommandHandler
    {
        public ReadFormHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadFormHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ReadFormHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ReadFormHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var readFormCmd = command.IsA<ReadFormCommand>($"Invalid parameter in the ReadForm Handle method. {nameof(ReadFormCommand)}");
            
            IReadFormEvents events = new ReadFormEvents(Connection, readFormCmd.Headers.RequestId);

            var result = await HandleReadForm(events, readFormCmd, cancel);
            await Connection.SendMessageAsync(new ReadFormCompletion(readFormCmd.Headers.RequestId, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var readFormcommand = command.IsA<ReadFormCommand>();

            ReadFormCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ReadFormCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => ReadFormCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => ReadFormCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ReadFormCompletion(readFormcommand.Headers.RequestId, new ReadFormCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPrinterDevice Device { get => Provider.Device.IsA<IPrinterDevice>(); }
        private IServiceProvider Provider { get; }
        private IPrinterServiceClass Printer { get; }
        private ILogger Logger { get; }
    }

}
