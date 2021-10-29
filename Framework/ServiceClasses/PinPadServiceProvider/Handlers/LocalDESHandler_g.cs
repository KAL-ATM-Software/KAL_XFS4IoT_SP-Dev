/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * LocalDESHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.PinPad
{
    [CommandHandler(XFSConstants.ServiceClass.PinPad, typeof(LocalDESCommand))]
    public partial class LocalDESHandler : ICommandHandler
    {
        public LocalDESHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(LocalDESHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(LocalDESHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IPinPadDevice>();

            PinPad = Provider.IsA<IPinPadServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(LocalDESHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var localDESCmd = command.IsA<LocalDESCommand>($"Invalid parameter in the LocalDES Handle method. {nameof(LocalDESCommand)}");
            localDESCmd.Header.RequestId.HasValue.IsTrue();

            ILocalDESEvents events = new LocalDESEvents(Connection, localDESCmd.Header.RequestId.Value);

            var result = await HandleLocalDES(events, localDESCmd, cancel);
            await Connection.SendMessageAsync(new LocalDESCompletion(localDESCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var localDEScommand = command.IsA<LocalDESCommand>();
            localDEScommand.Header.RequestId.HasValue.IsTrue();

            LocalDESCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => LocalDESCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => LocalDESCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => LocalDESCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => LocalDESCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => LocalDESCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new LocalDESCompletion(localDEScommand.Header.RequestId.Value, new LocalDESCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private IPinPadDevice Device { get => Provider.Device.IsA<IPinPadDevice>(); }
        private IServiceProvider Provider { get; }
        private IPinPadServiceClass PinPad { get; }
        private ILogger Logger { get; }
    }

}
