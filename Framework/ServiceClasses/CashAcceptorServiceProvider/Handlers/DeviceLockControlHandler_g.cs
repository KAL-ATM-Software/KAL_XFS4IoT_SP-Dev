/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
            await Connection.SendMessageAsync(new DeviceLockControlCompletion(deviceLockControlCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var deviceLockControlcommand = command.IsA<DeviceLockControlCommand>();
            deviceLockControlcommand.Header.RequestId.HasValue.IsTrue();

            DeviceLockControlCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DeviceLockControlCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => DeviceLockControlCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => DeviceLockControlCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => DeviceLockControlCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => DeviceLockControlCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DeviceLockControlCompletion(deviceLockControlcommand.Header.RequestId.Value, new DeviceLockControlCompletion.PayloadData(errorCode, commandException.Message));

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
