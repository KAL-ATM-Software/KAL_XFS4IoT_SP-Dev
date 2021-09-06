/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetCashUnitInfoHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(GetCashUnitInfoCommand))]
    public partial class GetCashUnitInfoHandler : ICommandHandler
    {
        public GetCashUnitInfoHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetCashUnitInfoHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetCashUnitInfoHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetCashUnitInfoHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var getCashUnitInfoCmd = command.IsA<GetCashUnitInfoCommand>($"Invalid parameter in the GetCashUnitInfo Handle method. {nameof(GetCashUnitInfoCommand)}");
            getCashUnitInfoCmd.Header.RequestId.HasValue.IsTrue();

            IGetCashUnitInfoEvents events = new GetCashUnitInfoEvents(Connection, getCashUnitInfoCmd.Header.RequestId.Value);

            var result = await HandleGetCashUnitInfo(events, getCashUnitInfoCmd, cancel);
            await Connection.SendMessageAsync(new GetCashUnitInfoCompletion(getCashUnitInfoCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var getCashUnitInfocommand = command.IsA<GetCashUnitInfoCommand>();
            getCashUnitInfocommand.Header.RequestId.HasValue.IsTrue();

            GetCashUnitInfoCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetCashUnitInfoCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => GetCashUnitInfoCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => GetCashUnitInfoCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => GetCashUnitInfoCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetCashUnitInfoCompletion(getCashUnitInfocommand.Header.RequestId.Value, new GetCashUnitInfoCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementServiceClass CashManagement { get; }
        private ILogger Logger { get; }
    }

}
