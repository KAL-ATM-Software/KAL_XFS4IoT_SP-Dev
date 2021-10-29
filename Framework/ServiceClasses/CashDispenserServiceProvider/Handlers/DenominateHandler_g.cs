/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * DenominateHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(DenominateCommand))]
    public partial class DenominateHandler : ICommandHandler
    {
        public DenominateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DenominateHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DenominateHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DenominateHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var denominateCmd = command.IsA<DenominateCommand>($"Invalid parameter in the Denominate Handle method. {nameof(DenominateCommand)}");
            denominateCmd.Header.RequestId.HasValue.IsTrue();

            IDenominateEvents events = new DenominateEvents(Connection, denominateCmd.Header.RequestId.Value);

            var result = await HandleDenominate(events, denominateCmd, cancel);
            await Connection.SendMessageAsync(new DenominateCompletion(denominateCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var denominatecommand = command.IsA<DenominateCommand>();
            denominatecommand.Header.RequestId.HasValue.IsTrue();

            DenominateCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DenominateCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => DenominateCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => DenominateCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => DenominateCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => DenominateCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DenominateCompletion(denominatecommand.Header.RequestId.Value, new DenominateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashDispenserDevice Device { get => Provider.Device.IsA<ICashDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashDispenserServiceClass CashDispenser { get; }
        private ILogger Logger { get; }
    }

}
