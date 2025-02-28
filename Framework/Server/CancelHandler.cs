/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;

namespace XFS4IoTServer
{
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(CancelCommand))]
    [CommandHandlerAsync]
    public class CancelHandler : ICommandHandler
    {
        public CancelHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CancelHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(CancelHandler)} constructor. {nameof(Provider.Device)}");

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CancelHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(CancelHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var cancelCmd = command.IsA<CancelCommand>($"Invalid parameter in the Cancel Handle method. {nameof(CancelCommand)}");
            cancelCmd.Header.RequestId.HasValue.IsTrue();

            bool res = await Provider.IsA<ICommandDispatcher>().AnyValidRequestID(Connection, cancelCmd.Payload.RequestIds, cancel);

            (CancelCompletion.PayloadData, MessageHeader.CompletionCodeEnum) result = res ? (null, MessageHeader.CompletionCodeEnum.Success)
                : (new(CancelCompletion.PayloadData.ErrorCodeEnum.NoMatchingRequestIDs), MessageHeader.CompletionCodeEnum.CommandErrorCode);
            await Connection.SendMessageAsync(new CancelCompletion(cancelCmd.Header.RequestId.Value, result.Item1, result.Item2, null));

            if(res)
            {
                Logger.Log(Constants.Component, "ICommandDispatcher.CancelCommandsAsync()");
                await Provider.IsA<ICommandDispatcher>().CancelCommandsAsync(Connection, cancelCmd.Payload.RequestIds, cancel);
                Logger.Log(Constants.Component, "ICommandDispatcher.CancelCommandsAsync() -> Success");
            }
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var cancelCommand = command.IsA<CancelCommand>();
            cancelCommand.Header.RequestId.HasValue.IsTrue();

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

            var response = new CancelCompletion(cancelCommand.Header.RequestId.Value, null, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IDevice Device { get => Provider.Device; }
        private IServiceProvider Provider { get; }
        private ILogger Logger { get; }
    }
}
