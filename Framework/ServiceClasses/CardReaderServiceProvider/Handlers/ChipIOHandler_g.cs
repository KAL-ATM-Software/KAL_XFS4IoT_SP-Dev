/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ChipIOHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(ChipIOCommand))]
    public partial class ChipIOHandler : ICommandHandler
    {
        public ChipIOHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ChipIOHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ChipIOHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ChipIOHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var chipIOCmd = command.IsA<ChipIOCommand>($"Invalid parameter in the ChipIO Handle method. {nameof(ChipIOCommand)}");
            chipIOCmd.Header.RequestId.HasValue.IsTrue();

            IChipIOEvents events = new ChipIOEvents(Connection, chipIOCmd.Header.RequestId.Value);

            var result = await HandleChipIO(events, chipIOCmd, cancel);
            await Connection.SendMessageAsync(new ChipIOCompletion(chipIOCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var chipIOcommand = command.IsA<ChipIOCommand>();
            chipIOcommand.Header.RequestId.HasValue.IsTrue();

            ChipIOCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ChipIOCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => ChipIOCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => ChipIOCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => ChipIOCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ChipIOCompletion(chipIOcommand.Header.RequestId.Value, new ChipIOCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
