/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * SetPinBlockDataHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(SetPinBlockDataCommand))]
    public partial class SetPinBlockDataHandler : ICommandHandler
    {
        public SetPinBlockDataHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetPinBlockDataHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetPinBlockDataHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetPinBlockDataHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetPinBlockDataHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setPinBlockDataCmd = command.IsA<SetPinBlockDataCommand>($"Invalid parameter in the SetPinBlockData Handle method. {nameof(SetPinBlockDataCommand)}");
            setPinBlockDataCmd.Header.RequestId.HasValue.IsTrue();

            ISetPinBlockDataEvents events = new SetPinBlockDataEvents(Connection, setPinBlockDataCmd.Header.RequestId.Value);

            var result = await HandleSetPinBlockData(events, setPinBlockDataCmd, cancel);
            await Connection.SendMessageAsync(new SetPinBlockDataCompletion(setPinBlockDataCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setPinBlockDatacommand = command.IsA<SetPinBlockDataCommand>();
            setPinBlockDatacommand.Header.RequestId.HasValue.IsTrue();

            SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetPinBlockDataCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetPinBlockDataCompletion(setPinBlockDatacommand.Header.RequestId.Value, new SetPinBlockDataCompletion.PayloadData(errorCode, commandException.Message));

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
