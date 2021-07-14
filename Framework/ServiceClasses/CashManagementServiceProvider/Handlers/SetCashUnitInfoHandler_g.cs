/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * SetCashUnitInfoHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(SetCashUnitInfoCommand))]
    public partial class SetCashUnitInfoHandler : ICommandHandler
    {
        public SetCashUnitInfoHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetCashUnitInfoHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetCashUnitInfoHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetCashUnitInfoHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setCashUnitInfoCmd = command.IsA<SetCashUnitInfoCommand>($"Invalid parameter in the SetCashUnitInfo Handle method. {nameof(SetCashUnitInfoCommand)}");
            setCashUnitInfoCmd.Header.RequestId.HasValue.IsTrue();

            ISetCashUnitInfoEvents events = new SetCashUnitInfoEvents(Connection, setCashUnitInfoCmd.Header.RequestId.Value);

            var result = await HandleSetCashUnitInfo(events, setCashUnitInfoCmd, cancel);
            await Connection.SendMessageAsync(new SetCashUnitInfoCompletion(setCashUnitInfoCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setCashUnitInfocommand = command.IsA<SetCashUnitInfoCommand>();
            setCashUnitInfocommand.Header.RequestId.HasValue.IsTrue();

            SetCashUnitInfoCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetCashUnitInfoCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => SetCashUnitInfoCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => SetCashUnitInfoCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetCashUnitInfoCompletion(setCashUnitInfocommand.Header.RequestId.Value, new SetCashUnitInfoCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementServiceClass CashManagement { get; }
        private ILogger Logger { get; }
    }

}
