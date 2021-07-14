/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * DispenseHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Dispenser.Commands;
using XFS4IoT.Dispenser.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Dispenser
{
    [CommandHandler(XFSConstants.ServiceClass.Dispenser, typeof(DispenseCommand))]
    public partial class DispenseHandler : ICommandHandler
    {
        public DispenseHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DispenseHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DispenseHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IDispenserDevice>();

            Dispenser = Provider.IsA<IDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DispenseHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var dispenseCmd = command.IsA<DispenseCommand>($"Invalid parameter in the Dispense Handle method. {nameof(DispenseCommand)}");
            dispenseCmd.Header.RequestId.HasValue.IsTrue();

            IDispenseEvents events = new DispenseEvents(Connection, dispenseCmd.Header.RequestId.Value);

            var result = await HandleDispense(events, dispenseCmd, cancel);
            await Connection.SendMessageAsync(new DispenseCompletion(dispenseCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var dispensecommand = command.IsA<DispenseCommand>();
            dispensecommand.Header.RequestId.HasValue.IsTrue();

            DispenseCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DispenseCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => DispenseCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => DispenseCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DispenseCompletion(dispensecommand.Header.RequestId.Value, new DispenseCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IDispenserDevice Device { get => Provider.Device.IsA<IDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private IDispenserServiceClass Dispenser { get; }
        private ILogger Logger { get; }
    }

}
