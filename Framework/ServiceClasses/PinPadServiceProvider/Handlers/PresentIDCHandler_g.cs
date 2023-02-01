/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * PresentIDCHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(PresentIDCCommand))]
    public partial class PresentIDCHandler : ICommandHandler
    {
        public PresentIDCHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PresentIDCHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(PresentIDCHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(PresentIDCHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(PresentIDCHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var presentIDCCmd = command.IsA<PresentIDCCommand>($"Invalid parameter in the PresentIDC Handle method. {nameof(PresentIDCCommand)}");
            presentIDCCmd.Header.RequestId.HasValue.IsTrue();

            IPresentIDCEvents events = new PresentIDCEvents(Connection, presentIDCCmd.Header.RequestId.Value);

            var result = await HandlePresentIDC(events, presentIDCCmd, cancel);
            await Connection.SendMessageAsync(new PresentIDCCompletion(presentIDCCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var presentIDCcommand = command.IsA<PresentIDCCommand>();
            presentIDCcommand.Header.RequestId.HasValue.IsTrue();

            PresentIDCCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => PresentIDCCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => PresentIDCCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => PresentIDCCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new PresentIDCCompletion(presentIDCcommand.Header.RequestId.Value, new PresentIDCCompletion.PayloadData(errorCode, commandException.Message));

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
