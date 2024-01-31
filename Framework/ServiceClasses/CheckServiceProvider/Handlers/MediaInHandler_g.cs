/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaInHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(MediaInCommand))]
    public partial class MediaInHandler : ICommandHandler
    {
        public MediaInHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(MediaInHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(MediaInHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(MediaInHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(MediaInHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var mediaInCmd = command.IsA<MediaInCommand>($"Invalid parameter in the MediaIn Handle method. {nameof(MediaInCommand)}");
            mediaInCmd.Header.RequestId.HasValue.IsTrue();

            IMediaInEvents events = new MediaInEvents(Connection, mediaInCmd.Header.RequestId.Value);

            var result = await HandleMediaIn(events, mediaInCmd, cancel);
            await Connection.SendMessageAsync(new MediaInCompletion(mediaInCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var mediaIncommand = command.IsA<MediaInCommand>();
            mediaIncommand.Header.RequestId.HasValue.IsTrue();

            MediaInCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MediaInCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => MediaInCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => MediaInCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => MediaInCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => MediaInCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => MediaInCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => MediaInCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => MediaInCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => MediaInCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => MediaInCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => MediaInCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => MediaInCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => MediaInCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => MediaInCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => MediaInCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new MediaInCompletion(mediaIncommand.Header.RequestId.Value, new MediaInCompletion.PayloadData(errorCode, commandException.Message));

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
