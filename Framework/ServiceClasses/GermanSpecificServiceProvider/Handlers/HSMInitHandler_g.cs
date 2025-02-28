/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT GermanSpecific interface.
 * HSMInitHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.GermanSpecific.Commands;
using XFS4IoT.GermanSpecific.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.GermanSpecific
{
    [CommandHandler(XFSConstants.ServiceClass.GermanSpecific, typeof(HSMInitCommand))]
    public partial class HSMInitHandler : ICommandHandler
    {
        public HSMInitHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(HSMInitHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(HSMInitHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IGermanSpecificDevice>();

            GermanSpecific = Provider.IsA<IGermanSpecificService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(HSMInitHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(HSMInitHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var hSMInitCmd = command.IsA<HSMInitCommand>($"Invalid parameter in the HSMInit Handle method. {nameof(HSMInitCommand)}");
            hSMInitCmd.Header.RequestId.HasValue.IsTrue();

            IHSMInitEvents events = new HSMInitEvents(Connection, hSMInitCmd.Header.RequestId.Value);

            var result = await HandleHSMInit(events, hSMInitCmd, cancel);
            await Connection.SendMessageAsync(new HSMInitCompletion(hSMInitCmd.Header.RequestId.Value, result.Payload, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var hSMInitCommand = command.IsA<HSMInitCommand>();
            hSMInitCommand.Header.RequestId.HasValue.IsTrue();

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

            var response = new HSMInitCompletion(hSMInitCommand.Header.RequestId.Value, null, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IGermanSpecificDevice Device { get => Provider.Device.IsA<IGermanSpecificDevice>(); }
        private IServiceProvider Provider { get; }
        private IGermanSpecificService GermanSpecific { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
