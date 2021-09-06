/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * LoadCertificateHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(LoadCertificateCommand))]
    public partial class LoadCertificateHandler : ICommandHandler
    {
        public LoadCertificateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(LoadCertificateHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(LoadCertificateHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(LoadCertificateHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var loadCertificateCmd = command.IsA<LoadCertificateCommand>($"Invalid parameter in the LoadCertificate Handle method. {nameof(LoadCertificateCommand)}");
            loadCertificateCmd.Header.RequestId.HasValue.IsTrue();

            ILoadCertificateEvents events = new LoadCertificateEvents(Connection, loadCertificateCmd.Header.RequestId.Value);

            var result = await HandleLoadCertificate(events, loadCertificateCmd, cancel);
            await Connection.SendMessageAsync(new LoadCertificateCompletion(loadCertificateCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var loadCertificatecommand = command.IsA<LoadCertificateCommand>();
            loadCertificatecommand.Header.RequestId.HasValue.IsTrue();

            LoadCertificateCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => LoadCertificateCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => LoadCertificateCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => LoadCertificateCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => LoadCertificateCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new LoadCertificateCompletion(loadCertificatecommand.Header.RequestId.Value, new LoadCertificateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementServiceClass KeyManagement { get; }
        private ILogger Logger { get; }
    }

}
