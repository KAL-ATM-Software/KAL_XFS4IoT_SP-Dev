/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT KeyManagement interface.
 * StartKeyExchangeHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.KeyManagement.Commands;
using XFS4IoT.KeyManagement.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.KeyManagement
{
    [CommandHandler(XFSConstants.ServiceClass.KeyManagement, typeof(StartKeyExchangeCommand))]
    public partial class StartKeyExchangeHandler : ICommandHandler
    {
        public StartKeyExchangeHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(StartKeyExchangeHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(StartKeyExchangeHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<IKeyManagementDevice>();

            KeyManagement = Provider.IsA<IKeyManagementService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(StartKeyExchangeHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(StartKeyExchangeHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var startKeyExchangeCmd = command.IsA<StartKeyExchangeCommand>($"Invalid parameter in the StartKeyExchange Handle method. {nameof(StartKeyExchangeCommand)}");
            startKeyExchangeCmd.Header.RequestId.HasValue.IsTrue();

            IStartKeyExchangeEvents events = new StartKeyExchangeEvents(Connection, startKeyExchangeCmd.Header.RequestId.Value);

            var result = await HandleStartKeyExchange(events, startKeyExchangeCmd, cancel);
            await Connection.SendMessageAsync(new StartKeyExchangeCompletion(startKeyExchangeCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var startKeyExchangecommand = command.IsA<StartKeyExchangeCommand>();
            startKeyExchangecommand.Header.RequestId.HasValue.IsTrue();

            StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => StartKeyExchangeCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new StartKeyExchangeCompletion(startKeyExchangecommand.Header.RequestId.Value, new StartKeyExchangeCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private IKeyManagementDevice Device { get => Provider.Device.IsA<IKeyManagementDevice>(); }
        private IServiceProvider Provider { get; }
        private IKeyManagementService KeyManagement { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
