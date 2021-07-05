/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * PrepareDispenseHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Dispenser, typeof(PrepareDispenseCommand))]
    public partial class PrepareDispenseHandler : ICommandHandler
    {
        public PrepareDispenseHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PrepareDispenseHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PrepareDispenseHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IDispenserDevice>();

            Dispenser = Provider.IsA<IDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PrepareDispenseHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var prepareDispenseCmd = command.IsA<PrepareDispenseCommand>($"Invalid parameter in the PrepareDispense Handle method. {nameof(PrepareDispenseCommand)}");
            prepareDispenseCmd.Headers.RequestId.HasValue.IsTrue();

            IPrepareDispenseEvents events = new PrepareDispenseEvents(Connection, prepareDispenseCmd.Headers.RequestId.Value);

            var result = await HandlePrepareDispense(events, prepareDispenseCmd, cancel);
            await Connection.SendMessageAsync(new PrepareDispenseCompletion(prepareDispenseCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var prepareDispensecommand = command.IsA<PrepareDispenseCommand>();
            prepareDispensecommand.Headers.RequestId.HasValue.IsTrue();

            PrepareDispenseCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PrepareDispenseCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => PrepareDispenseCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => PrepareDispenseCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PrepareDispenseCompletion(prepareDispensecommand.Headers.RequestId.Value, new PrepareDispenseCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IDispenserDevice Device { get => Provider.Device.IsA<IDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private IDispenserServiceClass Dispenser { get; }
        private ILogger Logger { get; }
    }

}
