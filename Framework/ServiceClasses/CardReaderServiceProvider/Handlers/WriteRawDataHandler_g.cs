/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using XFS4IoTFramework.Common;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CardReader
{
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(WriteRawDataCommand))]
    public partial class WriteRawDataHandler : ICommandHandler
    {
        public WriteRawDataHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(WriteRawDataHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(WriteRawDataHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(WriteRawDataHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(WriteRawDataHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var writeRawDataCmd = command.IsA<WriteRawDataCommand>($"Invalid parameter in the WriteRawData Handle method. {nameof(WriteRawDataCommand)}");
            writeRawDataCmd.Header.RequestId.HasValue.IsTrue();

            IWriteRawDataEvents events = new WriteRawDataEvents(Connection, writeRawDataCmd.Header.RequestId.Value);

            var result = await HandleWriteRawData(events, writeRawDataCmd, cancel);
            await Connection.SendMessageAsync(new WriteRawDataCompletion(writeRawDataCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var writeRawDatacommand = command.IsA<WriteRawDataCommand>();
            writeRawDatacommand.Header.RequestId.HasValue.IsTrue();

            WriteRawDataCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => WriteRawDataCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new WriteRawDataCompletion(writeRawDatacommand.Header.RequestId.Value, new WriteRawDataCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderService CardReader { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
