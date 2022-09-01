/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * PowerSaveControlHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(PowerSaveControlCommand))]
    public partial class PowerSaveControlHandler : ICommandHandler
    {
        public PowerSaveControlHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PowerSaveControlHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PowerSaveControlHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICommonDevice>();

            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PowerSaveControlHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(PowerSaveControlHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var powerSaveControlCmd = command.IsA<PowerSaveControlCommand>($"Invalid parameter in the PowerSaveControl Handle method. {nameof(PowerSaveControlCommand)}");
            powerSaveControlCmd.Header.RequestId.HasValue.IsTrue();

            IPowerSaveControlEvents events = new PowerSaveControlEvents(Connection, powerSaveControlCmd.Header.RequestId.Value);

            var result = await HandlePowerSaveControl(events, powerSaveControlCmd, cancel);
            await Connection.SendMessageAsync(new PowerSaveControlCompletion(powerSaveControlCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var powerSaveControlcommand = command.IsA<PowerSaveControlCommand>();
            powerSaveControlcommand.Header.RequestId.HasValue.IsTrue();

            PowerSaveControlCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PowerSaveControlCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PowerSaveControlCompletion(powerSaveControlcommand.Header.RequestId.Value, new PowerSaveControlCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICommonDevice Device { get => Provider.Device.IsA<ICommonDevice>(); }
        private IServiceProvider Provider { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
