/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * SetStorageHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Storage.Commands;
using XFS4IoT.Storage.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Storage
{
    [CommandHandler(XFSConstants.ServiceClass.Storage, typeof(SetStorageCommand))]
    public partial class SetStorageHandler : ICommandHandler
    {
        public SetStorageHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetStorageHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetStorageHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IStorageDevice>();

            Storage = Provider.IsA<IStorageServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetStorageHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var setStorageCmd = command.IsA<SetStorageCommand>($"Invalid parameter in the SetStorage Handle method. {nameof(SetStorageCommand)}");
            setStorageCmd.Header.RequestId.HasValue.IsTrue();

            ISetStorageEvents events = new SetStorageEvents(Connection, setStorageCmd.Header.RequestId.Value);

            var result = await HandleSetStorage(events, setStorageCmd, cancel);
            await Connection.SendMessageAsync(new SetStorageCompletion(setStorageCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var setStoragecommand = command.IsA<SetStorageCommand>();
            setStoragecommand.Header.RequestId.HasValue.IsTrue();

            SetStorageCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetStorageCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => SetStorageCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetStorageCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetStorageCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetStorageCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetStorageCompletion(setStoragecommand.Header.RequestId.Value, new SetStorageCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IStorageDevice Device { get => Provider.Device.IsA<IStorageDevice>(); }
        private IServiceProvider Provider { get; }
        private IStorageServiceClass Storage { get; }
        private ILogger Logger { get; }
    }

}
