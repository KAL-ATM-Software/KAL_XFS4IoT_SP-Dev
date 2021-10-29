/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * MoveHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CardReader
{
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(MoveCommand))]
    public partial class MoveHandler : ICommandHandler
    {
        public MoveHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(MoveHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(MoveHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(MoveHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var moveCmd = command.IsA<MoveCommand>($"Invalid parameter in the Move Handle method. {nameof(MoveCommand)}");
            moveCmd.Header.RequestId.HasValue.IsTrue();

            IMoveEvents events = new MoveEvents(Connection, moveCmd.Header.RequestId.Value);

            var result = await HandleMove(events, moveCmd, cancel);
            await Connection.SendMessageAsync(new MoveCompletion(moveCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var movecommand = command.IsA<MoveCommand>();
            movecommand.Header.RequestId.HasValue.IsTrue();

            MoveCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => MoveCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => MoveCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => MoveCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => MoveCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => MoveCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new MoveCompletion(movecommand.Header.RequestId.Value, new MoveCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
