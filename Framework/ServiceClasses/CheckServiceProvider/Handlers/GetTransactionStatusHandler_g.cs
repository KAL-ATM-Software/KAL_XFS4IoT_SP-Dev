/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * GetTransactionStatusHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.Check
{
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(GetTransactionStatusCommand))]
    public partial class GetTransactionStatusHandler : ICommandHandler
    {
        public GetTransactionStatusHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(GetTransactionStatusHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(GetTransactionStatusHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(GetTransactionStatusHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(GetTransactionStatusHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var getTransactionStatusCmd = command.IsA<GetTransactionStatusCommand>($"Invalid parameter in the GetTransactionStatus Handle method. {nameof(GetTransactionStatusCommand)}");
            getTransactionStatusCmd.Header.RequestId.HasValue.IsTrue();

            IGetTransactionStatusEvents events = new GetTransactionStatusEvents(Connection, getTransactionStatusCmd.Header.RequestId.Value);

            var result = await HandleGetTransactionStatus(events, getTransactionStatusCmd, cancel);
            await Connection.SendMessageAsync(new GetTransactionStatusCompletion(getTransactionStatusCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var getTransactionStatuscommand = command.IsA<GetTransactionStatusCommand>();
            getTransactionStatuscommand.Header.RequestId.HasValue.IsTrue();

            GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => GetTransactionStatusCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new GetTransactionStatusCompletion(getTransactionStatuscommand.Header.RequestId.Value, new GetTransactionStatusCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICheckDevice Device { get => Provider.Device.IsA<ICheckDevice>(); }
        private IServiceProvider Provider { get; }
        private ICheckService Check { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
