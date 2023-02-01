/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CloseShutterHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashManagement, typeof(CloseShutterCommand))]
    public partial class CloseShutterHandler : ICommandHandler
    {
        public CloseShutterHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CloseShutterHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CloseShutterHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashManagementDevice>();

            CashManagement = Provider.IsA<ICashManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CloseShutterHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(CloseShutterHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var closeShutterCmd = command.IsA<CloseShutterCommand>($"Invalid parameter in the CloseShutter Handle method. {nameof(CloseShutterCommand)}");
            closeShutterCmd.Header.RequestId.HasValue.IsTrue();

            ICloseShutterEvents events = new CloseShutterEvents(Connection, closeShutterCmd.Header.RequestId.Value);

            var result = await HandleCloseShutter(events, closeShutterCmd, cancel);
            await Connection.SendMessageAsync(new CloseShutterCompletion(closeShutterCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var closeShuttercommand = command.IsA<CloseShutterCommand>();
            closeShuttercommand.Header.RequestId.HasValue.IsTrue();

            CloseShutterCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => CloseShutterCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => CloseShutterCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => CloseShutterCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CloseShutterCompletion(closeShuttercommand.Header.RequestId.Value, new CloseShutterCompletion.PayloadData(errorCode, commandException.Message));

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
