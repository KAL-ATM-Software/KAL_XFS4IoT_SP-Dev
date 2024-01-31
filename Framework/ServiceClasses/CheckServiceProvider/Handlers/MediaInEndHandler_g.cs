/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * MediaInEndHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(MediaInEndCommand))]
    public partial class MediaInEndHandler : ICommandHandler
    {
        public MediaInEndHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(MediaInEndHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(MediaInEndHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(MediaInEndHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(MediaInEndHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var mediaInEndCmd = command.IsA<MediaInEndCommand>($"Invalid parameter in the MediaInEnd Handle method. {nameof(MediaInEndCommand)}");
            mediaInEndCmd.Header.RequestId.HasValue.IsTrue();

            IMediaInEndEvents events = new MediaInEndEvents(Connection, mediaInEndCmd.Header.RequestId.Value);

            var result = await HandleMediaInEnd(events, mediaInEndCmd, cancel);
            await Connection.SendMessageAsync(new MediaInEndCompletion(mediaInEndCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var mediaInEndcommand = command.IsA<MediaInEndCommand>();
            mediaInEndcommand.Header.RequestId.HasValue.IsTrue();

            MediaInEndCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => MediaInEndCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => MediaInEndCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => MediaInEndCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new MediaInEndCompletion(mediaInEndcommand.Header.RequestId.Value, new MediaInEndCompletion.PayloadData(errorCode, commandException.Message));

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
