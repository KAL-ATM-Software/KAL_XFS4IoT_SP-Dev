/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * SetDataPersistenceHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Biometric, typeof(SetDataPersistenceCommand))]
    public partial class SetDataPersistenceHandler : ICommandHandler
    {
        public SetDataPersistenceHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetDataPersistenceHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetDataPersistenceHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IBiometricDevice>();

            Biometric = Provider.IsA<IBiometricService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetDataPersistenceHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetDataPersistenceHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setDataPersistenceCmd = command.IsA<SetDataPersistenceCommand>($"Invalid parameter in the SetDataPersistence Handle method. {nameof(SetDataPersistenceCommand)}");
            setDataPersistenceCmd.Header.RequestId.HasValue.IsTrue();

            ISetDataPersistenceEvents events = new SetDataPersistenceEvents(Connection, setDataPersistenceCmd.Header.RequestId.Value);

            var result = await HandleSetDataPersistence(events, setDataPersistenceCmd, cancel);
            await Connection.SendMessageAsync(new SetDataPersistenceCompletion(setDataPersistenceCmd.Header.RequestId.Value, result.Payload, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setDataPersistenceCommand = command.IsA<SetDataPersistenceCommand>();
            setDataPersistenceCommand.Header.RequestId.HasValue.IsTrue();

            MessageHeader.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MessageHeader.CompletionCodeEnum.InvalidData,
                InternalErrorException => MessageHeader.CompletionCodeEnum.InternalError,
                UnsupportedDataException => MessageHeader.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => MessageHeader.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => MessageHeader.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => MessageHeader.CompletionCodeEnum.HardwareError,
                UserErrorException => MessageHeader.CompletionCodeEnum.UserError,
                FraudAttemptException => MessageHeader.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => MessageHeader.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => MessageHeader.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => MessageHeader.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => MessageHeader.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => MessageHeader.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => MessageHeader.CompletionCodeEnum.TimeOut,
                _ => MessageHeader.CompletionCodeEnum.InternalError
            };

            var response = new SetDataPersistenceCompletion(setDataPersistenceCommand.Header.RequestId.Value, null, errorCode, commandException.Message);

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
