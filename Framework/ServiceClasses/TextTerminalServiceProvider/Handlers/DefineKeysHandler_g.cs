/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * DefineKeysHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(DefineKeysCommand))]
    public partial class DefineKeysHandler : ICommandHandler
    {
        public DefineKeysHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DefineKeysHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DefineKeysHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DefineKeysHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(DefineKeysHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var defineKeysCmd = command.IsA<DefineKeysCommand>($"Invalid parameter in the DefineKeys Handle method. {nameof(DefineKeysCommand)}");
            defineKeysCmd.Header.RequestId.HasValue.IsTrue();

            IDefineKeysEvents events = new DefineKeysEvents(Connection, defineKeysCmd.Header.RequestId.Value);

            var result = await HandleDefineKeys(events, defineKeysCmd, cancel);
            await Connection.SendMessageAsync(new DefineKeysCompletion(defineKeysCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var defineKeyscommand = command.IsA<DefineKeysCommand>();
            defineKeyscommand.Header.RequestId.HasValue.IsTrue();

            DefineKeysCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => DefineKeysCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => DefineKeysCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => DefineKeysCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DefineKeysCompletion(defineKeyscommand.Header.RequestId.Value, new DefineKeysCompletion.PayloadData(errorCode, commandException.Message));

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
