/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * CountHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Dispenser, typeof(CountCommand))]
    public partial class CountHandler : ICommandHandler
    {
        public CountHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CountHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CountHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IDispenserDevice>();

            Dispenser = Provider.IsA<IDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CountHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var countCmd = command.IsA<CountCommand>($"Invalid parameter in the Count Handle method. {nameof(CountCommand)}");
            countCmd.Headers.RequestId.HasValue.IsTrue();

            ICountEvents events = new CountEvents(Connection, countCmd.Headers.RequestId.Value);

            var result = await HandleCount(events, countCmd, cancel);
            await Connection.SendMessageAsync(new CountCompletion(countCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var countcommand = command.IsA<CountCommand>();
            countcommand.Headers.RequestId.HasValue.IsTrue();

            CountCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CountCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => CountCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => CountCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CountCompletion(countcommand.Headers.RequestId.Value, new CountCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IDispenserDevice Device { get => Provider.Device.IsA<IDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private IDispenserServiceClass Dispenser { get; }
        private ILogger Logger { get; }
    }

}
