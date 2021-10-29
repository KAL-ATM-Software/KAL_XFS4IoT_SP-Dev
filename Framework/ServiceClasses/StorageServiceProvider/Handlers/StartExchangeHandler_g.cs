/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * StartExchangeHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Storage, typeof(StartExchangeCommand))]
    public partial class StartExchangeHandler : ICommandHandler
    {
        public StartExchangeHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(StartExchangeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(StartExchangeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IStorageDevice>();

            Storage = Provider.IsA<IStorageServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(StartExchangeHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var startExchangeCmd = command.IsA<StartExchangeCommand>($"Invalid parameter in the StartExchange Handle method. {nameof(StartExchangeCommand)}");
            startExchangeCmd.Header.RequestId.HasValue.IsTrue();

            IStartExchangeEvents events = new StartExchangeEvents(Connection, startExchangeCmd.Header.RequestId.Value);

            var result = await HandleStartExchange(events, startExchangeCmd, cancel);
            await Connection.SendMessageAsync(new StartExchangeCompletion(startExchangeCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var startExchangecommand = command.IsA<StartExchangeCommand>();
            startExchangecommand.Header.RequestId.HasValue.IsTrue();

            StartExchangeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => StartExchangeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => StartExchangeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => StartExchangeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => StartExchangeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => StartExchangeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new StartExchangeCompletion(startExchangecommand.Header.RequestId.Value, new StartExchangeCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IStorageDevice Device { get => Provider.Device.IsA<IStorageDevice>(); }
        private IServiceProvider Provider { get; }
        private IStorageServiceClass Storage { get; }
        private ILogger Logger { get; }
    }

}
