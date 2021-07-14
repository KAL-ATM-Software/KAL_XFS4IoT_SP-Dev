/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CardReader
{
    [CommandHandler(XFSConstants.ServiceClass.CardReader, typeof(QueryIFMIdentifierCommand))]
    public partial class QueryIFMIdentifierHandler : ICommandHandler
    {
        public QueryIFMIdentifierHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(QueryIFMIdentifierHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(QueryIFMIdentifierHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICardReaderDevice>();

            CardReader = Provider.IsA<ICardReaderServiceClass>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(QueryIFMIdentifierHandler)} constructor. {nameof(logger)}");
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel)
        {
            var queryIFMIdentifierCmd = command.IsA<QueryIFMIdentifierCommand>($"Invalid parameter in the QueryIFMIdentifier Handle method. {nameof(QueryIFMIdentifierCommand)}");
            queryIFMIdentifierCmd.Header.RequestId.HasValue.IsTrue();

            IQueryIFMIdentifierEvents events = new QueryIFMIdentifierEvents(Connection, queryIFMIdentifierCmd.Header.RequestId.Value);

            var result = await HandleQueryIFMIdentifier(events, queryIFMIdentifierCmd, cancel);
            await Connection.SendMessageAsync(new QueryIFMIdentifierCompletion(queryIFMIdentifierCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            var queryIFMIdentifiercommand = command.IsA<QueryIFMIdentifierCommand>();
            queryIFMIdentifiercommand.Header.RequestId.HasValue.IsTrue();

            QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                _ => QueryIFMIdentifierCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new QueryIFMIdentifierCompletion(queryIFMIdentifiercommand.Header.RequestId.Value, new QueryIFMIdentifierCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private ICardReaderDevice Device { get => Provider.Device.IsA<ICardReaderDevice>(); }
        private IServiceProvider Provider { get; }
        private ICardReaderServiceClass CardReader { get; }
        private ILogger Logger { get; }
    }

}
