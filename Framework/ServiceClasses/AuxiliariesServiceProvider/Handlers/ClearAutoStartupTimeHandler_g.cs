/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * ClearAutoStartUpTimeHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Auxiliaries.Commands;
using XFS4IoT.Auxiliaries.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Auxiliaries
{
    [CommandHandler(XFSConstants.ServiceClass.Auxiliaries, typeof(ClearAutoStartUpTimeCommand))]
    public partial class ClearAutoStartUpTimeHandler : ICommandHandler
    {
        public ClearAutoStartUpTimeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ClearAutoStartUpTimeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ClearAutoStartUpTimeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IAuxiliariesDevice>();

            Auxiliaries = Provider.IsA<IAuxiliariesService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ClearAutoStartUpTimeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ClearAutoStartUpTimeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var clearAutoStartUpTimeCmd = command.IsA<ClearAutoStartUpTimeCommand>($"Invalid parameter in the ClearAutoStartUpTime Handle method. {nameof(ClearAutoStartUpTimeCommand)}");
            clearAutoStartUpTimeCmd.Header.RequestId.HasValue.IsTrue();

            IClearAutoStartUpTimeEvents events = new ClearAutoStartUpTimeEvents(Connection, clearAutoStartUpTimeCmd.Header.RequestId.Value);

            var result = await HandleClearAutoStartUpTime(events, clearAutoStartUpTimeCmd, cancel);
            await Connection.SendMessageAsync(new ClearAutoStartUpTimeCompletion(clearAutoStartUpTimeCmd.Header.RequestId.Value, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var clearAutoStartUpTimeCommand = command.IsA<ClearAutoStartUpTimeCommand>();
            clearAutoStartUpTimeCommand.Header.RequestId.HasValue.IsTrue();

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

            var response = new ClearAutoStartUpTimeCompletion(clearAutoStartUpTimeCommand.Header.RequestId.Value, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IAuxiliariesDevice Device { get => Provider.Device.IsA<IAuxiliariesDevice>(); }
        private IServiceProvider Provider { get; }
        private IAuxiliariesService Auxiliaries { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
