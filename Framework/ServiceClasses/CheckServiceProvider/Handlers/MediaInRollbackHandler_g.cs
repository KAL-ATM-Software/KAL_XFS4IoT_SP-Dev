/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaInRollbackHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Check
{
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(MediaInRollbackCommand))]
    public partial class MediaInRollbackHandler : ICommandHandler
    {
        public MediaInRollbackHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(MediaInRollbackHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(MediaInRollbackHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(MediaInRollbackHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(MediaInRollbackHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var mediaInRollbackCmd = command.IsA<MediaInRollbackCommand>($"Invalid parameter in the MediaInRollback Handle method. {nameof(MediaInRollbackCommand)}");
            mediaInRollbackCmd.Header.RequestId.HasValue.IsTrue();

            IMediaInRollbackEvents events = new MediaInRollbackEvents(Connection, mediaInRollbackCmd.Header.RequestId.Value);

            var result = await HandleMediaInRollback(events, mediaInRollbackCmd, cancel);
            await Connection.SendMessageAsync(new MediaInRollbackCompletion(mediaInRollbackCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var mediaInRollbackcommand = command.IsA<MediaInRollbackCommand>();
            mediaInRollbackcommand.Header.RequestId.HasValue.IsTrue();

            MediaInRollbackCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => MediaInRollbackCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new MediaInRollbackCompletion(mediaInRollbackcommand.Header.RequestId.Value, new MediaInRollbackCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICheckDevice Device { get => Provider.Device.IsA<ICheckDevice>(); }
        private IServiceProvider Provider { get; }
        private ICheckService Check { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
