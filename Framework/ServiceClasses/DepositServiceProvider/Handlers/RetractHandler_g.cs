/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * RetractHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Deposit.Commands;
using XFS4IoT.Deposit.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Deposit
{
    [CommandHandler(XFSConstants.ServiceClass.Deposit, typeof(RetractCommand))]
    public partial class RetractHandler : ICommandHandler
    {
        public RetractHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(RetractHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(RetractHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IDepositDevice>();

            Deposit = Provider.IsA<IDepositService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(RetractHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(RetractHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var retractCmd = command.IsA<RetractCommand>($"Invalid parameter in the Retract Handle method. {nameof(RetractCommand)}");
            retractCmd.Header.RequestId.HasValue.IsTrue();

            IRetractEvents events = new RetractEvents(Connection, retractCmd.Header.RequestId.Value);

            var result = await HandleRetract(events, retractCmd, cancel);
            await Connection.SendMessageAsync(new RetractCompletion(retractCmd.Header.RequestId.Value, result.Payload, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var retractCommand = command.IsA<RetractCommand>();
            retractCommand.Header.RequestId.HasValue.IsTrue();

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

            var response = new RetractCompletion(retractCommand.Header.RequestId.Value, null, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IDepositDevice Device { get => Provider.Device.IsA<IDepositDevice>(); }
        private IServiceProvider Provider { get; }
        private IDepositService Deposit { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
