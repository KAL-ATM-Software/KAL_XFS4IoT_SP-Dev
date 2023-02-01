/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * StartLocalApplicationHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.VendorApplication, typeof(StartLocalApplicationCommand))]
    public partial class StartLocalApplicationHandler : ICommandHandler
    {
        public StartLocalApplicationHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(StartLocalApplicationHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(StartLocalApplicationHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IVendorApplicationDevice>();

            VendorApplication = Provider.IsA<IVendorApplicationService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(StartLocalApplicationHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(StartLocalApplicationHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var startLocalApplicationCmd = command.IsA<StartLocalApplicationCommand>($"Invalid parameter in the StartLocalApplication Handle method. {nameof(StartLocalApplicationCommand)}");
            startLocalApplicationCmd.Header.RequestId.HasValue.IsTrue();

            IStartLocalApplicationEvents events = new StartLocalApplicationEvents(Connection, startLocalApplicationCmd.Header.RequestId.Value);

            var result = await HandleStartLocalApplication(events, startLocalApplicationCmd, cancel);
            await Connection.SendMessageAsync(new StartLocalApplicationCompletion(startLocalApplicationCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var startLocalApplicationcommand = command.IsA<StartLocalApplicationCommand>();
            startLocalApplicationcommand.Header.RequestId.HasValue.IsTrue();

            StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => StartLocalApplicationCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new StartLocalApplicationCompletion(startLocalApplicationcommand.Header.RequestId.Value, new StartLocalApplicationCompletion.PayloadData(errorCode, commandException.Message));

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
