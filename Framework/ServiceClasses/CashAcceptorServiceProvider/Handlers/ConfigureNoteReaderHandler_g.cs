/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ConfigureNoteReaderHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashAcceptor
{
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(ConfigureNoteReaderCommand))]
    public partial class ConfigureNoteReaderHandler : ICommandHandler
    {
        public ConfigureNoteReaderHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ConfigureNoteReaderHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ConfigureNoteReaderHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ConfigureNoteReaderHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ConfigureNoteReaderHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var configureNoteReaderCmd = command.IsA<ConfigureNoteReaderCommand>($"Invalid parameter in the ConfigureNoteReader Handle method. {nameof(ConfigureNoteReaderCommand)}");
            configureNoteReaderCmd.Header.RequestId.HasValue.IsTrue();

            IConfigureNoteReaderEvents events = new ConfigureNoteReaderEvents(Connection, configureNoteReaderCmd.Header.RequestId.Value);

            var result = await HandleConfigureNoteReader(events, configureNoteReaderCmd, cancel);
            await Connection.SendMessageAsync(new ConfigureNoteReaderCompletion(configureNoteReaderCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var configureNoteReadercommand = command.IsA<ConfigureNoteReaderCommand>();
            configureNoteReadercommand.Header.RequestId.HasValue.IsTrue();

            ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ConfigureNoteReaderCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ConfigureNoteReaderCompletion(configureNoteReadercommand.Header.RequestId.Value, new ConfigureNoteReaderCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashAcceptorDevice Device { get => Provider.Device.IsA<ICashAcceptorDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashAcceptorService CashAcceptor { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
