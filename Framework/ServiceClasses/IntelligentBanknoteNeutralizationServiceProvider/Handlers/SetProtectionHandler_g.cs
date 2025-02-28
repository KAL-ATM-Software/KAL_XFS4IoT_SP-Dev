/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT IntelligentBanknoteNeutralization interface.
 * SetProtectionHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.IntelligentBanknoteNeutralization.Commands;
using XFS4IoT.IntelligentBanknoteNeutralization.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.IntelligentBanknoteNeutralization
{
    [CommandHandler(XFSConstants.ServiceClass.IntelligentBanknoteNeutralization, typeof(SetProtectionCommand))]
    public partial class SetProtectionHandler : ICommandHandler
    {
        public SetProtectionHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetProtectionHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetProtectionHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IIntelligentBanknoteNeutralizationDevice>();

            IntelligentBanknoteNeutralization = Provider.IsA<IIntelligentBanknoteNeutralizationService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetProtectionHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetProtectionHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setProtectionCmd = command.IsA<SetProtectionCommand>($"Invalid parameter in the SetProtection Handle method. {nameof(SetProtectionCommand)}");
            setProtectionCmd.Header.RequestId.HasValue.IsTrue();

            ISetProtectionEvents events = new SetProtectionEvents(Connection, setProtectionCmd.Header.RequestId.Value);

            var result = await HandleSetProtection(events, setProtectionCmd, cancel);
            await Connection.SendMessageAsync(new SetProtectionCompletion(setProtectionCmd.Header.RequestId.Value, result.Payload, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setProtectionCommand = command.IsA<SetProtectionCommand>();
            setProtectionCommand.Header.RequestId.HasValue.IsTrue();

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

            var response = new SetProtectionCompletion(setProtectionCommand.Header.RequestId.Value, null, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IIntelligentBanknoteNeutralizationDevice Device { get => Provider.Device.IsA<IIntelligentBanknoteNeutralizationDevice>(); }
        private IServiceProvider Provider { get; }
        private IIntelligentBanknoteNeutralizationService IntelligentBanknoteNeutralization { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
