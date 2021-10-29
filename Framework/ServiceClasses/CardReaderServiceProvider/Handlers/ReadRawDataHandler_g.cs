/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ReadRawDataHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(ReadRawDataCommand))]
    public partial class ReadRawDataHandler : ICommandHandler
    {
        public ReadRawDataHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadRawDataHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ReadRawDataHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ReadRawDataHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var readRawDataCmd = command.IsA<ReadRawDataCommand>($"Invalid parameter in the ReadRawData Handle method. {nameof(ReadRawDataCommand)}");
            readRawDataCmd.Header.RequestId.HasValue.IsTrue();

            IReadRawDataEvents events = new ReadRawDataEvents(Connection, readRawDataCmd.Header.RequestId.Value);

            var result = await HandleReadRawData(events, readRawDataCmd, cancel);
            await Connection.SendMessageAsync(new ReadRawDataCompletion(readRawDataCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var readRawDatacommand = command.IsA<ReadRawDataCommand>();
            readRawDatacommand.Header.RequestId.HasValue.IsTrue();

            ReadRawDataCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ReadRawDataCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ReadRawDataCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ReadRawDataCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ReadRawDataCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ReadRawDataCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ReadRawDataCompletion(readRawDatacommand.Header.RequestId.Value, new ReadRawDataCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
