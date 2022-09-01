/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * SetAutoStartupTimeHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Auxiliaries.Commands;
using XFS4IoT.Auxiliaries.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Auxiliaries
{
    [CommandHandler(XFSConstants.ServiceClass.Auxiliaries, typeof(SetAutoStartupTimeCommand))]
    public partial class SetAutoStartupTimeHandler : ICommandHandler
    {
        public SetAutoStartupTimeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SetAutoStartupTimeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SetAutoStartupTimeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IAuxiliariesDevice>();

            Auxiliaries = Provider.IsA<IAuxiliariesService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SetAutoStartupTimeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SetAutoStartupTimeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var setAutoStartupTimeCmd = command.IsA<SetAutoStartupTimeCommand>($"Invalid parameter in the SetAutoStartupTime Handle method. {nameof(SetAutoStartupTimeCommand)}");
            setAutoStartupTimeCmd.Header.RequestId.HasValue.IsTrue();

            ISetAutoStartupTimeEvents events = new SetAutoStartupTimeEvents(Connection, setAutoStartupTimeCmd.Header.RequestId.Value);

            var result = await HandleSetAutoStartupTime(events, setAutoStartupTimeCmd, cancel);
            await Connection.SendMessageAsync(new SetAutoStartupTimeCompletion(setAutoStartupTimeCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setAutoStartupTimecommand = command.IsA<SetAutoStartupTimeCommand>();
            setAutoStartupTimecommand.Header.RequestId.HasValue.IsTrue();

            SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetAutoStartupTimeCompletion(setAutoStartupTimecommand.Header.RequestId.Value, new SetAutoStartupTimeCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IAuxiliariesDevice Device { get => Provider.Device.IsA<IAuxiliariesDevice>(); }
        private IServiceProvider Provider { get; }
        private IAuxiliariesService Auxiliaries { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
