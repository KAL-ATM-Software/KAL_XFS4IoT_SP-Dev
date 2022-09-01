/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * DeleteKeyHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.KeyManagement
{
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(DeleteKeyCommand))]
    public partial class DeleteKeyHandler : ICommandHandler
    {
        public DeleteKeyHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DeleteKeyHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DeleteKeyHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DeleteKeyHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(DeleteKeyHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var deleteKeyCmd = command.IsA<DeleteKeyCommand>($"Invalid parameter in the DeleteKey Handle method. {nameof(DeleteKeyCommand)}");
            deleteKeyCmd.Header.RequestId.HasValue.IsTrue();

            IDeleteKeyEvents events = new DeleteKeyEvents(Connection, deleteKeyCmd.Header.RequestId.Value);

            var result = await HandleDeleteKey(events, deleteKeyCmd, cancel);
            await Connection.SendMessageAsync(new DeleteKeyCompletion(deleteKeyCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var deleteKeycommand = command.IsA<DeleteKeyCommand>();
            deleteKeycommand.Header.RequestId.HasValue.IsTrue();

            DeleteKeyCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => DeleteKeyCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DeleteKeyCompletion(deleteKeycommand.Header.RequestId.Value, new DeleteKeyCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementService KeyManagement { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
