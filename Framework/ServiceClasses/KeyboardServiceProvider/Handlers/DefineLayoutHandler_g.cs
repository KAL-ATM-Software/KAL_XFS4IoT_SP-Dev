/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * DefineLayoutHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Keyboard
{
    [CommandHandler(XFSConstants.ServiceClass.Keyboard, typeof(DefineLayoutCommand))]
    public partial class DefineLayoutHandler : ICommandHandler
    {
        public DefineLayoutHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(DefineLayoutHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(DefineLayoutHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyboardDevice>();

            Keyboard = Provider.IsA<IKeyboardService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(DefineLayoutHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(DefineLayoutHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var defineLayoutCmd = command.IsA<DefineLayoutCommand>($"Invalid parameter in the DefineLayout Handle method. {nameof(DefineLayoutCommand)}");
            defineLayoutCmd.Header.RequestId.HasValue.IsTrue();

            IDefineLayoutEvents events = new DefineLayoutEvents(Connection, defineLayoutCmd.Header.RequestId.Value);

            var result = await HandleDefineLayout(events, defineLayoutCmd, cancel);
            await Connection.SendMessageAsync(new DefineLayoutCompletion(defineLayoutCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var defineLayoutcommand = command.IsA<DefineLayoutCommand>();
            defineLayoutcommand.Header.RequestId.HasValue.IsTrue();

            DefineLayoutCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => DefineLayoutCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new DefineLayoutCompletion(defineLayoutcommand.Header.RequestId.Value, new DefineLayoutCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IKeyboardDevice Device { get => Provider.Device.IsA<IKeyboardDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyboardService Keyboard { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
