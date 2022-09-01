/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * GetPinBlockHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(GetPinBlockCommand))]
    public partial class GetPinBlockHandler : ICommandHandler
    {
        public GetPinBlockHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetPinBlockHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetPinBlockHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetPinBlockHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetPinBlockHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getPinBlockCmd = command.IsA<GetPinBlockCommand>($"Invalid parameter in the GetPinBlock Handle method. {nameof(GetPinBlockCommand)}");
            getPinBlockCmd.Header.RequestId.HasValue.IsTrue();

            IGetPinBlockEvents events = new GetPinBlockEvents(Connection, getPinBlockCmd.Header.RequestId.Value);

            var result = await HandleGetPinBlock(events, getPinBlockCmd, cancel);
            await Connection.SendMessageAsync(new GetPinBlockCompletion(getPinBlockCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getPinBlockcommand = command.IsA<GetPinBlockCommand>();
            getPinBlockcommand.Header.RequestId.HasValue.IsTrue();

            GetPinBlockCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetPinBlockCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetPinBlockCompletion(getPinBlockcommand.Header.RequestId.Value, new GetPinBlockCompletion.PayloadData(errorCode, commandException.Message));

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
