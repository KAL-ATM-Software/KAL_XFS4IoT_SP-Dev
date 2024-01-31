/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [CommandHandler(XFSConstants.ServiceClass.Auxiliaries, typeof(SetAutoStartUpTimeCommand))]
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
            var setAutoStartupTimeCmd = command.IsA<SetAutoStartUpTimeCommand>($"Invalid parameter in the SetAutoStartupTime Handle method. {nameof(SetAutoStartUpTimeCommand)}");
            setAutoStartupTimeCmd.Header.RequestId.HasValue.IsTrue();

            ISetAutoStartupTimeEvents events = new SetAutoStartupTimeEvents(Connection, setAutoStartupTimeCmd.Header.RequestId.Value);

            var result = await HandleSetAutoStartupTime(events, setAutoStartupTimeCmd, cancel);
            await Connection.SendMessageAsync(new SetAutoStartUpTimeCompletion(setAutoStartupTimeCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var setAutoStartupTimecommand = command.IsA<SetAutoStartUpTimeCommand>();
            setAutoStartupTimecommand.Header.RequestId.HasValue.IsTrue();

            SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SetAutoStartUpTimeCompletion(setAutoStartupTimecommand.Header.RequestId.Value, new SetAutoStartUpTimeCompletion.PayloadData(errorCode, commandException.Message));

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
