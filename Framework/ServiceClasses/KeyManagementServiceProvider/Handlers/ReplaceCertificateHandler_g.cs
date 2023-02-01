/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ReplaceCertificateHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.KeyManagement
{
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(ReplaceCertificateCommand))]
    public partial class ReplaceCertificateHandler : ICommandHandler
    {
        public ReplaceCertificateHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReplaceCertificateHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ReplaceCertificateHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ReplaceCertificateHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ReplaceCertificateHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var replaceCertificateCmd = command.IsA<ReplaceCertificateCommand>($"Invalid parameter in the ReplaceCertificate Handle method. {nameof(ReplaceCertificateCommand)}");
            replaceCertificateCmd.Header.RequestId.HasValue.IsTrue();

            IReplaceCertificateEvents events = new ReplaceCertificateEvents(Connection, replaceCertificateCmd.Header.RequestId.Value);

            var result = await HandleReplaceCertificate(events, replaceCertificateCmd, cancel);
            await Connection.SendMessageAsync(new ReplaceCertificateCompletion(replaceCertificateCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var replaceCertificatecommand = command.IsA<ReplaceCertificateCommand>();
            replaceCertificatecommand.Header.RequestId.HasValue.IsTrue();

            ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ReplaceCertificateCompletion(replaceCertificatecommand.Header.RequestId.Value, new ReplaceCertificateCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementService KeyManagement { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
