/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorMode interface.
 * EnterModeAcknowledgeHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.VendorMode.Commands;
using XFS4IoT.VendorMode.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.VendorMode
{
    [CommandHandler(XFSConstants.ServiceClass.VendorMode, typeof(EnterModeAcknowledgeCommand))]
    public partial class EnterModeAcknowledgeHandler : ICommandHandler
    {
        public EnterModeAcknowledgeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EnterModeAcknowledgeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(EnterModeAcknowledgeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IVendorModeDevice>();

            VendorMode = Provider.IsA<IVendorModeService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(EnterModeAcknowledgeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(EnterModeAcknowledgeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var enterModeAcknowledgeCmd = command.IsA<EnterModeAcknowledgeCommand>($"Invalid parameter in the EnterModeAcknowledge Handle method. {nameof(EnterModeAcknowledgeCommand)}");
            enterModeAcknowledgeCmd.Header.RequestId.HasValue.IsTrue();

            IEnterModeAcknowledgeEvents events = new EnterModeAcknowledgeEvents(Connection, enterModeAcknowledgeCmd.Header.RequestId.Value);

            var result = await HandleEnterModeAcknowledge(events, enterModeAcknowledgeCmd, cancel);
            await Connection.SendMessageAsync(new EnterModeAcknowledgeCompletion(enterModeAcknowledgeCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var enterModeAcknowledgecommand = command.IsA<EnterModeAcknowledgeCommand>();
            enterModeAcknowledgecommand.Header.RequestId.HasValue.IsTrue();

            EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => EnterModeAcknowledgeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new EnterModeAcknowledgeCompletion(enterModeAcknowledgecommand.Header.RequestId.Value, new EnterModeAcknowledgeCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IVendorModeDevice Device { get => Provider.Device.IsA<IVendorModeDevice>(); }
        private IServiceProvider Provider { get; }
        private IVendorModeService VendorMode { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
