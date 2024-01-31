/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * GetAutoStartupTimeHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Auxiliaries, typeof(GetAutoStartUpTimeCommand))]
    public partial class GetAutoStartupTimeHandler : ICommandHandler
    {
        public GetAutoStartupTimeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetAutoStartupTimeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetAutoStartupTimeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IAuxiliariesDevice>();

            Auxiliaries = Provider.IsA<IAuxiliariesService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetAutoStartupTimeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetAutoStartupTimeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getAutoStartupTimeCmd = command.IsA<GetAutoStartUpTimeCommand>($"Invalid parameter in the GetAutoStartupTime Handle method. {nameof(GetAutoStartUpTimeCommand)}");
            getAutoStartupTimeCmd.Header.RequestId.HasValue.IsTrue();

            IGetAutoStartupTimeEvents events = new GetAutoStartupTimeEvents(Connection, getAutoStartupTimeCmd.Header.RequestId.Value);

            var result = await HandleGetAutoStartupTime(events, getAutoStartupTimeCmd, cancel);
            await Connection.SendMessageAsync(new GetAutoStartUpTimeCompletion(getAutoStartupTimeCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getAutoStartupTimecommand = command.IsA<GetAutoStartUpTimeCommand>();
            getAutoStartupTimecommand.Header.RequestId.HasValue.IsTrue();

            GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetAutoStartUpTimeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetAutoStartUpTimeCompletion(getAutoStartupTimecommand.Header.RequestId.Value, new GetAutoStartUpTimeCompletion.PayloadData(errorCode, commandException.Message));

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
