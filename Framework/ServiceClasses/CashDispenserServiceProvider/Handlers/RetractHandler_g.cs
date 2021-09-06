/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * RetractHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashDispenser
{
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(RetractCommand))]
    public partial class RetractHandler : ICommandHandler
    {
        public RetractHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(RetractHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(RetractHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(RetractHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var retractCmd = command.IsA<RetractCommand>($"Invalid parameter in the Retract Handle method. {nameof(RetractCommand)}");
            retractCmd.Header.RequestId.HasValue.IsTrue();

            IRetractEvents events = new RetractEvents(Connection, retractCmd.Header.RequestId.Value);

            var result = await HandleRetract(events, retractCmd, cancel);
            await Connection.SendMessageAsync(new RetractCompletion(retractCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var retractcommand = command.IsA<RetractCommand>();
            retractcommand.Header.RequestId.HasValue.IsTrue();

            RetractCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => RetractCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => RetractCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => RetractCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => RetractCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new RetractCompletion(retractcommand.Header.RequestId.Value, new RetractCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashDispenserDevice Device { get => Provider.Device.IsA<ICashDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashDispenserServiceClass CashDispenser { get; }
        private ILogger Logger { get; }
    }

}
