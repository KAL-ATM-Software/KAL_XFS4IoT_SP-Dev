/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * SetActiveInterfaceHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.VendorApplication.Commands;
using XFS4IoT.VendorApplication.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.VendorApplication
{
    [CommandHandler(XFSConstants.ServiceClass.VendorApplication, typeof(SetActiveInterfaceCommand))]
    public partial class SetActiveInterfaceHandler : ICommandHandler
    {
        public SetActiveInterfaceHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetActiveInterfaceHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetActiveInterfaceHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IVendorApplicationDevice>();

            VendorApplication = Provider.IsA<IVendorApplicationService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetActiveInterfaceHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetActiveInterfaceHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setActiveInterfaceCmd = command.IsA<SetActiveInterfaceCommand>($"Invalid parameter in the SetActiveInterface Handle method. {nameof(SetActiveInterfaceCommand)}");
            setActiveInterfaceCmd.Header.RequestId.HasValue.IsTrue();

            ISetActiveInterfaceEvents events = new SetActiveInterfaceEvents(Connection, setActiveInterfaceCmd.Header.RequestId.Value);

            var result = await HandleSetActiveInterface(events, setActiveInterfaceCmd, cancel);
            await Connection.SendMessageAsync(new SetActiveInterfaceCompletion(setActiveInterfaceCmd.Header.RequestId.Value, result.CompletionCode, result.ErrorDescription));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setActiveInterfaceCommand = command.IsA<SetActiveInterfaceCommand>();
            setActiveInterfaceCommand.Header.RequestId.HasValue.IsTrue();

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

            var response = new SetActiveInterfaceCompletion(setActiveInterfaceCommand.Header.RequestId.Value, errorCode, commandException.Message);

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IVendorApplicationDevice Device { get => Provider.Device.IsA<IVendorApplicationDevice>(); }
        private IServiceProvider Provider { get; }
        private IVendorApplicationService VendorApplication { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
