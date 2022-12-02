/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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

            CancelCompletion.PayloadData result = res ?
                new(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success, null)
                : new(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode, null, CancelCompletion.PayloadData.ErrorCodeEnum.NoMatchingRequestIDs);
            await Connection.SendMessageAsync(new CancelCompletion(cancelCmd.Header.RequestId.Value, result));

            if(res)
            {
                Logger.Log(Constants.Component, "ICommandDispatcher.CancelCommandsAsync()");
                await Provider.IsA<ICommandDispatcher>().CancelCommandsAsync(Connection, cancelCmd.Payload.RequestIds, cancel);
                Logger.Log(Constants.Component, "ICommandDispatcher.CancelCommandsAsync() -> Success");
            }
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var cancelcommand = command.IsA<CancelCommand>();
            cancelcommand.Header.RequestId.HasValue.IsTrue();

            CancelCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CancelCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => CancelCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => CancelCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => CancelCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => CancelCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => CancelCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => CancelCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => CancelCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => CancelCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => CancelCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => CancelCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => CancelCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => CancelCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => CancelCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => CancelCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CancelCompletion(cancelcommand.Header.RequestId.Value, new CancelCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IDevice Device { get => Provider.Device; }
        private IServiceProvider Provider { get; }
        private ILogger Logger { get; }
    }
}
