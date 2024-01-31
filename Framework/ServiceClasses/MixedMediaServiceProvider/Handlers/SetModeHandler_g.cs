/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT MixedMedia interface.
 * SetModeHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.MixedMedia.Commands;
using XFS4IoT.MixedMedia.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.MixedMedia
{
    [CommandHandler(XFSConstants.ServiceClass.MixedMedia, typeof(SetModeCommand))]
    public partial class SetModeHandler : ICommandHandler
    {
        public SetModeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetModeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetModeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IMixedMediaDevice>();

            MixedMedia = Provider.IsA<IMixedMediaService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetModeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetModeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setModeCmd = command.IsA<SetModeCommand>($"Invalid parameter in the SetMode Handle method. {nameof(SetModeCommand)}");
            setModeCmd.Header.RequestId.HasValue.IsTrue();

            ISetModeEvents events = new SetModeEvents(Connection, setModeCmd.Header.RequestId.Value);

            var result = await HandleSetMode(events, setModeCmd, cancel);
            await Connection.SendMessageAsync(new SetModeCompletion(setModeCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setModecommand = command.IsA<SetModeCommand>();
            setModecommand.Header.RequestId.HasValue.IsTrue();

            SetModeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetModeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => SetModeCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => SetModeCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => SetModeCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => SetModeCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => SetModeCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => SetModeCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => SetModeCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => SetModeCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => SetModeCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => SetModeCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => SetModeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetModeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetModeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetModeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetModeCompletion(setModecommand.Header.RequestId.Value, new SetModeCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IMixedMediaDevice Device { get => Provider.Device.IsA<IMixedMediaDevice>(); }
        private IServiceProvider Provider { get; }
        private IMixedMediaService MixedMedia { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
