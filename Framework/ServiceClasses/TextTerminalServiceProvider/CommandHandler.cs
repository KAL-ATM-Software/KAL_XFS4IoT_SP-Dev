/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTTextTerminal;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;

namespace TextTerminal
{
    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(GetFormListCommand))]
    public class GetFormListHandler : ICommandHandler
    {
        public GetFormListHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetFormListHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, getFormList.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.GetFormList()");
            Task<GetFormListCompletion.PayloadData> task = ServiceProvider.Device.GetFormList(textTerminalConnection, getFormList.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.GetFormList() -> {task.Result.CompletionCode}");

            GetFormListCompletion response = new GetFormListCompletion(getFormList.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(GetQueryFormCommand))]
    public class GetQueryFormHandler : ICommandHandler
    {
        public GetQueryFormHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetQueryFormHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, getQueryForm.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.GetQueryForm()");
            Task<GetQueryFormCompletion.PayloadData> task = ServiceProvider.Device.GetQueryForm(textTerminalConnection, getQueryForm.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.GetQueryForm() -> {task.Result.CompletionCode}");

            GetQueryFormCompletion response = new GetQueryFormCompletion(getQueryForm.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(GetQueryFieldCommand))]
    public class GetQueryFieldHandler : ICommandHandler
    {
        public GetQueryFieldHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetQueryFieldHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, getQueryField.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.GetQueryField()");
            Task<GetQueryFieldCompletion.PayloadData> task = ServiceProvider.Device.GetQueryField(textTerminalConnection, getQueryField.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.GetQueryField() -> {task.Result.CompletionCode}");

            GetQueryFieldCompletion response = new GetQueryFieldCompletion(getQueryField.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(GetKeyDetailCommand))]
    public class GetKeyDetailHandler : ICommandHandler
    {
        public GetKeyDetailHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetKeyDetailHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(GetKeyDetailHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteGetKeyDetail(Connection, command as GetKeyDetailCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            GetKeyDetailCommand getKeyDetailcommand = command as GetKeyDetailCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            GetKeyDetailCompletion response = new GetKeyDetailCompletion(getKeyDetailcommand.Headers.RequestId, new GetKeyDetailCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteGetKeyDetail(IConnection connection, GetKeyDetailCommand getKeyDetail, CancellationToken cancel)
        {
            getKeyDetail.IsNotNull($"Invalid parameter in the ExecuteGetKeyDetail method. {nameof(getKeyDetail)}");

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, getKeyDetail.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.GetKeyDetail()");
            Task<GetKeyDetailCompletion.PayloadData> task = ServiceProvider.Device.GetKeyDetail(textTerminalConnection, getKeyDetail.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.GetKeyDetail() -> {task.Result.CompletionCode}");

            GetKeyDetailCompletion response = new GetKeyDetailCompletion(getKeyDetail.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(BeepCommand))]
    public class BeepHandler : ICommandHandler
    {
        public BeepHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(BeepHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(BeepHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteBeep(Connection, command as BeepCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            BeepCommand beepcommand = command as BeepCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            BeepCompletion response = new BeepCompletion(beepcommand.Headers.RequestId, new BeepCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteBeep(IConnection connection, BeepCommand beep, CancellationToken cancel)
        {
            beep.IsNotNull($"Invalid parameter in the ExecuteBeep method. {nameof(beep)}");

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, beep.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.Beep()");
            Task<BeepCompletion.PayloadData> task = ServiceProvider.Device.Beep(textTerminalConnection, beep.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.Beep() -> {task.Result.CompletionCode}");

            BeepCompletion response = new BeepCompletion(beep.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(ClearScreenCommand))]
    public class ClearScreenHandler : ICommandHandler
    {
        public ClearScreenHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ClearScreenHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ClearScreenHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteClearScreen(Connection, command as ClearScreenCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ClearScreenCommand clearScreencommand = command as ClearScreenCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            ClearScreenCompletion response = new ClearScreenCompletion(clearScreencommand.Headers.RequestId, new ClearScreenCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteClearScreen(IConnection connection, ClearScreenCommand clearScreen, CancellationToken cancel)
        {
            clearScreen.IsNotNull($"Invalid parameter in the ExecuteClearScreen method. {nameof(clearScreen)}");

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, clearScreen.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.ClearScreen()");
            Task<ClearScreenCompletion.PayloadData> task = ServiceProvider.Device.ClearScreen(textTerminalConnection, clearScreen.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.ClearScreen() -> {task.Result.CompletionCode}");

            ClearScreenCompletion response = new ClearScreenCompletion(clearScreen.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(DispLightCommand))]
    public class DispLightHandler : ICommandHandler
    {
        public DispLightHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DispLightHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(DispLightHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteDispLight(Connection, command as DispLightCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            DispLightCommand dispLightcommand = command as DispLightCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            DispLightCompletion response = new DispLightCompletion(dispLightcommand.Headers.RequestId, new DispLightCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteDispLight(IConnection connection, DispLightCommand dispLight, CancellationToken cancel)
        {
            dispLight.IsNotNull($"Invalid parameter in the ExecuteDispLight method. {nameof(dispLight)}");

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, dispLight.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.DispLight()");
            Task<DispLightCompletion.PayloadData> task = ServiceProvider.Device.DispLight(textTerminalConnection, dispLight.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.DispLight() -> {task.Result.CompletionCode}");

            DispLightCompletion response = new DispLightCompletion(dispLight.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(SetLedCommand))]
    public class SetLedHandler : ICommandHandler
    {
        public SetLedHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetLedHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(SetLedHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteSetLed(Connection, command as SetLedCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            SetLedCommand setLedcommand = command as SetLedCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            SetLedCompletion response = new SetLedCompletion(setLedcommand.Headers.RequestId, new SetLedCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSetLed(IConnection connection, SetLedCommand setLed, CancellationToken cancel)
        {
            setLed.IsNotNull($"Invalid parameter in the ExecuteSetLed method. {nameof(setLed)}");

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, setLed.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.SetLed()");
            Task<SetLedCompletion.PayloadData> task = ServiceProvider.Device.SetLed(textTerminalConnection, setLed.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.SetLed() -> {task.Result.CompletionCode}");

            SetLedCompletion response = new SetLedCompletion(setLed.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(SetResolutionCommand))]
    public class SetResolutionHandler : ICommandHandler
    {
        public SetResolutionHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetResolutionHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(SetResolutionHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteSetResolution(Connection, command as SetResolutionCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            SetResolutionCommand setResolutioncommand = command as SetResolutionCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            SetResolutionCompletion response = new SetResolutionCompletion(setResolutioncommand.Headers.RequestId, new SetResolutionCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteSetResolution(IConnection connection, SetResolutionCommand setResolution, CancellationToken cancel)
        {
            setResolution.IsNotNull($"Invalid parameter in the ExecuteSetResolution method. {nameof(setResolution)}");

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, setResolution.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.SetResolution()");
            Task<SetResolutionCompletion.PayloadData> task = ServiceProvider.Device.SetResolution(textTerminalConnection, setResolution.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.SetResolution() -> {task.Result.CompletionCode}");

            SetResolutionCompletion response = new SetResolutionCompletion(setResolution.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(WriteFormCommand))]
    public class WriteFormHandler : ICommandHandler
    {
        public WriteFormHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(WriteFormHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(WriteFormHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteWriteForm(Connection, command as WriteFormCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            WriteFormCommand writeFormcommand = command as WriteFormCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            WriteFormCompletion response = new WriteFormCompletion(writeFormcommand.Headers.RequestId, new WriteFormCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteWriteForm(IConnection connection, WriteFormCommand writeForm, CancellationToken cancel)
        {
            writeForm.IsNotNull($"Invalid parameter in the ExecuteWriteForm method. {nameof(writeForm)}");

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, writeForm.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.WriteForm()");
            Task<WriteFormCompletion.PayloadData> task = ServiceProvider.Device.WriteForm(textTerminalConnection, writeForm.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.WriteForm() -> {task.Result.CompletionCode}");

            WriteFormCompletion response = new WriteFormCompletion(writeForm.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(ReadFormCommand))]
    public class ReadFormHandler : ICommandHandler
    {
        public ReadFormHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadFormHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, readForm.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.ReadForm()");
            Task<ReadFormCompletion.PayloadData> task = ServiceProvider.Device.ReadForm(textTerminalConnection, readForm.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.ReadForm() -> {task.Result.CompletionCode}");

            ReadFormCompletion response = new ReadFormCompletion(readForm.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(WriteCommand))]
    public class WriteHandler : ICommandHandler
    {
        public WriteHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(WriteHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(WriteHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteWrite(Connection, command as WriteCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            WriteCommand writecommand = command as WriteCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            WriteCompletion response = new WriteCompletion(writecommand.Headers.RequestId, new WriteCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteWrite(IConnection connection, WriteCommand write, CancellationToken cancel)
        {
            write.IsNotNull($"Invalid parameter in the ExecuteWrite method. {nameof(write)}");

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, write.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.Write()");
            Task<WriteCompletion.PayloadData> task = ServiceProvider.Device.Write(textTerminalConnection, write.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.Write() -> {task.Result.CompletionCode}");

            WriteCompletion response = new WriteCompletion(write.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(ReadCommand))]
    public class ReadHandler : ICommandHandler
    {
        public ReadHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(ReadHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteRead(Connection, command as ReadCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            ReadCommand readcommand = command as ReadCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            ReadCompletion response = new ReadCompletion(readcommand.Headers.RequestId, new ReadCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteRead(IConnection connection, ReadCommand read, CancellationToken cancel)
        {
            read.IsNotNull($"Invalid parameter in the ExecuteRead method. {nameof(read)}");

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, read.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.Read()");
            Task<ReadCompletion.PayloadData> task = ServiceProvider.Device.Read(textTerminalConnection, read.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.Read() -> {task.Result.CompletionCode}");

            ReadCompletion response = new ReadCompletion(read.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(ResetCommand))]
    public class ResetHandler : ICommandHandler
    {
        public ResetHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ResetHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, reset.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.Reset()");
            Task<ResetCompletion.PayloadData> task = ServiceProvider.Device.Reset(textTerminalConnection, reset.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.Reset() -> {task.Result.CompletionCode}");

            ResetCompletion response = new ResetCompletion(reset.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(DefineKeysCommand))]
    public class DefineKeysHandler : ICommandHandler
    {
        public DefineKeysHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DefineKeysHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

            logger.IsNotNull($"Invalid parameter in the {nameof(DefineKeysHandler)} constructor. {nameof(logger)}");
            this.Logger = logger;
        }

        public async Task Handle(IConnection Connection, object command, CancellationToken cancel) => await ExecuteDefineKeys(Connection, command as DefineKeysCommand, cancel);

        public async Task HandleError(IConnection connection, object command, Exception commandException)
        {
            DefineKeysCommand defineKeyscommand = command as DefineKeysCommand;

            MessagePayload.CompletionCodeEnum errorCode = MessagePayload.CompletionCodeEnum.InternalError;
            if (commandException.GetType() == typeof(InvalidDataException))
                errorCode = MessagePayload.CompletionCodeEnum.InvalidData;

            DefineKeysCompletion response = new DefineKeysCompletion(defineKeyscommand.Headers.RequestId, new DefineKeysCompletion.PayloadData(errorCode, commandException.Message));

            await connection.SendMessageAsync(response);
        }

        private async Task ExecuteDefineKeys(IConnection connection, DefineKeysCommand defineKeys, CancellationToken cancel)
        {
            defineKeys.IsNotNull($"Invalid parameter in the ExecuteDefineKeys method. {nameof(defineKeys)}");

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, defineKeys.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.DefineKeys()");
            Task<DefineKeysCompletion.PayloadData> task = ServiceProvider.Device.DefineKeys(textTerminalConnection, defineKeys.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.DefineKeys() -> {task.Result.CompletionCode}");

            DefineKeysCompletion response = new DefineKeysCompletion(defineKeys.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(StatusCommand))]
    public class StatusHandler : ICommandHandler
    {
        public StatusHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(StatusHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, status.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.Status()");
            Task<StatusCompletion.PayloadData> task = ServiceProvider.Device.Status(textTerminalConnection, status.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.Status() -> {task.Result.CompletionCode}");

            StatusCompletion response = new StatusCompletion(status.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(CapabilitiesCommand))]
    public class CapabilitiesHandler : ICommandHandler
    {
        public CapabilitiesHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(CapabilitiesHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, capabilities.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.Capabilities()");
            Task<CapabilitiesCompletion.PayloadData> task = ServiceProvider.Device.Capabilities(textTerminalConnection, capabilities.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.Capabilities() -> {task.Result.CompletionCode}");

            CapabilitiesCompletion response = new CapabilitiesCompletion(capabilities.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(SetGuidanceLightCommand))]
    public class SetGuidanceLightHandler : ICommandHandler
    {
        public SetGuidanceLightHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetGuidanceLightHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, setGuidanceLight.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.SetGuidanceLight()");
            Task<SetGuidanceLightCompletion.PayloadData> task = ServiceProvider.Device.SetGuidanceLight(textTerminalConnection, setGuidanceLight.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.SetGuidanceLight() -> {task.Result.CompletionCode}");

            SetGuidanceLightCompletion response = new SetGuidanceLightCompletion(setGuidanceLight.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(PowerSaveControlCommand))]
    public class PowerSaveControlHandler : ICommandHandler
    {
        public PowerSaveControlHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(PowerSaveControlHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, powerSaveControl.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.PowerSaveControl()");
            Task<PowerSaveControlCompletion.PayloadData> task = ServiceProvider.Device.PowerSaveControl(textTerminalConnection, powerSaveControl.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.PowerSaveControl() -> {task.Result.CompletionCode}");

            PowerSaveControlCompletion response = new PowerSaveControlCompletion(powerSaveControl.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(SynchronizeCommandCommand))]
    public class SynchronizeCommandHandler : ICommandHandler
    {
        public SynchronizeCommandHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SynchronizeCommandHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, synchronizeCommand.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.SynchronizeCommand()");
            Task<SynchronizeCommandCompletion.PayloadData> task = ServiceProvider.Device.SynchronizeCommand(textTerminalConnection, synchronizeCommand.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.SynchronizeCommand() -> {task.Result.CompletionCode}");

            SynchronizeCommandCompletion response = new SynchronizeCommandCompletion(synchronizeCommand.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(SetTransactionStateCommand))]
    public class SetTransactionStateHandler : ICommandHandler
    {
        public SetTransactionStateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetTransactionStateHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, setTransactionState.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.SetTransactionState()");
            Task<SetTransactionStateCompletion.PayloadData> task = ServiceProvider.Device.SetTransactionState(textTerminalConnection, setTransactionState.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.SetTransactionState() -> {task.Result.CompletionCode}");

            SetTransactionStateCompletion response = new SetTransactionStateCompletion(setTransactionState.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

    [CommandHandler(typeof(TextTerminalServiceProvider), typeof(GetTransactionStateCommand))]
    public class GetTransactionStateHandler : ICommandHandler
    {
        public GetTransactionStateHandler(ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetTransactionStateHandler)} constructor. {nameof(Dispatcher)}");
            this.ServiceProvider = Dispatcher as TextTerminalServiceProvider;

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

            ITextTerminalConnection textTerminalConnection = new TextTerminalConnection(connection, getTransactionState.Headers.RequestId);

            Logger.Log(Constants.DeviceClass, "TextTerminalDev.GetTransactionState()");
            Task<GetTransactionStateCompletion.PayloadData> task = ServiceProvider.Device.GetTransactionState(textTerminalConnection, getTransactionState.Payload, cancel);
            Logger.Log(Constants.DeviceClass, $"TextTerminalDev.GetTransactionState() -> {task.Result.CompletionCode}");

            GetTransactionStateCompletion response = new GetTransactionStateCompletion(getTransactionState.Headers.RequestId, task.Result);

            await connection.SendMessageAsync(response);
        }

        public TextTerminalServiceProvider ServiceProvider { get; }
        private ILogger Logger { get; }
    }

}
