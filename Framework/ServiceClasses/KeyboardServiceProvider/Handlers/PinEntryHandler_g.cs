/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * PinEntryHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Keyboard
{
    [CommandHandler(XFSConstants.ServiceClass.Keyboard, typeof(PinEntryCommand))]
    public partial class PinEntryHandler : ICommandHandler
    {
        public PinEntryHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PinEntryHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PinEntryHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyboardDevice>();

            Keyboard = Provider.IsA<IKeyboardService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PinEntryHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(PinEntryHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var pinEntryCmd = command.IsA<PinEntryCommand>($"Invalid parameter in the PinEntry Handle method. {nameof(PinEntryCommand)}");
            pinEntryCmd.Header.RequestId.HasValue.IsTrue();

            IPinEntryEvents events = new PinEntryEvents(Connection, pinEntryCmd.Header.RequestId.Value);

            var result = await HandlePinEntry(events, pinEntryCmd, cancel);
            await Connection.SendMessageAsync(new PinEntryCompletion(pinEntryCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var pinEntrycommand = command.IsA<PinEntryCommand>();
            pinEntrycommand.Header.RequestId.HasValue.IsTrue();

            PinEntryCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PinEntryCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => PinEntryCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => PinEntryCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => PinEntryCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => PinEntryCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => PinEntryCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => PinEntryCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => PinEntryCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => PinEntryCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => PinEntryCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => PinEntryCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => PinEntryCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PinEntryCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PinEntryCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PinEntryCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PinEntryCompletion(pinEntrycommand.Header.RequestId.Value, new PinEntryCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IKeyboardDevice Device { get => Provider.Device.IsA<IKeyboardDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyboardService Keyboard { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
