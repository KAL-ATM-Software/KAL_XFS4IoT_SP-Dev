/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTFramework.Common;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashManagement
{
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(CalibrateCashUnitCommand))]
    public partial class CalibrateCashUnitHandler : ICommandHandler
    {
        public CalibrateCashUnitHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CalibrateCashUnitHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CalibrateCashUnitHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CalibrateCashUnitHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(CalibrateCashUnitHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var calibrateCashUnitCmd = command.IsA<CalibrateCashUnitCommand>($"Invalid parameter in the CalibrateCashUnit Handle method. {nameof(CalibrateCashUnitCommand)}");
            calibrateCashUnitCmd.Header.RequestId.HasValue.IsTrue();

            ICalibrateCashUnitEvents events = new CalibrateCashUnitEvents(Connection, calibrateCashUnitCmd.Header.RequestId.Value);

            var result = await HandleCalibrateCashUnit(events, calibrateCashUnitCmd, cancel);
            await Connection.SendMessageAsync(new CalibrateCashUnitCompletion(calibrateCashUnitCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var calibrateCashUnitcommand = command.IsA<CalibrateCashUnitCommand>();
            calibrateCashUnitcommand.Header.RequestId.HasValue.IsTrue();

            CalibrateCashUnitCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CalibrateCashUnitCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => CalibrateCashUnitCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => CalibrateCashUnitCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => CalibrateCashUnitCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => CalibrateCashUnitCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CalibrateCashUnitCompletion(calibrateCashUnitcommand.Header.RequestId.Value, new CalibrateCashUnitCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashManagementDevice Device { get => Provider.Device.IsA<ICashManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashManagementService CashManagement { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
