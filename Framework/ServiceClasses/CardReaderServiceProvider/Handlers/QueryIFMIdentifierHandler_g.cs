/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * QueryIFMIdentifierHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(QueryIFMIdentifierCommand))]
    public partial class QueryIFMIdentifierHandler : ICommandHandler
    {
        public QueryIFMIdentifierHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(QueryIFMIdentifierHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(QueryIFMIdentifierHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(QueryIFMIdentifierHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(QueryIFMIdentifierHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var queryIFMIdentifierCmd = command.IsA<QueryIFMIdentifierCommand>($"Invalid parameter in the QueryIFMIdentifier Handle method. {nameof(QueryIFMIdentifierCommand)}");
            queryIFMIdentifierCmd.Header.RequestId.HasValue.IsTrue();

            IQueryIFMIdentifierEvents events = new QueryIFMIdentifierEvents(Connection, queryIFMIdentifierCmd.Header.RequestId.Value);

            var result = await HandleQueryIFMIdentifier(events, queryIFMIdentifierCmd, cancel);
            await Connection.SendMessageAsync(new QueryIFMIdentifierCompletion(queryIFMIdentifierCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var queryIFMIdentifiercommand = command.IsA<QueryIFMIdentifierCommand>();
            queryIFMIdentifiercommand.Header.RequestId.HasValue.IsTrue();

            QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new QueryIFMIdentifierCompletion(queryIFMIdentifiercommand.Header.RequestId.Value, new QueryIFMIdentifierCompletion.PayloadData(errorCode, commandException.Message));

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
