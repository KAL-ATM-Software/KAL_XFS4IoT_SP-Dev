/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * LoadFormHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.TextTerminal
{
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(LoadFormCommand))]
    public partial class LoadFormHandler : ICommandHandler
    {
        public LoadFormHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(LoadFormHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(LoadFormHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(LoadFormHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(LoadFormHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var loadFormCmd = command.IsA<LoadFormCommand>($"Invalid parameter in the LoadForm Handle method. {nameof(LoadFormCommand)}");
            loadFormCmd.Header.RequestId.HasValue.IsTrue();

            ILoadFormEvents events = new LoadFormEvents(Connection, loadFormCmd.Header.RequestId.Value);

            var result = await HandleLoadForm(events, loadFormCmd, cancel);
            await Connection.SendMessageAsync(new LoadFormCompletion(loadFormCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var loadFormcommand = command.IsA<LoadFormCommand>();
            loadFormcommand.Header.RequestId.HasValue.IsTrue();

            LoadFormCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => LoadFormCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => LoadFormCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => LoadFormCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => LoadFormCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => LoadFormCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => LoadFormCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => LoadFormCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => LoadFormCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => LoadFormCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => LoadFormCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => LoadFormCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => LoadFormCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => LoadFormCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => LoadFormCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => LoadFormCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new LoadFormCompletion(loadFormcommand.Header.RequestId.Value, new LoadFormCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ITextTerminalDevice Device { get => Provider.Device.IsA<ITextTerminalDevice>(); }
        private IServiceProvider Provider { get; }
        private ITextTerminalService TextTerminal { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
