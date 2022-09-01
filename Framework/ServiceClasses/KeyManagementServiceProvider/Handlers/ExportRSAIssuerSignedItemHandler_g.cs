/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ExportRSAIssuerSignedItemHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(ExportRSAIssuerSignedItemCommand))]
    public partial class ExportRSAIssuerSignedItemHandler : ICommandHandler
    {
        public ExportRSAIssuerSignedItemHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ExportRSAIssuerSignedItemHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ExportRSAIssuerSignedItemHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ExportRSAIssuerSignedItemHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ExportRSAIssuerSignedItemHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var exportRSAIssuerSignedItemCmd = command.IsA<ExportRSAIssuerSignedItemCommand>($"Invalid parameter in the ExportRSAIssuerSignedItem Handle method. {nameof(ExportRSAIssuerSignedItemCommand)}");
            exportRSAIssuerSignedItemCmd.Header.RequestId.HasValue.IsTrue();

            IExportRSAIssuerSignedItemEvents events = new ExportRSAIssuerSignedItemEvents(Connection, exportRSAIssuerSignedItemCmd.Header.RequestId.Value);

            var result = await HandleExportRSAIssuerSignedItem(events, exportRSAIssuerSignedItemCmd, cancel);
            await Connection.SendMessageAsync(new ExportRSAIssuerSignedItemCompletion(exportRSAIssuerSignedItemCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var exportRSAIssuerSignedItemcommand = command.IsA<ExportRSAIssuerSignedItemCommand>();
            exportRSAIssuerSignedItemcommand.Header.RequestId.HasValue.IsTrue();

            ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ExportRSAIssuerSignedItemCompletion(exportRSAIssuerSignedItemcommand.Header.RequestId.Value, new ExportRSAIssuerSignedItemCompletion.PayloadData(errorCode, commandException.Message));

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
