/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.KeyManagement
{
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(ReplaceCertificateCommand))]
    public partial class ReplaceCertificateHandler : ICommandHandler
    {
        public ReplaceCertificateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReplaceCertificateHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ReplaceCertificateHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ReplaceCertificateHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var replaceCertificateCmd = command.IsA<ReplaceCertificateCommand>($"Invalid parameter in the ReplaceCertificate Handle method. {nameof(ReplaceCertificateCommand)}");
            replaceCertificateCmd.Header.RequestId.HasValue.IsTrue();

            IReplaceCertificateEvents events = new ReplaceCertificateEvents(Connection, replaceCertificateCmd.Header.RequestId.Value);

            var result = await HandleReplaceCertificate(events, replaceCertificateCmd, cancel);
            await Connection.SendMessageAsync(new ReplaceCertificateCompletion(replaceCertificateCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var replaceCertificatecommand = command.IsA<ReplaceCertificateCommand>();
            replaceCertificatecommand.Header.RequestId.HasValue.IsTrue();

            ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => ReplaceCertificateCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ReplaceCertificateCompletion(replaceCertificatecommand.Header.RequestId.Value, new ReplaceCertificateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementServiceClass KeyManagement { get; }
        private ILogger Logger { get; }
    }

}
