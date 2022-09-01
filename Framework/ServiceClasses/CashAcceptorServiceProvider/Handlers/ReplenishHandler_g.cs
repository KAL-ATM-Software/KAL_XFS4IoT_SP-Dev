/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ReplenishHandler_g.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoTFramework.Common;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using IServiceProvider = XFS4IoTServer.IServiceProvider;

namespace XFS4IoTFramework.CashAcceptor
{
    [CommandHandler(XFSConstants.ServiceClass.CashAcceptor, typeof(ReplenishCommand))]
    public partial class ReplenishHandler : ICommandHandler
    {
        public ReplenishHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(ReplenishHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(ReplenishHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICashAcceptorDevice>();

            CashAcceptor = Provider.IsA<ICashAcceptorService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(ReplenishHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(ReplenishHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var replenishCmd = command.IsA<ReplenishCommand>($"Invalid parameter in the Replenish Handle method. {nameof(ReplenishCommand)}");
            replenishCmd.Header.RequestId.HasValue.IsTrue();

            IReplenishEvents events = new ReplenishEvents(Connection, replenishCmd.Header.RequestId.Value);

            var result = await HandleReplenish(events, replenishCmd, cancel);
            await Connection.SendMessageAsync(new ReplenishCompletion(replenishCmd.Header.RequestId.Value, result));
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var replenishcommand = command.IsA<ReplenishCommand>();
            replenishcommand.Header.RequestId.HasValue.IsTrue();

            ReplenishCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => ReplenishCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => ReplenishCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => ReplenishCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => ReplenishCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => ReplenishCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => ReplenishCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => ReplenishCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => ReplenishCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => ReplenishCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => ReplenishCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => ReplenishCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => ReplenishCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => ReplenishCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => ReplenishCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => ReplenishCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new ReplenishCompletion(replenishcommand.Header.RequestId.Value, new ReplenishCompletion.PayloadData(errorCode, commandException.Message));

            await Connection.SendMessageAsync(response);
        }

        private IConnection Connection { get; }
        private ICashAcceptorDevice Device { get => Provider.Device.IsA<ICashAcceptorDevice>(); }
        private IServiceProvider Provider { get; }
        private ICashAcceptorService CashAcceptor { get; }
        private ICommonService Common { get; }
        private ILogger Logger { get; }
    }

}
