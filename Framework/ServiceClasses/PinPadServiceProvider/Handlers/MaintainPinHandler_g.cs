/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * MaintainPinHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.PinPad
{
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(MaintainPinCommand))]
    public partial class MaintainPinHandler : ICommandHandler
    {
        public MaintainPinHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(MaintainPinHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(MaintainPinHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(MaintainPinHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(MaintainPinHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var maintainPinCmd = command.IsA<MaintainPinCommand>($"Invalid parameter in the MaintainPin Handle method. {nameof(MaintainPinCommand)}");
            maintainPinCmd.Header.RequestId.HasValue.IsTrue();

            IMaintainPinEvents events = new MaintainPinEvents(Connection, maintainPinCmd.Header.RequestId.Value);

            var result = await HandleMaintainPin(events, maintainPinCmd, cancel);
            await Connection.SendMessageAsync(new MaintainPinCompletion(maintainPinCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var maintainPincommand = command.IsA<MaintainPinCommand>();
            maintainPincommand.Header.RequestId.HasValue.IsTrue();

            MaintainPinCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => MaintainPinCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => MaintainPinCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => MaintainPinCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new MaintainPinCompletion(maintainPincommand.Header.RequestId.Value, new MaintainPinCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IPinPadDevice Device { get => Provider.Device.IsA<IPinPadDevice>(); }
        private IServiceProvider Provider { get; }
        private IPinPadService PinPad { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
