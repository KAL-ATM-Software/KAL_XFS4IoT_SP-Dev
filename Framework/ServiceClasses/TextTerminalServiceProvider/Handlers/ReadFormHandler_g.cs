/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * ReadFormHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(ReadFormCommand))]
    public partial class ReadFormHandler : ICommandHandler
    {
        public ReadFormHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReadFormHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ReadFormHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ReadFormHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ReadFormHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var readFormCmd = command.IsA<ReadFormCommand>($"Invalid parameter in the ReadForm Handle method. {nameof(ReadFormCommand)}");
            readFormCmd.Header.RequestId.HasValue.IsTrue();

            IReadFormEvents events = new ReadFormEvents(Connection, readFormCmd.Header.RequestId.Value);

            var result = await HandleReadForm(events, readFormCmd, cancel);
            await Connection.SendMessageAsync(new ReadFormCompletion(readFormCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var readFormcommand = command.IsA<ReadFormCommand>();
            readFormcommand.Header.RequestId.HasValue.IsTrue();

            ReadFormCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ReadFormCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ReadFormCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ReadFormCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ReadFormCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ReadFormCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ReadFormCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ReadFormCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ReadFormCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ReadFormCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ReadFormCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ReadFormCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ReadFormCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ReadFormCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ReadFormCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ReadFormCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ReadFormCompletion(readFormcommand.Header.RequestId.Value, new ReadFormCompletion.PayloadData(errorCode, commandException.Message));

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
