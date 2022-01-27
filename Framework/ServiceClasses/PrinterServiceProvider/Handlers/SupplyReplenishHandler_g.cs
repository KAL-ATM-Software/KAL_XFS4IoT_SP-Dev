/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * SupplyReplenishHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Printer, typeof(SupplyReplenishCommand))]
    public partial class SupplyReplenishHandler : ICommandHandler
    {
        public SupplyReplenishHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SupplyReplenishHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SupplyReplenishHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPrinterDevice>();

            Printer = Provider.IsA<IPrinterService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SupplyReplenishHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SupplyReplenishHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var supplyReplenishCmd = command.IsA<SupplyReplenishCommand>($"Invalid parameter in the SupplyReplenish Handle method. {nameof(SupplyReplenishCommand)}");
            supplyReplenishCmd.Header.RequestId.HasValue.IsTrue();

            ISupplyReplenishEvents events = new SupplyReplenishEvents(Connection, supplyReplenishCmd.Header.RequestId.Value);

            var result = await HandleSupplyReplenish(events, supplyReplenishCmd, cancel);
            await Connection.SendMessageAsync(new SupplyReplenishCompletion(supplyReplenishCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var supplyReplenishcommand = command.IsA<SupplyReplenishCommand>();
            supplyReplenishcommand.Header.RequestId.HasValue.IsTrue();

            SupplyReplenishCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SupplyReplenishCompletion(supplyReplenishcommand.Header.RequestId.Value, new SupplyReplenishCompletion.PayloadData(errorCode, commandException.Message));

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
