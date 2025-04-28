/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT BanknoteNeutralization interface.
 * TriggerNeutralizationHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.BanknoteNeutralization.Commands;
using XFS4IoT.BanknoteNeutralization.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.BanknoteNeutralization
{
    [CommandHandler(XFSConstants.ServiceClass.BanknoteNeutralization, typeof(TriggerNeutralizationCommand))]
    public partial class TriggerNeutralizationHandler : ICommandHandler
    {
        public TriggerNeutralizationHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(TriggerNeutralizationHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(TriggerNeutralizationHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IBanknoteNeutralizationDevice>();

            BanknoteNeutralization = Provider.IsA<IBanknoteNeutralizationService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(TriggerNeutralizationHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(TriggerNeutralizationHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var triggerNeutralizationCmd = command.IsA<TriggerNeutralizationCommand>($"Invalid parameter in the TriggerNeutralization Handle method. {nameof(TriggerNeutralizationCommand)}");
            triggerNeutralizationCmd.Header.RequestId.HasValue.IsTrue();

            ITriggerNeutralizationEvents events = new TriggerNeutralizationEvents(Connection, triggerNeutralizationCmd.Header.RequestId.Value);

            var result = await HandleTriggerNeutralization(events, triggerNeutralizationCmd, cancel);
            await Connection.SendMessageAsync(new TriggerNeutralizationCompletion(triggerNeutralizationCmd.Header.RequestId.Value, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var triggerNeutralizationCommand = command.IsA<TriggerNeutralizationCommand>();
            triggerNeutralizationCommand.Header.RequestId.HasValue.IsTrue();

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

            var response = new TriggerNeutralizationCompletion(triggerNeutralizationCommand.Header.RequestId.Value, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IBanknoteNeutralizationDevice Device { get => Provider.Device.IsA<IBanknoteNeutralizationDevice>(); }
        private IServiceProvider Provider { get; }
        private IBanknoteNeutralizationService BanknoteNeutralization { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
