/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTCardReader;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;

namespace CardReader
{
    [CommandHandler(typeof(CardReaderServiceProvider), typeof(FormListCommand))]
    public class FormListHandler : ICommandHandler
    {
        public FormListHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(FormListHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(FormListHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteFormList(Connection, command as FormListCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            FormListCommand formListcommand = command as FormListCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            FormListCompletion response = new FormListCompletion(formListcommand.Headers.RequestId, new FormListCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteFormList(IConnection connection, FormListCommand formList, CancellationToken cancel)
        {
            formList.IsNotNull($"Invalid parameter in the ExecuteFormList method. {nameof(formList)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, formList.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.FormList()");
            Task<FormListCompletion.PayloadData> task = ServiceProvider.Device.FormList(cardReaderConnection, formList.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.FormList() -> {task.Result.CompletionCode}");

            FormListCompletion response = new FormListCompletion(formList.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(QueryFormCommand))]
    public class QueryFormHandler : ICommandHandler
    {
        public QueryFormHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(QueryFormHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(QueryFormHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteQueryForm(Connection, command as QueryFormCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            QueryFormCommand queryFormcommand = command as QueryFormCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            QueryFormCompletion response = new QueryFormCompletion(queryFormcommand.Headers.RequestId, new QueryFormCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteQueryForm(IConnection connection, QueryFormCommand queryForm, CancellationToken cancel)
        {
            queryForm.IsNotNull($"Invalid parameter in the ExecuteQueryForm method. {nameof(queryForm)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, queryForm.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.QueryForm()");
            Task<QueryFormCompletion.PayloadData> task = ServiceProvider.Device.QueryForm(cardReaderConnection, queryForm.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.QueryForm() -> {task.Result.CompletionCode}");

            QueryFormCompletion response = new QueryFormCompletion(queryForm.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(QueryIFMIdentifierCommand))]
    public class QueryIFMIdentifierHandler : ICommandHandler
    {
        public QueryIFMIdentifierHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(QueryIFMIdentifierHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(QueryIFMIdentifierHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteQueryIFMIdentifier(Connection, command as QueryIFMIdentifierCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            QueryIFMIdentifierCommand queryIFMIdentifiercommand = command as QueryIFMIdentifierCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            QueryIFMIdentifierCompletion response = new QueryIFMIdentifierCompletion(queryIFMIdentifiercommand.Headers.RequestId, new QueryIFMIdentifierCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteQueryIFMIdentifier(IConnection connection, QueryIFMIdentifierCommand queryIFMIdentifier, CancellationToken cancel)
        {
            queryIFMIdentifier.IsNotNull($"Invalid parameter in the ExecuteQueryIFMIdentifier method. {nameof(queryIFMIdentifier)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, queryIFMIdentifier.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.QueryIFMIdentifier()");
            Task<QueryIFMIdentifierCompletion.PayloadData> task = ServiceProvider.Device.QueryIFMIdentifier(cardReaderConnection, queryIFMIdentifier.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.QueryIFMIdentifier() -> {task.Result.CompletionCode}");

            QueryIFMIdentifierCompletion response = new QueryIFMIdentifierCompletion(queryIFMIdentifier.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(EMVClessQueryApplicationsCommand))]
    public class EMVClessQueryApplicationsHandler : ICommandHandler
    {
        public EMVClessQueryApplicationsHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EMVClessQueryApplicationsHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(EMVClessQueryApplicationsHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteEMVClessQueryApplications(Connection, command as EMVClessQueryApplicationsCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            EMVClessQueryApplicationsCommand eMVClessQueryApplicationscommand = command as EMVClessQueryApplicationsCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            EMVClessQueryApplicationsCompletion response = new EMVClessQueryApplicationsCompletion(eMVClessQueryApplicationscommand.Headers.RequestId, new EMVClessQueryApplicationsCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteEMVClessQueryApplications(IConnection connection, EMVClessQueryApplicationsCommand eMVClessQueryApplications, CancellationToken cancel)
        {
            eMVClessQueryApplications.IsNotNull($"Invalid parameter in the ExecuteEMVClessQueryApplications method. {nameof(eMVClessQueryApplications)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, eMVClessQueryApplications.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVClessQueryApplications()");
            Task<EMVClessQueryApplicationsCompletion.PayloadData> task = ServiceProvider.Device.EMVClessQueryApplications(cardReaderConnection, eMVClessQueryApplications.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVClessQueryApplications() -> {task.Result.CompletionCode}");

            EMVClessQueryApplicationsCompletion response = new EMVClessQueryApplicationsCompletion(eMVClessQueryApplications.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(ReadTrackCommand))]
    public class ReadTrackHandler : ICommandHandler
    {
        public ReadTrackHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadTrackHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ReadTrackHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteReadTrack(Connection, command as ReadTrackCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ReadTrackCommand readTrackcommand = command as ReadTrackCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            ReadTrackCompletion response = new ReadTrackCompletion(readTrackcommand.Headers.RequestId, new ReadTrackCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteReadTrack(IConnection connection, ReadTrackCommand readTrack, CancellationToken cancel)
        {
            readTrack.IsNotNull($"Invalid parameter in the ExecuteReadTrack method. {nameof(readTrack)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, readTrack.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ReadTrack()");
            Task<ReadTrackCompletion.PayloadData> task = ServiceProvider.Device.ReadTrack(cardReaderConnection, readTrack.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ReadTrack() -> {task.Result.CompletionCode}");

            ReadTrackCompletion response = new ReadTrackCompletion(readTrack.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(WriteTrackCommand))]
    public class WriteTrackHandler : ICommandHandler
    {
        public WriteTrackHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(WriteTrackHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(WriteTrackHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteWriteTrack(Connection, command as WriteTrackCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            WriteTrackCommand writeTrackcommand = command as WriteTrackCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            WriteTrackCompletion response = new WriteTrackCompletion(writeTrackcommand.Headers.RequestId, new WriteTrackCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteWriteTrack(IConnection connection, WriteTrackCommand writeTrack, CancellationToken cancel)
        {
            writeTrack.IsNotNull($"Invalid parameter in the ExecuteWriteTrack method. {nameof(writeTrack)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, writeTrack.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.WriteTrack()");
            Task<WriteTrackCompletion.PayloadData> task = ServiceProvider.Device.WriteTrack(cardReaderConnection, writeTrack.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.WriteTrack() -> {task.Result.CompletionCode}");

            WriteTrackCompletion response = new WriteTrackCompletion(writeTrack.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(EjectCardCommand))]
    public class EjectCardHandler : ICommandHandler
    {
        public EjectCardHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EjectCardHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(EjectCardHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteEjectCard(Connection, command as EjectCardCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            EjectCardCommand ejectCardcommand = command as EjectCardCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            EjectCardCompletion response = new EjectCardCompletion(ejectCardcommand.Headers.RequestId, new EjectCardCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteEjectCard(IConnection connection, EjectCardCommand ejectCard, CancellationToken cancel)
        {
            ejectCard.IsNotNull($"Invalid parameter in the ExecuteEjectCard method. {nameof(ejectCard)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, ejectCard.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EjectCard()");
            Task<EjectCardCompletion.PayloadData> task = ServiceProvider.Device.EjectCard(cardReaderConnection, ejectCard.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EjectCard() -> {task.Result.CompletionCode}");

            EjectCardCompletion response = new EjectCardCompletion(ejectCard.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);

            if (response.Payload.CompletionCode == MessagePayload.CompletionCodeEnum.Success)
            {
                await ServiceProvider.Device.WaitForCardTaken(cardReaderConnection, cancel);
            }
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(RetainCardCommand))]
    public class RetainCardHandler : ICommandHandler
    {
        public RetainCardHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(RetainCardHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(RetainCardHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteRetainCard(Connection, command as RetainCardCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            RetainCardCommand retainCardcommand = command as RetainCardCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            RetainCardCompletion response = new RetainCardCompletion(retainCardcommand.Headers.RequestId, new RetainCardCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteRetainCard(IConnection connection, RetainCardCommand retainCard, CancellationToken cancel)
        {
            retainCard.IsNotNull($"Invalid parameter in the ExecuteRetainCard method. {nameof(retainCard)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, retainCard.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.RetainCard()");
            Task<RetainCardCompletion.PayloadData> task = ServiceProvider.Device.RetainCard(cardReaderConnection, retainCard.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.RetainCard() -> {task.Result.CompletionCode}");

            RetainCardCompletion response = new RetainCardCompletion(retainCard.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(ResetCountCommand))]
    public class ResetCountHandler : ICommandHandler
    {
        public ResetCountHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ResetCountHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ResetCountHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteResetCount(Connection, command as ResetCountCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ResetCountCommand resetCountcommand = command as ResetCountCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            ResetCountCompletion response = new ResetCountCompletion(resetCountcommand.Headers.RequestId, new ResetCountCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteResetCount(IConnection connection, ResetCountCommand resetCount, CancellationToken cancel)
        {
            resetCount.IsNotNull($"Invalid parameter in the ExecuteResetCount method. {nameof(resetCount)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, resetCount.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ResetCount()");
            Task<ResetCountCompletion.PayloadData> task = ServiceProvider.Device.ResetCount(cardReaderConnection, resetCount.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ResetCount() -> {task.Result.CompletionCode}");

            ResetCountCompletion response = new ResetCountCompletion(resetCount.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(SetKeyCommand))]
    public class SetKeyHandler : ICommandHandler
    {
        public SetKeyHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetKeyHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(SetKeyHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteSetKey(Connection, command as SetKeyCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            SetKeyCommand setKeycommand = command as SetKeyCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            SetKeyCompletion response = new SetKeyCompletion(setKeycommand.Headers.RequestId, new SetKeyCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSetKey(IConnection connection, SetKeyCommand setKey, CancellationToken cancel)
        {
            setKey.IsNotNull($"Invalid parameter in the ExecuteSetKey method. {nameof(setKey)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, setKey.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.SetKey()");
            Task<SetKeyCompletion.PayloadData> task = ServiceProvider.Device.SetKey(cardReaderConnection, setKey.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.SetKey() -> {task.Result.CompletionCode}");

            SetKeyCompletion response = new SetKeyCompletion(setKey.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(ReadRawDataCommand))]
    public class ReadRawDataHandler : ICommandHandler
    {
        public ReadRawDataHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadRawDataHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ReadRawDataHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteReadRawData(Connection, command as ReadRawDataCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ReadRawDataCommand readRawDatacommand = command as ReadRawDataCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            ReadRawDataCompletion response = new ReadRawDataCompletion(readRawDatacommand.Headers.RequestId, new ReadRawDataCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteReadRawData(IConnection connection, ReadRawDataCommand readRawData, CancellationToken cancel)
        {
            readRawData.IsNotNull($"Invalid parameter in the ExecuteReadRawData method. {nameof(readRawData)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, readRawData.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ReadRawData()");
            Task<ReadRawDataCompletion.PayloadData> task = ServiceProvider.Device.ReadRawData(cardReaderConnection, readRawData.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ReadRawData() -> {task.Result.CompletionCode}");

            ReadRawDataCompletion response = new ReadRawDataCompletion(readRawData.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(WriteRawDataCommand))]
    public class WriteRawDataHandler : ICommandHandler
    {
        public WriteRawDataHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(WriteRawDataHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(WriteRawDataHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteWriteRawData(Connection, command as WriteRawDataCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            WriteRawDataCommand writeRawDatacommand = command as WriteRawDataCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            WriteRawDataCompletion response = new WriteRawDataCompletion(writeRawDatacommand.Headers.RequestId, new WriteRawDataCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteWriteRawData(IConnection connection, WriteRawDataCommand writeRawData, CancellationToken cancel)
        {
            writeRawData.IsNotNull($"Invalid parameter in the ExecuteWriteRawData method. {nameof(writeRawData)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, writeRawData.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.WriteRawData()");
            Task<WriteRawDataCompletion.PayloadData> task = ServiceProvider.Device.WriteRawData(cardReaderConnection, writeRawData.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.WriteRawData() -> {task.Result.CompletionCode}");

            WriteRawDataCompletion response = new WriteRawDataCompletion(writeRawData.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(ChipIOCommand))]
    public class ChipIOHandler : ICommandHandler
    {
        public ChipIOHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ChipIOHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ChipIOHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteChipIO(Connection, command as ChipIOCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ChipIOCommand chipIOcommand = command as ChipIOCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            ChipIOCompletion response = new ChipIOCompletion(chipIOcommand.Headers.RequestId, new ChipIOCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteChipIO(IConnection connection, ChipIOCommand chipIO, CancellationToken cancel)
        {
            chipIO.IsNotNull($"Invalid parameter in the ExecuteChipIO method. {nameof(chipIO)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, chipIO.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ChipIO()");
            Task<ChipIOCompletion.PayloadData> task = ServiceProvider.Device.ChipIO(cardReaderConnection, chipIO.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ChipIO() -> {task.Result.CompletionCode}");

            ChipIOCompletion response = new ChipIOCompletion(chipIO.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(ResetCommand))]
    public class ResetHandler : ICommandHandler
    {
        public ResetHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ResetHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ResetHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteReset(Connection, command as ResetCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ResetCommand resetcommand = command as ResetCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            ResetCompletion response = new ResetCompletion(resetcommand.Headers.RequestId, new ResetCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteReset(IConnection connection, ResetCommand reset, CancellationToken cancel)
        {
            reset.IsNotNull($"Invalid parameter in the ExecuteReset method. {nameof(reset)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, reset.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.Reset()");
            Task<ResetCompletion.PayloadData> task = ServiceProvider.Device.Reset(cardReaderConnection, reset.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.Reset() -> {task.Result.CompletionCode}");

            ResetCompletion response = new ResetCompletion(reset.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(ChipPowerCommand))]
    public class ChipPowerHandler : ICommandHandler
    {
        public ChipPowerHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ChipPowerHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ChipPowerHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteChipPower(Connection, command as ChipPowerCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ChipPowerCommand chipPowercommand = command as ChipPowerCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            ChipPowerCompletion response = new ChipPowerCompletion(chipPowercommand.Headers.RequestId, new ChipPowerCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteChipPower(IConnection connection, ChipPowerCommand chipPower, CancellationToken cancel)
        {
            chipPower.IsNotNull($"Invalid parameter in the ExecuteChipPower method. {nameof(chipPower)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, chipPower.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ChipPower()");
            Task<ChipPowerCompletion.PayloadData> task = ServiceProvider.Device.ChipPower(cardReaderConnection, chipPower.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ChipPower() -> {task.Result.CompletionCode}");

            ChipPowerCompletion response = new ChipPowerCompletion(chipPower.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(ParseDataCommand))]
    public class ParseDataHandler : ICommandHandler
    {
        public ParseDataHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ParseDataHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ParseDataHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteParseData(Connection, command as ParseDataCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ParseDataCommand parseDatacommand = command as ParseDataCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            ParseDataCompletion response = new ParseDataCompletion(parseDatacommand.Headers.RequestId, new ParseDataCompletion.PayloadData(errorCode, commandException.Message, 0));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteParseData(IConnection connection, ParseDataCommand parseData, CancellationToken cancel)
        {
            parseData.IsNotNull($"Invalid parameter in the ExecuteParseData method. {nameof(parseData)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, parseData.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ParseData()");
            Task<ParseDataCompletion.PayloadData> task = ServiceProvider.Device.ParseData(cardReaderConnection, parseData.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ParseData() -> {task.Result.CompletionCode}");

            ParseDataCompletion response = new ParseDataCompletion(parseData.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(ParkCardCommand))]
    public class ParkCardHandler : ICommandHandler
    {
        public ParkCardHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ParkCardHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ParkCardHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteParkCard(Connection, command as ParkCardCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ParkCardCommand parkCardcommand = command as ParkCardCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            ParkCardCompletion response = new ParkCardCompletion(parkCardcommand.Headers.RequestId, new ParkCardCompletion.PayloadData(errorCode, commandException.Message, 0));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteParkCard(IConnection connection, ParkCardCommand parkCard, CancellationToken cancel)
        {
            parkCard.IsNotNull($"Invalid parameter in the ExecuteParkCard method. {nameof(parkCard)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, parkCard.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.ParkCard()");
            Task<ParkCardCompletion.PayloadData> task = ServiceProvider.Device.ParkCard(cardReaderConnection, parkCard.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.ParkCard() -> {task.Result.CompletionCode}");

            ParkCardCompletion response = new ParkCardCompletion(parkCard.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(EMVClessConfigureCommand))]
    public class EMVClessConfigureHandler : ICommandHandler
    {
        public EMVClessConfigureHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EMVClessConfigureHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(EMVClessConfigureHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteEMVClessConfigure(Connection, command as EMVClessConfigureCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            EMVClessConfigureCommand eMVClessConfigurecommand = command as EMVClessConfigureCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            EMVClessConfigureCompletion response = new EMVClessConfigureCompletion(eMVClessConfigurecommand.Headers.RequestId, new EMVClessConfigureCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteEMVClessConfigure(IConnection connection, EMVClessConfigureCommand eMVClessConfigure, CancellationToken cancel)
        {
            eMVClessConfigure.IsNotNull($"Invalid parameter in the ExecuteEMVClessConfigure method. {nameof(eMVClessConfigure)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, eMVClessConfigure.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVClessConfigure()");
            Task<EMVClessConfigureCompletion.PayloadData> task = ServiceProvider.Device.EMVClessConfigure(cardReaderConnection, eMVClessConfigure.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVClessConfigure() -> {task.Result.CompletionCode}");

            EMVClessConfigureCompletion response = new EMVClessConfigureCompletion(eMVClessConfigure.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(EMVClessPerformTransactionCommand))]
    public class EMVClessPerformTransactionHandler : ICommandHandler
    {
        public EMVClessPerformTransactionHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EMVClessPerformTransactionHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(EMVClessPerformTransactionHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteEMVClessPerformTransaction(Connection, command as EMVClessPerformTransactionCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            EMVClessPerformTransactionCommand eMVClessPerformTransactioncommand = command as EMVClessPerformTransactionCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            EMVClessPerformTransactionCompletion response = new EMVClessPerformTransactionCompletion(eMVClessPerformTransactioncommand.Headers.RequestId, new EMVClessPerformTransactionCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteEMVClessPerformTransaction(IConnection connection, EMVClessPerformTransactionCommand eMVClessPerformTransaction, CancellationToken cancel)
        {
            eMVClessPerformTransaction.IsNotNull($"Invalid parameter in the ExecuteEMVClessPerformTransaction method. {nameof(eMVClessPerformTransaction)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, eMVClessPerformTransaction.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVClessPerformTransaction()");
            Task<EMVClessPerformTransactionCompletion.PayloadData> task = ServiceProvider.Device.EMVClessPerformTransaction(cardReaderConnection, eMVClessPerformTransaction.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVClessPerformTransaction() -> {task.Result.CompletionCode}");

            EMVClessPerformTransactionCompletion response = new EMVClessPerformTransactionCompletion(eMVClessPerformTransaction.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(EMVClessIssuerUpdateCommand))]
    public class EMVClessIssuerUpdateHandler : ICommandHandler
    {
        public EMVClessIssuerUpdateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(EMVClessIssuerUpdateHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(EMVClessIssuerUpdateHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteEMVClessIssuerUpdate(Connection, command as EMVClessIssuerUpdateCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            EMVClessIssuerUpdateCommand eMVClessIssuerUpdatecommand = command as EMVClessIssuerUpdateCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            EMVClessIssuerUpdateCompletion response = new EMVClessIssuerUpdateCompletion(eMVClessIssuerUpdatecommand.Headers.RequestId, new EMVClessIssuerUpdateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteEMVClessIssuerUpdate(IConnection connection, EMVClessIssuerUpdateCommand eMVClessIssuerUpdate, CancellationToken cancel)
        {
            eMVClessIssuerUpdate.IsNotNull($"Invalid parameter in the ExecuteEMVClessIssuerUpdate method. {nameof(eMVClessIssuerUpdate)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, eMVClessIssuerUpdate.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.EMVClessIssuerUpdate()");
            Task<EMVClessIssuerUpdateCompletion.PayloadData> task = ServiceProvider.Device.EMVClessIssuerUpdate(cardReaderConnection, eMVClessIssuerUpdate.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.EMVClessIssuerUpdate() -> {task.Result.CompletionCode}");

            EMVClessIssuerUpdateCompletion response = new EMVClessIssuerUpdateCompletion(eMVClessIssuerUpdate.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(StatusCommand))]
    public class StatusHandler : ICommandHandler
    {
        public StatusHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(StatusHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(StatusHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteStatus(Connection, command as StatusCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            StatusCommand statuscommand = command as StatusCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            StatusCompletion response = new StatusCompletion(statuscommand.Headers.RequestId, new StatusCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteStatus(IConnection connection, StatusCommand status, CancellationToken cancel)
        {
            status.IsNotNull($"Invalid parameter in the ExecuteStatus method. {nameof(status)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, status.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.Status()");
            Task<StatusCompletion.PayloadData> task = ServiceProvider.Device.Status(cardReaderConnection, status.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.Status() -> {task.Result.CompletionCode}");

            StatusCompletion response = new StatusCompletion(status.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(CapabilitiesCommand))]
    public class CapabilitiesHandler : ICommandHandler
    {
        public CapabilitiesHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CapabilitiesHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(CapabilitiesHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteCapabilities(Connection, command as CapabilitiesCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            CapabilitiesCommand capabilitiescommand = command as CapabilitiesCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            CapabilitiesCompletion response = new CapabilitiesCompletion(capabilitiescommand.Headers.RequestId, new CapabilitiesCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteCapabilities(IConnection connection, CapabilitiesCommand capabilities, CancellationToken cancel)
        {
            capabilities.IsNotNull($"Invalid parameter in the ExecuteCapabilities method. {nameof(capabilities)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, capabilities.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.Capabilities()");
            Task<CapabilitiesCompletion.PayloadData> task = ServiceProvider.Device.Capabilities(cardReaderConnection, capabilities.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.Capabilities() -> {task.Result.CompletionCode}");

            CapabilitiesCompletion response = new CapabilitiesCompletion(capabilities.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(SetGuidanceLightCommand))]
    public class SetGuidanceLightHandler : ICommandHandler
    {
        public SetGuidanceLightHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetGuidanceLightHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(SetGuidanceLightHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteSetGuidanceLight(Connection, command as SetGuidanceLightCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            SetGuidanceLightCommand setGuidanceLightcommand = command as SetGuidanceLightCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            SetGuidanceLightCompletion response = new SetGuidanceLightCompletion(setGuidanceLightcommand.Headers.RequestId, new SetGuidanceLightCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSetGuidanceLight(IConnection connection, SetGuidanceLightCommand setGuidanceLight, CancellationToken cancel)
        {
            setGuidanceLight.IsNotNull($"Invalid parameter in the ExecuteSetGuidanceLight method. {nameof(setGuidanceLight)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, setGuidanceLight.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.SetGuidanceLight()");
            Task<SetGuidanceLightCompletion.PayloadData> task = ServiceProvider.Device.SetGuidanceLight(cardReaderConnection, setGuidanceLight.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.SetGuidanceLight() -> {task.Result.CompletionCode}");

            SetGuidanceLightCompletion response = new SetGuidanceLightCompletion(setGuidanceLight.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(PowerSaveControlCommand))]
    public class PowerSaveControlHandler : ICommandHandler
    {
        public PowerSaveControlHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PowerSaveControlHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(PowerSaveControlHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecutePowerSaveControl(Connection, command as PowerSaveControlCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            PowerSaveControlCommand powerSaveControlcommand = command as PowerSaveControlCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            PowerSaveControlCompletion response = new PowerSaveControlCompletion(powerSaveControlcommand.Headers.RequestId, new PowerSaveControlCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecutePowerSaveControl(IConnection connection, PowerSaveControlCommand powerSaveControl, CancellationToken cancel)
        {
            powerSaveControl.IsNotNull($"Invalid parameter in the ExecutePowerSaveControl method. {nameof(powerSaveControl)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, powerSaveControl.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.PowerSaveControl()");
            Task<PowerSaveControlCompletion.PayloadData> task = ServiceProvider.Device.PowerSaveControl(cardReaderConnection, powerSaveControl.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.PowerSaveControl() -> {task.Result.CompletionCode}");

            PowerSaveControlCompletion response = new PowerSaveControlCompletion(powerSaveControl.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(SynchronizeCommandCommand))]
    public class SynchronizeCommandHandler : ICommandHandler
    {
        public SynchronizeCommandHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SynchronizeCommandHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(SynchronizeCommandHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteSynchronizeCommand(Connection, command as SynchronizeCommandCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            SynchronizeCommandCommand synchronizeCommandcommand = command as SynchronizeCommandCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            SynchronizeCommandCompletion response = new SynchronizeCommandCompletion(synchronizeCommandcommand.Headers.RequestId, new SynchronizeCommandCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSynchronizeCommand(IConnection connection, SynchronizeCommandCommand synchronizeCommand, CancellationToken cancel)
        {
            synchronizeCommand.IsNotNull($"Invalid parameter in the ExecuteSynchronizeCommand method. {nameof(synchronizeCommand)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, synchronizeCommand.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.SynchronizeCommand()");
            Task<SynchronizeCommandCompletion.PayloadData> task = ServiceProvider.Device.SynchronizeCommand(cardReaderConnection, synchronizeCommand.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.SynchronizeCommand() -> {task.Result.CompletionCode}");

            SynchronizeCommandCompletion response = new SynchronizeCommandCompletion(synchronizeCommand.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(SetTransactionStateCommand))]
    public class SetTransactionStateHandler : ICommandHandler
    {
        public SetTransactionStateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetTransactionStateHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(SetTransactionStateHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteSetTransactionState(Connection, command as SetTransactionStateCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            SetTransactionStateCommand setTransactionStatecommand = command as SetTransactionStateCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            SetTransactionStateCompletion response = new SetTransactionStateCompletion(setTransactionStatecommand.Headers.RequestId, new SetTransactionStateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSetTransactionState(IConnection connection, SetTransactionStateCommand setTransactionState, CancellationToken cancel)
        {
            setTransactionState.IsNotNull($"Invalid parameter in the ExecuteSetTransactionState method. {nameof(setTransactionState)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, setTransactionState.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.SetTransactionState()");
            Task<SetTransactionStateCompletion.PayloadData> task = ServiceProvider.Device.SetTransactionState(cardReaderConnection, setTransactionState.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.SetTransactionState() -> {task.Result.CompletionCode}");

            SetTransactionStateCompletion response = new SetTransactionStateCompletion(setTransactionState.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(CardReaderServiceProvider), typeof(GetTransactionStateCommand))]
    public class GetTransactionStateHandler : ICommandHandler
    {
        public GetTransactionStateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetTransactionStateHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as CardReaderServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(GetTransactionStateHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteGetTransactionState(Connection, command as GetTransactionStateCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            GetTransactionStateCommand getTransactionStatecommand = command as GetTransactionStateCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;
            else if (commandException.GetType() == typeof(NotImplementedException))
                errorCode = MessagePayload.CompletionCodeEnum.UnsupportedCommand;

            GetTransactionStateCompletion response = new GetTransactionStateCompletion(getTransactionStatecommand.Headers.RequestId, new GetTransactionStateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteGetTransactionState(IConnection connection, GetTransactionStateCommand getTransactionState, CancellationToken cancel)
        {
            getTransactionState.IsNotNull($"Invalid parameter in the ExecuteGetTransactionState method. {nameof(getTransactionState)}");

            ICardReaderConnection cardReaderConnection = new CardReaderConnection(connection, getTransactionState.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "CardReaderDev.GetTransactionState()");
            Task<GetTransactionStateCompletion.PayloadData> task = ServiceProvider.Device.GetTransactionState(cardReaderConnection, getTransactionState.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.GetTransactionState() -> {task.Result.CompletionCode}");

            GetTransactionStateCompletion response = new GetTransactionStateCompletion(getTransactionState.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public CardReaderServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

}
