/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * StatusHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Common
{
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(StatusCommand))]
    public partial class StatusHandler : ICommandHandler
    {
        public StatusHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(StatusHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(StatusHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICommonDevice>();

            Common = Provider.IsA<ICommonServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(StatusHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var statusCmd = command.IsA<StatusCommand>($"Invalid parameter in the Status Handle method. {nameof(StatusCommand)}");
            statusCmd.Headers.RequestId.HasValue.IsTrue();

            IStatusEvents events = new StatusEvents(Connection, statusCmd.Headers.RequestId.Value);

            var result = await HandleStatus(events, statusCmd, cancel);
            await Connection.SendMessageAsync(new StatusCompletion(statusCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var statuscommand = command.IsA<StatusCommand>();
            statuscommand.Headers.RequestId.HasValue.IsTrue();

            StatusCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => StatusCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => StatusCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => StatusCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new StatusCompletion(statuscommand.Headers.RequestId.Value, new StatusCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICommonDevice Device { get => Provider.Device.IsA<ICommonDevice>(); }
        private IServiceProvider Provider { get; }
        private ICommonServiceClass Common { get; }
        private ILogger Logger { get; }
    }

}
