/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [CommandHandler(XFSConstants.ServiceClass.Auxiliaries, typeof(ClearAutoStartUpTimeCommand))]
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
            var clearAutoStartupTimeCmd = command.IsA<ClearAutoStartUpTimeCommand>($"Invalid parameter in the ClearAutoStartupTime Handle method. {nameof(ClearAutoStartUpTimeCommand)}");
            clearAutoStartupTimeCmd.Header.RequestId.HasValue.IsTrue();

            IClearAutoStartupTimeEvents events = new ClearAutoStartupTimeEvents(Connection, clearAutoStartupTimeCmd.Header.RequestId.Value);

            var result = await HandleClearAutoStartupTime(events, clearAutoStartupTimeCmd, cancel);
            await Connection.SendMessageAsync(new ClearAutoStartUpTimeCompletion(clearAutoStartupTimeCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var clearAutoStartupTimecommand = command.IsA<ClearAutoStartUpTimeCommand>();
            clearAutoStartupTimecommand.Header.RequestId.HasValue.IsTrue();

            ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ClearAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ClearAutoStartUpTimeCompletion(clearAutoStartupTimecommand.Header.RequestId.Value, new ClearAutoStartUpTimeCompletion.PayloadData(errorCode, commandException.Message));

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
