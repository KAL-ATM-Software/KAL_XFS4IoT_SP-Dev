/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * ClearHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Biometric.Commands;
using XFS4IoT.Biometric.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Biometric
{
    [CommandHandler(XFSConstants.ServiceClass.Biometric, typeof(ClearCommand))]
    public partial class ClearHandler : ICommandHandler
    {
        public ClearHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ClearHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ClearHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IBiometricDevice>();

            Biometric = Provider.IsA<IBiometricService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ClearHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ClearHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var clearCmd = command.IsA<ClearCommand>($"Invalid parameter in the Clear Handle method. {nameof(ClearCommand)}");
            clearCmd.Header.RequestId.HasValue.IsTrue();

            IClearEvents events = new ClearEvents(Connection, clearCmd.Header.RequestId.Value);

            var result = await HandleClear(events, clearCmd, cancel);
            await Connection.SendMessageAsync(new ClearCompletion(clearCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var clearcommand = command.IsA<ClearCommand>();
            clearcommand.Header.RequestId.HasValue.IsTrue();

            ClearCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ClearCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ClearCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ClearCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ClearCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ClearCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ClearCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ClearCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ClearCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ClearCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ClearCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ClearCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ClearCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ClearCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ClearCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ClearCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ClearCompletion(clearcommand.Header.RequestId.Value, new ClearCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IBiometricDevice Device { get => Provider.Device.IsA<IBiometricDevice>(); }
        private IServiceProvider Provider { get; }
        private IBiometricService Biometric { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
