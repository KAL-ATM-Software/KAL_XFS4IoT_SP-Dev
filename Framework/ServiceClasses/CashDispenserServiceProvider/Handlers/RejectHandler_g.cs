/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * RejectHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashDispenser
{
    [CommandHandler(XFSConstants.ServiceClass.CashDispenser, typeof(RejectCommand))]
    public partial class RejectHandler : ICommandHandler
    {
        public RejectHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(RejectHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(RejectHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashDispenserDevice>();

            CashDispenser = Provider.IsA<ICashDispenserService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(RejectHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(RejectHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var rejectCmd = command.IsA<RejectCommand>($"Invalid parameter in the Reject Handle method. {nameof(RejectCommand)}");
            rejectCmd.Header.RequestId.HasValue.IsTrue();

            IRejectEvents events = new RejectEvents(Connection, rejectCmd.Header.RequestId.Value);

            var result = await HandleReject(events, rejectCmd, cancel);
            await Connection.SendMessageAsync(new RejectCompletion(rejectCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var rejectcommand = command.IsA<RejectCommand>();
            rejectcommand.Header.RequestId.HasValue.IsTrue();

            RejectCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => RejectCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => RejectCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => RejectCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => RejectCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => RejectCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => RejectCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => RejectCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => RejectCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => RejectCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => RejectCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => RejectCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => RejectCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => RejectCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => RejectCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => RejectCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new RejectCompletion(rejectcommand.Header.RequestId.Value, new RejectCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashDispenserDevice Device { get => Provider.Device.IsA<ICashDispenserDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashDispenserService CashDispenser { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
