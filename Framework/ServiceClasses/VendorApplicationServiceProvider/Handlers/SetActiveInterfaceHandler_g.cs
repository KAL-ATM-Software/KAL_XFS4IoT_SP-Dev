/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
            await Connection.SendMessageAsync(new SetActiveInterfaceCompletion(setActiveInterfaceCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setActiveInterfacecommand = command.IsA<SetActiveInterfaceCommand>();
            setActiveInterfacecommand.Header.RequestId.HasValue.IsTrue();

            SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetActiveInterfaceCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetActiveInterfaceCompletion(setActiveInterfacecommand.Header.RequestId.Value, new SetActiveInterfaceCompletion.PayloadData(errorCode, commandException.Message));

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
