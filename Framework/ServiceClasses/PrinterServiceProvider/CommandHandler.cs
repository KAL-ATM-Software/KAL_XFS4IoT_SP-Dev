/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTPrinter;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoT.Completions;

namespace Printer
{
    [CommandHandler(typeof(PrinterServiceProvider), typeof(GetFormListCommand))]
    public class GetFormListHandler : ICommandHandler
    {
        public GetFormListHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetFormListHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(GetFormListHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteGetFormList(Connection, command as GetFormListCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            GetFormListCommand getFormListcommand = command as GetFormListCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            GetFormListCompletion response = new GetFormListCompletion(getFormListcommand.Headers.RequestId, new GetFormListCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteGetFormList(IConnection connection, GetFormListCommand getFormList, CancellationToken cancel)
        {
            getFormList.IsNotNull($"Invalid parameter in the ExecuteGetFormList method. {nameof(getFormList)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, getFormList.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.GetFormList()");
            Task<GetFormListCompletion.PayloadData> task = ServiceProvider.Device.GetFormList(printerConnection, getFormList.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.GetFormList() -> {task.Result.CompletionCode}");

            GetFormListCompletion response = new GetFormListCompletion(getFormList.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(GetMediaListCommand))]
    public class GetMediaListHandler : ICommandHandler
    {
        public GetMediaListHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetMediaListHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(GetMediaListHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteGetMediaList(Connection, command as GetMediaListCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            GetMediaListCommand getMediaListcommand = command as GetMediaListCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            GetMediaListCompletion response = new GetMediaListCompletion(getMediaListcommand.Headers.RequestId, new GetMediaListCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteGetMediaList(IConnection connection, GetMediaListCommand getMediaList, CancellationToken cancel)
        {
            getMediaList.IsNotNull($"Invalid parameter in the ExecuteGetMediaList method. {nameof(getMediaList)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, getMediaList.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.GetMediaList()");
            Task<GetMediaListCompletion.PayloadData> task = ServiceProvider.Device.GetMediaList(printerConnection, getMediaList.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.GetMediaList() -> {task.Result.CompletionCode}");

            GetMediaListCompletion response = new GetMediaListCompletion(getMediaList.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(GetQueryFormCommand))]
    public class GetQueryFormHandler : ICommandHandler
    {
        public GetQueryFormHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetQueryFormHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(GetQueryFormHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteGetQueryForm(Connection, command as GetQueryFormCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            GetQueryFormCommand getQueryFormcommand = command as GetQueryFormCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            GetQueryFormCompletion response = new GetQueryFormCompletion(getQueryFormcommand.Headers.RequestId, new GetQueryFormCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteGetQueryForm(IConnection connection, GetQueryFormCommand getQueryForm, CancellationToken cancel)
        {
            getQueryForm.IsNotNull($"Invalid parameter in the ExecuteGetQueryForm method. {nameof(getQueryForm)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, getQueryForm.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.GetQueryForm()");
            Task<GetQueryFormCompletion.PayloadData> task = ServiceProvider.Device.GetQueryForm(printerConnection, getQueryForm.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.GetQueryForm() -> {task.Result.CompletionCode}");

            GetQueryFormCompletion response = new GetQueryFormCompletion(getQueryForm.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(GetQueryMediaCommand))]
    public class GetQueryMediaHandler : ICommandHandler
    {
        public GetQueryMediaHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetQueryMediaHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(GetQueryMediaHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteGetQueryMedia(Connection, command as GetQueryMediaCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            GetQueryMediaCommand getQueryMediacommand = command as GetQueryMediaCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            GetQueryMediaCompletion response = new GetQueryMediaCompletion(getQueryMediacommand.Headers.RequestId, new GetQueryMediaCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteGetQueryMedia(IConnection connection, GetQueryMediaCommand getQueryMedia, CancellationToken cancel)
        {
            getQueryMedia.IsNotNull($"Invalid parameter in the ExecuteGetQueryMedia method. {nameof(getQueryMedia)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, getQueryMedia.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.GetQueryMedia()");
            Task<GetQueryMediaCompletion.PayloadData> task = ServiceProvider.Device.GetQueryMedia(printerConnection, getQueryMedia.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.GetQueryMedia() -> {task.Result.CompletionCode}");

            GetQueryMediaCompletion response = new GetQueryMediaCompletion(getQueryMedia.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(GetQueryFieldCommand))]
    public class GetQueryFieldHandler : ICommandHandler
    {
        public GetQueryFieldHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetQueryFieldHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(GetQueryFieldHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteGetQueryField(Connection, command as GetQueryFieldCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            GetQueryFieldCommand getQueryFieldcommand = command as GetQueryFieldCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            GetQueryFieldCompletion response = new GetQueryFieldCompletion(getQueryFieldcommand.Headers.RequestId, new GetQueryFieldCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteGetQueryField(IConnection connection, GetQueryFieldCommand getQueryField, CancellationToken cancel)
        {
            getQueryField.IsNotNull($"Invalid parameter in the ExecuteGetQueryField method. {nameof(getQueryField)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, getQueryField.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.GetQueryField()");
            Task<GetQueryFieldCompletion.PayloadData> task = ServiceProvider.Device.GetQueryField(printerConnection, getQueryField.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.GetQueryField() -> {task.Result.CompletionCode}");

            GetQueryFieldCompletion response = new GetQueryFieldCompletion(getQueryField.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(GetCodelineMappingCommand))]
    public class GetCodelineMappingHandler : ICommandHandler
    {
        public GetCodelineMappingHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetCodelineMappingHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(GetCodelineMappingHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteGetCodelineMapping(Connection, command as GetCodelineMappingCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            GetCodelineMappingCommand getCodelineMappingcommand = command as GetCodelineMappingCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            GetCodelineMappingCompletion response = new GetCodelineMappingCompletion(getCodelineMappingcommand.Headers.RequestId, new GetCodelineMappingCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteGetCodelineMapping(IConnection connection, GetCodelineMappingCommand getCodelineMapping, CancellationToken cancel)
        {
            getCodelineMapping.IsNotNull($"Invalid parameter in the ExecuteGetCodelineMapping method. {nameof(getCodelineMapping)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, getCodelineMapping.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.GetCodelineMapping()");
            Task<GetCodelineMappingCompletion.PayloadData> task = ServiceProvider.Device.GetCodelineMapping(printerConnection, getCodelineMapping.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.GetCodelineMapping() -> {task.Result.CompletionCode}");

            GetCodelineMappingCompletion response = new GetCodelineMappingCompletion(getCodelineMapping.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(ControlMediaCommand))]
    public class ControlMediaHandler : ICommandHandler
    {
        public ControlMediaHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ControlMediaHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ControlMediaHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteControlMedia(Connection, command as ControlMediaCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ControlMediaCommand controlMediacommand = command as ControlMediaCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            ControlMediaCompletion response = new ControlMediaCompletion(controlMediacommand.Headers.RequestId, new ControlMediaCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteControlMedia(IConnection connection, ControlMediaCommand controlMedia, CancellationToken cancel)
        {
            controlMedia.IsNotNull($"Invalid parameter in the ExecuteControlMedia method. {nameof(controlMedia)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, controlMedia.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.ControlMedia()");
            Task<ControlMediaCompletion.PayloadData> task = ServiceProvider.Device.ControlMedia(printerConnection, controlMedia.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ControlMedia() -> {task.Result.CompletionCode}");

            ControlMediaCompletion response = new ControlMediaCompletion(controlMedia.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(PrintFormCommand))]
    public class PrintFormHandler : ICommandHandler
    {
        public PrintFormHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PrintFormHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(PrintFormHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecutePrintForm(Connection, command as PrintFormCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            PrintFormCommand printFormcommand = command as PrintFormCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            PrintFormCompletion response = new PrintFormCompletion(printFormcommand.Headers.RequestId, new PrintFormCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecutePrintForm(IConnection connection, PrintFormCommand printForm, CancellationToken cancel)
        {
            printForm.IsNotNull($"Invalid parameter in the ExecutePrintForm method. {nameof(printForm)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, printForm.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.PrintForm()");
            Task<PrintFormCompletion.PayloadData> task = ServiceProvider.Device.PrintForm(printerConnection, printForm.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.PrintForm() -> {task.Result.CompletionCode}");

            PrintFormCompletion response = new PrintFormCompletion(printForm.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(ReadFormCommand))]
    public class ReadFormHandler : ICommandHandler
    {
        public ReadFormHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadFormHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ReadFormHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteReadForm(Connection, command as ReadFormCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ReadFormCommand readFormcommand = command as ReadFormCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            ReadFormCompletion response = new ReadFormCompletion(readFormcommand.Headers.RequestId, new ReadFormCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteReadForm(IConnection connection, ReadFormCommand readForm, CancellationToken cancel)
        {
            readForm.IsNotNull($"Invalid parameter in the ExecuteReadForm method. {nameof(readForm)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, readForm.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.ReadForm()");
            Task<ReadFormCompletion.PayloadData> task = ServiceProvider.Device.ReadForm(printerConnection, readForm.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ReadForm() -> {task.Result.CompletionCode}");

            ReadFormCompletion response = new ReadFormCompletion(readForm.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(RawDataCommand))]
    public class RawDataHandler : ICommandHandler
    {
        public RawDataHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(RawDataHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(RawDataHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteRawData(Connection, command as RawDataCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            RawDataCommand rawDatacommand = command as RawDataCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            RawDataCompletion response = new RawDataCompletion(rawDatacommand.Headers.RequestId, new RawDataCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteRawData(IConnection connection, RawDataCommand rawData, CancellationToken cancel)
        {
            rawData.IsNotNull($"Invalid parameter in the ExecuteRawData method. {nameof(rawData)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, rawData.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.RawData()");
            Task<RawDataCompletion.PayloadData> task = ServiceProvider.Device.RawData(printerConnection, rawData.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.RawData() -> {task.Result.CompletionCode}");

            RawDataCompletion response = new RawDataCompletion(rawData.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(MediaExtentsCommand))]
    public class MediaExtentsHandler : ICommandHandler
    {
        public MediaExtentsHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(MediaExtentsHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(MediaExtentsHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteMediaExtents(Connection, command as MediaExtentsCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            MediaExtentsCommand mediaExtentscommand = command as MediaExtentsCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            MediaExtentsCompletion response = new MediaExtentsCompletion(mediaExtentscommand.Headers.RequestId, new MediaExtentsCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteMediaExtents(IConnection connection, MediaExtentsCommand mediaExtents, CancellationToken cancel)
        {
            mediaExtents.IsNotNull($"Invalid parameter in the ExecuteMediaExtents method. {nameof(mediaExtents)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, mediaExtents.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.MediaExtents()");
            Task<MediaExtentsCompletion.PayloadData> task = ServiceProvider.Device.MediaExtents(printerConnection, mediaExtents.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.MediaExtents() -> {task.Result.CompletionCode}");

            MediaExtentsCompletion response = new MediaExtentsCompletion(mediaExtents.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(ResetCountCommand))]
    public class ResetCountHandler : ICommandHandler
    {
        public ResetCountHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ResetCountHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

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

            ResetCountCompletion response = new ResetCountCompletion(resetCountcommand.Headers.RequestId, new ResetCountCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteResetCount(IConnection connection, ResetCountCommand resetCount, CancellationToken cancel)
        {
            resetCount.IsNotNull($"Invalid parameter in the ExecuteResetCount method. {nameof(resetCount)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, resetCount.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.ResetCount()");
            Task<ResetCountCompletion.PayloadData> task = ServiceProvider.Device.ResetCount(printerConnection, resetCount.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ResetCount() -> {task.Result.CompletionCode}");

            ResetCountCompletion response = new ResetCountCompletion(resetCount.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(ReadImageCommand))]
    public class ReadImageHandler : ICommandHandler
    {
        public ReadImageHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadImageHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ReadImageHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteReadImage(Connection, command as ReadImageCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ReadImageCommand readImagecommand = command as ReadImageCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            ReadImageCompletion response = new ReadImageCompletion(readImagecommand.Headers.RequestId, new ReadImageCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteReadImage(IConnection connection, ReadImageCommand readImage, CancellationToken cancel)
        {
            readImage.IsNotNull($"Invalid parameter in the ExecuteReadImage method. {nameof(readImage)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, readImage.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.ReadImage()");
            Task<ReadImageCompletion.PayloadData> task = ServiceProvider.Device.ReadImage(printerConnection, readImage.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ReadImage() -> {task.Result.CompletionCode}");

            ReadImageCompletion response = new ReadImageCompletion(readImage.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(ResetCommand))]
    public class ResetHandler : ICommandHandler
    {
        public ResetHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ResetHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

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

            ResetCompletion response = new ResetCompletion(resetcommand.Headers.RequestId, new ResetCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteReset(IConnection connection, ResetCommand reset, CancellationToken cancel)
        {
            reset.IsNotNull($"Invalid parameter in the ExecuteReset method. {nameof(reset)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, reset.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.Reset()");
            Task<ResetCompletion.PayloadData> task = ServiceProvider.Device.Reset(printerConnection, reset.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.Reset() -> {task.Result.CompletionCode}");

            ResetCompletion response = new ResetCompletion(reset.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(RetractMediaCommand))]
    public class RetractMediaHandler : ICommandHandler
    {
        public RetractMediaHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(RetractMediaHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(RetractMediaHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteRetractMedia(Connection, command as RetractMediaCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            RetractMediaCommand retractMediacommand = command as RetractMediaCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            RetractMediaCompletion response = new RetractMediaCompletion(retractMediacommand.Headers.RequestId, new RetractMediaCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteRetractMedia(IConnection connection, RetractMediaCommand retractMedia, CancellationToken cancel)
        {
            retractMedia.IsNotNull($"Invalid parameter in the ExecuteRetractMedia method. {nameof(retractMedia)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, retractMedia.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.RetractMedia()");
            Task<RetractMediaCompletion.PayloadData> task = ServiceProvider.Device.RetractMedia(printerConnection, retractMedia.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.RetractMedia() -> {task.Result.CompletionCode}");

            RetractMediaCompletion response = new RetractMediaCompletion(retractMedia.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(DispensePaperCommand))]
    public class DispensePaperHandler : ICommandHandler
    {
        public DispensePaperHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DispensePaperHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(DispensePaperHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteDispensePaper(Connection, command as DispensePaperCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            DispensePaperCommand dispensePapercommand = command as DispensePaperCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            DispensePaperCompletion response = new DispensePaperCompletion(dispensePapercommand.Headers.RequestId, new DispensePaperCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteDispensePaper(IConnection connection, DispensePaperCommand dispensePaper, CancellationToken cancel)
        {
            dispensePaper.IsNotNull($"Invalid parameter in the ExecuteDispensePaper method. {nameof(dispensePaper)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, dispensePaper.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.DispensePaper()");
            Task<DispensePaperCompletion.PayloadData> task = ServiceProvider.Device.DispensePaper(printerConnection, dispensePaper.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.DispensePaper() -> {task.Result.CompletionCode}");

            DispensePaperCompletion response = new DispensePaperCompletion(dispensePaper.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(PrintRawFileCommand))]
    public class PrintRawFileHandler : ICommandHandler
    {
        public PrintRawFileHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PrintRawFileHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(PrintRawFileHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecutePrintRawFile(Connection, command as PrintRawFileCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            PrintRawFileCommand printRawFilecommand = command as PrintRawFileCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            PrintRawFileCompletion response = new PrintRawFileCompletion(printRawFilecommand.Headers.RequestId, new PrintRawFileCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecutePrintRawFile(IConnection connection, PrintRawFileCommand printRawFile, CancellationToken cancel)
        {
            printRawFile.IsNotNull($"Invalid parameter in the ExecutePrintRawFile method. {nameof(printRawFile)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, printRawFile.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.PrintRawFile()");
            Task<PrintRawFileCompletion.PayloadData> task = ServiceProvider.Device.PrintRawFile(printerConnection, printRawFile.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.PrintRawFile() -> {task.Result.CompletionCode}");

            PrintRawFileCompletion response = new PrintRawFileCompletion(printRawFile.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(LoadDefinitionCommand))]
    public class LoadDefinitionHandler : ICommandHandler
    {
        public LoadDefinitionHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(LoadDefinitionHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(LoadDefinitionHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteLoadDefinition(Connection, command as LoadDefinitionCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            LoadDefinitionCommand loadDefinitioncommand = command as LoadDefinitionCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            LoadDefinitionCompletion response = new LoadDefinitionCompletion(loadDefinitioncommand.Headers.RequestId, new LoadDefinitionCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteLoadDefinition(IConnection connection, LoadDefinitionCommand loadDefinition, CancellationToken cancel)
        {
            loadDefinition.IsNotNull($"Invalid parameter in the ExecuteLoadDefinition method. {nameof(loadDefinition)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, loadDefinition.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.LoadDefinition()");
            Task<LoadDefinitionCompletion.PayloadData> task = ServiceProvider.Device.LoadDefinition(printerConnection, loadDefinition.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.LoadDefinition() -> {task.Result.CompletionCode}");

            LoadDefinitionCompletion response = new LoadDefinitionCompletion(loadDefinition.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(SupplyReplenishCommand))]
    public class SupplyReplenishHandler : ICommandHandler
    {
        public SupplyReplenishHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SupplyReplenishHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(SupplyReplenishHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteSupplyReplenish(Connection, command as SupplyReplenishCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            SupplyReplenishCommand supplyReplenishcommand = command as SupplyReplenishCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            SupplyReplenishCompletion response = new SupplyReplenishCompletion(supplyReplenishcommand.Headers.RequestId, new SupplyReplenishCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSupplyReplenish(IConnection connection, SupplyReplenishCommand supplyReplenish, CancellationToken cancel)
        {
            supplyReplenish.IsNotNull($"Invalid parameter in the ExecuteSupplyReplenish method. {nameof(supplyReplenish)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, supplyReplenish.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.SupplyReplenish()");
            Task<SupplyReplenishCompletion.PayloadData> task = ServiceProvider.Device.SupplyReplenish(printerConnection, supplyReplenish.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.SupplyReplenish() -> {task.Result.CompletionCode}");

            SupplyReplenishCompletion response = new SupplyReplenishCompletion(supplyReplenish.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(ControlPassbookCommand))]
    public class ControlPassbookHandler : ICommandHandler
    {
        public ControlPassbookHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ControlPassbookHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ControlPassbookHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteControlPassbook(Connection, command as ControlPassbookCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ControlPassbookCommand controlPassbookcommand = command as ControlPassbookCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            ControlPassbookCompletion response = new ControlPassbookCompletion(controlPassbookcommand.Headers.RequestId, new ControlPassbookCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteControlPassbook(IConnection connection, ControlPassbookCommand controlPassbook, CancellationToken cancel)
        {
            controlPassbook.IsNotNull($"Invalid parameter in the ExecuteControlPassbook method. {nameof(controlPassbook)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, controlPassbook.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.ControlPassbook()");
            Task<ControlPassbookCompletion.PayloadData> task = ServiceProvider.Device.ControlPassbook(printerConnection, controlPassbook.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.ControlPassbook() -> {task.Result.CompletionCode}");

            ControlPassbookCompletion response = new ControlPassbookCompletion(controlPassbook.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(SetBlackMarkModeCommand))]
    public class SetBlackMarkModeHandler : ICommandHandler
    {
        public SetBlackMarkModeHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetBlackMarkModeHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(SetBlackMarkModeHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteSetBlackMarkMode(Connection, command as SetBlackMarkModeCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            SetBlackMarkModeCommand setBlackMarkModecommand = command as SetBlackMarkModeCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            SetBlackMarkModeCompletion response = new SetBlackMarkModeCompletion(setBlackMarkModecommand.Headers.RequestId, new SetBlackMarkModeCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSetBlackMarkMode(IConnection connection, SetBlackMarkModeCommand setBlackMarkMode, CancellationToken cancel)
        {
            setBlackMarkMode.IsNotNull($"Invalid parameter in the ExecuteSetBlackMarkMode method. {nameof(setBlackMarkMode)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, setBlackMarkMode.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.SetBlackMarkMode()");
            Task<SetBlackMarkModeCompletion.PayloadData> task = ServiceProvider.Device.SetBlackMarkMode(printerConnection, setBlackMarkMode.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.SetBlackMarkMode() -> {task.Result.CompletionCode}");

            SetBlackMarkModeCompletion response = new SetBlackMarkModeCompletion(setBlackMarkMode.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(StatusCommand))]
    public class StatusHandler : ICommandHandler
    {
        public StatusHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(StatusHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

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

            StatusCompletion response = new StatusCompletion(statuscommand.Headers.RequestId, new StatusCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteStatus(IConnection connection, StatusCommand status, CancellationToken cancel)
        {
            status.IsNotNull($"Invalid parameter in the ExecuteStatus method. {nameof(status)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, status.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.Status()");
            Task<StatusCompletion.PayloadData> task = ServiceProvider.Device.Status(printerConnection, status.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.Status() -> {task.Result.CompletionCode}");

            StatusCompletion response = new StatusCompletion(status.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(CapabilitiesCommand))]
    public class CapabilitiesHandler : ICommandHandler
    {
        public CapabilitiesHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CapabilitiesHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

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

            CapabilitiesCompletion response = new CapabilitiesCompletion(capabilitiescommand.Headers.RequestId, new CapabilitiesCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteCapabilities(IConnection connection, CapabilitiesCommand capabilities, CancellationToken cancel)
        {
            capabilities.IsNotNull($"Invalid parameter in the ExecuteCapabilities method. {nameof(capabilities)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, capabilities.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.Capabilities()");
            Task<CapabilitiesCompletion.PayloadData> task = ServiceProvider.Device.Capabilities(printerConnection, capabilities.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.Capabilities() -> {task.Result.CompletionCode}");

            CapabilitiesCompletion response = new CapabilitiesCompletion(capabilities.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(SetGuidanceLightCommand))]
    public class SetGuidanceLightHandler : ICommandHandler
    {
        public SetGuidanceLightHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetGuidanceLightHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

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

            SetGuidanceLightCompletion response = new SetGuidanceLightCompletion(setGuidanceLightcommand.Headers.RequestId, new SetGuidanceLightCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSetGuidanceLight(IConnection connection, SetGuidanceLightCommand setGuidanceLight, CancellationToken cancel)
        {
            setGuidanceLight.IsNotNull($"Invalid parameter in the ExecuteSetGuidanceLight method. {nameof(setGuidanceLight)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, setGuidanceLight.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.SetGuidanceLight()");
            Task<SetGuidanceLightCompletion.PayloadData> task = ServiceProvider.Device.SetGuidanceLight(printerConnection, setGuidanceLight.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.SetGuidanceLight() -> {task.Result.CompletionCode}");

            SetGuidanceLightCompletion response = new SetGuidanceLightCompletion(setGuidanceLight.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(PowerSaveControlCommand))]
    public class PowerSaveControlHandler : ICommandHandler
    {
        public PowerSaveControlHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PowerSaveControlHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

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

            PowerSaveControlCompletion response = new PowerSaveControlCompletion(powerSaveControlcommand.Headers.RequestId, new PowerSaveControlCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecutePowerSaveControl(IConnection connection, PowerSaveControlCommand powerSaveControl, CancellationToken cancel)
        {
            powerSaveControl.IsNotNull($"Invalid parameter in the ExecutePowerSaveControl method. {nameof(powerSaveControl)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, powerSaveControl.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.PowerSaveControl()");
            Task<PowerSaveControlCompletion.PayloadData> task = ServiceProvider.Device.PowerSaveControl(printerConnection, powerSaveControl.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.PowerSaveControl() -> {task.Result.CompletionCode}");

            PowerSaveControlCompletion response = new PowerSaveControlCompletion(powerSaveControl.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(SynchronizeCommandCommand))]
    public class SynchronizeCommandHandler : ICommandHandler
    {
        public SynchronizeCommandHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SynchronizeCommandHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

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

            SynchronizeCommandCompletion response = new SynchronizeCommandCompletion(synchronizeCommandcommand.Headers.RequestId, new SynchronizeCommandCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSynchronizeCommand(IConnection connection, SynchronizeCommandCommand synchronizeCommand, CancellationToken cancel)
        {
            synchronizeCommand.IsNotNull($"Invalid parameter in the ExecuteSynchronizeCommand method. {nameof(synchronizeCommand)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, synchronizeCommand.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.SynchronizeCommand()");
            Task<SynchronizeCommandCompletion.PayloadData> task = ServiceProvider.Device.SynchronizeCommand(printerConnection, synchronizeCommand.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.SynchronizeCommand() -> {task.Result.CompletionCode}");

            SynchronizeCommandCompletion response = new SynchronizeCommandCompletion(synchronizeCommand.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(SetTransactionStateCommand))]
    public class SetTransactionStateHandler : ICommandHandler
    {
        public SetTransactionStateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetTransactionStateHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

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

            SetTransactionStateCompletion response = new SetTransactionStateCompletion(setTransactionStatecommand.Headers.RequestId, new SetTransactionStateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSetTransactionState(IConnection connection, SetTransactionStateCommand setTransactionState, CancellationToken cancel)
        {
            setTransactionState.IsNotNull($"Invalid parameter in the ExecuteSetTransactionState method. {nameof(setTransactionState)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, setTransactionState.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.SetTransactionState()");
            Task<SetTransactionStateCompletion.PayloadData> task = ServiceProvider.Device.SetTransactionState(printerConnection, setTransactionState.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.SetTransactionState() -> {task.Result.CompletionCode}");

            SetTransactionStateCompletion response = new SetTransactionStateCompletion(setTransactionState.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(PrinterServiceProvider), typeof(GetTransactionStateCommand))]
    public class GetTransactionStateHandler : ICommandHandler
    {
        public GetTransactionStateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetTransactionStateHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as PrinterServiceProvider;

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

            GetTransactionStateCompletion response = new GetTransactionStateCompletion(getTransactionStatecommand.Headers.RequestId, new GetTransactionStateCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteGetTransactionState(IConnection connection, GetTransactionStateCommand getTransactionState, CancellationToken cancel)
        {
            getTransactionState.IsNotNull($"Invalid parameter in the ExecuteGetTransactionState method. {nameof(getTransactionState)}");

            IPrinterConnection printerConnection = new PrinterConnection(connection, getTransactionState.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "PrinterDev.GetTransactionState()");
            Task<GetTransactionStateCompletion.PayloadData> task = ServiceProvider.Device.GetTransactionState(printerConnection, getTransactionState.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"PrinterDev.GetTransactionState() -> {task.Result.CompletionCode}");

            GetTransactionStateCompletion response = new GetTransactionStateCompletion(getTransactionState.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public PrinterServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

}
