/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * WriteRawDataHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(WriteRawDataCommand))]
    public partial class WriteRawDataHandler : ICommandHandler
    {
        public WriteRawDataHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(WriteRawDataHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(WriteRawDataHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(WriteRawDataHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var writeRawDataCmd = command.IsA<WriteRawDataCommand>($"Invalid parameter in the WriteRawData Handle method. {nameof(WriteRawDataCommand)}");
            writeRawDataCmd.Header.RequestId.HasValue.IsTrue();

            IWriteRawDataEvents events = new WriteRawDataEvents(Connection, writeRawDataCmd.Header.RequestId.Value);

            var result = await HandleWriteRawData(events, writeRawDataCmd, cancel);
            await Connection.SendMessageAsync(new WriteRawDataCompletion(writeRawDataCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var writeRawDatacommand = command.IsA<WriteRawDataCommand>();
            writeRawDatacommand.Header.RequestId.HasValue.IsTrue();

            WriteRawDataCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TaskCanceledException or OperationCanceledException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.Canceled,
                _ => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new WriteRawDataCompletion(writeRawDatacommand.Header.RequestId.Value, new WriteRawDataCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
