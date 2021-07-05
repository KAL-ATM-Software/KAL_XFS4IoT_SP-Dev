/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * DenominateHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Dispenser, typeof(DenominateCommand))]
    public partial class DenominateHandler : ICommandHandler
    {
        public DenominateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DenominateHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DenominateHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IDispenserDevice>();

            Dispenser = Provider.IsA<IDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DenominateHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var denominateCmd = command.IsA<DenominateCommand>($"Invalid parameter in the Denominate Handle method. {nameof(DenominateCommand)}");
            denominateCmd.Headers.RequestId.HasValue.IsTrue();

            IDenominateEvents events = new DenominateEvents(Connection, denominateCmd.Headers.RequestId.Value);

            var result = await HandleDenominate(events, denominateCmd, cancel);
            await Connection.SendMessageAsync(new DenominateCompletion(denominateCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var denominatecommand = command.IsA<DenominateCommand>();
            denominatecommand.Headers.RequestId.HasValue.IsTrue();

            DenominateCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DenominateCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => DenominateCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => DenominateCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DenominateCompletion(denominatecommand.Headers.RequestId.Value, new DenominateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IDispenserDevice Device { get => Provider.Device.IsA<IDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private IDispenserServiceClass Dispenser { get; }
        private ILogger Logger { get; }
    }

}
