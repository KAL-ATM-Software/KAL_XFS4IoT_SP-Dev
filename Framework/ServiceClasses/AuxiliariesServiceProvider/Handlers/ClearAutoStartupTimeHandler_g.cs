/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * ClearAutoStartupTimeHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Auxiliaries, typeof(ClearAutoStartupTimeCommand))]
    public partial class ClearAutoStartupTimeHandler : ICommandHandler
    {
        public ClearAutoStartupTimeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ClearAutoStartupTimeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ClearAutoStartupTimeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IAuxiliariesDevice>();

            Auxiliaries = Provider.IsA<IAuxiliariesService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ClearAutoStartupTimeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ClearAutoStartupTimeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var clearAutoStartupTimeCmd = command.IsA<ClearAutoStartupTimeCommand>($"Invalid parameter in the ClearAutoStartupTime Handle method. {nameof(ClearAutoStartupTimeCommand)}");
            clearAutoStartupTimeCmd.Header.RequestId.HasValue.IsTrue();

            IClearAutoStartupTimeEvents events = new ClearAutoStartupTimeEvents(Connection, clearAutoStartupTimeCmd.Header.RequestId.Value);

            var result = await HandleClearAutoStartupTime(events, clearAutoStartupTimeCmd, cancel);
            await Connection.SendMessageAsync(new ClearAutoStartupTimeCompletion(clearAutoStartupTimeCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var clearAutoStartupTimecommand = command.IsA<ClearAutoStartupTimeCommand>();
            clearAutoStartupTimecommand.Header.RequestId.HasValue.IsTrue();

            ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ClearAutoStartupTimeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ClearAutoStartupTimeCompletion(clearAutoStartupTimecommand.Header.RequestId.Value, new ClearAutoStartupTimeCompletion.PayloadData(errorCode, commandException.Message));

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
