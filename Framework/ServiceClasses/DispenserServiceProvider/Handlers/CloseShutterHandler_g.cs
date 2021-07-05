/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * CloseShutterHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Dispenser, typeof(CloseShutterCommand))]
    public partial class CloseShutterHandler : ICommandHandler
    {
        public CloseShutterHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CloseShutterHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CloseShutterHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IDispenserDevice>();

            Dispenser = Provider.IsA<IDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CloseShutterHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var closeShutterCmd = command.IsA<CloseShutterCommand>($"Invalid parameter in the CloseShutter Handle method. {nameof(CloseShutterCommand)}");
            closeShutterCmd.Headers.RequestId.HasValue.IsTrue();

            ICloseShutterEvents events = new CloseShutterEvents(Connection, closeShutterCmd.Headers.RequestId.Value);

            var result = await HandleCloseShutter(events, closeShutterCmd, cancel);
            await Connection.SendMessageAsync(new CloseShutterCompletion(closeShutterCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var closeShuttercommand = command.IsA<CloseShutterCommand>();
            closeShuttercommand.Headers.RequestId.HasValue.IsTrue();

            CloseShutterCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => CloseShutterCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CloseShutterCompletion(closeShuttercommand.Headers.RequestId.Value, new CloseShutterCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IDispenserDevice Device { get => Provider.Device.IsA<IDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private IDispenserServiceClass Dispenser { get; }
        private ILogger Logger { get; }
    }

}
