/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * PresentHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(PresentCommand))]
    public partial class PresentHandler : ICommandHandler
    {
        public PresentHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PresentHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PresentHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PresentHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var presentCmd = command.IsA<PresentCommand>($"Invalid parameter in the Present Handle method. {nameof(PresentCommand)}");
            presentCmd.Header.RequestId.HasValue.IsTrue();

            IPresentEvents events = new PresentEvents(Connection, presentCmd.Header.RequestId.Value);

            var result = await HandlePresent(events, presentCmd, cancel);
            await Connection.SendMessageAsync(new PresentCompletion(presentCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var presentcommand = command.IsA<PresentCommand>();
            presentcommand.Header.RequestId.HasValue.IsTrue();

            PresentCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PresentCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => PresentCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => PresentCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => PresentCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PresentCompletion(presentcommand.Header.RequestId.Value, new PresentCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashDispenserDevice Device { get => Provider.Device.IsA<ICashDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashDispenserServiceClass CashDispenser { get; }
        private ILogger Logger { get; }
    }

}
