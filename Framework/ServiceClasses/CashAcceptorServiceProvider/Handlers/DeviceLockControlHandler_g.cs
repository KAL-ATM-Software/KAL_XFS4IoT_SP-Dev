/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * DeviceLockControlHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashAcceptor
{
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(DeviceLockControlCommand))]
    public partial class DeviceLockControlHandler : ICommandHandler
    {
        public DeviceLockControlHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DeviceLockControlHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DeviceLockControlHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DeviceLockControlHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(DeviceLockControlHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var deviceLockControlCmd = command.IsA<DeviceLockControlCommand>($"Invalid parameter in the DeviceLockControl Handle method. {nameof(DeviceLockControlCommand)}");
            deviceLockControlCmd.Header.RequestId.HasValue.IsTrue();

            IDeviceLockControlEvents events = new DeviceLockControlEvents(Connection, deviceLockControlCmd.Header.RequestId.Value);

            var result = await HandleDeviceLockControl(events, deviceLockControlCmd, cancel);
            await Connection.SendMessageAsync(new DeviceLockControlCompletion(deviceLockControlCmd.Header.RequestId.Value, result.Payload, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var deviceLockControlCommand = command.IsA<DeviceLockControlCommand>();
            deviceLockControlCommand.Header.RequestId.HasValue.IsTrue();

            MessageHeader.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MessageHeader.CompletionCodeEnum.InvalidData,
                InternalErrorException => MessageHeader.CompletionCodeEnum.InternalError,
                UnsupportedDataException => MessageHeader.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => MessageHeader.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => MessageHeader.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => MessageHeader.CompletionCodeEnum.HardwareError,
                UserErrorException => MessageHeader.CompletionCodeEnum.UserError,
                FraudAttemptException => MessageHeader.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => MessageHeader.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => MessageHeader.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => MessageHeader.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => MessageHeader.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => MessageHeader.CompletionCodeEnum.TimeOut,
                _ => MessageHeader.CompletionCodeEnum.InternalError
            };

            var response = new DeviceLockControlCompletion(deviceLockControlCommand.Header.RequestId.Value, null, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashAcceptorDevice Device { get => Provider.Device.IsA<ICashAcceptorDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashAcceptorService CashAcceptor { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
