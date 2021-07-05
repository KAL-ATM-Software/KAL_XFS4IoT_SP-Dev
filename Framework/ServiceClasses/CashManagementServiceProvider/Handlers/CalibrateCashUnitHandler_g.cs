/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CalibrateCashUnitHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(CalibrateCashUnitCommand))]
    public partial class CalibrateCashUnitHandler : ICommandHandler
    {
        public CalibrateCashUnitHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CalibrateCashUnitHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CalibrateCashUnitHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CalibrateCashUnitHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var calibrateCashUnitCmd = command.IsA<CalibrateCashUnitCommand>($"Invalid parameter in the CalibrateCashUnit Handle method. {nameof(CalibrateCashUnitCommand)}");
            calibrateCashUnitCmd.Headers.RequestId.HasValue.IsTrue();

            ICalibrateCashUnitEvents events = new CalibrateCashUnitEvents(Connection, calibrateCashUnitCmd.Headers.RequestId.Value);

            var result = await HandleCalibrateCashUnit(events, calibrateCashUnitCmd, cancel);
            await Connection.SendMessageAsync(new CalibrateCashUnitCompletion(calibrateCashUnitCmd.Headers.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var calibrateCashUnitcommand = command.IsA<CalibrateCashUnitCommand>();
            calibrateCashUnitcommand.Headers.RequestId.HasValue.IsTrue();

            CalibrateCashUnitCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CalibrateCashUnitCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => CalibrateCashUnitCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => CalibrateCashUnitCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CalibrateCashUnitCompletion(calibrateCashUnitcommand.Headers.RequestId.Value, new CalibrateCashUnitCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementServiceClass CashManagement { get; }
        private ILogger Logger { get; }
    }

}
