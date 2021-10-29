/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * DeriveKeyHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(DeriveKeyCommand))]
    public partial class DeriveKeyHandler : ICommandHandler
    {
        public DeriveKeyHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DeriveKeyHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DeriveKeyHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DeriveKeyHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var deriveKeyCmd = command.IsA<DeriveKeyCommand>($"Invalid parameter in the DeriveKey Handle method. {nameof(DeriveKeyCommand)}");
            deriveKeyCmd.Header.RequestId.HasValue.IsTrue();

            IDeriveKeyEvents events = new DeriveKeyEvents(Connection, deriveKeyCmd.Header.RequestId.Value);

            var result = await HandleDeriveKey(events, deriveKeyCmd, cancel);
            await Connection.SendMessageAsync(new DeriveKeyCompletion(deriveKeyCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var deriveKeycommand = command.IsA<DeriveKeyCommand>();
            deriveKeycommand.Header.RequestId.HasValue.IsTrue();

            DeriveKeyCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DeriveKeyCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => DeriveKeyCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => DeriveKeyCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => DeriveKeyCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => DeriveKeyCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DeriveKeyCompletion(deriveKeycommand.Header.RequestId.Value, new DeriveKeyCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementServiceClass KeyManagement { get; }
        private ILogger Logger { get; }
    }

}
