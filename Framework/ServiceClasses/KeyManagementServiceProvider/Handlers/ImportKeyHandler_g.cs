/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * ImportKeyHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(ImportKeyCommand))]
    public partial class ImportKeyHandler : ICommandHandler
    {
        public ImportKeyHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ImportKeyHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ImportKeyHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ImportKeyHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var importKeyCmd = command.IsA<ImportKeyCommand>($"Invalid parameter in the ImportKey Handle method. {nameof(ImportKeyCommand)}");
            importKeyCmd.Header.RequestId.HasValue.IsTrue();

            IImportKeyEvents events = new ImportKeyEvents(Connection, importKeyCmd.Header.RequestId.Value);

            var result = await HandleImportKey(events, importKeyCmd, cancel);
            await Connection.SendMessageAsync(new ImportKeyCompletion(importKeyCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var importKeycommand = command.IsA<ImportKeyCommand>();
            importKeycommand.Header.RequestId.HasValue.IsTrue();

            ImportKeyCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ImportKeyCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ImportKeyCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ImportKeyCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ImportKeyCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ImportKeyCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ImportKeyCompletion(importKeycommand.Header.RequestId.Value, new ImportKeyCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementServiceClass KeyManagement { get; }
        private ILogger Logger { get; }
    }

}
