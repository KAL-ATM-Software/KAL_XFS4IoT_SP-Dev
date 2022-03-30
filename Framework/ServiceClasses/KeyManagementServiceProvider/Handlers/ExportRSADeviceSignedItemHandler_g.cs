/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ExportRSADeviceSignedItemHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(ExportRSADeviceSignedItemCommand))]
    public partial class ExportRSADeviceSignedItemHandler : ICommandHandler
    {
        public ExportRSADeviceSignedItemHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ExportRSADeviceSignedItemHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ExportRSADeviceSignedItemHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ExportRSADeviceSignedItemHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ExportRSADeviceSignedItemHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var exportRSADeviceSignedItemCmd = command.IsA<ExportRSADeviceSignedItemCommand>($"Invalid parameter in the ExportRSADeviceSignedItem Handle method. {nameof(ExportRSADeviceSignedItemCommand)}");
            exportRSADeviceSignedItemCmd.Header.RequestId.HasValue.IsTrue();

            IExportRSADeviceSignedItemEvents events = new ExportRSADeviceSignedItemEvents(Connection, exportRSADeviceSignedItemCmd.Header.RequestId.Value);

            var result = await HandleExportRSADeviceSignedItem(events, exportRSADeviceSignedItemCmd, cancel);
            await Connection.SendMessageAsync(new ExportRSADeviceSignedItemCompletion(exportRSADeviceSignedItemCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var exportRSADeviceSignedItemcommand = command.IsA<ExportRSADeviceSignedItemCommand>();
            exportRSADeviceSignedItemcommand.Header.RequestId.HasValue.IsTrue();

            ExportRSADeviceSignedItemCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ExportRSADeviceSignedItemCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ExportRSADeviceSignedItemCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ExportRSADeviceSignedItemCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ExportRSADeviceSignedItemCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ExportRSADeviceSignedItemCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ExportRSADeviceSignedItemCompletion(exportRSADeviceSignedItemcommand.Header.RequestId.Value, new ExportRSADeviceSignedItemCompletion.PayloadData(errorCode, commandException.Message));

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
