/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTServer
{
    [CommandHandlerAsync]
    [CommandHandler(XFSConstants.ServiceClass.Common, typeof(CancelCommand))]
    public class CancelHandler : ICommandHandler
    {
        public CancelHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CancelHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(CancelHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var cancelCmd = command.IsA<CancelCommand>($"Invalid parameter in the Cancel Handle method. {nameof(CancelCommand)}");
            cancelCmd.Header.RequestId.HasValue.IsTrue();

            var dispatcher = Provider.IsA<ICommandDispatcher>();

            bool result = await dispatcher.CancelCommandsAsync(Connection, cancelCmd.Payload.RequestIds);

            if(result)
                await Connection.SendMessageAsync(new CancelCompletion(cancelCmd.Header.RequestId.Value, new CancelCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success, null)));
            else
                await Connection.SendMessageAsync(new CancelCompletion(cancelCmd.Header.RequestId.Value, new CancelCompletion.PayloadData(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode, null, CancelCompletion.PayloadData.ErrorCodeEnum.NoMatchingRequestIDs)));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var cancelcommand = command.IsA<CancelCommand>();
            cancelcommand.Header.RequestId.HasValue.IsTrue();

            CancelCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => CancelCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => CancelCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => CancelCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => CancelCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new CancelCompletion(cancelcommand.Header.RequestId.Value, new CancelCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IServiceProvider Provider { get; }
        private ILogger Logger { get; }
    }
}
