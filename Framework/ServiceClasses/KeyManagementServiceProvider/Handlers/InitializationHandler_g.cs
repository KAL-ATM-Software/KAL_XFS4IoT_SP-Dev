/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * InitializationHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(InitializationCommand))]
    public partial class InitializationHandler : ICommandHandler
    {
        public InitializationHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(InitializationHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(InitializationHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(InitializationHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var initializationCmd = command.IsA<InitializationCommand>($"Invalid parameter in the Initialization Handle method. {nameof(InitializationCommand)}");
            initializationCmd.Header.RequestId.HasValue.IsTrue();

            IInitializationEvents events = new InitializationEvents(Connection, initializationCmd.Header.RequestId.Value);

            var result = await HandleInitialization(events, initializationCmd, cancel);
            await Connection.SendMessageAsync(new InitializationCompletion(initializationCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var initializationcommand = command.IsA<InitializationCommand>();
            initializationcommand.Header.RequestId.HasValue.IsTrue();

            InitializationCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => InitializationCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => InitializationCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => InitializationCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => InitializationCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new InitializationCompletion(initializationcommand.Header.RequestId.Value, new InitializationCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementServiceClass KeyManagement { get; }
        private ILogger Logger { get; }
    }

}
