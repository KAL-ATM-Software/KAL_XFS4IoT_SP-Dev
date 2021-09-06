/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * StartAuthenticateHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(StartAuthenticateCommand))]
    public partial class StartAuthenticateHandler : ICommandHandler
    {
        public StartAuthenticateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(StartAuthenticateHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(StartAuthenticateHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(StartAuthenticateHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var startAuthenticateCmd = command.IsA<StartAuthenticateCommand>($"Invalid parameter in the StartAuthenticate Handle method. {nameof(StartAuthenticateCommand)}");
            startAuthenticateCmd.Header.RequestId.HasValue.IsTrue();

            IStartAuthenticateEvents events = new StartAuthenticateEvents(Connection, startAuthenticateCmd.Header.RequestId.Value);

            var result = await HandleStartAuthenticate(events, startAuthenticateCmd, cancel);
            await Connection.SendMessageAsync(new StartAuthenticateCompletion(startAuthenticateCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var startAuthenticatecommand = command.IsA<StartAuthenticateCommand>();
            startAuthenticatecommand.Header.RequestId.HasValue.IsTrue();

            StartAuthenticateCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => StartAuthenticateCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => StartAuthenticateCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => StartAuthenticateCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => StartAuthenticateCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new StartAuthenticateCompletion(startAuthenticatecommand.Header.RequestId.Value, new StartAuthenticateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementServiceClass KeyManagement { get; }
        private ILogger Logger { get; }
    }

}
