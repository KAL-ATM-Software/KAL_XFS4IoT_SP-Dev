/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * StartExchangeHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashManagement
{
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(StartExchangeCommand))]
    public partial class StartExchangeHandler : ICommandHandler
    {
        public StartExchangeHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(StartExchangeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(StartExchangeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(StartExchangeHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var startExchangeCmd = command.IsA<StartExchangeCommand>($"Invalid parameter in the StartExchange Handle method. {nameof(StartExchangeCommand)}");
            startExchangeCmd.Headers.RequestId.HasValue.IsTrue();

            IStartExchangeEvents events = new StartExchangeEvents(Connection, startExchangeCmd.Headers.RequestId.Value);

            var result = await HandleStartExchange(events, startExchangeCmd, cancel);
            await Connection.SendMessageAsync(new StartExchangeCompletion(startExchangeCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var startExchangecommand = command.IsA<StartExchangeCommand>();
            startExchangecommand.Headers.RequestId.HasValue.IsTrue();

            StartExchangeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => StartExchangeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => StartExchangeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => StartExchangeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new StartExchangeCompletion(startExchangecommand.Headers.RequestId.Value, new StartExchangeCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementServiceClass CashManagement { get; }
        private ILogger Logger { get; }
    }

}
