/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * EndExchangeHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(EndExchangeCommand))]
    public partial class EndExchangeHandler : ICommandHandler
    {
        public EndExchangeHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EndExchangeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(EndExchangeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(EndExchangeHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var endExchangeCmd = command.IsA<EndExchangeCommand>($"Invalid parameter in the EndExchange Handle method. {nameof(EndExchangeCommand)}");
            endExchangeCmd.Headers.RequestId.HasValue.IsTrue();

            IEndExchangeEvents events = new EndExchangeEvents(Connection, endExchangeCmd.Headers.RequestId.Value);

            var result = await HandleEndExchange(events, endExchangeCmd, cancel);
            await Connection.SendMessageAsync(new EndExchangeCompletion(endExchangeCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var endExchangecommand = command.IsA<EndExchangeCommand>();
            endExchangecommand.Headers.RequestId.HasValue.IsTrue();

            EndExchangeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => EndExchangeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => EndExchangeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => EndExchangeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new EndExchangeCompletion(endExchangecommand.Headers.RequestId.Value, new EndExchangeCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementServiceClass CashManagement { get; }
        private ILogger Logger { get; }
    }

}
