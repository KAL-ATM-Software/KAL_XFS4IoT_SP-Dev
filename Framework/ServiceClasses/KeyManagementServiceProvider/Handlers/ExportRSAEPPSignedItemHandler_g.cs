/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ExportRSAEPPSignedItemHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(ExportRSAEPPSignedItemCommand))]
    public partial class ExportRSAEPPSignedItemHandler : ICommandHandler
    {
        public ExportRSAEPPSignedItemHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ExportRSAEPPSignedItemHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ExportRSAEPPSignedItemHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ExportRSAEPPSignedItemHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var exportRSAEPPSignedItemCmd = command.IsA<ExportRSAEPPSignedItemCommand>($"Invalid parameter in the ExportRSAEPPSignedItem Handle method. {nameof(ExportRSAEPPSignedItemCommand)}");
            exportRSAEPPSignedItemCmd.Header.RequestId.HasValue.IsTrue();

            IExportRSAEPPSignedItemEvents events = new ExportRSAEPPSignedItemEvents(Connection, exportRSAEPPSignedItemCmd.Header.RequestId.Value);

            var result = await HandleExportRSAEPPSignedItem(events, exportRSAEPPSignedItemCmd, cancel);
            await Connection.SendMessageAsync(new ExportRSAEPPSignedItemCompletion(exportRSAEPPSignedItemCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var exportRSAEPPSignedItemcommand = command.IsA<ExportRSAEPPSignedItemCommand>();
            exportRSAEPPSignedItemcommand.Header.RequestId.HasValue.IsTrue();

            ExportRSAEPPSignedItemCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ExportRSAEPPSignedItemCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ExportRSAEPPSignedItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ExportRSAEPPSignedItemCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ExportRSAEPPSignedItemCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ExportRSAEPPSignedItemCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ExportRSAEPPSignedItemCompletion(exportRSAEPPSignedItemcommand.Header.RequestId.Value, new ExportRSAEPPSignedItemCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementServiceClass KeyManagement { get; }
        private ILogger Logger { get; }
    }

}
