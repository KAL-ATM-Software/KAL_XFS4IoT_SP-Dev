/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * SupplyReplenishHandler_g.cs uses automatically generated parts.
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
    [CommandHandler(XFSConstants.ServiceClass.Check, typeof(SupplyReplenishCommand))]
    public partial class SupplyReplenishHandler : ICommandHandler
    {
        public SupplyReplenishHandler(IConnection Connection, ICommandDispatcher Dispatcher, ILogger logger)
        {
            Dispatcher.IsNotNull($"Invalid parameter received in the {nameof(SupplyReplenishHandler)} constructor. {nameof(Dispatcher)}");
            Provider = Dispatcher.IsA<IServiceProvider>();

            Provider.Device.IsNotNull($"Invalid parameter received in the {nameof(SupplyReplenishHandler)} constructor. {nameof(Provider.Device)}")
                           .IsA<ICheckDevice>();

            Check = Provider.IsA<ICheckService>();
            Common = Provider.IsA<ICommonService>();

            this.Logger = logger.IsNotNull($"Invalid parameter in the {nameof(SupplyReplenishHandler)} constructor. {nameof(logger)}");
            this.Connection = Connection.IsNotNull($"Invalid parameter in the {nameof(SupplyReplenishHandler)} constructor. {nameof(Connection)}");
        }

        public async Task Handle(object command, CancellationToken cancel)
        {
            var supplyReplenishCmd = command.IsA<SupplyReplenishCommand>($"Invalid parameter in the SupplyReplenish Handle method. {nameof(SupplyReplenishCommand)}");
            supplyReplenishCmd.Header.RequestId.HasValue.IsTrue();

            ISupplyReplenishEvents events = new SupplyReplenishEvents(Connection, supplyReplenishCmd.Header.RequestId.Value);

            var result = await HandleSupplyReplenish(events, supplyReplenishCmd, cancel);
            await Connection.SendMessageAsync(new SupplyReplenishCompletion(supplyReplenishCmd.Header.RequestId.Value, result));

            await this.IsA<ICommandHandler>().CommandPostProcessing(result);
        }

        public async Task HandleError(object command, Exception commandException)
        {
            var supplyReplenishcommand = command.IsA<SupplyReplenishCommand>();
            supplyReplenishcommand.Header.RequestId.HasValue.IsTrue();

            SupplyReplenishCompletion.PayloadData.CompletionCodeEnum errorCode = commandException switch
            {
                InvalidDataException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.InvalidData,
                InternalErrorException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.InternalError,
                UnsupportedDataException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.UnsupportedData,
                SequenceErrorException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.SequenceError,
                AuthorisationRequiredException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.AuthorisationRequired,
                HardwareErrorException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.HardwareError,
                UserErrorException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.UserError,
                FraudAttemptException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.FraudAttempt,
                DeviceNotReadyException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.DeviceNotReady,
                InvalidCommandException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.InvalidCommand,
                NotEnoughSpaceException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.NotEnoughSpace,
                NotImplementedException or NotSupportedException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.UnsupportedCommand,
                TimeoutCanceledException t when t.IsCancelRequested => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.Canceled,
                TimeoutCanceledException => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.TimeOut,
                _ => SupplyReplenishCompletion.PayloadData.CompletionCodeEnum.InternalError
            };

            var response = new SupplyReplenishCompletion(supplyReplenishcommand.Header.RequestId.Value, new SupplyReplenishCompletion.PayloadData(errorCode, commandException.Message));

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
