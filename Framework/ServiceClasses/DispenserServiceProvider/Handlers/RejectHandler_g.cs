/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * RejectHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Dispenser, typeof(RejectCommand))]
    public partial class RejectHandler : ICommandHandler
    {
        public RejectHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(RejectHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(RejectHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IDispenserDevice>();

            Dispenser = Provider.IsA<IDispenserServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(RejectHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var rejectCmd = command.IsA<RejectCommand>($"Invalid parameter in the Reject Handle method. {nameof(RejectCommand)}");
            rejectCmd.Header.RequestId.HasValue.IsTrue();

            IRejectEvents events = new RejectEvents(Connection, rejectCmd.Header.RequestId.Value);

            var result = await HandleReject(events, rejectCmd, cancel);
            await Connection.SendMessageAsync(new RejectCompletion(rejectCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var rejectcommand = command.IsA<RejectCommand>();
            rejectcommand.Header.RequestId.HasValue.IsTrue();

            RejectCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => RejectCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => RejectCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => RejectCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new RejectCompletion(rejectcommand.Header.RequestId.Value, new RejectCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IDispenserDevice Device { get => Provider.Device.IsA<IDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private IDispenserServiceClass Dispenser { get; }
        private ILogger Logger { get; }
    }

}
