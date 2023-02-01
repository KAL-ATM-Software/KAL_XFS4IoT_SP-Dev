/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Camera interface.
 * TakePictureHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Camera.Commands;
using XFS4IoT.Camera.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Camera
{
    [CommandHandler(XFSConstants.ServiceClass.Camera, typeof(TakePictureCommand))]
    public partial class TakePictureHandler : ICommandHandler
    {
        public TakePictureHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(TakePictureHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(TakePictureHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICameraDevice>();

            Camera = Provider.IsA<ICameraService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(TakePictureHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(TakePictureHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var takePictureCmd = command.IsA<TakePictureCommand>($"Invalid parameter in the TakePicture Handle method. {nameof(TakePictureCommand)}");
            takePictureCmd.Header.RequestId.HasValue.IsTrue();

            ITakePictureEvents events = new TakePictureEvents(Connection, takePictureCmd.Header.RequestId.Value);

            var result = await HandleTakePicture(events, takePictureCmd, cancel);
            await Connection.SendMessageAsync(new TakePictureCompletion(takePictureCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var takePicturecommand = command.IsA<TakePictureCommand>();
            takePicturecommand.Header.RequestId.HasValue.IsTrue();

            TakePictureCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => TakePictureCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => TakePictureCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => TakePictureCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => TakePictureCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => TakePictureCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => TakePictureCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => TakePictureCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => TakePictureCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => TakePictureCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => TakePictureCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => TakePictureCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => TakePictureCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => TakePictureCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => TakePictureCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => TakePictureCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new TakePictureCompletion(takePicturecommand.Header.RequestId.Value, new TakePictureCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICameraDevice Device { get => Provider.Device.IsA<ICameraDevice>(); }
        private IServiceProvider Provider { get; }
        private ICameraService Camera { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
