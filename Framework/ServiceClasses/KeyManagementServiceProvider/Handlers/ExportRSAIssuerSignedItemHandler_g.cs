/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.KeyManagement
{
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(ExportRSAIssuerSignedItemCommand))]
    public partial class ExportRSAIssuerSignedItemHandler : ICommandHandler
    {
        public ExportRSAIssuerSignedItemHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ExportRSAIssuerSignedItemHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ExportRSAIssuerSignedItemHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ExportRSAIssuerSignedItemHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var exportRSAIssuerSignedItemCmd = command.IsA<ExportRSAIssuerSignedItemCommand>($"Invalid parameter in the ExportRSAIssuerSignedItem Handle method. {nameof(ExportRSAIssuerSignedItemCommand)}");
            exportRSAIssuerSignedItemCmd.Header.RequestId.HasValue.IsTrue();

            IExportRSAIssuerSignedItemEvents events = new ExportRSAIssuerSignedItemEvents(Connection, exportRSAIssuerSignedItemCmd.Header.RequestId.Value);

            var result = await HandleExportRSAIssuerSignedItem(events, exportRSAIssuerSignedItemCmd, cancel);
            await Connection.SendMessageAsync(new ExportRSAIssuerSignedItemCompletion(exportRSAIssuerSignedItemCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var exportRSAIssuerSignedItemcommand = command.IsA<ExportRSAIssuerSignedItemCommand>();
            exportRSAIssuerSignedItemcommand.Header.RequestId.HasValue.IsTrue();

            ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => ExportRSAIssuerSignedItemCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ExportRSAIssuerSignedItemCompletion(exportRSAIssuerSignedItemcommand.Header.RequestId.Value, new ExportRSAIssuerSignedItemCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementServiceClass KeyManagement { get; }
        private ILogger Logger { get; }
    }

}
