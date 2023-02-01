/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * RegisterHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.VendorMode.Commands;
using XFS4IoT.VendorMode.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.VendorMode
{
    [CommandHandler(XFSConstants.ServiceClass.VendorMode, typeof(RegisterCommand))]
    public partial class RegisterHandler : ICommandHandler
    {
        public RegisterHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(RegisterHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(RegisterHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IVendorModeDevice>();

            VendorMode = Provider.IsA<IVendorModeService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(RegisterHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(RegisterHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var registerCmd = command.IsA<RegisterCommand>($"Invalid parameter in the Register Handle method. {nameof(RegisterCommand)}");
            registerCmd.Header.RequestId.HasValue.IsTrue();

            IRegisterEvents events = new RegisterEvents(Connection, registerCmd.Header.RequestId.Value);

            var result = await HandleRegister(events, registerCmd, cancel);
            await Connection.SendMessageAsync(new RegisterCompletion(registerCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var registercommand = command.IsA<RegisterCommand>();
            registercommand.Header.RequestId.HasValue.IsTrue();

            RegisterCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => RegisterCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => RegisterCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => RegisterCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => RegisterCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => RegisterCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => RegisterCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => RegisterCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => RegisterCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => RegisterCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => RegisterCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => RegisterCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => RegisterCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => RegisterCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => RegisterCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => RegisterCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new RegisterCompletion(registercommand.Header.RequestId.Value, new RegisterCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IVendorModeDevice Device { get => Provider.Device.IsA<IVendorModeDevice>(); }
        private IServiceProvider Provider { get; }
        private IVendorModeService VendorMode { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
