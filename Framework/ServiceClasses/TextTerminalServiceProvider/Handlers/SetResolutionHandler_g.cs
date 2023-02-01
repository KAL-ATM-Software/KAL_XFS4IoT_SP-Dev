/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * SetResolutionHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.TextTerminal, typeof(SetResolutionCommand))]
    public partial class SetResolutionHandler : ICommandHandler
    {
        public SetResolutionHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetResolutionHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetResolutionHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ITextTerminalDevice>();

            TextTerminal = Provider.IsA<ITextTerminalService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetResolutionHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetResolutionHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setResolutionCmd = command.IsA<SetResolutionCommand>($"Invalid parameter in the SetResolution Handle method. {nameof(SetResolutionCommand)}");
            setResolutionCmd.Header.RequestId.HasValue.IsTrue();

            ISetResolutionEvents events = new SetResolutionEvents(Connection, setResolutionCmd.Header.RequestId.Value);

            var result = await HandleSetResolution(events, setResolutionCmd, cancel);
            await Connection.SendMessageAsync(new SetResolutionCompletion(setResolutionCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setResolutioncommand = command.IsA<SetResolutionCommand>();
            setResolutioncommand.Header.RequestId.HasValue.IsTrue();

            SetResolutionCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetResolutionCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetResolutionCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetResolutionCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetResolutionCompletion(setResolutioncommand.Header.RequestId.Value, new SetResolutionCompletion.PayloadData(errorCode, commandException.Message));

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
