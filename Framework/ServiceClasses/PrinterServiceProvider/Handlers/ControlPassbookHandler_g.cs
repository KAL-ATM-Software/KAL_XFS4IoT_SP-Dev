/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ControlPassbookHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(ControlPassbookCommand))]
    public partial class ControlPassbookHandler : ICommandHandler
    {
        public ControlPassbookHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ControlPassbookHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ControlPassbookHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ControlPassbookHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ControlPassbookHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var controlPassbookCmd = command.IsA<ControlPassbookCommand>($"Invalid parameter in the ControlPassbook Handle method. {nameof(ControlPassbookCommand)}");
            controlPassbookCmd.Header.RequestId.HasValue.IsTrue();

            IControlPassbookEvents events = new ControlPassbookEvents(Connection, controlPassbookCmd.Header.RequestId.Value);

            var result = await HandleControlPassbook(events, controlPassbookCmd, cancel);
            await Connection.SendMessageAsync(new ControlPassbookCompletion(controlPassbookCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var controlPassbookcommand = command.IsA<ControlPassbookCommand>();
            controlPassbookcommand.Header.RequestId.HasValue.IsTrue();

            ControlPassbookCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ControlPassbookCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ControlPassbookCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ControlPassbookCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ControlPassbookCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ControlPassbookCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ControlPassbookCompletion(controlPassbookcommand.Header.RequestId.Value, new ControlPassbookCompletion.PayloadData(errorCode, commandException.Message));

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
