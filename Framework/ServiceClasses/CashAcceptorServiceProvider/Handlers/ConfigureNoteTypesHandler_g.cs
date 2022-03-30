/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ConfigureNoteTypesHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(ConfigureNoteTypesCommand))]
    public partial class ConfigureNoteTypesHandler : ICommandHandler
    {
        public ConfigureNoteTypesHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ConfigureNoteTypesHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ConfigureNoteTypesHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ConfigureNoteTypesHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ConfigureNoteTypesHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var configureNoteTypesCmd = command.IsA<ConfigureNoteTypesCommand>($"Invalid parameter in the ConfigureNoteTypes Handle method. {nameof(ConfigureNoteTypesCommand)}");
            configureNoteTypesCmd.Header.RequestId.HasValue.IsTrue();

            IConfigureNoteTypesEvents events = new ConfigureNoteTypesEvents(Connection, configureNoteTypesCmd.Header.RequestId.Value);

            var result = await HandleConfigureNoteTypes(events, configureNoteTypesCmd, cancel);
            await Connection.SendMessageAsync(new ConfigureNoteTypesCompletion(configureNoteTypesCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var configureNoteTypescommand = command.IsA<ConfigureNoteTypesCommand>();
            configureNoteTypescommand.Header.RequestId.HasValue.IsTrue();

            ConfigureNoteTypesCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ConfigureNoteTypesCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                NotImplementedException or NotSupportedException => ConfigureNoteTypesCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ConfigureNoteTypesCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ConfigureNoteTypesCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ConfigureNoteTypesCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ConfigureNoteTypesCompletion(configureNoteTypescommand.Header.RequestId.Value, new ConfigureNoteTypesCompletion.PayloadData(errorCode, commandException.Message));

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
